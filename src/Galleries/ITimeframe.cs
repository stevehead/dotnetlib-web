using System;

namespace Stevehead.Web.Galleries;

public interface ITimeframe
{
    string Name { get; }

    DateTimeOffset StartAt { get; }

    DateTimeOffset EndAt { get; }
}
