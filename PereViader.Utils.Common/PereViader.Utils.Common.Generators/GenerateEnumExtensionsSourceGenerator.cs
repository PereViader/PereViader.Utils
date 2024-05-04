using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace PereViader.Utils.Common.Generators
{
    [Generator]
    public class GenerateEnumExtensionsSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new TypeDeclarationWithAttributeSyntaxReceiver<EnumDeclarationSyntax, GenerateEnumExtensionsSourceGenerator>("GenerateEnumExtensions"));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not TypeDeclarationWithAttributeSyntaxReceiver<EnumDeclarationSyntax, GenerateEnumExtensionsSourceGenerator> receiver)
            {
                return;
            }
            
            foreach (var candidate in receiver.Candidates)
            {
                var model = context.Compilation.GetSemanticModel(candidate.Declaration.SyntaxTree);
                if (model.GetDeclaredSymbol(candidate.Declaration) is not INamedTypeSymbol enumSymbol)
                {
                    continue;
                }

                var enumName = enumSymbol.Name;
                var namespaceName = enumSymbol.ContainingNamespace.ToDisplayString();
                
                var source = GenerateExtensionClassSource(enumSymbol, enumName, namespaceName);
                context.AddSource($"{enumName}Extensions.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        private string GenerateExtensionClassSource(INamedTypeSymbol enumSymbol, string enumName, string namespaceName)
        {
            var builder = new StringBuilder();
            builder.AppendLine("using System;");
            builder.AppendLine($"namespace {namespaceName}");
            builder.AppendLine("{");
            builder.AppendLine("    [System.CodeDom.Compiler.GeneratedCode(\"PereViader.Utils.Common.Generators.GenerateEnumExtensionsSourceGenerator\", \"1.0.0.0\")]");
            builder.AppendLine($"    public static partial class {enumName}Extensions");
            builder.AppendLine("    {");
            builder.AppendLine($"        public static readonly {enumName}[] Values = new[] ");
            builder.AppendLine("        {");

            var enumMembers = enumSymbol
                .GetMembers()
                .OfType<IFieldSymbol>()
                .Where(f => f.HasConstantValue)
                .ToArray();
            
            foreach (var member in enumMembers)
            {
                builder.AppendLine($"            {enumName}.{member.Name},");
            }

            builder.AppendLine("        };");
            builder.AppendLine();
            builder.AppendLine($"        public static string ToStringOptimized(this {enumName} value)");
            builder.AppendLine("        {");
            builder.AppendLine("            return value switch");
            builder.AppendLine("            {");

            foreach (var member in enumMembers)
            {
                builder.AppendLine($"                {enumName}.{member.Name} => \"{member.Name}\",");
            }


            builder.AppendLine("                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)");
            builder.AppendLine("            };");
            builder.AppendLine("        }");
            builder.AppendLine();
            
            builder.AppendLine($"        public static {enumName}? ParseOptimized(string value)");
            builder.AppendLine("        {");
            builder.AppendLine("            return value switch");
            builder.AppendLine("            {");

            foreach (var member in enumMembers)
            {
                builder.AppendLine($"                \"{member.Name}\" => {enumName}.{member.Name},");
            }


            builder.AppendLine("                _ => null");
            builder.AppendLine("            };");
            builder.AppendLine("        }");
            
            builder.AppendLine("    }");
            builder.AppendLine("}");

            return builder.ToString();
        }
    }
}