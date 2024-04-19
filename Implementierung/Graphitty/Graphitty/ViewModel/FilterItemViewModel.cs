using Graphitty.Model.Filters;
using Prism.Mvvm;

namespace Graphitty.ViewModel
{
    /// <summary>
    /// The wrapper ViewModel for all filters.
    /// </summary>
    public class FilterItemViewModel : BindableBase
    {
        #region Private Fields

        private string args;
        private string displayName;
        private IFilter filter;
        private string id;

        #endregion Private Fields

        #region Public Constructors

        public FilterItemViewModel(IFilter filter)
        {
            this.filter = filter;
            DisplayName = filter.Displayname;
            Id = filter.Id;
        }

        public FilterItemViewModel(IFilter filter, string args)
        {
            this.filter = filter;
            DisplayName = filter.Displayname;
            Id = filter.Id;
            Args = args;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Args
        {
            get { return args; }
            set
            {
                SetProperty(ref args, value);
                RaisePropertyChanged("Args");
            }
        }

        public string DisplayName
        {
            get { return displayName; }
            set
            {
                SetProperty(ref displayName, value);
                RaisePropertyChanged("DisplayName");
            }
        }

        public string Id
        {
            get { return id; }
            set
            {
                SetProperty(ref id, value);
                RaisePropertyChanged("Id");
            }
        }

        #endregion Public Properties
    }
}