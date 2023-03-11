using commandlib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace project_manager
{
    internal static class JsonPreprocessor
    {

        #region api

        internal static IEnumerable<string> ActionTypeWordList
        {
            get
            {
                foreach (var actiontype in Enum.GetValues(typeof(ActionType)))
                {
                    yield return actiontype.ToString().ToLower();
                }
            }
        }

        internal static string DenumberActionWord(string textAction)
        {
            string pattern = @"_\d+$";
            string replacement = "";

            Regex regex = new Regex(pattern);
            return regex.Replace(textAction, replacement);
        }

        internal static string NumberActionWord(string text, int lineNumber)
        {
            foreach (var actionTypeWord in ActionTypeWordList)
            {
                text = Regex.Replace(text, $@"\b{actionTypeWord}\b",
                    $"{actionTypeWord}_{lineNumber}");
            }

            return text;
        }

        internal static string PreprocessJsonFile(string text)
        {
            text = CompleteFogottenCommas(text);
            text = AddHeaderAndFooterBrackets(text);
            text = NumberAllPropertiesInText(text);
            return text;

        }

        #endregion

        #region private 

        static private string AddHeaderAndFooterBrackets(string text)
        {
            if (text.Trim().StartsWith("{") == false)
            {
                text = "{\n" + text + "}\n";
            }

            return text;
        }

        static private string NumberAllPropertiesInText(string text)
        {
            string[] lines = text.Split('\n');

            for (int i = 0; i < lines.Count(); i++)
            {
                lines[i] = NumberActionWord(lines[i], i);
            }

            return string.Join("\n", lines);
        }

        static private string CompleteFogottenCommas(string text)
        {
            string[] lines = text.Split('\n');

            const string pattern = @"((^.*:.*[^,]\s*$)|}$)";

            for (int i = 0; i < lines.Count(); i++)
            {
                var match= Regex.Match(lines[i].Trim(), pattern,RegexOptions.Multiline );
                if (match.Success)
                {
                    lines[i] = lines[i] + ',';
                }


            }

            return string.Join("\n", lines);

        }



        #endregion

    }

}
