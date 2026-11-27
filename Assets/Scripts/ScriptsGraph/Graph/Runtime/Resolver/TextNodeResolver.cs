using System.Threading.Tasks;

public class TextNodeResolver : INodeResolver<TextRuntimeNode>
{
    public async Task Resolve(DialogController controller, TextRuntimeNode node)
    {
        controller.DisplayText(node.text);
        await Task.Delay(node.waitTime);
    }
}