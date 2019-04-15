using System;
using System.Windows.Input;

namespace HWEnchCalc.Common
{
    public class WpfCommand : ICommand
    {
        private readonly Action _act;

        public bool CanExecute(object val)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object val)
        {
            _act();
        }

        public WpfCommand(Action act)
        {
            _act = act;
        }
    }
}