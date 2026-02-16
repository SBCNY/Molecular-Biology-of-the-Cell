//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common_functions.Global_definitions;
using Common_functions.ReadWrite;
using Enrichment;

namespace Data
{
    interface IAdd_to_data
    {
        string NCBI_official_symbol_for_data { get; }
        Entry_type_enum EntryType_for_data { get; }
        float Timepoint_for_data { get; }
        Timeunit_enum Timeunit_for_data { get; }
        string SampleName_for_data { get; }
        double Value_for_data { get; }
        string IntegrationGroup_for_data { get; }
        System.Drawing.Color SampleColor_for_data { get; }
        int Results_number_for_data { get; }
        string UniqueDatasetName_for_data { get; }
    }

    class IAdd_to_data_line_class : IAdd_to_data
    {
        public string NCBI_official_symbol_for_data { get; set; }
        public Entry_type_enum EntryType_for_data { get; set; }
        public Timeunit_enum Timeunit_for_data { get; set; }
        public float Timepoint_for_data { get; set; }
        public string SampleName_for_data { get; set; }
        public double Value_for_data { get; set; }
        public string IntegrationGroup_for_data { get; set; }
        public System.Drawing.Color SampleColor_for_data { get; set; }
        public int Results_number_for_data { get; set; }
        public string UniqueDatasetName_for_data { get; set; }
    }

    ///////////////////////////////////////////////////////////////

    class Colchar_column_line_class
    {
        #region Fields
        public Entry_type_enum EntryType { get; set; }
        public string IntegrationGroup { get; set; }
        public float Timepoint { get; set; }
        public Timeunit_enum Timeunit { get; set; }
        public string SampleName { get; set; }
        public System.Drawing.Color SampleColor { get; set; }
        public string UniqueDataset_name { get; set; }
        public string Full_column_name
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (!EntryType.Equals(Entry_type_enum.E_m_p_t_y))
                {
                    if (sb.Length > 0) { sb.AppendFormat(Full_name_datalimiter.ToString()); }
                    sb.AppendFormat(EntryType.ToString());
                }
                if ((!String.IsNullOrEmpty(SampleName))
                    && (!SampleName.Equals(Global_class.Empty_entry)))
                {
                    if (sb.Length > 0) { sb.AppendFormat(Full_name_datalimiter.ToString()); }
                    sb.AppendFormat(SampleName);
                }
                if (Timepoint!=-1)
                {
                    if (sb.Length > 0) { sb.AppendFormat(Full_name_datalimiter.ToString()); }
                    sb.AppendFormat(Timepoint.ToString() + " " + Timeunit.ToString());
                }
                return sb.ToString();
            }
        }
        public static char Full_name_datalimiter { get { return '-'; } }
        public int Results_no { get; set; }
        #endregion

        public Colchar_column_line_class()
        {
        }

        public Colchar_column_line_class(Colchar_column_line_class add_line)
        {
            this.IntegrationGroup = (string)add_line.IntegrationGroup.Clone();
            this.EntryType = add_line.EntryType;
            this.Timepoint = add_line.Timepoint;
            this.Timeunit = add_line.Timeunit;
            this.SampleName = (string)add_line.SampleName.Clone();
            this.SampleColor = add_line.SampleColor;
            this.Results_no = add_line.Results_no;
            this.UniqueDataset_name = (string)add_line.UniqueDataset_name.Clone();
        }

        #region Standard
        public static Colchar_column_line_class[] Order_in_standard_way(Colchar_column_line_class[] columns)
        {
            //columns.OrderBy(l => l.IntegrationGroup).ThenBy(l => l.EntryType).ThenBy(l => l.Timeunit).ThenBy(l => l.Timepoint).ThenBy(l => l.SampleName).ToArray();
            Dictionary<string, Dictionary<Entry_type_enum, Dictionary<Timeunit_enum, Dictionary<float, Dictionary<string, List<Colchar_column_line_class>>>>>> integrationGroup_entryType_timeunit_timepoint_sampleName_dict = new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<Timeunit_enum, Dictionary<float, Dictionary<string, List<Colchar_column_line_class>>>>>>();
            Dictionary<Entry_type_enum, Dictionary<Timeunit_enum, Dictionary<float, Dictionary<string, List<Colchar_column_line_class>>>>> entryType_timeunit_timepoint_sampleName_dict = new Dictionary<Entry_type_enum, Dictionary<Timeunit_enum, Dictionary<float, Dictionary<string, List<Colchar_column_line_class>>>>>();
            Dictionary<Timeunit_enum, Dictionary<float, Dictionary<string, List<Colchar_column_line_class>>>> timeunit_timepoint_sampleName_dict = new Dictionary<Timeunit_enum, Dictionary<float, Dictionary<string, List<Colchar_column_line_class>>>>();
            Dictionary<float, Dictionary<string, List<Colchar_column_line_class>>> timepoint_sampleName_dict = new Dictionary<float, Dictionary<string, List<Colchar_column_line_class>>>();
            Dictionary<string, List<Colchar_column_line_class>> sampleName_dict = new Dictionary<string, List<Colchar_column_line_class>>();
            Colchar_column_line_class column_line;
            int columns_length = columns.Length;
            for (int indexC=0; indexC<columns_length;indexC++)
            {
                column_line = columns[indexC];
                if (!integrationGroup_entryType_timeunit_timepoint_sampleName_dict.ContainsKey(column_line.IntegrationGroup))
                {
                    integrationGroup_entryType_timeunit_timepoint_sampleName_dict.Add(column_line.IntegrationGroup, new Dictionary<Entry_type_enum, Dictionary<Timeunit_enum, Dictionary<float, Dictionary<string, List<Colchar_column_line_class>>>>>());
                }
                if (!integrationGroup_entryType_timeunit_timepoint_sampleName_dict[column_line.IntegrationGroup].ContainsKey(column_line.EntryType))
                {
                    integrationGroup_entryType_timeunit_timepoint_sampleName_dict[column_line.IntegrationGroup].Add(column_line.EntryType, new Dictionary<Timeunit_enum, Dictionary<float, Dictionary<string, List<Colchar_column_line_class>>>>());
                }
                if (!integrationGroup_entryType_timeunit_timepoint_sampleName_dict[column_line.IntegrationGroup][column_line.EntryType].ContainsKey(column_line.Timeunit))
                {
                    integrationGroup_entryType_timeunit_timepoint_sampleName_dict[column_line.IntegrationGroup][column_line.EntryType].Add(column_line.Timeunit, new Dictionary<float, Dictionary<string, List<Colchar_column_line_class>>>());
                }
                if (!integrationGroup_entryType_timeunit_timepoint_sampleName_dict[column_line.IntegrationGroup][column_line.EntryType][column_line.Timeunit].ContainsKey(column_line.Timepoint))
                {
                    integrationGroup_entryType_timeunit_timepoint_sampleName_dict[column_line.IntegrationGroup][column_line.EntryType][column_line.Timeunit].Add(column_line.Timepoint, new Dictionary<string, List<Colchar_column_line_class>>());
                }
                if (!integrationGroup_entryType_timeunit_timepoint_sampleName_dict[column_line.IntegrationGroup][column_line.EntryType][column_line.Timeunit][column_line.Timepoint].ContainsKey(column_line.SampleName))
                {
                    integrationGroup_entryType_timeunit_timepoint_sampleName_dict[column_line.IntegrationGroup][column_line.EntryType][column_line.Timeunit][column_line.Timepoint].Add(column_line.SampleName, new List<Colchar_column_line_class>());
                }
                integrationGroup_entryType_timeunit_timepoint_sampleName_dict[column_line.IntegrationGroup][column_line.EntryType][column_line.Timeunit][column_line.Timepoint][column_line.SampleName].Add(column_line);
            }
            columns = null;
            List<Colchar_column_line_class> ordered_lines = new List<Colchar_column_line_class>();
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            Timeunit_enum[] timeunits;
            Timeunit_enum timeunit;
            int timeunits_length;
            float[] timepoints;
            float timepoint;
            int timepoints_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            integrationGroups = integrationGroup_entryType_timeunit_timepoint_sampleName_dict.Keys.ToArray();
            integrationGroups = integrationGroups.OrderBy(l => l).ToArray();
            integrationGroups_length = integrationGroups.Length;
            for (int indexIG = 0; indexIG < integrationGroups_length; indexIG++)
            {
                integrationGroup = integrationGroups[indexIG];
                entryType_timeunit_timepoint_sampleName_dict = integrationGroup_entryType_timeunit_timepoint_sampleName_dict[integrationGroup];
                entryTypes = entryType_timeunit_timepoint_sampleName_dict.Keys.ToArray();
                entryTypes = entryTypes.OrderBy(l => l).ToArray();
                entryTypes_length = entryTypes.Length;
                for (int indexET = 0; indexET < entryTypes_length; indexET++)
                {
                    entryType = entryTypes[indexET];
                    timeunit_timepoint_sampleName_dict = entryType_timeunit_timepoint_sampleName_dict[entryType];
                    timeunits = timeunit_timepoint_sampleName_dict.Keys.ToArray();
                    timeunits = timeunits.OrderBy(l => l).ToArray();
                    timeunits_length = timeunits.Length;
                    for (int indexTU = 0; indexTU < timeunits_length; indexTU++)
                    {
                        timeunit = timeunits[indexTU];
                        timepoint_sampleName_dict = timeunit_timepoint_sampleName_dict[timeunit];
                        timepoints = timepoint_sampleName_dict.Keys.ToArray();
                        timepoints = timepoints.OrderBy(l => l).ToArray();
                        timepoints_length = timepoints.Length;
                        for (int indexTP = 0; indexTP < timepoints_length; indexTP++)
                        {
                            timepoint = timepoints[indexTP];
                            sampleName_dict = timepoint_sampleName_dict[timepoint];
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
                if (ordered_lines.Count!=columns_length) { throw new Exception(); }
                Colchar_column_line_class this_column_line;
                Colchar_column_line_class previous_column_line;
                //columns.OrderBy(l => l.IntegrationGroup).ThenBy(l => l.EntryType).ThenBy(l => l.Timeunit).ThenBy(l => l.Timepoint).ThenBy(l => l.SampleName).ToArray();

                for (int indexThis=1; indexThis<columns_length;indexThis++)
                {
                    this_column_line = ordered_lines[indexThis];
                    previous_column_line = ordered_lines[indexThis - 1];
                    if (this_column_line.IntegrationGroup.CompareTo(previous_column_line.IntegrationGroup)<0) { throw new Exception(); }
                    else if ((this_column_line.IntegrationGroup.Equals(previous_column_line.IntegrationGroup))
                             && (this_column_line.EntryType.CompareTo(previous_column_line.EntryType) < 0)) { throw new Exception(); }
                    else if ((this_column_line.IntegrationGroup.Equals(previous_column_line.IntegrationGroup))
                             && (this_column_line.EntryType.Equals(previous_column_line.EntryType))
                             && (this_column_line.Timeunit.CompareTo(previous_column_line.Timeunit) < 0)) { throw new Exception(); }
                    else if ((this_column_line.IntegrationGroup.Equals(previous_column_line.IntegrationGroup))
                             && (this_column_line.EntryType.Equals(previous_column_line.EntryType))
                             && (this_column_line.Timeunit.Equals(previous_column_line.Timeunit))
                             && (this_column_line.Timepoint.CompareTo(previous_column_line.Timepoint) < 0)) { throw new Exception(); }
                    else if ((this_column_line.IntegrationGroup.Equals(previous_column_line.IntegrationGroup))
                             && (this_column_line.EntryType.Equals(previous_column_line.EntryType))
                             && (this_column_line.Timeunit.Equals(previous_column_line.Timeunit))
                             && (this_column_line.Timepoint.Equals(previous_column_line.Timepoint))
                             && (this_column_line.SampleName.CompareTo(previous_column_line.SampleName) < 0)) { throw new Exception(); }
                }
            }
            //return columns.OrderBy(l=>l.IntegrationGroup).ThenBy(l => l.EntryType).ThenBy(l=>l.Timeunit).ThenBy(l => l.Timepoint).ThenBy(l => l.SampleName).ToArray();
            return ordered_lines.ToArray();
        }

        public bool Equal_in_standard_way(IAdd_to_data other)
        {
            return (   (this.IntegrationGroup.Equals(other.IntegrationGroup_for_data))
                    && (this.EntryType.Equals(other.EntryType_for_data))
                    && (this.Timeunit.Equals(other.Timeunit_for_data))
                    && (this.Timepoint.Equals(other.Timepoint_for_data))
                    && (this.SampleName.Equals(other.SampleName_for_data)));
        }

        public bool Equal_in_standard_way(Colchar_column_line_class other)
        {
            return (   (this.EntryType.Equals(other.EntryType))
                    && (this.IntegrationGroup.Equals(other.IntegrationGroup))
                    && (this.Timeunit.Equals(other.Timeunit))
                    && (this.Timepoint.Equals(other.Timepoint))
                    && (this.SampleName.Equals(other.SampleName)));
        }

        public static bool Equal_in_standard_way(Colchar_column_line_class line1, Colchar_column_line_class line2)
        {
            return ((line1.EntryType.Equals(line2.EntryType))
                    && (line1.IntegrationGroup.Equals(line2.IntegrationGroup))
                    && (line1.Timeunit.Equals(line2.Timeunit))
                    && (line1.Timepoint.Equals(line2.Timepoint))
                    && (line1.SampleName.Equals(line2.SampleName)));
        }
        #endregion

        public Colchar_column_line_class Deep_copy()
        {
            Colchar_column_line_class copy = (Colchar_column_line_class)this.MemberwiseClone();
            copy.SampleName = (string)this.SampleName.Clone();
            copy.IntegrationGroup = (string)this.IntegrationGroup.Clone();
            copy.UniqueDataset_name = (string)this.UniqueDataset_name.Clone();
            return copy;
        }
    }

    class Colchar_class
    {
        #region Fields
        bool column_rearrangements_adopted;

        public Colchar_column_line_class[] Columns { get; set; }
        public int Columns_length { get { return Columns.Length; } }
        public bool Column_rearrangements_adopted
        { get { return column_rearrangements_adopted; }
          set
            {
                if (value.Equals(false))
                {
                    IntegrationGroup_entryType_timepoint_timeunit_sampleName_colIndex_dict.Clear();
                }
                column_rearrangements_adopted = value;
            }
        }
        private Dictionary<string,Dictionary<Entry_type_enum,Dictionary<float,Dictionary<Timeunit_enum,Dictionary<string,int>>>>> IntegrationGroup_entryType_timepoint_timeunit_sampleName_colIndex_dict { get; set; }
        #endregion

        public Colchar_class()
        {
            Column_rearrangements_adopted = true;
            Columns = new Colchar_column_line_class[0];
            IntegrationGroup_entryType_timepoint_timeunit_sampleName_colIndex_dict = new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<Timeunit_enum, Dictionary<string, int>>>>>();
        }

        #region Check
        public void Correctness_check()
        {
            int col_length = Columns_length;
            Colchar_column_line_class[] columns_copy = Deep_copy_columns();
            columns_copy = Colchar_column_line_class.Order_in_standard_way(columns_copy);
            for (int indexC = 1; indexC < col_length; indexC++)
            {
                if (columns_copy[indexC].Equal_in_standard_way(columns_copy[indexC - 1]))
                {
                    throw new Exception("duplicated column charactarization");
                }
            }
            if (!Column_rearrangements_adopted)
            {
                throw new Exception("column rearrangements are not adopted");
            }
        }
        #endregion

        #region Get
        public int Get_columnIndex(IAdd_to_data add_line)
        {
            int columns_length = Columns_length;
            Colchar_column_line_class column;
            int columnIndex = -1;
            for (int indexC = 0; indexC < columns_length; indexC++)
            {
                column = Columns[indexC];
                if (column.Equal_in_standard_way(add_line))
                {
                    columnIndex = indexC;
                    break;
                }
            }
            if (columnIndex == -1) { throw new Exception("column does not exist"); }
            return columnIndex;
        }
        public int Get_index_of_iadd_data_line_from_dictionary(IAdd_to_data add_data_line)
        {
            return IntegrationGroup_entryType_timepoint_timeunit_sampleName_colIndex_dict[add_data_line.IntegrationGroup_for_data][add_data_line.EntryType_for_data][add_data_line.Timepoint_for_data][add_data_line.Timeunit_for_data][add_data_line.SampleName_for_data];
        }

        public int[] Get_indexes_of_input_sampleNames(params string[] sampleNames)
        {
            Dictionary<string, bool> sampleName_dict = new Dictionary<string, bool>();
            foreach (string sampleName in sampleNames)
            {
                if (!sampleName_dict.ContainsKey(sampleName))
                {
                    sampleName_dict.Add(sampleName, false);
                }
            }
            int this_length = this.Columns_length;
            Colchar_column_line_class this_col_line;

            List<int> colIndexes = new List<int>();
            for (int indexThis = 0; indexThis < this_length; indexThis++)
            {
                this_col_line = this.Columns[indexThis];
                if (sampleName_dict.ContainsKey(this_col_line.SampleName))
                {
                    colIndexes.Add(indexThis);
                    sampleName_dict[this_col_line.SampleName] = true;
                }
            }
            foreach (string sampleName in sampleNames)
            {
                if (!sampleName_dict[sampleName]) { throw new Exception(); }
            }
            return colIndexes.ToArray();
        }

        public int[] Get_indexes_of_columns_that_contain_at_least_one_of_indicated_substrings(params string[] substrings)
        {
            substrings = substrings.OrderBy(l => l).ToArray();
            string substring;
            int substrings_length = substrings.Length;
            int this_length = this.Columns_length;
            Colchar_column_line_class this_col_line;

            List<int> colIndexes = new List<int>();
            for (int indexSampleName = 0; indexSampleName < substrings_length; indexSampleName++)
            {
                substring = substrings[indexSampleName];
                for (int indexThis = 0; indexThis < this_length; indexThis++)
                {
                    this_col_line = this.Columns[indexThis];
                    if (this_col_line.SampleName.IndexOf(substring) !=-1)
                    { 
                        colIndexes.Add(indexThis);
                    }
                }
            }
            return colIndexes.ToArray();
        }

        public void Get_entryType_timepoint_and_name_of_indicated_column(out Entry_type_enum entryType, out float timepoint, out string name, int indexColumn)
        {
            entryType = Columns[indexColumn].EntryType;
            timepoint = Columns[indexColumn].Timepoint;
            name = (string)Columns[indexColumn].SampleName.Clone();
        }
        #endregion

        #region Keep
        public void Keep_only_input_columns(params int[] inputColumns)
        {
            Correctness_check();
            int inputColumns_length = inputColumns.Length;
            Colchar_column_line_class[] new_columns = new Colchar_column_line_class[inputColumns_length];
            for (int indexInput = 0; indexInput < inputColumns_length; indexInput++)
            {
                new_columns[indexInput] = this.Columns[inputColumns[indexInput]].Deep_copy();
            }
            Columns = new_columns;
            Column_rearrangements_adopted = false;
        }
        #endregion

        public void Generate_integrationGroup_entryType_timepoint_timeunit_sampleName_colIndex_dict()
        {
            IntegrationGroup_entryType_timepoint_timeunit_sampleName_colIndex_dict.Clear();
            int col_length = this.Columns_length;
            Colchar_column_line_class column_line;
            for (int indexCol=0; indexCol<col_length;indexCol++)
            {
                column_line = this.Columns[indexCol];
                if (!IntegrationGroup_entryType_timepoint_timeunit_sampleName_colIndex_dict.ContainsKey(column_line.IntegrationGroup))
                {
                    IntegrationGroup_entryType_timepoint_timeunit_sampleName_colIndex_dict.Add(column_line.IntegrationGroup, new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<Timeunit_enum, Dictionary<string, int>>>>());
                }
                if (!IntegrationGroup_entryType_timepoint_timeunit_sampleName_colIndex_dict[column_line.IntegrationGroup].ContainsKey(column_line.EntryType))
                {
                    IntegrationGroup_entryType_timepoint_timeunit_sampleName_colIndex_dict[column_line.IntegrationGroup].Add(column_line.EntryType, new Dictionary<float, Dictionary<Timeunit_enum, Dictionary<string, int>>>());
                }
                if (!IntegrationGroup_entryType_timepoint_timeunit_sampleName_colIndex_dict[column_line.IntegrationGroup][column_line.EntryType].ContainsKey(column_line.Timepoint))
                {
                    IntegrationGroup_entryType_timepoint_timeunit_sampleName_colIndex_dict[column_line.IntegrationGroup][column_line.EntryType].Add(column_line.Timepoint, new Dictionary<Timeunit_enum, Dictionary<string, int>>());
                }
                if (!IntegrationGroup_entryType_timepoint_timeunit_sampleName_colIndex_dict[column_line.IntegrationGroup][column_line.EntryType][column_line.Timepoint].ContainsKey(column_line.Timeunit))
                {
                    IntegrationGroup_entryType_timepoint_timeunit_sampleName_colIndex_dict[column_line.IntegrationGroup][column_line.EntryType][column_line.Timepoint].Add(column_line.Timeunit, new Dictionary<string, int>());
                }
                IntegrationGroup_entryType_timepoint_timeunit_sampleName_colIndex_dict[column_line.IntegrationGroup][column_line.EntryType][column_line.Timepoint][column_line.Timeunit].Add(column_line.SampleName, indexCol);
            }
        }

        #region Add
        public void Identify_new_columns_and_add_at_right_site(Colchar_column_line_class[] add_data)
        {
            Correctness_check();

            int old_columns_length = Columns_length;
            Colchar_column_line_class[] add_columns;

            #region Identify add columns
            List<Colchar_column_line_class> add_columns_list = new List<Colchar_column_line_class>();
            add_data = Colchar_column_line_class.Order_in_standard_way(add_data);
            int add_data_length = add_data.Length;
            Colchar_column_line_class add_line;
            bool old_columns_contain_new_combination = false;
            Colchar_column_line_class old_column_line;
            Colchar_column_line_class add_column_line;
            for (int indexAdd = 0; indexAdd < add_data_length; indexAdd++)
            {
                add_line = add_data[indexAdd];
                if ((indexAdd == 0) || (!Colchar_column_line_class.Equal_in_standard_way(add_line, add_data[indexAdd - 1])))
                {
                    old_columns_contain_new_combination = false;
                    for (int indexOld = 0; indexOld < old_columns_length; indexOld++)
                    {
                        old_column_line = Columns[indexOld];
                        if (old_column_line.Equal_in_standard_way(add_line))
                        {
                            old_columns_contain_new_combination = true;
                            break;
                        }
                    }
                    if (!old_columns_contain_new_combination)
                    {
                        add_column_line = new Colchar_column_line_class(add_line);
                        add_columns_list.Add(add_column_line);
                    }
                }
            }
            add_columns = add_columns_list.ToArray();
            #endregion

            #region Add add columns at right site
            int add_columns_length = add_columns.Length;
            int new_columns_length = add_columns_length + old_columns_length;
            Colchar_column_line_class[] new_columns = new Colchar_column_line_class[new_columns_length];
            int indexNew = -1;
            for (int indexOld = 0; indexOld < old_columns_length; indexOld++)
            {
                indexNew++;
                new_columns[indexNew] = this.Columns[indexOld].Deep_copy();
            }
            for (int indexAdd = 0; indexAdd < add_columns_length; indexAdd++)
            {
                indexNew++;
                new_columns[indexNew] = add_columns[indexAdd].Deep_copy();
            }
            Columns = new_columns;
            #endregion

            Correctness_check();
        }
        #endregion

        #region Copy
        private Colchar_column_line_class[] Deep_copy_columns()
        {
            int columns_length = Columns.Length;
            Colchar_column_line_class[] copy_columns = new Colchar_column_line_class[columns_length];
            for (int indexC = 0; indexC < columns_length; indexC++)
            {
                copy_columns[indexC] = this.Columns[indexC].Deep_copy();
            }
            return copy_columns;
        }

        public Colchar_class Deep_copy()
        {
            Colchar_class copy = (Colchar_class)this.MemberwiseClone();
            int columns_length = Columns.Length;
            copy.Columns = Deep_copy_columns();
            return copy;
        }
        #endregion
    }

    ///////////////////////////////////////////////////////////////

    class Data_line_class
    {
        #region Fields
        public static double Empty_entry { get { return 0; } }
        public double[] Columns { get; set; }
        public int Columns_length { get { return Columns.Length; } }
        public int NCBI_geneID { get; set; }
        public string NCBI_official_symbol { get; set; }
        public string[] Scps { get; set; }
        public string NCBI_description { get; set; }
        #endregion

        #region Order
        public static Data_line_class[] Order_by_ncbiOfficialSymbol(Data_line_class[] data_lines)
        {
            Dictionary<string, List<Data_line_class>> symbol_dict = new Dictionary<string, List<Data_line_class>>();
            int data_lines_length = data_lines.Length;
            Data_line_class data_line;
            for (int indexD=0; indexD<data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                if (!symbol_dict.ContainsKey(data_line.NCBI_official_symbol))
                {
                    symbol_dict.Add(data_line.NCBI_official_symbol, new List<Data_line_class>());
                }
                symbol_dict[data_line.NCBI_official_symbol].Add(data_line);
            }
            data_lines = null;
            List<Data_line_class> ordered_lines = new List<Data_line_class>();
            string[] symbols = symbol_dict.Keys.ToArray();
            string symbol;
            int symbols_length = symbols.Length;
            symbols = symbols.OrderBy(l => l).ToArray();
            for (int indexS=0; indexS<symbols_length;indexS++)
            {
                symbol = symbols[indexS];
                ordered_lines.AddRange(symbol_dict[symbol]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Data_line_class this_line;
                Data_line_class previous_line;
                for (int indexThis=1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (this_line.NCBI_official_symbol.CompareTo(previous_line.NCBI_official_symbol)<0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }

        public static Data_line_class[] Order_by_lengthOfNcbiOfficialSymbol_and_then_by_ncbiOfficialSymbol(Data_line_class[] data_lines)
        {
            Dictionary<int, Dictionary<string, List<Data_line_class>>> lengthOfSymbol_symbol_dict = new Dictionary<int, Dictionary<string, List<Data_line_class>>>();
            Dictionary<string, List<Data_line_class>> symbol_dict;
            int data_lines_length = data_lines.Length;
            Data_line_class data_line;
            int lengthOfSymbol;
            for (int indexD = 0; indexD < data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                lengthOfSymbol = data_line.NCBI_official_symbol.Length;
                if (!lengthOfSymbol_symbol_dict.ContainsKey(lengthOfSymbol))
                {
                    lengthOfSymbol_symbol_dict.Add(lengthOfSymbol, new Dictionary<string, List<Data_line_class>>());
                }
                if (!lengthOfSymbol_symbol_dict[lengthOfSymbol].ContainsKey(data_line.NCBI_official_symbol))
                {
                    lengthOfSymbol_symbol_dict[lengthOfSymbol].Add(data_line.NCBI_official_symbol, new List<Data_line_class>());
                }
                lengthOfSymbol_symbol_dict[lengthOfSymbol][data_line.NCBI_official_symbol].Add(data_line);
            }
            data_lines = null;
            List<Data_line_class> ordered_lines = new List<Data_line_class>();
            int[] lengthOfSymbol_array = lengthOfSymbol_symbol_dict.Keys.ToArray();
            int lengthOfSymbol_array_length = lengthOfSymbol_array.Length;
            string[] symbols;
            string symbol;
            int symbols_length;
            lengthOfSymbol_array = lengthOfSymbol_array.OrderBy(l => l).ToArray();
            for (int indexSL=0; indexSL < lengthOfSymbol_array_length;indexSL++) 
            {
                lengthOfSymbol = lengthOfSymbol_array[indexSL];
                symbol_dict = lengthOfSymbol_symbol_dict[lengthOfSymbol];
                symbols = symbol_dict.Keys.ToArray();
                symbols_length = symbols.Length;
                symbols = symbols.OrderBy(l => l).ToArray();
                for (int indexS = 0; indexS < symbols_length; indexS++)
                {
                    symbol = symbols[indexS];
                    ordered_lines.AddRange(symbol_dict[symbol]);
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Data_line_class this_line;
                Data_line_class previous_line;
                int this_lengthOfSymbol;
                int previous_lengthOfSymbol;
                for (int indexThis = 1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    this_lengthOfSymbol = this_line.NCBI_official_symbol.Length;
                    previous_lengthOfSymbol = previous_line.NCBI_official_symbol.Length;
                    if (this_lengthOfSymbol.CompareTo(previous_lengthOfSymbol) < 0) { throw new Exception(); }
                    else if (  (this_lengthOfSymbol.Equals(previous_lengthOfSymbol))
                             &&(this_line.NCBI_official_symbol.CompareTo(previous_line.NCBI_official_symbol)<0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }

        public static Data_line_class[] Order_by_descendingValueInIndicatedColumn(Data_line_class[] data_lines, int indexColumn)
        {
            Dictionary<double, List<Data_line_class>> value_dict = new Dictionary<double, List<Data_line_class>>();
            int data_lines_length = data_lines.Length;
            Data_line_class data_line;
            for (int indexD = 0; indexD < data_lines_length; indexD++)
            {
                data_line = data_lines[indexD];
                if (!value_dict.ContainsKey(data_line.Columns[indexColumn]))
                {
                    value_dict.Add(data_line.Columns[indexColumn], new List<Data_line_class>());
                }
                value_dict[data_line.Columns[indexColumn]].Add(data_line);
            }
            data_lines = null;
            List<Data_line_class> ordered_lines = new List<Data_line_class>();
            double[] data_values = value_dict.Keys.ToArray();
            double data_value;
            int data_values_length = data_values.Length;
            data_values = data_values.OrderByDescending(l => l).ToArray();
            for (int indexV = 0; indexV < data_values_length; indexV++)
            {
                data_value = data_values[indexV];
                ordered_lines.AddRange(value_dict[data_value]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != data_lines_length) { throw new Exception(); }
                Data_line_class this_line;
                Data_line_class previous_line;
                for (int indexThis = 1; indexThis < data_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (this_line.Columns[indexColumn].CompareTo(previous_line.Columns[indexColumn]) > 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        #endregion



        public Data_line_class(string ncbi_symbol, int columns_length)
        {
            NCBI_official_symbol = (string)ncbi_symbol.Clone();
            NCBI_description = "";
            Scps = new string[0];
            Columns = new double[columns_length];
            for (int indexCol = 0; indexCol < columns_length; indexCol++)
            {
                Columns[indexCol] = Empty_entry;
            }
        }

        public void Rearrange_columns(int[] old_new_index)
        {
            double[] old_columns = Deep_copy_columns();
            int columns_length = Columns.Length;
            for (int indexC = 0; indexC < columns_length; indexC++)
            {
                Columns[indexC] = 0;
            }
            for (int indexC = 0; indexC < columns_length; indexC++)
            {
                Columns[old_new_index[indexC]] = old_columns[indexC];
            }
        }

        public void Add_to_this_line_after_checking_if_this_line_has_empty_entry(IAdd_to_data add_line, int indexCol)
        {
            if (!NCBI_official_symbol.Equals(add_line.NCBI_official_symbol_for_data))
            {
                throw new Exception("rowNames do not match");
            }
            if (Columns[indexCol].Equals(Empty_entry))
            {
                Columns[indexCol] = add_line.Value_for_data;
            }
            else
            {
                throw new Exception("Position already filled");
            }
        }

        public void Add_nonemtpyt_values_of_other_line_to_this_line_if_this_line_has_empty_value(Data_line_class other)
        {
            int columns_length = Columns_length;
            for (int indexC = 0; indexC < columns_length; indexC++)
            {
                if ((this.Columns[indexC] != Empty_entry)
                    && (other.Columns[indexC] != Empty_entry))
                {
                    throw new Exception();
                }
                else if (other.Columns[indexC] != Empty_entry)
                {
                    this.Columns[indexC] = other.Columns[indexC];
                }
            }
        }

        public void Keep_columns(params int[] kept_columnIndexes)
        {
            List<double> keep_columns = new List<double>();
            int columns_length = this.Columns_length;
            for (int indexColumn = 0; indexColumn < columns_length; indexColumn++)
            {
                if (kept_columnIndexes.Contains(indexColumn))
                {
                    keep_columns.Add(this.Columns[indexColumn]);
                }
            }
            this.Columns = keep_columns.ToArray();
        }

        #region Copy
        private double[] Deep_copy_columns()
        {
            int columns_length = Columns.Length;
            double[] copy = new double[columns_length];
            for (int indexC = 0; indexC < columns_length; indexC++)
            {
                copy[indexC] = Columns[indexC];
            }
            return copy;
        }

        public Data_line_class Deep_copy()
        {
            Data_line_class copy = (Data_line_class)this.MemberwiseClone();
            copy.NCBI_official_symbol = (string)this.NCBI_official_symbol.Clone();
            copy.NCBI_description = (string)this.NCBI_description.Clone();
            copy.Columns = Deep_copy_columns();
            int scps_length = this.Scps.Length;
            copy.Scps = new string[scps_length];
            for (int indexScp=0; indexScp<scps_length; indexScp++)
            {
                copy.Scps[indexScp] = (string)this.Scps[indexScp].Clone();
            }
            return copy;
        }
        #endregion
    }

    class Data_private_readWriteOptions_class
    {
        public string Complete_fileName { get; set; }
        public char LineDelimiter { get; set; }

        public Data_private_readWriteOptions_class(string subdirectory, string file_name)
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            string complete_directory = global_dirFile.Results_directory + subdirectory;
            ReadWriteClass.Create_directory_if_it_does_not_exist(complete_directory);
            Complete_fileName = complete_directory + file_name;

            LineDelimiter = Global_class.Tab;
        }
    }

    class Data_class
    {
        #region Fields
        public Data_line_class[] Data { get; set; }
        public Data_line_class Column_entries_count_line { get; set; }
        public int Data_length { get { return Data.Length; } }
        public Colchar_class ColChar { get; set; }
        #endregion

        public Data_class()
        {
            ColChar = new Colchar_class();
            Data = new Data_line_class[0];
            Column_entries_count_line = new Data_line_class("Column entries count",0);
        }

        #region Check
        public void Correctness_check()
        {
            ColChar.Correctness_check();
            int col_length = ColChar.Columns.Length;
            int data_length = Data.Length;
            Data_line_class data_line;
            for (int indexD = 0; indexD < data_length; indexD++)
            {
                data_line = Data[indexD];
                if (data_line.Columns.Length != col_length)
                {
                    throw new Exception("Column lengths do not match");
                }
            }
        }
        #endregion

        #region Order
        public void Order_by_ncbiOfficialSymbol()
        {
            //Data = Data.OrderBy(l => l.NCBI_official_symbol).ToArray();
            Data = Data_line_class.Order_by_ncbiOfficialSymbol(Data);
        }

        public void Order_by_lengthOfNcbiOfficialSymbol_and_then_by_ncbiOfficialSymbol()
        {
            //Data = Data.OrderBy(l=>l.NCBI_official_symbol.Length).ThenBy(l => l.NCBI_official_symbol).ToArray();
            Data = Data_line_class.Order_by_lengthOfNcbiOfficialSymbol_and_then_by_ncbiOfficialSymbol(Data);
        }

        public void Order_by_descending_value_in_indicated_column(int indexColumn)
        {
            //Data = Data.OrderByDescending(l => l.Columns[indexColumn]).ToArray();
            Data = Data_line_class.Order_by_descendingValueInIndicatedColumn(Data, indexColumn);
        }
        #endregion

        #region Fill de instance
        private IAdd_to_data[] Order_IAdd_to_data_by_integrationGroup_entryType_timeunit_timepoint_sampleName_value(IAdd_to_data[] iadd_lines)
        {
            Dictionary<string, Dictionary<Entry_type_enum, Dictionary<Timeunit_enum, Dictionary<float, Dictionary<string, Dictionary<double, List<IAdd_to_data>>>>>>> integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict = new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<Timeunit_enum, Dictionary<float, Dictionary<string, Dictionary<double, List<IAdd_to_data>>>>>>>();
            Dictionary<Entry_type_enum, Dictionary<Timeunit_enum, Dictionary<float, Dictionary<string, Dictionary<double, List<IAdd_to_data>>>>>> entryType_timeunit_timepoint_sampleName_value_dict = new Dictionary<Entry_type_enum, Dictionary<Timeunit_enum, Dictionary<float, Dictionary<string, Dictionary<double, List<IAdd_to_data>>>>>>();
            Dictionary<Timeunit_enum, Dictionary<float, Dictionary<string, Dictionary<double, List<IAdd_to_data>>>>> timeunit_timepoint_sampleName_value_dict = new Dictionary<Timeunit_enum, Dictionary<float, Dictionary<string, Dictionary<double, List<IAdd_to_data>>>>>();
            Dictionary<float, Dictionary<string, Dictionary<double, List<IAdd_to_data>>>> timepoint_sampleName_value_dict = new Dictionary<float, Dictionary<string, Dictionary<double, List<IAdd_to_data>>>>();
            Dictionary<string, Dictionary<double, List<IAdd_to_data>>> sampleName_value_dict = new Dictionary<string, Dictionary<double, List<IAdd_to_data>>>();
            Dictionary<double, List<IAdd_to_data>> value_dict = new Dictionary<double, List<IAdd_to_data>>();
            int iadd_lines_length = iadd_lines.Length;
            IAdd_to_data iadd_line;
            for (int indexL = 0; indexL < iadd_lines_length; indexL++)
            {
                iadd_line = iadd_lines[indexL];
                if (!integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict.ContainsKey(iadd_line.IntegrationGroup_for_data))
                {
                    integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict.Add(iadd_line.IntegrationGroup_for_data, new Dictionary<Entry_type_enum, Dictionary<Timeunit_enum, Dictionary<float, Dictionary<string, Dictionary<double, List<IAdd_to_data>>>>>>());
                }
                if (!integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict[iadd_line.IntegrationGroup_for_data].ContainsKey(iadd_line.EntryType_for_data))
                {
                    integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict[iadd_line.IntegrationGroup_for_data].Add(iadd_line.EntryType_for_data, new Dictionary<Timeunit_enum, Dictionary<float, Dictionary<string, Dictionary<double, List<IAdd_to_data>>>>>());
                }
                if (!integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict[iadd_line.IntegrationGroup_for_data][iadd_line.EntryType_for_data].ContainsKey(iadd_line.Timeunit_for_data))
                {
                    integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict[iadd_line.IntegrationGroup_for_data][iadd_line.EntryType_for_data].Add(iadd_line.Timeunit_for_data, new Dictionary<float, Dictionary<string, Dictionary<double, List<IAdd_to_data>>>>());
                }
                if (!integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict[iadd_line.IntegrationGroup_for_data][iadd_line.EntryType_for_data][iadd_line.Timeunit_for_data].ContainsKey(iadd_line.Timepoint_for_data))
                {
                    integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict[iadd_line.IntegrationGroup_for_data][iadd_line.EntryType_for_data][iadd_line.Timeunit_for_data].Add(iadd_line.Timepoint_for_data, new Dictionary<string, Dictionary<double, List<IAdd_to_data>>>());
                }
                if (!integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict[iadd_line.IntegrationGroup_for_data][iadd_line.EntryType_for_data][iadd_line.Timeunit_for_data][iadd_line.Timepoint_for_data].ContainsKey(iadd_line.SampleName_for_data))
                {
                    integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict[iadd_line.IntegrationGroup_for_data][iadd_line.EntryType_for_data][iadd_line.Timeunit_for_data][iadd_line.Timepoint_for_data].Add(iadd_line.SampleName_for_data, new Dictionary<double, List<IAdd_to_data>>());
                }
                if (!integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict[iadd_line.IntegrationGroup_for_data][iadd_line.EntryType_for_data][iadd_line.Timeunit_for_data][iadd_line.Timepoint_for_data][iadd_line.SampleName_for_data].ContainsKey(iadd_line.Value_for_data))
                {
                    integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict[iadd_line.IntegrationGroup_for_data][iadd_line.EntryType_for_data][iadd_line.Timeunit_for_data][iadd_line.Timepoint_for_data][iadd_line.SampleName_for_data].Add(iadd_line.Value_for_data, new List<IAdd_to_data>());
                }
                if (!integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict[iadd_line.IntegrationGroup_for_data][iadd_line.EntryType_for_data][iadd_line.Timeunit_for_data][iadd_line.Timepoint_for_data][iadd_line.SampleName_for_data].ContainsKey(iadd_line.Value_for_data))
                {
                    integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict[iadd_line.IntegrationGroup_for_data][iadd_line.EntryType_for_data][iadd_line.Timeunit_for_data][iadd_line.Timepoint_for_data][iadd_line.SampleName_for_data].Add(iadd_line.Value_for_data, new List<IAdd_to_data>());
                }
                integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict[iadd_line.IntegrationGroup_for_data][iadd_line.EntryType_for_data][iadd_line.Timeunit_for_data][iadd_line.Timepoint_for_data][iadd_line.SampleName_for_data][iadd_line.Value_for_data].Add(iadd_line);
            }
            iadd_lines = null;
            List<IAdd_to_data> ordered_lines = new List<IAdd_to_data>();
            string[] integrationGroups;
            string integrationGroup;
            int integrationGroups_length;
            Entry_type_enum[] entryTypes;
            Entry_type_enum entryType;
            int entryTypes_length;
            Timeunit_enum[] timeunits;
            Timeunit_enum timeunit;
            int timeunits_length;
            float[] timepoints;
            float timepoint;
            int timepoints_length;
            string[] sampleNames;
            string sampleName;
            int sampleNames_length;
            double[] data_values;
            double data_value;
            int data_values_length;
            integrationGroups = integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict.Keys.ToArray();
            integrationGroups = integrationGroups.OrderBy(l => l).ToArray();
            integrationGroups_length = integrationGroups.Length;
            for (int indexIG = 0; indexIG < integrationGroups_length; indexIG++)
            {
                integrationGroup = integrationGroups[indexIG];
                entryType_timeunit_timepoint_sampleName_value_dict = integrationGroup_entryType_timeunit_timepoint_sampleName_value_dict[integrationGroup];
                entryTypes = entryType_timeunit_timepoint_sampleName_value_dict.Keys.ToArray();
                entryTypes = entryTypes.OrderBy(l => l).ToArray();
                entryTypes_length = entryTypes.Length;
                for (int indexET = 0; indexET < entryTypes_length; indexET++)
                {
                    entryType = entryTypes[indexET];
                    timeunit_timepoint_sampleName_value_dict = entryType_timeunit_timepoint_sampleName_value_dict[entryType];
                    timeunits = timeunit_timepoint_sampleName_value_dict.Keys.ToArray();
                    timeunits = timeunits.OrderBy(l => l).ToArray();
                    timeunits_length = timeunits.Length;
                    for (int indexTU = 0; indexTU < timeunits_length; indexTU++)
                    {
                        timeunit = timeunits[indexTU];
                        timepoint_sampleName_value_dict = timeunit_timepoint_sampleName_value_dict[timeunit];
                        timepoints = timepoint_sampleName_value_dict.Keys.ToArray();
                        timepoints = timepoints.OrderBy(l => l).ToArray();
                        timepoints_length = timepoints.Length;
                        for (int indexTP = 0; indexTP < timepoints_length; indexTP++)
                        {
                            timepoint = timepoints[indexTP];
                            sampleName_value_dict = timepoint_sampleName_value_dict[timepoint];
                            sampleNames = sampleName_value_dict.Keys.ToArray();
                            sampleNames = sampleNames.OrderBy(l => l).ToArray();
                            sampleNames_length = sampleNames.Length;
                            for (int indexSN = 0; indexSN < sampleNames_length; indexSN++)
                            {
                                sampleName = sampleNames[indexSN];
                                value_dict = sampleName_value_dict[sampleName];
                                data_values = value_dict.Keys.ToArray();
                                data_values = data_values.OrderBy(l => l).ToArray();
                                data_values_length = data_values.Length;
                                for (int indexDV = 0; indexDV < data_values_length; indexDV++)
                                {
                                    data_value = data_values[indexDV];
                                    ordered_lines.AddRange(value_dict[data_value]);
                                }
                            }
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != iadd_lines_length) { throw new Exception(); }
                IAdd_to_data this_line;
                IAdd_to_data previous_line;
                //integrationGroup_entryType_timeunit_timepoint_sampleName_value
                for (int indexO = 1; indexO < iadd_lines_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.IntegrationGroup_for_data.CompareTo(previous_line.IntegrationGroup_for_data) < 0) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup_for_data.Equals(previous_line.IntegrationGroup_for_data))
                             && (this_line.EntryType_for_data.CompareTo(previous_line.EntryType_for_data) < 0)) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup_for_data.Equals(previous_line.IntegrationGroup_for_data))
                             && (this_line.EntryType_for_data.Equals(previous_line.EntryType_for_data))
                             && (this_line.Timeunit_for_data.CompareTo(previous_line.Timeunit_for_data) < 0)) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup_for_data.Equals(previous_line.IntegrationGroup_for_data))
                             && (this_line.EntryType_for_data.Equals(previous_line.EntryType_for_data))
                             && (this_line.Timeunit_for_data.Equals(previous_line.Timeunit_for_data))
                             && (this_line.Timepoint_for_data.CompareTo(previous_line.Timepoint_for_data) < 0)) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup_for_data.Equals(previous_line.IntegrationGroup_for_data))
                             && (this_line.EntryType_for_data.Equals(previous_line.EntryType_for_data))
                             && (this_line.Timeunit_for_data.Equals(previous_line.Timeunit_for_data))
                             && (this_line.Timepoint_for_data.Equals(previous_line.Timepoint_for_data))
                             && (this_line.SampleName_for_data.CompareTo(previous_line.SampleName_for_data) < 0)) { throw new Exception(); }
                    else if ((this_line.IntegrationGroup_for_data.Equals(previous_line.IntegrationGroup_for_data))
                             && (this_line.EntryType_for_data.Equals(previous_line.EntryType_for_data))
                             && (this_line.Timeunit_for_data.Equals(previous_line.Timeunit_for_data))
                             && (this_line.Timepoint_for_data.Equals(previous_line.Timepoint_for_data))
                             && (this_line.SampleName_for_data.Equals(previous_line.SampleName_for_data))
                             && (this_line.Value_for_data.CompareTo(previous_line.Value_for_data) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        private IAdd_to_data[] Order_IAdd_to_data_by_ncbiOfficialSymbol(IAdd_to_data[] iadd_lines)
        {
            Dictionary<string, List<IAdd_to_data>> symbol_dict = new Dictionary<string, List<IAdd_to_data>>();
            int iadd_lines_length = iadd_lines.Length;
            IAdd_to_data iadd_line;
            for (int indexL = 0; indexL < iadd_lines_length; indexL++)
            {
                iadd_line = iadd_lines[indexL];
                if (!symbol_dict.ContainsKey(iadd_line.NCBI_official_symbol_for_data))
                {
                    symbol_dict.Add(iadd_line.NCBI_official_symbol_for_data, new List<IAdd_to_data>());
                }
                symbol_dict[iadd_line.NCBI_official_symbol_for_data].Add(iadd_line);
            }
            iadd_lines = null;
            List<IAdd_to_data> ordered_lines = new List<IAdd_to_data>();
            string[] symbols;
            string symbol;
            int symbols_length;
            symbols = symbol_dict.Keys.ToArray();
            symbols = symbols.OrderBy(l => l).ToArray();
            symbols_length = symbols.Length;
            for (int indexS = 0; indexS < symbols_length; indexS++)
            {
                symbol = symbols[indexS];
                ordered_lines.AddRange(symbol_dict[symbol]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != iadd_lines_length) { throw new Exception(); }
                IAdd_to_data this_line;
                IAdd_to_data previous_line;
                for (int indexO = 1; indexO < iadd_lines_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.NCBI_official_symbol_for_data.CompareTo(previous_line.NCBI_official_symbol_for_data) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        public void Add_to_data_instance(IAdd_to_data[] add_data)
        {
            int old_columns_length = ColChar.Columns_length;
            int add_data_length = add_data.Length;
            IAdd_to_data add_data_interface_line;

            #region Analyze, if new columns needed and add to colChar
            //add_data = add_data.OrderBy(l=>l.IntegrationGroup_for_data).ThenBy(l => l.EntryType_for_data).ThenBy(l=>l.Timeunit_for_data).ThenBy(l => l.Timepoint_for_data).ThenBy(l => l.SampleName_for_data).ThenBy(l=>l.Value_for_data).ToArray();
            add_data = Order_IAdd_to_data_by_integrationGroup_entryType_timeunit_timepoint_sampleName_value(add_data);
            List<Colchar_column_line_class> colChar_column_list = new List<Colchar_column_line_class>();
            Colchar_column_line_class colChar_column_line;
            for (int indexAdd=0; indexAdd<add_data_length; indexAdd++)
            {
                add_data_interface_line = add_data[indexAdd];
                if (  (indexAdd==0)
                    || (!add_data_interface_line.IntegrationGroup_for_data.Equals(add_data[indexAdd - 1].IntegrationGroup_for_data))
                    || (!add_data_interface_line.EntryType_for_data.Equals(add_data[indexAdd - 1].EntryType_for_data))
                    || (!add_data_interface_line.Timeunit_for_data.Equals(add_data[indexAdd - 1].Timeunit_for_data))
                    || (!add_data_interface_line.Timepoint_for_data.Equals(add_data[indexAdd - 1].Timepoint_for_data))
                    || (!add_data_interface_line.SampleName_for_data.Equals(add_data[indexAdd - 1].SampleName_for_data)))
                {
                    colChar_column_line = new Colchar_column_line_class();
                    colChar_column_line.IntegrationGroup = (string)add_data_interface_line.IntegrationGroup_for_data.Clone();
                    colChar_column_line.EntryType = add_data_interface_line.EntryType_for_data;
                    colChar_column_line.Timeunit = add_data_interface_line.Timeunit_for_data;
                    colChar_column_line.Timepoint = add_data_interface_line.Timepoint_for_data;
                    colChar_column_line.SampleName = (string)add_data_interface_line.SampleName_for_data.Clone();
                    colChar_column_line.SampleColor = add_data_interface_line.SampleColor_for_data;
                    colChar_column_line.Results_no = add_data_interface_line.Results_number_for_data;
                    colChar_column_line.UniqueDataset_name = (string)add_data_interface_line.UniqueDatasetName_for_data.Clone();
                    colChar_column_list.Add(colChar_column_line);
                }
            }
            ColChar.Identify_new_columns_and_add_at_right_site(colChar_column_list.ToArray());
            ColChar.Generate_integrationGroup_entryType_timepoint_timeunit_sampleName_colIndex_dict();
            int new_columns_length = ColChar.Columns.Length;
            #endregion

            int data_length = this.Data_length;
            Data_line_class data_line;

            #region Extend data lines by adding zero values at right site
            double[] new_columns;
            for (int indexD = 0; indexD < data_length; indexD++)
            {
                data_line = this.Data[indexD];
                new_columns = new double[new_columns_length];
                for (int indexOld = 0; indexOld < old_columns_length; indexOld++)
                {
                    new_columns[indexOld] = data_line.Columns[indexOld];
                }
                data_line.Columns = new_columns.ToArray();
            }
            #endregion

            #region Add to data and generate ordered data add lines to data
            //add_data = add_data.OrderBy(l => l.NCBI_official_symbol_for_data).ToArray();
            add_data = Order_IAdd_to_data_by_ncbiOfficialSymbol(add_data);


            int indexData = 0;
            int stringCompare;
            Data_line_class new_data_line;
            List<Data_line_class> add_lines_to_data_list = new List<Data_line_class>();
            int indexCol = -1;

            for (int indexAdd = 0; indexAdd < add_data_length; indexAdd++)
            {
                add_data_interface_line = add_data[indexAdd];
                indexCol = ColChar.Get_index_of_iadd_data_line_from_dictionary(add_data_interface_line);
                stringCompare = -2;
                while ((indexData < data_length) && (stringCompare < 0))
                {
                    data_line = Data[indexData];
                    stringCompare = data_line.NCBI_official_symbol.CompareTo(add_data_interface_line.NCBI_official_symbol_for_data);
                    if (stringCompare < 0)
                    {
                        indexData++;
                    }
                    else if (stringCompare == 0)
                    {
                        data_line.Add_to_this_line_after_checking_if_this_line_has_empty_entry(add_data_interface_line, indexCol);
                    }
                }
                if (stringCompare != 0)
                {
                    new_data_line = new Data_line_class(add_data_interface_line.NCBI_official_symbol_for_data, new_columns_length);
                    new_data_line.Columns[indexCol] = add_data_interface_line.Value_for_data;
                    add_lines_to_data_list.Add(new_data_line);
                }
            }
            //Data_line_class[] add_lines_to_data = add_lines_to_data_list.OrderBy(l => l.NCBI_official_symbol).ToArray();
            Data_line_class[] add_lines_to_data = Data_line_class.Order_by_ncbiOfficialSymbol(add_lines_to_data_list.ToArray());
            #endregion

            #region Combine add_lines_to_data to generate final add_data_lines
            int add_lines_length = add_lines_to_data.Length;
            int firstIndex_same_rowName = -1;
            Data_line_class add_data_line;
            Data_line_class inner_add_data_line;
            List<Data_line_class> final_add_data_list = new List<Data_line_class>();
            for (int indexAL = 0; indexAL < add_lines_length; indexAL++)
            {
                add_data_line = add_lines_to_data[indexAL];
                if ((indexAL == 0) || (!add_data_line.NCBI_official_symbol.Equals(add_lines_to_data[indexAL - 1].NCBI_official_symbol)))
                {
                    firstIndex_same_rowName = indexAL;
                }
                if ((indexAL == add_lines_length - 1) || (!add_data_line.NCBI_official_symbol.Equals(add_lines_to_data[indexAL + 1].NCBI_official_symbol)))
                {
                    for (int indexInner = firstIndex_same_rowName; indexInner < indexAL; indexInner++)
                    {
                        inner_add_data_line = add_lines_to_data[indexInner];
                        add_data_line.Add_nonemtpyt_values_of_other_line_to_this_line_if_this_line_has_empty_value(inner_add_data_line);
                    }
                    final_add_data_list.Add(add_data_line);
                }
            }
            #endregion

            #region Add final data line to data
            int final_data_length = final_add_data_list.Count;
            int new_data_length = final_data_length + data_length;
            Data_line_class[] new_data = new Data_line_class[new_data_length];
            int indexNew = -1;
            for (int indexOld = 0; indexOld < data_length; indexOld++)
            {
                indexNew++;
                new_data[indexNew] = Data[indexOld];
            }
            for (int indexFinal = 0; indexFinal < final_data_length; indexFinal++)
            {
                indexNew++;
                new_data[indexNew] = final_add_data_list[indexFinal];
            }
            Data = new_data;
            #endregion

            Keep_only_lines_that_contain_at_least_one_non_zero_value();
        }
        #endregion

        #region Keep
        public void Keep_only_input_rowNames(string[] input_rowNames)
        {
            List<Data_line_class> keep_data = new List<Data_line_class>();
            List<Data_line_class> remove_keep_data = new List<Data_line_class>();
            input_rowNames = input_rowNames.Distinct().OrderBy(l => l).ToArray();
            string input_rowName;
            int indexInput = 0;
            int input_rowNames_length = input_rowNames.Length;
            this.Order_by_ncbiOfficialSymbol();
            int data_length = Data_length;
            int stringCompare = -2;
            Data_line_class data_line;
            bool kept = false;
            for (int indexData = 0; indexData < data_length; indexData++)
            {
                data_line = Data[indexData];
                kept = false;
                stringCompare = -2;
                while ((indexInput < input_rowNames_length) && (stringCompare < 0))
                {
                    input_rowName = input_rowNames[indexInput];
                    stringCompare = input_rowName.CompareTo(data_line.NCBI_official_symbol);
                    if (stringCompare < 0)
                    {
                        indexInput++;
                    }
                    else if (stringCompare == 0)
                    {
                        keep_data.Add(data_line);
                        kept = true;
                    }
                }
                if (!kept)
                {
                    remove_keep_data.Add(data_line);
                }
            }
            Data = keep_data.ToArray();
        }

        public void Remove_empty_rows_and_columns()
        {
            int column_length = this.ColChar.Columns_length;
            List<int> keepColumns = new List<int>();
            bool keep_row;
            List<Data_line_class> newDE = new List<Data_line_class>();
            foreach (Data_line_class line in Data)
            {
                keep_row = false;
                for (int indexCol = 0; indexCol < column_length; indexCol++)
                {
                    if (line.Columns[indexCol] != 0)
                    {
                        keepColumns.Add(indexCol);
                        keep_row = true;
                    }
                }
                if (keep_row)
                {
                    newDE.Add(line);
                }
            }

            if (keepColumns.Count() < column_length)
            {
                ColChar.Keep_only_input_columns(keepColumns.ToArray());
                foreach (Data_line_class line in Data)
                {
                    line.Keep_columns(keepColumns.ToArray());
                }
                ColChar.Column_rearrangements_adopted = true;
            }
            Data = newDE.ToArray();
        }
        public void Keep_only_input_columns_and_remove_all_rows_that_are_left_over_with_only_zero_values(params int[] inputColumns)
        {
            inputColumns = inputColumns.Distinct().OrderBy(l => l).ToArray();
            ColChar.Keep_only_input_columns(inputColumns);
            inputColumns = inputColumns.Distinct().OrderBy(l => l).ToArray();
            int inputColumns_length = inputColumns.Length;
            int data_length = Data.Length;
            Data_line_class data_line;
            double[] new_columns;
            for (int indexData = 0; indexData < data_length; indexData++)
            {
                data_line = Data[indexData];
                new_columns = new double[inputColumns_length];
                for (int indexI = 0; indexI < inputColumns_length; indexI++)
                {
                    new_columns[indexI] = data_line.Columns[inputColumns[indexI]];
                }
                data_line.Columns = new_columns;
            }
            Remove_empty_rows_and_columns();
            ColChar.Column_rearrangements_adopted = true;
        }
        #endregion

        #region Set to upper case
        public void Set_all_ncbi_official_gene_symbols_to_upper_case()
        {
            foreach (Data_line_class data_line in Data)
            {
                data_line.NCBI_official_symbol = data_line.NCBI_official_symbol.ToUpper();
            }
        }
        #endregion

        #region Keep only above or below cutoff
        private void Keep_only_lines_that_contain_at_least_one_non_zero_value()
        {
            int data_length = this.Data_length;
            int column_length = this.ColChar.Columns_length;
            Data_line_class data_line;
            List<Data_line_class> kept_data_list = new List<Data_line_class>();
            bool keep_line;
            for (int indexData = 0; indexData < data_length; indexData++)
            {
                data_line = this.Data[indexData];
                keep_line = false;
                for (int indexCol = 0; indexCol < column_length; indexCol++)
                {
                    if (data_line.Columns[indexCol] != 0) { keep_line = true; }
                }
                if (keep_line) { kept_data_list.Add(data_line); }
            }
            Data = kept_data_list.ToArray();
        }

        private void Keep_only_columns_that_contain_at_least_one_non_zero_entry()
        {
            int data_length = this.Data_length;
            int column_length = this.ColChar.Columns_length;
            Data_line_class data_line;
            bool[] keep_colunns = new bool[column_length];
            for (int indexK = 0; indexK < column_length; indexK++) { keep_colunns[indexK] = false; }
            for (int indexData = 0; indexData < data_length; indexData++)
            {
                data_line = this.Data[indexData];
                for (int indexCol = 0; indexCol < column_length; indexCol++)
                {
                    if (data_line.Columns[indexCol] != 0) { keep_colunns[indexCol] = true; }
                }
            }

            List<int> keep_column_indexes = new List<int>();
            for (int indexCol = 0; indexCol < column_length; indexCol++)
            {
                keep_column_indexes.Add(indexCol);
            }

            Keep_only_input_columns_and_remove_all_rows_that_are_left_over_with_only_zero_values(keep_column_indexes.ToArray());
        }

        public void Keep_only_values_equal_to_or_greater_than_cutoff(float cutoff)
        {
            int data_length = this.Data_length;
            int column_length = this.ColChar.Columns_length;
            Data_line_class data_line;

            #region Set all values 0 that below cutoff
            for (int indexData = 0; indexData < data_length; indexData++)
            {
                data_line = this.Data[indexData];
                for (int indexCol = 0; indexCol < column_length; indexCol++)
                {
                    if (data_line.Columns[indexCol] < cutoff) { data_line.Columns[indexCol] = 0; }
                }
            }
            #endregion

            Keep_only_lines_that_contain_at_least_one_non_zero_value();
            Keep_only_columns_that_contain_at_least_one_non_zero_entry();
        }
        #endregion
 
        #region Write read copy
        public Data_class Deep_copy()
        {
            Data_class copy = (Data_class)this.MemberwiseClone();
            int data_length = Data.Length;
            copy.Data = new Data_line_class[data_length];
            copy.Column_entries_count_line = this.Column_entries_count_line.Deep_copy();
            for (int indexD = 0; indexD < data_length; indexD++)
            {
                copy.Data[indexD] = this.Data[indexD].Deep_copy();
            }
            copy.ColChar = this.ColChar.Deep_copy();
            return copy;
        }

        private void Write_simple_headline(StreamWriter writer, Data_private_readWriteOptions_class readWriteOptions, out int headline_columns_length)
        {
            int columns_length = ColChar.Columns.Length;
            char lineDelimiter = readWriteOptions.LineDelimiter;

            writer.Write("NCBI_symbol");
            for (int indexC = 0; indexC < columns_length; indexC++)
            {
                writer.Write("{0}{1}", readWriteOptions.LineDelimiter, ColChar.Columns[indexC].Full_column_name);
            }
            //writer.WriteLine("{0}NCBI_description{0}NCBI_geneID{0}SCPs", lineDelimiter);
            writer.WriteLine();
            headline_columns_length = columns_length + 1;
        }

        public void Set_all_rowNames_to_upperCase()
        {
            foreach (Data_line_class data_line in this.Data)
            {
                data_line.NCBI_official_symbol = (string)data_line.NCBI_official_symbol.ToUpper();
            }
        }

        public void Write_file_for_cluster_in_results_directory(string subdirectory, string file_name)
        {
            Global_directory_and_file_class global_directory = new Global_directory_and_file_class();
            StreamWriter writer = new StreamWriter(global_directory.Results_directory + subdirectory + file_name);

            //Generate and write Headline
            char headline_delimiter = Global_class.Tab; ;
            StringBuilder headline = new StringBuilder();
            headline.AppendFormat("Symbol");
            int col_count = ColChar.Columns.Length;
            for (int indexCol = 0; indexCol < col_count; indexCol++)
            {
                headline.AppendFormat("{0}{1}", headline_delimiter, ColChar.Columns[indexCol].Full_column_name);
            }
            writer.WriteLine(headline);

            //Generate and write lines
            char line_delimiter = Global_class.Tab;
            StringBuilder line = new StringBuilder();
            int DE_count = Data.Length;
            for (int deIndex = 0; deIndex < DE_count; deIndex++)
            {
                line.Clear();
                line.AppendFormat("{0}", Data[deIndex].NCBI_official_symbol);
                for (int colIndex = 0; colIndex < Data[deIndex].Columns.Length; colIndex++)
                {
                    string output = Data[deIndex].Columns[colIndex].ToString();
                    line.AppendFormat("{0}{1}", line_delimiter, output);
                }
                writer.WriteLine(line);
            }
            writer.Close();
        }

        private void Write_data(StreamWriter writer, Data_private_readWriteOptions_class readWriteOptions, int headlines_columns_length)
        {
            int columns_length = ColChar.Columns.Length;
            char lineDelimiter = readWriteOptions.LineDelimiter;

            Data_line_class data_line;
            int data_length = Data.Length;
            for (int indexD = 0; indexD < data_length; indexD++)
            {
                data_line = Data[indexD];
                writer.Write("{0}", data_line.NCBI_official_symbol);
                if (columns_length!=headlines_columns_length-1) { throw new Exception(); }
                for (int indexC = 0; indexC < columns_length; indexC++)
                {
                    writer.Write("{0}{1}", lineDelimiter, data_line.Columns[indexC]);
                }
                //writer.Write("{0}{1}{0}{2}", lineDelimiter, data_line.NCBI_description, data_line.NCBI_geneID);
                //scps_length = data_line.Scps.Length;
                //writer.Write(lineDelimiter);
                //for (int indexScp=0; indexScp<scps_length;indexScp++)
                //{
                //    scp = data_line.Scps[indexScp];
                //    if (indexScp>0) { writer.Write(scpDelimiter); }
                //    writer.Write("{0}", scp);
                //}
                writer.WriteLine();
            }
        }

        public void Write_with_simple_headline(string subdirectory, string file_name)
        {
            Data_private_readWriteOptions_class readWriteOptions = new Data_private_readWriteOptions_class(subdirectory, file_name);
            StreamWriter writer = new StreamWriter(readWriteOptions.Complete_fileName);
            int headline_columns_length;
            Write_simple_headline(writer, readWriteOptions, out headline_columns_length);
            Write_data(writer, readWriteOptions, headline_columns_length);
            writer.Close();
        }
        #endregion
    }
}
