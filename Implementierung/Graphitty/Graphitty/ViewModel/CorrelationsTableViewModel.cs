using Graphitty.Model.DataAccessLayer;
using Graphitty.Model.Graphs;
using Graphitty.ViewModel.Events;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Graphitty.ViewModel
{
    /// <summary>
    /// Offers a collection of correlation coefficients for all graph attributes which have a numeric value or can be converted to one.
    /// It subscribes to the DatabaseChangedEvent to calculate the correlations.
    /// </summary>
    public class CorrelationsTableViewModel : BindableBase
    {
        #region Private Fields

        private ObservableCollection<CorrelationCellViewModel> correlations;
        private IEventAggregator eventAggregator;
        private int nMostCorrelated;
        private IUnitOfWork unitOfWork;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Creates a CorrelationsTableViewModel. It creates all possible the combinations of attributes which have a numeric value,
        /// by creating a corresponding CorrelationCellViewModel.
        /// </summary>
        /// <param name="eventAggregator">Used to subscribe to the DataBaseChangedCommand.</param>
        public CorrelationsTableViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<DatabaseChangedEvent>().Subscribe(() =>
            {
                calcCoefficients();
                RaisePropertyChanged("Correlations");
            });
            unitOfWork = UnitOfWork.Instance;
            correlations = createCorrelationCells();
            NMostCorrelated = correlations.Count;
        }

        /// <summary>
        /// Creates a CorrelationsTableViewModel and initializes the used UnitOfWork with the given reference.
        /// </summary>
        /// <param name="eventAggregator"></param>
        /// <param name="unitOfWork"></param>
        /// <see cref="CorrelationsTableViewModel.CorrelationsTableViewModel(IEventAggregator)"/>
        public CorrelationsTableViewModel(IEventAggregator eventAggregator, IUnitOfWork unitOfWork) : this(eventAggregator)
        {
            this.unitOfWork = unitOfWork;
            correlations = createCorrelationCells();
            NMostCorrelated = correlations.Count;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Collection of all correlations with coefficient >= threshold and ordered by correlation-strength descending.
        /// </summary>
        public ObservableCollection<CorrelationCellViewModel> Correlations
        {
            get
            {
                var filteredCorrelations = correlations.OrderByDescending(c => Math.Abs(c.Coefficient)).Take(nMostCorrelated);
                return new ObservableCollection<CorrelationCellViewModel>(filteredCorrelations);
            }
        }

        /// <summary>
        /// Number of n most correlated values which should be displayed.
        /// </summary>
        [ExcludeFromCodeCoverageAttribute]
        public int NMostCorrelated
        {
            get => nMostCorrelated;
            set
            {
                SetProperty(ref nMostCorrelated, value);
                RaisePropertyChanged("NMostCorrelated");
                RaisePropertyChanged("Correlations");
            }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Calculates the Pearson correlation coefficients for each CorrelationCellViewModel.
        /// </summary>
        /// <remarks>
        /// The property has to be a numeric value or a property that can be converted to a numeric value.
        /// </remarks>
        private void calcCoefficients()
        {
            var allGraphs = unitOfWork.GraphRepository.GetAll().ToList();
            int n = allGraphs.Count();
            foreach (CorrelationCellViewModel c in correlations)
            {
                var firstPropertyValues = allGraphs.Select(x => Convert.ToDouble(x.GetType().GetProperty(c.FirstProperty).GetValue(x)));
                var secondPropertyValues = allGraphs.Select(x => Convert.ToDouble(x.GetType().GetProperty(c.SecondProperty).GetValue(x)));
                if (firstPropertyValues.Count() == 0 || secondPropertyValues.Count() == 0)
                {
                    c.Coefficient = 0;
                    continue;
                }
                try
                {
                    // sum_i^n x(i)*y(i)
                    var sumXTimesY = firstPropertyValues.Zip(secondPropertyValues, (first, second) =>
                    {
                        return first * second;
                    }
                    ).Sum();

                    // (1/n) sum_i^n x(i)
                    var avgX = firstPropertyValues.Average();
                    // (1/n) sum_i^n y(i)
                    var avgY = secondPropertyValues.Average();

                    // sum_i^n x(i)
                    var sumXPowTwo = firstPropertyValues.Select(i => Math.Pow(i, 2)).Sum();
                    // sum_i^n y(i)
                    var sumYPowTwo = secondPropertyValues.Select(i => Math.Pow(i, 2)).Sum();

                    var numerator = (sumXTimesY - n * avgX * avgY);
                    var denominator = Math.Sqrt(sumXPowTwo - n * Math.Pow(avgX, 2)) * Math.Sqrt(sumYPowTwo - n * Math.Pow(avgY, 2));
                    if (numerator != 0 && denominator != 0)
                    {
                        c.Coefficient = numerator / denominator;
                    }
                    else
                    {
                        c.Coefficient = 0;
                    }
                }
                catch (InvalidCastException e)
                {
                    c.Coefficient = 0;
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.Data);
                }
            }
        }

        /// <summary>
        /// Creates all possible correlation cells.
        /// </summary>
        /// <returns>All possible combilations of properties which can deliver a correlation coefficient.</returns>
        private ObservableCollection<CorrelationCellViewModel> createCorrelationCells()
        {
            ObservableCollection<CorrelationCellViewModel> res = new ObservableCollection<CorrelationCellViewModel>();
            List<PropertyInfo> properties = new List<PropertyInfo>(typeof(GraphEntity).GetProperties());
            int n = properties.Count;
            properties.RemoveAll(pr => !isRelevantProperty(pr));
            var p = properties.GetEnumerator();
            for (int i = 0; i < (Math.Pow(n, 2) - n) / 2; i++)
            {
                p.MoveNext();
                res.AddRange(Enumerable.Repeat<PropertyInfo>(p.Current, n).Zip(properties.Skip(i + 1), (first, second) => new CorrelationCellViewModel(first.Name, second.Name)));
            }
            return res;
        }

        /// <summary>
        /// Returns whether a property is relevant.
        /// </summary>
        /// <param name="property">Property to be checked.</param>
        /// <returns>If a property is relevant for calculating the correlations.</returns>
        private bool isRelevantProperty(PropertyInfo property)
        {
            //list here all properties which are numeric but not required
            if (property.Name == "Id")
            {
                return false;
            }
            //check if property is numeric
            switch (Type.GetTypeCode(property.PropertyType))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                case TypeCode.Boolean:
                    return true;

                default:
                    return false;
            }
        }

        #endregion Private Methods
    }
}