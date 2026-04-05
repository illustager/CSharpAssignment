namespace c5 {
    class AlarmClock {
        public event Action? OnAlarm;
        public event Action? OnTick;

        public void tick() {
            OnTick?.Invoke();
        }

        public void alarm() {
            OnAlarm?.Invoke();
        }
    }
    internal class Program {
        static void Main(string[] args) {
            var clock = new AlarmClock();

            clock.OnAlarm += () => Console.WriteLine("Alarm!!!");
            clock.OnTick += () => Console.WriteLine("Tick...");

            for (int i = 0; i < 10; i++) {
                clock.tick();
            }
            clock.alarm();
        }
    }
}
