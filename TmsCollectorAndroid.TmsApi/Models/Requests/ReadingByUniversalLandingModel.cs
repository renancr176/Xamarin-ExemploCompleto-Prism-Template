﻿namespace TmsCollectorAndroid.TmsApi.Models.Requests
{
    public class ReadingByUniversalLandingModel
    {
        public string BarCode { get; private set; }

        public int UnitLocalId { get; private set; }

        public string MacAddress { get; private set; }

        public ReadingByUniversalLandingModel(string barCode, int unitLocalId, string macAddress)
        {
            BarCode = barCode;
            UnitLocalId = unitLocalId;
            MacAddress = macAddress;
        }
    }
}