using System.Runtime.Serialization;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    public enum FakeTestingEnum
    {
        MyValue = 99,
        [EnumMember(Value = "Your Value is worse")]
        YourValue = 123,

        [EnumMember(Value = "Commas")]
        Commas = 124,
        [EnumMember(Value = "Problems")]
        Problems = 125,
        [EnumMember(Value = "Commas, Problems")]
        CommasProblems = 126


    }
}