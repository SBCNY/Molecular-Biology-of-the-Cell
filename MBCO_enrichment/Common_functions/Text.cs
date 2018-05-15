#region Author information
/*
The code was written by Jens Hansen working for the Ravi Iyengar Lab
The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
Please acknowledge the MBC Ontology in your publications by citing the following reference:
Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar: 
A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes
Sci Rep. 2017 Dec 18;7(1):17689. doi: 10.1038/s41598-017-16627-4.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common_functions.Report;

namespace Common_functions.Text
{
    class Text_class
    {
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
     }
}
