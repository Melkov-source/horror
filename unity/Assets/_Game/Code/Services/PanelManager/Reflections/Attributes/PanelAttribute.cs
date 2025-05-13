using JetBrains.Annotations;
using UnityEngine.Scripting;

namespace Code.PanelManager.Attributes
{
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    public class PanelAttribute : PreserveAttribute
    {
        public PanelType PanelType;
        public int Order;
        public string AssetId;
    }
}