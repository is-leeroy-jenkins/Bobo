namespace Bobo
{
    using System;
    using System.Windows.Input;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:System.Windows.Input.ICommand" />
    public class ClearFontSettingsCommand : ICommand
    {
        /// <summary>
        /// The trainer view model
        /// </summary>
        private readonly TrainerViewModel _trainerViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClearFontSettingsCommand"/> class.
        /// </summary>
        /// <param name="trainerViewModel">The trainer view model.</param>
        public ClearFontSettingsCommand(TrainerViewModel trainerViewModel)
        {
            _trainerViewModel = trainerViewModel;
        }

        /// <summary>
        /// Occurs when changes occur that affect
        /// whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged { add { } remove { } }

        /// <summary>
        /// Determines whether this instance
        /// can execute the specified .
        /// </summary>
        /// <param name="_">The .</param>
        /// <returns>
        ///   <c>true</c> if this instance
        /// can execute the specified ; otherwise, <c>false</c>.
        /// </returns>
        public bool CanExecute(object _)
        {
            return true;
        }

        /// <summary>
        /// Executes the specified .
        /// </summary>
        /// <param name="_">The .</param>
        public void Execute(object _)
        {
            _trainerViewModel?.TrainingFontSettingList?.Clear( );
        }
    }
}
