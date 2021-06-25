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
    public class FazaRazvojaController : Controller
    {
        private readonly ILogger<TroskoviController> _logger;
        private readonly PI07Context ctx;
        //private readonly ILogger<SignUpController> logger;
        private readonly AppSettings appSettings;

        public FazaRazvojaController(PI07Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            // this.logger = logger;
            appSettings = options.Value;
        }
        public IActionResult FazaRazvojaIndex(int id)
        {
            var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
            var query = ctx.FazaRazvojaBiljkes
                    .Where(fr => fr.BiljkaNavigation.BiljkaId == id)//BiljkaNavigation.BiljkaId
                    .Select(fr => new FazaRazvojaVM
                    {
                        Naziv = fr.FazaRazvojaNavigation.Naziv,
                        Opis = fr.Opis,
                        BiljkaKorisnikId = id,
                        BiljkaFazaRazvojaId = fr.FazaRazvojaBiljkeId,
                        TrajanjeFaze=fr.TrajanjeFaze,
                        GodisnjeDobaOdvijanjaFaze = fr.GodisnjeDobaOdvijanjaFaze
                    })
                    .ToList();
            var model = new FazaRazvojaViewModel
            {
                FazaRazvojaBiljke = query
            };
            var idKorBilj = ctx.BiljkeKorisniks
                    .Where(bk => bk.Biljka == id)
                    .Where(bk => bk.Korisnik == logiraniKorisnik)
                    .Select(bk => bk.BiljkaKorisnikId);
            foreach(var item in idKorBilj)
            {
                ViewBag.id = item;
            }
            ViewBag.IDe = id;
            return View("FazaRazvojaView",model);
        }
        public IActionResult DodajNovuView(int id)
        {
            ViewBag.IDe = id;
            var faze = ctx.FazaRazvojas
                          .OrderBy(f => f.FazaRazvojaId)
                          .Select(f => new { f.Naziv, f.FazaRazvojaId })
                          .ToList();
            ViewBag.FazaRazvoja = new SelectList(faze, nameof(FazaRazvoja.FazaRazvojaId),nameof(FazaRazvoja.Naziv));
            return View("DodajFazuRazvoja");
            // return Ok(queryForSelect);
        }
        public IActionResult Dodaj(FazaRazvojaBiljke frb)
        {
            /*
            if(frb.FazaRazvoja == null && frb.GodisnjeDobaOdvijanjaFaze==null && frb.TrajanjeFaze==null && frb.Opis == null)
            {
                ViewBag.err = "Niste popunili nijedno polje";
                return RedirectToAction("DodajNovuView", new { id = frb.Biljka });
            }*/
            ctx.FazaRazvojaBiljkes.Add(frb);
            ctx.SaveChanges();
            return RedirectToAction("FazaRazvojaIndex", new { id = frb.Biljka });
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var faze = ctx.FazaRazvojas
                          .OrderBy(f => f.FazaRazvojaId)
                          .Select(f => new { f.Naziv, f.FazaRazvojaId })
                          .ToList();
            ViewBag.FazaRazvoja = new SelectList(faze, nameof(FazaRazvoja.FazaRazvojaId), nameof(FazaRazvoja.Naziv));

            var query = ctx.FazaRazvojaBiljkes
                            .Where(f => f.FazaRazvojaBiljkeId == id)
                            .FirstOrDefault<FazaRazvojaBiljke>();

            ViewBag.IDe = query.Biljka;
            ViewBag.fazaID = query.FazaRazvojaBiljkeId; 
            return View("Edit", query);
        }
        [HttpPost]
        public IActionResult Edit(FazaRazvojaBiljke fzr)
        {
            FazaRazvojaBiljke fzrEdit = ctx.FazaRazvojaBiljkes.Find(fzr.FazaRazvojaBiljkeId);
            fzrEdit.FazaRazvoja = fzr.FazaRazvoja;
            ctx.SaveChanges();
            fzrEdit.Opis = fzr.Opis;
            ctx.SaveChanges();
            fzrEdit.TrajanjeFaze = fzr.TrajanjeFaze;
            ctx.SaveChanges();
            fzrEdit.GodisnjeDobaOdvijanjaFaze = fzr.GodisnjeDobaOdvijanjaFaze;
            ctx.SaveChanges();

            return RedirectToAction("FazaRazvojaIndex", new { id = fzr.Biljka });
        }
        public IActionResult Izbrisi(int id)
        {
            var query2 = ctx.FazaRazvojaBiljkes
                      .Where(a => a.FazaRazvojaBiljkeId == id)
                      .FirstOrDefault<FazaRazvojaBiljke>();
            var pomQ2 = query2.Biljka;
            ctx.FazaRazvojaBiljkes.Remove(query2);
            ctx.SaveChanges();

            return RedirectToAction("FazaRazvojaIndex", new { id = pomQ2 });
        }
    }
    
}
