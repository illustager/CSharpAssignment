namespace d3;

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public override string ToString() => $"Person[Name={Name}, Age={Age}]";
}

internal class Program
{
    static void Main()
    {
        var constructor = typeof(Person).GetConstructor(new[] { typeof(string), typeof(int) });
        var person = constructor!.Invoke(new object[] { "Alice", 25 }) as Person;
        Console.WriteLine(person!.ToString());
    }
}
