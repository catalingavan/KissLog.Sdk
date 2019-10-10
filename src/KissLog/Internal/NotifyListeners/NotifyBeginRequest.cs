using KissLog.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace KissLog.Internal.NotifyListeners
{
    public class NotifyBeginRequest
    {

    }

    public interface IBeginRequestListener
    {
        void OnBeginRequest(WebRequestProperties webRequestProperties);
    }

    public interface IOnMessageListener
    {
        void OnMessage(LogMessage message);
    }
}

namespace KissLog
{
    public interface IBeginRequestListener
    {
        void OnBeginRequest(WebRequestProperties webRequestProperties);
    }

    public interface ILogMessageListener
    {
        void OnMessage(LogMessage message);
    }
}
