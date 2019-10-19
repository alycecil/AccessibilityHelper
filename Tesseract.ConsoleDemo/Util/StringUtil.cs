using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace runner
{
    public class StringUtil
    {
        public static string SubString(string toLiteral, int i)
        {
            var _i = i;
            var _l = toLiteral.Length;
            i = Math.Min(_l, i);
            if (i > 0)
            {
                return toLiteral.Substring(0, i) + ((_l > i) ? "..." : "");
            }

            return "";
        }

        public static string ToLiteral(string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }
    }
}