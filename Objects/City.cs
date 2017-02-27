using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace AirlineApp
{
    public class City
    {
        private string _name;
        private int _id;

        public City(string Name, int Id = 0)
        {
            _name = Name;
            _id = Id;
        }

        public static List<City> GetAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * from cities", conn);

            SqlDataReader rdr = cmd.ExecuteReader();

            List<City> AllCities = new List<City>();

            while(rdr.Read())
            {
                int foundCityId = rdr.GetInt32(0);
                string foundCityName = rdr.GetString(1);
                City foundCity = new City(foundCityName, foundCityId);
                AllCities.Add(foundCity);
            }

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }

            return AllCities;

        }

    }
}
