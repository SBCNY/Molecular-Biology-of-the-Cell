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
using Windows_forms;
using yed_network;
using ClassLibrary1.eUtils;
using System.Xml;
using System.Web;

namespace Enrichment
{
    class Ontology_enrichment_condition_line_class
    {
        public Ontology_type_enum Ontology { get; set; }
        public float TimepointInDays { get; set; }
        public Entry_type_enum EntryType { get; set; }
        public string SampleName { get; set; }
        public string IntegrationGroup { get; set; }
        public int ProcessLevel { get; set; }

        public Ontology_enrichment_condition_line_class Deep_copy()
        {
            Ontology_enrichment_condition_line_class copy = (Ontology_enrichment_condition_line_class)this.MemberwiseClone();
            copy.SampleName = (string)this.SampleName.Clone();
            copy.IntegrationGroup = (string)this.IntegrationGroup.Clone();
            return copy;
        }
    }

    class Ontology_enrichment_line_class : ISet_uniqueDatasetName_line
    {

        #region Fields for MBCO
        public string Scp_id { get; set; }
        public string Scp_name { get; set; }
        public string Parent_scp_name { get; set; }
        public Ontology_type_enum Ontology_type { get; set; }
        public Organism_enum Organism { get; set; }
        public int ProcessLevel { get; set; }
        public int ProcessDepth { get; set; }
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
        public string SampleName { get; set; }
        public string Complete_sampleName { get { return Global_class.Get_complete_sampleName(IntegrationGroup, EntryType, Timepoint, Timeunit, SampleName); } }
        public string Unique_dataset_name { get; set; }
        public string IntegrationGroup { get; set; }
        public Color Sample_color { get; set; }
        public string Sample_color_string { get { return Color_conversion_class.Get_color_string(Sample_color); } set { Sample_color = Color_conversion_class.Set_color_from_string(value); } }
        public int Results_number { get; set; }
        #endregion

        public Ontology_enrichment_line_class()
        {
            Scp_id = Global_class.Empty_entry;
            Scp_name = Global_class.Empty_entry;
            Parent_scp_name = Global_class.Empty_entry;
            SampleName = Global_class.Empty_entry;
            IntegrationGroup = Global_class.Empty_entry;
            Overlap_symbols = new string[0];
            Unique_dataset_name = "";
            Pvalue = -1;
        }

        public bool Is_mbco_ontology()
        {
            return Ontology_classification_class.Is_mbco_ontology(this.Ontology_type);
        }

        #region Standard way
        public static Ontology_enrichment_line_class[] Order_by_processLevel(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<int, List<Ontology_enrichment_line_class>> level_dict = new Dictionary<int, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!level_dict.ContainsKey(onto_enrich_line.ProcessLevel))
                {
                    level_dict.Add(onto_enrich_line.ProcessLevel, new List<Ontology_enrichment_line_class>());
                }
                level_dict[onto_enrich_line.ProcessLevel].Add(onto_enrich_line);
            }
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            onto_enrich_array = null;
            int[] levels;
            int level;
            int levels_length;
            levels = level_dict.Keys.ToArray();
            levels = levels.OrderBy(l => l).ToArray();
            levels_length = levels.Length;
            for (int indexL = 0; indexL < levels_length; indexL++)
            {
                level = levels[indexL];
                ordered_lines.AddRange(level_dict[level]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.ProcessLevel.CompareTo(previous_line.ProcessLevel) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_resultsNo(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<int, List<Ontology_enrichment_line_class>> resultNo_dict = new Dictionary<int, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!resultNo_dict.ContainsKey(onto_enrich_line.Results_number))
                {
                    resultNo_dict.Add(onto_enrich_line.Results_number, new List<Ontology_enrichment_line_class>());
                }
                resultNo_dict[onto_enrich_line.Results_number].Add(onto_enrich_line);
            }
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            onto_enrich_array = null;
            int[] resultNos;
            int resultNo;
            int resultNos_length;
            resultNos = resultNo_dict.Keys.ToArray();
            resultNos = resultNos.OrderBy(l => l).ToArray();
            resultNos_length = resultNos.Length;
            for (int indexRN = 0; indexRN < resultNos_length; indexRN++)
            {
                resultNo = resultNos[indexRN];
                ordered_lines.AddRange(resultNo_dict[resultNo]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.Results_number.CompareTo(previous_line.Results_number) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_timepointInDays(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<float, List<Ontology_enrichment_line_class>> timepointInDays_dict = new Dictionary<float, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepointInDays;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepointInDays = onto_enrich_line.TimepointInDays;
                if (!timepointInDays_dict.ContainsKey(timepointInDays))
                {
                    timepointInDays_dict.Add(timepointInDays, new List<Ontology_enrichment_line_class>());
                }
                timepointInDays_dict[timepointInDays].Add(onto_enrich_line);
            }
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            onto_enrich_array = null;
            float[] timepointsInDays;
            float timepointsInDays_length;
            timepointsInDays = timepointInDays_dict.Keys.ToArray();
            timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
            timepointsInDays_length = timepointsInDays.Length;
            for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
            {
                timepointInDays = timepointsInDays[indexT];
                ordered_lines.AddRange(timepointInDays_dict[timepointInDays]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.TimepointInDays.CompareTo(previous_line.TimepointInDays) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_timepoint(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<float, List<Ontology_enrichment_line_class>> timepoint_dict = new Dictionary<float, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!timepoint_dict.ContainsKey(onto_enrich_line.Timepoint))
                {
                    timepoint_dict.Add(onto_enrich_line.Timepoint, new List<Ontology_enrichment_line_class>());
                }
                timepoint_dict[onto_enrich_line.Timepoint].Add(onto_enrich_line);
            }
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            onto_enrich_array = null;
            float[] timepoints;
            float timepoint;
            float timepoints_length;
            timepoints = timepoint_dict.Keys.ToArray();
            timepoints = timepoints.OrderBy(l => l).ToArray();
            timepoints_length = timepoints.Length;
            for (int indexT = 0; indexT < timepoints_length; indexT++)
            {
                timepoint = timepoints[indexT];
                ordered_lines.AddRange(timepoint_dict[timepoint]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.Timepoint.CompareTo(previous_line.Timepoint) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_descendingMinusLog10Pvalue(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<float, List<Ontology_enrichment_line_class>> minusLog10Pvalue_dict = new Dictionary<float, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!minusLog10Pvalue_dict.ContainsKey(onto_enrich_line.Minus_log10_pvalue))
                {
                    minusLog10Pvalue_dict.Add(onto_enrich_line.Minus_log10_pvalue, new List<Ontology_enrichment_line_class>());
                }
                minusLog10Pvalue_dict[onto_enrich_line.Minus_log10_pvalue].Add(onto_enrich_line);
            }
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            onto_enrich_array = null;
            float[] minusLog10Pvalues;
            float minusLog10Pvalue;
            int minusLog10Pvalues_length;
            minusLog10Pvalues = minusLog10Pvalue_dict.Keys.ToArray();
            minusLog10Pvalues = minusLog10Pvalues.OrderByDescending(l => l).ToArray();
            minusLog10Pvalues_length = minusLog10Pvalues.Length;
            for (int indexM = 0; indexM < minusLog10Pvalues_length; indexM++)
            {
                minusLog10Pvalue = minusLog10Pvalues[indexM];
                ordered_lines.AddRange(minusLog10Pvalue_dict[minusLog10Pvalue]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.Minus_log10_pvalue.CompareTo(previous_line.Minus_log10_pvalue) > 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_descendingProcessLevel(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<float, List<Ontology_enrichment_line_class>> processLevel_dict = new Dictionary<float, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!processLevel_dict.ContainsKey(onto_enrich_line.ProcessLevel))
                {
                    processLevel_dict.Add(onto_enrich_line.ProcessLevel, new List<Ontology_enrichment_line_class>());
                }
                processLevel_dict[onto_enrich_line.ProcessLevel].Add(onto_enrich_line);
            }
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            onto_enrich_array = null;
            float[] processLevels;
            float processLevel;
            int processLevels_length;
            processLevels = processLevel_dict.Keys.ToArray();
            processLevels = processLevels.OrderByDescending(l => l).ToArray();
            processLevels_length = processLevels.Length;
            for (int indexM = 0; indexM < processLevels_length; indexM++)
            {
                processLevel = processLevels[indexM];
                ordered_lines.AddRange(processLevel_dict[processLevel]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.ProcessLevel.CompareTo(previous_line.ProcessLevel) > 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_uniqueDatasetName_descendingMinusLog10Pvalue_scpName(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            //this.Enrich = this.Enrich.OrderBy(l => l.Unique_dataset_name).ThenByDescending(l => l.Minus_log10_pvalue).ThenBy(l => l.Scp_name).ToArray();
            Dictionary<string, Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>> uniqueDatasetName_minusLog10Pvalue_scpName_dict = new Dictionary<string, Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>>();
            Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>> minusLog10Pvalue_scpName_dict = new Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>();
            Dictionary<string, List<Ontology_enrichment_line_class>> scpName_dict = new Dictionary<string, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepoint_in_days;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepoint_in_days = onto_enrich_line.TimepointInDays;
                if (!uniqueDatasetName_minusLog10Pvalue_scpName_dict.ContainsKey(onto_enrich_line.Unique_dataset_name))
                {
                    uniqueDatasetName_minusLog10Pvalue_scpName_dict.Add(onto_enrich_line.Unique_dataset_name, new Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>());
                }
                if (!uniqueDatasetName_minusLog10Pvalue_scpName_dict[onto_enrich_line.Unique_dataset_name].ContainsKey(onto_enrich_line.Minus_log10_pvalue))
                {
                    uniqueDatasetName_minusLog10Pvalue_scpName_dict[onto_enrich_line.Unique_dataset_name].Add(onto_enrich_line.Minus_log10_pvalue, new Dictionary<string, List<Ontology_enrichment_line_class>>());
                }
                if (!uniqueDatasetName_minusLog10Pvalue_scpName_dict[onto_enrich_line.Unique_dataset_name][onto_enrich_line.Minus_log10_pvalue].ContainsKey(onto_enrich_line.Scp_name))
                {
                    uniqueDatasetName_minusLog10Pvalue_scpName_dict[onto_enrich_line.Unique_dataset_name][onto_enrich_line.Minus_log10_pvalue].Add(onto_enrich_line.Scp_name, new List<Ontology_enrichment_line_class>());
                }
                uniqueDatasetName_minusLog10Pvalue_scpName_dict[onto_enrich_line.Unique_dataset_name][onto_enrich_line.Minus_log10_pvalue][onto_enrich_line.Scp_name].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            string[] uniqueDatasetNames;
            string uniqueDatasetName;
            int uniqueDatasetNames_length;
            float[] minusLog10Pvalues;
            float minusLog10Pvalue;
            int minusLog10Pvalues_length;
            string[] scpNames;
            string scpName;
            int scpNames_length;
            uniqueDatasetNames = uniqueDatasetName_minusLog10Pvalue_scpName_dict.Keys.ToArray();
            uniqueDatasetNames = uniqueDatasetNames.OrderBy(l => l).ToArray();
            uniqueDatasetNames_length = uniqueDatasetNames.Length;
            for (int indexUID = 0; indexUID < uniqueDatasetNames_length; indexUID++)
            {
                uniqueDatasetName = uniqueDatasetNames[indexUID];
                minusLog10Pvalue_scpName_dict = uniqueDatasetName_minusLog10Pvalue_scpName_dict[uniqueDatasetName];
                minusLog10Pvalues = minusLog10Pvalue_scpName_dict.Keys.ToArray();
                minusLog10Pvalues = minusLog10Pvalues.OrderByDescending(l => l).ToArray();
                minusLog10Pvalues_length = minusLog10Pvalues.Length;
                for (int indexM = 0; indexM < minusLog10Pvalues_length; indexM++)
                {
                    minusLog10Pvalue = minusLog10Pvalues[indexM];
                    scpName_dict = minusLog10Pvalue_scpName_dict[minusLog10Pvalue];
                    scpNames = scpName_dict.Keys.ToArray();
                    scpNames = scpNames.OrderBy(l => l).ToArray();
                    scpNames_length = scpNames.Length;
                    for (int indexScp = 0; indexScp < scpNames_length; indexScp++)
                    {
                        scpName = scpNames[indexScp];
                        ordered_lines.AddRange(scpName_dict[scpName]);
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //uniqueDatasetName_descendingMinusLog10Pvalue_scpName
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    this_timepointInDays = this_line.TimepointInDays;
                    previous_timepointInDays = previous_line.TimepointInDays;
                    if ((this_line.Unique_dataset_name.CompareTo(previous_line.Unique_dataset_name) < 0)) { throw new Exception(); }
                    else if ((this_line.Unique_dataset_name.Equals(previous_line.Unique_dataset_name))
                             && (this_line.Minus_log10_pvalue.CompareTo(previous_line.Minus_log10_pvalue) > 0)) { throw new Exception(); }
                    else if ((this_line.Unique_dataset_name.Equals(previous_line.Unique_dataset_name))
                             && (this_line.Minus_log10_pvalue.Equals(previous_line.Minus_log10_pvalue))
                             && (this_line.Scp_name.CompareTo(previous_line.Scp_name) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();



        }
        public static Ontology_enrichment_line_class[] Order_by_processLevel_parentScp(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<int, Dictionary<string, List<Ontology_enrichment_line_class>>> processLevel_parentScp_dict = new Dictionary<int, Dictionary<string, List<Ontology_enrichment_line_class>>>();
            Dictionary<string, List<Ontology_enrichment_line_class>> parentScp_dict = new Dictionary<string, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!processLevel_parentScp_dict.ContainsKey(onto_enrich_line.ProcessLevel))
                {
                    processLevel_parentScp_dict.Add(onto_enrich_line.ProcessLevel, new Dictionary<string, List<Ontology_enrichment_line_class>>());
                }
                if (!processLevel_parentScp_dict[onto_enrich_line.ProcessLevel].ContainsKey(onto_enrich_line.Parent_scp_name))
                {
                    processLevel_parentScp_dict[onto_enrich_line.ProcessLevel].Add(onto_enrich_line.Parent_scp_name, new List<Ontology_enrichment_line_class>());
                }
                processLevel_parentScp_dict[onto_enrich_line.ProcessLevel][onto_enrich_line.Parent_scp_name].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            int[] processLevels;
            int processLevel;
            int processLevels_length;
            string[] parentScps;
            string parentScp;
            int parentScps_length;
            processLevels = processLevel_parentScp_dict.Keys.ToArray();
            processLevels = processLevels.OrderBy(l => l).ToArray();
            processLevels_length = processLevels.Length;
            for (int indexPL = 0; indexPL < processLevels_length; indexPL++)
            {
                processLevel = processLevels[indexPL];
                parentScp_dict = processLevel_parentScp_dict[processLevel];
                parentScps = parentScp_dict.Keys.ToArray();
                parentScps = parentScps.OrderBy(l => l).ToArray();
                parentScps_length = parentScps.Length;
                for (int indexPSCP = 0; indexPSCP < parentScps_length; indexPSCP++)
                {
                    parentScp = parentScps[indexPSCP];
                    ordered_lines.AddRange(parentScp_dict[parentScp]);
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class this_line;
                Ontology_enrichment_line_class previous_line;
                for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.ProcessLevel.CompareTo(previous_line.ProcessLevel) < 0) { throw new Exception(); }
                    else if ((this_line.ProcessLevel.Equals(previous_line.ProcessLevel))
                             && (this_line.Parent_scp_name.CompareTo(previous_line.Parent_scp_name) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_processLevel_scpName(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<int, Dictionary<string, List<Ontology_enrichment_line_class>>> processLevel_scpName_dict = new Dictionary<int, Dictionary<string, List<Ontology_enrichment_line_class>>>();
            Dictionary<string, List<Ontology_enrichment_line_class>> scpName_dict = new Dictionary<string, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!processLevel_scpName_dict.ContainsKey(onto_enrich_line.ProcessLevel))
                {
                    processLevel_scpName_dict.Add(onto_enrich_line.ProcessLevel, new Dictionary<string, List<Ontology_enrichment_line_class>>());
                }
                if (!processLevel_scpName_dict[onto_enrich_line.ProcessLevel].ContainsKey(onto_enrich_line.Scp_name))
                {
                    processLevel_scpName_dict[onto_enrich_line.ProcessLevel].Add(onto_enrich_line.Scp_name, new List<Ontology_enrichment_line_class>());
                }
                processLevel_scpName_dict[onto_enrich_line.ProcessLevel][onto_enrich_line.Scp_name].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            int[] processLevels;
            int processLevel;
            int processLevels_length;
            string[] scpNames;
            string scpName;
            int scpNames_length;
            processLevels = processLevel_scpName_dict.Keys.ToArray();
            processLevels = processLevels.OrderBy(l => l).ToArray();
            processLevels_length = processLevels.Length;
            for (int indexPL = 0; indexPL < processLevels_length; indexPL++)
            {
                processLevel = processLevels[indexPL];
                scpName_dict = processLevel_scpName_dict[processLevel];
                scpNames = scpName_dict.Keys.ToArray();
                scpNames = scpNames.OrderBy(l => l).ToArray();
                scpNames_length = scpNames.Length;
                for (int indexScp = 0; indexScp < scpNames_length; indexScp++)
                {
                    scpName = scpNames[indexScp];
                    ordered_lines.AddRange(scpName_dict[scpName]);
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class this_line;
                Ontology_enrichment_line_class previous_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.ProcessLevel.CompareTo(previous_line.ProcessLevel) < 0) { throw new Exception(); }
                    else if ((this_line.ProcessLevel.Equals(previous_line.ProcessLevel))
                             && (this_line.Scp_name.CompareTo(previous_line.Scp_name) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_integrationGroup_resultsNumber(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<string, Dictionary<int, List<Ontology_enrichment_line_class>>> integrationGroup_resultsNumber_dict = new Dictionary<string, Dictionary<int, List<Ontology_enrichment_line_class>>>();
            Dictionary<int, List<Ontology_enrichment_line_class>> resultNo_dict = new Dictionary<int, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!integrationGroup_resultsNumber_dict.ContainsKey(onto_enrich_line.IntegrationGroup))
                {
                    integrationGroup_resultsNumber_dict.Add(onto_enrich_line.IntegrationGroup, new Dictionary<int, List<Ontology_enrichment_line_class>>());
                }
                if (!integrationGroup_resultsNumber_dict[onto_enrich_line.IntegrationGroup].ContainsKey(onto_enrich_line.Results_number))
                {
                    integrationGroup_resultsNumber_dict[onto_enrich_line.IntegrationGroup].Add(onto_enrich_line.Results_number, new List<Ontology_enrichment_line_class>());
                }
                integrationGroup_resultsNumber_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.Results_number].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            int[] resultNumbers;
            int resultNumber;
            int resultNumbers_length;
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            integrationGroups = integrationGroup_resultsNumber_dict.Keys.ToArray();
            integrationGroups = integrationGroups.OrderBy(l => l).ToArray();
            integrationGroups_length = integrationGroups.Length;
            for (int indexIG = 0; indexIG < integrationGroups_length; indexIG++)
            {
                integrationGroup = integrationGroups[indexIG];
                resultNo_dict = integrationGroup_resultsNumber_dict[integrationGroup];
                resultNumbers = resultNo_dict.Keys.ToArray();
                resultNumbers = resultNumbers.OrderBy(l => l).ToArray();
                resultNumbers_length = resultNumbers.Length;
                for (int indexRNo = 0; indexRNo < resultNumbers_length; indexRNo++)
                {
                    resultNumber = resultNumbers[indexRNo];
                    ordered_lines.AddRange(resultNo_dict[resultNumber]);
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class this_line;
                Ontology_enrichment_line_class previous_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.IntegrationGroup.CompareTo(previous_line.IntegrationGroup) < 0) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.Results_number.CompareTo(previous_line.Results_number) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_integrationGroup_timepointInDays(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>> integrationGroup_timepointInDays_dict = new Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>();
            Dictionary<float, List<Ontology_enrichment_line_class>> timepointInDays_dict = new Dictionary<float, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepointInDays;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepointInDays = onto_enrich_line.TimepointInDays;
                if (!integrationGroup_timepointInDays_dict.ContainsKey(onto_enrich_line.IntegrationGroup))
                {
                    integrationGroup_timepointInDays_dict.Add(onto_enrich_line.IntegrationGroup, new Dictionary<float, List<Ontology_enrichment_line_class>>());
                }
                if (!integrationGroup_timepointInDays_dict[onto_enrich_line.IntegrationGroup].ContainsKey(timepointInDays))
                {
                    integrationGroup_timepointInDays_dict[onto_enrich_line.IntegrationGroup].Add(timepointInDays, new List<Ontology_enrichment_line_class>());
                }
                integrationGroup_timepointInDays_dict[onto_enrich_line.IntegrationGroup][timepointInDays].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            float[] timepointsInDays;
            int timepointsInDays_length;
            integrationGroups = integrationGroup_timepointInDays_dict.Keys.ToArray();
            integrationGroups = integrationGroups.OrderBy(l => l).ToArray();
            integrationGroups_length = integrationGroups.Length;
            for (int indexIG = 0; indexIG < integrationGroups_length; indexIG++)
            {
                integrationGroup = integrationGroups[indexIG];
                timepointInDays_dict = integrationGroup_timepointInDays_dict[integrationGroup];
                timepointsInDays = timepointInDays_dict.Keys.ToArray();
                timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
                timepointsInDays_length = timepointsInDays.Length;
                for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                {
                    timepointInDays = timepointsInDays[indexT];
                    ordered_lines.AddRange(timepointInDays_dict[timepointInDays]);
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class this_line;
                Ontology_enrichment_line_class previous_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.IntegrationGroup.CompareTo(previous_line.IntegrationGroup) < 0) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.TimepointInDays.CompareTo(previous_line.TimepointInDays) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_processLevel_descendingMinusLog10Pvalue(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            //Enrich = Enrich.OrderBy(l => l.ProcessLevel).ThenByDescending(l => l.Minus_log10_pvalue).ToArray();
            Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>> processLevel_minusLog10Pvalue_dict = new Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>();
            Dictionary<float, List<Ontology_enrichment_line_class>> minusLog10Pvalue_dict = new Dictionary<float, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!processLevel_minusLog10Pvalue_dict.ContainsKey(onto_enrich_line.ProcessLevel))
                {
                    processLevel_minusLog10Pvalue_dict.Add(onto_enrich_line.ProcessLevel, new Dictionary<float, List<Ontology_enrichment_line_class>>());
                }
                if (!processLevel_minusLog10Pvalue_dict[onto_enrich_line.ProcessLevel].ContainsKey(onto_enrich_line.Minus_log10_pvalue))
                {
                    processLevel_minusLog10Pvalue_dict[onto_enrich_line.ProcessLevel].Add(onto_enrich_line.Minus_log10_pvalue, new List<Ontology_enrichment_line_class>());
                }
                processLevel_minusLog10Pvalue_dict[onto_enrich_line.ProcessLevel][onto_enrich_line.Minus_log10_pvalue].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            int[] processLevels;
            int processLevel;
            int processLevels_length;
            float[] minusLog10Pvalues;
            float minusLog10Pvalue;
            int minusLog10Pvalues_length;
            processLevels = processLevel_minusLog10Pvalue_dict.Keys.ToArray();
            processLevels = processLevels.OrderBy(l => l).ToArray();
            processLevels_length = processLevels.Length;
            for (int indexPL = 0; indexPL < processLevels_length; indexPL++)
            {
                processLevel = processLevels[indexPL];
                minusLog10Pvalue_dict = processLevel_minusLog10Pvalue_dict[processLevel];
                minusLog10Pvalues = minusLog10Pvalue_dict.Keys.ToArray();
                minusLog10Pvalues = minusLog10Pvalues.OrderByDescending(l => l).ToArray();
                minusLog10Pvalues_length = minusLog10Pvalues.Length;
                for (int indexPSCP = 0; indexPSCP < minusLog10Pvalues_length; indexPSCP++)
                {
                    minusLog10Pvalue = minusLog10Pvalues[indexPSCP];
                    ordered_lines.AddRange(minusLog10Pvalue_dict[minusLog10Pvalue]);
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class this_line;
                Ontology_enrichment_line_class previous_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.ProcessLevel.CompareTo(previous_line.ProcessLevel) < 0) { throw new Exception(); }
                    else if ((this_line.ProcessLevel.Equals(previous_line.ProcessLevel))
                             && (this_line.Minus_log10_pvalue.CompareTo(previous_line.Minus_log10_pvalue) > 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();

        }
        public static Ontology_enrichment_line_class[] Order_by_descendingProcessLevel_minusLog10Pvalue(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>> processLevel_minusLog10Pvalue_dict = new Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>();
            Dictionary<float, List<Ontology_enrichment_line_class>> minusLog10Pvalue_dict = new Dictionary<float, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!processLevel_minusLog10Pvalue_dict.ContainsKey(onto_enrich_line.ProcessLevel))
                {
                    processLevel_minusLog10Pvalue_dict.Add(onto_enrich_line.ProcessLevel, new Dictionary<float, List<Ontology_enrichment_line_class>>());
                }
                if (!processLevel_minusLog10Pvalue_dict[onto_enrich_line.ProcessLevel].ContainsKey(onto_enrich_line.Minus_log10_pvalue))
                {
                    processLevel_minusLog10Pvalue_dict[onto_enrich_line.ProcessLevel].Add(onto_enrich_line.Minus_log10_pvalue, new List<Ontology_enrichment_line_class>());
                }
                processLevel_minusLog10Pvalue_dict[onto_enrich_line.ProcessLevel][onto_enrich_line.Minus_log10_pvalue].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            int[] processLevels;
            int processLevel;
            int processLevels_length;
            float[] minusLog10Pvalues;
            float minusLog10Pvalue;
            int minusLog10Pvalues_length;
            processLevels = processLevel_minusLog10Pvalue_dict.Keys.ToArray();
            processLevels = processLevels.OrderByDescending(l => l).ToArray();
            processLevels_length = processLevels.Length;
            for (int indexPL = 0; indexPL < processLevels_length; indexPL++)
            {
                processLevel = processLevels[indexPL];
                minusLog10Pvalue_dict = processLevel_minusLog10Pvalue_dict[processLevel];
                minusLog10Pvalues = minusLog10Pvalue_dict.Keys.ToArray();
                minusLog10Pvalues = minusLog10Pvalues.OrderBy(l => l).ToArray();
                minusLog10Pvalues_length = minusLog10Pvalues.Length;
                for (int indexPSCP = 0; indexPSCP < minusLog10Pvalues_length; indexPSCP++)
                {
                    minusLog10Pvalue = minusLog10Pvalues[indexPSCP];
                    ordered_lines.AddRange(minusLog10Pvalue_dict[minusLog10Pvalue]);
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class this_line;
                Ontology_enrichment_line_class previous_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.ProcessLevel.CompareTo(previous_line.ProcessLevel) > 0) { throw new Exception(); }
                    else if ((this_line.ProcessLevel.Equals(previous_line.ProcessLevel))
                             && (this_line.Minus_log10_pvalue.CompareTo(previous_line.Minus_log10_pvalue) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_scpName(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<string, List<Ontology_enrichment_line_class>> scpName_dict = new Dictionary<string, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!scpName_dict.ContainsKey(onto_enrich_line.Scp_name))
                {
                    scpName_dict.Add(onto_enrich_line.Scp_name, new List<Ontology_enrichment_line_class>());
                }
                scpName_dict[onto_enrich_line.Scp_name].Add(onto_enrich_line);
            }
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            onto_enrich_array = null;
            string[] scpNames;
            string scpName;
            int scpNames_length;
            scpNames = scpName_dict.Keys.ToArray();
            scpNames = scpNames.OrderBy(l => l).ToArray();
            scpNames_length = scpNames.Length;
            for (int indexS = 0; indexS < scpNames_length; indexS++)
            {
                scpName = scpNames[indexS];
                ordered_lines.AddRange(scpName_dict[scpName]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.Scp_name.CompareTo(previous_line.Scp_name) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_descendingScpName(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<string, List<Ontology_enrichment_line_class>> scpName_dict = new Dictionary<string, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!scpName_dict.ContainsKey(onto_enrich_line.Scp_name))
                {
                    scpName_dict.Add(onto_enrich_line.Scp_name, new List<Ontology_enrichment_line_class>());
                }
                scpName_dict[onto_enrich_line.Scp_name].Add(onto_enrich_line);
            }
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            onto_enrich_array = null;
            string[] scpNames;
            string scpName;
            int scpNames_length;
            scpNames = scpName_dict.Keys.ToArray();
            scpNames = scpNames.OrderByDescending(l => l).ToArray();
            scpNames_length = scpNames.Length;
            for (int indexS = 0; indexS < scpNames_length; indexS++)
            {
                scpName = scpNames[indexS];
                ordered_lines.AddRange(scpName_dict[scpName]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.Scp_name.CompareTo(previous_line.Scp_name) > 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_sampleName(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<string, List<Ontology_enrichment_line_class>> sampleName_dict = new Dictionary<string, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!sampleName_dict.ContainsKey(onto_enrich_line.SampleName))
                {
                    sampleName_dict.Add(onto_enrich_line.SampleName, new List<Ontology_enrichment_line_class>());
                }
                sampleName_dict[onto_enrich_line.SampleName].Add(onto_enrich_line);
            }
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            onto_enrich_array = null;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            sampleNames = sampleName_dict.Keys.ToArray();
            sampleNames = sampleNames.OrderBy(l => l).ToArray();
            sampleNames_length = sampleNames.Length;
            for (int indexS = 0; indexS < sampleNames_length; indexS++)
            {
                sampleName = sampleNames[indexS];
                ordered_lines.AddRange(sampleName_dict[sampleName]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.SampleName.CompareTo(previous_line.SampleName) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_uniqueDatasetName(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<string, List<Ontology_enrichment_line_class>> uniqueDatasetName_dict = new Dictionary<string, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!uniqueDatasetName_dict.ContainsKey(onto_enrich_line.Unique_dataset_name))
                {
                    uniqueDatasetName_dict.Add(onto_enrich_line.Unique_dataset_name, new List<Ontology_enrichment_line_class>());
                }
                uniqueDatasetName_dict[onto_enrich_line.Unique_dataset_name].Add(onto_enrich_line);
            }
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            onto_enrich_array = null;
            string[] unique_dataset_names;
            string unique_dataset_name;
            int unique_dataset_names_length;
            unique_dataset_names = uniqueDatasetName_dict.Keys.ToArray();
            unique_dataset_names = unique_dataset_names.OrderBy(l => l).ToArray();
            unique_dataset_names_length = unique_dataset_names.Length;
            for (int indexS = 0; indexS < unique_dataset_names_length; indexS++)
            {
                unique_dataset_name = unique_dataset_names[indexS];
                ordered_lines.AddRange(uniqueDatasetName_dict[unique_dataset_name]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.Unique_dataset_name.CompareTo(previous_line.Unique_dataset_name) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_sampleName_and_scpName(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            //onto_enrich_array = onto_enrich_array.OrderBy(l => l.SampleName).ThenBy(l => l.Scp_name).ToArray();
            Dictionary<string, Dictionary<string, List<Ontology_enrichment_line_class>>> sampleName_scpName_dict = new Dictionary<string, Dictionary<string, List<Ontology_enrichment_line_class>>>();
            Dictionary<string, List<Ontology_enrichment_line_class>> scpName_dict = new Dictionary<string, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!sampleName_scpName_dict.ContainsKey(onto_enrich_line.SampleName))
                {
                    sampleName_scpName_dict.Add(onto_enrich_line.SampleName, new Dictionary<string, List<Ontology_enrichment_line_class>>());
                }
                if (!sampleName_scpName_dict[onto_enrich_line.SampleName].ContainsKey(onto_enrich_line.Scp_name))
                {
                    sampleName_scpName_dict[onto_enrich_line.SampleName].Add(onto_enrich_line.Scp_name, new List<Ontology_enrichment_line_class>());
                }
                sampleName_scpName_dict[onto_enrich_line.SampleName][onto_enrich_line.Scp_name].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            string[] scpNames;
            string scpName;
            int scpNames_length;
            sampleNames = sampleName_scpName_dict.Keys.ToArray();
            sampleNames = sampleNames.OrderBy(l => l).ToArray();
            sampleNames_length = sampleNames.Length;
            for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
            {
                sampleName = sampleNames[indexSN];
                scpName_dict = sampleName_scpName_dict[sampleName];
                scpNames = scpName_dict.Keys.ToArray();
                scpNames = scpNames.OrderBy(l => l).ToArray();
                scpNames_length = scpNames.Length;
                for (int indexScp = 0; indexScp < scpNames_length; indexScp++)
                {
                    scpName = scpNames[indexScp];
                    ordered_lines.AddRange(scpName_dict[scpName]);
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class this_line;
                Ontology_enrichment_line_class previous_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.SampleName.CompareTo(previous_line.SampleName) < 0) { throw new Exception(); }
                    else if ((this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.Scp_name.CompareTo(previous_line.Scp_name) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_integrationGroup_and_scpName(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            //onto_enrich_array = onto_enrich_array.OrderBy(l => l.SampleName).ThenBy(l => l.Scp_name).ToArray();
            Dictionary<string, Dictionary<string, List<Ontology_enrichment_line_class>>> integrationGroup_scpName_dict = new Dictionary<string, Dictionary<string, List<Ontology_enrichment_line_class>>>();
            Dictionary<string, List<Ontology_enrichment_line_class>> scpName_dict = new Dictionary<string, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!integrationGroup_scpName_dict.ContainsKey(onto_enrich_line.IntegrationGroup))
                {
                    integrationGroup_scpName_dict.Add(onto_enrich_line.IntegrationGroup, new Dictionary<string, List<Ontology_enrichment_line_class>>());
                }
                if (!integrationGroup_scpName_dict[onto_enrich_line.IntegrationGroup].ContainsKey(onto_enrich_line.Scp_name))
                {
                    integrationGroup_scpName_dict[onto_enrich_line.IntegrationGroup].Add(onto_enrich_line.Scp_name, new List<Ontology_enrichment_line_class>());
                }
                integrationGroup_scpName_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.Scp_name].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            string[] scpNames;
            string scpName;
            int scpNames_length;
            integrationGroups = integrationGroup_scpName_dict.Keys.ToArray();
            integrationGroups = integrationGroups.OrderBy(l => l).ToArray();
            integrationGroups_length = integrationGroups.Length;
            for (int indexSN = 0; indexSN < integrationGroups_length; indexSN++)
            {
                integrationGroup = integrationGroups[indexSN];
                scpName_dict = integrationGroup_scpName_dict[integrationGroup];
                scpNames = scpName_dict.Keys.ToArray();
                scpNames = scpNames.OrderBy(l => l).ToArray();
                scpNames_length = scpNames.Length;
                for (int indexScp = 0; indexScp < scpNames_length; indexScp++)
                {
                    scpName = scpNames[indexScp];
                    ordered_lines.AddRange(scpName_dict[scpName]);
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class this_line;
                Ontology_enrichment_line_class previous_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.IntegrationGroup.CompareTo(previous_line.IntegrationGroup) < 0) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.Scp_name.CompareTo(previous_line.Scp_name) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_scpName_resultsNo(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<string, Dictionary<int, List<Ontology_enrichment_line_class>>> scpName_resultsNo_dict = new Dictionary<string, Dictionary<int, List<Ontology_enrichment_line_class>>>();
            Dictionary<int, List<Ontology_enrichment_line_class>> resultsNo_dict = new Dictionary<int, List<Ontology_enrichment_line_class>>();

            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!scpName_resultsNo_dict.ContainsKey(onto_enrich_line.Scp_name))
                {
                    scpName_resultsNo_dict.Add(onto_enrich_line.Scp_name, new Dictionary<int, List<Ontology_enrichment_line_class>>());
                }
                if (!scpName_resultsNo_dict[onto_enrich_line.Scp_name].ContainsKey(onto_enrich_line.Results_number))
                {
                    scpName_resultsNo_dict[onto_enrich_line.Scp_name].Add(onto_enrich_line.Results_number, new List<Ontology_enrichment_line_class>());
                }
                scpName_resultsNo_dict[onto_enrich_line.Scp_name][onto_enrich_line.Results_number].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            string[] scpNames;
            string scpName;
            int scpNames_length;
            int[] resultNos;
            int resultNo;
            int resultNos_length;
            scpNames = scpName_resultsNo_dict.Keys.ToArray();
            scpNames = scpNames.OrderBy(l => l).ToArray();
            scpNames_length = scpNames.Length;
            for (int indexScp = 0; indexScp < scpNames_length; indexScp++)
            {
                scpName = scpNames[indexScp];
                resultsNo_dict = scpName_resultsNo_dict[scpName];
                resultNos = resultsNo_dict.Keys.ToArray();
                resultNos = resultNos.OrderBy(l => l).ToArray();
                resultNos_length = resultNos.Length;
                for (int indexRNo = 0; indexRNo < resultNos_length; indexRNo++)
                {
                    resultNo = resultNos[indexRNo];
                    ordered_lines.AddRange(resultsNo_dict[resultNo]);
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class this_line;
                Ontology_enrichment_line_class previous_line;
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.Scp_name.CompareTo(previous_line.Scp_name) < 0) { throw new Exception(); }
                    else if ((this_line.Scp_name.Equals(previous_line.Scp_name))
                             && (this_line.Results_number.CompareTo(previous_line.Results_number) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_ontology_entryType_timepoinitInDays_sampleName_pvalue(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            //onto_enrich_array = onto_enrich_array.OrderBy(l => l.Ontology_type).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.SampleName).ThenBy(l => l.Pvalue).ToArray();
            Dictionary<Ontology_type_enum, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>>>>> ontology_entryType_timepointInDays_sampleName_pvalue_dict = new Dictionary<Ontology_type_enum, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>>>>>();
            Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>>>> entryType_timepointInDays_sampleName_pvalue_dict = new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>>>>();
            Dictionary<float, Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>>> timepointInDays_sampleName_pvalue_dict = new Dictionary<float, Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>>>();
            Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>> sampleName_pvalue_dict = new Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>>();
            Dictionary<double, List<Ontology_enrichment_line_class>> pvalue_dict = new Dictionary<double, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepoint_in_days;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepoint_in_days = onto_enrich_line.TimepointInDays;
                if (!ontology_entryType_timepointInDays_sampleName_pvalue_dict.ContainsKey(onto_enrich_line.Ontology_type))
                {
                    ontology_entryType_timepointInDays_sampleName_pvalue_dict.Add(onto_enrich_line.Ontology_type, new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>>>>());
                }
                if (!ontology_entryType_timepointInDays_sampleName_pvalue_dict[onto_enrich_line.Ontology_type].ContainsKey(onto_enrich_line.EntryType))
                {
                    ontology_entryType_timepointInDays_sampleName_pvalue_dict[onto_enrich_line.Ontology_type].Add(onto_enrich_line.EntryType, new Dictionary<float, Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>>>());
                }
                if (!ontology_entryType_timepointInDays_sampleName_pvalue_dict[onto_enrich_line.Ontology_type][onto_enrich_line.EntryType].ContainsKey(timepoint_in_days))
                {
                    ontology_entryType_timepointInDays_sampleName_pvalue_dict[onto_enrich_line.Ontology_type][onto_enrich_line.EntryType].Add(timepoint_in_days, new Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>>());
                }
                if (!ontology_entryType_timepointInDays_sampleName_pvalue_dict[onto_enrich_line.Ontology_type][onto_enrich_line.EntryType][timepoint_in_days].ContainsKey(onto_enrich_line.SampleName))
                {
                    ontology_entryType_timepointInDays_sampleName_pvalue_dict[onto_enrich_line.Ontology_type][onto_enrich_line.EntryType][timepoint_in_days].Add(onto_enrich_line.SampleName, new Dictionary<double, List<Ontology_enrichment_line_class>>());
                }
                if (!ontology_entryType_timepointInDays_sampleName_pvalue_dict[onto_enrich_line.Ontology_type][onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName].ContainsKey(onto_enrich_line.Pvalue))
                {
                    ontology_entryType_timepointInDays_sampleName_pvalue_dict[onto_enrich_line.Ontology_type][onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName].Add(onto_enrich_line.Pvalue, new List<Ontology_enrichment_line_class>());
                }
                ontology_entryType_timepointInDays_sampleName_pvalue_dict[onto_enrich_line.Ontology_type][onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName][onto_enrich_line.Pvalue].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            Ontology_type_enum[] ontologies;
            Ontology_type_enum ontology;
            int ontologies_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            float[] timepointsInDays;
            float timepointInDay;
            int timepointsInDays_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            double[] pvalues;
            double pvalue;
            int pvalues_length;
            ontologies = ontology_entryType_timepointInDays_sampleName_pvalue_dict.Keys.ToArray();
            ontologies = ontologies.OrderBy(l => l).ToArray();
            ontologies_length = ontologies.Length;
            for (int indexO = 0; indexO < ontologies_length; indexO++)
            {
                ontology = ontologies[indexO];
                entryType_timepointInDays_sampleName_pvalue_dict = ontology_entryType_timepointInDays_sampleName_pvalue_dict[ontology];
                entryTypes = entryType_timepointInDays_sampleName_pvalue_dict.Keys.ToArray();
                entryTypes = entryTypes.OrderBy(l => l).ToArray();
                entryTypes_length = entryTypes.Length;
                for (int indexE = 0; indexE < entryTypes_length; indexE++)
                {
                    entryType = entryTypes[indexE];
                    timepointInDays_sampleName_pvalue_dict = entryType_timepointInDays_sampleName_pvalue_dict[entryType];
                    timepointsInDays = timepointInDays_sampleName_pvalue_dict.Keys.ToArray();
                    timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
                    timepointsInDays_length = timepointsInDays.Length;
                    for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                    {
                        timepointInDay = timepointsInDays[indexT];
                        sampleName_pvalue_dict = timepointInDays_sampleName_pvalue_dict[timepointInDay];
                        sampleNames = sampleName_pvalue_dict.Keys.ToArray();
                        sampleNames = sampleNames.OrderBy(l => l).ToArray();
                        sampleNames_length = sampleNames.Length;
                        for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                        {
                            sampleName = sampleNames[indexSN];
                            pvalue_dict = sampleName_pvalue_dict[sampleName];
                            pvalues = pvalue_dict.Keys.ToArray();
                            pvalues = pvalues.OrderBy(l => l).ToArray();
                            pvalues_length = pvalues.Length;
                            for (int indexP = 0; indexP < pvalues_length; indexP++)
                            {
                                pvalue = pvalues[indexP];
                                ordered_lines.AddRange(pvalue_dict[pvalue]);
                            }
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //ontology_entryType_timepoinitInDays_sampleName_pvalue
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    this_timepointInDays = this_line.TimepointInDays;
                    previous_timepointInDays = previous_line.TimepointInDays;
                    if (this_line.Ontology_type.CompareTo(previous_line.Ontology_type) < 0) { throw new Exception(); }
                    else if ((this_line.Ontology_type.Equals(previous_line.Ontology_type))
                             && (this_line.EntryType.CompareTo(previous_line.EntryType) < 0)) { throw new Exception(); }
                    else if ((this_line.Ontology_type.Equals(previous_line.Ontology_type))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                    else if ((this_line.Ontology_type.Equals(previous_line.Ontology_type))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    else if ((this_line.Ontology_type.Equals(previous_line.Ontology_type))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.Pvalue.CompareTo(previous_line.Pvalue) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_entryType_timepoinitInDays_sampleName_pvalue(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            //onto_enrich_array = onto_enrich_array.OrderBy(l => l.Ontology_type).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.SampleName).ThenBy(l => l.Pvalue).ToArray();
            Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>>>> entryType_timepointInDays_sampleName_pvalue_dict = new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>>>>();
            Dictionary<float, Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>>> timepointInDays_sampleName_pvalue_dict = new Dictionary<float, Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>>>();
            Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>> sampleName_pvalue_dict = new Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>>();
            Dictionary<double, List<Ontology_enrichment_line_class>> pvalue_dict = new Dictionary<double, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepoint_in_days;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepoint_in_days = onto_enrich_line.TimepointInDays;
                if (!entryType_timepointInDays_sampleName_pvalue_dict.ContainsKey(onto_enrich_line.EntryType))
                {
                    entryType_timepointInDays_sampleName_pvalue_dict.Add(onto_enrich_line.EntryType, new Dictionary<float, Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>>>());
                }
                if (!entryType_timepointInDays_sampleName_pvalue_dict[onto_enrich_line.EntryType].ContainsKey(timepoint_in_days))
                {
                    entryType_timepointInDays_sampleName_pvalue_dict[onto_enrich_line.EntryType].Add(timepoint_in_days, new Dictionary<string, Dictionary<double, List<Ontology_enrichment_line_class>>>());
                }
                if (!entryType_timepointInDays_sampleName_pvalue_dict[onto_enrich_line.EntryType][timepoint_in_days].ContainsKey(onto_enrich_line.SampleName))
                {
                    entryType_timepointInDays_sampleName_pvalue_dict[onto_enrich_line.EntryType][timepoint_in_days].Add(onto_enrich_line.SampleName, new Dictionary<double, List<Ontology_enrichment_line_class>>());
                }
                if (!entryType_timepointInDays_sampleName_pvalue_dict[onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName].ContainsKey(onto_enrich_line.Pvalue))
                {
                    entryType_timepointInDays_sampleName_pvalue_dict[onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName].Add(onto_enrich_line.Pvalue, new List<Ontology_enrichment_line_class>());
                }
                entryType_timepointInDays_sampleName_pvalue_dict[onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName][onto_enrich_line.Pvalue].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            float[] timepointsInDays;
            float timepointInDay;
            int timepointsInDays_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            double[] pvalues;
            double pvalue;
            int pvalues_length;
            entryTypes = entryType_timepointInDays_sampleName_pvalue_dict.Keys.ToArray();
            entryTypes = entryTypes.OrderBy(l => l).ToArray();
            entryTypes_length = entryTypes.Length;
            for (int indexE = 0; indexE < entryTypes_length; indexE++)
            {
                entryType = entryTypes[indexE];
                timepointInDays_sampleName_pvalue_dict = entryType_timepointInDays_sampleName_pvalue_dict[entryType];
                timepointsInDays = timepointInDays_sampleName_pvalue_dict.Keys.ToArray();
                timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
                timepointsInDays_length = timepointsInDays.Length;
                for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                {
                    timepointInDay = timepointsInDays[indexT];
                    sampleName_pvalue_dict = timepointInDays_sampleName_pvalue_dict[timepointInDay];
                    sampleNames = sampleName_pvalue_dict.Keys.ToArray();
                    sampleNames = sampleNames.OrderBy(l => l).ToArray();
                    sampleNames_length = sampleNames.Length;
                    for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                    {
                        sampleName = sampleNames[indexSN];
                        pvalue_dict = sampleName_pvalue_dict[sampleName];
                        pvalues = pvalue_dict.Keys.ToArray();
                        pvalues = pvalues.OrderBy(l => l).ToArray();
                        pvalues_length = pvalues.Length;
                        for (int indexP = 0; indexP < pvalues_length; indexP++)
                        {
                            pvalue = pvalues[indexP];
                            ordered_lines.AddRange(pvalue_dict[pvalue]);
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //entryType_timepoinitInDays_sampleName_pvalue
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    this_timepointInDays = this_line.TimepointInDays;
                    previous_timepointInDays = previous_line.TimepointInDays;
                    if (this_line.EntryType.CompareTo(previous_line.EntryType) < 0) { throw new Exception(); }
                    else if (   (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                    else if (   (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    else if (   (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.Pvalue.CompareTo(previous_line.Pvalue) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_entryType_timepoinitInDays_sampleName_scpName(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            //this.Enrich = this.Enrich.OrderBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.SampleName).ThenBy(l => l.Scp_name).ToArray();
            Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<string, List<Ontology_enrichment_line_class>>>>> entryType_timepointInDays_sampleName_scpName_dict = new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<string, List<Ontology_enrichment_line_class>>>>>();
            Dictionary<float, Dictionary<string, Dictionary<string, List<Ontology_enrichment_line_class>>>> timepointInDays_sampleName_scpName_dict = new Dictionary<float, Dictionary<string, Dictionary<string, List<Ontology_enrichment_line_class>>>>();
            Dictionary<string, Dictionary<string, List<Ontology_enrichment_line_class>>> sampleName_scpName_dict = new Dictionary<string, Dictionary<string, List<Ontology_enrichment_line_class>>>();
            Dictionary<string, List<Ontology_enrichment_line_class>> scpName_dict = new Dictionary<string, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepoint_in_days;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepoint_in_days = onto_enrich_line.TimepointInDays;
                if (!entryType_timepointInDays_sampleName_scpName_dict.ContainsKey(onto_enrich_line.EntryType))
                {
                    entryType_timepointInDays_sampleName_scpName_dict.Add(onto_enrich_line.EntryType, new Dictionary<float, Dictionary<string, Dictionary<string, List<Ontology_enrichment_line_class>>>>());
                }
                if (!entryType_timepointInDays_sampleName_scpName_dict[onto_enrich_line.EntryType].ContainsKey(timepoint_in_days))
                {
                    entryType_timepointInDays_sampleName_scpName_dict[onto_enrich_line.EntryType].Add(timepoint_in_days, new Dictionary<string, Dictionary<string, List<Ontology_enrichment_line_class>>>());
                }
                if (!entryType_timepointInDays_sampleName_scpName_dict[onto_enrich_line.EntryType][timepoint_in_days].ContainsKey(onto_enrich_line.SampleName))
                {
                    entryType_timepointInDays_sampleName_scpName_dict[onto_enrich_line.EntryType][timepoint_in_days].Add(onto_enrich_line.SampleName, new Dictionary<string, List<Ontology_enrichment_line_class>>());
                }
                if (!entryType_timepointInDays_sampleName_scpName_dict[onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName].ContainsKey(onto_enrich_line.Scp_name))
                {
                    entryType_timepointInDays_sampleName_scpName_dict[onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName].Add(onto_enrich_line.Scp_name, new List<Ontology_enrichment_line_class>());
                }
                entryType_timepointInDays_sampleName_scpName_dict[onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName][onto_enrich_line.Scp_name].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            float[] timepointsInDays;
            float timepointInDay;
            int timepointsInDays_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            string[] scpNames;
            string scpName;
            int scpNames_length;
            entryTypes = entryType_timepointInDays_sampleName_scpName_dict.Keys.ToArray();
            entryTypes = entryTypes.OrderBy(l => l).ToArray();
            entryTypes_length = entryTypes.Length;
            for (int indexE = 0; indexE < entryTypes_length; indexE++)
            {
                entryType = entryTypes[indexE];
                timepointInDays_sampleName_scpName_dict = entryType_timepointInDays_sampleName_scpName_dict[entryType];
                timepointsInDays = timepointInDays_sampleName_scpName_dict.Keys.ToArray();
                timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
                timepointsInDays_length = timepointsInDays.Length;
                for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                {
                    timepointInDay = timepointsInDays[indexT];
                    sampleName_scpName_dict = timepointInDays_sampleName_scpName_dict[timepointInDay];
                    sampleNames = sampleName_scpName_dict.Keys.ToArray();
                    sampleNames = sampleNames.OrderBy(l => l).ToArray();
                    sampleNames_length = sampleNames.Length;
                    for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                    {
                        sampleName = sampleNames[indexSN];
                        scpName_dict = sampleName_scpName_dict[sampleName];
                        scpNames = scpName_dict.Keys.ToArray();
                        scpNames = scpNames.OrderBy(l => l).ToArray();
                        scpNames_length = scpNames.Length;
                        for (int indexP = 0; indexP < scpNames_length; indexP++)
                        {
                            scpName = scpNames[indexP];
                            ordered_lines.AddRange(scpName_dict[scpName]);
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //entryType_timepoinitInDays_sampleName_scpName
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    this_timepointInDays = this_line.TimepointInDays;
                    previous_timepointInDays = previous_line.TimepointInDays;
                    if (this_line.EntryType.CompareTo(previous_line.EntryType) < 0) { throw new Exception(); }
                    else if ((this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                    else if ((this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    else if ((this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.Scp_name.CompareTo(previous_line.Scp_name) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_scpName_integrationGroup_entryType_timepointInDays_sampleName(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            //this.Enrich = this.Enrich.OrderBy(l => l.Scp_name).ThenBy(l => l.Complete_SampleName).ToArray();
            //IntegrationGroup, EntryType, Timepoint, Timeunit, SampleName
            Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>>>> scpName_integrationGroup_entryType_timepointInDays_sampleName_dict = new Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>>>>();
            Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>>> integrationGroup_entryType_timepointInDays_sampleName_dict = new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>>>();
            Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>> entryType_timepointInDays_sampleName_dict = new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>>();
            Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>> timepointInDays_sampleName_dict = new Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>();
            Dictionary<string, List<Ontology_enrichment_line_class>> sampleName_dict = new Dictionary<string, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepointInDays;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepointInDays = onto_enrich_line.TimepointInDays;
                if (!scpName_integrationGroup_entryType_timepointInDays_sampleName_dict.ContainsKey(onto_enrich_line.Scp_name))
                {
                    scpName_integrationGroup_entryType_timepointInDays_sampleName_dict.Add(onto_enrich_line.Scp_name, new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>>>());
                }
                if (!scpName_integrationGroup_entryType_timepointInDays_sampleName_dict[onto_enrich_line.Scp_name].ContainsKey(onto_enrich_line.IntegrationGroup))
                {
                    scpName_integrationGroup_entryType_timepointInDays_sampleName_dict[onto_enrich_line.Scp_name].Add(onto_enrich_line.IntegrationGroup, new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>>());
                }
                if (!scpName_integrationGroup_entryType_timepointInDays_sampleName_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup].ContainsKey(onto_enrich_line.EntryType))
                {
                    scpName_integrationGroup_entryType_timepointInDays_sampleName_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup].Add(onto_enrich_line.EntryType, new Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>());
                }
                if (!scpName_integrationGroup_entryType_timepointInDays_sampleName_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType].ContainsKey(timepointInDays))
                {
                    scpName_integrationGroup_entryType_timepointInDays_sampleName_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType].Add(timepointInDays, new Dictionary<string, List<Ontology_enrichment_line_class>>());
                }
                if (!scpName_integrationGroup_entryType_timepointInDays_sampleName_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType][timepointInDays].ContainsKey(onto_enrich_line.SampleName))
                {
                    scpName_integrationGroup_entryType_timepointInDays_sampleName_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType][timepointInDays].Add(onto_enrich_line.SampleName, new List<Ontology_enrichment_line_class>());
                }
                scpName_integrationGroup_entryType_timepointInDays_sampleName_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType][timepointInDays][onto_enrich_line.SampleName].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            string[] scpNames;
            string scpName;
            int scpNames_length;
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            float[] timepointsInDays;
            int timepointsInDays_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            scpNames = scpName_integrationGroup_entryType_timepointInDays_sampleName_dict.Keys.ToArray();
            scpNames = scpNames.OrderBy(l => l).ToArray();
            scpNames_length = scpNames.Length;
            for (int indexScpN = 0; indexScpN < scpNames_length; indexScpN++)
            {
                scpName = scpNames[indexScpN];
                integrationGroup_entryType_timepointInDays_sampleName_dict = scpName_integrationGroup_entryType_timepointInDays_sampleName_dict[scpName];
                integrationGroups = integrationGroup_entryType_timepointInDays_sampleName_dict.Keys.ToArray();
                integrationGroups = integrationGroups.OrderBy(l => l).ToArray();
                integrationGroups_length = integrationGroups.Length;
                for (int indexInt = 0; indexInt < integrationGroups_length; indexInt++)
                {
                    integrationGroup = integrationGroups[indexInt];
                    entryType_timepointInDays_sampleName_dict = integrationGroup_entryType_timepointInDays_sampleName_dict[integrationGroup];
                    entryTypes = entryType_timepointInDays_sampleName_dict.Keys.ToArray();
                    entryTypes = entryTypes.OrderBy(l => l).ToArray();
                    entryTypes_length = entryTypes.Length;
                    for (int indexET = 0; indexET < entryTypes_length; indexET++)
                    {
                        entryType = entryTypes[indexET];
                        timepointInDays_sampleName_dict = entryType_timepointInDays_sampleName_dict[entryType];
                        timepointsInDays = timepointInDays_sampleName_dict.Keys.ToArray();
                        timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
                        timepointsInDays_length = timepointsInDays.Length;
                        for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                        {
                            timepointInDays = timepointsInDays[indexT];
                            sampleName_dict = timepointInDays_sampleName_dict[timepointInDays];
                            sampleNames = sampleName_dict.Keys.ToArray();
                            sampleNames = sampleNames.OrderBy(l => l).ToArray();
                            sampleNames_length = sampleNames.Length;
                            for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                            {
                                sampleName = sampleNames[indexSN];
                                ordered_lines.AddRange(sampleName_dict[sampleName]);
                            }
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //scpName_integrationGroup_entryType_timepointInDays_sampleName
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    this_timepointInDays = this_line.TimepointInDays;
                    previous_timepointInDays = previous_line.TimepointInDays;
                    if (this_line.Scp_name.CompareTo(previous_line.Scp_name) < 0) { throw new Exception(); }
                    else if ((this_line.Scp_name.Equals(previous_line.Scp_name))
                             && (this_line.IntegrationGroup.CompareTo(previous_line.IntegrationGroup) < 0)) { throw new Exception(); }
                    else if ((this_line.Scp_name.Equals(previous_line.Scp_name))
                             && (this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.EntryType.CompareTo(previous_line.EntryType) < 0)) { throw new Exception(); }
                    else if ((this_line.Scp_name.Equals(previous_line.Scp_name))
                             && (this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                    else if ((this_line.Scp_name.Equals(previous_line.Scp_name))
                             && (this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_scpName_integrationGroup_sampleName_entryType_timepointInDays(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, List<Ontology_enrichment_line_class>>>>>> scpName_integrationGroup_sampleName_entryType_timepointInDays_dict = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, List<Ontology_enrichment_line_class>>>>>>();
            Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, List<Ontology_enrichment_line_class>>>>> integrationGroup_sampleName_entryType_timepointInDays_dict = new Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, List<Ontology_enrichment_line_class>>>>>();
            Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, List<Ontology_enrichment_line_class>>>> sampleName_entryType_timepointInDays_dict = new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, List<Ontology_enrichment_line_class>>>>();
            Dictionary<Entry_type_enum, Dictionary<float, List<Ontology_enrichment_line_class>>> entryType_timepointInDays_dict = new Dictionary<Entry_type_enum, Dictionary<float, List<Ontology_enrichment_line_class>>>();
            Dictionary<float, List<Ontology_enrichment_line_class>> timepointInDays_dict = new Dictionary<float, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepointInDays;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepointInDays = onto_enrich_line.TimepointInDays;
                if (!scpName_integrationGroup_sampleName_entryType_timepointInDays_dict.ContainsKey(onto_enrich_line.Scp_name))
                {
                    scpName_integrationGroup_sampleName_entryType_timepointInDays_dict.Add(onto_enrich_line.Scp_name, new Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, List<Ontology_enrichment_line_class>>>>>());
                }
                if (!scpName_integrationGroup_sampleName_entryType_timepointInDays_dict[onto_enrich_line.Scp_name].ContainsKey(onto_enrich_line.IntegrationGroup))
                {
                    scpName_integrationGroup_sampleName_entryType_timepointInDays_dict[onto_enrich_line.Scp_name].Add(onto_enrich_line.IntegrationGroup, new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, List<Ontology_enrichment_line_class>>>>());
                }
                if (!scpName_integrationGroup_sampleName_entryType_timepointInDays_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup].ContainsKey(onto_enrich_line.SampleName))
                {
                    scpName_integrationGroup_sampleName_entryType_timepointInDays_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup].Add(onto_enrich_line.SampleName, new Dictionary<Entry_type_enum, Dictionary<float, List<Ontology_enrichment_line_class>>>());
                }
                if (!scpName_integrationGroup_sampleName_entryType_timepointInDays_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName].ContainsKey(onto_enrich_line.EntryType))
                {
                    scpName_integrationGroup_sampleName_entryType_timepointInDays_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName].Add(onto_enrich_line.EntryType, new Dictionary<float, List<Ontology_enrichment_line_class>>());
                }
                if (!scpName_integrationGroup_sampleName_entryType_timepointInDays_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType].ContainsKey(timepointInDays))
                {
                    scpName_integrationGroup_sampleName_entryType_timepointInDays_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType].Add(timepointInDays, new List<Ontology_enrichment_line_class>());
                }
                scpName_integrationGroup_sampleName_entryType_timepointInDays_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType][timepointInDays].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            string[] scpNames;
            string scpName;
            int scpNames_length;
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            float[] timepointsInDays;
            int timepointsInDays_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            scpNames = scpName_integrationGroup_sampleName_entryType_timepointInDays_dict.Keys.ToArray();
            scpNames = scpNames.OrderBy(l => l).ToArray();
            scpNames_length = scpNames.Length;
            for (int indexScpN = 0; indexScpN < scpNames_length; indexScpN++)
            {
                scpName = scpNames[indexScpN];
                integrationGroup_sampleName_entryType_timepointInDays_dict = scpName_integrationGroup_sampleName_entryType_timepointInDays_dict[scpName];
                integrationGroups = integrationGroup_sampleName_entryType_timepointInDays_dict.Keys.ToArray();
                integrationGroups = integrationGroups.OrderBy(l => l).ToArray();
                integrationGroups_length = integrationGroups.Length;
                for (int indexInt = 0; indexInt < integrationGroups_length; indexInt++)
                {
                    integrationGroup = integrationGroups[indexInt];
                    sampleName_entryType_timepointInDays_dict = integrationGroup_sampleName_entryType_timepointInDays_dict[integrationGroup];
                    sampleNames = sampleName_entryType_timepointInDays_dict.Keys.ToArray();
                    sampleNames = sampleNames.OrderBy(l => l).ToArray();
                    sampleNames_length = sampleNames.Length;
                    for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                    {
                        sampleName = sampleNames[indexSN];
                        entryType_timepointInDays_dict = sampleName_entryType_timepointInDays_dict[sampleName];
                        entryTypes = entryType_timepointInDays_dict.Keys.ToArray();
                        entryTypes = entryTypes.OrderBy(l => l).ToArray();
                        entryTypes_length = entryTypes.Length;
                        for (int indexET = 0; indexET < entryTypes_length; indexET++)
                        {
                            entryType = entryTypes[indexET];
                            timepointInDays_dict = entryType_timepointInDays_dict[entryType];
                            timepointsInDays = timepointInDays_dict.Keys.ToArray();
                            timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
                            timepointsInDays_length = timepointsInDays.Length;
                            for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                            {
                                timepointInDays = timepointsInDays[indexT];
                                ordered_lines.AddRange(timepointInDays_dict[timepointInDays]);
                            }
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //scpName_integrationGroup_sampleName_entryType_timepointInDays
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    this_timepointInDays = this_line.TimepointInDays;
                    previous_timepointInDays = previous_line.TimepointInDays;
                    if (this_line.Scp_name.CompareTo(previous_line.Scp_name) < 0) { throw new Exception(); }
                    else if ((this_line.Scp_name.Equals(previous_line.Scp_name))
                             && (this_line.IntegrationGroup.CompareTo(previous_line.IntegrationGroup) < 0)) { throw new Exception(); }
                    else if ((this_line.Scp_name.Equals(previous_line.Scp_name))
                             && (this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    else if ((this_line.Scp_name.Equals(previous_line.Scp_name))
                             && (this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.EntryType.CompareTo(previous_line.EntryType) < 0)) { throw new Exception(); }
                    else if ((this_line.Scp_name.Equals(previous_line.Scp_name))
                             && (this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_scpName_integrationGroup_entryType_sampleName_timepointInDays(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            //this.Enrich = this.Enrich.OrderBy(l => l.Scp_name).ThenBy(l => l.Complete_SampleName).ToArray();
            //IntegrationGroup, EntryType, Timepoint, Timeunit, SampleName
            Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>>>> scpName_integrationGroup_entryType_sampleName_timepointInDays_dict = new Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>>>>();
            Dictionary<string, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>>> integrationGroup_entryType_sampleName_timepointInDays_dict = new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>>>();
            Dictionary<Entry_type_enum, Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>> entryType_sampleName_timepointInDays_dict = new Dictionary<Entry_type_enum, Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>>();
            Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>> sampleName_timepointInDays_dict = new Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>();
            Dictionary<float, List<Ontology_enrichment_line_class>> timepointInDays_dict = new Dictionary<float, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepointInDays;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepointInDays = onto_enrich_line.TimepointInDays;
                if (!scpName_integrationGroup_entryType_sampleName_timepointInDays_dict.ContainsKey(onto_enrich_line.Scp_name))
                {
                    scpName_integrationGroup_entryType_sampleName_timepointInDays_dict.Add(onto_enrich_line.Scp_name, new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>>>());
                }
                if (!scpName_integrationGroup_entryType_sampleName_timepointInDays_dict[onto_enrich_line.Scp_name].ContainsKey(onto_enrich_line.IntegrationGroup))
                {
                    scpName_integrationGroup_entryType_sampleName_timepointInDays_dict[onto_enrich_line.Scp_name].Add(onto_enrich_line.IntegrationGroup, new Dictionary<Entry_type_enum, Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>>());
                }
                if (!scpName_integrationGroup_entryType_sampleName_timepointInDays_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup].ContainsKey(onto_enrich_line.EntryType))
                {
                    scpName_integrationGroup_entryType_sampleName_timepointInDays_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup].Add(onto_enrich_line.EntryType, new Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>());
                }
                if (!scpName_integrationGroup_entryType_sampleName_timepointInDays_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType].ContainsKey(onto_enrich_line.SampleName))
                {
                    scpName_integrationGroup_entryType_sampleName_timepointInDays_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType].Add(onto_enrich_line.SampleName, new Dictionary<float, List<Ontology_enrichment_line_class>>());
                }
                if (!scpName_integrationGroup_entryType_sampleName_timepointInDays_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType][onto_enrich_line.SampleName].ContainsKey(timepointInDays))
                {
                    scpName_integrationGroup_entryType_sampleName_timepointInDays_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType][onto_enrich_line.SampleName].Add(timepointInDays, new List<Ontology_enrichment_line_class>());
                }
                scpName_integrationGroup_entryType_sampleName_timepointInDays_dict[onto_enrich_line.Scp_name][onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType][onto_enrich_line.SampleName][timepointInDays].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            string[] scpNames;
            string scpName;
            int scpNames_length;
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            float[] timepointsInDays;
            int timepointsInDays_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            scpNames = scpName_integrationGroup_entryType_sampleName_timepointInDays_dict.Keys.ToArray();
            scpNames = scpNames.OrderBy(l => l).ToArray();
            scpNames_length = scpNames.Length;
            for (int indexScpN = 0; indexScpN < scpNames_length; indexScpN++)
            {
                scpName = scpNames[indexScpN];
                integrationGroup_entryType_sampleName_timepointInDays_dict = scpName_integrationGroup_entryType_sampleName_timepointInDays_dict[scpName];
                integrationGroups = integrationGroup_entryType_sampleName_timepointInDays_dict.Keys.ToArray();
                integrationGroups = integrationGroups.OrderBy(l => l).ToArray();
                integrationGroups_length = integrationGroups.Length;
                for (int indexInt = 0; indexInt < integrationGroups_length; indexInt++)
                {
                    integrationGroup = integrationGroups[indexInt];
                    entryType_sampleName_timepointInDays_dict = integrationGroup_entryType_sampleName_timepointInDays_dict[integrationGroup];
                    entryTypes = entryType_sampleName_timepointInDays_dict.Keys.ToArray();
                    entryTypes = entryTypes.OrderBy(l => l).ToArray();
                    entryTypes_length = entryTypes.Length;
                    for (int indexET = 0; indexET < entryTypes_length; indexET++)
                    {
                        entryType = entryTypes[indexET];
                        sampleName_timepointInDays_dict = entryType_sampleName_timepointInDays_dict[entryType];
                        sampleNames = sampleName_timepointInDays_dict.Keys.ToArray();
                        sampleNames = sampleNames.OrderBy(l => l).ToArray();
                        sampleNames_length = sampleNames.Length;
                        for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                        {
                            sampleName = sampleNames[indexSN];
                            timepointInDays_dict = sampleName_timepointInDays_dict[sampleName];
                            timepointsInDays = timepointInDays_dict.Keys.ToArray();
                            timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
                            timepointsInDays_length = timepointsInDays.Length;
                            for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                            {
                                timepointInDays = timepointsInDays[indexT];
                                ordered_lines.AddRange(timepointInDays_dict[timepointInDays]);
                            }
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //scpName_integrationGroup_entryType_sampleName_timepointInDays
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    this_timepointInDays = this_line.TimepointInDays;
                    previous_timepointInDays = previous_line.TimepointInDays;
                    if (this_line.Scp_name.CompareTo(previous_line.Scp_name) < 0) { throw new Exception(); }
                    else if ((this_line.Scp_name.Equals(previous_line.Scp_name))
                             && (this_line.IntegrationGroup.CompareTo(previous_line.IntegrationGroup) < 0)) { throw new Exception(); }
                    else if ((this_line.Scp_name.Equals(previous_line.Scp_name))
                             && (this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.EntryType.CompareTo(previous_line.EntryType) < 0)) { throw new Exception(); }
                    else if ((this_line.Scp_name.Equals(previous_line.Scp_name))
                             && (this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    else if ((this_line.Scp_name.Equals(previous_line.Scp_name))
                             && (this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_integrationGroup_entryType_sampleName_timepointInDays(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<string, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>>> integrationGroup_entryType_sampleName_timepointInDays_dict = new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>>>();
            Dictionary<Entry_type_enum, Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>> entryType_sampleName_timepointInDays_dict = new Dictionary<Entry_type_enum, Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>>();
            Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>> sampleName_timepointInDays_dict = new Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>();
            Dictionary<float, List<Ontology_enrichment_line_class>> timepointInDays_dict = new Dictionary<float, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepointInDays;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepointInDays = onto_enrich_line.TimepointInDays;
                if (!integrationGroup_entryType_sampleName_timepointInDays_dict.ContainsKey(onto_enrich_line.IntegrationGroup))
                {
                    integrationGroup_entryType_sampleName_timepointInDays_dict.Add(onto_enrich_line.IntegrationGroup, new Dictionary<Entry_type_enum, Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>>());
                }
                if (!integrationGroup_entryType_sampleName_timepointInDays_dict[onto_enrich_line.IntegrationGroup].ContainsKey(onto_enrich_line.EntryType))
                {
                    integrationGroup_entryType_sampleName_timepointInDays_dict[onto_enrich_line.IntegrationGroup].Add(onto_enrich_line.EntryType, new Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>());
                }
                if (!integrationGroup_entryType_sampleName_timepointInDays_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType].ContainsKey(onto_enrich_line.SampleName))
                {
                    integrationGroup_entryType_sampleName_timepointInDays_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType].Add(onto_enrich_line.SampleName, new Dictionary<float, List<Ontology_enrichment_line_class>>());
                }
                if (!integrationGroup_entryType_sampleName_timepointInDays_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType][onto_enrich_line.SampleName].ContainsKey(timepointInDays))
                {
                    integrationGroup_entryType_sampleName_timepointInDays_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType][onto_enrich_line.SampleName].Add(timepointInDays, new List<Ontology_enrichment_line_class>());
                }
                integrationGroup_entryType_sampleName_timepointInDays_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType][onto_enrich_line.SampleName][timepointInDays].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            float[] timepointsInDays;
            int timepointsInDays_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            integrationGroups = integrationGroup_entryType_sampleName_timepointInDays_dict.Keys.ToArray();
            integrationGroups = integrationGroups.OrderBy(l => l).ToArray();
            integrationGroups_length = integrationGroups.Length;
            for (int indexInt = 0; indexInt < integrationGroups_length; indexInt++)
            {
                integrationGroup = integrationGroups[indexInt];
                entryType_sampleName_timepointInDays_dict = integrationGroup_entryType_sampleName_timepointInDays_dict[integrationGroup];
                entryTypes = entryType_sampleName_timepointInDays_dict.Keys.ToArray();
                entryTypes = entryTypes.OrderBy(l => l).ToArray();
                entryTypes_length = entryTypes.Length;
                for (int indexET = 0; indexET < entryTypes_length; indexET++)
                {
                    entryType = entryTypes[indexET];
                    sampleName_timepointInDays_dict = entryType_sampleName_timepointInDays_dict[entryType];
                    sampleNames = sampleName_timepointInDays_dict.Keys.ToArray();
                    sampleNames = sampleNames.OrderBy(l => l).ToArray();
                    sampleNames_length = sampleNames.Length;
                    for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                    {
                        sampleName = sampleNames[indexSN];
                        timepointInDays_dict = sampleName_timepointInDays_dict[sampleName];
                        timepointsInDays = timepointInDays_dict.Keys.ToArray();
                        timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
                        timepointsInDays_length = timepointsInDays.Length;
                        for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                        {
                            timepointInDays = timepointsInDays[indexT];
                            ordered_lines.AddRange(timepointInDays_dict[timepointInDays]);
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //integrationGroup_entryType_sampleName_timepointInDays
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    this_timepointInDays = this_line.TimepointInDays;
                    previous_timepointInDays = previous_line.TimepointInDays;
                    if ((this_line.IntegrationGroup.CompareTo(previous_line.IntegrationGroup) < 0)) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.EntryType.CompareTo(previous_line.EntryType) < 0)) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_ontology_entryType_timepointInDays_sampleName_processLevel_descendingMinusLog10Pvalue(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            //this.Enrich = this.Enrich.OrderBy(l => l.Ontology_type).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.SampleName).ThenBy(l => l.ProcessLevel).ThenByDescending(l => l.Minus_log10_pvalue).ToArray();
            Dictionary<Ontology_type_enum, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>>>>> ontology_entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict = new Dictionary<Ontology_type_enum, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>>>>>();
            Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>>>> entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict = new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>>>>();
            Dictionary<float, Dictionary<string, Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>>> timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict = new Dictionary<float, Dictionary<string, Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>>>();
            Dictionary<string, Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>> sampleName_processLevel_minusLog10Pvalue_dict = new Dictionary<string, Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>>();
            Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>> processLevel_minusLog10Pvalue_dict = new Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>();
            Dictionary<float, List<Ontology_enrichment_line_class>> minusLog10Pvalue_dict = new Dictionary<float, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepoint_in_days;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepoint_in_days = onto_enrich_line.TimepointInDays;
                if (!ontology_entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict.ContainsKey(onto_enrich_line.Ontology_type))
                {
                    ontology_entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict.Add(onto_enrich_line.Ontology_type, new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>>>>());
                }
                if (!ontology_entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict[onto_enrich_line.Ontology_type].ContainsKey(onto_enrich_line.EntryType))
                {
                    ontology_entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict[onto_enrich_line.Ontology_type].Add(onto_enrich_line.EntryType, new Dictionary<float, Dictionary<string, Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>>>());
                }
                if (!ontology_entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict[onto_enrich_line.Ontology_type][onto_enrich_line.EntryType].ContainsKey(timepoint_in_days))
                {
                    ontology_entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict[onto_enrich_line.Ontology_type][onto_enrich_line.EntryType].Add(timepoint_in_days, new Dictionary<string, Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>>());
                }
                if (!ontology_entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict[onto_enrich_line.Ontology_type][onto_enrich_line.EntryType][timepoint_in_days].ContainsKey(onto_enrich_line.SampleName))
                {
                    ontology_entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict[onto_enrich_line.Ontology_type][onto_enrich_line.EntryType][timepoint_in_days].Add(onto_enrich_line.SampleName, new Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>());
                }
                if (!ontology_entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict[onto_enrich_line.Ontology_type][onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName].ContainsKey(onto_enrich_line.ProcessLevel))
                {
                    ontology_entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict[onto_enrich_line.Ontology_type][onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName].Add(onto_enrich_line.ProcessLevel, new Dictionary<float, List<Ontology_enrichment_line_class>>());
                }
                if (!ontology_entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict[onto_enrich_line.Ontology_type][onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName][onto_enrich_line.ProcessLevel].ContainsKey(onto_enrich_line.Minus_log10_pvalue))
                {
                    ontology_entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict[onto_enrich_line.Ontology_type][onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName][onto_enrich_line.ProcessLevel].Add(onto_enrich_line.Minus_log10_pvalue, new List<Ontology_enrichment_line_class>());
                }
                ontology_entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict[onto_enrich_line.Ontology_type][onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName][onto_enrich_line.ProcessLevel][onto_enrich_line.Minus_log10_pvalue].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            Ontology_type_enum[] ontologies;
            Ontology_type_enum ontology;
            int ontologies_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            float[] timepointsInDays;
            float timepointInDay;
            int timepointsInDays_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            int[] processLevels;
            int processLevel;
            int processLevels_length;
            float[] minusLog10Pvalues;
            float minusLog10Pvalue;
            int minusLog10Pvalues_length;
            ontologies = ontology_entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict.Keys.ToArray();
            ontologies = ontologies.OrderBy(l => l).ToArray();
            ontologies_length = ontologies.Length;
            for (int indexO = 0; indexO < ontologies_length; indexO++)
            {
                ontology = ontologies[indexO];
                entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict = ontology_entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict[ontology];
                entryTypes = entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict.Keys.ToArray();
                entryTypes = entryTypes.OrderBy(l => l).ToArray();
                entryTypes_length = entryTypes.Length;
                for (int indexE = 0; indexE < entryTypes_length; indexE++)
                {
                    entryType = entryTypes[indexE];
                    timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict = entryType_timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict[entryType];
                    timepointsInDays = timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict.Keys.ToArray();
                    timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
                    timepointsInDays_length = timepointsInDays.Length;
                    for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                    {
                        timepointInDay = timepointsInDays[indexT];
                        sampleName_processLevel_minusLog10Pvalue_dict = timepointInDays_sampleName_processLevel_minusLog10Pvalue_dict[timepointInDay];
                        sampleNames = sampleName_processLevel_minusLog10Pvalue_dict.Keys.ToArray();
                        sampleNames = sampleNames.OrderBy(l => l).ToArray();
                        sampleNames_length = sampleNames.Length;
                        for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                        {
                            sampleName = sampleNames[indexSN];
                            processLevel_minusLog10Pvalue_dict = sampleName_processLevel_minusLog10Pvalue_dict[sampleName];
                            processLevels = processLevel_minusLog10Pvalue_dict.Keys.ToArray();
                            processLevels = processLevels.OrderBy(l => l).ToArray();
                            processLevels_length = processLevels.Length;
                            for (int indexPL = 0; indexPL < processLevels_length; indexPL++)
                            {
                                processLevel = processLevels[indexPL];
                                minusLog10Pvalue_dict = processLevel_minusLog10Pvalue_dict[processLevel];
                                minusLog10Pvalues = minusLog10Pvalue_dict.Keys.ToArray();
                                minusLog10Pvalues = minusLog10Pvalues.OrderByDescending(l => l).ToArray();
                                minusLog10Pvalues_length = minusLog10Pvalues.Length;
                                for (int indexMLP=0; indexMLP<minusLog10Pvalues_length; indexMLP++)
                                {
                                    minusLog10Pvalue = minusLog10Pvalues[indexMLP];
                                    ordered_lines.AddRange(minusLog10Pvalue_dict[minusLog10Pvalue]);
                                }
                            }
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //ontology_entryType_timepointInDays_sampleName_processLevel_descendingMinusLog10Pvalue
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    this_timepointInDays = this_line.TimepointInDays;
                    previous_timepointInDays = previous_line.TimepointInDays;
                    if (this_line.Ontology_type.CompareTo(previous_line.Ontology_type) < 0) { throw new Exception(); }
                    else if ((this_line.Ontology_type.Equals(previous_line.Ontology_type))
                             && (this_line.EntryType.CompareTo(previous_line.EntryType) < 0)) { throw new Exception(); }
                    else if ((this_line.Ontology_type.Equals(previous_line.Ontology_type))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                    else if ((this_line.Ontology_type.Equals(previous_line.Ontology_type))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    else if ((this_line.Ontology_type.Equals(previous_line.Ontology_type))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.ProcessLevel.CompareTo(previous_line.ProcessLevel) < 0)) { throw new Exception(); }
                    else if ((this_line.Ontology_type.Equals(previous_line.Ontology_type))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.ProcessLevel.Equals(previous_line.ProcessLevel))
                             && (this_line.Minus_log10_pvalue.CompareTo(previous_line.Minus_log10_pvalue) > 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_processLevel_entryType_timepointInDays_sampleName(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            //onto_enrich_array = onto_enrich_array.OrderBy(l => l.ProcessLevel).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.SampleName).ToArray();
            Dictionary<int, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>>> processLevel_entryType_timepointInDays_sampleName_dict = new Dictionary<int, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>>>();
            Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>> entryType_timepointInDays_sampleName_dict = new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>>();
            Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>> timepointInDays_sampleName_dict = new Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>();
            Dictionary<string, List<Ontology_enrichment_line_class>> sampleName_dict = new Dictionary<string, List<Ontology_enrichment_line_class>>();

            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepoint_in_days;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepoint_in_days = onto_enrich_line.TimepointInDays;
                if (!processLevel_entryType_timepointInDays_sampleName_dict.ContainsKey(onto_enrich_line.ProcessLevel))
                {
                    processLevel_entryType_timepointInDays_sampleName_dict.Add(onto_enrich_line.ProcessLevel, new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>>());
                }
                if (!processLevel_entryType_timepointInDays_sampleName_dict[onto_enrich_line.ProcessLevel].ContainsKey(onto_enrich_line.EntryType))
                {
                    processLevel_entryType_timepointInDays_sampleName_dict[onto_enrich_line.ProcessLevel].Add(onto_enrich_line.EntryType, new Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>());
                }
                if (!processLevel_entryType_timepointInDays_sampleName_dict[onto_enrich_line.ProcessLevel][onto_enrich_line.EntryType].ContainsKey(timepoint_in_days))
                {
                    processLevel_entryType_timepointInDays_sampleName_dict[onto_enrich_line.ProcessLevel][onto_enrich_line.EntryType].Add(timepoint_in_days, new Dictionary<string, List<Ontology_enrichment_line_class>>());
                }
                if (!processLevel_entryType_timepointInDays_sampleName_dict[onto_enrich_line.ProcessLevel][onto_enrich_line.EntryType][timepoint_in_days].ContainsKey(onto_enrich_line.SampleName))
                {
                    processLevel_entryType_timepointInDays_sampleName_dict[onto_enrich_line.ProcessLevel][onto_enrich_line.EntryType][timepoint_in_days].Add(onto_enrich_line.SampleName, new List<Ontology_enrichment_line_class>());
                }
                processLevel_entryType_timepointInDays_sampleName_dict[onto_enrich_line.ProcessLevel][onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            int[] processLevels;
            int processLevel;
            int processLevels_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            float[] timepointsInDays;
            float timepointInDay;
            int timepointsInDays_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            processLevels = processLevel_entryType_timepointInDays_sampleName_dict.Keys.ToArray();
            processLevels = processLevels.OrderBy(l => l).ToArray();
            processLevels_length = processLevels.Length;
            for (int indexL = 0; indexL < processLevels_length; indexL++)
            {
                processLevel = processLevels[indexL];
                entryType_timepointInDays_sampleName_dict = processLevel_entryType_timepointInDays_sampleName_dict[processLevel];
                entryTypes = entryType_timepointInDays_sampleName_dict.Keys.ToArray();
                entryTypes = entryTypes.OrderBy(l => l).ToArray();
                entryTypes_length = entryTypes.Length;
                for (int indexE = 0; indexE < entryTypes_length; indexE++)
                {
                    entryType = entryTypes[indexE];
                    timepointInDays_sampleName_dict = entryType_timepointInDays_sampleName_dict[entryType];
                    timepointsInDays = timepointInDays_sampleName_dict.Keys.ToArray();
                    timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
                    timepointsInDays_length = timepointsInDays.Length;
                    for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                    {
                        timepointInDay = timepointsInDays[indexT];
                        sampleName_dict = timepointInDays_sampleName_dict[timepointInDay];
                        sampleNames = sampleName_dict.Keys.ToArray();
                        sampleNames = sampleNames.OrderBy(l => l).ToArray();
                        sampleNames_length = sampleNames.Length;
                        for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                        {
                            sampleName = sampleNames[indexSN];
                            ordered_lines.AddRange(sampleName_dict[sampleName]);
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //processLevel_entryType_timepointInDays_sampleName
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    this_timepointInDays = this_line.TimepointInDays;
                    previous_timepointInDays = previous_line.TimepointInDays;
                    if (this_line.ProcessLevel.CompareTo(previous_line.ProcessLevel) < 0) { throw new Exception(); }
                    else if ((this_line.ProcessLevel.Equals(previous_line.ProcessLevel))
                             && (this_line.EntryType.CompareTo(previous_line.EntryType) < 0)) { throw new Exception(); }
                    else if ((this_line.ProcessLevel.Equals(previous_line.ProcessLevel))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                    else if ((this_line.ProcessLevel.Equals(previous_line.ProcessLevel))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_by_integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            //this.Enrich.OrderBy(l => l.IntegrationGroup).ThenBy(l => l.SampleName).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.Timeunit).ThenBy(l => l.ProcessLevel).ThenBy(l => l.Pvalue).ToArray();
            Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>>>>> integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue_dict = new Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>>>>>();
            Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>>>> sampleName_entryType_timepointInDays_processLevel_pvalue_dict = new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>>>>();
            Dictionary<Entry_type_enum, Dictionary<float, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>>> entryType_timepointInDays_processLevel_pvalue_dict = new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>>>();
            Dictionary<float, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>> timepointInDays_processLevel_pvalue_dict = new Dictionary<float, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>>();
            Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>> processLevel_pvalue_dict = new Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>();
            Dictionary<double, List<Ontology_enrichment_line_class>> pvalue_dict = new Dictionary<double, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepoint_in_days;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepoint_in_days = onto_enrich_line.TimepointInDays;
                if (!integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue_dict.ContainsKey(onto_enrich_line.IntegrationGroup))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue_dict.Add(onto_enrich_line.IntegrationGroup, new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>>>>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue_dict[onto_enrich_line.IntegrationGroup].ContainsKey(onto_enrich_line.SampleName))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue_dict[onto_enrich_line.IntegrationGroup].Add(onto_enrich_line.SampleName, new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>>>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName].ContainsKey(onto_enrich_line.EntryType))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName].Add(onto_enrich_line.EntryType, new Dictionary<float, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType].ContainsKey(timepoint_in_days))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType].Add(timepoint_in_days, new Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType][timepoint_in_days].ContainsKey(onto_enrich_line.ProcessLevel))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType][timepoint_in_days].Add(onto_enrich_line.ProcessLevel, new Dictionary<double, List<Ontology_enrichment_line_class>>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.ProcessLevel].ContainsKey(onto_enrich_line.Pvalue))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.ProcessLevel].Add(onto_enrich_line.Pvalue, new List<Ontology_enrichment_line_class>());
                }
                integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.ProcessLevel][onto_enrich_line.Pvalue].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            float[] timepointsInDays;
            float timepointInDay;
            int timepointsInDays_length;
            int[] processLevels;
            int processLevel;
            int processLevels_length;
            double[] pvalues;
            double pvalue;
            int pvalues_length;
            integrationGroups = integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue_dict.Keys.ToArray();
            integrationGroups = integrationGroups.OrderBy(l => l).ToArray();
            integrationGroups_length = integrationGroups.Length;
            for (int indexIG = 0; indexIG < integrationGroups_length; indexIG++)
            {
                integrationGroup = integrationGroups[indexIG];
                sampleName_entryType_timepointInDays_processLevel_pvalue_dict = integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue_dict[integrationGroup];
                sampleNames = sampleName_entryType_timepointInDays_processLevel_pvalue_dict.Keys.ToArray();
                sampleNames = sampleNames.OrderBy(l => l).ToArray();
                sampleNames_length = sampleNames.Length;
                for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                {
                    sampleName = sampleNames[indexSN];
                    entryType_timepointInDays_processLevel_pvalue_dict = sampleName_entryType_timepointInDays_processLevel_pvalue_dict[sampleName];
                    entryTypes = entryType_timepointInDays_processLevel_pvalue_dict.Keys.ToArray();
                    entryTypes = entryTypes.OrderBy(l => l).ToArray();
                    entryTypes_length = entryTypes.Length;
                    for (int indexE = 0; indexE < entryTypes_length; indexE++)
                    {
                        entryType = entryTypes[indexE];
                        timepointInDays_processLevel_pvalue_dict = entryType_timepointInDays_processLevel_pvalue_dict[entryType];
                        timepointsInDays = timepointInDays_processLevel_pvalue_dict.Keys.ToArray();
                        timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
                        timepointsInDays_length = timepointsInDays.Length;
                        for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                        {
                            timepointInDay = timepointsInDays[indexT];
                            processLevel_pvalue_dict = timepointInDays_processLevel_pvalue_dict[timepointInDay];
                            processLevels = processLevel_pvalue_dict.Keys.ToArray();
                            processLevels = processLevels.OrderBy(l => l).ToArray();
                            processLevels_length = processLevels.Length;
                            for (int indexPL = 0; indexPL < processLevels_length; indexPL++)
                            {
                                processLevel = processLevels[indexPL];
                                pvalue_dict = processLevel_pvalue_dict[processLevel];
                                pvalues = pvalue_dict.Keys.ToArray();
                                pvalues = pvalues.OrderBy(l => l).ToArray();
                                pvalues_length = pvalues.Length;
                                for (int indexPV = 0; indexPV < pvalues_length; indexPV++)
                                {
                                    pvalue = pvalues[indexPV];
                                    ordered_lines.AddRange(pvalue_dict[pvalue]);
                                }
                            }
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    this_timepointInDays = this_line.TimepointInDays;
                    previous_timepointInDays = previous_line.TimepointInDays;
                    if (this_line.IntegrationGroup.CompareTo(previous_line.IntegrationGroup) < 0) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.EntryType.CompareTo(previous_line.EntryType) < 0)) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.ProcessLevel.CompareTo(previous_line.ProcessLevel) < 0)) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.ProcessLevel.Equals(previous_line.ProcessLevel))
                             && (this_line.Pvalue.CompareTo(previous_line.Pvalue) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();

        }
        public static Ontology_enrichment_line_class[] Order_by_integrationGroup_sampleName_entryType_timepointInDays_pvalue(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            //this.Enrich.OrderBy(l => l.IntegrationGroup).ThenBy(l => l.SampleName).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.Timeunit).ThenBy(l => l.ProcessLevel).ThenBy(l => l.Pvalue).ToArray();
            Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<double, List<Ontology_enrichment_line_class>>>>>> integrationGroup_sampleName_entryType_timepointInDays_pvalue_dict = new Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<double, List<Ontology_enrichment_line_class>>>>>>();
            Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<double, List<Ontology_enrichment_line_class>>>>> sampleName_entryType_timepointInDays_pvalue_dict = new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<double, List<Ontology_enrichment_line_class>>>>>();
            Dictionary<Entry_type_enum, Dictionary<float, Dictionary<double, List<Ontology_enrichment_line_class>>>> entryType_timepointInDays_pvalue_dict = new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<double, List<Ontology_enrichment_line_class>>>>();
            Dictionary<float, Dictionary<double, List<Ontology_enrichment_line_class>>> timepointInDays_pvalue_dict = new Dictionary<float, Dictionary<double, List<Ontology_enrichment_line_class>>>();
            Dictionary<double, List<Ontology_enrichment_line_class>> pvalue_dict = new Dictionary<double, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepoint_in_days;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepoint_in_days = onto_enrich_line.TimepointInDays;
                if (!integrationGroup_sampleName_entryType_timepointInDays_pvalue_dict.ContainsKey(onto_enrich_line.IntegrationGroup))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_pvalue_dict.Add(onto_enrich_line.IntegrationGroup, new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<double, List<Ontology_enrichment_line_class>>>>>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_pvalue_dict[onto_enrich_line.IntegrationGroup].ContainsKey(onto_enrich_line.SampleName))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_pvalue_dict[onto_enrich_line.IntegrationGroup].Add(onto_enrich_line.SampleName, new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<double, List<Ontology_enrichment_line_class>>>>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_pvalue_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName].ContainsKey(onto_enrich_line.EntryType))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_pvalue_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName].Add(onto_enrich_line.EntryType, new Dictionary<float, Dictionary<double, List<Ontology_enrichment_line_class>>>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_pvalue_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType].ContainsKey(timepoint_in_days))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_pvalue_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType].Add(timepoint_in_days, new Dictionary<double, List<Ontology_enrichment_line_class>>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_pvalue_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType][timepoint_in_days].ContainsKey(onto_enrich_line.Pvalue))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_pvalue_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType][timepoint_in_days].Add(onto_enrich_line.Pvalue, new List<Ontology_enrichment_line_class>());
                }
                integrationGroup_sampleName_entryType_timepointInDays_pvalue_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.Pvalue].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            float[] timepointsInDays;
            float timepointInDay;
            int timepointsInDays_length;
            double[] pvalues;
            double pvalue;
            int pvalues_length;
            integrationGroups = integrationGroup_sampleName_entryType_timepointInDays_pvalue_dict.Keys.ToArray();
            integrationGroups = integrationGroups.OrderBy(l => l).ToArray();
            integrationGroups_length = integrationGroups.Length;
            for (int indexIG = 0; indexIG < integrationGroups_length; indexIG++)
            {
                integrationGroup = integrationGroups[indexIG];
                sampleName_entryType_timepointInDays_pvalue_dict = integrationGroup_sampleName_entryType_timepointInDays_pvalue_dict[integrationGroup];
                sampleNames = sampleName_entryType_timepointInDays_pvalue_dict.Keys.ToArray();
                sampleNames = sampleNames.OrderBy(l => l).ToArray();
                sampleNames_length = sampleNames.Length;
                for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                {
                    sampleName = sampleNames[indexSN];
                    entryType_timepointInDays_pvalue_dict = sampleName_entryType_timepointInDays_pvalue_dict[sampleName];
                    entryTypes = entryType_timepointInDays_pvalue_dict.Keys.ToArray();
                    entryTypes = entryTypes.OrderBy(l => l).ToArray();
                    entryTypes_length = entryTypes.Length;
                    for (int indexE = 0; indexE < entryTypes_length; indexE++)
                    {
                        entryType = entryTypes[indexE];
                        timepointInDays_pvalue_dict = entryType_timepointInDays_pvalue_dict[entryType];
                        timepointsInDays = timepointInDays_pvalue_dict.Keys.ToArray();
                        timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
                        timepointsInDays_length = timepointsInDays.Length;
                        for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                        {
                            timepointInDay = timepointsInDays[indexT];
                            pvalue_dict = timepointInDays_pvalue_dict[timepointInDay];
                            pvalues = pvalue_dict.Keys.ToArray();
                            pvalues = pvalues.OrderBy(l => l).ToArray();
                            pvalues_length = pvalues.Length;
                            for (int indexPV = 0; indexPV < pvalues_length; indexPV++)
                            {
                                pvalue = pvalues[indexPV];
                                ordered_lines.AddRange(pvalue_dict[pvalue]);
                            }
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    this_timepointInDays = this_line.TimepointInDays;
                    previous_timepointInDays = previous_line.TimepointInDays;
                    if (this_line.IntegrationGroup.CompareTo(previous_line.IntegrationGroup) < 0) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.EntryType.CompareTo(previous_line.EntryType) < 0)) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.Pvalue.CompareTo(previous_line.Pvalue) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();

        }
        public static Ontology_enrichment_line_class[] Order_by_integrationGroup_sampleName_timepointInDays(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<string, Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>> integrationGroup_sampleName_timepointInDays_dict = new Dictionary<string, Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>>();
            Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>> sampleName_timepointInDays_dict = new Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>();
            Dictionary<float, List<Ontology_enrichment_line_class>> timepointInDays_dict = new Dictionary<float, List<Ontology_enrichment_line_class>>();
            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepoint_in_days;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepoint_in_days = onto_enrich_line.TimepointInDays;
                if (!integrationGroup_sampleName_timepointInDays_dict.ContainsKey(onto_enrich_line.IntegrationGroup))
                {
                    integrationGroup_sampleName_timepointInDays_dict.Add(onto_enrich_line.IntegrationGroup, new Dictionary<string, Dictionary<float, List<Ontology_enrichment_line_class>>>());
                }
                if (!integrationGroup_sampleName_timepointInDays_dict[onto_enrich_line.IntegrationGroup].ContainsKey(onto_enrich_line.SampleName))
                {
                    integrationGroup_sampleName_timepointInDays_dict[onto_enrich_line.IntegrationGroup].Add(onto_enrich_line.SampleName, new Dictionary<float, List<Ontology_enrichment_line_class>>());
                }
                if (!integrationGroup_sampleName_timepointInDays_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName].ContainsKey(timepoint_in_days))
                {
                    integrationGroup_sampleName_timepointInDays_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName].Add(timepoint_in_days, new List<Ontology_enrichment_line_class>());
                }
                integrationGroup_sampleName_timepointInDays_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][timepoint_in_days].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            float[] timepointsInDays;
            float timepointInDay;
            int timepointsInDays_length;
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
                    timepointsInDays = timepointInDays_dict.Keys.ToArray();
                    timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
                    timepointsInDays_length = timepointsInDays.Length;
                    for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                    {
                        timepointInDay = timepointsInDays[indexT];
                        ordered_lines.AddRange(timepointInDays_dict[timepointInDay]);
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //integrationGroup_sampleName_timepointInDays
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    this_timepointInDays = this_line.TimepointInDays;
                    previous_timepointInDays = previous_line.TimepointInDays;
                    if (this_line.IntegrationGroup.CompareTo(previous_line.IntegrationGroup) < 0) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup.Equals(previous_line.IntegrationGroup))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();

        }
        public static Ontology_enrichment_line_class[] Order_entryType_timepointInDays_sampleName(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            //onto_enrich_array = onto_enrich_array.OrderBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.SampleName).ToArray();
            Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>> entryType_timepointInDays_sampleName_dict = new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>>();
            Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>> timepointInDays_sampleName_dict = new Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>();
            Dictionary<string, List<Ontology_enrichment_line_class>> sampleName_dict = new Dictionary<string, List<Ontology_enrichment_line_class>>();

            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepoint_in_days;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepoint_in_days = onto_enrich_line.TimepointInDays;
                if (!entryType_timepointInDays_sampleName_dict.ContainsKey(onto_enrich_line.EntryType))
                {
                    entryType_timepointInDays_sampleName_dict.Add(onto_enrich_line.EntryType, new Dictionary<float, Dictionary<string, List<Ontology_enrichment_line_class>>>());
                }
                if (!entryType_timepointInDays_sampleName_dict[onto_enrich_line.EntryType].ContainsKey(timepoint_in_days))
                {
                    entryType_timepointInDays_sampleName_dict[onto_enrich_line.EntryType].Add(timepoint_in_days, new Dictionary<string, List<Ontology_enrichment_line_class>>());
                }
                if (!entryType_timepointInDays_sampleName_dict[onto_enrich_line.EntryType][timepoint_in_days].ContainsKey(onto_enrich_line.SampleName))
                {
                    entryType_timepointInDays_sampleName_dict[onto_enrich_line.EntryType][timepoint_in_days].Add(onto_enrich_line.SampleName, new List<Ontology_enrichment_line_class>());
                }
                entryType_timepointInDays_sampleName_dict[onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            float[] timepointsInDays;
            float timepointInDay;
            int timepointsInDays_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            entryTypes = entryType_timepointInDays_sampleName_dict.Keys.ToArray();
            entryTypes = entryTypes.OrderBy(l => l).ToArray();
            entryTypes_length = entryTypes.Length;
            for (int indexE = 0; indexE < entryTypes_length; indexE++)
            {
                entryType = entryTypes[indexE];
                timepointInDays_sampleName_dict = entryType_timepointInDays_sampleName_dict[entryType];
                timepointsInDays = timepointInDays_sampleName_dict.Keys.ToArray();
                timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
                timepointsInDays_length = timepointsInDays.Length;
                for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                {
                    timepointInDay = timepointsInDays[indexT];
                    sampleName_dict = timepointInDays_sampleName_dict[timepointInDay];
                    sampleNames = sampleName_dict.Keys.ToArray();
                    sampleNames = sampleNames.OrderBy(l => l).ToArray();
                    sampleNames_length = sampleNames.Length;
                    for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                    {
                        sampleName = sampleNames[indexSN];
                        ordered_lines.AddRange(sampleName_dict[sampleName]);
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //entryType_timepointInDays_sampleName
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    this_timepointInDays = this_line.TimepointInDays;
                    previous_timepointInDays = previous_line.TimepointInDays;
                    if ((this_line.EntryType.CompareTo(previous_line.EntryType) < 0)) { throw new Exception(); }
                    else if ((this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                    else if ((this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_sampleName_entryType_timepoint(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, List<Ontology_enrichment_line_class>>>> sampleName_entryType_timepoint_dict = new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, List<Ontology_enrichment_line_class>>>>();
            Dictionary<Entry_type_enum, Dictionary<float, List<Ontology_enrichment_line_class>>> entryType_timepoint_dict = new Dictionary<Entry_type_enum, Dictionary<float, List<Ontology_enrichment_line_class>>>();
            Dictionary<float, List<Ontology_enrichment_line_class>> timepoint_dict = new Dictionary<float, List<Ontology_enrichment_line_class>>();

            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                if (!sampleName_entryType_timepoint_dict.ContainsKey(onto_enrich_line.SampleName))
                {
                    sampleName_entryType_timepoint_dict.Add(onto_enrich_line.SampleName, new Dictionary<Entry_type_enum, Dictionary<float, List<Ontology_enrichment_line_class>>>());
                }
                if (!sampleName_entryType_timepoint_dict[onto_enrich_line.SampleName].ContainsKey(onto_enrich_line.EntryType))
                {
                    sampleName_entryType_timepoint_dict[onto_enrich_line.SampleName].Add(onto_enrich_line.EntryType, new Dictionary<float, List<Ontology_enrichment_line_class>>());
                }
                if (!sampleName_entryType_timepoint_dict[onto_enrich_line.SampleName][onto_enrich_line.EntryType].ContainsKey(onto_enrich_line.Timepoint))
                {
                    sampleName_entryType_timepoint_dict[onto_enrich_line.SampleName][onto_enrich_line.EntryType].Add(onto_enrich_line.Timepoint, new List<Ontology_enrichment_line_class>());
                }
                sampleName_entryType_timepoint_dict[onto_enrich_line.SampleName][onto_enrich_line.EntryType][onto_enrich_line.Timepoint].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            float[] timepoints;
            float timepoint;
            int timepoints_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            sampleNames = sampleName_entryType_timepoint_dict.Keys.ToArray();
            sampleNames = sampleNames.OrderBy(l => l).ToArray();
            sampleNames_length = sampleNames.Length;
            for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
            {
                sampleName = sampleNames[indexSN];
                entryType_timepoint_dict = sampleName_entryType_timepoint_dict[sampleName];
                entryTypes = entryType_timepoint_dict.Keys.ToArray();
                entryTypes = entryTypes.OrderBy(l => l).ToArray();
                entryTypes_length = entryTypes.Length;
                for (int indexET = 0; indexET < entryTypes_length; indexET++)
                {
                    entryType = entryTypes[indexET];
                    timepoint_dict = entryType_timepoint_dict[entryType];
                    timepoints = timepoint_dict.Keys.ToArray();
                    timepoints = timepoints.OrderBy(l => l).ToArray();
                    timepoints_length = timepoints.Length;
                    for (int indexT = 0; indexT < timepoints_length; indexT++)
                    {
                        timepoint = timepoints[indexT];
                        ordered_lines.AddRange(timepoint_dict[timepoint]);
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                //sampleName_entryType_timepoint
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if ((this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    else if ((this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.EntryType.CompareTo(previous_line.EntryType) < 0)) { throw new Exception(); }
                    else if ((this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_line.Timepoint.CompareTo(previous_line.Timepoint) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_sampleName_processLevel_timepointInDays(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            Dictionary<string, Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>> sampleName_processLevel_timepointinDays_dict = new Dictionary<string, Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>>();
            Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>> processLevel_timepointInDays_dict = new Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>();
            Dictionary<float, List<Ontology_enrichment_line_class>> timepointInDays_dict = new Dictionary<float, List<Ontology_enrichment_line_class>>();

            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepointInDays;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepointInDays = onto_enrich_line.TimepointInDays;
                if (!sampleName_processLevel_timepointinDays_dict.ContainsKey(onto_enrich_line.SampleName))
                {
                    sampleName_processLevel_timepointinDays_dict.Add(onto_enrich_line.SampleName, new Dictionary<int, Dictionary<float, List<Ontology_enrichment_line_class>>>());
                }
                if (!sampleName_processLevel_timepointinDays_dict[onto_enrich_line.SampleName].ContainsKey(onto_enrich_line.ProcessLevel))
                {
                    sampleName_processLevel_timepointinDays_dict[onto_enrich_line.SampleName].Add(onto_enrich_line.ProcessLevel, new Dictionary<float, List<Ontology_enrichment_line_class>>());
                }
                if (!sampleName_processLevel_timepointinDays_dict[onto_enrich_line.SampleName][onto_enrich_line.ProcessLevel].ContainsKey(timepointInDays))
                {
                    sampleName_processLevel_timepointinDays_dict[onto_enrich_line.SampleName][onto_enrich_line.ProcessLevel].Add(timepointInDays, new List<Ontology_enrichment_line_class>());
                }
                sampleName_processLevel_timepointinDays_dict[onto_enrich_line.SampleName][onto_enrich_line.ProcessLevel][timepointInDays].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            int[] processLevels;
            int processLevel;
            int processLevels_length;
            float[] timepointsInDays;
            int timepointsInDays_length;
            sampleNames = sampleName_processLevel_timepointinDays_dict.Keys.ToArray();
            sampleNames = sampleNames.OrderBy(l => l).ToArray();
            sampleNames_length = sampleNames.Length;
            for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
            {
                sampleName = sampleNames[indexSN];
                processLevel_timepointInDays_dict = sampleName_processLevel_timepointinDays_dict[sampleName];
                processLevels = processLevel_timepointInDays_dict.Keys.ToArray();
                processLevels = processLevels.OrderBy(l => l).ToArray();
                processLevels_length = processLevels.Length;
                for (int indexPL = 0; indexPL < processLevels_length; indexPL++)
                {
                    processLevel = processLevels[indexPL];
                    timepointInDays_dict = processLevel_timepointInDays_dict[processLevel];
                    timepointsInDays = timepointInDays_dict.Keys.ToArray();
                    timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
                    timepointsInDays_length = timepointsInDays.Length;
                    for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                    {
                        timepointInDays = timepointsInDays[indexT];
                        ordered_lines.AddRange(timepointInDays_dict[timepointInDays]);
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                //sampleName_processLevel_timepointInDays
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if ((this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    else if ((this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.ProcessLevel.CompareTo(previous_line.ProcessLevel) < 0)) { throw new Exception(); }
                    else if ((this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.ProcessLevel.Equals(previous_line.ProcessLevel))
                             && (this_line.TimepointInDays.CompareTo(previous_line.TimepointInDays) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public static Ontology_enrichment_line_class[] Order_entryType_timepointInDays_sampleName_processLevel_pvalue(Ontology_enrichment_line_class[] onto_enrich_array)
        {
            //onto_enrich_array = onto_enrich_array.OrderBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.SampleName).ThenBy(l=>l.ProcessLevel).ThenBy(l=>l.Pvalue).ToArray();
            Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>>>> entryType_timepointInDays_sampleName_level_pvalue_dict = new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>>>>();
            Dictionary<float, Dictionary<string, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>>> timepointInDays_sampleName_level_pvalue_dict = new Dictionary<float, Dictionary<string, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>>>();
            Dictionary<string, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>> sampleName_level_pvalue_dict = new Dictionary<string, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>>();
            Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>> level_pvalue_dict = new Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>();
            Dictionary<double, List<Ontology_enrichment_line_class>> pvalue_dict = new Dictionary<double, List<Ontology_enrichment_line_class>>();

            int onto_enrich_array_length = onto_enrich_array.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            float timepoint_in_days;
            for (int indexO = 0; indexO < onto_enrich_array_length; indexO++)
            {
                onto_enrich_line = onto_enrich_array[indexO];
                timepoint_in_days = onto_enrich_line.TimepointInDays;
                if (!entryType_timepointInDays_sampleName_level_pvalue_dict.ContainsKey(onto_enrich_line.EntryType))
                {
                    entryType_timepointInDays_sampleName_level_pvalue_dict.Add(onto_enrich_line.EntryType, new Dictionary<float, Dictionary<string, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>>>());
                }
                if (!entryType_timepointInDays_sampleName_level_pvalue_dict[onto_enrich_line.EntryType].ContainsKey(timepoint_in_days))
                {
                    entryType_timepointInDays_sampleName_level_pvalue_dict[onto_enrich_line.EntryType].Add(timepoint_in_days, new Dictionary<string, Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>>());
                }
                if (!entryType_timepointInDays_sampleName_level_pvalue_dict[onto_enrich_line.EntryType][timepoint_in_days].ContainsKey(onto_enrich_line.SampleName))
                {
                    entryType_timepointInDays_sampleName_level_pvalue_dict[onto_enrich_line.EntryType][timepoint_in_days].Add(onto_enrich_line.SampleName, new Dictionary<int, Dictionary<double, List<Ontology_enrichment_line_class>>>());
                }
                if (!entryType_timepointInDays_sampleName_level_pvalue_dict[onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName].ContainsKey(onto_enrich_line.ProcessLevel))
                {
                    entryType_timepointInDays_sampleName_level_pvalue_dict[onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName].Add(onto_enrich_line.ProcessLevel, new Dictionary<double, List<Ontology_enrichment_line_class>>());
                }
                if (!entryType_timepointInDays_sampleName_level_pvalue_dict[onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName][onto_enrich_line.ProcessLevel].ContainsKey(onto_enrich_line.Pvalue))
                {
                    entryType_timepointInDays_sampleName_level_pvalue_dict[onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName][onto_enrich_line.ProcessLevel].Add(onto_enrich_line.Pvalue, new List<Ontology_enrichment_line_class>());
                }
                entryType_timepointInDays_sampleName_level_pvalue_dict[onto_enrich_line.EntryType][timepoint_in_days][onto_enrich_line.SampleName][onto_enrich_line.ProcessLevel][onto_enrich_line.Pvalue].Add(onto_enrich_line);
            }
            onto_enrich_array = null;
            List<Ontology_enrichment_line_class> ordered_lines = new List<Ontology_enrichment_line_class>();
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            float[] timepointsInDays;
            float timepointInDay;
            int timepointsInDays_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            int[] processLevels;
            int processLevel;
            int processLevels_length;
            double[] pvalues;
            double pvalue;
            int pvalues_length;
            entryTypes = entryType_timepointInDays_sampleName_level_pvalue_dict.Keys.ToArray();
            entryTypes = entryTypes.OrderBy(l => l).ToArray();
            entryTypes_length = entryTypes.Length;
            for (int indexET = 0; indexET < entryTypes_length; indexET++)
            {
                entryType = entryTypes[indexET];
                timepointInDays_sampleName_level_pvalue_dict = entryType_timepointInDays_sampleName_level_pvalue_dict[entryType];
                timepointsInDays = timepointInDays_sampleName_level_pvalue_dict.Keys.ToArray();
                timepointsInDays = timepointsInDays.OrderBy(l => l).ToArray();
                timepointsInDays_length = timepointsInDays.Length;
                for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                {
                    timepointInDay = timepointsInDays[indexT];
                    sampleName_level_pvalue_dict = timepointInDays_sampleName_level_pvalue_dict[timepointInDay];
                    sampleNames = sampleName_level_pvalue_dict.Keys.ToArray();
                    sampleNames = sampleNames.OrderBy(l => l).ToArray();
                    sampleNames_length = sampleNames.Length;
                    for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                    {
                        sampleName = sampleNames[indexSN];
                        level_pvalue_dict = sampleName_level_pvalue_dict[sampleName];
                        processLevels = level_pvalue_dict.Keys.ToArray();
                        processLevels = processLevels.OrderBy(l => l).ToArray();
                        processLevels_length = processLevels.Length;
                        for (int indexPL = 0; indexPL < processLevels_length; indexPL++)
                        {
                            processLevel = processLevels[indexPL];
                            pvalue_dict = level_pvalue_dict[processLevel];
                            pvalues = pvalue_dict.Keys.ToArray();
                            pvalues = pvalues.OrderBy(l => l).ToArray();
                            pvalues_length = pvalues.Length;
                            for (int indexPV = 0; indexPV < pvalues_length; indexPV++)
                            {
                                pvalue = pvalues[indexPV];
                                ordered_lines.AddRange(pvalue_dict[pvalue]);
                            }
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != onto_enrich_array_length) { throw new Exception(); }
                Ontology_enrichment_line_class previous_line;
                Ontology_enrichment_line_class this_line;
                float this_timepointInDays;
                float previous_timepointInDays;
                //entryType_timepointInDays_sampleName_processLevel_pvalue
                for (int indexO = 1; indexO < onto_enrich_array_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    this_timepointInDays = this_line.TimepointInDays;
                    previous_timepointInDays = previous_line.TimepointInDays;
                    if (this_line.EntryType.CompareTo(previous_line.EntryType) < 0) { throw new Exception(); }
                    else if ((this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.CompareTo(previous_timepointInDays) < 0)) { throw new Exception(); }
                    else if ((this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.SampleName.CompareTo(previous_line.SampleName) < 0)) { throw new Exception(); }
                    else if ((this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.ProcessLevel.CompareTo(previous_line.ProcessLevel) < 0)) { throw new Exception(); }
                    else if ((this_line.EntryType.Equals(previous_line.EntryType))
                             && (this_timepointInDays.Equals(previous_timepointInDays))
                             && (this_line.SampleName.Equals(previous_line.SampleName))
                             && (this_line.ProcessLevel.Equals(previous_line.ProcessLevel))
                             && (this_line.Pvalue.CompareTo(previous_line.Pvalue) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }

        public bool Equal_complete_sample(Ontology_enrichment_line_class other)
        {
            bool equal = ((this.Ontology_type.Equals(other.Ontology_type))
                          && (this.EntryType.Equals(other.EntryType))
                          && (this.Timepoint.Equals(other.Timepoint))
                          && (this.SampleName.Equals(other.SampleName)));
            return equal;
        }
        #endregion

        public Ontology_enrichment_line_class Deep_copy()
        {
            Ontology_enrichment_line_class copy = (Ontology_enrichment_line_class)this.MemberwiseClone();
            copy.Scp_id = (string)this.Scp_id.Clone();
            copy.Scp_name = (string)this.Scp_name.Clone();
            copy.SampleName = (string)this.SampleName.Clone();
            copy.Parent_scp_name = (string)this.Parent_scp_name.Clone(); 
            copy.IntegrationGroup = (string)this.IntegrationGroup.Clone();
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
            key_propertyNames_list.AddRange(new string[] { "Ontology_type", "ProcessLevel", "ProcessDepth" });
            key_columnNames_list.AddRange(new string[] { "Ontology", "SCP level", "SCP depth" });
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
            //this.Enrich = this.Enrich.OrderBy(l => l.IntegrationGroup).ThenBy(l => l.Results_number).ToArray();
            this.Enrich = Ontology_enrichment_line_class.Order_by_integrationGroup_resultsNumber(this.Enrich);
            int custom_data_length = this.Enrich.Length;
            Ontology_enrichment_line_class previous_enrich_data_line;
            Ontology_enrichment_line_class this_enrich_data_line;
            Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, int>>> sampleName_timepointsInDays_entryType_resultsNumber_dict = new Dictionary<string, Dictionary<float, Dictionary<Entry_type_enum, int>>>();
            for (int indexC = 0; indexC < custom_data_length; indexC++)
            {
                this_enrich_data_line = this.Enrich[indexC];
                if (  (indexC==0)
                    ||(this_enrich_data_line.IntegrationGroup.Equals(this.Enrich[indexC-1].IntegrationGroup)))
                {
                    sampleName_timepointsInDays_entryType_resultsNumber_dict.Clear();
                }
                if (!sampleName_timepointsInDays_entryType_resultsNumber_dict.ContainsKey(this_enrich_data_line.SampleName))
                { sampleName_timepointsInDays_entryType_resultsNumber_dict.Add(this_enrich_data_line.SampleName, new Dictionary<float, Dictionary<Entry_type_enum, int>>()); }
                if (!sampleName_timepointsInDays_entryType_resultsNumber_dict[this_enrich_data_line.SampleName].ContainsKey(this_enrich_data_line.TimepointInDays))
                { sampleName_timepointsInDays_entryType_resultsNumber_dict[this_enrich_data_line.SampleName].Add(this_enrich_data_line.TimepointInDays, new Dictionary<Entry_type_enum, int>()); }
                if (!sampleName_timepointsInDays_entryType_resultsNumber_dict[this_enrich_data_line.SampleName][this_enrich_data_line.TimepointInDays].ContainsKey(this_enrich_data_line.EntryType))
                { sampleName_timepointsInDays_entryType_resultsNumber_dict[this_enrich_data_line.SampleName][this_enrich_data_line.TimepointInDays].Add(this_enrich_data_line.EntryType, this_enrich_data_line.Results_number); }
                else if (!sampleName_timepointsInDays_entryType_resultsNumber_dict[this_enrich_data_line.SampleName][this_enrich_data_line.TimepointInDays][this_enrich_data_line.EntryType].Equals(this_enrich_data_line.Results_number))
                { throw new Exception(); }

                if (indexC > 0)
                {
                    previous_enrich_data_line = this.Enrich[indexC - 1];
                    if (this_enrich_data_line.IntegrationGroup.Equals(previous_enrich_data_line.IntegrationGroup))
                    {
                        if ((previous_enrich_data_line.IntegrationGroup.Equals(this_enrich_data_line.IntegrationGroup))
                            && (previous_enrich_data_line.Results_number.Equals(this_enrich_data_line.Results_number)))
                        {
                            if (!previous_enrich_data_line.SampleName.Equals(this_enrich_data_line.SampleName)) { throw new Exception(); }
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
            string integrationGroup = Enrich[0].IntegrationGroup;
            foreach (Ontology_enrichment_line_class enrich_line in Enrich)
            {
                if (!enrich_line.IntegrationGroup.Equals(integrationGroup)) { throw new Exception(); }
            }
        }
        #endregion

        #region Order
        public void Order_by_scpName()
        {
            //Enrich = Enrich.OrderBy(l => l.Scp_name).ToArray();
            Enrich = Ontology_enrichment_line_class.Order_by_scpName(Enrich);
        }
        public void Order_by_level()
        {
            //Enrich = this.Enrich.OrderBy(l => l.ProcessLevel).ToArray();
            Enrich = Ontology_enrichment_line_class.Order_by_processLevel(Enrich);
        }
        public void Order_by_uniqueDatasetName_descendingMinusLog10pvalue_scpName()
        {
            //this.Enrich = this.Enrich.OrderBy(l => l.Unique_dataset_name).ThenByDescending(l => l.Minus_log10_pvalue).ThenBy(l => l.Scp_name).ToArray();
            this.Enrich = Ontology_enrichment_line_class.Order_by_uniqueDatasetName_descendingMinusLog10Pvalue_scpName(this.Enrich);
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
            //this.Enrich = this.Enrich.OrderBy(l => l.Ontology_type).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.SampleName).ThenBy(l => l.ProcessLevel).ThenByDescending(l => l.Minus_log10_pvalue).ToArray();
            this.Enrich = Ontology_enrichment_line_class.Order_by_ontology_entryType_timepointInDays_sampleName_processLevel_descendingMinusLog10Pvalue(this.Enrich);
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
            Dictionary<string, bool> keep_scpName_dict = new Dictionary<string, bool>();
            foreach (string keep_scpName in keep_scpNames)
            {
                keep_scpName_dict.Add(keep_scpName, true);
            }
            int enrich_length = this.Enrich.Length;
            Ontology_enrichment_line_class enrich_line;
            List<Ontology_enrichment_line_class> keep = new List<Ontology_enrichment_line_class>();
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = this.Enrich[indexE];
                if (keep_scpName_dict.ContainsKey(enrich_line.Scp_name))
                {
                    keep.Add(enrich_line);
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
                if (!integrationGroups_dict.ContainsKey(enrich_line.IntegrationGroup))
                {
                    integrationGroups_dict.Add(enrich_line.IntegrationGroup, true);
                }
            }
            return integrationGroups_dict.Keys.Distinct().OrderBy(l => l).ToArray();
        }

        public void Get_sampleName_timepoint_entryType_and_check_if_only_one(out string sampleName, out float timepoint, out Entry_type_enum entryType)
        {
            sampleName = (string)this.Enrich[0].SampleName.Clone();
            timepoint = this.Enrich[0].Timepoint;
            entryType = this.Enrich[0].EntryType;
            foreach (Ontology_enrichment_line_class enrich_line in this.Enrich)
            {
                if (  (!enrich_line.SampleName.Equals(sampleName))
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

        public void Get_networkNodeSizeDeterminant_max_and_min_values_dictionaries(out Dictionary<Yed_network_node_size_determinant_enum, float> networkNodeSizeDeterminant_maxValue_dict, out Dictionary<Yed_network_node_size_determinant_enum, float> networkNodeSizeDeterminant_minValue_dict)
        {
            Dictionary<string, Dictionary<string, Dictionary<string, bool>>> integrationGroup_scp_uniqueDatasetName_dict = new Dictionary<string, Dictionary<string, Dictionary<string, bool>>>();
            Dictionary<string, Dictionary<string, bool>> scp_uniqueDatasetName_dict = new Dictionary<string, Dictionary<string, bool>>();
            int uniqueDatasetsCount;
            Dictionary<string, Dictionary<string, Dictionary<Color, bool>>> integrationGroup_scp_color_dict = new Dictionary<string, Dictionary<string, Dictionary<Color, bool>>>();
            Dictionary<string, Dictionary<Color, bool>> scp_color_dict = new Dictionary<string, Dictionary<Color, bool>>();
            int uniqueColorsCount;
            Dictionary<string, Dictionary<string, float>> integrationGroup_scp_sumSplittedMinusLog10Pvalues_dict = new Dictionary<string, Dictionary<string, float>>();
            Dictionary<string, float> scp_sumSplittedMinusLog10Pvalues_dict = new Dictionary<string, float>();
            float sumSplittedMinusLog10Pvalue;
            string[] scps;
            string scp;
            int scps_length;

            //this.Enrich = this.Enrich.OrderByDescending(l => l.Minus_log10_pvalue).ToArray();
            this.Enrich = Ontology_enrichment_line_class.Order_by_descendingMinusLog10Pvalue(this.Enrich);

            foreach (Ontology_enrichment_line_class enrichment_line in this.Enrich)
            {
                scps = enrichment_line.Scp_name.Split('$');
                if (!integrationGroup_scp_color_dict.ContainsKey(enrichment_line.IntegrationGroup))
                {
                    integrationGroup_scp_uniqueDatasetName_dict.Add(enrichment_line.IntegrationGroup, new Dictionary<string, Dictionary<string, bool>>());
                    integrationGroup_scp_color_dict.Add(enrichment_line.IntegrationGroup, new Dictionary<string, Dictionary<Color, bool>>());
                    integrationGroup_scp_sumSplittedMinusLog10Pvalues_dict.Add(enrichment_line.IntegrationGroup, new Dictionary<string, float>());
                }
                scps_length = scps.Length;
                for (int indexScp = 0; indexScp < scps_length; indexScp++)
                {
                    scp = scps[indexScp];
                    if (!integrationGroup_scp_color_dict[enrichment_line.IntegrationGroup].ContainsKey(scp))
                    {
                        integrationGroup_scp_uniqueDatasetName_dict[enrichment_line.IntegrationGroup].Add(scp, new Dictionary<string, bool>());
                        integrationGroup_scp_color_dict[enrichment_line.IntegrationGroup].Add(scp, new Dictionary<Color, bool>());
                        integrationGroup_scp_sumSplittedMinusLog10Pvalues_dict[enrichment_line.IntegrationGroup].Add(scp, 0);
                    }
                    if (!integrationGroup_scp_uniqueDatasetName_dict[enrichment_line.IntegrationGroup][scp].ContainsKey(enrichment_line.Unique_dataset_name))
                    {
                        integrationGroup_scp_uniqueDatasetName_dict[enrichment_line.IntegrationGroup][scp].Add(enrichment_line.Unique_dataset_name, true);
                    }
                    integrationGroup_scp_sumSplittedMinusLog10Pvalues_dict[enrichment_line.IntegrationGroup][scp] += enrichment_line.Minus_log10_pvalue / scps_length;
                    if (!integrationGroup_scp_color_dict[enrichment_line.IntegrationGroup][scp].ContainsKey(enrichment_line.Sample_color))
                    {
                        integrationGroup_scp_color_dict[enrichment_line.IntegrationGroup][scp].Add(enrichment_line.Sample_color, true);
                    }
                }
            }
            string[] integrationGroups = integrationGroup_scp_sumSplittedMinusLog10Pvalues_dict.Keys.ToArray();
            string integrationGroup;
            int integrationGroups_length = integrationGroups.Length;
            float max_sumMinusLog10Pvalue = -1;
            float min_sumMinusLog10Pvalue = -1;
            int max_datasetCounts = -1;
            int min_datasetCounts = -1;
            int max_colorCounts = -1;
            int min_colorCounts = -1;

            for (int indexInt=0; indexInt < integrationGroups_length; indexInt++)
            {
                integrationGroup = integrationGroups[indexInt];
                scp_uniqueDatasetName_dict = integrationGroup_scp_uniqueDatasetName_dict[integrationGroup];
                scp_sumSplittedMinusLog10Pvalues_dict = integrationGroup_scp_sumSplittedMinusLog10Pvalues_dict[integrationGroup];
                scp_color_dict = integrationGroup_scp_color_dict[integrationGroup];
                scps = scp_uniqueDatasetName_dict.Keys.ToArray();
                scps_length = scps.Length;
                for (int indexScp = 0; indexScp < scps_length; indexScp++)
                {
                    scp = scps[indexScp];

                    sumSplittedMinusLog10Pvalue = scp_sumSplittedMinusLog10Pvalues_dict[scp];
                    if ((min_sumMinusLog10Pvalue == -1) || (sumSplittedMinusLog10Pvalue < min_sumMinusLog10Pvalue))
                    {
                        min_sumMinusLog10Pvalue = sumSplittedMinusLog10Pvalue;
                    }
                    if ((max_sumMinusLog10Pvalue == -1) || (sumSplittedMinusLog10Pvalue > max_sumMinusLog10Pvalue))
                    {
                        max_sumMinusLog10Pvalue = sumSplittedMinusLog10Pvalue;
                    }

                    uniqueDatasetsCount = scp_uniqueDatasetName_dict[scp].Keys.Count;
                    if ((min_datasetCounts == -1) || (uniqueDatasetsCount < min_datasetCounts))
                    {
                        min_datasetCounts = uniqueDatasetsCount;
                    }
                    if ((max_datasetCounts == -1) || (uniqueDatasetsCount > max_datasetCounts))
                    {
                        max_datasetCounts = uniqueDatasetsCount;
                    }

                    uniqueColorsCount = scp_color_dict[scp].Keys.Count;
                    if ((min_colorCounts == -1) || (uniqueColorsCount < min_colorCounts))
                    {
                        min_colorCounts = uniqueColorsCount;
                    }
                    if ((max_colorCounts == -1) || (uniqueColorsCount > max_colorCounts))
                    {
                        max_colorCounts = uniqueColorsCount;
                    }
                }
            }
            networkNodeSizeDeterminant_maxValue_dict = new Dictionary<Yed_network_node_size_determinant_enum, float>();
            networkNodeSizeDeterminant_maxValue_dict.Add(Yed_network_node_size_determinant_enum.No_of_sets, max_datasetCounts);
            networkNodeSizeDeterminant_maxValue_dict.Add(Yed_network_node_size_determinant_enum.No_of_different_colors, max_colorCounts);
            networkNodeSizeDeterminant_maxValue_dict.Add(Yed_network_node_size_determinant_enum.Minus_log10_pvalue, max_sumMinusLog10Pvalue);
            networkNodeSizeDeterminant_maxValue_dict.Add(Yed_network_node_size_determinant_enum.Uniform, 1);

            networkNodeSizeDeterminant_minValue_dict = new Dictionary<Yed_network_node_size_determinant_enum, float>();
            networkNodeSizeDeterminant_minValue_dict.Add(Yed_network_node_size_determinant_enum.No_of_sets, min_datasetCounts);
            networkNodeSizeDeterminant_minValue_dict.Add(Yed_network_node_size_determinant_enum.No_of_different_colors, min_colorCounts);
            networkNodeSizeDeterminant_minValue_dict.Add(Yed_network_node_size_determinant_enum.Minus_log10_pvalue, min_sumMinusLog10Pvalue);
            networkNodeSizeDeterminant_minValue_dict.Add(Yed_network_node_size_determinant_enum.Uniform, 1);
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
                if (!sampleName_dict.ContainsKey(enrich_line.SampleName))
                {
                    sampleName_dict.Add(enrich_line.SampleName, true);
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

        public Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<int, bool>>>>> Get_all_conditions_dict()
        {
            //this.Enrich = this.Enrich.OrderBy(l=>l.IntegrationGroup).ThenBy(l => l.SampleName).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l=>l.ProcessLevel).ToArray();
            Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<int, bool>>>>> integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict = new Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<int, bool>>>>>();
            int enrich_length = this.Enrich.Length;
            float timepoint_in_days;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexE=0; indexE<enrich_length; indexE++)
            {
                onto_enrich_line = this.Enrich[indexE];
                timepoint_in_days = onto_enrich_line.TimepointInDays;
                if (!integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict.ContainsKey(onto_enrich_line.IntegrationGroup))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict.Add(onto_enrich_line.IntegrationGroup, new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<int, bool>>>>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[onto_enrich_line.IntegrationGroup].ContainsKey(onto_enrich_line.SampleName))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[onto_enrich_line.IntegrationGroup].Add(onto_enrich_line.SampleName, new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<int, bool>>>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName].ContainsKey(onto_enrich_line.EntryType))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName].Add(onto_enrich_line.EntryType, new Dictionary<float, Dictionary<int, bool>>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType].ContainsKey(timepoint_in_days))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType].Add(timepoint_in_days, new Dictionary<int, bool>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType][timepoint_in_days].ContainsKey(onto_enrich_line.ProcessLevel))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType][timepoint_in_days].Add(onto_enrich_line.ProcessLevel, false);
                }
            }
            return integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict;
        }

        public void Keep_only_indicated_conditions(Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<int, bool>>>>> integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict)
        {
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            float[] timepointsInDays;
            float timepointInDays;
            int timepointsInDays_length;
            int[] processLevels;
            int processLevel;
            int processLevels_length;
            if (Global_class.Do_internal_checks)
            {
                integrationGroups = integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict.Keys.ToArray();
                integrationGroups_length = integrationGroups.Length;
                for (int indexI = 0; indexI < integrationGroups_length; indexI++)
                {
                    integrationGroup = integrationGroups[indexI];
                    sampleNames = integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[integrationGroup].Keys.ToArray();
                    sampleNames_length = sampleNames.Length;
                    for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                    {
                        sampleName = sampleNames[indexSN];
                        entryTypes = integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[integrationGroup][sampleName].Keys.ToArray();
                        entryTypes_length = entryTypes.Length;
                        for (int indexE = 0; indexE < entryTypes_length; indexE++)
                        {
                            entryType = entryTypes[indexE];
                            timepointsInDays = integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[integrationGroup][sampleName][entryType].Keys.ToArray();
                            timepointsInDays_length = timepointsInDays.Length;
                            for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                            {
                                timepointInDays = timepointsInDays[indexT];
                                processLevels = integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[integrationGroup][sampleName][entryType][timepointInDays].Keys.ToArray();
                                processLevels_length = processLevels.Length;
                                for (int indexPL = 0; indexPL < processLevels_length; indexPL++)
                                {
                                    processLevel = processLevels[indexPL];
                                    integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[integrationGroup][sampleName][entryType][timepointInDays][processLevel] = false;
                                }
                            }
                        }
                    }
                }
            }

            int enrich_length = this.Enrich.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            List<Ontology_enrichment_line_class> keep = new List<Ontology_enrichment_line_class>();
            for (int indexE=0; indexE<enrich_length;indexE++)
            {
                onto_enrich_line = this.Enrich[indexE];
                timepointInDays = onto_enrich_line.TimepointInDays;
                if (  (integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict.ContainsKey(onto_enrich_line.IntegrationGroup))
                    && (integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[onto_enrich_line.IntegrationGroup].ContainsKey(onto_enrich_line.SampleName))
                    && (integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName].ContainsKey(onto_enrich_line.EntryType))
                    && (integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType].ContainsKey(timepointInDays))
                    && (integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType][timepointInDays].ContainsKey(onto_enrich_line.ProcessLevel)))
                {
                    keep.Add(onto_enrich_line);
                    integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType][timepointInDays][onto_enrich_line.ProcessLevel] = true;
                }
            }
            this.Enrich = keep.ToArray();

            if (Global_class.Do_internal_checks)
            {
                integrationGroups = integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict.Keys.ToArray();
                integrationGroups_length = integrationGroups.Length;
                for (int indexI = 0; indexI < integrationGroups_length; indexI++)
                {
                    integrationGroup = integrationGroups[indexI];
                    sampleNames = integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[integrationGroup].Keys.ToArray();
                    sampleNames_length = sampleNames.Length;
                    for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                    {
                        sampleName = sampleNames[indexSN];
                        entryTypes = integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[integrationGroup][sampleName].Keys.ToArray();
                        entryTypes_length = entryTypes.Length;
                        for (int indexE = 0; indexE < entryTypes_length; indexE++)
                        {
                            entryType = entryTypes[indexE];
                            timepointsInDays = integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[integrationGroup][sampleName][entryType].Keys.ToArray();
                            timepointsInDays_length = timepointsInDays.Length;
                            for (int indexT = 0; indexT < timepointsInDays_length; indexT++)
                            {
                                timepointInDays = timepointsInDays[indexT];
                                processLevels = integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[integrationGroup][sampleName][entryType][timepointInDays].Keys.ToArray();
                                processLevels_length = processLevels.Length;
                                for (int indexPL = 0; indexPL < processLevels_length; indexPL++)
                                {
                                    processLevel = processLevels[indexPL];
                                    if (!integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict[integrationGroup][sampleName][entryType][timepointInDays][processLevel])
                                    {
                                        throw new Exception();
                                    }
                                }
                            }
                        }
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
                if (enrich_line.Complete_sampleName.Equals(completeSampleName))
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
                if (enrich_line.SampleName.Equals(sampleName))
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
        public Dictionary<string,bool> Get_all_scps_dict()
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
            return scpNames_dict;
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

        public void Adjust_unique_dataset_name_if_it_overlaps_with_scp(Dictionary<string, bool> scp_dict)
        {
            foreach (Ontology_enrichment_line_class enrich_line in this.Enrich)
            {
                if (scp_dict.ContainsKey(enrich_line.Unique_dataset_name))
                {
                    enrich_line.Unique_dataset_name += " (dataset name)";
                }
            }
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

        public Organism_enum Get_organism_and_check_if_only_one_ontology()
        {
            if (Enrich.Length > 0)
            {
                Organism_enum organism = Enrich[0].Organism;
                    ;
                int enrich_length = this.Enrich.Length;
                Ontology_enrichment_line_class onto_enrich_line;
                for (int indexE = 0; indexE < enrich_length; indexE++)
                {
                    onto_enrich_line = this.Enrich[indexE];
                    if (!onto_enrich_line.Organism.Equals(organism))
                    {
                        throw new Exception();
                    }
                }
                return organism;
            }
            return Organism_enum.E_m_p_t_y;
        }
        public string Get_results_fileName_addition_ontology_organism()
        {
            return Get_ontology_and_check_if_only_one_ontology() + "_" + Ontology_classification_class.Get_organismString_for_enum(Get_organism_and_check_if_only_one_ontology());
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
                if (enrich_line.IntegrationGroup.Equals(integrationGroup))
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

        public void Add_new_enrichment_lines_for_each_process_with_missing_integrationGroup_sampleName_entryType_timepointInDays_if_at_least_one()
        {
            Ontology_type_enum ontology = this.Enrich[0].Ontology_type;
            int onto_enrich_length = this.Enrich.Length;
            Ontology_enrichment_line_class onto_enrich_line;

            List<Ontology_enrichment_line_class> new_onto_enrich = new List<Ontology_enrichment_line_class>();
            Ontology_enrichment_line_class new_line;
            List<Ontology_enrichment_line_class> missing_completeSampleNames = new List<Ontology_enrichment_line_class>();

            //Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, bool>>> integrationGroup_sampleName_entryType_dict = new Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, bool>>>();
            //Dictionary<Entry_type_enum, bool> entryType_dict = new Dictionary<Entry_type_enum, bool>();
            //Dictionary<string, float[]> integrationGroup_timepointsInDays_dict = new Dictionary<string, float[]>();
            Dictionary<float, float> timepointsInDays_timepoint_dict = new Dictionary<float, float>();
            Dictionary<float, Timeunit_enum> timepointsInDays_timeunit_dict = new Dictionary<float, Timeunit_enum>();
            //Dictionary<float, bool> current_timepointInDays_dict = new Dictionary<float, bool>();

            Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, string>>>> integrationGroup_sampleName_entryType_timepointInDays_uniqueDatasetName_dict = new Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, string>>>>();
            Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, string>>> sampleName_entryType_timepointInDays_uniqueDatasetName_dict = new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, string>>>();
            Dictionary<Entry_type_enum, Dictionary<float, string>> entryType_timepointInDays_uniqueDatasetName_dict = new Dictionary<Entry_type_enum, Dictionary<float, string>>();
            Dictionary<float, string> timepointInDays_uniqueDatasetName_dict = new Dictionary<float, string>();

            Dictionary<string, int> uniqueDatasetName_resultsNo_dict = new Dictionary<string, int>();
            Dictionary<string, System.Drawing.Color> uniqueDatasetName_color_dict = new Dictionary<string, System.Drawing.Color>();

            this.Enrich = Ontology_enrichment_line_class.Order_by_integrationGroup_timepointInDays(this.Enrich);

            Timeunit_enum standard_timeunit = this.Enrich[0].Timeunit;
            float onto_enrich_timepointInDays;
            for (int indexE = 0; indexE < onto_enrich_length; indexE++)
            {
                onto_enrich_line = this.Enrich[indexE];
                onto_enrich_timepointInDays = onto_enrich_line.TimepointInDays;
                if (!onto_enrich_line.Ontology_type.Equals(ontology)) { throw new Exception(); }

                #region Fill timepointsInDays_timepoint_dict and timepointsInDays_timeunit_dict
                if (!timepointsInDays_timepoint_dict.ContainsKey(onto_enrich_timepointInDays))
                {
                    timepointsInDays_timepoint_dict.Add(onto_enrich_timepointInDays, onto_enrich_line.Timepoint);
                    timepointsInDays_timeunit_dict.Add(onto_enrich_timepointInDays, onto_enrich_line.Timeunit);
                }
                #endregion

                #region Fill integrationGroup_sampleName_entryType_timepointInDays_uniqueDatasetName_dict
                if (!integrationGroup_sampleName_entryType_timepointInDays_uniqueDatasetName_dict.ContainsKey(onto_enrich_line.IntegrationGroup))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_uniqueDatasetName_dict.Add(onto_enrich_line.IntegrationGroup, new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, string>>>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_uniqueDatasetName_dict[onto_enrich_line.IntegrationGroup].ContainsKey(onto_enrich_line.SampleName))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_uniqueDatasetName_dict[onto_enrich_line.IntegrationGroup].Add(onto_enrich_line.SampleName, new Dictionary<Entry_type_enum, Dictionary<float, string>>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_uniqueDatasetName_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName].ContainsKey(onto_enrich_line.EntryType))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_uniqueDatasetName_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName].Add(onto_enrich_line.EntryType, new Dictionary<float, string>());
                }
                if (!integrationGroup_sampleName_entryType_timepointInDays_uniqueDatasetName_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType].ContainsKey(onto_enrich_timepointInDays))
                {
                    integrationGroup_sampleName_entryType_timepointInDays_uniqueDatasetName_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.SampleName][onto_enrich_line.EntryType].Add(onto_enrich_timepointInDays, onto_enrich_line.Unique_dataset_name);
                }
                #endregion

                #region Fill uniqueDatasetName_resultsNo_dict
                if (!uniqueDatasetName_resultsNo_dict.ContainsKey(onto_enrich_line.Unique_dataset_name))
                {
                    uniqueDatasetName_resultsNo_dict.Add(onto_enrich_line.Unique_dataset_name, onto_enrich_line.Results_number);
                }
                else if (!uniqueDatasetName_resultsNo_dict[onto_enrich_line.Unique_dataset_name].Equals(onto_enrich_line.Results_number))
                {
                    throw new Exception();
                }
                #endregion

                #region Fill uniqueDatasetName_color_dict
                if (!uniqueDatasetName_color_dict.ContainsKey(onto_enrich_line.Unique_dataset_name))
                {
                    uniqueDatasetName_color_dict.Add(onto_enrich_line.Unique_dataset_name, onto_enrich_line.Sample_color);
                }
                else if (!uniqueDatasetName_color_dict[onto_enrich_line.Unique_dataset_name].Equals(onto_enrich_line.Sample_color))
                {
                    throw new Exception();
                }
                #endregion
            }
            Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, bool>>> sampleName_entryType_timepointInDays_exists_dict = new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, bool>>>();
            Dictionary<Entry_type_enum, Dictionary<float, bool>> entryType_timepointInDays_exists_dict = new Dictionary<Entry_type_enum, Dictionary<float, bool>>();
            Dictionary<float, bool> timepointInDays_exists_dict = new Dictionary<float, bool>();
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            float[] search_timepointsInDays;
            float search_timepointInDays;
            int search_timepointsInDays_length;
            this.Enrich = Ontology_enrichment_line_class.Order_by_integrationGroup_and_scpName(this.Enrich);
            for (int indexE = 0; indexE < onto_enrich_length; indexE++)
            {
                onto_enrich_line = this.Enrich[indexE];
                onto_enrich_timepointInDays = onto_enrich_line.TimepointInDays;
                if ((indexE == 0)
                    || (!onto_enrich_line.IntegrationGroup.Equals(this.Enrich[indexE - 1].IntegrationGroup))
                    || (!onto_enrich_line.Scp_name.Equals(this.Enrich[indexE - 1].Scp_name)))
                {
                    sampleName_entryType_timepointInDays_exists_dict.Clear();
                    sampleName_entryType_timepointInDays_uniqueDatasetName_dict = integrationGroup_sampleName_entryType_timepointInDays_uniqueDatasetName_dict[onto_enrich_line.IntegrationGroup];
                    sampleNames = sampleName_entryType_timepointInDays_uniqueDatasetName_dict.Keys.ToArray();
                    sampleNames_length = sampleNames.Length;
                    for (int indexSampleName=0; indexSampleName < sampleNames_length;indexSampleName++)
                    {
                        sampleName = sampleNames[indexSampleName];
                        sampleName_entryType_timepointInDays_exists_dict.Add(sampleName, new Dictionary<Entry_type_enum, Dictionary<float, bool>>());
                        entryType_timepointInDays_uniqueDatasetName_dict = sampleName_entryType_timepointInDays_uniqueDatasetName_dict[sampleName];
                        entryTypes = entryType_timepointInDays_uniqueDatasetName_dict.Keys.ToArray();
                        entryTypes_length = entryTypes.Length;
                        for (int indexEntryType = 0; indexEntryType < entryTypes_length; indexEntryType++)
                        {
                            entryType = entryTypes[indexEntryType];
                            sampleName_entryType_timepointInDays_exists_dict[sampleName].Add(entryType, new Dictionary<float, bool>());
                            timepointInDays_uniqueDatasetName_dict = entryType_timepointInDays_uniqueDatasetName_dict[entryType];
                            search_timepointsInDays = timepointInDays_uniqueDatasetName_dict.Keys.ToArray();
                            search_timepointsInDays_length = search_timepointsInDays.Length;
                            for (int indexSearchTimepointInDays = 0; indexSearchTimepointInDays < search_timepointsInDays_length; indexSearchTimepointInDays++)
                            {
                                search_timepointInDays = search_timepointsInDays[indexSearchTimepointInDays];
                                sampleName_entryType_timepointInDays_exists_dict[sampleName][entryType].Add(search_timepointInDays, false);
                            }
                        }
                    }
                }
                sampleName_entryType_timepointInDays_exists_dict[onto_enrich_line.SampleName][onto_enrich_line.EntryType][onto_enrich_timepointInDays] = true;
                if ((indexE == onto_enrich_length - 1)
                    || (!onto_enrich_line.IntegrationGroup.Equals(this.Enrich[indexE + 1].IntegrationGroup))
                    || (!onto_enrich_line.Scp_name.Equals(this.Enrich[indexE + 1].Scp_name)))
                {
                    sampleNames = sampleName_entryType_timepointInDays_uniqueDatasetName_dict.Keys.ToArray();
                    sampleNames_length = sampleNames.Length;
                    for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                    {
                        sampleName = sampleNames[indexSN];
                        entryType_timepointInDays_exists_dict = sampleName_entryType_timepointInDays_exists_dict[sampleName];
                        entryTypes = entryType_timepointInDays_exists_dict.Keys.ToArray();
                        entryTypes_length = entryTypes.Length;
                        for (int indexEntryType = 0; indexEntryType < entryTypes_length; indexEntryType++)
                        {
                            entryType = entryTypes[indexEntryType];
                            timepointInDays_exists_dict = entryType_timepointInDays_exists_dict[entryType];
                            search_timepointsInDays = timepointInDays_exists_dict.Keys.ToArray();
                            search_timepointsInDays_length = search_timepointsInDays.Length;
                            for (int indexSearchTimepointInDays = 0; indexSearchTimepointInDays < search_timepointsInDays_length; indexSearchTimepointInDays++)
                            {
                                search_timepointInDays = search_timepointsInDays[indexSearchTimepointInDays];
                                if (!timepointInDays_exists_dict[search_timepointInDays])
                                {
                                    new_line = new Ontology_enrichment_line_class();
                                    new_line.Ontology_type = onto_enrich_line.Ontology_type;
                                    new_line.Organism = onto_enrich_line.Organism;
                                    new_line.Scp_name = (string)onto_enrich_line.Scp_name.Clone();
                                    new_line.Scp_id = (string)onto_enrich_line.Scp_id.Clone();
                                    new_line.Parent_scp_name = (string)onto_enrich_line.Parent_scp_name.Clone();
                                    new_line.IntegrationGroup = (string)onto_enrich_line.IntegrationGroup.Clone();
                                    
                                    new_line.SampleName = (string)sampleName.Clone();
                                    new_line.EntryType = entryType;
                                    new_line.Timepoint = timepointsInDays_timepoint_dict[search_timepointInDays];
                                    new_line.Timeunit = timepointsInDays_timeunit_dict[search_timepointInDays];

                                    new_line.Unique_dataset_name = (string)integrationGroup_sampleName_entryType_timepointInDays_uniqueDatasetName_dict[new_line.IntegrationGroup][new_line.SampleName][new_line.EntryType][search_timepointInDays].Clone();
                                    new_line.Pvalue = 1;
                                    new_line.Minus_log10_pvalue = 0;
                                    new_line.Bg_symbol_count = onto_enrich_line.Bg_symbol_count;
                                    new_line.Experimental_symbols_count = onto_enrich_line.Experimental_symbols_count;
                                    new_line.FDR = 1;
                                    new_line.ProcessLevel = onto_enrich_line.ProcessLevel;
                                    new_line.ProcessDepth = onto_enrich_line.ProcessDepth;
                                    new_line.Process_symbols_count = onto_enrich_line.Process_symbols_count;
                                    new_line.Overlap_count = 0;
                                    new_line.Overlap_symbols = new string[0];
                                    new_line.Fractional_rank = 999999;
                                    new_line.Significant = false;
                                    new_line.Results_number = uniqueDatasetName_resultsNo_dict[new_line.Unique_dataset_name];
                                    new_line.Sample_color = uniqueDatasetName_color_dict[new_line.Unique_dataset_name];
                                    missing_completeSampleNames.Add(new_line);
                                }
                            }
                        }
                    }
                }
            }
            Add_other_lines_without_resetting_unique_datasetNames(missing_completeSampleNames.ToArray());
        }

        public void Set_equal_colors_for_all_entryTypes_and_timepoints_of_each_sampleName_to_first_timepoint_color()
        {
            this.Enrich = Ontology_enrichment_line_class.Order_by_integrationGroup_sampleName_timepointInDays(this.Enrich);
            int enrich_length = this.Enrich.Length;
            Ontology_enrichment_line_class enrich_line;
            System.Drawing.Color current_color = System.Drawing.Color.AliceBlue;
            for (int indexE=0; indexE<enrich_length;indexE++)
            {
                enrich_line = this.Enrich[indexE];
                if (  (indexE==0)
                    || (enrich_line.IntegrationGroup.Equals(this.Enrich[indexE - 1].IntegrationGroup))
                    || (enrich_line.SampleName.Equals(this.Enrich[indexE - 1].SampleName)))
                {
                    current_color = enrich_line.Sample_color;
                }
                enrich_line.Sample_color = current_color;
            }
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
                string integrationGroup = this.Enrich[0].IntegrationGroup;
                int enrich_length = this.Enrich.Length;
                Ontology_enrichment_line_class onto_enrich_line;
                Ontology_enrichment_line_class singleScp_onto_enrich_line;
                List<Ontology_enrichment_line_class> onto_enrich_keep = new List<Ontology_enrichment_line_class>();
                Dictionary<string, bool> considered_scps_of_current_condition = new Dictionary<string, bool>();
                //this.Enrich = this.Enrich.OrderBy(l => l.EntryType).ThenBy(l => l.TimepointInDays).ThenBy(l => l.SampleName).ThenBy(l => l.Pvalue).ToArray();
                this.Enrich = Ontology_enrichment_line_class.Order_by_entryType_timepoinitInDays_sampleName_pvalue(this.Enrich);
                string[] scps;
                string scp;
                int scps_length;
                for (int indexO = 0; indexO < enrich_length; indexO++)
                {
                    onto_enrich_line = this.Enrich[indexO];
                    if (!integrationGroup.Equals(onto_enrich_line.IntegrationGroup)) { throw new Exception(); }
                    if ((indexO == 0)
                        || (!onto_enrich_line.EntryType.Equals(this.Enrich[indexO - 1].EntryType))
                        || (!onto_enrich_line.TimepointInDays.Equals(this.Enrich[indexO - 1].TimepointInDays))
                        || (!onto_enrich_line.SampleName.Equals(this.Enrich[indexO - 1].SampleName)))
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

                //this.Enrich = this.Enrich.OrderBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.SampleName).ThenBy(l => l.Scp_name).ToArray();
                //standard_unfiltered.Enrich = standard_unfiltered.Enrich.OrderBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.SampleName).ThenBy(l => l.Scp_name).ToArray();
                this.Enrich = Ontology_enrichment_line_class.Order_by_entryType_timepoinitInDays_sampleName_scpName(this.Enrich);
                standard_unfiltered.Enrich = Ontology_enrichment_line_class.Order_by_entryType_timepoinitInDays_sampleName_scpName(standard_unfiltered.Enrich);

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
                            stringCompare = standard_onto_enrich_line.SampleName.CompareTo(this_onto_enrich_line.SampleName);
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

        public void Separate_scp_unions_into_single_scps_and_add_up_splitted_shared_pvalues_and_rerank()
        {
            if (this.Enrich.Length > 0)
            {
                int enrich_length = this.Enrich.Length;
                Ontology_enrichment_line_class onto_enrich_line;
                Ontology_enrichment_line_class singleScp_onto_enrich_line;
                List<Ontology_enrichment_line_class> onto_enrich_keep = new List<Ontology_enrichment_line_class>();
                //this.Enrich = Ontology_enrichment_line_class.Order_by_entryType_timepoinitInDays_sampleName_pvalue(this.Enrich);
                //this.Enrich = Ontology_enrichment_line_class.Order_by_integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue(this.Enrich);
                string[] scps;
                string scp;
                int scps_length;
                float distribute_minus_log10pvalue;
                for (int indexO = 0; indexO < enrich_length; indexO++)
                {
                    onto_enrich_line = this.Enrich[indexO];
                    scps = onto_enrich_line.Scp_name.Split('$');
                    scps_length = scps.Length;
                    distribute_minus_log10pvalue = onto_enrich_line.Minus_log10_pvalue / scps_length;
                    for (int indexScp = 0; indexScp < scps_length; indexScp++)
                    {
                        scp = scps[indexScp];
                        singleScp_onto_enrich_line = new Ontology_enrichment_line_class();
                        //singleScp_onto_enrich_line = onto_enrich_line.Deep_copy();
                        singleScp_onto_enrich_line.Unique_dataset_name = (string)onto_enrich_line.Unique_dataset_name.Clone();
                        singleScp_onto_enrich_line.Results_number = onto_enrich_line.Results_number;
                        singleScp_onto_enrich_line.SampleName = (string)onto_enrich_line.SampleName.Clone();
                        singleScp_onto_enrich_line.Sample_color = onto_enrich_line.Sample_color;
                        singleScp_onto_enrich_line.EntryType = onto_enrich_line.EntryType;
                        singleScp_onto_enrich_line.Timepoint = onto_enrich_line.Timepoint;
                        singleScp_onto_enrich_line.Timeunit = onto_enrich_line.Timeunit;
                        singleScp_onto_enrich_line.IntegrationGroup = (string)onto_enrich_line.IntegrationGroup.Clone();
                        singleScp_onto_enrich_line.ProcessLevel = onto_enrich_line.ProcessLevel;
                        singleScp_onto_enrich_line.ProcessDepth = onto_enrich_line.ProcessDepth;
                        singleScp_onto_enrich_line.Scp_id = (string)onto_enrich_line.Scp_id.Clone();
                        singleScp_onto_enrich_line.Scp_name = (string)onto_enrich_line.Scp_name.Clone();
                        singleScp_onto_enrich_line.Pvalue = -1;
                        singleScp_onto_enrich_line.Experimental_symbols_count = onto_enrich_line.Experimental_symbols_count;
                        singleScp_onto_enrich_line.Bg_symbol_count = onto_enrich_line.Bg_symbol_count;
                        singleScp_onto_enrich_line.Fractional_rank = -1;
                        singleScp_onto_enrich_line.FDR = -1;
                        singleScp_onto_enrich_line.Ontology_type = onto_enrich_line.Ontology_type;
                        singleScp_onto_enrich_line.Organism = onto_enrich_line.Organism;
                        singleScp_onto_enrich_line.Qvalue = -1;
                        singleScp_onto_enrich_line.Overlap_count = -1;
                        singleScp_onto_enrich_line.Overlap_symbols = new string[0];
                        singleScp_onto_enrich_line.Significant = onto_enrich_line.Significant;
                        singleScp_onto_enrich_line.Scp_name = (string)scp.Clone();
                        singleScp_onto_enrich_line.Scp_id = "broken up dynamic results from " + onto_enrich_line.Scp_name;
                        singleScp_onto_enrich_line.Overlap_symbols = new string[0];
                        singleScp_onto_enrich_line.Minus_log10_pvalue = distribute_minus_log10pvalue;
                        onto_enrich_keep.Add(singleScp_onto_enrich_line);
                    }
                }
                this.Enrich = Ontology_enrichment_line_class.Order_by_scpName_integrationGroup_entryType_sampleName_timepointInDays(onto_enrich_keep.ToArray());

                int this_length = this.Enrich.Length;
                Ontology_enrichment_line_class this_onto_enrich_line;
                Ontology_enrichment_line_class combined_onto_enrich_line;
                List<Ontology_enrichment_line_class> combined_enrich_lines = new List<Ontology_enrichment_line_class>();
                Ontology_enrichment_line_class standard_onto_enrich_line = new Ontology_enrichment_line_class();
                float current_minusLog10Pvalue = 0;
                bool is_significant = false;
                string unique_dataset_name = "";
                int results_no = -1;
                for (int indexThis = 0; indexThis < this_length; indexThis++)
                {
                    this_onto_enrich_line = this.Enrich[indexThis];
                    if ((indexThis == 0)
                        || (!this_onto_enrich_line.Scp_name.Equals(this.Enrich[indexThis - 1].Scp_name))
                        || (!this_onto_enrich_line.IntegrationGroup.Equals(this.Enrich[indexThis - 1].IntegrationGroup))
                        || (!this_onto_enrich_line.EntryType.Equals(this.Enrich[indexThis - 1].EntryType))
                        || (!this_onto_enrich_line.SampleName.Equals(this.Enrich[indexThis - 1].SampleName))
                        || (!this_onto_enrich_line.TimepointInDays.Equals(this.Enrich[indexThis - 1].TimepointInDays)))
                    {
                        current_minusLog10Pvalue = 0;
                        is_significant = false;
                        unique_dataset_name = this_onto_enrich_line.Unique_dataset_name;
                        results_no = this_onto_enrich_line.Results_number;
                    }
                    if (!this_onto_enrich_line.Unique_dataset_name.Equals(unique_dataset_name)) { throw new Exception(); }
                    if (!this_onto_enrich_line.Results_number.Equals(results_no)) { throw new Exception(); }
                    current_minusLog10Pvalue += this_onto_enrich_line.Minus_log10_pvalue;
                    if (this_onto_enrich_line.Significant) { is_significant = this_onto_enrich_line.Significant; }
                    if ((indexThis == this_length-1)
                        || (!this_onto_enrich_line.Scp_name.Equals(this.Enrich[indexThis + 1].Scp_name))
                        || (!this_onto_enrich_line.IntegrationGroup.Equals(this.Enrich[indexThis + 1].IntegrationGroup))
                        || (!this_onto_enrich_line.EntryType.Equals(this.Enrich[indexThis + 1].EntryType))
                        || (!this_onto_enrich_line.SampleName.Equals(this.Enrich[indexThis + 1].SampleName))
                        || (!this_onto_enrich_line.TimepointInDays.Equals(this.Enrich[indexThis + 1].TimepointInDays)))
                    {
                        combined_onto_enrich_line = this_onto_enrich_line.Deep_copy();
                        combined_onto_enrich_line.Minus_log10_pvalue = current_minusLog10Pvalue;
                        combined_onto_enrich_line.Pvalue = Math.Pow(10,-this_onto_enrich_line.Minus_log10_pvalue);
                        combined_onto_enrich_line.Fractional_rank = -1;
                        combined_onto_enrich_line.Scp_id = "separated and combined from dynamic enrichment results";
                        combined_onto_enrich_line.FDR = -1;
                        combined_onto_enrich_line.Qvalue = -1;
                        combined_onto_enrich_line.Overlap_count = -1;
                        combined_onto_enrich_line.Overlap_symbols = new string[0];
                        combined_onto_enrich_line.Significant = is_significant;
                        combined_enrich_lines.Add(combined_onto_enrich_line);
                    }
                }
                this.Enrich = combined_enrich_lines.ToArray();
                Calculate_fractional_ranks_for_SCPs_within_each_integrationGroup_sampleName_timepoint_timeunit_entryType_processLevel();
            }

        }
        #endregion

        #region Set
        public void Set_all_processLevels_to_input_level(int new_level)
        {
            foreach (Ontology_enrichment_line_class enrich_line in this.Enrich)
            {
                enrich_line.ProcessLevel = new_level;
            }
        }

        #endregion

        #region Rank
        public void Calculate_fractional_ranks_for_SCPs_within_each_integrationGroup_sampleName_timepoint_timeunit_entryType_processLevel()
        {
            if (this.Enrich.Length > 0)
            {
                Ontology_type_enum ontology = Get_ontology_and_check_if_only_one_ontology();
                bool is_mbco_ontlogy = Ontology_classification_class.Is_mbco_ontology(ontology);
                if (is_mbco_ontlogy)
                {
                    this.Enrich = Ontology_enrichment_line_class.Order_by_integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue(this.Enrich);
                }
                else
                {
                    this.Enrich = Ontology_enrichment_line_class.Order_by_integrationGroup_sampleName_entryType_timepointInDays_pvalue(this.Enrich);
                }

                int enrich_length = this.Enrich.Length;
                Ontology_enrichment_line_class enrich_line;
                Ontology_enrichment_line_class inner_enrich_line;
                int ordinal_rank = 0;
                int firstIndexSamePvalue = -1;
                List<float> current_ordinal_ranks = new List<float>();
                float fractional_rank;
                float this_timepointInDays;
                float previous_timepointInDays = -1;
                for (int indexE = 0; indexE < enrich_length; indexE++)
                {
                    enrich_line = this.Enrich[indexE];
                    this_timepointInDays = enrich_line.TimepointInDays;
                    if (indexE > 0) { previous_timepointInDays = this.Enrich[indexE - 1].TimepointInDays; }
                    if ((indexE == 0)
                        || (!enrich_line.IntegrationGroup.Equals(this.Enrich[indexE - 1].IntegrationGroup))
                        || (!enrich_line.SampleName.Equals(this.Enrich[indexE - 1].SampleName))
                        || (!this_timepointInDays.Equals(previous_timepointInDays))
                        || (!enrich_line.EntryType.Equals(this.Enrich[indexE - 1].EntryType))
                        || ((!enrich_line.ProcessLevel.Equals(this.Enrich[indexE - 1].ProcessLevel)) && (is_mbco_ontlogy)))
                    {
                        ordinal_rank = 0;
                    }
                    if ((indexE == 0)
                        || (!enrich_line.IntegrationGroup.Equals(this.Enrich[indexE - 1].IntegrationGroup))
                        || (!enrich_line.SampleName.Equals(this.Enrich[indexE - 1].SampleName))
                        || (!this_timepointInDays.Equals(previous_timepointInDays))
                        || (!enrich_line.EntryType.Equals(this.Enrich[indexE - 1].EntryType))
                        || ((!enrich_line.ProcessLevel.Equals(this.Enrich[indexE - 1].ProcessLevel)) && (is_mbco_ontlogy))
                        || (!enrich_line.Pvalue.Equals(this.Enrich[indexE - 1].Pvalue)))
                    {
                        current_ordinal_ranks.Clear();
                        firstIndexSamePvalue = indexE;
                    }
                    ordinal_rank++;
                    current_ordinal_ranks.Add(ordinal_rank);
                    if ((indexE == enrich_length - 1)
                        || (!enrich_line.IntegrationGroup.Equals(this.Enrich[indexE + 1].IntegrationGroup))
                        || (!enrich_line.SampleName.Equals(this.Enrich[indexE + 1].SampleName))
                        || (!this_timepointInDays.Equals(this.Enrich[indexE + 1].TimepointInDays))
                        || (!enrich_line.EntryType.Equals(this.Enrich[indexE + 1].EntryType))
                        || ((!enrich_line.ProcessLevel.Equals(this.Enrich[indexE + 1].ProcessLevel)) && (is_mbco_ontlogy))
                        || (!enrich_line.Pvalue.Equals(this.Enrich[indexE + 1].Pvalue)))
                    {
                        if (current_ordinal_ranks.Count > 1)
                        {
                            fractional_rank = Math_class.Get_average(current_ordinal_ranks.ToArray());
                            for (int indexInner = firstIndexSamePvalue; indexInner <= indexE; indexInner++)
                            {
                                inner_enrich_line = this.Enrich[indexInner];
                                inner_enrich_line.Fractional_rank = fractional_rank;
                            }
                        }
                        else if (current_ordinal_ranks.Count == 1)
                        {
                            if (firstIndexSamePvalue != indexE) { throw new Exception(); }
                            enrich_line.Fractional_rank = current_ordinal_ranks[0];
                        }
                        else { throw new Exception(); }
                    }
                }
            }
        }

        #endregion

        #region Write copy
        public void Write_and_return_fileOpen_success(string subdirectory, string file_name, ProgressReport_interface_class progressReport, out bool file_opened_successfully)
        {
            bool writeIntegrationGroups = Get_all_integrationGroups().Length > 1;
            bool writeEntryType = Get_all_entryTypes().Length > 1;
            bool writeTimepoint = Get_all_timepointsInDays().Length > 1;
            //this.Enrich = this.Enrich.OrderBy(l => l.SampleName).ThenBy(l => l.Timepoint).ThenBy(l => l.ProcessLevel).ThenBy(l => l.EntryType).ThenBy(l => l.Pvalue).ToArray();
            this.Enrich = Ontology_enrichment_line_class.Order_by_integrationGroup_sampleName_entryType_timepointInDays_processLevel_pvalue(this.Enrich);
            Ontology_enrich_readWriteOptions_class readWriteOptions = new Ontology_enrich_readWriteOptions_class(subdirectory, file_name, writeIntegrationGroups, writeEntryType,writeTimepoint);
            ReadWriteClass.WriteData_and_add_warning_to_progressReport_if_failed(Enrich, readWriteOptions, progressReport, out file_opened_successfully);
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
