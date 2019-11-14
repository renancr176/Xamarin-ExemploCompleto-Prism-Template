namespace TmsCollectorAndroid.Api_Old.Models
{
    public class EmployeeViewInfoModel
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Description { get; set; }
        public int UnitLinked { get; set; }

        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public bool UserMaster { get; set; }
        public bool UserSessionControl { get; set; }

        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyDescription { get; set; }
        public string CompanyAcronym { get; set; }

        public bool Authenticated { get; set; }
        public bool IsManager { get; set; }

        public string ExceptionMessage { get; set; }

        public int SessionId { get; set; }
    }
}