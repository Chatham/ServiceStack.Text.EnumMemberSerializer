using System.Runtime.Serialization;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    public enum FakeTestingEnum
    {
        MyValue = 99,
        [EnumMember(Value = "Your Value is worse")]
        YourValue = 123
    }
}