using System.Collections.Generic;
using System.Linq;

namespace Stevehead.Web.Galleries;

public interface IGalleryComponent
{
    string Name { get; }

    IGallery? Parent { get; }

    string Path { get; }

    IReadOnlyList<IGallery> Ancestors => GetAncestors().ToList();

    private IEnumerable<IGallery> GetAncestors()
    {
        var parent = Parent;

        if (parent != null)
        {
            yield return parent;

            foreach (var a in parent.GetAncestors())
            {
                yield return a;
            }
        }
    }
}
