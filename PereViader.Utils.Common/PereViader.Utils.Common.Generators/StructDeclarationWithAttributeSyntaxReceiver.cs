using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PereViader.Utils.Common.Generators
{
    public class StructDeclarationWithAttributeSyntaxReceiver : ISyntaxReceiver
    {
        public List<(StructDeclarationSyntax StructDeclarationSyntax, AttributeSyntax AttributeSyntax)> Candidates { get; } = new List<(StructDeclarationSyntax, AttributeSyntax)>();
        public string AttributeName { get; }

        public StructDeclarationWithAttributeSyntaxReceiver(string attributeName)
        {
            AttributeName = attributeName;
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is StructDeclarationSyntax declaration))
            {
                return;
            }

            foreach (var attributeList in declaration.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (attribute.Name.ToString().StartsWith(AttributeName))
                    {
                        Candidates.Add((declaration, attribute));
                    }
                }
            }
        }
    }
}