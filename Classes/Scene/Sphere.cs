/// <summary>
/// A sphere
/// Measurements are in meters to match unity's system
/// </summary>
/// <param name="center">The sphere origin in world coordinates</param>
/// <param name="rotation">Any rotation applied to the sphere</param>
/// <param name="xScale">The width of the sphere</param>
/// <param name="yScale">The height of the sphere</param>
/// <param name="zScale">The depth of the sphere</param> <summary>
/// 
/// </summary>
public record struct Sphere(Coordinate center = default, Rotation rotation = default, double xScale = 1.0, double yScale = 1.0, double zScale = 1.0, Material _material = null!) : ISceneObject
{
    // Get each dimensions radii
    private readonly double radiusX = xScale/2;
    private readonly double radiusY = yScale/2;
    private readonly double radiusZ = zScale/2;
    public Material material = _material ?? Material.Default;


    /// <summary>
    /// A collision function for spheres
    /// It treats the sphere center as (0,0,0) and determines the ray location relative to that 
    /// </summary>
    /// <param name="ray">The ray we're determining the collision for</param>
    /// <returns>True if there is a collision, False otherwise</returns>
    public readonly HitResult Trace(Ray ray)
    {
        // Convert everything to local coordinates
        // Translate and then rotate the ray so that the sphere is the origin and has no rotation
        // TODO: Make this less smelly
        Coordinate localRayOrigin = (ray.origin - center).InverseRotate(rotation).ToCoordinate();
        Direction localRayDirection = ray.direction.InverseRotate(rotation).Normalized();

        // Scale the ray so that the ellipsoid is the unit sphere
        Direction rayOrigin_Sphereized = new(localRayOrigin.X / radiusX, localRayOrigin.Y / radiusY, localRayOrigin.Z / radiusZ);
        Direction rayDirection_Sphereized = new(localRayDirection.X / radiusX, localRayDirection.Y / radiusY, localRayDirection.Z / radiusZ);


        // localOrigin + t * localDirection;
        Direction toSphereCenter = -rayOrigin_Sphereized; // Because the sphere is at (0, 0, 0)
        double distanceAlongRay = Direction.Dot(toSphereCenter, rayDirection_Sphereized)
                                / Direction.Dot(rayDirection_Sphereized, rayDirection_Sphereized); // (t) // What distance along the ray is closest to the center?

        
        Direction closestPoint = rayOrigin_Sphereized + rayDirection_Sphereized * distanceAlongRay;
        double distClosestPointToCenter = closestPoint.Length;
        
        if (distClosestPointToCenter <= 0.0) return HitResult.Miss;
        if (distClosestPointToCenter > 1.0) return HitResult.Miss;

        // Un-scale back to local (pre-sphereized) space
        Direction closestPoint_Local = new(
            closestPoint.X * radiusX,
            closestPoint.Y * radiusY,
            closestPoint.Z * radiusZ);

        // Un-rotate and un-translate back to world space
        Coordinate worldHitPoint = closestPoint_Local.Rotate(rotation).ToCoordinate() + center;

        return HitResult.Hit(distanceAlongRay, worldHitPoint, material);
    }

    public override readonly string ToString()
    {
        return $"""
        Sphere :
        Center:  {center}
        Rotation: {rotation}

        Width:   {xScale}
        Height:  {yScale}
        Depth:   {zScale}

        Material: {material}
        """;
    }
}