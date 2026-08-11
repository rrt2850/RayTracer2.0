/// <summary>
/// A rectangular prism (or a cube)
/// Measurements are in meters to match unity's system
/// </summary>
/// <param name="center">The rectangle origin in world coordinates</param>
/// <param name="rotation">Any rotation applied to the rectangle</param>
/// <param name="width">The width of the rectangle</param>
/// <param name="height">The height of the rectangle</param>
/// <param name="depth">The depth of the rectangle</param> <summary>
/// 
/// </summary>
public record struct Rectangle(Coordinate center = default, Rotation rotation = default, double width = 1.0, double height = 1.0, double depth = 1.0) : ISceneObject
{
    private readonly double xMin = -width/2;
    private readonly double xMax = width/2;

    private readonly double yMin = -height/2;
    private readonly double yMax = height/2;

    private readonly double zMin = -depth/2;
    private readonly double zMax = depth/2;


    /// <summary>
    /// A function that traces a ray and determines what color it is
    /// </summary>
    /// <param name="ray">The ray to trace</param>
    /// <returns>The calculated color</returns>
    public readonly bool Trace(Ray ray)
    {
        return Collides(ray);
    }

    /// <summary>
    /// A collision function for rectangular prisms.
    /// It treats the rectangle center as (0,0,0) and determines the ray location relative to that 
    /// </summary>
    /// <param name="ray">The ray we're determining the collision for</param>
    /// <returns>True if there is a collision, False otherwise</returns>
    public readonly bool Collides(Ray ray)
    {
        // Convert everything to local coordinates
        // Translate and then rotate the ray so that the rectangle is the origin and has no rotation
        // TODO: Make this less smelly
        Coordinate localOrigin = (ray.origin - center).InverseRotate(rotation).ToCoordinate();
        Direction localDirection = ray.direction.InverseRotate(rotation);

        // Since the ray is origin + t * direction, what t value makes the ray's x coordinate equal xMin?
        // origin + t * direction = xMin
        // (xMin - origin)/direction = t
        double t_xMin = (xMin - localOrigin.X)/localDirection.X;
        double t_xMax = (xMax - localOrigin.X)/localDirection.X;

        double t_yMin = (yMin - localOrigin.Y)/localDirection.Y;
        double t_yMax = (yMax - localOrigin.Y)/localDirection.Y;

        double t_zMin = (zMin - localOrigin.Z)/localDirection.Z;
        double t_zMax = (zMax - localOrigin.Z)/localDirection.Z;

        // Find which of the t values is closer between each set. That is where the ray intersects the box first
        double t_xNear = Math.Min(t_xMin, t_xMax);
        double t_xFar = Math.Max(t_xMin, t_xMax);

        double t_yNear = Math.Min(t_yMin, t_yMax);
        double t_yFar = Math.Max(t_yMin, t_yMax);

        double t_zNear = Math.Min(t_zMin, t_zMax);
        double t_zFar = Math.Max(t_zMin, t_zMax);

        // Find the smallest and largest t values to figure out the range of t values needed to satisfy all boundaries
        double lastArrival = Math.Max(t_xNear, Math.Max(t_yNear, t_zNear));
        double firstDeparture = Math.Min(t_xFar, Math.Min(t_yFar, t_zFar));

        if (firstDeparture < 0) return false;

        if(lastArrival <=firstDeparture) return true;
        
        return false;
    }

    public override readonly string ToString()
    {
        return $"""
        Rectangle:
        Center:  {center}
        Rotation: {rotation}

        Width:   {width}
        Height:  {height}
        Depth:   {depth}

        X Min:   {xMin}
        X Max:   {xMax}
        Y Min:   {yMin}
        Y Max:   {yMax}
        Z Min:   {zMin}
        Z Max:   {zMax}
        """;
    }
}