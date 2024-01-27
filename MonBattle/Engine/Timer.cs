namespace MonBattle.Engine
{
    public class Timer
    {
        private int elapsedTime = 0;

        public Timer()
        {
        }

        public void updateTime(int time)
        {
            elapsedTime += time;
        }

        public int getTime()
        {
            return elapsedTime;
        }

        public bool isTimeMet(int maxTime)
        {
            return elapsedTime >= maxTime;
        }

        public void restartTimer()
        {
            elapsedTime = 0;
        }
    }
}
