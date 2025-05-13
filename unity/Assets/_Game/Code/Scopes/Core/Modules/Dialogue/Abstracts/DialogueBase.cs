using System;
using System.Collections.Generic;

namespace Code.Core.Dialogue
{
	public abstract class DialogueBase
	{
		public abstract List<Func<bool>> conditions { get; }
		public abstract NPCType type { get; }
		public abstract List<NPCNode> sequence { get; }
	}
}