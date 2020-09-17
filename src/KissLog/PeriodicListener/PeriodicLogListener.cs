using KissLog.FlushArgs;
using KissLog.Internal;
using KissLog.Listeners;
using KissLog.Web;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KissLog.PeriodicListener
{
    public class PeriodicLogListener : ILogListener
    {
        private readonly PeriodicTimer _timer;
        private readonly TimeSpan _triggerInterval;
        private readonly TextFormatter _textFormatter;

        private readonly ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();

        private bool _timerScheduled = false;

        public PeriodicLogListener(PeriodicLogListenerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (options.TextFormatter == null)
                throw new ArgumentNullException(nameof(options.TextFormatter));

            _triggerInterval = options.TriggerInterval;
            _textFormatter = options.TextFormatter;

            _timer = new PeriodicTimer(cancelToken => TimerCallbackAsync());
        }

        public virtual int MinimumResponseHttpStatusCode { get; set; } = 0;
        public virtual LogLevel MinimumLogMessageLevel { get; set; } = LogLevel.Trace;
        public virtual LogListenerParser Parser { get; set; } = new LogListenerParser();

        public void OnBeginRequest(HttpRequest httpRequest, ILogger logger)
        {
            string text = _textFormatter.FormatBeginRequest(httpRequest);
            AddToQueue(text);
        }

        public void OnFlush(FlushLogArgs args, ILogger logger)
        {
            string text = _textFormatter.FormatEndRequest(args.WebProperties.Request, args.WebProperties.Response);
            AddToQueue(text);

            _timerScheduled = false;
            _timer.ScheduleExecution(TimeSpan.Zero);

            try
            {
                TimerCallbackAsync().Wait();
            }
            catch
            {

            }
        }

        public void OnMessage(LogMessage message, ILogger logger)
        {
            string text = _textFormatter.FormatLogMessage(message);
            AddToQueue(text);
        }

        private async Task TimerCallbackAsync()
        {
            try
            {
                Queue<string> batch = new Queue<string>();
                while(_queue.TryDequeue(out var message))
                {
                    batch.Enqueue(message);
                }

                if (batch.Count == 0)
                {
                    return;
                }

                await ProcessBatchAsync(batch);
            }
            catch(Exception ex)
            {
                InternalHelpers.Log($"PeriodicLogListener.TimerCallbackAsync exception: {ex}", LogLevel.Error);
            }
            finally
            {
                _timer.ScheduleExecution(_triggerInterval);
            }
        }

        protected virtual Task ProcessBatchAsync(IEnumerable<string> lines)
        {
            return Task.FromResult(true);
        }

        private void AddToQueue(string line)
        {
            if (string.IsNullOrEmpty(line))
                return;

            _queue.Enqueue(line);

            if (!_timerScheduled)
            {
                _timer.ScheduleExecution(_triggerInterval);
                _timerScheduled = true;
            }
        }
    }
}
