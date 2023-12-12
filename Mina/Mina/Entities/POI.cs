using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Mina.Entities;

//[Table("poi")]
//public class POI : Entity
//{
//    [Column("wkb_geometry", TypeName = "geometry")]
//    public Geometry Geometry { get; set; }

//}


//public class Future
//{
//    [Column("wkb_geometry", TypeName = "geometry")]
//    public Geometry Geometry { get; set; }
//    public string Type { get; set; }
//    public string Futures { get; set; }
//    public string Properties { get; set; }
//}