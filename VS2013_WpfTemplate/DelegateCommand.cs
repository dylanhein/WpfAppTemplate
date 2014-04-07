﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Ink;

namespace VS2013_WpfTemplate
{
    #region DelegateCommand Implementation

    /// <summary>
    /// From Kent Boogart's blog http://kentb.blogspot.com/2009/05/mvvm-infrastructure-activeawarecommand.html
    /// </summary>
    public class DelegateCommand : Command
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        /// <summary>
        /// Constructs an instance of <c>DelegateCommand</c>.
        /// </summary>
        /// <remarks>
        /// This constructor creates the command without a delegate for determining whether the command can execute. Therefore, the
        /// command will always be eligible for execution.
        /// </remarks>
        /// <param name="execute">
        /// The delegate to invoke when the command is executed.
        /// </param>
        public DelegateCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Constructs an instance of <c>DelegateCommand</c>.
        /// </summary>
        /// <param name="execute">
        /// The delegate to invoke when the command is executed.
        /// </param>
        /// <param name="canExecute">
        /// The delegate to invoke to determine whether the command can execute.
        /// </param>
        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Determines whether this command can execute.
        /// </summary>
        /// <remarks>
        /// If there is no delegate to determine whether the command can execute, this method will return <see langword="true"/>. If a delegate was provided, this
        /// method will invoke that delegate.
        /// </remarks>
        /// <param name="parameter">
        /// The command parameter.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the command can execute, otherwise <see langword="false"/>.
        /// </returns>
        public override bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <remarks>
        /// This method invokes the provided delegate to execute the command.
        /// </remarks>
        /// <param name="parameter">
        /// The command parameter.
        /// </param>
        public override void Execute(object parameter)
        {
            _execute(parameter);
        }
    }

    /// <summary>
    /// From Kent Boogart's blog http://kentb.blogspot.com/2009/05/mvvm-infrastructure-activeawarecommand.html
    /// </summary>
    public abstract class Command : ICommand
    {
        private readonly Dispatcher _dispatcher;

        /// <summary>
        /// Constructs an instance of <c>CommandBase</c>.
        /// </summary>
        protected Command()
        {
            if (Application.Current != null)
            {
                _dispatcher = Application.Current.Dispatcher;
            }
            else
            {
                //this is useful for unit tests where there is no application running
                _dispatcher = Dispatcher.CurrentDispatcher;
            }

            Debug.Assert(_dispatcher != null);
        }

        /// <summary>
        /// Occurs whenever the state of the application changes such that the result of a call to <see cref="CanExecute"/> may return a different value.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Determines whether this command can execute.
        /// </summary>
        /// <param name="parameter">
        /// The command parameter.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the command can execute, otherwise <see langword="false"/>.
        /// </returns>
        public abstract bool CanExecute(object parameter);

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="parameter">
        /// The command parameter.
        /// </param>
        public abstract void Execute(object parameter);

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.Invoke((ThreadStart)OnCanExecuteChanged, DispatcherPriority.Normal);
            }
            else
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }

    #endregion
}
