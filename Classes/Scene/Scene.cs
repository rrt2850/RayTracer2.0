using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Scene
{
    public List<ISceneObject> sceneObjects = [];
    public Camera camera;
    private readonly Color backgroundColor = new(0.0, 0.608, 1.0);
    
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
        string currType;
        (double x, double y, double z) currScale;

        foreach(var obj in objects){
            JToken tempType = obj["primitiveType"] ?? throw new Exception("No primitive type provided");
            currType = (string)tempType!;
            
            JToken position = obj["position"] ?? throw new Exception($"No '{currType}' position provided");
            JToken rotation = obj["rotation"] ?? throw new Exception($"No '{currType}' rotation provided");
            JToken scale = obj["scale"] ?? throw new Exception($"No '{currType}' scale provided");

            currPosition = new((double)position["x"]!, (double)position["y"]!, (double)position["z"]!);
            currRotation = new((double)rotation["x"]!, (double)rotation["y"]!, (double)rotation["z"]!);
            currScale = ((double)scale["x"]!, (double)scale["y"]!, (double)scale["z"]!);

            switch (currType)
            {
                case "Rectangle":
                    currObject = new Rectangle(currPosition, currRotation, currScale.x, currScale.y, currScale.z);
                    break;
                default:
                    throw new Exception($"'{currType}' is not a recognized ISceneObject type");
            }

            sceneObjects.Add(currObject);
        }
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
        bool collides = false;

        for(int currX = 0; currX < camera.xRes; currX++)
        {
            for(int currY = 0; currY < camera.yRes; currY++)
            {
                currRay = camera.rays[currX * camera.yRes + currY];
                foreach(ISceneObject obj in sceneObjects)
                {
                    collides = obj.Trace(currRay);
                    if(collides) break;
                }

                if (collides)
                {
                    camera.colorVals[currX, currY] = new Color(1.0, 0.0, 0.0);
                }
                else
                {
                    camera.colorVals[currX, currY] = backgroundColor;
                }
            }
        }

        Console.WriteLine(ToString());

        camera.SaveImage();
    }
}