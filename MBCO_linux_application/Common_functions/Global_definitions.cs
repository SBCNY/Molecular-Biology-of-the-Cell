//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Common_functions.Global_definitions
{
    public enum ReadWrite_report_enum { E_m_p_t_y = 0, Report_nothing, Report_main = 1, Report_everything = 2 }
    public enum Ontology_type_enum { E_m_p_t_y, Mbco_human, Mbco_mouse, Mbco_rat, 
                                                Go_bp_human, Go_mf_human, Go_cc_human,
                                                Mbco_na_glucose_tm_transport_human }
    public enum Namespace_type_enum {  E_m_p_t_y, biological_process, molecular_function, cellular_component }
    enum Add_selected_item_to_name_enum {  E_m_p_t_y, Source, Order_no }
    enum Entry_type_enum { E_m_p_t_y, Up, Down };
    enum Enrichment_algorithm_enum { E_m_p_t_y, Dynamic_enrichment, Standard_enrichment }
    enum Timeunit_enum { E_m_p_t_y = 0, sec = 1, min = 2, hrs = 3, days = 4, weeks = 5, months = 6, years = 7 }
    enum Enrichment_type_enum { E_m_p_t_y, Standard, Dynamic }
    enum Dataset_attributes_enum { E_m_p_t_y, Name, Timepoint, EntryType, Color, IntegrationGroup, Substring, SourceFile, BgGenes, Delete, Datasets_count, Genes_count, Dataset_order_no }
    enum Enrichment_value_type_enum { E_m_p_t_y, Fractional_rank, Minus_log10_pvalue }
    enum Manual_validation_enum {  True_positive, Custom_gene_scp_association };
    enum Order_of_values_for_signficance_enum { E_m_p_t_y, Higher_abs_values_are_more_significant, Lower_abs_values_are_more_significant }
    enum Significance_status_enum { E_m_p_t_y, Yes, No, Undetermined }
    enum Value_importance_order_enum { E_m_p_t_y, Value_1st_2nd, Value_2nd_1st }

    class Global_class
    {
        private const string empty_entry = "E_m_p_t_y";  //check enums, Empty has to be the same!!
        private const char tab = '\t';
        private const char scp_delimiter = '$';
        private const string background_genes_scpName = "Background genes";
        private const string mbco_exp_background_gene_list_name = "MBCO background genes";
        private const string bgGenes_label = "_bgGenes";

        public static char Parameter_spreadsheet_array_delimiter { get { return ';'; } }
        public static char Parameter_spreadsheet_array_delimiter0 { get { return '@'; } }
        public static string Network_legend_box_label { get { return "Legend"; } }
        public static string Network_genes_box_label { get { return "Genes"; } }
        public static int Network_legend_level { get { return 100; } }
        public static int Network_genes_level { get { return 10; } }
        public static int ProcessLevel_for_all_non_MBCO_SCPs { get { return 1; } }
        public static string CustomScp_id { get { return "Custom scp"; } }
        public static string Empty_entry {  get { return empty_entry; } }
        public static char Scp_delimiter { get { return scp_delimiter; } }
        public static char Tab {  get { return tab; } }
        public static string Background_genes_scpName { get { return background_genes_scpName; } }
        public static System.Drawing.Color Intermediate_network_scps_color {  get { return System.Drawing.Color.White; } }
        public static string Get_complete_sampleName(string integrationGroup, Entry_type_enum entryType, float timepoint, Timeunit_enum timeunit, string sampleName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(integrationGroup);
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
            sb.AppendFormat(timepoint.ToString() + " " + timeunit);
            return sb.ToString();
        }

        public static Dictionary<int, System.Drawing.Color> Get_level_scpColor_dict()
        {
            Dictionary<int, System.Drawing.Color> level_scpColor_dict = new Dictionary<int, System.Drawing.Color>();
            level_scpColor_dict.Add(0, System.Drawing.Color.Transparent);
            level_scpColor_dict.Add(1, System.Drawing.Color.DarkRed);
            level_scpColor_dict.Add(2, System.Drawing.Color.OrangeRed);
            level_scpColor_dict.Add(3, System.Drawing.Color.DodgerBlue);
            level_scpColor_dict.Add(4, System.Drawing.Color.LimeGreen);

            return level_scpColor_dict;
        }

        public static string Mbco_exp_background_gene_list_name {  get { return mbco_exp_background_gene_list_name; } }
        public static string Bg_genes_label {  get { return bgGenes_label; } }
    }

    class Color_conversion_class
    {
        public static string Get_color_string(System.Drawing.Color color)
        {
            string color_string = color.ToString().Replace("Color ", "").Replace("[","").Replace("]","");
            return color_string;
        }

        public static System.Drawing.Color Set_color_from_string(string color_string)
        {
            System.Drawing.Color return_color = System.Drawing.Color.FromName(color_string);
            return return_color;
        }
    }

    class Hexadecimal_color_class
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
                    throw new Exception("not considered");
            }
            return sign;
        }

        private static string Convert_into_two_digit_hexadecimal(int number)
        {
            if ((number > 255) || (number < 0))
            {
                throw new Exception("number is not between0 and 255");
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

        public static string Get_hexadecimal_code_for_color(System.Drawing.Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }
    }

    class Ontology_classification_class
    {
        public static string Get_name_of_scps_for_progress_report(Ontology_type_enum ontology)
        {
            switch (ontology)
            {
                case Ontology_type_enum.Go_bp_human:
                    return "GO biological processes";
                case Ontology_type_enum.Go_mf_human:
                    return "GO molecular function";
                case Ontology_type_enum.Go_cc_human:
                    return "GO cellular component";
                case Ontology_type_enum.Mbco_human:
                case Ontology_type_enum.Mbco_mouse:
                case Ontology_type_enum.Mbco_rat:
                    return "MBCO SCPs";
                case Ontology_type_enum.Mbco_na_glucose_tm_transport_human:
                    return "Na & Glucose TM transport SCPs";
                default:
                    throw new Exception();
            }
        }
        public static string Get_loadAndPrepare_report_for_enrichment(Ontology_type_enum ontology)
        {
            switch (ontology)
            {
                case Ontology_type_enum.Go_bp_human:
                    return "Loading and preparing GO biological processes";
                case Ontology_type_enum.Go_mf_human:
                    return "Loading and preparing GO molecular functions";
                case Ontology_type_enum.Go_cc_human:
                    return "Loading and preparing GO cellular components";
                case Ontology_type_enum.Mbco_human:
                case Ontology_type_enum.Mbco_mouse:
                case Ontology_type_enum.Mbco_rat:
                    return "Loading and preparing MBCO subcellular processes (SCPs)";
                case Ontology_type_enum.Mbco_na_glucose_tm_transport_human:
                    return "Loading and preparing Na & Glucose transmembrane (TM) transport processes";
                default:
                    throw new Exception();
            }
        }

        public static string Get_loadAndPrepare_report_for_network(Ontology_type_enum ontology)
        {
            switch (ontology)
            {
                case Ontology_type_enum.Go_bp_human:
                    return "Loading and preparing networks for GO biological processes";
                case Ontology_type_enum.Go_mf_human:
                    return "Loading and preparing networks for GO molecular functions";
                case Ontology_type_enum.Go_cc_human:
                    return "Loading and preparing networks for GO cellular components";
                case Ontology_type_enum.Mbco_human:
                case Ontology_type_enum.Mbco_mouse:
                case Ontology_type_enum.Mbco_rat:
                    return "Loading and preparing MBCO networks";
                case Ontology_type_enum.Mbco_na_glucose_tm_transport_human:
                    return "Loading and preparing networks for Na & Glucose TM transport";
                default:
                    throw new Exception();
            }
        }

        public static Ontology_type_enum Get_related_human_ontology(Ontology_type_enum ontology)
        {
            Ontology_type_enum human_ontology = Ontology_type_enum.E_m_p_t_y;
            switch (ontology)
            {
                case Ontology_type_enum.Mbco_mouse:
                case Ontology_type_enum.Mbco_rat:
                case Ontology_type_enum.Mbco_human:
                    human_ontology = Ontology_type_enum.Mbco_human;
                    break;
                case Ontology_type_enum.Mbco_na_glucose_tm_transport_human:
                    human_ontology = Ontology_type_enum.Mbco_na_glucose_tm_transport_human;
                    break;
                case Ontology_type_enum.Go_bp_human:
                    human_ontology = Ontology_type_enum.Go_bp_human;
                    break;
                case Ontology_type_enum.Go_mf_human:
                    human_ontology = Ontology_type_enum.Go_mf_human;
                    break;
                case Ontology_type_enum.Go_cc_human:
                    human_ontology = Ontology_type_enum.Go_cc_human;
                    break;
                default:
                    throw new Exception();
            }
            return human_ontology;
        }

        public static bool Is_mbco_ontology(Ontology_type_enum ontology)
        {
            switch (ontology)
            {
                case Ontology_type_enum.Mbco_human:
                case Ontology_type_enum.Mbco_mouse:
                case Ontology_type_enum.Mbco_rat:
                    return true;
                case Ontology_type_enum.Go_bp_human:
                case Ontology_type_enum.Go_mf_human:
                case Ontology_type_enum.Go_cc_human:
                case Ontology_type_enum.Mbco_na_glucose_tm_transport_human:
                    return false;
                default:
                    throw new Exception();
            }
        }
        public static bool Is_go_ontology(Ontology_type_enum ontology)
        {
            switch (ontology)
            {
                case Ontology_type_enum.Go_bp_human:
                case Ontology_type_enum.Go_cc_human:
                case Ontology_type_enum.Go_mf_human:
                    return true;
                case Ontology_type_enum.Mbco_human:
                case Ontology_type_enum.Mbco_mouse:
                case Ontology_type_enum.Mbco_rat:
                case Ontology_type_enum.Mbco_na_glucose_tm_transport_human:
                    return false;
                default:
                    throw new Exception();
            }
        }
    }

    class Global_directory_and_file_class
    {
        #region constant directory
        public char Delimiter { get; private set; }
        private char Not_used_delimiter { get; set; }
        private string Custom_data_subdirectory { get; set; }
        private string Mbco_datasets_subdirectory { get; set; }
        private string Specialized_mbco_datasets_subdirectory { get; set; }
        private string Go_datasets_subdirectory { get; set; }
        private string Results_subdirectory { get; set; }
        #endregion

        #region Custom data and results subdirectories
        private string Nog_subdirectory { get; set; }
        private string Kpmp_subdirectory { get; set; }
        private string Lincs_subdirectory { get; set; }
        #endregion

        #region MBCO files
        public string Mbco_v11_obo_fileName { get; private set; }
        public string Mbco_human_association_v11_fileName { get; private set; }
        public string Mbco_mouse_association_v11_fileName { get; private set; }
        public string Mbco_rat_association_v11_fileName { get; private set; }
        public string Mbco_inferred_scp_relationships_v11_fileName { get; private set; }
        #endregion

        #region Specialized mbco datasets
        public string Na_glucose_tm_transport_parentChild_fileName { get; private set; }
        public string Na_glucose_tm_transport_parentChild_minimum_fileName { get; private set; }
        public string Na_glucose_tm_transport_fileName { get; set; }
        public string Na_glucose_tm_transport_minimum_fileName { get; set; }
        #endregion

        #region Gene Ontology files
        public string Go_obo_2022_fileName { get; private set; }
        public string Go_human_association_2022_downloaded_fileName { get; private set; }
        public string Go_bp_human_association_2022_populatedParents_fileName { get; private set; }
        public string Go_mf_human_association_2022_populatedParents_fileName { get; private set; }
        public string Go_cc_human_association_2022_populatedParents_fileName { get; private set; }
        #endregion

        #region Result files
        public string Mbco_parameter_settings_fileName { get; private set; }
        public string FirstLine_of_mbco_parameter_setting_fileName { get; private set; }
        public string SCP_abbreviations_fileName { get; private set; }
        public string SCP_abbreviations_fileName_reference { get; private set; }
        #endregion

        public Global_directory_and_file_class()
        {
            string current_directory = System.Environment.CurrentDirectory;
            if (current_directory.IndexOf('/')!=-1)
            {
                Delimiter = '/';
                Not_used_delimiter = '\\';
            }
            else
            {
                Delimiter = '\\';
                Not_used_delimiter = '/';
            }

            Go_datasets_subdirectory = "GO_datasets" + Delimiter;
            Custom_data_subdirectory = "Custom_data" + Delimiter;
            Mbco_datasets_subdirectory = "MBCO_datasets" + Delimiter;
            Specialized_mbco_datasets_subdirectory = "MBCO_specialized_datasets" + Delimiter;
            Results_subdirectory = "Results" + Delimiter;
            Nog_subdirectory = "Neurite_outgrowth" + Delimiter;
            Kpmp_subdirectory = "KPMP_reference_atlas" + Delimiter;
            Lincs_subdirectory = "DToxS_svd_drug_signatures" + Delimiter;
            Mbco_v11_obo_fileName = "MBCO_v1.1_SCP_hierarchy.txt";
            Mbco_human_association_v11_fileName = "MBCO_v1.1_gene-SCP_associations_human.txt";
            Mbco_mouse_association_v11_fileName = "MBCO_v1.1_gene-SCP_associations_mouse.txt";
            Mbco_rat_association_v11_fileName = "MBCO_v1.1_gene-SCP_associations_rat.txt";
            Mbco_inferred_scp_relationships_v11_fileName = "MBCO_v1.1_inferred_SCP_relationships.txt";
            Na_glucose_tm_transport_fileName = "NaAndGluTMTransport_scpGeneAssociations.txt";
            Na_glucose_tm_transport_minimum_fileName = "NaAndGluTMTransport_scpGeneAssociations_original.txt";
            Na_glucose_tm_transport_parentChild_fileName = "NaAndGluTMTransport_scpHierarchy.txt";
            Na_glucose_tm_transport_parentChild_minimum_fileName = "NaAndGluTMTransport_scpHierarchy_original.txt";
            Go_obo_2022_fileName = "go-basic.obo";
            Go_human_association_2022_downloaded_fileName = "goa_human.gaf";
            Go_bp_human_association_2022_populatedParents_fileName = "GO_bp_human_for_MBCO_PathNet.txt";
            Go_mf_human_association_2022_populatedParents_fileName = "GO_mf_human_for_MBCO_PathNet.txt";
            Go_cc_human_association_2022_populatedParents_fileName = "GO_cc_human_for_MBCO_PathNet.txt";
            SCP_abbreviations_fileName = "SCP_abbreviations.txt";
            SCP_abbreviations_fileName_reference = "Abb: '" + SCP_abbreviations_fileName + "'";
            Mbco_parameter_settings_fileName = "MBCOApp_parameter_settings.txt";
            FirstLine_of_mbco_parameter_setting_fileName = "#MBCO parameter settings for re-import into application";
        }

        public string MbcoApp_major_directory
        {
            get { return System.Environment.CurrentDirectory + Delimiter; }
        }
        public string Results_directory
        {
            get { return MbcoApp_major_directory + Results_subdirectory; }
        }
        public string Custom_data_directory
        {
            get { return MbcoApp_major_directory + Custom_data_subdirectory; }
        }
        #region MBCO dataset fileNames
        public string Complete_mbco_v11_obo_fileName
        {
            get { return MbcoApp_major_directory + Mbco_datasets_subdirectory + Mbco_v11_obo_fileName; }
        }
        public string Complete_human_mbco_association_v11_fileName
        {
            get { return MbcoApp_major_directory + Mbco_datasets_subdirectory + Mbco_human_association_v11_fileName; }
        }

        public string Complete_mouse_mbco_association_v11_fileName
        {
            get { return MbcoApp_major_directory + Mbco_datasets_subdirectory + Mbco_mouse_association_v11_fileName; }
        }

        public string Complete_rat_mbco_association_v11_fileName
        {
            get { return MbcoApp_major_directory + Mbco_datasets_subdirectory + Mbco_rat_association_v11_fileName; }
        }
        public string Complete_mbco_inferred_scp_relationships_v11_fileName
        {
            get { return MbcoApp_major_directory + Mbco_datasets_subdirectory + Mbco_inferred_scp_relationships_v11_fileName; }
        }
        #endregion

        #region Specialized mbco dataset fileNames
        public string Complete_na_glucose_tm_transport_parentChild_fileName
        {
            get { return MbcoApp_major_directory + Specialized_mbco_datasets_subdirectory + Na_glucose_tm_transport_parentChild_fileName; }
        }
        public string Complete_na_glucose_tm_transport_parentChild_minimum_fileName
        {
            get { return MbcoApp_major_directory + Specialized_mbco_datasets_subdirectory + Na_glucose_tm_transport_parentChild_minimum_fileName; }
        }
        public string Complete_na_glucose_tm_transport_fileName
        {
            get { return MbcoApp_major_directory + Specialized_mbco_datasets_subdirectory + Na_glucose_tm_transport_fileName; }
        }
        public string Complete_na_glucose_tm_transport_minimum_fileName
        {
            get { return MbcoApp_major_directory + Specialized_mbco_datasets_subdirectory + Na_glucose_tm_transport_minimum_fileName; }
        }
        #endregion


        #region GO dataset fileNames
        public string Complete_go_obo_fileName
        {
            get { return MbcoApp_major_directory + Go_datasets_subdirectory + Go_obo_2022_fileName; }
        }
        public string Complete_human_go_association_2022_downloaded_fileName
        {
            get { return MbcoApp_major_directory + Go_datasets_subdirectory + Go_human_association_2022_downloaded_fileName; }
        }
        public string Complete_human_go_bp_association_2022_populatedParents_fileName
        {
            get { return MbcoApp_major_directory + Go_datasets_subdirectory + Go_bp_human_association_2022_populatedParents_fileName; }
        }
        public string Complete_human_go_mf_association_2022_populatedParents_fileName
        {
            get { return MbcoApp_major_directory + Go_datasets_subdirectory + Go_mf_human_association_2022_populatedParents_fileName; }
        }
        public string Complete_human_go_cc_association_2022_populatedParents_fileName
        {
            get { return MbcoApp_major_directory + Go_datasets_subdirectory + Go_cc_human_association_2022_populatedParents_fileName; }
        }
        #endregion

        public string Get_complete_fileName_of_gene_association_parentsPopulatedWithChildGenes(Ontology_type_enum ontology)
        {
            string complete_fileName = "";
            switch (ontology)
            {
                case Ontology_type_enum.Mbco_human:
                    complete_fileName = Complete_human_mbco_association_v11_fileName;
                    break;
                case Ontology_type_enum.Mbco_mouse:
                    complete_fileName = Complete_mouse_mbco_association_v11_fileName;
                    break;
                case Ontology_type_enum.Mbco_rat:
                    complete_fileName = Complete_rat_mbco_association_v11_fileName;
                    break;
                case Ontology_type_enum.Mbco_na_glucose_tm_transport_human:
                    complete_fileName = Complete_na_glucose_tm_transport_fileName;
                    break;
                case Ontology_type_enum.Go_bp_human:
                    complete_fileName = Complete_human_go_bp_association_2022_populatedParents_fileName;
                    break;
                case Ontology_type_enum.Go_cc_human:
                    complete_fileName = Complete_human_go_cc_association_2022_populatedParents_fileName;
                    break;
                case Ontology_type_enum.Go_mf_human:
                    complete_fileName = Complete_human_go_mf_association_2022_populatedParents_fileName;
                    break;
                default:
                    throw new Exception();
            }
            return complete_fileName;
        }

        public string Get_complete_fileName_of_minimum_gene_association_parentsPopulatedWithChildGenes(Ontology_type_enum ontology)
        {
            string complete_fileName = "";
            switch (ontology)
            {
                case Ontology_type_enum.Mbco_na_glucose_tm_transport_human:
                    complete_fileName = Complete_na_glucose_tm_transport_minimum_fileName;
                    break;
                default:
                    throw new Exception();
            }
            return complete_fileName;
        }

        public string Transform_into_compatible_directory(string directoryOrDirectoryPlusFile)
        {
            directoryOrDirectoryPlusFile = directoryOrDirectoryPlusFile.Replace(Not_used_delimiter, Delimiter);
            if (  (!directoryOrDirectoryPlusFile[directoryOrDirectoryPlusFile.Length - 1].Equals(Delimiter))
                &&(String.IsNullOrEmpty(System.IO.Path.GetExtension(directoryOrDirectoryPlusFile))))
            {
                directoryOrDirectoryPlusFile = directoryOrDirectoryPlusFile + Delimiter.ToString();
            }
            return directoryOrDirectoryPlusFile;
        }

        #region Get Custom input and results data directories
        public string Get_custom_data_nog_directory()
        {
            return Custom_data_directory + Nog_subdirectory;
        }
        public string Get_custom_data_kpmp_directory()
        {
            return Custom_data_directory + Kpmp_subdirectory;
        }
        public string Get_custom_data_lincs_directory()
        {
            return Custom_data_directory + Lincs_subdirectory;
        }
        public string Get_custom_results_nog_directory()
        {
            return Results_directory + Nog_subdirectory;
        }
        public string Get_custom_results_kpmp_directory()
        {
            return Results_directory + Kpmp_subdirectory;
        }
        public string Get_custom_results_lincs_directory()
        {
            return Results_directory + Lincs_subdirectory;
        }
        #endregion

        public string Get_results_subdirectory_for_indicated_ontology(Ontology_type_enum ontology, string base_file_name)
        {
            if (base_file_name.Contains(":"))
            {
                return base_file_name;
            }
            else
            {
                return base_file_name + Delimiter;
            }
        }

        public string Get_results_subdirectory_for_integrative_dynamic_scps_networks(Ontology_type_enum ontology, string base_file_name)
        {
            return Get_results_subdirectory_for_indicated_ontology(ontology, base_file_name);
        }


    }

    class Timeunit_conversion_class
    {
        public static void Get_lowest_and_highest_timeunit(Timeunit_enum[] timeunits, out Timeunit_enum lowest_timeunit, out Timeunit_enum highest_timeunit)
        {
            timeunits = timeunits.OrderBy(l => l).ToArray();
            lowest_timeunit = timeunits[0];
            highest_timeunit = timeunits[timeunits.Length-1];
        }

        public static float Convert_timepoint_from_old_unit_to_new_unit(float old_timepoint, Timeunit_enum old_timeunit, Timeunit_enum new_timeunit)
        {
            int timepoint_compare;
            double weeks_per_month = (3*365+366)/ (4 * 7.0);//3 regular and one leap year
            timepoint_compare = new_timeunit.CompareTo(old_timeunit);
            if ((old_timeunit.Equals(Timeunit_enum.sec)) && (timepoint_compare > 0))
            {
                old_timepoint = old_timepoint / 60;
                old_timeunit = Timeunit_enum.min;
            }
            timepoint_compare = new_timeunit.CompareTo(old_timeunit);
            if ((old_timeunit.Equals(Timeunit_enum.min)) && (timepoint_compare > 0))
            {
                old_timepoint = old_timepoint / 60;
                old_timeunit = Timeunit_enum.hrs;
            }
            timepoint_compare = new_timeunit.CompareTo(old_timeunit);
            if ((old_timeunit.Equals(Timeunit_enum.hrs)) && (timepoint_compare > 0))
            {
                old_timepoint = old_timepoint / 24;
                old_timeunit = Timeunit_enum.days;
            }
            timepoint_compare = new_timeunit.CompareTo(old_timeunit);
            if ((old_timeunit.Equals(Timeunit_enum.days)) && (timepoint_compare > 0))
            {
                old_timepoint = (float)((double)old_timepoint / 7);
                old_timeunit = Timeunit_enum.weeks;
            }
            timepoint_compare = new_timeunit.CompareTo(old_timeunit);
            if ((old_timeunit.Equals(Timeunit_enum.weeks)) && (timepoint_compare > 0))
            {
                old_timepoint = (float)((double)old_timepoint / weeks_per_month);
                old_timeunit = Timeunit_enum.months;
            }
            timepoint_compare = new_timeunit.CompareTo(old_timeunit);
            if ((old_timeunit.Equals(Timeunit_enum.months)) && (timepoint_compare > 0))
            {
                old_timepoint = old_timepoint / 12;
                old_timeunit = Timeunit_enum.years;
            }

            timepoint_compare = new_timeunit.CompareTo(old_timeunit);
            if ((old_timeunit.Equals(Timeunit_enum.years)) && (timepoint_compare < 0))
            {
                old_timepoint = old_timepoint * 12;
                old_timeunit = Timeunit_enum.months;
            }
            timepoint_compare = new_timeunit.CompareTo(old_timeunit);
            if ((old_timeunit.Equals(Timeunit_enum.months)) && (timepoint_compare < 0))
            {
                old_timepoint = (float)((double)old_timepoint * weeks_per_month);
                old_timeunit = Timeunit_enum.weeks;
            }
            timepoint_compare = new_timeunit.CompareTo(old_timeunit);
            if ((old_timeunit.Equals(Timeunit_enum.weeks)) && (timepoint_compare < 0))
            {
                old_timepoint = old_timepoint * 7;
                old_timeunit = Timeunit_enum.days;
            }
            timepoint_compare = new_timeunit.CompareTo(old_timeunit);
            if ((old_timeunit.Equals(Timeunit_enum.days)) && (timepoint_compare < 0))
            {
                old_timepoint = old_timepoint * 24;
                old_timeunit = Timeunit_enum.hrs;
            }
            timepoint_compare = new_timeunit.CompareTo(old_timeunit);
            if ((old_timeunit.Equals(Timeunit_enum.hrs)) && (timepoint_compare < 0))
            {
                old_timepoint = old_timepoint * 60;
                old_timeunit = Timeunit_enum.min;
            }
            timepoint_compare = new_timeunit.CompareTo(old_timeunit);
            if ((old_timeunit.Equals(Timeunit_enum.min)) && (timepoint_compare < 0))
            {
                old_timepoint = old_timepoint * 60;
                old_timeunit = Timeunit_enum.sec;
            }
            return old_timepoint;
        }

        public static float Get_timepoint_in_days(float timepoint, Timeunit_enum timeunit)
        { 
            float timepoint_in_days = -1;
            switch (timeunit)
            {
                case Timeunit_enum.sec:
                    timepoint_in_days = timepoint / (60 * 60 * 24);
                    break;
                case Timeunit_enum.min:
                    timepoint_in_days = timepoint / (60 * 24);
                    break;
                case Timeunit_enum.hrs:
                    timepoint_in_days = timepoint / 24;
                    break;
                case Timeunit_enum.days:
                    timepoint_in_days = timepoint;
                    break;
                case Timeunit_enum.weeks:
                    timepoint_in_days = timepoint * 7;
                    break;
                case Timeunit_enum.months:
                    timepoint_in_days = timepoint * 30;
                    break;
                case Timeunit_enum.years:
                    timepoint_in_days = timepoint * 365;
                    break;
                default:
                    timepoint_in_days = timepoint;
                    break;
            }
            return timepoint_in_days;
        }

        public static Timeunit_enum Convert_timepoint_string_to_timeunit(string timepoint_string)
        {
            switch (timepoint_string.ToLower())
            {
                case "s":
                case "sec":
                case "second":
                case "seconds":
                    return Timeunit_enum.sec;
                case "min":
                case "minutes":
                case "minute":
                    return Timeunit_enum.min;
                case "h":
                case "hrs":
                case "hour":
                case "hours":
                    return Timeunit_enum.hrs;
                case "d":
                case "day":
                case "days":
                    return Timeunit_enum.days;
                case "w":
                case "week":
                case "weeks":
                    return Timeunit_enum.weeks;
                case "month":
                case "months":
                    return Timeunit_enum.months;
                case "y":
                case "year":
                case "years":
                    return Timeunit_enum.years;
                default:
                    return Timeunit_enum.E_m_p_t_y;
            }
        }
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

        public static void Get_mean_and_sampleSD(float[] values, out float mean, out float sd)
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
