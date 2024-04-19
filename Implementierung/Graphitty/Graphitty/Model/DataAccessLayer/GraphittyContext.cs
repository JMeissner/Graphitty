using Graphitty.Model.Filters;
using Graphitty.Model.Graphs;
using MySql.Data.Entity;
using System.Data.Entity;

namespace Graphitty.Model.DataAccessLayer
{
    /// <summary>
    /// The database context used by the entity framework.
    /// </summary>
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class GraphittyContext : DbContext
    {
        #region Protected Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<Graph>();
        }

        #endregion Protected Methods

        #region Public Constructors

        public GraphittyContext() : base("GraphittyContext")
        {
        }

        public GraphittyContext(string connectionString) : base(connectionString)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public DbSet<FilterEntity> Filters { get; set; }
        public DbSet<GraphEntity> Graphs { get; set; }

        #endregion Public Properties
    }
}