using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyNote.Entities.ValueObjects
{
    public class RegisterViewModel
    {
        [Display(Name = "Kullanıcı Adı"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        public string Username { get; set; }

        [Display(Name = "E-posta"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez."), 
            EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Display(Name = "Şifre"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        public string Password { get; set; }

        [Display(Name = "Şifre Tekrarı"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez."), 
            Compare("Password",ErrorMessage = "{0} ile {1} uyuşmuyor.")]
        public string RePassword { get; set; }
    }
}