using ReturnToSender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnToSender.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        #region Private Variables
        List<HttpServer> httpServer;
        #endregion
        #region Constructor
        public MainWindowViewModel()
        {
            Title = "ReturnToSender";
        }
        #endregion
    }
}
