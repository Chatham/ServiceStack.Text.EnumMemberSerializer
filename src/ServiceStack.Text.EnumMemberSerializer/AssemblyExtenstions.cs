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
            
            var publicEnumtypes =
                assemblies
                    .Where(x => x != null)
                    .SelectMany(x => x.DefinedTypes)
                    .Where(x => enumNamespaceFilter(x.Namespace ?? ""))
                    .GetPublicEnums();

            return publicEnumtypes;
        }
    }
}