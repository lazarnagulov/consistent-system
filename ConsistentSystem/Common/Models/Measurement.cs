using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsistentSystem.Common.Models
{
    public class Measurement
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double Temperature { get; set; }
    }
}