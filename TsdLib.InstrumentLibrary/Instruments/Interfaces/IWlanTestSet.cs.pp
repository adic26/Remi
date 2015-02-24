using System;

namespace $rootnamespace$.Instruments
{
    public interface IWlanTestSet
    {
        void InitializeVsg(int portNumber);
        void VsgConfigureSignal(String band, Int32 channel, Int32 bandwidth);
        void VsgSetDownlinkPowerLevel(Double powerLevel);
        void VsgSetSamplingRate(Double rate);
        void VsgGenerateCw(Double frequency, Double power);
        void VsgPlayWaveform(String waveFile, Int32 count);
        void VsgStopWaveform();
        void DisableVsg();
        void InitializeVsa(Int32 portNumber);
        void VsaConfigureSignal(String band, Int32 channel, Int32 bandwidth, Double userMargin, Double expectedNominalPower, String modulation);
        Double VsaGetCenterFrequency();
        Double VsaMeasureUplinkPower(Double captureLength);
        Double VsaMeasureEvmDsss();
        Double VsaMeasureFreqErrorDsss();
        Double VsaMeasureEvmOfdm();
        Double VsaMeasureFreqErrorOfdm();
        Int32 VsgGetWaveformStatus();
    }
}