using KissLog.Json;
using System;

namespace KissLog
{
    public static class LogLevelsExtensionMethods
    {
        #region Trace

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        public static void Trace(this IKLogger logger, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Trace, message, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        public static void Trace(this IKLogger logger, object json, JsonSerializeOptions options = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Trace, json, options, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        public static void Trace(this IKLogger logger, Exception ex,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Trace, ex, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        public static void Trace(this IKLogger logger, Args args, JsonSerializeOptions options = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Trace, args, options, memberName, lineNumber, memberType);
        }

        #endregion

        #region Debug

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        public static void Debug(this IKLogger logger, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Debug, message, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        public static void Debug(this IKLogger logger, object json, JsonSerializeOptions options = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Debug, json, options, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        public static void Debug(this IKLogger logger, Exception ex,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Debug, ex, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        public static void Debug(this IKLogger logger, Args args, JsonSerializeOptions options = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Debug, args, options, memberName, lineNumber, memberType);
        }

        #endregion

        #region Info

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        public static void Info(this IKLogger logger, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Information, message, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        public static void Info(this IKLogger logger, object json, JsonSerializeOptions options = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Information, json, options, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        public static void Info(this IKLogger logger, Exception ex,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Information, ex, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        public static void Info(this IKLogger logger, Args args, JsonSerializeOptions options = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Information, args, options, memberName, lineNumber, memberType);
        }

        #endregion

        #region Warn

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        public static void Warn(this IKLogger logger, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Warning, message, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        public static void Warn(this IKLogger logger, object json, JsonSerializeOptions options = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Warning, json, options, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        public static void Warn(this IKLogger logger, Exception ex,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Warning, ex, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        public static void Warn(this IKLogger logger, Args args, JsonSerializeOptions options = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Warning, args, options, memberName, lineNumber, memberType);
        }

        #endregion

        #region Error

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        public static void Error(this IKLogger logger, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Error, message, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        public static void Error(this IKLogger logger, object json, JsonSerializeOptions options = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Error, json, options, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        public static void Error(this IKLogger logger, Exception ex,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Error, ex, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        public static void Error(this IKLogger logger, Args args, JsonSerializeOptions options = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Error, args, options, memberName, lineNumber, memberType);
        }

        #endregion

        #region Critical

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        public static void Critical(this IKLogger logger, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Critical, message, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        public static void Critical(this IKLogger logger, object json, JsonSerializeOptions options = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Critical, json, options, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        public static void Critical(this IKLogger logger, Exception ex,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Critical, ex, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        public static void Critical(this IKLogger logger, Args args, JsonSerializeOptions options = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            if (logger == null)
                return;

            logger.Log(LogLevel.Critical, args, options, memberName, lineNumber, memberType);
        }

        #endregion
    }
}
