using Project1.Core;
using Project1.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project1.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly LoginService loginService = LoginService.Instance;

        private BindableBase currentViewModel;

        public MainWindowViewModel()
        {
            loginService.UserLoggedIn += HandleUserLoggedIn;
            loginService.UserLoggedOut += HandleUserLoggedOut;

            CurrentViewModel = new LoginViewModel();
        }

        public BindableBase CurrentViewModel { get => currentViewModel; set => SetProperty(ref currentViewModel, value); }

        private void HandleUserLoggedIn(object sender, EventArgs e)
        {
            CurrentViewModel = new MainViewModel();
        }

        private void HandleUserLoggedOut(object sender, EventArgs e)
        {
            CurrentViewModel = new LoginViewModel();
        }
    }
}
