using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Code.Core.Dialogue;

namespace Code.Core.NPC
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class MomDialogue_001 : Dialogue.DialogueBase
	{
		public override NPCType type => NPCType.MOM;

		public override List<Func<bool>> conditions => new();

		public override List<NPCNode> sequence => new()
		{
			new NPCNode("...")
			{
				choices = new List<PlayerChoiceNode>
				{
					new("Ещё долго?"),
					new("А че мы так долго едем?"),
					new("Ёпта! Я проснулся, где мы?")
				}
			},

			new NPCNode("Почти приехали. Ещё пару километров и будем на развилке.")
			{
				choices = new List<PlayerChoiceNode>
				{
					new("Там хоть есть интернет?")
				}
			},

			new NPCNode("Вряд ли. Но там спокойно. И деду будет приятно, что ты приехал.")
			{
				choices = new List<PlayerChoiceNode>
				{
					new("Надеюсь, не заставит ловить белок или чинить крышу.")
				}
			},
			
			new NPCNode("Ну крышу уже починили. А вот с белками — не обещаю")
			{
				choices = new List<PlayerChoiceNode>
				{
					new("...")
				}
			}
		};
	}
}