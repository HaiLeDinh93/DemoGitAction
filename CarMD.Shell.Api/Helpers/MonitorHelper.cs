using Innova2.VehicleDataLib.Entities.PowerTrain;
using Innova2.VehicleDataLib.Enums.Vehicle;
using Innova2.VehicleDataLib.Parsing.Resources;
using Innova2.VehicleDataLib.Parsing.Resources.DataSet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CarMD.Shell.Api.Helpers
{
    public static class MonitorHelper
    {
        public static List<MonitorStatusItem> ParseMonitor(string monitorStatusRaw)
        {
            if (string.IsNullOrWhiteSpace(monitorStatusRaw))
            {
                return new List<MonitorStatusItem>();
            }

            var rawBytes = Convert.FromBase64String(monitorStatusRaw);

            if (rawBytes.Length == 9)
            {
                byte[] monitors = new byte[8];
                Array.Copy(rawBytes, 1, monitors, 0, 8);
                return ParseMonitorStatus(monitors.ToArray());
            }
            else
            {
                return ParseMonitorStatus(rawBytes);
            }
        }

        private static List<MonitorStatusItem> ParseMonitorStatus(byte[] service1Buffer)
        {
            //service 1 PID 1 has 4 bytes contains System Status Monitoring
            byte[] bytes = new[] { service1Buffer[4], service1Buffer[5], service1Buffer[6], service1Buffer[7] };

            //There are to kind of car to be monitoring. Read the fourth bit of second byte
            //1- Compression ignition (máy dầu)
            //2- Spark ignition (máy xăng)        
            bool isCompressionSupported = (bytes[1] & 0x08) > 0;

            //which each ignition, the list of monitoring item is different and
            //configured in the resource.
            string content = isCompressionSupported ? Resource.CompressionMonitorStatus : Resource.SparkMonitorStatus;

            //we read the resource into a dataseet
            MonitorStatus ds = new MonitorStatus();
            StringReader reader = new StringReader(content);
            ds.ReadXml(reader);

            List<MonitorStatusItem> items = new List<MonitorStatusItem>();
            //loop through each item defined
            foreach (MonitorStatus.MonitorItemRow itemRow in ds.MonitorItem.Rows)
            {
                //get the localized key name
                MonitorStatusItem item = new MonitorStatusItem { Name = itemRow.Name };

                //which Support Value 0xFF, this is MIL (always support, value will be ON/OFF )
                //otherwise Support flag and Status value is read from 2 bits among 4 bytes of pid 1
                if (itemRow.SupportValue < 255)
                {
                    //or bit to get the supported bit
                    if ((bytes[itemRow.SupportIndex] & itemRow.SupportValue) > 0)
                    {
                        item.Key = (bytes[itemRow.ReadyIndex] & itemRow.ReadyValue) > 0
                                        ? MonitorStatusKey.NotComplete
                                        : MonitorStatusKey.Complete;
                        //or bit to get the ready value
                        item.Value = (bytes[itemRow.ReadyIndex] & itemRow.ReadyValue) > 0
                                        ? Resource.NotComplete
                                        : Resource.Complete;
                    }
                    else
                    {
                        item.Key = MonitorStatusKey.NotSupported;
                        item.Value = Resource.NotSupported;
                    }
                }
                else
                {
                    //MIL case, ON/OFF only
                    item.Value = (bytes[itemRow.ReadyIndex] & itemRow.ReadyValue) > 0 ? Resource.ON : Resource.OFF;
                    item.Key = (bytes[itemRow.ReadyIndex] & itemRow.ReadyValue) > 0 ? MonitorStatusKey.ON : MonitorStatusKey.OFF;
                }
                items.Add(item);
            }

            return items;
        }
    }
}
