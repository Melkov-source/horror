﻿using Code.PanelManager.Interfaces;

namespace Code.PanelManager
{
    public interface IPanelController
    {
        public void Open();
        public void Close();
        public void Release();
        public IPanel GetPanel();
    }

    public interface IPanelController<out TPanel> : IPanelController where TPanel : IPanel
    {
        public TPanel panel { get; }
    }
    
    public interface IPanelControllerProcessor
    {
        public ushort Hash { get; }
        public void Setup(IPanelManager manager, IPanelFactory factory);
        public void Initialize();
        public void Load();
        public void OpenBefore();
        public void OpenAfter();
        public void CloseBefore();
        public void CloseAfter();
        public void Unload();
    }
}