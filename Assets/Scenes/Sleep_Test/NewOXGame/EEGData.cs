using System;

[Serializable]
public class EEGData
{
    public int attention;
    public int meditation;
    public int blink;
    public int delta, theta, lowAlpha, highAlpha;
    public int lowBeta, highBeta, lowGamma, highGamma;
}
