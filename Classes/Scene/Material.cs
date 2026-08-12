public class Material
{
    public readonly string materialName;
    public readonly Color color;
    public readonly Color emissionColor;
    public readonly double metallic;
    public readonly double smoothness;

    public Material(string _materialName, Color _color, Color _emissionColor, double _metallic, double _smoothness)
    {
        materialName = _materialName;
        color = _color;
        emissionColor = _emissionColor;
        metallic = _metallic;
        smoothness = _smoothness;   
    }

    public static Material Default => new(
        "Default",
        new Color(1.0, 1.0, 1.0),
        new Color(0.0, 0.0, 0.0),
        0.0,
        0.5
    );

    public override string ToString()
    {
        return $"Material({materialName}, color={color}, metallic={metallic}, smoothness={smoothness}, emission={emissionColor})";
    }
}