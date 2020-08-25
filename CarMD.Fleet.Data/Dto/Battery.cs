using CarMD.Fleet.Core.Utility.Extensions;

namespace CarMD.Fleet.Data.Dto
{
    public class Battery
    {
        public int? BatteryStatus { get; set; }
        public decimal? CurrentBatteryVoltage { get; set; }
        public decimal? OriginalBatteryVoltage { get; set; }

        public string GetCurrentBatteryVoltage()
        {
            return CurrentBatteryVoltage.HasValue ? MathExtension.TruncateToDecimalPlace(CurrentBatteryVoltage.Value / 1000, 2).ToString() : "N/A";
        }

        public decimal BatteryVoltagePercent()
        {
            if (CurrentBatteryVoltage.HasValue && OriginalBatteryVoltage.HasValue && OriginalBatteryVoltage.Value > 0)
            {
                return (CurrentBatteryVoltage.Value / OriginalBatteryVoltage.Value) * 100;
            }
            return 0;
        }

        public int BatteryVoltagePercentIcon()
        {
            var icon = (int)((BatteryVoltagePercent() / 25));
            if (icon == 0 && BatteryVoltagePercent() > 0)
            {
                return 1;
            }
            return icon > 3 ? 3 : icon;
        }

        public int BatteryStatusIcon()
        {
            if (CurrentBatteryVoltage.HasValue)
            {
                var vol = MathExtension.TruncateToDecimalPlace(CurrentBatteryVoltage.Value / 1000, 2);

                if (vol > 12)
                {
                    return 4;
                }
                else if (vol > 9)
                {
                    return 3;
                }
                else if (vol > 6)
                {
                    return 2;
                }
                else if (vol > 3)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }

        public string BatteryStatusString()
        {
            if (CurrentBatteryVoltage.HasValue)
            {
                var vol = MathExtension.TruncateToDecimalPlace(CurrentBatteryVoltage.Value / 1000, 2);

                if (vol > 12)
                {
                    return string.Format("Good at {0} volts", vol);
                }
                else
                {
                    return string.Format("Low at {0} volts", vol);
                }
            }
            return "N/A volts";
        }

        public int BatteryMonitorStatus()
        {
            if (CurrentBatteryVoltage.HasValue)
            {
                var vol = MathExtension.TruncateToDecimalPlace(CurrentBatteryVoltage.Value / 1000, 2);

                if (vol >= 12.5m)
                {
                    return 2;
                }
                else if (vol >= 12.1m)
                {
                    return 1;
                }
            }
            return 0;
        }
    }
}
