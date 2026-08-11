using System.Drawing;
using System.Runtime.CompilerServices;



public class Camera
{
    private readonly Direction refForward = new Direction(0, 0, 1);
    private readonly Direction refUp = new Direction(0, 1, 0);
    private readonly Direction refRight = new Direction (1, 0, 0);   
    private readonly Coordinate position;
    private readonly Direction forward;
    private readonly Direction up;
    private readonly Direction right;
    private readonly Rotation rotation;
    private readonly double fovAngle = 90.0;
    private readonly double fovDistance = 1.0; // I don't want to think about this yet

    private readonly int xRes;
    private readonly int yRes;

    private Color[,] colorVals;
    private Ray[] rays;
    
    public Camera(Coordinate _position = default, Rotation _rotation = default, double fov = 80.0, int _xRes = 500, int _yRes = 500)
    {
        position = _position;
        rotation = _rotation;
        forward = refForward.Rotate(_rotation);
        right = refRight.Rotate(_rotation);
        up = refUp.Rotate(_rotation);
        xRes = _xRes;
        yRes = _yRes;
        colorVals = new Color[xRes, yRes];
        rays = new Ray[xRes*yRes];
        
        CreateRays();
    }

    private void CreateRays()
    {
        double width = 2 * fovDistance * Math.Tan(fovAngle * Math.PI / 180.0 / 2.0);
        double height = width * ((double)yRes/xRes);

        double xUnit = width / xRes;
        double yUnit = height / yRes;

        double xOffset, yOffset;

        Direction cornerX = -right * (width/2.0);
        Direction cornerY = up * (height/2.0);

        for(int currX = 0; currX < xRes; currX++) 
        {
            xOffset = currX * xUnit;
            for(int currY = 0; currY < yRes; currY++)
            {
                yOffset = currY * yUnit;

                Coordinate planePoint = position + (forward*fovDistance).ToCoordinate() + (cornerX + right * xOffset + cornerY + -up * yOffset).ToCoordinate();
                Direction rayDirection = (planePoint - position).Normalized();

                rays[currX * yRes + currY] = new Ray(position, rayDirection);
            }
        }
    }

    public void SaveImage(string name = "output")
    {
        using var bitmap = new Bitmap(xRes, yRes);

        Color currColor;

        for(int currX = 0; currX < xRes; currX++)
        {
            for(int currY = 0; currY < yRes; currY++)
            {
                currColor = colorVals[currX, currY];
                bitmap.SetPixel(currX, currY, currColor.convert());
            }
        }

        bitmap.Save($"{name}.png");
    }
}