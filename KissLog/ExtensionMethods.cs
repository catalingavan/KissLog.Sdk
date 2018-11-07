using System;
using System.Net;

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
        public static void Trace(this ILogger logger, string message,
	        string memberName = null,
	        int lineNumber = 0,
	        string memberType = null)
        {
	        logger.Log(LogLevel.Trace, message, memberName, lineNumber, memberType);
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
        public static void Trace(this ILogger logger, object json,
	        string memberName = null,
	        int lineNumber = 0,
	        string memberType = null)
        {
	        logger.Log(LogLevel.Trace, json, memberName, lineNumber, memberType);
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
        public static void Trace(this ILogger logger, Exception ex,
	        string memberName = null,
	        int lineNumber = 0,
	        string memberType = null)
        {
	        logger.Log(LogLevel.Trace, ex, memberName, lineNumber, memberType);
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
        public static void Trace(this ILogger logger, KissLog.Args args,
	        string memberName = null,
	        int lineNumber = 0,
	        string memberType = null)
        {
	        logger.Log(LogLevel.Trace, args, memberName, lineNumber, memberType);
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
        public static void Debug(this ILogger logger, string message,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Debug, message, memberName, lineNumber, memberType);
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
        public static void Debug(this ILogger logger, object json,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Debug, json, memberName, lineNumber, memberType);
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
        public static void Debug(this ILogger logger, Exception ex,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Debug, ex, memberName, lineNumber, memberType);
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
        public static void Debug(this ILogger logger, KissLog.Args args,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Debug, args, memberName, lineNumber, memberType);
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
        public static void Info(this ILogger logger, string message,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Information, message, memberName, lineNumber, memberType);
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
        public static void Info(this ILogger logger, object json,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Information, json, memberName, lineNumber, memberType);
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
        public static void Info(this ILogger logger, Exception ex,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Information, ex, memberName, lineNumber, memberType);
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
        public static void Info(this ILogger logger, KissLog.Args args,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Information, args, memberName, lineNumber, memberType);
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
        public static void Warn(this ILogger logger, string message,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Warning, message, memberName, lineNumber, memberType);
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
        public static void Warn(this ILogger logger, object json,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Warning, json, memberName, lineNumber, memberType);
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
        public static void Warn(this ILogger logger, Exception ex,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Warning, ex, memberName, lineNumber, memberType);
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
        public static void Warn(this ILogger logger, KissLog.Args args,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Warning, args, memberName, lineNumber, memberType);
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
        public static void Error(this ILogger logger, string message,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Error, message, memberName, lineNumber, memberType);
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
        public static void Error(this ILogger logger, object json,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Error, json, memberName, lineNumber, memberType);
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
        public static void Error(this ILogger logger, Exception ex,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Error, ex, memberName, lineNumber, memberType);
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
        public static void Error(this ILogger logger, KissLog.Args args,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null)
        {
            logger.Log(LogLevel.Error, args, memberName, lineNumber, memberType);
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
        public static void Critical(this ILogger logger, string message,
	        string memberName = null,
	        int lineNumber = 0,
	        string memberType = null)
        {
	        logger.Log(LogLevel.Critical, message, memberName, lineNumber, memberType);
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
        public static void Critical(this ILogger logger, object json,
	        string memberName = null,
	        int lineNumber = 0,
	        string memberType = null)
        {
	        logger.Log(LogLevel.Critical, json, memberName, lineNumber, memberType);
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
        public static void Critical(this ILogger logger, Exception ex,
	        string memberName = null,
	        int lineNumber = 0,
	        string memberType = null)
        {
	        logger.Log(LogLevel.Critical, ex, memberName, lineNumber, memberType);
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
        public static void Critical(this ILogger logger, KissLog.Args args,
	        string memberName = null,
	        int lineNumber = 0,
	        string memberType = null)
        {
	        logger.Log(LogLevel.Critical, args, memberName, lineNumber, memberType);
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
        public static void Trace(this ILogger logger, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Trace, message, memberName, lineNumber, memberType);
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
        public static void Trace(this ILogger logger, object json,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Trace, json, memberName, lineNumber, memberType);
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
        public static void Trace(this ILogger logger, Exception ex,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Trace, ex, memberName, lineNumber, memberType);
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
        public static void Trace(this ILogger logger, KissLog.Args args,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Trace, args, memberName, lineNumber, memberType);
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
        public static void Debug(this ILogger logger, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Debug, message, memberName, lineNumber, memberType);
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
        public static void Debug(this ILogger logger, object json,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Debug, json, memberName, lineNumber, memberType);
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
        public static void Debug(this ILogger logger, Exception ex,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Debug, ex, memberName, lineNumber, memberType);
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
        public static void Debug(this ILogger logger, KissLog.Args args,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Debug, args, memberName, lineNumber, memberType);
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
        public static void Info(this ILogger logger, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Information, message, memberName, lineNumber, memberType);
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
        public static void Info(this ILogger logger, object json,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Information, json, memberName, lineNumber, memberType);
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
        public static void Info(this ILogger logger, Exception ex,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Information, ex, memberName, lineNumber, memberType);
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
        public static void Info(this ILogger logger, KissLog.Args args,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Information, args, memberName, lineNumber, memberType);
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
        public static void Warn(this ILogger logger, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Warning, message, memberName, lineNumber, memberType);
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
        public static void Warn(this ILogger logger, object json,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Warning, json, memberName, lineNumber, memberType);
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
        public static void Warn(this ILogger logger, Exception ex,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Warning, ex, memberName, lineNumber, memberType);
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
        public static void Warn(this ILogger logger, KissLog.Args args,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Warning, args, memberName, lineNumber, memberType);
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
        public static void Error(this ILogger logger, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Error, message, memberName, lineNumber, memberType);
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
        public static void Error(this ILogger logger, object json,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Error, json, memberName, lineNumber, memberType);
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
        public static void Error(this ILogger logger, Exception ex,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Error, ex, memberName, lineNumber, memberType);
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
        public static void Error(this ILogger logger, KissLog.Args args,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Error, args, memberName, lineNumber, memberType);
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
        public static void Critical(this ILogger logger, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Critical, message, memberName, lineNumber, memberType);
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
        public static void Critical(this ILogger logger, object json,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Critical, json, memberName, lineNumber, memberType);
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
        public static void Critical(this ILogger logger, Exception ex,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Critical, ex, memberName, lineNumber, memberType);
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
        public static void Critical(this ILogger logger, KissLog.Args args,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null)
        {
            logger.Log(LogLevel.Critical, args, memberName, lineNumber, memberType);
        }

    }
#endif

    public static partial class ExtensionMethods
    {
        /// <summary>
        /// <para>Sets the KissLog HttpStatusCode regardless of the Response returned by the server</para>
        /// <para>This is useful when you capture an error, log it, but return 200 to the client.</para>
        /// <para>Setting httpStatusCode to >= 400 will make the request be identified as error instead of success</para>
        /// </summary>
        public static void SetHttpStatusCode(this ILogger logger, HttpStatusCode httpStatusCode)
        {
            if (logger is Logger theLogger)
            {
                theLogger.SetHttpStatusCode(httpStatusCode);
            }
        }

        /// <summary>
        /// <para>Logs a file</para>
        /// </summary>
        public static void LogFile(this ILogger logger, string sourceFilePath, string fileName)
        {
            if (logger is Logger theLogger)
            {
                theLogger.LoggerFiles.LogFile(sourceFilePath, fileName);
            }
        }

        /// <summary>
        /// <para>Logs a file</para>
        /// </summary>
        public static void LogAsFile(this ILogger logger, byte[] content, string fileName)
        {
            if (logger is Logger theLogger)
            {
                theLogger.LoggerFiles.LogAsFile(content, fileName);
            }
        }

        /// <summary>
        /// <para>Logs a file</para>
        /// </summary>
        public static void LogAsFile(this ILogger logger, string content, string fileName)
        {
            if (logger is Logger theLogger)
            {
                theLogger.LoggerFiles.LogAsFile(content, fileName);
            }
        }

        /// <summary>
        /// <para>Explicitly instruct logger to capture the Request.InputStream property</para>
        /// </summary>
        public static void LogRequestInputStream(this ILogger logger, bool value = true)
        {
            if (logger is Logger theLogger)
            {
                theLogger.AddProperty(InternalHelpers.LogRequestInputStreamProperty, value);
            }
        }

        /// <summary>
        /// <para>Explicitly instruct logger to capture the Response Body value</para>
        /// </summary>
        public static void LogResponseBody(this ILogger logger, bool value = true)
        {
            if (logger is Logger theLogger)
            {
                theLogger.AddProperty(InternalHelpers.LogResponseBodyProperty, value);
            }
        }

        /// <summary>
        /// <para>Returns true if the ILogger is created and handled automatically by the HttpRequest.</para>
        /// </summary>
        public static bool IsCreatedByHttpRequest(this ILogger logger)
        {
            if (logger is Logger theLogger)
            {
                return theLogger.GetProperty(InternalHelpers.IsCreatedByHttpRequest) != null;
            }

            return false;
        }
    }
}
