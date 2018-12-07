using System;
using System.Collections.Generic;

namespace KissLog
{
    internal class CapturedExceptionComparer : IEqualityComparer<CapturedException>
    {
        public bool Equals(CapturedException x, CapturedException y)
        {
            // If reference same object including null then return true
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            // If one object null the return false
            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
            {
                return false;
            }

            if (string.Compare(x.ExceptionType, y.ExceptionType, StringComparison.OrdinalIgnoreCase) != 0)
                return false;

            if (string.Compare(x.ExceptionMessage, y.ExceptionMessage, StringComparison.OrdinalIgnoreCase) != 0)
                return false;

            if (string.Compare(x.Exception, y.Exception, StringComparison.OrdinalIgnoreCase) != 0)
                return false;

            return true;
        }

        public int GetHashCode(CapturedException obj)
        {
            return obj.GetHashCode();
        }
    }
}
