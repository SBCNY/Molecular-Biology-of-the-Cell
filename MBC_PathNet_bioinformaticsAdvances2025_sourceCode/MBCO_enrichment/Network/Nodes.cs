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
using Enrichment;
using Common_functions.Array_own;
using System.Drawing;
using Common_functions.Global_definitions;
using yed_network;
using Common_functions.ReadWrite;
using Data;
using MBCO;


namespace Network
{

    public enum Obo_interaction_type_enum { E_m_p_t_y, Is_a, Part_of, Regulates, Positively_regulates, Negatively_regulates }
    public enum Ontology_direction_enum { E_m_p_t_y, Parent_child, Child_parent }

    class Color_specification_line_class
    {
        public Color Fill_color { get; set; }
        public float Size { get; set; }
        public int Dataset_order_no { get; set; }

        public Color_specification_line_class()
        {
            Size = 1;
        }

        public Color_specification_line_class Deep_copy()
        {
            Color_specification_line_class copy = (Color_specification_line_class)this.MemberwiseClone();
            return copy;
        }
    }

    class Add_node_line_class
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Depth { get; set; }
        public int Size { get; set; }
        public Ontology_type_enum Ontology_type { get; set; }
        public Namespace_type_enum Ontology_namespace { get; set; }

        public static Add_node_line_class[] Order_by_standard_way(Add_node_line_class[] add_nodes)
        {
            Dictionary<string, Dictionary<string, Dictionary<Namespace_type_enum, Dictionary<Ontology_type_enum, List<Add_node_line_class>>>>> id_name_namespace_ontology_dict = new Dictionary<string, Dictionary<string, Dictionary<Namespace_type_enum, Dictionary<Ontology_type_enum, List<Add_node_line_class>>>>>();
            Dictionary<string, Dictionary<Namespace_type_enum, Dictionary<Ontology_type_enum, List<Add_node_line_class>>>> name_namespace_ontology_dict = new Dictionary<string, Dictionary<Namespace_type_enum, Dictionary<Ontology_type_enum, List<Add_node_line_class>>>>();
            Dictionary<Namespace_type_enum, Dictionary<Ontology_type_enum, List<Add_node_line_class>>> namespace_ontology_dict = new Dictionary<Namespace_type_enum, Dictionary<Ontology_type_enum, List<Add_node_line_class>>>();
            Dictionary<Ontology_type_enum, List<Add_node_line_class>> ontology_dict = new Dictionary<Ontology_type_enum, List<Add_node_line_class>>();
            Add_node_line_class add_node;
            int add_nodes_length = add_nodes.Length;
            for (int indexAdd=0; indexAdd<add_nodes_length;indexAdd++)
            {
                add_node = add_nodes[indexAdd];
                if (!id_name_namespace_ontology_dict.ContainsKey(add_node.Id))
                {
                    id_name_namespace_ontology_dict.Add(add_node.Id, new Dictionary<string, Dictionary<Namespace_type_enum, Dictionary<Ontology_type_enum, List<Add_node_line_class>>>>());
                }
                if (!id_name_namespace_ontology_dict[add_node.Id].ContainsKey(add_node.Name))
                {
                    id_name_namespace_ontology_dict[add_node.Id].Add(add_node.Name, new Dictionary<Namespace_type_enum, Dictionary<Ontology_type_enum, List<Add_node_line_class>>>());
                }
                if (!id_name_namespace_ontology_dict[add_node.Id][add_node.Name].ContainsKey(add_node.Ontology_namespace))
                {
                    id_name_namespace_ontology_dict[add_node.Id][add_node.Name].Add(add_node.Ontology_namespace, new Dictionary<Ontology_type_enum, List<Add_node_line_class>>());
                }
                if (!id_name_namespace_ontology_dict[add_node.Id][add_node.Name][add_node.Ontology_namespace].ContainsKey(add_node.Ontology_type))
                {
                    id_name_namespace_ontology_dict[add_node.Id][add_node.Name][add_node.Ontology_namespace].Add(add_node.Ontology_type, new List<Add_node_line_class>());
                }
                id_name_namespace_ontology_dict[add_node.Id][add_node.Name][add_node.Ontology_namespace][add_node.Ontology_type].Add(add_node);
            }
            add_nodes = null;
            List<Add_node_line_class> ordered_lines = new List<Add_node_line_class>();
            string[] ids;
            string id;
            int ids_length;
            string[] names;
            string name;
            int names_length;
            Namespace_type_enum[] ontology_namespaces;
            Namespace_type_enum ontology_namespace;
            int ontology_namespaces_length;
            Ontology_type_enum[] ontologies;
            Ontology_type_enum ontology;
            int ontologies_length;
            ids = id_name_namespace_ontology_dict.Keys.ToArray();
            ids = ids.OrderBy(l => l).ToArray();
            ids_length = ids.Length;
            for (int indexId = 0; indexId < ids_length; indexId++)
            {
                id = ids[indexId];
                name_namespace_ontology_dict = id_name_namespace_ontology_dict[id];
                names = name_namespace_ontology_dict.Keys.ToArray();
                names = names.OrderBy(l => l).ToArray();
                names_length = names.Length;
                for (int indexN = 0; indexN < names_length; indexN++)
                {
                    name = names[indexN];
                    namespace_ontology_dict = name_namespace_ontology_dict[name];
                    ontology_namespaces = namespace_ontology_dict.Keys.ToArray();
                    ontology_namespaces = ontology_namespaces.OrderBy(l => l).ToArray();
                    ontology_namespaces_length = ontology_namespaces.Length;
                    for (int indexON = 0; indexON < ontology_namespaces_length; indexON++)
                    {
                        ontology_namespace = ontology_namespaces[indexON];
                        ontology_dict = namespace_ontology_dict[ontology_namespace];
                        ontologies = ontology_dict.Keys.ToArray();
                        ontologies = ontologies.OrderBy(l => l).ToArray();
                        ontologies_length = ontologies.Length;
                        for (int indexO = 0; indexO < ontologies_length; indexO++)
                        {
                            ontology = ontologies[indexO];
                            ordered_lines.AddRange(ontology_dict[ontology]);
                        }
                    }
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count!=add_nodes_length) { throw new Exception(); }
                Add_node_line_class this_line;
                Add_node_line_class previous_line;
                for (int indexThis=1;indexThis<add_nodes_length;indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (this_line.Id.CompareTo(previous_line.Id)<0) {  throw new Exception(); }
                    else if ((this_line.Id.Equals(previous_line.Id))
                             && (this_line.Name.CompareTo(previous_line.Name) < 0)) { throw new Exception(); }
                    else if ((this_line.Id.Equals(previous_line.Id))
                             && (this_line.Name.Equals(previous_line.Name))
                             && (this_line.Ontology_namespace.CompareTo(previous_line.Ontology_namespace) < 0)) { throw new Exception(); }
                    else if ((this_line.Id.Equals(previous_line.Id))
                             && (this_line.Name.Equals(previous_line.Name))
                             && (this_line.Ontology_namespace.Equals(previous_line.Ontology_namespace))
                             && (this_line.Ontology_type.CompareTo(previous_line.Ontology_type) < 0)) { throw new Exception(); }
                }
            }
            //add_nodes = add_nodes.OrderBy(l => l.Id).ThenBy(l => l.Name).ThenBy(l => l.Ontology_namespace).ThenBy(l => l.Ontology_type).ToArray();
            return ordered_lines.ToArray();
        }

        public bool Equal_in_standard_way(Add_node_line_class other)
        {
            bool equal = ((this.Id.Equals(other.Id))
                          && (this.Name.Equals(other.Name))
                          && (this.Ontology_type.Equals(other.Ontology_type))
                          && (this.Ontology_namespace.Equals(other.Ontology_namespace)));
            return equal;
        }
    }

    class NetworkNodes_set_line_class
    {
        public string Set_name { get; set; }
        public float Minus_log10_pvalue { get; set; }

        public NetworkNodes_set_line_class Deep_copy()
        {
            NetworkNodes_set_line_class copy = (NetworkNodes_set_line_class)this.MemberwiseClone();
            copy.Set_name = (string)this.Set_name.Clone();
            return copy;
        }
    }

    class NetworkNode_line_class
    {
        #region Fields
        const string empty_entry = "E_m_p_t_y";

        public string Id { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public Namespace_type_enum Namespace_type { get; set; }
        public int NW_index { get; set; }
        public int NW_index_old { get; set; }
        public bool Populated { get; set; }
        public int Level { get; set; }
        public int Depth { get; set; }
        public int Size { get; set; }
        public NetworkNodes_set_line_class[] Sets_that_contain_nodes { get; set; }

        public static string Empty_entry { get { return empty_entry; } }
        #endregion

        #region Constructor
        public NetworkNode_line_class()
        {
            Name = (string)Empty_entry.Clone();
            Name2 = (string)Empty_entry.Clone();
            Id = (string)Empty_entry.Clone();
            NW_index_old = -1;
            NW_index = -1;
            Level = -1;
            Sets_that_contain_nodes = new NetworkNodes_set_line_class[0];
        }
        #endregion

        public bool Equal_in_standard_way(NetworkNode_line_class other)
        {
            bool equal = (this.Name.Equals(other.Name));
            //bool equal = ((this.Id.Equals(other.Id))
            //              && (this.Name.Equals(other.Name))
            //              && (this.Ontology_type.Equals(other.Ontology_type))
            //              && (this.Ontology_namespace.Equals(other.Ontology_namespace)));
            return equal;
        }

        public static NetworkNode_line_class[] Order_in_standard_way(NetworkNode_line_class[] nodes)
        {
            //return nodes.OrderBy(l => l.Id).ThenBy(l => l.Name).ThenBy(l => l.Ontology_type).ThenBy(l => l.Ontology_namespace).ToArray();
            //return nodes.OrderBy(l => l.Name).ToArray();
            return Order_by_name(nodes);
        }

        public static NetworkNode_line_class[] Order_by_id(NetworkNode_line_class[] nodes)
        {
            //Nodes = Nodes.OrderBy(l => l.Id).ToArray();
            Dictionary<string, List<NetworkNode_line_class>> id_dict = new Dictionary<string, List<NetworkNode_line_class>>();
            int nodes_length = nodes.Length;
            NetworkNode_line_class node;
            for (int indexN=0;indexN<nodes_length;indexN++)
            {
                node = nodes[indexN];
                if (!id_dict.ContainsKey(node.Id))
                {
                    id_dict.Add(node.Id, new List<NetworkNode_line_class>());
                }
                id_dict[node.Id].Add(node);
            }
            nodes = null;
            List<NetworkNode_line_class> ordered_lines = new List<NetworkNode_line_class>();
            string[] ids;
            string id;
            int ids_length;
            ids = id_dict.Keys.ToArray();
            ids = ids.OrderBy(l => l).ToArray();
            ids_length = ids.Length;
            for (int indexId=0; indexId<ids_length;indexId++)
            {
                id = ids[indexId];
                ordered_lines.AddRange(id_dict[id]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count!=nodes_length) { throw new Exception(); }
                NetworkNode_line_class this_line;
                NetworkNode_line_class previous_line;
                for (int indexThis=1; indexThis<nodes_length;indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (this_line.Id.CompareTo(previous_line.Id)<0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }

        public static NetworkNode_line_class[] Order_by_name(NetworkNode_line_class[] nodes)
        {
            //Nodes = Nodes.OrderBy(l => l.Name).ToArray();
            Dictionary<string, List<NetworkNode_line_class>> name_dict = new Dictionary<string, List<NetworkNode_line_class>>();
            int nodes_length = nodes.Length;
            NetworkNode_line_class node;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node = nodes[indexN];
                if (!name_dict.ContainsKey(node.Name))
                {
                    name_dict.Add(node.Name, new List<NetworkNode_line_class>());
                }
                name_dict[node.Name].Add(node);
            }
            nodes = null;
            List<NetworkNode_line_class> ordered_lines = new List<NetworkNode_line_class>();
            string[] names;
            string name;
            int names_length;
            names = name_dict.Keys.ToArray();
            names = names.OrderBy(l => l).ToArray();
            names_length = names.Length;
            for (int indexN = 0; indexN < names_length; indexN++)
            {
                name = names[indexN];
                ordered_lines.AddRange(name_dict[name]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != nodes_length) { throw new Exception(); }
                NetworkNode_line_class this_line;
                NetworkNode_line_class previous_line;
                for (int indexThis = 1; indexThis < nodes_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (this_line.Name.CompareTo(previous_line.Name) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }

        public static NetworkNode_line_class[] Order_by_nw_index_old(NetworkNode_line_class[] nodes)
        {
            //Nodes = Nodes.OrderBy(l => l.NW_index_old).ToArray();
            Dictionary<int, List<NetworkNode_line_class>> nwIndexOld_dict = new Dictionary<int, List<NetworkNode_line_class>>();
            int nodes_length = nodes.Length;
            NetworkNode_line_class node;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node = nodes[indexN];
                if (!nwIndexOld_dict.ContainsKey(node.NW_index_old))
                {
                    nwIndexOld_dict.Add(node.NW_index_old, new List<NetworkNode_line_class>());
                }
                nwIndexOld_dict[node.NW_index_old].Add(node);
            }
            nodes = null;
            List<NetworkNode_line_class> ordered_lines = new List<NetworkNode_line_class>();
            int[] nwIndexOlds;
            int nwIndexOld;
            int nwIndexOlds_length;
            nwIndexOlds = nwIndexOld_dict.Keys.ToArray();
            nwIndexOlds = nwIndexOlds.OrderBy(l => l).ToArray();
            nwIndexOlds_length = nwIndexOlds.Length;
            for (int indexNIO = 0; indexNIO < nwIndexOlds_length; indexNIO++)
            {
                nwIndexOld = nwIndexOlds[indexNIO];
                ordered_lines.AddRange(nwIndexOld_dict[nwIndexOld]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != nodes_length) { throw new Exception(); }
                NetworkNode_line_class this_line;
                NetworkNode_line_class previous_line;
                for (int indexThis = 1; indexThis < nodes_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (this_line.NW_index_old.CompareTo(previous_line.NW_index_old) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }

        public static NetworkNode_line_class[] Order_by_nw_index(NetworkNode_line_class[] nodes)
        {
            //Nodes = Nodes.OrderBy(l => l.NW_index).ToArray();
            Dictionary<int, List<NetworkNode_line_class>> nwIndex_dict = new Dictionary<int, List<NetworkNode_line_class>>();
            int nodes_length = nodes.Length;
            NetworkNode_line_class node;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node = nodes[indexN];
                if (!nwIndex_dict.ContainsKey(node.NW_index))
                {
                    nwIndex_dict.Add(node.NW_index, new List<NetworkNode_line_class>());
                }
                nwIndex_dict[node.NW_index].Add(node);
            }
            nodes = null;
            List<NetworkNode_line_class> ordered_lines = new List<NetworkNode_line_class>();
            int[] nwIndices;
            int nwIndex;
            int nwIndices_length;
            nwIndices = nwIndex_dict.Keys.ToArray();
            nwIndices = nwIndices.OrderBy(l => l).ToArray();
            nwIndices_length = nwIndices.Length;
            for (int indexNI = 0; indexNI < nwIndices_length; indexNI++)
            {
                nwIndex = nwIndices[indexNI];
                ordered_lines.AddRange(nwIndex_dict[nwIndex]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != nodes_length) { throw new Exception(); }
                NetworkNode_line_class this_line;
                NetworkNode_line_class previous_line;
                for (int indexThis = 1; indexThis < nodes_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (this_line.NW_index.CompareTo(previous_line.NW_index) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }

        public static NetworkNode_line_class[] Order_by_level(NetworkNode_line_class[] nodes)
        {
            //Nodes = Nodes.OrderBy(l => l.Level).ToArray();
            Dictionary<int, List<NetworkNode_line_class>> level_dict = new Dictionary<int, List<NetworkNode_line_class>>();
            int nodes_length = nodes.Length;
            NetworkNode_line_class node;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node = nodes[indexN];
                if (!level_dict.ContainsKey(node.Level))
                {
                    level_dict.Add(node.Level, new List<NetworkNode_line_class>());
                }
                level_dict[node.Level].Add(node);
            }
            nodes = null;
            List<NetworkNode_line_class> ordered_lines = new List<NetworkNode_line_class>();
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
                if (ordered_lines.Count != nodes_length) { throw new Exception(); }
                NetworkNode_line_class this_line;
                NetworkNode_line_class previous_line;
                for (int indexThis = 1; indexThis < nodes_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (this_line.Level.CompareTo(previous_line.Level) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }
        
        #region Copy
        public NetworkNode_line_class Deep_copy()
        {
            NetworkNode_line_class node_line = (NetworkNode_line_class)this.MemberwiseClone();
            node_line.Id = (string)this.Id.Clone();
            node_line.Name = (string)this.Name.Clone();
            node_line.Name2 = (string)this.Name2.Clone();
            int sets_length = this.Sets_that_contain_nodes.Length;
            node_line.Sets_that_contain_nodes = new NetworkNodes_set_line_class[sets_length];
            for (int indexSet = 0; indexSet < sets_length; indexSet++)
            {
                node_line.Sets_that_contain_nodes[indexSet] = this.Sets_that_contain_nodes[indexSet].Deep_copy();
            }
            return node_line;
        }
        #endregion
    }

    class NetworkNode_readWriteOptions_class : ReadWriteOptions_base
    {
        public NetworkNode_readWriteOptions_class(string file_name)
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            this.File = global_dirFile.Results_directory + file_name;
            this.Key_propertyNames = new string[] { "Id", "Name", "Name2", "NW_index", "NW_index_old", "Populated", "MinusLog10Pvalue", "Level" };
            this.Key_columnNames = this.Key_propertyNames;
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.Report = ReadWrite_report_enum.Report_main;
            this.File_has_headline = true;
        }
    }

    class NetworkNode_class
    {
        #region Fields
        public NetworkNode_line_class[] Nodes { get; set; }
        public bool Index_change_adopted { get; set; }
        public int Nodes_length { get { return Nodes.Length; } }
        public Ontology_direction_enum Direction { get; set; }
        #endregion

        public NetworkNode_class()
        {
            Nodes = new NetworkNode_line_class[0];
            Index_change_adopted = true;
        }

        #region Check
        public bool Correctness_check()
        {
            bool ok = true;
            if (!Index_change_adopted)
            {
                string text = "Index changes are not adopted";
                throw new Exception(text);
            }
            return ok;
        }
        #endregion

        #region Order
        public void Order_by_id()
        {
            //Nodes = Nodes.OrderBy(l => l.Id).ToArray();
            Nodes = NetworkNode_line_class.Order_by_id(Nodes);
        }

        public void Order_by_name()
        {
            //Nodes = Nodes.OrderBy(l => l.Name).ToArray();
            Nodes = NetworkNode_line_class.Order_by_name(Nodes);
        }

        public void Order_by_nw_index_old()
        {
            //Nodes = Nodes.OrderBy(l => l.NW_index_old).ToArray();
            Nodes = NetworkNode_line_class.Order_by_nw_index_old(Nodes);
        }

        public void Order_by_nw_index()
        {
            //Nodes = Nodes.OrderBy(l => l.NW_index).ToArray();
            Nodes = NetworkNode_line_class.Order_by_nw_index(Nodes);
        }

        public void Order_by_level()
        {
            //Nodes = Nodes.OrderBy(l => l.Level).ToArray();
            Nodes = NetworkNode_line_class.Order_by_level(Nodes);
        }
        #endregion

        #region Get
        public NetworkNode_line_class Get_indexed_node_line_if_index_is_correct(int indexNode)
        {
            NetworkNode_line_class node_line = Nodes[indexNode];
            if (node_line.NW_index != indexNode)
            {
                node_line = new NetworkNode_line_class();
                //Report_class.Write_error_line("{0}: Get indexed node line, Indexes do not match ({1} <-> {2})", typeof(NetworkNode_line_class).Name, indexNode, node_line.NW_index);
                throw new Exception();
            }
            return node_line;
        }

        public NetworkNode_line_class Get_indexed_node_line_without_checking_if_index_is_correct(int indexNode)
        {
            NetworkNode_line_class node_line = Nodes[indexNode];
            return node_line;
        }

        public NetworkNode_line_class Get_indexed_old_node_line_if_index_old_is_correct(int indexNode_old)
        {
            NetworkNode_line_class node_line = Nodes[indexNode_old];
            if (node_line.NW_index_old != indexNode_old)
            {
                node_line = new NetworkNode_line_class();
                string text = typeof(NetworkNode_class).Name + ": Get indexed old node line, Indexes_old do not match (" + indexNode_old + " <-> " + node_line.NW_index_old + ")";
                throw new Exception(text);
            }
            return node_line;
        }

        public int Get_max_nw_index()
        {
            int max_index = -1;
            foreach (NetworkNode_line_class node_line in Nodes)
            {
                max_index = Math.Max(max_index, node_line.NW_index);
            }
            return max_index;
        }

        private int Get_max_level()
        {
            int max_level = -1;
            foreach (NetworkNode_line_class node_line in Nodes)
            {
                if (node_line.Level > max_level)
                {
                    max_level = node_line.Level;
                }
            }
            return max_level;
        }

        public string[] Get_all_nodeNames_of_indicated_levels(params int[] levels)
        {
            //levels = levels.Distinct().OrderBy(l => l).ToArray();
            Dictionary<int, bool> level_dict = new Dictionary<int, bool>();
            foreach (int level in levels)
            {
                if (!level_dict.ContainsKey(level))
                { level_dict.Add(level, true); }
            }
            int nodes_length = Nodes_length;
            List<string> nodeNames = new List<string>();
            NetworkNode_line_class node_line;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes[indexN];
                if (node_line.Level == -1)
                {
                    throw new Exception();
                }
                if (level_dict.ContainsKey(node_line.Level))
                {
                    nodeNames.Add(node_line.Name);
                }
            }
            return nodeNames.ToArray();
        }
        public string[] Get_all_nodeNames_of_indicated_depths(params int[] depths)
        {
            //levels = levels.Distinct().OrderBy(l => l).ToArray();
            Dictionary<int, bool> depth_dict = new Dictionary<int, bool>();
            foreach (int depth in depths)
            {
                if (!depth_dict.ContainsKey(depth))
                { depth_dict.Add(depth, true); }
            }
            int nodes_length = Nodes_length;
            List<string> nodeNames = new List<string>();
            NetworkNode_line_class node_line;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes[indexN];
                if (node_line.Depth == -1)
                {
                    throw new Exception();
                }
                if (depth_dict.ContainsKey(node_line.Depth))
                {
                    nodeNames.Add(node_line.Name);
                }
            }
            return nodeNames.ToArray();
        }
        public string[] Get_all_nodeNames_of_indicated_level_from_min_to_max_size(int min_size, int max_size, int level)
        {
            int nodes_length = Nodes_length;
            List<string> nodeNames = new List<string>();
            NetworkNode_line_class node_line;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes[indexN];
                if (node_line.Level == -1) { throw new Exception(); }
                if (  (node_line.Level==level)
                    &&(node_line.Size>=min_size)
                    &&(node_line.Size<=max_size))
                {
                    nodeNames.Add(node_line.Name);
                }
            }
            return nodeNames.ToArray();
        }
        public Namespace_type_enum[] Get_all_ordered_namespaces()
        {
            Dictionary<Namespace_type_enum, bool> namespace_dict = new Dictionary<Namespace_type_enum, bool>();
            foreach (NetworkNode_line_class mbco_association_line in this.Nodes)
            {
                if (!namespace_dict.ContainsKey(mbco_association_line.Namespace_type))
                {
                    namespace_dict.Add(mbco_association_line.Namespace_type, true);
                }
            }
            return namespace_dict.Keys.OrderBy(l => l).ToArray();
        }
        public string[] Get_all_ordered_nodeNames_of_indicated_namespace(Namespace_type_enum namespace_type)
        {
            int nodes_length = Nodes_length;
            Dictionary<string, bool> nodeName_dict = new Dictionary<string, bool>();
            NetworkNode_line_class node_line;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes[indexN];
                if (node_line.Namespace_type.Equals(Namespace_type_enum.E_m_p_t_y)) { throw new Exception(); }
                if (node_line.Namespace_type.Equals(namespace_type))
                {
                    nodeName_dict.Add(node_line.Name, true);
                }
            }
            return nodeName_dict.Keys.OrderBy(l=>l).ToArray();
        }
        public Dictionary<string,Namespace_type_enum> Get_nodeName_namespace_dictionary()
        {
            Dictionary<string, Namespace_type_enum> nodeName_namespace_dict = new Dictionary<string, Namespace_type_enum>();
            int nodes_length = Nodes_length;
            NetworkNode_line_class node_line;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes[indexN];
                nodeName_namespace_dict.Add((string)node_line.Name.Clone(), node_line.Namespace_type);
            }
            return nodeName_namespace_dict;
        }

        public string[] Get_all_ordered_nodeNames()
        {
            NetworkNode_line_class node_line;
            Dictionary<string, bool> nodeName_dict = new Dictionary<string, bool>();
            int nodes_length = this.Nodes_length;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes[indexN];
                if (!nodeName_dict.ContainsKey(node_line.Name))
                {
                    nodeName_dict.Add(node_line.Name, true);
                }
            }
            return nodeName_dict.Keys.OrderBy(l=>l).ToArray();

        }

        public int[] Get_all_levels()
        {
            Dictionary<int, bool> level_dict = new Dictionary<int, bool>();
            int nodes_length = this.Nodes_length;
            NetworkNode_line_class node_line;
            for (int indexNode = 0; indexNode < nodes_length; indexNode++)
            {
                node_line = this.Nodes[indexNode];
                if (!level_dict.ContainsKey(node_line.Level))
                {
                    level_dict.Add(node_line.Level, true);
                }
            }
            return level_dict.Keys.OrderBy(l => l).ToArray();
        }
        public int[] Get_all_depths()
        {
            Dictionary<int, bool> depth_dict = new Dictionary<int, bool>();
            int nodes_length = this.Nodes_length;
            NetworkNode_line_class node_line;
            for (int indexNode = 0; indexNode < nodes_length; indexNode++)
            {
                node_line = this.Nodes[indexNode];
                if (!depth_dict.ContainsKey(node_line.Depth))
                {
                    depth_dict.Add(node_line.Level, true);
                }
            }
            return depth_dict.Keys.OrderBy(l => l).ToArray();
        }

        public int Get_level_of_processName(string node_name)
        {
            int processLevel = -1;
            foreach (NetworkNode_line_class node_line in Nodes)
            {
                if (node_line.Name.Equals(node_name))
                {
                    processLevel = node_line.Level;
                }
            }
            if (processLevel == -1) { throw new Exception(); }
            return processLevel;
        }
        public string[] Get_names_of_indexes_without_ordering_by_index(params int[] indexes)
        {
            Correctness_check();
            //this.Order_by_nw_index();
            List<string> node_names = new List<string>();
            NetworkNode_line_class node_line;
            foreach (int index in indexes)
            {
                node_line = Get_indexed_node_line_if_index_is_correct(index);
                if (node_line.NW_index!=index) { throw new Exception(); }
                node_names.Add(node_line.Name);
            }
            return node_names.ToArray();
        }

        public string[] Get_names_and_ids_of_indexes(out string[] node_ids, int[] indexes)
        {
            Correctness_check();
            this.Order_by_nw_index();
            List<string> node_names = new List<string>();
            List<string> node_ids_list = new List<string>();
            NetworkNode_line_class node_line;
            foreach (int index in indexes)
            {
                node_line = Get_indexed_node_line_if_index_is_correct(index);
                node_names.Add(node_line.Name);
                node_ids_list.Add(node_line.Id);
            }
            node_ids = node_ids_list.ToArray();
            return node_names.ToArray();
        }

        public string[] Get_ids_of_indexes(int[] indexes)
        {
            Correctness_check();
            this.Order_by_nw_index();
            List<string> node_ids = new List<string>();
            NetworkNode_line_class node_line;
            foreach (int index in indexes)
            {
                node_line = Get_indexed_node_line_if_index_is_correct(index);
                node_ids.Add(node_line.Id);
            }
            return node_ids.ToArray();
        }

        public int[] Get_nw_indexes_of_names(params string[] names)
        {
            Correctness_check();
            Dictionary<string, bool> names_dict = new Dictionary<string, bool>();
            foreach (string name in names)
            {
                if (!names_dict.ContainsKey(name))
                {
                    names_dict.Add(name, true);
                }
            }
            int node_length = Nodes_length;
            List<int> node_indexes = new List<int>();
            NetworkNode_line_class node_line;
            for (int indexNode = 0; indexNode < node_length; indexNode++)
            {
                node_line = Nodes[indexNode];
                if (names_dict.ContainsKey(node_line.Name))
                {
                    node_indexes.Add(node_line.NW_index);
                }
            }
            return node_indexes.OrderBy(l => l).ToArray();
        }

        public Dictionary<string,int> Get_nodeName_size_dict()
        {
            Dictionary<string,int> nodeName_size_dict = new Dictionary<string,int>();
            foreach (NetworkNode_line_class node_line in this.Nodes)
            {
                nodeName_size_dict.Add(node_line.Name, node_line.Size);
            }
            return nodeName_size_dict;
        }
        public Dictionary<string, string> Get_pathwayId_pathway_dict()
        {
            Dictionary<string, string> pathwayId_pathway_dict = new Dictionary<string, string>();
            foreach (NetworkNode_line_class node_line in this.Nodes)
            {
                pathwayId_pathway_dict.Add(node_line.Id, node_line.Name);
            }
            return pathwayId_pathway_dict;
        }

        #endregion

        #region Get dictionaries
        public Dictionary<string, int> Get_id_index_dictionary()
        {
            Dictionary<string, int> id_index = new Dictionary<string, int>();
            if (Correctness_check())
            {
                this.Order_by_id();
                foreach (NetworkNode_line_class node_line in Nodes)
                {
                    id_index.Add(node_line.Id, node_line.NW_index);
                }
            }
            return id_index;
        }
        public Dictionary<string, int> Get_name_index_dictionary()
        {
            Dictionary<string, int> name_index_dict = new Dictionary<string, int>();
            if (Correctness_check())
            {
                foreach (NetworkNode_line_class node_line in Nodes)
                {
                    name_index_dict.Add(node_line.Name, node_line.NW_index);
                }
            }
            return name_index_dict;
        }
        public Dictionary<string, int> Get_name_depth_dictionary()
        {
            Dictionary<string, int> name_depth_dict = new Dictionary<string, int>();
            foreach (NetworkNode_line_class node_line in Nodes)
            {
                name_depth_dict.Add(node_line.Name, node_line.Depth);
            }
            return name_depth_dict;
        }
        public Dictionary<string, int> Get_name_level_dictionary()
        {
            Dictionary<string, int> name_level_dict = new Dictionary<string, int>();
            foreach (NetworkNode_line_class node_line in Nodes)
            {
                name_level_dict.Add(node_line.Name, node_line.Level);
            }
            return name_level_dict;
        }
        #endregion

        #region Set
        public void Set_processLevel_for_all_nodes_based_on_dictionary(Dictionary<string, int> processName_level_dict)
        {
            int nodes_length = this.Nodes.Length;
            NetworkNode_line_class node_line;
            for (int indexNodes = 0; indexNodes < nodes_length; indexNodes++)
            {
                node_line = this.Nodes[indexNodes];
                if (processName_level_dict.ContainsKey(node_line.Name))
                {
                    node_line.Level = processName_level_dict[node_line.Name];
                }
            }
        }

        public void Set_process_level_if_index_is_rigth(int nodeIndex, int process_level)
        {
            NetworkNode_line_class node_line = Nodes[nodeIndex];
            if (node_line.NW_index != nodeIndex)
            {
                throw new Exception("Nodes not sorted properly");
            }
            else
            {
                node_line.Level = process_level;
            }
        }

        public void Set_level_for_all_nodes(int level)
        {
            int nodes_length = this.Nodes_length;
            NetworkNode_line_class node_line;
            for (int indexNode = 0; indexNode < nodes_length; indexNode++)
            {
                node_line = this.Nodes[indexNode];
                node_line.Level = level;
            }
        }
        private string Get_name_as_name_plus_setNos(NetworkNode_line_class node_line)
        {
            int setNos_length = node_line.Sets_that_contain_nodes.Length;
            string name = (string)node_line.Name.Clone();
            if (setNos_length > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                if (setNos_length == 1) { sb.AppendFormat("{0} - Set:", node_line.Name); }
                else { sb.AppendFormat("{0} - Sets:", node_line.Name); }
                for (int indexSet = 0; indexSet < setNos_length; indexSet++)
                {
                    if (indexSet != 0) { sb.AppendFormat(", "); }
                    sb.AppendFormat(node_line.Sets_that_contain_nodes[indexSet].ToString());
                }
                name = sb.ToString();
            }
            return name;
        }
        #endregion


        #region Generate node colors
        public yed_node_color_line_class[] Get_yED_node_colors_based_on_level(Ontology_type_enum ontology)
        {
            NetworkNode_line_class node_line;
            int nodes_length = this.Nodes_length;

            yed_node_color_line_class[] yED_node_color_lines = new yed_node_color_line_class[nodes_length];
            yed_node_color_line_class new_yED_node_color_line;
            Color_specification_line_class color_specification_line;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = this.Nodes[indexN];
                new_yED_node_color_line = new yed_node_color_line_class();
                new_yED_node_color_line.NodeName = (string)node_line.Name.Clone();
                color_specification_line = new Color_specification_line_class();
                color_specification_line.Size = 1F;
                color_specification_line.Fill_color = Mbc_level_color_class.Get_node_color_for_indicated_level(ontology, node_line.Level);
                new_yED_node_color_line.Color_specifications = new Color_specification_line_class[] { color_specification_line };
                yED_node_color_lines[indexN] = new_yED_node_color_line;
            }
            return yED_node_color_lines;
        }

        public yed_node_color_line_class[] Get_yED_node_colors_based_on_sets_if_not_indicated_different_in_dictionary(Dictionary<string, List<Color_specification_line_class>> nodeLabel_colorSpecifications_dict)
        {
            NetworkNode_line_class node_line;
            int nodes_length = this.Nodes_length;

            yed_node_color_line_class[] yED_node_color_lines = new yed_node_color_line_class[nodes_length];
            yed_node_color_line_class new_yED_node_color_line;
            int current_colors_length;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = this.Nodes[indexN];
                new_yED_node_color_line = new yed_node_color_line_class();
                new_yED_node_color_line.NodeName = (string)node_line.Name.Clone();
                if (!nodeLabel_colorSpecifications_dict.ContainsKey(new_yED_node_color_line.NodeName))
                {
                    throw new Exception();
                }
                current_colors_length = nodeLabel_colorSpecifications_dict[new_yED_node_color_line.NodeName].Count;
                new_yED_node_color_line.Color_specifications = new Color_specification_line_class[current_colors_length];
                for (int indexC = 0; indexC < current_colors_length; indexC++)
                {
                    new_yED_node_color_line.Color_specifications[indexC] = nodeLabel_colorSpecifications_dict[new_yED_node_color_line.NodeName][indexC].Deep_copy();
                }
                yED_node_color_lines[indexN] = new_yED_node_color_line;
            }
            return yED_node_color_lines;
        }
        #endregion

        #region Keep node lines
        public void Keep_only_input_nodeNames_and_reindex(string[] input_node_names)
        {
            if (input_node_names.Length == 0) { throw new Exception("no nodes"); }
            Dictionary<string, bool> keepNodeNames_dict = new Dictionary<string, bool>();
            foreach (string input_node_name in input_node_names)
            {
                if (!keepNodeNames_dict.ContainsKey(input_node_name))
                { keepNodeNames_dict.Add(input_node_name, true); }
            }
            int this_length = Nodes_length;
            NetworkNode_line_class node_line;
            List<NetworkNode_line_class> keep_nodes = new List<NetworkNode_line_class>();
            for (int indexThis = 0; indexThis < this_length; indexThis++)
            {
                node_line = Nodes[indexThis];
                if (keepNodeNames_dict.ContainsKey(node_line.Name))
                {
                    keep_nodes.Add(node_line);
                }
            }
            Nodes = keep_nodes.ToArray();
            Reindex_nodes_and_set_index_old();
        }
        #endregion

        #region Direction
        public void Reverse_direction()
        {
            switch (Direction)
            {
                case Ontology_direction_enum.Child_parent:
                    Direction = Ontology_direction_enum.Parent_child;
                    break;
                case Ontology_direction_enum.Parent_child:
                    Direction = Ontology_direction_enum.Child_parent;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Generate
        private NetworkNode_line_class[] Get_new_nodes_with_indexes_above_max_index(Add_node_line_class[] add_nodes)
        {
            int add_index = Get_max_nw_index();
            add_nodes = Add_node_line_class.Order_by_standard_way(add_nodes);
            Add_node_line_class add_node_line;
            int add_nodes_length = add_nodes.Length;
            List<NetworkNode_line_class> new_nodes = new List<NetworkNode_line_class>();
            NetworkNode_line_class new_node_line;
            for (int indexN = 0; indexN < add_nodes_length; indexN++)
            {
                add_node_line = add_nodes[indexN];
                if ((indexN == 0) || (!add_node_line.Equal_in_standard_way(add_nodes[indexN - 1])))
                {
                    add_index++;
                    new_node_line = new NetworkNode_line_class();
                    new_node_line.Id = (string)add_node_line.Id.Clone();
                    new_node_line.Name = (string)add_node_line.Name.Clone();
                    new_node_line.NW_index = add_index;
                    new_node_line.Namespace_type = add_node_line.Ontology_namespace;
                    new_node_line.Size = add_node_line.Size;
                    new_node_line.Level = add_node_line.Level;
                    new_node_line.Depth = add_node_line.Depth;
                    new_nodes.Add(new_node_line);
                }
                else
                {
                    throw new Exception();
                }
            }
            return new_nodes.ToArray();
        }

        private NetworkNode_line_class[] Get_new_nodes_with_indexes_above_max_index(string[] add_nodeNames)
        {
            int add_index = Get_max_nw_index();
            add_nodeNames = add_nodeNames.Distinct().OrderBy(l => l).ToArray();
            string add_nodeName;
            int add_nodeNames_length = add_nodeNames.Length;
            List<NetworkNode_line_class> new_nodes = new List<NetworkNode_line_class>();
            NetworkNode_line_class new_node_line;
            for (int indexN = 0; indexN < add_nodeNames_length; indexN++)
            {
                add_nodeName = add_nodeNames[indexN];
                add_index++;
                new_node_line = new NetworkNode_line_class();
                new_node_line.Id = "no id";
                new_node_line.Name = (string)add_nodeName.Clone();
                new_node_line.NW_index = add_index;
                new_nodes.Add(new_node_line);
            }
            return new_nodes.ToArray();
        }

        private void Add_nodes(NetworkNode_line_class[] add_nodes)
        {
            int add_length = add_nodes.Length;
            int this_length = this.Nodes_length;
            int new_length = add_length + this_length;
            NetworkNode_line_class[] new_nodes = new NetworkNode_line_class[new_length];
            int indexNew = -1;
            for (int indexThis = 0; indexThis < this_length; indexThis++)
            {
                indexNew++;
                new_nodes[indexNew] = this.Nodes[indexThis];
            }
            for (int indexAdd = 0; indexAdd < add_length; indexAdd++)
            {
                indexNew++;
                new_nodes[indexNew] = add_nodes[indexAdd].Deep_copy();
            }
            Nodes = new_nodes;
        }

        private void Remove_duplicates()
        {
            Nodes = NetworkNode_line_class.Order_in_standard_way(Nodes);
            int nodes_length = Nodes_length;
            NetworkNode_line_class kept_node_line;
            List<NetworkNode_line_class> kept_nodes_list = new List<NetworkNode_line_class>();
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                kept_node_line = this.Nodes[indexN];
                if ((indexN == 0) || (!kept_node_line.Equal_in_standard_way(this.Nodes[indexN - 1])))
                {
                    kept_nodes_list.Add(kept_node_line);
                }
                else
                {
                    //throw new Exception();
                }
            }
            Nodes = kept_nodes_list.ToArray();
        }

        public void Replace_old_scpNames_by_new_scpNames(Dictionary<string, string> oldScp_to_newScp_dict)
        {
            Dictionary<string, string> newScp_from_oldScp_dict = Dictionary_class.Reverse_dictionary(oldScp_to_newScp_dict);
            int nodes_length = this.Nodes.Length;
            NetworkNode_line_class node_line;
            for (int indexN=0; indexN<nodes_length; indexN++)
            {
                node_line = this.Nodes[indexN];
                if (newScp_from_oldScp_dict.ContainsKey(node_line.Name)) { throw new Exception(); }
                if (oldScp_to_newScp_dict.ContainsKey(node_line.Name))
                {
                    node_line.Name = oldScp_to_newScp_dict[node_line.Name];
                }
            }
        }
        public void Reindex_nodes_and_set_index_old()
        {
            Nodes = NetworkNode_line_class.Order_in_standard_way(Nodes);
            NetworkNode_line_class node_line;
            int nodes_length = Nodes_length;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes[indexN];
                node_line.NW_index_old = node_line.NW_index;
                node_line.NW_index = indexN;
            }
            Index_change_adopted = false;
        }

        public NetworkNode_class Merge_this_nodes_with_other_nodes_and_get_new_indexes_of_other_UN(NetworkNode_class inputOther)
        {
            NetworkNode_class other = inputOther.Deep_copy();
            this.Order_by_name();
            other.Order_by_name();
            int this_length = this.Nodes.Length;
            int indexThis = 0;
            int other_length = other.Nodes.Length;
            int indexOther = 0;
            int stringCompare = 0;
            List<NetworkNode_line_class> mergedUN = new List<NetworkNode_line_class>();
            NetworkNode_line_class this_line = new NetworkNode_line_class();
            NetworkNode_line_class other_line = new NetworkNode_line_class();
            int overlapping_nodes_count = 0;
            int indexOtherNew = this_length - 1;
            List<NetworkNodes_set_line_class> new_set_list = new List<NetworkNodes_set_line_class>();
            while ((indexThis < this_length) || (indexOther < other_length))
            {
                if ((indexThis < this_length) && (indexOther < other_length))
                {
                    this_line = this.Nodes[indexThis];
                    other_line = other.Nodes[indexOther];
                    this_line.NW_index_old = this_line.NW_index;
                    stringCompare = other_line.Name.CompareTo(this_line.Name);
                }
                else if (indexThis < this_length)
                {
                    this_line = this.Nodes[indexThis];
                    this_line.NW_index_old = this_line.NW_index;
                    stringCompare = 2;
                }
                else // if (indexOther < other_count)
                {
                    other_line = other.Nodes[indexOther];
                    stringCompare = -2;
                }
                if (stringCompare < 0) //other_line is not in this.un
                {
                    indexOtherNew++;
                    other_line.NW_index_old = other_line.NW_index;
                    other_line.NW_index = indexOtherNew;
                    NetworkNode_line_class newOtherLine = other_line.Deep_copy();
                    newOtherLine.NW_index = indexOtherNew;
                    newOtherLine.NW_index_old = indexOtherNew;
                    mergedUN.Add(newOtherLine);
                    indexOther++;
                }
                else if (stringCompare == 0)
                {
                    new_set_list.Clear();
                    new_set_list.AddRange(this_line.Sets_that_contain_nodes);
                    new_set_list.AddRange(other_line.Sets_that_contain_nodes);
                    this_line.Sets_that_contain_nodes = new_set_list.ToArray();
                    mergedUN.Add(this_line);
                    other_line.NW_index_old = other_line.NW_index;
                    other_line.NW_index = this_line.NW_index;
                    overlapping_nodes_count++;
                    indexOther++;
                    indexThis++;
                }
                else // (stringCompare > 0)
                {
                    mergedUN.Add(this_line);
                    indexThis++;
                }
            }
            Nodes = mergedUN.ToArray();
            other.Index_change_adopted = false;

            return other;
        }

        private void Add_new_obo_node_lines_remvoe_duplicates_and_reindex(NetworkNode_line_class[] add_node_lines)
        {
            Add_nodes(add_node_lines);
            Remove_duplicates();
            Reindex_nodes_and_set_index_old();
        }

        public void Add_other_nodes_remove_duplicates_and_reindex(NetworkNode_class other)
        {
            Add_new_obo_node_lines_remvoe_duplicates_and_reindex(other.Nodes);
        }

        public void Add_new_nodes_remove_duplicates_and_reindex(Add_node_line_class[] input_add_nodes)
        {
            if (Correctness_check())
            {
                NetworkNode_line_class[] add_node_lines = Get_new_nodes_with_indexes_above_max_index(input_add_nodes);
                Add_new_obo_node_lines_remvoe_duplicates_and_reindex(add_node_lines);
            }
        }

        public void Add_new_nodes_remove_duplicates_and_reindex(string[] newNodes_names)
        {
            if (Correctness_check())
            {
                NetworkNode_line_class[] add_node_lines = Get_new_nodes_with_indexes_above_max_index(newNodes_names);
                Add_new_obo_node_lines_remvoe_duplicates_and_reindex(add_node_lines);
            }
        }
        #endregion

        #region Read write copy
        public NetworkNode_class Deep_copy()
        {
            NetworkNode_class copy = (NetworkNode_class)this.MemberwiseClone();
            int nodes_length = Nodes_length;
            copy.Nodes = new NetworkNode_line_class[nodes_length];
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                copy.Nodes[indexN] = this.Nodes[indexN].Deep_copy();
            }
            return copy;
        }
        #endregion
    }
}
