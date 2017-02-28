using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace AirlineApp
{
    public class Flight
    {
        private string _flightNum;
        private string _status;
        private string _time;
        private int _id;

        public Flight(string FlightNum, string Status, string Time, int Id = 0)
        {
            _flightNum = FlightNum;
            _status = Status;
            _time = Time;
            _id = Id;
        }

        public override bool Equals(System.Object otherFlight)
        {
            if(!(otherFlight is Flight))
            {
                return false;
            }
            else {
                Flight newFlight = (Flight) otherFlight;
                bool idEquality = this.GetId() == newFlight.GetId();
                bool flightNumEquality = this.GetFlightNum() == newFlight.GetFlightNum();
                bool statusEquality = this.GetStatus() == newFlight.GetStatus();
                bool timeEquality = this.GetTime() == newFlight.GetTime();
                return (idEquality && flightNumEquality && statusEquality && timeEquality);
            }
        }

        public string GetFlightNum()
        {
            return _flightNum;
        }

        public string GetStatus()
        {
            return _status;
        }

        public string GetTime()
        {
            return _time;
        }

        public int GetId()
        {
            return _id;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO flights (flight_number, status, time) OUTPUT INSERTED.id VALUES (@FlightNum, @FlightStatus, @FlightTime);", conn);

            SqlParameter flightNumParameter = new SqlParameter();
            flightNumParameter.ParameterName = "@FlightNum";
            flightNumParameter.Value = this._flightNum;
            cmd.Parameters.Add(flightNumParameter);

            SqlParameter flightStatusParameter = new SqlParameter();
            flightStatusParameter.ParameterName = "@FlightStatus";
            flightStatusParameter.Value = this._status;
            cmd.Parameters.Add(flightStatusParameter);

            SqlParameter flightTimeParameter = new SqlParameter();
            flightTimeParameter.ParameterName = "@FlightTime";
            flightTimeParameter.Value = this._time;
            cmd.Parameters.Add(flightTimeParameter);

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

        public static Flight Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM flights WHERE id = @FlightId;", conn);

            SqlParameter flightIdParameter = new SqlParameter();
            flightIdParameter.ParameterName = "@FlightId";
            flightIdParameter.Value = id.ToString();
            cmd.Parameters.Add(flightIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundFlightId = 0;
            string foundFlightNum = null;
            string foundFlightStatus = null;
            string foundFlightTime = null;

            while(rdr.Read())
            {
                foundFlightId = rdr.GetInt32(0);
                foundFlightNum = rdr.GetString(1);
                foundFlightStatus = rdr.GetString(2);
                foundFlightTime = rdr.GetString(3);
            }

            Flight foundFlight = new Flight(foundFlightNum, foundFlightStatus, foundFlightTime, foundFlightId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundFlight;

        }

        public void AddFlight(City DepartureCity, City ArrivalCity)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO cities_flights(departure_city_id, arrival_city_id, flight_id) VALUES (@DepartureCityId, @ArrivalCityId, @FlightId);", conn);

            SqlParameter departParameter = new SqlParameter();
            departParameter.ParameterName= "@DepartureCityId";
            departParameter.Value = DepartureCity.GetId();
            cmd.Parameters.Add(departParameter);

            SqlParameter arrivalParameter = new SqlParameter();
            arrivalParameter.ParameterName = "@ArrivalCityId";
            arrivalParameter.Value = ArrivalCity.GetId();
            cmd.Parameters.Add(arrivalParameter);

            SqlParameter flightIdParameter = new SqlParameter();
            flightIdParameter.ParameterName = "@FlightId";
            flightIdParameter.Value = this._id;
            cmd.Parameters.Add(flightIdParameter);

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }

        }

        public List<City> GetCities()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM cities_flights WHERE flight_id = @FlightId;", conn);

            SqlParameter flightIdParameter = new SqlParameter();
            flightIdParameter.ParameterName = "@FlightId";
            flightIdParameter.Value = this._id;
            cmd.Parameters.Add(flightIdParameter);

            List<int> cityIds = new List<int> {};

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int departureCityId = int.Parse(rdr.GetString(1));
                int arrivalCityId = int.Parse(rdr.GetString(2));
                cityIds.Add(departureCityId);
                cityIds.Add(arrivalCityId);
            }

            if (rdr != null)
            {
                rdr.Close();
            }

            List<City> cities = new List<City> {};

            foreach(int id in cityIds)
            {
                SqlCommand cityQuery = new SqlCommand("SELECT * FROM cities WHERE id = @CityId;", conn);

                SqlParameter cityIdParameter = new SqlParameter();
                cityIdParameter.ParameterName = "@CityId";
                cityIdParameter.Value = id.ToString();
                cityQuery.Parameters.Add(cityIdParameter);

                SqlDataReader queryReader = cityQuery.ExecuteReader();

                while(queryReader.Read())
                {
                    int thisCityId = queryReader.GetInt32(0);
                    string cityName = queryReader.GetString(1);
                    City foundCity = new City(cityName, thisCityId);
                    cities.Add(foundCity);
                }

                if (queryReader != null)
                {
                    queryReader.Close();
                }
            }
            if (conn != null)
            {
                conn.Close();
            }
            return cities;
        }

        public void EditStatus(string status)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE flights SET status = @NewStatus WHERE id=@FlightId;", conn);


            SqlParameter statusParameter = new SqlParameter();
            statusParameter.ParameterName = "@NewStatus";
            try
            {
            statusParameter.Value = status;
            }
            finally
            {
                statusParameter.Value = "invalid";
            }
            SqlParameter flightIdParameter = new SqlParameter();
            flightIdParameter.ParameterName = "@FlightId";
            flightIdParameter.Value = this.GetId();

            cmd.Parameters.Add(statusParameter);
            cmd.Parameters.Add(flightIdParameter);

            cmd.ExecuteNonQuery();
            if(conn != null)
            {
                conn.Close();
            }
        }

        public void Delete()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM flights WHERE id = @FlightId; DELETE FROM cities_flights WHERE flight_id = @FlightId;", conn);

            SqlParameter idParameter = new SqlParameter();
            idParameter.ParameterName = "@FlightId";
            idParameter.Value = this.GetId();

            cmd.Parameters.Add(idParameter);

            cmd.ExecuteNonQuery();
            if(conn != null)
            {
                conn.Close();
            }
        }

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM flights; DELETE FROM cities_flights;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static List<Flight> GetAll()
        {
            List<Flight> allFlights = new List<Flight>{};
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM flights;", conn);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int foundId = rdr.GetInt32(0);
                string foundFlightNum = rdr.GetString(1);
                string foundStatus = rdr.GetString(2);
                string foundTime = rdr.GetString(3);
                Flight foundFlight = new Flight(foundFlightNum, foundStatus, foundTime, foundId);
                allFlights.Add(foundFlight);
            }

            if(rdr != null)
            {
                rdr.Close();
            }

            if(conn != null)
            {
                conn.Close();
            }

            return allFlights;
        }
    }
}
