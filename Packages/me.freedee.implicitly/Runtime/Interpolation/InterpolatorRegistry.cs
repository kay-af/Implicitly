using System;
using System.Collections.Generic;
using UnityEngine;

namespace Implicitly
{
    public static class InterpolatorRegistry
    {
        private static readonly Dictionary<Type, IInterpolator> s_interpolators = new();

        public static void Register(Type type, IInterpolator interpolator) =>
            s_interpolators[type] = interpolator;

        public static void Register<T>(IInterpolator<T> interpolator) =>
            Register(typeof(T), interpolator);

        public static bool Has(Type type) => s_interpolators.ContainsKey(type);

        public static bool Has<T>() => Has(typeof(T));

        public static IInterpolator Get(Type type)
        {
            if (s_interpolators.TryGetValue(type, out var interpolator))
            {
                return interpolator;
            }

            throw new KeyNotFoundException($"Interpolator not registered for type {type.Name}");
        }

        public static IInterpolator<T> Get<T>() => (IInterpolator<T>)Get(typeof(T));

        public static bool TryGet(Type type, out IInterpolator interpolator) =>
            s_interpolators.TryGetValue(type, out interpolator);

        public static bool TryGet<T>(out IInterpolator<T> interpolator)
        {
            if (TryGet(typeof(T), out var value))
            {
                interpolator = (IInterpolator<T>)value;
                return true;
            }

            interpolator = null;
            return false;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            Register(new ColorInterpolator());
            Register(new DoubleInterpolator());
            Register(new FloatInterpolator());
            Register(new IntegerInterpolator());
            Register(new QuaternionInterpolator());
            Register(new Vector2Interpolator());
            Register(new Vector3Interpolator());
            Register(new Vector4Interpolator());
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration() => s_interpolators.Clear();
    }
}
