﻿using System;

namespace XstarS.Text
{
    /// <summary>
    /// 提供将字符串转换为数值的默认方法。
    /// </summary>
    /// <typeparam name="T">要转换为的数值的类型。</typeparam>
    [Serializable]
    internal sealed class DefaultStringParser<T> : SimpleStringParser<T>
    {
        /// <summary>
        /// 表示将字符串解析为指定类型的数值的方法的委托。
        /// </summary>
        private readonly Converter<string, T> Parser;

        /// <summary>
        /// 初始化 <see cref="DefaultStringParser{T}"/> 类的新实例。
        /// </summary>
        public DefaultStringParser()
        {
            this.Parser = DefaultStringParser<T>.CreateParser();
        }

        /// <summary>
        /// 创建 <typeparamref name="T"/> 类型的字符串解析方法的委托。
        /// </summary>
        /// <returns><typeparamref name="T"/> 类型的字符串解析方法的委托。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static Converter<string, T> CreateParser()
        {
            try
            {
                if (typeof(T).IsEnum) { return DefaultStringParser<T>.ParseEnum; }
                var method = typeof(T).GetMethod(nameof(int.Parse), new[] { typeof(string) });
                return (!(method is null) && method.IsStatic && (method.ReturnType == typeof(T))) ?
                    (Converter<string, T>)method.CreateDelegate(typeof(Converter<string, T>)) : null;
            }
            catch (Exception) { return null; }
        }

        /// <summary>
        /// 将指定的字符串表示形式转换为其等效的枚举形式。
        /// </summary>
        /// <param name="text">包含要转换的枚举的字符串。</param>
        /// <returns>与 <paramref name="text"/> 等效的枚举形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException"><paramref name="text"/> 不表示有效的值。</exception>
        /// <exception cref="FormatException"><paramref name="text"/> 的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="OverflowException">
        /// <paramref name="text"/> 表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
        private static T ParseEnum(string text) => (T)Enum.Parse(typeof(T), text);

        /// <summary>
        /// 将指定的字符串表示形式转换为其等效的数值形式。
        /// </summary>
        /// <param name="text">包含要转换的数值的字符串。</param>
        /// <returns>与 <paramref name="text"/> 等效的数值形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException"><paramref name="text"/> 不表示有效的值。</exception>
        /// <exception cref="FormatException"><paramref name="text"/> 的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="OverflowException">
        /// <paramref name="text"/> 表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
        public override T Parse(string text)
        {
            return (this.Parser ?? throw new InvalidCastException()).Invoke(text);
        }
    }
}