using JqueryAjaxCrud.Models;
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
    public class CountryController : Controller
    {
        TestDbEntities1 db;
        public CountryController()
        {
            db = new TestDbEntities1();
        }

        // GET: City
        [HttpGet]
        public ActionResult Index()
        {
            var countries = (from row in db.tblCountries select row).ToList();
            return View(countries);
        }
        [HttpGet]
        public ActionResult AddUpdateCity(int id = 0)
        {
            ClsCountry cntry = new ClsCountry();
            if (id > 0)
            {
                cntry = (from c in db.tblCountries
                        where c.CountryId == id
                        select new ClsCountry
                        {
                            CountryId = c.CountryId,
                            CountryName = c.CountryName
                        }).FirstOrDefault();

            }
            else
            {
                cntry = new ClsCountry
                {
                    CountryId = 0,
                    CountryName = "",
                };
            }
            //ViewBag.Countries = new SelectList(db.tblCountries.OrderBy(x => x.CountryName).ToList(), "CountryId", "CountryName", city.CountryId);

            return PartialView(cntry);
        }
        [HttpPost]
        public ActionResult AddUpdateCity(ClsCountry cntry)
        {
            string message = "";
            bool status = false;
            try
            {
                string returnId = "0";
                string insertUpdateStatus = "";
                if (cntry.CountryId > 0)
                {
                    insertUpdateStatus = "Update";

                }
                else
                {
                    insertUpdateStatus = "Save";

                }
                returnId = InsertUpdateCountryDb(cntry, insertUpdateStatus);
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

        private string InsertUpdateCountryDb(ClsCountry st, string insertUpdateStatus)
        {
            string returnId = "0";
            string connection = System.Configuration.ConfigurationManager.ConnectionStrings["ADO"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connection))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spInsertUpdateCountry", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@CountryId", SqlDbType.Int).Value = st.CountryId;
                        cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = st.CountryName;
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
        public ActionResult DeleteCountry(int id)
        {
            string message = "";
            bool status = false;

            ClsCountry st = new ClsCountry();
            st.CountryId = id;
            string returnId = InsertUpdateCountryDb(st, "Delete");
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