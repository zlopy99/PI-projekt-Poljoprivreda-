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

    public class ProdajaController : Controller
    {
        private readonly ILogger<ProdajaController> _logger;
        private readonly PI07Context ctx;
        //private readonly ILogger<SignUpController> logger;
        private readonly AppSettings appSettings;

        public ProdajaController(PI07Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            // this.logger = logger;
            appSettings = options.Value;
        }
        public IActionResult NarudzbeIndex()
        {
            var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
            var query = ctx.Narudžbas
                .Where(n => n.Korisnik == logiraniKorisnik)
                .Select(n => new NarudzbaViewModel
                {
                    KorisnikId = (int)logiraniKorisnik,
                    NarudzbaId = n.NaruzdbaId,
                    KupacId = n.Kupac,
                    ImeKupca = n.KupacNavigation.ImePrezime,
                    datumNarudzbe = (DateTime)n.DatumNarudzbe,
                    datumIsporuke = (DateTime)n.DatumIsporuke
                })
                .ToList();
            var model = new NarudzbaVM
            {
                narudzba = query
            };
            return View("NarudzbeView", model);
        }
        public IActionResult DetaljiIndex(int id)
        {
            var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
            var query = ctx.DetaljiNarudžbes
                    .Where(dn => dn.Narudzba == id)
                    .Select(dn => new DetaljiNarudzbeViewModel
                    {
                        detaljiNarudzbeId = dn.DetaljiNarudzbeId,
                        NarudzbaId = id,
                        BiljkaId = (int)dn.Biljka,
                        NazivBiljke = dn.BiljkaNavigation.BiljkaNavigation.Naziv,
                        Cijena = dn.UkupnaCijena,
                        Kolicina = dn.kolicina,
                        KontaktKupca = dn.NarudzbaNavigation.KupacNavigation.Kontkat,
                        AdresaKupca = dn.NarudzbaNavigation.KupacNavigation.Adresa,
                        Opis = dn.Opis
                    })
                    .ToList();
            var model = new DetaljiNarudzbeVM
            {
                detalji = query
            };
            ViewBag.NarudzbaID = id;
            return View("DetaljiNarudzbaView", model);
        }
        public IActionResult DodajNaruzbuView()
        {
            var queryForSelect = ctx.Kupacs
                        .ToList();
            ViewBag.Kupac = new SelectList(queryForSelect, nameof(Kupac.KupacId), nameof(Kupac.ImePrezime));
            return View("DodajNovuNarudzbu");
        }
        public IActionResult DodajNarudzbu(Narudžba n)
        {
            if (n.Kupac != 0 && n.DatumNarudzbe != null && n.DatumIsporuke != null)
            {
                var query = ctx.Narudžbas
                            .Where(n => n.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                            .ToList();

                n.Korisnik = (int)HttpContext.Session.GetInt32("idLogiranogKorisnika");
                ctx.Narudžbas.Add(n);
                ctx.SaveChanges();
                //ViewBag.porukaNarudzba = "Narudžba je uspjesno dodana!";

                var query2 = ctx.Narudžbas
                            .Where(n => n.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                            .ToList();

                foreach(var item2 in query2)
                {
                    var br = 0;
                    foreach(var item in query)
                    {
                        if(item2.NaruzdbaId == item.NaruzdbaId)
                        {
                            br++;
                        }
                    }
                    if(br == 0)
                    {
                        ViewBag.NarudzbaID = item2.NaruzdbaId;
                        break;
                    }
                }

                return RedirectToAction("DodajDetaljeNarudzbeView", new { id = ViewBag.NarudzbaID });
            }
            else
            {
                var queryForSelect = ctx.Kupacs
                        .ToList();
                ViewBag.Kupac = new SelectList(queryForSelect, nameof(Kupac.KupacId), nameof(Kupac.ImePrezime));

                return View("DodajNovuNarudzbu", n);
            }
        }
        public IActionResult DodajDetaljeNarudzbeView(int id)
        {
            var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
            ViewBag.NarudzbaID = id;

            var query2 = ctx.BiljkeKorisniks
                    .Where(b => b.Korisnik == logiraniKorisnik)
                    //.OrderBy(b => b.BiljkaKorisnikId)
                    // .Select(b=>b.BiljkaKorisnikId))//new {ID = b.BiljkaKorisnikId, Naziv = b.BiljkaNavigation.Naziv })
                    //.Select(b => b.BiljkaKorisnikId)
                    .Select(b => new { b.BiljkaNavigation.Naziv, b.BiljkaKorisnikId })
                    .ToList();
            ViewBag.Biljka = new SelectList(query2, nameof(BiljkeKorisnik.BiljkaKorisnikId), nameof(BiljkeKorisnik.BiljkaNavigation.Naziv));

            return View("DodajDetaljeView");
        }
        /*
        public IActionResult AkoStanemoNaPola(int id)
        {
            var query = ctx.Narudžbas
                        .Where(n => n.NaruzdbaId == id)
                        .FirstOrDefault();

            ctx.Remove(query);
            ctx.SaveChanges();
            return RedirectToAction("NarudzbeIndex");
        }
        */
        public IActionResult DodajDetalje(DetaljiNarudžbe dm)
        {
              if(dm.Biljka== null && dm.kolicina== null && dm.UkupnaCijena==null && dm.Opis == null)
              {
                var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
                ViewBag.NarudzbaID = dm.Narudzba;

                var query2 = ctx.BiljkeKorisniks
                        .Where(b => b.Korisnik == logiraniKorisnik)
                        //.Select(b => b.BiljkaKorisnikId)
                        .Select(b => new { b.BiljkaNavigation.Naziv, b.BiljkaKorisnikId })
                        .ToList();
                ViewBag.Biljka = new SelectList(query2, nameof(BiljkeKorisnik.BiljkaKorisnikId), nameof(BiljkeKorisnik.BiljkaNavigation.Naziv));

                ViewBag.mess = "Niste popunili nijedno polje";
                return View("DodajDetaljeView");
              }
              else if(dm.Biljka == null || dm.UkupnaCijena == null)
              {
                var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
                ViewBag.NarudzbaID = dm.Narudzba;

                var query2 = ctx.BiljkeKorisniks
                        .Where(b => b.Korisnik == logiraniKorisnik)
                        //.Select(b => b.BiljkaKorisnikId)
                        .Select(b => new { b.BiljkaNavigation.Naziv, b.BiljkaKorisnikId })
                        .ToList();
                ViewBag.Biljka = new SelectList(query2, nameof(BiljkeKorisnik.BiljkaKorisnikId), nameof(BiljkeKorisnik.BiljkaNavigation.Naziv));

                ViewBag.mess = "Izabrati biljku obavezno!";
                return View("DodajDetaljeView");
              }
              else
              {
                    ctx.DetaljiNarudžbes.Add(dm);
                    ctx.SaveChanges();
                    //return RedirectToAction("DetaljiIndex",new { id = dm.Narudzba });
                    return RedirectToAction("NarudzbeIndex");
              }
        }
        public IActionResult IzbrisiNarudzbu(int id)
        {
            var query = ctx.Narudžbas
                    .Where(n => n.NaruzdbaId == id)
                    .FirstOrDefault<Narudžba>();
            ctx.Narudžbas.Remove(query);
            ctx.SaveChanges();
            return RedirectToAction("NarudzbeIndex", "Prodaja");
        }
        public IActionResult IzbrisiDetalje(int id)
        {
            var query = ctx.DetaljiNarudžbes
                    .Where(n => n.DetaljiNarudzbeId == id)
                    .FirstOrDefault<DetaljiNarudžbe>();
            ctx.DetaljiNarudžbes.Remove(query);
            ctx.SaveChanges();
            return RedirectToAction("DetaljiIndex", "Prodaja");
        }
         public IActionResult OstvareneNarudzbeIndex()
         {
            
            var query = ctx.OstvareneNarudzbes
               .Where(on => on.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
               .Select(on => new OstvareneNarudzbeViewModel
               {
                   Biljka = on.Biljka,
                   OstvareneNarudzbeId = on.OstvareneNarudzbeId,
                   NazivBiljke=on.BiljkeKorisnikNavigation.BiljkaNavigation.Naziv,
                   ImeKupca=on.KupacNavigation.ImePrezime,
                   KontaktKupca=on.KupacNavigation.Kontkat,
                   Datum = on.Datum,
                   Kupac = on.Kupac,
                   Zarada = on.Zarada,
                   Opis = on.Opis,
                   Korisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika"),
                   Kolicina=on.Kolicina
               });

            /*var query = ctx.OstvareneNarudzbes
                .Where(on => on.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                .ToList();*/
            var model = new OstvareneNarudzbeVM
            {
                ostvareneNarudzbe = query
            };

            return View("OstvareneNarudzbeView", model);
         }
         public IActionResult Prodano(int id)
         {
            var query = ctx.Narudžbas
                    .Where(n => n.NaruzdbaId == id)
                    .Where(n => n.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                    .FirstOrDefault<Narudžba>();
            var query2 = ctx.DetaljiNarudžbes
                   .Where(d => d.Narudzba == id)
                   .ToList();
           
           
            if(query2.Count > 0)
            {
                foreach (var item in query2)
                {
                    //var on = ctx.OstvareneNarudzbes.Find(query.Korisnik);
                    OstvareneNarudzbe on = new OstvareneNarudzbe();
                    on.Datum = query.DatumIsporuke;
                    on.Kupac = query.Kupac;
                    on.Korisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
                    on.Kolicina = item.kolicina;
                    on.Zarada = item.UkupnaCijena;
                    on.Biljka = item.Biljka;
                    on.Opis = item.Opis;
                    ctx.Add(on);
                    ctx.SaveChanges();
                }
            }

            if(query2.Count == 0)
            {
                /*
                OstvareneNarudzbe on = new OstvareneNarudzbe();
                on.Datum = query.DatumIsporuke;
                on.Kupac = query.Kupac;
                on.Korisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
                ctx.Add(on);
                ctx.SaveChanges();
                */
                var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
                var query3 = ctx.Narudžbas
                    .Where(n => n.Korisnik == logiraniKorisnik)
                    .Select(n => new NarudzbaViewModel
                    {
                        KorisnikId = (int)logiraniKorisnik,
                        NarudzbaId = n.NaruzdbaId,
                        KupacId = n.Kupac,
                        ImeKupca = n.KupacNavigation.ImePrezime,
                        datumNarudzbe = (DateTime)n.DatumNarudzbe,
                        datumIsporuke = (DateTime)n.DatumIsporuke
                    })
                    .ToList();
                var model = new NarudzbaVM
                {
                    narudzba = query3
                };
                ViewBag.msg = "Prilikom kreiranja Narudžbe niste stigli popuniti sve detalje. Molimo vas popunite Detalje Narudžbe.(Biljka i Cijena)";
                return View("NarudzbeView", model);
            }
            else
            {
                ctx.Narudžbas.Remove(query);
                ctx.SaveChanges();

                var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
                var query3 = ctx.Narudžbas
                    .Where(n => n.Korisnik == logiraniKorisnik)
                    .Select(n => new NarudzbaViewModel
                    {
                        KorisnikId = (int)logiraniKorisnik,
                        NarudzbaId = n.NaruzdbaId,
                        KupacId = n.Kupac,
                        ImeKupca = n.KupacNavigation.ImePrezime,
                        datumNarudzbe = (DateTime)n.DatumNarudzbe,
                        datumIsporuke = (DateTime)n.DatumIsporuke
                    })
                    .ToList();
                var model = new NarudzbaVM
                {
                    narudzba = query3
                };
                ViewBag.msg = "Uspiješno prodano!";
                return View("NarudzbeView", model);
            }
            
        }
        public IActionResult DodajKupcaView()
        {
            return View("DodajKupca");
        }
        public IActionResult DodajKupcaAkcija(Kupac k)
        {
            if(k.ImePrezime == null || k.Kontkat == null || k.Adresa == null)
            {
                return View("DodajKupca", k);
            }
            else
            {
                ctx.Kupacs.Add(k);
                ctx.SaveChanges();
               // ViewBag.poruka = "Kupac je uspjesno dodan!";
                return RedirectToAction("DodajKupcaView");
            }
        }
        public IActionResult KupciView()
        {
            var query = ctx.Kupacs
                .ToList();
            return View("ListaKupaca", query);
        }
        public IActionResult IzbrisiKupca(int id)
        {
            var query = ctx.Kupacs
                        .Where(q => q.KupacId == id)
                        .FirstOrDefault();

            ctx.Remove(query);
            ctx.SaveChanges();

            return RedirectToAction("KupciView");
        }
        [HttpGet]
        public IActionResult EditKupac(int id)
        {
            var query = ctx.Kupacs
                        .Where(k => k.KupacId == id)
                        .FirstOrDefault();
            ViewBag.KupacID = id;
            return View("EditKupac",query);
        }
        [HttpPost]
        public IActionResult EditKupac(Kupac ku)
        {
            ViewBag.KupacID = ku.KupacId;
            var query = ctx.Kupacs
                        .Where(k => k.KupacId == ku.KupacId)
                        .FirstOrDefault();
            if (ku.ImePrezime != null && ku.Kontkat != null && ku.Adresa != null)
            {
                var kuEdit = ctx.Kupacs.Find(ku.KupacId);
                kuEdit.ImePrezime = ku.ImePrezime;
                ctx.SaveChanges();
                kuEdit.Kontkat = ku.Kontkat;
                ctx.SaveChanges();
                kuEdit.Adresa = ku.Adresa;
                ctx.SaveChanges();

                return RedirectToAction("KupciView");
            }
            else
            {
                return View("EditKupac", query);
            }
        }

        [HttpGet]
        public IActionResult EditDetalje(int id)
        {
            var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");

            var query2 = ctx.BiljkeKorisniks
                    .Where(b => b.Korisnik == logiraniKorisnik)
                    //.Select(b => b.BiljkaKorisnikId)
                    .Select(b => new { b.BiljkaNavigation.Naziv, b.BiljkaKorisnikId })
                    .ToList();
            ViewBag.Biljka = new SelectList(query2, nameof(BiljkeKorisnik.BiljkaKorisnikId), nameof(BiljkeKorisnik.BiljkaNavigation.Naziv));
            //var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
            var query = ctx.DetaljiNarudžbes
                        .Where(dn => dn.DetaljiNarudzbeId == id)
                        .FirstOrDefault<DetaljiNarudžbe>();
            ViewBag.NarudzbaID = query.Narudzba;
            ViewBag.DetaljiNID = query.DetaljiNarudzbeId;

            return View("EditDetalje", query);
        }
        [HttpPost]
        public IActionResult EditDetalje(DetaljiNarudžbe dn)
        {
            ViewBag.DetaljiNID = dn.DetaljiNarudzbeId;
            ViewBag.NarudzbaID = dn.Narudzba;

            if (dn.UkupnaCijena == null || dn.Biljka == null)
            {
                var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");

                var query2 = ctx.BiljkeKorisniks
                        .Where(b => b.Korisnik == logiraniKorisnik)
                        //.Select(b => b.BiljkaKorisnikId)
                        .Select(b => new { b.BiljkaNavigation.Naziv, b.BiljkaKorisnikId })
                        .ToList();
                ViewBag.Biljka = new SelectList(query2, nameof(BiljkeKorisnik.BiljkaKorisnikId), nameof(BiljkeKorisnik.BiljkaNavigation.Naziv));

                @ViewBag.mess = "Polja Biljka i Cijena ne mogu biti prazna";
                return View("EditDetalje", dn);
            }
            else
            {
                var dnEdit = ctx.DetaljiNarudžbes.Find(dn.DetaljiNarudzbeId);
                dnEdit.Biljka = dn.Biljka;
                ctx.SaveChanges();
                dnEdit.kolicina = dn.kolicina;
                ctx.SaveChanges();
                dnEdit.UkupnaCijena = dn.UkupnaCijena;
                ctx.SaveChanges();
                dnEdit.Opis = dn.Opis;
                ctx.SaveChanges();

                return RedirectToAction("DetaljiIndex", new { id = ViewBag.NarudzbaID });
            }
        }
        public IActionResult Izbrisi(int id)
        {
            var query = ctx.OstvareneNarudzbes
                        .Where(on => on.OstvareneNarudzbeId == id)
                        .FirstOrDefault();

            ctx.Remove(query);
            ctx.SaveChanges();

            return RedirectToAction("OstvareneNarudzbeIndex");
        }
        [HttpGet]
        public IActionResult EditON(int id)
        {
            var queryForSelect = ctx.Kupacs
                        .ToList();
            ViewBag.Kupac = new SelectList(queryForSelect, nameof(Kupac.KupacId), nameof(Kupac.ImePrezime));

            var queryForSelect2 = ctx.BiljkeKorisniks
                                .Where(bk => bk.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                                .Select(bk => new { bk.BiljkaNavigation.Naziv, bk.BiljkaKorisnikId })
                                .ToList();
            ViewBag.BiljkaID = new SelectList(queryForSelect2, nameof(BiljkeKorisnik.BiljkaKorisnikId), nameof(BiljkeKorisnik.BiljkaNavigation.Naziv));

            var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
            var query = ctx.OstvareneNarudzbes
                        .Where(on => on.OstvareneNarudzbeId == id)
                        .Where(on => on.Korisnik == logiraniKorisnik)
                        .FirstOrDefault();
            ViewBag.ID = id;
            //ViewBag.BiljkaID = query.Biljka;
            ViewBag.KorisnikID = query.Korisnik;

            return View("EditON", query);
        }
        
        [HttpPost]
        public IActionResult EditON(OstvareneNarudzbe OS)
        {
            var queryForSelect = ctx.Kupacs
                        .ToList();
            ViewBag.Kupac = new SelectList(queryForSelect, nameof(Kupac.KupacId), nameof(Kupac.ImePrezime));

            var queryForSelect2 = ctx.BiljkeKorisniks
                                .Where(bk => bk.Korisnik == HttpContext.Session.GetInt32("idLogiranogKorisnika"))
                                .Select(bk => new { bk.BiljkaNavigation.Naziv, bk.BiljkaKorisnikId })
                                .ToList();
            ViewBag.BiljkaID = new SelectList(queryForSelect2, nameof(BiljkeKorisnik.BiljkaKorisnikId), nameof(BiljkeKorisnik.BiljkaNavigation.Naziv));

            var query = ctx.OstvareneNarudzbes
                        .Where(os => os.OstvareneNarudzbeId == OS.OstvareneNarudzbeId)
                        .FirstOrDefault();
            ViewBag.ID = OS.OstvareneNarudzbeId;
            //ViewBag.BiljkaID = OS.Biljka;
            ViewBag.KorisnikID = OS.Korisnik;
            
            if(OS.Datum != null && OS.Zarada != null)
            {
                var OSEdit = ctx.OstvareneNarudzbes.Find(OS.OstvareneNarudzbeId);
                OSEdit.Datum = OS.Datum;
                ctx.SaveChanges();
                OSEdit.Zarada = OS.Zarada;
                ctx.SaveChanges();
                OSEdit.Kupac = OS.Kupac;
                ctx.SaveChanges();
                OSEdit.Opis = OS.Opis;
                ctx.SaveChanges();
                OSEdit.Kolicina = OS.Kolicina;
                ctx.SaveChanges();
                OSEdit.Biljka = OS.Biljka;
                ctx.SaveChanges();

                return RedirectToAction("OstvareneNarudzbeIndex");
            }
            else
            {
                return View("EditON", query);
            }

        }
    }
}