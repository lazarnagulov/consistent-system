using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ConsistentSystem.Common.Models
{
    [DataContract]
    public class Measurement
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime Timestamp { get; set; }
        [DataMember]
        public double Temperature { get; set; }
    }
}