using ReturnToSender.Models;
using System;
using System.Collections.Generic;
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
        public List<HttpServer> HttpServer;
        public List<TabItem> Tabs
        {
            get
            {
                List<TabItem> tabs = new List<TabItem>();
                if (HttpServer.Count > 0)
                {
                    foreach (HttpServer httpServer in HttpServer)
                    {
                        tabs.Add(new TabItem()
                        {
                            Header = httpServer.Request.Length > 8 ? httpServer.Request.Substring(0, 5) + "..." : httpServer.Request,
                            Foreground = Theme.ForeGround,
                            Background = Theme.BackGround,
                            Content = new TabContent()
                            {
                                Text = "CouBear"
                            }
                        });
                        //tabs[0].IsSelected = true;
                    }
                }
                else
                {
                    tabs.Add(new TabItem()
                    {
                        Foreground = Theme.ForeGround,
                        Background = Theme.BackGround,
                        //IsSelected = true
                    });
                }
                
                return tabs;
            }
        }
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
        #endregion

        #region Constructor
        public MainWindowViewModel(Window window)
        {
            _window = window;
            Title = "ReturnToSender";
            SetCommands();
            // Testing
            HttpServer = new List<HttpServer>();
            HttpServer.Add(new HttpServer());
            HttpServer[0].Request = "test/";
            HttpServer[0].Response = "Testing";
            Task t1 = new Task(async () => { await HttpServer[0].Start(); });
            HttpServer.Add(new HttpServer());
            HttpServer[1].Request = "testtwo/";
            HttpServer[1].Response = "Testing";
            Task t2 = new Task(async () => { await HttpServer[1].Start(); });
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
        }
        #endregion

        #region CommandActions
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
            OnPropertyChanged(nameof(Tabs));
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
