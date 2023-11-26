using System;

namespace PereViader.Utils.Common.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class ExperimentalAttribute : Attribute
    {
    }
}