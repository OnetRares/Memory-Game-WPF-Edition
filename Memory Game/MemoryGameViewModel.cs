using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Memory_Game
{
    public class MemoryGameViewModel : BaseViewModel
    {
        private CardModel _firstSelected;
        private bool _isProcessing;
        private DispatcherTimer _timer;
        private int _secondsElapsed;
        private int _timeLimit;
        private string _category;
        private int _rows;
        private int _columns;
        private string _username;

        public ObservableCollection<CardModel> Cards { get; set; }
        public ICommand CardSelectedCommand { get; }
       
        public string TimerText => $"Time: {(_timeLimit - _secondsElapsed) / 60:D2}:{(_timeLimit - _secondsElapsed) % 60:D2}";
       
        public int Columns => _columns;

       
        public MemoryGameViewModel(string username,int timeLimitInSeconds, string category, int rows, int columns)
        {
            _timeLimit = timeLimitInSeconds;
            _category = category;
            _rows = rows;
            _columns = columns;
            Cards = new ObservableCollection<CardModel>();
            _username = username;

           
            CardSelectedCommand = new RelayCommand<CardModel>(async card => await OnCardSelected(card),
                                                                card => !_isProcessing && !card.IsMatched && !card.IsRevealed);

            LoadCards();
            StartTimer();
        }

        public MemoryGameViewModel(GameState state)
        {
            _username = state.Username;
            _timeLimit = state.TimeRemaining + _secondsElapsed;
            _category = state.Category;
            _rows = state.Rows;
            _columns = state.Columns;
            _secondsElapsed = _timeLimit - state.TimeRemaining;

            Cards = new ObservableCollection<CardModel>();

         
            foreach (var cardState in state.Cards)
            {
                Cards.Add(new CardModel
                {
                    ImagePath = cardState.ImagePath,
                    IsRevealed = cardState.IsRevealed,
                    IsMatched = cardState.IsMatched
                });
            }

            CardSelectedCommand = new RelayCommand<CardModel>(async card => await OnCardSelected(card),
                                                                card => !_isProcessing && !card.IsMatched && !card.IsRevealed);

            StartTimer();
        }

        private void StartTimer()
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) =>
            {
                _secondsElapsed++;
                OnPropertyChanged(nameof(TimerText));

                if (_secondsElapsed >= _timeLimit)
                {
                    _timer.Stop();
                    MessageBox.Show("Time is up! You lost the game..", "Game Over", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Application.Current.Shutdown();
                }
            };
            _timer.Start();
        }


        private void LoadCards()
        {
        
            string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", _category);
            if (!Directory.Exists(imagesFolder))
            {
                throw new DirectoryNotFoundException($"Folder {imagesFolder} not found.");
            }

           
            int pairs = (_rows * _columns) / 2;

            var imageFiles = Directory.GetFiles(imagesFolder, "*.jpg");
            if (imageFiles.Length < pairs)
            {
                throw new Exception($"There are not enough images in category '{_category}'. At least {pairs} images are required..");
            }

            var selectedImages = imageFiles.Take(pairs).ToList();

       
            var cardList = selectedImages.SelectMany(img =>
            {
                return new[]
                {
                    new CardModel { ImagePath = img },
                    new CardModel { ImagePath = img }
                };
            }).ToList();

           
            var rnd = new Random();
            cardList = cardList.OrderBy(x => rnd.Next()).ToList();

            foreach (var card in cardList)
                Cards.Add(card);
        }

        public GameState GetCurrentGameState()
        {
            var state = new GameState
            {
                Username = _username,
                Category = _category,
                GameMode = "Custom", 
                Rows = _rows,
                Columns = _columns,
                TimeRemaining = _timeLimit - _secondsElapsed,
                Cards = Cards.Select(card => new CardState
                {
                    ImagePath = card.ImagePath,
                    IsRevealed = card.IsRevealed,
                    IsMatched = card.IsMatched
                }).ToList()
            };
            return state;
        }



        private async Task OnCardSelected(CardModel selectedCard)
        {
            if (_isProcessing)
                return;

            selectedCard.IsRevealed = true;
            selectedCard.OnPropertyChanged(nameof(CardModel.DisplayImage));

            if (_firstSelected == null)
            {
                _firstSelected = selectedCard;
            }
            else
            {
                _isProcessing = true;
                if (_firstSelected.ImagePath == selectedCard.ImagePath)
                {
                    _firstSelected.IsMatched = true;
                    selectedCard.IsMatched = true;
                }
                else
                {
                    await Task.Delay(500);
                    _firstSelected.IsRevealed = false;
                    selectedCard.IsRevealed = false;
                    _firstSelected.OnPropertyChanged(nameof(CardModel.DisplayImage));
                    selectedCard.OnPropertyChanged(nameof(CardModel.DisplayImage));
                }
                _firstSelected = null;
                _isProcessing = false;
            }

         
            if (Cards.All(c => c.IsMatched))
            {
                _timer.Stop();
                MessageBox.Show($"Congratulations {_username} ! You finished the game in {TimerText}", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();
            }
        }
    }
}
