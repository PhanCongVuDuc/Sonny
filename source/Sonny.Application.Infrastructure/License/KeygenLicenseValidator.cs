using Sonny.Application.Domain.Entities.License ;
using Sonny.Application.Domain.Services ;
using Sonny.Keygen.Models ;
using Sonny.Keygen.Services ;
using Sonny.Keygen.UI.ViewModels ;
using Sonny.Keygen.UI.Views ;

namespace Sonny.Application.Infrastructure.License ;

public class KeygenLicenseValidator(AutoLoginService autoLoginService, UserInfoService userInfoService)
    : ILicenseValidator
{
    private LicenseStatus _cachedStatus = LicenseStatus.Invalid() ;

    private UserInfoResult? _userInfo ;

    public LicenseStatus GetLicenseStatus() => _cachedStatus ;

    public async Task<LicenseStatus> TryAutoLoginAsync()
    {
        try {
            var autoLoginResult = await autoLoginService.TryAutoLoginAsync() ;

            if (! autoLoginResult.IsSuccess) {
                _cachedStatus = LicenseStatus.Invalid("Not logged in") ;
                _userInfo = null ;
                return _cachedStatus ;
            }

            // Get user info for display
            if (! string.IsNullOrEmpty(autoLoginResult.Email)
                && ! string.IsNullOrEmpty(autoLoginResult.UserId)) {
                // Online mode - load full user info
                _userInfo = await userInfoService.LoadAndFormatUserInfoAsync(autoLoginResult.Email,
                    autoLoginResult.UserId) ;
            }
            else if (autoLoginResult.LicenseInfo != null) {
                // Offline mode - format license info
                _userInfo = userInfoService.FormatLicenseInfo(autoLoginResult.Email ?? "",
                    autoLoginResult.LicenseInfo) ;
            }

            // Update cached status
            if (_userInfo != null) {
                _cachedStatus = LicenseStatus.Valid(_userInfo.Email,
                    _userInfo.LicenseType,
                    autoLoginResult.IsOfflineLicense,
                    ParseDate(_userInfo.LicenseStartDate),
                    ParseDate(_userInfo.LicenseExpiryDate)) ;
            }
            else {
                _cachedStatus = LicenseStatus.Invalid("No license info") ;
            }

            return _cachedStatus ;
        }
        catch (Exception ex) {
            _cachedStatus = LicenseStatus.Invalid(ex.Message) ;
            return _cachedStatus ;
        }
    }

    public async Task ShowLicenseWindow()
    {
        LoginView window ;

        if (_userInfo != null) {
            var viewModel = new LoginViewModel(_userInfo) ;
            window = new LoginView(viewModel) ;
        }
        else {
            window = new LoginView() ;
        }

        window.ShowDialog() ;
        // After window closes, refresh cached status
        _ = await TryAutoLoginAsync() ;
    }

    private static DateTime? ParseDate(string? dateString)
    {
        if (string.IsNullOrEmpty(dateString)
            || dateString == "N/A"
            || dateString == "Unlimited") {
            return null ;
        }

        return DateTime.TryParse(dateString,
            out var result)
            ? result
            : null ;
    }
}
