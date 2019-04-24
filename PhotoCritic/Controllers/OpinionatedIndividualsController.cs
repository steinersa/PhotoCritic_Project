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
        public ActionResult Details(int id)
        {
            return View();
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
            if (ModelState.IsValid)
            {
                db.OpinionatedIndividuals.Add(opinionatedIndividual);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(opinionatedIndividual);
        }

        // GET: OpinionatedIndividuals/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OpinionatedIndividuals/Edit/5
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

        // GET: OpinionatedIndividuals/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OpinionatedIndividuals/Delete/5
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
