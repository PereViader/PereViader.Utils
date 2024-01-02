using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace PereViader.Utils.Common.Generators
{
    [Generator]
    public class ToStringSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // Register a syntax receiver that will be created for each compilation
            context.RegisterForSyntaxNotifications(() => new TypeDeclarationWithAttributeSyntaxReceiver<TypeDeclarationSyntax, ToStringSourceGenerator>("GenerateToString"));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // Retrieve the populated receiver containing the candidate classes/structs
            if (!(context.SyntaxReceiver is TypeDeclarationWithAttributeSyntaxReceiver<TypeDeclarationSyntax, ToStringSourceGenerator> receiver))
                return;

            foreach (var candidate in receiver.Candidates)
            {
                var model = context.Compilation.GetSemanticModel(candidate.Declaration.SyntaxTree);
                if (model.GetDeclaredSymbol(candidate.Declaration) is not INamedTypeSymbol symbol)
                {
                    continue;
                }

                var properties = symbol.GetMembers().Where(x => x.Kind is SymbolKind.Property);
                
                var sourceBuilder = new StringBuilder($@"
namespace {symbol.ContainingNamespace.ToDisplayString()}
{{
    partial {candidate.Declaration.Keyword} {symbol.Name}
    {{
        public override string ToString()
        {{
            ");

                sourceBuilder.Append("return $\"[ ");

                bool any = false;
                foreach (var property in properties)
                {
                    sourceBuilder.Append(property.Name);
                    sourceBuilder.Append(":{");
                    sourceBuilder.Append(property.Name);
                    sourceBuilder.Append("}, ");
                    any = true;
                }

                if (any)
                {
                    sourceBuilder.Remove(sourceBuilder.Length - 2, 2); // Remove last comma and space
                }
                sourceBuilder.Append(" ]\";");
                sourceBuilder.AppendLine(@"
        }
    }
}");

                context.AddSource($"{symbol.Name}_ToString.cs",
                    SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
            }
        }

    }
}