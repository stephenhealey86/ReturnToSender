using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public ICommand SystemMenuCommand { get; set; }

        /// <summary>
        /// The command to change theme
        /// </summary>
        public ICommand ThemeCommand { get; set; }

        /// <summary>
        /// The command to delete a HttpServer from the List
        /// </summary>
        public ICommand RemoveServerCommand { get; set; }

        /// <summary>
        /// The command to delete a HttpServer from the List
        /// </summary>
        public ICommand NewServerCommand { get; set; }

        /// <summary>
        /// The command to check the user entered rsponse hs correct JSON syntax
        /// </summary>
        public ICommand VerifyJsonCommand { get; set; }

        /// <summary>
        /// The command to start the http server
        /// </summary>
        public ICommand StartServerCommand { get; set; }
        #endregion

        #region Constructor
        public MainWindowViewModel(Window window)
        {
            _window = window;
            Title = "ReturnToSender";
            SetCommands();
            // Testing
            HttpServer = new ObservableCollection<HttpServer>();
            NewServerCommandAction();
            //Theme = ThemeDark.Theme;
        }
        #endregion

        #region Helper Methods
        private void SetCommands()
        {
            // Caption Buttons
            MinimizeCommand = new RelayCommand(() => _window.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => _window.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => _window.Close());
            SystemMenuCommand = new RelayCommand(() => SystemMenuCommandAction());

            // Menu Buttons
            ThemeCommand = new RelayParamterCommand((param) => ThemeCommandAction(param));

            // Tab Commands
            RemoveServerCommand = new RelayParamterCommand((param) => RemoveServerCommandAction(param));
            NewServerCommand = new RelayCommand(() => NewServerCommandAction());
            VerifyJsonCommand = new RelayParamterCommand((param) => VerifyJsonCommandAction(param));
            StartServerCommand = new RelayParamterCommand(async (param) => await StartServerCommandAction(param));
        }

        private void Refresh()
        {
            OnPropertyChanged(nameof(HttpServer));
            var tabs = _window.FindName("MyTabControl") as TabControl;
            if (tabs != null)
            {
                tabs.Items.Refresh();
            }
        }
        #endregion

        #region CommandActions
        /// <summary>
        /// Starts the http server
        /// </summary>
        private async Task StartServerCommandAction(object param)
        {
            // Get string
            var str = (string)param;
            if (str != null)
            {
                // Remove first matching HttpServer from list and update UI
                var http = HttpServer.FirstOrDefault(x => x.Request == str);
                if (http.Started)
                {
                    http.Stop = true;
                    http.Started = false;
                }
                else
                {
                    http.Started = true;
                    Refresh();
                    await http.Start();
                }
                Refresh();
            }
        }

        /// <summary>
        /// Checks the textbox text is correct JSON
        /// </summary>
        private void VerifyJsonCommandAction(object param)
        {
            var str = (string)param;
            var http = HttpServer.FirstOrDefault(x => x.Response == str);
            // Find http server
            try
            {
                JToken jo = JToken.Parse(http.Response);
                http.Response = jo.ToString(Formatting.Indented);
                http.VerificationString = "JSON Verified";
            }
            catch (Exception)
            {
                // Not verified
                http.VerificationString = "JSON Not Verified";
            }
            Refresh();
        }

        /// <summary>
        /// Adds a new blank HttpServer to the list
        /// </summary>
        private void NewServerCommandAction()
        {
            HttpServer.Add(new HttpServer());
            SelectedTab = HttpServer.Count - 1;
            OnPropertyChanged(nameof(SelectedTab));
            OnPropertyChanged(nameof(HttpServer));
        }

        /// <summary>
        /// Removes the first HttpServer from the list with a Request Uri thata matches the parameter
        /// </summary>
        /// <param name="param">HttpServer items Request property</param>
        private void RemoveServerCommandAction(object param)
        {
            // Get string
            var str = (string)param;
            if (str != null)
            {
                // Remove first matching HttpServer from list and update UI
                var http = HttpServer.FirstOrDefault(x => x.Request == str);
                http.Stop = true;
                HttpServer.Remove(http);
                OnPropertyChanged(nameof(HttpServer));
            }
            else
            {
                // Remove all blank HttpServers from the list and update UI
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
            // If the list is empty add one new blank HttpServer
            if (HttpServer.Count == 0)
            {
                NewServerCommandAction();
            }
        }

        /// <summary>
        /// Change UI theme
        /// </summary>
        /// <param name="param"></param>
        private void ThemeCommandAction(object param)
        {
            var theme = param as string;
            Theme = ThemeTypes.GetTheme(theme);
            OnPropertyChanged(nameof(Theme));
        }

        /// <summary>
        /// Displays the system menu
        /// </summary>
        private void SystemMenuCommandAction()
        {
            var x = _window.Left;
            var y = _window.Top;
            if (_window.WindowState == WindowState.Maximized)
            {
                x = 0; y = -5;
            }
            SystemCommands.ShowSystemMenu(_window, new Point(x, y + 40));
        }
        #endregion
    }
}
