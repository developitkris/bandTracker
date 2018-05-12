using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using BandTracker.Models;


namespace BandTracker.Tests
{
    [TestClass]
    public class VenueTests : IDisposable
    {
        public void Dispose()
        {
          Venue.DeleteAllVenues();
        }

        public VenueTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=band_tracker_test;";
        }

        [TestMethod]
        public void SaveVenue_ToDatabase_True()
        {
            Venue testVenue = new Venue("Crocodile");
            testVenue.SaveVenue();

            List<Venue> result = Venue.GetAllVenues();
            List<Venue> testList = new List<Venue>{testingVenue};

            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void AddBand_ToVenue_True()
        {
          Band testBand1 = new Band("Belle and Sebastian");
          Band testBand2 = new Band("Exit Calm");
          Band testBand3 = new Band("Of Monsters and Men");
          Venue testVenue1 = new Venue("Staples Center");

          testBand1.SaveBand();
          testBand2.SaveBand();
          testBand3.SaveBand();
          testVenue1.SaveVenue();
          testVenue1.AddBand(testBand1);
          testVenue1.AddBand(testBand2);
          testVenue1.AddBand(testBand3);

          List<Band> bandsAtVenue = testVenue1.GetBands();
          List<Band> myBands = new List<Band>{testBand1, testBand2, testBand3};

          CollectionAssert.AreEqual(myBands, bandsAtVenue);
        }

        [TestMethod]
        public void VenueTable_Clear_True()
        {
            int result = Venue.GetAllVenues().Count;

            Assert.AreEqual(0, result);
        }

    }
}
