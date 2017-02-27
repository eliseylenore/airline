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

        [Fact]
        public void Test_EqualOverride()
        {
            //Arrange, Act
            City firstCity = new City("Atlanta");
            City secondCity = new City("Atlanta");

            //Assert
            Assert.Equal(firstCity, secondCity);
        }

        [Fact]
        public void Test_Save()
        {
            //Arrange, Act
            City newCity = new City("Atlanta");
            newCity.Save();

            List<City> expected = new List<City>{newCity};
            List<City> actual = City.GetAll();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_Find()
        {
            City newCity = new City("Atlanta");
            newCity.Save();

            City foundCity = City.Find(newCity.GetId());

            Assert.Equal(newCity, foundCity);
        }

        [Fact]
        public void Test_Delete()
        {
            City newCity = new City("Atlanta");
            newCity.Save();

            newCity.Delete();

            List<City> expected = new List<City>{};
            List<City> actual = City.GetAll();

            //Assert
            Assert.Equal(expected, actual);
        }

        public void Dispose()
        {
            City.DeleteAll();
        }
    }
}
