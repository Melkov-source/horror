using System.Collections.Generic;
using Code.Core.Dialogue;

namespace Code.Core.NPC
{
	public class MomNPC : NPCBase
	{
		public override List<DialogueBase> dialogues => new()
		{
			new MomDialogue_001(),
		};
	}
}