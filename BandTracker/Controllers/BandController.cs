using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;

namespace BandTracker.Controllers
{
    public class BandController : Controller
    {

      [HttpGet("/view-add-band")]
      public ActionResult AddBand()
      {
          List<Band> allBands = Band.GetAll();
          return View(allBands);
      }
      [HttpPost("/create-band")]
      public ActionResult CreateBand()
      {
          Band newBand = new Band(Request.Form["band-name"], Request.Form["band-origin"]);
          newBand.Save();
          return RedirectToAction("AddBand");
      }
      [HttpGet("/band-index/{id}")]
      public ActionResult BandIndex(int id)
      {
          Dictionary<string, object> model = new Dictionary<string, object>{};
          Band myBand = Band.Find(id);
          model.Add("band", myBand);
          List<Band> myBands = myBand.GetBands();
          model.Add("bands", myBands);
          List<Band> allBands = Band.GetAll();
          // model.Add();
          return View(model);
      }
