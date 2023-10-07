namespace Onyx.Core.Domain.Models
{
    public class Instruction
    {
        public bool IsPrivatePlace { get; set; } = false;
        public string Notice { get; set; } = "";
        public string Warning { get; set; } = "";
        public string NoticeImagePath { get; set; } = "";
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}