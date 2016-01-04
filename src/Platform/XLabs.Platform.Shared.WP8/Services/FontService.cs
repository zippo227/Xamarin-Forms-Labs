using System.Collections.Generic;

namespace XLabs.Platform.Services
{
    /// <summary>
    /// A FontService implementation for Windows Platforms.
    /// </summary>
    public class FontService
    {
        /// <summary>
        /// Gets the font names.
        /// </summary>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public IEnumerable<string> GetFontNames()
        {
            var fontList = new List<string>();
#if NETFX_CORE || WINDOWS_PHONE_APP
            var factory = new SharpDX.DirectWrite.Factory();
            var fontCollection = factory.GetSystemFontCollection(false);
            var familyCount = fontCollection.FontFamilyCount;

            for (int i = 0; i < familyCount; i++)
            {
                var fontFamily = fontCollection.GetFontFamily(i);
                var familyNames = fontFamily.FamilyNames;
                int index;

                if (!familyNames.FindLocaleName(System.Globalization.CultureInfo.CurrentCulture.Name, out index))
                {
                    familyNames.FindLocaleName("en-us", out index);
                }

                string name = familyNames.GetString(index);
                fontList.Add(name);
            }
#endif

            return fontList;
        }
    }
}
