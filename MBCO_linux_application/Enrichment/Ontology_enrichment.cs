//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

using System;
using System.Collections.Generic;
using System.Linq;
using Common_functions.Global_definitions;
using Common_functions.ReadWrite;
using Common_functions.Array_own;
using System.Drawing;
using System.Text;
using Common_functions.Form_tools;

namespace Enrichment
{
    class Ontology_enrichment_condition_line_class
    {
        public Ontology_type_enum Ontology { get; set; }
        public float Timepoint { get; set; }
        public Entry_type_enum EntryType { get; set; }
        public string Sample_name { get; set; }
        public string Integration_group { get; set; }
        public int ProcessLevel { get; set; }

        public Ontology_enrichment_condition_line_class Deep_copy()
        {
            Ontology_enrichment_condition_line_class copy = (Ontology_enrichment_condition_line_class)this.MemberwiseClone();
            copy.Sample_name = (string)this.Sample_name.Clone();
            copy.Integration_group = (string)this.Integration_group.Clone();
            return copy;
        }
    }

    class Ontology_enrichment_line_class
    {

        #region Fields for MBCO
        public string Scp_id { get; set; }
        public string Scp_name { get; set; }
        public string Parent_scp_name { get; set; }
        public Ontology_type_enum Ontology_type { get; set; }
        public int ProcessLevel { get; set; }
        #endregion

        #region Fields enrichment results
        public float Relative_visitation_frequency { get; set; }
        public float Minus_log10_pvalue { get; set; }
        public double Pvalue { get; set; }
        public double Qvalue { get; set; }
        public double FDR { get; set; }
        public float Fractional_rank { get; set; }
        public int Overlap_count { get; set; }
        public int Process_symbols_count { get; set; }
        public int Experimental_symbols_count { get; set; }
        public int Bg_symbol_count { get; set; }
        public string[] Overlap_symbols { get; set; }
        public string[] Overlap_symbols_with_other_conditions { get; set; }
        public bool Significant { get; set; }
        public string ReadWrite_overlap_symbols
        {
            get { return ReadWriteClass.Get_writeLine_from_array(Overlap_symbols, Ontology_enrich_readWriteOptions_class.Delimiter); }
            set { Overlap_symbols = ReadWriteClass.Get_array_from_readLine<string>(value, Ontology_enrich_readWriteOptions_class.Delimiter); }
        }
        public string ReadWrite_overlap_symbols_with_other_conditions
        {
            get { return ReadWriteClass.Get_writeLine_from_array(Overlap_symbols_with_other_conditions, Ontology_enrich_readWriteOptions_class.Delimiter); }
            set { Overlap_symbols_with_other_conditions = ReadWriteClass.Get_array_from_readLine<string>(value, Ontology_enrich_readWriteOptions_class.Delimiter); }
        }
        #endregion

        #region Fields for sample
        public float Timepoint { get; set; }
        public Timeunit_enum Timeunit { get; set; }
        public float TimepointInDays {  get { return Timeunit_conversion_class.Get_timepoint_in_days(Timepoint, Timeunit); } }
        public Entry_type_enum EntryType { get; set; }
        public string Sample_name { get; set; }
        public string Complete_sample_name { get { return Global_class.Get_complete_sampleName(Integration_group, EntryType, Timepoint, Timeunit, Sample_name); } }
        public string Unique_dataset_name { get; set; }
        public string Integration_group { get; set; }
        public Color Sample_color { get; set; }
        public string Sample_color_string { get { return Color_conversion_class.Get_color_string(Sample_color); } set { Sample_color = Color_conversion_class.Set_color_from_string(value); } }
        public int Results_number { get; set; }
        #endregion

        public Ontology_enrichment_line_class()
        {
            Scp_id = Global_class.Empty_entry;
            Scp_name = Global_class.Empty_entry;
            Parent_scp_name = Global_class.Empty_entry;
            Sample_name = Global_class.Empty_entry;
            Integration_group = Global_class.Empty_entry;
            Overlap_symbols = new string[0];
            Unique_dataset_name = "";
            Pvalue = -1;
        }

        public bool Is_mbco_ontology()
        {
            switch (this.Ontology_type)
            {
                case Ontology_type_enum.Mbco_human:
                case Ontology_type_enum.Mbco_mouse:
                case Ontology_type_enum.Mbco_rat:
                    return true;
                default:
                    return false;
            }
        }

        #region Standard way
        public static Ontology_enrichment_line_class[] Order_by_sample_and_scpName(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            onto_enrich_array = onto_enrich_array.OrderBy(l => l.Ontology_type).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.Sample_name).ThenBy(l => l.Scp_name).ToArray();
            return onto_enrich_array;
        }

        public static Ontology_enrichment_line_class[] Order_by_sampleName_and_scpName(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            onto_enrich_array = onto_enrich_array.OrderBy(l => l.Sample_name).ThenBy(l => l.Scp_name).ToArray();
            return onto_enrich_array;
        }

        public static Ontology_enrichment_line_class[] Order_by_complete_sample_pvalue(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            onto_enrich_array = onto_enrich_array.OrderBy(l => l.Ontology_type).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.Sample_name).ThenBy(l => l.Pvalue).ToArray();
            return onto_enrich_array;
        }
        public static Ontology_enrichment_line_class[] Order_level_entryType_timepoint_sampleName(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            onto_enrich_array = onto_enrich_array.OrderBy(l => l.ProcessLevel).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.Sample_name).ToArray();
            return onto_enrich_array;
        }

        public static Ontology_enrichment_line_class[] Order_entryType_timepoint_sampleName(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            onto_enrich_array = onto_enrich_array.OrderBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.Sample_name).ToArray();
            return onto_enrich_array;
        }

        public static Ontology_enrichment_line_class[] Order_entryType_timepoint_sampleName_level_pvalue(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            onto_enrich_array = onto_enrich_array.OrderBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.Sample_name).ThenBy(l=>l.ProcessLevel).ThenBy(l=>l.Pvalue).ToArray();
            return onto_enrich_array;
        }

        public bool Equal_complete_sample(Ontology_enrichment_line_class other)
        {
            bool equal = ((this.Ontology_type.Equals(other.Ontology_type))
                          && (this.EntryType.Equals(other.EntryType))
                          && (this.Timepoint.Equals(other.Timepoint))
                          && (this.Sample_name.Equals(other.Sample_name)));
            return equal;
        }
        #endregion

        public Ontology_enrichment_line_class Deep_copy()
        {
            Ontology_enrichment_line_class copy = (Ontology_enrichment_line_class)this.MemberwiseClone();
            copy.Scp_id = (string)this.Scp_id.Clone();
            copy.Scp_name = (string)this.Scp_name.Clone();
            copy.Sample_name = (string)this.Sample_name.Clone();
            copy.Parent_scp_name = (string)this.Parent_scp_name.Clone(); 
            copy.Integration_group = (string)this.Integration_group.Clone();
            copy.Unique_dataset_name = (string)this.Unique_dataset_name.Clone();
            int symbols_length = this.Overlap_symbols.Length;
            copy.Overlap_symbols = new string[symbols_length];
            for (int indexS=0; indexS<symbols_length; indexS++)
            {
                copy.Overlap_symbols[indexS] = (string)this.Overlap_symbols[indexS].Clone();
            }
            return copy;
        }
    }

    class Ontology_enrich_readWriteOptions_class : ReadWriteOptions_base
    {
        public static char Delimiter { get {return ',';}}
        
        public Ontology_enrich_readWriteOptions_class(string subdirectory, string fileName, bool write_integrationGroup, bool write_entryType, bool write_timepoints)
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            string directory = subdirectory;
            ReadWriteClass.Create_directory_if_it_does_not_exist(directory);
            this.File = directory + fileName;

            List<string> key_propertyNames_list = new List<string>();
            List<string> key_columnNames_list = new List<string>();
            key_propertyNames_list.AddRange(new string[] { "Ontology_type", "ProcessLevel" });
            key_columnNames_list.AddRange(new string[] { "Ontology", "SCP level" });
            if (write_integrationGroup)
            {
                key_propertyNames_list.AddRange(new string[] { "Integration_group" });
                key_columnNames_list.AddRange(new string[] { "Integration group" });
            }
            key_propertyNames_list.AddRange(new string[] { "Sample_name" });
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
            key_propertyNames_list.AddRange(new string[] { "Sample_color_string", "Scp_name", "Bg_symbol_count",       "Experimental_symbols_count",      "Process_symbols_count",  "Overlap_count",              "Pvalue", "Minus_log10_pvalue", "Fractional_rank", "ReadWrite_overlap_symbols", "Significant" });
            key_columnNames_list.AddRange(new string[]   { "Dataset color",       "SCP",      "Bg gene symbols count", "Experimental gene symbols count", "SCP gene symbols count", "Overlap gene symbols count", "Pvalue", "Minus log10_pvalue", "Fractional rank", "Overlapping gene symbols",  "Significant" });

            Key_propertyNames = key_propertyNames_list.ToArray();
            Key_columnNames = key_columnNames_list.ToArray();

            File_has_headline = true;
            LineDelimiters = new char[] { Global_class.Tab };
            HeadlineDelimiters = new char[] { Global_class.Tab };
            Report = ReadWrite_report_enum.Report_main;
        }
    }

    class Ontology_enrichment_class
    {
        public Ontology_enrichment_line_class[] Enrich { get; set; }

        public Ontology_enrichment_class()
        {
            Enrich = new Ontology_enrichment_line_class[0];
        }

        #region Check
        public void Check_for_correctness()
        {
            this.Enrich = this.Enrich.OrderBy(l => l.Integration_group).ThenBy(l => l.Results_number).ToArray();
            int custom_data_length = this.Enrich.Length;
            Ontology_enrichment_line_class previous_enrich_data_line;
            Ontology_enrichment_line_class this_enrich_data_line;
            Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, int>>> sampleName_timepointsInDays_entryType_resultsNumber_dict = new Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, int>>>();
            for (int indexC = 0; indexC < custom_data_length; indexC++)
            {
                this_enrich_data_line = this.Enrich[indexC];
                if (  (indexC==0)
                    ||(this_enrich_data_line.Integration_group.Equals(this.Enrich[indexC-1].Integration_group)))
                {
                    sampleName_timepointsInDays_entryType_resultsNumber_dict.Clear();
                }
                if (!sampleName_timepointsInDays_entryType_resultsNumber_dict.ContainsKey(this_enrich_data_line.Sample_name))
                { sampleName_timepointsInDays_entryType_resultsNumber_dict.Add(this_enrich_data_line.Sample_name, new Dictionary<float, Dictionary<Entry_type_enum, int>>()); }
                if (!sampleName_timepointsInDays_entryType_resultsNumber_dict[this_enrich_data_line.Sample_name].ContainsKey(this_enrich_data_line.TimepointInDays))
                { sampleName_timepointsInDays_entryType_resultsNumber_dict[this_enrich_data_line.Sample_name].Add(this_enrich_data_line.TimepointInDays, new Dictionary<Entry_type_enum, int>()); }
                if (!sampleName_timepointsInDays_entryType_resultsNumber_dict[this_enrich_data_line.Sample_name][this_enrich_data_line.TimepointInDays].ContainsKey(this_enrich_data_line.EntryType))
                { sampleName_timepointsInDays_entryType_resultsNumber_dict[this_enrich_data_line.Sample_name][this_enrich_data_line.TimepointInDays].Add(this_enrich_data_line.EntryType, this_enrich_data_line.Results_number); }
                else if (!sampleName_timepointsInDays_entryType_resultsNumber_dict[this_enrich_data_line.Sample_name][this_enrich_data_line.TimepointInDays][this_enrich_data_line.EntryType].Equals(this_enrich_data_line.Results_number))
                { throw new Exception(); }

                if (indexC > 0)
                {
                    previous_enrich_data_line = this.Enrich[indexC - 1];
                    if (this_enrich_data_line.Integration_group.Equals(previous_enrich_data_line.Integration_group))
                    {
                        if ((previous_enrich_data_line.Integration_group.Equals(this_enrich_data_line.Integration_group))
                            && (previous_enrich_data_line.Results_number.Equals(this_enrich_data_line.Results_number)))
                        {
                            if (!previous_enrich_data_line.Sample_name.Equals(this_enrich_data_line.Sample_name)) { throw new Exception(); }
                            if (!previous_enrich_data_line.Sample_color.Equals(this_enrich_data_line.Sample_color)) { throw new Exception(); }
                            if (!previous_enrich_data_line.TimepointInDays.Equals(this_enrich_data_line.TimepointInDays)) { throw new Exception(); }
                            if (!previous_enrich_data_line.Timepoint.Equals(this_enrich_data_line.Timepoint)) { throw new Exception(); }
                            if (!previous_enrich_data_line.Timeunit.Equals(this_enrich_data_line.Timeunit)) { throw new Exception(); }
                            if (!previous_enrich_data_line.EntryType.Equals(this_enrich_data_line.EntryType)) { throw new Exception(); }
                        }
                    }
                }
            }
        }

        public void Check_if_SCP_exists(string scp)
        {
            bool scp_exists = false;
            foreach(Ontology_enrichment_line_class enrich_line in this.Enrich)
            {
                if (enrich_line.Scp_name.Equals(scp))
                {
                    scp_exists = true;
                }
            }
            if (!scp_exists) { throw new Exception(); }
        }

        public void Check_if_one_integrationGroup()
        {
            string integrationGroup = Enrich[0].Integration_group;
            foreach (Ontology_enrichment_line_class enrich_line in Enrich)
            {
                if (!enrich_line.Integration_group.Equals(integrationGroup)) { throw new Exception(); }
            }
        }
        #endregion

        #region Order
        public void Order_by_processLevel()
        {
            Enrich = Enrich.OrderBy(l => l.ProcessLevel).ToArray();
        }

        public void Order_by_processLevel_parentScp_scp()
        {
            Enrich = Enrich.OrderBy(l => l.ProcessLevel).ThenBy(l=>l.Parent_scp_name).ThenBy(l=>l.Scp_name).ToArray();
        }

        public void Order_by_processLevel_descendingMinusLog10Pvalue()
        {
            Enrich = Enrich.OrderBy(l => l.ProcessLevel).ThenByDescending(l=>l.Minus_log10_pvalue).ToArray();
        }

        public void Order_by_scpName()
        {
            Enrich = Enrich.OrderBy(l => l.Scp_name).ToArray();
        }

        public void Order_by_sample_scpName()
        {
            Enrich = Ontology_enrichment_line_class.Order_by_sample_and_scpName(this.Enrich);
        }

        public void Order_by_level()
        {
            Enrich = this.Enrich.OrderBy(l => l.ProcessLevel).ToArray();
        }

        public void Order_by_level_entryType_timepoint_sampleName()
        {
            Enrich = Ontology_enrichment_line_class.Order_level_entryType_timepoint_sampleName(this.Enrich);
        }

        public void Order_by_entryType_timepoint_sampleName()
        {
            Enrich = Ontology_enrichment_line_class.Order_entryType_timepoint_sampleName(this.Enrich);
        }

        public void Order_by_entryType_timepoint_sampleName_level_pvalue()
        {
            Enrich = Ontology_enrichment_line_class.Order_entryType_timepoint_sampleName(this.Enrich);
        }

        public void Order_by_sampleName_scpName()
        {
            Enrich = Ontology_enrichment_line_class.Order_by_sampleName_and_scpName(this.Enrich);
        }

        public void Order_by_sampleName_entryType()
        {
            Enrich = this.Enrich.OrderBy(l => l.Sample_name).ThenBy(l=>l.EntryType).ToArray();
        }

        public void Order_by_complete_sample_pvalue()
        {
            Enrich = Ontology_enrichment_line_class.Order_by_complete_sample_pvalue(this.Enrich);
        }
        public void Order_by_integrationGroup_level_scpName()
        {
            Enrich = Enrich.OrderBy(l => l.Integration_group).ThenBy(l => l.ProcessLevel).ThenBy(l=>l.Scp_name).ToArray();
        }
        public void Order_by_uniqueDatasetName_scpName()
        {
            this.Enrich = this.Enrich.OrderBy(l => l.Unique_dataset_name).ThenBy(l => l.Scp_name).ToArray();
        }
        public void Order_by_uniqueDatasetName_level()
        {
            this.Enrich = this.Enrich.OrderBy(l => l.Unique_dataset_name).ThenBy(l => l.ProcessLevel).ToArray();
        }
        public void Order_by_level_uniqueDatasetName()
        {
            this.Enrich = this.Enrich.OrderBy(l => l.ProcessLevel).ThenBy(l => l.Unique_dataset_name).ToArray();
        }
        public void Order_by_level_results_no()
        {
            this.Enrich = this.Enrich.OrderBy(l => l.ProcessLevel).ThenBy(l => l.Results_number).ToArray();
        }
        public void Order_by_level_scpName()
        {
            this.Enrich = this.Enrich.OrderBy(l => l.ProcessLevel).ThenBy(l => l.Scp_name).ToArray();
        }
        public void Order_by_results_no()
        {
            this.Enrich = this.Enrich.OrderBy(l => l.Results_number).ToArray();
        }
        #endregion

        #region Filter
        public void Keep_only_signficant_enrichment_lines_and_reset_uniqueDatasetNames()
        {
            List<Ontology_enrichment_line_class> keep_enrichment_lines = new List<Ontology_enrichment_line_class>();
            int enrich_length = this.Enrich.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                onto_enrich_line = this.Enrich[indexE];
                if (onto_enrich_line.Significant)
                {
                    keep_enrichment_lines.Add(onto_enrich_line);
                }
            }
            this.Enrich = keep_enrichment_lines.ToArray();
        }

        public void Keep_only_enrichment_lines_of_indicated_levels(params int[] levels)
        {
            int enrichment_length = this.Enrich.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            List<Ontology_enrichment_line_class> keep_enrich_list = new List<Ontology_enrichment_line_class>();
            for (int indexE = 0; indexE < enrichment_length; indexE++)
            {
                onto_enrich_line = this.Enrich[indexE];
                if (levels.Contains(onto_enrich_line.ProcessLevel))
                {
                    keep_enrich_list.Add(onto_enrich_line);
                }
            }
            this.Enrich = keep_enrich_list.ToArray();
        }

        public void Set_significance_based_on_ranks_and_pvalue_after_calculation_of_fractional_rank(int[] max_fractional_ranks_per_level, float max_pvalue)
        {
            Calculate_fractional_ranks_for_SCPs_within_each_integrationGroup_sampleName_timepoint_timeunit_entryType_processLevel();
            int enrich_length = this.Enrich.Length;
            Ontology_enrichment_line_class enrichment_line;
            List<Ontology_enrichment_line_class> keep_onto_list = new List<Ontology_enrichment_line_class>();
            int processLevel;
            this.Enrich = this.Enrich.OrderBy(l => l.Ontology_type).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.Sample_name).ThenBy(l => l.ProcessLevel).ThenByDescending(l => l.Minus_log10_pvalue).ToArray();
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrichment_line = this.Enrich[indexE];
                if (enrichment_line.Is_mbco_ontology())
                {
                    processLevel = enrichment_line.ProcessLevel;
                }
                else
                {
                    processLevel = Global_class.ProcessLevel_for_all_non_MBCO_SCPs;
                }
                if (  (enrichment_line.Fractional_rank <= max_fractional_ranks_per_level[processLevel])
                    &&(enrichment_line.Pvalue<= max_pvalue))
                {
                    enrichment_line.Significant = true;
                }
                else
                {
                    enrichment_line.Significant = false;
                }
            }
        }

        public void Identify_significant_predictions_and_keep_all_lines_with_these_SCPs_for_each_sample()
        {
            int enrich_length = this.Enrich.Length;
            Ontology_enrichment_line_class enrichment_line;
            List<Ontology_enrichment_line_class> keep_onto_list = new List<Ontology_enrichment_line_class>();
            int kept_lines_count = 0;
            Dictionary<string, bool> keepScp_dict = new Dictionary<string, bool>();
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrichment_line = this.Enrich[indexE];
                if (enrichment_line.Significant)
                {
                    if (!keepScp_dict.ContainsKey(enrichment_line.Scp_name))
                    {
                        keepScp_dict.Add(enrichment_line.Scp_name, true);
                    }
                }
            }
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrichment_line = this.Enrich[indexE];
                if (keepScp_dict.ContainsKey(enrichment_line.Scp_name))
                {
                    keep_onto_list.Add(enrichment_line);
                    kept_lines_count++;
                }
            }
            this.Enrich = keep_onto_list.ToArray();
        }

        public void Keep_only_input_scpNames(params string[] keep_scpNames)
        {
            keep_scpNames = keep_scpNames.Distinct().OrderBy(l => l).ToArray();
            this.Enrich = this.Enrich.OrderBy(l => l.Scp_name).ToArray();
            int enrich_length = this.Enrich.Length;
            string keep_scpName;
            int stringCompare;
            int indexKeep = 0;
            int keep_length = keep_scpNames.Length;
            Ontology_enrichment_line_class enrich_line;
            List<Ontology_enrichment_line_class> keep = new List<Ontology_enrichment_line_class>();
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                stringCompare = -2;
                while ((indexKeep < keep_length) && (stringCompare < 0))
                {
                    keep_scpName = keep_scpNames[indexKeep];
                    stringCompare = keep_scpName.CompareTo(enrich_line.Scp_name);
                    if (stringCompare < 0)
                    {
                        indexKeep++;
                    }
                    else if (stringCompare == 0)
                    {
                        keep.Add(enrich_line);
                    }
                }
            }
            this.Enrich = keep.ToArray();
        }
        #endregion

        #region Get
        public string[] Get_all_integrationGroups()
        {
            int enrich_length = this.Enrich.Length;
            Dictionary<string, bool> integrationGroups_dict = new Dictionary<string, bool>();
            Ontology_enrichment_line_class enrich_line;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                if (!integrationGroups_dict.ContainsKey(enrich_line.Integration_group))
                {
                    integrationGroups_dict.Add(enrich_line.Integration_group, true);
                }
            }
            return integrationGroups_dict.Keys.Distinct().OrderBy(l => l).ToArray();
        }

        public void Get_sampleName_timepoint_entryType_and_check_if_only_one(out string sampleName, out float timepoint, out Entry_type_enum entryType)
        {
            sampleName = (string)this.Enrich[0].Sample_name.Clone();
            timepoint = this.Enrich[0].Timepoint;
            entryType = this.Enrich[0].EntryType;
            foreach (Ontology_enrichment_line_class enrich_line in this.Enrich)
            {
                if (  (!enrich_line.Sample_name.Equals(sampleName))
                    ||(!enrich_line.Timepoint.Equals(timepoint))
                    ||(!enrich_line.EntryType.Equals(entryType)))
                {
                    throw new Exception();
                }
            }
        }

        public string[] Get_all_uniqueDatasetNames()
        {
            int enrich_length = this.Enrich.Length;
            Dictionary<string,bool> uniqueDatasetNames_dict = new Dictionary<string, bool>();
            Ontology_enrichment_line_class enrich_line;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                if (!uniqueDatasetNames_dict.ContainsKey(enrich_line.Unique_dataset_name))
                {
                    uniqueDatasetNames_dict.Add(enrich_line.Unique_dataset_name, true);
                }
            }
            return uniqueDatasetNames_dict.Keys.ToArray().OrderBy(l => l).ToArray();
        }

        public Entry_type_enum[] Get_all_entryTypes()
        {
            int enrich_length = this.Enrich.Length;
            List<Entry_type_enum> integrationGroups = new List<Entry_type_enum>();
            Ontology_enrichment_line_class enrich_line;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                integrationGroups.Add(enrich_line.EntryType);
            }
            return integrationGroups.Distinct().OrderBy(l => l).ToArray();
        }

        public string[] Get_all_sampleNames()
        {
            int enrich_length = this.Enrich.Length;
            Dictionary<string, bool> sampleName_dict = new Dictionary<string, bool>(); ;
            Ontology_enrichment_line_class enrich_line;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                if (!sampleName_dict.ContainsKey(enrich_line.Sample_name))
                {
                    sampleName_dict.Add(enrich_line.Sample_name, true);
                }
            }
            return sampleName_dict.Keys.ToArray();
        }

        public float[] Get_all_timepointsInDays()
        {
            int enrich_length = this.Enrich.Length;
            Dictionary<float, bool> timepointsInDay_dict = new Dictionary<float, bool>();
            Ontology_enrichment_line_class enrich_line;
            float timepointsInDays;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                timepointsInDays = enrich_line.TimepointInDays;
                if (!timepointsInDay_dict.ContainsKey(timepointsInDays))
                {
                    timepointsInDay_dict.Add(timepointsInDays, true);
                }
            }
            return timepointsInDay_dict.Keys.OrderBy(l => l).ToArray();
        }

        public Ontology_enrichment_condition_line_class[] Get_all_conditions()
        {
            this.Enrich = this.Enrich.OrderBy(l=>l.Integration_group).ThenBy(l => l.Sample_name).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l=>l.ProcessLevel).ToArray();
            int enrich_length = this.Enrich.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            Ontology_enrichment_condition_line_class condition_line;
            List<Ontology_enrichment_condition_line_class> conditions = new List<Ontology_enrichment_condition_line_class>();
            for (int indexE=0; indexE<enrich_length; indexE++)
            {
                onto_enrich_line = this.Enrich[indexE];
                if ((indexE==0)
                    || (!onto_enrich_line.Integration_group.Equals(this.Enrich[indexE - 1].Integration_group))
                    || (!onto_enrich_line.Sample_name.Equals(this.Enrich[indexE - 1].Sample_name))
                    || (!onto_enrich_line.EntryType.Equals(this.Enrich[indexE - 1].EntryType))
                    || (!onto_enrich_line.Timepoint.Equals(this.Enrich[indexE - 1].Timepoint))
                    || (!onto_enrich_line.ProcessLevel.Equals(this.Enrich[indexE - 1].ProcessLevel)))
                {
                    condition_line = new Ontology_enrichment_condition_line_class();
                    condition_line.Integration_group = (string)onto_enrich_line.Integration_group.Clone();
                    condition_line.Sample_name = (string)onto_enrich_line.Sample_name.Clone();
                    condition_line.EntryType = onto_enrich_line.EntryType;
                    condition_line.Timepoint = onto_enrich_line.Timepoint;
                    condition_line.ProcessLevel = onto_enrich_line.ProcessLevel;
                    conditions.Add(condition_line);
                }
            }
            return conditions.ToArray();
        }

        public void Keep_only_indicated_conditions(Ontology_enrichment_condition_line_class[] conditions)
        {
            conditions = conditions.OrderBy(l=>l.Integration_group).ThenBy(l => l.Sample_name).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.ProcessLevel).ToArray();
            int conditions_length = conditions.Length;
            int indexConditions = 0;
            int stringCompare;
            this.Enrich = this.Enrich.OrderBy(l => l.Integration_group).ThenBy(l => l.Sample_name).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.ProcessLevel).ToArray();
            int enrich_length = this.Enrich.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            Ontology_enrichment_condition_line_class condition_line;
            List<Ontology_enrichment_line_class> keep = new List<Ontology_enrichment_line_class>();
            bool condition_found = false;
            for (int indexE=0; indexE<enrich_length;indexE++)
            {
                onto_enrich_line = this.Enrich[indexE];
                stringCompare = -2;
                while ((stringCompare < 0)&&(indexConditions<conditions_length))
                {
                    condition_line = conditions[indexConditions];
                    stringCompare = condition_line.Integration_group.CompareTo(onto_enrich_line.Integration_group);
                    if (stringCompare == 0)
                    {
                        stringCompare = condition_line.Sample_name.CompareTo(onto_enrich_line.Sample_name);
                    }
                    if (stringCompare == 0)
                    {
                        stringCompare = condition_line.EntryType.CompareTo(onto_enrich_line.EntryType);
                    }
                    if (stringCompare == 0)
                    {
                        stringCompare = condition_line.Timepoint.CompareTo(onto_enrich_line.Timepoint);
                    }
                    if (stringCompare == 0)
                    {
                        stringCompare = condition_line.ProcessLevel.CompareTo(onto_enrich_line.ProcessLevel);
                    }
                    if (stringCompare < 0)
                    {
                        indexConditions++;
                        if (!condition_found) { throw new Exception(); }
                        condition_found = false;
                    }
                    else if (stringCompare == 0)
                    {
                        keep.Add(onto_enrich_line);
                        condition_found = true;
                    }
                }
            }
        }

        public string[] Get_all_scps_of_completeSample(string completeSampleName)
        {
            int enrich_length = this.Enrich.Length;
            List<string> scpNames = new List<string>();
            Ontology_enrichment_line_class enrich_line;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                if (enrich_line.Complete_sample_name.Equals(completeSampleName))
                {
                    scpNames.Add(enrich_line.Scp_name);
                }
            }
            if (scpNames.Distinct().ToArray().Length != scpNames.Count)
            {
                throw new Exception("if only one sample, double entries should not exist");
            }
            return scpNames.OrderBy(l => l).ToArray();
        }

        public string[] Get_all_scps_of_sample(string sampleName)
        {
            int enrich_length = this.Enrich.Length;
            List<string> scpNames = new List<string>();
            Ontology_enrichment_line_class enrich_line;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                if (enrich_line.Sample_name.Equals(sampleName))
                {
                    scpNames.Add(enrich_line.Scp_name);
                }
            }
            if (scpNames.Distinct().ToArray().Length != scpNames.Count)
            {
                throw new Exception("if only one sample, double entries should not exist");
            }
            return scpNames.OrderBy(l => l).ToArray();
        }

        public string[] Get_all_scps()
        {
            int enrich_length = this.Enrich.Length;
            Dictionary<string, bool> scpNames_dict = new Dictionary<string, bool>();
            Ontology_enrichment_line_class enrich_line;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                if (!scpNames_dict.ContainsKey(enrich_line.Scp_name))
                { scpNames_dict.Add(enrich_line.Scp_name, true); }
            }
            return scpNames_dict.Keys.Distinct().OrderBy(l => l).ToArray();
        }

        public int[] Get_all_levels()
        {
            List<int> scpLevels = new List<int>();
            Ontology_enrichment_line_class enrich_line;
            int enrich_length = this.Enrich.Length;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                scpLevels.Add(enrich_line.ProcessLevel);
            }
            return scpLevels.Distinct().OrderBy(l => l).ToArray();
        }

        public string[] Get_all_scps_after_spliting_scp_unions()
        {
            int enrich_length = this.Enrich.Length;
            List<string> scpNames = new List<string>();
            Ontology_enrichment_line_class enrich_line;
            string[] scps;
            string scp;
            int scps_length;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                scps = enrich_line.Scp_name.Split('$');
                scps_length = scps.Length;
                for (int indexS = 0; indexS < scps_length; indexS++)
                {
                    scp = scps[indexS];
                    scpNames.Add(scp);
                }
            }
            return scpNames.Distinct().OrderBy(l => l).ToArray();
        }

        public string[] Get_all_scps_of_indicated_levels(params int[] levels)
        {
            int enrich_length = this.Enrich.Length;
            List<string> scpNames = new List<string>();
            Ontology_enrichment_line_class enrich_line;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                if (levels.Contains(enrich_line.ProcessLevel))
                {
                    scpNames.Add(enrich_line.Scp_name);
                }
            }
            return scpNames.Distinct().OrderBy(l => l).ToArray();
        }

        public string[] Get_all_genes()
        {
            int enrich_length = this.Enrich.Length;
            List<string> genes = new List<string>();
            Ontology_enrichment_line_class enrich_line;
            int indexOpenBracket = -1;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                foreach (string gene in enrich_line.Overlap_symbols)
                {
                    indexOpenBracket = gene.IndexOf('(');
                    genes.Add(gene.Substring(0, indexOpenBracket - 1));
                }
            }
            return genes.Distinct().OrderBy(l => l).ToArray();
        }

        public Ontology_type_enum Get_ontology_and_check_if_only_one_ontology()
        {
            if (Enrich.Length > 0)
            {
                Ontology_type_enum ontology = Enrich[0].Ontology_type;
                int enrich_length = this.Enrich.Length;
                Ontology_enrichment_line_class onto_enrich_line;
                for (int indexE = 0; indexE < enrich_length; indexE++)
                {
                    onto_enrich_line = this.Enrich[indexE];
                    if (!onto_enrich_line.Ontology_type.Equals(ontology))
                    {
                        throw new Exception();
                    }
                }
                return ontology;
            }
            return Ontology_type_enum.E_m_p_t_y;
        }
        #endregion

        #region Get other
        public Ontology_enrichment_class Get_new_enrichment_instance_with_indicated_integrationGroup(string integrationGroup)
        {
            int enrich_length = this.Enrich.Length;
            Ontology_enrichment_line_class enrich_line;
            List<Ontology_enrichment_line_class> integrationGroup_enrichment_lines = new List<Ontology_enrichment_line_class>();
            for (int indexE=0; indexE<enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                if (enrich_line.Integration_group.Equals(integrationGroup))
                {
                    integrationGroup_enrichment_lines.Add(enrich_line.Deep_copy());
                }
            }
            Ontology_enrichment_class enrich = new Ontology_enrichment_class();
            enrich.Add_other_lines_without_resetting_unique_datasetNames(integrationGroup_enrichment_lines.ToArray());
            return enrich;
        }
        #endregion

        #region Add
        public void Add_other_lines_and_reset_uniqueDatasetNames(Ontology_enrichment_line_class[] other_lines)
        {
            int this_enrich_length = this.Enrich.Length;
            int other_enrich_length = other_lines.Length;
            int new_enrich_length = this_enrich_length + other_enrich_length;
            Ontology_enrichment_line_class[] new_enrich = new Ontology_enrichment_line_class[new_enrich_length];
            int indexNew = -1;
            for (int indexThis = 0; indexThis < this_enrich_length; indexThis++)
            {
                indexNew++;
                new_enrich[indexNew] = this.Enrich[indexThis];
            }
            for (int indexOther = 0; indexOther < other_enrich_length; indexOther++)
            {
                indexNew++;
                new_enrich[indexNew] = other_lines[indexOther].Deep_copy();
            }
            this.Enrich = new_enrich;
        }

        public void Add_other_lines_without_resetting_unique_datasetNames(Ontology_enrichment_line_class[] other_lines)
        {
            int this_enrich_length = this.Enrich.Length;
            int other_enrich_length = other_lines.Length;
            int new_enrich_length = this_enrich_length + other_enrich_length;
            Ontology_enrichment_line_class[] new_enrich = new Ontology_enrichment_line_class[new_enrich_length];
            int indexNew = -1;
            for (int indexThis = 0; indexThis < this_enrich_length; indexThis++)
            {
                indexNew++;
                new_enrich[indexNew] = this.Enrich[indexThis];
            }
            for (int indexOther = 0; indexOther < other_enrich_length; indexOther++)
            {
                indexNew++;
                new_enrich[indexNew] = other_lines[indexOther].Deep_copy();
            }
            this.Enrich = new_enrich;
        }

        public void Add_other(Ontology_enrichment_class other)
        {
            Add_other_lines_and_reset_uniqueDatasetNames(other.Enrich);
        }

        public void Add_new_enrichment_lines_for_each_process_with_missing_completeSampleNames()
        {
            int onto_enrich_length = this.Enrich.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            this.Enrich = this.Enrich.OrderBy(l => l.Scp_name).ThenBy(l => l.Complete_sample_name).ToArray();

            List<Ontology_enrichment_line_class> new_onto_enrich = new List<Ontology_enrichment_line_class>();
            Ontology_enrichment_line_class new_missing_completeSampleName;
            List<Ontology_enrichment_line_class> missing_completeSampleNames = new List<Ontology_enrichment_line_class>();

            List<int> all_resultNos_list = new List<int>();
            List<string> all_integrationGroups_list = new List<string>();
            List<string> all_completeSampleNames_list = new List<string>();
            List<string> all_sampleNames_list = new List<string>();
            List<Color> all_colors_list = new List<Color>();
            List<Entry_type_enum> all_entryTypes_list = new List<Entry_type_enum>();
            List<float> all_timepoints_list = new List<float>();
            List<string> all_uniqueDatasetNames_list = new List<string>();

            this.Enrich = this.Enrich.OrderBy(l => l.Complete_sample_name).ToArray();

            for (int indexE = 0; indexE < onto_enrich_length; indexE++)
            {
                onto_enrich_line = this.Enrich[indexE];
                if ((indexE == 0) || (!onto_enrich_line.Complete_sample_name.Equals(this.Enrich[indexE - 1].Complete_sample_name)))
                {
                    all_completeSampleNames_list.Add(onto_enrich_line.Complete_sample_name);
                    all_integrationGroups_list.Add(onto_enrich_line.Integration_group);
                    all_sampleNames_list.Add(onto_enrich_line.Sample_name);
                    all_timepoints_list.Add(onto_enrich_line.Timepoint);
                    all_entryTypes_list.Add(onto_enrich_line.EntryType);
                    all_colors_list.Add(onto_enrich_line.Sample_color);
                    all_resultNos_list.Add(onto_enrich_line.Results_number);
                    all_uniqueDatasetNames_list.Add(onto_enrich_line.Unique_dataset_name);
                }
            }

            string[] all_completeSampleNames = all_completeSampleNames_list.OrderBy(l => l).ToArray();
            string completeSampleName;
            int all_completeSampleNames_length = all_completeSampleNames.Length;
            int indexAllCompleteSampleNames = 0;
            this.Enrich = this.Enrich.OrderBy(l => l.Scp_name).ThenBy(l => l.Complete_sample_name).ToArray();
            int stringCompare = -2;

            for (int indexE = 0; indexE < onto_enrich_length; indexE++)
            {
                onto_enrich_line = this.Enrich[indexE];
                if ((indexE == 0)
                    || (!onto_enrich_line.Scp_name.Equals(this.Enrich[indexE - 1].Scp_name)))
                {
                    indexAllCompleteSampleNames = 0;
                }
                stringCompare = -2;
                while ((stringCompare<0)&&(indexAllCompleteSampleNames < all_completeSampleNames_length))
                {
                    completeSampleName = all_completeSampleNames[indexAllCompleteSampleNames];
                    stringCompare = completeSampleName.CompareTo(onto_enrich_line.Complete_sample_name);
                    if (stringCompare<0)
                    {
                        new_missing_completeSampleName = onto_enrich_line.Deep_copy();
                        new_missing_completeSampleName.Integration_group = (string)all_integrationGroups_list[indexAllCompleteSampleNames].Clone();
                        new_missing_completeSampleName.Sample_name = (string)all_sampleNames_list[indexAllCompleteSampleNames].Clone();
                        new_missing_completeSampleName.Timepoint = all_timepoints_list[indexAllCompleteSampleNames];
                        new_missing_completeSampleName.EntryType = all_entryTypes_list[indexAllCompleteSampleNames];
                        new_missing_completeSampleName.Sample_color = all_colors_list[indexAllCompleteSampleNames];
                        new_missing_completeSampleName.Results_number = all_resultNos_list[indexAllCompleteSampleNames];
                        new_missing_completeSampleName.Unique_dataset_name = (string)all_uniqueDatasetNames_list[indexAllCompleteSampleNames].Clone();
                        new_missing_completeSampleName.Pvalue = 1;
                        new_missing_completeSampleName.Minus_log10_pvalue = 0;
                        new_missing_completeSampleName.Fractional_rank = 999999;
                        new_missing_completeSampleName.Overlap_symbols = new string[0];
                        new_missing_completeSampleName.Significant = false;
                        missing_completeSampleNames.Add(new_missing_completeSampleName);
                        indexAllCompleteSampleNames++;
                    }
                    else if (stringCompare==0)
                    {
                        indexAllCompleteSampleNames++;
                    }
                }
                if ((indexE == onto_enrich_length - 1)
                    || (!onto_enrich_line.Scp_name.Equals(this.Enrich[indexE + 1].Scp_name)))
                {
                    while ((indexAllCompleteSampleNames < all_completeSampleNames_length))
                    {
                        completeSampleName = all_completeSampleNames[indexAllCompleteSampleNames];
                        new_missing_completeSampleName = onto_enrich_line.Deep_copy();
                        new_missing_completeSampleName.Integration_group = (string)all_integrationGroups_list[indexAllCompleteSampleNames].Clone();
                        new_missing_completeSampleName.Sample_name = (string)all_sampleNames_list[indexAllCompleteSampleNames].Clone();
                        new_missing_completeSampleName.Timepoint = all_timepoints_list[indexAllCompleteSampleNames];
                        new_missing_completeSampleName.EntryType = all_entryTypes_list[indexAllCompleteSampleNames];
                        new_missing_completeSampleName.Sample_color = all_colors_list[indexAllCompleteSampleNames];
                        new_missing_completeSampleName.Results_number = all_resultNos_list[indexAllCompleteSampleNames];
                        new_missing_completeSampleName.Unique_dataset_name = (string)all_uniqueDatasetNames_list[indexAllCompleteSampleNames].Clone();
                        new_missing_completeSampleName.Pvalue = 1;
                        new_missing_completeSampleName.Minus_log10_pvalue = 0;
                        new_missing_completeSampleName.Overlap_symbols = new string[0];
                        new_missing_completeSampleName.Fractional_rank = 999999;
                        new_missing_completeSampleName.Significant = false;
                        missing_completeSampleNames.Add(new_missing_completeSampleName);
                        indexAllCompleteSampleNames++;
                    }
                }
            }
            Add_other_lines_without_resetting_unique_datasetNames(missing_completeSampleNames.ToArray());
        }
        #endregion

        #region Replace scp names
        public void Replace_oldSCPname_by_newSCPName(Dictionary<string,string> oldScp_to_newScp_dict)
        {
            Dictionary<string, string> newScp_from_oldScp_dict = Dictionary_class.Reverse_dictionary(oldScp_to_newScp_dict);
            int enrich_length = this.Enrich.Length;
            Ontology_enrichment_line_class enrichment_line;
            string[] scps;
            string scp;
            int scps_length;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int indexE=0; indexE<enrich_length; indexE++)
            {
                enrichment_line = this.Enrich[indexE];
                scps = enrichment_line.Scp_name.Split('$');
                scps_length = scps.Length;
                sb.Clear();
                for (int indexScp=0; indexScp<scps_length; indexScp++)
                {
                    scp = scps[indexScp];
                    if (newScp_from_oldScp_dict.ContainsKey(scp)) { throw new Exception(); }
                    if (oldScp_to_newScp_dict.ContainsKey(scp))
                    {
                        scp = (string)oldScp_to_newScp_dict[scp].Clone();
                    }
                    if (indexScp!=0) { sb.AppendFormat("$"); }
                    sb.AppendFormat(scp);
                }
                enrichment_line.Scp_name = sb.ToString();
                if (newScp_from_oldScp_dict.ContainsKey(enrichment_line.Parent_scp_name)) { throw new Exception(); }
                if (oldScp_to_newScp_dict.ContainsKey(enrichment_line.Parent_scp_name))
                {
                    enrichment_line.Parent_scp_name = (string)oldScp_to_newScp_dict[enrichment_line.Parent_scp_name].Clone();
                }
            }
        }
        #endregion

        #region Dynamic enrichment analysis
        public void Separate_scp_unions_into_single_scps_and_keep_line_defined_by_lowest_pvalue_for_each_scp_and_add_scp_specific_genes(Ontology_enrichment_class standard_unfiltered)
        {
            if (this.Enrich.Length > 0)
            {
                string integrationGroup = this.Enrich[0].Integration_group;
                int enrich_length = this.Enrich.Length;
                Ontology_enrichment_line_class onto_enrich_line;
                Ontology_enrichment_line_class singleScp_onto_enrich_line;
                List<Ontology_enrichment_line_class> onto_enrich_keep = new List<Ontology_enrichment_line_class>();
                Dictionary<string, bool> considered_scps_of_current_condition = new Dictionary<string, bool>();
                this.Enrich = this.Enrich.OrderBy(l => l.EntryType).ThenBy(l => l.TimepointInDays).ThenBy(l => l.Sample_name).ThenBy(l => l.Pvalue).ToArray();
                string[] scps;
                string scp;
                int scps_length;
                for (int indexO = 0; indexO < enrich_length; indexO++)
                {
                    onto_enrich_line = this.Enrich[indexO];
                    if (!integrationGroup.Equals(onto_enrich_line.Integration_group)) { throw new Exception(); }
                    if ((indexO == 0)
                        || (!onto_enrich_line.EntryType.Equals(this.Enrich[indexO - 1].EntryType))
                        || (!onto_enrich_line.TimepointInDays.Equals(this.Enrich[indexO - 1].TimepointInDays))
                        || (!onto_enrich_line.Sample_name.Equals(this.Enrich[indexO - 1].Sample_name)))
                    {
                        considered_scps_of_current_condition.Clear();
                    }
                    scps = onto_enrich_line.Scp_name.Split('$');
                    scps_length = scps.Length;
                    for (int indexScp = 0; indexScp < scps_length; indexScp++)
                    {
                        scp = scps[indexScp];
                        if (!considered_scps_of_current_condition.ContainsKey(scp))
                        {
                            considered_scps_of_current_condition.Add(scp, true);
                            singleScp_onto_enrich_line = onto_enrich_line.Deep_copy();
                            singleScp_onto_enrich_line.Scp_name = (string)scp.Clone();
                            singleScp_onto_enrich_line.Scp_id = "broken up dynamic results from " + onto_enrich_line.Scp_name;
                            singleScp_onto_enrich_line.Overlap_symbols = new string[0];
                            onto_enrich_keep.Add(singleScp_onto_enrich_line);
                        }
                    }
                }
                //   if (onto_enrich_keep.Count<=this.Enrich.Length) { throw new Exception(); }
                this.Enrich = onto_enrich_keep.ToArray();

                this.Enrich = this.Enrich.OrderBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.Sample_name).ThenBy(l => l.Scp_name).ToArray();
                standard_unfiltered.Enrich = standard_unfiltered.Enrich.OrderBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.Sample_name).ThenBy(l => l.Scp_name).ToArray();

                int indexStandard = 0;
                int this_length = this.Enrich.Length;
                int standard_length = standard_unfiltered.Enrich.Length;
                Ontology_enrichment_line_class this_onto_enrich_line;
                Ontology_enrichment_line_class standard_onto_enrich_line = new Ontology_enrichment_line_class();
                int stringCompare;
                for (int indexThis = 0; indexThis < this_length; indexThis++)
                {
                    this_onto_enrich_line = this.Enrich[indexThis];
                    stringCompare = -2;
                    while ((indexStandard < standard_length) && (stringCompare < 0))
                    {
                        standard_onto_enrich_line = standard_unfiltered.Enrich[indexStandard];
                        stringCompare = standard_onto_enrich_line.EntryType.CompareTo(this_onto_enrich_line.EntryType);
                        if (stringCompare == 0)
                        {
                            stringCompare = standard_onto_enrich_line.Timepoint.CompareTo(this_onto_enrich_line.Timepoint);
                        }
                        if (stringCompare == 0)
                        {
                            stringCompare = standard_onto_enrich_line.Sample_name.CompareTo(this_onto_enrich_line.Sample_name);
                        }
                        if (stringCompare == 0)
                        {
                            stringCompare = standard_onto_enrich_line.Scp_name.CompareTo(this_onto_enrich_line.Scp_name);
                        }
                        if (stringCompare < 0) { indexStandard++; }
                    }
                    if (stringCompare != 0) { throw new Exception(); }
                    this_onto_enrich_line.Overlap_symbols = Array_class.Deep_copy_string_array(standard_onto_enrich_line.Overlap_symbols);
                }
            }
        }
        #endregion

        #region Rank
        public void Calculate_fractional_ranks_for_samples_based_on_pvalue_for_each_scp_entryType()
        {
            this.Enrich = this.Enrich.OrderBy(l => l.Scp_name).ThenBy(l => l.EntryType).ThenBy(l => l.Pvalue).ToArray();
            int enrich_length = this.Enrich.Length;
            Ontology_enrichment_line_class enrich_line;
            Ontology_enrichment_line_class inner_enrich_line;
            int ordinal_rank = 0;
            int firstIndexSamePvalue = -1;
            List<float> current_ordinal_ranks = new List<float>();
            float fractional_rank;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                if ((indexE == 0)
                    || (!enrich_line.Scp_name.Equals(this.Enrich[indexE - 1].Scp_name))
                    || (!enrich_line.EntryType.Equals(this.Enrich[indexE - 1].EntryType)))
                {
                    ordinal_rank = 0;
                }
                if ((indexE == 0)
                    || (!enrich_line.Scp_name.Equals(this.Enrich[indexE - 1].Scp_name))
                    || (!enrich_line.EntryType.Equals(this.Enrich[indexE - 1].EntryType))
                    || (!enrich_line.Pvalue.Equals(this.Enrich[indexE - 1].Pvalue)))
                {
                    current_ordinal_ranks.Clear();
                    firstIndexSamePvalue = indexE;
                }
                ordinal_rank++;
                current_ordinal_ranks.Add(ordinal_rank);
                if ((indexE == enrich_length-1)
                    || (!enrich_line.Scp_name.Equals(this.Enrich[indexE + 1].Scp_name))
                    || (!enrich_line.EntryType.Equals(this.Enrich[indexE + 1].EntryType))
                    || (!enrich_line.Pvalue.Equals(this.Enrich[indexE + 1].Pvalue)))
                {
                    fractional_rank = Math_class.Get_average(current_ordinal_ranks.ToArray());
                    for (int indexInner = firstIndexSamePvalue; indexInner <= indexE; indexInner++)
                    {
                        inner_enrich_line = this.Enrich[indexInner];
                        inner_enrich_line.Fractional_rank = fractional_rank;
                    }
                }
            }
        }

        public void Calculate_fractional_ranks_for_SCPs_within_each_integrationGroup_sampleName_timepoint_timeunit_entryType_processLevel()
        {
            this.Enrich = this.Enrich.OrderBy(l=>l.Integration_group).ThenBy(l => l.Sample_name).ThenBy(l => l.EntryType).ThenBy(l=>l.Timepoint).ThenBy(l=>l.Timeunit).ThenBy(l=>l.ProcessLevel).ThenBy(l => l.Pvalue).ToArray();
            int enrich_length = this.Enrich.Length;
            Ontology_enrichment_line_class enrich_line;
            Ontology_enrichment_line_class inner_enrich_line;
            int ordinal_rank = 0;
            int firstIndexSamePvalue = -1;
            List<float> current_ordinal_ranks = new List<float>();
            float fractional_rank;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                if ((indexE == 0)
                    || (!enrich_line.Integration_group.Equals(this.Enrich[indexE - 1].Integration_group))
                    || (!enrich_line.Sample_name.Equals(this.Enrich[indexE - 1].Sample_name))
                    || (!enrich_line.Timepoint.Equals(this.Enrich[indexE - 1].Timepoint))
                    || (!enrich_line.Timeunit.Equals(this.Enrich[indexE - 1].Timeunit))
                    || (!enrich_line.EntryType.Equals(this.Enrich[indexE - 1].EntryType))
                    || (!enrich_line.ProcessLevel.Equals(this.Enrich[indexE - 1].ProcessLevel)))
                {
                    ordinal_rank = 0;
                }
                if ((indexE == 0)
                    || (!enrich_line.Integration_group.Equals(this.Enrich[indexE - 1].Integration_group))
                    || (!enrich_line.Sample_name.Equals(this.Enrich[indexE - 1].Sample_name))
                    || (!enrich_line.Timepoint.Equals(this.Enrich[indexE - 1].Timepoint))
                    || (!enrich_line.Timeunit.Equals(this.Enrich[indexE - 1].Timeunit))
                    || (!enrich_line.EntryType.Equals(this.Enrich[indexE - 1].EntryType))
                    || (!enrich_line.ProcessLevel.Equals(this.Enrich[indexE - 1].ProcessLevel))
                    || (!enrich_line.Pvalue.Equals(this.Enrich[indexE - 1].Pvalue)))
                {
                    current_ordinal_ranks.Clear();
                    firstIndexSamePvalue = indexE;
                }
                ordinal_rank++;
                current_ordinal_ranks.Add(ordinal_rank);
                if ((indexE == enrich_length-1)
                    || (!enrich_line.Integration_group.Equals(this.Enrich[indexE + 1].Integration_group))
                    || (!enrich_line.Sample_name.Equals(this.Enrich[indexE + 1].Sample_name))
                    || (!enrich_line.Timepoint.Equals(this.Enrich[indexE + 1].Timepoint))
                    || (!enrich_line.Timeunit.Equals(this.Enrich[indexE + 1].Timeunit))
                    || (!enrich_line.EntryType.Equals(this.Enrich[indexE + 1].EntryType))
                    || (!enrich_line.ProcessLevel.Equals(this.Enrich[indexE + 1].ProcessLevel))
                    || (!enrich_line.Pvalue.Equals(this.Enrich[indexE + 1].Pvalue)))
                {
                    if (current_ordinal_ranks.Count>1)
                    {
                        fractional_rank = Math_class.Get_average(current_ordinal_ranks.ToArray());
                        for (int indexInner=firstIndexSamePvalue; indexInner<=indexE;indexInner++)
                        {
                            inner_enrich_line = this.Enrich[indexInner];
                            inner_enrich_line.Fractional_rank = fractional_rank;
                        }
                    }
                    else if (current_ordinal_ranks.Count==1)
                    {
                        if (firstIndexSamePvalue!=indexE) { throw new Exception(); }
                        enrich_line.Fractional_rank = current_ordinal_ranks[0];
                    }
                    else { throw new Exception(); }
                }
            }
        }

        #endregion

        #region Write copy
        public void Write_and_continue_trying_until_file_free(string subdirectory, string file_name, System.Windows.Forms.Label reportLabel, Form1_default_settings_class form_default_settings)
        {
            bool writeIntegrationGroups = Get_all_integrationGroups().Length > 1;
            bool writeEntryType = Get_all_entryTypes().Length > 1;
            bool writeTimepoint = Get_all_timepointsInDays().Length > 1;
            this.Enrich = this.Enrich.OrderBy(l => l.Sample_name).ThenBy(l => l.Timepoint).ThenBy(l => l.ProcessLevel).ThenBy(l => l.EntryType).ThenBy(l => l.Pvalue).ToArray();
            Ontology_enrich_readWriteOptions_class readWriteOptions = new Ontology_enrich_readWriteOptions_class(subdirectory, file_name, writeIntegrationGroups, writeEntryType,writeTimepoint);
            ReadWriteClass.WriteData(Enrich, readWriteOptions, reportLabel, form_default_settings);
        }

        public Ontology_enrichment_class Deep_copy()
        {
            Ontology_enrichment_class copy = (Ontology_enrichment_class)this.MemberwiseClone();
            int enrich_length = this.Enrich.Length;
            copy.Enrich = new Ontology_enrichment_line_class[enrich_length];
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                copy.Enrich[indexE] = this.Enrich[indexE].Deep_copy();
            }
            return copy;
        }
        #endregion
    }
}
