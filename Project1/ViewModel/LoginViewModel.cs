using Project1.Core;
using Project1.Database;
using Project1.Model;
using Project1.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Project1.ViewModel
{
    public class LoginViewModel : ValidatableBindableBase
    {
        private string username;
        private string password;

        private readonly DatabaseContext dbContext = new DatabaseContext();
        private readonly LoginService loginService = LoginService.Instance;

        public LoginViewModel()
        {
            LogInCommand = new MyICommand(OnLogIn, CanLogIn);
            RegisterCommand = new MyICommand(OnRegister, CanRegister);
        }

        [Required]
        [RegularExpression(@"^[^\d\W]\w+")]
        public string Username
        {
            get => username; set
            {
                SetProperty(ref username, value);
                LogInCommand.RaiseCanExecuteChanged();
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        [Required]
        [MinLength(7)]
        [RegularExpression(@"^\w+")]
        public string Password
        {
            get => password; set
            {
                SetProperty(ref password, value);
                LogInCommand.RaiseCanExecuteChanged();
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        public MyICommand LogInCommand { get; }

        public MyICommand RegisterCommand { get; }

        private void OnLogIn()
        {
            if (!dbContext.Users.Any(u => u.Username == Username))
            {
                MessageBox.Show("A user with this username doesn't exist!");

                return;
            }

            if (!dbContext.Users.Any(u => u.Username == Username && u.Password == Password))
            {
                MessageBox.Show("Invalid password!");

                return;
            }

            loginService.CurrentUser = dbContext.Users.Single(u => u.Username == Username);
            loginService.IsNew = false;
            loginService.RaiseUserLoggedIn();

            MessageBox.Show("Successfully logged in!");
        }

        private bool CanLogIn()
        {
            return Validate();
        }

        private void OnRegister()
        {
            if (dbContext.Users.Any(u => u.Username == Username))
            {
                MessageBox.Show("A user with this username already exists!");

                return;
            }

            User user = new User() { Username = Username, Password = Password };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            loginService.CurrentUser = user;
            loginService.IsNew = true;
            loginService.RaiseUserLoggedIn();

            MessageBox.Show("Registration successful!");
        }

        private bool CanRegister()
        {
            return Validate();
        }

        private bool Validate()
        {
            return !HasErrors;
        }
    }
}
