namespace Code.Core.Dialogue
{
	public class PlayerChoiceNode : DialogueNodeBase
	{
		public NPCNode next;

		public PlayerChoiceNode(string text)
		{
			this.text = text;
			next = null;
		}
	}
}