using Project1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Service
{
    public class LoginService
    {
        static LoginService() { }

        private LoginService() { }

        public static LoginService Instance { get; } = new LoginService();

        public User CurrentUser { get; set; }

        public bool IsNew { get; set; }

        public event EventHandler UserLoggedIn;

        public event EventHandler UserLoggedOut;

        public void RaiseUserLoggedIn()
        {
            UserLoggedIn?.Invoke(this, new EventArgs());
        }

        public void RaiseUserLoggedOut()
        {
            UserLoggedOut?.Invoke(this, new EventArgs());
        }
    }
}
