﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace ServiceStack.Text.EnumMemberSerializer
{
    /// <summary>
    ///     Fluent configuration for the custom enumeration serializer.
    /// </summary>
    public interface IEnumSerializerConfigurator
    {
        /// <summary>
        ///     Filter to apply to namespaces.
        /// </summary>
        /// <param name="enumNamespaceFilter">Returns true for an acceptable namespace.</param>
        IEnumSerializerConfigurator WithNamespaceFilter(Func<string, bool> enumNamespaceFilter);

        /// <summary>
        ///     Specifies assemblies to search.
        /// </summary>
        /// <param name="assembliesToScan"></param>
        IEnumSerializerConfigurator WithAssemblies(ICollection<Assembly> assembliesToScan);

        /// <summary>
        ///     Specifies individual enumeration types.
        /// </summary>
        IEnumSerializerConfigurator WithEnumTypes(ICollection<Type> enumTypes);

        /// <summary>
        /// This will configure the nullable enumeration as well as the non-nullable enumeration (recommended).
        /// </summary>
        [Obsolete("Nullable enumerations are always configured.")]
        IEnumSerializerConfigurator WithNullableEnumSerializers();

        /// <summary>
        ///     Perform configuration action.
        /// </summary>
        void Configure();
    }
}