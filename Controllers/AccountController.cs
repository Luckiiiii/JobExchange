using JobExchange.Data;
using JobExchange.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobExchange.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<StoreUser> _signInManger;
        private readonly UserManager<StoreUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IJobExchangeRepository _repository;

        public AccountController(ILogger<AccountController> logger, SignInManager<StoreUser> signInManager, UserManager<StoreUser> userManager, IConfiguration config, IJobExchangeRepository repository)
        {
            _logger = logger;
            _signInManger = signInManager;
            _userManager = userManager;
            _config = config;
            _repository = repository;
        }
        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ShowJobInfo", "App");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManger.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    if (user != null)
                    {
                        var hasEmployer = await _repository.HasEmployer(user.Id);
                        if (Request.Query.Keys.Contains("ReturnUrl"))
                        {
                            return Redirect(Request.Query["ReturnUrl"].First());
                        }
                        else
                        {
                            if (hasEmployer)
                            {
                                /* return RedirectToAction("Index", "App");*/
                                return RedirectToAction("ShowJobInfoUser", "App");
                            }
                            else
                            {
                                return RedirectToAction("EmployerView", "App");
                            }
                        }
                    }
                }
            }
            ModelState.AddModelError("", "Failed to login");
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new StoreUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManger.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("index", "App");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManger.SignOutAsync();
            return RedirectToAction("ShowJobInfo", "App");
        }
    }
}
