using Microsoft.Xna.Framework;

namespace Engine_lib
{
    public class CalendarSystem
	{
        public static CalendarSystem Current { get; private set; }
        private static bool Pause = false;

        public static int TotalDays = 0;
		public static int Day = 1;
		public static int Month = 1;
		public static int Year = 1;

		int Days_In_Month = 30;
		int Months_In_Year = 12;

		int Hour_In_Day = 24;
		int Minutes_In_Hour = 60;
		int Seconds_In_Minute = 60;

		public static int Current_Hour = 0;
		public static int Current_Minute = 0;
		public static float Current_Second = 0;

		public static GameTime gtime { get; private set; }

		public static float Multiplyer = 1;
		public static float Divider = 10;

		public CalendarSystem()
        { 
            Current = this;
		}

		public void Update(GameTime gt)
		{
            if (!Pause)
            {
                Current_Second += (float)gt.ElapsedGameTime.TotalSeconds * Multiplyer;
                handle_clock();
            }
		}

        public static void PauseCalendar()
        {
            Pause = !Pause;
        }

		private void handle_clock()
		{
            if (Current_Second >= Seconds_In_Minute)
            {
                Current_Minute++;
                Current_Second = 0;

                if (Current_Minute >= Minutes_In_Hour)
                {
                    Current_Hour++;
                    Current_Minute = 0;
                    if (Current_Hour >= Hour_In_Day)
                    {
                        Current_Hour = 0;
                        Day++;
                        TotalDays++;
                        if (Day >= Days_In_Month)
                        {
                            Month++;
                            Day = 1;
                            if (Month >= Months_In_Year)
                            {
                                Year++;
                                Month = 1;
                                TotalDays = 0;
                            }
                        }
                    }
                }
            }
        }
	}
}
