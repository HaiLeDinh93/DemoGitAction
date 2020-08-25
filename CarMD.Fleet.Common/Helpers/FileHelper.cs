using CarMD.Fleet.Core.Utility;
using log4net;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Reflection;

namespace CarMD.Fleet.Common.Helpers
{
    public static class FileHelper
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static bool UploadFile(IFormFile file, string fileName, string pathString)
        {
            try
            {
                bool isExists = Directory.Exists(pathString);
                if (!isExists)
                {
                    Directory.CreateDirectory(pathString);
                }

                using (var stream = new FileStream(Path.Combine(pathString, fileName), FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.FatalFormat("Upload shop logo exception: {0} ", LogUtility.GetDetailsErrorMessage(ex));
            }
            return false;
        }
    }
}
