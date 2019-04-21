using MyNote.DataAccessLayer.EntityFramework;
using MyNote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.BusinessLayer
{
    public class Test
    {
        private Repository<MyNoteUser> repo_user = new Repository<MyNoteUser>();
        private Repository<Category> repo_category = new Repository<Category>();
        private Repository<Comment> repo_comment = new Repository<Comment>();
        private Repository<Note> repo_note = new Repository<Note>();


        public Test()
        {

            List<Category> categories = repo_category.List();
        }

        public void InsertTest()
        {
            int result = repo_user.Insert(new MyNoteUser()
            {

                Name = "AAAErtan",
                Surname = "AAAKanter",
                Email = "ertankanter@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsAdmin = false,
                IsActive = true,
                Username = "ertankanter",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "ertankanter"

            });
        }

        public void UpdateTest()
        {
            MyNoteUser user = repo_user.Find(x => x.Name == "John");

            if (user != null)
            {
                user.Username = "xxx";

                int result = repo_user.Update(user);
            }

        }

        public void DeleteTest()
        {
            MyNoteUser user = repo_user.Find(x => x.Name == "AAAErtan");

            if (user != null)
            {
                repo_user.Delete(user);
            }
        }

        public void CommentTest()
        {
            MyNoteUser user = repo_user.Find(x => x.Id == 1);
            Note note = repo_note.Find(x => x.Id == 3);

            Comment comment = new Comment()
            {
                Text = "Bu bir testtir.",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "ertankanter",
                Note = note,
                Owner = user
            };

            repo_comment.Insert(comment);
        }
    }
}
