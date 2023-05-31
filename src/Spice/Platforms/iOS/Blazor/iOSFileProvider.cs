// From: https://github.com/dotnet/maui/blob/c8a6093ee805388732af8d75b437b099a301db22/src/BlazorWebView/src/Maui/iOS/iOSMauiAssetFileProvider.cs
using Foundation;
using System.Collections;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Spice;

/// <summary>
/// A minimal implementation of an IFileProvider to be used by the BlazorWebView and WebViewManager types.
/// </summary>
internal sealed class iOSFileProvider : IFileProvider
{
	private readonly string _bundleRootDir;

	public iOSFileProvider(string contentRootDir)
	{
		_bundleRootDir = Path.Combine(NSBundle.MainBundle.ResourcePath, contentRootDir);
	}

	public IDirectoryContents GetDirectoryContents(string subpath)
		=> new iOSMauiAssetDirectoryContents(Path.Combine(_bundleRootDir, subpath));

	public IFileInfo GetFileInfo(string subpath)
		=> new iOSMauiAssetFileInfo(Path.Combine(_bundleRootDir, subpath));

	public IChangeToken Watch(string filter)
		=> NullChangeToken.Singleton;

	private sealed class iOSMauiAssetFileInfo : IFileInfo
	{
		private readonly string _filePath;

		public iOSMauiAssetFileInfo(string filePath)
		{
			_filePath = filePath;

			Name = Path.GetFileName(_filePath);

			var fileInfo = new FileInfo(_filePath);
			Exists = fileInfo.Exists;
			Length = Exists ? fileInfo.Length : -1;
		}

		public bool Exists { get; }
		public long Length { get; }
		public string PhysicalPath { get; } = null!;
		public string Name { get; }
		public DateTimeOffset LastModified { get; } = DateTimeOffset.FromUnixTimeSeconds(0);
		public bool IsDirectory => false;

		public Stream CreateReadStream()
			=> File.OpenRead(_filePath);
	}

	// This is never used by BlazorWebView or WebViewManager
	private sealed class iOSMauiAssetDirectoryContents : IDirectoryContents
	{
		public iOSMauiAssetDirectoryContents(string filePath)
		{
		}

		public bool Exists => false;

		public IEnumerator<IFileInfo> GetEnumerator()
			=> throw new NotImplementedException();

		IEnumerator IEnumerable.GetEnumerator()
			=> throw new NotImplementedException();
	}
}