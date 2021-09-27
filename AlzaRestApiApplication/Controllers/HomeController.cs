using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using System;
using System.Linq;
using System.Configuration;
using AlzaRestApiApplication.Models;
using RichardSzalay.MockHttp;
using Newtonsoft.Json;

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

                    client.BaseAddress = new Uri("http://" + Request.Url.Authority + "/api/ShoppingItems/UpdateShoppingItemInfo");

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


        public ActionResult Edit(int id)
        {
            // TODO: don't hardcode, fetch from repository

            MemberViewModel members = new MemberViewModel();

            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder("http://" + Request.Url.Authority + "/api/ShoppingItems/GetShoppingListInfoById");

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

        public ActionResult Detail(int id)
        {
            // TODO: don't hardcode, fetch from repository

            MemberViewModel members = new MemberViewModel();

            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder("http://" + Request.Url.Authority + "/api/ShoppingItems/GetShoppingListInfoById");

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

        public ActionResult Index(int pageNumber = 1, int pageSize = 10, bool testing = false)
        {
            IEnumerable<MemberViewModel> members = null;

            if (testing)
            {
                members = JsonConvert.DeserializeObject<IEnumerable<MemberViewModel>>("[{'MaxPageSize': '22','Status': '0','Description': 'Najtenší a najlahší notebook','Id': '1','ImgUri': '1.jpg','Name': 'Macbook Air 13 M1 Vesmírne sivý','Price': '999'}]");

                return View(members);
            }
            else
            {

                using (var client = new HttpClient())
                {

                    UriBuilder builder = new UriBuilder("http://" + Request.Url.Authority + "/api/ShoppingItems/GetShoppingList");

                    builder.Query = "pageNumber=" + pageNumber + "&pageSize=" + pageSize;

                    var responseTask = client.GetAsync(builder.Uri);
                    responseTask.Wait();

                    //To store result of web api response.   
                    var result = responseTask.Result;

                    //If success received   
                    if (result.IsSuccessStatusCode)
                    {

                        var readTask = result.Content.ReadAsAsync<IList<MemberViewModel>>();
                        readTask.Wait();

                        // Setup paging links
                        int pageNext = pageNumber;
                        int pagePrev = pageNumber;

                        int MaxPageSize = readTask.Result[0].MaxPageSize;
                        if ((pageSize * pageNumber) < MaxPageSize)
                        {
                            pageNext++;
                        }

                        if (pageNumber > 1)
                        {
                            pagePrev--;
                        }
                        else
                        {
                            pagePrev = 1;
                        }

                        members = readTask.Result;


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
        }
    }
}
