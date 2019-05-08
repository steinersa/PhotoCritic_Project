using Microsoft.AspNet.Identity;
using PhotoCritic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
//using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
//using System.Web.UI.DataVisualization.Charting;

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
                var photosInCategory = db.Photos.Include(x => x.Category).Where(x => x.Category.Name.Contains(categorySelectionForMe) && userResult == x.ApplicationId).OrderByDescending(x => x.WhenCreated).ToList();
                return View(photosInCategory);
            }

            var myPhotos = db.Photos.Include(x => x.Category).Where(x => userResult == x.ApplicationId).OrderByDescending(x => x.WhenCreated).ToList();
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
        public ActionResult Create([Bind(Include = "Id,ApplicationId,ImageName,ImagePath,CategoryId,Hidden,CommentsEnabled,TotalLikes,TotalDislikes,ImageFile,WhenCreated,TotalInteractions,Compare")] Photo photo)
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
        public ActionResult Edit([Bind(Include = "Id,ApplicationId,ImageName,ImagePath,CategoryId,Hidden,CommentsEnabled,TotalLikes,TotalDislikes,ImageFile,WhenCreated,TotalInteractions,Compare")] Photo photo)
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

            Photo photo = db.Photos.Find(id);

            return View(photo);
        }

        public ActionResult GetAgeChartDataLike(int id)
        {
            List<OpinionatedIndividual> ListOfLikers = new List<OpinionatedIndividual>();
            var gettingOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == true).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.Age).Where(x => x.Id == OpId).FirstOrDefault();
                ListOfLikers.Add(gettingPerson);
            }

            var keyCountPair = ListOfLikers.ToList().GroupBy(x => x.Age).Select(x => new { x.Key, Count = x.Count() }); 
            var ageKeys = keyCountPair.Select(x => x.Key.Name).ToArray(); 
            var ageCounts = keyCountPair.Select(x => x.Count).ToArray(); 
            TempData["ageKeyData"] = ageKeys;
            TempData["ageCountData"] = ageCounts;

            return null;
        }

        public ActionResult GetSexChartDataLike(int id)
        {
            List<OpinionatedIndividual> ListOfLikers = new List<OpinionatedIndividual>();
            var gettingOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == true).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.Sex).Where(x => x.Id == OpId).FirstOrDefault();
                ListOfLikers.Add(gettingPerson);
            }

            var keyCountPair = ListOfLikers.ToList().GroupBy(x => x.Sex).Select(x => new { x.Key, Count = x.Count() });
            var sexKeys = keyCountPair.Select(x => x.Key.Name).ToArray();
            var sexCounts = keyCountPair.Select(x => x.Count).ToArray();
            TempData["sexKeyData"] = sexKeys;
            TempData["sexCountData"] = sexCounts;

            return null;
        }

        public ActionResult GetRaceChartDataLike(int id)
        {
            List<OpinionatedIndividual> ListOfLikers = new List<OpinionatedIndividual>();
            var gettingOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == true).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.Race).Where(x => x.Id == OpId).FirstOrDefault();
                ListOfLikers.Add(gettingPerson);
            }

            var keyCountPair = ListOfLikers.ToList().GroupBy(x => x.Race).Select(x => new { x.Key, Count = x.Count() });
            var raceKeys = keyCountPair.Select(x => x.Key.Name).ToArray();
            var raceCounts = keyCountPair.Select(x => x.Count).ToArray();
            TempData["raceKeyData"] = raceKeys;
            TempData["raceCountData"] = raceCounts;

            return null;
        }

        public ActionResult GetLocationChartDataLike(int id)
        {
            List<OpinionatedIndividual> ListOfLikers = new List<OpinionatedIndividual>();
            var gettingOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == true).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.Location).Where(x => x.Id == OpId).FirstOrDefault();
                ListOfLikers.Add(gettingPerson);
            }

            var keyCountPair = ListOfLikers.ToList().GroupBy(x => x.Location).Select(x => new { x.Key, Count = x.Count() });
            var locationKeys = keyCountPair.Select(x => x.Key.Name).ToArray();
            var locationCounts = keyCountPair.Select(x => x.Count).ToArray();
            TempData["locationKeyData"] = locationKeys;
            TempData["locationCountData"] = locationCounts;

            return null;
        }

        public ActionResult GetEducationChartDataLike(int id)
        {
            List<OpinionatedIndividual> ListOfLikers = new List<OpinionatedIndividual>();
            var gettingOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == true).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.Education).Where(x => x.Id == OpId).FirstOrDefault();
                ListOfLikers.Add(gettingPerson);
            }

            var keyCountPair = ListOfLikers.ToList().GroupBy(x => x.Education).Select(x => new { x.Key, Count = x.Count() });
            var educationKeys = keyCountPair.Select(x => x.Key.Name).ToArray();
            var educationCounts = keyCountPair.Select(x => x.Count).ToArray();
            TempData["educationKeyData"] = educationKeys;
            TempData["educationCountData"] =educationCounts;

            return null;
        }

        public ActionResult GetProfessionChartDataLike(int id)
        {
            List<OpinionatedIndividual> ListOfLikers = new List<OpinionatedIndividual>();
            var gettingOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == true).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.Profession).Where(x => x.Id == OpId).FirstOrDefault();
                ListOfLikers.Add(gettingPerson);
            }

            var keyCountPair = ListOfLikers.ToList().GroupBy(x => x.Profession).Select(x => new { x.Key, Count = x.Count() });
            var professionKeys = keyCountPair.Select(x => x.Key.Name).ToArray();
            var professionCounts = keyCountPair.Select(x => x.Count).ToArray();
            TempData["professionKeyData"] = professionKeys;
            TempData["professionCountData"] = professionCounts;

            return null;
        }

        public ActionResult GetMaritalStatusChartDataLike(int id)
        {
            List<OpinionatedIndividual> ListOfLikers = new List<OpinionatedIndividual>();
            var gettingOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == true).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.MaritalStatus).Where(x => x.Id == OpId).FirstOrDefault();
                ListOfLikers.Add(gettingPerson);
            }

            var keyCountPair = ListOfLikers.ToList().GroupBy(x => x.MaritalStatus).Select(x => new { x.Key, Count = x.Count() });
            var maritalStatusKeys = keyCountPair.Select(x => x.Key.Name).ToArray();
            var maritalStatusCounts = keyCountPair.Select(x => x.Count).ToArray();
            TempData["maritalStatusKeyData"] = maritalStatusKeys;
            TempData["maritalStatusCountData"] = maritalStatusCounts;

            return null;
        }

        public ActionResult GetIncomeLevelChartDataLike(int id)
        {
            List<OpinionatedIndividual> ListOfLikers = new List<OpinionatedIndividual>();
            var gettingOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == true).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.IncomeLevel).Where(x => x.Id == OpId).FirstOrDefault();
                ListOfLikers.Add(gettingPerson);
            }

            var keyCountPair = ListOfLikers.ToList().GroupBy(x => x.IncomeLevel).Select(x => new { x.Key, Count = x.Count() });
            var incomeLevelKeys = keyCountPair.Select(x => x.Key.Name).ToArray();
            var incomeLevelCounts = keyCountPair.Select(x => x.Count).ToArray();
            TempData["incomeLevelKeyData"] = incomeLevelKeys;
            TempData["incomeLevelCountData"] = incomeLevelCounts;

            return null;
        }

        public ActionResult LikesCommentsReasons(int id)
        {
            ViewBag.allComments = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == true && x.Comment != null).Select(x => x.Comment).ToList();
            var reasonCountPair = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == true && x.Reason1 != null).ToList().GroupBy(a => a.Reason1).Select(a => new { a.Key, Count = a.Count() }).OrderByDescending(x => x.Count);
            var reasons = reasonCountPair.Select(x => x.Key).ToArray();
            var counts = reasonCountPair.Select(x => x.Count).ToArray();
            ViewBag.reasonWithCount = reasons.Zip(counts, (r, c) => r + " " + "(" + c.ToString() + ")");

            return View();
        }

        public ActionResult Dislikes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Photo photo = db.Photos.Find(id);

            return View(photo);
        }

        public ActionResult GetAgeChartDataDislike(int id)
        {
            List<OpinionatedIndividual> ListOfDislikers = new List<OpinionatedIndividual>();
            var gettingOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == false).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.Age).Where(x => x.Id == OpId).FirstOrDefault();
                ListOfDislikers.Add(gettingPerson);
            }

            var keyCountPair = ListOfDislikers.ToList().GroupBy(x => x.Age).Select(x => new { x.Key, Count = x.Count() });
            var ageKeys = keyCountPair.Select(x => x.Key.Name).ToArray();
            var ageCounts = keyCountPair.Select(x => x.Count).ToArray();
            TempData["dislikeAgeKeyData"] = ageKeys;
            TempData["dislikeAgeCountData"] = ageCounts;

            return null;
        }

        public ActionResult GetSexChartDataDislike(int id)
        {
            List<OpinionatedIndividual> ListOfDislikers = new List<OpinionatedIndividual>();
            var gettingOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == false).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.Sex).Where(x => x.Id == OpId).FirstOrDefault();
                ListOfDislikers.Add(gettingPerson);
            }

            var keyCountPair = ListOfDislikers.ToList().GroupBy(x => x.Sex).Select(x => new { x.Key, Count = x.Count() });
            var sexKeys = keyCountPair.Select(x => x.Key.Name).ToArray();
            var sexCounts = keyCountPair.Select(x => x.Count).ToArray();
            TempData["dislikeSexKeyData"] = sexKeys;
            TempData["dislikeSexCountData"] = sexCounts;

            return null;
        }

        public ActionResult GetRaceChartDataDislike(int id)
        {
            List<OpinionatedIndividual> ListOfDislikers = new List<OpinionatedIndividual>();
            var gettingOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == false).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.Race).Where(x => x.Id == OpId).FirstOrDefault();
                ListOfDislikers.Add(gettingPerson);
            }

            var keyCountPair = ListOfDislikers.ToList().GroupBy(x => x.Race).Select(x => new { x.Key, Count = x.Count() });
            var raceKeys = keyCountPair.Select(x => x.Key.Name).ToArray();
            var raceCounts = keyCountPair.Select(x => x.Count).ToArray();
            TempData["dislikeRaceKeyData"] = raceKeys;
            TempData["dislikeRaceCountData"] = raceCounts;

            return null;
        }

        public ActionResult GetLocationChartDataDislike(int id)
        {
            List<OpinionatedIndividual> ListOfDislikers = new List<OpinionatedIndividual>();
            var gettingOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == false).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.Location).Where(x => x.Id == OpId).FirstOrDefault();
                ListOfDislikers.Add(gettingPerson);
            }

            var keyCountPair = ListOfDislikers.ToList().GroupBy(x => x.Location).Select(x => new { x.Key, Count = x.Count() });
            var locationKeys = keyCountPair.Select(x => x.Key.Name).ToArray();
            var locationCounts = keyCountPair.Select(x => x.Count).ToArray();
            TempData["dislikeLocationKeyData"] = locationKeys;
            TempData["dislikeLocationCountData"] = locationCounts;

            return null;
        }

        public ActionResult GetEducationChartDataDislike(int id)
        {
            List<OpinionatedIndividual> ListOfDislikers = new List<OpinionatedIndividual>();
            var gettingOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == false).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.Education).Where(x => x.Id == OpId).FirstOrDefault();
                ListOfDislikers.Add(gettingPerson);
            }

            var keyCountPair = ListOfDislikers.ToList().GroupBy(x => x.Education).Select(x => new { x.Key, Count = x.Count() });
            var educationKeys = keyCountPair.Select(x => x.Key.Name).ToArray();
            var educationCounts = keyCountPair.Select(x => x.Count).ToArray();
            TempData["dislikeEducationKeyData"] = educationKeys;
            TempData["dislikeEducationCountData"] = educationCounts;

            return null;
        }

        public ActionResult GetProfessionChartDataDislike(int id)
        {
            List<OpinionatedIndividual> ListOfDislikers = new List<OpinionatedIndividual>();
            var gettingOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == false).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.Profession).Where(x => x.Id == OpId).FirstOrDefault();
                ListOfDislikers.Add(gettingPerson);
            }

            var keyCountPair = ListOfDislikers.ToList().GroupBy(x => x.Profession).Select(x => new { x.Key, Count = x.Count() });
            var professionKeys = keyCountPair.Select(x => x.Key.Name).ToArray();
            var professionCounts = keyCountPair.Select(x => x.Count).ToArray();
            TempData["dislikeProfessionKeyData"] = professionKeys;
            TempData["dislikeProfessionCountData"] = professionCounts;

            return null;
        }

        public ActionResult GetMaritalStatusChartDataDislike(int id)
        {
            List<OpinionatedIndividual> ListOfDislikers = new List<OpinionatedIndividual>();
            var gettingOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == false).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.MaritalStatus).Where(x => x.Id == OpId).FirstOrDefault();
                ListOfDislikers.Add(gettingPerson);
            }

            var keyCountPair = ListOfDislikers.ToList().GroupBy(x => x.MaritalStatus).Select(x => new { x.Key, Count = x.Count() });
            var maritalStatusKeys = keyCountPair.Select(x => x.Key.Name).ToArray();
            var maritalStatusCounts = keyCountPair.Select(x => x.Count).ToArray();
            TempData["dislikeMaritalStatusKeyData"] = maritalStatusKeys;
            TempData["dislikeMaritalStatusCountData"] = maritalStatusCounts;

            return null;
        }

        public ActionResult GetIncomeLevelChartDataDislike(int id)
        {
            List<OpinionatedIndividual> ListOfDislikers = new List<OpinionatedIndividual>();
            var gettingOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == false).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.IncomeLevel).Where(x => x.Id == OpId).FirstOrDefault();
                ListOfDislikers.Add(gettingPerson);
            }

            var keyCountPair = ListOfDislikers.ToList().GroupBy(x => x.IncomeLevel).Select(x => new { x.Key, Count = x.Count() });
            var incomeLevelKeys = keyCountPair.Select(x => x.Key.Name).ToArray();
            var incomeLevelCounts = keyCountPair.Select(x => x.Count).ToArray();
            TempData["dislikeIncomeLevelKeyData"] = incomeLevelKeys;
            TempData["dislikeIncomeLevelCountData"] = incomeLevelCounts;

            return null;
        }

        public ActionResult DislikesCommentsReasons(int id)
        {
            ViewBag.allComments = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == false && x.Comment != null).Select(x => x.Comment).ToList();
            var reasonCountPair = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == false && x.Reason1 != null).ToList().GroupBy(a => a.Reason1).Select(a => new { a.Key, Count = a.Count() }).OrderByDescending(x => x.Count);
            var reasons = reasonCountPair.Select(x => x.Key).ToArray();
            var counts = reasonCountPair.Select(x => x.Count).ToArray();
            ViewBag.reasonWithCount = reasons.Zip(counts, (r, c) => r + " " + "(" + c.ToString() + ")");

            return View();
        }

        public ActionResult Filtered(int id, string ageSelection, string sexSelection, string raceSelection, string locationSelection, string educationSelection, string professionSelection, string maritalStatusSelection, string incomeLevelSelection)
        {
            var ages = db.Ages.Select(x => x.Name);
            ViewBag.ageSelection = new SelectList(ages);
            var sexes = db.Sexes.Select(x => x.Name);
            ViewBag.sexSelection = new SelectList(sexes);
            var races = db.Races.Select(x => x.Name);
            ViewBag.raceSelection = new SelectList(races);
            var locations = db.Locations.Select(x => x.Name);
            ViewBag.locationSelection = new SelectList(locations);
            var educations = db.Educations.Select(x => x.Name);
            ViewBag.educationSelection = new SelectList(educations);
            var professions = db.Professions.Select(x => x.Name);
            ViewBag.professionSelection = new SelectList(professions);
            var maritalStatuses = db.MaritalStatuses.Select(x => x.Name);
            ViewBag.maritalStatusSelection = new SelectList(maritalStatuses);
            var incomeLevels = db.IncomeLevels.Select(x => x.Name);
            ViewBag.incomeLevelSelection = new SelectList(incomeLevels);

            List<OpinionatedIndividual> photoLikeOpinionatedIndividuals = new List<OpinionatedIndividual>();
            var gettingLikeOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == true).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingLikeOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.Age).Include(x => x.Sex).Include(x => x.Race).Include(x => x.Location).Include(x => x.Education).Include(x => x.Profession).Include(x => x.MaritalStatus).Include(x => x.IncomeLevel).Where(x => x.Id == OpId).FirstOrDefault();
                photoLikeOpinionatedIndividuals.Add(gettingPerson);
            }

            List<OpinionatedIndividual> photoDislikeOpinionatedIndividuals = new List<OpinionatedIndividual>();
            var gettingDislikeOpinionatedIndividualId = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == false).Select(x => x.OpinionatedIndividualId).ToList();
            foreach (var OpId in gettingDislikeOpinionatedIndividualId)
            {
                var gettingPerson = db.OpinionatedIndividuals.Include(x => x.Age).Include(x => x.Sex).Include(x => x.Race).Include(x => x.Location).Include(x => x.Education).Include(x => x.Profession).Include(x => x.MaritalStatus).Include(x => x.IncomeLevel).Where(x => x.Id == OpId).FirstOrDefault();
                photoDislikeOpinionatedIndividuals.Add(gettingPerson);
            }

            if (!string.IsNullOrEmpty(ageSelection))
            {
                photoLikeOpinionatedIndividuals = photoLikeOpinionatedIndividuals.Where(x => x.Age.Name.Contains(ageSelection)).ToList();
                photoDislikeOpinionatedIndividuals = photoDislikeOpinionatedIndividuals.Where(x => x.Age.Name.Contains(ageSelection)).ToList();
            }

            if (!string.IsNullOrEmpty(sexSelection))
            {
                photoLikeOpinionatedIndividuals = photoLikeOpinionatedIndividuals.Where(x => x.Sex.Name.Contains(sexSelection)).ToList();
                photoDislikeOpinionatedIndividuals = photoDislikeOpinionatedIndividuals.Where(x => x.Sex.Name.Contains(sexSelection)).ToList();
            }

            if (!string.IsNullOrEmpty(raceSelection))
            {
                photoLikeOpinionatedIndividuals = photoLikeOpinionatedIndividuals.Where(x => x.Race.Name.Contains(raceSelection)).ToList();
                photoDislikeOpinionatedIndividuals = photoDislikeOpinionatedIndividuals.Where(x => x.Race.Name.Contains(raceSelection)).ToList();
            }

            if (!string.IsNullOrEmpty(locationSelection))
            {
                photoLikeOpinionatedIndividuals = photoLikeOpinionatedIndividuals.Where(x => x.Location.Name.Contains(locationSelection)).ToList();
                photoDislikeOpinionatedIndividuals = photoDislikeOpinionatedIndividuals.Where(x => x.Location.Name.Contains(locationSelection)).ToList();
            }

            if (!string.IsNullOrEmpty(educationSelection))
            {
                photoLikeOpinionatedIndividuals = photoLikeOpinionatedIndividuals.Where(x => x.Education.Name.Contains(educationSelection)).ToList();
                photoDislikeOpinionatedIndividuals = photoDislikeOpinionatedIndividuals.Where(x => x.Education.Name.Contains(educationSelection)).ToList();
            }

            if (!string.IsNullOrEmpty(professionSelection))
            {
                photoLikeOpinionatedIndividuals = photoLikeOpinionatedIndividuals.Where(x => x.Profession.Name.Contains(professionSelection)).ToList();
                photoDislikeOpinionatedIndividuals = photoDislikeOpinionatedIndividuals.Where(x => x.Profession.Name.Contains(professionSelection)).ToList();
            }

            if (!string.IsNullOrEmpty(maritalStatusSelection))
            {
                photoLikeOpinionatedIndividuals = photoLikeOpinionatedIndividuals.Where(x => x.MaritalStatus.Name.Contains(maritalStatusSelection)).ToList();
                photoDislikeOpinionatedIndividuals = photoDislikeOpinionatedIndividuals.Where(x => x.MaritalStatus.Name.Contains(maritalStatusSelection)).ToList();
            }

            if (!string.IsNullOrEmpty(incomeLevelSelection))
            {
                photoLikeOpinionatedIndividuals = photoLikeOpinionatedIndividuals.Where(x => x.IncomeLevel.Name.Contains(incomeLevelSelection)).ToList();
                photoDislikeOpinionatedIndividuals = photoDislikeOpinionatedIndividuals.Where(x => x.IncomeLevel.Name.Contains(incomeLevelSelection)).ToList();
            }

            var countOfFilteredLikers = photoLikeOpinionatedIndividuals.Count();
            var countOfFilteredDislikers = photoDislikeOpinionatedIndividuals.Count();
            var yValues = new int[2] { countOfFilteredLikers, countOfFilteredDislikers };
            var xValues = new string[2] { "Like", "Dislike" };

            TempData["chartYValues"] = yValues;
            TempData["chartXValues"] = xValues;

            return View();
        }

        public ActionResult Compare(string compareChoice)
        {
            try
            {
                var userResult = User.Identity.GetUserId();
                var photoIdsToCompare = db.Photos.Where(x => userResult == x.ApplicationId && x.Compare == true).Select(x => x.Id).ToArray();

                var choices = new List<string> { "Age", "Sex", "Race", "Location", "Education", "Profession", "Marital Status", "Income Level" };
                ViewBag.compareChoice = new SelectList(choices);
                ViewBag.choice = compareChoice;

                var photoOneId = photoIdsToCompare[0];
                ViewBag.photoOneId = photoOneId;
                var photoTwoId = photoIdsToCompare[1];
                ViewBag.PhotoTwoId = photoTwoId;

                ViewBag.photoOneImage = db.Photos.Where(x => x.Id == photoOneId).Select(x => x.ImagePath).FirstOrDefault();
                ViewBag.PhotoTwoImage = db.Photos.Where(x => x.Id == photoTwoId).Select(x => x.ImagePath).FirstOrDefault();

                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Photos");
            }
        }
    }
}
