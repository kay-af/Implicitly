using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Implicitly
{
    public static class EqualityComparerRegistry
    {
        private static readonly Dictionary<Type, IEqualityComparer> s_comparers = new();

        public static void Register(Type type, IEqualityComparer comparer) =>
            s_comparers[type] = comparer;

        public static void Register<T>(IEqualityComparer<T> comparer) =>
            Register(typeof(T), (IEqualityComparer)comparer);

        public static bool Has(Type type) => s_comparers.ContainsKey(type);

        public static bool Has<T>() => Has(typeof(T));

        public static IEqualityComparer Get(Type type)
        {
            if (s_comparers.TryGetValue(type, out var comparer))
            {
                return comparer;
            }

            throw new KeyNotFoundException($"EqualityComparer not registered for type {type.Name}");
        }

        public static IEqualityComparer<T> Get<T>() => (IEqualityComparer<T>)Get(typeof(T));

        public static bool TryGet(Type type, out IEqualityComparer comparer) =>
            s_comparers.TryGetValue(type, out comparer);

        public static bool TryGet<T>(out IEqualityComparer<T> comparer)
        {
            if (TryGet(typeof(T), out var value))
            {
                comparer = (IEqualityComparer<T>)value;
                return true;
            }

            comparer = null;
            return false;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration() => s_comparers.Clear();
    }
}
