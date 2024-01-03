using System;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.TimeSlicing
{
    public sealed class TimeSlicer
    {
        private readonly Func<DateTime> _currentDateTimeFunc;
        
        public TimeSpan TimeSpanBetweenSlices { get; set; }
        public DateTime DateTimeOnPreviousSlice { get; set; }
        

        public TimeSlicer(TimeSpan timeSpanBetweenSlices, Func<DateTime> currentDateTimeFunc)
        {
            TimeSpanBetweenSlices = timeSpanBetweenSlices;
            _currentDateTimeFunc = currentDateTimeFunc;
            DateTimeOnPreviousSlice = currentDateTimeFunc();
        }

        public bool WillSlice()
        {
            DateTime now = _currentDateTimeFunc();
            return now - DateTimeOnPreviousSlice >= TimeSpanBetweenSlices;
        }

        public bool Slice()
        {
            DateTime now = _currentDateTimeFunc();
            if (now - DateTimeOnPreviousSlice <= TimeSpanBetweenSlices)
            {
                return false;
            }

            DateTimeOnPreviousSlice = _currentDateTimeFunc();
            return true;
        }

        public async ValueTask SliceAsync()
        {
            if (Slice())
            {
                await Task.Yield();
            }
        }

        public void Reset()
        {
            DateTimeOnPreviousSlice = _currentDateTimeFunc();
        }
    }
}