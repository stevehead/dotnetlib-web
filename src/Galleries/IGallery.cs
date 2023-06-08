using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Stevehead.Web.Galleries;

public interface IGallery : IGalleryComponent
{
    IReadOnlyList<IGallery> Galleries { get; }

    IReadOnlyList<IMedia> Media { get; }

    string? ThumbnailId { get; }

    IReadOnlyList<ITimeframe> Timeframes { get; }

    bool TryGetFirstImage([MaybeNullWhen(false)] out string image)
    {
        foreach (var media in Media)
        {
            if (media.IsImage)
            {
                image = media.Id;
                return true;
            }
        }

        foreach (var child in Galleries)
        {
            if (child.ThumbnailId != null)
            {
                image = child.ThumbnailId;
                return true;
            }

            if (child.TryGetFirstImage(out image))
            {
                return true;
            }
        }

        image = null;
        return false;
    }
}
