using System;
using Banks.Interfaces;

namespace Banks.Models.Time
{
    public class FutureTime : ITime
    {
        public FutureTime() =>
            Time = DateTime.Today;

        public DateTime Time { get; private set; }

        public void NextDay() =>
            Time = Time.AddDays(1);

        public void NextMonth() =>
            Time = Time.AddMonths(1);
    }
}