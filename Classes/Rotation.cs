public readonly record struct Rotation(double X = 0.0, double Y = 0.0, double Z = 0.0)
{

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
        var applied = rotateX(point.X, point.Y, point.Z, X);
        applied = rotateY(applied.x, applied.y, applied.z, Y);
        applied = rotateZ(applied.x, applied.y, applied.z, Z);

        return new Coordinate(applied.x, applied.y, applied.z);
    }

    /// <summary>
    /// Rotates a point around the origin away from this rotation's direction
    /// </summary>
    /// <param name="point">The point to rotate</param>
    /// <returns>The coordinate with the inverse rotation applied</returns>
    public Coordinate ApplyInverse(Coordinate point)
    {
        var applied = rotateX(point.X, point.Y, point.Z, -X);
        applied = rotateY(applied.x, applied.y, applied.z, -Y);
        applied = rotateZ(applied.x, applied.y, applied.z, -Z);

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
        var applied = rotateX(direction.X, direction.Y, direction.Z, X);
        applied = rotateY(applied.x, applied.y, applied.z, Y);
        applied = rotateZ(applied.x, applied.y, applied.z, Z);

        return new Direction(applied.x, applied.y, applied.z);
    }

    /// <summary>
    /// Rotates a point around the origin away from this rotation's direction
    /// </summary>
    /// <param name="direction">The point to rotate</param>
    /// <returns>The coordinate with the inverse rotation applied</returns>
    public Direction ApplyInverse(Direction direction)
    {
        var applied = rotateX(direction.X, direction.Y, direction.Z, -X);
        applied = rotateY(applied.x, applied.y, applied.z, -Y);
        applied = rotateZ(applied.x, applied.y, applied.z, -Z);

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
    private static double toRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    private static (double x, double y, double z) rotateX(double x, double y, double z, double angle)
    {
        angle = toRadians(angle);
        double sin = Math.Sin(angle);
        double cos = Math.Cos(angle);
        return (x, y * cos - z * sin, y * sin + z * cos);
    }

    private static (double x, double y, double z) rotateY(double x, double y, double z, double angle)
    {
        angle = toRadians(angle);
        double sin = Math.Sin(angle);
        double cos = Math.Cos(angle);
        return (x * cos + z * sin, y, -x * sin + z * cos);
    }

    private static (double x, double y, double z) rotateZ(double x, double y, double z, double angle)
    {
        angle = toRadians(angle);
        double sin = Math.Sin(angle);
        double cos = Math.Cos(angle);
        return (x * cos - y * sin, x * sin + y * cos, z);
    }
}