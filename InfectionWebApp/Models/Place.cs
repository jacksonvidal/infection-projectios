using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;

namespace InfectionWebApp.Models
{
    public class History
    {
        public DateTime Date { get; set; }
        public int Confirmed { get; set; }
        public int Deaths { get; set; }
        public int Recovered { get; set; }
    }

    public class Projection
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public int Confirmed { get; set; }
        public int Deaths { get; set; }
        public int Recovered { get; set; }
    }

    [DataContract]
    public class DataPoint
    {
        public DataPoint(string label, double y, string lineColor)
        {
            this.Label = label;
            this.Y = y;
            this.LineColor = lineColor;
        }

        [DataMember(Name = "label")]
        public string Label = "";

        [DataMember(Name = "y")]
        public Nullable<double> Y = null;

        [DataMember(Name = "lineColor")]
        public string LineColor = "";
    }

    public class Place
    {
        public Place()
        {
            Projections = new List<Projection>();
        }

        public string Key { get; set; }
        public string Api { get; set; }
        public string Name { get; set; }
        public string NameFr { get; set; }
        public string Country { get; set; }
        public string CountryFr { get; set; }
        public string State { get; set; }
        public string StateFr { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Confirmed { get; set; }
        public string Deaths { get; set; }
        public string Recovered { get; set; }

        public List<History> History { get; set; }
        public List<Projection> Projections { get; set; }

        public object Regions { get; set; }

        public DateTime DateUpdate { get; set; }

        public static Place GetInfections()
        {
            var place = new Place();

            using (var client = new WebClient())
            {
                JArray json = JArray.Parse(client.DownloadString("https://kustom.radio-canada.ca/coronavirus/canada_quebec"));
                //JArray json = JArray.Parse(client.DownloadString("https://kustom.radio-canada.ca/coronavirus/brazil"));

                place = (Place)json[0].ToObject(typeof(Place));
            }

            place.ProjectCases(place, 7);

            return place;
        }

        private void ProjectCases(Place place, int days)
        {
            float accumulatedRate = 0;

            var totalDays = place.History.Count(i => i.Confirmed > 0);

            place.History = place.History.Where(i => i.Confirmed > 0).ToList();

            for (int i = 0; i < totalDays; i++)
            {
                int j = i + 1;

                if (j <= (totalDays - 1))
                    accumulatedRate += CalculateRate(place.History[i].Confirmed, place.History[j].Confirmed);
            }

            float pastRate = accumulatedRate / place.History.Count;

            var @case = place.History.FirstOrDefault();

            DateTime date = @case.Date;

            float futureRate = pastRate;

            var projection = new List<Projection>();

            for (int i = 1; i <= days + 1; ++i)
            {
                date = date.AddDays(1);

                if (projection.Count == 0)
                {
                    projection.Add(new Projection
                    {
                        Date = date,
                        Confirmed = (int)(@case.Confirmed + ((@case.Confirmed * futureRate) / 100)),
                    });

                    futureRate = CalculateRate(projection[0].Confirmed, @case.Confirmed) / projection.Count;
                }
                else
                {
                    var current = projection.LastOrDefault();

                    projection.Add(new Projection
                    {
                        Date = date,
                        Confirmed = (int)(current.Confirmed + ((current.Confirmed * futureRate) / 100)),
                    });

                    current = projection.LastOrDefault();

                    var previous = projection[projection.IndexOf(current) - 1];

                    futureRate += CalculateRate(current.Confirmed, previous.Confirmed) / projection.Count;
                }
            }

            place.Projections = projection;
        }

        public List<DataPoint> HistoricalDataSet(Place place)
        {
            return place.History.OrderBy(h => h.Date).Select(h => new DataPoint(h.Date.ToString("dd/MM/yyy"), h.Confirmed, "blue")).ToList();
        }

        public List<DataPoint> ProjectedDataSet(Place place)
        {
            return place.Projections.OrderBy(h => h.Date).Select(h => new DataPoint(h.Date.ToString("dd/MM/yyy"), h.Confirmed, "red")).ToList();
        }

        private float CalculateRate(int current, int previous)
        {
            float rate = 0;

            float change = current - previous;

            if (previous > 0)
                rate += change / previous * 100;

            return rate;
        }
    }
}