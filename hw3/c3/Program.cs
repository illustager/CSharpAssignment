namespace c3 {
    class Graphics2DFactory {
        public enum ShapeType {
            Circle,
            Rectangle,
            Triangle
        }

        public static c2.IGraphics2D CreateGraphics2D(ShapeType shapeType, params double[] parameters) {
            return shapeType switch {
                ShapeType.Circle => CreateCircle(parameters[0]),
                ShapeType.Rectangle => CreateRectangle(parameters[0], parameters[1]),
                ShapeType.Triangle => CreateTriangle(parameters[0], parameters[1], parameters[2]),
                _ => throw new ArgumentException("Invalid shape type")
            };
        }

        public static c2.IGraphics2D CreateCircle(double radius) {
            return new c2.Circle(radius);
        }

        public static c2.IGraphics2D CreateRectangle(double width, double height) {
            return new c2.Rectangle(width, height);
        }

        public static c2.IGraphics2D CreateTriangle(double edge1, double edge2, double edge3) {
            return new c2.Triangle(edge1, edge2, edge3);
        }
    }

    internal class Program {
        static void Main(string[] args) {
            var shapes = new c2.IGraphics2D[] {
                Graphics2DFactory.CreateGraphics2D(Graphics2DFactory.ShapeType.Circle, -1),
                Graphics2DFactory.CreateGraphics2D(Graphics2DFactory.ShapeType.Circle, 1),
                Graphics2DFactory.CreateGraphics2D(Graphics2DFactory.ShapeType.Rectangle, 2, 3),
                Graphics2DFactory.CreateGraphics2D(Graphics2DFactory.ShapeType.Triangle, 1, 1, -1),
                Graphics2DFactory.CreateGraphics2D(Graphics2DFactory.ShapeType.Triangle, 3, 4, 5)
            };

            foreach (var shape in shapes) {
                Console.WriteLine(shape.valid ? shape.area.ToString() : "Invalid shape!");
            }
        }
    }
}
