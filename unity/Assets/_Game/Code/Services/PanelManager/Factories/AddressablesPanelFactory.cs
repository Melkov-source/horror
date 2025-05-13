using Code.PanelManager.Attributes;
using Code.PanelManager.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.PanelManager
{
	public class AddressablesPanelFactory : IPanelFactory
	{
		public IPanel Create(PanelAttribute meta)
		{
			var prefab = Addressables
				.LoadAssetAsync<GameObject>(meta.AssetId)
				.WaitForCompletion();
			
			var panel = Object.Instantiate(prefab);

			return panel.GetComponent<PanelBase>();
		}
	}
}