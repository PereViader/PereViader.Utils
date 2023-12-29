using System;

namespace PereViader.Utils.Common.DiscriminatedUnions
{
    public readonly struct OneOf<TFirst, TSecond>
    {
        private readonly int _index;
        private readonly TFirst _first;
        private readonly TSecond _second;

        public bool HasFirst => _index == 0;
        public bool HasSecond => _index == 1;
        public int Index => _index;

        private OneOf(int index, TFirst first = default!, TSecond second = default!)
        {
            _index = index;
            _first = first;
            _second = second;
        }

        public bool TryGetFirst(out TFirst value)
        {
            value = _first;
            return HasFirst;
        }

        public bool TryGetSecond(out TSecond value)
        {
            value = _second;
            return HasSecond;
        }

        public void Deconstruct(out int index, out TFirst first, out TSecond second)
        {
            index = _index;
            first = _first;
            second = _second;
        }

        public TFirst UnsafeGetFirst()
        {
            if (HasFirst)
            {
                throw new InvalidOperationException($"{nameof(OneOf<TFirst, TSecond>)} does not have First");
            }
            
            return _first;
        }

        public TSecond UnsafeGetSecond()
        {
            if (HasSecond)
            {
                throw new InvalidOperationException($"{nameof(OneOf<TFirst, TSecond>)} does not have Second");
            }            
            
            return _second;
        }
        
        public static OneOf<TFirst, TSecond> First(TFirst first)
        {
            return new OneOf<TFirst, TSecond>(0, first: first);
        }
        
        public static OneOf<TFirst, TSecond> Second(TSecond second)
        {
            return new OneOf<TFirst, TSecond>(1, second: second);
        }
        
        public static implicit operator OneOf<TFirst, TSecond>(TFirst first)
        {
            return First(first);
        }

        public static implicit operator OneOf<TFirst, TSecond>(TSecond second)
        {
            return Second(second);
        }
    }

    public struct OneOf<TFirst, TSecond, TThird>
    {
        private readonly TFirst _first;
        private readonly TSecond _second;
        private readonly TThird _third;
        private readonly int _index;
        
        public bool HasFirst => _index == 0;
        public bool HasSecond => _index == 1;
        public bool HasThird => _index == 2;
        public int Index => _index;

        private OneOf(int index, TFirst first = default!, TSecond second = default!, TThird third = default!)
        {
            _index = index;
            _first = first;
            _second = second;
            _third = third;
        }
        
        public bool TryGetFirst(out TFirst value)
        {
            value = _first;
            return HasFirst;
        }

        public bool TryGetSecond(out TSecond value)
        {
            value = _second;
            return HasSecond;
        }

        public bool TryGetThird(out TThird value)
        {
            value = _third;
            return HasThird;
        }
        
        public void Deconstruct(out int index, out TFirst first, out TSecond second, out TThird third)
        {
            index = _index;
            first = _first;
            second = _second;
            third = _third;
        }

        public TFirst UnsafeGetFirst()
        {
            if (HasFirst)
            {
                throw new InvalidOperationException($"{nameof(OneOf<TFirst, TSecond>)} does not have First");
            }
            
            return _first;
        }

        public TSecond UnsafeGetSecond()
        {
            if (HasSecond)
            {
                throw new InvalidOperationException($"{nameof(OneOf<TFirst, TSecond>)} does not have Second");
            }
            
            return _second;
        }

        public TThird UnsafeGetThird()
        {
            if (HasThird)
            {
                throw new InvalidOperationException($"{nameof(OneOf<TFirst, TSecond>)} does not have Third");
            }
            
            return _third;
        }
        
        public static OneOf<TFirst, TSecond, TThird> First(TFirst first)
        {
            return new OneOf<TFirst, TSecond, TThird>(0, first: first);
        }
        
        public static OneOf<TFirst, TSecond, TThird> Second(TSecond second)
        {
            return new OneOf<TFirst, TSecond, TThird>(1, second: second);
        }
        
        public static OneOf<TFirst, TSecond, TThird> Third(TThird third)
        {
            return new OneOf<TFirst, TSecond, TThird>(2, third: third);
        }
        
        public static implicit operator OneOf<TFirst, TSecond, TThird>(TFirst first)
        {
            return First(first);
        }

        public static implicit operator OneOf<TFirst, TSecond, TThird>(TSecond second)
        {
            return Second(second);
        }
        
        public static implicit operator OneOf<TFirst, TSecond, TThird>(TThird third)
        {
            return Third(third);
        }

        public TResult Match<TResult>(
            Func<TFirst, TResult> firstFunc, 
            Func<TSecond, TResult> secondFunc, 
            Func<TThird, TResult> thirdFunc
            )
        {
            switch (_index)
            {
                case 0: return firstFunc(_first);
                case 1: return secondFunc(_second);
                case 2: return thirdFunc(_third);
                default: throw new InvalidOperationException("Invalid union type");
            }
        }
    }

}