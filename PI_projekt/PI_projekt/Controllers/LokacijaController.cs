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
    public class LokacijaController : Controller
    {
        private readonly ILogger<LokacijaController> _logger;
        private readonly PI07Context ctx;
        //private readonly ILogger<SignUpController> logger;
        private readonly AppSettings appSettings;

        public LokacijaController(PI07Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            // this.logger = logger;
            appSettings = options.Value;
        }
        public IActionResult LokacijaIndex()
        {
            var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
            var query = ctx.Lokacijas
                .Where(l => l.Korisnik == logiraniKorisnik)
                .Select(l => new LokacijaViewModel
                {
                    KorisnikID = (int)logiraniKorisnik,
                    LokacijaID = l.LokacijaId,
                    NazivLokacije = l.Naziv,
                    Povrsina = l.UkupnaPovrsinaParcele,
                    ObradjenaPovrsina = l.ObrađenaPovrsina,
                    VrstaTla = l.VrstaTlaNavigation.Naziv
                })
                .ToList();
            var model = new LokacijaVM
            {
                Lokacija = query
            };
            return View("LokacijaView", model);
        }
        public IActionResult DodajNovu()
        {
            var query = ctx.VrstaTlas
                          .OrderBy(t => t.Naziv)
                          .Select(t => new { t.Naziv, t.VrstaTlaId })
                          .ToList();
            ViewBag.VrstaTla = new SelectList(query, nameof(VrstaTla.VrstaTlaId), nameof(VrstaTla.Naziv));
            return View("FormaDodajLokaciju");
        }
        public IActionResult DodajLokaciju(Lokacija lokacija)
        {
            var query = ctx.VrstaTlas
                          .OrderBy(t => t.Naziv)
                          .Select(t => new { t.Naziv, t.VrstaTlaId })
                          .ToList();
            ViewBag.VrstaTla = new SelectList(query, nameof(VrstaTla.VrstaTlaId), nameof(VrstaTla.Naziv));
            if (lokacija.Naziv != null)
            {
                lokacija.Korisnik = (int)HttpContext.Session.GetInt32("idLogiranogKorisnika");
                ctx.Lokacijas.Add(lokacija);
                ctx.SaveChanges();
                return RedirectToAction("LokacijaIndex");
            }
            else
            {
                lokacija.Korisnik = (int)HttpContext.Session.GetInt32("idLogiranogKorisnika");
                return View("FormaDodajLokaciju", lokacija);
            }
        }
        public IActionResult MikrolokacijeIndex(int id)
        {
            var query = ctx.Mikrolokacijas
                    .Where(m => m.Lokacija == id)
                    .Where(m => m.LokacijaNavigation.Korisnik == (int)HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                    .Select(m => new MikrolokacijaViewModel
                    {
                        mikrolokacijaId = m.MikrolokacijaId,
                        korisnikId = (int)HttpContext.Session.GetInt32("idLogiranogKorisnika"),
                        lokacijaId = id,
                        korisnikBiljkaId = m.Biljka,
                        tipUzgojaId = m.TipUzgoja,
                        nazivTipaUzgoja = m.TipUzgojaNavigation.Naziv,
                        nazivBiljke = m.BiljkaNavigation.BiljkaNavigation.Naziv,
                        povrsina = m.Površina,
                        ocekivaniUrod = m.OčekivaniUrod,
                        opis = m.Opis
                    })
                    .ToList();
            ViewBag.idLokacije = id;
            var model = new MikrolokacijaVM
            {
                mikrolokacija = query
            };
            return View("MikroLokacijaIndex", model);
        }
        public IActionResult UrediLokacijuView(int id)
        {
            var pomQuery = ctx.VrstaTlas
                          .OrderBy(t => t.Naziv)
                          .Select(t => new { t.Naziv, t.VrstaTlaId })
                          .ToList();
            ViewBag.VrstaTla = new SelectList(pomQuery, nameof(VrstaTla.VrstaTlaId), nameof(VrstaTla.Naziv));
            ViewBag.id = id;

            var query = ctx.Lokacijas
                        .Where(l => l.LokacijaId == id)
                        .Where(l => l.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                        .FirstOrDefault<Lokacija>();
            return View("UrediLokaciju", query);
        }
        public IActionResult Uredi(Lokacija lokacija)
        {
            var query = ctx.Lokacijas
                       .Where(a => a.LokacijaId == lokacija.LokacijaId)
                       .Where(a => a.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                       .FirstOrDefault<Lokacija>();
            Lokacija lokedit = ctx.Lokacijas.Find(lokacija.LokacijaId);
            if (lokacija.Naziv == null)
            {
                var pomQuery = ctx.VrstaTlas
                          .OrderBy(t => t.Naziv)
                          .Select(t => new { t.Naziv, t.VrstaTlaId })
                          .ToList();
                ViewBag.VrstaTla = new SelectList(pomQuery, nameof(VrstaTla.VrstaTlaId), nameof(VrstaTla.Naziv));
                //return View("urediTrosak");
                ViewBag.id = lokacija.LokacijaId;
                return View(viewName: "UrediLokaciju", query);
            }
            else
            {
                lokedit.Naziv = lokacija.Naziv;
                ctx.SaveChanges();
                lokedit.UkupnaPovrsinaParcele = lokacija.UkupnaPovrsinaParcele;
                ctx.SaveChanges();
                lokedit.ObrađenaPovrsina = lokacija.ObrađenaPovrsina;
                ctx.SaveChanges();
                lokedit.VrstaTla = lokacija.VrstaTla;
                ctx.SaveChanges();
                lokedit.Korisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
                ctx.SaveChanges();
                return RedirectToAction("LokacijaIndex", "Lokacija");
            }
        }
        public IActionResult Izbrisi(int id)
        {
            /*
            var query2 = ctx.Mikrolokacijas
                        .Where(m => m.Lokacija == id)
                        .ToList();
            foreach(var m in query2)
            {
                if( m != null)
                {
                    var query3 = ctx.Urods
                            .Where(u => u.Mikrolokacija == m.MikrolokacijaId)
                            .ToList();
                    foreach(var u in query3)
                    {
                        if(u != null)
                        {
                            ctx.Urods.Remove(u);
                        }
                    }
                    ctx.Mikrolokacijas.Remove(m);
                }
            }
            */

            var query = ctx.Lokacijas
                .Where(a => a.LokacijaId == id)
                .Where(a => a.Korisnik == (int)HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                .FirstOrDefault<Lokacija>();

            ctx.Lokacijas.Remove(query);
            ctx.SaveChanges();
            return RedirectToAction("LokacijaIndex", "Lokacija");
        }
        public IActionResult DodajNovuMikrolokaciju(int id)
        {
            var query = ctx.TipUzgojas
                          .OrderBy(t => t.Naziv)
                          .Select(t => new { t.Naziv, t.TipUzgojaId })
                          .ToList();
            var query2 = ctx.BiljkeKorisniks
                          .Where(b => b.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                          .Select(b => new { b.BiljkaNavigation.Naziv, b.BiljkaKorisnikId})
                          .ToList();
            ViewBag.LokacijaId = id;
            ViewBag.TipUzgoja = new SelectList(query, nameof(TipUzgoja.TipUzgojaId), nameof(TipUzgoja.Naziv));
            ViewBag.Biljka = new SelectList(query2, nameof(BiljkeKorisnik.BiljkaKorisnikId), nameof(BiljkeKorisnik.BiljkaNavigation.Naziv));
            return View("FormaDodajMikroLokaciju");
        }
        public IActionResult DodajMikroLokaciju(Mikrolokacija ml)
        {
            ctx.Mikrolokacijas.Add(ml);
            ctx.SaveChanges();
            return RedirectToAction("MikrolokacijeIndex", new { id = ml.Lokacija });
        }
        public IActionResult UrediMikroLokacijuView(int id)
        {
            var query = ctx.TipUzgojas
                          .OrderBy(t => t.Naziv)
                          .Select(t => new { t.Naziv, t.TipUzgojaId })
                          .ToList();
            var query2 = ctx.BiljkeKorisniks
                          .Where(b => b.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                          .Select(b => new { b.BiljkaNavigation.Naziv, b.BiljkaKorisnikId })
                          .ToList();
            ViewBag.TipUzgoja = new SelectList(query, nameof(TipUzgoja.TipUzgojaId), nameof(TipUzgoja.Naziv));
            ViewBag.Biljka = new SelectList(query2, nameof(BiljkeKorisnik.BiljkaKorisnikId), nameof(BiljkeKorisnik.BiljkaNavigation.Naziv));
            ViewBag.mikrolokacijaId = id;
            var query3 = ctx.Mikrolokacijas
                        .Where(m => m.MikrolokacijaId == id)
                        .FirstOrDefault<Mikrolokacija>();
            ViewBag.LokacijaId = query3.Lokacija;
            return View("UrediMikroLokaciju", query3);
        }
        public IActionResult UrediML(Mikrolokacija ml)
        {
            /*var query = ctx.Mikrolokacijas
                       .Where(a => a.MikrolokacijaId == ml.MikrolokacijaId)
                       .FirstOrDefault<Mikrolokacija>();*/
            Mikrolokacija mledit = ctx.Mikrolokacijas.Find(ml.MikrolokacijaId);

            mledit.Površina = ml.Površina;
            ctx.SaveChanges();
            mledit.Biljka = ml.Biljka;
            ctx.SaveChanges();
            mledit.TipUzgoja = ml.TipUzgoja;
            ctx.SaveChanges();
            mledit.OčekivaniUrod = ml.OčekivaniUrod;
            ctx.SaveChanges();
            mledit.Opis = ml.Opis;
            ctx.SaveChanges();

            return RedirectToAction("MikrolokacijeIndex", new { id = ml.Lokacija });
        }
        public IActionResult IzbrisiML(int id)
        {
            /*
            var delUrod = ctx.Urods
                        .Where(u => u.Mikrolokacija == id)
                        .ToList();
            foreach(var u in delUrod)
            {
                ctx.Urods.Remove(u);
            }
            */

            var delMikrolokaciju = ctx.Mikrolokacijas
                    .Where(a => a.MikrolokacijaId == id)
                    .FirstOrDefault();
            ViewBag.LocationID = delMikrolokaciju.Lokacija;
            ctx.Mikrolokacijas.Remove(delMikrolokaciju);
            ctx.SaveChanges();
            return RedirectToAction("MikrolokacijeIndex", new { id = ViewBag.LocationID });
        }
    }
}
