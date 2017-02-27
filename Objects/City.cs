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

        public string GetName()
        {
            return _name;
        }

        public int GetId()
        {
            return _id;
        }

        public override bool Equals(System.Object otherCity)
        {
            if(!(otherCity is City))
            {
                return false;
            }
            else
            {
                City newCity = (City) otherCity;
                bool idEquality = this.GetId() == newCity.GetId();
                bool nameEquality = this.GetName() == newCity.GetName();
                return (idEquality && nameEquality);
            }
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO cities(name) OUTPUT INSERTED.id VALUES(@NewCityName);", conn);

            SqlParameter newCityParameter = new SqlParameter();
            newCityParameter.ParameterName = "@NewCityName";
            newCityParameter.Value = this._name;
            cmd.Parameters.Add(newCityParameter);

            SqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
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

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM cities;", conn);
            cmd.ExecuteNonQuery();

            if(conn != null)
            {
                conn.Close();
            }
        }



    }
}
