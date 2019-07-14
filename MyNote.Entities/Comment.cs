using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Entities
{
    [Table("Comments")]
    public class Comment : MyEntityBase
    {
        [Required(ErrorMessage = "{0} alanı gereklidir."), 
            StringLength(300, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Text { get; set; }

        public virtual Note Note { get; set; }

        public virtual  MyNoteUser Owner { get; set; }

    }
}
