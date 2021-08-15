using System;
using System.Collections.Concurrent;

#nullable enable
namespace SweetMoleHouse.MarioForever.Scripts.Util
{
    public static class Pool
    {
        public static Pool<T> Of<T>(Action<T>? initializer = null, Action<T>? finalizer = null) where T : new()
        {
            return new Pool<T>(() => new T(), initializer, finalizer);
        }
    }

    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T">对象池中对象的类型</typeparam>
    public class Pool<T>
    {
        private readonly Func<T> factory;
        private readonly Action<T>? initializer;
        private readonly Action<T>? finalizer;
        private readonly ConcurrentBag<T> storage = new ConcurrentBag<T>();

        public Pool(Func<T> factory, Action<T>? initializer = null, Action<T>? finalizer = null)
        {
            this.factory = factory;
            this.initializer = initializer;
            this.finalizer = finalizer;
        }

        public T Rent()
        {
            if (storage.TryTake(out T result))
            {
                initializer?.Invoke(result);
                return result;
            }

            return factory();
        }

        public void Return(T instance)
        {
            finalizer?.Invoke(instance);
            storage.Add(instance);
        }
    }
}
#nullable disable
