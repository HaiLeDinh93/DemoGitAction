using System;
using System.Collections.Generic;

namespace CarMD.Shell.Api.Helpers
{
    public static class FreezeFrameHelper
    {
        public static List<Models.FreezeFrameItem> ParseFreezeFrame(string ffRaw, out string milDtc)
        {
            milDtc = string.Empty;

            var ffItems = new List<Models.FreezeFrameItem>();

            if (string.IsNullOrWhiteSpace(ffRaw))
            {
                return ffItems;
            }

            var bytes = Convert.FromBase64String(ffRaw);
            int length = BitConverter.ToInt16(bytes, 0);
            if (length == 0)
            {
                return ffItems;
            }

            var startIndex = 2;
            var milDtcLength = bytes[startIndex];

            for (int i = 1; i <= milDtcLength; i++)
            {
                milDtc += ((char)bytes[startIndex + i]).ToString();
            }

            
            if (string.IsNullOrWhiteSpace(milDtc))
            {
                ffItems.Add(new Models.FreezeFrameItem("No Freeze Frame Available", ""));
                return ffItems;
            }

            //ffItems.Add(new FreezeFrameItem("MIL DTC", milDtc));

            startIndex = 8;

            var names = new List<string>();

            for (int i = 0; i < length; i++)
            {
                startIndex = AddFFNameItem(names, bytes, startIndex);
            }

            var units = new List<string>();

            for (int i = 0; i < length; i++)
            {
                startIndex = AddFFUnitNValueItem(units, bytes, startIndex);
            }

            var values = new List<string>();
            for (int i = 0; i < length; i++)
            {
                startIndex = AddFFUnitNValueItem(values, bytes, startIndex);
            }

            int index = 0;
            foreach (var name in names)
            {
                string value = "";
                try
                {
                    decimal val = Math.Round(decimal.Parse(values[index]), 2);
                    value = val.ToString();
                }
                catch (Exception)
                {
                    value = values[index];
                }
                ffItems.Add(new Models.FreezeFrameItem(name, string.Format("{0}{1}", value, string.IsNullOrWhiteSpace(units[index]) ? "" : " " + units[index].Replace("Â", ""))));

                index++;
            }
            return ffItems;
        }

        private static int AddFFNameItem(List<string> items, byte[] data, int startIndex)
        {
            int length = BitConverter.ToInt16(data, startIndex);
            if (length == 0)
                return 0;
            startIndex += 2;
            var item = "";
            for (int i = 0; i < length; i++)
            {
                item += ((char)data[startIndex]).ToString();
                startIndex++;
            }

            items.Add(item);
            return startIndex;
        }

        private static int AddFFUnitNValueItem(List<string> items, byte[] data, int startIndex)
        {
            int length = data[startIndex];
            startIndex++;
            var item = "";
            for (int i = 0; i < length; i++)
            {
                item += ((char)data[startIndex]).ToString();
                startIndex++;
            }

            items.Add(item);
            return startIndex;
        }
    }
}
