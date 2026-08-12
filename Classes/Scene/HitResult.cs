public readonly struct HitResult
{
    public readonly bool hit;
    public readonly double distance;
    public readonly Coordinate point;
    public readonly Material material;

    private HitResult(bool hit, double distance, Coordinate point, Material material)
    {
        this.hit = hit;
        this.distance = distance;
        this.point = point;
        this.material = material;
    }
    public static HitResult Hit(double distance, Coordinate point, Material material) =>
        new(true, distance, point, material);

    public static HitResult Miss => new(false, double.PositiveInfinity, default, null!);
}