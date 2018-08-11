using System;

namespace KissLog
{
#if NET40
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Trace(this ILogger logger, string message, Action<LogMessage> action = null,
	        string memberName = null,
	        int lineNumber = 0,
	        string memberType = null)
        {
	        logger.Log(LogLevel.Trace, message, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Trace(this ILogger logger, object json, Action<LogMessage> action = null,
	        string memberName = null,
	        int lineNumber = 0,
	        string memberType = null)
        {
	        logger.Log(LogLevel.Trace, json, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Trace(this ILogger logger, Exception ex, Action<LogMessage> action = null,
	        string memberName = null,
	        int lineNumber = 0,
	        string memberType = null)
        {
	        logger.Log(LogLevel.Trace, ex, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Trace(this ILogger logger, KissLog.Args args, Action<LogMessage> action = null,
	        string memberName = null,
	        int lineNumber = 0,
	        string memberType = null)
        {
	        logger.Log(LogLevel.Trace, args, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Debug(this ILogger logger, string message, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Debug, message, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Debug(this ILogger logger, object json, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Debug, json, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Debug(this ILogger logger, Exception ex, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Debug, ex, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Debug(this ILogger logger, KissLog.Args args, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Debug, args, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Info(this ILogger logger, string message, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Information, message, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Info(this ILogger logger, object json, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Information, json, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Info(this ILogger logger, Exception ex, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Information, ex, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Info(this ILogger logger, KissLog.Args args, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Information, args, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Warn(this ILogger logger, string message, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Warning, message, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Warn(this ILogger logger, object json, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Warning, json, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Warn(this ILogger logger, Exception ex, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Warning, ex, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Warn(this ILogger logger, KissLog.Args args, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Warning, args, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Error(this ILogger logger, string message, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Error, message, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Error(this ILogger logger, object json, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Error, json, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Error(this ILogger logger, Exception ex, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Error, ex, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Error(this ILogger logger, KissLog.Args args, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Error, args, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Critical(this ILogger logger, string message, Action<LogMessage> action = null,
	        string memberName = null,
	        int lineNumber = 0,
	        string memberType = null)
        {
	        logger.Log(LogLevel.Critical, message, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Critical(this ILogger logger, object json, Action<LogMessage> action = null,
	        string memberName = null,
	        int lineNumber = 0,
	        string memberType = null)
        {
	        logger.Log(LogLevel.Critical, json, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Critical(this ILogger logger, Exception ex, Action<LogMessage> action = null,
	        string memberName = null,
	        int lineNumber = 0,
	        string memberType = null)
        {
	        logger.Log(LogLevel.Critical, ex, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        /// <param name="memberName">
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Critical(this ILogger logger, KissLog.Args args, Action<LogMessage> action = null,
	        string memberName = null,
	        int lineNumber = 0,
	        string memberType = null)
        {
	        logger.Log(LogLevel.Critical, args, action, memberName, lineNumber, memberType);
        }
    }
#else
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Trace(this ILogger logger, string message, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Trace, message, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Trace(this ILogger logger, object json, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Trace, json, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Trace(this ILogger logger, Exception ex, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Trace, ex, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Trace(this ILogger logger, KissLog.Args args, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Trace, args, action, memberName, lineNumber, memberType);
        }


        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Debug(this ILogger logger, string message, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Debug, message, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Debug(this ILogger logger, object json, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Debug, json, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Debug(this ILogger logger, Exception ex, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Debug, ex, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Debug(this ILogger logger, KissLog.Args args, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Debug, args, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Info(this ILogger logger, string message, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Information, message, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Info(this ILogger logger, object json, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Information, json, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Info(this ILogger logger, Exception ex, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Information, ex, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Info(this ILogger logger, KissLog.Args args, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Information, args, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Warn(this ILogger logger, string message, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Warning, message, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Warn(this ILogger logger, object json, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Warning, json, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Warn(this ILogger logger, Exception ex, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Warning, ex, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Warn(this ILogger logger, KissLog.Args args, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Warning, args, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Error(this ILogger logger, string message, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Error, message, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Error(this ILogger logger, object json, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Error, json, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Error(this ILogger logger, Exception ex, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Error, ex, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Error(this ILogger logger, KissLog.Args args, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Error, args, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="message">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Critical(this ILogger logger, string message, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Critical, message, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="json">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Critical(this ILogger logger, object json, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Critical, json, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="ex">Log data</param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param> 
        public static void Critical(this ILogger logger, Exception ex, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Critical, ex, action, memberName, lineNumber, memberType);
        }

        /// <summary>
        /// Writes a Log message
        /// </summary>
        /// <param name="args">
        ///     <para>A collection of Log data</para>
        ///     <para>Eg: new KissLog.Args("value1", 100)</para>
        /// </param>
        /// <param name="memberName">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: GetProductById</para>
        /// </param>
        /// <param name="lineNumber">
        ///     <para>Will be automatically populated</para>
        ///     <para>Eg: 116</para>
        /// </param>
        /// <param name="memberType">
        ///     <para>If ILogger is an instance of LoggerMemberTypeDecorator, it will be automatically populated</para>
        ///     <para>Eg: MyProject.Domain.ProductsRepository</para>
        /// </param>
        public static void Critical(this ILogger logger, KissLog.Args args, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Critical, args, action, memberName, lineNumber, memberType);
        }

    }
#endif

    public static partial class ExtensionMethods
    {
        public static void LogFile(this ILogger logger, string sourceFilePath, string fileName)
        {
            if (logger is Logger theLogger)
            {
                theLogger.LoggerFiles.LogFile(sourceFilePath, fileName);
            }
        }

        public static void LogAsFile(this ILogger logger, byte[] content, string fileName)
        {
            if (logger is Logger theLogger)
            {
                theLogger.LoggerFiles.LogAsFile(content, fileName);
            }
        }

        public static void LogAsFile(this ILogger logger, string content, string fileName)
        {
            if (logger is Logger theLogger)
            {
                theLogger.LoggerFiles.LogAsFile(content, fileName);
            }
        }

        public static void SaveResponseBodyOnFlush(this ILogger logger)
        {
            if (logger is Logger theLogger)
            {
                theLogger.AddCustomProperty(InternalHelpers.SaveResponseBodyProperty, true);
            }
        }
    }
}
