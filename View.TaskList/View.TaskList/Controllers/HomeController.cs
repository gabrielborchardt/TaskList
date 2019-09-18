using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using View.TaskList.Models;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.TaskList.Controllers
{
    public class HomeController : Controller
    {
        private string _apiTaskList;
        public HomeController(IConfiguration Configuration)
        {
            _apiTaskList = Configuration["AppSettings:ApiTaskList"];
        }

        public async Task<IActionResult> Tasks()
        {
            var retorno = new List<Models.Task>();

            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri(_apiTaskList);
                api.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var apiResult = await api.GetAsync("gettasks");

                if (apiResult.StatusCode == HttpStatusCode.OK)
                {
                    var result = apiResult.Content.ReadAsStringAsync().Result;
                    retorno = JsonConvert.DeserializeObject<List<Models.Task>>(result);
                }
            }

            return View(retorno);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var retorno = new Models.Task();

            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri(_apiTaskList);
                api.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var apiResult = await api.GetAsync("gettasks");

                if (apiResult.StatusCode == HttpStatusCode.OK)
                {
                    var result = apiResult.Content.ReadAsStringAsync().Result;
                    retorno = JsonConvert.DeserializeObject<List<Models.Task>>(result).Where(x => x.Id == id).FirstOrDefault();
                }
            }

            return View(retorno);
        }

        public async Task<IActionResult> Finish(int id)
        {
            using (var api = new HttpClient())
            {
                api.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var url = "";
                HttpResponseMessage apiResult;

                url = _apiTaskList + "/alterstate/?id=" + id + "&status=C";
                apiResult = await api.PutAsync(url,null);
                

                if (apiResult.StatusCode == HttpStatusCode.OK)
                {
                    var result = apiResult.Content.ReadAsStringAsync().Result;
                }
            }

            return RedirectToAction("Tasks");
        }

        public async Task<IActionResult> Open(int id)
        {
            using (var api = new HttpClient())
            {
                api.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var url = "";
                HttpResponseMessage apiResult;

                url = _apiTaskList + "/alterstate/?id=" + id + "&status=A";
                apiResult = await api.PutAsync(url, null);


                if (apiResult.StatusCode == HttpStatusCode.OK)
                {
                    var result = apiResult.Content.ReadAsStringAsync().Result;
                }
            }

            return RedirectToAction("Tasks");
        }

        public async Task<IActionResult> Delete(int id)
        {
            using (var api = new HttpClient())
            {
                api.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var url = new Uri(_apiTaskList + "/DeleteTask/?id=" + id);

                var apiResult = await api.DeleteAsync(url);

                if (apiResult.StatusCode == HttpStatusCode.OK)
                {
                    var result = apiResult.Content.ReadAsStringAsync().Result;
                }
            }

            return RedirectToAction("Tasks");
        }

        [HttpPost]
        public async Task<IActionResult> Save(Models.Task task)
        {
            using (var api = new HttpClient())
            {
                api.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var url = "";
                HttpResponseMessage apiResult;

                if (task.Id == 0)
                {
                    url = _apiTaskList + "/posttask";
                    apiResult = await api.PostAsJsonAsync(url, task);
                }
                else
                {
                    url = _apiTaskList + "/puttask";
                    apiResult = await api.PutAsJsonAsync(url, task);
                }
                
                if (apiResult.StatusCode == HttpStatusCode.OK)
                {
                    var result = apiResult.Content.ReadAsStringAsync().Result;
                }
            }

            return RedirectToAction("Tasks");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
