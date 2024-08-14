using Microsoft.CodeAnalysis;

namespace Generators;

[Generator]
public class RequirementsGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // This method left empty intentionally
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var source = @"// Auto-generated code
using System;

namespace Generators
{
    public class IgnoreGenerationAttribute : Attribute
    {
    }
}
";
        context.AddSource("IgnoreGenerationAttribute.g.cs", source);
    }
}