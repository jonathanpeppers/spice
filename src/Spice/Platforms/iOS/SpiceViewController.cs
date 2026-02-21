namespace Spice;

/// <summary>
/// A UIViewController that detects system appearance (dark/light mode) changes
/// and notifies <see cref="PlatformAppearance"/>.
/// </summary>
class SpiceViewController : UIViewController
{
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		if (OperatingSystem.IsIOSVersionAtLeast(17))
		{
			RegisterForTraitChanges([typeof(UITraitUserInterfaceStyle)], (IUITraitEnvironment _, UITraitCollection _) =>
			{
				PlatformAppearance.OnChanged(PlatformAppearance.IsDarkMode);
			});
		}
	}

#pragma warning disable CA1422 // Validate platform compatibility (fallback for iOS 16)
	public override void TraitCollectionDidChange(UITraitCollection? previousTraitCollection)
	{
		base.TraitCollectionDidChange(previousTraitCollection);

		if (!OperatingSystem.IsIOSVersionAtLeast(17) &&
			previousTraitCollection?.UserInterfaceStyle != TraitCollection.UserInterfaceStyle)
		{
			PlatformAppearance.OnChanged(PlatformAppearance.IsDarkMode);
		}
	}
#pragma warning restore CA1422
}
