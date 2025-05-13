using System.Collections.Generic;

namespace Code.Core.Dialogue
{
	public class NPCNode : DialogueNodeBase
	{
		public List<PlayerChoiceNode> choices;

		public NPCNode(string text)
		{
			this.text = text;
			choices = new List<PlayerChoiceNode>();
		}
	}
}