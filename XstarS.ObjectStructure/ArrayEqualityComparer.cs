﻿using System;
using System.Collections.Generic;
using XstarS.Collections.Generic;
using XstarS.Runtime;

namespace XstarS
{
    using ObjectPair = KeyValuePair<object, object>;

    /// <summary>
    /// 提供数组基于元素的相等比较的方法。
    /// </summary>
    /// <typeparam name="T">数组的类型。</typeparam>
    [Serializable]
    internal sealed class ArrayEqualityComparer<T> : StructureEqualityComparer<T>
    {
        /// <summary>
        /// 初始化 <see cref="ArrayEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        public ArrayEqualityComparer() { }

        /// <summary>
        /// 确定两个指定数组中所包含的元素是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个数组。</param>
        /// <param name="y">要比较的第二个数组。</param>
        /// <param name="compared">已经比较过的对象。</param>
        /// <returns>如果 <paramref name="x"/> 和 <paramref name="y"/> 中的元素相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        protected override bool EqualsCore(T x, T y, ISet<ObjectPair> compared)
        {
            var xArray = (Array)(object)x;
            var yArray = (Array)(object)y;

            if (xArray.Rank != yArray.Rank) { return false; }
            if (xArray.Length != yArray.Length) { return false; }
            for (int rank = 0; rank < xArray.Rank; rank++)
            {
                if (xArray.GetLength(rank) != yArray.GetLength(rank))
                {
                    return false;
                }
            }

            var typeArray = xArray.GetType();
            if (typeArray.GetElementType().IsPointer)
            {
                var methodGet = typeArray.GetMethod("Get");
                for (int index = 0; index < xArray.Length; index++)
                {
                    var xItem = methodGet.Invoke(xArray, xArray.OffsetToIndices(index).Box());
                    var yItem = methodGet.Invoke(yArray, yArray.OffsetToIndices(index).Box());
                    if (!ObjectRuntimeHelper.BoxedPointerEquals(xItem, yItem)) { return false; }
                }
            }
            else
            {
                for (int index = 0; index < xArray.Length; index++)
                {
                    var xItem = xArray.GetValue(xArray.OffsetToIndices(index));
                    var yItem = yArray.GetValue(yArray.OffsetToIndices(index));

                    if (xItem?.GetType() != yItem?.GetType()) { return false; }

                    var comparer = StructureEqualityComparer.OfType(xItem?.GetType());
                    if (!comparer.Equals(xItem, yItem, compared)) { return false; }
                }
            }
            return true;
        }

        /// <summary>
        /// 获取指定的数组中的元素的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的数组。</param>
        /// <param name="computed">已经计算过哈希代码的对象。</param>
        /// <returns><paramref name="obj"/> 中的元素的哈希代码。</returns>
        protected override int GetHashCodeCore(T obj, ISet<object> computed)
        {
            var array = (Array)(object)obj;

            var hashCode = 0;

            var typeArray = array.GetType();
            if (typeArray.GetElementType().IsPointer)
            {
                var methodGet = typeArray.GetMethod("Get");
                for (int index = 0; index < array.Length; index++)
                {
                    var item = methodGet.Invoke(array, array.OffsetToIndices(index).Box());
                    hashCode = this.CombineHashCode(
                        hashCode, ObjectRuntimeHelper.GetBoxedPointerHashCode(item));
                }
            }
            else
            {
                for (int index = 0; index < array.Length; index++)
                {
                    var item = array.GetValue(array.OffsetToIndices(index));
                    var comparer = StructureEqualityComparer.OfType(item?.GetType());
                    hashCode = this.CombineHashCode(
                        hashCode, comparer.GetHashCode(item, computed));
                }
            }

            return hashCode;
        }
    }
}