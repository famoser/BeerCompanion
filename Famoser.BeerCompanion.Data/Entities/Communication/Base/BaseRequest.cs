using System;
using System.Runtime.Serialization;
using Famoser.BeerCompanion.Common.Framework.Logging;
using Famoser.BeerCompanion.Data.Enums;

namespace Famoser.BeerCompanion.Data.Entities.Communication.Base
{
    [DataContract]
    public class BaseRequest
    {
        public BaseRequest(PossibleActions action, Guid guid)
        {
            PossibleAction = action;
            Guid = guid;
        }

        private PossibleActions PossibleAction { get; }

        [DataMember]
        public Guid Guid { get; }
        [DataMember]
        public string Action
        {
            get
            {
                if (PossibleAction == PossibleActions.Add)
                    return "add";
                if (PossibleAction == PossibleActions.Exists)
                    return "exists";
                if (PossibleAction == PossibleActions.Remove)
                    return "remove";
                if (PossibleAction == PossibleActions.Update)
                    return "update";
                if (PossibleAction == PossibleActions.Autheticate)
                    return "authenticate";
                if (PossibleAction == PossibleActions.Deautheticate)
                    return "deauthenticate";
                if (PossibleAction == PossibleActions.Sync)
                    return "sync";
                if (PossibleAction == PossibleActions.RemoveForeign)
                    return "removeforeign";
                LogHelper.Instance.Log(LogLevel.WtfAreYouDoingError, this, "Unknown Possible Action used!");
                return "";
            }
        }
    }
}
