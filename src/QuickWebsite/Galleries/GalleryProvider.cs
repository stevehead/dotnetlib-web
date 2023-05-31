using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Stevehead.Web.QuickWebsite.Galleries;

public class GalleryProvider
{
    private readonly DirectoryInfo _directory;

    public GalleryProvider(string directoryPath)
    {
        if (directoryPath == null)
        {
            throw new ArgumentNullException(nameof(directoryPath));
        }

        _directory = new DirectoryInfo(directoryPath);
    }

    public GalleryProvider(DirectoryInfo directory)
    {
        _directory = directory ?? throw new ArgumentNullException(nameof(directory));
    }

    public Gallery Root => new(_directory, null);

    public bool TryGet(string? path, [MaybeNullWhen(false)] out object result)
    {
        path ??= string.Empty;
        path = path.Trim().Trim('/');

        var root = Root;

        if (path.Length == 0)
        {
            result = root;
            return true;
        }

        string[] parts = path.Split('/');
        return root.TryGet(parts, out result);
    }
}
