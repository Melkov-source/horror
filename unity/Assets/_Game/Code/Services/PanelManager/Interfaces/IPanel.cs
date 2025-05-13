using UnityEngine;

namespace Code.PanelManager
{
	public interface IPanel
	{
		public PanelState State { get; }
		public PanelInfo Info { get; }
		public RectTransform RectTransform { get; }
		public void SetActive(bool isActive);
		public void SetParent(Transform parent);
		public void SetStretch();
		public GameObject GetGameObject();
	}
	
	public interface IPanelProcessor
	{
		public void Setup(PanelInfo info);
	}
}
