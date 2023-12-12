using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mina.Entities
{
    [Table("bina")]
    public class Building : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public new int Id { get; set; }

        [Column("geometry", TypeName = "geometry")]
        //[Column(TypeName = "geometry(Polygon,4326)")]
        public Geometry Geometry { get; set; }
    }
}
