using System;

namespace Stevehead.Web.QuickWebsite;

/// <summary>
/// Configuration for quick MVC websites.
/// </summary>
public interface IMVCSiteConfiguration
{
    /// <summary>
    /// Use forwarded headers. Should be set to <c>true</c> for websites behind a reverse proxy.
    /// </summary>
    bool UseForwardedHeaders { get; }

    /// <summary>
    /// The path to the 500 error page.
    /// </summary>
    string ExceptionHandler { get; }

    /// <summary>
    /// Option to enable URL rewrites to force a trailing slash.
    /// </summary>
    bool RewriteUrlsToForceTrailingSlash { get; }

    /// <summary>
    /// The controller route pattern for the default map.
    /// </summary>
    string DefaultControllerRoutePattern { get; }

    /// <summary>
    /// The length of time a session is allowed to idle. If set to <c>null</c>, sessions are disabled.
    /// </summary>
    TimeSpan? SessionTimeout { get; }
}
