namespace Assimalign.Extensions.FileSystemGlobbing.PathSegments;

public class FilePathCurrentSegment : IFilePathSegment
{
    public bool CanProduceStem { get { return false; } }

    public bool Match(string value)
    {
        return false;
    }
}
