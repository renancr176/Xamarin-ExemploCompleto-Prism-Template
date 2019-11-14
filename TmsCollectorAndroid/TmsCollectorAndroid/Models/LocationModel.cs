namespace TmsCollectorAndroid.Models
{
    public class LocationModel
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public LocationModel(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}