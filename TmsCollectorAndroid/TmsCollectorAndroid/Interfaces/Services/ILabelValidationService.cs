namespace TmsCollectorAndroid.Interfaces.Services
{
    public interface ILabelValidationService
    {
        bool ValidateCommonLabel(string commonLabel);
        bool ValidateMotherLabel(string motherLabel);
    }
}