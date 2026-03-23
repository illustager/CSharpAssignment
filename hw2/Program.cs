namespace hw2 {
    internal class Program {
        static object GetStaticFieldValue(Type type, string fieldName) {
            var field = type.GetField(fieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            return field?.GetValue(null);
        }
        static void Main() {
            var types = new Type[] {
                typeof(sbyte),
                typeof(byte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(decimal)
            };

            var sheetFormat = "|{0,8}|{1,5}|{2,31}|{3,30}|";
            var sheetLine = new string[] {
                new string('-', 8),
                new string('-', 5),
                new string('-', 31),
                new string('-', 30)
            };

            Console.WriteLine(sheetFormat, "Type", "Size", "Min Value", "Max Value");
            Console.WriteLine(sheetFormat, sheetLine);
            foreach (var t in types) {
                var name = t.Name;
                var size = System.Runtime.InteropServices.Marshal.SizeOf(t);
                var minValue = GetStaticFieldValue(t, "MinValue");
                var maxValue = GetStaticFieldValue(t, "MaxValue");

                Console.WriteLine(sheetFormat, name, size, minValue, maxValue);
            }
        }
    }
}
