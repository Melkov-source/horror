using System;
using Cysharp.Threading.Tasks;
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

		public async UniTask Setup(DialogueNodeBase node, int ms)
		{
			_node = node;

			_text.text = "";

			if (node.text.Length <= 10)
			{
				ms /= 2;
			}

			var count = Mathf.CeilToInt(node.text.Length / 35f);
			
			Debug.Log($"L: {node.text.Length}, M: {count}");

			if (count == 0)
			{
				count = 1;
			}

			var size = count * 60;
			
			GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, size);

			var time =  ms / node.text.Length;

			foreach (var c in node.text)
			{
				_text.text += c;
				await UniTask.Delay(time);
			}
		}

		private void OnClick()
		{
			on_click?.Invoke(_node);
		}
	}
}