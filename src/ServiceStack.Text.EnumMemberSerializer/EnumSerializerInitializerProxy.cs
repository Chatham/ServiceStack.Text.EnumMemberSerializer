using System;
using System.Reflection;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal class EnumSerializerInitializerProxy : IEnumSerializerInitializerProxy
    {
        //Hide the static class interaction as much as possible
        public void ConfigEnumSerializers(Type type)
        {
            ExecuteConfigureMethod(nameof(EnumSerializerInitializer<int>.InitializeEnumSerializer), type);
        }

        public void ConfigEnumAndNullableEnumSerializers(Type type)
        {
            ExecuteConfigureMethod(nameof(EnumSerializerInitializer<int>.InitializeEnumAndNullableEnumSerializer), type);
        }

        private static void ExecuteConfigureMethod(string methodName, Type type)
        {
            var genericType = typeof(EnumSerializerInitializer<>).MakeGenericType(type);
            var genericTypeMyMethodInfo = genericType.GetTypeInfo().GetDeclaredMethod(methodName);
          
            genericTypeMyMethodInfo.Invoke(Activator.CreateInstance(genericType), null);
        }
    }
}