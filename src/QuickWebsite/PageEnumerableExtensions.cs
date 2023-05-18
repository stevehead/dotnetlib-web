using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Stevehead.Web.QuickWebsite;

/// <summary>
/// Extensions for page collections.
/// </summary>
public static class PageEnumerableExtensions
{
    /// <summary>
    /// Tries to get the page that matches a provided URL. Will search descendants.
    /// </summary>
    /// 
    /// <param name="pages">The collection of <see cref="IPage"/> instances to search.</param>
    /// <param name="path">The path to match.</param>
    /// <param name="result">The result if found.</param>
    /// <returns><c>true</c> if found; otherwise, <c>false</c>.</returns>
    public static bool TryGetPage(this IReadOnlyList<IPage> pages, string? path, [MaybeNullWhen(false)] out IPage result)
    {
        foreach (var page in pages)
        {
            if (page.IsMatch(path))
            {
                result = page;
                return true;
            }
        }

        foreach (var page in pages)
        {
            if (TryGetPage(page.Children, path, out result))
            {
                return true;
            }
        }

        result = null;
        return false;
    }
}