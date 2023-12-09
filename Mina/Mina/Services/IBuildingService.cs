using Mina.Models.BuildingDtos;
using NetTopologySuite.Geometries;

namespace Mina.Services;

public interface IBuildingService
{
    Task<BuildingDto> GetByIdAsync(int id);
    Task<List<BuildingDto>> GetAllAsync();
    Task<int> CreateAsync(CreateBuildingDto entity);
    Task UpdateAsync(int id, UpdateBuildingDto value);
    Task DeleteAsync(int id);
    Task<List<Geometry>> GetPoiAsync(int id);
}