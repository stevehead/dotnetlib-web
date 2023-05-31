using System.Collections.Generic;
using System.Linq;

namespace Stevehead.Web.QuickWebsite.Galleries;

public abstract class GalleryComponent
{
    internal GalleryComponent(Gallery? parent)
    {
        Parent = parent;
    }

    public abstract string Name { get; }

    public Gallery? Parent { get; }

    public abstract string Path { get; }

    public IReadOnlyList<Gallery> Ancestors => GetAncestors().ToList().AsReadOnly();

    private IEnumerable<Gallery> GetAncestors()
    {
        var parent = Parent;

        while (parent != null)
        {
            yield return parent;
            parent = parent.Parent;
        }
    }
}
