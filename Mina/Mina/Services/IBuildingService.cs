using Mina.Entities;
using Mina.Models.BuildingDtos;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;

namespace Mina.Services;

public interface IBuildingService
{
    Task<FeatureCollection> GetPointAndPolygon();
    Task<Feature> GetPoint(string id);
    Task<Feature> GetLinestring(string id);
    Task<Feature> GetPolygon(int id);
    Task<string> AddPoint(Feature feature);
    Task CheckPoint(Geometry geometry);
    Task CheckLinestring(Geometry geometry);
    Task CheckPolygon(Geometry geometry);
    Task AddLinestring(Feature feature);
    Task AddPolygon(Feature feature);
}