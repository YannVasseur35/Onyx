using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Infrastructure.Models.Entities
{
    public class WeatherForecastEntity : IAudit
    {
        public WeatherForecastEntity()
        {
            Init(Guid.NewGuid(), DateTime.UtcNow);
        }

        public void Init(Guid id, DateTime createdAt)
        {
            Id = id;
            CreatedAt = createdAt;
            ModifiedAt = createdAt;
        }

        public Guid Id { get; private set; }
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }
        public string? Summary { get; set; }
        public bool Current { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Humidity { get; set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime ModifiedAt { get; set; }
    }
}