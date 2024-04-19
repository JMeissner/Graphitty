using Prism.Mvvm;
using System;

namespace Graphitty.ViewModel
{
    /// <summary>
    /// Represents two property header and their correlation coefficient.
    /// </summary>
    public class CorrelationCellViewModel : BindableBase
    {
        #region Private Fields

        private double coefficient;

        private String firstProperty;

        private String secondProperty;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Creates a CorrelationCell and sets its coefficient to zero.
        /// </summary>
        /// <param name="firstProperty"></param>
        /// <param name="secondProperty"></param>
        public CorrelationCellViewModel(String firstProperty, String secondProperty)
        {
            FirstProperty = firstProperty;
            SecondProperty = secondProperty;
            Coefficient = 0;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Correlation coefficient.
        /// </summary>
        public double Coefficient
        {
            get { return Math.Round(coefficient, 2); }
            set
            {
                SetProperty(ref coefficient, value);
                RaisePropertyChanged("Coefficient");
            }
        }

        /// <summary>
        /// Header of the first property.
        /// </summary>
        public String FirstProperty
        {
            get { return firstProperty; }
            private set
            {
                SetProperty(ref firstProperty, value);
                RaisePropertyChanged("FirstProperty");
            }
        }

        /// <summary>
        /// Header of the second property.
        /// </summary>
        public String SecondProperty
        {
            get { return secondProperty; }
            private set
            {
                SetProperty(ref secondProperty, value);
                RaisePropertyChanged("SecondProperty");
            }
        }

        #endregion Public Properties
    }
}