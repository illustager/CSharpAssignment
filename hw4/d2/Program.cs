namespace d2;

internal class Program
{
    static void Main()
    {
        var numbers = new List<int>();
        for (int i = 0; i < 100; i++)
            numbers.Add(Random.Shared.Next(0, 1001));

        var sorted = numbers.OrderByDescending(n => n).ToList();
        var sum = sorted.Sum();
        var average = sorted.Average();

        Console.WriteLine("Sorted (descending):");
        Console.WriteLine(string.Join(", ", sorted));
        Console.WriteLine();
        Console.WriteLine($"Sum: {sum}");
        Console.WriteLine($"Average: {average:F2}");
    }
}
