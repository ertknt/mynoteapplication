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


            //kategori ile ilişkili notların silinmesi gerekiyor.
            foreach (Note note in category.Notes.ToList()) //kategorinin notları.
            {
                //Note ile ilişkili like ların silinmesi gerekiyor.
                foreach (Liked liked in note.Likes.ToList())
                {
                    likedManager.Delete(liked); //nota ait like ların silinmesi.
                }

                //Note ile ilişkili comment lerin silinmesi gerekiyor.
                foreach (Comment comment in note.Comments.ToList())
                {
                    commentManager.Delete(comment); // nota ait commentlerin silinmesi.
                }


                noteManager.Delete(note); //kategoriye ait notların silinmesi.
            }

            return base.Delete(category);
        }

    }
}
