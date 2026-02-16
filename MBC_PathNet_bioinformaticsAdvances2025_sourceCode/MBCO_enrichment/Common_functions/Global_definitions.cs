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
using System.Globalization;
using System.Windows.Forms;
using System.Linq.Expressions;
using Enrichment;
using Data;
using Common_functions.Array_own;
using Network;
using MBCO;
using System.Web;
using Common_functions.Form_tools;
using yed_network;
using System.Drawing;

namespace Common_functions.Global_definitions
{
    public enum ReadWrite_report_enum { E_m_p_t_y = 0, Report_nothing, Report_main = 1, Report_everything = 2 }
    public enum Ontology_type_enum { E_m_p_t_y, Mbco, 
                                                Go_bp, Go_mf, Go_cc,
                                                Reactome,
                                                Mbco_na_glucose_tm_transport,
                                                Custom_1, Custom_2 }
    //public enum Organism_enum { E_m_p_t_y, All, Bos_taurus = 9913, Caenorhabditis_elegans = 247477, Canis_lupus_familiaris = 9615, Danio_rerio = 7955, Dictyostelium_discoideum = 44689, Drosophila_melanogaster = 7227, Homo_sapiens = 9606, Gallus_gallus, Mus_musculus = 10090, Rattus_norvegicus = 10116, Sus_scrofa = 9823, Xenopus_tropicalis = 8364 }
    public enum Options_menu_enum { E_m_p_t_y, Read_data };
        
    public enum Organism_enum { E_m_p_t_y, Bos_taurus = 9913,
                                           Caenorhabditis_elegans = 6239,
                                           Canis_lupus_familiaris = 9615, 
                                           Danio_rerio = 7955, 
                                           Drosophila_melanogaster = 7227,
                                           Homo_sapiens = 9606, 
                                           Gallus_gallus = 9031,
                                           Mus_musculus = 10090,
                                           Rattus_norvegicus = 10116,
                                           Sus_scrofa = 9823,
                                           Xenopus_tropicalis = 8364 }
    public enum Namespace_type_enum {  E_m_p_t_y, Biological_process, Molecular_function, Cellular_component, Go_overall_parent }
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
    enum Species_selection_order_enum {  E_m_p_t_y, Insist_on_file, Generate_orthologues_if_missing, Always_generate_orthologues }

    class Global_class
    {
        public static bool Do_internal_checks { get { return false; } }
        public static bool Check_ordering { get { return true; } }
        public static bool Check_population_of_parents_with_children_genes { get { return false; } }

        private const string empty_entry = "E_m_p_t_y";  //check enums, Empty has to be the same!!
        private const char tab = '\t';
        private const char space = ' ';
        private const char scp_delimiter = '$';
        private const string mbco_exp_background_gene_list_name = "MBCO background genes";
        private const string bgGenes_label = "_bgGenes";

        public static char Parameter_spreadsheet_array_delimiter { get { return ';'; } }
        public static char Parameter_spreadsheet_array_delimiter0 { get { return '@'; } }
        public static string Network_legend_box_label { get { return "Legend"; } }
        public static string Network_genes_box_label { get { return "Genes"; } }
        public static int Network_legend_level { get { return -100; } }
        public static int Network_genes_level { get { return -10; } }
        public static int Not_assigned_level { get { return -1; } }
        public static int ProcessLevel_for_all_non_MBCO_SCPs { get { return 1; } }
        public static string CustomScp_id { get { return "Custom scp"; } }
        public static string Empty_entry {  get { return empty_entry; } }
        public static char Scp_delimiter { get { return scp_delimiter; } }
        public static char Tab {  get { return tab; } }
        public static char Space {  get { return space; } }
        public static System.Drawing.Color Intermediate_network_scps_color { get { return System.Drawing.Color.White; } }
        public static System.Drawing.Color Legend_node_size_color { get { return System.Drawing.Color.White; } }
        public static float Intermediate_node_size { get { return -(float)Math.Log10(0.05); } }
        public static float Edge_width_default {  get { return 2; } }
        public static NWedge_type_enum Functional_scp_network_default_nw_edge_line_type {  get { return NWedge_type_enum.Solid_line; } }
        public static float Legend_node_size { get { return -(float)Math.Log10(0.01); } }
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
            level_scpColor_dict.Add(0, System.Drawing.Color.White);
            level_scpColor_dict.Add(1, System.Drawing.Color.DarkRed);
            level_scpColor_dict.Add(2, System.Drawing.Color.OrangeRed);
            level_scpColor_dict.Add(3, System.Drawing.Color.DodgerBlue);
            level_scpColor_dict.Add(4, System.Drawing.Color.LimeGreen);

            return level_scpColor_dict;
        }
        public static Dictionary<Ontology_type_enum, System.Drawing.Color> Get_ontology_scpColor_dict()
        {
            Dictionary<Ontology_type_enum, System.Drawing.Color> ontology_scpColor_dict = new Dictionary<Ontology_type_enum, System.Drawing.Color>();
            ontology_scpColor_dict.Add(Ontology_type_enum.Go_mf, System.Drawing.Color.DodgerBlue);
            ontology_scpColor_dict.Add(Ontology_type_enum.Go_bp, System.Drawing.Color.DodgerBlue);
            ontology_scpColor_dict.Add(Ontology_type_enum.Go_cc, System.Drawing.Color.DodgerBlue);
            ontology_scpColor_dict.Add(Ontology_type_enum.Reactome, System.Drawing.Color.DodgerBlue);
            ontology_scpColor_dict.Add(Ontology_type_enum.Custom_1, System.Drawing.Color.DodgerBlue);
            ontology_scpColor_dict.Add(Ontology_type_enum.Custom_2, System.Drawing.Color.DodgerBlue);
            ontology_scpColor_dict.Add(Ontology_type_enum.Mbco_na_glucose_tm_transport, System.Drawing.Color.DodgerBlue);

            return ontology_scpColor_dict;
        }

        public static string Mbco_exp_background_gene_list_name {  get { return mbco_exp_background_gene_list_name; } }
        public static string Bg_genes_label {  get { return bgGenes_label; } }
    }


    interface ISet_uniqueDatasetName_line
    {
        string IntegrationGroup { get; }
        string SampleName { get; }
        float TimepointInDays { get; }
        float Timepoint { get; }
        Timeunit_enum Timeunit { get; }
        Entry_type_enum EntryType { get; set; }
        string Unique_dataset_name { get; set; }

    }

    class Dataset_setUniqueDatasetName_class
    {
        public ISet_uniqueDatasetName_line[] Set_unique_datasetName_within_each_integrationGroup1(ISet_uniqueDatasetName_line[] dataset_lines)
        {
            int dataset_lines_length = dataset_lines.Length;
            ISet_uniqueDatasetName_line dataset_line;
            ISet_uniqueDatasetName_line inner_dataset_line;
            //this.Order_by_integrationGroup();
            dataset_lines = dataset_lines.OrderBy(l => l.IntegrationGroup).ToArray();
            Dictionary<float, bool> timepointInDays_dict = new Dictionary<float, bool>();
            Dictionary<Entry_type_enum, bool> entryTypes_dict = new Dictionary<Entry_type_enum, bool>();
            int firstIndexSameIntegrationGroup = -1;
            bool add_timepoints;
            bool add_entryTypes;
            System.Text.StringBuilder uniqueName_sb = new System.Text.StringBuilder();
            float timepointInDays;
            for (int indexDataset = 0; indexDataset < dataset_lines_length; indexDataset++)
            {
                dataset_line = dataset_lines[indexDataset];
                timepointInDays = dataset_line.TimepointInDays;
                if ((indexDataset == 0) || (!dataset_line.IntegrationGroup.Equals(dataset_lines[indexDataset - 1].IntegrationGroup)))
                {
                    timepointInDays_dict.Clear();
                    entryTypes_dict.Clear();
                    firstIndexSameIntegrationGroup = indexDataset;
                }
                if (!timepointInDays_dict.ContainsKey(timepointInDays))
                {
                    timepointInDays_dict.Add(timepointInDays, true);
                }
                if (!entryTypes_dict.ContainsKey(dataset_line.EntryType))
                {
                    entryTypes_dict.Add(dataset_line.EntryType, true);
                }
                if (  (indexDataset == dataset_lines_length - 1)
                    ||(!dataset_line.IntegrationGroup.Equals(dataset_lines[indexDataset + 1].IntegrationGroup)))
                {
                    add_timepoints = timepointInDays_dict.Keys.ToArray().Length > 1;
                    add_entryTypes = entryTypes_dict.Keys.ToArray().Length > 1;
                    for (int indexInner = firstIndexSameIntegrationGroup; indexInner <= indexDataset; indexInner++)
                    {
                        inner_dataset_line = dataset_lines[indexInner];
                        uniqueName_sb.Clear();
                        uniqueName_sb.AppendFormat(inner_dataset_line.SampleName);
                        if (add_timepoints) { uniqueName_sb.AppendFormat(" - {0} {1}", inner_dataset_line.Timepoint, inner_dataset_line.Timeunit); }
                        if (add_entryTypes) { uniqueName_sb.AppendFormat(" - {0}", inner_dataset_line.EntryType); }
                        inner_dataset_line.Unique_dataset_name = uniqueName_sb.ToString();
                    }
                }
            }
            return dataset_lines;
        }
    }


    class Color_conversion_class
    {
        System.Drawing.Color[] All_csharp_colors { get; set; }

        public Color_conversion_class()
        {
            All_csharp_colors = Get_all_csharp_colors();
        }

        #region Static Conversion functions
        public static string Get_color_string(System.Drawing.Color color)
        {
            string color_string = color.ToString().Replace("Color ", "").Replace("[","").Replace("]","");
            return color_string;
        }

        public static System.Drawing.Color Set_color_from_string(string color_string)
        {
            System.Drawing.Color return_color;
            return_color = System.Drawing.Color.FromName(color_string);
            return return_color;
        }
        #endregion

        private System.Drawing.Color[] Get_all_csharp_colors()
        {
            Dictionary<System.Drawing.Color, bool> selectable_colors_dict = new Dictionary<System.Drawing.Color, bool>();
            System.Drawing.Color add_color;
            foreach (System.Reflection.PropertyInfo property in typeof(System.Drawing.Color).GetProperties())
            {
                if (property.PropertyType == typeof(System.Drawing.Color))
                {
                    add_color = (System.Drawing.Color)property.GetValue(null, null);
                    if (   (!selectable_colors_dict.ContainsKey(add_color))
                        && (!add_color.Equals(System.Drawing.Color.Transparent))
                        && (!add_color.Equals(System.Drawing.Color.White)))
                    {
                        selectable_colors_dict.Add(add_color, false);
                    }
                }
            }
            return selectable_colors_dict.Keys.ToArray();
        }

        private System.Drawing.Color Get_closest_csharp_color(int input_red, int input_green, int input_blue)
        {
            int all_colors_length = All_csharp_colors.Length;
            System.Drawing.Color current_color;
            int csharp_red = -1;
            int csharp_green = -1;
            int csharp_blue = -1;
            float current_distance;
            float minimum_distance = 999999999;
            System.Drawing.Color selected_csharp_color = System.Drawing.Color.Gray;
            for (int indexColor = 0; indexColor < all_colors_length; indexColor++)
            {
                current_color = All_csharp_colors[indexColor];
                csharp_blue = int.Parse(current_color.B.ToString());
                csharp_red = int.Parse(current_color.R.ToString());
                csharp_green = int.Parse(current_color.G.ToString());
                current_distance = (float)Math.Sqrt(Math.Pow(input_red - csharp_red, 2) + Math.Pow(input_blue - csharp_blue, 2) + Math.Pow(input_green - csharp_green, 2));
                if (current_distance < minimum_distance)
                {
                    minimum_distance = current_distance;
                    selected_csharp_color = current_color;
                }
            }
            return selected_csharp_color;
        }

        public System.Drawing.Color Get_closest_csharp_color_for_hexadecimal_color_if_exists(string color_string)
        {
            System.Drawing.Color closest_color = System.Drawing.Color.FromName(color_string);
            if (  (color_string.Substring(0, 1).Equals("#"))
                &&(color_string.Length==7))
            {
                try
                {
                    int red = int.Parse(color_string.Substring(1, 2), NumberStyles.AllowHexSpecifier);
                    int green = int.Parse(color_string.Substring(3, 2), NumberStyles.AllowHexSpecifier);
                    int blue = int.Parse(color_string.Substring(5, 2), NumberStyles.AllowHexSpecifier);
                    closest_color = Get_closest_csharp_color(red, green, blue);

                }
                catch { }
            }
            return closest_color;
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
                    throw new Exception(number + " not considered in Get_hexadecimal_sign");
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
        const string go_bp_name = "GO biological process";
        const string go_mf_name = "GO molecular function";
        const string go_cc_name = "GO cellular component";
        const string mbco_name = "MBCO";
        const string sodium_glucose_TM_name = "Na & Glucose TM transport";
        const string reactome_name = "Reactome";
        const string custom_1_name = "Custom 1";
        const string custom_2_name = "Custom 2";
        const string no_annotated_parent_scp = "No annotated parent SCP";
        const string background_genes_scp = "Background genes";
        const string dynamic_scp_combination = "Dynamic SCP combination";
        const string human = "human";
        const string mouse = "mouse";
        const string cattle = "cow";
        const string celegans = "c_elegans";
        const string dmelanogaster = "fly";
        const string ddiscoidem = "d.discoideum";
        const string mtuberculosis = "m.tuberculosis";
        const string pfalciparium = "p.falciparum";
        const string scerevisia = "s.cerevisiae";
        const string spombe = "s.pombe";
        const string zebrafish = "zebrafish";
        const string rat = "rat";
        const string pig = "pig";
        const string dog = "dog";
        const string chicken = "chicken";
        const string frog = "frog";
        const string all = "all";
        const string empty = "empty";

        public static string No_annotated_parent_scp {  get { return no_annotated_parent_scp; } }
        public static string Background_genes_scp { get { return background_genes_scp; } }
        public static string Dynamic_scp_combination { get { return dynamic_scp_combination; } }


        public static string Get_name_of_scps_for_progress_report(Ontology_type_enum ontology)
        {
            switch (ontology)
            {
                case Ontology_type_enum.Go_bp:
                    return "GO biological processes";
                case Ontology_type_enum.Go_mf:
                    return "GO molecular functions";
                case Ontology_type_enum.Go_cc:
                    return "GO cellular components";
                case Ontology_type_enum.Mbco:
                    return "MBCO SCPs";
                case Ontology_type_enum.Reactome:
                    return "Reactome pathway";
                case Ontology_type_enum.Custom_1:
                    return "Pathways of custom ontology 1";
                case Ontology_type_enum.Custom_2:
                    return "Pathways of custom ontology 2";
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                    return "Na & Glucose TM transport SCPs";
                default:
                    throw new Exception(ontology + " not considered in switch statement");
            }
        }
        public static string Get_organismString_for_enum(Organism_enum organism)
        {
            Dictionary<Organism_enum, string> organismEnum_organismString_dict = Get_organismEnum_organismString_dict();
            return organismEnum_organismString_dict[organism];
        }
        public static Dictionary<Organism_enum, string> Get_organismEnum_organismString_dict()
        {
            Dictionary<Organism_enum, string> organismEnum_organismString_dict = new Dictionary<Organism_enum, string>();
            organismEnum_organismString_dict.Add(Organism_enum.Homo_sapiens, human);
            organismEnum_organismString_dict.Add(Organism_enum.Mus_musculus, mouse);
            organismEnum_organismString_dict.Add(Organism_enum.Rattus_norvegicus, rat);
            organismEnum_organismString_dict.Add(Organism_enum.Sus_scrofa, pig);
            organismEnum_organismString_dict.Add(Organism_enum.Gallus_gallus, chicken);
            organismEnum_organismString_dict.Add(Organism_enum.Canis_lupus_familiaris, dog);
            organismEnum_organismString_dict.Add(Organism_enum.Bos_taurus, cattle);
            organismEnum_organismString_dict.Add(Organism_enum.Caenorhabditis_elegans, celegans);
            organismEnum_organismString_dict.Add(Organism_enum.Danio_rerio, zebrafish);
            organismEnum_organismString_dict.Add(Organism_enum.Drosophila_melanogaster, dmelanogaster);
            organismEnum_organismString_dict.Add(Organism_enum.Xenopus_tropicalis, frog);
            organismEnum_organismString_dict.Add(Organism_enum.E_m_p_t_y, empty);
            return organismEnum_organismString_dict;
        }
        public static Dictionary<Ontology_type_enum, string> Get_ontology_ontologyName_dict()
        {
            Dictionary<Ontology_type_enum, string> ontology_ontologyName_dict = new Dictionary<Ontology_type_enum, string>();
            ontology_ontologyName_dict.Add(Ontology_type_enum.Mbco, mbco_name);
            ontology_ontologyName_dict.Add(Ontology_type_enum.Mbco_na_glucose_tm_transport, sodium_glucose_TM_name);
            ontology_ontologyName_dict.Add(Ontology_type_enum.Reactome, reactome_name);
            ontology_ontologyName_dict.Add(Ontology_type_enum.Go_bp, go_bp_name);
            ontology_ontologyName_dict.Add(Ontology_type_enum.Go_mf, go_mf_name);
            ontology_ontologyName_dict.Add(Ontology_type_enum.Go_cc, go_cc_name);
            ontology_ontologyName_dict.Add(Ontology_type_enum.Custom_1, custom_1_name);
            ontology_ontologyName_dict.Add(Ontology_type_enum.Custom_2, custom_2_name);
            return ontology_ontologyName_dict;
        }
        public static Dictionary<string, Ontology_type_enum> Get_ontologyName_ontology_dict()
        {
            Dictionary<Ontology_type_enum, string> ontology_ontologyName_dict = Get_ontology_ontologyName_dict();
            Dictionary<string, Ontology_type_enum> ontologyName_ontology_dict = new Dictionary<string, Ontology_type_enum>();
            Ontology_type_enum[] ontologies = ontology_ontologyName_dict.Keys.ToArray();
            Ontology_type_enum ontology;
            int ontologies_length = ontologies.Length;
            for (int indexO=0; indexO<ontologies_length; indexO++)
            {
                ontology = ontologies[indexO];
                ontologyName_ontology_dict.Add(ontology_ontologyName_dict[ontology], ontology);
            }
            string name;
            name = "GO_biological_process";
            ontology = Ontology_type_enum.Go_bp;
            if (!ontologyName_ontology_dict.ContainsKey(name)) { ontologyName_ontology_dict.Add(name, ontology); }
            name = "GO_cellular_component";
            ontology = Ontology_type_enum.Go_cc;
            if (!ontologyName_ontology_dict.ContainsKey(name)) { ontologyName_ontology_dict.Add(name, ontology); }
            name = "GO_molecular_function";
            ontology = Ontology_type_enum.Go_mf;
            if (!ontologyName_ontology_dict.ContainsKey(name)) { ontologyName_ontology_dict.Add(name, ontology); }
            name = "Molecular_biology_of_the_cell_ontology";
            ontology = Ontology_type_enum.Mbco;
            if (!ontologyName_ontology_dict.ContainsKey(name)) { ontologyName_ontology_dict.Add(name, ontology); }
            name = "Molecular_biology_of_the_cell";
            ontology = Ontology_type_enum.Mbco;
            if (!ontologyName_ontology_dict.ContainsKey(name)) { ontologyName_ontology_dict.Add(name, ontology); }
            name = "GO_bp";
            ontology = Ontology_type_enum.Go_bp;
            if (!ontologyName_ontology_dict.ContainsKey(name)) { ontologyName_ontology_dict.Add(name, ontology); }
            name = "GO_cc";
            ontology = Ontology_type_enum.Go_cc;
            if (!ontologyName_ontology_dict.ContainsKey(name)) { ontologyName_ontology_dict.Add(name, ontology); }
            name = "GO_mf";
            ontology = Ontology_type_enum.Go_mf;
            if (!ontologyName_ontology_dict.ContainsKey(name)) { ontologyName_ontology_dict.Add(name, ontology); }
            name = "Reactome";
            ontology = Ontology_type_enum.Reactome;
            if (!ontologyName_ontology_dict.ContainsKey(name)) { ontologyName_ontology_dict.Add(name, ontology); }
            name = "Mbco_na_glucose";
            ontology = Ontology_type_enum.Mbco_na_glucose_tm_transport;
            if (!ontologyName_ontology_dict.ContainsKey(name)) { ontologyName_ontology_dict.Add(name, ontology); }
            name = "Mbco_na_glu";
            ontology = Ontology_type_enum.Mbco_na_glucose_tm_transport;
            if (!ontologyName_ontology_dict.ContainsKey(name)) { ontologyName_ontology_dict.Add(name, ontology); }
            name = "Mbco_sodium_glucose";
            ontology = Ontology_type_enum.Mbco_na_glucose_tm_transport;
            if (!ontologyName_ontology_dict.ContainsKey(name)) { ontologyName_ontology_dict.Add(name, ontology); }
            name = "Mbco";
            ontology = Ontology_type_enum.Mbco;
            if (!ontologyName_ontology_dict.ContainsKey(name)) { ontologyName_ontology_dict.Add(name, ontology); }

            var values = Enum.GetValues(typeof(Ontology_type_enum));
            foreach (var value in values)
            {
                ontology = (Ontology_type_enum)value;
                if (!ontology.Equals(Ontology_type_enum.E_m_p_t_y))
                {
                    name = Get_name_of_scps_for_progress_report(ontology);
                    if (!ontologyName_ontology_dict.ContainsKey(name)) { ontologyName_ontology_dict.Add(name, ontology); }
                    name = ontology.ToString();
                    if (!ontologyName_ontology_dict.ContainsKey(name)) { ontologyName_ontology_dict.Add(name, ontology); }
                }
            }

            string[] names = ontologyName_ontology_dict.Keys.ToArray();
            string name_in_lowerCase;
            string name_without_underscore;
            int names_length = names.Length;
            for (int indexN=0; indexN< names_length;indexN++)
            {
                name = names[indexN];
                name_in_lowerCase = name.ToLower();
                name_without_underscore = name_in_lowerCase.Replace("_", " ");
                if (!ontologyName_ontology_dict.ContainsKey(name_in_lowerCase))
                {
                    ontologyName_ontology_dict.Add(name_in_lowerCase, ontologyName_ontology_dict[name]);
                }
                if (!ontologyName_ontology_dict.ContainsKey(name_without_underscore))
                {
                    ontologyName_ontology_dict.Add(name_without_underscore, ontologyName_ontology_dict[name]);
                }
            }
            return ontologyName_ontology_dict;
        }
        public static Dictionary<string, Organism_enum> Get_organismString_organismEnum_dict()
        {
            Dictionary<Organism_enum, string> organismEnum_organismString_dict = Get_organismEnum_organismString_dict();
            Dictionary<string, Organism_enum> organismString_organismEnum_dict = new Dictionary<string, Organism_enum>();
            Organism_enum[] organisms = organismEnum_organismString_dict.Keys.ToArray();
            Organism_enum organism;
            int organisms_length = organisms.Length;
            for (int indexO = 0; indexO<organisms_length; indexO++)
            {
                organism = organisms[indexO];
                organismString_organismEnum_dict.Add(organismEnum_organismString_dict[organism], organism);
                if (!organismEnum_organismString_dict.ContainsKey(organism))
                {
                    organismString_organismEnum_dict.Add(organism.ToString(), organism);
                }
            }
            var values = Enum.GetValues(typeof(Organism_enum));
            string organism_string;
            foreach (var value in values)
            {
                organism = (Organism_enum)value;
                if (!organism.Equals(Organism_enum.E_m_p_t_y))
                {
                    organism_string = organism.ToString();
                    if (!organismString_organismEnum_dict.ContainsKey(organism_string)) { organismString_organismEnum_dict.Add(organism_string, organism); }
                }
            }
            string[] names = organismString_organismEnum_dict.Keys.ToArray();
            string name;
            string name_in_lowerCase;
            string name_without_underscore;
            int names_length = names.Length;
            for (int indexN = 0; indexN < names_length; indexN++)
            {
                name = names[indexN];
                name_in_lowerCase = name.ToLower();
                name_without_underscore = name_in_lowerCase.Replace("_", " ");
                if (!organismString_organismEnum_dict.ContainsKey(name_in_lowerCase))
                {
                    organismString_organismEnum_dict.Add(name_in_lowerCase, organismString_organismEnum_dict[name]);
                }
                if (!organismString_organismEnum_dict.ContainsKey(name_without_underscore))
                {
                    organismString_organismEnum_dict.Add(name_without_underscore, organismString_organismEnum_dict[name]);
                }
            }
            return organismString_organismEnum_dict;
        }
        public static bool Is_real_organism(string organism_string)
        {
            return ((!organism_string.Equals(all)) && (!organism_string.Equals(empty)));
        }
        public static string Get_name_of_ontology_plus_organism(Ontology_type_enum ontology, Organism_enum organism)
        {
            string ontology_name = Get_name_of_ontology(ontology);
            return ontology_name + "_" + Get_organismString_for_enum(organism);
        }
        public static string Get_name_of_ontology_plus_organism_without_underlines(Ontology_type_enum ontology, Organism_enum organism)
        {
            return Get_name_of_ontology_plus_organism(ontology, organism).Replace("_", " ");
        }
        public static string Get_name_of_ontology(Ontology_type_enum ontology)
        {
            Dictionary<Ontology_type_enum, string> ontology_ontologyName_dict = Get_ontology_ontologyName_dict();
            return ontology_ontologyName_dict[ontology];
        }

        public static string Get_abbreviation_of_ontology(Ontology_type_enum ontology)
        {
            string ontology_abb = "error";
            switch (ontology)
            {
                case Ontology_type_enum.Mbco:
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                case Ontology_type_enum.Reactome:
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_mf:
                case Ontology_type_enum.Go_cc:
                case Ontology_type_enum.Custom_1:
                case Ontology_type_enum.Custom_2:
                    ontology_abb = ontology.ToString();
                    break;
                default:
                    throw new Exception(ontology + " is not considered in switch statement.");
            }
            return ontology_abb;
        }
        public static string Get_name_of_ontology_pathway(Ontology_type_enum ontology)
        {
            string pathway_name = "error";
            switch (ontology)
            {
                case Ontology_type_enum.Mbco:
                    pathway_name = "SCP";
                    pathway_name = "SCP";
                    break;
                case Ontology_type_enum.Reactome:
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_mf:
                case Ontology_type_enum.Go_cc:
                case Ontology_type_enum.Custom_1:
                case Ontology_type_enum.Custom_2:
                    pathway_name = "pathway";
                    break;
                default:
                    throw new Exception(ontology + " is not considered in switch function.");
            }
            return pathway_name;
        }
        public static string Get_name_of_goHyperparameter(GO_hyperParameter_enum go_HyperParameter)
        {
            switch (go_HyperParameter)
            {
                //case GO_hyperParameter_enum.Min_level:
                //    return "minL";
                //case GO_hyperParameter_enum.Max_level:
                //    return "maxL";
                //case GO_hyperParameter_enum.Min_depth:
                //    return "minD";
                //case GO_hyperParameter_enum.Max_depth:
                //    return "maxD";
                case GO_hyperParameter_enum.Min_size:
                    return "minS";
                case GO_hyperParameter_enum.Max_size:
                    return "maxS";
                default:
                    throw new Exception(go_HyperParameter + " is not considered in switch function.");
            }
        }

        public static string Get_name_of_hierarchicalScpInteractionType(SCP_hierarchy_interaction_type_enum interaction_type)
        {
            switch (interaction_type)
            {
                case SCP_hierarchy_interaction_type_enum.Parent_child:
                    return "parentChild";
                case SCP_hierarchy_interaction_type_enum.Parent_child_regulatory:
                    return "parentChildReg";
                default:
                    throw new Exception(interaction_type + " is not considered in switch function.");
            }
        }
        public static Ontology_type_enum Get_ontology_of_namespace(Namespace_type_enum namespace_type)
        {
            switch (namespace_type)
            {
                case Namespace_type_enum.Biological_process:
                    return Ontology_type_enum.Go_bp;
                case Namespace_type_enum.Molecular_function:
                    return Ontology_type_enum.Go_mf;
                case Namespace_type_enum.Cellular_component:
                    return Ontology_type_enum.Go_cc;
                default:
                    throw new Exception(namespace_type + " is not considered in switch function.");
            }
        }
        public static Ontology_type_enum Get_ontology_of_ontology_name(string ontology_string)
        {
            switch (ontology_string)
            {
                case mbco_name:
                    return Ontology_type_enum.Mbco;
                case sodium_glucose_TM_name:
                    return Ontology_type_enum.Mbco_na_glucose_tm_transport;
                case reactome_name:
                    return Ontology_type_enum.Reactome;
                case go_bp_name:
                    return Ontology_type_enum.Go_bp;
                case go_mf_name:
                    return Ontology_type_enum.Go_mf;
                case go_cc_name:
                    return Ontology_type_enum.Go_cc;
                case custom_1_name:
                    return Ontology_type_enum.Custom_1;
                case custom_2_name:
                    return Ontology_type_enum.Custom_2;
                default:
                    throw new Exception(ontology_string + " is not considered in switch function.");
            }
        }
        public static string Get_suggested_layout_for_hierarchy_network(Ontology_type_enum ontology)
        {
            switch (ontology)
            {
                case Ontology_type_enum.Reactome:
                    return "tree - directed or circular";
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_mf:
                case Ontology_type_enum.Go_cc:
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                case Ontology_type_enum.Custom_1:
                case Ontology_type_enum.Custom_2:
                    return "tree - directed";
                case Ontology_type_enum.Mbco:
                    return "circular";
                default:
                    throw new Exception(ontology + " is not considered in switch function.");
            }
        }
        public static string Get_suggested_layout_for_scpInteraction_network(Ontology_type_enum ontology)
        {
            switch (ontology)
            {
                case Ontology_type_enum.Reactome:
                    return "tree - directed or circular";
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_mf:
                case Ontology_type_enum.Go_cc:
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                case Ontology_type_enum.Custom_1:
                case Ontology_type_enum.Custom_2:
                    return "tree - directed";
                case Ontology_type_enum.Mbco:
                    return "organic";
                default:
                    throw new Exception(ontology + " is not considered in switch function.");
            }
        }
        public static string Get_loading_report_for_enrichment(Ontology_type_enum ontology)
        {
            switch (ontology)
            {
                case Ontology_type_enum.Go_bp:
                    return "Reading gene annotations for GO biological processes";
                case Ontology_type_enum.Go_mf:
                    return "Reading gene annotations for GO molecular functions";
                case Ontology_type_enum.Go_cc:
                    return "Reading gene annotations for GO cellular components";
                case Ontology_type_enum.Mbco:
                    return "Reading gene annotations for MBCO subcellular processes (SCPs)";
                case Ontology_type_enum.Reactome:
                    return "Reading gene annotations for Reactome pathways";
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                    return "Reading gene annotations for Na & Glucose transmembrane (TM) transport processes";
                case Ontology_type_enum.Custom_1:
                    return "Reading gene annotations for pathways of custom ontology 1";
                case Ontology_type_enum.Custom_2:
                    return "Reading gene annotations for pathways of custom ontology 2";
                default:
                    throw new Exception(ontology + " is not considered in switch function.");
            }
        }
        public static string Get_loading_report_for_GO_all_namespaces()
        {
            return "Reading gene annotations for all GO terms (three namespaces)";
        }

        public static string Get_loading_and_preparing_report_for_enrichment(Ontology_type_enum ontology)
        {
            switch (ontology)
            {
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_mf:
                case Ontology_type_enum.Go_cc:
                    return "Reading and populating GO terms of all three namespaces with genes of their children (results will be saved for faster access next time)";
                case Ontology_type_enum.Reactome:
                    return "Reading and preparing Reactome pathways including mapping of NCBI gene symbols to gene IDs in Reactome library (results will be saved for faster access next time)";
                case Ontology_type_enum.Custom_1:
                    return "Reading and preparing pathways of custom ontology 1 (results will be saved for faster access next time)";
                case Ontology_type_enum.Custom_2:
                    return "Reading and preparing pathways of custom ontology 2 (results will be saved for faster access next time)";
                case Ontology_type_enum.Mbco:
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                default:
                    throw new Exception(ontology + " is not considered in switch function.");
            }
        }

        public static string Get_pleaseDownloadMbcoPathNet_again_message()
        {
            return "Please download MBC PathNet application from mbc-ontology.org again.";
        }
        public static string Get_pleaseDonwload_file_again_message(string completeFileName)
        {
            return "Please download " + System.IO.Path.GetFileName(completeFileName) + " in " + System.IO.Path.GetDirectoryName(completeFileName) + " again.";
        }
        public static string Get_pleaseDoubleCheck_file_messge(string completeFileName)
        {
            return "Please double check " + System.IO.Path.GetFileName(completeFileName) + " in " + System.IO.Path.GetDirectoryName(completeFileName) + ".";
        }
        public static string Get_pleaseDelete_file_message(string completeFileName)
        {
            return "Please delete " + System.IO.Path.GetFileName(completeFileName) + " in " + System.IO.Path.GetDirectoryName(completeFileName) + ".";
        }

        public static string Get_loadAndPrepare_report_for_network(Ontology_type_enum ontology)
        {
            switch (ontology)
            {
                case Ontology_type_enum.Go_bp:
                    return "Reading and preparing networks for GO biological processes";
                case Ontology_type_enum.Go_mf:
                    return "Reading and preparing networks for GO molecular functions";
                case Ontology_type_enum.Go_cc:
                    return "Reading and preparing networks for GO cellular components";
                case Ontology_type_enum.Reactome:
                case Ontology_type_enum.Custom_1:
                case Ontology_type_enum.Custom_2:
                    return "Reading and preparing networks for " + Ontology_classification_class.Get_name_of_ontology(ontology);
                case Ontology_type_enum.Mbco:
                    return "Reading and preparing MBCO networks";
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                    return "Reading and preparing networks for Na & Glucose TM transport";
                default:
                    throw new Exception(ontology + " is not considered in switch function.");
            }
        }
        public static bool Is_mbco_ontology(Ontology_type_enum ontology)
        {
            switch (ontology)
            {
                case Ontology_type_enum.Mbco:
                    return true;
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_mf:
                case Ontology_type_enum.Go_cc:
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                case Ontology_type_enum.Reactome:
                case Ontology_type_enum.Custom_1:
                case Ontology_type_enum.Custom_2:
                    return false;
                default:
                    throw new Exception(ontology + " is not considered in switch function.");
            }
        }
        public static bool Is_specialized_mbco_ontology(Ontology_type_enum ontology)
        {
            switch (ontology)
            {
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                    return true;
                case Ontology_type_enum.Mbco:
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_mf:
                case Ontology_type_enum.Go_cc:
                case Ontology_type_enum.Reactome:
                case Ontology_type_enum.Custom_1:
                case Ontology_type_enum.Custom_2:
                    return false;
                default:
                    throw new Exception(ontology + " is not considered in switch function.");
            }
        }
        public static bool Is_go_ontology(Ontology_type_enum ontology)
        {
            switch (ontology)
            {
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_cc:
                case Ontology_type_enum.Go_mf:
                    return true;
                case Ontology_type_enum.Mbco:
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                case Ontology_type_enum.Reactome:
                case Ontology_type_enum.Custom_1:
                case Ontology_type_enum.Custom_2:
                    return false;
                default:
                    throw new Exception(ontology + " is not considered in switch function.");
            }
        }
        public static bool Is_reactome_ontology(Ontology_type_enum ontology)
        {
            switch (ontology)
            {
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_cc:
                case Ontology_type_enum.Go_mf:
                case Ontology_type_enum.Mbco:
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                case Ontology_type_enum.Custom_1:
                case Ontology_type_enum.Custom_2:
                    return false;
                case Ontology_type_enum.Reactome:
                    return true;
                default:
                    throw new Exception(ontology + " is not considered in switch function.");
            }
        }
        public static bool Is_custom_ontology(Ontology_type_enum ontology)
        {
            switch (ontology)
            {
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_cc:
                case Ontology_type_enum.Go_mf:
                case Ontology_type_enum.Mbco:
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                case Ontology_type_enum.Reactome:
                    return false;
                case Ontology_type_enum.Custom_1:
                case Ontology_type_enum.Custom_2:
                    return true;
                default:
                    throw new Exception(ontology + " is not considered in switch function.");
            }
        }
        public static bool Is_appOntology(Ontology_type_enum ontology)
        {
            return Is_mbco_ontology(ontology) || (Is_specialized_mbco_ontology(ontology));
        }
    }

    class Global_directory_and_file_class
    {
        const string Human_genes_will_be_mapped_to_species_text_const = "Species-specific gene-pathway annotations will be inferred from human annotations using MGI and NCBI Gene orthologue mappings.";

        #region constant directory
        public char Delimiter { get; private set; }
        private char Not_used_delimiter { get; set; }
        private string Custom_data_subdirectory { get; set; }
        private string Mbco_datasets_subdirectory { get; set; }
        private string Specialized_mbco_datasets_subdirectory { get; set; }
        private string Custom_libraries_subdirectory { get; set; }
        public string Other_ontologies_datasets_subdirectory { get; private set; }
        private string Results_subdirectory { get; set; }
        #endregion

        #region Custom data and results subdirectories
        private string Nog_subdirectory { get; set; }
        private string Kpmp_subdirectory { get; set; }
        private string Dtoxs_subdirectory { get; set; }
        #endregion

        #region Ontology organism input file names
        public Organism_enum[] Organisms { get; set; }
        public Dictionary<Ontology_type_enum, string> Ontology_inputDirectory_dict { get; set; }
        public Dictionary<Ontology_type_enum, Dictionary<Organism_enum, string>> Ontology_organism_geneAssociationInputFileName_dict { get; set; }
        public Dictionary<Ontology_type_enum, string> Ontology_hierarchyInputFileName_dict { get; set; }
        public Dictionary<Ontology_type_enum, string> Ontology_scpNetworkInputFileName_dict { get; set; }
        public Dictionary<Ontology_type_enum, string> Ontology_pathwayAnnotation_dict { get; set; }
        public Dictionary<Ontology_type_enum, Dictionary<Organism_enum,Species_selection_order_enum>> Ontology_species_selectionOrder_dict { get; set; }
        #endregion

        #region App generated files
        private string App_generated_datasets_subdirectory { get; set; }
        private string App_generated_scpNetworks_fileNameAddition { get; set; }
        private string App_generated_scpHierarchy_fileNameAddition { get; set; }
        private string App_generated_orthologs_fileNameStart { get; set; }
        private string App_generated_geneInfo_fileNameStart { get; set; }
        private string Analysis_finished_fileName { get; set; }
        public string Command_line_error_reports_fileName { get; private set; }
        #endregion


        #region NCBI databases
        public string GeneInfo_download_fileName { get; private set; }
        public string Ncbi_orthologs_download_fileName { get; private set; }
        public string Mgi_orthologs_download_fileName { get; private set; }
        #endregion

        #region Result files
        public string Mbco_parameter_settings_fileName { get; private set; }
        public string FirstLine_of_mbco_parameter_setting_fileName { get; private set; }
        public string SCP_abbreviations_fileName { get; private set; }
        public string SCP_abbreviations_fileName_reference { get; private set; }
        #endregion

        #region Read menue datasets
        public string Custom_columnNames_1_fileName { get; set; }
        public string Custom_columnNames_2_fileName { get; set; }
        public string ReadDataMenu_save_setting_fileName { get; set; }
        #endregion

        #region Ontology hierarchy
        public string Ontology_hierarchy_subdirectory { get;set; }
        #endregion

        public string Human_genes_will_be_mapped_to_species_text {  get { return Human_genes_will_be_mapped_to_species_text_const; } }

        public Global_directory_and_file_class()
        {
            string current_directory = System.Environment.CurrentDirectory;
           // if (current_directory.IndexOf('/')!=-1)
            //{
                Delimiter = '/';
                Not_used_delimiter = '\\';
            //}
            //else
            //{
            //    Delimiter = '\\';
            //    Not_used_delimiter = '/';
            //}

            Other_ontologies_datasets_subdirectory = "Other_datasets" + Delimiter;
            Custom_data_subdirectory = "Custom_data" + Delimiter;
            Mbco_datasets_subdirectory = "MBCO_datasets" + Delimiter;
            Specialized_mbco_datasets_subdirectory = "MBCO_special_datasets" + Delimiter;
            Custom_libraries_subdirectory = Other_ontologies_datasets_subdirectory;
            Results_subdirectory = "Results" + Delimiter;
            Nog_subdirectory = "Neurite_outgrowth" + Delimiter;
            Kpmp_subdirectory = "KPMP_reference_atlas" + Delimiter;
            Dtoxs_subdirectory = "LINCS_DToxS_predicTox" + Delimiter;

            List<Organism_enum> preliminary_organisms_add_homologues_if_not_specified = Enum.GetValues(typeof(Organism_enum)).Cast<Organism_enum>().ToList();
            preliminary_organisms_add_homologues_if_not_specified.Remove(Organism_enum.E_m_p_t_y);
            Organism_enum[] organisms_add_homologues_if_not_specified = preliminary_organisms_add_homologues_if_not_specified.ToArray();
            organisms_add_homologues_if_not_specified = new Organism_enum[] { Organism_enum.Homo_sapiens, Organism_enum.Mus_musculus, Organism_enum.Rattus_norvegicus,
                                                                              Organism_enum.Gallus_gallus, Organism_enum.Canis_lupus_familiaris, Organism_enum.Sus_scrofa,
                                                                              Organism_enum.Bos_taurus, Organism_enum.Danio_rerio, Organism_enum.Xenopus_tropicalis,
                                                                              Organism_enum.Caenorhabditis_elegans, Organism_enum.Drosophila_melanogaster };
            Ontology_species_selectionOrder_dict = new Dictionary<Ontology_type_enum, Dictionary<Organism_enum, Species_selection_order_enum>>();
            Organisms = organisms_add_homologues_if_not_specified;


            Ontology_organism_geneAssociationInputFileName_dict = new Dictionary<Ontology_type_enum, Dictionary<Organism_enum, string>>();
            Ontology_inputDirectory_dict = new Dictionary<Ontology_type_enum, string>();
            Ontology_scpNetworkInputFileName_dict = new Dictionary<Ontology_type_enum, string>();
            Ontology_hierarchyInputFileName_dict = new Dictionary<Ontology_type_enum, string>();
            Ontology_pathwayAnnotation_dict = new Dictionary<Ontology_type_enum, string>();

            #region MBCO
            Ontology_inputDirectory_dict.Add(Ontology_type_enum.Mbco, MbcoApp_major_directory + Mbco_datasets_subdirectory);
            Ontology_organism_geneAssociationInputFileName_dict.Add(Ontology_type_enum.Mbco, new Dictionary<Organism_enum, string>());
            Ontology_organism_geneAssociationInputFileName_dict[Ontology_type_enum.Mbco][Organism_enum.Homo_sapiens] = "MBCO_v1.1_gene-SCP_associations_human.txt";
            Ontology_species_selectionOrder_dict.Add(Ontology_type_enum.Mbco, new Dictionary<Organism_enum, Species_selection_order_enum>());
            Ontology_species_selectionOrder_dict[Ontology_type_enum.Mbco].Add(Organism_enum.Homo_sapiens, Species_selection_order_enum.Insist_on_file);
            Ontology_organism_geneAssociationInputFileName_dict[Ontology_type_enum.Mbco][Organism_enum.Mus_musculus] = "MBCO_v1.1_gene-SCP_associations_mouse.txt";
            Ontology_species_selectionOrder_dict[Ontology_type_enum.Mbco].Add(Organism_enum.Mus_musculus, Species_selection_order_enum.Insist_on_file);
            Ontology_organism_geneAssociationInputFileName_dict[Ontology_type_enum.Mbco][Organism_enum.Rattus_norvegicus] = "MBCO_v1.1_gene-SCP_associations_rat.txt";
            Ontology_species_selectionOrder_dict[Ontology_type_enum.Mbco].Add(Organism_enum.Rattus_norvegicus, Species_selection_order_enum.Insist_on_file);
            foreach (Organism_enum organism in organisms_add_homologues_if_not_specified)
            {
                if (!Ontology_organism_geneAssociationInputFileName_dict[Ontology_type_enum.Mbco].ContainsKey(organism))
                {
                    Ontology_organism_geneAssociationInputFileName_dict[Ontology_type_enum.Mbco].Add(organism, Human_genes_will_be_mapped_to_species_text_const);
                    Ontology_species_selectionOrder_dict[Ontology_type_enum.Mbco].Add(organism, Species_selection_order_enum.Always_generate_orthologues);
                }
            }
            Ontology_scpNetworkInputFileName_dict.Add(Ontology_type_enum.Mbco, "MBCO_v1.1_inferred_SCP_relationships.txt");
            Ontology_hierarchyInputFileName_dict.Add(Ontology_type_enum.Mbco, "MBCO_v1.1_SCP_hierarchy.txt");
            #endregion

            #region Sodium glucose tm transport
            Ontology_organism_geneAssociationInputFileName_dict.Add(Ontology_type_enum.Mbco_na_glucose_tm_transport, new Dictionary<Organism_enum, string>());
            Ontology_organism_geneAssociationInputFileName_dict[Ontology_type_enum.Mbco_na_glucose_tm_transport].Add(Organism_enum.Homo_sapiens, "NaAndGluTMTransport_scpGeneAssociations.txt");
            Ontology_species_selectionOrder_dict.Add(Ontology_type_enum.Mbco_na_glucose_tm_transport, new Dictionary<Organism_enum, Species_selection_order_enum>());
            Ontology_species_selectionOrder_dict[Ontology_type_enum.Mbco_na_glucose_tm_transport].Add(Organism_enum.Homo_sapiens, Species_selection_order_enum.Insist_on_file);
            foreach (Organism_enum organism in organisms_add_homologues_if_not_specified)
            {
                if (!Ontology_organism_geneAssociationInputFileName_dict[Ontology_type_enum.Mbco_na_glucose_tm_transport].ContainsKey(organism))
                { 
                    Ontology_organism_geneAssociationInputFileName_dict[Ontology_type_enum.Mbco_na_glucose_tm_transport].Add(organism, Human_genes_will_be_mapped_to_species_text_const);
                    Ontology_species_selectionOrder_dict[Ontology_type_enum.Mbco_na_glucose_tm_transport].Add(organism, Species_selection_order_enum.Always_generate_orthologues);
                }
            }
            Ontology_hierarchyInputFileName_dict.Add(Ontology_type_enum.Mbco_na_glucose_tm_transport, "NaAndGluTMTransport_scpHierarchy.txt");
            Ontology_inputDirectory_dict.Add(Ontology_type_enum.Mbco_na_glucose_tm_transport, MbcoApp_major_directory + Specialized_mbco_datasets_subdirectory);
            #endregion

            #region Gene Ontology
            Ontology_type_enum[] go_namespaces = new Ontology_type_enum[] { Ontology_type_enum.Go_bp, Ontology_type_enum.Go_cc, Ontology_type_enum.Go_mf };
            foreach (Ontology_type_enum go_namespace in go_namespaces)
            {
                Ontology_inputDirectory_dict.Add(go_namespace, MbcoApp_major_directory + Other_ontologies_datasets_subdirectory);
                Ontology_species_selectionOrder_dict.Add(go_namespace, new Dictionary<Organism_enum, Species_selection_order_enum>());
                Ontology_organism_geneAssociationInputFileName_dict.Add(go_namespace, new Dictionary<Organism_enum, string>());
                Ontology_organism_geneAssociationInputFileName_dict[go_namespace][Organism_enum.Homo_sapiens] = "goa_human.gaf";
                Ontology_species_selectionOrder_dict[go_namespace].Add(Organism_enum.Homo_sapiens, Species_selection_order_enum.Insist_on_file);
                Ontology_organism_geneAssociationInputFileName_dict[go_namespace][Organism_enum.Mus_musculus] = "mgi.gaf";
                Ontology_species_selectionOrder_dict[go_namespace].Add(Organism_enum.Mus_musculus, Species_selection_order_enum.Insist_on_file);
                Ontology_organism_geneAssociationInputFileName_dict[go_namespace][Organism_enum.Rattus_norvegicus] = "rgd.gaf";
                Ontology_species_selectionOrder_dict[go_namespace].Add(Organism_enum.Rattus_norvegicus, Species_selection_order_enum.Insist_on_file);
                Ontology_organism_geneAssociationInputFileName_dict[go_namespace][Organism_enum.Sus_scrofa] = "goa_pig.gaf";
                Ontology_species_selectionOrder_dict[go_namespace].Add(Organism_enum.Sus_scrofa, Species_selection_order_enum.Insist_on_file);
                Ontology_organism_geneAssociationInputFileName_dict[go_namespace][Organism_enum.Gallus_gallus] = "goa_chicken.gaf";
                Ontology_species_selectionOrder_dict[go_namespace].Add(Organism_enum.Gallus_gallus, Species_selection_order_enum.Insist_on_file);
                Ontology_organism_geneAssociationInputFileName_dict[go_namespace][Organism_enum.Canis_lupus_familiaris] = "goa_dog.gaf";
                Ontology_species_selectionOrder_dict[go_namespace].Add(Organism_enum.Canis_lupus_familiaris, Species_selection_order_enum.Insist_on_file);
                Ontology_organism_geneAssociationInputFileName_dict[go_namespace][Organism_enum.Xenopus_tropicalis] = "xenbase.gaf";
                Ontology_species_selectionOrder_dict[go_namespace].Add(Organism_enum.Xenopus_tropicalis, Species_selection_order_enum.Insist_on_file);
                Ontology_organism_geneAssociationInputFileName_dict[go_namespace][Organism_enum.Danio_rerio] = "zfin.gaf";
                Ontology_species_selectionOrder_dict[go_namespace].Add(Organism_enum.Danio_rerio, Species_selection_order_enum.Insist_on_file);
                Ontology_organism_geneAssociationInputFileName_dict[go_namespace][Organism_enum.Caenorhabditis_elegans] = "wb.gaf";
                Ontology_species_selectionOrder_dict[go_namespace].Add(Organism_enum.Caenorhabditis_elegans, Species_selection_order_enum.Insist_on_file);
                foreach (Organism_enum organism in organisms_add_homologues_if_not_specified)
                {
                    if (!Ontology_organism_geneAssociationInputFileName_dict[go_namespace].ContainsKey(organism))
                    { 
                        Ontology_organism_geneAssociationInputFileName_dict[go_namespace].Add(organism, Human_genes_will_be_mapped_to_species_text_const);
                        Ontology_species_selectionOrder_dict[go_namespace].Add(organism, Species_selection_order_enum.Always_generate_orthologues);
                    }
                }
                Ontology_hierarchyInputFileName_dict.Add(go_namespace, "go-basic.obo");
            }
            #endregion

            #region Reactome
            Ontology_inputDirectory_dict.Add(Ontology_type_enum.Reactome, MbcoApp_major_directory + Other_ontologies_datasets_subdirectory);
            Ontology_organism_geneAssociationInputFileName_dict.Add(Ontology_type_enum.Reactome, new Dictionary<Organism_enum, string>());
            Ontology_species_selectionOrder_dict.Add(Ontology_type_enum.Reactome, new Dictionary<Organism_enum, Species_selection_order_enum>());
            foreach (Organism_enum organism in organisms_add_homologues_if_not_specified)
            {
                Ontology_organism_geneAssociationInputFileName_dict[Ontology_type_enum.Reactome].Add(organism, "NCBI2Reactome_All_Levels.txt");
                Ontology_species_selectionOrder_dict[Ontology_type_enum.Reactome].Add(organism, Species_selection_order_enum.Insist_on_file);
            }
            Ontology_hierarchyInputFileName_dict.Add(Ontology_type_enum.Reactome, "ReactomePathwaysRelation.txt");
            Ontology_pathwayAnnotation_dict.Add(Ontology_type_enum.Reactome, "ReactomePathways.txt");
            //organism_string = Ontology_classification_class.Get_organismString_for_enum(Organism_enum.All);
            //Ontology_organism_organismLabel_dict.Add(Ontology_type_enum.Reactome, new Dictionary<string, string>());
            //organism_string = Ontology_classification_class.Get_organismString_for_enum(Organism_enum.Bos_taurus);
            //Ontology_organism_organismLabel_dict[Ontology_type_enum.Reactome].Add(organism_string, "BTA");
            //organism_string = Ontology_classification_class.Get_organismString_for_enum(Organism_enum.Caenorhabditis_elegans);
            //Ontology_organism_organismLabel_dict[Ontology_type_enum.Reactome].Add(organism_string, "CEL");
            //organism_string = Ontology_classification_class.Get_organismString_for_enum(Organism_enum.Canis_lupus_familiaris);
            //Ontology_organism_organismLabel_dict[Ontology_type_enum.Reactome].Add(organism_string, "CFA");
            //organism_string = Ontology_classification_class.Get_organismString_for_enum(Organism_enum.Danio_rerio);
            //Ontology_organism_organismLabel_dict[Ontology_type_enum.Reactome].Add(organism_string, "DRE");
            //organism_string = Ontology_classification_class.Get_organismString_for_enum(Organism_enum.Dictyostelium_discoideum);
            //Ontology_organism_organismLabel_dict[Ontology_type_enum.Reactome].Add(organism_string, "DDI");
            //organism_string = Ontology_classification_class.Get_organismString_for_enum(Organism_enum.Drosophila_melanogaster);
            //Ontology_organism_organismLabel_dict[Ontology_type_enum.Reactome].Add(organism_string, "DME");
            //organism_string = Ontology_classification_class.Get_organismString_for_enum(Organism_enum.Gallus_gallus);
            //Ontology_organism_organismLabel_dict[Ontology_type_enum.Reactome].Add(organism_string, "GGA");
            //organism_string = Ontology_classification_class.Get_organismString_for_enum(Organism_enum.Homo_sapiens);
            //Ontology_organism_organismLabel_dict[Ontology_type_enum.Reactome].Add(organism_string, "HSA");
            //organism_string = Ontology_classification_class.Get_organismString_for_enum(Organism_enum.Mus_musculus);
            //Ontology_organism_organismLabel_dict[Ontology_type_enum.Reactome].Add(organism_string, "MMU");
            //organism_string = Ontology_classification_class.Get_organismString_for_enum(Organism_enum.Sus_scrofa);
            //Ontology_organism_organismLabel_dict[Ontology_type_enum.Reactome].Add(organism_string, "SSC");
            //organism_string = Ontology_classification_class.Get_organismString_for_enum(Organism_enum.Xenopus_tropicalis);
            //Ontology_organism_organismLabel_dict[Ontology_type_enum.Reactome].Add(organism_string, "XTR");
            //foreach (Organism_enum organism in organisms_add_homologues_if_not_specified)
            //{
            //    organism_string = Ontology_classification_class.Get_organismString_for_enum(organism);
            //    if (!Ontology_organism_organismLabel_dict[Ontology_type_enum.Reactome].ContainsKey(organism_string))
            //    { Ontology_organism_organismLabel_dict[Ontology_type_enum.Reactome].Add(organism_string, Human_genes_will_be_mapped_text); }
            //}
            #endregion

            #region Custom
            Ontology_inputDirectory_dict.Add(Ontology_type_enum.Custom_1, MbcoApp_major_directory + Other_ontologies_datasets_subdirectory);
            Ontology_inputDirectory_dict.Add(Ontology_type_enum.Custom_2, MbcoApp_major_directory + Other_ontologies_datasets_subdirectory);
            Ontology_species_selectionOrder_dict.Add(Ontology_type_enum.Custom_1, new Dictionary<Organism_enum, Species_selection_order_enum>());
            Ontology_species_selectionOrder_dict.Add(Ontology_type_enum.Custom_2, new Dictionary<Organism_enum, Species_selection_order_enum>());
            Ontology_species_selectionOrder_dict[Ontology_type_enum.Custom_1].Add(Organism_enum.Homo_sapiens, Species_selection_order_enum.Insist_on_file);
            Ontology_species_selectionOrder_dict[Ontology_type_enum.Custom_2].Add(Organism_enum.Homo_sapiens, Species_selection_order_enum.Insist_on_file);
            Ontology_organism_geneAssociationInputFileName_dict.Add(Ontology_type_enum.Custom_1, new Dictionary<Organism_enum, string>());
            Ontology_organism_geneAssociationInputFileName_dict[Ontology_type_enum.Custom_1].Add(Organism_enum.Homo_sapiens, "Custom_1_scpGeneAssociations_human.txt");

            Ontology_hierarchyInputFileName_dict.Add(Ontology_type_enum.Custom_1, "Custom_1_scpHierarchy.txt");
            foreach (Organism_enum organism in organisms_add_homologues_if_not_specified)
            {
                if (!Ontology_organism_geneAssociationInputFileName_dict[Ontology_type_enum.Custom_1].ContainsKey(organism))
                { 
                    Ontology_organism_geneAssociationInputFileName_dict[Ontology_type_enum.Custom_1].Add(organism, "Custom_1_scpGeneAssociations_" + Ontology_classification_class.Get_organismString_for_enum(organism) + ".txt");
                    Ontology_species_selectionOrder_dict[Ontology_type_enum.Custom_1].Add(organism, Species_selection_order_enum.Generate_orthologues_if_missing);
                }
            }
            Ontology_organism_geneAssociationInputFileName_dict.Add(Ontology_type_enum.Custom_2, new Dictionary<Organism_enum, string>());
            Ontology_organism_geneAssociationInputFileName_dict[Ontology_type_enum.Custom_2].Add(Organism_enum.Homo_sapiens, "Custom_2_scpGeneAssociations_human.txt");
            Ontology_hierarchyInputFileName_dict.Add(Ontology_type_enum.Custom_2, "Custom_2_scpHierarchy.txt");
            foreach (Organism_enum organism in organisms_add_homologues_if_not_specified)
            {
                if (!Ontology_organism_geneAssociationInputFileName_dict[Ontology_type_enum.Custom_2].ContainsKey(organism))
                {
                    Ontology_organism_geneAssociationInputFileName_dict[Ontology_type_enum.Custom_2].Add(organism, "Custom_2_scpGeneAssociations_" + Ontology_classification_class.Get_organismString_for_enum(organism) + ".txt");
                    Ontology_species_selectionOrder_dict[Ontology_type_enum.Custom_2].Add(organism, Species_selection_order_enum.Generate_orthologues_if_missing);
                }
            }
            #endregion

            Ontology_hierarchy_subdirectory = "Ontology_hierarchies" + Delimiter;

            Custom_columnNames_1_fileName = "Custom_1_columnNames_userData.txt";
            Custom_columnNames_2_fileName = "Custom_2_columnNames_userData.txt";
            ReadDataMenu_save_setting_fileName = "ReadDataMenu_saved_settings.txt";

            #region NCBI and Jackson lab databases
            GeneInfo_download_fileName = "gene_info";
            Ncbi_orthologs_download_fileName = "gene_orthologs";
            Mgi_orthologs_download_fileName = "HOM_ALLOrganism.rpt";
            #endregion

            SCP_abbreviations_fileName = "SCP_abbreviations.txt";
            SCP_abbreviations_fileName_reference = "Abb: '" + SCP_abbreviations_fileName + "'";
            Mbco_parameter_settings_fileName = "MBC_pathNet_parameter_settings.txt";
            FirstLine_of_mbco_parameter_setting_fileName = "#MBC PathNet parameter settings for re-import into application";

            #region App generated files and directory
            App_generated_scpHierarchy_fileNameAddition = "_hierachy.txt";
            App_generated_scpNetworks_fileNameAddition = "_horizontalScpNetworks.txt";
            App_generated_datasets_subdirectory = "Datasets_generated_by_app" + Delimiter;
            App_generated_orthologs_fileNameStart = "Orhologs_";
            App_generated_geneInfo_fileNameStart = "GeneInfo_";
            Analysis_finished_fileName = "Analysis_finished.txt";
            Command_line_error_reports_fileName = "Command_line_error_reports.txt";
            #endregion

        }
        public string MbcoApp_major_directory
        {
            get { return System.Environment.CurrentDirectory + Delimiter; }
        }
        public string Results_directory
        {
            get { return MbcoApp_major_directory + Results_subdirectory; }
        }
        public string[] Get_all_analysis_finished_complete_fileNames(string[] additional_directories)
        {
            int fileNames_length = additional_directories.Length + 1;
            string[] complete_analysis_finished_fileNames = new string[fileNames_length];
            for (int indexFN = 0; indexFN < fileNames_length-1; indexFN++)
            {
                complete_analysis_finished_fileNames[indexFN] = additional_directories[indexFN] + Analysis_finished_fileName;
            }
            complete_analysis_finished_fileNames[fileNames_length - 1] = App_generated_datasets_directory + Analysis_finished_fileName;

            return complete_analysis_finished_fileNames;
        }
        public string[] Get_all_command_line_error_report_complete_fileNames(string[] additional_directories)
        {
            int fileNames_length = additional_directories.Length + 1;
            string[] complete_analysis_finished_fileNames = new string[fileNames_length];
            for (int indexFN = 0; indexFN < fileNames_length-1; indexFN++)
            {
                complete_analysis_finished_fileNames[indexFN] = additional_directories[indexFN] + Command_line_error_reports_fileName;
            }
            complete_analysis_finished_fileNames[fileNames_length - 1] = App_generated_datasets_directory + Command_line_error_reports_fileName;

            return complete_analysis_finished_fileNames;
        }
        public string[] Get_all_appGenerated_analysisRelated_fileNames(string[] additional_directories)
        {
            return Overlap_class.Get_ordered_union_of_string_arrays(Get_all_analysis_finished_complete_fileNames(additional_directories),
                                                                    Get_all_command_line_error_report_complete_fileNames(additional_directories));
        }
        public string[] Remove_appGenerated_fileNames(string[] complete_fileNames)
        {
            List<string> keep = new List<string>();
            string fileName;
            foreach (string complete_fileName in complete_fileNames)
            {
                fileName = System.IO.Path.GetFileName(complete_fileName);
                if (  (!fileName.Equals(Command_line_error_reports_fileName))
                    && (!fileName.Equals(Analysis_finished_fileName))
                    && (!fileName.Equals(Custom_columnNames_1_fileName))
                    && (!fileName.Equals(Custom_columnNames_2_fileName)))
                {
                    keep.Add(complete_fileName);
                }
            }
            return keep.ToArray();
        }
        public string Custom_data_directory
        {
            get { return MbcoApp_major_directory + Custom_data_subdirectory; }
        }

        #region App generated directory and file names
        public string App_generated_datasets_directory
        {
            get { return MbcoApp_major_directory + App_generated_datasets_subdirectory; }
        }
        public string Get_appGenerated_complete_ontology_parentChild_allInfo_fileName(Ontology_type_enum ontology, Organism_enum organism, SCP_hierarchy_interaction_type_enum interaction_type)
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            string hierarchy_allInfo_fileEnd = "_" + Ontology_classification_class.Get_name_of_hierarchicalScpInteractionType(interaction_type) + gdf.App_generated_scpHierarchy_fileNameAddition;
            string organism_string = Ontology_classification_class.Get_organismString_for_enum(organism);
            switch (ontology)
            {
                case Ontology_type_enum.Reactome:
                    return App_generated_datasets_directory + Ontology_classification_class.Get_name_of_ontology(ontology) + "_" + organism_string + hierarchy_allInfo_fileEnd;
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                case Ontology_type_enum.Custom_1:
                case Ontology_type_enum.Custom_2:
                    return App_generated_datasets_directory + Ontology_classification_class.Get_name_of_ontology(ontology) + hierarchy_allInfo_fileEnd;
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_cc:
                case Ontology_type_enum.Go_mf:
                    return App_generated_datasets_directory + "GO" + hierarchy_allInfo_fileEnd;
                case Ontology_type_enum.Mbco:
                    return App_generated_datasets_directory + "MBCO" + hierarchy_allInfo_fileEnd;
                default:
                    throw new Exception(ontology + " is not considered in switch function.");
            }
        }
        public string Get_appGenerated_complete_ontology_scpNetworks_fileName(Ontology_type_enum ontology)
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            switch (ontology)
            {
                case Ontology_type_enum.Mbco:
                    return App_generated_datasets_directory + Ontology_classification_class.Get_name_of_ontology(ontology) + gdf.App_generated_scpNetworks_fileNameAddition;
                default:
                    throw new Exception(ontology + " is not considered in switch statement.");
            }
        }
        public string Get_customColumName_fileName(int custom_column_names_no)
        {
            switch (custom_column_names_no)
            {
                case 1:
                    return Custom_columnNames_1_fileName;
                case 2:
                    return Custom_columnNames_2_fileName;
                default:
                    throw new Exception(custom_column_names_no + " is not considered in switch function.");
            }
        }
        public string Get_appGenerated_complete_orthology_fileName(Organism_enum source_organism, Organism_enum target_organism)
        {
            return App_generated_datasets_directory + App_generated_orthologs_fileNameStart + Ontology_classification_class.Get_organismString_for_enum(source_organism) + "_to_" + Ontology_classification_class.Get_organismString_for_enum(target_organism) + ".txt";
        }
        public string Get_appGenerated_complete_geneInfo_fileName(Organism_enum organism)
        {
            return App_generated_datasets_directory + App_generated_geneInfo_fileNameStart + Ontology_classification_class.Get_organismString_for_enum(organism) + ".txt";
        }

        #endregion


        #region Final ontology association file
        public string Get_appGenerated_complete_ontologyAssociationPopulatedParentsAppliedCutoff_fileName(Ontology_type_enum ontology, Organism_enum organism, Dictionary<Ontology_type_enum, Dictionary<GO_hyperParameter_enum, int>> ontology_goHyperparameter_cutoff_dict)
        {
            string fileBaseName = Ontology_classification_class.Get_name_of_ontology_plus_organism(ontology, organism);
            StringBuilder sb = new StringBuilder();
            sb.Append(App_generated_datasets_directory);
            sb.Append(fileBaseName);
            if (ontology_goHyperparameter_cutoff_dict.ContainsKey(ontology))
            {
                Dictionary<GO_hyperParameter_enum, int> goHyperparameter_cutoff_dict = ontology_goHyperparameter_cutoff_dict[ontology];
                GO_hyperParameter_enum[] hyperparameters = goHyperparameter_cutoff_dict.Keys.ToArray();
                if (hyperparameters.Length == 0) { }
                else
                {
                    foreach (GO_hyperParameter_enum hyperParameter in hyperparameters)
                    {
                        if (goHyperparameter_cutoff_dict[hyperParameter] > 0)
                        {
                            sb.AppendFormat("_{0}{1}", Ontology_classification_class.Get_name_of_goHyperparameter(hyperParameter), goHyperparameter_cutoff_dict[hyperParameter]);
                        }
                    }
                }
            }
            sb.Append(".txt");
            return sb.ToString();
        }
        public string Get_appGenerated_complete_ontologyAssociationPopulatedParentsWithoutCutoff_fileName(Ontology_type_enum ontology, Organism_enum organism)
        {
            return Get_appGenerated_complete_ontologyAssociationPopulatedParentsAppliedCutoff_fileName(ontology, organism, new Dictionary<Ontology_type_enum, Dictionary<GO_hyperParameter_enum, int>>());
        }
        public string Get_complete_standardOntology_gene_association_populatedParents_fileName(Ontology_type_enum ontology, Organism_enum organism)
        {
            string ontology_string = Ontology_classification_class.Get_name_of_ontology_plus_organism(ontology, organism);
            return App_generated_datasets_subdirectory + ontology_string + ".txt";
        }
        #endregion


        #region NCBI, MGI databases
        public string Complete_geneInfo_download_fileName
        {
            get { return MbcoApp_major_directory + Other_ontologies_datasets_subdirectory + GeneInfo_download_fileName; }
        }
        public string Complete_ncbi_ortholgs_download_fileName
        {
            get { return MbcoApp_major_directory + Other_ontologies_datasets_subdirectory + Ncbi_orthologs_download_fileName; }
        }
        public string Complete_mgi_orthologs_download_fileName
        {
            get { return MbcoApp_major_directory + Other_ontologies_datasets_subdirectory + Mgi_orthologs_download_fileName; }
        }
        public string Complete_appGenerated_orthologs_fileName
        {
            get { return App_generated_datasets_directory + Mgi_orthologs_download_fileName; }
        }
        #endregion

        public string Transform_into_compatible_directory_and_clean_up(string directoryOrDirectoryPlusFile)
        {
            directoryOrDirectoryPlusFile = directoryOrDirectoryPlusFile.Replace(Not_used_delimiter, Delimiter);
            if (  (!directoryOrDirectoryPlusFile[directoryOrDirectoryPlusFile.Length - 1].Equals(Delimiter))
                &&(String.IsNullOrEmpty(System.IO.Path.GetExtension(directoryOrDirectoryPlusFile))))
            {
                directoryOrDirectoryPlusFile = directoryOrDirectoryPlusFile + Delimiter.ToString();
            }
            string double_delimiter_string = Delimiter.ToString() + Delimiter.ToString();
            while (directoryOrDirectoryPlusFile.Contains(double_delimiter_string))
            {
                directoryOrDirectoryPlusFile = directoryOrDirectoryPlusFile.Replace(double_delimiter_string, Delimiter.ToString());
            }
            return directoryOrDirectoryPlusFile;
        }
        public bool Does_path_contain_invalid_characters(string path, Form1_default_settings_class form_default)
        {
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                if (form_default.Is_mono)
                {
                    if (  (!c.Equals('/'))
                        &&(!c.Equals('\\'))
                        &&(path.Contains(c)))
                    {
                        return true;  // Invalid character found
                    }
                }
                else
                {
                    if (   (!c.Equals(':'))
                        && (!c.Equals('/'))
                        && (!c.Equals('\\'))
                        && (path.Contains(c)))
                    {
                        return true;  // Invalid character found
                    }
                }
            }
            return false;  // All characters are valid
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
        public string Get_custom_data_dtoxs_directory()
        {
            return Custom_data_directory + Dtoxs_subdirectory;
        }
        public string Get_custom_results_nog_directory()
        {
            return Results_directory + Nog_subdirectory;
        }
        public string Get_custom_results_kpmp_directory()
        {
            return Results_directory + Kpmp_subdirectory;
        }
        public string Get_custom_results_DToxS_directory()
        {
            return Results_directory + Dtoxs_subdirectory;
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

        #region Hierarchy of ontologies
        public string Get_name_for_ontology_hierarchy(Ontology_type_enum ontology)
        {
            string ontology_string = Ontology_classification_class.Get_name_of_scps_for_progress_report(ontology);
            return "Hierarchy_of_" + ontology_string;
        }
        public string Get_name_for_ontology_scpInteractions(Ontology_type_enum ontology)
        {
            string ontology_string = Ontology_classification_class.Get_name_of_scps_for_progress_report(ontology);
            return "ScpInteractions_of_" + ontology_string;
        }
        #endregion

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
                    throw new Exception(number + " is not considered in switch function.");
            }
            return sign;
        }

        public static string Convert_into_two_digit_hexadecimal(int number)
        {
            if ((number > 255) || (number < 0))
            {
                throw new Exception(number + " is outside of color range.");
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
