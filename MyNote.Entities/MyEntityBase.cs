using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Entities
{
    public class MyEntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} alanı gereklidir.")]
        public DateTime CreatedOn { get; set; }

        [Required(ErrorMessage = "{0} alanı gereklidir.")]
        public DateTime ModifiedOn { get; set; }

        [Required(ErrorMessage = "{0} alanı gereklidir."), 
            StringLength(30, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string ModifiedUsername { get; set; }
    }
}
