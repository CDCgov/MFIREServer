using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MFireProtocol
{
    /// <summary>
    /// An ID is needed for each command to identify the type when it is deserialized over TCP.
    /// </summary>
    public static class CommandIds
    {
        private static Dictionary<Type, UInt64> types = new Dictionary<Type, UInt64>();
        private static Dictionary<UInt64, Type> ids = new Dictionary<UInt64, Type>();

        static CommandIds()
        {
            UInt64 id = 1;
            foreach(var t in FindAllDerivedTypes<MFireCmd>())
                addType(t, id++);
        }

        /// <summary>
        /// Get the ID for a given type.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static UInt64 GetId(Type t)
        {
            if (!types.ContainsKey(t))
                throw new ArgumentException($"No command ID for type: {t.FullName}", nameof(t));
            return types[t];
        }

        /// <summary>
        /// Get the type from a given ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Type GetType(UInt64 id)
        {
            if (!ids.ContainsKey(id))
                throw new ArgumentException($"No matching command for ID: {id}", nameof(id));
            return ids[id];
        }

        private static void addType(Type t, UInt64 id)
        {
            types.Add(t, id);
            ids.Add(id, t);
        }

        private static List<Type> FindAllDerivedTypes<T>()
        {
            return FindAllDerivedTypes<T>(Assembly.GetAssembly(typeof(T)));
        }

        private static List<Type> FindAllDerivedTypes<T>(Assembly assembly)
        {
            var derivedType = typeof(T);
            return assembly
                .GetTypes()
                .Where(t =>
                    t != derivedType &&
                    derivedType.IsAssignableFrom(t)
                    ).ToList();
        }
    }
}
