using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PereViader.Utils.Common.Generators
{
    class TaskWaitForEventsSyntaxReceiver : ISyntaxReceiver
    {
        public List<(ClassDeclarationSyntax ClassDeclarationSyntax, AttributeSyntax AttributeSyntax)> Candidates { get; } = new List<(ClassDeclarationSyntax, AttributeSyntax)>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is ClassDeclarationSyntax classDeclaration))
            {
                return;
            }

            foreach (var attributeList in classDeclaration.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (attribute.Name.ToString() == "GenerateEventTaskWaits")
                    {
                        Candidates.Add((classDeclaration, attribute));
                    }
                }
            }
        }
    }
}