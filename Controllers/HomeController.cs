﻿using JqueryAjaxCrud.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace JqueryAjaxCrud.Controllers
{
    public class HomeController : Controller
    {
        TestDbEntities1 db;
        public HomeController()
        {
            db = new TestDbEntities1();
        }

        // GET: City
        [HttpGet]
        public ActionResult Index()
        {
            List<ClsCity> listcity = new List<ClsCity>();
            listcity = (from c in db.tblCities
                        join cc in db.tblCountries on c.CountryId equals cc.CountryId
                        select new ClsCity
                        {
                            CityId = c.CityId,
                            CityName = c.CityName,
                            CountryId = c.CountryId,
                            CountryName = cc.CountryName
                        }).ToList();
            return View(listcity);
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
            string message = "";
            bool status = false;
            try
            {
                string returnId = "0";
                string insertUpdateStatus = "";
                if (ccity.CityId > 0)
                {
                    insertUpdateStatus = "Update";

                }
                else
                {
                    insertUpdateStatus = "Save";

                }
                returnId = InsertUpdateCityDb(ccity, insertUpdateStatus);
                if (returnId == "Success")
                {
                    status = true;
                    message = "User Type Successfully Updated";
                }
                else
                {
                    status = false;
                    message = returnId;
                }
            }
            catch (Exception ex)
            {
                status = false;
                message = ex.Message.ToString();
            }

            return new JsonResult { Data = new { status = status, message = message } };
            //string message = "";
            //bool status = false;
            //clsCity city = new clsCity();
            //try
            //{
            //    if (ccity.CityId > 0)
            //    {
            //        var res = db.tblCities.Where(x => x.CityId == ccity.CityId).FirstOrDefault();
            //        res.CityName = ccity.CityName;
            //        res.CountryId = ccity.CountryId;
            //        db.SaveChanges();
            //    }
            //    else
            //    {

            //        tblCity cityy = new tblCity();
            //        cityy.CityName = ccity.CityName;
            //        cityy.CountryId = ccity.CountryId;
            //        db.tblCities.Add(cityy);
            //        db.SaveChanges();
            //    }
            //    status = true;

            //}
            //catch (Exception ex)
            //{
            //    message = ex.Message.ToString();
            //}

            //return new JsonResult { Data = new { status = status, message = message } };
        }

        private string InsertUpdateCityDb(ClsCity st, string insertUpdateStatus)
        {
            string returnId = "0";
            string connection = System.Configuration.ConfigurationManager.ConnectionStrings["ADO"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connection))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spInsertUpdateCity", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = st.CityId;
                        cmd.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = st.CityName;
                        cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = st.CountryId;
                        cmd.Parameters.Add("@InsertUpdateStatus", SqlDbType.NVarChar).Value = insertUpdateStatus;
                        cmd.Parameters.Add("@CheckReturn", SqlDbType.NVarChar, 300).Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        returnId = cmd.Parameters["@CheckReturn"].Value.ToString();
                        cmd.Dispose();
                    }
                    con.Close();
                    con.Dispose();
                }
                catch (Exception ex)
                {
                    returnId = ex.Message.ToString();
                }
            }
            return returnId;
        }
        [HttpPost]
        public ActionResult DeleteCity(int id)
        {
            string message = "";
            bool status = false;

            ClsCity st = new ClsCity();
            st.CityId = id;
            string returnId = InsertUpdateCityDb(st, "Delete");
            if (returnId == "Success")
            {
                ModelState.Clear();
                status = true;
                message = "User Type Successfully Deleted";
            }
            else
            {
                ModelState.Clear();
                status = false;
                message = returnId;
            }
            return new JsonResult { Data = new { status = status, message = message } };
        }

    }
}