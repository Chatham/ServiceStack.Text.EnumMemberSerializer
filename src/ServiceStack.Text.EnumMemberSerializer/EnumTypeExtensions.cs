using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal static class EnumTypeExtensions
    {
        public static HashSet<Type> GetPublicEnums(this ICollection<Type> types)
        {
            if (types.IsEmpty())
            {
                return new HashSet<Type>();
            }

            var enumTypes = 
                (from type in types.AsParallel()
                 where
                     type != null
                     && type.IsEnum
                     && type.IsPublic
                 select type
                ).ToList();

            return new HashSet<Type>(enumTypes);
        }
    }
}