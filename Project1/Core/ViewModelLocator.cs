using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project1.Core
{
    public class ViewModelLocator
    {
        public static DependencyProperty AutoHookedUpViewModelProperty =
            DependencyProperty.RegisterAttached("AutoHookedUpViewModel", typeof(bool), typeof(ViewModelLocator), new PropertyMetadata(false, AutoHookedUpViewModelChanged));

        public static bool GetAutoHookedUpViewModel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoHookedUpViewModelProperty);
        }

        public static void SetAutoHookedUpViewModel(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoHookedUpViewModelProperty, value);
        }

        private static void AutoHookedUpViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d)) return;
            var viewType = d.GetType();

            var str = viewType.FullName;
            str = str.Replace(".View.", ".ViewModel.");

            var viewTypeName = str;
            var viewModelTypeName = viewTypeName + "Model";
            var viewModelType = Type.GetType(viewModelTypeName);
            var viewModel = Activator.CreateInstance(viewModelType);

            ((FrameworkElement)d).DataContext = viewModel;
        }
    }
}
