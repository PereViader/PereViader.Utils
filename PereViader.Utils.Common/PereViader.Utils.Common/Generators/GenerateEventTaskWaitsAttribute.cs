using System;

namespace PereViader.Utils.Common.Generators
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class GenerateEventTaskWaitsAttribute : Attribute
    {
        private Type TargetType { get; }

        public GenerateEventTaskWaitsAttribute(Type targetType = null)
        {
            TargetType = targetType;
        }
        
    }
}