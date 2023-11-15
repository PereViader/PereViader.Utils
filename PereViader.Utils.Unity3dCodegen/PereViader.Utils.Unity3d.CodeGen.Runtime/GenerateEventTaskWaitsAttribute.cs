using System;

namespace PereViader.Utils.Unity3d.CodeGen.Runtime
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