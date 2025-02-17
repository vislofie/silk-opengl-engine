using System.Numerics;

public class Scene
{
    public List<SceneObject> SceneObjects { get; private set; } = new List<SceneObject>();
    
    public Scene()
    {
    }

    public void AddCube(Vector3 position, Vector3 scale, Vector3 rotation, Shader shader)
    {
        Cube cube = new Cube(position, scale, rotation, shader);
        SceneObjects.Add(cube);
    }

    public void Render()
    {
        foreach (SceneObject obj in SceneObjects)
        {
            MeshShapes.SetBrush(obj.Shape);
            MeshShapes.Paint(obj.Position, obj.Scale, obj.Rotation, obj.Shader);
        }
    }

    public void RemoveObject(SceneObject sceneObj)
    {
        if (!SceneObjects.Contains(sceneObj))
        {
            return;
        }

        SceneObjects.Remove(sceneObj);
    }
}