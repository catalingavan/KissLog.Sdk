using Microsoft.AspNet.SignalR.Client;
using System;

namespace KissLog.AspNet.SignalR.Client
{
    public class KissLogSignalRClient
    {
        public static string KissLogApiUrl = "https://api.kisslog.net";

        private readonly string _organizationId;
        private readonly string _environment;

        private readonly HubConnection _hubConnection;
        private readonly IHubProxy _logsStreamProxy;

        private const int MaxConnectAttempts = 10;
        private int ConnectAttempts = 0;

        public KissLogSignalRClient(
            string organizationId,
            string environment)
        {
            _organizationId = organizationId;
            _environment = environment;

            _hubConnection = new HubConnection($"{KissLogApiUrl}/signalr", useDefaultUrl: false);
            _logsStreamProxy = _hubConnection.CreateHubProxy("LogsStreamHub");
        }

        public void Connect()
        {
            if (ConnectAttempts > MaxConnectAttempts)
                return;

            if (_hubConnection.State == ConnectionState.Connected || _hubConnection.State == ConnectionState.Connecting || _hubConnection.State == ConnectionState.Reconnecting)
                return;

            try
            {
                _hubConnection.Start().Wait();
                ConnectAttempts = 0;
            }
            catch(Exception)
            {
                ConnectAttempts++;
            }
        }

        private bool IsConnected()
        {
            if (_hubConnection == null)
                return false;

            if (_hubConnection.State != ConnectionState.Connected)
                return false;

            return true;
        }

        public void AddLogStreamMessage(string content, string groupName = "Default")
        {
            AddLogStreamMessage(new[] { content }, groupName);
        }

        public void AddLogStreamMessage(string[] content, string groupName = "Default")
        {
            if (string.IsNullOrEmpty(_organizationId))
                return;

            if (!IsConnected())
                Connect();

            if (!IsConnected())
                return;

            var data = new
            {
                OrganizationId = _organizationId,
                GroupName = groupName,
                Environment = _environment,
                Content = content
            };

            _logsStreamProxy.Invoke("AppendMessage", data);
        }
    }
}
