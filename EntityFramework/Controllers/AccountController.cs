using EntityFramework.Models;
using EntityFramework.Services;
using EntityFramework.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace EntityFramework.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        public AccountController(IEmailService emailService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            AppUser appUser = new AppUser()
            {
                FullName = registerVM.FullName,
                UserName = registerVM.UserName,
                Email = registerVM.Email
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                return View(registerVM);
            }

            //await _signInManager.SignInAsync(appUser, false);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

            var link = Url.Action(nameof(VerifyEmail), "Account", new { userId = appUser.Id, token = code },Request.Scheme,Request.Host.ToString());

            await _emailService.SendEmai(appUser.Email, link);

            return RedirectToAction(nameof(EmailVerification));
        }

        public IActionResult EmailVerification()
        {
            return View();
        }

        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            if (userId is null || token is null) return BadRequest();

            AppUser user = await _userManager.FindByIdAsync(userId);

            if (user is null) return BadRequest();

            await _userManager.ConfirmEmailAsync(user, token);

            await _signInManager.SignInAsync(user,false);

            return RedirectToAction(nameof(Index),"Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            AppUser user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);

            if(user == null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
            }

            if(user is null)
            {
                ModelState.AddModelError("", "Email or Password is wrong");
                return View(loginVM);
            }

            if (!user.IsActiveted)
            {
                ModelState.AddModelError("", "Please Contact With Admin");
                return View(loginVM);
            }

            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Please Confirm Your Account");
                    return View(loginVM);
                }
                ModelState.AddModelError("", "Email or Password is wrong");
                return View(loginVM);
            }

            return RedirectToAction("Index","Home");
        }

        

        

    }
}
