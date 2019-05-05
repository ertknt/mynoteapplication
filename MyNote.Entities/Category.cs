using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Entities
{
    [Table("Categories")]
    public class Category : MyEntityBase
    {
       
        [Display(Name ="Kategori"), 
            Required(ErrorMessage = "{0} alanı gereklidir."), 
            StringLength(50)]
        public string Title { get; set; }

        [Display(Name = "Açıklama"), 
            Required(ErrorMessage = "{0} alanı gereklidir."),  
            StringLength(150)]
        public string Description { get; set; }

        public virtual List<Note> Notes { get; set; }

        public Category()
        {
            Notes = new List<Note>();
        }
    }
}
