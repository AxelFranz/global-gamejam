using UnityEngine;

public static class Utils
{
    public static Color MaskStateToColor(MaskState state)
    {
        switch (state)
        {
            case MaskState.Fire:
                return Color.red;
            case MaskState.Water:
                return Color.blue;
            case MaskState.Plant:
                return Color.green;
            case MaskState.Unmasked:
                return Color.grey;
            default:
                return Color.grey;
        }
    }

}
