using Generators.Receivers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Generators;

[Generator]
public class DisposableGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // Register a syntax receiver to collect all candidate classes
        context.RegisterForSyntaxNotifications(() => new DisposableSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // Retrieve the populated receiver
        if (context.SyntaxContextReceiver is not DisposableSyntaxReceiver receiver)
            return;

        foreach (var candidateClass in receiver.CandidateClasses)
        {
            if (context.Compilation.GetSemanticModel(candidateClass.SyntaxTree)
                    .GetDeclaredSymbol(candidateClass) is not ITypeSymbol candidateType)
            {
                continue;
            }

            var className = candidateClass.Identifier.Text;
            var namespaceName = candidateType.ContainingNamespace.IsGlobalNamespace
                ? string.Empty
                : candidateType.ContainingNamespace.ToDisplayString();

            var source = GenerateSource(namespaceName, className);
            context.AddSource($"{className}Disposable.g.cs", source);
        }
    }

    private static string GenerateSource(string namespaceName, string className)
    {
        var source = "// Auto-generated code";
        if (!string.IsNullOrWhiteSpace(namespaceName))
        {
            source += $@"
namespace {namespaceName}
{{";
        }

        source += $@"
    partial class {className}
    {{
        private bool _disposed = false;

        public void Dispose()
        {{
            Dispose(true);
            GC.SuppressFinalize(this);
        }}

        protected virtual void Dispose(bool disposing)
        {{
            if (!_disposed)
            {{
                if (disposing)
                {{
                    DisposeManaged();
                }}

                DisposeUnmanaged();
                _disposed = true;
            }}
        }}

        ~{className}()
        {{
            Dispose(false);
        }}

        partial void DisposeManaged();

        partial void DisposeUnmanaged();
    }}";
        if (!string.IsNullOrWhiteSpace(namespaceName))
        {
            source += @"
}";
        }

        return source;
    }
}