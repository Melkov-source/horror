using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Code.Utils
{
    public static class TypeUtils
    {
        public static List<Type> GetImplementationsOfInterface<TInterface>()
        {
            var interface_type = typeof(TInterface);
            var result = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
				
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null).ToArray();
                }

                for (int index = 0, count = types.Length; index < count; ++index)
                {
                    var type = types[index];
					
                    if (type.IsClass && type.IsAbstract == false && interface_type.IsAssignableFrom(type))
                    {
                        result.Add(type);
                    }
                }
            }

            return result;
        }
        
        public static ushort GetStableHash(this Type controller)
        {
            var key = controller.FullName;

            if (key == null)
            {
                return 0;
            }

            unchecked
            {
                return (ushort)key.Aggregate(23, (current, c) => current * 31 + c);
            }
        }
    }
}