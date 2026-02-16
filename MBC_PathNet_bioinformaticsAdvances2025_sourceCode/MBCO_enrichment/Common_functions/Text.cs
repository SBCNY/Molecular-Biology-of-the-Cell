//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

using System;
using System.Text;

namespace Common_functions.Text
{
    class Text_class
    {
        public static string Split_text_over_multiple_lines(string text, int max_nchar_per_line, int max_lines)
        {
            int nchars = text.Length;
            if ((nchars > max_nchar_per_line)||(text.Split('\n').Length>max_lines))
            {
                int new_lines_count = Math.Min(max_lines, (int)Math.Ceiling((float)nchars / max_nchar_per_line));
                int new_nchar_each_line = (int)Math.Ceiling((float)nchars / new_lines_count);
                int remaining_nchars_length = nchars;
                int remaining_lines_length = new_lines_count;
                int[] nchar_each_line = new int[new_lines_count];
                StringBuilder new_name_sb = new StringBuilder();
                int nchar_current_line = 0;
                string[] splitStrings = text.Split(' ','\n','\r');
                string splitString;
                int splitStrings_length = splitStrings.Length;
                int lines_count = 1;
                new_name_sb.AppendFormat(splitStrings[0]);
                nchar_current_line = splitStrings[0].Length;
                remaining_nchars_length -= splitStrings[0].Length;
                remaining_lines_length--;
                int current_add_string_length;
                for (int indexSplit = 1; indexSplit < splitStrings_length; indexSplit++)
                {
                    splitString = splitStrings[indexSplit];
                    if (!String.IsNullOrEmpty(splitString))
                    {
                        current_add_string_length = splitString.Length + 1;
                        if (   (remaining_lines_length==0)
                            || ((nchar_current_line + current_add_string_length) <= (remaining_nchars_length / remaining_lines_length)))
                        {
                            new_name_sb.AppendFormat(" {0}", splitString);
                            nchar_current_line += splitString.Length + 1;
                            remaining_nchars_length -= splitString.Length + 1;
                        }
                        else
                        {
                            new_name_sb.AppendFormat("\n\r{0}", splitString);
                            nchar_current_line = splitString.Length;
                            remaining_nchars_length -= splitString.Length;
                            remaining_lines_length--;
                            lines_count++;
                        }
                    }
                }
                return new_name_sb.ToString();
            }
            else { return text; }
        }
        public static string[] Split_texts_over_multiple_lines(string[] texts, int max_nchar_per_line, int max_lines)
        {
            int texts_length = texts.Length;
            for (int indexText=0; indexText<texts_length; indexText++)
            {
                texts[indexText] = Split_text_over_multiple_lines(texts[indexText],max_nchar_per_line,max_lines);
            }
            return texts;
        }
        public static string Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(string text)
        {
            int text_length = text.Length;
            bool space_comma_semicolon_colon_at_beginning = true;
            bool space_comma_semicolon_colon_at_end = true;
            while (((space_comma_semicolon_colon_at_beginning) || (space_comma_semicolon_colon_at_end)) && ((!String.IsNullOrEmpty(text)) && (text_length >= 2)))
            {
                text_length = text.Length;
                space_comma_semicolon_colon_at_beginning = text[0].Equals(' ') || (text[0].Equals(',')) || (text[0].Equals(';')) || (text[0].Equals(':'));
                space_comma_semicolon_colon_at_end = (text[text_length - 1].Equals(' ')) || (text[text_length - 1].Equals(',')) || (text[text_length - 1].Equals(';')) || (text[text_length - 1].Equals(':'));
                if (space_comma_semicolon_colon_at_beginning && space_comma_semicolon_colon_at_end)
                {
                    text = text.Substring(1, text_length - 2);
                }
                else if (space_comma_semicolon_colon_at_beginning)
                {
                    text = text.Substring(1, text_length - 1);
                }
                else if (space_comma_semicolon_colon_at_end)
                {
                    text = text.Substring(0, text_length - 1);
                }
            }
            return text;
        }
        public static string Remove_enter_characters_from_beginning_of_text(string text)
        {
            if (text.IndexOf("\n") == 0)
            {
                text = text.Substring(1, text.Length - 1);
            }
            return text;
        }
        public static string Remove_given_substring_from_beginning_and_end_of_text_if_exists(string text, string removeSubstring)
        {
            int indexRemoveSubstring = text.IndexOf(removeSubstring);
            int remove_substring_length = removeSubstring.Length;
            if (indexRemoveSubstring==0)
            {
                text = text.Substring(remove_substring_length, text.Length - remove_substring_length);
            }
            indexRemoveSubstring = text.LastIndexOf(removeSubstring);
            if (  (indexRemoveSubstring + remove_substring_length == text.Length)
                &&(indexRemoveSubstring!=-1))
            {
                text = text.Substring(0, indexRemoveSubstring);
            }
            return text;
        }
        public static string Replace_characters_that_are_incompatible_with_fileNames_by_underline(string fileName)
        {
            fileName = fileName.Replace('/', '_');
            fileName = fileName.Replace(';', '_');
            fileName = fileName.Replace(',', '_');
            fileName = fileName.Replace('\\','_');
            return fileName;
        }
        public static string Set_first_character_to_upperCase_and_rest_toLowerCase(string word)
        {
            word = word.ToLower();
            word = word.Substring(0,1).ToUpper() + word.Substring(1,word.Length-1);
            return word;
        }
    }
}
