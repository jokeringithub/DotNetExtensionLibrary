﻿using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    partial class CollectionExtensions
    {
        /// <summary>
        /// 确定当前 <see cref="IEnumerable{T}"/> 与指定 <see cref="IEnumerable{T}"/> 的元素是否对应相等。
        /// </summary>
        /// <param name="enumerable">要进行相等比较的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <param name="other">要与当前集合进行相等比较的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <param name="comparer">用于比较集合元素的比较器。</param>
        /// <returns>若 <paramref name="enumerable"/> 与 <paramref name="other"/> 的每个元素都对应相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool CollectionEquals<T>(this IEnumerable<T> enumerable, IEnumerable<T> other,
            IEqualityComparer<T> comparer = null)
        {
            return new CollectionEqualityComparer<T>(comparer).Equals(enumerable, other);
        }

        /// <summary>
        /// 获取当前 <see cref="IEnumerable{T}"/> 对象中指定对象的数量。
        /// </summary>
        /// <typeparam name="T"><see cref="IEnumerable{T}"/> 中的元素的类型。</typeparam>
        /// <param name="enumerable">要进行计数的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <param name="value">要获取在 <see cref="IEnumerable{T}"/> 中的数量的对象。</param>
        /// <param name="comparer">指定用于进行相等比较的比较器。</param>
        /// <returns><paramref name="enumerable"/> 中与
        /// <paramref name="value"/> 相等的对象的数量。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerable"/> 为 <see langword="null"/>。</exception>
        public static int CountOf<T>(this IEnumerable<T> enumerable, T value,
            IEqualityComparer<T> comparer = null)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            comparer = comparer ?? EqualityComparer<T>.Default;

            int count = 0;
            foreach (var item in enumerable)
            {
                if (comparer.Equals(item, value))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 获取当前 <see cref="IEnumerable{T}"/> 对象中满足指定条件的对象的数量。
        /// </summary>
        /// <typeparam name="T"><see cref="IEnumerable{T}"/> 中的元素的类型。</typeparam>
        /// <param name="enumerable">要进行计数的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <param name="match"><see cref="IEnumerable{T}"/> 中参与计数的对象应满足的条件。</param>
        /// <returns><paramref name="enumerable"/> 中满足条件
        /// <paramref name="match"/> 的对象的数量。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/>
        /// 或 <paramref name="match"/> 为 <see langword="null"/>。</exception>
        public static int CountOf<T>(this IEnumerable<T> enumerable, Predicate<T> match)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            int count = 0;
            foreach (var item in enumerable)
            {
                if (match(item))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 对当前 <see cref="IEnumerable{T}"/> 对象中的每个元素执行指定操作。
        /// </summary>
        /// <typeparam name="T"><see cref="IEnumerable{T}"/> 中的元素的类型。</typeparam>
        /// <param name="enumerable">要对元素执行操作的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <param name="action">要对 <see cref="IEnumerable{T}"/> 中每个元素执行的操作。</param>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/>
        /// 或 <paramref name="action"/> 为 <see langword="null"/>。</exception>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var item in enumerable)
            {
                action(item);
            }
        }
    }
}