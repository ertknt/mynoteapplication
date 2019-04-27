using MyNote.BusinessLayer.Abstract;
using MyNote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        public override int Delete(Category category)
        {
            NoteManager noteManager = new NoteManager();
            LikedManager likedManager = new LikedManager();
            CommentManager commentManager = new CommentManager();

            //Kategori ile ilişkili notların, beğenilerin, yorumların silinmesi gerekiyor.
            foreach ( Note note in category.Notes.ToList())
            {

                // not ile ilişkili beğenilerin silinmesi.
                foreach (Liked liked in note.Likes.ToList())
                {
                    likedManager.Delete(liked);
                }

                // not ile ilişkili yorumların silinmesi
                foreach (Comment comment in note.Comments.ToList())
                {
                    commentManager.Delete(comment);
                }

                noteManager.Delete(note);
            }


            return base.Delete(category);
        }

    }
}
