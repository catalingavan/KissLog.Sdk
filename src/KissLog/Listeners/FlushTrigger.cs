namespace KissLog.Listeners
{
    public enum FlushTrigger
    {
        /// <summary>
        /// Writes the messages as soon as they get logged
        /// </summary>
        OnMessage,

        /// <summary>
        /// Writes the messages at the end of the request, or when the NotifyListeners() is called
        /// </summary>
        OnFlush
    }
}
