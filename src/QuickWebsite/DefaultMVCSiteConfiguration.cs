namespace Stevehead.Web.QuickWebsite;

// Internal default configuration.
internal sealed class DefaultMVCSiteConfiguration : IMVCSiteConfiguration
{
    public static IMVCSiteConfiguration Instance { get; } = new DefaultMVCSiteConfiguration();

    private DefaultMVCSiteConfiguration() { }

    public bool UseForwardedHeaders => false;

    public string ExceptionHandler => "/error-500/";

    public bool RewriteUrlsToForceTrailingSlash => true;

    public string DefaultControllerRoutePattern => "{controller=Public}/{action=Index}/{id?}";
}