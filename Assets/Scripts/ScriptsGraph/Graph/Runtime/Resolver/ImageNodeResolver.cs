using System.Threading.Tasks;

public class ImageNodeResolver : INodeResolver<ImageRuntimeNode>
{
    public Task Resolve(DialogController controller, ImageRuntimeNode node)
    {
        controller.DisplayImage(node.image);

        return Task.CompletedTask;
    }
}