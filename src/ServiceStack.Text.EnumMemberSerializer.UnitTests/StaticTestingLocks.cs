namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    public static class StaticTestingLocks
    {
        public static object JsConfigLockObject = new object();
        public static object DeserializeCacheLockObject = new object();
        public static object SerializeCacheLockObject = new object();
    }
}