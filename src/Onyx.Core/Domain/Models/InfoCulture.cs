using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Core.Domain.Models
{
    public class InfoCulture
    {
        public string Title { get; set; } = "";
        public string Info { get; set; } = "";
        public string ImagePath { get; set; } = "";
        public string Content { get; set; } = "";
        public string ContentTitle { get; set; } = "";
        public ContentType ContentType { get; set; }
    }

    public enum ContentType
    {
        None = 0,
        Pdf = 1,
        VideoYoutube = 2,
    }
}