public static class RayTracer
{
    public static void Main()
    {
       Console.WriteLine("Hello Everyone!");
       Camera camera = new Camera(new Coordinate(0, 0, 0), new Direction(0, 0, 0), 500, 500);
       camera.saveImage();
    }
}