using JqueryAjaxCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace JqueryAjaxCrud.Controllers
{
    public class HomeController : Controller
    {
        TestDbEntities db;
        public HomeController()
        {
            db = new TestDbEntities();
        }

        // GET: City
        [HttpGet]
        public ActionResult Index()
        {
            List<ClsCity> listt = new List<ClsCity>();
            listt = (from c in db.tblCities
                        join cc in db.tblCountries on c.CountryId equals cc.CountryId
                        select new ClsCity
                        {
                            CityId = c.CityId,
                            CityName = c.CityName,
                            CountryId = c.CountryId,
                            CountryName = cc.CountryName
                        }).ToList();
            var results = (from row in db.tblCities select row).ToList();
            return View(listt);
        }
        [HttpGet]
        public ActionResult AddUpdateCity(int id = 0)
        {
            ClsCity city = new ClsCity();
            if (id > 0)
            {
                city = (from c in db.tblCities
                        where c.CityId == id
                        select new ClsCity
                        {
                            CityId = c.CityId,
                            CityName = c.CityName,
                            CountryId = c.CountryId
                        }).FirstOrDefault();

            }
            else
            {
                city = new ClsCity
                {
                    CityId = 0,
                    CityName = "",
                    CountryId = 0
                };
            }
            ViewBag.Countries = new SelectList(db.tblCountries.OrderBy(x => x.CountryName).ToList(), "CountryId", "CountryName", city.CountryId);

            return PartialView(city);
        }
        [HttpPost]
        public ActionResult AddUpdateCity(ClsCity ccity)
        {
            ClsCity city = new ClsCity();
            if (ccity.CityId > 0)
            {
                var res = db.tblCities.Where(x => x.CityId == ccity.CityId).FirstOrDefault();
                res.CityName = ccity.CityName;
                res.CountryId = ccity.CountryId;
                db.SaveChanges();
            }
            else
            {

                tblCity cityy = new tblCity();
                cityy.CityName = ccity.CityName;
                cityy.CountryId = ccity.CountryId;
                db.tblCities.Add(cityy);
            }

            ViewBag.Countries = new SelectList(db.tblCountries.OrderBy(x => x.CountryName).ToList(), "CountryId", "CountryName", city.CountryId);


            db.SaveChanges();
            return Json(new { msg = "data saved" });
        }

        [HttpPost]
        public ActionResult DeleteCity(int id)
        {
            var result = db.tblCities.Single(city => city.CityId == id);
            db.tblCities.Remove(result);
            db.SaveChanges();
            return Json(new { msg = "deleted" });
        }

    }
}