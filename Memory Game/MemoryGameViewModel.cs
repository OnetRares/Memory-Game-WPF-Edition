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
                    MessageBox.Show("Timpul a expirat! Ai pierdut jocul.", "Game Over", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                throw new DirectoryNotFoundException($"Folderul {imagesFolder} nu a fost găsit.");
            }

           
            int pairs = (_rows * _columns) / 2;

            var imageFiles = Directory.GetFiles(imagesFolder, "*.jpg");
            if (imageFiles.Length < pairs)
            {
                throw new Exception($"Nu există suficiente imagini în categoria '{_category}'. Sunt necesare cel puțin {pairs} imagini.");
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
                MessageBox.Show($"Felicitări {_username} ! Ai terminat jocul în {TimerText}", "Joc Terminat", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();
            }
        }
    }
}
