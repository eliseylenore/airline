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

        public void Dispose()
        {

        }

    }
}
