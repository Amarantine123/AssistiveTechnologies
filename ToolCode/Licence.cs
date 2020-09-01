using System.Configuration;
using System.Web;

namespace NFine.Code
{
    public sealed class Licence
    {
        /// <summary>
        /// judge if Licence
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsLicence(string key)
        {
            string host = HttpContext.Current.Request.Url.Host.ToLower();
            if (host.Equals("localhost"))
                return true;
            string licence = ConfigurationManager.AppSettings["LicenceKey"];
            if (licence != null && licence == Md5.md5(key, 32))
                return true;

            return false;
        }
        /// <summary>
        /// get Licence
        /// </summary>
        /// <returns></returns>
        public static string GetLicence()
        {
            var licence = Configs.GetValue("LicenceKey");
            if (string.IsNullOrEmpty(licence))
            {
                licence = Common.GuId();
                Configs.SetValue("LicenceKey", licence);
            }
            return Md5.md5(licence, 32);
        }
    }
}
