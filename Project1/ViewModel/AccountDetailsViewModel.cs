using Project1.Core;
using Project1.Database;
using Project1.Model;
using Project1.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project1.ViewModel
{
    public class AccountDetailsViewModel : ValidatableBindableBase
    {
        private int id;
        private string username;
        private string password;

        private readonly DatabaseContext dbContext = new DatabaseContext();
        private readonly LoginService loginService = LoginService.Instance;

        public AccountDetailsViewModel()
        {
            ApplyChangesCommand = new MyICommand(OnApplyChanges, CanApplyChanges);

            id = loginService.CurrentUser.Id;
            username = loginService.CurrentUser.Username;
            password = loginService.CurrentUser.Password;
        }

        public int Id { get => id; set => id = value; }

        [Required]
        [RegularExpression(@"^[^\d\W]\w+")]
        public string Username
        {
            get => username; set
            {
                SetProperty(ref username, value);
                ApplyChangesCommand.RaiseCanExecuteChanged();
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
                ApplyChangesCommand.RaiseCanExecuteChanged();
            }
        }

        public MyICommand ApplyChangesCommand { get; }

        private void OnApplyChanges()
        {
            if (dbContext.Users.Any(u => u.Username == Username))
            {
                MessageBox.Show("A user with this username already exists!");

                return;
            }

            User user = dbContext.Users.Single(u => u.Id == id);
            user.Username = username;
            user.Password = password;
            dbContext.SaveChanges();

            loginService.CurrentUser = null;
            loginService.RaiseUserLoggedOut();

            MessageBox.Show("Changes successfully saved!");
        }

        private bool CanApplyChanges()
        {
            return Validate();
        }

        private bool Validate()
        {
            return !HasErrors;
        }
    }
}
