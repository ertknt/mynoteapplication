using MyNote.BusinessLayer;
using MyNote.Entities;
using MyNote.Entities.Messages;
using MyNote.Entities.ValueObjects;
using MyNote.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyNote.WebApp.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            if (TempData["mm"] != null)
            {
                return View(TempData["mm"] as List<Note>);
            }

            NoteManager nm = new NoteManager();




            return View(nm.GetAllNote().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryManager cm = new CategoryManager();
            Category cat = cm.GetCategoryById(id.Value);

            if (cat == null)
            {
                return HttpNotFound();
            }



            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();

            return View("Index", nm.GetAllNote().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult ShowProfile()
        {
            MyNoteUser currentUser = Session["login"] as MyNoteUser;

            MyNoteUserManager mum = new MyNoteUserManager();
            BusinessLayerResult<MyNoteUser> res = mum.GetUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu.",
                    Items = res.Errors
                };


                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }

        public ActionResult EditProfile()
        {
            MyNoteUser currentUser = Session["login"] as MyNoteUser;

            MyNoteUserManager mum = new MyNoteUserManager();
            BusinessLayerResult<MyNoteUser> res = mum.GetUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }

        [HttpPost]
        public ActionResult EditProfile(MyNoteUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpg" ||
                    ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFilename = filename;
                }

                MyNoteUserManager mum = new MyNoteUserManager();
                BusinessLayerResult<MyNoteUser> res = mum.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi.",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorNotifyObj);
                }

                Session["login"] = res.Result; //profil güncellendiği için session güncellendi.

                return RedirectToAction("ShowProfile");
            }

            return View(model);
        }

        public ActionResult DeleteProfile()
        {
            MyNoteUser currentUser = Session["login"] as MyNoteUser;

            MyNoteUserManager mum = new MyNoteUserManager();
            BusinessLayerResult<MyNoteUser> res = mum.RemoveUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil silinemedi.",
                    RedirectingUrl = "/Home/ShowProfile"
                };

                return View("Error", errorNotifyObj);
            }

            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult TestNotify()
        {
            OkViewModel model = new OkViewModel()
            {
                Header = "Yönlendirme",
                Title = "Ok Test",
                RedirectingTimeout = 2000,
                Items = new List<string>() { "Test başarılı 1", "Test başarılı 2" }
            };

            return View("Ok", model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid) //model valid ise
            {
                MyNoteUserManager num = new MyNoteUserManager();

                var res = num.LoginUser(model); //login olmayı dene

                if (res.Errors.Count > 0) //hata varsa
                {
                    if (res.Errors.Find(x => x.Code == ErrorMessagesCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/123456";
                    }


                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message)); //mesajları yaz


                    return View(model);
                }

                Session["login"] = res.Result; //kullanıcıyı sessiona atma

                return RedirectToAction("Index"); //yönlendirme   
            }

            return View(model); //modeli geri gönder
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                MyNoteUserManager mum = new MyNoteUserManager();

                var res = mum.RegisterUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(model);
                }

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login",
                };
                notifyObj.Items.Add("Kayıt oldunuz.");

                return View("Ok", notifyObj);
            }

            return View(model);

        }





        public ActionResult UserActivate(Guid id)
        {
            //kullanıcı aktivasyonu sağlanacak

            MyNoteUserManager mum = new MyNoteUserManager();
            BusinessLayerResult<MyNoteUser> res = mum.ActivateUser(id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz işlem",
                    Items = res.Errors
                };


                return View("Error", errorNotifyObj);
            }

            OkViewModel okNotifyObj = new OkViewModel()
            {
                Title = "Hesap aktifleştirildi.",
                RedirectingUrl = "/Home/Login",
            };

            okNotifyObj.Items.Add("Hesabınız aktifleştirildi.");

            return View("ok", okNotifyObj);
        }


    }

}