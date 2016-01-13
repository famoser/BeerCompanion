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
			_possibleAction = action;
			_guid = guid;
        }

		private PossibleActions _possibleAction;
		private Guid _guid;

        [DataMember]
		public Guid Guid { get { return _guid; } }
        [DataMember]
        public string Action
        {
            get
            {
				if (_possibleAction == PossibleActions.Add)
                    return "add";
				if (_possibleAction == PossibleActions.Exists)
                    return "exists";
				if (_possibleAction == PossibleActions.Remove)
                    return "remove";
				if (_possibleAction == PossibleActions.Update)
                    return "update";
				if (_possibleAction == PossibleActions.Autheticate)
                    return "authenticate";
				if (_possibleAction == PossibleActions.Deautheticate)
                    return "deauthenticate";
				if (_possibleAction == PossibleActions.Sync)
                    return "sync";
				if (_possibleAction == PossibleActions.RemoveForeign)
                    return "removeforeign";
                LogHelper.Instance.Log(LogLevel.WtfAreYouDoingError, this, "Unknown Possible Action used!");
                return "";
            }
        }
    }
}
