using System;

public static class Events
{
    public static Action<MaskState> MaskChanged;
    public static Action<MaskState> PlayerDetected;
    public static Action PlayerLoaded;
}
