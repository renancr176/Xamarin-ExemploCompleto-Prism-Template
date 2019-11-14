using System.Collections.Generic;

namespace TmsCollectorAndroid.TmsApi.Models.Responses
{
    public class GetBillOfLadingInformationsByBarCodeResponseModel
    {
        public IEnumerable<string> BillOfLadingInformations { get; set; }
    }
}