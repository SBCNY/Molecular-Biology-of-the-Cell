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
using Common_functions.Global_definitions;
using Common_functions.ReadWrite;

namespace Enrichment
{
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
        public float Minus_log10_pvalue { get; set; }
        public double Pvalue { get; set; }
        public double Qvalue { get; set; }
        public double FDR { get; set; }
        public int Overlap_count { get; set; }
        public int Process_symbols_count { get; set; }
        public int Experimental_symbols_count { get; set; }
        public int Bg_symbol_count { get; set; }
        public string[] Overlap_symbols { get; set; }
        public string ReadWrite_overlap_symbols
        {
            get { return ReadWriteClass.Get_writeLine_from_array(Overlap_symbols, Ontology_enrich_readWriteOptions_class.Delimiter); }
            set { Overlap_symbols = ReadWriteClass.Get_array_from_readLine<string>(value, Ontology_enrich_readWriteOptions_class.Delimiter); }
        }
        #endregion

        #region Fields for sample
        public int Timepoint { get; set; }
        public Entry_type_enum EntryType { get; set; }
        public string Sample_name { get; set; }
        public string Complete_sample_name { get { return Global_class.Get_complete_sampleName(EntryType, Timepoint, Sample_name); } }
        #endregion

        public Ontology_enrichment_line_class()
        {
            Scp_id = Global_class.Empty_entry;
            Scp_name = Global_class.Empty_entry;
        }

        #region Standard way
        public static Ontology_enrichment_line_class[] Order_by_sample_and_scpName(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            onto_enrich_array = onto_enrich_array.OrderBy(l => l.Ontology_type).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.Sample_name).ThenBy(l => l.Scp_name).ToArray();
            return onto_enrich_array;
        }

        public static Ontology_enrichment_line_class[] Order_by_complete_sample_pvalue(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            onto_enrich_array = onto_enrich_array.OrderBy(l => l.Ontology_type).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.Sample_name).ThenBy(l => l.Pvalue).ToArray();
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
            if (!String.IsNullOrEmpty(copy.Sample_name)) { copy.Sample_name = (string)this.Sample_name.Clone(); }
            return copy;
        }
    }

    class Ontology_enrich_readWriteOptions_class : ReadWriteOptions_base
    {
        public static char Delimiter { get {return ',';}}
        
        public Ontology_enrich_readWriteOptions_class(string subdirectory, string fileName)
        {
            string directory = Global_directory_and_file_class.Results_directory + subdirectory;
            ReadWriteClass.Create_directory_if_it_does_not_exist(directory);
            this.File = directory + fileName;

            Key_propertyNames = new string[] { "Ontology_type", "ProcessLevel", "EntryType", "Timepoint","Sample_name", "Scp_id", "Scp_name", "Minus_log10_pvalue", "Pvalue", "FDR", "ReadWrite_overlap_symbols" };
            Key_columnNames = new string[] { "Ontology_type", "ProcessLevel", "EntryType", "Timepoint", "Sample_name", "Scp_id", "Scp_name", "Minus_log10_pvalue", "Pvalue", "FDR", "Overlap_symbols" };
            Key_columnNames = Key_propertyNames;
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

        #region Order
        public void Order_by_processLevel()
        {
            Enrich = Enrich.OrderBy(l => l.ProcessLevel).ToArray();
        }

        public void Order_by_scpName()
        {
            Enrich = Enrich.OrderBy(l => l.Scp_name).ToArray();
        }

        public void Order_by_sample_scpName()
        {
            Enrich = Ontology_enrichment_line_class.Order_by_sample_and_scpName(this.Enrich);
        }

        public void Order_by_complete_sample_pvalue()
        {
            Enrich = Ontology_enrichment_line_class.Order_by_complete_sample_pvalue(this.Enrich);
        }
        #endregion

        #region Filter
        public void Keep_enrichment_lines_below_pvalue_cutoff(float pvalue_cutoff)
        {
            List<Ontology_enrichment_line_class> keep_enrichment_lines = new List<Ontology_enrichment_line_class>();
            int enrich_length = this.Enrich.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                onto_enrich_line = this.Enrich[indexE];
                if (onto_enrich_line.Pvalue <= pvalue_cutoff)
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

        public void Keep_top_x_predictions_per_level_for_each_sample(int[] top_x_lines_per_level)
        {
            int enrich_length = this.Enrich.Length;
            Ontology_enrichment_line_class enrichment_line;
            List<Ontology_enrichment_line_class> keep_onto_list = new List<Ontology_enrichment_line_class>();
            this.Enrich = this.Enrich.OrderBy(l=>l.Ontology_type).ThenBy(l=>l.EntryType).ThenBy(l=>l.Timepoint).ThenBy(l=>l.Sample_name).ThenBy(l=>l.ProcessLevel).ThenByDescending(l => l.Minus_log10_pvalue).ToArray();
            int kept_lines_count = 0;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrichment_line = this.Enrich[indexE];
                if (!enrichment_line.Ontology_type.Equals(Ontology_type_enum.Molecular_biology_cell))
                {
                    throw new Exception();
                }
                if (  (indexE == 0)
                    || (!enrichment_line.Ontology_type.Equals(this.Enrich[indexE - 1].Ontology_type))
                    || (!enrichment_line.EntryType.Equals(this.Enrich[indexE - 1].EntryType))
                    || (!enrichment_line.Timepoint.Equals(this.Enrich[indexE - 1].Timepoint))
                    ||(!enrichment_line.Sample_name.Equals(this.Enrich[indexE - 1].Sample_name))
                    ||(!enrichment_line.ProcessLevel.Equals(this.Enrich[indexE - 1].ProcessLevel)))
                {
                    kept_lines_count = 0;
                }
                if (kept_lines_count < top_x_lines_per_level[enrichment_line.ProcessLevel])
                {
                    kept_lines_count++;
                    keep_onto_list.Add(enrichment_line);
                }
            }
            this.Enrich = keep_onto_list.ToArray();
        }

         public void Keep_top_x_predictedSCPs_as_part_of_SCPunit_or_singleSCPs(int[] keep_singleSCPs_per_level)
        {
            int enrich_length = this.Enrich.Length;
            Ontology_enrichment_line_class enrichment_line;
            List<Ontology_enrichment_line_class> keep_onto_list = new List<Ontology_enrichment_line_class>();
            this.Enrich = this.Enrich.OrderBy(l => l.Ontology_type).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.Sample_name).ThenBy(l => l.ProcessLevel).ThenByDescending(l => l.Minus_log10_pvalue).ToArray();
            int kept_scps_count = 0;
            string[] single_scps;
            List<string> kept_scps_list = new List<string>();
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrichment_line = this.Enrich[indexE];
                if (!enrichment_line.Ontology_type.Equals(Ontology_type_enum.Molecular_biology_cell))
                {
                    throw new Exception();
                }
                if ((indexE == 0)
                    || (!enrichment_line.Ontology_type.Equals(this.Enrich[indexE - 1].Ontology_type))
                    || (!enrichment_line.EntryType.Equals(this.Enrich[indexE - 1].EntryType))
                    || (!enrichment_line.Timepoint.Equals(this.Enrich[indexE - 1].Timepoint))
                    || (!enrichment_line.Sample_name.Equals(this.Enrich[indexE - 1].Sample_name))
                    || (!enrichment_line.ProcessLevel.Equals(this.Enrich[indexE - 1].ProcessLevel)))
                {
                    kept_scps_count = 0;
                }
                if (kept_scps_count < keep_singleSCPs_per_level[enrichment_line.ProcessLevel])
                {
                    single_scps = enrichment_line.Scp_name.Split(Global_class.Scp_delimiter);
                    kept_scps_list.AddRange(single_scps);
                    kept_scps_list = kept_scps_list.Distinct().ToList();
                    if (kept_scps_list.Count <= keep_singleSCPs_per_level[enrichment_line.ProcessLevel])
                    {
                        keep_onto_list.Add(enrichment_line);
                    }
                }
            }
            this.Enrich = keep_onto_list.ToArray();
        }
        #endregion

        #region Get
        public string[] Get_all_scps_of_completeSample(string completeSampleName)
        {
            int enrich_length = this.Enrich.Length;
            List<string> scpNames = new List<string>();
            Ontology_enrichment_line_class enrich_line;
            for (int indexE=0; indexE<enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                if (enrich_line.Complete_sample_name.Equals(completeSampleName))
                {
                    scpNames.Add(enrich_line.Scp_name);
                }
            }
            if (scpNames.Distinct().ToArray().Length!=scpNames.Count)
            {
                throw new Exception("if only one sample, double entries should not exist");
            }
            return scpNames.OrderBy(l => l).ToArray();
        }
        #endregion

        #region Add
        public void Add_other_lines(Ontology_enrichment_line_class[] other_lines)
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
            Add_other_lines(other.Enrich);
        }
        #endregion

        #region Write copy
        public void Write(string subdirectory, string file_name)
        {
            Ontology_enrich_readWriteOptions_class readWriteOptions = new Ontology_enrich_readWriteOptions_class(subdirectory, file_name);
            ReadWriteClass.WriteData(Enrich, readWriteOptions);
        }

        public Ontology_enrichment_class Deep_copy()
        {
            Ontology_enrichment_class copy = (Ontology_enrichment_class)this.MemberwiseClone();
            Ontology_enrichment_line_class onto_enrich_line;
            int enrich_length = this.Enrich.Length;
            copy.Enrich = new Ontology_enrichment_line_class[enrich_length];
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                onto_enrich_line = this.Enrich[indexE];
                copy.Enrich[indexE] = this.Enrich[indexE].Deep_copy();
            }
            return copy;
        }
        #endregion
    }
}
