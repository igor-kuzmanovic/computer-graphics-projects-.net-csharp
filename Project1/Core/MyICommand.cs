using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project1.Core
{
    public class MyICommand : ICommand
    {
        Action targetExecuteMethod;
        Func<bool> targetCanExecuteMethod;

        public MyICommand(Action executeMethod)
        {
            targetExecuteMethod = executeMethod;
        }

        public MyICommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            targetExecuteMethod = executeMethod;
            targetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        bool ICommand.CanExecute(object parameter)
        {

            if (targetCanExecuteMethod != null)
            {
                return targetCanExecuteMethod();
            }

            if (targetExecuteMethod != null)
            {
                return true;
            }

            return false;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        void ICommand.Execute(object parameter)
        {
            targetExecuteMethod?.Invoke();
        }
    }

    public class MyICommand<T> : ICommand
    {
        Action<T> targetExecuteMethod;
        Func<T, bool> targetCanExecuteMethod;

        public MyICommand(Action<T> executeMethod)
        {
            targetExecuteMethod = executeMethod;
        }

        public MyICommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            targetExecuteMethod = executeMethod;
            targetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        bool ICommand.CanExecute(object parameter)
        {

            if (targetCanExecuteMethod != null)
            {
                T tparm = (T)parameter;
                return targetCanExecuteMethod(tparm);
            }

            if (targetExecuteMethod != null)
            {
                return true;
            }

            return false;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        void ICommand.Execute(object parameter)
        {
            targetExecuteMethod?.Invoke((T)parameter);
        }
    }
}
