﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供 <see cref="IDictionary{TKey, TValue}"/> 的扩展方法的静态类。
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 根据指定的值获取 <see cref="IDictionary{TKey, TValue}"/> 中对应的键的方法。
        /// </summary>
        /// <typeparam name="TKey"><paramref name="source"/> 中的键的类型。</typeparam>
        /// <typeparam name="TValue"><paramref name="source"/> 中的值的类型。</typeparam>
        /// <param name="source">要获取键的 <see cref="IDictionary{TKey, TValue}"/> 的实例。</param>
        /// <param name="value">指定要查询的值。</param>
        /// <returns><paramref name="source"/> 中值为 <paramref name="value"/> 的键的集合。</returns>
        public static ICollection<TKey> GetKeys<TKey, TValue>(
            this IDictionary<TKey, TValue> source, TValue value)
        {
            var keys = new List<TKey>(source.Count);
            var valueComparer = EqualityComparer<TValue>.Default;
            foreach (var item in source)
            { if (valueComparer.Equals(item.Value, value)) { keys.Add(item.Key); } }
            keys.TrimExcess();
            return keys;
        }
    }
}