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
using Common_functions.Text;
using Common_functions.Form_tools;
using Common_functions.ReadWrite;
using Windows_forms;
using System.Reflection.Emit;
using System.Windows.Markup;
using Other_ontologies_and_databases;
using Enrichment;
using System.CodeDom;
using MBCO;

namespace Network
{
    enum NWedge_type_enum { E_m_p_t_y, Arrow, Thick_dotted_line, Dotted_line, Dashed_line, Solid_line };

    class NetworkTable_line_class
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public string Edge_label { get; set; }
        public float Width { get; set; }
        public NWedge_type_enum Edge_type { get; set; }

        public NetworkTable_line_class()
        {
            Width = Global_class.Edge_width_default;
            Edge_label = "";
            Edge_type = NWedge_type_enum.Arrow;
        }

        public NetworkTable_line_class Deep_copy()
        {
            NetworkTable_line_class copy = (NetworkTable_line_class)this.MemberwiseClone();
            copy.Source = (string)this.Source.Clone();
            copy.Target = (string)this.Target.Clone();
            copy.Edge_label = (string)this.Edge_label.Clone();
            return copy;
        }
    }

    class Obo_networkTable_line_class
    {
        #region Fields
        public int Child_level { get; set; }
        public int Child_depth { get; set; }
        public string Child_id { get; set; }
        public string Child_name { get; set; }
        public int Child_size_based_on_human_genes { get; set; }
        public string Parent_id { get; set; }
        public string Parent_name { get; set; }
        public Obo_interaction_type_enum Interaction_type { get; set; }
        public Namespace_type_enum Child_namespace_type { get; set; }
        #endregion

        public Obo_networkTable_line_class()
        {
            this.Child_id = "";
            this.Child_name = "";
            this.Child_level = -1;
            this.Child_depth = -1;
            this.Child_size_based_on_human_genes = -1;
            this.Parent_id = (string)Global_class.Empty_entry.Clone();
            this.Parent_name = (string)Global_class.Empty_entry.Clone();
        }
        public Obo_networkTable_line_class(string child_id, string child_name, int child_level) : this()
        {
            this.Child_id = (string)child_id.Clone();
            this.Child_name = (string)child_name.Clone();
            this.Child_level = child_level;
        }

        #region Standard way
        public static Obo_networkTable_line_class[] Order_in_standard_way(Obo_networkTable_line_class[] networkTable_lines)
        {
            //networkTable = networkTable.OrderBy(l => l.Child_id).ThenBy(l => l.Child_name).ThenBy(l => l.Parent_id).ThenBy(l => l.Parent_name).ToArray();
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, List<Obo_networkTable_line_class>>>>> childId_childName_parentId_parentName_dict = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, List<Obo_networkTable_line_class>>>>>();
            Dictionary<string, Dictionary<string, Dictionary<string, List<Obo_networkTable_line_class>>>> childName_parentId_parentName_dict = new Dictionary<string, Dictionary<string, Dictionary<string, List<Obo_networkTable_line_class>>>>();
            Dictionary<string, Dictionary<string, List<Obo_networkTable_line_class>>> parentId_parentName_dict = new Dictionary<string, Dictionary<string, List<Obo_networkTable_line_class>>>();
            Dictionary<string, List<Obo_networkTable_line_class>> parentName_dict = new Dictionary<string, List<Obo_networkTable_line_class>>();
            int networkTable_lines_length = networkTable_lines.Length;
            Obo_networkTable_line_class networkTable_line;
            for (int indexNT=0; indexNT<networkTable_lines_length; indexNT++)
            {
                networkTable_line = networkTable_lines[indexNT];
                if (!childId_childName_parentId_parentName_dict.ContainsKey(networkTable_line.Child_id))
                {
                    childId_childName_parentId_parentName_dict.Add(networkTable_line.Child_id, new Dictionary<string, Dictionary<string, Dictionary<string, List<Obo_networkTable_line_class>>>>());
                }
                if (!childId_childName_parentId_parentName_dict[networkTable_line.Child_id].ContainsKey(networkTable_line.Child_name))
                {
                    childId_childName_parentId_parentName_dict[networkTable_line.Child_id].Add(networkTable_line.Child_name, new Dictionary<string, Dictionary<string, List<Obo_networkTable_line_class>>>());
                }
                if (!childId_childName_parentId_parentName_dict[networkTable_line.Child_id][networkTable_line.Child_name].ContainsKey(networkTable_line.Parent_id))
                {
                    childId_childName_parentId_parentName_dict[networkTable_line.Child_id][networkTable_line.Child_name].Add(networkTable_line.Parent_id, new Dictionary<string, List<Obo_networkTable_line_class>>());
                }
                if (!childId_childName_parentId_parentName_dict[networkTable_line.Child_id][networkTable_line.Child_name][networkTable_line.Parent_id].ContainsKey(networkTable_line.Parent_name))
                {
                    childId_childName_parentId_parentName_dict[networkTable_line.Child_id][networkTable_line.Child_name][networkTable_line.Parent_id].Add(networkTable_line.Parent_name, new List<Obo_networkTable_line_class>());
                }
                childId_childName_parentId_parentName_dict[networkTable_line.Child_id][networkTable_line.Child_name][networkTable_line.Parent_id][networkTable_line.Parent_name].Add(networkTable_line);
            }
            List<Obo_networkTable_line_class> ordered_lines = new List<Obo_networkTable_line_class>();
            networkTable_lines = null;
            string[] childIds;
            string childId;
            int childIds_length;
            string[] childNames;
            string childName;
            int childNames_length;
            string[] parentIds;
            string parentId;
            int parentIds_length;
            string[] parentNames;
            string parentName;
            int parentNames_length;
            childIds = childId_childName_parentId_parentName_dict.Keys.ToArray();
            childIds = childIds.OrderBy(l => l).ToArray();
            childIds_length = childIds.Length;
            for (int indexChildId=0; indexChildId<childIds_length; indexChildId++)
            {
                childId = childIds[indexChildId];
                childName_parentId_parentName_dict = childId_childName_parentId_parentName_dict[childId];
                childNames = childName_parentId_parentName_dict.Keys.ToArray();
                childNames = childNames.OrderBy(l => l).ToArray();
                childNames_length = childNames.Length;
                for (int indexChildName=0; indexChildName<childNames_length;indexChildName++)
                {
                    childName = childNames[indexChildName];
                    parentId_parentName_dict = childName_parentId_parentName_dict[childName];
                    parentIds = parentId_parentName_dict.Keys.ToArray();
                    parentIds = parentIds.OrderBy(l => l).ToArray();
                    parentIds_length = parentIds.Length;
                    for (int indexParentId = 0; indexParentId < parentIds_length; indexParentId++)
                    {
                        parentId = parentIds[indexParentId];
                        parentName_dict = parentId_parentName_dict[parentId];
                        parentNames = parentName_dict.Keys.ToArray();
                        parentNames = parentNames.OrderBy(l => l).ToArray();
                        parentNames_length = parentNames.Length;
                        for (int indexParentName=0; indexParentName<parentNames_length; indexParentName++)
                        {
                            parentName = parentNames[indexParentName];
                            ordered_lines.AddRange(parentName_dict[parentName]);
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != networkTable_lines_length) { throw new Exception(); }
                Obo_networkTable_line_class this_line;
                Obo_networkTable_line_class previous_line;
                //networkTable = networkTable.OrderBy(l => l.Child_id).ThenBy(l => l.Child_name).ThenBy(l => l.Parent_id).ThenBy(l => l.Parent_name).ToArray();
                for (int indexO=1; indexO<networkTable_lines_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.Child_id.CompareTo(previous_line.Child_id)<0) { throw new Exception(); }
                    if ((this_line.Child_id.Equals(previous_line.Child_id))
                        && (this_line.Child_name.CompareTo(previous_line.Child_name) < 0)) { throw new Exception(); }
                    if ((this_line.Child_id.Equals(previous_line.Child_id))
                        && (this_line.Child_name.Equals(previous_line.Child_name))
                        && (this_line.Parent_id.CompareTo(previous_line.Parent_id) < 0)) { throw new Exception(); }
                    if ((this_line.Child_id.Equals(previous_line.Child_id))
                        && (this_line.Child_name.Equals(previous_line.Child_name))
                        && (this_line.Parent_id.Equals(previous_line.Parent_id))
                        && (this_line.Parent_name.CompareTo(previous_line.Parent_name) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray(); ;
        }

        public static Obo_networkTable_line_class[] Order_by_childLevel_childName(Obo_networkTable_line_class[] networkTable_lines)
        {
            //mbco_networkTable.NetworkTable.OrderBy(l => l.Child_level).ThenBy(l => l.Child_name).ToArray();
            Dictionary<int, Dictionary<string, List<Obo_networkTable_line_class>>> childLevel_childName_dict = new Dictionary<int, Dictionary<string, List<Obo_networkTable_line_class>>>();
            Dictionary<string, List<Obo_networkTable_line_class>> childName_dict = new Dictionary<string, List<Obo_networkTable_line_class>>();
            int networkTable_lines_length = networkTable_lines.Length;
            Obo_networkTable_line_class networkTable_line;
            for (int indexNT = 0; indexNT < networkTable_lines_length; indexNT++)
            {
                networkTable_line = networkTable_lines[indexNT];
                if (!childLevel_childName_dict.ContainsKey(networkTable_line.Child_level))
                {
                    childLevel_childName_dict.Add(networkTable_line.Child_level, new Dictionary<string, List<Obo_networkTable_line_class>>());
                }
                if (!childLevel_childName_dict[networkTable_line.Child_level].ContainsKey(networkTable_line.Child_name))
                {
                    childLevel_childName_dict[networkTable_line.Child_level].Add(networkTable_line.Child_name, new List<Obo_networkTable_line_class>());
                }
                childLevel_childName_dict[networkTable_line.Child_level][networkTable_line.Child_name].Add(networkTable_line);
            }
            List<Obo_networkTable_line_class> ordered_lines = new List<Obo_networkTable_line_class>();
            networkTable_lines = null;
            int[] childLevels;
            int childLevel;
            int childLevels_length;
            string[] childNames;
            string childName;
            int childNames_length;
            childLevels = childLevel_childName_dict.Keys.ToArray();
            childLevels = childLevels.OrderBy(l => l).ToArray();
            childLevels_length = childLevels.Length;
            for (int indexChildLevel = 0; indexChildLevel < childLevels_length; indexChildLevel++)
            {
                childLevel = childLevels[indexChildLevel];
                childName_dict = childLevel_childName_dict[childLevel];
                childNames = childName_dict.Keys.ToArray();
                childNames = childNames.OrderBy(l => l).ToArray();
                childNames_length = childNames.Length;
                for (int indexChildName = 0; indexChildName < childNames_length; indexChildName++)
                {
                    childName = childNames[indexChildName];
                    ordered_lines.AddRange(childName_dict[childName]);
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != networkTable_lines_length) { throw new Exception(); }
                Obo_networkTable_line_class this_line;
                Obo_networkTable_line_class previous_line;
                //childLevel_childName
                for (int indexO = 1; indexO < networkTable_lines_length; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.Child_level.CompareTo(previous_line.Child_level) < 0) { throw new Exception(); }
                    if ((this_line.Child_level.Equals(previous_line.Child_level))
                        && (this_line.Child_name.CompareTo(previous_line.Child_name) < 0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray(); ;
        }


        public bool Equal_in_standard_way(Obo_networkTable_line_class other)
        {
            bool equal = ((this.Child_id.Equals(other.Child_id))
                          && (this.Parent_id.Equals(other.Parent_id)));
            return equal;
        }
        #endregion

        public Obo_networkTable_line_class Deep_copy()
        {
            Obo_networkTable_line_class copy = (Obo_networkTable_line_class)this.MemberwiseClone();
            copy.Child_id = (string)this.Child_id.Clone();
            copy.Child_name = (string)this.Child_name.Clone();
            copy.Parent_id = (string)this.Parent_id.Clone();
            copy.Parent_name = (string)this.Parent_name.Clone();
            return copy;
        }
    }

    class Obo_networkTable_oboFile_readOptions_class
    {
        #region Fields
        public const string End_reading_marker = "[Typedef]";
        public const string Id_marker = "id";
        public const string Name_marker = "name";
        public const string Is_a_marker = "is_a";
        public const string Part_of_marker = "part_of";
        public const string Regulates_label = "regulates";
        public const string Positivley_regulates_label = "positively_regulates";
        public const string Negatively_regulates_label = "negatively_regulates";
        public const string Namespace_marker = "namespace";
        public const char Parent_id_parent_name_separator = '!';
        public const string New_term_marker = "[Term]";
        public const string Is_obsolete_marker = "is_obsolete";
        public const string Level_marker = "level";

        public string Complete_file_name { get; set; }
        #endregion

        public Obo_networkTable_oboFile_readOptions_class(Ontology_type_enum ontology)
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            switch (ontology)
            {
                case Ontology_type_enum.Mbco:
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_mf:
                case Ontology_type_enum.Go_cc:
                    Complete_file_name = global_dirFile.Ontology_inputDirectory_dict[ontology] + global_dirFile.Ontology_hierarchyInputFileName_dict[ontology];
                    break;
                default:
                    throw new Exception();
            }
        }
    }

    class Hierachy_networkTable_specialMbcoInput_readOptions_class : ReadWriteOptions_base
    {
        public Hierachy_networkTable_specialMbcoInput_readOptions_class(Ontology_type_enum ontology)
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            this.File = global_dirFile.Ontology_inputDirectory_dict[ontology] + global_dirFile.Ontology_hierarchyInputFileName_dict[ontology];
            this.Key_propertyNames = new string[] { "Child_id", "Child_name", "Parent_id", "Parent_name" };
            this.Key_columnNames = new string[] { "Child_id", "Child_scp", "Parent_id", "Parent_scp" };
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.File_has_headline = true;
            this.Report = ReadWrite_report_enum.Report_nothing;
        }
    }

    class Hierachy_networkTable_standardInput_readOptions_class : ReadWriteOptions_base
    {
        public Hierachy_networkTable_standardInput_readOptions_class(Ontology_type_enum ontology)
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            this.File = global_dirFile.Ontology_inputDirectory_dict[ontology] +  global_dirFile.Ontology_hierarchyInputFileName_dict[ontology];
            this.Key_propertyNames = new string[] { "Child_name", "Parent_name" };
            this.Key_columnNames = new string[] { "Child_scp", "Parent_scp" };
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.File_has_headline = true;
            this.Report = ReadWrite_report_enum.Report_nothing;
        }
    }

    class Hierachy_networkTable_reactome_readOptions_class : ReadWriteOptions_base
    {
        public Hierachy_networkTable_reactome_readOptions_class()
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            this.File = global_dirFile.Ontology_inputDirectory_dict[Ontology_type_enum.Reactome] + global_dirFile.Ontology_hierarchyInputFileName_dict[Ontology_type_enum.Reactome];
            this.Key_propertyNames = new string[] { "Parent_id", "Child_id" };
            this.Key_columnIndexes = new int[] { 0, 1 };
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.File_has_headline = false;
            this.Report = ReadWrite_report_enum.Report_nothing;
        }
    }

    class Hierarchy_networkTable_allInfo_readOptions_class : ReadWriteOptions_base
    {
        public Hierarchy_networkTable_allInfo_readOptions_class(Ontology_type_enum ontology, Organism_enum organism, SCP_hierarchy_interaction_type_enum interaction_type)
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            this.File = global_dirFile.Get_appGenerated_complete_ontology_parentChild_allInfo_fileName(ontology, organism, interaction_type);
            this.Key_propertyNames = new string[] { "Child_level","Child_depth","Child_id", "Child_name", "Parent_id", "Parent_name", "Interaction_type", "Child_namespace_type", "Child_size_based_on_human_genes" };
            this.Key_columnNames = new string[] { "Child_level","Child_depth","Child_id", "Child_scp", "Parent_id", "Parent_scp", "Interaction_type", "Namespace_type", "Child_size_based_on_human_genes" };
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.File_has_headline = true;
            this.Report = ReadWrite_report_enum.Report_nothing;
        }
    }

    class Obo_networkTable_class
    {
        public Obo_networkTable_line_class[] NetworkTable { get; set; }

        public Obo_networkTable_class()
        {
            NetworkTable = new Obo_networkTable_line_class[0];
        }

        #region Order
        public void Order_by_childLevel_childName()
        {
            this.NetworkTable = Obo_networkTable_line_class.Order_by_childLevel_childName(this.NetworkTable);
        }
        public void Order_in_standard_way()
        {
            this.NetworkTable = Obo_networkTable_line_class.Order_in_standard_way(this.NetworkTable);
        }
        #endregion
        private void Add_to_array(Obo_networkTable_line_class[] add_networkTables)
        {
            int add_length = add_networkTables.Length;
            int this_length = this.NetworkTable.Length;
            int new_length = this_length + add_length;
            Obo_networkTable_line_class[] new_networkTables = new Obo_networkTable_line_class[new_length];
            int indexNew = -1;
            for (int indexThis=0; indexThis<this_length;indexThis++)
            {
                indexNew++;
                new_networkTables[indexNew] = this.NetworkTable[indexThis];
            }
            for (int indexAdd=0; indexAdd<add_length;indexAdd++)
            {
                indexNew++;
                new_networkTables[indexNew] = add_networkTables[indexAdd];
            }
            this.NetworkTable = new_networkTables;
        }

        public bool Are_all_sizes_set()
        {
            foreach (Obo_networkTable_line_class networkTable_line in this.NetworkTable)
            {
                if (networkTable_line.Child_size_based_on_human_genes < 0) { return false; }
            }
            return true;
        }

        private void Remove_duplicates()
        {
            NetworkTable = Obo_networkTable_line_class.Order_in_standard_way(NetworkTable);
            int sigNW_length = NetworkTable.Length;
            Obo_networkTable_line_class obo_networkTable_line;
            List<Obo_networkTable_line_class> new_sigNW = new List<Obo_networkTable_line_class>();
            for (int indexSigNW = 0; indexSigNW < sigNW_length; indexSigNW++)
            {
                obo_networkTable_line = NetworkTable[indexSigNW];
                if ((indexSigNW == 0)
                    || (!obo_networkTable_line.Equal_in_standard_way(NetworkTable[indexSigNW - 1])))
                {
                    new_sigNW.Add(obo_networkTable_line);
                }
            }
            this.NetworkTable = new_sigNW.ToArray();
        }

        private void Add_to_networkTables(Obo_networkTable_line_class[] add_networkTable)
        {
            int add_length = add_networkTable.Length;
            int this_length = this.NetworkTable.Length;
            int new_length = add_length + this_length;
            Obo_networkTable_line_class[] new_networkTable = new Obo_networkTable_line_class[new_length];
            int indexNew = -1;
            for (int indexThis = 0; indexThis < this_length; indexThis++)
            {
                indexNew++;
                new_networkTable[indexNew] = this.NetworkTable[indexThis].Deep_copy();
            }
            for (int indexAdd = 0; indexAdd < add_length; indexAdd++)
            {
                indexNew++;
                new_networkTable[indexNew] = add_networkTable[indexAdd].Deep_copy();
            }
            this.NetworkTable = new_networkTable;
        }

        public void Add_potentially_missing_scps_as_children_of_no_annotated_parent_if_not_background_genes(string[] scps)
        {
            Dictionary<string, bool> existingScp_dict = new Dictionary<string, bool>();
            foreach (Obo_networkTable_line_class networkTable_line in this.NetworkTable)
            {
                if (!existingScp_dict.ContainsKey(networkTable_line.Parent_name))
                { existingScp_dict.Add(networkTable_line.Parent_name, true); }
                if (!existingScp_dict.ContainsKey(networkTable_line.Child_name))
                { existingScp_dict.Add(networkTable_line.Child_name, true); }
            }
            string notAnnotatedParentSCPName = Ontology_classification_class.No_annotated_parent_scp;
            existingScp_dict.Add(Ontology_classification_class.Background_genes_scp,true);
            Obo_networkTable_line_class add_nwTable_line;
            List<Obo_networkTable_line_class> add_nwTable_lines = new List<Obo_networkTable_line_class>();
            foreach (string scp in scps)
            {
                if (!existingScp_dict.ContainsKey(scp))
                {
                    existingScp_dict.Add(scp, true);
                    add_nwTable_line = new Obo_networkTable_line_class();
                    add_nwTable_line.Parent_name = (string)notAnnotatedParentSCPName.Clone();
                    add_nwTable_line.Child_name = (string)scp.Clone();
                    add_nwTable_lines.Add(add_nwTable_line);
                }
            }
            Add_to_array(add_nwTable_lines.ToArray());
        }

        private bool Read_obo_file_update_progressReport_and_return_if_file_exists(Ontology_type_enum ontology, SCP_hierarchy_interaction_type_enum scp_hierarchial_interactions, ProgressReport_interface_class progressReport)
        {
            Obo_networkTable_oboFile_readOptions_class readOptions = new Obo_networkTable_oboFile_readOptions_class(ontology);
            bool file_exits = false;
            if (System.IO.File.Exists(readOptions.Complete_file_name))
            {
                StreamReader reader = Common_functions.ReadWrite.ReadWriteClass.Get_new_stream_reader_and_sent_notice_if_file_in_use(readOptions.Complete_file_name, progressReport, out bool file_opened);
                if (file_opened)
                {
                    file_exits = true;
                    string input_line;
                    bool new_term = false;
                    bool inside_term = false;
                    string id = "error";
                    string name = "error";
                    string parent_id = "error";
                    string parent_name = "error";
                    Namespace_type_enum namespace_type = Namespace_type_enum.E_m_p_t_y;

                    string[] splitStrings;
                    string[] parent_id_parent_name;
                    Obo_interaction_type_enum interaction_type;
                    StringBuilder sb;
                    int splitString_length;
                    bool continue_reading = true;
                    bool new_parent = false;
                    bool at_least_one_line_added = false;
                    bool end_of_term = false;
                    bool is_obsolete = false;
                    int level = -1;
                    List<Obo_networkTable_line_class> networkTable_list = new List<Obo_networkTable_line_class>();
                    Obo_networkTable_line_class new_networkTable_line;
                    Dictionary<string, bool> added_scps_dict = new Dictionary<string, bool>();
                    string[] second_splitStrings;
                    string second_splitString;
                    int second_splitStrings_length;
                    StringBuilder sb_splitStringTwo = new StringBuilder();
                    int parent_childRelationShips_count = 0;
                    int part_of_relationships_count = 0;
                    int is_a_relationships_count = 0;
                    int regulates_relationships_count = 0;
                    int positively_regulates_relationships_count = 0;
                    int negatively_regulates_relationships_count = 0;
                    bool part_of_in_input_line_recognized_by_alogorithm = false;
                    bool is_a_in_input_line_recognized_by_alogorithm = false;
                    List<string> missed_part_ofs_list = new List<string>();
                    List<string> missed_is_list = new List<string>();
                    List<Obo_networkTable_line_class> lines_without_parents = new List<Obo_networkTable_line_class>();
                    while (((input_line = reader.ReadLine()) != null) && (continue_reading))
                    {
                        is_a_in_input_line_recognized_by_alogorithm = input_line.IndexOf(Obo_networkTable_oboFile_readOptions_class.Is_a_marker) != -1;
                        part_of_in_input_line_recognized_by_alogorithm = input_line.IndexOf(Obo_networkTable_oboFile_readOptions_class.Part_of_marker) != -1;

                        #region Get split stings
                        splitStrings = input_line.Split(':');
                        splitString_length = splitStrings.Length;
                        if ((splitString_length > 2) && (!splitStrings[0].Equals("synonym")) && (!splitStrings[0].Equals("xref")) && (!splitStrings[0].Equals("def")))
                        {
                            sb = new StringBuilder();
                            for (int indexS = 1; indexS < splitString_length; indexS++)
                            {
                                splitStrings[indexS] = splitStrings[indexS].Replace("{", "").Replace("}", "");
                                if (indexS > 1) { sb.AppendFormat(":"); }
                                sb.AppendFormat(splitStrings[indexS]);
                            }
                            splitStrings = new string[] { splitStrings[0], sb.ToString() };
                        }
                        if (splitStrings[0].Equals("relationship"))
                        {
                            second_splitStrings = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(splitStrings[1]).Split(' ');
                            splitStrings[0] = second_splitStrings[0];
                            sb_splitStringTwo.Clear();
                            second_splitStrings_length = second_splitStrings.Length;
                            for (int indexSecondSplit = 1; indexSecondSplit < second_splitStrings_length; indexSecondSplit++)
                            {
                                second_splitString = second_splitStrings[indexSecondSplit];
                                if (indexSecondSplit > 1) { sb_splitStringTwo.AppendFormat(" "); }
                                sb_splitStringTwo.AppendFormat(second_splitString);
                            }
                            splitStrings[1] = sb_splitStringTwo.ToString();
                        }
                        #endregion

                        #region Set every turn default values
                        parent_id = Global_class.Empty_entry;
                        parent_name = Global_class.Empty_entry;
                        interaction_type = Obo_interaction_type_enum.E_m_p_t_y;
                        new_parent = false;
                        #endregion

                        if ((String.IsNullOrEmpty(splitStrings[0])) && (inside_term))
                        {
                            end_of_term = true;
                            inside_term = false;
                        }
                        else
                        {
                            switch (splitStrings[0])
                            {
                                case Obo_networkTable_oboFile_readOptions_class.New_term_marker:
                                    new_term = true;
                                    is_obsolete = false;
                                    break;
                                case Obo_networkTable_oboFile_readOptions_class.Id_marker:
                                    id = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(splitStrings[1]);
                                    break;
                                case Obo_networkTable_oboFile_readOptions_class.Namespace_marker:
                                    namespace_type = (Namespace_type_enum)Enum.Parse(typeof(Namespace_type_enum), Text_class.Set_first_character_to_upperCase_and_rest_toLowerCase(Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(splitStrings[1])));
                                    break;
                                case Obo_networkTable_oboFile_readOptions_class.Name_marker:
                                    name = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(splitStrings[1]);
                                    break;
                                case Obo_networkTable_oboFile_readOptions_class.Is_a_marker:
                                    parent_id_parent_name = splitStrings[1].Split(Obo_networkTable_oboFile_readOptions_class.Parent_id_parent_name_separator);
                                    parent_id = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(parent_id_parent_name[0]);
                                    if (parent_id_parent_name.Length == 2) { parent_name = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(parent_id_parent_name[1]); }
                                    interaction_type = Obo_interaction_type_enum.Is_a;
                                    new_parent = true;
                                    break;
                                case Obo_networkTable_oboFile_readOptions_class.Part_of_marker:
                                    parent_id_parent_name = splitStrings[1].Split(Obo_networkTable_oboFile_readOptions_class.Parent_id_parent_name_separator);
                                    parent_id = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(parent_id_parent_name[0]);
                                    parent_name = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(parent_id_parent_name[1]);
                                    interaction_type = Obo_interaction_type_enum.Part_of;
                                    new_parent = true;
                                    break;
                                case Obo_networkTable_oboFile_readOptions_class.Regulates_label:
                                case Obo_networkTable_oboFile_readOptions_class.Positivley_regulates_label:
                                case Obo_networkTable_oboFile_readOptions_class.Negatively_regulates_label:
                                    switch (scp_hierarchial_interactions)
                                    {
                                        case SCP_hierarchy_interaction_type_enum.Parent_child:
                                            break;
                                        case SCP_hierarchy_interaction_type_enum.Parent_child_regulatory:
                                            parent_id_parent_name = splitStrings[1].Split(Obo_networkTable_oboFile_readOptions_class.Parent_id_parent_name_separator);
                                            parent_id = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(parent_id_parent_name[0]);
                                            parent_name = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(parent_id_parent_name[1]);
                                            switch (splitStrings[0])
                                            {
                                                case Obo_networkTable_oboFile_readOptions_class.Regulates_label:
                                                    interaction_type = Obo_interaction_type_enum.Regulates;
                                                    break;
                                                case Obo_networkTable_oboFile_readOptions_class.Positivley_regulates_label:
                                                    interaction_type = Obo_interaction_type_enum.Positively_regulates;
                                                    break;
                                                case Obo_networkTable_oboFile_readOptions_class.Negatively_regulates_label:
                                                    interaction_type = Obo_interaction_type_enum.Negatively_regulates;
                                                    break;
                                                default:
                                                    throw new Exception();
                                            }
                                            new_parent = true;
                                            break;
                                        default:
                                            throw new Exception();
                                    }
                                    break;
                                case Obo_networkTable_oboFile_readOptions_class.End_reading_marker:
                                    continue_reading = false;
                                    break;
                                case Obo_networkTable_oboFile_readOptions_class.Is_obsolete_marker:
                                    is_obsolete = true;
                                    break;
                                case Obo_networkTable_oboFile_readOptions_class.Level_marker:
                                    level = int.Parse(splitStrings[1]);
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (new_term)
                        {
                            inside_term = true;
                            new_term = false;
                            id = (string)Global_class.Empty_entry.Clone();
                            name = (string)Global_class.Empty_entry.Clone();
                            namespace_type = Namespace_type_enum.E_m_p_t_y;
                            at_least_one_line_added = false;
                            end_of_term = false;
                            level = -1;
                        }
                        else if ((new_parent) && (!is_obsolete) && (inside_term))
                        {
                            new_networkTable_line = new Obo_networkTable_line_class(id, name, level);
                            new_networkTable_line.Parent_id = (string)parent_id.Clone();
                            new_networkTable_line.Parent_name = (string)parent_name.Clone();
                            new_networkTable_line.Interaction_type = interaction_type;
                            new_networkTable_line.Child_namespace_type = namespace_type;
                            networkTable_list.Add(new_networkTable_line);
                            at_least_one_line_added = true;
                            parent_childRelationShips_count++;
                            switch (interaction_type)
                            {
                                case Obo_interaction_type_enum.Is_a:
                                    is_a_relationships_count++;
                                    is_a_in_input_line_recognized_by_alogorithm = false;
                                    break;
                                case Obo_interaction_type_enum.Part_of:
                                    part_of_relationships_count++;
                                    part_of_in_input_line_recognized_by_alogorithm = false;
                                    break;
                                case Obo_interaction_type_enum.Regulates:
                                    regulates_relationships_count++;
                                    break;
                                case Obo_interaction_type_enum.Negatively_regulates:
                                    negatively_regulates_relationships_count++;
                                    break;
                                case Obo_interaction_type_enum.Positively_regulates:
                                    positively_regulates_relationships_count++;
                                    break;
                                default:
                                    throw new Exception();
                            }
                        }
                        else if ((end_of_term) && (!at_least_one_line_added) && (!is_obsolete))
                        {
                            if (!added_scps_dict.ContainsKey(name))
                            {
                                new_networkTable_line = new Obo_networkTable_line_class(id, name, level);
                                new_networkTable_line.Child_namespace_type = namespace_type;
                                networkTable_list.Add(new_networkTable_line);
                                at_least_one_line_added = true;
                                lines_without_parents.Add(new_networkTable_line);
                            }
                            else
                            {
                                throw new Exception("Process name already exists");
                            }
                        }
                        if (is_a_in_input_line_recognized_by_alogorithm)
                        {
                            //missed_is_list.Add(input_line);
                        }
                        if (part_of_in_input_line_recognized_by_alogorithm)
                        {
                            //missed_part_ofs_list.Add(input_line);
                        }
                    }
                    switch (ontology)
                    {
                        case Ontology_type_enum.Mbco:
                            if (part_of_relationships_count == 0) { throw new Exception(); }
                            if (is_a_relationships_count != 0) { throw new Exception(); }
                            break;
                        case Ontology_type_enum.Go_bp:
                        case Ontology_type_enum.Go_cc:
                        case Ontology_type_enum.Go_mf:
                            if (part_of_relationships_count == 0) { throw new Exception(); }
                            if (is_a_relationships_count == 0) { throw new Exception(); }
                            switch (scp_hierarchial_interactions)
                            {
                                case SCP_hierarchy_interaction_type_enum.Parent_child_regulatory:
                                    if (regulates_relationships_count == 0) { throw new Exception(); }
                                    if (positively_regulates_relationships_count == 0) { throw new Exception(); }
                                    if (negatively_regulates_relationships_count == 0) { throw new Exception(); }
                                    break;
                                case SCP_hierarchy_interaction_type_enum.Parent_child:
                                    if (regulates_relationships_count != 0) { throw new Exception(); }
                                    if (positively_regulates_relationships_count != 0) { throw new Exception(); }
                                    if (negatively_regulates_relationships_count != 0) { throw new Exception(); }
                                    break;
                                default:
                                    throw new Exception();
                            }
                            break;
                        default:
                            throw new Exception();
                    }
                    reader.Close();
                    this.NetworkTable = networkTable_list.ToArray();
                }
                else
                {
                    string fileName = System.IO.Path.GetFileName(readOptions.Complete_file_name);
                    progressReport.Update_progressReport_text_and_visualization("Parent-child hierarchy cannot be generated. Is file '" + fileName + "' open in a different program?");
                }
            }
            else
            {
                string fileName = System.IO.Path.GetFileName(readOptions.Complete_file_name);
                progressReport.Update_progressReport_text_and_visualization("Parent-child hierarchy cannot be generated. File '" + fileName + "' is missing");
            }
            return file_exits;
        }

        private void Read_parentChild_networkTable_lines_specialMbcoInput(Ontology_type_enum ontology, ProgressReport_interface_class progressReport)
        {
            Hierachy_networkTable_specialMbcoInput_readOptions_class readWriteOptions = new Hierachy_networkTable_specialMbcoInput_readOptions_class(ontology);
            string shared_error_response = Ontology_classification_class.Get_pleaseDownloadMbcoPathNet_again_message();
            Obo_networkTable_line_class[] add_nwTable = ReadWriteClass.Read_data_fill_array_and_complain_including_shared_response_and_delete_if_error_message<Obo_networkTable_line_class>(readWriteOptions, progressReport, shared_error_response);
            this.NetworkTable = add_nwTable;
        }

        public bool Generate_by_reading_obo_file_and_return_if_successful(Ontology_type_enum ontology, SCP_hierarchy_interaction_type_enum scp_hierarchical_interactions, ProgressReport_interface_class progressReport)
        {
            bool file_exists = false;
            switch (ontology)
            {
                case Ontology_type_enum.Mbco:
                case Ontology_type_enum.Go_cc:
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_mf:
                    file_exists = Read_obo_file_update_progressReport_and_return_if_file_exists(ontology, scp_hierarchical_interactions, progressReport);
                    break;
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                    Read_parentChild_networkTable_lines_specialMbcoInput(ontology, progressReport);
                    break;
                default:
                    throw new Exception();
            }
            if (file_exists) { Remove_duplicates(); }
            return file_exists;
        }

        #region Add level and depth
        private void Add_level_and_depth_for_directed_acyclic_graphs_under_consideration_of_namespace(Ontology_type_enum ontology)
        {
            Dictionary<string, Namespace_type_enum> node_namespace_dict = new Dictionary<string, Namespace_type_enum>();
            Dictionary<string, string> node_nodeID_dict = new Dictionary<string, string>();
            int networkTables_length = this.NetworkTable.Length;
            Obo_networkTable_line_class networkTable_line;
            //Dictionary<string,int> pathwayName_noPathwayIds_dict = new Dictionary<string,int>();
            Dictionary<string, bool> node_isOverallParent_dict = new Dictionary<string, bool>();
            for (int indexNT = 0; indexNT < networkTables_length; indexNT++)
            {
                networkTable_line = this.NetworkTable[indexNT];

                #region Add information to node_namespace_dict
                if (!node_namespace_dict.ContainsKey(networkTable_line.Child_name))
                {
                    node_namespace_dict.Add(networkTable_line.Child_name, networkTable_line.Child_namespace_type);
                }
                else if (!node_namespace_dict[networkTable_line.Child_name].Equals(networkTable_line.Child_namespace_type))
                {
                    throw new Exception();
                }
                #endregion

                #region Add information to node_nodeID_dict
                if (!node_nodeID_dict.ContainsKey(networkTable_line.Child_name))
                {
                    node_nodeID_dict.Add(networkTable_line.Child_name, networkTable_line.Child_id);
                }
                else if (!node_nodeID_dict[networkTable_line.Child_name].Equals(networkTable_line.Child_id))
                {
                    throw new Exception();
                    //if (!pathwayName_noPathwayIds_dict.ContainsKey(networkTable_line.Child_name))
                    //{
                    //    pathwayName_noPathwayIds_dict.Add(networkTable_line.Child_name, 1);
                    //}
                    //pathwayName_noPathwayIds_dict[networkTable_line.Child_name]++;
                }
                #endregion
            }

            #region Identify overall parents and fill parentNode_childNodes_dict
            List<string> overall_parents = new List<string>();
            Dictionary<string, List<string>> parentNode_childNodes_dict = new Dictionary<string, List<string>>();
            for (int indexNT = 0; indexNT < networkTables_length; indexNT++)
            {
                networkTable_line = this.NetworkTable[indexNT];
                if (networkTable_line.Parent_name.Equals(Global_class.Empty_entry))
                {
                    overall_parents.Add(networkTable_line.Child_name);
                }
                else if (  (networkTable_line.Child_namespace_type.Equals(node_namespace_dict[networkTable_line.Parent_name]))
                         ||(node_namespace_dict[networkTable_line.Parent_name].Equals(Namespace_type_enum.Go_overall_parent)))
                {
                    if (!parentNode_childNodes_dict.ContainsKey(networkTable_line.Parent_name))
                    {
                        parentNode_childNodes_dict.Add(networkTable_line.Parent_name, new List<string>());
                    }
                    parentNode_childNodes_dict[networkTable_line.Parent_name].Add(networkTable_line.Child_name);
                }
            }
            #endregion

            overall_parents = overall_parents.Distinct().ToList();
            if (overall_parents.Count!=1) { throw new Exception(); }
            Dictionary<string, int> child_level_dict = new Dictionary<string, int>();
            Dictionary<string, int> child_depth_dict = new Dictionary<string, int>();
            Dictionary<string, string> child_overallParent_dict = new Dictionary<string, string>();

            #region Fill child_level and child_depth_dict
            int overallParents_length = overall_parents.Count;
            string overall_parent;
            int current_distance;
            string[] current_nodes;
            string current_node;
            int current_nodes_length;
            List<string> next_nodes = new List<string>();
            int max_level = 0;
            for (int indexOP = 0; indexOP < overallParents_length; indexOP++)
            {
                overall_parent = overall_parents[indexOP];
                current_distance = 0;
                child_depth_dict.Add(overall_parent, current_distance);
                child_level_dict.Add(overall_parent, current_distance);
                current_nodes = new string[] { overall_parent };
                current_nodes_length = current_nodes.Length;
                while (current_nodes_length > 0)
                {
                    next_nodes.Clear();
                    for (int indexCurrent = 0; indexCurrent < current_nodes_length; indexCurrent++)
                    {
                        current_node = current_nodes[indexCurrent];
                        if (parentNode_childNodes_dict.ContainsKey(current_node))
                        {
                            next_nodes.AddRange(parentNode_childNodes_dict[current_node]);
                        }
                    }
                    current_nodes = next_nodes.ToArray();
                    current_distance++;
                    current_nodes_length = current_nodes.Length;
                    for (int indexCurrent = 0; indexCurrent < current_nodes_length; indexCurrent++)
                    {
                        current_node = current_nodes[indexCurrent];
                        if (!child_overallParent_dict.ContainsKey(current_node))
                        {
                            child_overallParent_dict.Add(current_node, overall_parent);
                        }
                        else if (Global_class.Do_internal_checks)
                        {
                            switch (ontology)
                            {
                                case Ontology_type_enum.Go_cc:
                                case Ontology_type_enum.Go_bp:
                                case Ontology_type_enum.Go_mf:
                                case Ontology_type_enum.Custom_1:
                                case Ontology_type_enum.Custom_2:
                                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                                case Ontology_type_enum.Reactome:
                                    if (!child_overallParent_dict[current_node].Equals(overall_parent)) { throw new Exception(); }
                                    break;
                                default:
                                    throw new Exception();
                            }
                        }
                        if (max_level<current_distance) { max_level = current_distance; }
                        if (!child_level_dict.ContainsKey(current_node))
                        {
                            child_level_dict.Add(current_node, current_distance);
                        }
                        if (!child_depth_dict.ContainsKey(current_node))
                        {
                            child_depth_dict.Add(current_node, current_distance);
                        }
                        if (child_depth_dict[current_node] < current_distance)
                        {
                            child_depth_dict[current_node] = current_distance;
                        }
                        if (child_level_dict[current_node] > current_distance)
                        {
                            child_level_dict[current_node] = current_distance;
                        }
                    }
                }
            }
            #endregion

            for (int indexNT = 0; indexNT < networkTables_length; indexNT++)
            {
                networkTable_line = this.NetworkTable[indexNT];
                networkTable_line.Child_level = child_level_dict[networkTable_line.Child_name];
                networkTable_line.Child_depth = child_depth_dict[networkTable_line.Child_name];
            }
        }

        private void Fill_depth_with_level_values()
        {
            foreach (Obo_networkTable_line_class network_table_line in this.NetworkTable)
            {
                network_table_line.Child_depth = network_table_line.Child_level;
            }
        }

        public void Add_level_and_depth(Ontology_type_enum ontology)
        { 
            switch (ontology)
            {
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_cc:
                case Ontology_type_enum.Go_mf:
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                case Ontology_type_enum.Reactome:
                case Ontology_type_enum.Custom_1:
                case Ontology_type_enum.Custom_2:
                    Add_level_and_depth_for_directed_acyclic_graphs_under_consideration_of_namespace(ontology);
                    break;
                case Ontology_type_enum.Mbco:
                    Fill_depth_with_level_values();
                    break;
                default:
                    throw new Exception();
            }
        }
        #endregion

        public void Add_human_processSizes_and_write_populated_ontology_associations(MBCO_association_class ontology_associations, ProgressReport_interface_class progress_report)
        {
            Dictionary<string, int> scp_scpSize_dict = ontology_associations.Get_scp_scpSize_dictionary();
            foreach (Obo_networkTable_line_class networkTable_line in this.NetworkTable)
            {
                if (scp_scpSize_dict.ContainsKey(networkTable_line.Child_name))
                { networkTable_line.Child_size_based_on_human_genes = scp_scpSize_dict[networkTable_line.Child_name]; }
                else { networkTable_line.Child_size_based_on_human_genes = 0; }
            }
        }

        public void Add_pathwayNames_and_keep_only_lines_with_names(Dictionary<string, string> pathwayID_pathwayName_dict)
        {
            List<Obo_networkTable_line_class> keep = new List<Obo_networkTable_line_class>();
            string current_parent_organism_string_in_id;
            string current_child_organism_string_in_id;
            Dictionary<string, bool> removedOrganismString_dict = new Dictionary<string, bool>();
            string kept_organism_string = "";
            foreach (Obo_networkTable_line_class networkTable_line in this.NetworkTable)
            {
                current_child_organism_string_in_id = networkTable_line.Child_id.Split('-')[1];
                current_parent_organism_string_in_id = networkTable_line.Parent_id.Split('-')[1];
                if ((pathwayID_pathwayName_dict.ContainsKey(networkTable_line.Parent_id))
                    && (pathwayID_pathwayName_dict.ContainsKey(networkTable_line.Child_id)))
                {
                    if (String.IsNullOrEmpty(kept_organism_string)) { kept_organism_string = (string)current_child_organism_string_in_id.Clone(); }
                    else if (!kept_organism_string.Equals(current_child_organism_string_in_id)) { throw new Exception(); }
                    if (!kept_organism_string.Equals(current_parent_organism_string_in_id)) { throw new Exception(); }
                    networkTable_line.Parent_name = (string)pathwayID_pathwayName_dict[networkTable_line.Parent_id].Clone();
                    networkTable_line.Child_name = (string)pathwayID_pathwayName_dict[networkTable_line.Child_id].Clone();
                    keep.Add(networkTable_line);
                }
                else
                {
                    if (!removedOrganismString_dict.ContainsKey(current_child_organism_string_in_id)) { removedOrganismString_dict.Add(current_child_organism_string_in_id, true); }
                    if (!removedOrganismString_dict.ContainsKey(current_parent_organism_string_in_id)) { removedOrganismString_dict.Add(current_parent_organism_string_in_id, true); }
                }
            }
            if (removedOrganismString_dict.ContainsKey(kept_organism_string)) { throw new Exception(); }
            if (keep.Count==0) { throw new Exception(); }
            this.NetworkTable = keep.ToArray();
        }

        public void Add_overall_parents_as_children_with_given_parent_and_add_overall_given_parent(Ontology_type_enum ontology)
        {
            Namespace_type_enum overall_parent_namespace_type = Namespace_type_enum.E_m_p_t_y;
            Dictionary<string, bool> nodeName_hasParent_dict = new Dictionary<string, bool>();
            Dictionary<string, Namespace_type_enum> nodeName_namespaceType_dict = new Dictionary<string, Namespace_type_enum>();
            Dictionary<string, string> parentNodeName_nodeId_dict = new Dictionary<string, string>();
            Dictionary<string, bool> nodeName_namespaceIsForParentFromChild_dict = new Dictionary<string, bool>();
            string overall_parent_name = "error";
            switch (ontology)
            {
                case Ontology_type_enum.Custom_1:
                case Ontology_type_enum.Custom_2:
                case Ontology_type_enum.Reactome:
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                    overall_parent_name = Ontology_classification_class.Get_name_of_ontology(ontology);
                    break;
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_cc:
                case Ontology_type_enum.Go_mf:
                    overall_parent_name = "Gene Ontology";
                    overall_parent_namespace_type = Namespace_type_enum.Go_overall_parent;
                    break;
                default:
                    throw new Exception();
            }
            foreach (Obo_networkTable_line_class networkTable_line in this.NetworkTable)
            {
                if (!nodeName_hasParent_dict.ContainsKey(networkTable_line.Parent_name))
                {
                    nodeName_hasParent_dict.Add(networkTable_line.Parent_name, false);
                }
                if (!nodeName_hasParent_dict.ContainsKey(networkTable_line.Child_name))
                {
                    nodeName_hasParent_dict.Add(networkTable_line.Child_name, false);
                }
                if (!nodeName_namespaceType_dict.ContainsKey(networkTable_line.Parent_name))
                {
                    nodeName_namespaceType_dict.Add(networkTable_line.Parent_name, networkTable_line.Child_namespace_type);
                    nodeName_namespaceIsForParentFromChild_dict.Add(networkTable_line.Parent_name, true);
                }
                if (!nodeName_namespaceType_dict.ContainsKey(networkTable_line.Child_name))
                {
                    nodeName_namespaceType_dict.Add(networkTable_line.Child_name, networkTable_line.Child_namespace_type);
                }
                else if (  (nodeName_namespaceIsForParentFromChild_dict.ContainsKey(networkTable_line.Child_name))
                         &&(nodeName_namespaceIsForParentFromChild_dict[networkTable_line.Child_name]))
                {
                    nodeName_namespaceType_dict[networkTable_line.Child_name] = networkTable_line.Child_namespace_type;
                    nodeName_namespaceIsForParentFromChild_dict[networkTable_line.Child_name] = false;
                }
                else if (!nodeName_namespaceType_dict[networkTable_line.Child_name].Equals(networkTable_line.Child_namespace_type))
                {
                    throw new Exception();
                }
                if (networkTable_line.Parent_name.Equals(Global_class.Empty_entry))
                {
                    networkTable_line.Parent_name = (string)overall_parent_name.Clone();
                    networkTable_line.Parent_id = (string)overall_parent_name.Clone();
                }
                else if (String.IsNullOrEmpty(networkTable_line.Parent_name))
                {
                    throw new Exception();
                }
                nodeName_hasParent_dict[networkTable_line.Child_name] = true;
                if (!parentNodeName_nodeId_dict.ContainsKey(networkTable_line.Parent_name))
                {
                    parentNodeName_nodeId_dict.Add(networkTable_line.Parent_name, networkTable_line.Parent_id);
                }
                else if (!parentNodeName_nodeId_dict[networkTable_line.Parent_name].Equals(networkTable_line.Parent_id)) { throw new Exception(); }
            }
            string[] nodeNames = nodeName_hasParent_dict.Keys.ToArray();
            string nodeName;
            int nodeNames_length = nodeNames.Length;
            Obo_networkTable_line_class new_networkTable_line;
            List<Obo_networkTable_line_class> new_networkTables = new List<Obo_networkTable_line_class>();
            for (int indexN=0; indexN<nodeNames_length; indexN++)
            {
                nodeName = nodeNames[indexN];
                if ((!nodeName_hasParent_dict[nodeName])&&(!nodeName.Equals(Global_class.Empty_entry)))
                {
                    new_networkTable_line = new Obo_networkTable_line_class();
                    new_networkTable_line.Parent_name = (string)overall_parent_name.Clone();
                    new_networkTable_line.Parent_id = (string)overall_parent_name.Clone();
                    new_networkTable_line.Child_name = (string)nodeName.Clone();
                    new_networkTable_line.Child_id = (string)parentNodeName_nodeId_dict[nodeName].Clone();
                    new_networkTable_line.Child_namespace_type = nodeName_namespaceType_dict[nodeName];
                    new_networkTables.Add(new_networkTable_line);
                }
            }
            new_networkTable_line = new Obo_networkTable_line_class();
            new_networkTable_line.Parent_name = Global_class.Empty_entry;
            new_networkTable_line.Parent_id = Global_class.Empty_entry;
            new_networkTable_line.Child_name = (string)overall_parent_name.Clone();
            new_networkTable_line.Child_id = (string)overall_parent_name.Clone();
            new_networkTable_line.Child_namespace_type = overall_parent_namespace_type;
            new_networkTables.Add(new_networkTable_line);
            this.Add_to_networkTables(new_networkTables.ToArray());
        }

        public void Fill_ids_with_names_and_remove_duplicates()
        {
            List<Obo_networkTable_line_class> keep = new List<Obo_networkTable_line_class>();
            Dictionary<string,Dictionary<string,bool>> child_parent_dict = new Dictionary<string, Dictionary<string, bool>>();
            foreach (Obo_networkTable_line_class networkTable_line in this.NetworkTable)
            {
                if (!child_parent_dict.ContainsKey(networkTable_line.Child_name))
                {
                    child_parent_dict.Add(networkTable_line.Child_name, new Dictionary<string,bool>());
                }
                if (!child_parent_dict[networkTable_line.Child_name].ContainsKey(networkTable_line.Parent_name))
                {
                    child_parent_dict[networkTable_line.Child_name].Add(networkTable_line.Parent_name, true);
                    networkTable_line.Parent_id = (string)networkTable_line.Parent_name.Clone();
                    networkTable_line.Child_id = (string)networkTable_line.Child_name.Clone();
                    if (String.IsNullOrEmpty(networkTable_line.Parent_id)) { throw new Exception(); }
                    if (String.IsNullOrEmpty(networkTable_line.Child_id)) { throw new Exception(); }
                    keep.Add(networkTable_line);
                }
            }
            this.NetworkTable = keep.ToArray();
        }
        public void Add_pathwayID_to_pathway_if_duplicated_pathway_name_for_same_organism_and_check_if_only_one_organism()
        {
            Dictionary<string, Dictionary<string, Dictionary<string, bool>>> organism_pathwayName_pathwayIDs_dict = new Dictionary<string, Dictionary<string, Dictionary<string, bool>>>();
            string[] splitStrings;
            string organism_string;
            foreach (Obo_networkTable_line_class networkTable_line in this.NetworkTable)
            {
                splitStrings = networkTable_line.Child_id.Split('-');
                if (Global_class.Do_internal_checks && splitStrings.Length!=3) { throw new Exception(); }
                organism_string = splitStrings[1];
                if (!organism_pathwayName_pathwayIDs_dict.ContainsKey(organism_string))
                {
                    organism_pathwayName_pathwayIDs_dict.Add(organism_string, new Dictionary<string, Dictionary<string, bool>>());
                }
                if (!organism_pathwayName_pathwayIDs_dict[organism_string].ContainsKey(networkTable_line.Child_name))
                {
                    organism_pathwayName_pathwayIDs_dict[organism_string].Add(networkTable_line.Child_name, new Dictionary<string, bool>());
                }
                if (!organism_pathwayName_pathwayIDs_dict[organism_string][networkTable_line.Child_name].ContainsKey(networkTable_line.Child_id))
                {
                    organism_pathwayName_pathwayIDs_dict[organism_string][networkTable_line.Child_name].Add(networkTable_line.Child_id, true);
                }

                splitStrings = networkTable_line.Parent_id.Split('-');
                if (Global_class.Do_internal_checks && splitStrings.Length != 3) { throw new Exception(); }
                organism_string = splitStrings[1];
                if (!organism_pathwayName_pathwayIDs_dict.ContainsKey(organism_string))
                {
                    organism_pathwayName_pathwayIDs_dict.Add(organism_string, new Dictionary<string, Dictionary<string, bool>>());
                }
                if (!organism_pathwayName_pathwayIDs_dict[organism_string].ContainsKey(networkTable_line.Parent_name))
                {
                    organism_pathwayName_pathwayIDs_dict[organism_string].Add(networkTable_line.Parent_name, new Dictionary<string, bool>());
                }
                if (!organism_pathwayName_pathwayIDs_dict[organism_string][networkTable_line.Parent_name].ContainsKey(networkTable_line.Parent_id))
                {
                    organism_pathwayName_pathwayIDs_dict[organism_string][networkTable_line.Parent_name].Add(networkTable_line.Parent_id, true);
                }
            }

            if (organism_pathwayName_pathwayIDs_dict.Keys.Count!=1) { throw new Exception(); }

            foreach (Obo_networkTable_line_class networkTable_line in this.NetworkTable)
            {
                splitStrings = networkTable_line.Child_id.Split('-');
                if (Global_class.Do_internal_checks && splitStrings.Length != 3) { throw new Exception(); }
                organism_string = splitStrings[1];
                if (organism_pathwayName_pathwayIDs_dict[organism_string][networkTable_line.Child_name].Count > 1)
                {
                    networkTable_line.Child_name = networkTable_line.Child_name + " (" + networkTable_line.Child_id + ")";
                }

                splitStrings = networkTable_line.Parent_id.Split('-');
                if (Global_class.Do_internal_checks && splitStrings.Length != 3) { throw new Exception(); }
                organism_string = splitStrings[1];
                if (organism_pathwayName_pathwayIDs_dict[organism_string][networkTable_line.Parent_name].Count > 1)
                {
                    networkTable_line.Parent_name = networkTable_line.Parent_name + " (" + networkTable_line.Parent_id + ")";
                }
            }
        }

        public void Keep_only_lines_with_parent_and_child_scp_part_of_input_scps(string[] keep_scps)
        {
            keep_scps = keep_scps.Distinct().ToArray();
            Dictionary<string, bool> keep_scp_dict = new Dictionary<string, bool>();
            foreach (string keep_scp in keep_scps)
            {
                keep_scp_dict.Add(keep_scp, true);
            }
            List<Obo_networkTable_line_class> keep = new List<Obo_networkTable_line_class>();
            foreach (Obo_networkTable_line_class obo_line in this.NetworkTable)
            {
                if (  (keep_scp_dict.ContainsKey(obo_line.Child_name))
                    &&(keep_scp_dict.ContainsKey(obo_line.Parent_name)))
                {
                    keep.Add(obo_line);
                }
            }
            this.NetworkTable = keep.ToArray();
        }
        public void Read_hierarchy_networkTables_standardInput(Ontology_type_enum ontology, ProgressReport_interface_class progressReport)
        {
            Hierachy_networkTable_standardInput_readOptions_class readOptions = new Hierachy_networkTable_standardInput_readOptions_class(ontology);
            string shared_error_message = Ontology_classification_class.Get_pleaseDoubleCheck_file_messge(readOptions.File);
            this.NetworkTable = ReadWriteClass.Read_data_fill_array_and_complain_including_shared_response_and_delete_if_error_message<Obo_networkTable_line_class>(readOptions, progressReport, shared_error_message);
        }
        public void Read_hierarchy_networkTables_specialMbcoDatasetsInput(Ontology_type_enum ontology, ProgressReport_interface_class progressReport)
        {
            string shared_error_response = Ontology_classification_class.Get_pleaseDownloadMbcoPathNet_again_message();
            Hierachy_networkTable_specialMbcoInput_readOptions_class readOptions = new Hierachy_networkTable_specialMbcoInput_readOptions_class(ontology);
            this.NetworkTable = ReadWriteClass.Read_data_fill_array_and_complain_including_shared_response_and_delete_if_error_message<Obo_networkTable_line_class>(readOptions, progressReport, shared_error_response);
        }
        public void Read_hierarchy_networkTables_reactome(ProgressReport_interface_class progressReport)
        {
            Hierachy_networkTable_reactome_readOptions_class readOptions = new Hierachy_networkTable_reactome_readOptions_class();
            string shared_error_response = Ontology_classification_class.Get_pleaseDonwload_file_again_message(readOptions.File);
            this.NetworkTable = ReadWriteClass.Read_data_fill_array_and_complain_including_shared_response_and_delete_if_error_message<Obo_networkTable_line_class>(readOptions, progressReport, shared_error_response);
        }
        public void Read_allInfo_networkTables(Ontology_type_enum ontology, Organism_enum organism, SCP_hierarchy_interaction_type_enum interaction_type, ProgressReport_interface_class progressReport)
        {
            Hierarchy_networkTable_allInfo_readOptions_class readOptions = new Hierarchy_networkTable_allInfo_readOptions_class(ontology, organism, interaction_type);
            string shared_error_response = Ontology_classification_class.Get_pleaseDelete_file_message(readOptions.File);
            this.NetworkTable = ReadWriteClass.Read_data_fill_array_and_complain_including_shared_response_and_delete_if_error_message<Obo_networkTable_line_class>(readOptions, progressReport, shared_error_response);
        }
        public void Write_allInfo_networkTables(Ontology_type_enum ontology, Organism_enum organism, SCP_hierarchy_interaction_type_enum interaction_type, ProgressReport_interface_class progressReport, out bool file_opened_successfully)
        {
            Hierarchy_networkTable_allInfo_readOptions_class readOptions = new Hierarchy_networkTable_allInfo_readOptions_class(ontology, organism, interaction_type);
            ReadWriteClass.WriteData_and_add_warning_to_progressReport_if_failed(this.NetworkTable,readOptions, progressReport, out file_opened_successfully);
        }

        public Obo_networkTable_class Deep_copy()
        {
            Obo_networkTable_class copy = (Obo_networkTable_class)this.MemberwiseClone();
            int nw_length = this.NetworkTable.Length;
            copy.NetworkTable = new Obo_networkTable_line_class[nw_length];
            for (int indexSigNW = 0; indexSigNW < nw_length; indexSigNW++)
            {
                copy.NetworkTable[indexSigNW] = this.NetworkTable[indexSigNW].Deep_copy();
            }
            return copy;
        }
    }
}
