using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal static class EnumTypeExtensions
    {
        public static HashSet<Type> GetPublicEnums(this IEnumerable<Type> types)
        {
            if (types == null)
            {
                return new HashSet<Type>();
            }

            return types.Where(x => x != null).Select(x => x.GetTypeInfo()).GetPublicEnums();
        }

        public static HashSet<Type> GetPublicEnums(this IEnumerable<TypeInfo> typeInfos)
        {
            if (typeInfos == null)
            {
                return new HashSet<Type>();
            }

            var enumTypes =
                (from info in typeInfos
                 where
                     info != null
                     && info.IsEnum
                     && info.IsPublic
                 select info.AsType()
                ).ToList();

            return new HashSet<Type>(enumTypes);
        }
    }
}