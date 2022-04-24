using System;
using System.IO;
using System.Linq;

namespace Assimalign.Extensions.Primitives;

/// <summary>
/// A simple case-insensitive file path
/// </summary>
/// <remarks>
/// This type is useful when working with File Systems on OS's such as 
/// Linux, iOS, MacOS being as the system is case-sensitive.
/// </remarks>
public record FilePath
{
	/// <summary>
	/// 
	/// </summary>
	public FilePath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			throw new ArgumentException("path cannot be null or empty");
		}
		else if (System.IO.Path.GetInvalidPathChars().Intersect(path).Any())
		{
			throw new ArgumentException("path contains illegal characters");
		}
		else
		{
			Path = System.IO.Path.GetFullPath(path.Trim());
		}
	}


	public string Path { get; }

	public override string ToString() => Path;
	public virtual bool Equals(FilePath? other) => (Path).Equals(other?.Path, StringComparison.InvariantCultureIgnoreCase);

	public static implicit operator FilePath(string name) => new FilePath(name);
	public static implicit operator string(FilePath? other) => other?.Path;

	public FileInfo GetFileInfo() => new FileInfo(Path);
	public FilePath Combine(params string[] paths) => System.IO.Path.Combine(paths.Prepend(Path).ToArray());

}
