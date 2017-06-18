using System;
using System.Reflection;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal static class FieldInfoExtensions
    {
        public static bool MatchesDescription(this FieldInfo field, string description)
        {
            return string.Equals(field.Name, (description ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }
}