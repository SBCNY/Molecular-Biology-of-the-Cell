//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

using System;
using System.Linq;
using System.Collections.Generic;
using Common_functions.Global_definitions;
using Common_functions.ReadWrite;
using System.Drawing;
using Common_functions.Array_own;
using Common_functions.Options_base;
using Common_functions.Form_tools;
using System.Windows.Markup;
using Windows_forms;
using ClassLibrary1.eUtils;
using System.Windows.Controls;
using System.Web;

namespace Data
{
    class Custom_data_summary_line_class
    {
        public string Integration_group { get; set; }
        public string UniqueDatasetName { get; set; }
        public string Dataset_name { get; set; }
        public Entry_type_enum EntryType { get; set; }
        public float Timepoint { get; set; }
        public Timeunit_enum Timeunit { get; set; }
        public float TimepointInDays { get { return Timeunit_conversion_class.Get_timepoint_in_days(Timepoint, Timeunit); } }
        public int Sig_genes_count { get; set; }
        public int Total_genes_count { get; set; }
        public Color Dataset_color { get; set; }
        public string Dataset_color_string {  get { return Color_conversion_class.Get_color_string(Dataset_color); } set { Dataset_color = Color_conversion_class.Set_color_from_string(value); } }
        public string Source_fileName { get; set; }
        public string Exp_bgGenes_list_name { get; set; }
        public int Experimental_background_genes_count { get; set; }
        public int Mbco_background_genes_count { get; set; }
        public int Final_background_genes_count { get; set; }
        public int Sig_genes_among_experimental_background_genes_count { get; set; }
        public int Total_genes_among_experimental_background_genes_count { get; set; }
        public int Sig_genes_among_MBCO_background_genes_count { get; set; }
        public int Sig_genes_among_final_background_genes_count { get; set; }

        public Custom_data_summary_line_class(Custom_data_line_class custom_data_line)
        {
            this.UniqueDatasetName = (string)custom_data_line.Unique_dataset_name.Clone();
            this.Integration_group = (string)custom_data_line.IntegrationGroup.Clone();
            this.Dataset_name = (string)custom_data_line.SampleName.Clone();
            this.EntryType = custom_data_line.EntryType;
            this.Timepoint = custom_data_line.Timepoint;
            this.Timeunit = custom_data_line.Timeunit;
            this.Exp_bgGenes_list_name = (string)custom_data_line.BgGenes_listName.Clone();
            this.Dataset_color = custom_data_line.SampleColor;
            this.Source_fileName = (string)custom_data_line.Source_fileName.Clone();
        }

        public Custom_data_summary_line_class Deep_copy()
        {
            Custom_data_summary_line_class copy = (Custom_data_summary_line_class)this.MemberwiseClone();
            copy.Integration_group = (string)this.Integration_group.Clone();
            copy.Dataset_name = (string)this.Dataset_name.Clone();
            copy.Source_fileName = (string)this.Source_fileName.Clone();
            copy.Exp_bgGenes_list_name = (string)this.Exp_bgGenes_list_name.Clone();
            copy.Source_fileName = (string)this.Source_fileName.Clone();
            return copy;
        }
    }

    class Custom_data_summary_readWriteOptions_class : ReadWriteOptions_base
    {
        public Custom_data_summary_readWriteOptions_class(string directory, string fileName, bool writeIntegrationGroups, bool writeTimepoints, bool writeEntryType)
        {
            this.File = directory + fileName;
            List<string> key_property_names_list = new List<string>();
            List<string> key_column_names_list = new List<string>();
            if (writeIntegrationGroups)
            {
                key_property_names_list.Add("Integration_group");
                key_column_names_list.Add("Integration group");
            }
            key_property_names_list.Add("Dataset_name");
            key_column_names_list.Add("Dataset name");
            if (writeTimepoints)
            {
                key_property_names_list.AddRange(new string[] { "Timepoint", "Timeunit" } );
                key_column_names_list.AddRange(new string[] { "Timepoint", "Timeunit" });
            }
            if (writeEntryType)
            {
                key_property_names_list.AddRange(new string[] { "EntryType" });
                key_column_names_list.AddRange(new string[] { "UpDown status" });
            }
            key_property_names_list.AddRange(new string[] { "Dataset_color_string", "Total_genes_count", "Sig_genes_count",   "Sig_genes_among_final_background_genes_count",                 "Final_background_genes_count",                      "Exp_bgGenes_list_name",              "Source_fileName",  "Sig_genes_among_experimental_background_genes_count",                 "Experimental_background_genes_count",        "Sig_genes_among_MBCO_background_genes_count",                 "Mbco_background_genes_count" });
            key_column_names_list.AddRange(new string[]   { "Color",                "Total genes count", "Sign. genes count", "Count of sign. genes that are part of final background genes", "Count of all genes in final background genes list", "Experimental background genes list", "Source", "Count of sign. genes that are part of experimental background genes", "Count of all experimental background genes", "Count of sign. genes that are part of MBCO background genes", "Count of all MBCO background genes"});
            this.Key_propertyNames = key_property_names_list.ToArray();
            this.Key_columnNames = key_column_names_list.ToArray();
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.File_has_headline = true;
            this.No_entry_fields_allowed = false;
        }
    }

    class Custom_data_summary_class
    {
        public Custom_data_summary_line_class[] Summaries { get; set; }

        public Custom_data_summary_class()
        {
            this.Summaries = new Custom_data_summary_line_class[0];
        }

        private Dictionary<string,bool> Generate_dictionary_with_background_genes(string[] background_genes)
        {
            int background_genes_length = background_genes.Length;
            string background_gene;
            Dictionary<string, bool> bg_genes_dict = new Dictionary<string, bool>();
            for (int indexBg = 0; indexBg < background_genes_length; indexBg++)
            {
                background_gene = background_genes[indexBg];
                if (!bg_genes_dict.ContainsKey(background_gene))
                {
                    bg_genes_dict.Add(background_gene, true);
                }
            }
            return bg_genes_dict;
        }

        private Dictionary<string,Dictionary<string,bool>> Generate_dictionary_of_dictionaries_with_background_genes(Custom_data_class custom_data, string[] mbco_background_genes)
        {
            Dictionary<string, string[]> bgGenesListName_bgGenes_dict = custom_data.ExpBgGenesList_bgGenes_dict;
            Dictionary<string, bool> bgGenes_dict;
            Dictionary<string, Dictionary<string, bool>> bgGeneListName_bgGenes_bool_dict = new Dictionary<string, Dictionary<string, bool>>();
            string[] bgGeneListsNames = bgGenesListName_bgGenes_dict.Keys.ToArray();
            string bgGeneListsName;
            int bgGeneListsNames_length = bgGeneListsNames.Length;
            for (int indexBgGeneLists=0; indexBgGeneLists<bgGeneListsNames_length;indexBgGeneLists++)
            {
                bgGeneListsName = bgGeneListsNames[indexBgGeneLists];
                if (!bgGeneListsName.Equals(Global_class.Mbco_exp_background_gene_list_name))
                {
                    bgGenes_dict = Generate_dictionary_with_background_genes(bgGenesListName_bgGenes_dict[bgGeneListsName]);
                    bgGeneListName_bgGenes_bool_dict.Add(bgGeneListsName, bgGenes_dict);
                }
            }
            bgGenes_dict = Generate_dictionary_with_background_genes(mbco_background_genes);
            bgGeneListName_bgGenes_bool_dict.Add(Global_class.Mbco_exp_background_gene_list_name, bgGenes_dict);
            return bgGeneListName_bgGenes_bool_dict;
        }

        private void Generate_from_custom_data_and_bgGeneListName_bgGenes_dict(Custom_data_class custom_data, Dictionary<string, Dictionary<string, bool>> bgGeneListName_bgGenes_dict)
        {
            custom_data.Order_by_integrationGroup_sampleName_timepointInDays_entryType();
            int custom_data_length = custom_data.Custom_data.Length;
            Custom_data_line_class custom_data_line;
            int total_genes_count = 0;
            int sig_genes_count = 0;
            int sig_genes_in_exp_background_list_count = 0;
            int sig_genes_in_mbco_background_list_count = 0;
            int sig_genes_in_final_background_list_count = 0;
            int total_genes_in_exp_background_list_count = 0;
            bool sig_gene_is_in_exp_background_list = false;
            bool sig_gene_is_in_mbco_background_list = false;
            Custom_data_summary_line_class data_summary_line;
            List<Custom_data_summary_line_class> new_summary_lines = new List<Custom_data_summary_line_class>();

            #region Fill bgGeneListName_geneCount_dict and finalBgGeneListName_geneCount_dict
            Dictionary<string, int> bgGeneListName_geneCount_dict = new Dictionary<string, int>();
            Dictionary<string, int> finalBgGeneListName_geneCount_dict = new Dictionary<string, int>();
            Dictionary<string, bool> mbco_bgGenes_dict = bgGeneListName_bgGenes_dict[Global_class.Mbco_exp_background_gene_list_name];
            string[] bgGeneListNames = bgGeneListName_bgGenes_dict.Keys.ToArray();
            string bgGeneListName;
            int bgGeneListNames_length = bgGeneListNames.Length;
            string[] currentBgGenes;
            string currentBgGene;
            int currentBgGenes_length;
            int expGenes_in_mbcoBgGenes_count = 0;
            for (int indexBg=0; indexBg<bgGeneListNames_length; indexBg++)
            {
                bgGeneListName = bgGeneListNames[indexBg];
                bgGeneListName_geneCount_dict.Add(bgGeneListName, bgGeneListName_bgGenes_dict[bgGeneListName].ToArray().Length);
                currentBgGenes = bgGeneListName_bgGenes_dict[bgGeneListName].Keys.ToArray();
                currentBgGenes_length = currentBgGenes.Length;
                expGenes_in_mbcoBgGenes_count = 0;
                for (int indexCurrent=0; indexCurrent<currentBgGenes_length;indexCurrent++)
                {
                    currentBgGene = currentBgGenes[indexCurrent];
                    if (mbco_bgGenes_dict.ContainsKey(currentBgGene))
                    {
                        expGenes_in_mbcoBgGenes_count++;
                    }
                }
                finalBgGeneListName_geneCount_dict.Add(bgGeneListName, expGenes_in_mbcoBgGenes_count);
            }
            #endregion

            for (int indexC = 0; indexC < custom_data_length; indexC++)
            {
                custom_data_line = custom_data.Custom_data[indexC];
                if (  (indexC==0)
                    || (!custom_data_line.IntegrationGroup.Equals(custom_data.Custom_data[indexC-1].IntegrationGroup))
                    || (!custom_data_line.SampleName.Equals(custom_data.Custom_data[indexC - 1].SampleName))
                    || (!custom_data_line.Timepoint_in_days.Equals(custom_data.Custom_data[indexC - 1].Timepoint_in_days))
                    || (!custom_data_line.EntryType.Equals(custom_data.Custom_data[indexC - 1].EntryType)))
                {
                    sig_genes_count = 0;
                    total_genes_count = 0;
                    sig_genes_in_exp_background_list_count = 0;
                    sig_genes_in_mbco_background_list_count = 0;
                    sig_genes_in_final_background_list_count = 0;
                    total_genes_in_exp_background_list_count = 0;
                }
                total_genes_count++;
                switch (custom_data_line.Significance_status)
                {
                    case Significance_status_enum.Yes:
                        sig_genes_count++;
                        sig_gene_is_in_exp_background_list = bgGeneListName_bgGenes_dict[custom_data_line.BgGenes_listName].ContainsKey(custom_data_line.NCBI_official_symbol);
                        sig_gene_is_in_mbco_background_list = bgGeneListName_bgGenes_dict[Global_class.Mbco_exp_background_gene_list_name].ContainsKey(custom_data_line.NCBI_official_symbol);
                        if (sig_gene_is_in_exp_background_list)
                        {
                            sig_genes_in_exp_background_list_count++;
                        }
                        if (sig_gene_is_in_mbco_background_list)
                        {
                            sig_genes_in_mbco_background_list_count++;
                        }
                        if ((sig_gene_is_in_exp_background_list) && (sig_gene_is_in_mbco_background_list))
                        {
                            sig_genes_in_final_background_list_count++;
                        }
                        break;
                    case Significance_status_enum.No:
                        break;
                    default:
                        throw new Exception();
                }
                if (bgGeneListName_bgGenes_dict[custom_data_line.BgGenes_listName].ContainsKey(custom_data_line.NCBI_official_symbol))
                {
                    total_genes_in_exp_background_list_count++;
                }
                if ((indexC == custom_data_length-1)
                    || (!custom_data_line.IntegrationGroup.Equals(custom_data.Custom_data[indexC + 1].IntegrationGroup))
                    || (!custom_data_line.SampleName.Equals(custom_data.Custom_data[indexC + 1].SampleName))
                    || (!custom_data_line.Timepoint_in_days.Equals(custom_data.Custom_data[indexC + 1].Timepoint_in_days))
                    || (!custom_data_line.EntryType.Equals(custom_data.Custom_data[indexC + 1].EntryType)))
                {
                    data_summary_line = new Custom_data_summary_line_class(custom_data_line);
                    data_summary_line.Mbco_background_genes_count = bgGeneListName_geneCount_dict[Global_class.Mbco_exp_background_gene_list_name];
                    data_summary_line.Experimental_background_genes_count = bgGeneListName_geneCount_dict[custom_data_line.BgGenes_listName];
                    data_summary_line.Integration_group = (string)custom_data_line.IntegrationGroup.Clone();
                    data_summary_line.Final_background_genes_count = finalBgGeneListName_geneCount_dict[custom_data_line.BgGenes_listName];
                    data_summary_line.Total_genes_count = total_genes_count;
                    data_summary_line.Sig_genes_count = sig_genes_count;
                    data_summary_line.Sig_genes_among_MBCO_background_genes_count = sig_genes_in_mbco_background_list_count;
                    data_summary_line.Sig_genes_among_final_background_genes_count = sig_genes_in_final_background_list_count;
                    data_summary_line.Sig_genes_among_experimental_background_genes_count = sig_genes_in_exp_background_list_count;
                    data_summary_line.Total_genes_among_experimental_background_genes_count = total_genes_in_exp_background_list_count;
                    new_summary_lines.Add(data_summary_line);
                }
            }
            this.Summaries = new_summary_lines.ToArray();
        }

        public void Generate_from_custom_data(Custom_data_class custom_data, string[] mbco_background_genes)
        {
            custom_data.Check_for_correctness();
            Dictionary<string, Dictionary<string, bool>> bgGeneListName_bgGenes_dict = Generate_dictionary_of_dictionaries_with_background_genes(custom_data, mbco_background_genes);
            Generate_from_custom_data_and_bgGeneListName_bgGenes_dict(custom_data, bgGeneListName_bgGenes_dict);
        }

        public int Get_number_of_significant_genes_in_final_background_gene_list_of_all_datasets()
        {
            int number_of_sig_genes_in_final_background_gene_lists = 0;
            foreach (Custom_data_summary_line_class data_summary_line in this.Summaries)
            {
                number_of_sig_genes_in_final_background_gene_lists += data_summary_line.Sig_genes_among_final_background_genes_count;
            }
            return number_of_sig_genes_in_final_background_gene_lists;
        }

        public string[] Get_uniqueDatasetNames_plus_integrationGroup_with_no_signficant_genes_in_final_background_gene_list()
        {
            List<string> uniqueDatasetNames_with_no_genes = new List<string>();
            foreach (Custom_data_summary_line_class data_summary_line in this.Summaries)
            {
                if (data_summary_line.Sig_genes_among_final_background_genes_count == 0)
                {
                    uniqueDatasetNames_with_no_genes.Add(data_summary_line.UniqueDatasetName + " in " + data_summary_line.Integration_group);
                }
            }
            return uniqueDatasetNames_with_no_genes.ToArray();
        }

        private string[] Get_all_ordered_unique_integrationGroups()
        {
            Dictionary<string, bool> integrationGroup_dict = new Dictionary<string, bool>();
            foreach (Custom_data_summary_line_class custom_data_summary_line in this.Summaries)
            {
                if (!integrationGroup_dict.ContainsKey(custom_data_summary_line.Integration_group))
                {
                    integrationGroup_dict.Add(custom_data_summary_line.Integration_group, true);
                }
            }
            return integrationGroup_dict.Keys.OrderBy(l => l).ToArray();
        }

        private Entry_type_enum[] Get_all_ordered_unique_entryTypes()
        {
            Dictionary<Entry_type_enum, bool> entryType_dict = new Dictionary<Entry_type_enum, bool>();
            foreach (Custom_data_summary_line_class custom_data_summary_line in this.Summaries)
            {
                if (!entryType_dict.ContainsKey(custom_data_summary_line.EntryType))
                {
                    entryType_dict.Add(custom_data_summary_line.EntryType, true);
                }
            }
            return entryType_dict.Keys.OrderBy(l => l).ToArray();
        }

        private float[] Get_all_ordered_unique_timepointInDays()
        {
            Dictionary<float, bool> timepointInDays_dict = new Dictionary<float, bool>();
            foreach (Custom_data_summary_line_class custom_data_summary_line in this.Summaries)
            {
                if (!timepointInDays_dict.ContainsKey(custom_data_summary_line.TimepointInDays))
                {
                    timepointInDays_dict.Add(custom_data_summary_line.TimepointInDays, true);
                }
            }
            return timepointInDays_dict.Keys.OrderBy(l => l).ToArray();
        }

        public void Write(string directory, string fileName, ProgressReport_interface_class progressReport, out bool file_opened_successful)
        {
            bool write_integrationGroups = Get_all_ordered_unique_integrationGroups().Length > 1;
            bool write_entrytypes = Get_all_ordered_unique_entryTypes().Length > 1;
            bool write_timepoints = Get_all_ordered_unique_timepointInDays().Length > 1;
            Custom_data_summary_readWriteOptions_class custom_data_summary_readWriteOptions = new Custom_data_summary_readWriteOptions_class(directory, fileName, write_integrationGroups, write_timepoints, write_entrytypes);
            ReadWriteClass.WriteData_and_add_warning_to_progressReport_if_failed(this.Summaries, custom_data_summary_readWriteOptions, progressReport, out file_opened_successful);
        }

        public Dictionary<string, int> Get_uniqueDatasetName_missingGenes_dictionary_that_containGenes_that_are_not_part_of_experimental_bgGenes()
        {
            Dictionary<string, int> uniqueDataset_name_missingGenesCount_dict = new Dictionary<string, int>();
            Custom_data_summary_line_class custom_data_summary_line;
            int custom_data_summary_length = this.Summaries.Length;
            for (int indexC=0; indexC<custom_data_summary_length;indexC++)
            {
                custom_data_summary_line = this.Summaries[indexC];
                if (  (custom_data_summary_line.Total_genes_count != custom_data_summary_line.Total_genes_among_experimental_background_genes_count)
                    &&(!custom_data_summary_line.Exp_bgGenes_list_name.Equals(Global_class.Mbco_exp_background_gene_list_name)))
                {
                    uniqueDataset_name_missingGenesCount_dict.Add(custom_data_summary_line.UniqueDatasetName, custom_data_summary_line.Total_genes_count - custom_data_summary_line.Total_genes_among_experimental_background_genes_count);
                }
            }
            return uniqueDataset_name_missingGenesCount_dict;
        }
    }

    class User_data_options_class : Options_readWrite_base_class
    {
        #region Min and max values
        public const float min_value_1st_cutoff = 0;
        public const float min_value_2nd_cutoff = 0;
        public const int min_keep_top_ranks = 1;
        #endregion

        #region Internal fields
        private float value_1st_cutoff { get; set; }
        private float value_2nd_cutoff { get; set; }
        private int keep_top_ranks { get; set; }
        #endregion

        public float Value_1st_cutoff 
        {
            get { return value_1st_cutoff; }
            set { value_1st_cutoff = Math.Max(min_value_1st_cutoff, value); } 
        }
        public float Value_2nd_cutoff 
        {
            get { return value_2nd_cutoff; }
            set { value_2nd_cutoff = Math.Max(min_value_2nd_cutoff, value); }
        }
        public Order_of_values_for_signficance_enum Significance_definition_value_1st { get; set; }
        public Order_of_values_for_signficance_enum Significance_definition_value_2nd { get; set; }
        public Value_importance_order_enum Value_importance_order { get; set; }
        public int Keep_top_ranks
        {
            get { return keep_top_ranks; }
            set { keep_top_ranks = Math.Max(min_keep_top_ranks, value); }
        }
        public bool Merge_upDown_before_ranking { get; set; }
        public bool All_genes_significant { get; set; }
        public bool Delete_all_not_significant_genes { get; set; }

        public User_data_options_class()
        {
            Value_1st_cutoff = 0;
            Value_2nd_cutoff = 999;
            Significance_definition_value_1st = Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant;
            Significance_definition_value_2nd = Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant;
            Value_importance_order = Value_importance_order_enum.Value_2nd_1st;
            Keep_top_ranks = 9999;
            Merge_upDown_before_ranking = true;
            All_genes_significant = true;
            Delete_all_not_significant_genes = false;
        }

        public bool Equals_other(User_data_options_class other_options)
        {
            bool equals_other = true;
            if (!this.Value_1st_cutoff.Equals(other_options.Value_1st_cutoff)) { equals_other = false; }
            if (!this.Value_2nd_cutoff.Equals(other_options.Value_2nd_cutoff)) { equals_other = false; }
            if (!this.Significance_definition_value_1st.Equals(other_options.Significance_definition_value_1st)) { equals_other = false; }
            if (!this.Significance_definition_value_2nd.Equals(other_options.Significance_definition_value_2nd)) { equals_other = false; }
            if (!this.Value_importance_order.Equals(other_options.Value_importance_order)) { equals_other = false; }
            if (!this.Keep_top_ranks.Equals(other_options.Keep_top_ranks)) { equals_other = false; }
            if (!this.Merge_upDown_before_ranking.Equals(other_options.Merge_upDown_before_ranking)) { equals_other = false; }
            if (!this.All_genes_significant.Equals(other_options.All_genes_significant)) { equals_other = false; }
            if (!this.Delete_all_not_significant_genes.Equals(other_options.Delete_all_not_significant_genes)) { equals_other = false; }
            return equals_other;
        }

        public override void Write_option_entries(System.IO.StreamWriter writer)
        {
            base.Write_entries_excluding_dictionaries(writer, typeof(User_data_options_class), "Delete_all_not_significant_genes");
        }
        public override bool Add_read_entry_to_options_and_return_if_successful(string readLine)
        {
            return base.Add_read_entry_and_return_if_succesful(readLine, typeof(User_data_options_class));
        }

        public User_data_options_class Deep_copy()
        {
            User_data_options_class copy = (User_data_options_class)this.MemberwiseClone();
            return copy;
        }
    }

    class Custom_data_line_class : IAdd_to_data, ISet_uniqueDatasetName_line
    {
        public static string Not_set_uniqueDatasetName_string { get { return "Not set"; } }

        private string _unique_dataset_identifier;
        public int Results_number { get; set; }
        public Entry_type_enum EntryType 
        { get { return entryType; } 
          set 
          {
                entryType = value;
                switch (entryType)
                {
                    case Entry_type_enum.Up:
                        Value_1st = Math.Abs(Value_1st);
                        break;
                    case Entry_type_enum.Down:
                        Value_1st = -Math.Abs(Value_1st);
                        break;
                    default:
                        throw new Exception();
                }
            }
        }
        public Entry_type_enum entryType { get; set; }
        public float Timepoint { get; set; }
        public Timeunit_enum Timeunit { get; set;}
        public float TimepointInDays { get { return Timeunit_conversion_class.Get_timepoint_in_days(Timepoint, Timeunit); } }
        public string Timeunit_string { get; set; }
        public string Unique_dataset_name { get; set; }
        public string SampleName { get; set; }
        public string NCBI_official_symbol { get; set; }
        public float Fractional_rank { get; set; }
        public double Value_1st { get; set; }
        public double Value_2nd { get; set; }
        public string IntegrationGroup { get; set; }
        public Color SampleColor { get; set; }
        public string Source_fileName { get; set; }
        public string BgGenes_listName { get; set; }
        public Significance_status_enum Significance_status { get; set; }
        public string SampleColor_string { get { return Color_conversion_class.Get_color_string(SampleColor); } set { SampleColor = Color_conversion_class.Set_color_from_string(value); } }

        public string NCBI_official_symbol_for_data { get { return NCBI_official_symbol; } }
        public Entry_type_enum EntryType_for_data { get { return EntryType; } }
        public float Timepoint_for_data { get { return Timepoint; } }
        public Timeunit_enum Timeunit_for_data { get { return Timeunit; } } 
        public string SampleName_for_data { get { return SampleName; } }
        public double Value_for_data { get { return Value_1st; } }
        public string IntegrationGroup_for_data {  get { return IntegrationGroup; } }
        public Color SampleColor_for_data {  get { return SampleColor; } }
        public int Results_number_for_data {  get { return Results_number; } }
        public string UniqueDatasetName_for_data {  get { return Unique_dataset_name; } }
        public string Unique_fixed_dataset_identifier 
        {  
            get { return _unique_dataset_identifier; }
            set
            {
                if (String.IsNullOrEmpty(_unique_dataset_identifier))
                {
                    _unique_dataset_identifier = value;
                }
                else { throw new Exception(); }
            }
        }

        public float Timepoint_in_days
        {
            get
            {
                return Timeunit_conversion_class.Get_timepoint_in_days(Timepoint, Timeunit);
            }
        }

        #region Order
        public static Custom_data_line_class[] Order_sourceFileName_sampleName_timepointInDays_entryType_integrationGroup(Custom_data_line_class[] data_lines)
        {
            //this.Custom_data = this.Custom_data.OrderBy(l => l.Source_fileName).ThenBy(l => l.SampleName).ThenBy(l => l.Timepoint_in_days).ThenBy(l => l.EntryType).ThenBy(l => l.IntegrationGroup).ToArray();
            Dictionary<string, Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>>> sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict = new Dictionary<string, Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>>>();
            Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>> sampleName_timepointInDays_entryType_integrationGroup_dict = new Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>>();
            Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>> timepointInDays_entryType_integrationGroup_dict = new Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>();
            Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>> entryType_integrationGroup_dict = new Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>();
            Dictionary<string, List<Custom_data_line_class>> integrationGroup_dict = new Dictionary<string, List<Custom_data_line_class>>();
            Custom_data_line_class data_line;
            int data_lines_length = data_lines.Length;
            float timepoint_in_days;
            for (int indexD = 0; indexD < data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                timepoint_in_days = data_line.Timepoint_in_days;
                if (!sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict.ContainsKey(data_line.Source_fileName))
                {
                    sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict.Add(data_line.Source_fileName, new Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>>());
                }
                if (!sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName].ContainsKey(data_line.SampleName))
                {
                    sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName].Add(data_line.SampleName, new Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>());
                }
                if (!sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName][data_line.SampleName].ContainsKey(timepoint_in_days))
                {
                    sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName][data_line.SampleName].Add(timepoint_in_days, new Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>());
                }
                if (!sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName][data_line.SampleName][timepoint_in_days].ContainsKey(data_line.EntryType))
                {
                    sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName][data_line.SampleName][timepoint_in_days].Add(data_line.EntryType, new Dictionary<string, List<Custom_data_line_class>>());
                }
                if (!sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName][data_line.SampleName][timepoint_in_days][data_line.EntryType].ContainsKey(data_line.IntegrationGroup))
                {
                    sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName][data_line.SampleName][timepoint_in_days][data_line.EntryType].Add(data_line.IntegrationGroup, new List<Custom_data_line_class>());
                }
                sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName][data_line.SampleName][timepoint_in_days][data_line.EntryType][data_line.IntegrationGroup].Add(data_line);
            }
            data_lines = null;
            List<Custom_data_line_class> ordered_lines = new List<Custom_data_line_class>();
            string[] sourceFileNames;
            string sourceFileName;
            int sourceFileNames_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            float[] timepointInDays_array;
            float timepointInDays;
            int timepointInDays_array_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            sourceFileNames = sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict.Keys.ToArray();
            sourceFileNames = sourceFileNames.OrderBy(l => l).ToArray();
            sourceFileNames_length = sourceFileNames.Length;
            for (int indexSFN=0; indexSFN<sourceFileNames_length;indexSFN++)
            {
                sourceFileName = sourceFileNames[indexSFN];
                sampleName_timepointInDays_entryType_integrationGroup_dict = sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[sourceFileName];
                sampleNames = sampleName_timepointInDays_entryType_integrationGroup_dict.Keys.ToArray();
                sampleNames_length = sampleNames.Length;
                for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                {
                    sampleName = sampleNames[indexSN];
                    timepointInDays_entryType_integrationGroup_dict = sampleName_timepointInDays_entryType_integrationGroup_dict[sampleName];
                    timepointInDays_array = timepointInDays_entryType_integrationGroup_dict.Keys.ToArray();
                    timepointInDays_array = timepointInDays_array.OrderBy(l => l).ToArray();
                    timepointInDays_array_length = timepointInDays_array.Length;
                    for (int indexT = 0; indexT < timepointInDays_array_length; indexT++)
                    {
                        timepointInDays = timepointInDays_array[indexT];
                        entryType_integrationGroup_dict = timepointInDays_entryType_integrationGroup_dict[timepointInDays];
                        entryTypes = entryType_integrationGroup_dict.Keys.ToArray();
                        entryTypes = entryTypes.OrderBy(l=>l).ToArray();
                        entryTypes_length = entryTypes.Length;
                        for (int indexET=0; indexET < sampleNames_length;indexET++)
                        {
                            entryType = entryTypes[indexET];
                            integrationGroup_dict = entryType_integrationGroup_dict[entryType];
                            integrationGroups = integrationGroup_dict.Keys.ToArray();
                            integrationGroups = integrationGroups.OrderBy(l => l).ToArray();
                            integrationGroups_length = integrationGroups.Length;
                            for (int indexIG = 0; indexIG < integrationGroups_length; indexIG++)
                            {
                                integrationGroup = integrationGroups[indexIG];
                                ordered_lines.AddRange(integrationGroup_dict[integrationGroup]);
                            }
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Custom_data_line_class this_line;
                Custom_data_line_class previous_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //Order_sourceFileName_sampleName_timepointInDays_entryType_integrationGroup
                for (int indexThis = 1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    this_timepointInDays = this_line.Timepoint_in_days;
                    previous_timepointInDays = previous_line.Timepoint_in_days;
                    if (this_line.Source_fileName.CompareTo(previous_line.Source_fileName) < 0) { throw new Exception(); }
                    if ((this_line.Source_fileName.Equals(previous_line.Source_fileName))
                        && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    if ((this_line.Source_fileName.Equals(previous_line.Source_fileName))
                        && (this_line.SampleName.Equals(previous_line.SampleName))
                        && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                    if ((this_line.Source_fileName.Equals(previous_line.Source_fileName))
                        && (this_line.SampleName.Equals(previous_line.SampleName))
                        && (this_timepointInDays.Equals(previous_timepointInDays))
                        && (this_line.EntryType.CompareTo(previous_line.EntryType) < 0)) { throw new Exception(); }
                    if ((this_line.Source_fileName.Equals(previous_line.Source_fileName))
                        && (this_line.SampleName.Equals(previous_line.SampleName))
                        && (this_timepointInDays.Equals(previous_timepointInDays))
                        && (this_line.EntryType.Equals(previous_line.EntryType))
                        && (this_line.IntegrationGroup.CompareTo(previous_line.IntegrationGroup) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Custom_data_line_class[] Order_by_integrationGroup_sampleName_timepointInDays_entryType(Custom_data_line_class[] data_lines)
        {
            Dictionary<string, Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, List<Custom_data_line_class>>>>> integrationGroup_sampleName_timepointInDays_entryType_dict = new Dictionary<string, Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, List<Custom_data_line_class>>>>>();
            Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, List<Custom_data_line_class>>>> sampleName_timepointInDays_entryType_dict = new Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, List<Custom_data_line_class>>>>();
            Dictionary<float, Dictionary<Entry_type_enum, List<Custom_data_line_class>>> timepointInDays_entryType_dict = new Dictionary<float, Dictionary<Entry_type_enum, List<Custom_data_line_class>>>();
            Dictionary<Entry_type_enum, List<Custom_data_line_class>> entryType_dict = new Dictionary<Entry_type_enum, List<Custom_data_line_class>>();
            Custom_data_line_class data_line;
            int data_lines_length = data_lines.Length;
            float timepoint_in_days;
            for (int indexD = 0; indexD < data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                timepoint_in_days = data_line.Timepoint_in_days;
                if (!integrationGroup_sampleName_timepointInDays_entryType_dict.ContainsKey(data_line.IntegrationGroup))
                {
                    integrationGroup_sampleName_timepointInDays_entryType_dict.Add(data_line.IntegrationGroup, new Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, List<Custom_data_line_class>>>>());
                }
                if (!integrationGroup_sampleName_timepointInDays_entryType_dict[data_line.IntegrationGroup].ContainsKey(data_line.SampleName))
                {
                    integrationGroup_sampleName_timepointInDays_entryType_dict[data_line.IntegrationGroup].Add(data_line.SampleName, new Dictionary<float, Dictionary<Entry_type_enum, List<Custom_data_line_class>>>());
                }
                if (!integrationGroup_sampleName_timepointInDays_entryType_dict[data_line.IntegrationGroup][data_line.SampleName].ContainsKey(timepoint_in_days))
                {
                    integrationGroup_sampleName_timepointInDays_entryType_dict[data_line.IntegrationGroup][data_line.SampleName].Add(timepoint_in_days, new Dictionary<Entry_type_enum, List<Custom_data_line_class>>());
                }
                if (!integrationGroup_sampleName_timepointInDays_entryType_dict[data_line.IntegrationGroup][data_line.SampleName][timepoint_in_days].ContainsKey(data_line.EntryType))
                {
                    integrationGroup_sampleName_timepointInDays_entryType_dict[data_line.IntegrationGroup][data_line.SampleName][timepoint_in_days].Add(data_line.EntryType, new List<Custom_data_line_class>());
                }
                integrationGroup_sampleName_timepointInDays_entryType_dict[data_line.IntegrationGroup][data_line.SampleName][timepoint_in_days][data_line.EntryType].Add(data_line);
            }
            data_lines = null;
            List<Custom_data_line_class> ordered_lines = new List<Custom_data_line_class>();
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            float[] timepointInDays_array;
            float timepointInDays;
            int timepointInDays_array_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            integrationGroups = integrationGroup_sampleName_timepointInDays_entryType_dict.Keys.ToArray();
            integrationGroups = integrationGroups.OrderBy(l => l).ToArray();
            integrationGroups_length = integrationGroups.Length;
            for (int indexIG = 0; indexIG < integrationGroups_length; indexIG++)
            {
                integrationGroup = integrationGroups[indexIG];
                sampleName_timepointInDays_entryType_dict = integrationGroup_sampleName_timepointInDays_entryType_dict[integrationGroup];
                sampleNames = sampleName_timepointInDays_entryType_dict.Keys.ToArray();
                sampleNames = sampleNames.OrderBy(l => l).ToArray();
                sampleNames_length = sampleNames.Length;
                for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                {
                    sampleName = sampleNames[indexSN];
                    timepointInDays_entryType_dict = sampleName_timepointInDays_entryType_dict[sampleName];
                    timepointInDays_array = timepointInDays_entryType_dict.Keys.ToArray();
                    timepointInDays_array = timepointInDays_array.OrderBy(l => l).ToArray();
                    timepointInDays_array_length = timepointInDays_array.Length;
                    for (int indexT = 0; indexT < timepointInDays_array_length; indexT++)
                    {
                        timepointInDays = timepointInDays_array[indexT];
                        entryType_dict = timepointInDays_entryType_dict[timepointInDays];
                        entryTypes = entryType_dict.Keys.ToArray();
                        entryTypes = entryTypes.OrderBy(l => l).ToArray();
                        entryTypes_length = entryTypes.Length;
                        for (int indexET = 0; indexET < entryTypes_length; indexET++)
                        {
                            entryType = entryTypes[indexET];
                            ordered_lines.AddRange(entryType_dict[entryType]);
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Custom_data_line_class this_line;
                Custom_data_line_class previous_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //integrationGroup_sampleName_timepointInDays_entryType
                for (int indexThis = 1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    this_timepointInDays = this_line.Timepoint_in_days;
                    previous_timepointInDays = previous_line.Timepoint_in_days;
                    if (this_line.IntegrationGroup.CompareTo(previous_line.IntegrationGroup) < 0) { throw new Exception(); }
                    if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                        && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                        && (this_line.SampleName.Equals(previous_line.SampleName))
                        && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                    if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                        && (this_line.SampleName.Equals(previous_line.SampleName))
                        && (this_timepointInDays.Equals(previous_timepointInDays))
                        && (this_line.EntryType.CompareTo(previous_line.EntryType) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Custom_data_line_class[] Order_by_integratinoGroup_resultsNumber_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier(Custom_data_line_class[] data_lines)
        {
            //this.Custom_data.OrderBy(l => l.IntegrationGroup).ThenBy(l => l.Results_number).ThenBy(l => l.SampleName).ThenBy(l => l.Timepoint_in_days).ThenBy(l => l.EntryType).ThenBy(l => l.Unique_fixed_dataset_identifier).ToArray();
            Dictionary<string, Dictionary<int, Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>>>> integrationGroup_resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict = new Dictionary<string, Dictionary<int, Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>>>>();
            Dictionary<int, Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>>> resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict = new Dictionary<int, Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>>>();
            Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>> sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict = new Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>>();
            Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>> timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict = new Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>();
            Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>> entryType_uniqueFixedDatasetIdentifier_dict = new Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>();
            Dictionary<string, List<Custom_data_line_class>> uniqueFixedDatasetIdentifier_dict = new Dictionary<string, List<Custom_data_line_class>>();
            Custom_data_line_class data_line;
            int data_lines_length = data_lines.Length;
            float timepoint_in_days;
            for (int indexD = 0; indexD < data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                timepoint_in_days = data_line.Timepoint_in_days;
                if (!integrationGroup_resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict.ContainsKey(data_line.IntegrationGroup))
                {
                    integrationGroup_resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict.Add(data_line.IntegrationGroup, new Dictionary<int, Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>>>());
                }
                if (!integrationGroup_resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict[data_line.IntegrationGroup].ContainsKey(data_line.Results_number))
                {
                    integrationGroup_resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict[data_line.IntegrationGroup].Add(data_line.Results_number, new Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>>());
                }
                if (!integrationGroup_resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict[data_line.IntegrationGroup][data_line.Results_number].ContainsKey(data_line.SampleName))
                {
                    integrationGroup_resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict[data_line.IntegrationGroup][data_line.Results_number].Add(data_line.SampleName, new Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>());
                }
                if (!integrationGroup_resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict[data_line.IntegrationGroup][data_line.Results_number][data_line.SampleName].ContainsKey(timepoint_in_days))
                {
                    integrationGroup_resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict[data_line.IntegrationGroup][data_line.Results_number][data_line.SampleName].Add(timepoint_in_days, new Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>());
                }
                if (!integrationGroup_resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict[data_line.IntegrationGroup][data_line.Results_number][data_line.SampleName][timepoint_in_days].ContainsKey(data_line.EntryType))
                {
                    integrationGroup_resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict[data_line.IntegrationGroup][data_line.Results_number][data_line.SampleName][timepoint_in_days].Add(data_line.EntryType, new Dictionary<string, List<Custom_data_line_class>>());
                }
                if (!integrationGroup_resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict[data_line.IntegrationGroup][data_line.Results_number][data_line.SampleName][timepoint_in_days][data_line.EntryType].ContainsKey(data_line.Unique_fixed_dataset_identifier))
                {
                    integrationGroup_resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict[data_line.IntegrationGroup][data_line.Results_number][data_line.SampleName][timepoint_in_days][data_line.EntryType].Add(data_line.Unique_fixed_dataset_identifier, new List<Custom_data_line_class>());
                }
                integrationGroup_resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict[data_line.IntegrationGroup][data_line.Results_number][data_line.SampleName][timepoint_in_days][data_line.EntryType][data_line.Unique_fixed_dataset_identifier].Add(data_line);
            }
            data_lines = null;
            List<Custom_data_line_class> ordered_lines = new List<Custom_data_line_class>();
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            int[] resultNos;
            int resultNo;
            int resultNos_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            float[] timepointInDays_array;
            float timepointInDays;
            int timepointInDays_array_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            string[] uniqueFixedDatasetIdentifiers;
            string uniqueFixedDatasetIdentifier;
            int uniqueFixedDatasetIdentifier_length;
            integrationGroups = integrationGroup_resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict.Keys.ToArray();
            integrationGroups = integrationGroups.OrderBy(l => l).ToArray();
            integrationGroups_length = integrationGroups.Length;
            for (int indexIG = 0; indexIG < integrationGroups_length; indexIG++)
            {
                integrationGroup = integrationGroups[indexIG];
                resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict = integrationGroup_resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict[integrationGroup];
                resultNos = resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict.Keys.ToArray();
                resultNos = resultNos.OrderBy(l => l).ToArray();
                resultNos_length = resultNos.Length;
                for (int indexRNo = 0; indexRNo < resultNos_length; indexRNo++)
                {
                    resultNo = resultNos[indexRNo];
                    sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict = resultsNo_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict[resultNo];
                    sampleNames = sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict.Keys.ToArray();
                    sampleNames = sampleNames.OrderBy(l => l).ToArray();
                    sampleNames_length = sampleNames.Length;
                    for (int indexSN=0; indexSN < sampleNames_length;indexSN++)
                    {
                        sampleName = sampleNames[indexSN];
                        timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict = sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict[sampleName];
                        timepointInDays_array = timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict.Keys.ToArray();
                        timepointInDays_array = timepointInDays_array.OrderBy(l => l).ToArray();
                        timepointInDays_array_length = timepointInDays_array.Length;
                        for (int indexT = 0; indexT < timepointInDays_array_length; indexT++)
                        {
                            timepointInDays = timepointInDays_array[indexT];
                            entryType_uniqueFixedDatasetIdentifier_dict = timepointInDays_entryType_uniqueFixedDatasetIdentifier_dict[timepointInDays];
                            entryTypes = entryType_uniqueFixedDatasetIdentifier_dict.Keys.ToArray();
                            entryTypes = entryTypes.OrderBy(l => l).ToArray();
                            entryTypes_length = entryTypes.Length;
                            for (int indexET = 0; indexET < entryTypes_length; indexET++)
                            {
                                entryType = entryTypes[indexET];
                                uniqueFixedDatasetIdentifier_dict = entryType_uniqueFixedDatasetIdentifier_dict[entryType];
                                uniqueFixedDatasetIdentifiers = uniqueFixedDatasetIdentifier_dict.Keys.ToArray();
                                uniqueFixedDatasetIdentifiers = uniqueFixedDatasetIdentifiers.OrderBy(l => l).ToArray();
                                uniqueFixedDatasetIdentifier_length = uniqueFixedDatasetIdentifiers.Length;
                                for (int indexUFID=0; indexUFID < uniqueFixedDatasetIdentifier_length; indexUFID++)
                                {
                                    uniqueFixedDatasetIdentifier = uniqueFixedDatasetIdentifiers[indexUFID];
                                    ordered_lines.AddRange(uniqueFixedDatasetIdentifier_dict[uniqueFixedDatasetIdentifier]);
                                }
                            }
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Custom_data_line_class this_line;
                Custom_data_line_class previous_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //integratinoGroup_resultsNumber_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier
                for (int indexThis = 1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    this_timepointInDays = this_line.Timepoint_in_days;
                    previous_timepointInDays = previous_line.Timepoint_in_days;
                    if (this_line.IntegrationGroup.CompareTo(previous_line.IntegrationGroup) < 0) { throw new Exception(); }
                    if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                        && (this_line.Results_number.CompareTo(previous_line.Results_number) < 0)) { throw new Exception(); }
                    if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                        && (this_line.Results_number.Equals(previous_line.Results_number))
                        && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                        && (this_line.Results_number.Equals(previous_line.Results_number))
                        && (this_line.SampleName.Equals(previous_line.SampleName))
                        && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                    if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                        && (this_line.Results_number.Equals(previous_line.Results_number))
                        && (this_line.SampleName.Equals(previous_line.SampleName))
                        && (this_timepointInDays.Equals(previous_timepointInDays))
                        && (this_line.EntryType.CompareTo(previous_line.EntryType) < 0)) { throw new Exception(); }
                    if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                        && (this_line.Results_number.Equals(previous_line.Results_number))
                        && (this_line.SampleName.Equals(previous_line.SampleName))
                        && (this_timepointInDays.Equals(previous_timepointInDays))
                        && (this_line.EntryType.Equals(previous_line.EntryType))
                        && (this_line.Unique_fixed_dataset_identifier.CompareTo(previous_line.Unique_fixed_dataset_identifier) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Custom_data_line_class[] Order_by_sourceFileName_sampleName_timepointInDays_entryType_integrationGroup(Custom_data_line_class[] data_lines)
        {
            //this.Custom_data.OrderBy(l => l.Source_fileName).ThenBy(l => l.SampleName).ThenBy(l => l.Timepoint_in_days).ThenBy(l => l.EntryType).ThenBy(l => l.IntegrationGroup).ToArray();
            Dictionary<string, Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>>> sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict = new Dictionary<string, Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>>>();
            Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>> sampleName_timepointInDays_entryType_integrationGroup_dict = new Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>>();
            Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>> timepointInDays_entryType_integrationGroup_dict = new Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>();
            Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>> entryType_integrationGroup_dict = new Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>();
            Dictionary<string, List<Custom_data_line_class>> integrationGroup_dict = new Dictionary<string, List<Custom_data_line_class>>();
            Custom_data_line_class data_line;
            int data_lines_length = data_lines.Length;
            float timepoint_in_days;
            for (int indexD = 0; indexD < data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                timepoint_in_days = data_line.Timepoint_in_days;
                if (!sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict.ContainsKey(data_line.Source_fileName))
                {
                    sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict.Add(data_line.Source_fileName, new Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>>());
                }
                if (!sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName].ContainsKey(data_line.SampleName))
                {
                    sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName].Add(data_line.SampleName, new Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>>());
                }
                if (!sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName][data_line.SampleName].ContainsKey(timepoint_in_days))
                {
                    sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName][data_line.SampleName].Add(timepoint_in_days, new Dictionary<Entry_type_enum, Dictionary<string, List<Custom_data_line_class>>>());
                }
                if (!sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName][data_line.SampleName][timepoint_in_days].ContainsKey(data_line.EntryType))
                {
                    sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName][data_line.SampleName][timepoint_in_days].Add(data_line.EntryType, new Dictionary<string, List<Custom_data_line_class>>());
                }
                if (!sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName][data_line.SampleName][timepoint_in_days][data_line.EntryType].ContainsKey(data_line.IntegrationGroup))
                {
                    sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName][data_line.SampleName][timepoint_in_days][data_line.EntryType].Add(data_line.IntegrationGroup, new List<Custom_data_line_class>());
                }
                sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[data_line.Source_fileName][data_line.SampleName][timepoint_in_days][data_line.EntryType][data_line.IntegrationGroup].Add(data_line);
            }
            data_lines = null;
            List<Custom_data_line_class> ordered_lines = new List<Custom_data_line_class>();
            string[] sourceFileNames;
            string sourceFileName;
            int sourceFileNames_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            float[] timepointInDays_array;
            float timepointInDays;
            int timepointInDays_array_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            sourceFileNames = sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict.Keys.ToArray();
            sourceFileNames = sourceFileNames.OrderBy(l => l).ToArray();
            sourceFileNames_length = sourceFileNames.Length;
            for (int indexSFN = 0; indexSFN < sourceFileNames_length; indexSFN++)
            {
                sourceFileName = sourceFileNames[indexSFN];
                sampleName_timepointInDays_entryType_integrationGroup_dict = sourceFileName_sampleName_timepointInDays_entryType_integrationGroup_dict[sourceFileName];
                sampleNames = sampleName_timepointInDays_entryType_integrationGroup_dict.Keys.ToArray();
                sampleNames = sampleNames.OrderBy(l => l).ToArray();
                sampleNames_length = sampleNames.Length;
                for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                {
                    sampleName = sampleNames[indexSN];
                    timepointInDays_entryType_integrationGroup_dict = sampleName_timepointInDays_entryType_integrationGroup_dict[sampleName];
                    timepointInDays_array = timepointInDays_entryType_integrationGroup_dict.Keys.ToArray();
                    timepointInDays_array = timepointInDays_array.OrderBy(l => l).ToArray();
                    timepointInDays_array_length = timepointInDays_array.Length;
                    for (int indexT = 0; indexT < timepointInDays_array_length; indexT++)
                    {
                        timepointInDays = timepointInDays_array[indexT];
                        entryType_integrationGroup_dict = timepointInDays_entryType_integrationGroup_dict[timepointInDays];
                        entryTypes = entryType_integrationGroup_dict.Keys.ToArray();
                        entryTypes = entryTypes.OrderBy(l => l).ToArray();
                        entryTypes_length = entryTypes.Length;
                        for (int indexET = 0; indexET < entryTypes_length; indexET++)
                        {
                            entryType = entryTypes[indexET];
                            integrationGroup_dict = entryType_integrationGroup_dict[entryType];
                            integrationGroups = integrationGroup_dict.Keys.ToArray();
                            integrationGroups = integrationGroups.OrderBy(l => l).ToArray();
                            integrationGroups_length = integrationGroups.Length;
                            for (int indexInt=0; indexInt<integrationGroups_length; indexInt++)
                            {
                                integrationGroup = integrationGroups[indexInt];
                                ordered_lines.AddRange(integrationGroup_dict[integrationGroup]);
                            }
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Custom_data_line_class this_line;
                Custom_data_line_class previous_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //sourceFileName_sampleName_timepointInDays_entryType_integrationGroup
                for (int indexThis = 1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    this_timepointInDays = this_line.Timepoint_in_days;
                    previous_timepointInDays = previous_line.Timepoint_in_days;
                    if (this_line.Source_fileName.CompareTo(previous_line.Source_fileName) < 0) { throw new Exception(); }
                    if ((this_line.Source_fileName.Equals(previous_line.Source_fileName))
                        && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    if ((this_line.Source_fileName.Equals(previous_line.Source_fileName))
                        && (this_line.SampleName.Equals(previous_line.SampleName))
                        && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                    if ((this_line.Source_fileName.Equals(previous_line.Source_fileName))
                        && (this_line.SampleName.Equals(previous_line.SampleName))
                        && (this_timepointInDays.Equals(previous_timepointInDays))
                        && (this_line.EntryType.CompareTo(previous_line.EntryType) < 0)) { throw new Exception(); }
                    if ((this_line.Source_fileName.Equals(previous_line.Source_fileName))
                        && (this_line.SampleName.Equals(previous_line.SampleName))
                        && (this_timepointInDays.Equals(previous_timepointInDays))
                        && (this_line.EntryType.Equals(previous_line.EntryType))
                        && (this_line.IntegrationGroup.CompareTo(previous_line.IntegrationGroup) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Custom_data_line_class[] Order_by_integrationGroup_sampleName_timepointInDays(Custom_data_line_class[] data_lines)
        {
            Dictionary<string, Dictionary<string, Dictionary<float, List<Custom_data_line_class>>>> integrationGroup_sampleName_timepointInDays_dict = new Dictionary<string, Dictionary<string, Dictionary<float, List<Custom_data_line_class>>>>();
            Dictionary<string, Dictionary<float, List<Custom_data_line_class>>> sampleName_timepointInDays_dict = new Dictionary<string, Dictionary<float, List<Custom_data_line_class>>>();
            Dictionary<float, List<Custom_data_line_class>> timepointInDays_dict = new Dictionary<float, List<Custom_data_line_class>>();
            Custom_data_line_class data_line;
            int data_lines_length = data_lines.Length;
            float timepoint_in_days;
            for (int indexD = 0; indexD < data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                timepoint_in_days = data_line.Timepoint_in_days;
                if (!integrationGroup_sampleName_timepointInDays_dict.ContainsKey(data_line.IntegrationGroup))
                {
                    integrationGroup_sampleName_timepointInDays_dict.Add(data_line.IntegrationGroup, new Dictionary<string, Dictionary<float, List<Custom_data_line_class>>>());
                }
                if (!integrationGroup_sampleName_timepointInDays_dict[data_line.IntegrationGroup].ContainsKey(data_line.SampleName))
                {
                    integrationGroup_sampleName_timepointInDays_dict[data_line.IntegrationGroup].Add(data_line.SampleName, new Dictionary<float, List<Custom_data_line_class>>());
                }
                if (!integrationGroup_sampleName_timepointInDays_dict[data_line.IntegrationGroup][data_line.SampleName].ContainsKey(timepoint_in_days))
                {
                    integrationGroup_sampleName_timepointInDays_dict[data_line.IntegrationGroup][data_line.SampleName].Add(timepoint_in_days, new List<Custom_data_line_class>());
                }
                integrationGroup_sampleName_timepointInDays_dict[data_line.IntegrationGroup][data_line.SampleName][timepoint_in_days].Add(data_line);
            }
            data_lines = null;
            List<Custom_data_line_class> ordered_lines = new List<Custom_data_line_class>();
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            float[] timepointInDays_array;
            float timepointInDays;
            int timepointInDays_array_length;
            integrationGroups = integrationGroup_sampleName_timepointInDays_dict.Keys.ToArray();
            integrationGroups = integrationGroups.OrderBy(l => l).ToArray();
            integrationGroups_length = integrationGroups.Length;
            for (int indexIG = 0; indexIG < integrationGroups_length; indexIG++)
            {
                integrationGroup = integrationGroups[indexIG];
                sampleName_timepointInDays_dict = integrationGroup_sampleName_timepointInDays_dict[integrationGroup];
                sampleNames = sampleName_timepointInDays_dict.Keys.ToArray();
                sampleNames = sampleNames.OrderBy(l => l).ToArray();
                sampleNames_length = sampleNames.Length;
                for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                {
                    sampleName = sampleNames[indexSN];
                    timepointInDays_dict = sampleName_timepointInDays_dict[sampleName];
                    timepointInDays_array = timepointInDays_dict.Keys.ToArray();
                    timepointInDays_array = timepointInDays_array.OrderBy(l => l).ToArray();
                    timepointInDays_array_length = timepointInDays_array.Length;
                    for (int indexT = 0; indexT < timepointInDays_array_length; indexT++)
                    {
                        timepointInDays = timepointInDays_array[indexT];
                        ordered_lines.AddRange(timepointInDays_dict[timepointInDays]);
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Custom_data_line_class this_line;
                Custom_data_line_class previous_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //integrationGroup_sampleName_timepointInDays
                for (int indexThis = 1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    this_timepointInDays = this_line.Timepoint_in_days;
                    previous_timepointInDays = previous_line.Timepoint_in_days;
                    if (this_line.IntegrationGroup.CompareTo(previous_line.IntegrationGroup) < 0) { throw new Exception(); }
                    if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                        && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                        && (this_line.SampleName.Equals(previous_line.SampleName))
                        && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Custom_data_line_class[] Order_by_integrationGroup(Custom_data_line_class[] data_lines)
        {
            Dictionary<string, List<Custom_data_line_class>> integrationGroup_dict = new Dictionary<string, List<Custom_data_line_class>>();
            Custom_data_line_class data_line;
            int data_lines_length = data_lines.Length;
            for (int indexD = 0; indexD < data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                if (!integrationGroup_dict.ContainsKey(data_line.IntegrationGroup))
                {
                    integrationGroup_dict.Add(data_line.IntegrationGroup, new List<Custom_data_line_class>());
                }
                integrationGroup_dict[data_line.IntegrationGroup].Add(data_line);
            }
            data_lines = null;
            List<Custom_data_line_class> ordered_lines = new List<Custom_data_line_class>();
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            integrationGroups = integrationGroup_dict.Keys.ToArray();
            integrationGroups = integrationGroups.OrderBy(l => l).ToArray();
            integrationGroups_length = integrationGroups.Length;
            for (int indexIG = 0; indexIG < integrationGroups_length; indexIG++)
            {
                integrationGroup = integrationGroups[indexIG];
                ordered_lines.AddRange(integrationGroup_dict[integrationGroup]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Custom_data_line_class this_line;
                Custom_data_line_class previous_line;
                for (int indexThis = 1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (this_line.IntegrationGroup.CompareTo(previous_line.IntegrationGroup) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Custom_data_line_class[] Order_by_sourceFileName(Custom_data_line_class[] data_lines)
        {
            Dictionary<string, List<Custom_data_line_class>> sourceFileName_dict = new Dictionary<string, List<Custom_data_line_class>>();
            Custom_data_line_class data_line;
            int data_lines_length = data_lines.Length;
            for (int indexD = 0; indexD < data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                if (!sourceFileName_dict.ContainsKey(data_line.Source_fileName))
                {
                    sourceFileName_dict.Add(data_line.Source_fileName, new List<Custom_data_line_class>());
                }
                sourceFileName_dict[data_line.Source_fileName].Add(data_line);
            }
            data_lines = null;
            List<Custom_data_line_class> ordered_lines = new List<Custom_data_line_class>();
            string[] sourceFileNames;
            string sourceFileName;
            int sourceFileNames_length;
            sourceFileNames = sourceFileName_dict.Keys.ToArray();
            sourceFileNames = sourceFileNames.OrderBy(l => l).ToArray();
            sourceFileNames_length = sourceFileNames.Length;
            for (int indexSN = 0; indexSN < sourceFileNames_length; indexSN++)
            {
                sourceFileName = sourceFileNames[indexSN];
                ordered_lines.AddRange(sourceFileName_dict[sourceFileName]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Custom_data_line_class this_line;
                Custom_data_line_class previous_line;
                for (int indexThis = 1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (this_line.Source_fileName.CompareTo(previous_line.Source_fileName) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Custom_data_line_class[] Order_by_absValue1st(Custom_data_line_class[] data_lines)
        {
            Dictionary<double, List<Custom_data_line_class>> value1st_dict = new Dictionary<double, List<Custom_data_line_class>>();
            Custom_data_line_class data_line;
            int data_lines_length = data_lines.Length;
            for (int indexD = 0; indexD < data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                if (!value1st_dict.ContainsKey(data_line.Value_1st))
                {
                    value1st_dict.Add(data_line.Value_1st, new List<Custom_data_line_class>());
                }
                value1st_dict[data_line.Value_1st].Add(data_line);
            }
            data_lines = null;
            List<Custom_data_line_class> ordered_lines = new List<Custom_data_line_class>();
            double[] value1sts;
            double value1st;
            int value1sts_length;
            value1sts = value1st_dict.Keys.ToArray();
            value1sts = value1sts.OrderBy(l => Math.Abs(l)).ToArray();
            value1sts_length = value1sts.Length;
            for (int indexV = 0; indexV < value1sts_length; indexV++)
            {
                value1st = value1sts[indexV];
                ordered_lines.AddRange(value1st_dict[value1st]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Custom_data_line_class this_line;
                Custom_data_line_class previous_line;
                for (int indexThis = 1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (Math.Abs(this_line.Value_1st).CompareTo(Math.Abs(previous_line.Value_1st)) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Custom_data_line_class[] Order_by_descendingAbsValue1st(Custom_data_line_class[] data_lines)
        {
            Dictionary<double, List<Custom_data_line_class>> value1st_dict = new Dictionary<double, List<Custom_data_line_class>>();
            Custom_data_line_class data_line;
            int data_lines_length = data_lines.Length;
            for (int indexD = 0; indexD < data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                if (!value1st_dict.ContainsKey(data_line.Value_1st))
                {
                    value1st_dict.Add(data_line.Value_1st, new List<Custom_data_line_class>());
                }
                value1st_dict[data_line.Value_1st].Add(data_line);
            }
            data_lines = null;
            List<Custom_data_line_class> ordered_lines = new List<Custom_data_line_class>();
            double[] value1sts;
            double value1st;
            int value1sts_length;
            value1sts = value1st_dict.Keys.ToArray();
            value1sts = value1sts.OrderByDescending(l => Math.Abs(l)).ToArray();
            value1sts_length = value1sts.Length;
            for (int indexV = 0; indexV < value1sts_length; indexV++)
            {
                value1st = value1sts[indexV];
                ordered_lines.AddRange(value1st_dict[value1st]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Custom_data_line_class this_line;
                Custom_data_line_class previous_line;
                for (int indexThis = 1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (Math.Abs(this_line.Value_1st).CompareTo(Math.Abs(previous_line.Value_1st)) > 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Custom_data_line_class[] Order_by_absValue2nd(Custom_data_line_class[] data_lines)
        {
            //this.Custom_data = Custom_data.OrderByDescending(l => Math.Abs(l.Value_2nd)).ToArray();
            Dictionary<double, List<Custom_data_line_class>> value2nd_dict = new Dictionary<double, List<Custom_data_line_class>>();
            Custom_data_line_class data_line;
            int data_lines_length = data_lines.Length;
            for (int indexD = 0; indexD < data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                if (!value2nd_dict.ContainsKey(data_line.Value_2nd))
                {
                    value2nd_dict.Add(data_line.Value_2nd, new List<Custom_data_line_class>());
                }
                value2nd_dict[data_line.Value_2nd].Add(data_line);
            }
            data_lines = null;
            List<Custom_data_line_class> ordered_lines = new List<Custom_data_line_class>();
            double[] value2nds;
            double value2nd;
            int value2nds_length;
            value2nds = value2nd_dict.Keys.ToArray();
            value2nds = value2nds.OrderBy(l => Math.Abs(l)).ToArray();
            value2nds_length = value2nds.Length;
            for (int indexV = 0; indexV < value2nds_length; indexV++)
            {
                value2nd = value2nds[indexV];
                ordered_lines.AddRange(value2nd_dict[value2nd]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Custom_data_line_class this_line;
                Custom_data_line_class previous_line;
                for (int indexThis = 1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (Math.Abs(this_line.Value_2nd).CompareTo(Math.Abs(previous_line.Value_2nd)) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Custom_data_line_class[] Order_by_descendingAbsValue2nd(Custom_data_line_class[] data_lines)
        {
            //this.Custom_data = Custom_data.OrderByDescending(l => Math.Abs(l.Value_2nd)).ToArray();
            Dictionary<double, List<Custom_data_line_class>> value2nd_dict = new Dictionary<double, List<Custom_data_line_class>>();
            Custom_data_line_class data_line;
            int data_lines_length = data_lines.Length;
            for (int indexD = 0; indexD < data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                if (!value2nd_dict.ContainsKey(data_line.Value_2nd))
                {
                    value2nd_dict.Add(data_line.Value_2nd, new List<Custom_data_line_class>());
                }
                value2nd_dict[data_line.Value_2nd].Add(data_line);
            }
            data_lines = null;
            List<Custom_data_line_class> ordered_lines = new List<Custom_data_line_class>();
            double[] value2nds;
            double value2nd;
            int value2nds_length;
            value2nds = value2nd_dict.Keys.ToArray();
            value2nds = value2nds.OrderByDescending(l => Math.Abs(l)).ToArray();
            value2nds_length = value2nds.Length;
            for (int indexIG = 0; indexIG < value2nds_length; indexIG++)
            {
                value2nd = value2nds[indexIG];
                ordered_lines.AddRange(value2nd_dict[value2nd]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Custom_data_line_class this_line;
                Custom_data_line_class previous_line;
                for (int indexThis = 1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (Math.Abs(this_line.Value_2nd).CompareTo(Math.Abs(previous_line.Value_2nd)) > 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }

        public static Custom_data_line_class[] Order_by_uniqueFixedDatasetIdentifier(Custom_data_line_class[] data_lines)
        {
            Dictionary<string, List<Custom_data_line_class>> uniqueFixedDatasetIdentifier_dict = new Dictionary<string, List<Custom_data_line_class>>();
            Custom_data_line_class data_line;
            int data_lines_length = data_lines.Length;
            for (int indexD = 0; indexD < data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                if (!uniqueFixedDatasetIdentifier_dict.ContainsKey(data_line.Unique_fixed_dataset_identifier))
                {
                    uniqueFixedDatasetIdentifier_dict.Add(data_line.Unique_fixed_dataset_identifier, new List<Custom_data_line_class>());
                }
                uniqueFixedDatasetIdentifier_dict[data_line.Unique_fixed_dataset_identifier].Add(data_line);
            }
            data_lines = null;
            List<Custom_data_line_class> ordered_lines = new List<Custom_data_line_class>();
            string[] uniqueFixedDatasetIdentifiers = uniqueFixedDatasetIdentifier_dict.Keys.ToArray();
            string uniqueFixedDatasetIdentifier;
            int uniqueFixedDatasetIdentifiers_length = uniqueFixedDatasetIdentifiers.Length;
            uniqueFixedDatasetIdentifiers = uniqueFixedDatasetIdentifiers.OrderBy(l => l).ToArray();
            for (int indexU = 0; indexU < uniqueFixedDatasetIdentifiers_length; indexU++)
            {
                uniqueFixedDatasetIdentifier = uniqueFixedDatasetIdentifiers[indexU];
                ordered_lines.AddRange(uniqueFixedDatasetIdentifier_dict[uniqueFixedDatasetIdentifier]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Custom_data_line_class this_line;
                Custom_data_line_class previous_line;
                for (int indexThis = 1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (this_line.Unique_fixed_dataset_identifier.CompareTo(previous_line.Unique_fixed_dataset_identifier) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Custom_data_line_class[] Order_by_uniqueFixedDatasetIdentifier_ncbiOfficialSymbol(Custom_data_line_class[] data_lines)
        {
            //this.Custom_data = this.Custom_data.OrderBy(l => l.Unique_fixed_dataset_identifier).ThenBy(l => l.NCBI_official_symbol).ToArray();
            Dictionary<string, Dictionary<string, List<Custom_data_line_class>>> uniqueFixedDatasetIdentifier_symbol_dict = new Dictionary<string, Dictionary<string, List<Custom_data_line_class>>>();
            Dictionary<string, List<Custom_data_line_class>> symbol_dict = new Dictionary<string, List<Custom_data_line_class>>();
            Custom_data_line_class data_line;
            int data_lines_length = data_lines.Length;
            for (int indexD = 0; indexD < data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                if (!uniqueFixedDatasetIdentifier_symbol_dict.ContainsKey(data_line.Unique_fixed_dataset_identifier))
                {
                    uniqueFixedDatasetIdentifier_symbol_dict.Add(data_line.Unique_fixed_dataset_identifier, new Dictionary<string, List<Custom_data_line_class>>());
                }
                if (!uniqueFixedDatasetIdentifier_symbol_dict[data_line.Unique_fixed_dataset_identifier].ContainsKey(data_line.NCBI_official_symbol))
                {
                    uniqueFixedDatasetIdentifier_symbol_dict[data_line.Unique_fixed_dataset_identifier].Add(data_line.NCBI_official_symbol, new List<Custom_data_line_class>());
                }
                uniqueFixedDatasetIdentifier_symbol_dict[data_line.Unique_fixed_dataset_identifier][data_line.NCBI_official_symbol].Add(data_line);
            }
            data_lines = null;
            List<Custom_data_line_class> ordered_lines = new List<Custom_data_line_class>();
            string[] uniqueFixedDatasetIdentifiers;
            string uniqueFixedDatasetIdentifier;
            int uniqueFixedDatasetIdentifiers_length;
            string[] symbols;
            string symbol;
            int symbols_length;
            uniqueFixedDatasetIdentifiers = uniqueFixedDatasetIdentifier_symbol_dict.Keys.ToArray();
            uniqueFixedDatasetIdentifiers = uniqueFixedDatasetIdentifiers.OrderBy(l => l).ToArray();
            uniqueFixedDatasetIdentifiers_length = uniqueFixedDatasetIdentifiers.Length;
            for (int indexU = 0; indexU < uniqueFixedDatasetIdentifiers_length; indexU++)
            {
                uniqueFixedDatasetIdentifier = uniqueFixedDatasetIdentifiers[indexU];
                symbol_dict = uniqueFixedDatasetIdentifier_symbol_dict[uniqueFixedDatasetIdentifier];
                symbols = symbol_dict.Keys.ToArray();
                symbols = symbols.OrderBy(l => l).ToArray();
                symbols_length = symbols.Length;
                for (int indexS=0; indexS<symbols_length; indexS++)
                {
                    symbol = symbols[indexS];
                    ordered_lines.AddRange(symbol_dict[symbol]);
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Custom_data_line_class this_line;
                Custom_data_line_class previous_line;
                //uniqueFixedDatasetIdentifier_ncbiGeneSymbol
                for (int indexThis = 1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (this_line.Unique_fixed_dataset_identifier.CompareTo(previous_line.Unique_fixed_dataset_identifier) < 0) { throw new Exception(); }
                    if (  (this_line.Unique_fixed_dataset_identifier.Equals(previous_line.Unique_fixed_dataset_identifier))
                        &&(this_line.NCBI_official_symbol.CompareTo(previous_line.NCBI_official_symbol)<0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Custom_data_line_class[] Order_by_backgroundGenesListName(Custom_data_line_class[] data_lines)
        {
            Dictionary<string, List<Custom_data_line_class>> backgroundGenesList_dict = new Dictionary<string, List<Custom_data_line_class>>();
            Custom_data_line_class data_line;
            int data_lines_length = data_lines.Length;
            for (int indexD = 0; indexD < data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                if (!backgroundGenesList_dict.ContainsKey(data_line.BgGenes_listName))
                {
                    backgroundGenesList_dict.Add(data_line.BgGenes_listName, new List<Custom_data_line_class>());
                }
                backgroundGenesList_dict[data_line.BgGenes_listName].Add(data_line);
            }
            data_lines = null;
            List<Custom_data_line_class> ordered_lines = new List<Custom_data_line_class>();
            string[] backgroundGenesLists = backgroundGenesList_dict.Keys.ToArray();
            string backgroundGenesList;
            int backgroundGenesLists_length = backgroundGenesLists.Length;
            backgroundGenesLists = backgroundGenesLists.OrderBy(l => l).ToArray();
            for (int indexBGL = 0; indexBGL < backgroundGenesLists_length; indexBGL++)
            {
                backgroundGenesList = backgroundGenesLists[indexBGL];
                ordered_lines.AddRange(backgroundGenesList_dict[backgroundGenesList]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Custom_data_line_class this_line;
                Custom_data_line_class previous_line;
                for (int indexThis = 1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (this_line.BgGenes_listName.CompareTo(previous_line.BgGenes_listName) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Custom_data_line_class[] Order_by_color(Custom_data_line_class[] data_lines)
        {
            Dictionary<System.Drawing.Color, List<Custom_data_line_class>> color_dict = new Dictionary<System.Drawing.Color, List<Custom_data_line_class>>();
            Custom_data_line_class data_line;
            int data_lines_length = data_lines.Length;
            for (int indexD = 0; indexD < data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                if (!color_dict.ContainsKey(data_line.SampleColor))
                {
                    color_dict.Add(data_line.SampleColor, new List<Custom_data_line_class>());
                }
                color_dict[data_line.SampleColor].Add(data_line);
            }
            data_lines = null;
            List<Custom_data_line_class> ordered_lines = new List<Custom_data_line_class>();
            System.Drawing.Color[] colors = color_dict.Keys.ToArray();
            System.Drawing.Color color;
            int colors_length = colors.Length;
            colors = colors.OrderBy(l => l).ToArray();
            for (int indexC = 0; indexC < colors_length; indexC++)
            {
                color = colors[indexC];
                ordered_lines.AddRange(color_dict[color]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Custom_data_line_class this_line;
                Custom_data_line_class previous_line;
                for (int indexThis = 1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    //if (this_line.SampleColor.CompareTo(previous_line.SampleColor) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        #endregion



        public Custom_data_line_class()
        {
            this.IntegrationGroup = "";
            this.SampleName = "";
            this.BgGenes_listName = "";
            this.Source_fileName = "";
            this.NCBI_official_symbol = "";
            this.Unique_dataset_name = (string)Not_set_uniqueDatasetName_string.Clone();
            this.Fractional_rank = -1;
            this.Timeunit_string = "";
        }

        public Custom_data_line_class Deep_copy()
        {
            Custom_data_line_class copy = (Custom_data_line_class)this.MemberwiseClone();
            copy.SampleName = (string)this.SampleName.Clone();
            copy.NCBI_official_symbol = (string)this.NCBI_official_symbol.Clone();
            copy.Timeunit_string = (string)this.Timeunit_string.Clone();
            copy.IntegrationGroup = (string)this.IntegrationGroup.Clone();
            copy.BgGenes_listName = (string)this.BgGenes_listName.Clone();
            copy.Source_fileName = (string)this.Source_fileName.Clone();
            copy.Unique_fixed_dataset_identifier = (string)this.Unique_fixed_dataset_identifier.Clone();
            copy.Unique_dataset_name = (string)this.Unique_dataset_name.Clone();
            return copy;
        }
    }

    class Custom_data_readWriteOptions_class : ReadWriteOptions_base
    {
        public static string ColumName_results_order_no_for_data { get { return "Dataset order #"; } }
        public Custom_data_readWriteOptions_class(string subdirectory, string file_name)
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            string directory = "";
            directory = subdirectory;
            this.File = directory + file_name;
            Key_propertyNames = new string[] { };// "EntryType", "Timepoint", "Time_unit", "SampleName", "SampleColor_string", "NCBI_official_symbol", "Value", "BgGenes_listName", "Source_fileName", "Unique_fixed_dataset_identifier" };
            Key_columnNames = Key_propertyNames;
            HeadlineDelimiters = new char[] { Global_class.Tab };
            LineDelimiters = new char[] { Global_class.Tab };
            File_has_headline = true;
            Report = ReadWrite_report_enum.Report_nothing;
            Remove_not_existing_columnNames_and_corresponding_properties = true;
        }
    }

    class Custom_data_writeOptions_class : ReadWriteOptions_base
    {
        public Custom_data_writeOptions_class(string subdirectory, string file_name, bool write_integrationGroup, bool write_timepoints, bool write_entryType)
        {
            string directory = "";
            directory = subdirectory;

            this.File = directory + file_name;
            List<string> key_propertyNames_list = new List<string>();
            List<string> key_columnNames_list = new List<string>();
            if (write_integrationGroup)
            {
                key_propertyNames_list.AddRange(new string[] { "IntegrationGroup" });
                key_columnNames_list.AddRange(new string[] { "Integration group" });
            }
            key_propertyNames_list.AddRange(new string[] { "SampleName" });
            key_columnNames_list.AddRange(new string[] { "Dataset name" });
            if (write_timepoints)
            {
                key_propertyNames_list.AddRange(new string[] { "Timepoint", "Timeunit" });
                key_columnNames_list.AddRange(new string[] { "Timepoint", "Timeunit" });
            }
            if (write_entryType)
            {
                key_propertyNames_list.AddRange(new string[] { "EntryType" });
                key_columnNames_list.AddRange(new string[] { "UpDown Status" });
            }
            key_propertyNames_list.AddRange(new string[] { "SampleColor_string", "NCBI_official_symbol", "Value_1st", "Value_2nd","Fractional_rank","BgGenes_listName", "Source_fileName", "Significance_status", "Results_number" });
            key_columnNames_list.AddRange(new string[] { "Dataset color", "NCBI official gene symbol", "Value_1st", "Value_2nd","Rank","Background gene list", "Source", "Significant", Custom_data_readWriteOptions_class.ColumName_results_order_no_for_data });
            HeadlineDelimiters = new char[] { Global_class.Tab };
            LineDelimiters = new char[] { Global_class.Tab };
            File_has_headline = true;
            Report = ReadWrite_report_enum.Report_nothing;
            Remove_not_existing_columnNames_and_corresponding_properties = true;
            this.Key_propertyNames = key_propertyNames_list.ToArray();
            this.Key_columnNames = key_columnNames_list.ToArray();
        }
    }

    class Custom_data_class
    {
        public Custom_data_line_class[] Custom_data { get; set; }
        public Dictionary<string, string[]> ExpBgGenesList_bgGenes_dict { get; set; }
        public User_data_options_class Options { get; set; }
        const string Manual_added_unique_fixed_dataset_identifier_baseName = "Manually_added_dataset_no";
        public Custom_data_class()
        {
            Custom_data = new Custom_data_line_class[0];
            ExpBgGenesList_bgGenes_dict = new Dictionary<string,string[]>();
            Options = new User_data_options_class();
            Clear_and_reset_expBgGenesList_to_default();
        }

        #region Check
        public void Check_for_correctness()
        {
            int custom_data_length = this.Custom_data.Length;
            Custom_data_line_class previous_custom_data_line;
            Custom_data_line_class this_custom_data_line;
            //this.Custom_data = this.Custom_data.OrderBy(l => l.Unique_fixed_dataset_identifier).ToArray();
            this.Custom_data = Custom_data_line_class.Order_by_uniqueFixedDatasetIdentifier(this.Custom_data);
            List<Read_error_message_line_class> error_messages = new List<Read_error_message_line_class>();
            for (int indexC = 0; indexC < custom_data_length; indexC++)
            {
                this_custom_data_line = this.Custom_data[indexC];
                if (String.IsNullOrEmpty(this_custom_data_line.Unique_fixed_dataset_identifier)) { throw new Exception(); }
                if (this_custom_data_line.EntryType.Equals(Entry_type_enum.E_m_p_t_y)) { throw new Exception(); }
                if (this_custom_data_line.Timeunit.Equals(Timeunit_enum.E_m_p_t_y)) { throw new Exception(); }
                if (indexC > 0)
                {
                    previous_custom_data_line = this.Custom_data[indexC - 1];
                    if (previous_custom_data_line.Unique_fixed_dataset_identifier.Equals(this_custom_data_line.Unique_fixed_dataset_identifier))
                    {
                        if (!this_custom_data_line.SampleName.Equals(previous_custom_data_line.SampleName)) { throw new Exception(); }
                        if (!this_custom_data_line.SampleColor.Equals(previous_custom_data_line.SampleColor)) { throw new Exception(); }
                        if (!this_custom_data_line.Timepoint.Equals(previous_custom_data_line.Timepoint)) { throw new Exception(); }
                        if (!this_custom_data_line.Timeunit.Equals(previous_custom_data_line.Timeunit)) { throw new Exception(); }
                        if (!this_custom_data_line.EntryType.Equals(previous_custom_data_line.EntryType)) { throw new Exception(); }
                        if (!this_custom_data_line.Results_number.Equals(previous_custom_data_line.Results_number)) { throw new Exception(); }
                        if (!this_custom_data_line.IntegrationGroup.Equals(previous_custom_data_line.IntegrationGroup)) { throw new Exception(); }
                        if (!this_custom_data_line.Source_fileName.Equals(previous_custom_data_line.Source_fileName)) { throw new Exception(); }
                    }
                }
            }
        }
        #endregion

        public bool Analyse_if_all_timepoints_larger_zero()
        {
            bool all_above_zero = true;
            foreach (Custom_data_line_class custom_data_line in this.Custom_data)
            {
                if (custom_data_line.Timepoint<=0) { all_above_zero = false;break; }
            }
            return all_above_zero;
        }

        public bool Analyze_if_data_contains_at_least_one_non_zero_value_1st()
        {
            bool contains_non_zero_value_1st = false;
            foreach (Custom_data_line_class custom_data_line in this.Custom_data)
            {
                if (custom_data_line.Value_1st!=0) 
                { 
                    contains_non_zero_value_1st = true;
                    break;
                }
            }
            return contains_non_zero_value_1st;
        }

        public bool Analyse_if_data_can_be_submitted_to_enrichment_analysis(bool timeline_in_log_scale_requested)
        {
            bool submission_allowed = true;
            if (this.Custom_data.Length == 0) { submission_allowed = false; }
            else
            {
                Check_for_correctness();
                int custom_data_length = Custom_data.Length;
                Custom_data_line_class custom_data_line;
                Dictionary<string, Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, bool>>>>> integrationGroup_sampleName_timepointInDays_entryType_symbol_dict = new Dictionary<string, Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, bool>>>>>();

                float timepointInDays;
                for (int indexC = 1; indexC < custom_data_length; indexC++)
                {
                    custom_data_line = this.Custom_data[indexC];
                    timepointInDays = custom_data_line.Timepoint_in_days;
                    if (!integrationGroup_sampleName_timepointInDays_entryType_symbol_dict.ContainsKey(custom_data_line.IntegrationGroup))
                    {
                        integrationGroup_sampleName_timepointInDays_entryType_symbol_dict.Add(custom_data_line.IntegrationGroup, new Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, bool>>>>());
                    }
                    if (!integrationGroup_sampleName_timepointInDays_entryType_symbol_dict[custom_data_line.IntegrationGroup].ContainsKey(custom_data_line.SampleName))
                    {
                        integrationGroup_sampleName_timepointInDays_entryType_symbol_dict[custom_data_line.IntegrationGroup].Add(custom_data_line.SampleName, new Dictionary<float, Dictionary<Entry_type_enum, Dictionary<string, bool>>>());
                    }
                    if (!integrationGroup_sampleName_timepointInDays_entryType_symbol_dict[custom_data_line.IntegrationGroup][custom_data_line.SampleName].ContainsKey(timepointInDays))
                    {
                        integrationGroup_sampleName_timepointInDays_entryType_symbol_dict[custom_data_line.IntegrationGroup][custom_data_line.SampleName].Add(timepointInDays, new Dictionary<Entry_type_enum, Dictionary<string, bool>>());
                    }
                    if (!integrationGroup_sampleName_timepointInDays_entryType_symbol_dict[custom_data_line.IntegrationGroup][custom_data_line.SampleName][timepointInDays].ContainsKey(custom_data_line.EntryType))
                    {
                        integrationGroup_sampleName_timepointInDays_entryType_symbol_dict[custom_data_line.IntegrationGroup][custom_data_line.SampleName][timepointInDays].Add(custom_data_line.EntryType, new Dictionary<string, bool>());
                    }
                    if (!integrationGroup_sampleName_timepointInDays_entryType_symbol_dict[custom_data_line.IntegrationGroup][custom_data_line.SampleName][timepointInDays][custom_data_line.EntryType].ContainsKey(custom_data_line.NCBI_official_symbol))
                    {
                        integrationGroup_sampleName_timepointInDays_entryType_symbol_dict[custom_data_line.IntegrationGroup][custom_data_line.SampleName][timepointInDays][custom_data_line.EntryType].Add(custom_data_line.NCBI_official_symbol, true);
                    }
                    else
                    {
                        submission_allowed = false;
                    }
                    //previous_custom_data_line = this.Custom_data[indexC - 1];
                    //if (   (custom_data_line.IntegrationGroup.Equals(previous_custom_data_line.IntegrationGroup))
                    //    && (custom_data_line.SampleName.Equals(previous_custom_data_line.SampleName))
                    //    && (custom_data_line.Timepoint_in_days.Equals(previous_custom_data_line.Timepoint_in_days))
                    //    && (custom_data_line.EntryType.Equals(previous_custom_data_line.EntryType))
                    //    && (custom_data_line.NCBI_official_symbol.Equals(previous_custom_data_line.NCBI_official_symbol)))
                    //{
                    //    submission_allowed = false;
                    //    break;
                    //}
                }
                if (timeline_in_log_scale_requested)
                {
                    float[] timepointsInDays = this.Get_all_unique_ordered_timepointsInDays();
                    //timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
                    foreach (int timepointInDay in timepointsInDays)
                    {
                        if (timepointInDay <= 0) 
                        { 
                            submission_allowed = false;
                            break;
                        }
                    }
                }
            }
            return submission_allowed;
        }

        #region Ranks and significance
        private void Calculate_fractional_ranks_for_datasets_specified_in_options()
        {
            switch (Options.Value_importance_order)
            {
                case Value_importance_order_enum.Value_1st_2nd:
                    switch (Options.Significance_definition_value_2nd)
                    {
                        case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                            //this.Custom_data = Custom_data.OrderByDescending(l => Math.Abs(l.Value_2nd)).ToArray();
                            this.Custom_data = Custom_data_line_class.Order_by_descendingAbsValue2nd(this.Custom_data);
                            break;
                        case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                            //this.Custom_data = Custom_data.OrderBy(l => Math.Abs(l.Value_2nd)).ToArray();
                            this.Custom_data = Custom_data_line_class.Order_by_absValue2nd(this.Custom_data);
                            break;
                        default:
                            throw new Exception();
                    }
                    switch (Options.Significance_definition_value_1st)
                    {
                        case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                            //this.Custom_data = Custom_data.OrderByDescending(l => Math.Abs(l.Value_1st)).ToArray();
                            this.Custom_data = Custom_data_line_class.Order_by_descendingAbsValue1st(this.Custom_data);
                            break;
                        case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                            //this.Custom_data = Custom_data.OrderBy(l => Math.Abs(l.Value_1st)).ToArray();
                            this.Custom_data = Custom_data_line_class.Order_by_absValue1st(this.Custom_data);
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case Value_importance_order_enum.Value_2nd_1st:
                    switch (Options.Significance_definition_value_1st)
                    {
                        case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                            //this.Custom_data = Custom_data.OrderByDescending(l => Math.Abs(l.Value_1st)).ToArray();
                            this.Custom_data = Custom_data_line_class.Order_by_descendingAbsValue1st(this.Custom_data);
                            break;
                        case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                            //this.Custom_data = Custom_data.OrderBy(l => Math.Abs(l.Value_1st)).ToArray();
                            this.Custom_data = Custom_data_line_class.Order_by_absValue1st(this.Custom_data);
                            break;
                        default:
                            throw new Exception();
                    }
                    switch (Options.Significance_definition_value_2nd)
                    {
                        case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                            //this.Custom_data = Custom_data.OrderByDescending(l => Math.Abs(l.Value_2nd)).ToArray();
                            this.Custom_data = Custom_data_line_class.Order_by_descendingAbsValue2nd(this.Custom_data);
                            break;
                        case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                            //this.Custom_data = Custom_data.OrderBy(l => Math.Abs(l.Value_2nd)).ToArray();
                            this.Custom_data = Custom_data_line_class.Order_by_absValue2nd(this.Custom_data);
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                default:
                    throw new Exception();
            }
            if (Options.Merge_upDown_before_ranking)
            {
                //Custom_data = Custom_data.OrderBy(l => l.IntegrationGroup).ThenBy(l => l.SampleName).ThenBy(l => l.Timepoint_in_days).ToArray();
                Custom_data = Custom_data_line_class.Order_by_integrationGroup_sampleName_timepointInDays(Custom_data);
            }
            else
            {
                //Custom_data = Custom_data.OrderBy(l => l.IntegrationGroup).ThenBy(l => l.SampleName).ThenBy(l => l.Timepoint_in_days).ThenBy(l => l.EntryType).ToArray();
                Custom_data = Custom_data_line_class.Order_by_integrationGroup_sampleName_timepointInDays_entryType(Custom_data);
            }
            int custom_count = Custom_data.Length;
            Custom_data_line_class custom_data_line;
            Custom_data_line_class inner_custom_data_line;
            int running_rank = -1;
            int firstIndexSameRank = -1;
            List<float> current_running_ranks = new List<float>();
            float current_rank;
            bool set_ranks = false;
            bool delete_all_ranks = false;
            Dictionary<Entry_type_enum, string> entryType_uniqueIdentifier_dict = new Dictionary<Entry_type_enum, string>();
            for (int indexC = 0; indexC < custom_count; indexC++)
            {
                custom_data_line = Custom_data[indexC];
                set_ranks = false;
                if (Options.Merge_upDown_before_ranking)
                {
                    if ((indexC == 0)
                        || (!custom_data_line.IntegrationGroup.Equals(Custom_data[indexC - 1].IntegrationGroup))
                        || (!custom_data_line.SampleName.Equals(Custom_data[indexC - 1].SampleName))
                        || (!custom_data_line.Timepoint_in_days.Equals(Custom_data[indexC - 1].Timepoint_in_days)))
                    {
                        running_rank = 0;
                        entryType_uniqueIdentifier_dict.Clear();
                    }
                    else
                    {
                        if (!entryType_uniqueIdentifier_dict.ContainsKey(custom_data_line.EntryType))
                        {
                            entryType_uniqueIdentifier_dict.Add(custom_data_line.EntryType, custom_data_line.Unique_fixed_dataset_identifier);
                        }
                        else if (!entryType_uniqueIdentifier_dict[custom_data_line.EntryType].Equals(custom_data_line.Unique_fixed_dataset_identifier))
                        { delete_all_ranks = true; }

                        #region Check, if values sorted correctly, copy paste
                        switch (Options.Value_importance_order)
                        {
                            case Value_importance_order_enum.Value_1st_2nd:
                                switch (Options.Significance_definition_value_1st)
                                {
                                    case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                                        if (Math.Abs(custom_data_line.Value_1st).CompareTo(Math.Abs(Custom_data[indexC - 1].Value_1st)) > 0)
                                        { throw new Exception(); }
                                        break;
                                    case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                                        if (Math.Abs(custom_data_line.Value_1st).CompareTo(Math.Abs(Custom_data[indexC - 1].Value_1st)) < 0)
                                        { throw new Exception(); }
                                        break;
                                    default:
                                        throw new Exception();
                                }
                                switch (Options.Significance_definition_value_2nd)
                                {
                                    case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                                        if ((Math.Abs(custom_data_line.Value_1st).Equals(Math.Abs(Custom_data[indexC - 1].Value_1st)))
                                            && (Math.Abs(custom_data_line.Value_2nd).CompareTo(Math.Abs(Custom_data[indexC - 1].Value_2nd)) > 0))
                                        { throw new Exception(); }
                                        break;
                                    case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                                        if ((Math.Abs(custom_data_line.Value_1st).Equals(Math.Abs(Custom_data[indexC - 1].Value_1st)))
                                            && (Math.Abs(custom_data_line.Value_2nd).CompareTo(Math.Abs(Custom_data[indexC - 1].Value_2nd)) < 0))
                                        { throw new Exception(); }
                                        break;
                                    default:
                                        throw new Exception();
                                }
                                break;
                            case Value_importance_order_enum.Value_2nd_1st:
                                switch (Options.Significance_definition_value_2nd)
                                {
                                    case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                                        if (Math.Abs(custom_data_line.Value_2nd).CompareTo(Math.Abs(Custom_data[indexC - 1].Value_2nd)) > 0)
                                        { throw new Exception(); }
                                        break;
                                    case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                                        if (Math.Abs(custom_data_line.Value_2nd).CompareTo(Math.Abs(Custom_data[indexC - 1].Value_2nd)) < 0)
                                        { throw new Exception(); }
                                        break;
                                    default:
                                        throw new Exception();
                                }
                                switch (Options.Significance_definition_value_1st)
                                {
                                    case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                                        if ((Math.Abs(custom_data_line.Value_2nd).Equals(Math.Abs(Custom_data[indexC - 1].Value_2nd)))
                                            && (Math.Abs(custom_data_line.Value_1st).CompareTo(Math.Abs(Custom_data[indexC - 1].Value_1st)) > 0))
                                        { throw new Exception(); }
                                        break;
                                    case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                                        if ((Math.Abs(custom_data_line.Value_2nd).Equals(Math.Abs(Custom_data[indexC - 1].Value_2nd)))
                                            && (Math.Abs(custom_data_line.Value_1st).CompareTo(Math.Abs(Custom_data[indexC - 1].Value_1st)) < 0))
                                        { throw new Exception(); }
                                        break;
                                    default:
                                        throw new Exception();
                                }
                                break;
                        }
                        #endregion
                    }
                    if ((indexC == 0)
                        || (!custom_data_line.IntegrationGroup.Equals(Custom_data[indexC - 1].IntegrationGroup))
                        || (!custom_data_line.SampleName.Equals(Custom_data[indexC - 1].SampleName))
                        || (!custom_data_line.Timepoint_in_days.Equals(Custom_data[indexC - 1].Timepoint_in_days))
                        || (!Math.Abs(custom_data_line.Value_1st).Equals(Math.Abs(Custom_data[indexC - 1].Value_1st)))
                        || (!Math.Abs(custom_data_line.Value_2nd).Equals(Math.Abs(Custom_data[indexC - 1].Value_2nd))))
                    {
                        current_running_ranks.Clear();
                        firstIndexSameRank = indexC;
                    }
                }
                else
                {
                    if ((indexC == 0)
                        || (!custom_data_line.IntegrationGroup.Equals(Custom_data[indexC - 1].IntegrationGroup))
                        || (!custom_data_line.SampleName.Equals(Custom_data[indexC - 1].SampleName))
                        || (!custom_data_line.EntryType.Equals(Custom_data[indexC - 1].EntryType))
                        || (!custom_data_line.Timepoint_in_days.Equals(Custom_data[indexC - 1].Timepoint_in_days)))
                    {
                        running_rank = 0;
                    }
                    else
                    {
                        if (!custom_data_line.Unique_fixed_dataset_identifier.Equals(Custom_data[indexC - 1].Unique_fixed_dataset_identifier))
                        { delete_all_ranks = true; }

                        #region Check, if values sorted correctly, copy paste
                        switch (Options.Value_importance_order)
                        {
                            case Value_importance_order_enum.Value_1st_2nd:
                                switch (Options.Significance_definition_value_1st)
                                {
                                    case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                                        if (Math.Abs(custom_data_line.Value_1st).CompareTo(Math.Abs(Custom_data[indexC - 1].Value_1st)) > 0)
                                        { throw new Exception(); }
                                        break;
                                    case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                                        if (Math.Abs(custom_data_line.Value_1st).CompareTo(Math.Abs(Custom_data[indexC - 1].Value_1st)) < 0)
                                        { throw new Exception(); }
                                        break;
                                    default:
                                        throw new Exception();
                                }
                                switch (Options.Significance_definition_value_2nd)
                                {
                                    case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                                        if ((Math.Abs(custom_data_line.Value_1st).Equals(Math.Abs(Custom_data[indexC - 1].Value_1st)))
                                            && (Math.Abs(custom_data_line.Value_2nd).CompareTo(Math.Abs(Custom_data[indexC - 1].Value_2nd)) > 0))
                                        { throw new Exception(); }
                                        break;
                                    case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                                        if ((Math.Abs(custom_data_line.Value_1st).Equals(Math.Abs(Custom_data[indexC - 1].Value_1st)))
                                            && (Math.Abs(custom_data_line.Value_2nd).CompareTo(Math.Abs(Custom_data[indexC - 1].Value_2nd)) < 0))
                                        { throw new Exception(); }
                                        break;
                                    default:
                                        throw new Exception();
                                }
                                break;
                            case Value_importance_order_enum.Value_2nd_1st:
                                switch (Options.Significance_definition_value_2nd)
                                {
                                    case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                                        if (Math.Abs(custom_data_line.Value_2nd).CompareTo(Math.Abs(Custom_data[indexC - 1].Value_2nd)) > 0)
                                        { throw new Exception(); }
                                        break;
                                    case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                                        if (Math.Abs(custom_data_line.Value_2nd).CompareTo(Math.Abs(Custom_data[indexC - 1].Value_2nd)) < 0)
                                        { throw new Exception(); }
                                        break;
                                    default:
                                        throw new Exception();
                                }
                                switch (Options.Significance_definition_value_1st)
                                {
                                    case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                                        if ((Math.Abs(custom_data_line.Value_2nd).Equals(Math.Abs(Custom_data[indexC - 1].Value_2nd)))
                                            && (Math.Abs(custom_data_line.Value_1st).CompareTo(Math.Abs(Custom_data[indexC - 1].Value_1st)) > 0))
                                        { throw new Exception(); }
                                        break;
                                    case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                                        if ((Math.Abs(custom_data_line.Value_2nd).Equals(Math.Abs(Custom_data[indexC - 1].Value_2nd)))
                                            && (Math.Abs(custom_data_line.Value_1st).CompareTo(Math.Abs(Custom_data[indexC - 1].Value_1st)) < 0))
                                        { throw new Exception(); }
                                        break;
                                    default:
                                        throw new Exception();
                                }
                                break;
                        }
                        #endregion
                    }
                    if ((indexC == 0)
                        || (!custom_data_line.IntegrationGroup.Equals(Custom_data[indexC - 1].IntegrationGroup))
                        || (!custom_data_line.SampleName.Equals(Custom_data[indexC - 1].SampleName))
                        || (!custom_data_line.EntryType.Equals(Custom_data[indexC - 1].EntryType))
                        || (!custom_data_line.Timepoint_in_days.Equals(Custom_data[indexC - 1].Timepoint_in_days))
                        || (!Math.Abs(custom_data_line.Value_1st).Equals(Math.Abs(Custom_data[indexC - 1].Value_1st)))
                        || (!Math.Abs(custom_data_line.Value_2nd).Equals(Math.Abs(Custom_data[indexC - 1].Value_2nd))))
                    {
                        current_running_ranks.Clear();
                        firstIndexSameRank = indexC;
                    }
                }
                running_rank++;
                current_running_ranks.Add(running_rank);
                if (Options.Merge_upDown_before_ranking)
                {
                    if ((indexC == custom_count - 1)
                        || (!custom_data_line.IntegrationGroup.Equals(Custom_data[indexC + 1].IntegrationGroup))
                        || (!custom_data_line.SampleName.Equals(Custom_data[indexC + 1].SampleName))
                        || (!custom_data_line.Timepoint_in_days.Equals(Custom_data[indexC + 1].Timepoint_in_days))
                        || (!Math.Abs(custom_data_line.Value_1st).Equals(Math.Abs(Custom_data[indexC + 1].Value_1st)))
                        || (!Math.Abs(custom_data_line.Value_2nd).Equals(Math.Abs(Custom_data[indexC + 1].Value_2nd))))
                    {
                        set_ranks = true;
                    }
                }
                else
                {
                    if ((indexC == custom_count - 1)
                        || (!custom_data_line.IntegrationGroup.Equals(Custom_data[indexC + 1].IntegrationGroup))
                        || (!custom_data_line.SampleName.Equals(Custom_data[indexC + 1].SampleName))
                        || (!custom_data_line.EntryType.Equals(Custom_data[indexC + 1].EntryType))
                        || (!custom_data_line.Timepoint_in_days.Equals(Custom_data[indexC + 1].Timepoint_in_days))
                        || (!Math.Abs(custom_data_line.Value_1st).Equals(Math.Abs(Custom_data[indexC + 1].Value_1st)))
                        || (!Math.Abs(custom_data_line.Value_2nd).Equals(Math.Abs(Custom_data[indexC + 1].Value_2nd))))
                    {
                        set_ranks = true;
                    }
                }
                if (set_ranks)
                {
                    if (current_running_ranks.Count == 1)
                    {
                        current_rank = current_running_ranks[0];
                    }
                    else
                    {
                        current_rank = Math_class.Get_average(current_running_ranks.ToArray());
                    }
                    for (int indexInner = firstIndexSameRank; indexInner <= indexC; indexInner++)
                    {
                        inner_custom_data_line = Custom_data[indexInner];
                        inner_custom_data_line.Fractional_rank = current_rank;
                    }
                }
            }
            if (delete_all_ranks)
            {
                for (int indexC = 0; indexC < custom_count; indexC++)
                {
                    custom_data_line = Custom_data[indexC];
                    custom_data_line.Fractional_rank = 999999;
                }
            }
        }

        private void Delete_not_significant_genes()
        {
            List<Custom_data_line_class> keep = new List<Custom_data_line_class>();
            foreach (Custom_data_line_class custom_data_line in this.Custom_data)
            {
                if (custom_data_line.Significance_status.Equals(Significance_status_enum.Yes))
                {
                    keep.Add(custom_data_line);
                }
            }
            this.Custom_data = keep.ToArray();
        }

        public void Update_significance_after_calculation_of_fractional_ranks_based_on_options()
        {
            Calculate_fractional_ranks_for_datasets_specified_in_options();
            Custom_data_line_class custom_data_line;
            int custom_length = this.Custom_data.Length;
            Significance_status_enum significance_status;
            for (int indexC=0; indexC<custom_length;indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                significance_status = Significance_status_enum.Yes;
                if (custom_data_line.Fractional_rank == -1) { throw new Exception(); }
                if (!Options.All_genes_significant)
                {
                    if (custom_data_line.Fractional_rank > Options.Keep_top_ranks) { significance_status = Significance_status_enum.No; }
                    switch (Options.Significance_definition_value_1st)
                    {
                        case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                            if (Math.Abs(custom_data_line.Value_1st) < Options.Value_1st_cutoff) { significance_status = Significance_status_enum.No; }
                            break;
                        case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                            if (Math.Abs(custom_data_line.Value_1st) > Options.Value_1st_cutoff) { significance_status = Significance_status_enum.No; }
                            break;
                        default:
                            throw new Exception();
                    }
                    switch (Options.Significance_definition_value_2nd)
                    {
                        case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                            if (Math.Abs(custom_data_line.Value_2nd) < Options.Value_2nd_cutoff) { significance_status = Significance_status_enum.No; }
                            break;
                        case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                            if (Math.Abs(custom_data_line.Value_2nd) > Options.Value_2nd_cutoff) { significance_status = Significance_status_enum.No; }
                            break;
                        default:
                            throw new Exception();
                    }
                }
                custom_data_line.Significance_status = significance_status;
            }
            if (Options.Delete_all_not_significant_genes) 
            { 
                Delete_not_significant_genes();
                Options.Delete_all_not_significant_genes = false;
            }
        }
        #endregion

        #region Order by
        public void Order_by_integrationGroup_sampleName_timepointInDays_entryType()
        {
            //this.Custom_data = this.Custom_data.OrderBy(l=>l.IntegrationGroup).ThenBy(l => l.SampleName).ThenBy(l => l.Timepoint_in_days).ThenBy(l => l.EntryType).ToArray();
            this.Custom_data = Custom_data_line_class.Order_by_integrationGroup_sampleName_timepointInDays_entryType(this.Custom_data);
        }
        public void Order_by_integrationGroup()
        {
            //this.Custom_data = this.Custom_data.OrderBy(l => l.IntegrationGroup).ToArray();
            this.Custom_data = Custom_data_line_class.Order_by_integrationGroup(this.Custom_data);
        }
        public void Order_by_uniqueFixedDatasetIdentifier()
        {
            //this.Custom_data = this.Custom_data.OrderBy(l => l.Unique_fixed_dataset_identifier).ToArray();
            this.Custom_data = Custom_data_line_class.Order_by_uniqueFixedDatasetIdentifier(this.Custom_data);
        }
        public void Order_by_backgroundGenes_list()
        {
            //this.Custom_data = this.Custom_data.OrderBy(l => l.BgGenes_listName).ToArray();
            this.Custom_data = Custom_data_line_class.Order_by_backgroundGenesListName(this.Custom_data);
        }
        public void Order_by_source_fileName()
        {
            //this.Custom_data = this.Custom_data.OrderBy(l => l.Source_fileName).ToArray();
            this.Custom_data = Custom_data_line_class.Order_by_sourceFileName(this.Custom_data);
        }
        public void Order_by_color()
        {
            //this.Custom_data = this.Custom_data.OrderBy(l=>l.SampleColor).ToArray();
            this.Custom_data = Custom_data_line_class.Order_by_color(this.Custom_data);
        }
        #endregion

        #region Set missing entries
        public void Set_missing_colors(Color[] selectable_colors)
        {
            //this.Custom_data = this.Custom_data.OrderBy(l => l.Unique_fixed_dataset_identifier).ToArray();
            this.Custom_data = Custom_data_line_class.Order_by_uniqueFixedDatasetIdentifier(this.Custom_data);
            Dictionary<Color, bool> usedColors_dict = new Dictionary<Color, bool>();
            Dictionary<string, Color> uniqueFixedDatasetIdentifier_color_dict = new Dictionary<string, Color>();
            int custom_data_length = this.Custom_data.Length;
            Custom_data_line_class custom_data_line;
            for (int indexC = 0; indexC < custom_data_length; indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                if (!usedColors_dict.ContainsKey(custom_data_line.SampleColor))
                {
                    usedColors_dict.Add(custom_data_line.SampleColor, true);
                }
                if (  (!uniqueFixedDatasetIdentifier_color_dict.ContainsKey(custom_data_line.Unique_fixed_dataset_identifier))
                    &&(!custom_data_line.SampleColor.Equals(Color.Empty))
                    &&(!custom_data_line.SampleColor_string.Equals(Color_conversion_class.Get_color_string(Color.Empty))))
                {
                    uniqueFixedDatasetIdentifier_color_dict.Add(custom_data_line.Unique_fixed_dataset_identifier, custom_data_line.SampleColor);
                }
            }

            int indexColor = -1;
            Color next_color = Color.Empty;
            int selectable_colors_length = selectable_colors.Length;

            for (int indexC = 0; indexC < custom_data_length; indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                if (  (custom_data_line.SampleColor.Equals(Color.Empty))
                    ||(custom_data_line.SampleColor_string.Equals(Color_conversion_class.Get_color_string(Color.Empty))))
                {
                    if (uniqueFixedDatasetIdentifier_color_dict.ContainsKey(custom_data_line.Unique_fixed_dataset_identifier))
                    {
                        next_color = uniqueFixedDatasetIdentifier_color_dict[custom_data_line.Unique_fixed_dataset_identifier];
                    }
                    else
                    {
                        do
                        {
                            indexColor++;
                            if (indexColor == selectable_colors_length) { indexColor = 0; usedColors_dict.Clear(); }
                            next_color = selectable_colors[indexColor];
                        } while (usedColors_dict.ContainsKey(next_color));
                        uniqueFixedDatasetIdentifier_color_dict.Add(custom_data_line.Unique_fixed_dataset_identifier, next_color);
                    }
                    custom_data_line.SampleColor = next_color;
                }
                else if (!custom_data_line.SampleColor.Equals( uniqueFixedDatasetIdentifier_color_dict[custom_data_line.Unique_fixed_dataset_identifier]))
                {
                    throw new Exception();
                }
            }
        }

        public void Set_missing_integrationGroups(string default_integrationGroup)
        {
            foreach (Custom_data_line_class custom_data_line in this.Custom_data)
            {
                if (String.IsNullOrEmpty(custom_data_line.IntegrationGroup))
                {
                    custom_data_line.IntegrationGroup = (string)default_integrationGroup.Clone();
                }
            }
        }

        public void Set_missing_dataset_names(string default_sampleName)
        {
            foreach (Custom_data_line_class custom_data_line in this.Custom_data)
            {
                if (String.IsNullOrEmpty(custom_data_line.SampleName))
                {
                    custom_data_line.SampleName = (string)default_sampleName.Clone();
                }
            }
        }

        public void Adjust_results_number_to_consecutive_numbers_within_each_integrationGroup()
        {
            Custom_data_line_class custom_data_line;
            int custom_data_length = this.Custom_data.Length;
            int current_result_number = -1;
            //this.Custom_data = this.Custom_data.OrderBy(l => l.IntegrationGroup).ThenBy(l => l.Results_number).ThenBy(l => l.SampleName).ThenBy(l => l.Timepoint_in_days).ThenBy(l => l.EntryType).ThenBy(l => l.Unique_fixed_dataset_identifier).ToArray();
            this.Custom_data = Custom_data_line_class.Order_by_integratinoGroup_resultsNumber_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier(this.Custom_data);
            for (int indexC = 0; indexC < custom_data_length; indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                if ((indexC == 0)
                    || (!custom_data_line.IntegrationGroup.Equals(this.Custom_data[indexC - 1].IntegrationGroup)))
                {
                    current_result_number = 0;
                }
                if ((indexC == 0)
                    || (!custom_data_line.IntegrationGroup.Equals(this.Custom_data[indexC - 1].IntegrationGroup))
                    || (!custom_data_line.Unique_fixed_dataset_identifier.Equals(this.Custom_data[indexC - 1].Unique_fixed_dataset_identifier))
                    || (!custom_data_line.SampleName.Equals(this.Custom_data[indexC - 1].SampleName))
                    || (!custom_data_line.EntryType.Equals(this.Custom_data[indexC - 1].EntryType))
                    || (!custom_data_line.Timepoint_in_days.Equals(this.Custom_data[indexC - 1].Timepoint_in_days)))
                {
                    current_result_number++;
                }
                custom_data_line.Results_number = current_result_number;
            }
        }

        public void Set_missing_results_numbers_and_adjust_to_consecutive_numbers_within_each_integrationGroup()
        {
            Custom_data_line_class custom_data_line;
            Custom_data_line_class inner_custom_data_line;
            int custom_data_length = this.Custom_data.Length;
            int highest_results_number = -1;
            int current_result_number = -1;
            //this.Custom_data = this.Custom_data.OrderBy(l => l.IntegrationGroup).ThenBy(l=>l.Results_number).ThenBy(l => l.SampleName).ThenBy(l => l.Timepoint_in_days).ThenBy(l => l.EntryType).ThenBy(l=>l.Unique_fixed_dataset_identifier).ToArray();
            this.Custom_data = Custom_data_line_class.Order_by_integratinoGroup_resultsNumber_sampleName_timepointInDays_entryType_uniqueFixedDatasetIdentifier(this.Custom_data);
            for (int indexC = 0; indexC < custom_data_length; indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                if ((indexC == 0)
                    || (!custom_data_line.IntegrationGroup.Equals(this.Custom_data[indexC - 1].IntegrationGroup)))
                {
                    highest_results_number = -1;
                    for (int indexInner = indexC; indexInner < custom_data_length; indexInner++)
                    {
                        inner_custom_data_line = this.Custom_data[indexInner];
                        if (!inner_custom_data_line.IntegrationGroup.Equals(custom_data_line.IntegrationGroup)) 
                        { 
                            break; 
                        }
                        else if (inner_custom_data_line.Results_number > highest_results_number)
                        { highest_results_number = inner_custom_data_line.Results_number; }
                    }
                }
                if ((indexC == 0)
                    || (!custom_data_line.IntegrationGroup.Equals(this.Custom_data[indexC - 1].IntegrationGroup))
                    || (!custom_data_line.Unique_fixed_dataset_identifier.Equals(this.Custom_data[indexC - 1].Unique_fixed_dataset_identifier))
                    || (!custom_data_line.SampleName.Equals(this.Custom_data[indexC - 1].SampleName))
                    || (!custom_data_line.EntryType.Equals(this.Custom_data[indexC - 1].EntryType))
                    || (!custom_data_line.Timepoint_in_days.Equals(this.Custom_data[indexC - 1].Timepoint_in_days)))
                {
                    current_result_number = custom_data_line.Results_number;
                    if (current_result_number == 0)
                    {
                        highest_results_number++;
                        current_result_number = highest_results_number;
                    }
                }
                custom_data_line.Results_number = current_result_number;
            }
            Adjust_results_number_to_consecutive_numbers_within_each_integrationGroup();
        }
        #endregion

        #region Unique dataset name
        public void Set_unique_datasetName_within_each_integrationGroup1()
        {
            int custom_length = this.Custom_data.Length;
            Custom_data_line_class custom_data_line;
            Custom_data_line_class inner_custom_data_line;
            //this.Order_by_integrationGroup();
            this.Custom_data = Custom_data_line_class.Order_by_integrationGroup(this.Custom_data);
            Dictionary<float, bool> timepointInDays_dict = new Dictionary<float, bool>();
            Dictionary<Entry_type_enum, bool> entryTypes_dict = new Dictionary<Entry_type_enum, bool>();
            int firstIndexSameIntegrationGroup = -1;
            bool add_timepoints;
            bool add_entryTypes;
            System.Text.StringBuilder uniqueName_sb = new System.Text.StringBuilder();
            for (int indexCustom=0; indexCustom<custom_length;indexCustom++)
            {
                custom_data_line = this.Custom_data[indexCustom];
                if ((indexCustom==0)||(!custom_data_line.IntegrationGroup.Equals(this.Custom_data[indexCustom-1].IntegrationGroup)))
                {
                    timepointInDays_dict.Clear();
                    entryTypes_dict.Clear();
                    firstIndexSameIntegrationGroup = indexCustom;
                }
                if (!timepointInDays_dict.ContainsKey(custom_data_line.Timepoint_in_days))
                {
                    timepointInDays_dict.Add(custom_data_line.Timepoint_in_days, true);
                }
                if (!entryTypes_dict.ContainsKey(custom_data_line.EntryType))
                {
                    entryTypes_dict.Add(custom_data_line.EntryType, true);
                }
                if ((indexCustom == custom_length - 1) || (!custom_data_line.IntegrationGroup.Equals(this.Custom_data[indexCustom + 1].IntegrationGroup)))
                {
                    add_timepoints = timepointInDays_dict.Keys.ToArray().Length > 1;
                    add_entryTypes = entryTypes_dict.Keys.ToArray().Length > 1;
                    for (int indexInner = firstIndexSameIntegrationGroup; indexInner<=indexCustom;indexInner++)
                    {
                        inner_custom_data_line = this.Custom_data[indexInner];
                        uniqueName_sb.Clear();
                        uniqueName_sb.AppendFormat(inner_custom_data_line.SampleName);
                        if (add_timepoints) { uniqueName_sb.AppendFormat(" - {0} {1}", inner_custom_data_line.Timepoint, inner_custom_data_line.Timeunit); }
                        if (add_entryTypes) { uniqueName_sb.AppendFormat(" - {0}", inner_custom_data_line.EntryType); }
                        inner_custom_data_line.Unique_dataset_name = uniqueName_sb.ToString();
                    }
                }
            }
        }

        public void Set_unique_datasetName_within_whole_custom_data_ignoring_integrationGroups()
        {
            Dataset_setUniqueDatasetName_class dataset = new Dataset_setUniqueDatasetName_class();
            ISet_uniqueDatasetName_line[] lines = dataset.Set_unique_datasetName_within_each_integrationGroup1(this.Custom_data);
            int custom_length = this.Custom_data.Length;
            for (int indexC=0; indexC<custom_length;indexC++)
            {
                this.Custom_data[indexC] = (Custom_data_line_class)lines[indexC];
            }
        }

        public void Reset_unique_datasetName()
        {
            foreach (Custom_data_line_class custom_data_line in this.Custom_data)
            {
                custom_data_line.Unique_dataset_name = "";
            }
        }
        #endregion

        private void Set_all_genes_to_upper_case()
        {
            int custom_length = this.Custom_data.Length;
            Custom_data_line_class custom_data_line;
            for (int indexC=0; indexC<custom_length;indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                custom_data_line.NCBI_official_symbol = custom_data_line.NCBI_official_symbol.ToUpper();
            }
        }

        private void Check_for_duplicated_de_characterization_values()
        {
            int custom_data_length = Custom_data.Length;
            Custom_data_line_class custom_data_line;
            //this.Custom_data = this.Custom_data.OrderBy(l=>l.IntegrationGroup).ThenBy(l => l.SampleName).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint_in_days).ThenBy(l => l.NCBI_official_symbol).ToArray();
            Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, bool>>>>> integrationGroup_sampleName_entryType_timepointInDays_symbol_dict = new Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, bool>>>>>();

            float timepointInDays;
            for (int indexC = 0; indexC < custom_data_length; indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                timepointInDays = custom_data_line.Timepoint_in_days;
                if (!integrationGroup_sampleName_entryType_timepointInDays_symbol_dict.ContainsKey(custom_data_line.IntegrationGroup))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_symbol_dict.Add(custom_data_line.IntegrationGroup, new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, bool>>>>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_symbol_dict[custom_data_line.IntegrationGroup].ContainsKey(custom_data_line.SampleName))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_symbol_dict[custom_data_line.IntegrationGroup].Add(custom_data_line.SampleName, new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, bool>>>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_symbol_dict[custom_data_line.IntegrationGroup][custom_data_line.SampleName].ContainsKey(custom_data_line.EntryType))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_symbol_dict[custom_data_line.IntegrationGroup][custom_data_line.SampleName].Add(custom_data_line.EntryType, new Dictionary<float, Dictionary<string, bool>>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_symbol_dict[custom_data_line.IntegrationGroup][custom_data_line.SampleName][custom_data_line.EntryType].ContainsKey(timepointInDays))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_symbol_dict[custom_data_line.IntegrationGroup][custom_data_line.SampleName][custom_data_line.EntryType].Add(timepointInDays, new Dictionary<string, bool>());
                }
                integrationGroup_sampleName_entryType_timepointInDays_symbol_dict[custom_data_line.IntegrationGroup][custom_data_line.SampleName][custom_data_line.EntryType][timepointInDays].Add(custom_data_line.NCBI_official_symbol, true);
                //previous_custom_data_line = this.Custom_data[indexC - 1];
                //if (   (custom_data_line.IntegrationGroup.Equals(previous_custom_data_line.IntegrationGroup))
                //    && (custom_data_line.SampleName.Equals(previous_custom_data_line.SampleName))
                //    && (custom_data_line.EntryType.Equals(previous_custom_data_line.EntryType))
                //    && (custom_data_line.Timepoint_in_days.Equals(previous_custom_data_line.Timepoint_in_days))
                //    && (custom_data_line.NCBI_official_symbol.Equals(previous_custom_data_line.NCBI_official_symbol)))
                //{
                //    throw new Exception();
                //}
            }
        }

        public void Delete_indicated_datasets(Dictionary<string,bool> uniqueFixedDatasetIdentifier_delete_dict)
        {
            List<Custom_data_line_class> keep = new List<Custom_data_line_class>();
            int custom_data_length = this.Custom_data.Length;
            Custom_data_line_class custom_line;
            for (int indexC = 0; indexC < custom_data_length; indexC++)
            {
                custom_line = this.Custom_data[indexC];
                if (!uniqueFixedDatasetIdentifier_delete_dict.ContainsKey(custom_line.Unique_fixed_dataset_identifier))
                {
                    keep.Add(custom_line);
                }
            }
            this.Custom_data = keep.ToArray();
        }

        public Read_error_message_line_class[] Add_to_array_and_return_error_message_if_not_possible(Custom_data_line_class[] add_custom_data)
        {
            int this_length = this.Custom_data.Length;
            int add_length = add_custom_data.Length;
            int new_length = this_length + add_length;
            List<Read_error_message_line_class> error_messages = new List<Read_error_message_line_class>();
            try
            {
                Custom_data_line_class[] new_custom_data = new Custom_data_line_class[new_length];
                int indexNew = -1;
                for (int indexThis = 0; indexThis < this_length; indexThis++)
                {
                    indexNew++;
                    new_custom_data[indexNew] = this.Custom_data[indexThis];
                }
                for (int indexAdd = 0; indexAdd < add_length; indexAdd++)
                {
                    indexNew++;
                    new_custom_data[indexNew] = add_custom_data[indexAdd];
                }
                this.Custom_data = new_custom_data;
            }
            catch
            {
                Read_error_message_line_class new_error_message_line = new Read_error_message_line_class();
                new_error_message_line.Error_message = Read_error_message_enum.Custom_data_array_too_long;
                error_messages.Add(new_error_message_line);
            }
            return error_messages.ToArray();
        }

        #region Background genes
        public bool Add_new_experimental_background_genes_and_return_success(string new_bgGenesListName, string[] new_bgGenes)
        {
            int bg_genes_length = new_bgGenes.Length;
            for (int indexBg=0; indexBg<bg_genes_length; indexBg++)
            {
                new_bgGenes[indexBg] = new_bgGenes[indexBg].ToUpper();
            }
            bool successful = true;
            if (!this.ExpBgGenesList_bgGenes_dict.ContainsKey(new_bgGenesListName))
            {
                new_bgGenes = new_bgGenes.Distinct().OrderBy(l =>l).ToArray();
                this.ExpBgGenesList_bgGenes_dict.Add(new_bgGenesListName, new_bgGenes);
                successful = true;
            }
            else
            {
                successful = false;
            }
            return successful;
        }
        private void Reset_all_bgGeneLists_that_are_missing_in_dictionary_to_mbco_background_gene_list()
        {
            foreach (Custom_data_line_class custom_data_line in this.Custom_data)
            {
                if (!ExpBgGenesList_bgGenes_dict.ContainsKey(custom_data_line.BgGenes_listName))
                {
                    custom_data_line.BgGenes_listName = (string)Global_class.Mbco_exp_background_gene_list_name.Clone();
                }
            }
        }

        public void Remove_indicated_bgGenes_list(string remove_bgGenesListName)
        {
            if (!remove_bgGenesListName.Equals(Global_class.Mbco_exp_background_gene_list_name))
            {
                this.ExpBgGenesList_bgGenes_dict.Remove(remove_bgGenesListName);
            }
            Reset_all_bgGeneLists_that_are_missing_in_dictionary_to_mbco_background_gene_list();
        }
        public void Clear_and_reset_expBgGenesList_to_default()
        {
            this.ExpBgGenesList_bgGenes_dict.Clear();
            this.ExpBgGenesList_bgGenes_dict.Add((string)Global_class.Mbco_exp_background_gene_list_name.Clone(), new string[0]);
            Reset_all_bgGeneLists_that_are_missing_in_dictionary_to_mbco_background_gene_list();
        }
        public void Reset_bgGeneAssignments_to_default()
        {
            foreach (Custom_data_line_class custom_line in Custom_data)
            {
                custom_line.BgGenes_listName = (string)Global_class.Mbco_exp_background_gene_list_name.Clone();
            }
        }
        public Read_error_message_line_class[] Read_and_add_background_genes_and_return_error_messages(string complete_fileName, ProgressReport_interface_class progressReport)
        {
            Read_error_message_line_class new_error_message;
            List<Read_error_message_line_class> error_messages = new List<Read_error_message_line_class>();
            string directory = System.IO.Path.GetDirectoryName(complete_fileName);
            if (!System.IO.Directory.Exists(directory))
            {
                new_error_message = new Read_error_message_line_class();
                new_error_message.File_type = Read_file_type.Data;
                new_error_message.Complete_fileName = (string)directory.Clone();
                new_error_message.Error_message = Read_error_message_enum.Directory_does_not_exist;
                error_messages.Add(new_error_message);
            }
            else if (!System.IO.File.Exists(complete_fileName))
            {
                new_error_message = new Read_error_message_line_class();
                new_error_message.File_type = Read_file_type.Data;
                new_error_message.Complete_fileName = (string)complete_fileName.Clone();
                new_error_message.Error_message = Read_error_message_enum.File_does_not_exist;
                error_messages.Add(new_error_message);
            }
            else
            {
                string[] bgGenes = ReadWriteClass.Read_string_array_and_remove_non_letters_from_beginning_and_end_of_each_line(complete_fileName, progressReport);
                bgGenes = bgGenes.Distinct().OrderBy(l => l).ToArray();
                if (bgGenes.Length > 0)
                {
                    string bgGenesListName = System.IO.Path.GetFileNameWithoutExtension(complete_fileName);
                    if (!this.ExpBgGenesList_bgGenes_dict.ContainsKey(bgGenesListName))
                    {
                        this.ExpBgGenesList_bgGenes_dict.Add(bgGenesListName, bgGenes);
                        Read_error_message_line_class new_success_message = new Read_error_message_line_class();
                        new_success_message.File_type = Read_file_type.Background_genes;
                        new_success_message.Complete_fileName = (string)complete_fileName.Clone();
                        new_success_message.Error_message = Read_error_message_enum.BgGenes_file_read;
                        error_messages.Add(new_success_message);
                    }
                    else
                    {
                        new_error_message = new Read_error_message_line_class();
                        new_error_message.File_type = Read_file_type.Background_genes;
                        new_error_message.Complete_fileName = (string)complete_fileName.Clone();
                        new_error_message.Error_message = Read_error_message_enum.Duplicated_bggenes_dataset;
                        error_messages.Add(new_error_message);
                    }
                }
            }
            return error_messages.ToArray();
        }
        public void Automatically_override_bgGeneListNames_if_matching_names()
        {
            //this.Custom_data = this.Custom_data.OrderBy(l => l.Source_fileName).ToArray();
            this.Custom_data = Custom_data_line_class.Order_by_sourceFileName(this.Custom_data);
            int custom_data_length = this.Custom_data.Length;
            Custom_data_line_class custom_data_line;
            string suggested_bgGenesList = "error";
            bool add_suggested_bgGenesList = false;
            for (int indexC=0; indexC<custom_data_length;indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                if (  (indexC==0)
                    ||(!custom_data_line.Source_fileName.Equals(this.Custom_data[indexC-1].Source_fileName)))
                {
                    add_suggested_bgGenesList = false;
                    suggested_bgGenesList = System.IO.Path.GetFileNameWithoutExtension(custom_data_line.Source_fileName) + Global_class.Bg_genes_label;
                    if (ExpBgGenesList_bgGenes_dict.ContainsKey(suggested_bgGenesList))
                    {
                        add_suggested_bgGenesList = true;
                    }
                }
                if (add_suggested_bgGenesList)
                {
                    custom_data_line.BgGenes_listName = (string)suggested_bgGenesList.Clone();
                }
            }
        }
        #endregion

        #region Get other
        public Custom_data_class Get_custom_data_class_with_indicated_background_genes_list(string bgGeneListName)
        {
            int custom_length = this.Custom_data.Length;
            Custom_data_line_class custom_data_line;
            List<Custom_data_line_class> custom_data_with_background_genes = new List<Custom_data_line_class>();
            for (int indexC=0; indexC<custom_length;indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                if (custom_data_line.BgGenes_listName.Equals(bgGeneListName))
                {
                    custom_data_with_background_genes.Add(custom_data_line);
                }
            }
            Custom_data_class custom_data = new Custom_data_class();
            Read_error_message_line_class[] error_messages = custom_data.Add_to_array_and_return_error_message_if_not_possible(custom_data_with_background_genes.ToArray());
            if (error_messages.Length>0) { throw new Exception(); }
            custom_data.Add_new_experimental_background_genes_and_return_success(bgGeneListName, this.ExpBgGenesList_bgGenes_dict[bgGeneListName]);
            return custom_data;
        }
        #endregion

        #region Keep and remove
        public void Keep_only_significant_lines()
        {
            List<Custom_data_line_class> keep = new List<Custom_data_line_class>();
            foreach (Custom_data_line_class custom_line in this.Custom_data)
            {
                switch (custom_line.Significance_status)
                {
                    case Significance_status_enum.Yes:
                        keep.Add(custom_line);
                        break;
                    case Significance_status_enum.No:
                    case Significance_status_enum.Undetermined:
                        break;
                    default:
                        throw new Exception();
                }
            }
            this.Custom_data = keep.ToArray();
        }
        public Read_error_message_line_class[] Remove_all_lines_with_zero_1stvalues_and_error_if_no_lines_left(string complete_fileName, int max_error_messages_left)
        {
            Read_error_message_line_class[] error_messages = new Read_error_message_line_class[0];
            List<Custom_data_line_class> keep = new List<Custom_data_line_class>();
            foreach (Custom_data_line_class custom_line in this.Custom_data)
            {
                if (custom_line.Value_1st!=0)
                {
                    keep.Add(custom_line);
                }
            }
            this.Custom_data = keep.ToArray();
            if ((Custom_data.Length==0)&&(max_error_messages_left>0))
            {
                Read_error_message_line_class new_error_message_line = new Read_error_message_line_class();
                new_error_message_line.File_type = Read_file_type.Data;
                new_error_message_line.Error_message = Read_error_message_enum.All_values_in_column_specified_for_1st_value_are_zero;
                new_error_message_line.Complete_fileName = (string)complete_fileName.Clone();
                error_messages = new Read_error_message_line_class[] { new_error_message_line };
            }
            return error_messages;
        }
        #endregion

        public string[] Get_all_unique_ordered_sampleNames()
        {
            Dictionary<string, bool> sampleName_dict = new Dictionary<string, bool>();
            foreach (Custom_data_line_class custom_data_line in this.Custom_data)
            {
                if (!sampleName_dict.ContainsKey(custom_data_line.SampleName))
                {
                    sampleName_dict.Add(custom_data_line.SampleName, true);
                }
            }
            return sampleName_dict.Keys.OrderBy(l => l).ToArray();
        }

        public string[] Get_all_unique_ordered_timeunit_strings()
        {
            Dictionary<string, bool> timeunit_string_dict = new Dictionary<string, bool>();
            foreach (Custom_data_line_class custom_data_line in this.Custom_data)
            {
                if (!timeunit_string_dict.ContainsKey(custom_data_line.Timeunit_string))
                {
                    timeunit_string_dict.Add(custom_data_line.Timeunit_string, true);
                }
            }
            return timeunit_string_dict.Keys.OrderBy(l => l).ToArray();
        }

        public string[] Get_all_unique_ordered_integrationGroups()
        {
            Dictionary<string, bool> integrationGroup_dict = new Dictionary<string, bool>();
            foreach (Custom_data_line_class custom_data_line in this.Custom_data)
            {
                if (!integrationGroup_dict.ContainsKey(custom_data_line.IntegrationGroup))
                {
                    integrationGroup_dict.Add(custom_data_line.IntegrationGroup, true);
                }
            }
            return integrationGroup_dict.Keys.OrderBy(l => l).ToArray();
        }

        public Entry_type_enum[] Get_all_unique_ordered_entryTypes()
        {
            Dictionary<Entry_type_enum, bool> entryType_dict = new Dictionary<Entry_type_enum, bool>();
            foreach (Custom_data_line_class custom_data_line in this.Custom_data)
            {
                if (!entryType_dict.ContainsKey(custom_data_line.EntryType))
                {
                    entryType_dict.Add(custom_data_line.EntryType, true);
                }
            }
            return entryType_dict.Keys.OrderBy(l => l).ToArray();
        }

        public float[] Get_all_unique_ordered_timepointsInDays()
        {
            Dictionary<float, bool> timepointInDays_dict = new Dictionary<float, bool>();
            foreach (Custom_data_line_class custom_data_line in this.Custom_data)
            {
                if (!timepointInDays_dict.ContainsKey(custom_data_line.Timepoint_in_days))
                {
                    timepointInDays_dict.Add(custom_data_line.Timepoint_in_days, true);
                }
            }
            return timepointInDays_dict.Keys.OrderBy(l => l).ToArray();
        }

        public string[] Get_all_unique_ordered_fixed_datasetIdentifies()
        {
            Dictionary<string, bool> uniqueDatasetIdentifier_dict = new Dictionary<string, bool>();
            foreach (Custom_data_line_class custom_data_line in this.Custom_data)
            {
                if (!uniqueDatasetIdentifier_dict.ContainsKey(custom_data_line.Unique_fixed_dataset_identifier))
                {
                    uniqueDatasetIdentifier_dict.Add(custom_data_line.Unique_fixed_dataset_identifier, true);
                }
            }
            return uniqueDatasetIdentifier_dict.Keys.OrderBy(l => l).ToArray();
        }

        public string[] Get_all_unique_ordered_bgGeneListNames()
        {
            Dictionary<string, bool> bgGeneListNames_dict = new Dictionary<string, bool>();
            foreach (Custom_data_line_class custom_data_line in this.Custom_data)
            {
                if (!bgGeneListNames_dict.ContainsKey(custom_data_line.BgGenes_listName))
                {
                    bgGeneListNames_dict.Add(custom_data_line.BgGenes_listName, true);
                }
            }
            return bgGeneListNames_dict.Keys.OrderBy(l => l).ToArray();
        }

        public Dictionary<string, bool> Get_sourceFileNames_dict()
        {
            Dictionary<string, bool> source_fileName_dict = new Dictionary<string, bool>();
            foreach (Custom_data_line_class custom_data_line in this.Custom_data)
            {
                if (!source_fileName_dict.ContainsKey(custom_data_line.Source_fileName))
                {
                    source_fileName_dict.Add(custom_data_line.Source_fileName, true);
                }
            }
            return source_fileName_dict;
        }

        public string Get_available_uniqueFixedDatasetIdentifier_for_manual_addition()
        {
            int custom_length = this.Custom_data.Length;
            int used_number;
            List<int> used_numbers_list = new List<int>();
            Custom_data_line_class custom_data_line;
            for (int indexC=0;indexC<custom_length;indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                if (  (indexC==0)
                    ||(!custom_data_line.Unique_fixed_dataset_identifier.Equals(this.Custom_data[indexC-1].Unique_fixed_dataset_identifier)))
                {
                    if (custom_data_line.Unique_fixed_dataset_identifier.IndexOf(Manual_added_unique_fixed_dataset_identifier_baseName) != -1)
                    {
                        used_number = int.Parse(custom_data_line.Unique_fixed_dataset_identifier.Replace(Manual_added_unique_fixed_dataset_identifier_baseName, ""));
                        used_numbers_list.Add(used_number);
                    }
                }
            }
            used_numbers_list = used_numbers_list.OrderBy(l => l).ToList();
            int used_numbers_length = used_numbers_list.Count();
            int indexUsed = 0;
            int test_numbers_length = used_numbers_length + 1;
            int number_compare = -2;
            int final_number = -1;
            for (int suggesetedNo=0; suggesetedNo < test_numbers_length; suggesetedNo++)
            {
                number_compare = -2;
                while ((indexUsed<used_numbers_length)&&(number_compare<0))
                {
                    number_compare = used_numbers_list[indexUsed] - suggesetedNo;
                    if (number_compare < 0) { indexUsed++; }
                }
                if (number_compare !=0)
                {
                    final_number = suggesetedNo;
                    break;
                }
            }
            return Manual_added_unique_fixed_dataset_identifier_baseName + final_number;
        }
        public Dataset_attributes_enum[] Get_all_attributes_with_different_entries()
        {
            List<Dataset_attributes_enum> attributes_with_different_entries = new List<Dataset_attributes_enum>();
            Dictionary<string, bool> names_dict = new Dictionary<string, bool>();
            Dictionary<Color, bool> color_dict = new Dictionary<Color, bool>();
            Dictionary<float, bool> timepointInDays_dict = new Dictionary<float, bool>();
            Dictionary<Entry_type_enum, bool> entryType_dict = new Dictionary<Entry_type_enum, bool>();
            Dictionary<string, bool> integrationGroup_dict = new Dictionary<string, bool>();
            int custom_data_length = this.Custom_data.Length;
            Custom_data_line_class custom_data_line;
            for (int indexC = 0; indexC < custom_data_length; indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                if (!names_dict.ContainsKey(custom_data_line.SampleName)) { names_dict.Add(custom_data_line.SampleName, true); }
                if (!timepointInDays_dict.ContainsKey(custom_data_line.Timepoint_in_days)) { timepointInDays_dict.Add(custom_data_line.Timepoint_in_days, true); }
                if (!entryType_dict.ContainsKey(custom_data_line.EntryType)) { entryType_dict.Add(custom_data_line.EntryType, true); }
                if (!color_dict.ContainsKey(custom_data_line.SampleColor)) { color_dict.Add(custom_data_line.SampleColor, true); }
                if (!integrationGroup_dict.ContainsKey(custom_data_line.IntegrationGroup)) { integrationGroup_dict.Add(custom_data_line.IntegrationGroup, true); }
            }
            //if (names_dict.Keys.ToArray().Length > 1) 
            attributes_with_different_entries.Add(Dataset_attributes_enum.Name);
            if (timepointInDays_dict.Keys.ToArray().Length > 1) { attributes_with_different_entries.Add(Dataset_attributes_enum.Timepoint); }
            if (entryType_dict.Keys.ToArray().Length > 1) { attributes_with_different_entries.Add(Dataset_attributes_enum.EntryType); }
            if (color_dict.Keys.ToArray().Length > 1) { attributes_with_different_entries.Add(Dataset_attributes_enum.Color); }
            if (integrationGroup_dict.Keys.ToArray().Length > 1) { attributes_with_different_entries.Add(Dataset_attributes_enum.IntegrationGroup); }
            return attributes_with_different_entries.ToArray();
        }

        public void Clear_custom_data()
        {
            this.Custom_data = new Custom_data_line_class[0];
            Clear_and_reset_expBgGenesList_to_default();
        }

        public Read_error_message_line_class[] Generate_custom_data_instance_if_no_errors_and_return_error_messages(bool column_for_first_value_specified, Custom_data_readWriteOptions_class readWriteOptions, ProgressReport_interface_class progressReport)
        {
            Read_error_message_line_class[] read_read_error_messages =  Read_if_no_error_and_return_error_messages(column_for_first_value_specified, readWriteOptions, progressReport);
            List<Read_error_message_line_class> final_read_error_messages = new List<Read_error_message_line_class>();
            final_read_error_messages.AddRange(read_read_error_messages);
            return final_read_error_messages.ToArray();
        }

        public Data_class Generate_new_data_instance()
        {
            Check_for_duplicated_de_characterization_values();
            Data_class data = new Data_class();
            data.Add_to_data_instance(this.Custom_data);
            return data;
        }

        public Dictionary<string, Dictionary<float, Dictionary<Timeunit_enum, Dictionary<Entry_type_enum, int>>>> Generate_sampleName_timepoint_timeunit_entryType_geneCount_dict()
        {
            int custom_data_length = this.Custom_data.Length;
            Custom_data_line_class custom_data_line;
            Dictionary<string, Dictionary<float, Dictionary<Timeunit_enum, Dictionary<Entry_type_enum, int>>>> sampleName_timepoint_entryType_geneCount_dict = new Dictionary<string, Dictionary<float, Dictionary<Timeunit_enum, Dictionary<Entry_type_enum, int>>>>();
            for (int indexC = 0; indexC < custom_data_length; indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                if (!sampleName_timepoint_entryType_geneCount_dict.ContainsKey(custom_data_line.SampleName))
                {
                    sampleName_timepoint_entryType_geneCount_dict.Add(custom_data_line.SampleName, new Dictionary<float, Dictionary<Timeunit_enum, Dictionary<Entry_type_enum, int>>>());
                }
                if (!sampleName_timepoint_entryType_geneCount_dict[custom_data_line.SampleName].ContainsKey(custom_data_line.Timepoint))
                {
                    sampleName_timepoint_entryType_geneCount_dict[custom_data_line.SampleName].Add(custom_data_line.Timepoint, new Dictionary<Timeunit_enum, Dictionary<Entry_type_enum, int>>());
                }
                if (!sampleName_timepoint_entryType_geneCount_dict[custom_data_line.SampleName][custom_data_line.Timepoint].ContainsKey(custom_data_line.Timeunit))
                {
                    sampleName_timepoint_entryType_geneCount_dict[custom_data_line.SampleName][custom_data_line.Timepoint].Add(custom_data_line.Timeunit, new Dictionary<Entry_type_enum, int>());
                }
                if (!sampleName_timepoint_entryType_geneCount_dict[custom_data_line.SampleName][custom_data_line.Timepoint][custom_data_line.Timeunit].ContainsKey(custom_data_line.EntryType))
                {
                    sampleName_timepoint_entryType_geneCount_dict[custom_data_line.SampleName][custom_data_line.Timepoint][custom_data_line.Timeunit].Add(custom_data_line.EntryType, 0);
                }
                sampleName_timepoint_entryType_geneCount_dict[custom_data_line.SampleName][custom_data_line.Timepoint][custom_data_line.Timeunit][custom_data_line.EntryType]++;
            }
            return sampleName_timepoint_entryType_geneCount_dict;
        }

        public Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, int>>> Generate_sampleName_timepointInDays_entryType_geneCount_dict()
        {
            int custom_data_length = this.Custom_data.Length;
            Custom_data_line_class custom_data_line;
            Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, int>>> sampleName_timepointInDays_entryType_geneCount_dict = new Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, int>>>();
            float timepoint_in_days;
            for (int indexC = 0; indexC < custom_data_length; indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                timepoint_in_days = custom_data_line.Timepoint_in_days;
                if (!sampleName_timepointInDays_entryType_geneCount_dict.ContainsKey(custom_data_line.SampleName))
                {
                    sampleName_timepointInDays_entryType_geneCount_dict.Add(custom_data_line.SampleName, new Dictionary<float, Dictionary<Entry_type_enum, int>>());
                }
                if (!sampleName_timepointInDays_entryType_geneCount_dict[custom_data_line.SampleName].ContainsKey(timepoint_in_days))
                {
                    sampleName_timepointInDays_entryType_geneCount_dict[custom_data_line.SampleName].Add(timepoint_in_days, new Dictionary<Entry_type_enum, int>());
                }
                if (!sampleName_timepointInDays_entryType_geneCount_dict[custom_data_line.SampleName][timepoint_in_days].ContainsKey(custom_data_line.EntryType))
                {
                    sampleName_timepointInDays_entryType_geneCount_dict[custom_data_line.SampleName][timepoint_in_days].Add(custom_data_line.EntryType, 0);
                }
                sampleName_timepointInDays_entryType_geneCount_dict[custom_data_line.SampleName][timepoint_in_days][custom_data_line.EntryType]++;
            }
            return sampleName_timepointInDays_entryType_geneCount_dict;
        }

        public Read_error_message_line_class[] Add_other_and_return_error_messages_if_not_possible(Custom_data_class other_data)
        {
            Read_error_message_line_class[] error_messages = this.Add_to_array_and_return_error_message_if_not_possible(other_data.Custom_data);
            if (error_messages.Length == 1)
            {
                string[] other_bgGenes_names = other_data.ExpBgGenesList_bgGenes_dict.Keys.ToArray();
                string other_bgGenes_name;
                int other_bgGenes_names_length = other_bgGenes_names.Length;
                for (int indexO = 0; indexO < other_bgGenes_names_length; indexO++)
                {
                    other_bgGenes_name = other_bgGenes_names[indexO];
                    if (!this.ExpBgGenesList_bgGenes_dict.ContainsKey(other_bgGenes_name))
                    {
                        this.ExpBgGenesList_bgGenes_dict.Add((string)other_bgGenes_name.Clone(), Array_class.Deep_copy_string_array(other_data.ExpBgGenesList_bgGenes_dict[other_bgGenes_name]));
                    }
                }
                Set_missing_results_numbers_and_adjust_to_consecutive_numbers_within_each_integrationGroup();
            }
            return error_messages;
        }

        public void Set_unique_fixed_dataset_identifier_after_reading(int uploadedFileNo)
        {
            int custom_length = this.Custom_data.Length;
            //this.Custom_data = this.Custom_data.OrderBy(l => l.Source_fileName).ThenBy(l => l.SampleName).ThenBy(l => l.Timepoint_in_days).ThenBy(l => l.EntryType).ThenBy(l=>l.IntegrationGroup).ToArray();
            this.Custom_data = Custom_data_line_class.Order_by_sourceFileName_sampleName_timepointInDays_entryType_integrationGroup(this.Custom_data);
            Custom_data_line_class custom_data_line;
            int unique_data_identifier_no = -1;
            for (int indexC = 0; indexC < custom_length; indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                if (!String.IsNullOrEmpty(custom_data_line.Unique_fixed_dataset_identifier)) { throw new Exception(); }
                if ((indexC == 0)
                    || (!custom_data_line.Source_fileName.Equals(this.Custom_data[indexC-1].Source_fileName)))
                {
                    unique_data_identifier_no = -1;
                }
                if ((indexC == 0)
                    || (!custom_data_line.Source_fileName.Equals(this.Custom_data[indexC-1].Source_fileName))
                    || (!custom_data_line.SampleName.Equals(this.Custom_data[indexC-1].SampleName))
                    || (!custom_data_line.Timepoint_in_days.Equals(this.Custom_data[indexC-1].Timepoint_in_days))
                    || (!custom_data_line.EntryType.Equals(this.Custom_data[indexC - 1].EntryType))
                    || (!custom_data_line.IntegrationGroup.Equals(this.Custom_data[indexC - 1].IntegrationGroup)))
                {
                    unique_data_identifier_no++;
                }
                custom_data_line.Unique_fixed_dataset_identifier = System.IO.Path.GetFileNameWithoutExtension(custom_data_line.Source_fileName) + " fileNo" + uploadedFileNo + " inFileNo" + unique_data_identifier_no;
            }
        }

        private Custom_data_line_class[] Set_all_1st_values_to_one_after_checking_if_zero(Custom_data_line_class[] read_lines)
        {
            int custom_length = read_lines.Length;
            Custom_data_line_class custom_data_line;
            for (int indexRead = 0; indexRead < custom_length; indexRead++)
            {
                custom_data_line = read_lines[indexRead];
                if (custom_data_line.Value_1st!=0) { throw new Exception(); }
                custom_data_line.Value_1st = 1;
            }
            return read_lines;
        }

        public void Set_entryType_based_on_value_after_checking_that_not_specified()
        {
            int custom_length = this.Custom_data.Length;
            Custom_data_line_class custom_data_line;
            for (int indexRead = 0; indexRead < custom_length; indexRead++)
            {
                custom_data_line = Custom_data[indexRead];
                if (!custom_data_line.EntryType.Equals(Entry_type_enum.E_m_p_t_y)) { throw new Exception(); }
                if (custom_data_line.Value_1st > 0)
                {
                    custom_data_line.EntryType = Entry_type_enum.Up;
                }
                else if (custom_data_line.Value_1st < 0)
                {
                    custom_data_line.EntryType = Entry_type_enum.Down;
                }
                else { throw new Exception(); }
            }
        }

        private Read_error_message_line_class[] Test_if_all_colors_are_valid(int left_over_error_messages, string complete_fileName)
        {
            Read_error_message_line_class new_error_line;
            List<Read_error_message_line_class> new_error_lines = new List<Read_error_message_line_class>();
            int read_lines_length = this.Custom_data.Length;
            Custom_data_line_class custom_data_line;
            Dictionary<string, bool> considered_wrong_color_dict = new Dictionary<string, bool>();
            for (int indexRead=0; indexRead< read_lines_length;indexRead++)
            {
                custom_data_line = Custom_data[indexRead];
                if (  (   (!custom_data_line.SampleColor.IsKnownColor)
                       && (!custom_data_line.SampleColor.IsEmpty))
                    || (custom_data_line.SampleColor.Equals(Color.White))
                    || (custom_data_line.SampleColor.Equals(Color.Transparent)))
                {
                    if (  (!considered_wrong_color_dict.ContainsKey(custom_data_line.SampleColor.ToString()))
                        &&(left_over_error_messages > 0))
                    {
                        left_over_error_messages--;
                        considered_wrong_color_dict.Add(custom_data_line.SampleColor.ToString(), true);
                        new_error_line = new Read_error_message_line_class();
                        new_error_line.Complete_fileName = (string)complete_fileName.Clone();
                        new_error_line.ColumnName = "";
                        new_error_line.LineIndex = indexRead + 1;
                        new_error_line.Value = custom_data_line.SampleColor_string;
                        new_error_line.Error_message = Read_error_message_enum.Not_an_accepted_color;
                        new_error_lines.Add(new_error_line);
                    }
                }
            }
            return new_error_lines.ToArray();
        }

        private Read_error_message_line_class[] Read_if_no_error_and_return_error_messages(bool column_for_first_value_specified, Custom_data_readWriteOptions_class readWriteOptions, ProgressReport_interface_class progressReport)
        {
            List<Read_error_message_line_class> all_error_messages = new List<Read_error_message_line_class>();
            Read_error_message_line_class[] read_error_messages;
            Custom_data_line_class[] read_custom_data = ReadWriteClass.Read_data_fill_array_and_return_error_messages<Custom_data_line_class>(out read_error_messages, readWriteOptions, progressReport);
            all_error_messages.AddRange(read_error_messages);
            string fileName = System.IO.Path.GetFileName(readWriteOptions.File);
            foreach (Custom_data_line_class read_custom_line in read_custom_data)
            {
                read_custom_line.Source_fileName = (string)fileName.Clone();
                read_custom_line.BgGenes_listName = (string)Global_class.Mbco_exp_background_gene_list_name.Clone();
            }
            if (!this.ExpBgGenesList_bgGenes_dict.ContainsKey(Global_class.Mbco_exp_background_gene_list_name))
            {
                this.ExpBgGenesList_bgGenes_dict.Add(Global_class.Mbco_exp_background_gene_list_name, new string[0]);
            }
            if (!column_for_first_value_specified)
            {
                read_custom_data = Set_all_1st_values_to_one_after_checking_if_zero(read_custom_data);
            }
            this.Custom_data = read_custom_data;
            if (this.Custom_data.Length > 0)
            {
                Read_error_message_line_class[] error_messages_no_lines_left = Remove_all_lines_with_zero_1stvalues_and_error_if_no_lines_left(readWriteOptions.File, readWriteOptions.Max_error_messages - all_error_messages.Count);
                all_error_messages.AddRange(error_messages_no_lines_left);
                Convert_hexadecimal_colors_to_csharp_colors();
                Read_error_message_line_class[] error_messages_not_valid_colors = Test_if_all_colors_are_valid(readWriteOptions.Max_error_messages - all_error_messages.Count, readWriteOptions.File);
                all_error_messages.AddRange(error_messages_not_valid_colors);
                Set_entryType_based_on_value_after_checking_that_not_specified();
                Set_all_genes_to_upper_case();
            }
            if (all_error_messages.Count > 0) { this.Custom_data = new Custom_data_line_class[0]; }
            return all_error_messages.ToArray();
        }

        public Read_error_message_line_class[] Analyze_if_any_duplicated_lines_based_on_uniqueFixedDatasetIdentifier_and_geneSymbol(string completeFileName, int max_error_messages_left)
        {
            Set_unique_datasetName_within_whole_custom_data_ignoring_integrationGroups();
            bool consider_integration_groups = this.Get_all_unique_ordered_integrationGroups().Length > 1;
            int custom_data_length = Custom_data.Length;
            Custom_data_line_class custom_data_line;
            //this.Custom_data = this.Custom_data.OrderBy(l => l.Unique_fixed_dataset_identifier).ThenBy(l => l.NCBI_official_symbol).ToArray();
            this.Custom_data = Custom_data_line_class.Order_by_uniqueFixedDatasetIdentifier_ncbiOfficialSymbol(this.Custom_data);
            List<Read_error_message_line_class> error_messages = new List<Read_error_message_line_class>();
            Read_error_message_line_class new_error_message_line;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int sameGeneCount = 0;
            for (int indexC = 0; indexC < custom_data_length; indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                if ((indexC == 0)
                    || (!custom_data_line.Unique_fixed_dataset_identifier.Equals(Custom_data[indexC - 1].Unique_fixed_dataset_identifier))
                    || (!custom_data_line.NCBI_official_symbol.Equals(Custom_data[indexC - 1].NCBI_official_symbol)))
                {
                    sameGeneCount = 0;
                }
                sameGeneCount++;
                if ((indexC == custom_data_length - 1)
                    || (!custom_data_line.Unique_fixed_dataset_identifier.Equals(Custom_data[indexC + 1].Unique_fixed_dataset_identifier))
                    || (!custom_data_line.NCBI_official_symbol.Equals(Custom_data[indexC + 1].NCBI_official_symbol)))
                {
                    if (sameGeneCount > 1)
                    {
                        new_error_message_line = new Read_error_message_line_class();
                        new_error_message_line.File_type = Read_file_type.Data;
                        new_error_message_line.Complete_fileName = (string)completeFileName.Clone();
                        new_error_message_line.Error_message = Read_error_message_enum.Duplicated_entry;
                        sb.Clear();
                        if (consider_integration_groups) { sb.AppendFormat((string)custom_data_line.IntegrationGroup.Clone()); }
                        if (sb.Length > 0) { sb.AppendFormat(" - "); }
                        sb.AppendFormat("{0}: {1} different lines with the same gene {2}", custom_data_line.Unique_dataset_name, sameGeneCount, custom_data_line.NCBI_official_symbol);
                        new_error_message_line.Value = sb.ToString();
                        error_messages.Add(new_error_message_line);
                        max_error_messages_left--;
                    }
                }
                if (max_error_messages_left == 0) { break; }
            }
            Reset_unique_datasetName();
            return error_messages.ToArray();
        }

        private void Convert_hexadecimal_colors_to_csharp_colors()
        {
            Color_conversion_class color_conversion = new Color_conversion_class();
            int custom_data_length = Custom_data.Length;
            Custom_data_line_class custom_data_line;
            string color_string;
            Dictionary<string, Color> colorString_color_dict = new Dictionary<string, Color>();
            for (int indexC = 0; indexC < custom_data_length; indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                color_string = custom_data_line.SampleColor_string;
                if ((color_string.IndexOf('#') == 0)
                    && (color_string.Length == 7))
                {
                    if (!colorString_color_dict.ContainsKey(color_string))
                    {
                        custom_data_line.SampleColor = color_conversion.Get_closest_csharp_color_for_hexadecimal_color_if_exists(color_string);
                        colorString_color_dict.Add(color_string, custom_data_line.SampleColor);
                    }
                    else
                    {
                        custom_data_line.SampleColor = colorString_color_dict[color_string];
                    }
                }

            }
        }

        public Read_error_message_line_class[] Analyze_if_all_sampleColors_resultNumbers_integrationGroups_are_identical_for_each_uniqueFixedDatasetIdentifier_and_geneSymbol(string completeFileName, int max_error_messages_left)
        {
            Set_unique_datasetName_within_whole_custom_data_ignoring_integrationGroups();
            bool consider_integration_groups = this.Get_all_unique_ordered_integrationGroups().Length > 1;
            int custom_data_length = Custom_data.Length;
            Custom_data_line_class custom_data_line;
            //this.Custom_data = this.Custom_data.OrderBy(l => l.Unique_fixed_dataset_identifier).ThenBy(l => l.NCBI_official_symbol).ToArray();
            this.Custom_data = Custom_data_line_class.Order_by_uniqueFixedDatasetIdentifier_ncbiOfficialSymbol(this.Custom_data);
            Dictionary<string, Dictionary<string,bool>> uniqueFixedDatasetIdentifier_integrationGroup_dict = new Dictionary<string, Dictionary<string,bool>>();
            Dictionary<string, Dictionary<Color, bool>> uniqueFixedDatasetIdentifier_sampleColor_dict = new Dictionary<string, Dictionary<Color, bool>>();
            Dictionary<string, Dictionary<int, bool>> uniqueFixedDatasetIdentifier_resultNo_dict = new Dictionary<string, Dictionary<int, bool>>();
            Dictionary<string, string> uniqueFixedDatasetIdentifier_datasetName_dict = new Dictionary<string, string>();
            List<Read_error_message_line_class> error_messages = new List<Read_error_message_line_class>();
            Read_error_message_line_class new_error_message_line;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int indexC = 0; indexC < custom_data_length; indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                if (!uniqueFixedDatasetIdentifier_datasetName_dict.ContainsKey(custom_data_line.Unique_fixed_dataset_identifier))
                {
                    uniqueFixedDatasetIdentifier_datasetName_dict.Add(custom_data_line.Unique_fixed_dataset_identifier, custom_data_line.Unique_dataset_name);
                }

                if (!uniqueFixedDatasetIdentifier_integrationGroup_dict.ContainsKey(custom_data_line.Unique_fixed_dataset_identifier))
                {
                    uniqueFixedDatasetIdentifier_integrationGroup_dict.Add(custom_data_line.Unique_fixed_dataset_identifier, new Dictionary<string, bool>());
                }
                if (!uniqueFixedDatasetIdentifier_integrationGroup_dict[custom_data_line.Unique_fixed_dataset_identifier].ContainsKey(custom_data_line.IntegrationGroup))
                {
                    uniqueFixedDatasetIdentifier_integrationGroup_dict[custom_data_line.Unique_fixed_dataset_identifier].Add(custom_data_line.IntegrationGroup,true);
                    if (uniqueFixedDatasetIdentifier_integrationGroup_dict[custom_data_line.Unique_fixed_dataset_identifier].Keys.ToArray().Length>1)
                    {
                        max_error_messages_left--;
                    }
                }
                if (max_error_messages_left == 0) { break; }
                if (!uniqueFixedDatasetIdentifier_sampleColor_dict.ContainsKey(custom_data_line.Unique_fixed_dataset_identifier))
                {
                    uniqueFixedDatasetIdentifier_sampleColor_dict.Add(custom_data_line.Unique_fixed_dataset_identifier, new Dictionary<Color, bool>());
                }
                if (!uniqueFixedDatasetIdentifier_sampleColor_dict[custom_data_line.Unique_fixed_dataset_identifier].ContainsKey(custom_data_line.SampleColor))
                {
                    uniqueFixedDatasetIdentifier_sampleColor_dict[custom_data_line.Unique_fixed_dataset_identifier].Add(custom_data_line.SampleColor, true);
                    if (uniqueFixedDatasetIdentifier_sampleColor_dict[custom_data_line.Unique_fixed_dataset_identifier].Keys.ToArray().Length > 1)
                    {
                        max_error_messages_left--;
                    }
                }
                if (max_error_messages_left == 0) { break; }
                if (!uniqueFixedDatasetIdentifier_resultNo_dict.ContainsKey(custom_data_line.Unique_fixed_dataset_identifier))
                {
                    uniqueFixedDatasetIdentifier_resultNo_dict.Add(custom_data_line.Unique_fixed_dataset_identifier, new Dictionary<int, bool>());
                }
                if (!uniqueFixedDatasetIdentifier_resultNo_dict[custom_data_line.Unique_fixed_dataset_identifier].ContainsKey(custom_data_line.Results_number))
                {
                    uniqueFixedDatasetIdentifier_resultNo_dict[custom_data_line.Unique_fixed_dataset_identifier].Add(custom_data_line.Results_number, true);
                    if (uniqueFixedDatasetIdentifier_resultNo_dict[custom_data_line.Unique_fixed_dataset_identifier].Keys.ToArray().Length > 1)
                    {
                        max_error_messages_left--;
                    }
                }
                if (max_error_messages_left == 0) { break; }
            }

            int max_multiple_assignments_named = 5;
            string unique_fixed_dataset_identifier;
            string datasetName;
            System.Text.StringBuilder value_sb = new System.Text.StringBuilder();

            #region Add error messages for unique dataset identifiers with mulitple integration group assignments
            string[] integrationGroup_unique_fixed_dataset_identifiers = uniqueFixedDatasetIdentifier_integrationGroup_dict.Keys.ToArray();
            int integrationGroup_unique_fixed_dataset_identifiers_length = integrationGroup_unique_fixed_dataset_identifiers.Length;
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            for (int indexUI = 0; indexUI < integrationGroup_unique_fixed_dataset_identifiers_length; indexUI++)
            {
                unique_fixed_dataset_identifier = integrationGroup_unique_fixed_dataset_identifiers[indexUI];
                integrationGroups = uniqueFixedDatasetIdentifier_integrationGroup_dict[unique_fixed_dataset_identifier].Keys.ToArray();
                integrationGroups_length = integrationGroups.Length;
                if (integrationGroups_length > 1)
                {
                    datasetName = uniqueFixedDatasetIdentifier_datasetName_dict[unique_fixed_dataset_identifier];
                    value_sb.Clear();
                    value_sb.AppendFormat(datasetName + " (e.g.");
                    if (integrationGroups_length> max_multiple_assignments_named) { integrationGroups_length = max_multiple_assignments_named; }
                    for (int indexIG = 0; indexIG < integrationGroups_length; indexIG++)
                    {
                        integrationGroup = integrationGroups[indexIG];
                        value_sb.AppendFormat(", ");
                        value_sb.AppendFormat(integrationGroup);
                    }
                    value_sb.AppendFormat(")");
                    new_error_message_line = new Read_error_message_line_class();
                    new_error_message_line.File_type = Read_file_type.Data;
                    new_error_message_line.Complete_fileName = (string)completeFileName.Clone();
                    new_error_message_line.Error_message = Read_error_message_enum.Multiple_integration_group_assignments_for_dataset;
                    new_error_message_line.Value = value_sb.ToString();
                    error_messages.Add(new_error_message_line);
                }
            }
            #endregion

            #region Add error messages for unique dataset identifiers with mulitple color assignments
            string[] color_unique_fixed_dataset_identifiers = uniqueFixedDatasetIdentifier_sampleColor_dict.Keys.ToArray();
            Color[] current_colors;
            Color current_color;
            int current_colors_length;
            int color_unique_fixed_dataset_identifiers_length = color_unique_fixed_dataset_identifiers.Length;
            for (int indexUI = 0; indexUI < color_unique_fixed_dataset_identifiers_length; indexUI++)
            {
                unique_fixed_dataset_identifier = color_unique_fixed_dataset_identifiers[indexUI];
                current_colors = uniqueFixedDatasetIdentifier_sampleColor_dict[unique_fixed_dataset_identifier].Keys.ToArray();
                current_colors_length = current_colors.Length;
                if (current_colors_length > 1)
                {
                    datasetName = uniqueFixedDatasetIdentifier_datasetName_dict[unique_fixed_dataset_identifier];
                    value_sb.Clear();
                    value_sb.AppendFormat(datasetName + " (e.g.");
                    if (current_colors_length > max_multiple_assignments_named) { current_colors_length = max_multiple_assignments_named; }
                    for (int indexColor = 0; indexColor < current_colors_length; indexColor++)
                    {
                        current_color = current_colors[indexColor];
                        value_sb.AppendFormat(", ");
                        value_sb.AppendFormat(Color_conversion_class.Get_color_string(current_color));
                    }
                    value_sb.AppendFormat(")");
                    new_error_message_line = new Read_error_message_line_class();
                    new_error_message_line.File_type = Read_file_type.Data;
                    new_error_message_line.Complete_fileName = (string)completeFileName.Clone();
                    new_error_message_line.Error_message = Read_error_message_enum.Multiple_color_assignments_for_dataset;
                    new_error_message_line.Value = value_sb.ToString();
                    error_messages.Add(new_error_message_line);
                }
            }
            #endregion

            #region Add error messages for unique dataset identifiers with mulitple result numbers
            string[] resultNo_unique_fixed_dataset_identifiers = uniqueFixedDatasetIdentifier_resultNo_dict.Keys.ToArray();
            int[] current_resultNos;
            int current_resultNo;
            int current_resultNos_length;
            int resultNo_unique_fixed_dataset_identifiers_length = resultNo_unique_fixed_dataset_identifiers.Length;
            for (int indexUI = 0; indexUI < color_unique_fixed_dataset_identifiers_length; indexUI++)
            {
                unique_fixed_dataset_identifier = resultNo_unique_fixed_dataset_identifiers[indexUI];
                current_resultNos = uniqueFixedDatasetIdentifier_resultNo_dict[unique_fixed_dataset_identifier].Keys.ToArray();
                current_resultNos_length = current_resultNos.Length;
                if (current_resultNos_length > 1)
                {
                    datasetName = uniqueFixedDatasetIdentifier_datasetName_dict[unique_fixed_dataset_identifier];
                    value_sb.Clear();
                    value_sb.AppendFormat(datasetName + " (e.g.");
                    if (current_resultNos_length > max_multiple_assignments_named) { current_colors_length = max_multiple_assignments_named; }
                    for (int indexColor = 0; indexColor < current_resultNos_length; indexColor++)
                    {
                        current_resultNo = current_resultNos[indexColor];
                        value_sb.AppendFormat(", ");
                        value_sb.AppendFormat(current_resultNo.ToString());
                    }
                    value_sb.AppendFormat(")");
                    new_error_message_line = new Read_error_message_line_class();
                    new_error_message_line.File_type = Read_file_type.Data;
                    new_error_message_line.Complete_fileName = (string)completeFileName.Clone();
                    new_error_message_line.Error_message = Read_error_message_enum.Multiple_resultNo_assignments_for_dataset;
                    new_error_message_line.Value = value_sb.ToString();
                    error_messages.Add(new_error_message_line);
                }
            }
            #endregion

            Reset_unique_datasetName();
            return error_messages.ToArray();
        }

        public Read_error_message_line_class[] Set_timeunits_based_on_timeunit_strings_and_return_error_messages(string complete_fileName, string columnName, int max_error_messages_left)
        {
            string[] timeunit_strings = this.Get_all_unique_ordered_timeunit_strings();
            bool add_timeunits = false;
            List<Read_error_message_line_class> new_error_messages = new List<Read_error_message_line_class>();
            if ((timeunit_strings.Length>0)&&(!String.IsNullOrEmpty(timeunit_strings[0])))
            {
                add_timeunits = true;
            }
            if (add_timeunits)
            {
                int custom_data_length = this.Custom_data.Length;
                Custom_data_line_class custom_data_line;
                Read_error_message_line_class new_error_message_line;
                for (int indexC = 0; indexC < custom_data_length; indexC++)
                {
                    custom_data_line = this.Custom_data[indexC];
                    custom_data_line.Timeunit = Timeunit_conversion_class.Convert_timepoint_string_to_timeunit(custom_data_line.Timeunit_string);
                    if (custom_data_line.Timeunit.Equals(Timeunit_enum.E_m_p_t_y))
                    {
                        new_error_message_line = new Read_error_message_line_class();
                        new_error_message_line.File_type = Read_file_type.Data;
                        new_error_message_line.LineIndex = indexC+2;
                        new_error_message_line.ColumnName = (string)columnName.Clone();
                        new_error_message_line.Complete_fileName = (string)complete_fileName.Clone();
                        new_error_message_line.Error_message = Read_error_message_enum.Timeunit_not_recognized;
                        new_error_message_line.Value = (string)custom_data_line.Timeunit_string.Clone();
                        new_error_messages.Add(new_error_message_line);
                    }
                    if (new_error_messages.Count > max_error_messages_left)
                    {
                        break;
                    }
                }
            }
            return new_error_messages.ToArray();
        }

        public void Convert_all_timeunits_to_input_unit(Timeunit_enum new_timeunit)
        {
            foreach (Custom_data_line_class custom_data_line in this.Custom_data)
            {
                custom_data_line.Timepoint = Timeunit_conversion_class.Convert_timepoint_from_old_unit_to_new_unit(custom_data_line.Timepoint, custom_data_line.Timeunit, new_timeunit);
                custom_data_line.Timeunit = new_timeunit;
            }
        }

        public void Set_empty_timeunits_to_input_timeunit_and_check_if_all_or_no_timeunits_are_empty(Timeunit_enum default_timeunit)
        {
            if (this.Custom_data.Length > 0)
            {
                bool timeunits_are_empyty = this.Custom_data[0].Timeunit.Equals(Timeunit_enum.E_m_p_t_y);
                if (timeunits_are_empyty)
                {
                    foreach (Custom_data_line_class custom_data_line in this.Custom_data)
                    {
                        if (!custom_data_line.Timeunit.Equals(Timeunit_enum.E_m_p_t_y)) { throw new Exception(); }
                        custom_data_line.Timeunit = default_timeunit;
                    }
                }
            }
        }

        public void Write(string subdirectory, string fileName, ProgressReport_interface_class progressReport, out bool files_opened_successful)
        {
            bool writeIntegrationGroups = true; //Get_all_unique_ordered_integrationGroups().Length > 1;
            bool writeEntryType = true; //Get_all_unique_ordered_entryTypes().Length > 1;
            bool writeTimepoint = true; //Get_all_unique_ordered_timepointsInDays().Length > 1;
            //this.Order_by_source_fileName();
            this.Custom_data = Custom_data_line_class.Order_by_sourceFileName(this.Custom_data);
            int custom_data_length = this.Custom_data.Length;
            Custom_data_line_class custom_data_line;
            List<Custom_data_line_class> sameSource_custom_data = new List<Custom_data_line_class>();
            files_opened_successful = true;
            for (int indexCustom=0; indexCustom<custom_data_length;indexCustom++)
            {
                custom_data_line = this.Custom_data[indexCustom];
                if ((indexCustom==0)||(!custom_data_line.Source_fileName.Equals(this.Custom_data[indexCustom-1].Source_fileName)))
                {
                    sameSource_custom_data.Clear();
                }
                sameSource_custom_data.Add(custom_data_line);
                if ((indexCustom == custom_data_length-1) || (!custom_data_line.Source_fileName.Equals(this.Custom_data[indexCustom + 1].Source_fileName)))
                {
                    fileName = (string)custom_data_line.Source_fileName.Clone();
                    Custom_data_writeOptions_class writeOptions = new Custom_data_writeOptions_class(subdirectory, fileName, writeIntegrationGroups, writeTimepoint, writeEntryType);
                    ReadWriteClass.WriteData_and_add_warning_to_progressReport_if_failed(sameSource_custom_data.ToArray(), writeOptions, progressReport, out bool opened_successful);
                    if (!opened_successful) { files_opened_successful = false; }
                }
            }
            string[] bgGenes_lists_array = Get_all_unique_ordered_bgGeneListNames();
            string bgGenes_list;
            int bgGenes_lists_array_length = bgGenes_lists_array.Length;
            string bgGenes_complete_fileName = "";
            string[] bgGenes;
            for (int indexBg=0; indexBg<bgGenes_lists_array_length;indexBg++)
            {
                bgGenes_list = bgGenes_lists_array[indexBg];
                if (!bgGenes_list.Equals(Global_class.Mbco_exp_background_gene_list_name))
                {
                    bgGenes = this.ExpBgGenesList_bgGenes_dict[bgGenes_list];
                    bgGenes_complete_fileName = subdirectory + bgGenes_list + ".txt";
                    ReadWriteClass.WriteArray(bgGenes, bgGenes_complete_fileName, progressReport, out bool opened_successful);
                    if (!opened_successful) { files_opened_successful = false; }
                }
            }
        }
    }
}
