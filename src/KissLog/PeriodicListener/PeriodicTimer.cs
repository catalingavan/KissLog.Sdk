using KissLog.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KissLog.PeriodicListener
{
    internal class PeriodicTimer : IDisposable
    {
        private readonly Timer _timer;
        private readonly CancellationTokenSource _cancelToken;

        readonly Func<CancellationToken, Task> _timerCallback;

        private bool _running;
        private bool _disposed;

        public PeriodicTimer(Func<CancellationToken, Task> callback)
        {
            _timerCallback = callback;
            _cancelToken = new CancellationTokenSource();

            _timer = new Timer(async o => await TimerCallbackAsync(), null, Timeout.Infinite, Timeout.Infinite);
        }

        public void ScheduleExecution(TimeSpan dueTime)
        {
            if(dueTime < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(dueTime));
            }

            if (_disposed)
            {
                return;
            }

            _timer.Change(dueTime, Timeout.InfiniteTimeSpan);
        }

        private async Task TimerCallbackAsync()
        {
            try
            {
                if(_disposed)
                {
                    return;
                }

                if(_cancelToken.Token.IsCancellationRequested)
                {
                    return;
                }

                if(_running)
                {
                    // timer is already running. Maybe we should have an option to control what happens in this scenario
                    return;
                }

                _running = true;

                await _timerCallback(_cancelToken.Token);
            }
            catch(Exception ex)
            {
                InternalHelpers.Log($"PeriodicTimer.TimerCallbackAsync exception: {ex}", LogLevel.Error);
            }
            finally
            {
                _running = false;
            }
        }

        public void Dispose()
        {
            _cancelToken.Cancel();
            _timer?.Dispose();

            _disposed = true;
        }
    }
}
