using System.Reflection.Metadata;

namespace hw1 {
    internal class Program {

        static bool IsPrime(uint n) {
            if (n < 2) return false;
            for (uint i = 2; i <= Math.Sqrt(n); i++) {
                if (n % i == 0) return false;
            }
            return true;
        }
        static void Main(string[] args) {
            Console.WriteLine("你好！我是杨东霖");
            Console.Write("请输入上下限，以空格分隔：");

            string[] inputs = Console.ReadLine().Split(' ');
            uint lb = uint.Parse(inputs[0]);
            uint ub = uint.Parse(inputs[1]);

            int cnt = 0;
            for (uint i = lb; i <= ub; i++) {
                if (IsPrime(i)) {
                    Console.Write(i + "\t");
                    cnt++;

                    if (cnt >= 10) {
                        cnt = 0;
                        Console.WriteLine("");
                    }
                }
            }
        }
    }
}
