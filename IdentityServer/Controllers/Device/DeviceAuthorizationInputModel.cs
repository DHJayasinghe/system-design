using BnA.IAM.Presentation.API.Controllers.Consent;

namespace BnA.IAM.Presentation.API.Controllers.Device
{
    public class DeviceAuthorizationInputModel : ConsentInputModel
    {
        public string UserCode { get; set; }
    }
}