using System;

namespace KissLog.Apis.v1.Listeners
{
    public enum ApiVersion
    {
        [Obsolete("v1 will be deprecated in the near future. Please use v2", false)]
        v1,
        v2
    }
}
