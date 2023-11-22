using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyEx1.Models
{
    [Table("tbAdmin")]
    public class Admin
    {
        [Key]
        [StringLength(20)]
        public string? Username { get; set; }
        [StringLength(20)]
        public string? Password { get; set; }
    }
}
