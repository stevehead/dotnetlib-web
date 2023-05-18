using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Stevehead.Web.QuickWebsite;

/// <summary>
/// The cached site content of the website.
/// </summary>
/// 
/// <typeparam name="TSiteConstants">The site constants implementation.</typeparam>
/// <typeparam name="TPage">The page implemention.</typeparam>
/// <typeparam name="TISiteConstants">The site constants interface.</typeparam>
public sealed class SiteContent<TSiteConstants, TPage, TISiteConstants> : ISiteContentProvider<TISiteConstants>
    where TSiteConstants : TISiteConstants
    where TPage : Page
{
    private readonly CachedObject<ISiteContentProvider<TISiteConstants>> _cachedSiteContentFile;

    /// <summary>
    /// Initializes a new <see cref="SiteContent{TSiteConstants, TPage, TISiteConstants}"/>.
    /// </summary>
    /// 
    /// <param name="siteContentFileName">The location of the site content file.</param>
    /// <param name="siteContentFileTTL">How long the content is to be cached.</param>
    /// <exception cref="ArgumentNullException"><paramref name="siteContentFileName"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="siteContentFileTTL"/> is less than <see cref="TimeSpan.Zero"/></exception>
    public SiteContent(string siteContentFileName, TimeSpan siteContentFileTTL)
    {
        if (siteContentFileName == null)
        {
            throw new ArgumentNullException(nameof(siteContentFileName));
        }

        if (siteContentFileTTL < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(siteContentFileTTL));
        }

        _cachedSiteContentFile = new(siteContentFileTTL, () =>
        {
            string json = File.ReadAllText(siteContentFileName);
            return JsonSerializer.Deserialize<SiteContentFile<TSiteConstants, TPage, TISiteConstants>>(json)!;
        });
    }

    public TISiteConstants Constants => _cachedSiteContentFile.Value.Constants;

    public IReadOnlyList<IPage> Pages => _cachedSiteContentFile.Value.Pages;
}