using System;
using System.Runtime.Serialization;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal static class EnumMemberAttributeExtensions
    {
        public static bool MatchesDescription(this EnumMemberAttribute attribute, string description)
        {
            return
                attribute != null && string.Equals(attribute.Value, description, StringComparison.OrdinalIgnoreCase);
        }
    }
}