using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators.Receivers;

/// <summary>
/// The syntax receiver collects candidate disposable classes during the syntax tree walk
/// </summary>
class DisposableSyntaxReceiver : ISyntaxContextReceiver
{
    public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        // Look for partial classes that implement IDisposable
        if (context.Node is ClassDeclarationSyntax classDeclaration &&
            classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword))
        {
            var semanticModel = context.SemanticModel;
            var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
            if (IsDisposableCandidate(classSymbol))
            {
                CandidateClasses.Add(classDeclaration);
            }
        }
    }

    private static bool IsDisposableCandidate(INamedTypeSymbol classSymbol)
    {
        return classSymbol != null && classSymbol.AllInterfaces.Any(i => i.ToString() == "System.IDisposable")
                                   && !HasAttribute(classSymbol, "IgnoreGeneration", "IgnoreGenerationAttribute");
    }

    private static bool HasAttribute(INamedTypeSymbol classSymbol, params string[] attributes)
    {
        return classSymbol.GetAttributes().Any(ad => attributes.Contains(ad.AttributeClass?.ToDisplayString()));
    }
}