#region Author information
/*
The code was written by Jens Hansen working for the Ravi Iyengar Lab
The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
Please acknowledge the MBC Ontology in your publications by citing the following reference:
Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar: 
A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes
Sci Rep. 2017 Dec18th
*/
#endregion

using System;
using System.Linq;
using System.Text;

namespace Common_functions.Global_definitions
{
    public enum ReadWrite_report_enum { E_m_p_t_y = 0, Report_nothing, Report_main = 1, Report_everything = 2 }
    public enum Ontology_type_enum { E_m_p_t_y, Molecular_biology_cell }
    enum Entry_type_enum { E_m_p_t_y, Counts, Diffrna, Diffrna_up, Diffrna_down, Diffprot, Diffprot_up, Diffprot_down, sirna_kd, Various, Various_up, Various_down };

    class Global_class
    {
        private const string empty_entry = "E_m_p_t_y";  //check enums, Empty has to be the same!!
        private const char tab = '\t';
        private const char scp_delimiter = '$';
        private const string background_genes_scpName = "Background genes";

        public static string Empty_entry {  get { return empty_entry; } }
        public static char Scp_delimiter { get { return scp_delimiter; } }
        public static char Tab {  get { return tab; } }
        public static string Background_genes_scpName { get { return background_genes_scpName; } }

        public static string Get_complete_sampleName(Entry_type_enum entryType, int timepoint, string sampleName)
        {
            StringBuilder sb = new StringBuilder();
            if (!entryType.Equals(Entry_type_enum.E_m_p_t_y))
            {
                if (sb.Length>0) { sb.AppendFormat("-"); }
                sb.AppendFormat(entryType.ToString());
            }
            if (!String.IsNullOrEmpty(sampleName))
            {
                if (sb.Length > 0) { sb.AppendFormat("-"); }
                sb.AppendFormat(sampleName);
            }
            if (sb.Length > 0) { sb.AppendFormat("-"); }
            sb.AppendFormat(timepoint.ToString());
            return sb.ToString();
        }
    }

    class Global_directory_and_file_class
    {
        #region constant directory
        private const string mbco_major_directory = "E://MBCO_enrichment//";
        private const string custom_data_directory = mbco_major_directory + "Custom_data_sets//";
        private const string mbco_datasets_directory = mbco_major_directory + "MBCO_datasets//";
        private const string results_directory = mbco_major_directory + "Results//";
        #endregion

        #region constant file
        private const string mbco_obo_fileName = "Supplementary Table S1B.txt";
        private const string mbco_association_fileName = "Supplementary Table S32 - gene-SCP associations.txt";
        private const string mbco_inferred_scp_relationships_fileName = "Supplementary Table S35 - inferred SCP relationships.txt";
        #endregion

        public static string Results_directory
        {
            get { return results_directory; }
        }

        public static string Custom_data_directory
        {
            get { return custom_data_directory; }
        }

        #region MBCO dataset fileNames
        public static string MBCO_datasets_directory
        {
            get { return mbco_datasets_directory; }
        }

        public static string Complete_mbco_obo_fileName
        {
            get { return mbco_datasets_directory + mbco_obo_fileName; }
        }

        public static string Complete_mbco_association_fileName
        {
            get { return mbco_datasets_directory + mbco_association_fileName; }
        }

        public static string Complete_mbco_inferred_scp_relationships_fileName
        {
            get { return mbco_datasets_directory + mbco_inferred_scp_relationships_fileName; }
        }
        #endregion

    }

    class Math_class
    {
        private static string Get_hexadecimal_sign(int number)
        {
            string sign = "no value";
            switch (number)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    sign = number.ToString();
                    break;
                case 10:
                    sign = "A";
                    break;
                case 11:
                    sign = "B";
                    break;
                case 12:
                    sign = "C";
                    break;
                case 13:
                    sign = "D";
                    break;
                case 14:
                    sign = "E";
                    break;
                case 15:
                    sign = "F";
                    break;
                default:
                    throw new Exception();
            }
            return sign;
        }

        public static string Convert_into_two_digit_hexadecimal(int number)
        {
            if ((number > 255) || (number < 0))
            {
                throw new Exception();
            }
            else
            {
                int multiples_of_16 = (int)Math.Floor((double)number / (double)16);
                int modulus = number % 16;
                return Get_hexadecimal_sign(multiples_of_16) + Get_hexadecimal_sign(modulus);
            }
        }

        public static string Get_hexadecimal_code(int red, int green, int blue)
        {
            return "#" + Convert_into_two_digit_hexadecimal(red) + Convert_into_two_digit_hexadecimal(green) + Convert_into_two_digit_hexadecimal(blue);
        }

        public static string Get_hexadecimal_light_blue()
        {
            return Get_hexadecimal_code(23, 141, 214);
        }

        public static string Get_hexadecimal_light_red()
        {
            return Get_hexadecimal_code(230, 0, 0);
        }

        public static string Get_hexadecimal_light_green()
        {
            return Get_hexadecimal_code(134, 196, 64);
        }

        public static string Get_hexadecimal_bright_green()
        {
            return Get_hexadecimal_code(0, 255, 0);
        }

        public static string Get_hexadecimal_dark_green()
        {
            return Get_hexadecimal_code(0, 100, 0);
        }

        public static string Get_hexadecimal_black()
        {
            return Get_hexadecimal_code(0, 0, 0);
        }

        public static float Get_median(float[] values)
        {
            values = values.OrderBy(l => l).ToArray();
            int values_length = values.Length;
            if (values_length == 0)
            {
                throw new InvalidOperationException("Empty collection");
            }
            else if (values_length % 2 == 0)
            {
                // count is even, average two middle elements
                float a = values[(values_length / 2) - 1];
                float b = values[(values_length / 2)];
                return (a + b) / 2;
            }
            else if (values_length == 1)
            {
                return values[0];
            }
            else
            {
                // count is odd, return the middle element
                float return_value = values[(int)Math.Floor((double)values_length / (double)2)];
                return return_value;
            }
        }

        public static float Get_average(float[] values)
        {
            int values_length = values.Length;
            float sum = 0;
            for (int indexV = 0; indexV < values_length; indexV++)
            {
                sum += values[indexV];
            }
            return sum / (float)values_length;
        }

        public static void Get_mean_and_sd(float[] values, out float mean, out float sd)
        {
            int values_length = values.Length;
            float sum = 0;
            double sum_of_squares = 0;
            for (int indexV = 0; indexV < values_length; indexV++)
            {
                sum += values[indexV];
                sum_of_squares += Math.Pow(values[indexV], 2);
            }
            mean = sum / (float)values_length;
            double sd_double = (float)Math.Sqrt(sum_of_squares / (float)values_length - Math.Pow(mean, 2));
            sd = (float)(sd_double * (float)(values_length - 1) / (float)values_length);
        }

        public static void Get_max_min_of_array(float[] array, out float max, out float min)
        {
            int array_length = array.Length;
            max = -1;
            min = -1;
            float array_entry;
            for (int indexA = 0; indexA < array_length; indexA++)
            {
                array_entry = array[indexA];
                if ((max == -1) || (array_entry > max))
                {
                    max = array_entry;
                }
                if ((min == -1) || (array_entry < min))
                {
                    min = array_entry;
                }
            }
        }
    }

}
