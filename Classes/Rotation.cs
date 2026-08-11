public readonly record struct Rotation
{
    public readonly double X;
    public readonly double Y;
    public readonly double Z;

    private readonly double sinX, cosX;
    private readonly double sinY, cosY;
    private readonly double sinZ, cosZ;

        public Rotation(double x = 0.0, double y = 0.0, double z = 0.0)
    {
        X = x;
        Y = y;
        Z = z;
 
        double radX = ToRadians(x);
        double radY = ToRadians(y);
        double radZ = ToRadians(z);
 
        sinX = Math.Sin(radX);
        cosX = Math.Cos(radX);
        sinY = Math.Sin(radY);
        cosY = Math.Cos(radY);
        sinZ = Math.Sin(radZ);
        cosZ = Math.Cos(radZ);
    }


    ///
    /// Coordinate Functions
    /// 

    /// <summary>
    /// Rotates a point around the origin towards this rotation's direction
    /// </summary>
    /// <param name="point">The point to rotate</param>
    /// <returns>The coordinate with the rotation applied</returns>
    public Coordinate Apply(Coordinate point)
    {
        var applied = rotateX(point.X, point.Y, point.Z, cosX, sinX);
        applied = rotateY(applied.x, applied.y, applied.z, cosY, sinY);
        applied = rotateZ(applied.x, applied.y, applied.z, cosZ, sinZ);

        return new Coordinate(applied.x, applied.y, applied.z);
    }

    /// <summary>
    /// Rotates a point around the origin away from this rotation's direction
    /// </summary>
    /// <param name="point">The point to rotate</param>
    /// <returns>The coordinate with the inverse rotation applied</returns>
    public Coordinate ApplyInverse(Coordinate point)
    {
        var applied = rotateX(point.X, point.Y, point.Z, cosX, -sinX);
        applied = rotateY(applied.x, applied.y, applied.z, cosY, -sinY);
        applied = rotateX(applied.x, applied.y, applied.z, cosZ, -sinZ);

        return new Coordinate(applied.x, applied.y, applied.z);
    }

    
    ///
    /// Direction Functions
    /// 

    /// <summary>
    /// Rotates a point around the origin towards this rotation's direction
    /// </summary>
    /// <param name="direction">The point to rotate</param>
    /// <returns>The coordinate with the rotation applied</returns>
    public Direction Apply(Direction direction)
    {
        var applied = rotateX(direction.X, direction.Y, direction.Z, cosX, sinX);
        applied = rotateY(applied.x, applied.y, applied.z, cosY, sinY);
        applied = rotateZ(applied.x, applied.y, applied.z, cosZ, sinZ);

        return new Direction(applied.x, applied.y, applied.z);
    }

    /// <summary>
    /// Rotates a point around the origin away from this rotation's direction
    /// </summary>
    /// <param name="direction">The point to rotate</param>
    /// <returns>The coordinate with the inverse rotation applied</returns>
    public Direction ApplyInverse(Direction direction)
    {
        var applied = rotateZ(direction.X, direction.Y, direction.Z, cosX, -sinX);
        applied = rotateY(applied.x, applied.y, applied.z, cosY, -sinY);
        applied = rotateX(applied.x, applied.y, applied.z, cosZ, -sinZ);

        return new Direction(applied.x, applied.y, applied.z);
    }

    ///
    /// Utility Functions 
    /// 

    /// <summary>
    /// A utility function to convert degrees to radians
    /// </summary>
    /// <param name="degrees">The angle in degrees</param>
    /// <returns>The angle in radians</returns>
    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    private static (double x, double y, double z) rotateX(double x, double y, double z, double sin, double cos)
        => (x, y * cos - z * sin, y * sin + z * cos);
    private static (double x, double y, double z) rotateY(double x, double y, double z, double sin, double cos)
        => (x * cos + z * sin, y, -x * sin + z * cos);
    private static (double x, double y, double z) rotateZ(double x, double y, double z, double sin, double cos)
        => (x * cos - y * sin, x * sin + y * cos, z);
    
}