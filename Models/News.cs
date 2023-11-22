using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyEx1.Models
{
    [Table("tbNews")]
    public class News
    {
        [Key]
        public int NewsId { get; set; }
        [StringLength(20)]
        public string? HeadLine { get; set; }
        [DataType(DataType.MultilineText)]
        public string? ContentOfNews { get; set; }
    }
}
