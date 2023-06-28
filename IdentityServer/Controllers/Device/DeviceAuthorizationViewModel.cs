using BnA.IAM.Presentation.API.Controllers.Consent;

namespace BnA.IAM.Presentation.API.Controllers.Device
{
    public class DeviceAuthorizationViewModel : ConsentViewModel
    {
        public string UserCode { get; set; }
        public bool ConfirmUserCode { get; set; }
    }
}