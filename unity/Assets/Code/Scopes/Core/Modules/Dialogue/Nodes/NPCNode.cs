using System.Collections.Generic;

namespace Code.Core.Dialogue
{
	public class NPCNode
	{
		public string text;
		public List<PlayerChoiceNode> choices;

		public NPCNode(string text)
		{
			this.text = text;
			choices = new List<PlayerChoiceNode>();
		}
	}
}