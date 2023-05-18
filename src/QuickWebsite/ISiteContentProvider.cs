using System.Collections.Generic;

namespace Stevehead.Web.QuickWebsite;

/// <summary>
/// Represents the website's content.
/// </summary>
/// 
/// <typeparam name="TISiteConstants">The site constants interface.</typeparam>
public interface ISiteContentProvider<TISiteConstants>
{
    /// <summary>
    /// Gets the site constants.
    /// </summary>
    TISiteConstants Constants { get; }

    /// <summary>
    /// Gets the site's pages.
    /// </summary>
    IReadOnlyList<IPage> Pages { get; }
}
