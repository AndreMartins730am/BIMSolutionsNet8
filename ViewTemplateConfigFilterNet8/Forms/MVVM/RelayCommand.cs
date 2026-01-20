using System;
using System.Windows.Input;

namespace ViewTemplateConfigFilterNet8.Forms.MVVM
{
    /// <summary>
    /// Simple implementation of the <see cref="ICommand"/> for MVVM.
    /// Encapsulate the action to be executed and, optionally, the enabling rule (CanExecute).
    /// </summary>
    internal class RelayCommand : ICommand
    {
        /// <summary>
        /// Action executed when the command is trigged/invoked.
        /// </summary>
        private Action<object?> execute;

        /// <summary>
        /// Predicate that defines, whether the command can execute for a parameter.
        /// When null, the command is considered always enabled.
        /// </summary>
        private Func<object, bool> canExecute;        

        //predicado é só um função que recebe algo e devolve true ou false
        //Predicate is a function that receive a function and return true or false

        /// <summary>
        /// Default event of <see cref="ICommand"/>. WPF re-queries  CanExecute when
        /// the <see cref="CommandManager.RequerySuggested"/> is trigged (change focus, input, etc).
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove {  CommandManager.RequerySuggested -= value;}
        }

        /// <summary>
        /// Creates a command with a mandatory action and an optional condition.
        /// </summary>
        /// <param name="execute">Action to be executed.</param>
        /// <param name="canExecute">Optional predicate to enable/disable the command.</param>
        public RelayCommand(Action<object?> execute, Func<object, bool> canExecute = null!)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        #region Methods .

        /// <summary>
        /// Indicates to WPF whether the command is enabled to the current parameter.
        /// </summary>
        /// <param name="parameter">Parameter passed by the command system(may be null)</param>
        /// <returns>Return True if it can execute, otherwise, False</returns>        
        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter!);
            //canExecute == null, convert to true 
            //or
            //Predicate function return true Func<object, bool>
            //public delegate TResult Func<in T, out TResult>(T arg);
            //the TResult is a boolean
        }

        /// <summary>
        /// Execute a action encapsulated(wrapped) by the command.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object? parameter)
        {
            execute(parameter);
        } 
        #endregion
    }
}