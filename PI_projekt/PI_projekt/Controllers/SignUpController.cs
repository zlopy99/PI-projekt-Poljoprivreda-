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
    public class SignUpController : Controller
    {
        private readonly PI07Context ctx;
        //private readonly ILogger<SignUpController> logger;
        private readonly AppSettings appSettings;

        public SignUpController(PI07Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            // this.logger = logger;
            appSettings = options.Value;
        }
        public IActionResult Signup()
        {
            return View(viewName: "SignUp");
        }
        public IActionResult Registracija(Korisnik korisnk)
        {
            if (korisnk.ImePrezime == null || korisnk.Email == null || korisnk.Lozinka == null)
            {
                ViewBag.Error = "Sva polja moraju biti unesena";
                return View(viewName: "SignUp");
            }
            else
            {
                if (korisnk.Lozinka.Length < 6)
                {
                    ViewBag.Error = "Lozinka mora sadržavati minimalno 6 znakova";
                    return View(viewName: "SignUp");
                }
                else
                {
                    ctx.Korisniks.Add(korisnk);
                    ctx.SaveChanges();
                    HttpContext.Session.SetString("imePrezimeLogiranogKorisnika", korisnk.ImePrezime);
                    HttpContext.Session.SetInt32("idLogiranogKorisnika", korisnk.KorisnikId);
                    return RedirectToAction("Profil", "Home");
                }
            }
        }

    }
}

/*
namespace PI_projekt.Controllers
{
    public class SignUpController : Controller
    {
        private readonly PI07Context ctx;
        //private readonly ILogger<SignUpController> logger;
        private readonly AppSettings appSettings;

        public SignUpController(PI07Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
           // this.logger = logger;
            appSettings = options.Value;
        }
        public IActionResult Signup()
        {
            return View(viewName: "SignUp"); 
        } 
        public IActionResult Registracija(Korisnik korisnk)
        {
            
            if(korisnk.ImePrezime == null || korisnk.Email == null || korisnk.Lozinka == null)
            {
                ViewBag.Error = "Sva polja moraju biti unesena";
                return View(viewName: "SignUp");
            }
            else
            {
                if (korisnk.Lozinka.Length < 6)
                {
                    ViewBag.Error = "Lozinka mora sadržavati minimalno 6 znakova";
                    return View(viewName: "SignUp");
                }
                else
                {
                    ctx.Korisniks.Add(korisnk);
                    ctx.SaveChanges();
                    return RedirectToAction("Login", "Home");
                }
            }
        }

    }
        }
*/

