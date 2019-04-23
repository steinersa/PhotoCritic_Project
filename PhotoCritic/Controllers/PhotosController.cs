using Microsoft.AspNet.Identity;
using PhotoCritic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoCritic.Controllers
{
    public class PhotosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Photos
        public ActionResult Index()
        {
            return View();
        }

        // GET: Photos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Photos/Create
        public ActionResult Create()
        {
            var categories = db.Categories.ToList();

            Photo photo = new Photo()
            {
                Categories = categories
            };
            return View(photo);
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ApplicationId,ImageName,ImagePath,CategoryId,Hidden,CommentsEnabled,TotalLikes,TotalDislikes,ImageFile")] Photo photo)
        {
            photo.ApplicationId = User.Identity.GetUserId();

            string fileName = Path.GetFileNameWithoutExtension(photo.ImageFile.FileName);
            string extension = Path.GetExtension(photo.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            photo.ImagePath = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            photo.ImageFile.SaveAs(fileName);

            db.Photos.Add(photo);
            db.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("Index");
        }

        // GET: Photos/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Photos/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Photos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Photos/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
