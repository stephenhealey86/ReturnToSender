using ReturnToSender.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ReturnToSender.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Private Variables
        Window _window;
        #endregion

        #region Public Variables
        public bool ToggleMenuVisible { get; set; } = false;
        public ObservableCollection<HttpServer> HttpServer { get; set; }
        public int SelectedTab { get; set; } = 0;
        #endregion

        #region Commands
        /// <summary>
        /// The command to minimize the window
        /// </summary>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        /// The command to maximize the window
        /// </summary>
        public ICommand MaximizeCommand { get; set; }

        /// <summary>
        /// The command to close the window
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command to show the system menu of the window
        /// </summary>
        public ICommand MenuCommand { get; set; }

        /// <summary>
        /// The command to change theme
        /// </summary>
        public ICommand ThemeCommand { get; set; }

        /// <summary>
        /// The command to toggle menu
        /// </summary>
        public ICommand ToggleCommand { get; set; }

        /// <summary>
        /// The command to delete a HttpServer from the List
        /// </summary>
        public ICommand RemoveServerCommand { get; set; }

        /// <summary>
        /// The command to delete a HttpServer from the List
        /// </summary>
        public ICommand NewServerCommand { get; set; }
        #endregion

        #region Constructor
        public MainWindowViewModel(Window window)
        {
            _window = window;
            Title = "ReturnToSender";
            SetCommands();
            // Testing
            HttpServer = new ObservableCollection<HttpServer>();
            HttpServer.Add(new HttpServer());
            HttpServer[0].Request = "test/";
            HttpServer[0].Response = "Testing";
            Task t1 = new Task(async () => { await HttpServer[0].Start(); });
            HttpServer.Add(new HttpServer());
            HttpServer[1].Request = "testingtwo/";
            HttpServer[1].Response = "Testing";
            Task t2 = new Task(async () => { await HttpServer[1].Start(); });
            OnPropertyChanged(nameof(HttpServer));
        }
        #endregion

        #region Helper Methods
        private void SetCommands()
        {
            // Caption Buttons
            MinimizeCommand = new RelayCommand(() => _window.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => _window.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => _window.Close());
            MenuCommand = new RelayCommand(() => ShowSystemMenu());

            // Menu Buttons
            ToggleCommand = new RelayCommand(() => ToggleCommandAction());

            ThemeCommand = new RelayParamterCommand((param) => ThemeCommandAction(param));

            // Tab Commands
            RemoveServerCommand = new RelayParamterCommand((param) => RemoveServerCommandAction(param));
            NewServerCommand = new RelayCommand(() => NewServerCommandAction());
        }
        #endregion

        #region CommandActions
        private void NewServerCommandAction()
        {
            HttpServer.Add(new HttpServer());
            SelectedTab = HttpServer.Count - 1;
            OnPropertyChanged(nameof(SelectedTab));
            OnPropertyChanged(nameof(HttpServer));
        }

        private void RemoveServerCommandAction(object param)
        {
            var str = (string)param;
            if (str != null)
            {
                HttpServer.Remove(HttpServer.FirstOrDefault(x => x.Request == str));
                OnPropertyChanged(nameof(HttpServer));
            }
            else
            {
                var list = new List<HttpServer>();
                foreach (HttpServer item in HttpServer)
                {
                    if (item.Request == null)
                    {
                        list.Add(item);
                    }
                }
                foreach (HttpServer item in list)
                {
                    HttpServer.Remove(item);
                }
                OnPropertyChanged(nameof(HttpServer));
            }
        }

        private void ThemeCommandAction(object param)
        {
            var theme = param as string;
            Theme = ThemeTypes.GetTheme(theme);
            OnPropertyChanged(nameof(Theme));
        }

        private void ToggleCommandAction()
        {
            ToggleMenuVisible = ToggleMenuVisible ? false : true; OnPropertyChanged(nameof(ToggleMenuVisible));
            var num = new Random();
            switch (num.Next(1,5))
            {
                case 1:
                    Theme = ThemeDefault.Theme;
                    break;
                case 2:
                    Theme = ThemeDark.Theme;
                    break;
                case 3:
                    Theme = ThemePlanet.Theme;
                    break;
                case 4:
                    Theme = ThemeTransparent.Theme;
                    break;
            }
            OnPropertyChanged(nameof(Theme));
        }

        private void ShowSystemMenu()
        {
            var x = _window.Left;
            var y = _window.Top;
            SystemCommands.ShowSystemMenu(_window, new Point(x, y));
        }
        #endregion
    }
}
