using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PereViader.Utils.Common.Generators
{
    public static class CodeGenerationExtensions
    {
        public static SyntaxList<UsingDirectiveSyntax>? GetUsingDirectives(this SyntaxNode node)
        {
            var root = node.SyntaxTree.GetRoot() as CompilationUnitSyntax;
            return root?.Usings;
        }
        
        public static NamespaceDeclarationSyntax GetNamespaceDeclarationSyntax(SyntaxNode node)
        {
            // Attention: Unity is not compatible with this, not adding support for now.
            
            // Check for file-scoped namespace
            // var root = node.SyntaxTree.GetRoot() as CompilationUnitSyntax;
            // var fileScopedNamespace = root?.Members.OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault();
            //
            // if (fileScopedNamespace != null)
            // {
            //     return fileScopedNamespace;
            // }

            // If not file-scoped, get the nearest enclosing namespace
            var namespaceDeclaration = node.Ancestors().OfType<NamespaceDeclarationSyntax>().LastOrDefault();
            return namespaceDeclaration;
        }
        
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