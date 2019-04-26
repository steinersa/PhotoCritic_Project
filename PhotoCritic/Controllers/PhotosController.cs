using Microsoft.AspNet.Identity;
using PhotoCritic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PhotoCritic.Controllers
{
    public class PhotosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Photos
        public ActionResult Index(string categorySelectionForMe)
        {
            var userResult = User.Identity.GetUserId();
            var categories = db.Categories.Select(x => x.Name);
            ViewBag.categorySelectionForMe = new SelectList(categories);

            if (!string.IsNullOrEmpty(categorySelectionForMe))
            {
                var photosInCategory = db.Photos.Where(x => x.Category.Name.Contains(categorySelectionForMe) && userResult == x.ApplicationId).OrderByDescending(x => x.WhenCreated).ToList();
                return View(photosInCategory);
            }

            var myPhotos = db.Photos.Where(x => userResult == x.ApplicationId).OrderByDescending(x => x.WhenCreated).ToList();
            return View(myPhotos);
        }

        // GET: Photos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var categories = db.Categories.ToList();
            Photo photo = db.Photos.Find(id);
            {
                photo.Categories = categories;
            };
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
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
        public ActionResult Create([Bind(Include = "Id,ApplicationId,ImageName,ImagePath,CategoryId,Hidden,CommentsEnabled,TotalLikes,TotalDislikes,ImageFile,WhenCreated,TotalInteractions")] Photo photo)
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
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var categories = db.Categories.ToList();
            Photo photo = db.Photos.Find(id);
            {
                photo.Categories = categories;
            };
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ApplicationId,ImageName,ImagePath,CategoryId,Hidden,CommentsEnabled,TotalLikes,TotalDislikes,ImageFile,WhenCreated,TotalInteractions")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(photo);
        }

        // GET: Photos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Photo photo = db.Photos.Find(id);
            db.Photos.Remove(photo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Likes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.allComments = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == true && x.Comment != null).Select(x => x.Comment).ToList();
            ViewBag.reasons = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == true && x.Reason1 != null).ToList().GroupBy(a => a.Reason1).Select(a => new { a.Key, Count = a.Count() }).OrderByDescending(x => x.Count);

            //List<OpinionatedIndividual> ListOfPeople = new List<OpinionatedIndividual>();
            //var gettingOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == true).Select(x => x.OpinionatedIndividualId).ToList();
            //foreach (var OpId in gettingOpinionatedIndividualId)
            //{
            //    var gettingPerson = db.OpinionatedIndividuals.Where(x => x.Id == OpId).FirstOrDefault();
            //    ListOfPeople.Add(gettingPerson);
            //}

            return View(/*ListOfPeople*/);
        }
    }
}
