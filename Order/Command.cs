using System;
using System.Windows.Input;

namespace Order {
    public class Command<T> : ICommand {
        Action<object> ExecuteMethod;
        
        private Action<T> commandExecuteAction;
        private Func<object, bool> commandCanExecute;
        private EventArgs args;

        /*
        Func<object, bool> CanExecuteFunc;
        public Command(Action<object> execute_Method, Func<object, bool> canexecute_Method) {
            this.ExecuteMethod = execute_Method;
            this.CanExecuteFunc = canexecute_Method;
        }*/

        public Command(Action<T> commandExecuteAction, Func<object, bool> commandCanExecute) {
            this.commandExecuteAction = commandExecuteAction;
            this.commandCanExecute = commandCanExecute;
        }


        public Command(Action<T> commandExecuteAction, EventArgs arg) {
            this.commandExecuteAction = commandExecuteAction;
            this.args = arg;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            ExecuteMethod(parameter);
        }
    }
}
