using Nice3point.Revit.Toolkit.External ;
using Revit.Async ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Infrastructure.Resource.Implements ;
using Sonny.Application.Modules ;
using Sonny.Application.UIStyle ;
using Sonny.Keygen.Services ;

namespace Sonny.Application ;

/// <summary>
///     Application entry point
/// </summary>
public class SonnyApp : ExternalApplication
{
    private readonly SonnyModule _module = new() ;

    public override void OnStartup()
    {
        KeygenConfigBuilder.Create()
            .WithKeygenAccountId("d8f5481f-2c1c-48d1-a242-be2d0c44c3d3")
            .WithUserApiToken("admin-149a34d7d72bb3facd8fd3f44e31b102668738b1d6137496c272703f559550f2v3")
            .WithAdminToken("admin-a0433e5355de96d5efdd04e4cadeb589ed212c39635d895d6917d128215cfb96v3")
            .WithProductToken("prod-66746adcb0c3419666f9bf2727e26d0ab340519e98c539e73dc3784e0e62341ev3")
            .WithDeleteToken("prod-befff7a6e11abeb599f32973c1f45e5ba38bf0a94cc833498d7a7f081a17b03fv3")
            .WithTrialPolicyId("4d51a028-9f29-4995-83b4-8cd17537aa02")
            .WithKeygenPublicKey("3a88de66119935f6c2d5fa3877d4e489518c64fc599014c8d06168402bb48112")
            .WithAuth0Domain("dev-fmvobru3d1fp5d5v.us.auth0.com")
            .WithAuth0ClientId("TneVS20JOknsMZvYGOvFT1rA4sHBNTLt")
            .WithAuth0RedirectUri("sonny://callback")
            .Build() ;

        // Initialize RevitTask for async Revit API calls
        RevitTask.Initialize(Application) ;

        // Load UI theme
        UIStyleManager.LoadTheme() ;

        Host.Start() ;

        // Initialize application resources
        var resourcesInitializer = Host.GetService<ResourcesInitializer>() ;
        resourcesInitializer.Initialize() ;

        var licenseValidator = Host.GetService<ILicenseValidator>() ;
        licenseValidator.TryAutoLoginAsync() ;

        // Initialize EasyRibbon module
        _module.OnStartup(Application) ;
    }

    public override void OnShutdown() =>
        // Shutdown EasyRibbon module
        _module.OnShutdown(Application) ;
}
