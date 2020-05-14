using CentralErros.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralErros.Utils
{
    public static class Utils
    {
        public static string DecryptConnectionString(string connection)
        {
            AppSettings app = new AppSettings();

            Byte[] b = Convert.FromBase64String(connection);

            string decryptedConnection = System.Text.ASCIIEncoding.ASCII.GetString(b);
            return decryptedConnection;

        }

        public static string EncryptConnectionString(string connectionString)
        {
            Byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(connectionString);
            string encryptedConnection = Convert.ToBase64String(b);
            return encryptedConnection;

        }
    }
}
