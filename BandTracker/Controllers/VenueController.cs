using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;

namespace BandTracker.Controllers
{
    public class VenueController : Controller
    {

      [HttpGet("/view-add-venue")]
      public ActionResult AddVenue()
      {
          List<Venue> allVenues = Venue.GetAll();
          return View(allVenues);
      }
      [HttpPost("/create-venue")]
      public ActionResult CreateVenue()
      {
          Venue newVenue = new Venue(Request.Form["venue-name"], Request.Form["venue-location"], Request.Form["venue-date"]);
          newVenue.Save();
          return RedirectToAction("AddVenue");
      }
      [HttpGet("/venue-index/{id}")]
      public ActionResult VenueIndex(int id)
      {
          Dictionary<string, object> model = new Dictionary<string, object>{};
          Venue myVenue = Venue.Find(id);
          model.Add("venue", myVenue);
          List<Band> myBands = myVenue.GetBands();
          model.Add("bands", myBands);
          List<Band> allBands = Band.GetAll();
          // model.Add();
          return View(model);
      }
      [HttpGet("/delete-venue/{id}")]
      public ActionResult DeleteVenue(int id)
      {
          Venue deleteVenue = Venue.Find(id);
          deleteVenue.DeleteVenue();
          return RedirectToAction("AddVenue");
      }
      [HttpGet("/update-venue/{id}")]
      public ActionResult UpdateVenue(int id)
      {
          Venue updateVenue = Venue.Find(id);
          return View(updateVenue);
      }
      [HttpPost("/venue-updated/{id}")]
      public ActionResult UpdatedVenue(int id)
      {
          string newVenueName = (Request.Form["venue-name"], Request.Form["venue-location"], Request.Form["venue-date"]);
          Venue newVenue = new Venue(newVenueName, id);
          newVenue.UpdateVenue(newVenueName);
          return RedirectToAction("AddVenue");
      }
    }
}
