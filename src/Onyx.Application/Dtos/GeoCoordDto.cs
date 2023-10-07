namespace Onyx.Application.Dtos
{
    public class GeoCoordDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime? At { get; set; }
        public double? Accuracy { get; set; }
    }
}
