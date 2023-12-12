using Microsoft.AspNetCore.Http.Features;
using Mina.Entities;
using Mina.Models.BuildingDtos;
using Mina.Repositories;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using System.Drawing;
using System.Runtime.InteropServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using FeatureCollection = NetTopologySuite.Features.FeatureCollection;
using POI = Mina.Models.BuildingDtos.POI;
using Point = NetTopologySuite.Geometries.Point;

namespace Mina.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly string _connString = "Server=localhost;Database=Mina;User Id=postgres;Password=123456;";

        public async Task<Feature> GetPoint(string id)
        {
            var feature = new Feature();

            await using var conn = new NpgsqlConnection(_connString);
            conn.Open();

            string query = @"select wkb_geometry from poi where id = @id";

            await using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using NpgsqlDataReader reader = cmd.ExecuteReader();

            while (await reader.ReadAsync())
            {
                var building = new Building
                {
                    Geometry = GetValueOrDefault<Geometry>(reader, "wkb_geometry")
                };

                feature = new Feature(building.Geometry, new AttributesTable());
            }

            await conn.CloseAsync();

            return feature;
        }

        public async Task<Feature> GetLinestring(string id)
        {
            var feature = new Feature();

            await using var conn = new NpgsqlConnection(_connString);
            conn.Open();

            string query = @"select wkb_geometry from yollar where id = @id";

            await using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using NpgsqlDataReader reader = cmd.ExecuteReader();

            while (await reader.ReadAsync())
            {
                var building = new Building
                {
                    //Id = reader.GetInt32(0),
                    Geometry = GetValueOrDefault<Geometry>(reader, "wkb_geometry")
                };

                feature = new Feature(building.Geometry, new AttributesTable());
            }

            await conn.CloseAsync();

            return feature;
        }

        public async Task<Feature> GetPolygon(int id)
        {
            var feature = new Feature();

            await using var conn = new NpgsqlConnection(_connString);
            conn.Open();

            string query = @"select geometry from bina where id = @id";

            await using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using NpgsqlDataReader reader = cmd.ExecuteReader();

            while (await reader.ReadAsync())
            {
                var building = new Building
                {
                    //Id = reader.GetInt32(0),
                    Geometry = GetValueOrDefault<Geometry>(reader, "geometry")
                };

                feature = new Feature(building.Geometry, new AttributesTable());
            }

            await conn.CloseAsync();

            return feature;
        }

        public async Task<FeatureCollection> GetPointAndPolygon()
        {
            var featureCollection = new FeatureCollection();

            await using var conn = new NpgsqlConnection(_connString);
            conn.Open();

            string query = @"select wkb_geometry from poi
                            union all
                            select geometry from bina";

            await using var cmd = new NpgsqlCommand(query, conn);

            await using NpgsqlDataReader reader = cmd.ExecuteReader();

            while (await reader.ReadAsync())
            {
                var building = new Building
                {
                    Geometry = GetValueOrDefault<Geometry>(reader, "wkb_geometry")
                };

                featureCollection.Add(new Feature(building.Geometry, new AttributesTable()));
            }

            await conn.CloseAsync();

            return featureCollection;
        }

        public async Task CheckPoint(Geometry geometry)
        {
            await using var conn = new NpgsqlConnection(_connString);

            geometry.SRID = 4326;

            try
            {
                await conn.OpenAsync();

                string queryCheck = @"select count(*) from poi
                                where wkb_geometry = ST_GeomFromText(@geometry, 4326)";

                await using var cmdCheck = new NpgsqlCommand(queryCheck, conn);

                cmdCheck.Parameters.AddWithValue("@geometry", geometry.ToText());

                int count = Convert.ToInt32(await cmdCheck.ExecuteScalarAsync());

                if (count > 0)
                {
                    throw new Exception("This geometry exist!");
                }

            }
            catch (Exception e)
            {

            }

            await conn.CloseAsync();

        }

        public async Task CheckLinestring(Geometry geometry)
        {
            await using var conn = new NpgsqlConnection(_connString);

            geometry.SRID = 4326;

            try
            {
                await conn.OpenAsync();

                string queryCheck = @"select count(*) from poi
                                where wkb_geometry = ST_GeomFromText(@geometry, 4326)";

                await using var cmdCheck = new NpgsqlCommand(queryCheck, conn);

                cmdCheck.Parameters.AddWithValue("@geometry", geometry.ToText());

                int count = Convert.ToInt32(await cmdCheck.ExecuteScalarAsync());

                if (count > 0)
                {
                    throw new Exception("This geometry exist!");
                }

            }
            catch (Exception e)
            {

            }

            await conn.CloseAsync();

        }

        public async Task CheckPolygon(Geometry geometry)
        {
            await using var conn = new NpgsqlConnection(_connString);

            geometry.SRID = 4326;

            try
            {
                await conn.OpenAsync();

                string queryCheck = @"select count(*) from poi
                                where wkb_geometry = ST_GeomFromText(@geometry, 4326)";

                await using var cmdCheck = new NpgsqlCommand(queryCheck, conn);

                cmdCheck.Parameters.AddWithValue("@geometry", geometry.ToText());

                int count = Convert.ToInt32(await cmdCheck.ExecuteScalarAsync());

                if (count > 0)
                {
                    throw new Exception("This geometry exist!");
                }

            }
            catch (Exception e)
            {

            }

            await conn.CloseAsync();

        }

        public async Task<string> AddPoint(Feature feature)
        {
            await using var conn = new NpgsqlConnection(_connString);

            feature.Geometry.SRID = 4326;
            var insertedId = Guid.Empty;

            try
            {
                await conn.OpenAsync();

                string query = "INSERT INTO poi (id, wkb_geometry) VALUES (@id, @geometry)";

                await using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@geometry", feature.Geometry);
                cmd.Parameters.AddWithValue("@id", Guid.NewGuid());

                var result = await cmd.ExecuteScalarAsync();

                if (result != null && result != DBNull.Value)
                {
                    insertedId = (Guid)result;
                }

                await conn.CloseAsync();

            }

            catch (Exception e)
            {
                await conn.CloseAsync();
            }

            return insertedId.ToString();
        }

        public async Task AddLinestring(Feature feature)
        {
            await using var conn = new NpgsqlConnection(_connString);

            feature.Geometry.SRID = 4326;

            try
            {
                await conn.OpenAsync();

                string query = "INSERT INTO yollar (id, wkb_geometry) VALUES (@id, @geometry)";

                await using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@geometry", feature.Geometry);
                cmd.Parameters.AddWithValue("@id", Guid.NewGuid());

                await cmd.ExecuteNonQueryAsync();

                await conn.CloseAsync();

            }
            catch (Exception e)
            {
                await conn.CloseAsync();
            }
        }

        public async Task AddPolygon(Feature feature)
        {
            await using var conn = new NpgsqlConnection(_connString);

            feature.Geometry.SRID = 4326;

            try
            {
                await conn.OpenAsync();

                string query = "INSERT INTO bina (geometry) VALUES (@geometry)";

                await using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@geometry", feature.Geometry);


                await cmd.ExecuteNonQueryAsync();

                await conn.CloseAsync();

            }
            catch (Exception e)
            {
                await conn.CloseAsync();
            }
        }

        private T GetValueOrDefault<T>(NpgsqlDataReader reader, string columnName)
        {
            object value = reader[columnName];
            if (value == DBNull.Value)
            {
                return default(T);
            }
            return (T)value;
        }
    }
}
