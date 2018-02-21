using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestExample.Models;

namespace RestExample.Controllers
{
    public class LoginController : Controller
    {
        static HttpClient client = new HttpClient();

        private Dictionary<String, List<string>> groups = new Dictionary<string, List<string>>()
        {
            { "engineers", new List<string>{"jos", "willy"} },
            { "scientists", new List<string>{"manu", "joanna"} }
        };

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.User user)
        {
            if (ModelState.IsValid)
            {
                User userCreated = GetPostAsync("https://jsonplaceholder.typicode.com/posts", user).GetAwaiter().GetResult();
               
                return View("Created", userCreated);
            }

            return View();
        }

        static async Task<User> GetPostAsync(string path, User newUser)
        {
            User user = null;
            HttpResponseMessage response = await client.PostAsJsonAsync(path, newUser);
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<User>();
            }
            return user;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
