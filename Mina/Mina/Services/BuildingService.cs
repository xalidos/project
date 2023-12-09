using Mina.Entities;
using Mina.Models.BuildingDtos;
using Mina.Repositories;
using NetTopologySuite.Geometries;

namespace Mina.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IPoiRepository _poiRepository;

        public BuildingService(IBuildingRepository buildingRepository, IPoiRepository poiRepository)
        {
            _buildingRepository = buildingRepository;
            _poiRepository = poiRepository;
        }

        public async Task<List<BuildingDto>> GetAllAsync()
        {
            var entity = await _buildingRepository.GetAllAsync();

            var list = entity.Select(x => new BuildingDto
            {
                Id = x.Id,
                Geometry = x.Geometry
            }).ToList();

            return list;
        }

        public async Task<BuildingDto> GetByIdAsync(int id)
        {
            var entity = await _buildingRepository.GetByIdAsync(id);

            return new BuildingDto
            {
                Id = entity.Id,
                Geometry = entity.Geometry
            };
        }

        public async Task<List<Geometry>> GetPoiAsync(int id)
        {
            var poiGeometries = (from poi in _poiRepository.GetAll()
                select poi.Geometry).ToList();

            var binaGeometry = (from bina in _buildingRepository.GetAll()
                where bina.Id == id
                select bina.Geometry).ToList();

            var all = poiGeometries.Concat(binaGeometry).ToList();

            return all;
        }

        public async Task<int> CreateAsync(CreateBuildingDto entity)
        {
            var building = new Building
            {
                Geometry = entity.Geometry
            };

            await _buildingRepository.AddAsync(building);

            await _buildingRepository.SaveChangesAsync();

            return building.Id;
        }

        public async Task DeleteAsync(int id)
        {
            await _buildingRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(int id, UpdateBuildingDto value)
        {
            var entity = await _buildingRepository.GetByIdAsync(id);

            if (entity is not null)
            {
                entity.Geometry = value.Geometry;

                await _buildingRepository.UpdateAsync(entity);

                await _buildingRepository.SaveChangesAsync();
            }
        }
    }
}
