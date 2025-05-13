using System.Collections.Generic;
using Code.Core.Dialogue;

namespace Code.Core.NPC
{
	public abstract class NPCBase
	{
		public abstract List<DialogueBase> dialogues { get; }
	}
}