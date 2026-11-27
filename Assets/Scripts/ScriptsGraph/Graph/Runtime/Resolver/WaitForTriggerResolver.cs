using System.Threading.Tasks;
public class WaitForTriggerResolver : INodeResolver<WaitForTriggerRuntimeNode>
{
    public async Task Resolve(
        DialogController controller,
        WaitForTriggerRuntimeNode node)
    {
        await controller.WaitForTrigger(node.colliderTag);
    }
}