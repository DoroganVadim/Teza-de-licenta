namespace DoctorApplication.Classes
{
    public class ImageStringConvert
    {
        public static string ConvertImageToString(IFormFile file)
        {
            if (file == null)
            {
                return "";
            }
            var ms = new MemoryStream();
            file.CopyTo(ms);
            var fileBytes = ms.ToArray();
            return Convert.ToBase64String(fileBytes);
        }
    }
}
