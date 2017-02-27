using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using Nancy;

namespace AirlineApp
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => {
                return View["index.cshtml", ModelMaker()];
            };

            Get["/add-cities"] = _ => {
                return View["add-cities.cshtml"];
            };

            Post["/add-cities"] = _ => {
                City newCity = new City(Request.Form["city-name"]);
                newCity.Save();
                return View["index.cshtml", ModelMaker()];
            };

            Get["/add-flights"] = _ => {
                return View["add-flights.cshtml", ModelMaker()];
            };

            Post["/add-flights"] = _ => {
                Flight newFlight = new Flight(Request.Form["flight-num"], Request.Form["flight-status"], Request.Form["flight-time"]);
                newFlight.Save();

                City departureCity = City.Find(Request.Form["departure"]);
                City arrivalCity = City.Find(Request.Form["arrival"]);

                newFlight.AddFlight(departureCity, arrivalCity);

                return View["index.cshtml", ModelMaker()];
            };

            Delete["/delete/{id}"] = parameters => {
                Flight.Find(parameters.id).Delete();
                return View["index.cshtml", ModelMaker()];
            };

            Patch["/edit/{id}"] = parameters => {
                Flight.Find(parameters.id).EditStatus(Request.Form["status"]);
                return View["index.cshtml", ModelMaker()];
            };
        }
        public static Dictionary<string, object> ModelMaker()
        {
            Dictionary<string, object> model = new Dictionary<string, object>{};
            model.Add("Flights", Flight.GetAll());
            model.Add("Cities", City.GetAll());
            return model;
        }
    }
}
