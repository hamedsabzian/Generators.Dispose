using System.Runtime.InteropServices;

namespace Generators.SampleUsage;

public partial class SampleDisposable : IDisposable
{
    private Stream _managedResource = new MemoryStream();
    private IntPtr _unmanagedResource = Marshal.AllocHGlobal(100);

    partial void DisposeManaged()
    {
        _managedResource.Dispose();
        _managedResource = null!;
        Console.WriteLine($"Managed resources of {nameof(SampleDisposable)} are released");
    }

    partial void DisposeUnmanaged()
    {
        Marshal.FreeHGlobal(_unmanagedResource);
        _unmanagedResource = IntPtr.Zero;
        Console.WriteLine($"Unmanaged resources of {nameof(SampleDisposable)} are released");
    }
}