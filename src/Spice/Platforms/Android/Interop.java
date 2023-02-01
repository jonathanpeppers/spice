package spice;

import android.content.Context;
import android.content.res.ColorStateList;
import android.widget.ImageView;

public class Interop {
	public static void setImage(ImageView image, String value)
	{
		Context context = image.getContext();
		int id = context.getResources().getIdentifier(value, "drawable", context.getPackageName());
		if (id != 0) {
			image.setImageResource(id);
		}
	}

	// TODO: @NonNull
	public static ColorStateList getDefaultColorStateList(int color)
	{
		return new ColorStateList(ColorStates.DEFAULT, new int[] { color });
	}

	private static class ColorStates
	{
		static final int[] EMPTY = new int[] { };
		static final int[][] DEFAULT = new int[][] { EMPTY };
	}
}