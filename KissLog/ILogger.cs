using System;
using System.Collections.Generic;
using System.Net;
using KissLog.Web;

namespace KissLog
{
    public interface ILogger
    {
        string CategoryName { get; }

        IEnumerable<LogMessage> LogMessages { get; }

        string ErrorMessage { get; }

        WebRequestProperties  WebRequestProperties { get; }

        HttpStatusCode? HttpStatusCode { get; }

#if NET40

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
        void Log(LogLevel logLevel, string message, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null);

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
        void Log(LogLevel logLevel, object json, Action<LogMessage> action = null,
            string memberName = "",
            int lineNumber = 0,
            string memberType = null);

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
        void Log(LogLevel logLevel, Exception ex, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null);

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
        void Log(LogLevel logLevel, KissLog.Args args, Action<LogMessage> action = null,
            string memberName = null,
            int lineNumber = 0,
            string memberType = null);

#else
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
        void Log(LogLevel logLevel, string message, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null);

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
        void Log(LogLevel logLevel, object json, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null);


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
        void Log(LogLevel logLevel, Exception ex, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null);

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
        void Log(LogLevel logLevel, KissLog.Args args, Action<LogMessage> action = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null,
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string memberType = null);
#endif

        /// <summary>
        /// <para>Sets the KissLog HttpStatusCode regardless of the Response returned by the server</para>
        /// <para>This is useful when you capture an error, log it, but return 200 to the client.</para>
        /// <para>Setting httpStatusCode to >= 400 will make the request be identified as error instead of success</para>
        /// </summary>
        void SetHttpStatusCode(HttpStatusCode httpStatusCode);
    }
}
