using System.Drawing;

public class Camera
{   
    private readonly Coordinate position;
    private readonly Direction direction;
    private readonly int xRes;
    private readonly int yRes;

    private Color[,] colorVals;
    
    public Camera(Coordinate _position, Direction _direction, int _xRes, int _yRes)
    {
        position = _position;
        direction = _direction;
        xRes = _xRes;
        yRes = _yRes;
        colorVals = new Color[xRes, yRes];

    }

    public void saveImage(string name = "output")
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