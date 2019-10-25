using System;

namespace runner
{
    static internal class OCRHelper
    {
        public static bool CleanUpOcr(string ocr, out string s, string resultValue, string match = null)
        {
            if (String.IsNullOrEmpty(match))
            {
                match = resultValue;
            }

            if (String.Equals(ocr, match, StringComparison.OrdinalIgnoreCase)
                || ocr.StartsWith(match, StringComparison.OrdinalIgnoreCase))
            {
                s = resultValue;
                return true;
            }

            s = null;
            return false;
        }
    }
}