using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI_projekt.Models;
using PI_projekt.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PI_projekt.Controllers
{
    public class UrodController : Controller
    {
        private readonly ILogger<UrodController> _logger;
        private readonly PI07Context ctx;
        //private readonly ILogger<SignUpController> logger;
        private readonly AppSettings appSettings;

        public UrodController(PI07Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            // this.logger = logger;
            appSettings = options.Value;
        }
        public IActionResult UrodIndex(int id)
        {
            var query = ctx.Urods
                        .Where(u => u.Mikrolokacija == id)
                        .ToList();
            ViewBag.MikroID = id;

            var pomQ = ctx.Mikrolokacijas
                        .Where(m => m.MikrolokacijaId == id)
                        .FirstOrDefault<Mikrolokacija>();
            ViewBag.Location = pomQ.Lokacija;

            return View("UrodIndex", query);
        }
        public IActionResult DodajUrod(int id)
        {
            var query = ctx.Urods
                        .Where(m => m.Mikrolokacija == id)
                        .FirstOrDefault<Urod>();
            ViewBag.mlid = id;
            return View("DodajUrodView");
        }
        public IActionResult Dodaj(Urod u)
        {
            var query = ctx.Urods
                        .Where(m => m.Mikrolokacija == u.Mikrolokacija)
                        .FirstOrDefault<Urod>();
            ViewBag.mlid = u.Mikrolokacija;

            if (u.Opis != null && u.Kolicina != null)
            {
                ctx.Urods.Add(u);
                ctx.SaveChanges();
                return RedirectToAction("UrodIndex", new { id = u.Mikrolokacija });
            }
            else
            {
                return View("DodajUrodView", u);
            }
        }

        public IActionResult Izbrisi(int id)
        {
            var query = ctx.Urods
                        .Where(u => u.UrodId == id)
                        .FirstOrDefault<Urod>();
            ViewBag.UrodID = query.Mikrolokacija;

            ctx.Urods.Remove(query);
            ctx.SaveChanges();

            return RedirectToAction("UrodIndex", new { id = ViewBag.UrodID });
        }
        [HttpGet]
        public IActionResult Uredi(int id)
        {
            var query = ctx.Urods
                        .Where(u => u.UrodId == id)
                        .FirstOrDefault<Urod>();
            ViewBag.MikroID = query.Mikrolokacija;
            ViewBag.UrodID = id;

            return View("Uredi", query);
        }
        [HttpPost]
        public IActionResult Uredi(Urod u)
        {
            var query = ctx.Urods
                        .Where(ur => ur.UrodId == u.UrodId)
                        .FirstOrDefault<Urod>();
            ViewBag.MikroID = query.Mikrolokacija;
            ViewBag.UrodID = u.UrodId;

            var uEdit = ctx.Urods.Find(u.UrodId);
            if(u.Kolicina != null && u.Opis != null)
            {
                uEdit.Kolicina = u.Kolicina;
                ctx.SaveChanges();
                uEdit.Opis = u.Opis;
                ctx.SaveChanges();

                return RedirectToAction("UrodIndex", new { id = u.Mikrolokacija });
            }
            else
            {
                return View("Uredi", u);
            }
        }
    }
}
