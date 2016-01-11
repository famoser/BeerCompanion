using System;
using System.Runtime.Serialization;

namespace Famoser.BeerCompanion.Business.Models
{
    [DataContract]
    public class Beer
    {
        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public DateTime DrinkTime { get; set; }

        [DataMember]
        public bool Posted { get; set; }

        [DataMember]
        public bool DeletePending { get; set; }
    }
}
