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
using Enrichment;
using yed_network;
using System.Drawing;
using Common_functions.Array_own;
using Common_functions.Form_tools;
using System.Windows.Forms;

namespace Network
{
    class Mbc_level_color_class
    {
        public static Color[] Get_node_colors_for_all_levels()
        {
            Color[] level_node_fill_colors = new Color[] { Color.Black, Color.DarkRed, Color.OrangeRed, Color.CornflowerBlue, Color.LimeGreen };
            return level_node_fill_colors;
        }

        public static Color Get_node_color_for_indicated_level(int level)
        {
            Color[] level_node_fill_colors = Get_node_colors_for_all_levels();
            if (level == -1)
            {
                return Color.LightGray;
            }
            else
            {
                return level_node_fill_colors[level];
            }
        }

        public static string Get_hexadecimal_color(Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        public static Color Get_node_color_for_indicated_set(string set)
        {
            int set_no = Int32.Parse(set);
            //Color_enum[] level_node_fill_colors = new Color_enum[] { Color_enum.Light_blue, Color_enum.Light_red, Color_enum.Orange, Color_enum.Blue, Color_enum.Dark_blue, Color_enum.Light_red, Color_enum.Dark_green, Color_enum.Bright_green, Color_enum.Yellow, Color_enum.White, Color_enum.Purple, Color_enum.Light_green, Color_enum.Light_gray, Color_enum.Dark_red };
            Color[] level_node_fill_colors = new Color[] { Color.MidnightBlue, Color.Blue, Color.LightBlue, Color.Orange, Color.OrangeRed, Color.LimeGreen };//, Color_enum.Dark_green, Color_enum.Bright_green, Color_enum.Yellow, Color_enum.White, Color_enum.Purple, Color_enum.Light_green, Color_enum.Light_gray, Color_enum.Dark_red };
            while (set_no >= level_node_fill_colors.Length)
            {
                set_no -= level_node_fill_colors.Length;
            }
            return level_node_fill_colors[set_no];
        }

        public static string Get_hexadeimal_color_for_indicated_level(int level)
        {
            string hexadecimal_color = Get_hexadecimal_color(Get_node_color_for_indicated_level(level));
            return hexadecimal_color;
        }

        public static string Get_hexadecimal_color_for_indicated_set(string set)
        {
            string hexadecimal_color = Get_hexadecimal_color(Get_node_color_for_indicated_set(set));
            return hexadecimal_color;
        }

        public static string Get_hexadecimal_color_for_no_set()
        {
            return Get_hexadecimal_color(Color.LightGray);
        }

    }

    class Network_target_line_class
    {
        #region Fields
        public int NW_index { get; set; }
        public Obo_interaction_type_enum Interaction_type { get; set; }
        public NWedge_type_enum Edge_type { get; set; }
        public float Width { get; set; }
        public string Label { get; set; }
        #endregion

        public Network_target_line_class()
        {
            Width = 1;
            Label = "";
            Edge_type = NWedge_type_enum.Arrow;
        }

        public Network_target_line_class(int nw_index, Obo_interaction_type_enum interaction_type) : this()
        {
            this.NW_index = nw_index;
            this.Interaction_type = interaction_type;
        }

        #region Equals, compare, order in standard way
        public bool Equals_in_standard_way(Network_target_line_class other)
        {
            bool equal = ((this.NW_index.Equals(other.NW_index))
                          && (this.Interaction_type.Equals(other.Interaction_type)));
            return equal;
        }

        public int Compare_in_standard_way(Network_target_line_class other)
        {
            int targetCompare = this.NW_index - other.NW_index;
            if (targetCompare == 0)
            {
                targetCompare = this.Interaction_type.CompareTo(other.Interaction_type);
            }
            return targetCompare;
        }

        public static Network_target_line_class[] Order_in_standard_way(Network_target_line_class[] target_lines)
        {
            return target_lines.OrderBy(l => l.NW_index).ThenBy(l => l.Interaction_type).ToArray();
        }
        #endregion

        #region Copy
        public Network_target_line_class Deep_copy()
        {
            Network_target_line_class copy = (Network_target_line_class)this.MemberwiseClone();
            copy.Label = (string)this.Label.Clone();
            return copy;
        }
        #endregion
    }

    class Network_line_class
    {
        public Network_target_line_class[] Targets { get; set; }
        public int Targets_length { get { return Targets.Length; } }

        public Network_line_class()
        {
            Targets = new Network_target_line_class[0];
        }

        public void Add_not_existing_targets_and_order_in_standard_way(params Network_target_line_class[] new_target_lines)
        {
            new_target_lines = Network_target_line_class.Order_in_standard_way(new_target_lines);
            Network_target_line_class new_target_line;
            Network_target_line_class this_target_line;
            int new_target_length = new_target_lines.Length;
            int this_length = Targets_length;
            int indexThis = 0;
            int targetCompare;

            List<Network_target_line_class> new_targets = new List<Network_target_line_class>();

            #region Get new targets that do not exist among old targets
            for (int indexNew = 0; indexNew < new_target_length; indexNew++)
            {
                new_target_line = new_target_lines[indexNew];
                if ((indexNew == 0)
                    || (!new_target_line.Equals_in_standard_way(new_target_lines[indexNew - 1])))
                {
                    targetCompare = -2;
                    while ((indexThis < this_length) && (targetCompare < 0))
                    {
                        this_target_line = this.Targets[indexThis];
                        targetCompare = this_target_line.Compare_in_standard_way(new_target_line);
                        if (targetCompare < 0)
                        {
                            indexThis++;
                        }
                    }
                    if (targetCompare != 0)
                    {
                        new_targets.Add(new_target_line);
                    }
                }
            }
            #endregion

            #region Add new targets and order
            new_targets.AddRange(this.Targets);
            this.Targets = Network_target_line_class.Order_in_standard_way(new_targets.ToArray());
            #endregion
        }

        public Network_line_class Deep_copy()
        {
            Network_line_class copy = (Network_line_class)this.MemberwiseClone();
            int targets_length = Targets.Length;
            copy.Targets = new Network_target_line_class[targets_length];
            for (int indexT = 0; indexT < targets_length; indexT++)
            {
                copy.Targets[indexT] = (Network_target_line_class)this.Targets[indexT].Deep_copy();
            }
            return copy;
        }
    }

    class Network_class
    {
        #region Fields
        public Network_line_class[] NW { get; set; }
        public NetworkNode_class Nodes { get; set; }
        public int NW_length { get { return NW.Length; } }
        #endregion

        public Network_class()
        {
            NW = new Network_line_class[0];
            Nodes = new NetworkNode_class();
        }

        public bool Check_for_edge_types()
        {
            foreach (Network_line_class nw_line in NW)
            {
                foreach (Network_target_line_class target_line in nw_line.Targets)
                {
                    if (!target_line.Edge_type.Equals(NWedge_type_enum.Dashed_line))
                    {
                    }
                }
            }
            return true;
        }

        public void Switch_all_edge_types_to_input_type(NWedge_type_enum edge)
        {
            foreach (Network_line_class nw_line in NW)
            {
                foreach (Network_target_line_class target in nw_line.Targets)
                {
                    target.Edge_type = edge;
                }
            }
        }

        public void Replace_old_scpNames_by_new_scpNames(Dictionary<string,string> oldSCP_to_newSCP_dict)
        {
            Nodes.Replace_old_scpNames_by_new_scpNames(oldSCP_to_newSCP_dict);
        }

        protected void Reindex_network_based_on_nodes_and_add_new_nw_lines_if_neccessary()
        {
            int nw_length = NW_length;

            #region Get indexOld - indexNew array
            int[] indexOld_indexNew = new int[nw_length];
            for (int indexArray = 0; indexArray < nw_length; indexArray++)
            {
                indexOld_indexNew[indexArray] = -1;
            }

            Nodes.Order_by_nw_index_old();
            int nodes_length = Nodes.Nodes_length;
            NetworkNode_line_class node_line;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes.Nodes[indexN];
                if (node_line.NW_index_old < nw_length)
                {
                    if (indexOld_indexNew[node_line.NW_index_old] != -1)
                    {
                        throw new Exception("index new has already been assigned");
                    }
                    else
                    {
                        indexOld_indexNew[node_line.NW_index_old] = node_line.NW_index;
                    }
                }
            }
            #endregion

            int new_nw_length = Nodes.Get_max_nw_index() + 1;
            Network_line_class[] new_nw = new Network_line_class[new_nw_length];
            Network_line_class source_nw_line;
            Network_target_line_class old_target_line;
            int targets_length;
            Nodes.Order_by_nw_index();
            NetworkNode_line_class source_node_line;
            NetworkNode_line_class target_node_line;
            int newIndex;
            List<Network_target_line_class> kept_target_lines = new List<Network_target_line_class>();
            for (int indexNodeNew = 0; indexNodeNew < new_nw_length; indexNodeNew++)
            {
                source_node_line = Nodes.Get_indexed_node_line_if_index_is_correct(indexNodeNew);
                if (source_node_line.NW_index_old < nw_length)
                {
                    source_nw_line = NW[source_node_line.NW_index_old];
                    targets_length = source_nw_line.Targets.Length;
                    kept_target_lines.Clear();
                    for (int indexT = 0; indexT < targets_length; indexT++)
                    {
                        old_target_line = source_nw_line.Targets[indexT];
                        newIndex = indexOld_indexNew[old_target_line.NW_index];
                        if (newIndex != -1)
                        {
                            target_node_line = Nodes.Get_indexed_node_line_if_index_is_correct(newIndex);
                            if (target_node_line.NW_index_old != old_target_line.NW_index) { throw new Exception("index old does not match"); }
                            old_target_line.NW_index = target_node_line.NW_index;
                            kept_target_lines.Add(old_target_line);
                        }
                    }
                    source_nw_line.Targets = kept_target_lines.OrderBy(l => l.NW_index).ToArray();
                    new_nw[source_node_line.NW_index] = source_nw_line;
                }
                else
                {
                    new_nw[source_node_line.NW_index] = new Network_line_class();
                }
            }
            NW = new_nw;
            Nodes.Index_change_adopted = true;
        }

        private Dictionary<string, Visualization_nw_edge_characterisation_line_class[]> Get_visualization_sourceName_nw_edge_characterisation_dictionary()
        {
            Dictionary<string, Visualization_nw_edge_characterisation_line_class[]> source_name_target_name = new Dictionary<string, Visualization_nw_edge_characterisation_line_class[]>();
            Visualization_nw_edge_characterisation_line_class new_edge_line;
            int nw_length = NW_length;
            Network_line_class nw_line;
            Network_target_line_class target_line;
            NetworkNode_line_class source_node_line;
            NetworkNode_line_class target_node_line;
            int targets_length;
            Nodes.Order_by_nw_index();
            List<Visualization_nw_edge_characterisation_line_class> nw_edge_characterisation_list = new List<Visualization_nw_edge_characterisation_line_class>();
            Visualization_nw_node_characterisation_line_class new_nw_node_characterisation_line;
            string source_name;
            for (int indexNW = 0; indexNW < nw_length; indexNW++)
            {
                nw_line = NW[indexNW];
                source_node_line = Nodes.Get_indexed_node_line_if_index_is_correct(indexNW);
                nw_edge_characterisation_list.Clear();
                targets_length = nw_line.Targets_length;
                source_name = source_node_line.Name;
                for (int indexT = 0; indexT < targets_length; indexT++)
                {
                    target_line = nw_line.Targets[indexT];
                    target_node_line = Nodes.Get_indexed_node_line_if_index_is_correct(target_line.NW_index);
                    new_edge_line = new Visualization_nw_edge_characterisation_line_class();
                    new_edge_line.Edge_width = target_line.Width;
                    new_edge_line.Edge_label = (string)target_line.Label.Clone();
                    new_edge_line.Target = (string)target_node_line.Name.Clone();
                    switch (target_line.Edge_type)
                    {
                        case NWedge_type_enum.Arrow:
                            new_edge_line.EdgeArrow_type = EdgeArrow_type_enum.Arrow;
                            break;
                        case NWedge_type_enum.Dotted_line:
                            new_edge_line.EdgeArrow_type = EdgeArrow_type_enum.Dotted_line;
                            break;
                        case NWedge_type_enum.Thick_dotted_line:
                            new_edge_line.EdgeArrow_type = EdgeArrow_type_enum.Thick_dotted_line;
                            break;
                        case NWedge_type_enum.Dashed_line:
                            new_edge_line.EdgeArrow_type = EdgeArrow_type_enum.Dashed_line;
                            break;
                        default:
                            throw new Exception();
                    }
                    nw_edge_characterisation_list.Add(new_edge_line);
                }
                new_nw_node_characterisation_line = new Visualization_nw_node_characterisation_line_class();
                new_nw_node_characterisation_line.NodeName = (string)source_node_line.Name.Clone();
                new_nw_node_characterisation_line.Level = source_node_line.Level;
                source_name_target_name.Add(source_name, nw_edge_characterisation_list.ToArray());
            }
            return source_name_target_name;
        }

        #region Generate
        private void Reindex_and_add_nw_connections_form_networkTable_lines(NetworkTable_line_class[] networkTable_lines)
        {
            Reindex_network_based_on_nodes_and_add_new_nw_lines_if_neccessary();
            int sigNW_length = networkTable_lines.Length;
            NetworkTable_line_class networkTable_line;
            Dictionary<string, int> name_nw_index_dict = Nodes.Get_name_index_dictionary();
            string source_name;
            string target_name;
            int source_index;
            int target_index;
            Network_target_line_class new_target_line;
            for (int indexSigNW = 0; indexSigNW < sigNW_length; indexSigNW++)
            {
                networkTable_line = networkTable_lines[indexSigNW];
                target_name = networkTable_line.Target;
                source_name = networkTable_line.Source;
                if ((!source_name.Equals(Global_class.Empty_entry)) && (!target_name.Equals(Global_class.Empty_entry)))
                {
                    source_index = name_nw_index_dict[source_name];
                    target_index = name_nw_index_dict[target_name];
                    new_target_line = new Network_target_line_class(target_index, Obo_interaction_type_enum.E_m_p_t_y);
                    new_target_line.Width = networkTable_line.Width;
                    new_target_line.Label = (string)networkTable_line.Edge_label.Clone();
                    new_target_line.Edge_type = networkTable_line.Edge_type;
                    NW[source_index].Add_not_existing_targets_and_order_in_standard_way(new_target_line);
                    Check_for_edge_types();
                }
            }
        }

        public void Add_from_networkTable_lines(NetworkTable_line_class[] networkTable_lines)
        {
            #region Get all nodes
            List<string> all_nodes = new List<string>();
            foreach (NetworkTable_line_class networkTable_line in networkTable_lines)
            {
                all_nodes.Add(networkTable_line.Source);
                all_nodes.Add(networkTable_line.Target);
            }
            all_nodes = all_nodes.Distinct().ToList();
            #endregion

            Nodes.Add_new_nodes_remove_duplicates_and_reindex(all_nodes.ToArray());
            Check_for_edge_types();
            Reindex_and_add_nw_connections_form_networkTable_lines(networkTable_lines);
            Check_for_edge_types();
        }

        public void Add_single_nodes(params string[] nodeNames)
        {
            Nodes.Add_new_nodes_remove_duplicates_and_reindex(nodeNames.ToArray());
            Reindex_network_based_on_nodes_and_add_new_nw_lines_if_neccessary();
        }
        #endregion

        public Dictionary<string, string[]> Get_sourceNodeName_targetNodeNames_dict()
        {
            this.Nodes.Order_by_nw_index();
            Dictionary<string, string[]> sourceNodeName_targetNodeNames_dict = new Dictionary<string, string[]>();
            int nw_length = this.NW_length;
            Network_line_class nw_line;
            int targets_length;
            NetworkNode_line_class source_node_line;
            NetworkNode_line_class target_node_line;
            Network_target_line_class target_line;
            List<string> target_list = new List<string>();
            for (int indexNW=0; indexNW<nw_length; indexNW++)
            {
                nw_line = this.NW[indexNW];
                targets_length = nw_line.Targets_length;
                target_list.Clear();
                source_node_line = this.Nodes.Get_indexed_node_line_if_index_is_correct(indexNW);
                for (int indexT=0;indexT<targets_length;indexT++)
                {
                    target_line = nw_line.Targets[indexT];
                    target_node_line = this.Nodes.Get_indexed_node_line_if_index_is_correct(target_line.NW_index);
                    target_list.Add((string)target_node_line.Name.Clone());
                }
                sourceNodeName_targetNodeNames_dict.Add((string)source_node_line.Name.Clone(),target_list.ToArray());
            }
            return sourceNodeName_targetNodeNames_dict;
        }

        #region Keep
        public void Keep_only_input_nodeIDs(string[] input_node_IDs)
        {
            Nodes.Keep_only_input_nodeIDs_and_reindex(input_node_IDs);
            Reindex_network_based_on_nodes_and_add_new_nw_lines_if_neccessary();
        }

        public void Keep_only_input_nodeNames(string[] input_node_names)
        {
            Nodes.Keep_only_input_nodeNames_and_reindex(input_node_names);
            Reindex_network_based_on_nodes_and_add_new_nw_lines_if_neccessary();
        }
        #endregion

        #region Direction
        public void Reverse_direction()
        {
            int nw_length = NW.Length;
            Network_line_class nw_line;
            Network_line_class[] reversed_nw = new Network_line_class[nw_length];
            for (int indexRevNW = 0; indexRevNW < nw_length; indexRevNW++)
            {
                reversed_nw[indexRevNW] = new Network_line_class();
            }
            Network_target_line_class target_line;
            Network_target_line_class reversed_target_line;
            int target_length;
            for (int indexNW = 0; indexNW < nw_length; indexNW++)
            {
                nw_line = NW[indexNW];
                target_length = nw_line.Targets_length;
                for (int indexT = 0; indexT < target_length; indexT++)
                {
                    target_line = nw_line.Targets[indexT];
                    reversed_target_line = new Network_target_line_class();
                    reversed_target_line.NW_index = indexNW;
                    reversed_target_line.Interaction_type = target_line.Interaction_type;
                    reversed_target_line.Edge_type = target_line.Edge_type;
                    reversed_nw[target_line.NW_index].Add_not_existing_targets_and_order_in_standard_way(reversed_target_line);
                }
            }
            Nodes.Reverse_direction();
            NW = reversed_nw;
        }

        public void Transform_into_undirected_double_network()
        {
            Network_class copy = this.Deep_copy();
            copy.Reverse_direction();
            Network_line_class this_line;
            Network_line_class copy_line;
            int nw_length = this.NW_length;
            for (int indexNW = 0; indexNW < nw_length; indexNW++)
            {
                this_line = this.NW[indexNW];
                copy_line = copy.NW[indexNW];
                this_line.Add_not_existing_targets_and_order_in_standard_way(copy_line.Targets);
            }
        }

        public void Transform_into_undirected_single_network_and_set_all_widths_to_one()
        {
            Transform_into_undirected_double_network();
            int nw_length = this.NW_length;
            Network_target_line_class current_target;
            int targets_length;

            List<Network_target_line_class> current_kept_network_target_lines = new List<Network_target_line_class>();
            Network_line_class this_line;
            for (int indexNW = 0; indexNW < nw_length; indexNW++)
            {
                this_line = this.NW[indexNW];
                targets_length = this_line.Targets_length;
                current_kept_network_target_lines.Clear();
                for (int indexT = 0; indexT < targets_length; indexT++)
                {
                    current_target = this_line.Targets[indexT];
                    if (current_target.NW_index <= indexNW)
                    {
                        current_target.Width = 1;
                        current_kept_network_target_lines.Add(current_target);
                    }
                }
                this_line.Targets = current_kept_network_target_lines.ToArray();
            }
        }
        #endregion

        #region Add significance and remove unseq
        public void Add_significance_and_remove_unsignificant_nodes(Ontology_enrichment_class onto_enrich, params string[] always_keep_nodes)
        {
            this.Nodes.Reset_and_add_new_significance(onto_enrich.Enrich);
            string[] keepNodes = this.Nodes.Get_all_ordered_nodes_with_minusLog10Pvalue_larger0();
            if (keepNodes.Length > 0)
            {
                List<string> keep = new List<string>();
                keep.AddRange(keepNodes);
                keep.AddRange(always_keep_nodes);
                this.Keep_only_input_nodeNames(keep.ToArray());
            }
        }

        public void Add_significance_without_removing_unsignificant_nodes(Ontology_enrichment_class onto_enrich)
        {
            this.Nodes.Reset_and_add_new_significance(onto_enrich.Enrich);
        }
        #endregion

        protected int[] Get_all_leaf_source_indexes()
        {
            Network_line_class nw_line;
            int nw_length = NW.Length;
            List<int> allTargetIndexes_list = new List<int>();
            for (int indexNW = 0; indexNW < nw_length; indexNW++)
            {
                nw_line = NW[indexNW];
                foreach (Network_target_line_class target_line in nw_line.Targets)
                {
                    allTargetIndexes_list.Add(target_line.NW_index);
                }
            }
            int[] allTargetIndexes = allTargetIndexes_list.Distinct().OrderBy(l => l).ToArray();

            List<int> leafSourceIndexes_list = new List<int>();
            int targetIndex;
            int targetIndexes_length = allTargetIndexes.Length;
            int indexTargetIndexes = 0;
            int intCompare;
            for (int indexNW = 0; indexNW < nw_length; indexNW++)
            {
                intCompare = -2;
                while ((indexTargetIndexes < targetIndexes_length) && (intCompare < 0))
                {
                    targetIndex = allTargetIndexes[indexTargetIndexes];
                    intCompare = targetIndex - indexNW;
                    if (intCompare < 0)
                    {
                        indexTargetIndexes++;
                    }
                }
                if (intCompare != 0)
                {
                    leafSourceIndexes_list.Add(indexNW);
                }
            }
            return leafSourceIndexes_list.ToArray();
        }

        public void Merge_this_network_with_other_network(Network_class input_otherNW)
        {
            int this_nodes_length_old = this.Nodes.Nodes.Length;
            Network_class otherNW = input_otherNW.Deep_copy();
            otherNW.Nodes = this.Nodes.Merge_this_nodes_with_other_nodes_and_get_new_indexes_of_other_UN(otherNW.Nodes);

            List<Network_target_line_class> target_nodes_list = new List<Network_target_line_class>();
            NetworkNode_line_class source_node_line;
            NetworkNode_line_class target_node_line;
            Network_target_line_class target_line;
            int targets_length;

            otherNW.Nodes.Order_by_nw_index_old();
            Dictionary<int, Network_target_line_class[]> other_nw_nodes_dict = new Dictionary<int, Network_target_line_class[]>();
            int other_nw_length = otherNW.NW_length;
            Network_line_class other_nw_line;
            for (int indexOtherOld = 0; indexOtherOld < other_nw_length; indexOtherOld++)
            {
                other_nw_line = otherNW.NW[indexOtherOld];
                source_node_line = otherNW.Nodes.Get_indexed_old_node_line_if_index_old_is_correct(indexOtherOld);
                targets_length = other_nw_line.Targets_length;
                target_nodes_list.Clear();
                for (int indexT = 0; indexT < targets_length; indexT++)
                {
                    target_line = other_nw_line.Targets[indexT];
                    target_node_line = otherNW.Nodes.Get_indexed_old_node_line_if_index_old_is_correct(target_line.NW_index);
                    target_line.NW_index = target_node_line.NW_index;
                    target_nodes_list.Add(target_line);
                }
                other_nw_nodes_dict.Add(source_node_line.NW_index, target_nodes_list.ToArray());
            }

            this.Nodes.Order_by_nw_index_old();
            Dictionary<int, Network_target_line_class[]> this_nw_nodes_dict = new Dictionary<int, Network_target_line_class[]>();
            int this_nw_length = this.NW_length;
            Network_line_class this_nw_line;
            for (int indexThisOld = 0; indexThisOld < this_nw_length; indexThisOld++)
            {
                this_nw_line = this.NW[indexThisOld];
                source_node_line = this.Nodes.Get_indexed_old_node_line_if_index_old_is_correct(indexThisOld);
                targets_length = this_nw_line.Targets_length;
                target_nodes_list.Clear();
                for (int indexT = 0; indexT < targets_length; indexT++)
                {
                    target_line = this_nw_line.Targets[indexT];
                    target_node_line = this.Nodes.Get_indexed_old_node_line_if_index_old_is_correct(target_line.NW_index);
                    target_line.NW_index = target_node_line.NW_index;
                    target_nodes_list.Add(target_line);
                }
                this_nw_nodes_dict.Add(source_node_line.NW_index, target_nodes_list.ToArray());
            }

            this.Nodes.Order_by_nw_index();
            int new_nw_length = this.Nodes.Nodes_length;
            Network_line_class[] new_nw = new Network_line_class[new_nw_length];
            for (int indexNew = 0; indexNew < new_nw_length; indexNew++)
            {
                target_nodes_list.Clear();
                if (this_nw_nodes_dict.ContainsKey(indexNew))
                {
                    target_nodes_list.AddRange(this_nw_nodes_dict[indexNew]);
                }
                if (other_nw_nodes_dict.ContainsKey(indexNew))
                {
                    target_nodes_list.AddRange(other_nw_nodes_dict[indexNew]);
                }
                new_nw[indexNew] = new Network_line_class();
                new_nw[indexNew].Targets = target_nodes_list.ToArray();
            }
            this.NW = new_nw;
            Nodes.Index_change_adopted = true;
        }

        public string[] Get_all_scps()
        {
            return Nodes.Get_all_nodeNames();
        }

        public string[] Get_all_scps_of_indicated_levels(params int[] levels)
        {
            return Nodes.Get_all_nodeNames_of_indicated_levels(levels);
        }

        public int[] Get_all_levels()
        {
            return Nodes.Get_all_levels();
        }

        #region Breadth first search
        public int[] Get_all_finalChildren_leaves_nodeIndexes()
        {
            int nw_length = this.NW_length;
            Network_line_class nw_line;
            List<int> finalChildren = new List<int>();
            for (int indexNW=0; indexNW<nw_length;indexNW++)
            {
                nw_line = this.NW[indexNW];
                if (nw_line.Targets_length==0)
                {
                    finalChildren.Add(indexNW);
                }
            }
            return finalChildren.ToArray();
        }
        public void Get_direct_neighbors_with_breadth_first_search(out int[] direct_neighbors, out int[] distances, int max_distance, params int[] seedNodeIndexes)
        {
            if (seedNodeIndexes.Length == 0)
            {
                throw new Exception("no seed nodes");
            }

            int nw_length = NW.Length;
            int[] all_nodes = new int[nw_length];
            int[] all_distances = new int[nw_length];

            //Generate and fill queue and distance arrays
            bool[] already_visited = new bool[nw_length];
            int seedNode_length = seedNodeIndexes.Length;
            for (int i = 0; i < seedNode_length; i++)
            {
                all_nodes[i] = seedNodeIndexes[i];
                all_distances[i] = 0;
                already_visited[seedNodeIndexes[i]] = true;
            }

            //Do breadth first search
            int writePointer = seedNode_length;
            int readPointer = 0;
            int actNode;
            Network_target_line_class[] target_lines;

            while ((readPointer != writePointer) && (all_distances[readPointer] + 1 <= max_distance))
            {
                actNode = all_nodes[readPointer];
                target_lines = NW[actNode].Targets;
                foreach (Network_target_line_class target_line in target_lines)
                {
                    if (!already_visited[target_line.NW_index])
                    {
                        already_visited[target_line.NW_index] = true;
                        all_nodes[writePointer] = target_line.NW_index;
                        all_distances[writePointer] = all_distances[readPointer] + 1;
                        writePointer++;
                    }
                }
                readPointer++;
            }

            int results_length = writePointer;
            direct_neighbors = new int[results_length];
            distances = new int[results_length];
            for (int indexR = 0; indexR < results_length; indexR++)
            {
                direct_neighbors[indexR] = all_nodes[indexR];
                distances[indexR] = all_distances[indexR];
            }
        }

        public void Keep_seedNodes_and_all_direct_neighbors(params string[] seedNode_names)
        {
            Nodes.Order_by_nw_index();
            int[] seedNode_indexes = Nodes.Get_nw_indexes_of_names(seedNode_names);
            int[] direct_neighbor_indexes;
            int[] distances;
            Get_direct_neighbors_with_breadth_first_search(out direct_neighbor_indexes, out distances, 99999, seedNode_indexes);
            string[] keep_nodeIDs = Nodes.Get_ids_of_indexes(direct_neighbor_indexes);
            Keep_only_input_nodeIDs(keep_nodeIDs);
        }

        public void Keep_only_nodes_with_indicated_levels(params int[] levels)
        {
            string[] keep_nodeNames = this.Nodes.Get_all_nodeNames_of_indicated_levels(levels);
            Keep_only_input_nodeNames(keep_nodeNames);
        }

        #endregion

        #region Write yed network
        public bool Write_yED_nw_in_results_directory_with_nodes_colored_by_level_and_return_if_interrupted(string subdirectory, string nw_name_without_extension, Shape_enum standard_node_shape, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            Dictionary<string, Visualization_nw_edge_characterisation_line_class[]> source_nw_edge_characterisation_dict = Get_visualization_sourceName_nw_edge_characterisation_dictionary();

            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            string complete_file_name_without_extension = global_dirFile.Results_directory + subdirectory + nw_name_without_extension;
            yED_class yed = new yED_class();
            yed.Options.Group_same_level_processes = false;
            yed.Options.NodeLabel_nodeShape_dict = new Dictionary<string, Shape_enum>();
            yed.Options.Node_shape = standard_node_shape;
            yed_node_color_line_class[] node_colors = this.Nodes.Get_yED_node_colors_based_on_level();
            bool nw_generation_interupted = yed.Write_yED_file_and_return_if_error(this.Nodes.Nodes, source_nw_edge_characterisation_dict, complete_file_name_without_extension, node_colors, error_report_label, form_default_settings, new yed_node_color_line_class[0]);
            return nw_generation_interupted;
        }

        public bool Write_yED_nw_in_results_directory_with_nodes_colored_by_set_and_return_if_interrupted(string complete_file_name_without_extension, Shape_enum standard_node_shape, Dictionary<string, List<Color>> nodeLabel_colors_dict, bool group_scps_by_level, Yed_network_node_size_determinant_enum node_size_determinant, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            Dictionary<string, Visualization_nw_edge_characterisation_line_class[]> source_nw_edge_characterisation_dict = Get_visualization_sourceName_nw_edge_characterisation_dictionary();

            yED_class yed = new yED_class();
            yed.Options.Node_size_determinant = node_size_determinant; 
            yed.Options.Group_same_level_processes = group_scps_by_level;
            yed.Options.NodeLabel_nodeShape_dict = new Dictionary<string, Shape_enum>();
            yed.Options.Node_shape = standard_node_shape;
            yed_node_color_line_class[] node_colors = this.Nodes.Get_yED_node_colors_based_on_sets_if_not_indicated_different_in_dictionary(nodeLabel_colors_dict);
            bool nw_generation_interupted = yed.Write_yED_file_and_return_if_error(this.Nodes.Nodes, source_nw_edge_characterisation_dict, complete_file_name_without_extension, node_colors, error_report_label, form_default_settings, new yed_node_color_line_class[0]);
            return nw_generation_interupted;
        }
        #endregion

        #region copy
        public Network_class Deep_copy()
        {
            Network_class copy = (Network_class)this.MemberwiseClone();
            int nw_length = NW_length;
            copy.NW = new Network_line_class[nw_length];
            for (int indexNW = 0; indexNW < nw_length; indexNW++)
            {
                copy.NW[indexNW] = this.NW[indexNW].Deep_copy();
            }
            copy.Nodes = this.Nodes.Deep_copy();
            return copy;
        }
        #endregion
    }

    ///////////////////////////////////////////////////////////////////////////////////////

    class MBCO_obo_network_class : Network_class
    {
        public Ontology_type_enum Ontology { get; private set; }
        public MBCO_obo_network_class(Ontology_type_enum ontology) : base()
        {
            this.Ontology = ontology;
        }

        public void Mark_input_nodes_as_populated(string[] inputNode_IDs)
        {
            inputNode_IDs = inputNode_IDs.Distinct().OrderBy(l => l).ToArray();
            int inputNode_IDs_length = inputNode_IDs.Length;
        }

        private Obo_networkTable_class Generate_mbco_networkTable_instance(System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            Obo_networkTable_class obo_networkTable = new Obo_networkTable_class();
            obo_networkTable.Generate_for_mbco_by_reading_obo_file(this.Ontology, error_report_label, form_default_settings);
            return obo_networkTable;
        }

        private void Add_nodes(Obo_networkTable_class mbco_networkTable)
        {
            int networkTable_length = mbco_networkTable.NetworkTable.Length;
            List<Add_node_line_class> add_nodes = new List<Add_node_line_class>();
            Add_node_line_class add_node_line;
            Obo_networkTable_line_class networkTable_line;
            Dictionary<string, string> nodeName_id_dict = new Dictionary<string, string>();
            Dictionary<string, Namespace_type_enum> nodeName_namespace_dict = new Dictionary<string, Namespace_type_enum>();
            for (int indexNetworkTable = 0; indexNetworkTable < networkTable_length; indexNetworkTable++)
            {
                networkTable_line = mbco_networkTable.NetworkTable[indexNetworkTable];
                if (!nodeName_id_dict.ContainsKey(networkTable_line.Name))
                {
                    add_node_line = new Add_node_line_class();
                    add_node_line.Id = (string)networkTable_line.Id.Clone();
                    add_node_line.Name = (string)networkTable_line.Name.Clone();
                    add_node_line.Ontology_namespace = networkTable_line.Namespace_type;
                    add_nodes.Add(add_node_line);
                    nodeName_id_dict.Add(add_node_line.Name, add_node_line.Id);
                    nodeName_namespace_dict.Add(add_node_line.Name, add_node_line.Ontology_namespace);
                }
                else if (!nodeName_id_dict[networkTable_line.Name].Equals(networkTable_line.Id)) { throw new Exception(); }
                else if (!nodeName_namespace_dict[networkTable_line.Name].Equals(networkTable_line.Namespace_type)) { throw new Exception(); }
            }
            for (int indexNetworkTable = 0; indexNetworkTable < networkTable_length; indexNetworkTable++)
            {
                networkTable_line = mbco_networkTable.NetworkTable[indexNetworkTable];
                if (!networkTable_line.Parent_name.Equals(Global_class.Empty_entry))
                {
                    if (!nodeName_id_dict.ContainsKey(networkTable_line.Parent_name))
                    {
                        switch (this.Ontology)
                        {
                            case Ontology_type_enum.Mbco_na_glucose_tm_transport_human:
                                break;
                            case Ontology_type_enum.Mbco_human:
                            case Ontology_type_enum.Mbco_mouse:
                            case Ontology_type_enum.Mbco_rat:
                            case Ontology_type_enum.Go_cc_human:
                            case Ontology_type_enum.Go_bp_human:
                            case Ontology_type_enum.Go_mf_human:
                            default:
                                throw new Exception();
                        }
                        add_node_line = new Add_node_line_class();
                        add_node_line.Id = (string)networkTable_line.Parent_id.Clone();
                        add_node_line.Name = (string)networkTable_line.Parent_name.Clone();
                        add_node_line.Ontology_namespace = networkTable_line.Namespace_type;
                        ////Only root parents will be added here, assumption that they have the same namespace as their children
                        ///this applies only, if the parent-child hierarchy was read as a spreadsheet and not as an obo file
                        add_nodes.Add(add_node_line);
                        nodeName_id_dict.Add(add_node_line.Name, add_node_line.Id);
                    }
                    else if (!nodeName_id_dict[networkTable_line.Parent_name].Equals(networkTable_line.Parent_id)) { throw new Exception(); }
                }
            }
            Nodes.Add_new_nodes_remove_duplicates_and_reindex(add_nodes.ToArray());
        }

        private void Add_nw_connections(Obo_networkTable_class mbco_networkTable)
        {
            base.Reindex_network_based_on_nodes_and_add_new_nw_lines_if_neccessary();
            int networkTable_length = mbco_networkTable.NetworkTable.Length;
            Obo_networkTable_line_class networkTable_line;
            Dictionary<string, int> id_nw_index_dict = Nodes.Get_id_index_dictionary();
            string source_id;
            string target_id;
            int source_index;
            int target_index;
            Network_target_line_class new_target_line;
            for (int indexNetworkTable = 0; indexNetworkTable < networkTable_length; indexNetworkTable++)
            {
                networkTable_line = mbco_networkTable.NetworkTable[indexNetworkTable];
                target_id = networkTable_line.Id;
                source_id = networkTable_line.Parent_id;
                if ((!source_id.Equals(Global_class.Empty_entry)) && (!target_id.Equals(Global_class.Empty_entry)))
                {
                    source_index = id_nw_index_dict[source_id];
                    target_index = id_nw_index_dict[target_id];
                    new_target_line = new Network_target_line_class(target_index, networkTable_line.Interaction_type);
                    NW[source_index].Add_not_existing_targets_and_order_in_standard_way(new_target_line);
                }
            }
        }

        public void Add_obo_networkTable(Obo_networkTable_class mbco_networkTable)
        {
            Nodes.Direction = Ontology_direction_enum.Parent_child;
            Add_nodes(mbco_networkTable);
            Add_nw_connections(mbco_networkTable);
        }

        public void Generate_by_reading_safed_obo_file(System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            Obo_networkTable_class mbco_networkTable = Generate_mbco_networkTable_instance(error_report_label, form_default_settings);
            Add_obo_networkTable(mbco_networkTable);
            if (Ontology_classification_class.Is_mbco_ontology(this.Ontology))
            {
                Set_process_level_for_mbco_ontology();
            }
            else
            {
                Set_process_level_for_non_mbco_ontologies();
            }
        }

        public void Keep_only_scps_of_selected_namespace_if_gene_ontology()
        {
            if (!Ontology_classification_class.Is_go_ontology(this.Ontology)) { throw new Exception(); }
            Namespace_type_enum keep_namespace = Namespace_type_enum.E_m_p_t_y;
            switch (this.Ontology)
            {
                case Ontology_type_enum.Go_bp_human:
                    keep_namespace = Namespace_type_enum.biological_process;
                    break;
                case Ontology_type_enum.Go_mf_human:
                    keep_namespace = Namespace_type_enum.molecular_function;
                    break;
                case Ontology_type_enum.Go_cc_human:
                    keep_namespace = Namespace_type_enum.cellular_component;
                    break;
                default:
                    throw new Exception();
            }
            string[] keep_scps = this.Nodes.Get_all_ordered_nodeNames_of_indicated_namespace(keep_namespace);
            Keep_only_input_nodeNames(keep_scps);
        }
        public string[] Get_input_scps_sorted_by_level_with_level_announcing_headlines(bool consider_all_scps, Dictionary<string,int> ownScp_level_dict, params string[] inputSCPs)
        {
            Dictionary<int,string[]> level_ownScp_dict = Dictionary_class.Reverse_dictionary_with_overlapping_values(ownScp_level_dict);
            int[] levels = Get_all_levels();
            levels = levels.OrderBy(l => l).ToArray();
            List<string> mbco_scps_list = new List<string>();
            string[] level_scps;
            string[] input_scps_in_level_scps;
            string[] own_level_scps;
            foreach (int level in levels)
            {
                if (level != 0)
                {
                    level_scps = Get_all_scps_of_indicated_levels(level);
                    if (level_ownScp_dict.ContainsKey(level))
                    {
                        own_level_scps = level_ownScp_dict[level];
                        level_scps = Overlap_class.Get_union(level_scps, own_level_scps);
                    }
                    level_scps = level_scps.OrderBy(l => l).ToArray();
                    if (!consider_all_scps) { input_scps_in_level_scps = Overlap_class.Get_intersection(level_scps, inputSCPs); }
                    else { input_scps_in_level_scps = level_scps; }
                    if (input_scps_in_level_scps.Length > 0)
                    {
                        mbco_scps_list.Add("Level " + level + " SCPs:");
                        mbco_scps_list.AddRange(input_scps_in_level_scps);
                        mbco_scps_list.Add("");
                    }
                }
            }
            return mbco_scps_list.ToArray();
        }

        public string[] Get_all_scps_sorted_by_level_with_level_announcing_headlines(Dictionary<string, int> ownScp_level_dict)
        {
            return Get_input_scps_sorted_by_level_with_level_announcing_headlines(true, ownScp_level_dict);
        }

        public string[] Get_input_scps_sorted_by_level_and_parent_scp_with_headlines_if_parent_child(bool consider_all_scps, Dictionary<string, int> ownScp_level_dict, params string[] inputSCPs)
        {
            Dictionary<int, string[]> level_ownScp_dict = Dictionary_class.Reverse_dictionary_with_overlapping_values(ownScp_level_dict);
            int[] levels = Get_all_levels();
            levels = levels.OrderBy(l => l).ToArray();
            List<string> mbco_scps_list = new List<string>();
            string[] level_scps;
            string[] children_scps;
            string[] children_scps_in_inputScps;
            string[] own_level_scps;
            string[] own_level_scps_in_inputScps;
            foreach (int level in levels)
            {
                if (level < 4)
                {
                    level_scps = Get_all_scps_of_indicated_levels(level);
                    foreach (string level_scp in level_scps)
                    {
                        children_scps = Get_all_children_if_direction_is_parent_child(level_scp);
                        children_scps = Overlap_class.Get_part_of_list1_but_not_of_list2(children_scps, level_scp);
                        if (!consider_all_scps) { children_scps_in_inputScps = Overlap_class.Get_intersection(inputSCPs, children_scps); }
                        else { children_scps_in_inputScps = children_scps; }
                        if (children_scps_in_inputScps.Length > 0)
                        {
                            mbco_scps_list.Add("Level " + (level + 1) + " children of " + level_scp);
                            children_scps_in_inputScps = children_scps_in_inputScps.OrderBy(l => l).ToArray();
                            mbco_scps_list.AddRange(children_scps_in_inputScps);
                            mbco_scps_list.Add("");
                        }
                    }
                }
                if (level_ownScp_dict.ContainsKey(level+1))
                {
                    own_level_scps = level_ownScp_dict[level+1].OrderBy(l => l).ToArray();
                    if (!consider_all_scps) { own_level_scps_in_inputScps = Overlap_class.Get_intersection(inputSCPs, own_level_scps); }
                    else { own_level_scps_in_inputScps = own_level_scps; }
                    mbco_scps_list.Add("Level " + (level + 1) + " custom SCPs");
                    mbco_scps_list.AddRange(own_level_scps_in_inputScps);
                    mbco_scps_list.Add("");
                }
            }
            return mbco_scps_list.ToArray();
        }

        public string[] Get_all_scps_sorted_by_level_and_parent_scp_with_headlines_if_parent_child(Dictionary<string, int> ownScp_level_dict)
        {
            return Get_input_scps_sorted_by_level_and_parent_scp_with_headlines_if_parent_child(true, ownScp_level_dict);
        }

        public string[] Get_all_finalChildren_leaves_if_parent_child()
        {
            if (!this.Nodes.Direction.Equals(Ontology_direction_enum.Parent_child)) { throw new Exception(); }
            int[] leave_indexes = base.Get_all_finalChildren_leaves_nodeIndexes();
            int leave_index;
            int leave_indexes_length = leave_indexes.Length;
            string[] final_children_scps = new string[leave_indexes.Length];
            NetworkNode_line_class leave_node_line;
            for (int indexLeave = 0; indexLeave < leave_indexes_length; indexLeave++)
            {
                leave_index = leave_indexes[indexLeave];
                leave_node_line = Nodes.Get_indexed_node_line_if_index_is_correct(leave_index);
                final_children_scps[indexLeave] = (string)leave_node_line.Name.Clone();
            }
            return final_children_scps;
        }
        public string[] Get_all_finalParents_leaves_if_child_parent()
        {
            if (!this.Nodes.Direction.Equals(Ontology_direction_enum.Child_parent)) { throw new Exception(); }
            int[] leave_indexes = base.Get_all_finalChildren_leaves_nodeIndexes();
            int leave_index;
            int leave_indexes_length = leave_indexes.Length;
            string[] final_parents_scps = new string[leave_indexes.Length];
            NetworkNode_line_class leave_node_line;
            for (int indexLeave = 0; indexLeave < leave_indexes_length; indexLeave++)
            {
                leave_index = leave_indexes[indexLeave];
                leave_node_line = Nodes.Get_indexed_node_line_if_index_is_correct(leave_index);
                final_parents_scps[indexLeave] = (string)leave_node_line.Name.Clone();
            }
            return final_parents_scps;
        }

        #region Change direction
        public void Transform_into_child_parent_direction()
        {
            switch (Nodes.Direction)
            {
                case Ontology_direction_enum.Child_parent:
                    break;
                case Ontology_direction_enum.Parent_child:
                    Reverse_direction();
                    break;
                default:
                    throw new Exception("is not accepted");
            }
        }

        public void Transform_into_parent_child_direction()
        {
            switch (Nodes.Direction)
            {
                case Ontology_direction_enum.Child_parent:
                    Reverse_direction();
                    break;
                case Ontology_direction_enum.Parent_child:
                    break;
                default:
                    throw new Exception("is not accepted");
            }
        }
        #endregion

        public void Set_process_level_for_mbco_ontology()
        {
            if (!Ontology_classification_class.Is_mbco_ontology(this.Ontology)) { throw new Exception(); }
            Ontology_direction_enum original_ontology_direction = Nodes.Direction;
            switch (Nodes.Direction)
            {
                case Ontology_direction_enum.Parent_child:
                    break;
                case Ontology_direction_enum.Child_parent:
                    Transform_into_parent_child_direction();
                    break;
                default:
                    throw new Exception("not accepted");
            }

            Nodes.Order_by_nw_index();
            int[] leafSourceIndexes = base.Get_all_leaf_source_indexes();
            int[] direct_neighbors;
            int[] distances;
            int nw_length = NW_length;
            base.Get_direct_neighbors_with_breadth_first_search(out direct_neighbors, out distances, nw_length, leafSourceIndexes);
            int direct_neighbors_length = direct_neighbors.Length;
            int max_distance = distances[direct_neighbors_length - 1];
            int direct_neighbor;
            int level;
            int firstIndexSameDistance = -1;
            int same_distance_length;
            int nodeIndex;
            for (int indexD = 0; indexD < direct_neighbors_length; indexD++)
            {
                direct_neighbor = direct_neighbors[indexD];
                level = distances[indexD];
                if ((indexD == 0) || (!level.Equals(distances[indexD - 1])))
                {
                    firstIndexSameDistance = indexD;
                }
                if ((indexD == direct_neighbors_length - 1) || (!level.Equals(distances[indexD + 1])))
                {
                    same_distance_length = indexD - firstIndexSameDistance + 1;
                    for (int indexInner = firstIndexSameDistance; indexInner <= indexD; indexInner++)
                    {
                        nodeIndex = direct_neighbors[indexInner];
                        Nodes.Set_process_level_if_index_is_rigth(nodeIndex, level);
                    }
                }
            }
            switch (original_ontology_direction)
            {
                case Ontology_direction_enum.Parent_child:
                    break;
                case Ontology_direction_enum.Child_parent:
                    Transform_into_child_parent_direction();
                    break;
                default:
                    throw new Exception("not accepted");
            }
        }

        private void Set_process_level_for_non_mbco_ontologies()
        {
            if (Ontology_classification_class.Is_mbco_ontology(this.Ontology)) { throw new Exception(); }
            Nodes.Set_level_for_all_nodes(Global_class.ProcessLevel_for_all_non_MBCO_SCPs);
        }

        public string[] Get_parent_names_and_ids_if_direction_is_child_parent(out string[] parent_processIDs, params string[] child_names)
        {
            if (Nodes.Direction != Ontology_direction_enum.Child_parent)
            {
                throw new Exception();
            }
            int[] nw_child_indexes = Nodes.Get_nw_indexes_of_names(child_names);
            int[] neighbor_indexes;
            int[] distances;
            Get_direct_neighbors_with_breadth_first_search(out neighbor_indexes, out distances, 1, nw_child_indexes);
            int neighbors_length = neighbor_indexes.Length;
            List<int> parent_indexes_list = new List<int>();
            for (int indexN = 0; indexN < neighbors_length; indexN++)
            {
                if (distances[indexN] == 1)
                {
                    parent_indexes_list.Add(neighbor_indexes[indexN]);
                }
            }
            string[] parent_names = Nodes.Get_names_and_ids_of_indexes(out parent_processIDs, parent_indexes_list.ToArray());
            return parent_names;
        }

        public string[] Get_grandparent_names_if_direction_is_child_parent(params string[] child_names)
        {
            if (Nodes.Direction != Ontology_direction_enum.Child_parent)
            {
                throw new Exception();
            }
            int[] nw_child_indexes = Nodes.Get_nw_indexes_of_names(child_names);
            if (nw_child_indexes.Length != child_names.Length)
            {
                throw new Exception();
            }
            int[] ancestor_indexes;
            int[] distances;
            Get_direct_neighbors_with_breadth_first_search(out ancestor_indexes, out distances, 2, nw_child_indexes);
            List<int> grandparent_indexes = new List<int>();
            int ancestors_length = ancestor_indexes.Length;
            for (int indexA = 0; indexA < ancestors_length; indexA++)
            {
                if (distances[indexA] == 2)
                {
                    grandparent_indexes.Add(ancestor_indexes[indexA]);
                }
            }
            string[] grandparent_names = Nodes.Get_names_of_indexes(grandparent_indexes.ToArray());
            return grandparent_names;
        }

        private string[] Get_children_names_if_direction_is_parent_child(params string[] parent_names)
        {
            if (Nodes.Direction != Ontology_direction_enum.Parent_child)
            {
                throw new Exception();
            }
            int[] nw_child_indexes = Nodes.Get_nw_indexes_of_names(parent_names);
            int[] neighbor_indexes;
            int[] distances;
            Get_direct_neighbors_with_breadth_first_search(out neighbor_indexes, out distances, 1, nw_child_indexes);
            int neighbors_length = neighbor_indexes.Length;
            List<int> children_indexes_list = new List<int>();
            for (int indexN = 0; indexN < neighbors_length; indexN++)
            {
                if (distances[indexN] == 1)
                {
                    children_indexes_list.Add(neighbor_indexes[indexN]);
                }
            }
            string[] child_names = Nodes.Get_names_of_indexes(children_indexes_list.ToArray());
            return child_names;
        }

        private string[] Get_ancestor_names_if_direction_is_child_parent(params string[] offspring_names)
        {
            if (Nodes.Direction != Ontology_direction_enum.Child_parent)
            {
                throw new Exception();
            }
            List<int> all_ancestor_indexes_list = new List<int>();
            List<int> current_ancestor_indexes_list = new List<int>();
            int[] current_offspring_indexes = Nodes.Get_nw_indexes_of_names(offspring_names);
            while (current_offspring_indexes.Length > 0)
            {
                int[] neighbor_indexes;
                int[] distances;
                Get_direct_neighbors_with_breadth_first_search(out neighbor_indexes, out distances, 1, current_offspring_indexes);
                int neighbors_length = neighbor_indexes.Length;
                current_ancestor_indexes_list.Clear();
                for (int indexN = 0; indexN < neighbors_length; indexN++)
                {
                    if (distances[indexN] == 1)
                    {
                        current_ancestor_indexes_list.Add(neighbor_indexes[indexN]);
                    }
                }
                all_ancestor_indexes_list.AddRange(current_ancestor_indexes_list);
                current_offspring_indexes = current_ancestor_indexes_list.ToArray();
            }
            string[] ancestor_names = Nodes.Get_names_of_indexes(all_ancestor_indexes_list.ToArray());
            return ancestor_names;
        }

        public Dictionary<string, int> Get_processName_level_dictionary_without_setting_process_level()
        {
            Dictionary<string, int> processName_level_dict = new Dictionary<string, int>();
            int nodes_length = Nodes.Nodes_length;
            NetworkNode_line_class node_line;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes.Nodes[indexN];
                if (node_line.Level==-1) { throw new Exception(); }
                processName_level_dict.Add(node_line.Name, node_line.Level);
            }
            return processName_level_dict;
        }
        public Dictionary<string, string> Get_processID_processName_dictionary()
        {
            Dictionary<string, string> processID_processName_dict = new Dictionary<string, string>();
            int nodes_length = Nodes.Nodes_length;
            NetworkNode_line_class node_line;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes.Nodes[indexN];
                processID_processName_dict.Add(node_line.Id, node_line.Name);
            }
            return processID_processName_dict;
        }
        public Dictionary<string, string> Get_processName_processId_dictionary()
        {
            Dictionary<string, string> processName_processID_dict = new Dictionary<string, string>();
            int nodes_length = Nodes.Nodes_length;
            NetworkNode_line_class node_line;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes.Nodes[indexN];
                processName_processID_dict.Add(node_line.Name, node_line.Id);
            }
            return processName_processID_dict;
        }
        public Dictionary<string, string[]> Get_processName_siblings_dictionary_if_direction_is_parent_child()
        {
            if (Nodes.Direction != Ontology_direction_enum.Parent_child)
            {
                throw new Exception();
            }
            MBCO_obo_network_class child_parent_nw = this.Deep_copy_mbco_obo_nw();
            child_parent_nw.Transform_into_child_parent_direction();

            int nodes_length = Nodes.Nodes_length;
            NetworkNode_line_class obo_node_line;
            string processName;
            string[] parents;
            string parent;
            string[] children;
            string[] parent_processIDs;
            List<string> siblings = new List<string>();
            Dictionary<string, string[]> processName_siblings_dictionary = new Dictionary<string, string[]>();
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                obo_node_line = Nodes.Nodes[indexN];
                processName = obo_node_line.Name;
                parents = child_parent_nw.Get_parent_names_and_ids_if_direction_is_child_parent(out parent_processIDs, processName);
                if (parents.Length > 1) { throw new Exception(); }
                else if (parents.Length == 1)
                {
                    parent = parents[0];
                    children = this.Get_children_names_if_direction_is_parent_child(parent);
                    siblings.Clear();
                    foreach (string child in children)
                    {
                        if (!child.Equals(processName))
                        {
                            siblings.Add(child);
                        }
                    }
                    processName_siblings_dictionary.Add(processName, siblings.ToArray());
                }
            }
            return processName_siblings_dictionary;
        }

        public string[] Get_all_processNames_of_input_level_without_after_setting_level(int level)
        {
            return Nodes.Get_all_nodeNames_of_indicated_levels(level);
        }

        public Dictionary<string,Namespace_type_enum> Get_processName_namespace_dictionary()
        {
            return Nodes.Get_nodeName_namespace_dictionary();
        }

        public string[] Get_all_offspring_if_direction_is_parent_child(params string[] parentProcess_names)
        {
            if (!Nodes.Direction.Equals(Ontology_direction_enum.Parent_child))
            {
                throw new Exception();
            }
            int[] parentNode_indexes = Nodes.Get_nw_indexes_of_names(parentProcess_names);
            int[] childrenNode_indexes;
            int[] distances;
            int max_distance = 9999;
            if (parentNode_indexes.Length > 0)
            {
                Get_direct_neighbors_with_breadth_first_search(out childrenNode_indexes, out distances, max_distance, parentNode_indexes);
                string[] childrenProcess_names = Nodes.Get_names_of_indexes(childrenNode_indexes);
                return childrenProcess_names;
            }
            else { return new string[0]; }
        }

        public string Get_youngest_offspring_if_direction_is_parent_child(string parentProcess_name)
        {
            if (!Nodes.Direction.Equals(Ontology_direction_enum.Parent_child))
            {
                throw new Exception();
            }
            int[] parentNode_indexes = Nodes.Get_nw_indexes_of_names(parentProcess_name);
            int[] childrenNode_indexes;
            int[] distances;
            int max_distance = 9999;
            if (parentNode_indexes.Length > 0)
            {
                Get_direct_neighbors_with_breadth_first_search(out childrenNode_indexes, out distances, max_distance, parentNode_indexes);
                string[] childrenProcess_names = Nodes.Get_names_of_indexes(new int[] { childrenNode_indexes[childrenNode_indexes.Length - 1] });
                return childrenProcess_names[0];
            }
            else { return ""; }
        }

        public string[] Get_all_children_if_direction_is_parent_child(params string[] parentProcess_names)
        {
            if (!Nodes.Direction.Equals(Ontology_direction_enum.Parent_child))
            {
                throw new Exception();
            }
            int[] parentNode_indexes = Nodes.Get_nw_indexes_of_names(parentProcess_names);
            int[] childrenNode_indexes;
            int[] distances;
            Get_direct_neighbors_with_breadth_first_search(out childrenNode_indexes, out distances, 1, parentNode_indexes);
            string[] childrenProcess_names = Nodes.Get_names_of_indexes(childrenNode_indexes);
            return childrenProcess_names;
        }

        public string[] Get_all_parents_if_direction_is_child_parent(params string[] childrenProcess_names)
        {
            if (!Nodes.Direction.Equals(Ontology_direction_enum.Child_parent))
            {
                throw new Exception();
            }
            int[] childrenNode_indexes = Nodes.Get_nw_indexes_of_names(childrenProcess_names);
            if (childrenNode_indexes.Length > 0)
            {
                int[] parentNode_indexes;
                int[] distances;
                Get_direct_neighbors_with_breadth_first_search(out parentNode_indexes, out distances, 1, childrenNode_indexes);
                parentNode_indexes = Overlap_class.Get_part_of_list1_but_not_of_list2(parentNode_indexes, childrenNode_indexes);
                string[] parentProcess_names = Nodes.Get_names_of_indexes(parentNode_indexes);
                return parentProcess_names;
            }
            else { return new string[0]; }
        }

        public string[] Get_all_ancestors_if_direction_is_child_parent(params string[] childrenProcess_names)
        {
            if (!Nodes.Direction.Equals(Ontology_direction_enum.Child_parent))
            {
                throw new Exception();
            }
            int[] childrenNode_indexes = Nodes.Get_nw_indexes_of_names(childrenProcess_names);
            int[] ancestorNode_indexes;
            int[] distances;
            if (childrenNode_indexes.Length == 0) { return new string[0]; }
            else
            {
                Get_direct_neighbors_with_breadth_first_search(out ancestorNode_indexes, out distances, 99, childrenNode_indexes);
                ancestorNode_indexes = Overlap_class.Get_part_of_list1_but_not_of_list2(ancestorNode_indexes, childrenNode_indexes);
                string[] ancestorProcess_names = Nodes.Get_names_of_indexes(ancestorNode_indexes);
                return ancestorProcess_names;
            }
        }

        #region yed node colors including legend
        private yed_node_color_line_class[] Set_process_level_and_generate_node_colors_based_on_level()
        {
            List<yed_node_color_line_class> node_color_list = new List<yed_node_color_line_class>();
            yed_node_color_line_class new_node_color_line;
            Color[] level_node_fill_colors = Mbc_level_color_class.Get_node_colors_for_all_levels();
            int level_node_fill_colors_length = level_node_fill_colors.Length;
            int nodes_length = Nodes.Nodes_length;
            NetworkNode_line_class node_line;
            Color_specification_line_class color_speci_line;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                new_node_color_line = new yed_node_color_line_class();
                node_line = Nodes.Nodes[indexN];
                if (node_line.Level==-1) { throw new Exception(); }
                color_speci_line = new Color_specification_line_class();
                color_speci_line.Fill_color = Mbc_level_color_class.Get_node_color_for_indicated_level(node_line.Level);
                new_node_color_line.Color_specifications = new Color_specification_line_class[] { color_speci_line };
                new_node_color_line.NodeName = (string)node_line.Name.Clone();
                node_color_list.Add(new_node_color_line);
            }
            return node_color_list.ToArray();
        }
        #endregion

        #region copy
        public MBCO_obo_network_class Deep_copy_mbco_obo_nw()
        {
            MBCO_obo_network_class copy = (MBCO_obo_network_class)base.Deep_copy();
            copy.Ontology = this.Ontology;
            return copy;
        }
        #endregion
    }
}
