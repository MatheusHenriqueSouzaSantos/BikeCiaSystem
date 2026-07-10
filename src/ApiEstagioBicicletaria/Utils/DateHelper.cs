namespace ApiEstagioBicicletaria.Utils
{
    public static class DateHelper
    {
        public static DateTime AgoraBrasil()
        {
            var fusoBrasil = TimeZoneInfo.FindSystemTimeZoneById(
                OperatingSystem.IsWindows()
                    ? "E. South America Standard Time"
                    : "America/Sao_Paulo");

            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, fusoBrasil);
        }
    }
}
