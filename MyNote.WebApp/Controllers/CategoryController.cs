using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyNote.BusinessLayer;
using MyNote.Entities;

namespace MyNote.WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryManager CategoryManager = new CategoryManager();

        // GET: Category
        public ActionResult Index()
        {
            return View(CategoryManager.List());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = CategoryManager.Find(x => x.Id == id.Value);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                CategoryManager.Insert(category);

                return RedirectToAction("Index");
            }

            return View(category);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = CategoryManager.Find(x => x.Id == id.Value);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                // todo
                CategoryManager.Update(category);

                return RedirectToAction("Index");
            }
            return View(category);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = CategoryManager.Find(x => x.Id == id.Value);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = CategoryManager.Find(x => x.Id == id);
            CategoryManager.Delete(category);

            return RedirectToAction("Index");
        }

 
    }
}
