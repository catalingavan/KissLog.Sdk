namespace KissLog.CloudListeners.RequestLogsListener
{
    public interface IObfuscationService
    {
        bool ShouldObfuscate(string key, string value, string propertyName);
    }
}
