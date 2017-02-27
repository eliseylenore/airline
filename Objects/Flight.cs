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
