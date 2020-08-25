using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMD.Shell.Api.Helpers
{
    public static class VehicleParser
    {
        private const byte ValidStatus = 0xAA;

        public static byte[] GetFreezeFrameBuffer(byte[] ffBuffer)
        {
            byte[] ecmBuffer = null;
            if (ffBuffer != null && ffBuffer[0] == ValidStatus)
            {
                var ecmData = Innova2.VehicleDataLib.Parsing.Version5.FreezeFrameParser.Parse(null, ffBuffer, ((int)Innova2.VehicleDataLib.Enums.Common.Language.English).ToString(), "0");
                ecmBuffer = BuildFreezeFrameBuffer(ecmData);
            }
            return ecmBuffer;
        }

        public static byte[] BuildFreezeFrameBuffer(Innova2.VehicleDataLib.Entities.Version5.LiveDatas ffData)
        {
            //Ref: PCLink Command Specification V00 00 15D.docx
            var lstECMDTC = new List<byte>();
            if (ffData != null)
            {
                //number of items: 2 bytes
                var bNumberItem = BitConverter.GetBytes((short)ffData.Count());
                lstECMDTC.AddRange(bNumberItem);

                //MIL dtc len: 1 byte, MIL dtc buffer data
                byte bMilDtcLenData = (byte)((!string.IsNullOrEmpty(ffData.MilDTC) ? ffData.MilDTC.Length : 0) & 0xff);
                //var bMilDtcLen = BitConverter.GetBytes(bMilDtcLenData);
                lstECMDTC.Add(bMilDtcLenData);
                if (!string.IsNullOrEmpty(ffData.MilDTC) && ffData.MilDTC.Length > 0)
                {
                    var bMilDtc = ASCIIEncoding.ASCII.GetBytes(ffData.MilDTC);
                    lstECMDTC.AddRange(bMilDtc);
                }

                //names
                foreach (var item in ffData)
                {
                    //Nx[name(len: 2 bytes, buffer data of the item)]
                    var nameLength = (short)(!string.IsNullOrEmpty(item.Name) ? item.Name.Length : 1);
                    var bNameLength = BitConverter.GetBytes(nameLength);
                    lstECMDTC.AddRange(bNameLength);

                    var bName = ASCIIEncoding.ASCII.GetBytes(!string.IsNullOrEmpty(item.Name) ? item.Name : " ");
                    lstECMDTC.AddRange(bName);
                }

                //units
                foreach (var item in ffData)
                {
                    //Nx[units(len: 1 bytes, buffer data of the item)] 
                    var unitLength = (short)(!string.IsNullOrEmpty(item.Unit) ? item.Unit.Length : 1);
                    byte bunitLength = (byte)(unitLength & 0xff);
                    lstECMDTC.Add(bunitLength);

                    var bUnit = ASCIIEncoding.ASCII.GetBytes(!string.IsNullOrEmpty(item.Unit) ? item.Unit : " ");
                    lstECMDTC.AddRange(bUnit);
                }

                //values
                foreach (var item in ffData)
                {
                    //Nx[value(len: 1 bytes, buffer data of the item)]
                    var value = item.Values != null && item.Values.Any() ? String.Join(" ", item.Values) : " ";
                    byte bvalueLength = (byte)(((short)value.Length) & 0xff);
                    lstECMDTC.Add(bvalueLength);

                    var bValue = ASCIIEncoding.ASCII.GetBytes(value);
                    lstECMDTC.AddRange(bValue);
                }
            }

            return lstECMDTC.Any() ? lstECMDTC.ToArray() : null;
        }

        //public static VehicleInformation GetOBD2VehicleInfo(byte[] ecmBuffer, byte[] tcmBuffer)
        //{
        //    VehicleInformation vInfo = null;
        //    if (ecmBuffer != null)
        //    {
        //        var data = Innova2.VehicleDataLib.Parsing.Version5.VehicleInfoParser.Parse(ecmBuffer);
        //        if (data != null)
        //            vInfo = data.ToModel();
        //    }
        //    if (tcmBuffer != null)
        //    {
        //        var data = Innova2.VehicleDataLib.Parsing.Version5.VehicleInfoParser.Parse(tcmBuffer);
        //        if (data != null)
        //        {
        //            if (vInfo == null)
        //            {
        //                vInfo = data.ToModel();
        //            }
        //            else
        //            {
        //                vInfo.CallIds.AddRange(data.CallibrationIds.ReplaceSpace() ?? new List<string>());
        //                vInfo.CVNs.AddRange(data.CallibrationVerificationNumber.ReplaceSpace() ?? new List<string>());
        //                if (data.InusePerformanceTracking != null)
        //                    vInfo.IPTs.AddRange(data.InusePerformanceTracking?.Select(x => new IPTItem { Name = x.Name, Value = x.Value }).ToList());
        //            }
        //        }
        //    }

        //    return vInfo;
        //}
    }
}
