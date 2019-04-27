using MyNote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.DataAccessLayer.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        public DbSet<MyNoteUser> MyNoteUsers { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Liked> Likes { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    // FluentAPI

        //    modelBuilder.Entity<Note>()
        //        .HasMany(n => n.Comments) //çok ilişkili
        //        .WithRequired(c => c.Note) //not null
        //        .WillCascadeOnDelete(true); //ilişki entity yi de silsin


        //    modelBuilder.Entity<Note>()
        //        .HasMany(n => n.Likes) //çok ilişkili
        //        .WithRequired(c => c.Note) //not null
        //        .WillCascadeOnDelete(true); //ilişki entity yi de silsin

        //}
    }
}
