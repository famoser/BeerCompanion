﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.BeerCompanion.Data.Entities.Communication.Base;
using Famoser.BeerCompanion.Data.Entities.Communication.Generic;

namespace Famoser.BeerCompanion.Data.Entities.Communication
{
    public class DrinkerResponse : BaseResponse
    {
        public DrinkerEntity Drinker { get; set; }
    }
}
