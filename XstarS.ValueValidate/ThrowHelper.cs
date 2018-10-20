﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XstarS
{
    /// <summary>
    /// 提供抛出异常的方法。
    /// </summary>
    internal static partial class ThrowHelper
    {
        /// <summary>
        /// 抛出 <see cref="ArgumentException"/> 类型的异常。
        /// </summary>
        /// <param name="paramName">引发异常的参数的名称。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <param name="innerException">引发当前异常的异常。</param>
        /// <exception cref="ArgumentException">
        /// 总是抛出 <see cref="ArgumentException"/> 类型的异常。
        /// </exception>
        public static void ThrowArgumentException(string paramName = null,
            string message = null, Exception innerException = null)
        {
            message = message ?? new ArgumentException().Message;
            throw new ArgumentException(message, paramName, innerException);
        }

        /// <summary>
        /// 抛出 <see cref="ArgumentNullException"/> 类型的异常。
        /// </summary>
        /// <param name="paramName">引发异常的参数的名称。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <exception cref="ArgumentNullException">
        /// 总是抛出 <see cref="ArgumentNullException"/> 类型的异常。
        /// </exception>
        public static void ThrowArgumentNullException(string paramName = null,
            string message = null)
        {
            message = message ?? new ArgumentNullException().Message;
            throw new ArgumentNullException(paramName, message);
        }

        /// <summary>
        /// 抛出 <see cref="ArgumentOutOfRangeException"/> 类型的异常。
        /// </summary>
        /// <param name="paramName">引发异常的参数的名称。</param>
        /// <param name="actualValue">参数的实际值。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 总是抛出 <see cref="ArgumentOutOfRangeException"/> 类型的异常。
        /// </exception>
        public static void ThrowArgumentOutOfRangeException(string paramName = null,
            object actualValue = null, string message = null)
        {
            message = message ?? new ArgumentOutOfRangeException().Message;
            throw new ArgumentOutOfRangeException(paramName, actualValue, message);
        }

        /// <summary>
        /// 抛出 <see cref="IndexOutOfRangeException"/> 类型的异常。
        /// </summary>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <param name="innerException">引发当前异常的异常。</param>
        /// <exception cref="IndexOutOfRangeException">
        /// 总是抛出 <see cref="IndexOutOfRangeException"/> 类型的异常。
        /// </exception>
        public static void ThrowIndexOutOfRangeException(
            string message = null, Exception innerException = null)
        {
            message = message ?? new IndexOutOfRangeException().Message;
            throw new IndexOutOfRangeException(message, innerException);
        }

        /// <summary>
        /// 抛出 <see cref="KeyNotFoundException"/> 类型的异常。
        /// </summary>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <param name="innerException">引发当前异常的异常。</param>
        /// <exception cref="KeyNotFoundException">
        /// 总是抛出 <see cref="KeyNotFoundException"/> 类型的异常。
        /// </exception>
        public static void ThrowKeyNotFoundException(
            string message = null, Exception innerException = null)
        {
            message = message ?? new KeyNotFoundException().Message;
            throw new KeyNotFoundException(message, innerException);
        }

        /// <summary>
        /// 抛出 <see cref="FileNotFoundException"/> 类型的异常。
        /// </summary>
        /// <param name="fileName">无法找到的文件的路径。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <param name="innerException">引发当前异常的异常。</param>
        /// <exception cref="FileNotFoundException">
        /// 总是抛出 <see cref="FileNotFoundException"/> 类型的异常。
        /// </exception>
        public static void ThrowFileNotFoundException(string fileName = null,
            string message = null, Exception innerException = null)
        {
            message = message ?? new FileNotFoundException().Message;
            throw new FileNotFoundException(message, fileName, innerException);
        }

        /// <summary>
        /// 抛出 <see cref="DirectoryNotFoundException"/> 类型的异常。
        /// </summary>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <param name="innerException">引发当前异常的异常。</param>
        /// <exception cref="DirectoryNotFoundException">
        /// 总是抛出 <see cref="DirectoryNotFoundException"/> 类型的异常。
        /// </exception>
        public static void ThrowDirectoryNotFoundException(
            string message = null, Exception innerException = null)
        {
            message = message ?? new DirectoryNotFoundException().Message;
            throw new DirectoryNotFoundException(message, innerException);
        }
    }
}