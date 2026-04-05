namespace c1 {
    internal class Program {
        static void Main() {
            var arr = Console.ReadLine()!.Split().Select(int.Parse).ToArray();

            int sum = 0, minValue = int.MaxValue, maxValue = int.MinValue;
            foreach (int item in arr) {
                sum += item;

                if (item < minValue) {
                    minValue = item;
                }
                if (item > maxValue) {
                    maxValue = item;
                }
            }

            double average = (double)sum / arr.Length;

            Console.WriteLine($"Min Value: {minValue}");
            Console.WriteLine($"Max Value: {maxValue}");
            Console.WriteLine($"Average  : {average}");
            Console.WriteLine($"Sum      : {sum}");
        }
    }
}
