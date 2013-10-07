using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ServiceStack.Text.EnumMemberSerializer
{
    /// <summary>
    ///     Fluent configuration for the enum member enumeration serializer
    /// </summary>
    public sealed class EnumSerializerConfigurator : IEnumSerializerConfigurator
    {
        private readonly HashSet<Assembly> _assembliesToScan = new HashSet<Assembly>();
        private readonly HashSet<Type> _enumTypes = new HashSet<Type>();
        private Func<string, bool> _enumNamespaceFilter = AlwaysTrueFilter;

        private IEnumSerializerInitializerProxy _jsConfigManager;

        internal static Func<string, bool> AlwaysTrueFilter
        {
            get { return s => true; }
        }

        internal IEnumSerializerInitializerProxy JsConfigProxy
        {
            get { return _jsConfigManager ?? (_jsConfigManager = new EnumSerializerInitializerProxy()); }
            set { _jsConfigManager = value; }
        }
        
        /// <summary>
        ///     Only configure enumerations that match the provided namespace filter.
        ///     This filter applies to the types found in the provided assembly list.
        /// </summary>
        /// <param name="enumNamespaceFilter">Returns true for an acceptable namespace.</param>
        public IEnumSerializerConfigurator WithNamespaceFilter(Func<string, bool> enumNamespaceFilter)
        {
            if (enumNamespaceFilter != null)
            {
                _enumNamespaceFilter = enumNamespaceFilter;
            }

            return this;
        }

        /// <summary>
        ///     Search the provided assemblies for enumerations to configure.
        ///     Multiple calls will add to the existing list.
        /// </summary>
        /// <param name="assembliesToScan"></param>
        public IEnumSerializerConfigurator WithAssemblies(ICollection<Assembly> assembliesToScan)
        {
            if (!assembliesToScan.IsEmpty())
            {
                foreach (var assembly in assembliesToScan)
                {
                    if (assembly != null)
                    {
                        _assembliesToScan.Add(assembly);
                    }
                }
            }

            return this;
        }

        /// <summary>
        ///     Allows individual enumeration types to be specified.
        ///     Multiple calls will add to the existing list.
        /// </summary>
        public IEnumSerializerConfigurator WithEnumTypes(ICollection<Type> enumTypes)
        {
            if (!enumTypes.IsEmpty())
            {
                var publicEnums = enumTypes.GetPublicEnums();
                _enumTypes.UnionWith(publicEnums);
            }

            return this;
        }

        /// <summary>
        ///     Configures ServiceStack JsConfig with the custom enumeration serializers.
        /// </summary>
        public void Configure()
        {
            var assemblyPublicEnums = _assembliesToScan.GetPublicEnums(_enumNamespaceFilter);

            foreach (var assemblyPublicEnum in assemblyPublicEnums)
            {
                _enumTypes.Add(assemblyPublicEnum);
            }

            Parallel.ForEach(_enumTypes, JsConfigProxy.ConfigEnumSerializers);
        }
    }
}