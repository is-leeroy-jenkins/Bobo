namespace Bobo
{
    using System;
    using System.Windows.Input;

    public class AddFontSettingCommand : ICommand
    {
        public AddFontSettingCommand(TrainerViewModel trainer)
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
                if (_trainer.FontSetting.FontName == _trainer.TrainingFontSettingList[_index].FontName)
                {
                    return;
                }
            }

            _trainer.TrainingFontSettingList.Add(new FontSetting(_trainer.FontSetting));
        }
    }
}
