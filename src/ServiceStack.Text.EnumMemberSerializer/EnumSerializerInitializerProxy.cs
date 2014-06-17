using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal class EnumSerializerInitializerProxy : IEnumSerializerInitializerProxy
    {
        //Hide the static class interaction as much as possible
        public void ConfigEnumSerializers(Type type)
        {
            var mi = GetMethodInfo<EnumSerializerInitializer<int>>(x => x.InitializeEnumSerializer());
            ExecuteConfigureMethod(mi, type);
        }

        private static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
        {
            return (expression.Body as MethodCallExpression).Method;
        }

        public void ConfigEnumAndNullableEnumSerializers(Type type)
        {
            var mi = GetMethodInfo<EnumSerializerInitializer<int>>(x => x.InitializeEnumAndNullableEnumSerializer());
            ExecuteConfigureMethod(mi, type);
        }

        private static void ExecuteConfigureMethod(MethodInfo mi, Type type)
        {
            var genericType = typeof(EnumSerializerInitializer<>).MakeGenericType(new[] { type });
            var genericTypeMyMethodInfo = genericType.GetMethod(mi.Name);

            genericTypeMyMethodInfo.Invoke(Activator.CreateInstance(genericType), null);
        }
    }
}