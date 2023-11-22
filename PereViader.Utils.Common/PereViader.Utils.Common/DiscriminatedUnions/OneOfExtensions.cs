using System;

namespace PereViader.Utils.Common.DiscriminatedUnions
{
    public static class OneOfExtensions
    {
        public static TResult Match<TFirst, TSecond, TResult>(
            this OneOf<TFirst, TSecond> oneOf, 
            Func<TFirst, TResult> firstFunc, 
            Func<TSecond, TResult> secondFunc)
        {
            var (index, first, second) = oneOf;
            
            switch (index)
            {
                case 0: return firstFunc(first);
                case 1: return secondFunc(second);
                default: throw new InvalidOperationException();
            }
        }
        
        public static TResult Match<TFirst, TSecond, TThird, TResult>(
            this OneOf<TFirst, TSecond, TThird> oneOf, 
            Func<TFirst, TResult> firstFunc, 
            Func<TSecond, TResult> secondFunc, 
            Func<TThird, TResult> thirdFunc)
        {
            var (index, first, second, third) = oneOf;
            
            switch (index)
            {
                case 0: return firstFunc(first);
                case 1: return secondFunc(second);
                case 2: return thirdFunc(third);
                default: throw new InvalidOperationException();
            }
        }
    }
}