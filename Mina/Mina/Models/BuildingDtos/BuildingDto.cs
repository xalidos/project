using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mina.Entities;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Features;

namespace Mina.Models.BuildingDtos
{
    public class BuildingDto
    {
        public Point Geometry { get; set; }
    }

    public class GeoJsonData
    {
        public int Type { get; set; }
        public List<Feature> Features { get; set; }
    }

    public class Properties
    {
    }

    public class CreateBuildingDto
    {
        public string Type { get; set; }
        public Point Geometry { get; set; }
    }

    public class UpdateBuildingDto
    {
        public string Type { get; set; }
        public Point Geometry { get; set; }
    }


    public class POI
    {
        public string Type { get; set; }
        public List<Future> Futures { get; set; }
    }

    public class Future
    {
        public Geometry Geometry { get; set; }
        public string Type { get; set; }
        public string Futures { get; set; }
        public string Properties { get; set; }
    }
}
