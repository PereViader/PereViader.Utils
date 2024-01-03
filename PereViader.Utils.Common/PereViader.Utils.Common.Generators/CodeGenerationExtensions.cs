using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PereViader.Utils.Common.Generators
{
    public static class CodeGenerationExtensions
    {
        public static void ReportDebugDiagnostic(this GeneratorExecutionContext context, string message)
        {
            var diagnosticDescriptor = new DiagnosticDescriptor("debug-message", "title", message, string.Empty, DiagnosticSeverity.Error, true);
            var diagnostic = Diagnostic.Create(diagnosticDescriptor, Location.None);
            context.ReportDiagnostic(diagnostic);
        }
        
        public static string GetGenericArgumentIdentifiers(this TypeDeclarationSyntax typeDeclaration)
        {
            if (typeDeclaration.TypeParameterList == null)
            {
                return string.Empty;
            }
            
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<");
            for (var index = 0; index < typeDeclaration.TypeParameterList.Parameters.Count - 1; index++)
            {
                var typeParam = typeDeclaration.TypeParameterList.Parameters[index];
                stringBuilder.Append(typeParam.Identifier.ValueText);
                stringBuilder.Append(", ");
            }

            stringBuilder.Append(typeDeclaration.TypeParameterList
                .Parameters[typeDeclaration.TypeParameterList.Parameters.Count - 1].Identifier.ValueText);
            stringBuilder.Append(">");

            return stringBuilder.ToString();
        }
        
        public static SyntaxList<UsingDirectiveSyntax>? GetUsingDirectives(this SyntaxNode node)
        {
            var root = node.SyntaxTree.GetRoot() as CompilationUnitSyntax;
            return root?.Usings;
        }

        public static INamedTypeSymbol? GetEventFieldDeclarationSyntaxDelegateNamedTypeSymbol(EventFieldDeclarationSyntax eventFieldSyntax, SemanticModel semanticModel)
        {
            // Assuming there's only one variable in the declaration.
            // For multiple variables, you need to handle each separately.
            var variable = eventFieldSyntax.Declaration.Variables.FirstOrDefault();
            if (variable == null)
            {
                return null;
            }

            // Get the symbol for the event
            if (!(semanticModel.GetDeclaredSymbol(variable) is IEventSymbol eventSymbol))
            {
                return null;
            }

            // The Type of the event symbol should be the delegate type
            return (INamedTypeSymbol)eventSymbol.Type;
        }
        
        public static ImmutableArray<IParameterSymbol> GetDelegateParameters(INamedTypeSymbol delegateSymbol)
        {
            return delegateSymbol.DelegateInvokeMethod?.Parameters ?? ImmutableArray<IParameterSymbol>.Empty;
        }
    }
}