using DesignBureau.BLL.Managers;
using DesignBureau.DAL.Services;
using DesignBureau.Entities.Entity;
using DesignBureau.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DesignBureau.MVC.Controllers
{
    public class HomeController : BaseController
    {
        [Login]
        [HttpGet]
        public ActionResult Index()
        {
            var deals = new BaseDalService<Deal>().GetAll();
            ViewBag.Deals = deals;
            ViewBag.IsAdminSession = UserSession.Current.IsAdmin;
            return View();
        }

        [Admin]
        [HttpGet]
        public ActionResult Settings(int id)
        {
            return View(new BaseDalService<Deal>().GetItem(id));
        }

        [Admin]
        [HttpPost]
        public ActionResult SaveSettings(Deal deal)
        {
            new BaseDalService<Deal>().Update(deal);
            return RedirectToAction("Index");
        }
        [Admin]
        [HttpPost]
        public ActionResult SaveDeal(Deal deal)
        {
            new BaseDalService<Deal>().Add(deal);
            return RedirectToAction("Index");
        }
        [Admin]
        [HttpGet]
        public ActionResult CreateDeal()
        {
            return View();
        }

        [Login]
        [HttpGet]
        public ActionResult SubscribeDeal(int id)
        {
            var deal = new BaseDalService<Deal>().GetItem(id);
            ViewBag.Details = deal;
            return View(deal);
        }
    }
}