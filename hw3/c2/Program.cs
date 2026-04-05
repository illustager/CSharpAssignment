namespace c2 {
    public interface IGraphics2D {
        double? area {  get; }
        bool valid { get; }
    }

    public class Circle : IGraphics2D {
        public double radius { get; set; }
        public double? area => valid ? Math.PI * radius * radius : null;
        public bool valid => radius > 0;

        public Circle(double radius) {
            this.radius = radius;
        }
    }

    public class Rectangle : IGraphics2D {
        public double width { get; set; }
        public double height { get; set; }
        public double? area => valid ? width * height : null;
        public bool valid => width > 0 && height > 0;
        public Rectangle(double width, double height) {
            this.width = width;
            this.height = height;
        }
    }

    public class Triangle : IGraphics2D {
        private double[] _edges = new double[3];
        public double[] edges => _edges;
        public double? area {
            get {
                if (!valid) return null;

                double p = (edges[0] + edges[1] + edges[2]) / 2;
                return Math.Sqrt(p * (p - edges[0]) * (p - edges[1]) * (p - edges[2]));
            }
        }

        public bool valid {
            get {
                _edges.Sort();
                return _edges[0] > 0 && _edges[0] + _edges[1] > _edges[2];
            }
        }

        public Triangle(double edge1, double edge2, double edge3) {
            _edges[0] = edge1;
            _edges[1] = edge2;
            _edges[2] = edge3;
        }
    }

        internal class Program {
        static void Main() {
            IGraphics2D[] shapes = new IGraphics2D[] {
                new Circle(-1),
                new Circle(1),
                new Rectangle(2, 3),
                new Triangle(1, 1, -1),
                new Triangle(1, 1, 5),
                new Triangle(3, 4, 5)
            };

            foreach (var shape in shapes) {
                Console.WriteLine(shape.valid ? shape.area.ToString() : "Invalid shape!");
            }
        }
    }
}
