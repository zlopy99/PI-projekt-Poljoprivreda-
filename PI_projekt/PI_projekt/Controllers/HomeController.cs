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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PI07Context ctx;
        //private readonly ILogger<SignUpController> logger;
        private readonly AppSettings appSettings;

        public HomeController(PI07Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            // this.logger = logger;
            appSettings = options.Value;
        }
        /* public HomeController(ILogger<HomeController> logger)
         {
             _logger = logger;
         }*/

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult ProvjeraPrijave(Korisnik korisnik)
        {
            if (korisnik.ImePrezime == null || korisnik.Lozinka == null)
            {
                ViewBag.error = "Oba polja moraju biti ispunjena";
                return View("Login");
            }
            else
            {
                var query = ctx.Korisniks
                                     .Where(k => k.ImePrezime == korisnik.ImePrezime)
                                     .Where(k => k.Lozinka == korisnik.Lozinka)
                                     .FirstOrDefault<Korisnik>();

                if (query != null)
                {
                    HttpContext.Session.SetString("imePrezimeLogiranogKorisnika", query.ImePrezime);
                    HttpContext.Session.SetInt32("idLogiranogKorisnika", query.KorisnikId);


                    return RedirectToAction("Profil");

                }
                else
                {
                    ViewBag.error = "Pogrešno korisničko ime ili lozinka";
                    return View("Login");
                }
            }
        }

        public IActionResult Profil()
        {
            if (HttpContext.Session.GetString("imePrezimeLogiranogKorisnika") != null)
            {
                var logiraniKorisnik = HttpContext.Session.GetInt32("idLogiranogKorisnika");
                var query = ctx.Korisniks
                            .Where(a => a.KorisnikId == logiraniKorisnik)
                            .FirstOrDefault<Korisnik>();

                return View(viewName: "Profil", query);
            }
            else
            {
                return Ok("Imamo nekih problema");
            }
        }
        public IActionResult Odjava()
        {
            HttpContext.Session.Remove("idLogiranogKorisnika");
            HttpContext.Session.Remove("imePrezimeLogiranogKorisnika");
            return RedirectToAction("Login");
        }
        /* [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
       public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}

/*
namespace PI_projekt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PI07Context ctx;
        //private readonly ILogger<SignUpController> logger;
        private readonly AppSettings appSettings;

        public HomeController(PI07Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            // this.logger = logger;
            appSettings = options.Value;
        }
       # public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }#

        public IActionResult Login()
        {
            return View();
        }
        
       public IActionResult ProvjeraPrijave(Korisnik korisnik)
        {
            if (korisnik.ImePrezime == null || korisnik.Lozinka == null)
            {
                ViewBag.error = "Oba polja moraju biti ispunjena";
                return View("Login");
            }
            else
            {
                var query = ctx.Korisniks
                                     .Where(k => k.ImePrezime == korisnik.ImePrezime)
                                     .Where(k => k.Lozinka == korisnik.Lozinka)
                                     .FirstOrDefault<Korisnik>();

                if (query != null)
                {
                    HttpContext.Session.SetString("imePrezimeLogiranogKorisnika", query.ImePrezime);
                    HttpContext.Session.SetInt32("idLogiranogKorisnika", query.KorisnikId);


                    return View("Profil");

                }
                else
                {
                    ViewBag.error = "Pogrešno korisničko ime ili lozinka";
                    return View("Login");
                }
            }
                

            }
       public IActionResult Odjava()
       {
             HttpContext.Session.Remove("idLogiranogKorisnika");
             HttpContext.Session.Remove("imePrezimeLogiranogKorisnika");
             return RedirectToAction("Login");
       }
        # [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
       public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }#
    }
}
*/