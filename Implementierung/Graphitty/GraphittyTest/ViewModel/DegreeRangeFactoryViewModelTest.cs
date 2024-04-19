using Graphitty.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphittyTest.ViewModel
{
    [TestClass]
    public class DegreeRangeFactoryViewModelTest
    {
        #region Public Methods

        [TestMethod]
        public void MaxDegreeTest()
        {
            DegreeRangeFactoryViewModel degreeRangeFactoryViewModel = new DegreeRangeFactoryViewModel
            {
                MinDegree = 1,
                MaxDegree = 1
            };

            Assert.AreEqual(2, degreeRangeFactoryViewModel.MaxDegree);
        }

        #endregion Public Methods
    }
}