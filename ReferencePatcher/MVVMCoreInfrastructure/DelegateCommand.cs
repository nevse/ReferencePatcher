using System;
using System.Linq;
using System.Windows.Input;

namespace ReferencePatcher {
    public class DelegateCommand : ICommand {
        Action handler;
        bool isEnabled = true;

        public DelegateCommand(Action handler) {
            this.handler = handler;
        }

        public bool IsEnabled {
            get { return this.isEnabled; }
        }
        
        public event EventHandler CanExecuteChanged;

        void ICommand.Execute(object arg) {
            this.handler();
        }

        bool ICommand.CanExecute(object arg) {
            return this.IsEnabled;
        }

        void OnCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }        
    }
}
