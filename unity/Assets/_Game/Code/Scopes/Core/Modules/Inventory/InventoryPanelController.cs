using System.Collections.Generic;
using System.Linq;
using Code.PanelManager;
using Code.PanelManager.Attributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;

namespace Code.Core.Modules.Inventory
{
	[Panel(PanelType = PanelType.OVERLAY, Order = 3, AssetId = "Inventory/Prefabs/InventoryPanel.prefab")]
	public class InventoryPanelController : PanelControllerBase<InventoryPanel>
	{
		private ObjectPool<InventoryItem> _inventory_item_pool;

		private GameObject _inventory_item_prefab;

		private readonly Dictionary<ItemInfo, InventoryItem> _items = new();

		private int _current_mass;
		private const int MAX_MASS = 10;

		protected override void OnLoad()
		{
			base.OnLoad();

			_inventory_item_prefab = Addressables
				.LoadAssetAsync<GameObject>("Inventory/Prefabs/InventoryItem.prefab")
				.WaitForCompletion();
		}

		protected override void Initialize()
		{
			base.Initialize();

			_inventory_item_pool = new ObjectPool<InventoryItem>
			(
				CreateInventoryItem,
				OnGetInventoryItem,
				OnReleaseInventoryItem,
				OnDestroyInventoryItem
			);
		}
		
		protected override void OnUnload()
		{
			base.OnUnload();

			Addressables.ReleaseInstance(_inventory_item_prefab);
		}

		public bool TryAddItem(ItemInfo info)
		{
			if (_current_mass + info.mass > MAX_MASS)
			{
				return false;
			}
			
			var item = _inventory_item_pool.Get();
			
			item.Setup(info);

			item.on_drop += OnDropItem;
			item.on_take += OnTakeItem;
			item.on_use += OnUseItem;
			
			_items[info] = item;

			_current_mass += info.mass;
			
			item.transform.SetSiblingIndex(_items.Count);
			
			panel.mass_total_tmp.text = $"{_current_mass} / {MAX_MASS} кг";

			return true;
		}

		private void OnDropItem(ItemInfo info)
		{
			var item = _items[info];
			
			item.on_drop -= OnDropItem;
			item.on_take -= OnTakeItem;
			item.on_use -= OnUseItem;
			
			_inventory_item_pool.Release(item);
			
			_items.Remove(info);
			
			_current_mass -= info.mass;

			panel.mass_total_tmp.text = $"{_current_mass} / {MAX_MASS} кг";
		}
		
		private void OnTakeItem(ItemInfo info)
		{
			
		}
		
		private void OnUseItem(ItemInfo info)
		{
			
		}
		
		private InventoryItem CreateInventoryItem()
		{
			var instance = Object.Instantiate
			(
				_inventory_item_prefab,
				panel.items_content,
				false
			);

			return instance.GetComponent<InventoryItem>();
		}

		private static void OnGetInventoryItem(InventoryItem item)
		{
			item.gameObject.SetActive(true);
		}

		private static void OnReleaseInventoryItem(InventoryItem item)
		{
			item.gameObject.SetActive(false);
		}

		private static void OnDestroyInventoryItem(InventoryItem item)
		{
			Object.Destroy(item.gameObject);
		}
	}
}