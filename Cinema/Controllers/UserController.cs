using Cinema.Domain.Domain_models;
using Cinema.Domain.DTO;
using Cinema.Domain.Identity;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Web.Controllers
{
    [Authorize(Roles = "ADMINISTRATOR")]
    public class UserController : Controller
    {
        private readonly UserManager<ShopApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<ShopApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            this._userManager = _userManager;
            this._roleManager = _roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ImportUsers()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ImportUsers(IFormFile file)
        {
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);

                fileStream.Flush();
            }

            List<RegistrationDTO> users = getUsersFromExcelFile(file.FileName);

            foreach (var item in users)
            {
                var userCheck = _userManager.FindByEmailAsync(item.Email).Result;
                if (userCheck == null)
                {
                    var user = new ShopApplicationUser
                    {
                        UserName = item.Email,
                        NormalizedUserName = item.Email,
                        Email = item.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        UserShoppingCart = new ShoppingCart()
                    };

                    var roleExist = await _roleManager.RoleExistsAsync(item.Role);
                    if (!roleExist)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(item.Role));
                    }

                    var result = _userManager.CreateAsync(user, item.Password).Result;
                    await _userManager.AddToRoleAsync(user, item.Role);
                }
                else
                {
                    continue;
                }
            }

            return RedirectToAction("Index", "Order");
        }

        private List<RegistrationDTO> getUsersFromExcelFile(string fileName)
        {

            string pathToFile = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            List<RegistrationDTO> userList = new List<RegistrationDTO>();

            using (var stream = System.IO.File.Open(pathToFile, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        userList.Add(new RegistrationDTO
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            Role = reader.GetValue(2).ToString()

                        });
                    }
                }
            }

            return userList;

        }
    }
        
}

