using Xunit;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace AirlineApp
{
    public class FlightTest : IDisposable
    {
        public FlightTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=airline_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_IfEmpty()
        {
            int result = Flight.GetAll().Count;

            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_EqualOverride()
        {
            //Arrange, Act
            Flight firstFlight = new Flight("1", "On time", "6:30 PM");
            Flight secondFlight = new Flight("1", "On time", "6:30 PM");

            //Assert
            Assert.Equal(firstFlight, secondFlight);
        }

        [Fact]
        public void Test_Save()
        {
            Flight newFlight = new Flight("1", "On time", "6:30 PM");
            newFlight.Save();

            List<Flight> expected = new List<Flight>{newFlight};
            List<Flight> actual = Flight.GetAll();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_Find()
        {
            Flight newFlight = new Flight("1", "On time", "6:30 PM");
            newFlight.Save();

            Flight result = Flight.Find(newFlight.GetId());

            Assert.Equal(newFlight, result);
        }
        [Fact]
        public void Test_Delete()
        {
            Flight newFlight = new Flight("1", "On time", "6:30 PM");
            newFlight.Save();
            List<Flight> expected = new List<Flight>{};

            newFlight.Delete();
            List<Flight> actual = Flight.GetAll();

            Assert.Equal(expected, actual);

        }

        [Fact]
        public void TestAddCitiesToFlightAndFindFlight()
        {
            Flight newFlight = new Flight("1", "On time", "6:30 PM");
            newFlight.Save();
            City departure = new City("Atlanta");
            departure.Save();
            City arrival = new City("New York");
            arrival.Save();

            newFlight.AddFlight(departure.GetId(), arrival.GetId());


        }

        public void Dispose()
        {
            Flight.DeleteAll();
        }

    }
}
