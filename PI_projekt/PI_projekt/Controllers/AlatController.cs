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
    public class AlatController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PI07Context ctx;
        //private readonly ILogger<SignUpController> logger;
        private readonly AppSettings appSettings;

        public AlatController(PI07Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            // this.logger = logger;
            appSettings = options.Value;
        }
        public IActionResult AlatIndex()
        {
            if (HttpContext.Session.GetString("imePrezimeLogiranogKorisnika") != null)
            {
                var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
                var query = ctx.Alats
                            .Where(a => a.Korisnik == logiraniKorisnik)
                            .ToList();

                return View(viewName: "Alat", query);
            }
            else
            {
                return Ok("Imamo nekih problema");
            }
        }



        public IActionResult formaZaDodavanjeNovogAlata()
        {
            return View("dodavanjeNovogAlata");
        }
        public IActionResult dodajNoviAlat(Alat alat)
        {
            if (alat.Naziv != null && (alat.Količina >= 1))
            {
                alat.Korisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
                ctx.Alats.Add(alat);
                ctx.SaveChanges();
                return RedirectToAction("AlatIndex", "Alat");
            }
            else
            {
                ViewBag.Error = "Molimo unesite sva polja i Količina ne smije biti 0 ili niže.";
                return View("dodavanjeNovogAlata");
            }
        }

        public IActionResult FormaZaAlatUpdate(int id)
        {
            var query = ctx.Alats
                    .Where(a => a.AlatId == id)
                    .Where(a => a.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                    .FirstOrDefault<Alat>();
            ViewBag.Message = query.AlatId;
            return View(viewName: "urediAlat", query);
        }
        public IActionResult AlatEdit(Alat alat)
        {
            var query = ctx.Alats
                        .Where(a => a.AlatId == alat.AlatId)
                        .Where(a => a.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                        .FirstOrDefault<Alat>();

            Alat alatedit = ctx.Alats.Find(alat.AlatId);
            if (alat.Naziv == null || alat.Količina <= 0)
            {
                ViewBag.error = "Molimo unesite sva polja i Količina ne smije biti 0 ili negativna vrijednost.";
                ViewBag.Message = query.AlatId;
                return View(viewName: "urediAlat", query);
            }
            else
            {
                alatedit.Naziv = alat.Naziv;
                ctx.SaveChanges();
                alatedit.Količina = alat.Količina;
                ctx.SaveChanges();
                return RedirectToAction("AlatIndex", "Alat");
            }
            /*
            if(alat.Naziv==null && alat.Količina != null)
            {
                 alatedit.Količina = alat.Količina;
                 ctx.SaveChanges();
                 return RedirectToAction("AlatIndex", "Alat");

            }
            if(alat.Naziv!=null && alat.Količina == null)
            {
                 // query.Naziv = alat.Naziv;
                 alatedit.Naziv = alat.Naziv;
                 ctx.SaveChanges();
                 return RedirectToAction("AlatIndex", "Alat");
            }
            if(alat.Naziv!=null && alat.Količina >= 0)
            {
                 alatedit.Naziv=alat.Naziv;
                 ctx.SaveChanges();
                 alatedit.Količina = alat.Količina;
                 ctx.SaveChanges();
                 return RedirectToAction("AlatIndex", "Alat");
            }
            return View("urediAlat");
            */
            //return Ok(query.Količina);
        }

        public IActionResult IzbrisiAlat(int id)
        {
            var query = ctx.Alats
                    .Where(a => a.AlatId == id)
                    .Where(a => a.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                    .FirstOrDefault<Alat>();
            ctx.Alats.Remove(query);
            ctx.SaveChanges();
            return RedirectToAction("AlatIndex", "Alat");
        }
    }
}
