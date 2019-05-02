using Microsoft.AspNet.Identity;
using PhotoCritic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;

namespace PhotoCritic.Controllers
{
    public class OpinionatedIndividualsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OpinionatedIndividuals
        public ActionResult Index(string categorySelection)
        {
            var userResult = User.Identity.GetUserId();
            var categories = db.Categories.Select(x => x.Name);
            ViewBag.categorySelection = new SelectList(categories);

            if (!string.IsNullOrEmpty(categorySelection))
            {
                var photosInCategory = db.Photos.Where(x => x.Category.Name.Contains(categorySelection) && x.Hidden == false && userResult != x.ApplicationId).OrderByDescending(x => x.WhenCreated).ToList();
                return View(photosInCategory);
            }

            var recentImages = db.Photos.Where(x => userResult != x.ApplicationId && x.Hidden == false).OrderByDescending(x => x.WhenCreated).ToList();
            return View(recentImages);
        }

        // GET: OpinionatedIndividuals/Details/5
        public ActionResult Dashboard()
        {
            var userResult = User.Identity.GetUserId();
            var currentUser = db.OpinionatedIndividuals.Include(x => x.Age).Include(x => x.Sex).Include(x => x.Race).Include(x => x.Location).Include(x => x.Education).Include(x => x.Profession).Include(x => x.IncomeLevel).Include(x => x.MaritalStatus).Where(x => userResult == x.ApplicationId).FirstOrDefault();

            var myInteractionCount = db.OpinionatedIndividualPhotos.Where(x => x.OpinionatedIndividualId == currentUser.Id).ToList().Count();
            ViewBag.interactionCount = myInteractionCount;
            var myLikeCount = db.OpinionatedIndividualPhotos.Where(x => x.OpinionatedIndividualId == currentUser.Id && x.LikeDislike == true).ToList().Count();
            var myDislikeCount = db.OpinionatedIndividualPhotos.Where(x => x.OpinionatedIndividualId == currentUser.Id && x.LikeDislike == false).ToList().Count();

            try
            {
                double likeDivision = ((double)myLikeCount / (double)myInteractionCount);
                if (double.IsNaN(likeDivision) == true)
                {
                    throw new DivideByZeroException();
                }
                else
                {
                    double likeResult = likeDivision * 100;
                    var likePercent = Convert.ToInt32(likeResult);
                    ViewBag.percentLike = likePercent;
                }
            }
            catch (DivideByZeroException)
            {
                ViewBag.percentLike = 0;
            }

            try
            {

                var dislikeDivision = ((double)myDislikeCount / (double)myInteractionCount);
                if (double.IsNaN(dislikeDivision) == true)
                {
                    throw new DivideByZeroException();
                }
                else
                {
                    double dislikeResult = dislikeDivision * 100;
                    var dislikePercent = Convert.ToInt32(dislikeResult);
                    ViewBag.percentDislike = dislikePercent;
                }
            }
            catch (DivideByZeroException)
            {
                ViewBag.percentDislike = 0;
            }
            
            try
            {
                var myMostInteractedPhoto = db.Photos.Where(x => userResult == x.ApplicationId).OrderByDescending(x => x.TotalInteractions).FirstOrDefault();
                ViewBag.myMostInteractedImagePath = myMostInteractedPhoto.ImagePath;
            }
            catch (Exception e)
            {
                ViewBag.myMostInteractedImagePath = "None";
            }

            try
            {
                var myMostLikedPhoto = db.Photos.Where(x => userResult == x.ApplicationId).OrderByDescending(x => x.TotalLikes).FirstOrDefault();
                ViewBag.myMostLikedImagePath = myMostLikedPhoto.ImagePath;
            }
            catch (Exception e)
            {
                ViewBag.myMostLikedImagePath = "None";
            }

            try
            {
                var myMostDislikedPhoto = db.Photos.Where(x => userResult == x.ApplicationId).OrderByDescending(x => x.TotalDislikes).FirstOrDefault();
                ViewBag.myMostDislikedImagePath = myMostDislikedPhoto.ImagePath;
            }
            catch (Exception e)
            {
                ViewBag.myMostDislikedImagePath = "None";
            }

            return View(currentUser);
        }

        // GET: OpinionatedIndividuals/Create
        public ActionResult Create()
        {
            var ages = db.Ages.ToList();
            var sexes = db.Sexes.ToList();
            var races = db.Races.ToList();
            var locations = db.Locations.ToList();
            var educations = db.Educations.ToList();
            var professions = db.Professions.ToList();
            var maritalStatuses = db.MaritalStatuses.ToList();
            var incomeLevels = db.IncomeLevels.ToList();

            OpinionatedIndividual opinionatedIndividual = new OpinionatedIndividual()
            {
                Ages = ages,
                Sexes = sexes,
                Races = races,
                Locations = locations,
                Educations = educations,
                Professions = professions,
                MaritalStatuses = maritalStatuses,
                IncomeLevels = incomeLevels
            };
            return View(opinionatedIndividual);
        }

        // POST: OpinionatedIndividuals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AgeId,SexId,RaceId,LocationId,EducationId,ProfessionId,IncomeLevelId,MaritalStatusId,ApplicationId")] OpinionatedIndividual opinionatedIndividual)
        {
            opinionatedIndividual.ApplicationId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.OpinionatedIndividuals.Add(opinionatedIndividual);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(opinionatedIndividual);
        }

        // GET: Photos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ages = db.Ages.ToList();
            var sexes = db.Sexes.ToList();
            var races = db.Races.ToList();
            var locations = db.Locations.ToList();
            var educations = db.Educations.ToList();
            var professions = db.Professions.ToList();
            var maritalStatuses = db.MaritalStatuses.ToList();
            var incomeLevels = db.IncomeLevels.ToList();

            OpinionatedIndividual opinionatedIndividual = db.OpinionatedIndividuals.Find(id);
            {
                opinionatedIndividual.Ages = ages;
                opinionatedIndividual.Sexes = sexes;
                opinionatedIndividual.Races = races;
                opinionatedIndividual.Locations = locations;
                opinionatedIndividual.Educations = educations;
                opinionatedIndividual.Professions = professions;
                opinionatedIndividual.MaritalStatuses = maritalStatuses;
                opinionatedIndividual.IncomeLevels = incomeLevels;
            };

            if (opinionatedIndividual == null)
            {
                return HttpNotFound();
            }
            return View(opinionatedIndividual);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ApplicationId,AgeId,SexId,RaceId,LocationId,EducationId,ProfessionId,IncomeLevelId,MaritalStatusId")] OpinionatedIndividual opinionatedIndividual)
        {
            if (ModelState.IsValid)
            {
                db.Entry(opinionatedIndividual).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View(opinionatedIndividual);
        }

        public ActionResult CheckIfVoted(int id)
        {
            var userResult = User.Identity.GetUserId();
            var currentOpinionatedIndividualId = db.OpinionatedIndividuals.Where(x => userResult == x.ApplicationId).Select(x => x.Id).FirstOrDefault();
            var voteForPhoto = db.OpinionatedIndividualPhotos.Where(x => currentOpinionatedIndividualId == x.OpinionatedIndividualId && id == x.PhotoId).FirstOrDefault();
            TempData["vote"] = voteForPhoto;

            return null;
        }

    }
}
