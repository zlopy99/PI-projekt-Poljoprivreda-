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
    public class BezNarudzbiController : Controller
    {
        private readonly ILogger<BezNarudzbiController> _logger;
        private readonly PI07Context ctx;
        //private readonly ILogger<SignUpController> logger;
        private readonly AppSettings appSettings;

        public BezNarudzbiController(PI07Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            // this.logger = logger;
            appSettings = options.Value;
        }
        public IActionResult ProdajaIndex()
        {
            var query = ctx.ProdajaBezNarudzbes
                    .Where(pb => pb.BiljkaNavigation.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                    .Select(pb => new ProdajaBezNarudzbeViewModel
                    {
                        ProdajaBezNarudzbeId=pb.ProdajaBezNarudzbeId,
                        KorisnikId= (int)HttpContext.Session.GetInt32("idLogiranogKorisnika"),
                        KupacId=(int)pb.Kupac,
                        biljkaKorisnikId=(int)pb.Biljka,
                        NazivBiljke=pb.BiljkaNavigation.BiljkaNavigation.Naziv,
                        ImeKupca=pb.KupacNavigation.ImePrezime,
                        KontaktKupca=pb.KupacNavigation.Kontkat,
                        Kolicina=pb.Kolicina,
                        Cijena=pb.UkupnaCijena,
                        Opis=pb.Opis
                    })
                    .ToList();
            var model = new ProdajaBezNarudzbeVM
            {
                ProdajaBezNarudzbe = query
            };
            return View("ProdajaBezNaruzbiView",model);
        }
        public IActionResult DodajNovuView()
        {
            var queryForSelect = ctx.Kupacs
                        .ToList();
            ViewBag.Kupac = new SelectList(queryForSelect, nameof(Kupac.KupacId), nameof(Kupac.ImePrezime));
            var query = ctx.BiljkeKorisniks
                .Where(b => b.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                .Select(b => new { b.BiljkaNavigation.Naziv, b.BiljkaKorisnikId })
                .ToList();
            ViewBag.Biljka = new SelectList(query, nameof(BiljkeKorisnik.BiljkaKorisnikId), nameof(BiljkeKorisnik.BiljkaNavigation.Naziv));
            return View("DodajNovu");
        }
        public IActionResult Dodaj(ProdajaBezNarudzbe pbn)
        {
            var queryForSelect = ctx.Kupacs
                        .ToList();
            ViewBag.Kupac = new SelectList(queryForSelect, nameof(Kupac.KupacId), nameof(Kupac.ImePrezime));

            var query = ctx.BiljkeKorisniks
                .Where(b => b.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                .Select(b => new { b.BiljkaNavigation.Naziv, b.BiljkaKorisnikId })
                .ToList();
            ViewBag.Biljka = new SelectList(query, nameof(BiljkeKorisnik.BiljkaKorisnikId), nameof(BiljkeKorisnik.BiljkaNavigation.Naziv));

            ViewBag.ErrBiljka = "Izaberite biljku!";
            ViewBag.ErrKupac = "Izaberite kupca!";

            if (pbn.Biljka == null || pbn.Kupac == null || pbn.UkupnaCijena == null)
            {
                return View("DodajNovu", pbn);
            }
            else
            {
                ctx.ProdajaBezNarudzbes.Add(pbn);
                ctx.SaveChanges();
                ViewBag.poruka = "Uspjesno dodano!";
                return RedirectToAction("ProdajaIndex");
            }
        }
        public IActionResult Izbrisi(int id)
        {
            var query = ctx.ProdajaBezNarudzbes
                    .Where(n => n.ProdajaBezNarudzbeId == id)
                    .FirstOrDefault<ProdajaBezNarudzbe>();
            ctx.ProdajaBezNarudzbes.Remove(query);
            ctx.SaveChanges();
            return RedirectToAction("ProdajaIndex");
        }
        public IActionResult Uredi(int id)
        {
            var queryForSelect = ctx.Kupacs
                        .ToList();
            ViewBag.Kupac = new SelectList(queryForSelect, nameof(Kupac.KupacId), nameof(Kupac.ImePrezime));

            var query = ctx.BiljkeKorisniks
                .Where(b => b.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                .Select(b => new { b.BiljkaNavigation.Naziv, b.BiljkaKorisnikId })
                .ToList();
            ViewBag.Biljka = new SelectList(query, nameof(BiljkeKorisnik.BiljkaKorisnikId), nameof(BiljkeKorisnik.BiljkaNavigation.Naziv));

            var query3 = ctx.ProdajaBezNarudzbes
                        .Where(m => m.ProdajaBezNarudzbeId == id)
                        .FirstOrDefault<ProdajaBezNarudzbe>();
            ViewBag.ProdajaBezNarudzbeId = id;

            return View("EditProdajuBezNarudzbe", query3);
        }
        public IActionResult UrediAkcija(ProdajaBezNarudzbe pbn)
        {
            var queryForSelect = ctx.Kupacs
                        .ToList();
            ViewBag.Kupac = new SelectList(queryForSelect, nameof(Kupac.KupacId), nameof(Kupac.ImePrezime));

            var query = ctx.BiljkeKorisniks
                .Where(b => b.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                .Select(b => new { b.BiljkaNavigation.Naziv, b.BiljkaKorisnikId })
                .ToList();
            ViewBag.Biljka = new SelectList(query, nameof(BiljkeKorisnik.BiljkaKorisnikId), nameof(BiljkeKorisnik.BiljkaNavigation.Naziv));

            ViewBag.ProdajaBezNarudzbeId = pbn.ProdajaBezNarudzbeId;

            if (pbn.Biljka == null || pbn.Kupac == null || pbn.UkupnaCijena == null)
            {
                return View("EditProdajuBezNarudzbe", pbn);
            }
            else
            {
                ProdajaBezNarudzbe edit = ctx.ProdajaBezNarudzbes.Find(pbn.ProdajaBezNarudzbeId);
                edit.Kupac = pbn.Kupac;
                ctx.SaveChanges();
                edit.Biljka = pbn.Biljka;
                ctx.SaveChanges();
                edit.Kolicina = pbn.Kolicina;
                ctx.SaveChanges();
                edit.UkupnaCijena = pbn.UkupnaCijena;
                ctx.SaveChanges();
                edit.Opis = pbn.Opis;
                ctx.SaveChanges();
                return RedirectToAction("ProdajaIndex");
            }
        }
    }
}
