using Newtonsoft.Json.Linq;

public class Scene
{
    public List<ISceneObject> sceneObjects = [];
    public Camera camera;
    public static readonly Color backgroundColor = new(0.0, 0.608, 1.0);
    
    public Scene(string path)
    {
        try
        {
            var json = File.ReadAllText(path) ?? throw new Exception("Issue reading from path");
            JObject jsonData = JObject.Parse(json) ?? throw new Exception("Parsed data was null");
            ParseData(jsonData);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Json loading failed: {e}");
            throw;
        }
    }

    private void ParseData(JObject data)
    {
        try
        {
            ParseCamera(data);
            ParseSceneObjects(data);
        }
        catch(Exception e)
        {
            Console.Error.WriteLine($"Json parsing failed: {e}");
            
        }
    }



    private void ParseSceneObjects(JObject data)
    {
        JArray objects = data["objects"] as JArray ?? throw new Exception("No objects array provided");

        ISceneObject currObject;
        Coordinate currPosition;
        Rotation currRotation;
        Material currMaterial;
        string currType;
        (double x, double y, double z) currScale;

        Console.WriteLine(data);
        foreach(var obj in objects){
            JToken tempType = obj["primitiveType"] ?? throw new Exception("No primitive type provided");
            currType = (string)tempType!;
            
            JToken position = obj["position"] ?? throw new Exception($"No '{currType}' position provided");
            JToken rotation = obj["rotation"] ?? throw new Exception($"No '{currType}' rotation provided");
            JToken scale = obj["scale"] ?? throw new Exception($"No '{currType}' scale provided");
            JToken materialData = obj["material"]!;

            currPosition = new((double)position["x"]!, (double)position["y"]!, (double)position["z"]!);
            currRotation = new((double)rotation["x"]!, (double)rotation["y"]!, (double)rotation["z"]!);
            currScale = ((double)scale["x"]!, (double)scale["y"]!, (double)scale["z"]!);

            currMaterial = ParseMaterial(materialData, currType);

            switch (currType)
            {
                case "Rectangle":
                    currObject = new Rectangle(currPosition, currRotation, currScale.x, currScale.y, currScale.z, currMaterial);
                    break;
                case "Sphere":
                    currObject = new Sphere(currPosition, currRotation, currScale.x, currScale.y, currScale.z, currMaterial);
                    break;
                default:
                    throw new Exception($"'{currType}' is not a recognized ISceneObject type");
            }

            sceneObjects.Add(currObject);
        }
    }

    /// <summary>
    /// Parses a material object from the scene data. Falls back to Material.Default
    /// if no material was provided, so scene files without materials still load.
    /// </summary>
    /// <param name="materialData">The material JObject, or null if absent</param>
    /// <param name="ownerType">The primitive type this material belongs to, used for error messages</param>
    /// <exception cref="Exception">Thrown if a material block is present but missing required color data</exception>
    private Material ParseMaterial(JToken materialData, string ownerType)
    {
        if (materialData == null)
        {
            return Material.Default;
        }

        string materialName = (string)materialData["materialName"]! ?? "Unnamed Material";

        JToken colorToken = materialData["color"] ?? throw new Exception($"No '{ownerType}' material color provided");
        Color color = new((double)colorToken["r"]!, (double)colorToken["g"]!, (double)colorToken["b"]!);

        double metallic = (double)(materialData["metallic"] ?? 0.0);
        double smoothness = (double)(materialData["smoothness"] ?? 0.5);

        Color emissionColor = new(0.0, 0.0, 0.0);
        JToken emissionToken = materialData["emissionColor"]!;
        if (emissionToken != null)
        {
            emissionColor = new((double)emissionToken["r"]!, (double)emissionToken["g"]!, (double)emissionToken["b"]!);
        }

        return new Material(materialName, color, emissionColor, metallic, smoothness);
    }

    /// <summary>
    /// Tries to parse a camera object from the scene data
    /// </summary>
    /// <param name="data">The scene data</param>
    /// <exception cref="Exception">Any exception finding data in the data object</exception>
    private void ParseCamera(JObject data)
    {
        JToken cameraData = data["camera"] ?? throw new Exception("No camera data provided");
        JToken position = cameraData["position"] ?? throw new Exception("No camera position provided");
        JToken rotation = cameraData["rotation"] ?? throw new Exception("No camera rotation provided");


        Coordinate cameraPosition = new((double)position["x"]!, (double)position["y"]!, (double)position["z"]!);
        Rotation cameraRotation = new((double)rotation["x"]!, (double)rotation["y"]!, (double)rotation["z"]!);

        camera = new Camera(cameraPosition, cameraRotation, (double)cameraData["fov"]!
                        , (int)cameraData["xResolution"]!, (int)cameraData["yResolution"]!);
    }

    public override string ToString()
    {
        string returnString = camera.ToString();
        foreach(ISceneObject obj in sceneObjects)
        {
            returnString += "\n" + obj.ToString();
        }

        return returnString;
    }

    public void Render()
    {   
        Ray currRay;
        HitResult closestCollision;
        HitResult collisionData;

        for(int currX = 0; currX < camera.xRes; currX++)
        {
            for(int currY = 0; currY < camera.yRes; currY++)
            {
                currRay = camera.rays[currX * camera.yRes + currY];
                closestCollision = HitResult.Miss;

                foreach(ISceneObject obj in sceneObjects)
                {
                    collisionData = obj.Trace(currRay);
                    if (collisionData.hit && collisionData.distance < closestCollision.distance)
                    {
                        closestCollision = collisionData;
                    }
                    
                }

                camera.colorVals[currX, currY] = closestCollision.material?.color ?? backgroundColor;
            }
        }

        camera.SaveImage();
    }
}