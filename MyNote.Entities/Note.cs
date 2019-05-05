using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Entities
{
    public class Note : MyEntityBase
    {
        [Display(Name ="Not Başlığı"), Required, StringLength(60)]
        public string Title { get; set; }

        [Display(Name = "Not Metni"), Required, StringLength(2000)]
        public string Text { get; set; }

        [Display(Name = "Taslak")]
        public bool IsDraft { get; set; } //taslak mı?

        [Display(Name = "Beğenilme")]
        public int LikeCount { get; set; }

        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }


        public virtual MyNoteUser Owner { get; set; }

        public virtual Category Category { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Liked> Likes { get; set; }

        public Note()
        {
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }




    }
}
