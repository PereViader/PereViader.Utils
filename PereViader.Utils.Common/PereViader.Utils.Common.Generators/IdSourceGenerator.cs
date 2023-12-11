using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace PereViader.Utils.Common.Generators
{
    [Generator]
    public class IdSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new StructDeclarationWithAttributeSyntaxReceiver("GenerateId"));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is StructDeclarationWithAttributeSyntaxReceiver receiver))
            {
                return;
            }
            
            foreach (var candidate in receiver.Candidates)
            {
                var className = candidate.StructDeclarationSyntax.Identifier.Text;
                var namespaceName = CodeGenerationExtensions.GetNamespaceDeclarationSyntax(candidate.StructDeclarationSyntax)
                    ?.Name
                    .ToString() ?? "PereViader.Utils.Common.Generators";

                GenerateId(context, namespaceName, className);
            }
        }

        private static void GenerateId(GeneratorExecutionContext context, string namespaceName, string className)
        {
            var stringBuilder = new StringBuilder();

            var nullableIndicator = context.Compilation.Options.NullableContextOptions == NullableContextOptions.Enable ? "?" : string.Empty;

            stringBuilder.Append($@"
using System;

namespace {namespaceName}
{{
    [System.CodeDom.Compiler.GeneratedCode(""PereViader.Utils.Common.Generators.IdSourceGenerator"", ""1.0.0.0"")]
    public readonly partial struct {className} : IEquatable<{className}>, IComparable, IComparable<{className}>
    {{
        public static readonly {className} Empty;

        private readonly Guid _guid;

        private {className}(Guid guid)
        {{
            _guid = guid;
        }}

        public static {className} New()
        {{
            return new {className}(Guid.NewGuid());
        }}

        public static {className} FromGuid(Guid guid)
        {{
            return new {className}(guid);
        }}

        public static {className} FromGuid(string guid)
        {{
            return new {className}(new Guid(guid));
        }}

        public Guid ToGuid()
        {{ 
            return _guid;
        }}

        public bool Equals({className} other)
        {{
            return _guid.Equals(other._guid);
        }}

        public override bool Equals(object{nullableIndicator} obj)
        {{
            if (!(obj is {className} other)) 
            {{
                return false;
            }}

            return Equals(other);
        }}

        public int CompareTo({className} value) 
        {{ 
            return _guid.CompareTo(value._guid);
        }}

        public int CompareTo(object{nullableIndicator} value)
        {{
            if(!(value is {className} other))
            {{
                throw new ArgumentException($""Object of type {{value?.GetType().FullName ?? ""null""}} is not a {{typeof({className}).FullName}}"");
            }}
            return CompareTo(other);
        }}

        public override int GetHashCode()
        {{
            return _guid.GetHashCode();
        }}

        public static bool operator ==({className} a, {className} b) 
        {{
            return a.Equals(b);
        }}

        public static bool operator !=({className} a, {className} b) 
        {{ 
            return !a.Equals(b);
        }}
    }}
}}");
            context.AddSource($"{className}.IdSourceGenerator.cs", SourceText.From(stringBuilder.ToString(), Encoding.UTF8));
        }
    }
}