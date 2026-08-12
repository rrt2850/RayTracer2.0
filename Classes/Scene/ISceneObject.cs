public interface ISceneObject
{
    HitResult Trace(Ray ray);
    string ToString();
}