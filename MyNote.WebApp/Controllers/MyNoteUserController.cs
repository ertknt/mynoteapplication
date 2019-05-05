using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyNote.BusinessLayer;
using MyNote.BusinessLayer.Results;
using MyNote.Entities;

namespace MyNote.WebApp.Controllers
{
    public class MyNoteUserController : Controller
    {
        private MyNoteUserManager MyNoteUserManager = new MyNoteUserManager();


        public ActionResult Index()
        {
            return View(MyNoteUserManager.List());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyNoteUser myNoteUser = MyNoteUserManager.Find(x => x.Id == id.Value);

            if (myNoteUser == null)
            {
                return HttpNotFound();
            }
            return View(myNoteUser);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MyNoteUser myNoteUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<MyNoteUser> res = MyNoteUserManager.Insert(myNoteUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(myNoteUser);
                }


                return RedirectToAction("Index");
            }

            return View(myNoteUser);
        }

        // GET: MyNoteUser/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyNoteUser myNoteUser = MyNoteUserManager.Find(x => x.Id == id.Value);

            if (myNoteUser == null)
            {
                return HttpNotFound();
            }

            return View(myNoteUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MyNoteUser myNoteUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<MyNoteUser> res = MyNoteUserManager.Update(myNoteUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(myNoteUser);
                }

                return RedirectToAction("Index");
            }

            return View(myNoteUser);
        }

        // GET: MyNoteUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyNoteUser myNoteUser = MyNoteUserManager.Find(x => x.Id == id.Value);

            if (myNoteUser == null)
            {
                return HttpNotFound();
            }

            return View(myNoteUser);
        }

        // POST: MyNoteUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MyNoteUser myNoteUser = MyNoteUserManager.Find(x => x.Id == id);

            MyNoteUserManager.Delete(myNoteUser);

            return RedirectToAction("Index");
        }
    }
}
