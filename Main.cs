using System.IO;

public static class RayTracer
{
    public static void Main()
    {
        Console.WriteLine("Starting Raytracer :)");

        string path = Path.GetFullPath("scene_export.json");

        Scene scene = new(path);
        scene.Render();
    }
}