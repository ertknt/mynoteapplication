using MyNote.Common;
using MyNote.DataAccessLayer.EntityFramework;
using MyNote.Entities;
using MyNote.Entities.Messages;
using MyNote.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.BusinessLayer
{
    public class MyNoteUserManager
    {
        private Repository<MyNoteUser> repo_user = new Repository<MyNoteUser>();

        public BusinessLayerResult<MyNoteUser> RegisterUser(RegisterViewModel data)
        {
            //kullanıcı username kontrolü
            //kullanıcı e posta kontrolü
            //kayıt işlemi
            //aktivasyon e postası gönderimi

            MyNoteUser user = repo_user.Find(x => x.Username == data.Username || x.Email == data.Email); //kullanıcı var mı?
            BusinessLayerResult<MyNoteUser> layerResult = new BusinessLayerResult<MyNoteUser>();

            if (user != null)
            {
                if (user.Username == data.Username) //username aynı mı?
                {
                    layerResult.AddError(ErrorMessagesCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı.");
                }

                if (user.Email == data.Email) //email aynı mı?
                {
                    layerResult.AddError(ErrorMessagesCode.EmailAldreadyExist, "E-mail adresi kayıtlı.");
                }
            }

            else
            {
                int dbResult = repo_user.Insert(new MyNoteUser()
                {
                    Username = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ActivateGuid = Guid.NewGuid(),
                    ModifiedUsername = App.Common.GetCurrentUsername(),
                    IsActive = false,
                    IsAdmin = false

                });

                if (dbResult > 0)
                {
                    layerResult.Result = repo_user.Find(x => x.Email == data.Email && x.Username == data.Username);

                    //aktivasyon maili atılacak
                    //layerResult.Result.ActivateGuid
                }
            }

            return layerResult;



        }

        public BusinessLayerResult<MyNoteUser> LoginUser(LoginViewModel data)
        {
            //Giriş kontrolü ve yönlendirme
            //Sessiona kullanıcı bilgi saklama

            BusinessLayerResult<MyNoteUser> res = new BusinessLayerResult<MyNoteUser>();
            res.Result = repo_user.Find(x => x.Username == data.Username && x.Password == data.Password); //kayıt eşleşti mi?



            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessagesCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir.");
                    res.AddError(ErrorMessagesCode.CheckYourEmail, "E-posta adresinizi kontrol ediniz.");
                }

            }

            else
            {
                res.AddError(ErrorMessagesCode.UsernameOrPassWrong, "Kullanıcı adı kayıtlı."); ;
            }

            return res;
        }
    }
}
