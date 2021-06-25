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
    public class UpotrebaController : Controller
    {
        private readonly ILogger<TroskoviController> _logger;
        private readonly PI07Context ctx;
        //private readonly ILogger<SignUpController> logger;
        private readonly AppSettings appSettings;

        public UpotrebaController(PI07Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            // this.logger = logger;
            appSettings = options.Value;
        }
        public IActionResult UpotrebaIndex(int id)
        {
            var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
            /*var imeBiljke = ctx.BiljkeKorisniks
                        .Where(i => i.BiljkaKorisnikId == id)
                        .Select(i => new
                        {
                            naziv = i.BiljkaNavigation.Naziv
                        });*/

            var query = ctx.BiljkaUpotrebas
                    .Where(bu => bu.BiljkaNavigation.BiljkaId == id)
                    .Select(bu => new UpotrebaViewModel
                    {
                        Naziv = bu.UpotrebaNavigation.Naziv,
                        Opis = bu.Opis,
                        BiljkaKorisnikId = id,
                        BiljkaUpotrebaId = bu.BiljkaUpotrebaId
                    })
                    .ToList();
            var model = new BiljUpotrebaVM
            {
                UpotrebaBiljke = query
            };
            var idUpotrebe = ctx.BiljkeKorisniks
                    .Where(bk => bk.Biljka == id)
                    .Where(bk => bk.Korisnik == logiraniKorisnik)
                    .Select(bk => bk.BiljkaKorisnikId);
            foreach (var item in idUpotrebe)
            {
                ViewBag.id = item;
            }
            ViewBag.IDe = id;
            return View("UpotrebaBiljka", model);
        }
        public IActionResult PrikazFormeZaDodavanjeNoveUpotrebe(int id)
        {
            var query = ctx.BiljkaUpotrebas
                     .Where(bu => bu.BiljkaNavigation.BiljkaId == id)
                         .Select(bu => new UpotrebaViewModel
                         {
                             Naziv = bu.UpotrebaNavigation.Naziv,
                             Opis = bu.Opis,
                             BiljkaKorisnikId = id,
                             BiljkaUpotrebaId = bu.BiljkaUpotrebaId
                         })
                         .ToList();
            var model = new BiljUpotrebaVM
            {
                UpotrebaBiljke = query
            };
            var queryForSelect = ctx.Upotrebas
                        .ToList();
            ViewBag.IDe = id;
            ViewBag.Upotreba = new SelectList(queryForSelect, nameof(Upotreba.UpotrebaId), nameof(Upotreba.Naziv));
            return View("dodavanjeNoveUpotrebe");
        }
        public IActionResult DodajNovuUpotrebu(BiljkaUpotreba bu)
        {
            
            ctx.BiljkaUpotrebas.Add(bu);
            ctx.SaveChanges();
            //return RedirectToAction("UpotrebaIndex","Upotreba");
            return RedirectToAction("UpotrebaIndex", new { id = bu.Biljka });
        }
        public IActionResult FormaZaCraeteUpotrebu(int id)
        {
            ViewBag.IDe = id;
            return View("FormaZaUpotrebuCreate");
            //return Ok(ViewBag.id);
        }
        public IActionResult DodajUpotrebu(Upotreba upotreba)
        {
            var pom = ctx.Upotrebas;
            var istina = true;
            foreach(var p in pom)
            {
                if (p.Naziv == upotreba.Naziv)
                {
                    istina = false;
                    ViewBag.Message = "Upotreba već postoji.";
                    break;
                }
            }
            ViewBag.IDe = upotreba.UpotrebaId;
            // var ID = upotreba.para;
            if (upotreba.Naziv != null && istina)
            {
                upotreba.UpotrebaId = 0;
                ctx.Upotrebas.Add(upotreba);
                ctx.SaveChanges();
                ViewBag.Message = "Upotreba je uspjesno dodana!";

                // return RedirectToAction("DodajNovuUpotrebu", new { id = ID });
                return View("FormaZaUpotrebuCreate", upotreba);//new { id = ViewBag.IDe }
            }
            else
            {
                return View("FormaZaUpotrebuCreate", upotreba);
                //return View("FormaZaUpotrebuCreate");
            }
            
        }

        public IActionResult Izbrisi(int id)
        {
            var pomQuery = ctx.BiljkaUpotrebas;
            foreach(var p in pomQuery)
            {
                if (p.BiljkaUpotrebaId == id)
                    ViewBag.IDe = p.Biljka;
            }

            var query = ctx.BiljkaUpotrebas
                        .Where(b => b.BiljkaUpotrebaId == id)
                        .FirstOrDefault<BiljkaUpotreba>();
            ctx.BiljkaUpotrebas.Remove(query);
            ctx.SaveChanges();
            return RedirectToAction("UpotrebaIndex", new { id = ViewBag.IDe });
        }

        [HttpGet]
        public IActionResult Uredi(int id)
        {
            var query = ctx.BiljkaUpotrebas
                        .Where(bu => bu.BiljkaUpotrebaId == id)
                        .FirstOrDefault<BiljkaUpotreba>();
            ViewBag.IDe = query.Biljka;

            var queryForSelect = ctx.Upotrebas.ToList();
            ViewBag.Upotreba = new SelectList(queryForSelect, nameof(Upotreba.UpotrebaId), nameof(Upotreba.Naziv));

            return View("Uredi", query);
        }
        [HttpPost]
        public IActionResult Uredi(BiljkaUpotreba bu)
        {
            var buEdit = ctx.BiljkaUpotrebas.Find(bu.BiljkaUpotrebaId);
            buEdit.Upotreba = bu.Upotreba;
            ctx.SaveChanges();
            buEdit.Opis = bu.Opis;
            ctx.SaveChanges();

            return RedirectToAction("UpotrebaIndex", new { id = bu.Biljka });
        }
    }
}
