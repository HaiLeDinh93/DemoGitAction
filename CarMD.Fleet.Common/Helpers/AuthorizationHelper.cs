using CarMD.Fleet.Common.Configuration;
using CarMD.Fleet.Core.Common;
using CarMD.Fleet.Core.Cryptography;
using Newtonsoft.Json;
using System;

namespace CarMD.Fleet.Common.Helper
{
    public static class AuthorizationHelper
    {
        public static AuthorizationModel Decrypt(string enc)
        {
            try
            {
                var json = (new Crypto(Crypto.Provider.Rijndael)).Decrypt(enc, CommonConfiguration.EncryptKey);
                return JsonConvert.DeserializeObject<AuthorizationModel>(json);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Creates the ticket.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public static string CreateAuthorizationKey(AuthorizationModel ticket)
        {
            string json = JsonConvert.SerializeObject(ticket);
            json = (new Crypto(Crypto.Provider.Rijndael)).Encrypt(json, CommonConfiguration.EncryptKey);
            return json;
        }
    }
}
