using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace PereViader.Utils.Common.Generators
{
    [Generator]
    public class TaskWaitForEventsSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new TypeDeclarationWithAttributeSyntaxReceiver("GenerateEventTaskWaits"));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is TypeDeclarationWithAttributeSyntaxReceiver receiver))
            {
                return;
            }
            
            foreach (var candidate in receiver.Candidates)
            {
                var namespaceName = CodeGenerationExtensions.GetNamespaceDeclarationSyntax(candidate.TypeDeclarationSyntax)
                    ?.Name
                    .ToString() ?? "PereViader.Utils.Common.Generators";

                var className = candidate.TypeDeclarationSyntax.Identifier.Text;
                var eventFields = candidate.TypeDeclarationSyntax
                    .DescendantNodes()
                    .OfType<EventFieldDeclarationSyntax>()
                    .ToArray();

                var usingStrings = candidate.TypeDeclarationSyntax.GetUsingDirectives();
                var usingDirectiveSet = new HashSet<string>()
                {
                    "using System.Threading;",
                    "using System.Threading.Tasks;"
                };

                foreach (var usingElement in usingStrings)
                {
                    usingDirectiveSet.Add(usingElement.ToString());
                }

                if (eventFields.Length == 0)
                {
                    continue;
                }

                var stringBuilder = new StringBuilder();

                foreach (var usingElement in usingDirectiveSet)
                {
                    stringBuilder.AppendLine(usingElement);
                }

                stringBuilder.Append($@"
namespace {namespaceName}
{{
    [System.CodeDom.Compiler.GeneratedCode(""PereViader.Utils.Common.Generators.TaskWaitForEventsSourceGenerator"", ""1.0.0.0"")]
    public static class {className}Extensions
    {{");
                    
                foreach (var eventField in eventFields)
                {
                    var semanticModel = context.Compilation.GetSemanticModel(eventField.SyntaxTree);

                    var eventNamedTypeSymbol = CodeGenerationExtensions.GetEventFieldDeclarationSyntaxDelegateNamedTypeSymbol(eventField, semanticModel);
                    var parameters = CodeGenerationExtensions.GetDelegateParameters(eventNamedTypeSymbol);
                    
                    foreach (var eventVariable in eventField.Declaration.Variables)
                    {
                        var eventName = eventVariable.Identifier.Text;
                        
                        string returnType;
                        string internalReturnType;
                        string setResultArgs;
                        string paramType;

                        if (parameters.Length == 0)
                        {
                            internalReturnType = "<object>";
                            returnType = string.Empty;
                            setResultArgs = "null";
                            paramType = "()";
                        }
                        else if (parameters.Length == 1)
                        {
                            returnType = $"<{parameters[0].Type}>";
                            internalReturnType = returnType;
                            setResultArgs = parameters[0].Name;
                            paramType = $"({parameters[0].Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {parameters[0].Name})";
                        }
                        else
                        {
                            paramType = $"({string.Join(", ", parameters.Select(x => $"{x.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {x.Name}"))})";
                            setResultArgs = $"({string.Join(", ", parameters.Select(x => x.Name))})";
                            returnType = $"<{paramType}>";
                            internalReturnType = returnType;
                        }

                        var generatedCode = $@"

        public static Task{returnType} Wait{eventName}(this {className} o, CancellationToken ct = default)
        {{
            if (ct.IsCancellationRequested)
            {{
                return Task.FromCanceled{returnType}(ct);
            }}
            
            TaskCompletionSource{internalReturnType} tcs = new TaskCompletionSource{internalReturnType}();
            if(ct.CanBeCanceled)
            {{
                ct.Register(() => tcs.TrySetCanceled());
            }}

            void SetResultOn{eventName}{paramType}
            {{
                o.{eventName} -= SetResultOn{eventName};
                tcs.SetResult({setResultArgs});
            }}

            o.{eventName} += SetResultOn{eventName};
            return tcs.Task;
        }}";
                        
                        stringBuilder.Append(generatedCode);
                    }
                }

                stringBuilder.Append(@"
    }
}");
                context.AddSource($"{className}.GenerateTaskWaitForEventsAttribute.GeneratedExtensions.cs", SourceText.From(stringBuilder.ToString(), Encoding.UTF8));
            }
        }
    }
}