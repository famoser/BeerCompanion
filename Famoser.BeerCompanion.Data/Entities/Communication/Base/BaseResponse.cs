﻿using System.Runtime.Serialization;

namespace Famoser.BeerCompanion.Data.Entities.Communication.Base
{
    [DataContract]
    public class BaseResponse 
    {
        public bool IsSuccessfull
        {
            get { return ErrorMessage == null; }
        }

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
