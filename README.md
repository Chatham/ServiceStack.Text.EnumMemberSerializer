ServiceStack.Text.EnumMemberSerializer
======================================

Extension for [`ServiceStack.Text`](https://github.com/ServiceStack/ServiceStack.Text) to allow using [`EnumMemberAttribute`](http://msdn.microsoft.com/en-us/library/system.runtime.serialization.enummemberattribute.aspx) to serialize and deserialize enumerations. This allows you to use more human readable values while still leveraging the benefits of using enumerations.

Custom enumeration serialization currently only applies to the json serializer. It works by assigning custom delegates to [`JsConfig<T>.SerializeFn`](https://github.com/ServiceStack/ServiceStack.Text/blob/master/src/ServiceStack.Text/JsConfig.cs) and [`JsConfig<T>.DeSerializeFn`](https://github.com/ServiceStack/ServiceStack.Text/blob/master/src/ServiceStack.Text/JsConfig.cs).

#Example Configuration

Configure explicit enumerations:
```c#
new EnumSerializerConfigurator()
  .WithEnumTypes(new Type[] { typeof(ReturnPolicy) })
  .Configure();
```

Configure all enumerations in the ExampleCode namespace for all assemblies in my current app domain:
```c#
new EnumSerializerConfigurator()
  .WithAssemblies(AppDomain.CurrentDomain.GetAssemblies())
  .WithNamespaceFilter(ns => ns.StartsWith("ExampleCode"))
  .Configure();
```

#Example Enumeration
```c#
using System.Runtime.Serialization;

namespace ExampleCode
{
  public enum ReturnPolicy
  {
    NotSet = 0,
    [EnumMember(Value = @"90 Days w/Receipt")]
    _90DayswReceipt = 1,
    [EnumMember(Value = @"15% Restocking Fee")]
    _15RestockingFee = 2,
    [EnumMember(Value = @"Exchange Only")]
    ExchangeOnly = 3,
    [EnumMember(Value = @"As-Is")]
    AsIs = 4,
    ...
  }
}
```

#Example Dto
```c#
namespace ExampleCode
{
  public class ProductInfo
  {
    public string ProductName { get; set; }
    public ReturnPolicy ReturnPolicy { get; set; }
    ...
  }
}
```

#Url Examples

These will search for all product returnable within 90 days with receipt (all of these will work):
```
http://myhost/products?returnpolicy=90%20Days%20w%2FReceipt
http://myhost/products?returnpolicy=90%20DaYS%20w%2FReceIPt
http://myhost/products?returnpolicy=_90DayswReceipt
http://myhost/products?returnpolicy=1
```

#Example Response

With ServiceStack.Text.EnumMemberSerializer:
```JSON
[
   {
     "ProductName": "Hammer",
     "ReturnPolicy": "90 Days w/Receipt"
   },
   {
     "ProductName": "Chisel",
     "ReturnPolicy": "90 Days w/Receipt"
   }
]
```

Without ServiceStack.Text.EnumMemberSerializer:
```JSON
[
   {
     "ProductName": "Hammer",
     "ReturnPolicy": "_90DayswReceipt"
   },
   {
     "ProductName": "Chisel",
     "ReturnPolicy": "_90DayswReceipt"
   }
]
```
#Considerations
* Only configures public enumerations.
* `.WithNamespaceFilter()` only applies to filtering public enums found in the assemblies passed in using `.WithAssemblies()`. Any enumerations explicitly identified `.WithEnumTypes()` will not be filtered by namespace. The namespace filter applies to all provided assemblies.
* Multiple calls to `.WithEnumTypes()` and `.WithNamespaceFilter()` will be added and not replace previous specified values.
* This manipulates the static `JsConfig<T>`. Other code called later may overwrite the custom serialization/deserialization delegates.
* Both `.WithEnumTypes()` and `.WithAssemblies()` may be used at the same time, the results will be combined.
* Configure should be called before serializing/deserializing anything with `ServiceStack.Text` or the custom methods may not be setup correctly in `JsConfig`

#Using the Code

* [Install the NuGet Package](https://nuget.org/packages/ServiceStack.Text.EnumMemberSerializer/)
* You can check out the code and run build.bat.
  * It will create NuGet packages you can consume in `.\ReleasePackages` or you can directly use the resulting binaries. 
* Build requirements
  * .Net 4.0
  * Powershell 2.0
