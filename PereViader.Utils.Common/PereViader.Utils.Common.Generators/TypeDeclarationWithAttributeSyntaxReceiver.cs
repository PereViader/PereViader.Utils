using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PereViader.Utils.Common.Generators
{
    public class TypeDeclarationWithAttributeSyntaxReceiver : ISyntaxReceiver
    {
        public List<(TypeDeclarationSyntax TypeDeclarationSyntax, AttributeSyntax AttributeSyntax)> Candidates { get; } = new List<(TypeDeclarationSyntax, AttributeSyntax)>();
        public string AttributeName { get; }

        public TypeDeclarationWithAttributeSyntaxReceiver(string attributeName)
        {
            AttributeName = attributeName;
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is TypeDeclarationSyntax classDeclaration))
            {
                return;
            }

            foreach (var attributeList in classDeclaration.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (attribute.Name.ToString().StartsWith(AttributeName))
                    {
                        Candidates.Add((classDeclaration, attribute));
                    }
                }
            }
        }
    }
}