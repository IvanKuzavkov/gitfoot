using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.Commands;

namespace BitTorrent.WP7.Services
{
    public class CommandViewModel<T> : DelegateCommandBase, ICommandViewModel
    {
        public CommandViewModel(Action<T> executeMethod)
            : this(executeMethod, (o)=>true)
        {
        }

        public CommandViewModel(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : base((o) => executeMethod((T)o), (o) => canExecuteMethod((T)o))
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");
        }

        public bool CanExecute(T parameter)
        {
            return base.CanExecute(parameter);
        }

        public void Execute(T parameter)
        {
            base.Execute(parameter);
        }

        #region ICommandViewModel Members

        public string Name { get; set; }

        #endregion

        #region INotifyPropertyChanged Members

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    public class CommandViewModel : DelegateCommandBase, ICommandViewModel
    {
        public CommandViewModel(Action executeMethod)
            : this(executeMethod, () => true)
        { }

        public CommandViewModel(Action executeMethod, Func<bool> canExecuteMethod)
            : base((o) => executeMethod(), (o) => canExecuteMethod())
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");
        }

        #region ICommandViewModel Members

        public string Name { get; set; }

        #endregion

        public void Execute()
        {
            Execute(null);
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }

        #region INotifyPropertyChanged Members

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

}
