namespace Code.Core.Dialogue
{
	public class PlayerChoiceNode
	{
		public string text;
		public NPCNode next;

		public PlayerChoiceNode(string text)
		{
			this.text = text;
			next = null;
		}
	}
}