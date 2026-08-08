public readonly record struct Coordinate(double X = 0.0, double Y = 0.0, double Z = 0.0)
{
    public Coordinate Rotate(Rotation rotation) => rotation.Apply(this);

    public Coordinate InverseRotate(Rotation rotation) => rotation.ApplyInverse(this);

    public static Coordinate operator -(Coordinate a, Coordinate b)
    {
        return new Coordinate(a.X - b.X, a.Y - b.Y, a.Z - b.Z);    
    }
    public static Coordinate operator +(Coordinate a, Coordinate b)
    {
        return new Coordinate(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }
    public static Coordinate operator *(Coordinate a, double scalar)
    {
        return new Coordinate(a.X * scalar, a.Y * scalar, a.Z * scalar);
    }
}