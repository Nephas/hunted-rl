using System.Linq;
using Godot;

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
    }
}