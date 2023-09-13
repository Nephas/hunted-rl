using System.Linq;
using Godot;
using Vector2 = System.Numerics.Vector2;

namespace huntedrl.script
{
    public static class Selectors
    {
        public static Entity GetPC(this Node node)
        {
            return node.GetTree().GetNodesInGroup("pc").OfType<Entity>().FirstOrDefault();
        }

        public static Entity GetEntity(this Node node)
        {
            return node.GetParent<Entity>();
        }

        public static T GetChildByName<T>(this Node node, string name) where T : Node
        {
            return node.GetChildren().OfType<T>().FirstOrDefault(n => n.Name == name);
        }
    }
}