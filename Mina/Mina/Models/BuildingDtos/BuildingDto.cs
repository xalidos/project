using NetTopologySuite.Geometries;

namespace Mina.Models.BuildingDtos
{
    public class BuildingDto
    {
        public int Id { get; set; }
        public Geometry Geometry { get; set; }
    }

    public class CreateBuildingDto
    {
        public string Type { get; set; }
        public Geometry Geometry { get; set; }
    }

    public class UpdateBuildingDto
    {
        public string Type { get; set; }
        public Geometry Geometry { get; set; }
    }
}
