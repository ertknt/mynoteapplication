using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Entities
{
    [Table("EvernoteUsers")]
    public class MyNoteUser : MyEntityBase
    {
        [Display(Name = "İsim")]
        [StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Name { get; set; }

        [Display(Name = "Soyad")]
        [StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Surname { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "{0} alanı gereklidir."), 
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Username { get; set; }

        [Display(Name = "E-posta")]
        [Required(ErrorMessage = "{0} alanı gereklidir."), 
            StringLength(70, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Email { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "{0} alanı gereklidir."), 
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Password { get; set; }

        [StringLength(30, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır."), 
            ScaffoldColumn(false)]
        public string ProfileImageFilename { get; set; }

        [Display(Name = "Aktif mi?")]
        public bool IsActive { get; set; }

        [Display(Name = "Admin mi?")]
        public bool IsAdmin { get; set; }

        [Required, 
            ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }



        public virtual List<Note> Notes { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Liked> Likes { get; set; }

        public MyNoteUser()
        {
            Notes = new List<Note>();
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }

    }
}
