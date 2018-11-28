using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using GC_Car_Dealership.Models;
using System.Web.Http;
using Newtonsoft.Json.Linq;


namespace GC_Car_Dealership.Controllers
{

    //System.Web.HTTP and System.Web.MVC have a conflict?
    public class CarController : ApiController
    {
        //making orm to access data
        GC_DealershipEntities carORM = new GC_DealershipEntities();

        [HttpGet]
        public JObject AllCars()
        {
            List<Car> allCarsList = new List<Car>();
            allCarsList = carORM.Cars.ToList();
            JObject sendObj = new JObject();
            JArray arrayForCars = new JArray();

            foreach(Car c in allCarsList)
            {
                JObject temp = new JObject
                {
                    ["id"] = c.CarID - 1000,
                    ["make"] = c.Make,
                    ["model"] = c.Model,
                    ["color"] = c.Color,
                    ["year"] = c.Year
                };

                arrayForCars.Add(temp);
            }

            sendObj.Add("cars", arrayForCars);
            return sendObj;

           // return Json(objectNameHere, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JObject SearchCars(string make, string model, string color, string year)
        {
            List<Car> searchCarList = new List<Car>();
            JObject sendObj = new JObject();
            JArray arraySearchCars = new JArray();

            IQueryable<Car> carQuery = carORM.Cars.AsQueryable();

                if (model != null)
                {
                carQuery = carQuery.Where(x => x.Model == model);
                }

                if (make != null)
                {
                carQuery = carQuery.Where(x => x.Make == make);
                }

                if (color != null)
                {
                carQuery = carQuery.Where(x => x.Color == color);
                }

                if (year != null)
                {
                carQuery = carQuery.Where(x => x.Year == year);
                }

            searchCarList = carQuery.ToList();
            //searchCarList = carORM.Cars.Where(x => x.searchString == )

            if (searchCarList.Count != 0)
            {
                foreach (var c in searchCarList)
                {
                    JObject temp = new JObject
                    {
                        ["id"] = c.CarID - 1000,
                        ["make"] = c.Make,
                        ["model"] = c.Model,
                        ["color"] = c.Color,
                        ["year"] = c.Year
                    };

                    arraySearchCars.Add(temp);
                }

                sendObj.Add("cars", arraySearchCars);
                return sendObj;
            }
            else
            {
                sendObj.Add("no data", "");
                return sendObj;
            }
        }
    }

}
