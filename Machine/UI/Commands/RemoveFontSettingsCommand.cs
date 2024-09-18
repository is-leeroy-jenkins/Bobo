namespace Bobo
{
    using System;
    using System.Windows.Input;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:System.Windows.Input.ICommand" />
    public class RemoveFontSettingsCommand : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="RemoveFontSettingsCommand"/> class.
        /// </summary>
        /// <param name="trainer">The trainer.</param>
        public RemoveFontSettingsCommand(TrainerViewModel trainer)
        {
            _trainer = trainer;
        }

        private readonly TrainerViewModel _trainer;

        public event EventHandler CanExecuteChanged { add { } remove { } }

        public bool CanExecute(object _)
        {
            return true;
        }

        public void Execute(object _)
        {
            for (var _index = 0; _index < _trainer.TrainingFontSettingList.Count; _index++)
            {
                if (_trainer.TrainingFontSettingList[_index].FontName == _trainer.FontSetting.FontName)
                {
                    _trainer.TrainingFontSettingList.RemoveAt(_index);
                    return;
                }
            }
        }
    }
}
