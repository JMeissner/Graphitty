using Graphitty.ViewModel.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace Graphitty.ViewModel
{
    /// <summary>
    /// Provides current log file content and a clear commnd.
    /// Subscribes to the WriteLogEvent.
    /// </summary>
    /// <see cref="ViewModel.Events.WriteLogEvent"/>
    /// <see cref="View.ConsoleView"/>
    public class ConsoleViewModel : BindableBase
    {
        #region Private Fields

        private IEventAggregator eventAggregator;
        private String logText = string.Empty;
        private string path;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Creates a new ConsoleViewModel.
        /// </summary>
        /// <param name="eventAggregator">Used to subscribe the WriteLogEvent.</param>
        /// <param name="logPath">The initial logPath.</param>
        public ConsoleViewModel(IEventAggregator eventAggregator, string logPath)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<WriteLogEvent>().Subscribe(s => appendLine(s));
            Path = logPath;
            ClearCommand = new DelegateCommand<object>(onClear);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Clears the content of the current log file.
        /// </summary>
        public ICommand ClearCommand { get; private set; }

        /// <summary>
        /// Text of the current log file.
        /// </summary>
        public String LogText
        {
            get
            {
                if (path != null && !File.Exists(path))
                {
                    File.Create(path);
                }
                else if (path != null)
                {
                    return File.ReadAllText(path);
                }
                return logText;
            }
            set
            {
                if (path != null)
                {
                    File.WriteAllText(path, value);
                }
                RaisePropertyChanged("LogText");
            }
        }

        /// <summary>
        /// Path to the current log file.
        /// </summary>
        public string Path
        {
            get => path;
            set
            {
                SetProperty(ref path, value);
                RaisePropertyChanged("Path");
                RaisePropertyChanged("LogText");
            }
        }

        #endregion Public Properties

        #region Private Methods

        private bool appendLine(string line)
        {
            StringBuilder sb = new StringBuilder(LogText);
            sb.AppendLine(DateTime.Now.ToLocalTime() + "> " + line);
            LogText = sb.ToString();
            return true;
        }

        private void onClear(object arg)
        {
            LogText = String.Empty;
        }

        #endregion Private Methods
    }
}