using System.Diagnostics.CodeAnalysis;

namespace Stevehead.Web.Galleries;

public interface IGalleryProvider
{
    static abstract IGallery Root { get; }

    static abstract bool TryGet(string? path, [MaybeNullWhen(false)] out IGalleryComponent result);
}
