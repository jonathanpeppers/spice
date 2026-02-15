namespace Spice.Tests;

/// <summary>
/// Simple test ViewModel for binding tests.
/// Implements INotifyPropertyChanged manually to avoid requiring CommunityToolkit.Mvvm
/// source generators in the test project.
/// </summary>
internal class TestViewModel : System.ComponentModel.INotifyPropertyChanged
{
	public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;

	private string _title = string.Empty;
	public string Title
	{
		get => _title;
		set { _title = value; PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Title))); }
	}

	private int _count;
	public int Count
	{
		get => _count;
		set { _count = value; PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Count))); }
	}

	private bool _isEnabled;
	public bool IsEnabled
	{
		get => _isEnabled;
		set { _isEnabled = value; PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(IsEnabled))); }
	}
}
