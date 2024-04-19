using System.Diagnostics;
using Graphitty.Model.Graphs;
using Graphitty.Model.Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphitty.Model.DataAccessLayer;
using GraphittyTest.Model.DataAccessLayer;
using Graphitty.Model.Filters;
using System.Collections.Generic;

namespace GraphittyTest.Model.Algorithms
{
    [TestClass]
    public class Benchmarks
    {
        #region Public Methods

        //[TestMethod]
        public void Benchmark1()
        {
            IRepository<GraphEntity> grepo = new RepositoryMock<GraphEntity>(new List<GraphEntity>());
            IRepository<FilterEntity> frepo = new RepositoryMock<FilterEntity>(new List<FilterEntity>());
            IUnitOfWork uoW = new UnitOfWorkMock(grepo, frepo);
            Stopwatch s = new Stopwatch();
            Graph g;
            AlgorithmRunner ar = new AlgorithmRunner(uoW);

            g = new Graph("1,1,2;1,1,3;-1,2,3,0");
            s.Start();
            ar.RunAlgorithms(g);
            s.Stop();
            Debug.WriteLine("Benchmark: 3V 3E, TCN: 3. Algorithms took: " + s.ElapsedMilliseconds + "ms.");
            s.Reset();

            g = new Graph("1,1,2;1,1,3;-1,2,3;1,1,4,0");
            s.Start();
            ar.RunAlgorithms(g);
            s.Stop();
            Debug.WriteLine("Benchmark: 4V 4E, TCN: 4. Algorithms took: " + s.ElapsedMilliseconds + "ms.");
            s.Reset();

            g = new Graph("1,1,2;1,1,3;1,2,4;1,3,5,0");
            s.Start();
            ar.RunAlgorithms(g);
            s.Stop();
            Debug.WriteLine("Benchmark: 5V 4E, TCN: 3. Algorithms took: " + s.ElapsedMilliseconds + "ms.");
            s.Reset();

            g = new Graph("1,1,2;1,1,3;-1,2,3;1,1,4;-1,2,4;1,3,5;-1,4,5,0");
            s.Start();
            ar.RunAlgorithms(g);
            s.Stop();
            Debug.WriteLine("Benchmark: 5V 7E, TCN: 4. Algorithms took: " + s.ElapsedMilliseconds + "ms.");
            s.Reset();

            g = new Graph("1,1,2;1,1,3;-1,2,3;1,1,4;-1,2,4;-1,3,4;1,1,5;-1,2,5;-1,3,5;-1,4,5;1,1,6;-1,2,6;-1,3,6;-1,4,6,0");
            s.Start();
            ar.RunAlgorithms(g);
            s.Stop();
            Debug.WriteLine("Benchmark: 6V 14E, TCN: 7. Algorithms took: " + s.ElapsedMilliseconds + "ms.");
            s.Reset();

            g = new Graph("1,1,2;1,1,3;-1,2,3;1,1,4;1,2,5;1,3,6;1,4,7,0");
            s.Start();
            ar.RunAlgorithms(g);
            s.Stop();
            Debug.WriteLine("Benchmark: 7V 7E, TCN: 4. Algorithms took: " + s.ElapsedMilliseconds + "ms.");
            s.Reset();

            g = new Graph("1,1,2;1,1,3;-1,2,3;1,1,4;-1,2,4;-1,3,4;1,1,5;-1,2,5;1,3,6;-1,4,6;-1,5,6;1,5,7;-1,6,7,0");
            s.Start();
            ar.RunAlgorithms(g);
            s.Stop();
            Debug.WriteLine("Benchmark: 7V 13E, TCN: 6. Algorithms took: " + s.ElapsedMilliseconds + "ms.");
            s.Reset();

            g = new Graph("1,1,2;1,1,3;-1,2,3;1,1,4;-1,2,4;1,1,5;-1,3,5;1,1,6;-1,4,6;1,2,7;1,5,8,0");
            s.Start();
            ar.RunAlgorithms(g);
            s.Stop();
            Debug.WriteLine("Benchmark: 8V 11E, TCN: 6. Algorithms took: " + s.ElapsedMilliseconds + "ms.");
            s.Reset();

            g = new Graph("1,1,2;1,1,3;-1,2,3;1,1,4;-1,2,4;-1,3,4;1,1,5;-1,2,5;-1,3,5;-1,4,5;1,1,6;-1,2,6;-1,3,6;1,1,7;-1,2,7;-1,4,7;-1,6,7;1,1,8;-1,3,8;-1,4,8;-1,6,8;-1,7,8,0");
            s.Start();
            ar.RunAlgorithms(g);
            s.Stop();
            Debug.WriteLine("Benchmark: 8V 22E, TCN: 8. Algorithms took: " + s.ElapsedMilliseconds + "ms.");
            s.Reset();

            g = new Graph("1,1,2;1,1,3;-1,2,3;1,1,4;-1,2,4;-1,3,4;1,1,5;-1,2,5;-1,3,5;-1,4,5;1,1,6;-1,2,6;-1,3,6;-1,4,6;-1,5,6;1,1,7;-1,2,7;-1,3,7;-1,4,7;-1,5,7;-1,6,7;1,1,8;-1,2,8;-1,3,8;-1,4,8;-1,5,8;-1,6,8;1,1,9;-1,2,9;-1,3,9;-1,4,9;-1,5,9;-1,7,9;-1,8,9,0");
            s.Start();
            ar.RunAlgorithms(g);
            s.Stop();
            Debug.WriteLine("Benchmark: 9V 34E, TCN: 10. Algorithms took: " + s.ElapsedMilliseconds + "ms.");
            s.Reset();

            g = new Graph("1,1,2;1,1,3;1,1,4;1,2,5;1,2,6;1,3,7;1,4,8;1,7,9,0");
            s.Start();
            ar.RunAlgorithms(g);
            s.Stop();
            Debug.WriteLine("Benchmark: 9V 8E, TCN: 4. Algorithms took: " + s.ElapsedMilliseconds + "ms.");
            s.Reset();

            g = new Graph("1,1,2;1,1,3;1,1,4;1,2,5;1,2,6;1,3,7;-1,4,7;-1,5,7;1,5,8;1,6,9;1,6,10,0");
            s.Start();
            ar.RunAlgorithms(g);
            s.Stop();
            Debug.WriteLine("Benchmark: 10V 11E, TCN: 5. Algorithms took: " + s.ElapsedMilliseconds + "ms.");
            s.Reset();

            g = new Graph("1,1,2;1,1,3;-1,2,3;1,1,4;-1,2,4;-1,3,4;1,1,5;-1,2,5;-1,3,5;1,1,6;-1,2,6;-1,3,6;1,1,7;-1,4,7;-1,5,7;-1,6,7;1,2,8;-1,4,8;1,4,9;-1,5,9;-1,6,9;-1,7,9;-1,8,9;1,5,10;-1,6,10;-1,7,10;-1,9,10,0");
            s.Start();
            ar.RunAlgorithms(g);
            s.Stop();
            Debug.WriteLine("Benchmark: 10V 27E, TCN: 8. Algorithms took: " + s.ElapsedMilliseconds + "ms.");
            s.Reset();
        }

        #endregion Public Methods
    }
}