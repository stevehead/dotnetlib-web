using System;

namespace Stevehead.Web.QuickWebsite;

/// <summary>
/// Implementation of the <see cref="IMVCSiteConfiguration"/> interface.
/// </summary>
public sealed class MvcSiteConfiguration : IMVCSiteConfiguration
{
    private string _exceptionHandler;
    private string _defaultControllerRoutePattern;

    /// <summary>
    /// Initializes a new <see cref="MvcSiteConfiguration"/> instance.
    /// </summary>
    public MvcSiteConfiguration() : this(DefaultMVCSiteConfiguration.Instance)
    {
    }

    // Internal copy constructor.
    internal MvcSiteConfiguration(IMVCSiteConfiguration toCopyFrom)
    {
        UseForwardedHeaders = toCopyFrom.UseForwardedHeaders;
        RewriteUrlsToForceTrailingSlash = toCopyFrom.RewriteUrlsToForceTrailingSlash;
        _exceptionHandler = toCopyFrom.ExceptionHandler;
        _defaultControllerRoutePattern = toCopyFrom.DefaultControllerRoutePattern;
        SessionTimeout = toCopyFrom.SessionTimeout;
    }

    public bool UseForwardedHeaders { get; set; }

    public string ExceptionHandler
    {
        get => _exceptionHandler;

        set
        {
            _exceptionHandler = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public bool RewriteUrlsToForceTrailingSlash { get; set; }

    public string DefaultControllerRoutePattern
    {
        get => _defaultControllerRoutePattern;

        set
        {
            _defaultControllerRoutePattern = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public TimeSpan? SessionTimeout { get; set; }
}
