using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Scope.Wpf.Controls
{
    class Command : ICommand
    {
        private Action methodToExecute = null;
        private Func<bool> methodToDetectCanExecute = null;

        public Command(Action methodToExecute, Func<bool> methodToDetectCanExecute)
        {
            this.methodToExecute = methodToExecute;
            this.methodToDetectCanExecute = methodToDetectCanExecute;
        }

        // This event is triggered when the state of the ViewModel changes. Under
        // these circumstances, commands that could previously run might now be
        // disabled, and vice versa.
        public event EventHandler CanExecuteChanged;

        // This method is called when the state of the ViewModel changes
        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return methodToDetectCanExecute == null ? true : methodToDetectCanExecute();
        }

        public void Execute(object parameter)
        {
            methodToExecute?.Invoke();
        }
    }
}
