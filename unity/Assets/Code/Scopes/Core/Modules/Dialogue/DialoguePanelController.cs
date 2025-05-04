using Code.PanelManager;
using Code.PanelManager.Attributes;

namespace Code.Core.Dialogue
{
	[Panel(PanelType = PanelType.OVERLAY, Order = 1, AssetId = "Dialogue/Prefabs/DialoguePanel.prefab")]
	public class DialoguePanelController : PanelControllerBase<DialoguePanel>
	{
		
	}
}