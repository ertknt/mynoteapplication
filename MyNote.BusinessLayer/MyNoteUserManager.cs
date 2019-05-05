using MyNote.BusinessLayer.Abstract;
using MyNote.BusinessLayer.Results;
using MyNote.Common;
using MyNote.Common.Helpers;
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
    public class MyNoteUserManager : ManagerBase<MyNoteUser>
    {

        public BusinessLayerResult<MyNoteUser> RegisterUser(RegisterViewModel data)
        {
            //kullanıcı username kontrolü
            //kullanıcı e posta kontrolü
            //kayıt işlemi
            //aktivasyon e postası gönderimi

            MyNoteUser user =Find(x => x.Username == data.Username || x.Email == data.Email); //kullanıcı var mı?
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
                int dbResult = base.Insert(new MyNoteUser()
                {
                    Username = data.Username,
                    Email = data.Email,
                    ProfileImageFilename = "index.jpg",
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
                    layerResult.Result = Find(x => x.Email == data.Email && x.Username == data.Username);

                    //aktivasyon maili atılacak
                    //layerResult.Result.ActivateGuid
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string body = $"Merhaba {layerResult.Result.Username};<br><br> Hesabınızı akifleştirmek için <a href='activateUri' target='_blank'></a> tıklayınız.";
                    string activateUri = $"{siteUri}/Home/UserActivate/{layerResult.Result.ActivateGuid}";
                    string about = "MyNote Hesap Aktifleştirme";
                    //mail göndermede hata var
                    //MailHelper.SendMail(body, layerResult.Result.Email, about);
                }
            }

            return layerResult;


             
        }

        public BusinessLayerResult<MyNoteUser> GetUserById(int id)
        {
            BusinessLayerResult<MyNoteUser> res = new BusinessLayerResult<MyNoteUser>();
            res.Result = Find(x => x.Id == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessagesCode.UserNotFound, "Kullanıcı bulunamadı.");
            }

            return res;
        }

        public BusinessLayerResult<MyNoteUser> LoginUser(LoginViewModel data)
        {
            //Giriş kontrolü ve yönlendirme
            //Sessiona kullanıcı bilgi saklama

            BusinessLayerResult<MyNoteUser> res = new BusinessLayerResult<MyNoteUser>();
            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password); //kayıt eşleşti mi?



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
                res.AddError(ErrorMessagesCode.UsernameOrPassWrong, "Kullanıcı adı veya şifre hatalı."); ;
            }

            return res;
        }

        public BusinessLayerResult<MyNoteUser> UpdateProfile(MyNoteUser data)
        {
            MyNoteUser user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email)); //kullanıcı kontrolü
            BusinessLayerResult<MyNoteUser> res = new BusinessLayerResult<MyNoteUser>();

            if (user != null && user.Id != data.Id)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessagesCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı.");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessagesCode.EmailAldreadyExist, "E-mail adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;

            if (string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessagesCode.ProfileCouldNotUpdated, "Profil güncellenemedi.");
            }

            return res;
        }

        public BusinessLayerResult<MyNoteUser> RemoveUserById(int id)
        {
            BusinessLayerResult<MyNoteUser> res = new BusinessLayerResult<MyNoteUser>();
            MyNoteUser user = Find(x => x.Id == id);

            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    res.AddError(ErrorMessagesCode.UserCouldNotRemove, "Kullanıcı silinemedi.");
                    return res;
                }
            }

            else
            {
                res.AddError(ErrorMessagesCode.UserCouldNotFind, "Kullanıcı bulunamadı.");
            }

            return res;

        }

        public BusinessLayerResult<MyNoteUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<MyNoteUser> res = new BusinessLayerResult<MyNoteUser>();
            res.Result = Find(x => x.ActivateGuid == activateId); //kullanıcı var mı?

            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessagesCode.UserAldreadyActive, "Kullanıcı zaten aktif edilmiştir.");

                    return res;
                }
                res.Result.IsActive = true;
                Update(res.Result);
            }

            else
            {
                res.AddError(ErrorMessagesCode.ActivateIdDoesNotExist, "Aktifleştirilecek kullanıcı bulunamadı.");
            }

            return res;


        }

        //managerbase classından miras ile gelen insert metodunu override edebilmek için new anahtar kelimesiyle metot gizleme kullandım. Bu metoda dönüş tipi 'Businesslayerresult' nesnesine ihtiyacım olduğu için kullandım. Çünkü hata mesajı basmak istiyorum. Bu metotu kullanmayıp base classdan gelen Insert metodunu kullansaydım dönüş tipi olarak int dönüyordu. 
        public new BusinessLayerResult<MyNoteUser> Insert(MyNoteUser data) 
        {
            //kullanıcı username kontrolü
            //kullanıcı e posta kontrolü
            //kayıt işlemi
            //aktivasyon e postası gönderimi

            MyNoteUser user = Find(x => x.Username == data.Username || x.Email == data.Email); //kullanıcı var mı?
            BusinessLayerResult<MyNoteUser> layerResult = new BusinessLayerResult<MyNoteUser>();

            layerResult.Result = data;

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
                layerResult.Result.ProfileImageFilename = "index.jpg";
                layerResult.Result.ActivateGuid = Guid.NewGuid();

                if(base.Insert(layerResult.Result) == 0)
                {
                    layerResult.AddError(ErrorMessagesCode.UserCouldNotInserted, "Kullanıcı eklenemedi.");

                }



            }

            return layerResult;
        }

        public new BusinessLayerResult<MyNoteUser> Update(MyNoteUser data)
        {
            MyNoteUser user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email)); //kullanıcı kontrolü
            BusinessLayerResult<MyNoteUser> res = new BusinessLayerResult<MyNoteUser>();
            res.Result = data;

            if (user != null && user.Id != data.Id)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessagesCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı.");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessagesCode.EmailAldreadyExist, "E-mail adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;

            

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessagesCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }

            return res;
        }
    }
}
