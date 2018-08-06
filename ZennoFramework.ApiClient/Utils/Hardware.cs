using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace ZennoFramework.Api.Client.Utils
{
    public static class Hardware
    {
        public static string GetId()
        {
            var os = GetOs();
            var motherboard = GetMotherBoardId();
            var processId = GetProcessId();

            var full =  os + motherboard + processId;
            return GetMd5Hash(HashAlgorithm.Create(), full);
        }

        private static string GetOs()
        {
            string str = "";
            var searcher = new ManagementObjectSearcher("root\\CIMV2",
                "SELECT * FROM CIM_OperatingSystem");

            foreach (var queryObj in searcher.Get())
            {
                try
                {
                    str = queryObj["SerialNumber"].ToString();
                }
                catch
                {
                    // ignored
                }
            }
            return str;
        }

        private static string GetMotherBoardId()
        {
            string str = "";
            var searcher = new ManagementObjectSearcher("root\\CIMV2",
                "SELECT * FROM CIM_Card");
            foreach (var queryObj in searcher.Get())
            {
                try
                {
                    str = queryObj["SerialNumber"].ToString();
                }
                catch
                {
                }
            }


            return str;
        }

        private static string GetProcessId()
        {
            string str = "";
            var searcher =
                new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");
            foreach (var queryObj in searcher.Get())
            {
                try
                {
                    str = queryObj["ProcessorId"].ToString();
                }
                catch
                {
                    //ignore
                }
            }

            return str;
        }

        static string GetMd5Hash(HashAlgorithm md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }


            return sBuilder.ToString();
        }
    }
}
