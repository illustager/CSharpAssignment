namespace c4 {
    class Node<T> {
        public Node<T>? Next { get; set; }
        public T Value { get; set; }

        public Node(T value) {
            Value = value;
            Next = null;
        }

        public Node(Node<T>? next) {
            Next = next;
        }
    }

    class GenericList<T> { 
        private Node<T>? head;
        private Node<T>? tail;

        public Node<T>? Head => head;
        public int Size { get; private set; }


        public GenericList() {
            head = null;
            tail = null;
            Size = 0;
        }

        public void Add(T value) {
            var newNode = new Node<T>(value);
            if (head == null) {
                head = newNode;
                tail = newNode;
            }
            else {
                tail!.Next = newNode;
                tail = newNode;
            }
            ++Size;
        }

        public void ForEach(Action<T> action) {
            var current = head;
            while (current != null) {
                action(current.Value);
                current = current.Next;
            }
        }
    }


    internal class Program {
        static void Main() {
            var list = new GenericList<int>();

            var inputs = Console.ReadLine()!.Trim().Split().Select(int.Parse);
            foreach (var input in inputs) {
                list.Add(input);
            }

            int minValue = int.MaxValue, maxValue = int.MinValue, sum = 0;
            list.ForEach(x => {
                sum += x;
                if (x < minValue) minValue = x;
                if (x > maxValue) maxValue = x;
            });

            Console.WriteLine($"Min Value: {minValue}");
            Console.WriteLine($"Max Value: {maxValue}");
            Console.WriteLine($"Average  : {(double)sum / list.Size}");
            Console.WriteLine($"Sum      : {sum}");
        }
    }
}
