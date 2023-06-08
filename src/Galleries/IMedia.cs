using System;
using System.IO;

namespace Stevehead.Web.Galleries;

public interface IMedia : IGalleryComponent
{
    DateTimeOffset CreatedAt { get; }

    FileInfo? File { get; }

    bool FileExists => File?.Exists ?? false;

    GeoCoordinate? GPSCoordinates { get; }

    string Id { get; }

    string MIMEType { get; }

    FileInfo? Thumbnail { get; }

    bool IsImage => MIMEType.StartsWith("image");

    bool IsVideo => MIMEType.StartsWith("video");
}
