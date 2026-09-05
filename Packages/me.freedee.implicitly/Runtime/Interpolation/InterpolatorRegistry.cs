using System;
using System.Collections.Generic;
using UnityEngine;

namespace Implicitly
{
    public static class InterpolatorRegistry
    {
        private static readonly Dictionary<Type, IInterpolator> s_interpolators = new();

        public static void Register(Type type, IInterpolator interpolator)
        {
            ThrowIfNotValueType(type);

            s_interpolators[type] = interpolator;
        }

        public static void Register<T>(IInterpolator<T> interpolator)
            where T : struct => Register(typeof(T), interpolator);

        public static bool Has(Type type)
        {
            ThrowIfNotValueType(type);

            return s_interpolators.ContainsKey(type);
        }

        public static bool Has<T>() => Has(typeof(T));

        public static IInterpolator Get(Type type)
        {
            ThrowIfNotValueType(type);

            if (s_interpolators.TryGetValue(type, out var interpolator))
            {
                return interpolator;
            }

            throw new KeyNotFoundException($"Interpolator not registered for type: {type.Name}");
        }

        public static IInterpolator<T> Get<T>()
            where T : struct => (IInterpolator<T>)Get(typeof(T));

        public static bool TryGet(Type type, out IInterpolator interpolator)
        {
            ThrowIfNotValueType(type);

            return s_interpolators.TryGetValue(type, out interpolator);
        }

        public static bool TryGet<T>(out IInterpolator<T> interpolator)
            where T : struct
        {
            if (TryGet(typeof(T), out var value))
            {
                interpolator = (IInterpolator<T>)value;
                return true;
            }

            interpolator = null;
            return false;
        }

        private static void ThrowIfNotValueType(Type type)
        {
            if (!type.IsValueType || Nullable.GetUnderlyingType(type) != null)
            {
                throw new ArgumentException(
                    $"Interpolators can only be registered for non-nullable value types, but got: {type.Name}",
                    nameof(type)
                );
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            Register(new ColorInterpolator());
            Register(new Color32Interpolator());
            Register(new DoubleInterpolator());
            Register(new FloatInterpolator());
            Register(new IntegerInterpolator());
            Register(new QuaternionInterpolator());
            Register(new RectInterpolator());
            Register(new Vector2Interpolator());
            Register(new Vector2IntInterpolator());
            Register(new Vector3Interpolator());
            Register(new Vector3IntInterpolator());
            Register(new Vector4Interpolator());
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration() => s_interpolators.Clear();
    }
}
