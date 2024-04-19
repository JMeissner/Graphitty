using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graphitty.Model.Filters
{
    [Table("Filter")]
    public class FilterEntity
    {
        #region Public Properties

        [Key, Column(Order = 3)]
        public string Args { get; set; }

        [Key, Column(Order = 2)]
        public int GroupID { get; set; }

        [Key, Column(Order = 1)]
        public string ID { get; set; }

        #endregion Public Properties
    }
}