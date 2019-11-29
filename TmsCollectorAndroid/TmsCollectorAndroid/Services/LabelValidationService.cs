using System.Text.RegularExpressions;
using TmsCollectorAndroid.Interfaces.Services;

namespace TmsCollectorAndroid.Services
{
    public class LabelValidationService : ILabelValidationService
    {
        public Regex CommonLabelValidator { get; private set; }
        public Regex MotherLabelValidator { get; private set; }

        public LabelValidationService()
        {
            CommonLabelValidator = new Regex("[0-9]{13}[a-zA-Z][0-9]{6}");
            MotherLabelValidator = new Regex("[0-9]{21}");
        }


        public bool ValidateCommonLabel(string commonLabel)
        {
            return CommonLabelValidator.IsMatch(commonLabel);
        }

        public bool ValidateMotherLabel(string motherLabel)
        {
            return MotherLabelValidator.IsMatch(motherLabel);
        }
    }
}