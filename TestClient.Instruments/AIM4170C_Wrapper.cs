using System;
using System.Linq;
using System.Numerics;
using TestClient.Instruments;

namespace TsdLib.Instrument
{
    public class AIM4170C_Wrapper
    {
        const double CharacteristicResistance = 100.9;

        private readonly AIM4170C _aim4170C;

        public AIM4170C_Wrapper(AIM4170C aim4170C)
        {
            _aim4170C = aim4170C;
        }

        public Complex MeasureImpedance(double frequency)
        {
            try
            {
                double u = frequency / 400d * 4294967296d + 0.5;
                int k = (int)u;
                string freqString = k.ToString("X8");

                _aim4170C.CloseRelay();
                byte[] rawMeasurementData = _aim4170C.Measure(freqString);
                _aim4170C.OpenRelay();

                if (rawMeasurementData.Count() != 72)
                    throw new InstrumentException("Invalid data received from AIM device. Measurement data must be 72 bytes.");

                int checkSum = 0;
                for (int i = 0; i < 70; i += 2)
                    checkSum += rawMeasurementData.ElementAt(i) * 256 + rawMeasurementData.ElementAt(i + 1);
                checkSum = checkSum & 0xFFFF;
                if (checkSum != rawMeasurementData.ElementAt(70) * 256 + rawMeasurementData.ElementAt(71))
                    throw new InstrumentException("Invalid data received from AIM device. Checksum failure.");

                string freqStr = string.Join("", rawMeasurementData.Take(4).Select(b => b.ToString("X2")));
                double rawFreq = Convert.ToInt32(freqStr, 16);

                double receivedFrequency = (rawFreq - 0.5) * 400d / 4294967296d;

                Complex current = Dft(rawMeasurementData.Skip(4).Take(32).ToArray());
                Complex voltage = Dft(rawMeasurementData.Skip(36).Take(32).ToArray());

                Complex characteristicImpedance = new Complex(CharacteristicResistance, 0);
                Complex impedance = (voltage / current) * CharacteristicResistance;

// ReSharper disable once EqualExpressionComparison - this is a way to check for NaN
                if (impedance != impedance)
                    throw new InstrumentException("Invalid data received from AIM device. NaN (Not a Number) received.");

                return impedance;
            }
            catch
            {
                _aim4170C.OpenRelay();
                throw;
            }

        }

        public Complex Dft(byte[] waveform)
        {
            Complex result = new Complex();

            for (int i = 0; i < waveform.Length; i += 2)
            {
                double rootOfUnity = 2 * Math.PI * i / waveform.Length;
                double sample = waveform.ElementAt(i) * 256 + waveform.ElementAt(i + 1);

                double real = sample * Math.Cos(rootOfUnity) / (waveform.Length / 2);
                double imaginary = sample * Math.Sin(rootOfUnity) / (waveform.Length / 2);

                result += new Complex(real, imaginary);
            }
            return result;
        }
    }
}