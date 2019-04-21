using MyNote.BusinessLayer;
using MyNote.Entities;
using MyNote.Entities.Messages;
using MyNote.Entities.ValueObjects;
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

                return RedirectToAction("RegisterOk");
            }

            return View(model);

        }

        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult UserActivate(Guid activate_id)
        {
            //kullanıcı aktivasyonu sağlanacak
            return View();
        }
    }

}