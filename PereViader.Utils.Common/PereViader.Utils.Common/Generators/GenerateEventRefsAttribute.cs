using System;

namespace PereViader.Utils.Common.Generators
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public sealed class GenerateEventRefsAttribute : Attribute
    {
    }
}