using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PereViader.Utils.Unity3d.CodeGen.Generators
{
    public static class CodeGenerationExtensions
    {
        public static INamedTypeSymbol GetEventFieldDeclarationSyntaxDelegateNamedTypeSymbol(EventFieldDeclarationSyntax eventFieldSyntax, SemanticModel semanticModel)
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
            if (delegateSymbol == null || delegateSymbol.DelegateInvokeMethod == null)
            {
                return ImmutableArray<IParameterSymbol>.Empty;
            }

            return delegateSymbol.DelegateInvokeMethod.Parameters;
        }
    }
}