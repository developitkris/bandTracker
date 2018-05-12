using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using BandTracker.Models;

namespace BandTracker.Tests
{
    [TestClass]
    public class BandTests : IDisposable
    {
        public void Dispose()
        {
          Band.DeleteAllBands();
        }

        public BandTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=band_tracker_test;";
        }

        [TestMethod]
        public void SaveBand_ToDatabase_True()
        {
            Band testBand = new Band("Decemberists");
            testBand.SaveBand();

            List<Band> result = Band.GetAllBands();
            List<Band> bandList = new List<Band>{testingBand};

            CollectionAssert.AreEqual(bandList, result);
        }

        [TestMethod]
        public void AddVenue_ToBand_True()
        {
          Venue testVenue1 = new Venue("02 Academy");
          Venue testVenue2 = new Venue("The Aquarium");
          Venue testVenue3 = new Venue("Cambridge Junction");
          Band testBand1 = new Band("Fleet Foxes");

          testVenue1.SaveVenue();
          testVenue2.SaveVenue();
          testVenue3.SaveVenue();
          testBand1.SaveBand();
          testBand1.AddBand(testVenue1);
          testBand1.AddBand(testVenue2);
          testBand1.AddBand(testVenue3);

          List<Venue> venuesByBand = testBand1.GetVenues();
          List<Venue> myVenues = new List<Venue>{testVenue1, testVenue2, testVenue3};

          CollectionAssert.AreEqual(myVenues, venuesByBand);
        }

        [TestMethod]
        public void BandTable_Clear_True()
        {
            int result = Band.GetAllBands().Count;

            Assert.AreEqual(0, result);
        }
    }
}
