using Microsoft.AspNetCore.Mvc;
using Mina.Models.BuildingDtos;
using Mina.Services;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace Mina.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingService _buildingService;

        public BuildingController(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        [HttpGet("GetPoint/{id}")]
        public async Task<Feature> GetPoint(string id)
        {
            var data = await _buildingService.GetPoint(id);

            return data;
        }

        [HttpGet("GetLinestring/{id}")]
        public async Task<Feature> GetLinestring(string id)
        {
            var data = await _buildingService.GetLinestring(id);

            return data;
        }

        [HttpGet("GetPolygon/{id}")]
        public async Task<Feature> GetPolygon(int id)
        {
            var data = await _buildingService.GetPolygon(id);

            return data;
        }

        [HttpGet("GetPointAndPolygon")]
        public async Task<FeatureCollection> GetPointAndPolygon()
        {
            var list = await _buildingService.GetPointAndPolygon();

            return list;
        }


        [HttpPost("AddPoint")]
        public async Task AddPoint([FromBody] Feature value)
        {
            await _buildingService.CheckPoint(value.Geometry);

            await _buildingService.AddPoint(value);
        }

        [HttpPost("AddLinestring")]
        public async Task AddLinestring([FromBody] Feature value)
        {
            await _buildingService.CheckLinestring(value.Geometry);

            await _buildingService.AddLinestring(value);
        }

        [HttpPost("AddPolygon")]
        public async Task AAddPolygon([FromBody] Feature value)
        {
            await _buildingService.CheckPolygon(value.Geometry);

            await _buildingService.AddPolygon(value);
        }
    }
}
