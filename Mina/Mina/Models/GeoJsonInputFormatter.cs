using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System.Text;
using Mina.Models.BuildingDtos;
using NetTopologySuite.Geometries;

namespace Mina.Models
{
    public class GeoJsonInputFormatter : TextInputFormatter
    {
        public GeoJsonInputFormatter()
        {
            SupportedMediaTypes.Add("application/geo+json");
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanReadType(Type type)
        {
            // You can adjust this condition to check for the specific type you want to deserialize.
            return type == typeof(Geometry);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            using (var reader = new StreamReader(context.HttpContext.Request.Body, encoding))
            {
                try
                {
                    var content = await reader.ReadToEndAsync();
                    var result = YourDeserializationLogic(content);

                    return await InputFormatterResult.SuccessAsync(result);
                }
                catch
                {
                    return await InputFormatterResult.FailureAsync();
                }
            }
        }

        private Geometry YourDeserializationLogic(string content)
        {
            var deserializedModel = JsonConvert.DeserializeObject<Geometry>(content);
            return deserializedModel;
        }
    }

    public class GeoJsonFeature
    {
        public string Type { get; } = "Feature";
        //public int Id { get; set; }
        public Geometry Geometry { get; set; }
    }

    public class GeoJsonFeatureCollection
    {
        public string Type { get; } = "FeatureCollection";
        public List<GeoJsonFeature> Features { get; set; }
    }

    public static class GeoJsonConverter
    {
        public static string ConvertToGeoJson(Geometry geometry)
        {
            var feature = new GeoJsonFeature
            {
                Geometry = geometry
            };

            var featureCollection = new GeoJsonFeatureCollection
            {
                Features = new List<GeoJsonFeature> { feature }
            };

            return JsonConvert.SerializeObject(featureCollection);
        }
    }


}
