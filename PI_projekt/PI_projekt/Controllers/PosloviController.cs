using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI_projekt.Models;
using PI_projekt.ViewModels;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PI_projekt.Controllers
{
    public class PosloviController : Controller
    {
        private readonly ILogger<PosloviController> _logger;
        private readonly PI07Context ctx;
        //private readonly ILogger<SignUpController> logger;
        private readonly AppSettings appSettings;

        public PosloviController(PI07Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            // this.logger = logger;
            appSettings = options.Value;
        }
        public IActionResult PosloviIndex(int id)
        {
            var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
            var query = ctx.PosloviBiljkas
                    .Where(pb => pb.Biljka == id)
                    .Select(pb => new PosloviViewModel
                    {
                        PosloviId=pb.PosloviBiljkaId,
                        BiljkaId = id,
                        NazivPosla = pb.PosloviNavigation.Naziv,
                        NazivBiljke = pb.BiljkaNavigation.BiljkaNavigation.Naziv,
                        Opis = pb.Opis,
                       
                    })
                    .ToList();
            var model = new PosloviVM
            {
                PosaoVezanZaBiljku = query
            };
            ViewBag.ID = id;
            return View("PosloviBiljka",model);
        }
        public IActionResult DodajNovuAktivnost(int id)
        {
            ViewBag.ID = id;
            var posao = ctx.Poslovis
                           .OrderBy(p => p.Naziv)
                           .Select(p => new { p.Naziv, p.PosloviId })
                           .ToList();
            ViewBag.Poslovi = new SelectList(posao, nameof(Poslovi.PosloviId), nameof(Poslovi.Naziv));
            return View("FormaCreatePosaoBiljka");
        }
        public IActionResult DodajBiljkaPosao(PosloviBiljka pb)
        {
            ctx.PosloviBiljkas.Add(pb);
            ctx.SaveChanges();
            return RedirectToAction("PosloviIndex", new { id = pb.Biljka });
        }
        public IActionResult EditPosaoBiljkaView(int id)
        {
           var query = ctx.PosloviBiljkas
                .Where(pb => pb.PosloviBiljkaId == id)
                .FirstOrDefault<PosloviBiljka>();
            ViewBag.message = query.PosloviBiljkaId;
            ViewBag.BiljkaID = query.Biljka;
            return View("EditPosaoBiljka", query);
        }
        public IActionResult EditBiljkaPosao(PosloviBiljka pb)
        {
            var query = ctx.PosloviBiljkas
                       .Where(pb => pb.PosloviBiljkaId == pb.PosloviBiljkaId)
                       .FirstOrDefault<PosloviBiljka>();

            PosloviBiljka pbiljka = ctx.PosloviBiljkas.Find(pb.PosloviBiljkaId);
            /*
            if (pb.Opis == null)
            {
                ViewBag.error = "Polje ne može ostati prazno";
                return View("EditPosaoBiljka");

            }*/
            pbiljka.Opis = pb.Opis;
            ctx.SaveChanges();
            return RedirectToAction("PosloviIndex", new { id = pb.Biljka });
        }
        public IActionResult IzbrisiPosaoBiljka(int id) {
            var query = ctx.PosloviBiljkas
                      .Where(p => p.PosloviBiljkaId == id)
                      .FirstOrDefault<PosloviBiljka>();
            ctx.PosloviBiljkas.Remove(query);
            ctx.SaveChanges();
            return RedirectToAction("PosloviIndex", new { id = query.Biljka });
        }
        public IActionResult DodajNoviPosaoView(int id)
        {
            ViewBag.ID = id;
            return View("FormaDodajPosao");
        }
        public IActionResult DodajPosao(Poslovi p)
        {
            var query = ctx.Poslovis;
            var istina = true;
            foreach(var q in query)
            {
                if(q.Naziv == p.Naziv)
                {
                    ViewBag.err = "Naziv već postoji";
                    istina = false;
                    break;
                }
            }
            ViewBag.ID = p.PosloviId;
            if (p.Naziv != null && istina)
            {
                p.PosloviId = 0;
                ctx.Poslovis.Add(p);
                ctx.SaveChanges();
                ViewBag.Message = "Posao je uspješno dodan";
                return RedirectToAction("DodajNoviPosaoView", new { id = ViewBag.ID});
            }
            else
            {
                //ViewBag.err = "Polje Naziv ne može biti prazno";
                return View("FormaDodajPosao", p);
            }
        }
    }
}
