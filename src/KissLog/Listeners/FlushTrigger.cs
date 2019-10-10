namespace KissLog.Listeners
{
    public enum FlushTrigger
    {
        /// <summary>
        /// Writes the message as soon as it gets logged
        /// </summary>
        OnMessage,

        /// <summary>
        /// Writes the messages when the NotifyListeners() is called
        /// </summary>
        OnFlush
    }
}
