using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Stevehead.Web.QuickWebsite;

/// <summary>
/// Default JSON representation of a website's page.
/// </summary>
public class Page : IPage
{
    private IReadOnlyList<Page> _children = Array.Empty<Page>();

    /// <summary>
    /// Initializes a new <see cref="Page"/>.
    /// </summary>
    public Page()
    {
        View = DefaultView;
    }

    #region JSON Properties
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("view")]
    public string View { get; set; }

    [JsonPropertyName("is-hidden")]
    public bool IsHidden { get; set; } = false;

    [JsonPropertyName("components")]
    public IReadOnlyDictionary<string, string>? Components { get; set; }

    [JsonPropertyName("children")]
    public IReadOnlyList<Page> Children
    {
        get => _children;

        set
        {
            foreach (var child in value)
            {
                child.Parent = this;
            }

            _children = value;
        }
    }
    #endregion

    public string this[string? componentName]
    {
        get
        {
            if (componentName != null && Components != null && Components.TryGetValue(componentName, out string? value) && value != null)
            {
                return value;
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// The default view when View is not set.
    /// </summary>
    [JsonIgnore]
    protected virtual string DefaultView => "Views/Public/Content.cshtml";

    [JsonIgnore]
    public Page? Parent { get; set; }

    [JsonIgnore]
    public IEnumerable<IPage> NonHiddenChildren
    {
        get
        {
            foreach (var child in Children)
            {
                if (!child.IsHidden)
                {
                    yield return child;
                }
            }
        }
    }

    [JsonIgnore]
    public string Url
    {
        get
        {
            string slug = Slug;
            IPage? parent = Parent;

            if (parent == null)
            {
                if (slug == string.Empty)
                {
                    return "/";
                }

                return $"/{slug}/";
            }

            return $"{parent.Url}{slug}/";
        }
    }

    public bool IsMatch(string? url)
    {
        if (string.IsNullOrEmpty(url))
        {
            url = "/";
        }
        else if (url != "/")
        {
            url = url.Trim('/');
            url = $"/{url}/";
        }

        return Url == url;
    }

    IPage? IPage.Parent => Parent;

    IReadOnlyList<IPage> IPage.Children => _children;
}
