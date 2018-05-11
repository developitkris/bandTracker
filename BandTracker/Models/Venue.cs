using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using BandTracker;
using BandTracker.Models;

namespace BandTracker.Models
{
  public class Venue
  {
    private int _id;
    private string _venueName;
    private string _location;
    private string _date;
  }
  public Venue(string name, string location, string date, int id=0)
  {
    _id= id;
    _venueName = name;
    _location = location;
    _date = date;
  }

  public override bool Equals(System.Object otherVenue)
    {
      if (!(otherVenue is Venue))
      {
        return false;
      }
      else
      {
        Venue newVenue = (Venue) otherVenue;
        bool idEquality = (this.GetId() == newVenue.GetId());
        bool venueNameEquality = (this.GetVenueName() == newVenue.GetVenueName());
        return (idEquality && venueNameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetVenueName().GetHashCode();
    }

    public int GetId()
    {
      return _id;
    }

     public string GetVenueName()
     {
       return _venueName;
     }

     public string GetLocation()
     {
       return _location;
     }

     public string GetDate()
     {
       return _date;
     }

     public static List<Venue> GetAllVenues()
     {
       List<Venue> allVenues = new List<Venue>{};
       MySqlConnection conn = DB.Connection();
       conn.Open();
       MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
       cmd.CommandText = @"SELECT * FROM venues;";
       MySqlReader rdr = cmd.ExecuteReader() as MySqlReader;
       while(rdr.Read())
       {
         int id = rdr.GetInt32(0);
         string venueName = rdr.GetString(1);
         string location = rdr.GetString(2);
         string date = rdr.GetString(3);
         Venue newVenue = new Venue(venueName, location, date, id);
         allVenues.Add(newVenue);
       }
       conn.Close();
       if(conn !=null)
       {
         conn.Dispose();
       }
       return allVenues;
     }

     public void Save()
     {
       MySqlConnection conn = DB.Connection();
       conn.Open();
       MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
       cmd.CommandText = @"INSERT INTO venues (venue_name) VALUES (@venueName);";

       MySqlParameter venueName = new MySqlParameter();
       venueName.ParameterName = "@venueName";
       venueName.Value = this._venueName;
       cmd.Parameters.Add(venueName);

       cmd.ExecuteNonQuery();
       _id = (int) cmd.LastInsertedId;

       conn.Close();
       if(conn != null)
       {
         conn.Dispose();
       }
     }

     public List<Band> GetBands()
     {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT bands.* FROM venues
          JOIN venue_band ON (venues.id = venue_band.venue_id)
          JOIN bands ON (venue_band.band_id = bands.id)
          WHERE venues.id = @VenueId;";

      MySqlParameter venueIdParameter = new MySqlParameter();
      venueIdParameter.ParameterName = "@VenueId";
      venueIdParameter.Value = _id;
      cmd.Parameters.Add(venueIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Band> bands = new List<Band>{};

      while(rdr.Read())
      {
        int bandId = rdr.GetInt32(0);
        string bandName = rdr.GetString(1);
        string origin = rdr.GetString(2);
        Band newBand = new Band(bandName, origin, bandId);
        bands.Add(newBand);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return bands;
    }

    public void AddBand(Band newBand)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO venue_band (venue_id, band_id) VALUES (@venueId, @bandId);";

      cmd.Parameters.Add(new MySqlParameter("@venueId", _id));
      cmd.Parameters.Add(new MySqlParameter("@bandId", newBand.GetId()));

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM venues; DELETE FROM venue_band;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public static Venue Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM venues WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      int venueId =0;
      string venueName="";
      string location="";
      string date="";

      while (rdr.Read())
      {
        venueId = rdr.GetInt32(0);
        venueName = rdr.GetString(1);
        location = rdr.GetString(2);
        date= rdr.GetString(3);
      }
      Venue foundVenue= new Venue(venueName, location, date, venueId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return foundVenue;
    }

    public void DeleteVenue()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM venues WHERE id = @thisId; DELETE FROM venue_band WHERE venue_id = @thisId;";
      cmd.Parameters.Add(new MySqlParameter("@thisId", _id));

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public void UpdateVenue(string newVenue)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE venues SET venue_name = @venueName WHERE id = @matchId";
      cmd.Parameters.Add(new MySqlParameter("@matchId", _id));
      cmd.Parameters.Add(new MySqlParameter("@venueName", newVenue));

      cmd.ExecuteNonQuery();
      _venueName = newVenue;

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
  }
