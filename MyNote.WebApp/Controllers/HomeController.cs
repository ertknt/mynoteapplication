using MyNote.BusinessLayer;
using MyNote.BusinessLayer.Results;
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
        private NoteManager noteManager = new NoteManager();
        private CategoryManager CategoryManager = new CategoryManager();
        private MyNoteUserManager myNoteUserManager = new MyNoteUserManager();



        public ActionResult Index()
        {
            if (TempData["mm"] != null)
            {
                return View(TempData["mm"] as List<Note>);
            }





            return View(noteManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category cat = CategoryManager.Find(x => x.Id == id.Value);

            if (cat == null)
            {
                return HttpNotFound();
            }



            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            return View("Index", noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult ShowProfile()
        {
            MyNoteUser currentUser = Session["login"] as MyNoteUser;

            BusinessLayerResult<MyNoteUser> res = myNoteUserManager.GetUserById(currentUser.Id);

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

            BusinessLayerResult<MyNoteUser> res = myNoteUserManager.GetUserById(currentUser.Id);

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

                BusinessLayerResult<MyNoteUser> res = myNoteUserManager.UpdateProfile(model);

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

            BusinessLayerResult<MyNoteUser> res = myNoteUserManager.RemoveUserById(currentUser.Id);

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

                var res = myNoteUserManager.LoginUser(model); //login olmayı dene

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

                var res = myNoteUserManager.RegisterUser(model);

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

            BusinessLayerResult<MyNoteUser> res = myNoteUserManager.ActivateUser(id);

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