using System;
using System.Collections.Generic;
using System.Linq;

namespace Praedium.Services
{
    public class Injector
    {
        private static readonly Dictionary<Type, object> _singletons = new Dictionary<Type, object>();
        private static readonly Dictionary<Type, HashSet<Type>> _registrations = new Dictionary<Type, HashSet<Type>>();

        public static TType GetSingleton<TType>() where TType : class
        {
            if (_singletons.TryGetValue(typeof(TType), out var s))
                return s as TType;

            if (_registrations.TryGetValue(typeof(TType), out var values))
            {
                foreach (var val in values)
                {
                    if (_singletons.TryGetValue(val, out var singleton))
                        return singleton as TType;
                }
            }

            if(!typeof(TType).GetConstructors().Any(c => c.GetParameters().Length == 0 && c.IsPublic))
                throw new ArgumentException("Given type has no empty, public ctor");

            var result = Activator.CreateInstance<TType>();
            _singletons.Add(typeof(TType), result);
            return result;
        }

        public static void Register<TType, TRealType>()
        {
            if(!_registrations.ContainsKey(typeof(TType)))
                _registrations.Add(typeof(TType), new HashSet<Type>());

            if(!_registrations.ContainsKey(typeof(TRealType)))
                _registrations.Add(typeof(TRealType), new HashSet<Type>());

            // register it both ways
            if (!_registrations[typeof(TType)].Contains(typeof(TRealType)))
                _registrations[typeof(TType)].Add(typeof(TRealType));

            if (!_registrations[typeof(TRealType)].Contains(typeof(TType)))
                _registrations[typeof(TRealType)].Add(typeof(TType));
        }
    }
}