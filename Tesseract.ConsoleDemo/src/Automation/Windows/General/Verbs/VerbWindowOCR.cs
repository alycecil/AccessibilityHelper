using System;
using runner;
using static runner.OCRHelper;

namespace runner
{
    static internal class VerbWindowOCR
    {
        internal static bool cleanUpOCRTT(string ocr, out string s)
        {
            if (CleanUpOcr(ocr, out s, Verb.Repair, VerbToolTips.Repair)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Fight, VerbToolTips.Fight)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Sell, VerbToolTips.Sell)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Shop, VerbToolTips.Shop)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Talk, VerbToolTips.Talk)) return true;
            if (CleanUpOcr(ocr, out s, Verb.WalkTo, VerbToolTips.WalkTo)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Cast, VerbToolTips.Cast)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Enter, VerbToolTips.Enter)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Close, VerbToolTips.Close)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Steal, VerbToolTips.Steal)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Sit, VerbToolTips.Sit)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Stand, VerbToolTips.Stand)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Take, VerbToolTips.Take)) return true;


            Console.WriteLine("Dropping TT Unknown Verb [{0}]", ocr);
            s = null;
            return false;
        }

        internal  static bool cleanUpOCR(string ocr, out string s)
        {
            if (CleanUpOcr(ocr, out s, Verb.LOOKAT)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Repair)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Fight)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Sell)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Shop)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Steal)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Talk)) return true;
            if (CleanUpOcr(ocr, out s, Verb.WalkTo)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Cast)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Enter)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Close)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Sit)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Stand)) return true;
            if (CleanUpOcr(ocr, out s, Verb.Take)) return true;

            Console.WriteLine("Dropping OCR Unknown Verb [{0}]", ocr);
            s = null;
            return false;
        }
    }
}