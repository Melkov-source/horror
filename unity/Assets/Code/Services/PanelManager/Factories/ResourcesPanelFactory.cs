using System;
using Code.PanelManager.Attributes;
using Code.PanelManager.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.PanelManager
{
    public class ResourcesPanelFactory : IPanelFactory
    {
        public IPanel Create(PanelAttribute meta)
        {
            var panelPrefab = Resources.Load<PanelBase>(meta.AssetId);

            if (panelPrefab == default)
            {
                throw new Exception($"Not found asset in Resources for path: {meta.AssetId}");
            }

            var panel = Object.Instantiate(panelPrefab);

            return panel;
        }
    }
}