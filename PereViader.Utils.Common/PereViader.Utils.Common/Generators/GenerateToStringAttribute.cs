using System;

namespace PereViader.Utils.Common.Generators
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public sealed class GenerateToStringAttribute : Attribute
    {
    }
}