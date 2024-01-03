using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace PereViader.Utils.Common.Generators
{
    [Generator]
    public class GenerateEventRefsSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new TypeDeclarationWithAttributeSyntaxReceiver<TypeDeclarationSyntax, ToStringSourceGenerator>("GenerateEventRefs"));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not TypeDeclarationWithAttributeSyntaxReceiver<TypeDeclarationSyntax, ToStringSourceGenerator> receiver)
                return;

            foreach (var candidate in receiver.Candidates)
            {
                var model = context.Compilation.GetSemanticModel(candidate.Declaration.SyntaxTree);
                if (model.GetDeclaredSymbol(candidate.Declaration) is not INamedTypeSymbol symbol)
                {
                    continue;
                }

                var events = symbol.GetMembers().OfType<IEventSymbol>();

                var sourceBuilder = new StringBuilder($@"
using PereViader.Utils.Common.Events;

namespace {symbol.ContainingNamespace.ToDisplayString()}
{{
    [System.CodeDom.Compiler.GeneratedCode(""PereViader.Utils.Common.Generators.GenerateEventRefsSourceGenerator"", ""1.0.0.0"")]
    public static class {symbol.Name}EventRefsExtensions
    {{");

                foreach (var @event in events)
                {
                    ITypeSymbol actualType = @event.Type switch
                    {
                        INamedTypeSymbol { NullableAnnotation: NullableAnnotation.Annotated } namedType => namedType.WithNullableAnnotation(NullableAnnotation.NotAnnotated),
                        _ => @event.Type
                    };
                    
                    sourceBuilder.AppendLine($@"
        private static readonly Action<object, {actualType}> {@event.Name}SubscribeAction = (obj, action) => (({symbol.Name})obj).{@event.Name} += action;
        private static readonly Action<object, {actualType}> {@event.Name}UnsubscribeAction = (obj, action) => (({symbol.Name})obj).{@event.Name} -= action;
        
        public static EventRef<{actualType}> Get{@event.Name}EventRef(
                this {symbol.Name} obj)
        {{
            return new EventRef<{actualType}>(
                    obj, 
                    {@event.Name}SubscribeAction,
                    {@event.Name}UnsubscribeAction);
        }}");
                }

                sourceBuilder.AppendLine(@"    }
}");

                context.AddSource($"{symbol.Name}EventRefsExtensions.cs",
                    SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
            }
        }
    }
}