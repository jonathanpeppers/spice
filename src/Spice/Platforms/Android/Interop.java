package spice;

import android.content.res.ColorStateList;

public class Interop {
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