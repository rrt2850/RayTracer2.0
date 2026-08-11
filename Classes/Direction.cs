/// <summary>
/// A struct representing a direction.
/// I thought about combining coordinate and direction into one class,
/// but I wanted to keep them separate so their usages are clear in the code
/// </summary>
/// <param name="X">The x direction</param>
/// <param name="Y">The y direction</param>
/// <param name="Z">The z direction</param> 
public readonly record struct Direction(double X = 0.0, double Y = 0.0, double Z = 0.0)
{
    public Direction Rotate(Rotation r) => r.Apply(this);
    public Direction InverseRotate(Rotation r) => r.ApplyInverse(this);
    public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);
    public Direction Normalized() => this * (1.0 / Length);

    public Coordinate ToCoordinate() => new(X, Y, Z);

    public static double Dot(Direction a, Direction b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;
    public static Direction Cross(Direction a, Direction b) => new(
        a.Y * b.Z - a.Z * b.Y,
        a.Z * b.X - a.X * b.Z,
        a.X * b.Y - a.Y * b.X
    );

    public static Direction operator +(Direction a, Direction b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Direction operator *(Direction a, double s) => new(a.X * s, a.Y * s, a.Z * s);
    public static Direction operator -(Direction a) => new(-a.X, -a.Y, -a.Z);

    public override string ToString() => $"<{X}, {Y}, {Z}>";
    
}