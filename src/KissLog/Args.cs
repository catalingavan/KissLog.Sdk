namespace KissLog
{
    public class Args
    {
        private readonly object[] _args;
        public Args(params object[] args)
        {
            _args = args;
        }

        internal object[] GetArgs()
        {
            return _args;
        }
    }
}
