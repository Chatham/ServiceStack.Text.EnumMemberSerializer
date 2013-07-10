using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal static class AssemblyExtenstions
    {
        public static HashSet<Type> GetPublicEnums(this ICollection<Assembly> assemblies, Func<string, bool> enumNamespaceFilter)
        {
            if (assemblies.IsEmpty())
            {
                return new HashSet<Type>();
            }

            if (enumNamespaceFilter == null)
            {
                enumNamespaceFilter = EnumSerializerConfigurator.AlwaysTrueFilter;
            }

            var enumTypes = new List<Type>();

            foreach (var assembly in assemblies)
            {
                if (assembly != null)
                {
                    var assemblyPublicEnums =
                        (from publicEnumType in assembly.GetTypes().GetPublicEnums().AsParallel()
                         where enumNamespaceFilter(publicEnumType.Namespace)
                         select publicEnumType
                        ).ToList();
                    enumTypes.AddRange(assemblyPublicEnums);
                }
            }
            return new HashSet<Type>(enumTypes);
        }
    }
}