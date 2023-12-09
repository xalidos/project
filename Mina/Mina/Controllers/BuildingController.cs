using Microsoft.AspNetCore.Mvc;
using Mina.Models;
using Mina.Models.BuildingDtos;
using Mina.Services;
using NetTopologySuite.Geometries;
using Newtonsoft.Json.Linq;

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

        [HttpGet]
        public async Task<List<Geometry>> GetAll()
        {
            var list = await _buildingService.GetAllAsync();

            return list.Select(x => x.Geometry).ToList();
        }

        [HttpGet("{id}")]
        public async Task<Geometry> Get(int id)
        {
            var data = await _buildingService.GetByIdAsync(id);

            return data.Geometry;
        }


        [HttpGet("GetPOI/{id}")]
        public async Task<List<Geometry>> GetPOI(int id)
        {
            return await _buildingService.GetPoiAsync(id);
        }

        [HttpPost]
        public async Task<int> Add([FromBody] CreateBuildingDto value)
        {
            return await _buildingService.CreateAsync(value);
        }

        [HttpPut("{id}")]
        public async Task Update(int id, [FromBody] UpdateBuildingDto value)
        {
            await _buildingService.UpdateAsync(id, value);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _buildingService.DeleteAsync(id);
        }
    }
}
