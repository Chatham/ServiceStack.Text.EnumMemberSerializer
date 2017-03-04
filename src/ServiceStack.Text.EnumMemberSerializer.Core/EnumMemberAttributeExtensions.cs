using System;
using System.Runtime.Serialization;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal static class EnumMemberAttributeExtensions
    {
        public static bool MatchesDescription(this EnumMemberAttribute attribute, string description)
        {
            var attributeValue = attribute?.Value;
            return
                attributeValue != null
                && string.Equals(
                    attribute.Value, (description ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }
}