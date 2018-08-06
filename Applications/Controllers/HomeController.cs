using Applications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Applications.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext database = new DatabaseContext();

        [HttpGet]
        public ActionResult Index()
        {
            List<Application> applications = new List<Application>();

            return View(applications);
        }

        [HttpGet]
        public ActionResult AllApplications()
        {

            List<Application> applications = this.database.Applications.ToList();           

            return View(this.GetAllAppsForApplicationTypeID(applications));

        }

        [HttpGet]
        public ActionResult Report()
        {
            //DateTime dt = new DateTime(0);

            List<string> date = new List<string>();

            return View(date);
        }

        private Queue<string> GetAllAppsForApplicationTypeID(List<Application> apps)
        {
            Queue<string> strs = new Queue<string>();

            int lastid = -1;

            foreach (var app in apps)
            {
                if (app.ApplicationTypeID == lastid)
                {
                    strs.Enqueue(this.database.Items.ToList()[app.ItemID - 1].ItemName + " " + app.Count);
                }
                else
                {
                    lastid = app.ApplicationTypeID;
                    strs.Enqueue("");
                    strs.Enqueue(app.Date.Month.ToString() + " " + app.Date.Year);
                    strs.Enqueue(this.database.Items.ToList()[app.ItemID - 1].ItemName + " " + app.Count);
                }
            }

            if(strs.Count <= 0)
            {
                strs.Enqueue("");
            }

            return strs;
        }

        private List<string> GetSumsStringsForSomeListOfApps(List<Application> apps)
        {
            List<string> strs = new List<string>();

            if (apps.Count > 0)
            {
                strs.Add(apps[0].Date.Month + " " + apps[0].Date.Year);

                for (int i = 0; i < this.database.Items.Count(); i++)
                {
                    List<Application> aps = apps.FindAll(x => x.ItemID == i+1);

                    if (aps != null)
                    {
                        int sum = 0;
                        foreach (var a in aps)
                        {
                            sum += a.Count;
                        }

                        strs.Add(this.database.Items.ToList<Item>()[i].ItemName + " " + sum);
                    }
                }
            }
            else
            {
                strs.Add("Ничего не найдено!");
            }

            return strs;
        }

        [HttpPost]
        public ActionResult GetReport(List<string> date)
        {
            List<Application> apps = this.database.Applications.ToList();

            //New comment
            
            List<Application> sorted_apps = 
                apps.FindAll(x => (x.Date.Month == int.Parse(date[0]) && x.Date.Year == int.Parse(date[1])));


            return View(this.GetSumsStringsForSomeListOfApps(sorted_apps));
        }

        [HttpPost]
        public ActionResult SendApplication(List<Application> applications)
        {
            int counter = 0; 

            foreach(var application in applications)
            {
                if(application.Count > 0)
                {
                    application.ItemID = counter + 1;

                    //Add comment

                    List<Application> appsList = this.database.Applications.ToList();

                    if (appsList.Count > 0)
                        application.ApplicationTypeID = appsList[appsList.Count-1].ApplicationTypeID + 1;
                    else
                        application.ApplicationTypeID = 0;

                    application.Date = DateTime.Now;
                    this.database.Applications.Add(application);
                }
                counter++;
            }

            this.database.SaveChanges();

            return View();
        }
    }
}