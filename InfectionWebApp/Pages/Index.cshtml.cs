using InfectionWebApp.Data;
using InfectionWebApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace InfectionWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ProjectionDataContext _projectionDataContext;

        public IndexModel(ILogger<IndexModel> logger, ProjectionDataContext projectionDataContext)
        {
            _logger = logger;
            _projectionDataContext = projectionDataContext;
        }

        public void OnGet()
        {
            var place = Place.GetInfections();

            var infectionDataPoints = place.HistoricalDataSet(place);
            infectionDataPoints.AddRange(place.ProjectedDataSet(place));

            var projections = _projectionDataContext.Projections.ToList();

            int infectionsAvoided = 0;

            if (!_projectionDataContext.Projections.Any(e => e.Date == place.DateUpdate.AddDays(7).Date))
            {
                _projectionDataContext.Add(place.Projections.LastOrDefault());
                _projectionDataContext.SaveChanges();
            }



            foreach (var projection in projections.OrderByDescending(e => e.Confirmed))
                infectionsAvoided = projection.Confirmed - infectionsAvoided;

            infectionsAvoided = (infectionsAvoided * -1);

            ViewData["Place"] = place;
            ViewData["InfectionDataPoints"] = JsonConvert.SerializeObject(infectionDataPoints);
            ViewData["PastProjection"] = projections.OrderBy(e => e.Date).ToList();
            ViewData["InfectionsAvoided"] = infectionsAvoided;
        }
    }
}
