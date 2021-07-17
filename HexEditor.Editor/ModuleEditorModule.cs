using HexEditor.EditorModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace HexEditor.EditorModule
{
    public class ModuleEditorModule : IModule
    {
        private readonly IRegionManager regionManager;

        public ModuleEditorModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager.RegisterViewWithRegion("MainRegion", typeof(Editor));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .RegisterSingleton<object, Editor>("Editor");
        }
    }
}
