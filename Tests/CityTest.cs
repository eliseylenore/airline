using Xunit;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace AirlineApp
{
    public class CityTest : IDisposable
    {
        public CityTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=airline_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_EmptyAtFirst()
        {
            //Arrange, act
            int result = City.GetAll().Count;

            //Assert
            Assert.Equal(0, result);
        }

        public void Dispose()
        {
            
        }
    }
}
