using System;
using System.Reflection;
using Code.PanelManager.Attributes;

namespace Code.PanelManager
{
    internal static class PanelReflector
    {
        internal static PanelAttribute GetMeta(Type controller)
        {
            var meta = controller.GetCustomAttribute<PanelAttribute>();

            if (meta == default)
            {
                throw new Exception($"Not found Attribute.Panel for controller: {controller.Name}");
            }

            return meta;
        }
       
    }
}