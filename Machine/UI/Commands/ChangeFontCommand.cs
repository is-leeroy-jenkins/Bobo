

namespace Bobo
{
    using System;
    using OpenCvSharp;
    using System.Drawing;
    using System.Windows.Input;

    public class ChangeFontCommand : ICommand
    {
        public ChangeFontCommand(TrainerViewModel trainer)
        {
            _trainer = trainer;
        }

        private readonly TrainerViewModel _trainer;
    
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is int _step)
            {
                _trainer.SelectedFontIndex += _step;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }
    }
}
