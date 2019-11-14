using System.ComponentModel;

namespace TmsCollectorAndroid.TmsApi.Enums
{
    public enum ExceptionCodeEnum
    {
        [Description("")]
        NoError,

        #region Códigos de erros (Romaneio)

        [Description("ROMERR001")]
        TrafficScheduleNotFound,
        [Description("ROMERR002")]
        UnitSendInvalid,
        [Description("ROMERR003")]
        BillOfLadingInOtherPackinglist,
        [Description("ROMERR004")]
        PackRead,
        [Description("ROMERR005")]
        PackinglistInvalid,
        [Description("ROMERR006")]
        PackinglistNotFound,
        [Description("ROMERR007")]
        BarcideNotFound,
        [Description("ROMERR008")]
        BillOfLadingWithOtherUnitDestination,
        [Description("ROMERR009")]
        RequestUnitDestination,
        [Description("ROMERR010")]
        UnitDestinationInvalid,
        [Description("ROMERR011")]
        AmountSealsSuperior,
        [Description("ROMERR012")]
        AmountSealsInferior,
        [Description("ROMERR013")]
        ExistsPacksUnread,
        [Description("ROMERR014")]
        SealWarning,
        [Description("ROMERR015")]
        WeightAlert,
        [Description("ROMERR016")]
        PackNotFoundWarehouse,
        [Description("ROMERR017")]
        PackNotLoading,
        [Description("ROMERR018")]
        IncompleteBoardingBol,
        [Description("ROMERR019")]
        CompleteJointUnit,
        [Description("ROMERR020")]
        RequestWarehousePassword,
        [Description("ROMERR021")]
        CteUnitised,
        [Description("ROMERR022")]
        PackinglistAccessoryEmpty,
        [Description("ROMERR023")]
        PackinglistAccessoryDelete,
        [Description("ROMERR024")]
        ExistsPackinglistDelivery,
        [Description("ROMERR025")]
        PackinglistDeliveryDelete,

        #endregion

        #region Códigos de erros (Controle Operacional)

        [Description("CTRERR001")]
        VehicleNotBoardingLanding,
        [Description("CTRERR002")]
        VehicleOpened,
        [Description("CTRERR003")]
        VehicleInBoarding,
        [Description("CTRERR004")]
        VehicleInBoardingClosed,
        [Description("CTRERR005")]
        VehicleInLanding,
        [Description("CTRERR006")]
        VehicleInLandingClosed,
        [Description("CTRERR007")]
        VehicleInBoardingPaused,
        [Description("CTRERR008")]
        VehicleInLandingPaused,
        [Description("CTRERR009")]
        VehicleClosed,
        [Description("CTRERR010")]
        LandingUnlicensed,
        [Description("CTRERR011")]
        EmployeeNotRelatedTeam,
        [Description("CTRERR012")]
        EmployeeNotPermissionCollector

        #endregion

    }
}