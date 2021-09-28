using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using System;
using System.Linq;
using System.Configuration;
using AlzaRestApiApplication.Models;
using RichardSzalay.MockHttp;
using Newtonsoft.Json.Linq;

namespace AlzaRestApiApplication.Controllers
{
    public class HomeController : Controller
    {
        
        // POST: /Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ImgUri,Price,Description")] MemberViewModel modelUpdate)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["RestApiHost"] + "/api/ShoppingItems/UpdateShoppingItemInfo");

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<MemberViewModel>("formData", modelUpdate);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("/Detail/" + modelUpdate.Id);
                    }

                }
                
            }
            return View(modelUpdate);
        }


        public ActionResult Edit(int id, bool mockHttpClient = false)
        {
            MemberViewModel members = new MemberViewModel();

            //mockup data for unit testing
            using (var client = mockHttpClient ? new HttpClient(mockHttpClientHandler()) : new HttpClient())
            {
                UriBuilder builder = new UriBuilder(ConfigurationManager.AppSettings["RestApiHost"] + "/api/ShoppingItems/GetShoppingListInfoById");

                builder.Query = "ShoppingListId=" + id;

                var responseTask = client.GetAsync(builder.Uri);
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<MemberViewModel>>();
                    readTask.Wait();

                    if (readTask.Result.Count == 0)
                    {
                        members = new MemberViewModel();
                        ModelState.AddModelError(string.Empty, "Product detail is not available.");
                    }
                    else
                    {
                        members = readTask.Result.First();
                    }
                }
                else
                {
                    //Error response received   
                    members = new MemberViewModel();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            ViewBag.Message = members;
            return View(members);
        }

        public ActionResult Detail(int id, bool mockHttpClient = false)
        {
            MemberViewModel members = new MemberViewModel();

            //mockup data for unit testing
            using (var client = mockHttpClient ? new HttpClient(mockHttpClientHandler()) : new HttpClient())
            {
                UriBuilder builder = new UriBuilder(ConfigurationManager.AppSettings["RestApiHost"] + "/api/ShoppingItems/GetShoppingListInfoById");

                builder.Query = "ShoppingListId=" + id;

                var responseTask = client.GetAsync(builder.Uri);
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<MemberViewModel>>();
                    readTask.Wait();

                    if (readTask.Result.Count == 0) {
                        members = new MemberViewModel();
                        ModelState.AddModelError(string.Empty, "Product detail is not available.");
                    } else {
                        members = readTask.Result.First();
                    }
                }
                else
                {
                    //Error response received   
                    members = new MemberViewModel();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            ViewBag.Message = members;
            return View(members);
        }

        public ActionResult Index(int Id = 1, bool mockHttpClient = false)
        {
            IEnumerable<MemberViewModel> members = null;

            //mockup data for unit testing
            using (var client = mockHttpClient ? new HttpClient(mockHttpClientHandler()) : new HttpClient())
            {

                int pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);

                UriBuilder builder = new UriBuilder(ConfigurationManager.AppSettings["RestApiHost"] + "/api/ShoppingItems/GetShoppingList");

                builder.Query = "pageNumber=" + Id + "&pageSize=" + pageSize;

                var responseTask = client.GetAsync(builder.Uri);
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    // Parse JSON result
                    JObject o = JObject.Parse("{'d': " + readTask.Result + "}");
                    JArray jsonArray = (JArray)o["d"];
                    IList<MemberViewModel> readTaskConverted = jsonArray.ToObject<IList<MemberViewModel>>();

                    // Setup paging links
                    int pageNext = Id;
                    int pagePrev = Id;
                    int MaxPageSize = readTaskConverted[0].MaxPageSize;
                    if ((pageSize * Id) < MaxPageSize)
                    {
                        pageNext++;
                    }

                    if (Id > 1)
                    {
                        pagePrev--;
                    }
                    else
                    {
                        pagePrev = 1;
                    }

                    members = readTaskConverted;

                    PageViewModel rec = new PageViewModel
                    {
                        PageNumberNext = pageNext,
                        PageNumberPrev = pagePrev,
                        PageSize = pageSize
                    };

                    ViewBag.Message = rec;
                }
                else
                {
                    //Error response received   
                    members = Enumerable.Empty<MemberViewModel>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(members);
        }

        private HttpMessageHandler mockHttpClientHandler()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(ConfigurationManager.AppSettings["RestApiHost"] + "/api/ShoppingItems/GetShoppingList")
                    .Respond("application/json", @"[{'Id':1,'Name':'Macbook Air 13\' M1 Vesmírne sivý','ImgUri':'1.jpg','Price':999.00,'Description':'Najtenší a najlahší notebook','Status':0,'Message':null,'MaxPageSize':22}]"); // Respond with JSON

            return mockHttp;

        }
    }
}
