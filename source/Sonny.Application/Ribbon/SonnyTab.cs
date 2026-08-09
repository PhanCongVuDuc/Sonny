using Sonny.Application.Commands ;
using Sonny.EasyRibbon.UIAttributeBase ;

namespace Sonny.Application.Ribbon ;

[Tab("Sonny")]
public class SonnyTab
{
    [Panel("Settings")]
    public class SettingsPanel
    {
        [Button("License",
            typeof( LoginCommand ),
            Image = "/Sonny.Application;component/Resources/Icons/LoginCommand16.png",
            LargeImage = "/Sonny.Application;component/Resources/Icons/LoginCommand32.png",
            ToolTip = "View license information",
            LongDescription = "View and manage your license information")]
        public class LicenseButton ;

        [Button("Settings",
            typeof( SettingsCommand ),
            Image = "/Sonny.Application;component/Resources/Icons/SettingsCommand16.png",
            LargeImage = "/Sonny.Application;component/Resources/Icons/SettingsCommand32.png",
            ToolTip = "Open application settings",
            LongDescription = "Configure application preferences including display units")]
        public class SettingsButton ;
    }

    [Panel("Dimension Tools")]
    public class DimensionPanel
    {
        [Button("Auto Column Dimension",
            typeof( AutoColumnDimensionCommand ),
            Image = "/Sonny.Application;component/Resources/Icons/AutoColumnDimensionCommand16.png",
            LargeImage = "/Sonny.Application;component/Resources/Icons/AutoColumnDimensionCommand32.png",
            ToolTip = "Automatically create dimensions for columns",
            LongDescription = "This tool automatically creates dimensions for columns based on grids")]
        public class AutoColumnDimensionButton ;
    }

    [Panel("Model from CAD")]
    public class ColumnFromCadPanel
    {
        [Button("Column from CAD",
            typeof( ColumnFromCadCommand ),
            Image = "/Sonny.Application;component/Resources/Icons/ColumnFromCadCommand16.png",
            LargeImage = "/Sonny.Application;component/Resources/Icons/ColumnFromCadCommand32.png",
            ToolTip = "Create columns from AutoCAD",
            LongDescription = "Model columns from AutoCAD CAD link by selecting layers and column families")]
        public class ColumnFromCadButton ;
    }
}
