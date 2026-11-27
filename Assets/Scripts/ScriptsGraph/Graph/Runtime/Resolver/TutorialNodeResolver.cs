using System.Threading.Tasks;

public class TutorialNodeResolver : INodeResolver<TutorialRuntimeNode>
{
    public async Task Resolve(DialogController controller, TutorialRuntimeNode node)
    {
        await controller.ExecuteTutorialStep(node);
    }
}