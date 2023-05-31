using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Stevehead.Web.QuickWebsite.Galleries;

public sealed class MediaObject : GalleryComponent
{
    private static readonly string[] ImageExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
    private static readonly string[] VideoExtensions = new string[] { ".mp4", ".avi", ".mov" };

    private readonly DirectoryInfo _directory;

    internal MediaObject(DirectoryInfo directory, Gallery parent) : base(parent)
    {
        _directory = directory;
    }

    public new Gallery Parent => base.Parent!;

    public bool IsVideo
    {
        get
        {
            string dirName = _directory.Name.ToLowerInvariant();

            foreach (var ext in VideoExtensions)
            {
                if (dirName.EndsWith(ext))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public bool IsImage
    {
        get
        {
            string dirName = _directory.Name.ToLowerInvariant();

            foreach (var ext in ImageExtensions)
            {
                if (dirName.EndsWith(ext))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public sealed override string Name
    {
        get
        {
            string dirName = _directory.Name;
            Debug.Assert(dirName.StartsWith("file__"));
            return dirName[6..];
        }
    }

    public FileInfo? File
    {
        get
        {
            foreach (var file in _directory.GetFiles())
            {
                if (file.Name.StartsWith("main"))
                {
                    return file;
                }
            }

            return null;
        }
    }

    public sealed override string Path
    {
        get
        {
            if (Parent.Parent == null)
            {
                return Name;
            }

            return $"{Parent.Path}/{Name}";
        }
    }

    public FileInfo? ThumbnailFile
    {
        get
        {
            foreach (var file in _directory.GetFiles())
            {
                if (file.Name.StartsWith("thumb"))
                {
                    return file;
                }
            }

            return null;
        }
    }

    public string ThumbnailPath => Path + "/thumb";

    internal static MediaObject? Create(DirectoryInfo directory, Gallery parent)
    {
        string directoryName = directory.Name.ToLowerInvariant();

        if (directoryName.StartsWith("file__"))
        {
            var mediaObject = new MediaObject(directory, parent);

            if (mediaObject.IsVideo || mediaObject.IsImage)
            {
                return mediaObject;
            }
        }

        return null;
    }

    public bool IsStartOfSubdivision([MaybeNullWhen(false)] out string subdivisionName)
    {
        string fileName = _directory.Name[6..];
        return Parent.IsStartOfSubdivision(fileName, out subdivisionName);
    }

    public bool IsEndOfSubdivision([MaybeNullWhen(false)] out string subdivisionName)
    {
        string fileName = _directory.Name[6..];
        return Parent.IsEndOfSubdivision(fileName, out subdivisionName);
    }
}