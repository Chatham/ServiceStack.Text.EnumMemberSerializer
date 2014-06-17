using System;

namespace ServiceStack.Text.EnumMemberSerializer
{
    /// <summary>
    ///     Serialize/Deserialize enumerations using EnumMember Attribute Value if present.
    /// </summary>
    public static class PrettyEnumExtensions
    {
        /// <summary>
        ///     Gets the optimal string representation of a nullable enumeration.
        /// </summary>
        /// <returns>Returns EnumMember Value if present, otherwise returns enumValue.ToString().</returns>
        /// <exception cref="InvalidOperationException">The type parameter is not an Enumeration.</exception>
        /// <typeparam name="TEnum">The type must be an enumeration.</typeparam>
        public static string GetOptimalEnumDescription<TEnum>(this TEnum? enumValue) where TEnum : struct
        {
            return PrettyEnumHelpers<TEnum>.GetOptimalEnumDescription(enumValue);
        }

        /// <summary>
        ///     Gets the optimal string representation of an enumeration.
        /// </summary>
        /// <returns>Returns EnumMember Value if present, otherwise returns enumValue.ToString().</returns>
        /// <exception cref="InvalidOperationException">The type parameter is not an Enumeration.</exception>
        /// <typeparam name="TEnum">The type must be an enumeration.</typeparam>
        public static string GetOptimalEnumDescription<TEnum>(this TEnum enumValue) where TEnum : struct
        {
            return PrettyEnumHelpers<TEnum>.GetOptimalEnumDescription(enumValue);
        }

        /// <summary>
        ///     Gets the enumeration for the given string representation.
        /// </summary>
        /// <param name="enumValue">The EnumMemberAttribute value, enumeration string value, or integer value (as string) of the enumeration.</param>
        /// <exception cref="InvalidOperationException">The type parameter is not an Enumeration.</exception>
        /// <typeparam name="TEnum">The type must be an enumeration.</typeparam>
        public static TEnum GetEnum<TEnum>(this string enumValue) where TEnum : struct
        {
            return PrettyEnumHelpers<TEnum>.GetEnumFrom(enumValue);
        }

        /// <summary>
        ///     Gets the nullable enumeration for the given string representation.
        /// </summary>
        /// <param name="enumValue">The EnumMemberAttribute value, enumeration string value, or integer value (as string) of the enumeration.</param>
        /// <exception cref="InvalidOperationException">The type parameter is not an Enumeration.</exception>
        /// <typeparam name="TEnum">The type must be an enumeration.</typeparam>
        public static TEnum? GetNullableEnum<TEnum>(this string enumValue) where TEnum : struct
        {
            return PrettyEnumHelpers<TEnum>.GetNullableEnumFrom(enumValue);
        }
    }
}