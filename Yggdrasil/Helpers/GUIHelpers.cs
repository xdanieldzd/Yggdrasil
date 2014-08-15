using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Text;

namespace Yggdrasil.Helpers
{
    public static class GUIHelpers
    {
        static FontFamily jpnFontFamily;

        static GUIHelpers()
        {
            jpnFontFamily = new InstalledFontCollection().Families.FirstOrDefault(x => x.Name == "Segoe UI");
        }

        public static Font GetSuggestedGUIFont(GameDataManager.Versions gameVersion)
        {
            if (gameVersion == GameDataManager.Versions.Japanese)
                return new Font(jpnFontFamily, 8.0f);
            else
                return MainForm.DefaultFont;
        }
    }
}
