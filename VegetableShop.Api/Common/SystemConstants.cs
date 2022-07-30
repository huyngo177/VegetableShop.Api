namespace VegetableShop.Api.Common
{
    public static class SystemConstants
    {
        public const string ConnectionString = "DefaultConnection";
    }
    public enum Status
    {
        Available,
        Unavailable
    }
    public static class StatusExtensions
    {
        public static string GetString(this Status stt)
        {
            switch (stt)
            {
                case Status.Available:
                    return "Available";
                default:
                    return "Unavailable";
            }
        }
    }
}
