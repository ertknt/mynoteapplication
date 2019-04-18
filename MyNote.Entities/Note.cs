using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Entities
{
   public class Note : MyEntityBase
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public bool IsDraft { get; set; } //taslak mı?

        public int LikeCount { get; set; }

        public int CategoryId { get; set; }

        public virtual MyNoteUser Owner { get; set; }

        public virtual List<Comment> Notes { get; set; }

        public virtual Category Category { get; set; }

        public virtual List<Liked> Likes { get; set; }


    }
}
