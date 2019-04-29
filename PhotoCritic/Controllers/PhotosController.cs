using Microsoft.AspNet.Identity;
using PhotoCritic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
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

            var keyCountPair = ListOfLikers.ToList().GroupBy(x => x.Age).Select(x => new { x.Key, Count = x.Count() }); //key value pairs
            var ageKeys = keyCountPair.Select(x => x.Key.Name).ToArray(); //just the keys (distinct ages)
            var ageCounts = keyCountPair.Select(x => x.Count).ToArray(); //just the values (how many fall in each age group)

            Chart Ages = new Chart(width: 300, height: 300)
                .AddTitle("Age")
                .AddLegend()
                .AddSeries(
                    chartType: "pie",
                    xValue: ageKeys,
                    yValues: ageCounts)
                .Write("png");

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

            Chart Ages = new Chart(width: 300, height: 300)
                .AddTitle("Sex")
                .AddLegend()
                .AddSeries(
                    chartType: "pie",
                    xValue: sexKeys,
                    yValues: sexCounts)
                .Write("png");

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

            Chart Ages = new Chart(width: 300, height: 300)
                .AddTitle("Race")
                .AddLegend()
                .AddSeries(
                    chartType: "pie",
                    xValue: raceKeys,
                    yValues: raceCounts)
                .Write("png");

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

            Chart Ages = new Chart(width: 300, height: 300)
                .AddTitle("Location")
                .AddLegend()
                .AddSeries(
                    chartType: "pie",
                    xValue: locationKeys,
                    yValues: locationCounts)
                .Write("png");

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

            Chart Ages = new Chart(width: 300, height: 300)
                .AddTitle("Education")
                .AddLegend()
                .AddSeries(
                    chartType: "pie",
                    xValue: educationKeys,
                    yValues: educationCounts)
                .Write("png");

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

            Chart Ages = new Chart(width: 300, height: 300)
                .AddTitle("Locations")
                .AddLegend()
                .AddSeries(
                    chartType: "pie",
                    xValue: professionKeys,
                    yValues: professionCounts)
                .Write("png");

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

            Chart Ages = new Chart(width: 300, height: 300)
                .AddTitle("Locations")
                .AddLegend()
                .AddSeries(
                    chartType: "pie",
                    xValue: maritalStatusKeys,
                    yValues: maritalStatusCounts)
                .Write("png");

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

            Chart Ages = new Chart(width: 300, height: 300)
                .AddTitle("Locations")
                .AddLegend()
                .AddSeries(
                    chartType: "pie",
                    xValue: incomeLevelKeys,
                    yValues: incomeLevelCounts)
                .Write("png");

            return null;
        }

        public ActionResult LikesCommentsReasons(int id)
        {
            ViewBag.allComments = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == true && x.Comment != null).Select(x => x.Comment).ToList();
            ViewBag.reasons = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == true && x.Reason1 != null).ToList().GroupBy(a => a.Reason1).Select(a => new { a.Key, Count = a.Count() }).OrderByDescending(x => x.Count);

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

            Chart Ages = new Chart(width: 300, height: 300)
                .AddTitle("Age")
                .AddLegend()
                .AddSeries(
                    chartType: "pie",
                    xValue: ageKeys,
                    yValues: ageCounts)
                .Write("png");

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

            Chart Ages = new Chart(width: 300, height: 300)
                .AddTitle("Sex")
                .AddLegend()
                .AddSeries(
                    chartType: "pie",
                    xValue: sexKeys,
                    yValues: sexCounts)
                .Write("png");

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

            Chart Ages = new Chart(width: 300, height: 300)
                .AddTitle("Race")
                .AddLegend()
                .AddSeries(
                    chartType: "pie",
                    xValue: raceKeys,
                    yValues: raceCounts)
                .Write("png");

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

            Chart Ages = new Chart(width: 300, height: 300)
                .AddTitle("Location")
                .AddLegend()
                .AddSeries(
                    chartType: "pie",
                    xValue: locationKeys,
                    yValues: locationCounts)
                .Write("png");

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

            Chart Ages = new Chart(width: 300, height: 300)
                .AddTitle("Education")
                .AddLegend()
                .AddSeries(
                    chartType: "pie",
                    xValue: educationKeys,
                    yValues: educationCounts)
                .Write("png");

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

            Chart Ages = new Chart(width: 300, height: 300)
                .AddTitle("Locations")
                .AddLegend()
                .AddSeries(
                    chartType: "pie",
                    xValue: professionKeys,
                    yValues: professionCounts)
                .Write("png");

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

            Chart Ages = new Chart(width: 300, height: 300)
                .AddTitle("Locations")
                .AddLegend()
                .AddSeries(
                    chartType: "pie",
                    xValue: maritalStatusKeys,
                    yValues: maritalStatusCounts)
                .Write("png");

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

            Chart Ages = new Chart(width: 300, height: 300)
                .AddTitle("Locations")
                .AddLegend()
                .AddSeries(
                    chartType: "pie",
                    xValue: incomeLevelKeys,
                    yValues: incomeLevelCounts)
                .Write("png");

            return null;
        }

        public ActionResult DislikesCommentsReasons(int id)
        {
            ViewBag.allComments = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == false && x.Comment != null).Select(x => x.Comment).ToList();
            ViewBag.reasons = db.OpinionatedIndividualPhotos.Where(x => x.PhotoId == id && x.LikeDislike == false && x.Reason1 != null).ToList().GroupBy(a => a.Reason1).Select(a => new { a.Key, Count = a.Count() }).OrderByDescending(x => x.Count);

            return View();
        }
    }
}
