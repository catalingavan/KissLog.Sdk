namespace KissLog.ReadStream
{
    internal class NullReadStream : IReadStreamStrategy
    {
        public ReadStreamResult Read()
        {
            return new ReadStreamResult();
        }
    }
}
