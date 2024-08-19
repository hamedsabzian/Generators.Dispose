## How to use?

You can install its latest version using your IDE Nuget package manager by using dotnet command as follows:

```bash
dotnet add package Generators.Dispose
```
| Package | Version |
|:-------:|:-------:|
| Generators.Dispose | [![NuGet Badge](https://buildstats.info/nuget/Generators.Dispose)](https://www.nuget.org/packages/Generators.Dispose) |

It generates a partial class for all partial classes that implement `IDisposable` interface.

```csharp
public partial class SampleClass : IDisposable
{
}
```

The generated class declares `DisposeManaged` partial method that will be called to release managed resources.

```csharp
public partial class SampleClass : IDisposable
{
    partial void DisposeManaged()
    {
        // Call Dispose method of the managed resources
    }
}
```

If your class works with unmanaged resources, you need to implement `DisposeUnmanaged` partial method.

```csharp
public partial class SampleClass : IDisposable
{
    partial void DisposeManaged()
    {
        // Call Dispose method of the managed resources
    }

    partial void DisposeUnmanaged()
    {
        // Release the managed resources
    }
}
```

If you have a partial class implements `IDisposable` interface, but you want to exclude it from generation, just use `IgnoreGeneration` attribute.

```csharp
[IgnoreGeneration]
public partial class IgnoredDisposable : IDisposable
{
    // Implement Dispose pattern manually
}
```