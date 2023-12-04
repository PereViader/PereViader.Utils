#nullable enable
using System;

namespace PereViader.Utils.Common.Generators
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public sealed class GenerateEventTaskWaitsAttribute : Attribute
    {
        public Type? TargetType { get; }

        public GenerateEventTaskWaitsAttribute(Type? targetType = null)
        {
            TargetType = targetType;
        }
    }
}