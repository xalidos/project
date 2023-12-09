using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Mina.Entities;

[Table("poi")]
public class POI : Entity
{
    [Column("wkb_geometry", TypeName = "geometry")]
    public Geometry Geometry { get; set; }
}