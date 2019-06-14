using ReturnToSender.CustomControls;
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
        List<HttpServer> httpServer;
        #endregion

        #region Public Variables
        public List<MyMenuItem> MenuConstants { get; set; }
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
        #endregion

        #region Constructor
        public MainWindowViewModel(Window window)
        {
            _window = window;
            Title = "ReturnToSender";
            SetCommands();
            // Testing
            httpServer = new List<HttpServer>();
            httpServer.Add(new HttpServer("test/"));
            httpServer[0].Response = "Testing";
            Task t1 = new Task(async () => { await httpServer[0].Start(); });
            t1.Start();
            httpServer.Add(new HttpServer("test/two/"));
            httpServer[1].Response = "TestingTwo";
            Task t2 = new Task(async () => { await httpServer[1].Start(); });
            t2.Start();
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
            ThemeCommand = new RelayParamterCommand((param) => ThemeCommandAction(param));            

            MenuConstants = new List<MyMenuItem>()
            {
                new MyMenuItem()
                {
                    Header = "File",
                    ItemsSource = new List<MyMenuItem>()
                    {
                        new MyMenuItem()
                        {
                            Header = "New"
                        },
                        new MyMenuItem()
                        {
                            Header = "Open"
                        },
                        new MyMenuItem()
                        {
                            Header = "Save"
                        }
                    }
                },
                new MyMenuItem()
                {
                    Header = "Theme",
                    ItemsSource = new List<MyMenuItem>()
                    {
                        new MyMenuItem()
                        {
                            Header = ThemeTypes.Default,
                            Command = ThemeCommand,
                            CommandParameter = ThemeTypes.Default
                        },
                        new MyMenuItem()
                        {
                            Header = ThemeTypes.Dark,
                            Command = ThemeCommand,
                            CommandParameter = ThemeTypes.Dark
                        },
                        new MyMenuItem()
                        {
                            Header = ThemeTypes.Planet,
                            Command = ThemeCommand,
                            CommandParameter = ThemeTypes.Planet
                        }
                    }
                }
            };

            OnPropertyChanged(nameof(MenuConstants));
        }
        #endregion

        #region CommandActions
        private void ThemeCommandAction(object param)
        {
            var theme = param as string;
            Theme = ThemeTypes.GetTheme(theme);
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
