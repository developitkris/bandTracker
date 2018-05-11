using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using BandTracker;
using BandTracker.Models;


namespace BandTracker.Models
{
  public class Band
  {
    private int _id;
    private string _bandName;
    private string _origin;
  }
  public Venue(string name, string origin, int id=0)
  {
    _id= id;
    _bandName = name;
    _origin = origin;
  }

  public override bool Equals(System.Object otherBand)
    {
        if (!(otherBand is Band))
        {
          return false;
        }
        else
        {
          Band newBand = (Band) otherBand;
          bool idEquality = (this.GetId() == newBand.GetId());
          bool bandNameEquality = (this.GetBandName() == newBand.GetBandName());
          return (idEquality && bandNameEquality);
        }
    }

    public override int GetHashCode()
    {
      return this.GetBandName().GetHashCode();
    }

    public int GetId()
    {
      return _id;
    }

    public string GetBandName()
    {
      return _bandName;
    }

    public string GetOrigin()
    {
      return _origin;
    }

    public static List<Band> GetAllBands()
    {
     List<Band> allBands = new List<Band>{};
     MySqlConnection conn = DB.Connection();
     conn.Open();
     MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = @"SELECT * FROM bands;";
     MySqlReader rdr = cmd.ExecuteReader() as MySqlReader;
     while(rdr.Read())
     {
       int id = rdr.GetInt32(0);
       string bandName = rdr.GetString(1);
       string origin = rdr.GetString(2);
       Band newBand = new Band(bandName, origin, id);
       allBands.Add(newBand);
     }
     conn.Close();
     if(conn !=null)
     {
       conn.Dispose();
     }
     return allBands;
    }

    public void Save()
    {
     MySqlConnection conn = DB.Connection();
     conn.Open();
     MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = @"INSERT INTO bands (band_name) VALUES (@bandName);";

     MySqlParameter bandName = new MySqlParameter();
     bandName.ParameterName = "@bandName";
     bandName.Value = this._bandName;
     cmd.Parameters.Add(bandName);

     cmd.ExecuteNonQuery();
     _id = (int) cmd.LastInsertedId;

     conn.Close();
     if(conn != null)
     {
       conn.Dispose();
     }
    }

    public List<Venue> GetVenues()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT venues.* FROM bands
          JOIN venue_band ON (bands.id = venue_band.band_id)
          JOIN venues ON (venue_band.venue_id = venues.id)
          WHERE bands.id = @BandId;";

      MySqlParameter bandIdParameter = new MySqlParameter();
      bandIdParameter.ParameterName = "@BandId";
      bandIdParameter.Value = _id;
      cmd.Parameters.Add(bandIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Venue> venues = new List<Venue>{};

      while(rdr.Read())
      {
        int venueId = rdr.GetInt32(0);
        string venueName = rdr.GetString(1);
        string location = rdr.GetString(2);
        string date = rdr.GetString(3);
        Venue newVenue = new Venue(venueName, location, date, venueId);
        venues.Add(newVenue);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return bands;
    }

    public void AddVenue(Venue newVenue)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO venue_band (venue_id, band_id) VALUES (@venueId, @bandId);";

      cmd.Parameters.Add(new MySqlParameter("@venueId", newVenue.GetId()));
      cmd.Parameters.Add(new MySqlParameter("@bandId", _id));

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Band Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM bands WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      int bandId =0;
      string bandName="";
      string origin="";

      while (rdr.Read())
      {
        bandId = rdr.GetInt32(0);
        bandName = rdr.GetString(1);
        origin = rdr.GetString(2);
      }
      Band foundBand= new Band(bandName, origin, bandId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return foundBand;
    }
  }
