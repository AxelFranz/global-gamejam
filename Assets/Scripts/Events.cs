using System;
using UnityEngine.UIElements;

public static class Events
{
    public static Action<MaskState> MaskChanged;
    public static Action<MaskState> PlayerDetected;
    public static Action PlayerLoaded;
    public static Action MenuOpened;
    public static Action MenuClosed;
    public static Action Win;
    public static Action Die;
}
