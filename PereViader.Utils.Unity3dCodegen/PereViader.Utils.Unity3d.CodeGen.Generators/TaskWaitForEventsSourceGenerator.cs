using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace PereViader.Utils.Unity3d.CodeGen.Generators
{
    [Generator]
    public class TaskWaitForEventsSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new TaskWaitForEventsSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is TaskWaitForEventsSyntaxReceiver receiver))
            {
                return;
            }
            
            foreach (var candidate in receiver.Candidates)
            {
                var namespaceName = candidate.ClassDeclarationSyntax.Ancestors().OfType<NamespaceDeclarationSyntax>().FirstOrDefault()?.Name.ToString();
                if (string.IsNullOrEmpty(namespaceName))
                {
                    namespaceName = "PereViader.Utils.Unity3d.CodeGen.Runtime";
                }
                
                var className = candidate.ClassDeclarationSyntax.Identifier.Text;
                var eventFields = candidate.ClassDeclarationSyntax.DescendantNodes().OfType<EventFieldDeclarationSyntax>().ToArray();

                if (eventFields.Length == 0)
                {
                    continue;
                }

                var stringBuilder = new StringBuilder();

                stringBuilder.Append($@"
namespace {namespaceName}
{{
    [System.CodeDom.Compiler.GeneratedCode(""PereViader.Utils.Unity3d.CodeGen.Generators.TaskWaitForEventsSourceGenerator"", ""1.0.0.0"")]
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
                        string setResultArgs;
                        string paramType;

                        if (parameters.Length == 0)
                        {
                            returnType = string.Empty;
                            setResultArgs = string.Empty;
                            paramType = "()";
                        }
                        else if (parameters.Length == 1)
                        {
                            returnType = $"<{parameters[0].Type}>";
                            setResultArgs = parameters[0].Name;
                            paramType = $"({parameters[0].Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {parameters[0].Name})";
                        }
                        else
                        {
                            paramType = $"({string.Join(", ", parameters.Select(x => $"{x.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {x.Name}"))})";
                            setResultArgs = $"({string.Join(", ", parameters.Select(x => x.Name))})";
                            returnType = $"<{paramType}>";
                        }

                        var generatedCode = $@"

        public static Task{returnType} Wait{eventName}(this {className} o, CancellationToken ct = default)
        {{
            if (ct.IsCancellationRequested)
            {{
                return Task.FromCanceled{returnType}(ct);
            }}
            
            TaskCompletionSource{returnType} tcs = new TaskCompletionSource{returnType}();
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