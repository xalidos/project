using Mina.DbContexts;
using Mina.Entities;

namespace Mina.Repositories;

public class BuildingRepository : Repository<Building>, IBuildingRepository
{
    public BuildingRepository(MinaDbContext context) : base(context)
    {
    }
}