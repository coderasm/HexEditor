using HexEditor.Core.PrismAdapters;
using HexEditor.Core.Services;
using HexEditor.EditorModule;
using HexEditor.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Windows;
using System.Windows.Controls;

namespace HexEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                    .RegisterSingleton<IFolderBrowserDialogService, FolderBrowserDialogService>()
                    .RegisterSingleton<IMessageBoxService, MessageBoxService>()
                    .RegisterSingleton<IFileService, FileService>()
                    .RegisterSingleton<IEnvironmentService, EnvironmentService>()
                    .RegisterSingleton<IErrorService, ErrorService>()
                    .RegisterSingleton<IFileSelectDialogService, FileSelectDialogService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule(typeof(ModuleEditorModule));
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping(typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());
        }
    }
}
