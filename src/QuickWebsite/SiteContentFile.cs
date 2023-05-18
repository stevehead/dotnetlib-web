using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Stevehead.Web.QuickWebsite;

/// <summary>
/// Represents the full JSON of the site content file.
/// </summary>
/// 
/// <typeparam name="TSiteConstants">The site constants implementation.</typeparam>
/// <typeparam name="TPage">The page implemention.</typeparam>
/// <typeparam name="TISiteConstants">The site constants interface.</typeparam>
public sealed class SiteContentFile<TSiteConstants, TPage, TISiteConstants> : ISiteContentProvider<TISiteConstants>
    where TSiteConstants : TISiteConstants
    where TPage : Page
{
    public SiteContentFile() { }

    [JsonPropertyName("constants")]
    public TSiteConstants Constants { get; set; } = default!;

    [JsonPropertyName("pages")]
    public IReadOnlyList<TPage> Pages { get; set; } = Array.Empty<TPage>();

    TISiteConstants ISiteContentProvider<TISiteConstants>.Constants => Constants;

    IReadOnlyList<IPage> ISiteContentProvider<TISiteConstants>.Pages => Pages;
}
