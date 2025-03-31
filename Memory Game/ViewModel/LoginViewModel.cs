using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.IO;
using System.Windows;
using Memory_Game.Model;

namespace Memory_Game.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private const string UserFilePath = "users.txt";
        private UserModel _selectedUser;

        public ObservableCollection<UserModel> Users { get; set; } = new ObservableCollection<UserModel>();
        public UserModel SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                OnPropertyChanged(nameof(IsUserSelected));
            }
        }
        public bool IsUserSelected => SelectedUser != null;

        public ICommand PlayCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand CreateUserCommand { get; }
        public ICommand ExitCommand { get; }

        public ICommand NavigateLeftCommand { get; }
        public ICommand NavigateRightCommand { get; }

        

        public LoginViewModel()
        {
            LoadUsers();

            PlayCommand = new RelayCommand(ExecutePlay, () => IsUserSelected);
            DeleteUserCommand = new RelayCommand(ExecuteDeleteUser, () => IsUserSelected);
            CreateUserCommand = new RelayCommand(ExecuteCreateUser);
            ExitCommand = new RelayCommand(ExecuteExit);
            NavigateLeftCommand = new RelayCommand(ExecuteNavigateLeft, () => Users.Count > 1);
            NavigateRightCommand = new RelayCommand(ExecuteNavigateRight, () => Users.Count > 1);

        }

        private void ExecutePlay()
        {
            MenuWindow menuWindow = new MenuWindow(SelectedUser.Name);
            menuWindow.Show();
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()?.Close();
        }
        private void ExecuteDeleteUser()
        {
            if (SelectedUser != null)
            {
                PlayerStatisticsService.DeleteUserStatistics(SelectedUser.Name);
                string userFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MemoryGame", SelectedUser.Name);

                Users.Remove(SelectedUser);
                SaveUsers();

                if (Directory.Exists(userFolderPath))
                {
                    try
                    {
                        Directory.Delete(userFolderPath, true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting user folder: {ex.Message}");
                    }
                }

                
            }
        }

        private void ExecuteCreateUser()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select Image",
                Filter = "Image Files|*.jpg;*.png;*.gif"
            };

            if (dialog.ShowDialog() == true)
            {
                string imagePath = dialog.FileName;
                string userName = Path.GetFileNameWithoutExtension(imagePath);

                if (!Users.Any(u => u.Name == userName))
                {
                    var newUser = new UserModel { Name = userName, ImagePath = imagePath };
                    Users.Add(newUser);
                    SaveUsers();
                }
            }
        }
        private void ExecuteExit()
        {
            Application.Current.Shutdown();
        }
        private void ExecuteNavigateLeft()
        {
            int currentIndex = Users.IndexOf(SelectedUser);
            if (currentIndex > 0)
            {
                SelectedUser = Users[currentIndex - 1];
            }
        }

        private void ExecuteNavigateRight()
        {
            int currentIndex = Users.IndexOf(SelectedUser);
            if (currentIndex < Users.Count - 1)
            {
                SelectedUser = Users[currentIndex + 1];
            }
        }

        private void LoadUsers()
        {
            if (File.Exists(UserFilePath))
            {
                var lines = File.ReadAllLines(UserFilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2)
                    {
                        Users.Add(new UserModel { Name = parts[0], ImagePath = parts[1] });
                    }
                }
            }
        }


        public void SaveUsers()
        {
            try
            {
                if (!File.Exists(UserFilePath))
                {
                    using (File.Create(UserFilePath)) { }
                }

                var usersToSave = Users.Select(u => $"{u.Name}|{u.ImagePath}").ToList();
                File.WriteAllLines(UserFilePath, usersToSave);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving users: {ex.Message}");
            }
        }
    }
}
