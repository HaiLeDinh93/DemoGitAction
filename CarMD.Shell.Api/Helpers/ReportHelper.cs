using Innova.Utilities.Shared.Enums;
using Innova.Utilities.Shared.Tool;
using CarMD.Fleet.Core.Utility;
using Innova.Utilities.Shared.Model.OBD2;
using Innova2.VehicleDataLib.Enums.Device;
using Innova2.VehicleDataLib.Enums.Version5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Data.Request.Api;

namespace CarMD.Shell.Api.Helpers
{
    public class ReportHelper
    {
        private const byte ValidStatus = 0xAA;

        public static string GetRawData(VehicleInnovaDataModel model)
        {
            var buffer = BuildVehicleDataBuffer(model);
            var rawData = new Innova2.VehicleDataLib.Entities.Version5.RawData
            {
                Language = Innova2.VehicleDataLib.Enums.Common.Language.English,
                SoftwareVersion = new Version(5, 0, 0),
                VehicleRaw = buffer,
                SystemInfoRaw = string.Empty,
                ProductId = model.UsbProductId == null ? (UsbProductId)720 : (UsbProductId)model.UsbProductId,
            };

            return rawData.ToBase64String();
        }

        private static string BuildVehicleDataBuffer(VehicleInnovaDataModel model)
        {
            List<byte> vehicleDataBuffer = new List<byte>();

            if (!string.IsNullOrEmpty(model.VinProfileRaw))
            {
                AppendBufferSegment(BufferModules.None, BufferSegmentTypes.VinProfile, model.VinProfileRaw, vehicleDataBuffer);
            }
            else
            {
                var vinProfileBuffer = BuildVinProfileBuffer(model.Vin);
                AppendBufferSegment(BufferModules.None, BufferSegmentTypes.VinProfile, vinProfileBuffer, vehicleDataBuffer);
            }
            if (!string.IsNullOrWhiteSpace(model.MonitorStatusTcmRaw)
                && model.MonitorStatusTcmRaw.Equals("AAAAAAAAAAAA", StringComparison.OrdinalIgnoreCase))
                model.MonitorStatusTcmRaw = string.Empty;

            AppendBufferSegment(BufferModules.ECM, BufferSegmentTypes.MonitorStatus, model.MonitorStatusEcmRaw, vehicleDataBuffer);
            AppendBufferSegment(BufferModules.TCM, BufferSegmentTypes.MonitorStatus, model.MonitorStatusTcmRaw, vehicleDataBuffer);

            AppendBufferSegment(BufferModules.ECM, BufferSegmentTypes.FreezeFrame, model.FreezeFrameEcmRaw, vehicleDataBuffer);
            AppendBufferSegment(BufferModules.TCM, BufferSegmentTypes.FreezeFrame, model.FreezeFrameTcmRaw, vehicleDataBuffer);

            AppendBufferSegment(BufferModules.ECM, BufferSegmentTypes.Dtc, model.DtcsEcmRaw, vehicleDataBuffer);
            AppendBufferSegment(BufferModules.TCM, BufferSegmentTypes.Dtc, model.DtcsTcmRaw, vehicleDataBuffer);


            AppendBufferSegment(BufferModules.ECM, BufferSegmentTypes.VehicleInfo, model.VehicleInfoEcmRaw, vehicleDataBuffer);
            AppendBufferSegment(BufferModules.TCM, BufferSegmentTypes.VehicleInfo, model.VehicleInfoTcmRaw, vehicleDataBuffer);

            //AppendBufferSegment(BufferModules.None, BufferSegmentTypes.LiveData, model.LiveDataRaw, vehicleDataBuffer);
            //AppendBufferSegment(BufferModules.None, BufferSegmentTypes.OemData, model.OemModuleRaw, vehicleDataBuffer);

            return Convert.ToBase64String(vehicleDataBuffer.ToArray());
        }

        private static void AppendBufferSegment(BufferModules module, BufferSegmentTypes type, string raw, List<byte> dataBuffer)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return;
            var rawBytes = Convert.FromBase64String(raw).ToList();
            if (type == BufferSegmentTypes.FreezeFrame)
            {
                // FF: 2227 bytes
                var freezeFrameBuffer = VehicleParser.GetFreezeFrameBuffer(rawBytes.ToArray());
                if (freezeFrameBuffer == null)
                    return;
                rawBytes = freezeFrameBuffer.ToList();
            }

            var bytes = new List<byte>();
            bytes.Add((byte)module);

            bytes.AddRange(ConvertIntToBytes((int)type));

            var targets = new List<byte>();
            targets.AddRange(rawBytes);

            bytes.AddRange(ConvertLongToBytes(targets.Count()));

            bytes.AddRange(targets);

            dataBuffer.AddRange(bytes);
        }

        private static void AppendBufferSegment(BufferModules module, BufferSegmentTypes type, byte[] rawBytes, List<byte> dataBuffer)
        {
            if (rawBytes == null)
                return;
            var bytes = new List<byte>();
            bytes.Add((byte)module);

            bytes.AddRange(ConvertIntToBytes((int)type));

            var targets = new List<byte>();
            targets.AddRange(rawBytes);

            bytes.AddRange(ConvertLongToBytes(targets.Count()));

            bytes.AddRange(targets);

            dataBuffer.AddRange(bytes);
        }

        public static byte[] BuildVinProfileBuffer(string vin)
        {
            var vinBuffer = new byte[17];
            vinBuffer = Encoding.UTF8.GetBytes(vin);

            var vinProfileBuffer = new byte[512];
            vinProfileBuffer[0] = ValidStatus;

            if (!string.IsNullOrWhiteSpace(vin))
            {
                Array.Copy(vinBuffer, 0, vinProfileBuffer, 1, vinBuffer.Length);
            }

            vinProfileBuffer[19] = 6;
            vinProfileBuffer[21] = 28;
            vinProfileBuffer[23] = 17;
            vinProfileBuffer[25] = 103;
            vinProfileBuffer[27] = 255;
            vinProfileBuffer[28] = 255;
            vinProfileBuffer[29] = 255;
            vinProfileBuffer[30] = 255;
            vinProfileBuffer[31] = 8;
            vinProfileBuffer[33] = 2;
            vinProfileBuffer[35] = 7;

            return vinProfileBuffer;
        }

        private static IEnumerable<byte> ConvertIntToBytes(int length)
        {
            byte[] bytes = new byte[2];
            bytes[0] = (byte)((length >> 8) & 0xFF);
            bytes[1] = (byte)(length & 0xFF);
            return bytes.Reverse().ToList();
        }

        private static IEnumerable<byte> ConvertLongToBytes(long length)
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)((length >> 24) & 0xFF);
            bytes[1] = (byte)((length >> 16) & 0xFF);
            bytes[2] = (byte)((length >> 8) & 0xFF);
            bytes[3] = (byte)(length & 0xFF);
            return bytes.Reverse().ToList();
        }

        public static int GetMonitorStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return 1;// Not Supported
            }
            switch (status.Trim().ToLower())
            {
                case "notsupported":
                    return 1;
                case "notcomplete":
                    return 2;
                case "complete":
                    return 3;
            }
            return 1;
        }

        public static string GetMilStatus(List<MonitorInfo> monitors)
        {
            var result = Innova.Utilities.Shared.Tool.AdditionDiagnosticData.GetValueByKey(
                        OBD2Strings.pidstrings_eng[(int)OBD2Strings.pidindex._PID01g], monitors);

            if (string.Equals("Not Supported", result, StringComparison.OrdinalIgnoreCase) && monitors != null)
            {
                var monitor = monitors.Where(m => m.Description.Equals("MIL", StringComparison.OrdinalIgnoreCase)
                    || m.Description.Equals("Malfunction Indicator Lamp (MIL)", StringComparison.OrdinalIgnoreCase)
                    || m.Description.Equals("Malfunction Indicator Lamb (MIL)", StringComparison.OrdinalIgnoreCase)
                    || m.Description.Equals("MIL Check Engine Light", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (monitor != null)
                {
                    result = monitor.Value;
                }
            }

            return result;
        }

        public static ToolLEDStatus GetEngineLedStatus(List<MonitorInfo> monitors, string PrimaryDtcs, List<string> storedPowerTrainsDtcs, List<string> pendingPowerTrainsDtcs, List<string> permanentPowerTrainsDtcs)
        {
            var milStatus = GetMilStatus(monitors);

            var isExistPowerTrainDtc = !string.IsNullOrWhiteSpace(PrimaryDtcs) || storedPowerTrainsDtcs.Count > 0
                || pendingPowerTrainsDtcs.Count > 0 || permanentPowerTrainsDtcs.Count > 0;

            //MIL = ON & exist Powertrain DTC => RED
            //MILL = ON & not exist Powertrain DTC => YELLOW
            if ((string.Equals("ON", milStatus, StringComparison.OrdinalIgnoreCase) ||
               string.Equals("«ON»", milStatus, StringComparison.OrdinalIgnoreCase)))
                return isExistPowerTrainDtc ? ToolLEDStatus.Red : ToolLEDStatus.Yellow;

            //MIL = OFF & (all monitors = not support || completed) => GREEN
            var isAllNotSupport = monitors.SkipWhile(m => m.Description.Equals("MIL", StringComparison.OrdinalIgnoreCase)
            || m.Description.Equals("Malfunction Indicator Lamp (MIL)", StringComparison.OrdinalIgnoreCase)
            || m.Description.Equals("Malfunction Indicator Lamb (MIL)", StringComparison.OrdinalIgnoreCase)
            || m.Description.Equals("MIL Check Engine Light", StringComparison.OrdinalIgnoreCase))
              .All(m => m.Value.Replace(" ", "").Equals(MonitorStatus.NotSupported.ToName(), StringComparison.OrdinalIgnoreCase));
            if (isAllNotSupport)
                return ToolLEDStatus.Green;

            //MIL = OFF & (one or more monitor is not completed) => YELLOW
            var notCompleteCount = monitors.Count(m => m.Value.Replace(" ", "").Equals(MonitorStatus.NotComplete.ToName(), StringComparison.OrdinalIgnoreCase));
            if (notCompleteCount > 0)
                return ToolLEDStatus.Yellow;

            //MIL = OFF & (all monitors = not support || completed) => GREEN
            return ToolLEDStatus.Green;
        }

    }
}
