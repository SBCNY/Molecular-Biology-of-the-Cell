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
using System.Collections.Generic;
using System.Linq;
using Enrichment;
using Common_functions.Report;
using Common_functions.Global_definitions;
using yed_network;

namespace Network
{
    class Ontology_dates_class
    {
        public static string Obo_disease_download { get { return "2014June28"; } }
        public static string Obo_pathway_ontology_download { get { return "2015June11"; } }
        public static string Obo_gene_ontology_download { get { return "2013October29"; } }
    }

    public enum Obo_interaction_type_enum { E_m_p_t_y, Is_a, Part_of }
    public enum Ontology_namespace_enum { E_m_p_t_y, Disease_ontology }
    public enum Ontology_direction_enum { E_m_p_t_y, Parent_child, Child_parent }

    class Add_node_line_class
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Ontology_type_enum Ontology_type { get; set; }
        public Ontology_namespace_enum Ontology_namespace { get; set; }

        public static Add_node_line_class[] Order_by_standard_way(Add_node_line_class[] add_nodes)
        {
            add_nodes = add_nodes.OrderBy(l => l.Id).ThenBy(l => l.Name).ThenBy(l => l.Ontology_namespace).ThenBy(l => l.Ontology_type).ToArray();
            return add_nodes;
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

    class NetworkNode_line_class
    {
        #region Fields
        const string empty_entry = "E_m_p_t_y";

        public string Id { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public int NW_index { get; set; }
        public int NW_index_old { get; set; }
        public bool Populated { get; set; }
        public float MinusLog10Pvalue { get; set; }
        public int Level { get; set; }

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
            MinusLog10Pvalue = -1;
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
            return nodes.OrderBy(l => l.Name).ToArray();
        }

        #region Copy
        public NetworkNode_line_class Deep_copy()
        {
            NetworkNode_line_class node_line = (NetworkNode_line_class)this.MemberwiseClone();
            node_line.Id = (string)this.Id.Clone();
            node_line.Name = (string)this.Name.Clone();
            node_line.Name2 = (string)this.Name2.Clone();
            return node_line;
        }
        #endregion
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
            Nodes = Nodes.OrderBy(l => l.Id).ToArray();
        }

        public void Order_by_name()
        {
            Nodes = Nodes.OrderBy(l => l.Name).ToArray();
        }

        public void Order_by_nw_index_old()
        {
            Nodes = Nodes.OrderBy(l => l.NW_index_old).ToArray();
        }

        public void Order_by_nw_index()
        {
            Nodes = Nodes.OrderBy(l => l.NW_index).ToArray();
        }

        public void Order_by_level()
        {
            Nodes = Nodes.OrderBy(l => l.Level).ToArray();
        }
        #endregion

        #region Get
        public NetworkNode_line_class Get_indexed_node_line_if_index_is_correct(int indexNode)
        {
            NetworkNode_line_class node_line = Nodes[indexNode];
            if (node_line.NW_index != indexNode)
            {
                node_line = new NetworkNode_line_class();
                Report_class.Write_error_line("{0}: Get indexed node line, Indexes do not match ({1} <-> {2})", typeof(NetworkNode_line_class).Name, indexNode, node_line.NW_index);
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
            levels = levels.Distinct().OrderBy(l => l).ToArray();
            int nodes_length = Nodes_length;
            int levels_length = levels.Length;
            int indexLevel = 0;
            int levelCompare;
            List<string> nodeNames = new List<string>();
            NetworkNode_line_class node_line;
            this.Order_by_level();
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes[indexN];
                if (node_line.Level == -1)
                {
                    throw new Exception();
                }
                levelCompare = -2;
                while ((indexLevel < levels_length) && (levelCompare < 0))
                {
                    levelCompare = levels[indexLevel].CompareTo(node_line.Level);
                    if (levelCompare < 0)
                    {
                        indexLevel++;
                    }
                    else if (levelCompare == 0)
                    {
                        nodeNames.Add(node_line.Name);
                    }
                }
            }
            return nodeNames.ToArray();
        }

        public string[] Get_all_nodeNames()
        {
            NetworkNode_line_class node_line;
            this.Order_by_level();
            int nodes_length = this.Nodes_length;
            List<string> nodeNames = new List<string>();
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes[indexN];
                nodeNames.Add(node_line.Name);
            }
            return nodeNames.ToArray();

        }

        public int[] Get_all_levels()
        {
            this.Order_by_level();
            List<int> level_list = new List<int>();
            int nodes_length = this.Nodes_length;
            NetworkNode_line_class node_line;
            for (int indexNode=0; indexNode<nodes_length; indexNode++)
            {
                node_line = this.Nodes[indexNode];
                if ((indexNode==0) || (!node_line.Level.Equals(this.Nodes[indexNode-1].Level)))
                {
                    level_list.Add(node_line.Level);
                }
            }
            return level_list.ToArray();
        }

        public string[][] Get_jagged_array_of_same_level_nodesNames_with_indicated_levels(params int[] levels)
        {
            levels = levels.Distinct().OrderBy(l => l).ToArray();
            int nodes_length = Nodes_length;
            int levels_length = levels.Length;
            int indexLevel = 0;
            int levelCompare;
            string[][] level_nodesNames_array = new string[levels_length][];
            List<string> processNames_of_sameLevel = new List<string>();
            NetworkNode_line_class node_line;
            this.Order_by_level();
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes[indexN];
                if ((indexN == 0)
                    || (!node_line.Level.Equals(Nodes[indexN - 1].Level)))
                {
                    processNames_of_sameLevel.Clear();
                }
                levelCompare = -2;
                while ((indexLevel < levels_length) && (levelCompare < 0))
                {
                    levelCompare = levels[indexLevel].CompareTo(node_line.Level);
                    if (levelCompare < 0)
                    {
                        indexLevel++;
                    }
                    else if (levelCompare == 0)
                    {
                        processNames_of_sameLevel.Add(node_line.Name);
                        if ((indexN == nodes_length - 1)
                            || (!node_line.Level.Equals(Nodes[indexN + 1].Level)))
                        {
                            level_nodesNames_array[indexLevel] = processNames_of_sameLevel.OrderBy(l => l).ToArray();
                        }
                    }
                }
            }
            return level_nodesNames_array;
        }

        public string[][] Get_level_nodesNames_array()
        {
            int nodes_length = Nodes_length;
            int max_level = Get_max_level();
            string[][] level_nodesNames_array = new string[max_level + 1][];
            List<string> processNames_of_sameLevel = new List<string>();
            NetworkNode_line_class node_line;
            this.Order_by_level();
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes[indexN];
                if ((indexN == 0)
                    || (!node_line.Level.Equals(Nodes[indexN - 1].Level)))
                {
                    processNames_of_sameLevel.Clear();
                }
                processNames_of_sameLevel.Add(node_line.Name);
                if ((indexN == nodes_length - 1)
                    || (!node_line.Level.Equals(Nodes[indexN + 1].Level)))
                {
                    level_nodesNames_array[node_line.Level] = processNames_of_sameLevel.OrderBy(l => l).ToArray();
                }
            }
            return level_nodesNames_array;
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

        public int[] Get_nodes_count_per_level()
        {
            this.Order_by_level();
            NetworkNode_line_class node_line;
            int nodes_length = Nodes_length;
            int max_level = Get_max_level();
            int[] nodes_per_level = new int[max_level + 1];
            int nodes_count = 0;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes[indexN];
                if ((indexN == 0)
                    || (!node_line.Level.Equals(Nodes[indexN - 1].Level)))
                {
                    nodes_count = 0;
                }
                nodes_count++;
                if ((indexN == nodes_length - 1)
                    || (!node_line.Level.Equals(Nodes[indexN + 1].Level)))
                {
                    nodes_per_level[node_line.Level] = nodes_count;
                }
            }
            return nodes_per_level;
        }

        public string[] Get_names_of_indexes(int[] indexes)
        {
            Correctness_check();
            this.Order_by_nw_index();
            List<string> node_names = new List<string>();
            NetworkNode_line_class node_line;
            foreach (int index in indexes)
            {
                node_line = Get_indexed_node_line_if_index_is_correct(index);
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
            names = names.Distinct().OrderBy(l => l).ToArray();
            this.Order_by_name();
            string name;
            int names_length = names.Length;
            this.Order_by_name();
            int this_length = Nodes_length;
            int indexThis = 0;
            int stringCompare = -2;

            List<int> node_indexes = new List<int>();
            NetworkNode_line_class node_line;
            for (int indexName = 0; indexName < names_length; indexName++)
            {
                name = names[indexName];
                stringCompare = -2;
                while ((indexThis < this_length) && (stringCompare < 0))
                {
                    node_line = Nodes[indexThis];
                    stringCompare = node_line.Name.CompareTo(name);
                    if (stringCompare < 0)
                    {
                        indexThis++;
                    }
                    else if (stringCompare == 0)
                    {
                        node_indexes.Add(node_line.NW_index);
                    }
                }
            }
            return node_indexes.OrderBy(l => l).ToArray();
        }

        public string[] Get_ordered_ids_of_names(params string[] names)
        {
            this.Order_by_name();
            int nodes_length = this.Nodes_length;
            NetworkNode_line_class node_line;

            names = names.OrderBy(l => l).ToArray();
            string name;
            int names_length = names.Length;
            int indexNames = 0;

            int stringCompare = -2;
            List<string> node_ids_list = new List<string>();
            for (int indexNodes = 0; indexNodes < nodes_length; indexNodes++)
            {
                node_line = Nodes[indexNodes];
                stringCompare = -2;
                while ((indexNames < names_length) && (stringCompare < 0))
                {
                    name = names[indexNames];
                    stringCompare = name.CompareTo(node_line.Name);
                    if (stringCompare < 0)
                    {
                        indexNames++;
                    }
                    else if (stringCompare == 0)
                    {
                        node_ids_list.Add(node_line.Id);
                    }
                }
            }
            return node_ids_list.OrderBy(l => l).ToArray();
        }

        public string[] Get_alphabetically_ordered_distinct_names_of_all_nodes()
        {
            int nodes_length = Nodes_length;
            string[] all_names = new string[nodes_length];
            NetworkNode_line_class node_line;
            for (int indexA = 0; indexA < nodes_length; indexA++)
            {
                node_line = Nodes[indexA];
                all_names[indexA] = (string)node_line.Name.Clone();
            }
            return all_names.Distinct().OrderBy(l => l).ToArray();
        }

        public string[] Get_ordered_ids_of_level(int level)
        {
            List<string> ids_list = new List<string>();
            int nodes_length = Nodes_length;
            NetworkNode_line_class node_line;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes[indexN];
                if (node_line.Level == level)
                {
                    ids_list.Add(node_line.Id);
                }
            }
            return ids_list.OrderBy(l => l).ToArray();
        }

        public string[] Get_ordered_process_ids_of_index_if_level_matches(int[] nw_indexes, params int[] levels)
        {
            this.Order_by_nw_index();
            int nw_indexes_length = nw_indexes.Length;
            NetworkNode_line_class node_line;
            List<string> process_ids_list = new List<string>();
            for (int indexIndex = 0; indexIndex < nw_indexes_length; indexIndex++)
            {
                node_line = Get_indexed_node_line_if_index_is_correct(nw_indexes[indexIndex]);
                if (node_line.Level == -1)
                {
                    throw new Exception("levels not set");
                }
                else if (levels.Contains(node_line.Level))
                {
                    process_ids_list.Add(node_line.Id);
                }
            }
            return process_ids_list.OrderBy(l => l).ToArray();
        }
        #endregion

        #region Get dictionaries
        public Dictionary<string, int> Get_id_index_dictionary()
        {
            Dictionary<string, int> id_index = new Dictionary<string, int>();
            if (Correctness_check())
            {
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
        #endregion

        #region Set
        public void Set_processLevel_for_all_nodes_based_on_dictionary(Dictionary<string,int> processName_level_dict)
        {
            int nodes_length = this.Nodes.Length;
            NetworkNode_line_class node_line;
            for (int indexNodes=0; indexNodes<nodes_length; indexNodes++)
            {
                node_line = this.Nodes[indexNodes];
                node_line.Level = processName_level_dict[node_line.Name];
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
            for (int indexNode=0; indexNode < nodes_length; indexNode++)
            {
                node_line = this.Nodes[indexNode];
                node_line.Level = level;
            }
        }

        public void Set_nodeIDs_based_on_nodeNames(Dictionary<string, string> nodeName_nodeID_dict)
        {
            int nodes_length = this.Nodes_length;
            NetworkNode_line_class node_line;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = this.Nodes[indexN];
                if (nodeName_nodeID_dict.ContainsKey(node_line.Name))
                {
                    node_line.Id = (string)nodeName_nodeID_dict[node_line.Name].Clone();
                }
            }
        }
        #endregion

        public void Mark_input_nodeNames_as_populated_and_rest_as_unpopulated(string[] inputNode_names)
        {
            inputNode_names = inputNode_names.Distinct().OrderBy(l => l).ToArray();
            this.Order_by_name();
            string inputNode_name;
            int inputNode_names_length = inputNode_names.Length;
            int indexInput = 0;
            int nodes_length = Nodes.Length;
            NetworkNode_line_class node_line;
            this.Order_by_name();
            int stringCompare;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes[indexN];
                stringCompare = -2;
                while ((indexInput < inputNode_names_length) && (stringCompare < 0))
                {
                    inputNode_name = inputNode_names[indexInput];
                    stringCompare = inputNode_name.CompareTo(node_line.Name);
                    if (stringCompare < 0)
                    {
                        indexInput++;
                    }
                    else if (stringCompare == 0)
                    {
                        node_line.Populated = true;
                    }
                }
                if (stringCompare != 0)
                {
                    node_line.Populated = false;
                }
            }
        }

        public string[] Get_missing_inputNode_names(string[] inputNode_names)
        {
            inputNode_names = inputNode_names.Distinct().OrderBy(l => l).ToArray();
            this.Order_by_name();
            string inputNode_name;
            int inputNode_names_length = inputNode_names.Length;
            int indexNode = 0;
            int stringCompare;
            int nodes_length = Nodes_length;
            NetworkNode_line_class node_line;
            List<string> missing_node_names = new List<string>();
            for (int indexInput = 0; indexInput < inputNode_names_length; indexInput++)
            {
                inputNode_name = inputNode_names[indexInput];
                stringCompare = -2;
                while ((indexNode < nodes_length) && (stringCompare < 0))
                {
                    node_line = Nodes[indexNode];
                    stringCompare = node_line.Name.CompareTo(inputNode_name);
                    if (stringCompare < 0)
                    {
                        indexNode++;
                    }
                }
                if (stringCompare != 0)
                {
                    missing_node_names.Add(inputNode_name);
                }
            }
            return missing_node_names.ToArray();
        }

        public void Reset_and_add_new_significance(Ontology_enrichment_line_class[] onto_enrich_lines)
        {
            int nodes_length = this.Nodes_length;
            NetworkNode_line_class node_line;

            Ontology_enrichment_line_class onto_enrich_line;
            int onto_enrich_length = onto_enrich_lines.Length;
            int indexOnto = 0;
            onto_enrich_lines = onto_enrich_lines.OrderBy(l => l.Scp_name).ToArray();
            this.Order_by_name();
            int stringCompare = -2;

            for (int indexNodes = 0; indexNodes < nodes_length; indexNodes++)
            {
                node_line = this.Nodes[indexNodes];
                node_line.MinusLog10Pvalue = -1;
                stringCompare = -2;
                while ((indexOnto < onto_enrich_length) && (stringCompare < 0))
                {
                    onto_enrich_line = onto_enrich_lines[indexOnto];
                    stringCompare = onto_enrich_line.Scp_name.CompareTo(node_line.Name);
                    if (stringCompare < 0)
                    {
                        indexOnto++;
                    }
                    else if (stringCompare == 0)
                    {
                        node_line.MinusLog10Pvalue = onto_enrich_line.Minus_log10_pvalue;
                    }
                }
            }
        }

        #region Generate node colors
        private string Get_hexadecimal_nodeColor_gradient(float nodeValue, float maxValue, float minValue)
        {
            if (float.IsPositiveInfinity(nodeValue))
            {
                nodeValue = float.MaxValue;
            }
            if (float.IsPositiveInfinity(maxValue))
            {
                maxValue = float.MaxValue;
            }
            float relative_node_value = (nodeValue - minValue) / (maxValue - minValue);
            int red = -1;
            int green = -1;
            int blue = -1;
            int max_blue = 100;
            int max_green = 150;
            int max_red = 255;
            float step_size = 0.33333333333333333333333333333333F;
            if (relative_node_value < step_size)  // Blue --> green
            {
                red = 0;
                green = (int)Math.Round(relative_node_value / step_size * max_green);
                blue = max_blue - (int)Math.Round(relative_node_value / step_size * max_blue);
            }
            else if ((relative_node_value >= step_size) && (relative_node_value < 2 * step_size)) // Green --> Yellow
            {
                red = (int)Math.Round((relative_node_value - step_size) / step_size * max_red);
                green = max_green;
                blue = 0;
            }
            else   // Yellow --> red
            {
                red = max_red;
                green = max_green - (int)Math.Round((relative_node_value - 2 * step_size) / step_size * max_green);
                blue = 0;
            }
            return Math_class.Get_hexadecimal_code(red, green, blue);
        }

        private void Set_nodeColor_gradient(ref yed_node_color_line_class yED_node_color_line, float nodeValue, float maxValue, float minValue)
        {
            if (nodeValue <= 0)
            {
                yED_node_color_line.Hexadecimal_color = Math_class.Get_hexadecimal_code(150, 150, 150);
            }
            else
            {
                yED_node_color_line.Hexadecimal_color = Get_hexadecimal_nodeColor_gradient(nodeValue, maxValue, minValue);
            }
        }

        private void Get_max_and_min_minusLog10Pvalue(out float max_minusLog10Pvalue, out float min_minusLog10Pvalue)
        {
            NetworkNode_line_class node_line;
            int nodes_length = this.Nodes_length;
            max_minusLog10Pvalue = -1;
            min_minusLog10Pvalue = -1;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = this.Nodes[indexN];
                if ((max_minusLog10Pvalue == -1)
                    || (node_line.MinusLog10Pvalue > max_minusLog10Pvalue))
                {
                    max_minusLog10Pvalue = node_line.MinusLog10Pvalue;
                }
                if ((min_minusLog10Pvalue == -1)
                    || (node_line.MinusLog10Pvalue < min_minusLog10Pvalue))
                {
                    min_minusLog10Pvalue = node_line.MinusLog10Pvalue;
                }
            }
        }

        private yed_node_color_line_class Generate_legend_node(float value, float max_value, float min_value)
        {
            yed_node_color_line_class yed_node_color_line = new yed_node_color_line_class();
            yed_node_color_line.NodeName = value.ToString();
            yed_node_color_line.Hexadecimal_color = Get_hexadecimal_nodeColor_gradient(value, max_value, min_value);
            return yed_node_color_line;
        }

        public yed_node_color_line_class[] Generate_legend_visualization_of_nw_node_lines(int number_of_legend_nodes_count)
        {
            float max_minusLog10Pvalue;
            float min_minusLog10Pvalue;
            Get_max_and_min_minusLog10Pvalue(out max_minusLog10Pvalue, out min_minusLog10Pvalue);
            min_minusLog10Pvalue = 0;

            yed_node_color_line_class[] legend_node_color_lines = new yed_node_color_line_class[number_of_legend_nodes_count];
            float value;
            for (int indexN = 0; indexN < number_of_legend_nodes_count; indexN++)
            {
                value = min_minusLog10Pvalue + indexN * (max_minusLog10Pvalue - min_minusLog10Pvalue) / (number_of_legend_nodes_count - 1);
                legend_node_color_lines[indexN] = Generate_legend_node(value, max_minusLog10Pvalue, min_minusLog10Pvalue);
            }
            return legend_node_color_lines;
        }

        public yed_node_color_line_class[] Get_yED_node_colors_based_on_minusLog10Pvalues()
        {
            float maxValue = -1;
            float minValue = 0;
            NetworkNode_line_class node_line;
            int nodes_length = this.Nodes_length;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = this.Nodes[indexN];
                if ((maxValue == -1)
                    || (maxValue < node_line.MinusLog10Pvalue))
                {
                    maxValue = node_line.MinusLog10Pvalue;
                }
            }

            yed_node_color_line_class[] yED_node_color_lines = new yed_node_color_line_class[nodes_length];
            yed_node_color_line_class new_yED_node_color_line;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = this.Nodes[indexN];
                new_yED_node_color_line = new yed_node_color_line_class();
                new_yED_node_color_line.NodeName = (string)node_line.Name.Clone();
                Set_nodeColor_gradient(ref new_yED_node_color_line, node_line.MinusLog10Pvalue, maxValue, minValue);
                yED_node_color_lines[indexN] = new_yED_node_color_line;
            }
            return yED_node_color_lines;
        }

        public yed_node_color_line_class[] Get_yED_node_colors_based_on_level()
        {
            NetworkNode_line_class node_line;
            int nodes_length = this.Nodes_length;

            yed_node_color_line_class[] yED_node_color_lines = new yed_node_color_line_class[nodes_length];
            yed_node_color_line_class new_yED_node_color_line;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = this.Nodes[indexN];
                new_yED_node_color_line = new yed_node_color_line_class();
                new_yED_node_color_line.NodeName = (string)node_line.Name.Clone();
                new_yED_node_color_line.Hexadecimal_color = Mbc_level_color_class.Get_hexadeimal_color_for_indicated_level(node_line.Level);
                yED_node_color_lines[indexN] = new_yED_node_color_line;
            }
            return yED_node_color_lines;
        }
        #endregion

        public string[] Get_all_ordered_nodes_with_minusLog10Pvalue_larger0()
        {
            List<string> node_names = new List<string>();
            foreach (NetworkNode_line_class node_line in this.Nodes)
            {
                if (node_line.MinusLog10Pvalue > 0)
                {
                    node_names.Add(node_line.Name);
                }
            }
            return node_names.OrderBy(l => l).ToArray();
        }

        #region Keep node lines
        public void Keep_only_input_nodeIDs_and_reindex(string[] input_node_IDs)
        {
            input_node_IDs = input_node_IDs.Distinct().OrderBy(l => l).ToArray();
            if (input_node_IDs.Length == 0)
            {
                throw new Exception("no nodes");
            }
            string input_nodeID;
            int input_nodes_length = input_node_IDs.Length;
            int this_length = Nodes_length;
            NetworkNode_line_class node_line;
            int stringCompare;
            int indexInput = 0;
            List<NetworkNode_line_class> keep_nodes = new List<NetworkNode_line_class>();
            this.Order_by_id();
            for (int indexThis = 0; indexThis < this_length; indexThis++)
            {
                node_line = Nodes[indexThis];
                stringCompare = -2;
                while ((indexInput < input_nodes_length) && (stringCompare < 0))
                {
                    input_nodeID = input_node_IDs[indexInput];
                    stringCompare = input_nodeID.CompareTo(node_line.Name);
                    if (stringCompare < 0)
                    {
                        indexInput++;
                    }
                    else if (stringCompare == 0)
                    {
                        keep_nodes.Add(node_line);
                    }
                }
            }
            Nodes = keep_nodes.ToArray();
            Reindex_nodes_and_set_index_old();
        }

        public void Keep_only_input_nodeNames_and_reindex(string[] input_node_Names)
        {
            input_node_Names = input_node_Names.Distinct().OrderBy(l => l).ToArray();
            if (input_node_Names.Length == 0)
            {
                throw new Exception("no nodes");
            }
            string input_nodeID;
            int input_nodes_length = input_node_Names.Length;
            int this_length = Nodes_length;
            NetworkNode_line_class node_line;
            int stringCompare;
            int indexInput = 0;
            List<NetworkNode_line_class> keep_nodes = new List<NetworkNode_line_class>();
            this.Order_by_name();
            for (int indexThis = 0; indexThis < this_length; indexThis++)
            {
                node_line = Nodes[indexThis];
                stringCompare = -2;
                while ((indexInput < input_nodes_length) && (stringCompare < 0))
                {
                    input_nodeID = input_node_Names[indexInput];
                    stringCompare = input_nodeID.CompareTo(node_line.Name);
                    if (stringCompare < 0)
                    {
                        indexInput++;
                    }
                    else if (stringCompare == 0)
                    {
                        keep_nodes.Add(node_line);
                    }
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
                    throw new Exception();
                }
            }
            Nodes = kept_nodes_list.ToArray();
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
    }
}
