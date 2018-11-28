using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using GC_Car_Dealership.Models;

namespace GC_Car_Dealership.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            UriBuilder builder = new UriBuilder
            {
                Host = "localhost",
                Path = "api/Car/AllCars",
                Port = 63387
            };
            HttpWebRequest requestAPI = WebRequest.CreateHttp(builder.ToString());

            //making the useragent
            requestAPI.UserAgent = "Mozilla / 5.0(Windows NT 6.1; WOW64; rv: 64.0) Gecko / 20100101 Firefox / 64.0";

            // getting reponse
            HttpWebResponse response = (HttpWebResponse)requestAPI.GetResponse();

            //check for code 200
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //get response stream
                StreamReader reader = new StreamReader(response.GetResponseStream());

                //store it in string
                string readerString = reader.ReadToEnd();
                reader.Close();
                response.Close();

                //convert to Json Object
                JObject jcar = JObject.Parse(readerString);

                List<Car> carListAgain = new List<Car>();

                foreach (JToken vcar in jcar["cars"])
                {
                    Car newCar = new Car
                    {
                        CarID = int.Parse(vcar["id"].ToString()),
                        Make = vcar["make"].Value<string>(),
                        Model = vcar["model"].ToString(),
                        Color = vcar["color"].Value<string>(),
                        Year = vcar["year"].Value<string>()
                    };

                    carListAgain.Add(newCar);
                }
                return View(carListAgain);
            }
            else
            {
                ViewBag.WhatError = "Error getting API";
                return View("/Shared/Error");
            }

        }

        public ActionResult Result(string make, string model, string color, string year)
        {
            UriBuilder builder = new UriBuilder
            {
                Host = "localhost",
                Path = "api/Car/SearchCars",
                Port = 63387,
                Query = $"Make={make}&Model={model}&Color={color}&Year={year}"
            };
            HttpWebRequest requestAPI = WebRequest.CreateHttp(builder.ToString());

            //making the useragent
            requestAPI.UserAgent = "Mozilla / 5.0(Windows NT 6.1; WOW64; rv: 64.0) Gecko / 20100101 Firefox / 64.0";

            // getting reponse
            HttpWebResponse response = (HttpWebResponse)requestAPI.GetResponse();

            //check for code 200
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //get response stream
                StreamReader reader = new StreamReader(response.GetResponseStream());

                //store it in string
                string readerString = reader.ReadToEnd();
                reader.Close();
                response.Close();

                //convert to Json Object
                JObject jcar = JObject.Parse(readerString);

                List<Car> carListAgain = new List<Car>();

                foreach (JToken vcar in jcar["cars"])
                {
                    Car newCar = new Car
                    {
                        CarID = int.Parse(vcar["id"].ToString()),
                        Make = vcar["make"].Value<string>(),
                        Model = vcar["model"].ToString(),
                        Color = vcar["color"].Value<string>(),
                        Year = vcar["year"].Value<string>()
                    };

                    carListAgain.Add(newCar);
                }

                if (carListAgain.Count != 0)
                {
                    return View(carListAgain);
                }
                else
                {
                    ViewBag.NoData = "Search returned no results";
                    return View(carListAgain);
                }
            }
            else
            {
                ViewBag.WhatError = "Error getting API";
                return View("/Shared/Error");
            }
        }
    }
}
