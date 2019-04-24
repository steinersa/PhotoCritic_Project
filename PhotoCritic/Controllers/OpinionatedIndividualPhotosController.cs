using Microsoft.AspNet.Identity;
using PhotoCritic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoCritic.Controllers
{
    public class OpinionatedIndividualPhotosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OpinionatedIndividualPhotos
        public ActionResult Index()
        {
            return View();
        }

        // GET: OpinionatedIndividualPhotos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OpinionatedIndividualPhotos/Create
        public ActionResult LikeCreate([Bind(Include = "Id,PhotoId,OpinionatedIndividualId,LikeDislike,Comment,Reason1,Reason2")] int id)
        {
            OpinionatedIndividualPhoto opinionatedIndividualPhoto = new OpinionatedIndividualPhoto();
            opinionatedIndividualPhoto.LikeDislike = true;
            opinionatedIndividualPhoto.PhotoId = id;
            return View(opinionatedIndividualPhoto);
        }

        // POST: OpinionatedIndividualPhotos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LikeCreate([Bind(Include = "Id,PhotoId,OpinionatedIndividualId,LikeDislike,Comment,Reason1,Reason2")] OpinionatedIndividualPhoto opinionatedIndividualPhoto)
        {
            var userResult = User.Identity.GetUserId();
            var currentUser = db.OpinionatedIndividuals.Where(x => userResult == x.ApplicationId).FirstOrDefault();
            opinionatedIndividualPhoto.OpinionatedIndividualId = currentUser.Id;

            if (ModelState.IsValid)
            {
                db.OpinionatedIndividualPhotos.Add(opinionatedIndividualPhoto);
                db.SaveChanges();
                return RedirectToAction("Index", "OpinionatedIndividuals");
            }

            return View(opinionatedIndividualPhoto);
        }

        // GET: OpinionatedIndividualPhotos/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OpinionatedIndividualPhotos/Edit/5
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

        // GET: OpinionatedIndividualPhotos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OpinionatedIndividualPhotos/Delete/5
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
