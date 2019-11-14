using TmsCollectorAndroid.TmsApi.Models;
using TmsCollectorAndroid.TmsApi.Models.Responses;

namespace TmsCollectorAndroid.Models
{
    public class UserModel
    {
        public UnitViewInfoModel Unit { get; private set; }
        public EmployeeViewInfoModel EmployeeAuthenticated { get; private set; }
        public string CompanyAcronym => EmployeeAuthenticated?.CompanyAcronym;
        public string Name => EmployeeAuthenticated?.Description;

        public UserModel(UnitViewInfoModel unit, EmployeeViewInfoModel employeeAuthenticated)
        {
            Unit = unit;
            EmployeeAuthenticated = employeeAuthenticated;
        }
    }
}