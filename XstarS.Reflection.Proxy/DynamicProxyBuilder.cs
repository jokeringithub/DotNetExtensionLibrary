﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供从原型类型和代理委托动态构造代理派生类型及其实例的方法。
    /// </summary>
    /// <typeparam name="T">代理类型的原型类型，应为接口或非密封类。</typeparam>
    public class DynamicProxyBuilder<T> : ProxyBuilderBase<T> where T : class
    {
        /// <summary>
        /// 初始化 <see cref="DynamicProxyBuilder{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        internal DynamicProxyBuilder() : this(typeof(T)) { }

        /// <summary>
        /// 以指定类型为原型类型初始化 <see cref="DynamicProxyBuilder{T}"/> 类的新实例。
        /// </summary>
        /// <param name="type">代理类型的原型类型的 <see cref="Type"/> 对象。</param>
        /// <exception cref="InvalidCastException">
        /// <paramref name="type"/> 类型的实例不能转换为 <typeparamref name="T"/> 类型。</exception>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        internal DynamicProxyBuilder(Type type) : base()
        {
            if (!typeof(T).IsAssignableFrom(type))
            {
                throw new InvalidCastException();
            }
            this.PrototypeType = type;
            this.InternalBuilder = DynamicObjectProxyBuilder.Create(type);
        }

        /// <summary>
        /// 代理类型的原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type PrototypeType { get; }

        /// <summary>
        /// 用于构造代理类型的 <see cref="DynamicObjectProxyBuilder"/> 对象。
        /// </summary>
        internal DynamicObjectProxyBuilder InternalBuilder { get; }

        /// <summary>
        /// 以 <typeparamref name="T"/> 为原型类型创建一个 <see cref="DynamicProxyBuilder{T}"/> 类的实例。
        /// </summary>
        /// <returns>以 <typeparamref name="T"/> 为原型类型的
        /// <see cref="DynamicProxyBuilder{T}"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static DynamicProxyBuilder<T> Create() => new DynamicProxyBuilder<T>();

        /// <summary>
        /// 以 <typeparamref name="T"/> 为原型类型创建一个 <see cref="DynamicProxyBuilder{T}"/> 类的实例，
        /// 并将指定 <see cref="OnInvokeHandler"/> 代理委托添加到所有可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <returns>以 <typeparamref name="T"/> 为原型类型的 <see cref="DynamicProxyBuilder{T}"/> 类的实例，
        /// 其中 <paramref name="handler"/> 代理委托已添加到所有可重写方法。</returns>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static DynamicProxyBuilder<T> Create(OnInvokeHandler handler)
        {
            var builder = DynamicProxyBuilder<T>.Create();
            builder.AddOnInvoke(handler);
            return builder;
        }

        /// <summary>
        /// 以 <typeparamref name="T"/> 为原型类型创建一个 <see cref="DynamicProxyBuilder{T}"/> 类的实例，
        /// 并根据指定规则将指定 <see cref="OnInvokeHandler"/> 代理委托添加到可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="methodFilter">筛选要添加代理委托的方法的 <see cref="Predicate{T}"/> 委托。</param>
        /// <returns>以 <typeparamref name="T"/> 为原型类型的 <see cref="DynamicProxyBuilder{T}"/> 类的实例，
        /// 其中 <paramref name="handler"/> 代理委托已根据
        /// <paramref name="methodFilter"/> 的指示添加到可重写方法。</returns>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static DynamicProxyBuilder<T> Create(
            OnInvokeHandler handler, Predicate<MethodInfo> methodFilter)
        {
            var builder = DynamicProxyBuilder<T>.Create();
            builder.AddOnInvoke(handler, methodFilter);
            return builder;
        }

        /// <summary>
        /// 以指定类型为原型类型创建一个 <see cref="DynamicProxyBuilder{T}"/> 类的实例。
        /// </summary>
        /// <param name="type">作为原型类型的 <see cref="Type"/> 对象。</param>
        /// <returns>以 <paramref name="type"/> 为原型类型的
        /// <see cref="DynamicProxyBuilder{T}"/> 类的实例。</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="type"/> 类型的实例不能转换为 <typeparamref name="T"/> 类型。</exception>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public static DynamicProxyBuilder<T> Create(Type type) => new DynamicProxyBuilder<T>(type);

        /// <summary>
        /// 以指定类型为原型类型创建一个 <see cref="DynamicProxyBuilder{T}"/> 类的实例，
        /// 并将指定 <see cref="OnInvokeHandler"/> 代理委托添加到所有可重写方法。
        /// </summary>
        /// <param name="type">作为原型类型的 <see cref="Type"/> 对象。</param>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <returns>以 <paramref name="type"/> 为原型类型的 <see cref="DynamicProxyBuilder{T}"/> 类的实例，
        /// 其中 <paramref name="handler"/> 代理委托已添加到所有可重写方法。</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="type"/> 类型的实例不能转换为 <typeparamref name="T"/> 类型。</exception>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public static DynamicProxyBuilder<T> Create(Type type, OnInvokeHandler handler)
        {
            var builder = DynamicProxyBuilder<T>.Create(type);
            builder.AddOnInvoke(handler);
            return builder;
        }

        /// <summary>
        /// 以指定类型为原型类型创建一个 <see cref="DynamicProxyBuilder{T}"/> 类的实例，
        /// 并根据指定规则将指定 <see cref="OnInvokeHandler"/> 代理委托添加到可重写方法。
        /// </summary>
        /// <param name="type">作为原型类型的 <see cref="Type"/> 对象。</param>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="methodFilter">筛选要添加代理委托的方法的 <see cref="Predicate{T}"/> 委托。</param>
        /// <returns>以 <paramref name="type"/> 为原型类型的 <see cref="DynamicProxyBuilder{T}"/> 类的实例，
        /// 其中 <paramref name="handler"/> 代理委托已根据
        /// <paramref name="methodFilter"/> 的指示添加到可重写方法。</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="type"/> 类型的实例不能转换为 <typeparamref name="T"/> 类型。</exception>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public static DynamicProxyBuilder<T> Create(
            Type type, OnInvokeHandler handler, Predicate<MethodInfo> methodFilter)
        {
            var builder = DynamicProxyBuilder<T>.Create(type);
            builder.AddOnInvoke(handler, methodFilter);
            return builder;
        }

        /// <summary>
        /// 将指定 <see cref="OnInvokeHandler"/> 代理委托添加到指定的可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="method">要添加代理委托的可重写方法的 <see cref="MemberInfo"/> 对象。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="method"/> 不为原型类型中的可重写方法。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyBuilderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler, MethodInfo method) =>
            this.InternalBuilder.AddOnInvoke(handler, method);

        /// <summary>
        /// 将指定 <see cref="OnInvokeHandler"/> 代理委托添加到指定的多个可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="methods">要添加代理委托的多个可重写方法的 <see cref="MemberInfo"/> 对象。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="methods"/> 不全为原型类型中的可重写方法。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyBuilderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler, params MethodInfo[] methods) =>
            this.InternalBuilder.AddOnInvoke(handler, methods);

        /// <summary>
        /// 将指定 <see cref="OnInvokeHandler"/> 代理委托添加到指定的多个可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="methods">要添加代理委托的多个可重写方法的 <see cref="MemberInfo"/> 对象。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="methods"/> 不全为原型类型中的可重写方法。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyBuilderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler, IEnumerable<MethodInfo> methods) =>
            this.InternalBuilder.AddOnInvoke(handler, methods);

        /// <summary>
        /// 根据指定规则将指定 <see cref="OnInvokeHandler"/> 代理委托添加到可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="methodFilter">筛选要添加代理委托的方法的 <see cref="Predicate{T}"/> 委托。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyBuilderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler, Predicate<MethodInfo> methodFilter) =>
            this.InternalBuilder.AddOnInvoke(handler, methodFilter);

        /// <summary>
        /// 将指定 <see cref="OnInvokeHandler"/> 代理委托添加到所有可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyBuilderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler) =>
            this.InternalBuilder.AddOnInvoke(handler);

        /// <summary>
        /// 构造代理派生类型。
        /// </summary>
        /// <returns>构造完成的派生类型。</returns>
        protected override Type BuildProxyType() => this.InternalBuilder.ProxyType;
    }
}
