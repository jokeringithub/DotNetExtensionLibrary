﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using XstarS.Reflection;

namespace XstarS
{
    /// <summary>
    /// 提供两个对象的值相等比较的方法。
    /// </summary>
    [Serializable]
    internal sealed class ValueEquatablePair
    {
        /// <summary>
        /// 要进行值相等比较的第一个对象。
        /// </summary>
        public readonly object Value;

        /// <summary>
        /// 要进行值相等比较的第二个对象。
        /// </summary>
        public readonly object Other;

        /// <summary>
        /// 已经比较过的对象的 <see cref="KeyValuePair{TKey, TValue}"/>。
        /// </summary>
        [NonSerialized]
        private HashSet<KeyValuePair<object, object>> Compared;

        /// <summary>
        /// 使用要进行值相等比较的两个对象初始化 <see cref="ValueEquatablePair"/> 类的新实例。
        /// </summary>
        /// <param name="value">要进行值相等比较的第一个对象。</param>
        /// <param name="other">要进行值相等比较的第二个对象。</param>
        public ValueEquatablePair(object value, object other)
        {
            this.Value = value;
            this.Other = other;
        }

        /// <summary>
        /// 确定当前实例包含的两个对象的值是否相等。
        /// 将递归比较至对象的字段（数组的元素）为 .NET 基元类型 (<see cref="Type.IsPrimitive"/>)、
        /// 字符串 <see cref="string"/> 或指针类型 (<see cref="Type.IsPointer"/>)。
        /// </summary>
        /// <returns>若当前实例的 <see cref="ValueEquatablePair.Value"/> 和
        /// <see cref="ValueEquatablePair.Other"/> 的值相等，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        public bool ValueEquals()
        {
            this.Compared = new HashSet<KeyValuePair<object, object>>(
                PairReferenceEqualityComparer.Default);

            var equals = this.ValueEquals(this.Value, this.Other);

            this.Compared = null;
            return equals;
        }

        /// <summary>
        /// 确定指定的两个对象的值是否相等。
        /// </summary>
        /// <param name="value">要进行值相等比较的第一个对象。</param>
        /// <param name="other">要进行值相等比较的第二个对象。</param>
        /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        private bool ValueEquals(object value, object other)
        {
            // 已比较过的对象。
            var pair = new KeyValuePair<object, object>(value, other);
            if (!this.Compared.Add(pair)) { return true; }
            // 引用比较。
            if (object.ReferenceEquals(value, other)) { return true; }
            if ((value is null) ^ (other is null)) { return false; }
            // 类型不同。
            if (value.GetType() != other.GetType()) { return false; }

            // 根据类型进行值相等比较。
            var type = value.GetType();
            return
                type.IsPrimitive ? this.PrimitiveValueEquals(value, other) :
                type == typeof(string) ? this.StringValueEquals((string)value, (string)other) :
                type == typeof(Pointer) ? this.PointerValueEquals((Pointer)value, (Pointer)other) :
                type.IsArray ? this.ArrayValueEquals((Array)value, (Array)other) :
                this.ObjectValueEquals(value, other);
        }

        /// <summary>
        /// 确定指定的两个基元类型对象 (<see cref="Type.IsPrimitive"/>) 的值是否相等。
        /// </summary>
        /// <param name="value">要进行值相等比较的第一个基元类型对象。</param>
        /// <param name="other">要进行值相等比较的第二个基元类型对象。</param>
        /// <returns>若 <paramref name="value"/> 和 <paramref name="other"/> 的类型相同且值相等，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        private bool PrimitiveValueEquals(object value, object other)
        {
            return object.Equals(value, other);
        }

        /// <summary>
        /// 确定指定的两个字符串 <see cref="string"/> 的值是否相等。
        /// </summary>
        /// <param name="value">要进行值相等比较的第一个字符串。</param>
        /// <param name="other">要进行值相等比较的第二个字符串。</param>
        /// <returns>若 <paramref name="value"/> 和 <paramref name="other"/>
        /// 均为字符串 <see cref="string"/> 且值相等，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        private bool StringValueEquals(string value, string other)
        {
            return string.Equals(value, other);
        }

        /// <summary>
        /// 确定指定的两个指针包装 <see cref="Pointer"/> 的值是否相等。
        /// </summary>
        /// <param name="value">要进行值相等比较的第一个指针包装。</param>
        /// <param name="other">要进行值相等比较的第二个指针包装。</param>
        /// <returns>若 <paramref name="value"/> 和 <paramref name="other"/>
        /// 均为指针包装 <see cref="Pointer"/>，且指针的值和类型都相等，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        private bool PointerValueEquals(Pointer value, Pointer other)
        {
            return value.GetPointerValue() == other.GetPointerValue();
        }

        /// <summary>
        /// 确定指定的两个数组 <see cref="Array"/> 的所有元素的值是否相等。
        /// </summary>
        /// <param name="value">要进行值相等比较的第一个数组。</param>
        /// <param name="other">要进行值相等比较的第二个数组。</param>
        /// <returns>若 <paramref name="value"/> 和 <paramref name="other"/>
        /// 均为数组 <see cref="Array"/>，且类型和尺寸相同，每个元素的值相等，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        private bool ArrayValueEquals(Array value, Array other)
        {
            // 大小不等。
            if (value.Rank != other.Rank) { return false; }
            if (value.Length != other.Length) { return false; }
            for (int i = 0; i < value.Rank; i++)
            {
                if (value.GetLength(i) != other.GetLength(i))
                {
                    return false;
                }
            }

            var typeArray = value.GetType();
            // 指针数组，反射调用无法访问的 Get 方法。
            if (typeArray.GetElementType().IsPointer)
            {
                var methodGet = typeArray.GetMethod("Get");
                for (int i = 0; i < value.Length; i++)
                {
                    if (!this.ValueEquals(
                        methodGet.Invoke(value, Array.ConvertAll(
                            value.OffsetToIndices(i), index => (object)index)),
                        methodGet.Invoke(other, Array.ConvertAll(
                            other.OffsetToIndices(i), index => (object)index))))
                    {
                        return false;
                    }
                }
            }
            // 一般数组。
            else
            {
                bool isMultiDim = value.Rank > 1;
                for (int i = 0; i < value.Length; i++)
                {
                    if (!this.ValueEquals(
                        isMultiDim ? value.GetValue(value.OffsetToIndices(i)) : value.GetValue(i),
                        isMultiDim ? other.GetValue(other.OffsetToIndices(i)) : other.GetValue(i)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 确定指定的两个对象的所有字段的值是否相等。
        /// </summary>
        /// <param name="value">要进行值相等比较的第一个对象。</param>
        /// <param name="other">要进行值相等比较的第二个对象。</param>
        /// <returns>若 <paramref name="value"/> 和 <paramref name="other"/>
        /// 的类型相同，且每个实例字段的值相等，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        private bool ObjectValueEquals(object value, object other)
        {
            // 循环获取基类。
            for (var type = value.GetType(); !(type is null); type = type.BaseType)
            {
                // 获取每个实例字段。
                var fields = type.GetFields(BindingFlags.DeclaredOnly |
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                // 依次递归比较每个字段。
                foreach (var field in fields)
                {
                    if (!this.ValueEquals(field.GetValue(value), field.GetValue(other)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 用于比较两个 <see cref="KeyValuePair{TKey, TValue}"/> 包含的对象的引用是否相等的比较器。
        /// </summary>
        private sealed class PairReferenceEqualityComparer : EqualityComparer<KeyValuePair<object, object>>
        {
            /// <summary>
            /// 初始化 <see cref="PairReferenceEqualityComparer"/> 类的新实例。
            /// </summary>
            private PairReferenceEqualityComparer() : base() { }

            /// <summary>
            /// 返回一个默认的 <see cref="PairReferenceEqualityComparer"/> 实例。
            /// </summary>
            public static new PairReferenceEqualityComparer Default { get; } = new PairReferenceEqualityComparer();

            /// <summary>
            /// 确定两个 <see cref="KeyValuePair{TKey, TValue}"/> 包含的对象的引用是否相等。
            /// </summary>
            /// <param name="x">要比较对象引用的第一个 <see cref="KeyValuePair{TKey, TValue}"/>。</param>
            /// <param name="y">要比较对象引用的第二个 <see cref="KeyValuePair{TKey, TValue}"/>。</param>
            /// <returns>若 <paramref name="x"/> 与 <paramref name="y"/> 的包含的对象的引用分别相等，
            /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
            public override bool Equals(KeyValuePair<object, object> x, KeyValuePair<object, object> y) =>
                object.ReferenceEquals(x.Key, y.Key) && object.ReferenceEquals(x.Value, y.Value);

            /// <summary>
            /// 获取指定 <see cref="KeyValuePair{TKey, TValue}"/> 包含的对象基于引用的哈希代码。
            /// </summary>
            /// <param name="obj">要获取包含的对象的哈希代码的 <see cref="KeyValuePair{TKey, TValue}"/>。</param>
            /// <returns><paramref name="obj"/> 包含的对象基于引用的哈希代码。</returns>
            public override int GetHashCode(KeyValuePair<object, object> obj) =>
                RuntimeHelpers.GetHashCode(obj.Key) * -1521134295 + RuntimeHelpers.GetHashCode(obj.Value);
        }
    }
}
