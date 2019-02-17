using Project1.Core;
using Project1.Model;
using Project1.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project1.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private readonly LoginService loginService = LoginService.Instance;

        private BindableBase currentViewModel;

        public MainViewModel()
        {
            NavCommand = new MyICommand<string>(OnNav);

            switch (loginService.IsNew)
            {
                case true:
                    CurrentViewModel = new AddImageViewModel();
                    break;
                case false:
                    CurrentViewModel = new MyImagesViewModel();
                    break;
            }
        }

        public BindableBase CurrentViewModel { get => currentViewModel; set => SetProperty(ref currentViewModel, value); }

        public MyICommand<string> NavCommand { get; }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "myImages":
                    CurrentViewModel = new MyImagesViewModel();
                    break;
                case "addImage":
                    CurrentViewModel = new AddImageViewModel();
                    break;
                case "accountDetails":
                    CurrentViewModel = new AccountDetailsViewModel();
                    break;
            }
        }
    }
}
