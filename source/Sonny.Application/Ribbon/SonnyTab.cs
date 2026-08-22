using Sonny.Application.Commands ;
using Sonny.EasyRibbon.UIAttributeBase ;

namespace Sonny.Application.Ribbon ;

/// <summary>
///     Main ribbon tab for Sonny application
/// </summary>
[Tab("Sonny")]
public class SonnyTab
{
    /// <summary>
    ///     Panel for application settings
    /// </summary>
    [Panel("Settings")]
    public class SettingsPanel
    {
        /// <summary>
        ///     Button for license info
        /// </summary>
        [Button("License",
            typeof( LoginCommand ),
            Image = "/Sonny.Application;component/Resources/Icons/LoginCommand16.png",
            LargeImage = "/Sonny.Application;component/Resources/Icons/LoginCommand32.png",
            ToolTip = "View license information",
            LongDescription = "View and manage your license information")]
        public class LicenseButton ;

        /// <summary>
        ///     Button for application settings
        /// </summary>
        [Button("Settings",
            typeof( SettingsCommand ),
            Image = "/Sonny.Application;component/Resources/Icons/SettingsCommand16.png",
            LargeImage = "/Sonny.Application;component/Resources/Icons/SettingsCommand32.png",
            ToolTip = "Open application settings",
            LongDescription = "Configure application preferences including display units")]
        public class SettingsButton ;
    }

    /// <summary>
    ///     Panel for dimension tools
    /// </summary>
    [Panel("Dimension Tools")]
    public class DimensionPanel
    {
        /// <summary>
        ///     Button for auto column dimension feature
        /// </summary>
        [Button("Auto Column Dimension",
            typeof( AutoColumnDimensionCommand ),
            Image = "/Sonny.Application;component/Resources/Icons/AutoColumnDimensionCommand16.png",
            LargeImage = "/Sonny.Application;component/Resources/Icons/AutoColumnDimensionCommand32.png",
            ToolTip = "Automatically create dimensions for columns",
            LongDescription = "This tool automatically creates dimensions for columns based on grids")]
        public class AutoColumnDimensionButton ;
    }

    /// <summary>
    ///     Panel for geometry join tools
    /// </summary>
    [Panel("Join Tools")]
    public class JoinPanel
    {
        /// <summary>
        ///     Button for auto join feature
        /// </summary>
        [Button("Auto Join",
            typeof( AutoJoinCommand ),
            Image = "/Sonny.Application;component/Resources/Icons/AutoJoinCommand16.png",
            LargeImage = "/Sonny.Application;component/Resources/Icons/AutoJoinCommand32.png",
            ToolTip = "Automatically join intersecting elements",
            LongDescription = "Joins intersecting elements by priority rules, with the cut order following each rule")]
        public class AutoJoinButton ;
    }

    /// <summary>
    ///     Panel for column creation tools
    /// </summary>
    [Panel("Model from CAD")]
    public class ColumnFromCadPanel
    {
        /// <summary>
        ///     Button for column from CAD feature
        /// </summary>
        [Button("Column from CAD",
            typeof( ColumnFromCadCommand ),
            Image = "/Sonny.Application;component/Resources/Icons/ColumnFromCadCommand16.png",
            LargeImage = "/Sonny.Application;component/Resources/Icons/ColumnFromCadCommand32.png",
            ToolTip = "Create columns from AutoCAD",
            LongDescription = "Model columns from AutoCAD CAD link by selecting layers and column families")]
        public class ColumnFromCadButton ;
    }
}
