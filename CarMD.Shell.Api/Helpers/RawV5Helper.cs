using CarMD.Fleet.Data.Request.Api;
using Innova2.VehicleDataLib.Enums.Device;
using Innova2.VehicleDataLib.Enums.Version5;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarMD.Shell.Api.Helpers
{
    public class RawV5Helper
    {
        private const int VinLength = 17;
        private const int VinProfileBufferLength = 512;

        public static string GetRawDataString(VehicleDataModel model)
        {
            var buffer = BuildVehicleDataBuffer(model, BuildVinProfileBuffer(model.Vin));
            var rawData = new Innova2.VehicleDataLib.Entities.Version5.RawData
            {
                Language = Innova2.VehicleDataLib.Enums.Common.Language.English,
                SoftwareVersion = new Version(5, 0, 0),
                VehicleRaw = buffer,
                SystemInfoRaw = string.Empty,
                ProductId = (UsbProductId)model.UsbProductId,
            };

            return rawData.ToBase64String();
        }

        static string BuildVehicleDataBuffer(VehicleDataModel model, byte[] vinProfileBuffer)
        {
            List<byte> vehicleDataBufferBytes = new List<byte>();

            AddBuffer(BufferModules.None, BufferSegmentTypes.VinProfile, vinProfileBuffer, vehicleDataBufferBytes);

            AddBuffer(BufferModules.ECM, BufferSegmentTypes.MonitorStatus, model.MonitorStatusRaw, vehicleDataBufferBytes);

            AddBuffer(BufferModules.ECM, BufferSegmentTypes.FreezeFrame, model.FreezeFrameRaw, vehicleDataBufferBytes);

            AddBuffer(BufferModules.ECM, BufferSegmentTypes.Dtc, model.ECMDTCs, vehicleDataBufferBytes);

            AddBuffer(BufferModules.TCM, BufferSegmentTypes.Dtc, model.TCMDTCs, vehicleDataBufferBytes);

            return Convert.ToBase64String(vehicleDataBufferBytes.ToArray());
        }

        private static byte[] BuildVinProfileBuffer(string vin)
        {
            var vinBuffer = new byte[VinLength];
            vinBuffer = System.Text.Encoding.UTF8.GetBytes(vin);

            var vinProfileBuffer = new byte[VinProfileBufferLength];
            vinProfileBuffer[0] = 0xAA;

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

        private const int FreezeFrameBufferSizeNotFormatted = 1556;
        private static void AddBuffer(BufferModules module, BufferSegmentTypes type, string raw, List<byte> dataBuffer, bool formatted = false)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return;
            var bytes = new List<byte>();
            bytes.Add((byte)module);

            bytes.AddRange(GetLengthIntBytes((int)type));

            var rawBytes = Convert.FromBase64String(raw).ToList();
            if (type == BufferSegmentTypes.MonitorStatus && rawBytes.Count() == 8)
            {
                rawBytes.Insert(0, 0xaa);
            }
            if (type == BufferSegmentTypes.OemData)
                rawBytes.Add(0);
            if (type == BufferSegmentTypes.FreezeFrame && rawBytes.Count == FreezeFrameBufferSizeNotFormatted)
            {
                rawBytes = RebuildFreezeFrameBuffer(rawBytes.ToArray());
            }
            bytes.AddRange(GetLengthBytes(rawBytes.Count));

            bytes.AddRange(rawBytes);

            dataBuffer.AddRange(bytes);
        }

        private static void AddBuffer(BufferModules module, BufferSegmentTypes type, byte[] raw, List<byte> dataBuffer, bool formatted = false)
        {
            if (raw == null)
                return;
            var bytes = new List<byte> { (byte)module };

            bytes.AddRange(BitConverter.GetBytes((short)type));
            var rawBytes = raw.ToList();

            if (type == BufferSegmentTypes.OemData)
            {
                byte length = 0;
                if (raw.Length == 256)
                    length = 1;
                if (raw.Length == 512)
                    length = 2;
                rawBytes.Insert(0, length);
            }

            bytes.AddRange(BitConverter.GetBytes(rawBytes.Count));

            bytes.AddRange(rawBytes);

            dataBuffer.AddRange(bytes);
        }

        private static List<byte> RebuildFreezeFrameBuffer(byte[] rawBytes)
        {
            var newFFBuffer = new List<byte>();
            int startIndex = 5;
            newFFBuffer.Add(rawBytes[startIndex]);
            startIndex++;
            newFFBuffer.Add(rawBytes[startIndex]);
            startIndex = 7;
            var milDTCLength = rawBytes[startIndex];
            for (int i = 0; i <= milDTCLength; i++)
            {
                newFFBuffer.Add(rawBytes[startIndex]);
                startIndex++;
            }
            startIndex = 20;
            var bufferIndex = 0;
            var totalItemCount = BitConverter.ToInt16(new byte[2] { newFFBuffer[0], newFFBuffer[1] }, 0);
            var itemIndex = 1;
            while (true)
            {
                var length = bufferIndex == 0 ? BitConverter.ToInt16(new byte[2] { rawBytes[startIndex], rawBytes[startIndex + 1] }, 0) : rawBytes[startIndex];
                if (length == 0)
                {
                    if (itemIndex <= totalItemCount)
                    {
                        newFFBuffer.Add(rawBytes[startIndex]);
                        startIndex++;
                        itemIndex++;
                        continue;
                    }
                    itemIndex = 1;
                    if (bufferIndex == 0) // units
                    {
                        startIndex = 512 + 20;
                        bufferIndex++;
                        continue;
                    }
                    if (bufferIndex == 1) // values
                    {
                        startIndex = 1024 + 20;
                        bufferIndex++;
                        continue;
                    }
                    if (bufferIndex == 2)
                    {
                        break;
                    }
                }
                newFFBuffer.Add(rawBytes[startIndex]);
                if (bufferIndex == 0)
                {
                    newFFBuffer.Add(rawBytes[startIndex + 1]);
                    startIndex += 2;
                }
                else
                {
                    startIndex++;
                }
                for (int i = 0; i < length; i++)
                {
                    newFFBuffer.Add(rawBytes[startIndex + i]);
                }
                itemIndex++;
                startIndex += length;
                if (length == 0)
                    break;
            }
            return newFFBuffer;
        }

        private static IEnumerable<byte> GetLengthBytes(long length)
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)((length >> 24) & 0xFF);
            bytes[1] = (byte)((length >> 16) & 0xFF);
            bytes[2] = (byte)((length >> 8) & 0xFF);
            bytes[3] = (byte)(length & 0xFF);
            return bytes.Reverse().ToList();
        }

        private static IEnumerable<byte> GetLengthIntBytes(int length)
        {
            byte[] bytes = new byte[2];
            bytes[0] = (byte)((length >> 8) & 0xFF);
            bytes[1] = (byte)(length & 0xFF);
            return bytes.Reverse().ToList();
        }
    }
}