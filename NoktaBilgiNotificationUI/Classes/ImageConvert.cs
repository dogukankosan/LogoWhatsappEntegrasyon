using System;
using System.Drawing;
using System.IO;


namespace NoktaBilgiNotificationUI.Classes
{
   internal class ImageConvert
    {
        internal static string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                if (!(image is  null))
                {
                    image.Save(ms, format);
                    byte[] imageBytes = ms.ToArray();
                    ms.Dispose();
                    ms.Close();
                    return Convert.ToBase64String(imageBytes);
                }
            }
            catch (Exception ex)
            {
                ms.Dispose();
                ms.Close();
                TextLog.TextLogging(ex.Message + " --- " + ex.ToString());
                return null;
            }
            ms.Dispose();
            ms.Close();
            return null;
        }
        internal static Image Base64ToImage(string base64String)
        {
            if (!string.IsNullOrEmpty(base64String))
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                var ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                Image image = Image.FromStream(ms, true);
                return image;
            }
            return null;
        }
    }
}