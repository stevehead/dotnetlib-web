using System;
using System.Collections.Generic;

namespace Stevehead.Web.QuickWebsite;

/// <summary>
/// Represents a single page on the website.
/// </summary>
public interface IPage
{
    /// <summary>
    /// Gets the value of the provided component.
    /// </summary>
    /// 
    /// <param name="componentName">The name of the component.</param>
    /// <returns>The value of the component if it exists; otherwise, an empty string.</returns>
    string this[string? componentName] { get; }

    /// <summary>
    /// The title of the page.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// The url slug of the page.
    /// </summary>
    string Slug { get; }

    /// <summary>
    /// Determines if this page is hidden from navigation.
    /// </summary>
    bool IsHidden { get; }

    /// <summary>
    /// Determines if this page is the home page.
    /// </summary>
    bool IsHome { get; }

    /// <summary>
    /// The parent page.
    /// </summary>
    IPage? Parent { get; }

    /// <summary>
    /// Gets all the children that are not hidden.
    /// </summary>
    IEnumerable<IPage> NonHiddenChildren { get; }

    /// <summary>
    /// The page's children.
    /// </summary>
    IReadOnlyList<IPage> Children { get; }

    /// <summary>
    /// The file path to the view for this page.
    /// </summary>
    string View { get; }

    /// <summary>
    /// The full url of the page.
    /// </summary>
    string Url { get; }

    /// <summary>
    /// The date the page was last modified.
    /// </summary>
    DateOnly? LastModified { get; }

    /// <summary>
    /// Determines if the provided URL matches this page's url.
    /// </summary>
    /// 
    /// <param name="url">The url to match against.</param>
    /// <returns><c>true</c> if the URLs match; otherwise, <c>false</c>.</returns>
    bool IsMatch(string? url);
}
