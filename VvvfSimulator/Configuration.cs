namespace VvvfSimulator.Configuration
{
    public class RealTimeConfig
    {
        public const bool LimitMaxOutputFrequency = false;
        public const double MaxOutputFrequency = 180.0; // Hz

        public const bool LimitMaxFrequencyChangeRate = false;
        public const double MaxFrequencyChangeRate = 10.0; // Hz/s

        public const bool LimitWaveformCalculation = false;
        public const double WaveformCalculationFrequency = 10.0; // Hz

        // public const bool LimitOvervoltage = false;
        // public const double MaxVoltagePerFrequencyRate = ; // 
    }
}
