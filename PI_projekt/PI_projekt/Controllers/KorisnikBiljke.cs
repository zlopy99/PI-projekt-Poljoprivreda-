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
    public class KorisnikBiljke : Controller
    {
        private readonly ILogger<TroskoviController> _logger;
        private readonly PI07Context ctx;
        //private readonly ILogger<SignUpController> logger;
        private readonly AppSettings appSettings;

        public KorisnikBiljke(PI07Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            // this.logger = logger;
            appSettings = options.Value;
        }
        public IActionResult KorisnikBiljkeIndex()
        {
            if (HttpContext.Session.GetString("imePrezimeLogiranogKorisnika") != null)
            {
                var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
                
                var query = ctx.BiljkeKorisniks
                        .Where(bk => bk.Korisnik == logiraniKorisnik)
                        .Select(bk => new BiljkaKorisnikViewModel
                        {
                            Naziv = bk.BiljkaNavigation.Naziv,
                            CijenaPoKg = bk.CijenaPoKg,
                            Opis = bk.Opis,
                            BiljkaKorisnikId=bk.BiljkaKorisnikId
                        })
                        .ToList();
                var model = new BKViewModel
                {
                    KorisnikoveBiljke = query
                };
                
                return View(viewName: "korisnikBiljke", model);
                //return Ok(list);
            }
            else
            {
                return Ok("Imamo nekih problema");
            }
            
        }
        public IActionResult formaZaEditKorisnikoveBiljke(int id)
        {
            var query = ctx.BiljkeKorisniks
                   .Where(a => a.BiljkaKorisnikId == id)
                   .Where(a => a.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                   .FirstOrDefault<BiljkeKorisnik>();
            ViewBag.Message = query.BiljkaKorisnikId;
            return View(viewName:"urediKorisnikoveBiljke", query);
        }
        public IActionResult korisnikBiljkaEdit(BiljkeKorisnik korBilj)
        {
            var query = ctx.BiljkeKorisniks
                       .Where(a => a.BiljkaKorisnikId == korBilj.BiljkaKorisnikId)
                       .Where(a => a.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                       .FirstOrDefault<BiljkeKorisnik>();

            BiljkeKorisnik kb = ctx.BiljkeKorisniks.Find(korBilj.BiljkaKorisnikId);
            /*
            if (korBilj.CijenaPoKg == null  && korBilj.Opis==null)
            {
                ViewBag.error = "Niste popunili nijedno polje";
                return View("urediKorisnikoveBiljke");
            }
            */
            kb.CijenaPoKg = korBilj.CijenaPoKg;
            ctx.SaveChanges();
            kb.Opis = korBilj.Opis;
            ctx.SaveChanges();
            kb.Korisnik = (int)HttpContext.Session.GetInt32("idLogiranogKorisnika");
            ctx.SaveChanges();
            return RedirectToAction("KorisnikBiljkeIndex", "KorisnikBiljke");

        }
        public IActionResult KorisnikBiljkaDetalji(int id)
        {
            var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");

            var query = ctx.BiljkeKorisniks
                    .Where(bk=>bk.BiljkaKorisnikId==id)
                    .Where(bk => bk.Korisnik == logiraniKorisnik)
                    .Select(bk => new BiljkaKorisnikModelZaDetalje
                    {
                        Naziv = bk.BiljkaNavigation.Naziv,
                        LatinskiNaziv = bk.BiljkaNavigation.Vrsta,
                        Red = bk.BiljkaNavigation.RedNavigation.Naziv,
                        Razred = bk.BiljkaNavigation.RazredNavigation.Naziv,
                        Porodica=bk.BiljkaNavigation.PorodicaNavigation.Naziv,
                        Rod = bk.BiljkaNavigation.RodNavigation.Naziv,
                        Opis = bk.BiljkaNavigation.Opis,
                        BiljkaKorisnikId = id,
                        BiljkaID=bk.BiljkaNavigation.BiljkaId
                    })
                    .ToList();
            var model = new BiljKorDetModel
            {
                DetaljiOKorisnikovojBiljci = query
            };
            ViewBag.Message = id;
            return View("DetaljiKorisnikBiljka",model);
        }
        /*public IActionResult FazaRazvojaIndex(int id)
        {

        }*/
       /* public IActionResult UpotrebaBiljkeIndex(int id)
        {

        }*/
       public IActionResult UrediOpisKodDetalja(int id) {
            var query = ctx.Biljkas
                   .Where(bi => bi.BiljkaId == id)
                   .FirstOrDefault<Biljka>();
            ViewBag.Message = id;
           // TempData["id"] = query.BiljkaId;
            return View("formaZaEditOpisaDetalja");
        }
       //public IActionResult OpisEdit(KorisnikBiljke) { }
       public IActionResult IzbrisiKorisnikovuBiljku(int id)
        {
            var query = ctx.BiljkeKorisniks
                      .Where(a => a.BiljkaKorisnikId == id)
                      .Where(a => a.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                      .FirstOrDefault<BiljkeKorisnik>();
            ctx.BiljkeKorisniks.Remove(query);
            ctx.SaveChanges();
            return RedirectToAction("KorisnikBiljkeIndex", "KorisnikBiljke");
        }

        public IActionResult FormaZaDodavanjeKorisnikoveBiljke()
        {
            var query = ctx.BiljkeKorisniks
                        .Where(bk => bk.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                        .ToList();

            var query2 = ctx.Biljkas
                        .ToList();

            //var queryForSelect = new Biljka[query2.Count];
            var ponovljeni = new string[query.Count];
            for(int i = 0; i < query.Count; i++)
            {
                foreach(var qu in query2)
                {
                    if(query.ElementAt(i).BiljkaNavigation.Naziv == qu.Naziv)
                    {
                        ponovljeni[i] = qu.Naziv;
                    }
                }
            }

            var queryForSelect = ctx.Biljkas
                                .Where(b => !ponovljeni.Contains(b.Naziv))
                                .ToList();

            ViewBag.Biljka = new SelectList(queryForSelect, nameof(Biljka.BiljkaId), nameof(Biljka.Naziv));

            return View("DodajNovuKorisnikBiljku");
        }
        public IActionResult DodajKorisnikBiljku(BiljkeKorisnik bk)
        {
            bk.Korisnik = (int)HttpContext.Session.GetInt32("idLogiranogKorisnika");
            ctx.BiljkeKorisniks.Add(bk);
            ctx.SaveChanges();
            return RedirectToAction("KorisnikBiljkeIndex","KorisnikBiljke");
        }
        /*public IActionResult ProvjeriBiljku()
        {
            var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
            var query = ctx.BiljkeKorisniks
                        .Where(b => b.Korisnik == logiraniKorisnik)
                        .Select(b => new { ID = b.BiljkaKorisnikId, Naziv = b.BiljkaNavigation.Naziv })
                        .ToList();
            return View("IDbiljke",query);
                    
        }*/
    }

    
}
