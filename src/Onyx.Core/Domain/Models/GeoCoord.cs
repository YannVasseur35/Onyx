namespace Onyx.Core.Domain.Models
{
    public class GeoCoord
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime? At { get; set; }
        public double? Accuracy { get; set; }
    }
}