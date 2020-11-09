using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rauan.DbData;
using Rauan.ViewModels;

namespace Rauan.Controllers
{
    public class AccountController : Controller
    {

        private readonly AppDbContext db;
        private SignInManager<User> sM;
        private UserManager<User> uM;

        public AccountController(SignInManager<User> _sM, AppDbContext _db, UserManager<User> _uM)
        {
            db = _db;
            sM = _sM;
            uM = _uM;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginModel { ReturnUrl = returnUrl });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await uM.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await sM.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        // проверяем, принадлежит ли URL приложению
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Такого пользователя не существует");
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, PhoneNumber = model.PhoneNumber, UserName = model.Email };
                // добавляем пользователя
                var result = await uM.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await sM.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }


        public IActionResult ForgetPassword()
        {
            return View();
        }



        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await sM.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
