using System;
using Code.Core.Modules.Inventory.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Core.Modules.Inventory
{
	public class InventoryItem : MonoBehaviour
	{
		public event Action<ItemInfo> on_take; 
		public event Action<ItemInfo> on_use; 
		public event Action<ItemInfo> on_drop;
		
		[SerializeField] private Image _icon_img;
		[SerializeField] private TMP_Text _name_tmp;
		[SerializeField] private TMP_Text _mass_tmp;
		
		[SerializeField] private Button _use_btn;
		[SerializeField] private Button _drop_btn;
		[SerializeField] private Button _take_btn;

		private ItemInfo _info;

		private void OnEnable()
		{
			_use_btn.onClick.AddListener(OnUse);
			_take_btn.onClick.AddListener(OnTake);
			_drop_btn.onClick.AddListener(OnDrop);
		}

		private void OnDisable()
		{
			_use_btn.onClick.RemoveListener(OnUse);
			_take_btn.onClick.RemoveListener(OnTake);
			_drop_btn.onClick.RemoveListener(OnDrop);
		}

		public void Setup(ItemInfo info)
		{
			_info = info;
			
			_icon_img.sprite = info.icon;
			_name_tmp.text = info.name;
			_mass_tmp.text = $"{info.mass} кг";

			switch (info.action)
			{
				case ITEM_ACTION.NONE:
					_use_btn.gameObject.SetActive(false);
					_take_btn.gameObject.SetActive(false);
					break;
				
				case ITEM_ACTION.USE:
					_use_btn.gameObject.SetActive(true);
					_take_btn.gameObject.SetActive(false);
					break;
				case ITEM_ACTION.TAKE:
					_use_btn.gameObject.SetActive(false);
					_take_btn.gameObject.SetActive(true);
					break;
				
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void OnUse()
		{
			on_use?.Invoke(_info);
		}
		
		private void OnTake()
		{
			on_take?.Invoke(_info);
		}
		
		private void OnDrop()
		{
			on_drop?.Invoke(_info);
		}
	}
}