using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Stevehead.Web.QuickWebsite.Galleries;

public sealed class Gallery : GalleryComponent
{
    internal const string SubdivisionsFileName = "subdivisions.dat";
    private readonly DirectoryInfo _directory;
    private readonly Lazy<IReadOnlyList<Subdivision>> l_subdivisions;

    internal Gallery(DirectoryInfo directory, Gallery? parent) : base(parent)
    {
        _directory = directory;
        l_subdivisions = new(GetSubdivisions);
    }

    public IReadOnlyList<GalleryComponent> Children => GetGalleries().Concat<GalleryComponent>(GetMedia()).ToList().AsReadOnly();

    public string DisplayName
    {
        get
        {
            if (Parent == null)
            {
                return "Home";
            }

            string name = _directory.Name;
            int dotIndex = name.IndexOf('.');
            return name[(dotIndex + 1)..].Trim();
        }
    }

    public IReadOnlyList<Gallery> Galleries => GetGalleries().ToList().AsReadOnly();

    public IReadOnlyList<MediaObject> Media => GetMedia().ToList().AsReadOnly();

    public override string Name
    {
        get
        {
            if (Parent == null)
            {
                return string.Empty;
            }

            string name = _directory.Name;
            int dotIndex = name.IndexOf('.');
            return name[..dotIndex].Trim();
        }
    }

    public override string Path
    {
        get
        {
            if (Parent == null)
            {
                return string.Empty;
            }

            if (Parent.Parent == null)
            {
                return Name;
            }

            return $"{Parent.Path}/{Name}";
        }
    }

    public bool TryGetFirstImage([MaybeNullWhen(false)] out MediaObject image)
    {
        var img = GetMedia().Where(i => i.IsImage).FirstOrDefault();

        if (img != null)
        {
            image = img;
            return true;
        }

        foreach (var gallery in GetGalleries())
        {
            if (gallery.TryGetFirstImage(out image))
            {
                return true;
            }
        }

        image = default;
        return false;
    }

    private IEnumerable<MediaObject> GetAllMedia()
    {
        foreach (var media in GetMedia())
        {
            yield return media;
        }

        foreach (var gallery in GetGalleries())
        {
            foreach (var media in gallery.GetAllMedia())
            {
                yield return media;
            }
        }
    }

    private IEnumerable<Gallery> GetGalleries() => _directory.GetDirectories().Where(d =>
    {
        string name = d.Name;

        if (name.StartsWith("file__"))
        {
            return false;
        }

        int dotIndex = name.IndexOf('.');

        if (dotIndex > 0)
        {
            return uint.TryParse(name[0..dotIndex], out _);
        }

        return false;
    }).OrderBy(d =>
    {
        string name = d.Name;
        int dotIndex = name.IndexOf('.');
        return uint.Parse(name[0..dotIndex]);
    }).Select(d => new Gallery(d, this));

    private IEnumerable<MediaObject> GetMedia() => _directory.GetDirectories().Select(d =>
    {
        return MediaObject.Create(d, this);
    }).Where(m => m != null)!;

    internal bool TryGet(ReadOnlySpan<string> paths, [MaybeNullWhen(false)] out object result)
    {
        if (paths.Length == 1 || (paths.Length == 2 && paths[1] == "thumb"))
        {
            foreach (var media in GetMedia())
            {
                if (media.Name == paths[0])
                {
                    result = paths.Length == 2 ? media.ThumbnailFile : media.File;
                    return result != null;
                }
            }

            if (paths.Length == 1)
            {
                foreach (var childGallery in GetGalleries())
                {
                    if (childGallery.Name == paths[0])
                    {
                        result = childGallery;
                        return true;
                    }
                }
            }
        }
        else
        {
            foreach (var childGallery in GetGalleries())
            {
                if (childGallery.Name == paths[0])
                {
                    return childGallery.TryGet(paths[1..], out result);
                }
            }
        }

        result = default;
        return false;
    }

    internal bool IsStartOfSubdivision(string fileName, [MaybeNullWhen(false)] out string subdivisionName)
    {
        foreach (var subdiv in l_subdivisions.Value)
        {
            if (subdiv.FirstItem == fileName)
            {
                subdivisionName = subdiv.Name;
                return true;
            }
        }

        subdivisionName = null;
        return false;
    }

    internal bool IsEndOfSubdivision(string fileName, [MaybeNullWhen(false)] out string subdivisionName)
    {
        foreach (var subdiv in l_subdivisions.Value)
        {
            if (subdiv.LastItem == fileName)
            {
                subdivisionName = subdiv.Name;
                return true;
            }
        }

        subdivisionName = null;
        return false;
    }

    private IReadOnlyList<Subdivision> GetSubdivisions()
    {
        List<Subdivision> subdivisions = new();

        string filePath = System.IO.Path.Combine(_directory.FullName, SubdivisionsFileName);

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            string[] values = new string[3];
            int i = 0;

            foreach (var rawLine in lines)
            {
                var line = rawLine.Trim();

                if (line.Length == 0) continue;

                values[i++] = line;

                if (i == 3)
                {
                    subdivisions.Add(new()
                    {
                        Name = values[0],
                        FirstItem = values[1],
                        LastItem = values[2]
                    });

                    i = 0;
                    Array.Clear(values);
                }
            }
        }

        return subdivisions.AsReadOnly();
    }

    private sealed class Subdivision
    {
        public required string Name { get; init; }

        public required string FirstItem { get; init; }

        public required string LastItem { get; init; }
    }
}
