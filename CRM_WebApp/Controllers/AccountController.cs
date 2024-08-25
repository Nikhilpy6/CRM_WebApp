using CRM_WebApp.Data;
using CRM_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace CRM_WebApp.Controllers
{
    public class AccountController : Controller
    {
     
        private readonly ApplicationDbContext context;

        public AccountController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Signup()
        {

          return View();
        }

        [HttpPost]
        public async Task<ActionResult> Signup(string email, string password,string username,DateTime createdDate)

        {

            try

            {

                if (ModelState.IsValid)

                {

                    // Check if the username already exists

                    if (await context.Users.AnyAsync(u => u.Email == email))

                    {

                        ViewBag.ErrorMessage = "Username already exists.";

                        return View();

                    }

                    // Hash the password

                    //  string hashedPassword = HashPassword(password);

                    // Create a new user and save to the database

                    var newUser = new User
                    {
                        Email = email,
                        Password = password,
                        Username = username,
                        CreatedDate = createdDate

                    };

                    context.Users.Add(newUser);

                    await context.SaveChangesAsync();

                    return RedirectToAction("LogIn");

                }

                return View();

            }

            catch (DbUpdateException dbEx)

            {

                // Handle database-related exceptions

                // Log the exception (use a logging framework)

                // _logger.LogError(dbEx, "A database error occurred during signup.");

                ViewBag.ErrorMessage = "A database error occurred. Please try again later.";

                return View();

            }

            catch (Exception ex)

            {

                // Handle other unexpected exceptions

                // Log the exception (use a logging framework)

                // _logger.LogError(ex, "An unexpected error occurred during signup.");

                ViewBag.ErrorMessage = "An unexpected error occurred. Please try again later.";

                return View();

            }

        }

        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LogIn(User user)
        {
            var myUser = context.Users.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
            if (myUser != null)
            {
                HttpContext.Session.SetString("UserSession", myUser.Email);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Login Failed";
            }
            return View();
        }
        public IActionResult Logout()
        {
            if(HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login");
            }
            return View();
        }
        

    }
}