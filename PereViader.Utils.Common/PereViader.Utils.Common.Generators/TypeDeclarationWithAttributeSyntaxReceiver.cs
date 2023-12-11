using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PereViader.Utils.Common.Generators
{
    public class TypeDeclarationWithAttributeSyntaxReceiver<TDeclaration, TGenerator> : ISyntaxReceiver
        where TDeclaration : TypeDeclarationSyntax
    {
        public List<(TDeclaration Declaration, AttributeSyntax AttributeSyntax)> Candidates { get; } = new List<(TDeclaration, AttributeSyntax)>();
        public string AttributeName { get; }

        public TypeDeclarationWithAttributeSyntaxReceiver(string attributeName)
        {
            AttributeName = attributeName;
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is TDeclaration declaration))
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