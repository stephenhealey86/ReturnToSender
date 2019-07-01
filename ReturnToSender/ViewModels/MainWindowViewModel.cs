using HtmlAgilityPack;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReturnToSender.Events;
using ReturnToSender.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace ReturnToSender.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Private Variables
        Window _window;
        private int CurrentTheme;
        private string errorMessage;
        private Snackbar DisplayError;
        #endregion

        #region Public Variables
        public ObservableCollection<HttpServer> HttpServer { get; set; }
        public int SelectedTab { get; set; } = 0;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                DisplayError.MessageQueue.Enqueue(ErrorMessage, "Ok", () => { errorMessage = null; });
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

        /// <summary>
        /// The command for when the user changed the response content type
        /// </summary>
        public ICommand ContentTypeChangedCommand { get; set; }

        /// <summary>
        /// The command for saving the Http server list as a file
        /// </summary>
        public ICommand SaveFileCommand { get; set; }

        /// <summary>
        /// The command for generating the Http server list from a file
        /// </summary>
        public ICommand OpenFileCommand { get; set; }
        #endregion

        #region Constructor
        public MainWindowViewModel(Window window)
        {
            _window = window;
            Title = "ReturnToSender";
            SetCommands();
            HttpServer = new ObservableCollection<HttpServer>();
            GetUserSettings();
            SubscribeToEvents();
            DisplayError = _window.FindName("MyMessageQueue") as Snackbar;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Loop through all <see cref="HttpServer"/> in <seealso cref="ObservableCollection{T}"/> and subscribe to ServerErrorEvent
        /// </summary>
        private void SubscribeToEvents()
        {
            foreach (HttpServer server in HttpServer)
            {
                server.ServerErrorEvent += ServerErrorEvent;
                server.ServerEvent += delegate { Refresh(); };
            }
        }
        /// <summary>
        /// Add <see cref="HttpServer"/> error message to <seealso cref="MainWindowViewModel.ErrorMessage"/>
        /// </summary>
        /// <param name="sender"><see cref="HttpServer"/></param>
        /// <param name="e"><see cref="ServerErrorEventArgs"/></param>
        private void ServerErrorEvent(object sender, ServerErrorEventArgs e)
        {
            ErrorMessage = e.ErrorMessage;
        }
        /// <summary>
        /// Set all <see cref="MainWindowViewModel"/> <seealso cref="ICommand"/>s
        /// </summary>
        private void SetCommands()
        {
            // Caption Buttons
            MinimizeCommand = new RelayCommand(() => _window.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => _window.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => CloseCommandAction());
            SystemMenuCommand = new RelayCommand(() => SystemMenuCommandAction());

            // Menu Buttons
            ThemeCommand = new RelayCommand(() => ThemeCommandAction());
            SaveFileCommand = new RelayCommand(async () => await SaveFileCommandAction());
            OpenFileCommand = new RelayCommand(async () => await OpenFileCommandAction());

            // Tab Commands
            RemoveServerCommand = new RelayParamterCommand((param) => RemoveServerCommandAction(param));
            NewServerCommand = new RelayCommand(() => NewServerCommandAction());
            VerifyJsonCommand = new RelayCommand(() => VerifyJsonCommandAction());
            StartServerCommand = new RelayParamterCommand(async (param) => await StartServerCommandAction(param));
        }
        /// <summary>
        /// Refreshes the <see cref="MainWindow"/> <seealso cref="TabControl"/> <seealso cref="TabItem"/>s after changes made
        /// </summary>
        private void Refresh()
        {
            OnPropertyChanged(nameof(HttpServer));
            var tabs = _window.FindName("MyTabControl") as TabControl;
            if (tabs != null)
            {
                tabs.Items.Refresh();
            }
        }
        /// <summary>
        /// Verifies the <see cref="HttpServer.Response"/> matches the <seealso cref="HttpServer.ContentType"/>
        /// </summary>
        /// <param name="server"><see cref="HttpServer"/></param>
        /// <param name="contentType"><see cref="HttpServer.ContentType"/></param>
        private void VerifyContent(HttpServer server, string contentType)
        {
            switch (contentType)
            {
                case "JSON":
                    try
                    {
                        JToken jo = JToken.Parse(server.Response);
                        server.Response = jo.ToString(Newtonsoft.Json.Formatting.Indented);
                        server.VerificationString = contentType + " Verified";
                    }
                    catch (Exception)
                    {
                        // Not verified
                        server.VerificationString = contentType + " Not Verified";
                    }
                    break;

                case "Text":
                    if (server.Response.Length > 0)
                    {
                        server.VerificationString = contentType + " Verified";
                    }
                    else
                    {
                        server.VerificationString = contentType + " Not Verified";
                    }
                    break;

                case "JavaScript":
                    try
                    {
                        server.VerificationString = contentType;
                    }
                    catch (Exception)
                    {
                        
                    }
                    break;

                case "XML":
                    try
                    {
                        MemoryStream mStream = new MemoryStream();
                        XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode);
                        XmlDocument xml = new XmlDocument();

                        xml.LoadXml(server.Response);

                        writer.Formatting = System.Xml.Formatting.Indented;

                        // Write the XML into a formatting XmlTextWriter
                        xml.WriteContentTo(writer);
                        writer.Flush();
                        mStream.Flush();
                        mStream.Position = 0;

                        server.Response = new StreamReader(mStream).ReadToEnd().ToString();

                        server.VerificationString = contentType + " Verified";
                    }
                    catch (Exception)
                    {
                        server.VerificationString = contentType + " Not Verified";
                    }
                    break;

                case "HTML":
                    try
                    {
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(server.Response);
                        if (doc.ParseErrors != null && doc.ParseErrors.Count() > 0)
                        {
                            server.VerificationString = contentType + " Not Verified";
                        }
                        else
                        {
                            server.Response = doc.ParsedText;
                            server.VerificationString = contentType + " Verified";
                        }
                    }
                    catch (Exception)
                    {
                        server.VerificationString = contentType + " Not Verified";
                    }
                    break;
                default:
                    server.VerificationString = null;
                    break;
            }
            Refresh();
        }
        /// <summary>
        /// Serializes the <see cref="Object"/> into a JSON string and returns it
        /// </summary>
        /// <param name="obj">Object to turn in JSON string</param>
        /// <param name="subscribe">If true will resubscribe to object events</param>
        /// <returns></returns>
        private string ObjectToJsonString(object obj, bool subscribe)
        {
            foreach (HttpServer server in HttpServer)
            {
                // Remove event handler before serialization
                server.ServerErrorEvent = null;
                server.ServerEvent = null;
                server.ClientRequest = null;
            }
            var jsonObject = JsonConvert.SerializeObject(obj);
            JToken jo = JToken.Parse(jsonObject);
            jsonObject = jo.ToString(Newtonsoft.Json.Formatting.Indented);
            if (subscribe) { SubscribeToEvents(); }
            return jsonObject;
        }
        /// <summary>
        /// Gets the application user settings and updates the application with them
        /// </summary>
        private void GetUserSettings()
        {
            var jsonObject = (string)Properties.Settings.Default["Servers"];
            if (jsonObject != null)
            {
                try
                {
                    HttpServer = JsonConvert.DeserializeObject<ObservableCollection<HttpServer>>(jsonObject);
                    foreach (HttpServer server in HttpServer)
                    {
                        server.Started = false;
                        server.Stop = false;
                    }
                    if (HttpServer.Count == 0)
                    {
                        throw new Exception("Settings are empty.");
                    }
                }
                catch (Exception)
                {
                    NewServerCommandAction();
                }
            }
            // Get theme
            var theme = (int)Properties.Settings.Default["Theme"];
            CurrentTheme = theme;
            Theme = ThemeTypes.GetTheme(CurrentTheme);
            OnPropertyChanged(nameof(Theme));

            // Get selected tab
            SelectedTab = (int)Properties.Settings.Default["SelectedTab"];

            Refresh();
        }
        #endregion

        #region CommandActions
        /// <summary>
        /// Closes the window
        /// </summary>
        private void CloseCommandAction()
        {
            // Servers to remove before saving
            var list = new List<HttpServer>();
            // Loop through servers and stop and store unused for removing
            foreach (HttpServer server in HttpServer)
            {
                // Stop the servers
                server.Stop = true;
                // Add to removal list
                if (server.Request == null)
                {
                    list.Add(server);
                }
            }
            // Remove blank servers
            foreach (HttpServer item in list)
            {
                HttpServer.Remove(item);
            }
            Refresh();
            // Store user settings
            var jsonObject = ObjectToJsonString(HttpServer, false);
            Properties.Settings.Default["Servers"] = jsonObject;
            Properties.Settings.Default["Theme"] = CurrentTheme;
            Properties.Settings.Default["SelectedTab"] = SelectedTab;
            Properties.Settings.Default.Save();
            _window.Close();
        }

        /// <summary>
        /// Populates the HttpServer list from json file
        /// </summary>
        /// <returns></returns>
        private async Task OpenFileCommandAction()
        {
            IsBusy = true;
            // Open windows save dialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            // Get the path name chosen by the user
            var fullPath = openFileDialog.FileName;
            // Get json string from file
            // Check user didn't just close window
            if (fullPath != null && fullPath.Length > 0)
            {
                try
                {
                    if (File.Exists(fullPath))
                    {
                        FileStream fileStream = File.Open(fullPath, FileMode.Open, FileAccess.Read);
                        byte[] buffer = new byte[1028];
                        await fileStream.ReadAsync(buffer, 0, 1028);
                        await fileStream.FlushAsync();
                        fileStream.Close();
                        var jsonObject = Encoding.ASCII.GetString(buffer);
                        HttpServer = JsonConvert.DeserializeObject<ObservableCollection<HttpServer>>(jsonObject);
                        Refresh();
                        ErrorMessage = $"Opened {Path.GetFileName(fullPath)}";
                    }
                }
                catch (Exception)
                {
                    ErrorMessage = "File not compatable";
                }
            }
            IsBusy = false;
        }

        /// <summary>
        /// Saves Http server list as a json file
        /// </summary>
        private async Task SaveFileCommandAction()
        {
            IsBusy = true;
            // Create json string from HttpServer and format
            var jsonObject = ObjectToJsonString(HttpServer, true);
            if (jsonObject.Length > 0)
            {
                // Open windows save dialog
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.ShowDialog();
                // Get the path name chosen by the user
                var fullPath = saveFileDialog.FileName;
                // Check user didn't just close window
                if (fullPath != null && fullPath.Length > 0)
                {
                    // Ensure file extension is json
                    fullPath = Path.ChangeExtension(fullPath, null);
                    fullPath = Path.ChangeExtension(fullPath, "json");
                    byte[] buffer = Encoding.ASCII.GetBytes(jsonObject);
                    FileStream fileStream;
                    // Save file
                    if (File.Exists(fullPath))
                    {
                        fileStream = File.Open(fullPath, FileMode.Truncate, FileAccess.ReadWrite);
                        await fileStream.WriteAsync(buffer, 0, buffer.Length);
                        await fileStream.FlushAsync();
                        fileStream.Close();
                    }
                    else
                    {
                        fileStream = File.Create(fullPath);
                        await fileStream.WriteAsync(buffer, 0, buffer.Length);
                        await fileStream.FlushAsync();
                        fileStream.Close();
                    }
                    ErrorMessage = $"Current configuration saved as {Path.GetFileName(fullPath)}";
                }
            }
            IsBusy = false;
        }

        /// <summary>
        /// Starts the http server
        /// </summary>
        private async Task StartServerCommandAction(object param)
        {
            // Get string
            var str = (string)param;
            // Get the current HttpServer
            var http = HttpServer[SelectedTab];
            if (http.Started)
            {
                http.Stop = true;
                http.Started = false;
                ErrorMessage = "Server stopped";
            }
            else
            {
                http.Started = true;
                http.Stop = false;
                Refresh();
                ErrorMessage = str != null ? $"Server listening for {str}" : "Server started";
                try
                {
                    await http.Start();
                }
                catch (Exception e)
                {
                    http.Stop = false;
                    Debug.WriteLine(e.Message);
                    // User has stoped http server and restarted without sending request
                }
            }
            Refresh();
        }

        /// <summary>
        /// Checks the textbox text is correct JSON
        /// </summary>
        private void VerifyJsonCommandAction()
        {
            var http = HttpServer[SelectedTab];
            var contentType = HttpContentType.GetContentType(http.ContentType).Key;
            VerifyContent(http, contentType);
        }

        /// <summary>
        /// Adds a new blank HttpServer to the list
        /// </summary>
        private void NewServerCommandAction()
        {
            if (HttpServer.Count <= 9)
            {
                HttpServer.Add(new HttpServer());
                SelectedTab = HttpServer.Count - 1;
                HttpServer[SelectedTab].ServerErrorEvent += ServerErrorEvent;
                OnPropertyChanged(nameof(SelectedTab));
                Refresh();
            }
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
        private void ThemeCommandAction()
        {
            CurrentTheme++;
            CurrentTheme = CurrentTheme > 3 ? 0 : CurrentTheme;
            Theme = ThemeTypes.GetTheme(CurrentTheme);
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
