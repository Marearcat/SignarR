using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Socket_MVC_Identity.Data;
using System.Web;

namespace Socket_MVC_Identity.Controllers
{
    public class RolesController : Controller
    {
        private ApplicationDbContext context;
        private UserManager<IdentityUser> _userManager;

        public RolesController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            context = new ApplicationDbContext(ApplicationDbContext.Opts());
        }

        public ActionResult Index()
        {
            return RedirectPermanent("~/home/index");
        }

        //[Authorize(Roles = "admin")]
        public ActionResult AddInChat()
        {
            var users = context.Users.Where(x => !_userManager.IsInRoleAsync(x, "chat").Result);
            var model = users.Select(x => x.UserName);
            return View(model);
        }
        [HttpPost]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult> AddInChat(string name)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(name);
                await _userManager.AddToRoleAsync(user, "admin");
                //context.UserRoles.Add(new IdentityUserRole<string> { RoleId = "72b51e3c-6c5e-4bb9-8361-86cb682fe118", UserId = user.Id });
                //context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult RemoveFromChat()
        {
            var users = context.Users.Where(x => _userManager.IsInRoleAsync(x, "chat").Result);
            var model = users.Select(x => x.UserName);
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult RemoveFromChat(string name)
        {
            try
            {
                var user = context.Users.First(x => x.UserName == name);
                _userManager.RemoveFromRoleAsync(user, "chat");
                //context.UserRoles.Remove(new IdentityUserRole<string> { RoleId = "72b51e3c-6c5e-4bb9-8361-86cb682fe118", UserId = user.Id });
                //context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}