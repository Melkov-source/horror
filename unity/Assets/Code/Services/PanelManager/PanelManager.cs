using System;
using System.Collections.Generic;
using Code.PanelManager.Interfaces;
using Code.Utils;

namespace Code.PanelManager
{
    public class PanelManager : IPanelManager, IPanelManagerProcessor
    {
        #region Fields

        #region Public Fields

        public PanelDispatcher dispatcher { get; }

        #endregion

        #region Private Fields

        private readonly IPanelControllerFactory _panel_controller_factory;
        private readonly IPanelFactory _panel_factory;

        private readonly Dictionary<ushort, IPanelController> _panels_cashed = new();

        #endregion

        #endregion;

        #region Initialize

        public PanelManager(IPanelControllerFactory panel_controller_factory, IPanelFactory panel_factory, PanelDispatcher dispatcher)
        {
            _panel_controller_factory = panel_controller_factory;
            _panel_factory = panel_factory;

            if (dispatcher == null)
            {
                dispatcher = PanelDispatcherBuilder.Create().Build();
            }

            this.dispatcher = dispatcher;
        }

        #endregion

        #region Public API (Methods)

        public TPanelController LoadPanel<TPanelController>() where TPanelController : IPanelController
        {
            var type = typeof(TPanelController);

            var controller = LoadController(type);

            return (TPanelController)controller;
        }

        public void ClosePanels(params Type[] type_controllers)
        {
            for (int index = 0, count = type_controllers.Length; index < count; index++)
            {
                var type_controller = type_controllers[index];

                var controller = LoadController(type_controller);
                controller.Close();
            }
        }

        public void OpenPanels(params Type[] type_controllers)
        {
            for (int index = 0, count = type_controllers.Length; index < count; index++)
            {
                var type_controller = type_controllers[index];

                var controller = LoadController(type_controller);
                controller.Open();
            }
        }

        private IPanelController LoadController(Type type_panel_controller)
        {
            var meta = PanelReflector.GetMeta(type_panel_controller);
            var hash = type_panel_controller.GetStableHash();

            if (_panels_cashed.TryGetValue(hash, out var controller))
            {
                return controller;
            }

            controller = _panel_controller_factory.Create(type_panel_controller, meta);

            var processor = (IPanelControllerProcessor)controller;

            processor.Setup(this, _panel_factory);
            processor.Load();
            processor.Initialize();

            var panel = controller.GetPanel();

            dispatcher.Cache(panel);

            _panels_cashed.Add(hash, controller);

            return controller;
        }

        #endregion

        void IPanelManagerProcessor.Open(IPanelControllerProcessor processor, IPanel panel)
        {
            processor.OpenBefore();
            dispatcher.Activate(panel);
            processor.OpenAfter();
        }

        void IPanelManagerProcessor.Close(IPanelControllerProcessor processor, IPanel panel)
        {
            processor.CloseBefore();
            dispatcher.Cache(panel);
            processor.CloseAfter();
        }

        void IPanelManagerProcessor.Release(IPanelControllerProcessor processor, IPanel panel)
        {
            dispatcher.Remove(panel);

            processor.Unload();

            _panels_cashed.Remove(processor.Hash);
        }
    }
}