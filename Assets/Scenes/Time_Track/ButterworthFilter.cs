public class ButterworthFilter
{
    private readonly float[] a;
    private readonly float[] b;
    private readonly float[] x;
    private readonly float[] y;

    public ButterworthFilter(float fs, float f1, float f2)
    {
        float nyq = fs / 2f;
        float low = f1 / nyq;
        float high = f2 / nyq;

        // 간단한 4차 IIR 필터 파라미터 (Biquad 등은 SciPy 수준에 비해 단순화됨)
        a = new float[] { 1f, -1.143f, 0.4128f };
        b = new float[] { 0.0675f, 0.1349f, 0.0675f };

        x = new float[2];
        y = new float[2];
    }

    public float[] Apply(float[] input)
    {
        float[] output = new float[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            float newY = b[0] * input[i] + b[1] * x[0] + b[2] * x[1]
                         - a[1] * y[0] - a[2] * y[1];

            output[i] = newY;

            // Shift
            x[1] = x[0];
            x[0] = input[i];
            y[1] = y[0];
            y[0] = newY;
        }

        return output;
    }
}
