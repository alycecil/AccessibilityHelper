using System;
using static runner.OCRHelper;

namespace runner
{
    internal static class VerbWindowOCR
    {
        internal static bool CleanUpOcrTooltips(string ocr, out string s)
        {
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Repair, VerbToolTips.Repair)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Fight, VerbToolTips.Fight)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Sell, VerbToolTips.Sell)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Shop, VerbToolTips.Shop)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Talk, VerbToolTips.Talk)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.WalkTo, VerbToolTips.WalkTo)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Cast, VerbToolTips.Cast)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Enter, VerbToolTips.Enter)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Close, VerbToolTips.Close)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Steal, VerbToolTips.Steal)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Sit, VerbToolTips.Sit)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Stand, VerbToolTips.Stand)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Take, VerbToolTips.Take)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Follow, VerbToolTips.Follow)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Group, VerbToolTips.Group)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Chat, VerbToolTips.Chat)) return true;


            Console.WriteLine("Dropping TT Unknown Verb [{0}]", ocr);
            s = null;
            return false;
        }

        internal static bool CleanUpOcr(string ocr, out string s)
        {
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.LOOKAT)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Repair)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Fight)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Sell)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Shop)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Steal)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Talk)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.WalkTo)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Cast)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Enter)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Close)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Sit)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Stand)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Take)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Follow)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Group)) return true;
            if (OCRHelper.CleanUpOcr(ocr, out s, Verb.Chat)) return true;


            Console.WriteLine("Dropping OCR Unknown Verb [{0}]", ocr);
            s = null;
            return false;
        }
    }
}