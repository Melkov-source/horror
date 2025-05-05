using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Core.Dialogue
{
	public class DialogueTextBlock : MonoBehaviour
	{
		public event Action<DialogueNodeBase> on_click;

		[SerializeField] private Button _button;
		[SerializeField] private TMP_Text _text;

		private DialogueNodeBase _node;

		public void OnEnable()
		{
			_button.onClick.AddListener(OnClick);
		}

		public void OnDisable()
		{
			_button.onClick.RemoveListener(OnClick);
		}

		public void SetText(DialogueNodeBase node)
		{
			_node = node;
			
			_text.text = node.text;
		}

		private void OnClick()
		{
			on_click?.Invoke(_node);
		}
	}
}