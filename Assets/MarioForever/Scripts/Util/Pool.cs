using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

#nullable enable
namespace SweetMoleHouse.MarioForever.Scripts.Util
{
    /// <summary>
    /// 工厂类
    /// 生产线程*不*安全的对象池。
    /// </summary>
    public static class Pool
    {
        public static Pool<T> Of<T>(Action<T>? initializer = null, Action<T>? finalizer = null)
            where T : class, new()
        {
            return new Pool<T>(() => new T(), initializer, finalizer);
        }
    }

    /// <summary>
    /// 工厂类，
    /// 生产线程安全的对象池。
    /// </summary>
    public static class ConcurrentPool
    {
        public static ConcurrentPool<T> Of<T>(Action<T>? initializer = null, Action<T>? finalizer = null)
            where T : class, new()
        {
            return new ConcurrentPool<T>(() => new T(), initializer, finalizer);
        }
    }

    /// <summary>
    /// 对象池
    /// </summary>
    public abstract class AbstractPool<T>
    {
        private readonly Func<T> _factory;
        private readonly Action<T>? _initializer;
        private readonly Action<T>? _finalizer;

        internal AbstractPool(Func<T> factory, Action<T>? initializer = null, Action<T>? finalizer = null)
        {
            _factory = factory;
            _initializer = initializer;
            _finalizer = finalizer;
        }

        public T Rent()
        {
            if (TryTake(out T result))
            {
                _initializer?.Invoke(result);
                return result;
            }

            return _factory();
        }

        public void Return(T instance)
        {
            _finalizer?.Invoke(instance);
            Add(instance);
        }

        /// <summary>
        /// 尝试从对象池中取出一个对象，不执行<see cref="_initializer"/>>
        /// </summary>
        protected abstract bool TryTake(out T result);

        /// <summary>
        /// 将一个实例加入对象池，不执行<see cref="_finalizer"/>
        /// </summary>
        protected abstract void Add(T instance);
    }

    /// <summary>
    /// 线程*不*安全的对象池
    /// </summary>
    public sealed class Pool<T> : AbstractPool<T>
    {
        private readonly Stack<T> _storage = new Stack<T>();

        public Pool(Func<T> factory, Action<T>? initializer = null, Action<T>? finalizer = null)
            : base(factory, initializer, finalizer)
        {
        }

        protected override bool TryTake(out T result)
        {
            if (_storage.Count > 0)
            {
                result = _storage.Pop();
                return true;
            }

            result = default!;
            return false;
        }

        protected override void Add(T instance) => _storage.Push(instance);
    }

    /// <summary>
    /// 线程安全的对象池
    /// </summary>
    public sealed class ConcurrentPool<T> : AbstractPool<T>
    {
        private readonly ConcurrentBag<T> _storage = new ConcurrentBag<T>();

        public ConcurrentPool(Func<T> factory, Action<T>? initializer = null, Action<T>? finalizer = null)
            : base(factory, initializer, finalizer)
        {
        }

        protected override bool TryTake(out T result) => _storage.TryTake(out result);

        protected override void Add(T instance) => _storage.Add(instance);
    }
}
#nullable disable