using Cinema.Domain.Domain_models;
using Cinema.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Web.Controllers
{
    [Authorize(Roles = "ADMINISTRATOR")]
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ShopApplicationUser> _userManager;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<ShopApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public ActionResult AddRole()
        {
            AddToRoleModel model = new AddToRoleModel();
            List<ShopApplicationUser> users = _userManager.Users.ToList();
            model.Emails = new List<string>();
            foreach (ShopApplicationUser user in users)
            {
                model.Emails.Add(user.Email);
            }
            model.Roles = new List<string>() {"ADMINISTRATOR","USER" };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddRole(AddToRoleModel model)
        {
            var user = _userManager.Users.Where(x => x.Email.Equals(model.Email)).FirstOrDefault();

            string[] roleNames = { "ADMINISTRATOR", "USER" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var role = await _roleManager.FindByNameAsync(model.SelectedRole);
            await _userManager.AddToRoleAsync(user, role.Name);
            return RedirectToAction("Index", "Tickets");
        }
    }
}
