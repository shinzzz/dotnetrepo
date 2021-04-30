using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UsersAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace UsersAPI.Controllers {

    public class UsersController : Controller {
        private readonly IHttpClientFactory _clientFactory;
        public IEnumerable<User> Users { get; set; }

        const string BASE_URL = "https://jsonplaceholder.cypress.io/todos";

        public UsersController (IHttpClientFactory clientFactory) {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index () {
            var message = new HttpRequestMessage ();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri ($"{BASE_URL}");
            message.Headers.Add ("Accept", "application/json");

            var client = _clientFactory.CreateClient ();

            var response = await client.SendAsync (message);

            if (response.IsSuccessStatusCode) {
                using var responseStream = await response.Content.ReadAsStreamAsync ();
                Users = await JsonSerializer.DeserializeAsync<IEnumerable<User>> (responseStream);
            } else {
                Users = Array.Empty<User> ();
            }

            return View (Users);
        }

        public IActionResult Create () {
            return View ();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create ([Bind ("id,userId,title,completed")] User user) {
            if (ModelState.IsValid) {
                HttpContent httpContent = new StringContent (Newtonsoft.Json.JsonConvert.SerializeObject (user), Encoding.UTF8);
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue ("application/json");

                var message = new HttpRequestMessage ();
                message.Content = httpContent;
                message.Method = HttpMethod.Post;
                message.RequestUri = new Uri ($"{BASE_URL}");

                HttpClient client = _clientFactory.CreateClient ();
                HttpResponseMessage response = await client.SendAsync (message);

                var result = await response.Content.ReadAsStringAsync ();

                return RedirectToAction (nameof (Index));
            }

            return View (user);
        }

        public async Task<IActionResult> Details (int? id) {
            if (id == null)
                return NotFound ();

            var message = new HttpRequestMessage ();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri ($"{BASE_URL}/{id}");
            message.Headers.Add ("Accept", "application/json");

            var client = _clientFactory.CreateClient ();

            var response = await client.SendAsync (message);

            User user = null;

            if (response.IsSuccessStatusCode) {
                using var responseStream = await response.Content.ReadAsStreamAsync ();
                user = await JsonSerializer.DeserializeAsync<User> (responseStream);
            }

            if (user == null)
                return NotFound ();

            return View (user);

        }

        public async Task<IActionResult> Delete (int? id) {
            if (id == null)
                return NotFound ();

            var message = new HttpRequestMessage ();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri ($"{BASE_URL}/{id}");
            message.Headers.Add ("Accept", "application/json");

            var client = _clientFactory.CreateClient ();

            var response = await client.SendAsync (message);

            User user = null;

            if (response.IsSuccessStatusCode) {
                using var responseStream = await response.Content.ReadAsStreamAsync ();
                user = await JsonSerializer.DeserializeAsync<User> (responseStream);
            }

            if (user == null)
                return NotFound ();

            return View (user);

        }

        [HttpPost, ActionName ("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (int? id) {
            var message = new HttpRequestMessage ();
            message.Method = HttpMethod.Delete;
            message.RequestUri = new Uri ($"{BASE_URL}/{id}");

            HttpClient client = _clientFactory.CreateClient ();
            HttpResponseMessage response = await client.SendAsync (message);

            var result = await response.Content.ReadAsStringAsync ();

            return RedirectToAction (nameof (Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id) {
            if (id == null)
                return NotFound ();

            var message = new HttpRequestMessage ();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri ($"{BASE_URL}/{id}");
            message.Headers.Add ("Accept", "application/json");

            var client = _clientFactory.CreateClient ();

            var response = await client.SendAsync (message);

            User user = null;

            if (response.IsSuccessStatusCode) {
                using var responseStream = await response.Content.ReadAsStreamAsync ();
                user = await JsonSerializer.DeserializeAsync<User> (responseStream);
            } 

            if (user == null)
                return NotFound ();

            return View (user);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind ("id,userId,title,completed")] User user) {
            if (id != user.id)
                return NotFound ();

            if (ModelState.IsValid) {
                HttpContent httpContent = new StringContent (Newtonsoft.Json.JsonConvert.SerializeObject (user), Encoding.UTF8);
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue ("application/json");

                var message = new HttpRequestMessage ();
                message.Content = httpContent;
                message.Method = HttpMethod.Put;
                message.RequestUri = new Uri ($"{BASE_URL}/{id}");

                HttpClient client = _clientFactory.CreateClient ();
                HttpResponseMessage response = await client.SendAsync(message);

                var result = await response.Content.ReadAsStringAsync ();

                return RedirectToAction (nameof (Index));
            }

            return View (user);
        }
    }

}