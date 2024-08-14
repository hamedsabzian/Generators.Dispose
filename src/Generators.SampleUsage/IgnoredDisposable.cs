namespace Generators.SampleUsage;

[IgnoreGeneration]
public partial class IgnoredDisposable : IDisposable
{
    // To detect redundant calls
    private bool _disposed;

    ~IgnoredDisposable() => Dispose(false);

    // Public implementation of Dispose pattern callable by consumers.
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Protected implementation of Dispose pattern.
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            _disposed = true;
        }
    }
}