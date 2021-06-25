using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI_projekt.Models;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace PI_projekt.Controllers
{
    public class TroskoviController : Controller
    {
        private readonly ILogger<TroskoviController> _logger;
        private readonly PI07Context ctx;
        //private readonly ILogger<SignUpController> logger;
        private readonly AppSettings appSettings;

        public TroskoviController(PI07Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            // this.logger = logger;
            appSettings = options.Value;
        }
        public IActionResult TroskoviIndex()
        {

            if (HttpContext.Session.GetString("imePrezimeLogiranogKorisnika") != null)
            {
                var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");

                var query = ctx.Troškovis
                            .Where(a => a.Korisnik == logiraniKorisnik)
                            .ToList();

                return View(viewName: "Troskovi", query);

            }
            else
            {
                return Ok("Imamo nekih problema");
            }


        }
        public IActionResult FormaZaDodavanjeNovogTroska()
        {
            return View("dodavanjeNovogTroska");
        }
        public IActionResult DodajNoviTrosak(Troškovi trosak)
        {
            if (trosak.Datum == null || trosak.Namjena == null || trosak.Iznos == null)
            {
                ViewBag.error = "Sva polja moraju biti ispunjena!";
                return View("dodavanjeNovogTroska");
            }
            else
            {
                trosak.Korisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
                ctx.Troškovis.Add(trosak);
                ctx.SaveChanges();
                return RedirectToAction("TroskoviIndex", "Troskovi");
            }
        }
        public IActionResult IzbrisiTrosak(int id)
        {

            var query = ctx.Troškovis
                    .Where(t => t.TroskoviId == id)
                    .Where(t => t.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                    .FirstOrDefault<Troškovi>();
            ctx.Troškovis.Remove(query);
            ctx.SaveChanges();
            return RedirectToAction("TroskoviIndex", "Troskovi");

        }
        public IActionResult FormaZaEditTroska(int id)
        {
            var query = ctx.Troškovis
                    .Where(t => t.TroskoviId == id)
                    .Where(t => t.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                    .FirstOrDefault<Troškovi>();
            ViewBag.Message = query.TroskoviId;
            return View(viewName: "urediTrosak", query);
        }
        public IActionResult UrediTrosak(Troškovi trosak)
        {
            var query = ctx.Troškovis
                       .Where(t => t.TroskoviId == trosak.TroskoviId)
                       .Where(t => t.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                       .FirstOrDefault<Troškovi>();

            Troškovi trosakedit = ctx.Troškovis.Find(trosak.TroskoviId);
            if (trosak.Namjena == null && trosak.Iznos == null && trosak.Datum == null)
            {
                ViewBag.error = "Niste popunili nijedno polje";
                //return View("urediTrosak");
                ViewBag.Message = query.TroskoviId;
                return View(viewName: "urediTrosak", query);
            }
            else
            {
                if (trosak.Namjena != null)
                {
                    trosakedit.Namjena = trosak.Namjena;
                    ctx.SaveChanges();
                }
                if (trosak.Iznos != null)
                {
                    trosakedit.Iznos = trosak.Iznos;
                    ctx.SaveChanges();
                }
                if (trosak.Datum != null)
                {
                    trosakedit.Datum = trosak.Datum;
                    ctx.SaveChanges();
                }
                if (trosak.Opis != null)
                {
                    trosakedit.Opis = trosak.Opis;
                    ctx.SaveChanges();
                }
                trosakedit.Korisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
                ctx.SaveChanges();
                return RedirectToAction("TroskoviIndex", "Troskovi");
            }
        }
        public IActionResult PlaniraniTrosakIndex()
        {

            if (HttpContext.Session.GetString("imePrezimeLogiranogKorisnika") != null)
            {
                var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
                var query = ctx.PlaniraniTroškovis
                            .Where(a => a.Korisnik == logiraniKorisnik)
                            .ToList();

                return View(viewName: "PlaniraniTrosak", query);
            }
            else
            {
                return Ok("Imamo nekih problema");
            }


        }
        public IActionResult FormaZaDodavanjePlaniranogTroska()
        {
            return View("noviPlaniraniTrosak");
        }
        public IActionResult DodajPlaniraniTrosak(PlaniraniTroškovi plTrosak)
        {
            if (plTrosak.Datum == null || plTrosak.Namjena == null || plTrosak.Iznos == null)
            {
                ViewBag.error = "Sva polja moraju biti ispunjena!";
                return View("noviPlaniraniTrosak");
            }
            else
            {
                plTrosak.Korisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
                ctx.PlaniraniTroškovis.Add(plTrosak);
                ctx.SaveChanges();
                return RedirectToAction("PlaniraniTrosakIndex", "Troskovi");
            }
        }
        public IActionResult IzbrisiPlaniraniTrosak(int id)
        {
            var query = ctx.PlaniraniTroškovis
                    .Where(pt => pt.PlaniraniTroskoviId == id)
                    .Where(pt => pt.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                    .FirstOrDefault<PlaniraniTroškovi>();
            ctx.PlaniraniTroškovis.Remove(query);
            ctx.SaveChanges();
            return RedirectToAction("PlaniraniTrosakIndex", "Troskovi");
        }
        public IActionResult FormaZaEditPlaniranogTroska(int id)
        {
            var query = ctx.PlaniraniTroškovis
                    .Where(t => t.PlaniraniTroskoviId == id)
                    .Where(t => t.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                    .FirstOrDefault<PlaniraniTroškovi>();
            ViewBag.Message = query.PlaniraniTroskoviId;
            return View(viewName: "urediPlaniraniTrosak", query);
        }
        public IActionResult UrediPlaniraniTrosak(PlaniraniTroškovi pltrosak)
        {
            var query = ctx.PlaniraniTroškovis
                       .Where(pt => pt.PlaniraniTroskoviId == pltrosak.PlaniraniTroskoviId)
                       .Where(pt => pt.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                       .FirstOrDefault<PlaniraniTroškovi>();

            PlaniraniTroškovi plTrosakedit = ctx.PlaniraniTroškovis.Find(pltrosak.PlaniraniTroskoviId);
            if (pltrosak.Namjena == null && pltrosak.Iznos == null && pltrosak.Datum == null)
            {
                ViewBag.error = "Niste popunili nijedno polje";
                ViewBag.Message = query.PlaniraniTroskoviId;
                return View(viewName: "urediPlaniraniTrosak", query);
            }
            else
            {
                if (pltrosak.Namjena != null)
                {
                    plTrosakedit.Namjena = pltrosak.Namjena;
                    ctx.SaveChanges();
                }
                if (pltrosak.Iznos != null)
                {
                    plTrosakedit.Iznos = pltrosak.Iznos;
                    ctx.SaveChanges();
                }
                if (pltrosak.Datum != null)
                {
                    plTrosakedit.Datum = pltrosak.Datum;
                    ctx.SaveChanges();
                }
                if (pltrosak.Opis != null)
                {
                    plTrosakedit.Opis = pltrosak.Opis;
                    ctx.SaveChanges();
                }
                plTrosakedit.Korisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
                ctx.SaveChanges();
                return RedirectToAction("PlaniraniTrosakIndex", "Troskovi");
            }
        }
    }
}

/*
namespace PI_projekt.Controllers
{
    public class TroskoviController : Controller
    {
        private readonly ILogger<TroskoviController> _logger;
        private readonly PI07Context ctx;
        //private readonly ILogger<SignUpController> logger;
        private readonly AppSettings appSettings;

        public TroskoviController(PI07Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            // this.logger = logger;
            appSettings = options.Value;
        }
         public IActionResult TroskoviIndex()
         {
             
                 if (HttpContext.Session.GetString("imePrezimeLogiranogKorisnika") != null)
                 {
                     var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");

                     var query = ctx.Troškovis
                                 .Where(a => a.Korisnik == logiraniKorisnik)
                                 .ToList();

                     return View(viewName: "Troskovi", query);

                 }
                 else
                 {
                     return Ok("Imamo nekih problema");
                 }

             
         }
        public IActionResult FormaZaDodavanjeNovogTroska()
        {
            return View("dodavanjeNovogTroska");
        }
        public IActionResult DodajNoviTrosak(Troškovi trosak)
        {
            trosak.Korisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
            ctx.Troškovis.Add(trosak);
            ctx.SaveChanges();
            return RedirectToAction("TroskoviIndex", "Troskovi");
        }
        public IActionResult IzbrisiTrosak(int id)
        {

            var query = ctx.Troškovis
                    .Where(t => t.TroskoviId == id)
                    .Where(t => t.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                    .FirstOrDefault<Troškovi>();
            ctx.Troškovis.Remove(query);
            ctx.SaveChanges();
            return RedirectToAction("TroskoviIndex", "Troskovi");

        }
        public IActionResult FormaZaEditTroska(int id)
        {

            var query = ctx.Troškovis
                    .Where(t => t.TroskoviId == id)
                    .Where(t => t.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                    .FirstOrDefault<Troškovi>();
            ViewBag.Message = query.TroskoviId;
            return View("urediTrosak");


        }
        public IActionResult UrediTrosak(Troškovi trosak)
        {
            var query = ctx.Troškovis
                       .Where(t => t.TroskoviId== trosak.TroskoviId)
                       .Where(t => t.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                       .FirstOrDefault<Troškovi>();

            Troškovi trosakedit = ctx.Troškovis.Find(trosak.TroskoviId);
            if(trosak.Namjena == null && trosak.Iznos == null && trosak.Datum == null && trosak.Opis == null)
            {
                ViewBag.error = "Niste popunili nijedno polje";
                return View("urediTrosak");
            }
            if (trosak.Namjena != null)
            {
                trosakedit.Namjena = trosak.Namjena;
                ctx.SaveChanges();
            }
            if (trosak.Iznos != null)
            {
                trosakedit.Iznos = trosak.Iznos;
                ctx.SaveChanges();
            }
            if (trosak.Datum != null)
            {
                trosakedit.Datum = trosak.Datum;
                ctx.SaveChanges();
            }
            if (trosak.Opis != null)
            {
                trosakedit.Opis = trosak.Opis;
                ctx.SaveChanges();
            }
            trosakedit.Korisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
            ctx.SaveChanges();
            return RedirectToAction("TroskoviIndex","Troskovi");
        }
        public IActionResult PlaniraniTrosakIndex()
        {

            if (HttpContext.Session.GetString("imePrezimeLogiranogKorisnika") != null)
            {
                var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");

                var query = ctx.PlaniraniTroškovis
                            .Where(a => a.Korisnik == logiraniKorisnik)
                            .ToList();

                return View(viewName: "PlaniraniTrosak", query);

            }
            else
            {
                return Ok("Imamo nekih problema");
            }
            

        }
        public IActionResult FormaZaDodavanjePlaniranogTroska()
        {
            return View("noviPlaniraniTrosak");
        }
        public IActionResult DodajPlaniraniTrosak(PlaniraniTroškovi plTrosak)
        {
            plTrosak.Korisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
            ctx.PlaniraniTroškovis.Add(plTrosak);
            ctx.SaveChanges();
            return RedirectToAction("PlaniraniTrosakIndex", "Troskovi");
        }
        public IActionResult IzbrisiPlaniraniTrosak(int id)
        {
            var query = ctx.PlaniraniTroškovis
                    .Where(pt => pt.PlaniraniTroskoviId == id)
                    .Where(pt => pt.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                    .FirstOrDefault<PlaniraniTroškovi>();
            ctx.PlaniraniTroškovis.Remove(query);
            ctx.SaveChanges();
            return RedirectToAction("PlaniraniTrosakIndex", "Troskovi");
        }
        public IActionResult FormaZaEditPlaniranogTroska(int id)
        {

            var query = ctx.PlaniraniTroškovis
                    .Where(t => t.PlaniraniTroskoviId == id)
                    .Where(t => t.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                    .FirstOrDefault<PlaniraniTroškovi>();
            ViewBag.Message = query.PlaniraniTroskoviId;
            return View("urediPlaniraniTrosak");


        }
        public IActionResult UrediPlaniraniTrosak(PlaniraniTroškovi pltrosak)
        {
            var query = ctx.PlaniraniTroškovis
                       .Where(pt => pt.PlaniraniTroskoviId == pltrosak.PlaniraniTroskoviId)
                       .Where(pt => pt.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                       .FirstOrDefault<PlaniraniTroškovi>();

            PlaniraniTroškovi plTrosakedit = ctx.PlaniraniTroškovis.Find(pltrosak.PlaniraniTroskoviId);
            if (pltrosak.Namjena == null && pltrosak.Iznos == null && pltrosak.Datum == null && pltrosak.Opis == null)
            {
                ViewBag.error = "Niste popunili nijedno polje";
                return View("urediPlaniraniTrosak");
            }
            if (pltrosak.Namjena != null)
            {
                plTrosakedit.Namjena = pltrosak.Namjena;
                ctx.SaveChanges();
            }
            if (pltrosak.Iznos != null)
            {
                plTrosakedit.Iznos = pltrosak.Iznos;
                ctx.SaveChanges();
            }
            if (pltrosak.Datum != null)
            {
                plTrosakedit.Datum = pltrosak.Datum;
                ctx.SaveChanges();
            }
            if (pltrosak.Opis != null)
            {
                plTrosakedit.Opis = pltrosak.Opis;
                ctx.SaveChanges();
            }
            plTrosakedit.Korisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
            ctx.SaveChanges();
            return RedirectToAction("PlaniraniTrosakIndex", "Troskovi");
        }
    }
}
*/
