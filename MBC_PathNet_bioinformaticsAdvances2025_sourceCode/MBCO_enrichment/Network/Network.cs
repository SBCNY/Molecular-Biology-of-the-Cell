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
using Windows_forms;
using Other_ontologies_and_databases;
using MBCO;

namespace Network
{
    enum Ontology_overview_network_enum { E_m_p_t_y, Parent_child_hierarchy, Scp_interactions }

    class Mbc_level_color_class
    {
        public static Color[] Get_node_colors_for_all_levels()
        {
            Color[] level_node_fill_colors = new Color[] { Color.Black, Color.DarkRed, Color.OrangeRed, Color.CornflowerBlue, Color.LimeGreen };
            return level_node_fill_colors;
        }

        public static Color Get_node_color_for_indicated_level(Ontology_type_enum ontology, int level)
        {
            switch (ontology)
            {
                case Ontology_type_enum.Mbco:
                    Color[] level_node_fill_colors = Get_node_colors_for_all_levels();
                    if (level == -1)
                    {
                        return Color.LightGray;
                    }
                    else
                    {
                        return level_node_fill_colors[level];
                    }
                case Ontology_type_enum.Go_bp:
                case Ontology_type_enum.Go_cc:
                case Ontology_type_enum.Go_mf:
                case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                case Ontology_type_enum.Reactome:
                case Ontology_type_enum.Custom_1:
                case Ontology_type_enum.Custom_2:
                    return Color.LightSkyBlue;
                default:
                    throw new Exception();
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
            Width = Global_class.Edge_width_default;
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
            //target_lines.OrderBy(l => l.NW_index).ThenBy(l => l.Interaction_type).ToArray();
            Dictionary<int, Dictionary<Obo_interaction_type_enum, List<Network_target_line_class>>> nwIndex_interactionType_dict = new Dictionary<int, Dictionary<Obo_interaction_type_enum, List<Network_target_line_class>>>();
            Dictionary<Obo_interaction_type_enum, List<Network_target_line_class>> interactionType_dict = new Dictionary<Obo_interaction_type_enum, List<Network_target_line_class>>();
            int target_lines_length = target_lines.Length;
            Network_target_line_class target_line;
            for (int indexT=0; indexT<target_lines_length; indexT++)
            {
                target_line = target_lines[indexT];
                if (!nwIndex_interactionType_dict.ContainsKey(target_line.NW_index))
                {
                    nwIndex_interactionType_dict.Add(target_line.NW_index, new Dictionary<Obo_interaction_type_enum, List<Network_target_line_class>>());
                }
                if (!nwIndex_interactionType_dict[target_line.NW_index].ContainsKey(target_line.Interaction_type))
                {
                    nwIndex_interactionType_dict[target_line.NW_index].Add(target_line.Interaction_type, new List<Network_target_line_class>());
                }
                nwIndex_interactionType_dict[target_line.NW_index][target_line.Interaction_type].Add(target_line);
            }
            List<Network_target_line_class> ordered_lines = new List<Network_target_line_class>();
            target_lines = null;
            int[] nwIndices;
            int nwIndex;
            int nwIndices_length;
            Obo_interaction_type_enum[] interaction_types;
            Obo_interaction_type_enum interaction_type;
            int interaction_types_length;
            nwIndices = nwIndex_interactionType_dict.Keys.ToArray();
            nwIndices = nwIndices.OrderBy(l => l).ToArray();
            nwIndices_length = nwIndices.Length;
            for (int indexNwIndex = 0; indexNwIndex < nwIndices_length; indexNwIndex++)
            {
                nwIndex = nwIndices[indexNwIndex];
                interactionType_dict = nwIndex_interactionType_dict[nwIndex];
                interaction_types = interactionType_dict.Keys.ToArray();
                interaction_types = interaction_types.OrderBy(l => l).ToArray();
                interaction_types_length = interaction_types.Length;
                for (int indexInt=0; indexInt<interaction_types_length; indexInt++)
                {
                    interaction_type = interaction_types[indexInt];
                    ordered_lines.AddRange(interactionType_dict[interaction_type]);
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count!=target_lines_length) { throw new Exception(); }
                Network_target_line_class this_line;
                Network_target_line_class previous_line;
                for (int indexThis=1; indexThis<target_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (this_line.NW_index.CompareTo(previous_line.NW_index)<0) { throw new Exception(); }
                    if (  (this_line.NW_index.Equals(previous_line.NW_index))
                        &&(this_line.Interaction_type.CompareTo(previous_line.Interaction_type)<0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }

        public static Network_target_line_class[] Order_by_nwIndex(Network_target_line_class[] target_lines)
        {
            Dictionary<int, List<Network_target_line_class>> nwIndex_dict = new Dictionary<int, List<Network_target_line_class>>();
            int target_lines_length = target_lines.Length;
            Network_target_line_class target_line;
            for (int indexT = 0; indexT < target_lines_length; indexT++)
            {
                target_line = target_lines[indexT];
                if (!nwIndex_dict.ContainsKey(target_line.NW_index))
                {
                    nwIndex_dict.Add(target_line.NW_index, new List<Network_target_line_class>());
                }
                nwIndex_dict[target_line.NW_index].Add(target_line);
            }
            List<Network_target_line_class> ordered_lines = new List<Network_target_line_class>();
            target_lines = null;
            int[] nwIndices;
            int nwIndex;
            int nwIndices_length;
            nwIndices = nwIndex_dict.Keys.ToArray();
            nwIndices = nwIndices.OrderBy(l => l).ToArray();
            nwIndices_length = nwIndices.Length;
            for (int indexNwIndex = 0; indexNwIndex < nwIndices_length; indexNwIndex++)
            {
                nwIndex = nwIndices[indexNwIndex];
                ordered_lines.AddRange(nwIndex_dict[nwIndex]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != target_lines_length) { throw new Exception(); }
                Network_target_line_class this_line;
                Network_target_line_class previous_line;
                for (int indexThis = 1; indexThis < target_lines_length; indexThis++)
                {
                    this_line = ordered_lines[indexThis];
                    previous_line = ordered_lines[indexThis - 1];
                    if (this_line.NW_index.CompareTo(previous_line.NW_index) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
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
                    //source_nw_line.Targets = kept_target_lines.OrderBy(l => l.NW_index).ToArfray();
                    source_nw_line.Targets = Network_target_line_class.Order_by_nwIndex(kept_target_lines.ToArray());
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
            string[] scps;
            Dictionary<string, Dictionary<string, NWedge_type_enum>> scp_scp_dict = new Dictionary<string, Dictionary<string, NWedge_type_enum>>();
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
                    scps = new string[] { source_node_line.Name, target_node_line.Name };
                    scps = scps.OrderBy(l => l).ToArray();
                    if (!scp_scp_dict.ContainsKey(scps[0])) { scp_scp_dict.Add(scps[0], new Dictionary<string, NWedge_type_enum>()); }
                    if (!scp_scp_dict[scps[0]].ContainsKey(scps[1]))
                    {
                        scp_scp_dict[scps[0]].Add(scps[1], target_line.Edge_type);
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
                            case NWedge_type_enum.Solid_line:
                                new_edge_line.EdgeArrow_type = EdgeArrow_type_enum.Solid_line;
                                break;
                            default:
                                throw new Exception();
                        }
                        nw_edge_characterisation_list.Add(new_edge_line);
                    }
                    else if (!scp_scp_dict[scps[0]][scps[1]].Equals(target_line.Edge_type)) { throw new Exception(); }
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

        public void Transform_into_undirected_single_network_and_set_all_widths_to_default()
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
                        current_target.Width = Global_class.Edge_width_default;
                        current_kept_network_target_lines.Add(current_target);
                    }
                }
                this_line.Targets = current_kept_network_target_lines.ToArray();
            }
        }
        #endregion

        protected int[] Get_all_leaf_source_indexes()
        {
            Network_line_class nw_line;
            int nw_length = NW.Length;
            Dictionary<int, bool> targetIndex_dict = new Dictionary<int, bool>();
            for (int indexNW = 0; indexNW < nw_length; indexNW++)
            {
                nw_line = NW[indexNW];
                foreach (Network_target_line_class target_line in nw_line.Targets)
                {
                    if (!targetIndex_dict.ContainsKey(target_line.NW_index))
                    {
                        targetIndex_dict.Add(target_line.NW_index, true);
                    }
                }
            }

            List<int> leafSourceIndexes_list = new List<int>();
            for (int indexNW = 0; indexNW < nw_length; indexNW++)
            {
                if (!targetIndex_dict.ContainsKey(indexNW))
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
            return Nodes.Get_all_ordered_nodeNames();
        }

        public string[] Get_all_scps_of_indicated_levels(params int[] levels)
        {
            return Nodes.Get_all_nodeNames_of_indicated_levels(levels);
        }

        public string[] Get_all_scps_of_indicated_depths(params int[] depths)
        {
            return Nodes.Get_all_nodeNames_of_indicated_depths(depths);
        }

        public int[] Get_all_levels()
        {
            return Nodes.Get_all_levels();
        }
        public int[] Get_all_depths()
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
        
        public string[] Return_all_scps_meeting_minimum_and_maximum_size_criteria_if_specified(string[] scps, int min_scp_size, int max_scp_size)
        {
            if (  (min_scp_size>=0)
                &&(max_scp_size>=0)
                &&(min_scp_size>max_scp_size)) { throw new Exception(); }
            Dictionary<string, int> scp_size_dict = this.Nodes.Get_nodeName_size_dict();
            List<string> keep_add_scps = new List<string>();
            foreach (string scp in scps)
            {
                if (   (!scp_size_dict.ContainsKey(scp))
                    || (   ((min_scp_size <= 0) || (scp_size_dict[scp] >= min_scp_size))
                        && ((max_scp_size <= 0) || (scp_size_dict[scp] <= max_scp_size))
                       )
                   )
                {
                    keep_add_scps.Add(scp);
                }
            }
            return keep_add_scps.ToArray();
        }

        #endregion

        #region Write yed network
        private string Escape_xml(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            return input
                .Replace(" & ", " &amp; ")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;");
        }

        public bool Write_yED_nw_in_results_directory_with_nodes_colored_by_level_and_return_if_interrupted(Ontology_type_enum ontology, Ontology_overview_network_enum network_type, string subdirectory, string nw_name_without_extension, string[] legend_dataset_nodes, Graph_editor_enum graphEditor, ProgressReport_interface_class progressReport)
        {
            foreach (NetworkNode_line_class node_line in this.Nodes.Nodes)
            {
                node_line.Name = Escape_xml(node_line.Name);
            }

            Common_functions.Global_definitions.Global_directory_and_file_class global_dirFile = new Common_functions.Global_definitions.Global_directory_and_file_class();
            string text = "";
            switch (network_type)
            {
                case Ontology_overview_network_enum.Parent_child_hierarchy:
                    text = "Writing SCP hierarchy into " + global_dirFile.Results_directory + subdirectory;
                    break;
                case Ontology_overview_network_enum.Scp_interactions:
                    text = "Writing SCP interactions into " + global_dirFile.Results_directory + subdirectory;
                    break;
                default:
                    throw new Exception();
            }

            progressReport.Update_progressReport_text_and_visualization(text);

            Dictionary<string, Visualization_nw_edge_characterisation_line_class[]> source_nw_edge_characterisation_dict = Get_visualization_sourceName_nw_edge_characterisation_dictionary();

            string complete_file_name_without_extension = global_dirFile.Results_directory + subdirectory + nw_name_without_extension;
            yED_class yed = new yED_class();
            yed.Options.Graph_editor = graphEditor;
            yed.Options.Group_same_level_processes = false;
            yed.Options.Legend_dataset_node_dict.Clear();
            yed.Options.Min_label_size = 40;
            yed.Options.Max_label_size = 40;
            yed.Options.Node_size_determinant = Yed_network_node_size_determinant_enum.Uniform;
            yed.Options.Node_shape = Shape_enum.Rectangle;
            foreach (string legend_dataset_node in legend_dataset_nodes)
            {
                yed.Options.Legend_dataset_node_dict.Add(Escape_xml(legend_dataset_node), true);
            }

            yed_node_color_line_class[] node_colors = this.Nodes.Get_yED_node_colors_based_on_level(ontology);
            bool nw_generation_interupted = yed.Write_yED_file_and_return_if_error(this.Nodes.Nodes, source_nw_edge_characterisation_dict, complete_file_name_without_extension, node_colors, progressReport);

            string pathways_name = "pathways";
            if (Ontology_classification_class.Is_mbco_ontology(ontology))
            {
                pathways_name = "subcellular processes (SCPs)";
            }

            if (!nw_generation_interupted)
            {
                switch (yed.Options.Graph_editor)
                {
                    case Graph_editor_enum.yED:
                        switch (network_type)
                        {
                            case Ontology_overview_network_enum.Parent_child_hierarchy:
                                text = "Open '" + nw_name_without_extension + ".graphml' in '" + global_dirFile.Results_directory + subdirectory
                                       + "' with yED graph editor. Use layout '" + Ontology_classification_class.Get_suggested_layout_for_hierarchy_network(ontology) + "'. Switch off 'Consider node labels'. "
                                       + "Arrows point from parent to child " + pathways_name + ".";
                                break;
                            case Ontology_overview_network_enum.Scp_interactions:
                                text = "'Open " + nw_name_without_extension + ".graphml' in '" + global_dirFile.Results_directory + subdirectory
                                       + "' with yED graph editor. Use layout '" + Ontology_classification_class.Get_suggested_layout_for_scpInteraction_network(ontology) + "'. "
                                       + "Use 'Windows - Context views - Neighborhood' to explore all neighbors of a selected SCP. "
                                       + "Edge thickness increases with interaction strength. Level-2 / -3 SCPs: red / blue";
                                break;
                            default:
                                throw new Exception();
                        }
                        break;
                    case Graph_editor_enum.Cytoscape:
                        switch (network_type)
                        {
                            case Ontology_overview_network_enum.Parent_child_hierarchy:
                                text = "Import both the '" + nw_name_without_extension + ".xgmml' and the 'Cytoscape_styles.xml' file in '" + global_dirFile.Results_directory + subdirectory
                                       + "into Cytoscape. " + "Arrows point from parent to child " + pathways_name + ".";
                                break;
                            case Ontology_overview_network_enum.Scp_interactions:
                                text = "Import both the '" + nw_name_without_extension + ".xgmml' and the 'Cytoscape_styles.xml' file in '" + global_dirFile.Results_directory + subdirectory
                                       + "into Cytoscape. " + "Edge thickness increases with interaction strength. Level-2 / -3 SCPs: red / blue";
                                break;
                            default:
                                throw new Exception();
                        }
                        break;
                    default:
                        throw new Exception();
                }
                progressReport.Update_progressReport_text_and_visualization(text);
            }
            else
            {
                switch (network_type)
                {
                    case Ontology_overview_network_enum.Parent_child_hierarchy:
                        text = "Writing of hierarchical interactions failed.";
                        break;
                    case Ontology_overview_network_enum.Scp_interactions:
                        text = "Writing of SCP interactions failed.";
                        break;
                    default:
                        throw new Exception();
                }
            }
            progressReport.Update_progressReport_text_and_visualization(text);

            return nw_generation_interupted;
        }

        public bool Write_yED_nw_in_results_directory_with_nodes_colored_by_set_and_return_if_interrupted(string complete_file_name_without_extension, string[] legend_dataset_nodes, Dictionary<string, List<Color_specification_line_class>> nodeLabel_colorSpecifications_dict, bool group_scps_by_level, Yed_network_node_size_determinant_enum node_size_determinant, int max_node_diameter, int min_label_size, int max_label_size, Graph_editor_enum graphEditor, ProgressReport_interface_class progressReport)
        {
            foreach (NetworkNode_line_class node_line in this.Nodes.Nodes)
            {
                node_line.Name = Escape_xml(node_line.Name);
            }
            string[] nodes = nodeLabel_colorSpecifications_dict.Keys.ToArray();
            string escaped_node;
            foreach (string node in nodes)
            {
                escaped_node = Escape_xml(node);
                if ((!escaped_node.Equals(node)) && (!nodeLabel_colorSpecifications_dict.ContainsKey(escaped_node)))
                { nodeLabel_colorSpecifications_dict.Add(escaped_node, nodeLabel_colorSpecifications_dict[node]); }
            }

            Dictionary<string, Visualization_nw_edge_characterisation_line_class[]> source_nw_edge_characterisation_dict = Get_visualization_sourceName_nw_edge_characterisation_dictionary();
            yED_class yed = new yED_class();
            yed.Options.Graph_editor = graphEditor;
            yed.Options.Node_size_determinant = node_size_determinant;
            yed.Options.Min_label_size = min_label_size;
            yed.Options.Max_label_size = max_label_size;
            yed.Options.Group_same_level_processes = group_scps_by_level;
            yed.Options.Max_node_diameter = max_node_diameter;
            yed.Options.Min_node_diameter = 0;
            yed.Options.Legend_dataset_node_dict.Clear();
            foreach (string legend_dataset_node in legend_dataset_nodes)
            {
                yed.Options.Legend_dataset_node_dict.Add(Escape_xml(legend_dataset_node), true);
            }
            yed_node_color_line_class[] node_colors = this.Nodes.Get_yED_node_colors_based_on_sets_if_not_indicated_different_in_dictionary(nodeLabel_colorSpecifications_dict);
            bool nw_generation_interupted = yed.Write_yED_file_and_return_if_error(this.Nodes.Nodes, source_nw_edge_characterisation_dict, complete_file_name_without_extension, node_colors, progressReport);
            return nw_generation_interupted;
        }
        #endregion

        #region copy clear
        public void Clear()
        {
            this.Nodes = new NetworkNode_class();
            this.NW = new Network_line_class[0];
        }
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
        public Organism_enum Organism { get; set; }
        public SCP_hierarchy_interaction_type_enum Scp_hierarchal_interactions { get; set; }
        public MBCO_obo_network_class(Ontology_type_enum ontology, SCP_hierarchy_interaction_type_enum scp_hierarchal_interactions, Organism_enum organism) : base()
        {
            this.Ontology = ontology;
            this.Organism = organism;
            Scp_hierarchal_interactions = scp_hierarchal_interactions; 
        }
        private Obo_networkTable_class Generate_mbco_networkTable_instance_from_obo_file(ProgressReport_interface_class progressReport)
        {
            Obo_networkTable_class obo_networkTable = new Obo_networkTable_class();
            obo_networkTable.Generate_by_reading_obo_file_and_return_if_successful(this.Ontology, this.Scp_hierarchal_interactions, progressReport);
            return obo_networkTable;
        }

        private Obo_networkTable_class Generate_reactome_networkTable_instance(Organism_enum organism, ProgressReport_interface_class progressReport)
        {
            Obo_networkTable_class reactome_networkTable = new Obo_networkTable_class();
            reactome_networkTable.Read_hierarchy_networkTables_reactome(progressReport);

            Reactome_pathway_annotation_class reactome_pathways = new Reactome_pathway_annotation_class();
            reactome_pathways.Generate_by_reading(progressReport);
            reactome_pathways.Keep_only_pathways_with_selected_organism(organism);
            Dictionary<string, string> pathwayID_pathwayName_dict = reactome_pathways.Get_pathwayID_pathwayName_dict();
            reactome_networkTable.Add_pathwayNames_and_keep_only_lines_with_names(pathwayID_pathwayName_dict);
            reactome_networkTable.Add_pathwayID_to_pathway_if_duplicated_pathway_name_for_same_organism_and_check_if_only_one_organism();
            return reactome_networkTable;
        }

        private Obo_networkTable_class Generate_standardInput_networkTable_instance(ProgressReport_interface_class progressReport, Ontology_type_enum ontology)
        {
            Obo_networkTable_class standardInput_networkTable = new Obo_networkTable_class();
            standardInput_networkTable.Read_hierarchy_networkTables_standardInput(this.Ontology, progressReport);
            return standardInput_networkTable;
        }

        private Obo_networkTable_class Generate_specialMbcoDatasetsInput_networkTable_instance(ProgressReport_interface_class progressReport, Ontology_type_enum ontology)
        {
            Obo_networkTable_class standardInput_networkTable = new Obo_networkTable_class();
            standardInput_networkTable.Read_hierarchy_networkTables_specialMbcoDatasetsInput(this.Ontology, progressReport);
            return standardInput_networkTable;
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
                if (!nodeName_id_dict.ContainsKey(networkTable_line.Child_name))
                {
                    add_node_line = new Add_node_line_class();
                    add_node_line.Id = (string)networkTable_line.Child_id.Clone();
                    add_node_line.Name = (string)networkTable_line.Child_name.Clone();
                    add_node_line.Size = networkTable_line.Child_size_based_on_human_genes;
                    if (String.IsNullOrEmpty(add_node_line.Id)) { throw new Exception(); }
                    if (String.IsNullOrEmpty(add_node_line.Name)) { throw new Exception(); }
                    add_node_line.Ontology_namespace = networkTable_line.Child_namespace_type;
                    add_node_line.Level = networkTable_line.Child_level;
                    add_node_line.Depth = networkTable_line.Child_depth;
                    add_nodes.Add(add_node_line);
                    nodeName_id_dict.Add(add_node_line.Name, add_node_line.Id);
                    nodeName_namespace_dict.Add(add_node_line.Name, add_node_line.Ontology_namespace);
                }
                else if (!nodeName_id_dict[networkTable_line.Child_name].Equals(networkTable_line.Child_id))
                {
                    string id = nodeName_id_dict[networkTable_line.Child_name];
                    throw new Exception();
                }
                else if (!nodeName_namespace_dict[networkTable_line.Child_name].Equals(networkTable_line.Child_namespace_type)) { throw new Exception(); }
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
                            case Ontology_type_enum.Custom_1:
                            case Ontology_type_enum.Custom_2:
                                break;
                            case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                            case Ontology_type_enum.Mbco:
                            case Ontology_type_enum.Go_cc:
                            case Ontology_type_enum.Go_bp:
                            case Ontology_type_enum.Go_mf:
                            case Ontology_type_enum.Reactome:
                            default:
                                throw new Exception();
                        }
                        //add_node_line = new Add_node_line_class();
                        //add_node_line.Id = (string)networkTable_line.Parent_id.Clone();
                        //add_node_line.Name = (string)networkTable_line.Parent_name.Clone();
                        //add_node_line.Ontology_namespace = networkTable_line.Namespace_type;
                        //////Only root parents will be added here, assumption that they have the same namespace as their children
                        /////this applies only, if the parent-child hierarchy was read as a spreadsheet and not as an obo file
                        //add_nodes.Add(add_node_line);
                        //nodeName_id_dict.Add(add_node_line.Name, add_node_line.Id);
                    }
                    //else if (!nodeName_id_dict[networkTable_line.Parent_name].Equals(networkTable_line.Parent_id)) { throw new Exception(); }
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
                target_id = networkTable_line.Child_id;
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

        private Obo_networkTable_class Generate_by_reading_safed_spreadsheet_file_or_obo_file_private_and_return_if_finalized(ProgressReport_interface_class progressReport, out bool task_not_interrupted)
        {
            base.Clear();
            Hierarchy_networkTable_allInfo_readOptions_class networkTable_readWriteOptions = new Hierarchy_networkTable_allInfo_readOptions_class(this.Ontology, this.Organism, this.Scp_hierarchal_interactions);
            string ontology_string = Ontology_classification_class.Get_name_of_ontology(this.Ontology);
            Obo_networkTable_class mbco_networkTable = new Obo_networkTable_class();
            bool do_continue = true;
            if (!System.IO.File.Exists(networkTable_readWriteOptions.File))
            {
                progressReport.Update_progressReport_text_and_visualization("Generating scp hierarchy for " + ontology_string + " (results will be saved for faster access next time)");
                switch (this.Ontology)
                {
                    case Ontology_type_enum.Mbco:
                        mbco_networkTable = Generate_mbco_networkTable_instance_from_obo_file(progressReport);
                        break;
                    case Ontology_type_enum.Go_bp:
                    case Ontology_type_enum.Go_cc:
                    case Ontology_type_enum.Go_mf:
                        mbco_networkTable = Generate_mbco_networkTable_instance_from_obo_file(progressReport);
                        mbco_networkTable.Add_overall_parents_as_children_with_given_parent_and_add_overall_given_parent(this.Ontology);
                        break;
                    case Ontology_type_enum.Reactome:
                        Global_directory_and_file_class gdf = new Global_directory_and_file_class();
                        mbco_networkTable = Generate_reactome_networkTable_instance(this.Organism, progressReport);
                        if (mbco_networkTable.NetworkTable.Length==0)
                        {
                            mbco_networkTable = Generate_reactome_networkTable_instance(Organism_enum.Homo_sapiens, progressReport);
                        }
                        mbco_networkTable.Add_overall_parents_as_children_with_given_parent_and_add_overall_given_parent(this.Ontology);
                        break;
                    case Ontology_type_enum.Mbco_na_glucose_tm_transport:
                        mbco_networkTable = Generate_specialMbcoDatasetsInput_networkTable_instance(progressReport, this.Ontology);
                        mbco_networkTable.Add_overall_parents_as_children_with_given_parent_and_add_overall_given_parent(this.Ontology);
                        break;
                    case Ontology_type_enum.Custom_1:
                    case Ontology_type_enum.Custom_2:
                        mbco_networkTable = Generate_standardInput_networkTable_instance(progressReport, this.Ontology);
                        if (mbco_networkTable.NetworkTable.Length < 0) { do_continue = false; }
                        else
                        {
                            MBCO_association_class custom = new MBCO_association_class();
                            custom.Read_download_standardInput_gene_scp_association_and_fill_ids_with_names(this.Ontology, this.Organism, progressReport);
                            if (custom.MBCO_associations.Length == 0) { custom.Read_download_standardInput_gene_scp_association_and_fill_ids_with_names(this.Ontology, Organism_enum.Homo_sapiens, progressReport); }
                            if (custom.MBCO_associations.Length == 0) { do_continue = false; }
                            if (do_continue)
                            {
                                string[] scps = custom.Get_all_distinct_ordered_scps();
                                mbco_networkTable.Add_potentially_missing_scps_as_children_of_no_annotated_parent_if_not_background_genes(scps);
                                mbco_networkTable.Fill_ids_with_names_and_remove_duplicates();
                                mbco_networkTable.Add_overall_parents_as_children_with_given_parent_and_add_overall_given_parent(this.Ontology);
                            }
                        }
                        break;
                    default:
                        throw new Exception();
                }
                if (do_continue)
                {
                    mbco_networkTable.Add_level_and_depth(this.Ontology);
                    //mbco_networkTable.NetworkTable = mbco_networkTable.NetworkTable.OrderBy(l => l.Child_level).ThenBy(l => l.Child_name).ToArray();
                    mbco_networkTable.Order_by_childLevel_childName();
                    mbco_networkTable.Write_allInfo_networkTables(this.Ontology, this.Organism, this.Scp_hierarchal_interactions, progressReport, out bool file_written_successfully);
                    progressReport.Update_progressReport_text_and_visualization("");
                }
            }
            else
            {
                progressReport.Update_progressReport_text_and_visualization("Reading scp hierarchy for " + ontology_string);
                mbco_networkTable.Read_allInfo_networkTables(this.Ontology, this.Organism, this.Scp_hierarchal_interactions, progressReport);
                if (mbco_networkTable.NetworkTable.Length > 0)
                { 
                    progressReport.Update_progressReport_text_and_visualization("");
                }
                else
                {
                    do_continue = false;
                }
            }
            task_not_interrupted = do_continue;
            return mbco_networkTable;
        }

        public void Generate_by_reading_safed_spreadsheet_file_or_obo_file_and_return_if_finalized(ProgressReport_interface_class progressReport, out bool not_interrupted)
        {
            this.Clear();
            Obo_networkTable_class mbco_networkTable = Generate_by_reading_safed_spreadsheet_file_or_obo_file_private_and_return_if_finalized(progressReport, out not_interrupted);
            if (not_interrupted)
            {
                Add_obo_networkTable(mbco_networkTable);
            }
        }


        public void Generate_by_reading_safed_spreadsheet_file_or_obo_file_add_missing_scps_if_custom_add_human_processSizes_and_return_if_not_interrupted(ProgressReport_interface_class progressReport, out bool not_interrupted)
        {
            this.Clear();
            Obo_networkTable_class mbco_networkTable = Generate_by_reading_safed_spreadsheet_file_or_obo_file_private_and_return_if_finalized(progressReport, out not_interrupted);
            if (not_interrupted)
            {
                Global_directory_and_file_class gdf = new Global_directory_and_file_class();
                if (System.IO.File.Exists(gdf.Ontology_inputDirectory_dict[this.Ontology] + gdf.Ontology_organism_geneAssociationInputFileName_dict[this.Ontology][Organism_enum.Homo_sapiens]))
                {
                    MBCO_association_class human_association = new MBCO_association_class();
                    human_association.Generate_after_reading_safed_file_or_de_novo_and_save(this.Ontology, Organism_enum.Homo_sapiens,
                                                                                            new Dictionary<Ontology_type_enum, Dictionary<GO_hyperParameter_enum, int>>(),
                                                                                            progressReport);

                    if (!mbco_networkTable.Are_all_sizes_set())
                    { mbco_networkTable.Add_human_processSizes_and_write_populated_ontology_associations(human_association, progressReport); }
                }
                Add_obo_networkTable(mbco_networkTable);
                mbco_networkTable.Write_allInfo_networkTables(this.Ontology, this.Organism, this.Scp_hierarchal_interactions, progressReport, out bool file_written_successfully);
            }
        }

        public void Keep_only_scps_of_selected_namespace_if_gene_ontology()
        {
            if (!Ontology_classification_class.Is_go_ontology(this.Ontology)) { throw new Exception(); }
            Namespace_type_enum keep_namespace = Namespace_type_enum.E_m_p_t_y;
            switch (this.Ontology)
            {
                case Ontology_type_enum.Go_bp:
                    keep_namespace = Namespace_type_enum.Biological_process;
                    break;
                case Ontology_type_enum.Go_mf:
                    keep_namespace = Namespace_type_enum.Molecular_function;
                    break;
                case Ontology_type_enum.Go_cc:
                    keep_namespace = Namespace_type_enum.Cellular_component;
                    break;
                default:
                    throw new Exception();
            }
            string[] keep_scps = this.Nodes.Get_all_ordered_nodeNames_of_indicated_namespace(keep_namespace);
            Keep_only_input_nodeNames(keep_scps);
        }
        public string[] Get_input_scps_sorted_by_level_with_level_announcing_headlines(bool consider_all_scps, Dictionary<string, int> ownScp_level_dict, params string[] inputSCPs)
        {
            Dictionary<int, string[]> level_ownScp_dict = Dictionary_class.Reverse_dictionary_with_overlapping_values(ownScp_level_dict);
            int[] levels = Get_all_levels();
            levels = levels.OrderBy(l => l).ToArray();
            List<string> mbco_scps_list = new List<string>();
            string[] level_scps;
            string[] input_scps_in_level_scps;
            string[] own_level_scps;
            string pathways_name = Ontology_classification_class.Get_name_of_ontology_pathway(this.Ontology) + "s";
            foreach (int level in levels)
            {
                if (level != 0)
                {
                    level_scps = Get_all_scps_of_indicated_levels(level);
                    if (level_ownScp_dict.ContainsKey(level))
                    {
                        own_level_scps = level_ownScp_dict[level];
                        level_scps = Overlap_class.Get_union_of_string_arrays_keeping_the_order(level_scps, own_level_scps);
                        inputSCPs = Overlap_class.Get_union_of_string_arrays_keeping_the_order(inputSCPs, own_level_scps);
                    }
                    if (!consider_all_scps) { input_scps_in_level_scps = Overlap_class.Get_ordered_intersection(level_scps, inputSCPs); }
                    else
                    { 
                        input_scps_in_level_scps = level_scps;
                        input_scps_in_level_scps = input_scps_in_level_scps.OrderBy(l => l).ToArray();
                    }
                    if (input_scps_in_level_scps.Length > 0)
                    {
                        mbco_scps_list.Add(level + ": Level " + level + " " + pathways_name + ":");
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

        public string[] Get_input_scps_sorted_by_depth_with_depth_announcing_headlines(bool consider_all_scps, Dictionary<string, int> ownScp_depth_dict, params string[] inputSCPs)
        {
            Dictionary<int, string[]> depth_ownScp_dict = Dictionary_class.Reverse_dictionary_with_overlapping_values(ownScp_depth_dict);
            int[] depths = Get_all_depths();
            depths = depths.OrderBy(l => l).ToArray();
            List<string> mbco_scps_list = new List<string>();
            string[] depth_scps;
            string[] input_scps_in_depth_scps;
            string[] own_depth_scps;
            string pathways_name = Ontology_classification_class.Get_name_of_ontology_pathway(this.Ontology) + "s";
            foreach (int depth in depths)
            {
                if (depth != 0)
                {
                    depth_scps = Get_all_scps_of_indicated_depths(depth);
                    if (depth_ownScp_dict.ContainsKey(depth))
                    {
                        own_depth_scps = depth_ownScp_dict[depth];
                        depth_scps = Overlap_class.Get_union_of_string_arrays_keeping_the_order(depth_scps, own_depth_scps);
                        inputSCPs = Overlap_class.Get_union_of_string_arrays_keeping_the_order(inputSCPs, own_depth_scps);
                    }
                    if (!consider_all_scps) { input_scps_in_depth_scps = Overlap_class.Get_ordered_intersection(depth_scps, inputSCPs); }
                    else 
                    { 
                        input_scps_in_depth_scps = depth_scps;
                        input_scps_in_depth_scps = input_scps_in_depth_scps.OrderBy(l => l).ToArray();
                    }
                    if (input_scps_in_depth_scps.Length > 0)
                    {
                        mbco_scps_list.Add(depth + ": Depth " + depth + " " + pathways_name + ":");
                        mbco_scps_list.AddRange(input_scps_in_depth_scps);
                        mbco_scps_list.Add("");
                    }
                }
            }
            return mbco_scps_list.ToArray();
        }

        public string[] Get_all_scps_sorted_by_depth_with_depth_announcing_headlines(Dictionary<string, int> ownScp_level_dict)
        {
            return Get_input_scps_sorted_by_depth_with_depth_announcing_headlines(true, ownScp_level_dict);
        }

        private string Get_headline_for_level_and_parent_scp(int level, string level_scp)
        {
            return (level + 1) + ": '" + level_scp + "' L" + (level + 1) + " children";
        }
        private string Get_headline_for_custom_parent_scp_of_level(int level)
        {
            string pathways_name = Ontology_classification_class.Get_name_of_ontology_pathway(this.Ontology) + "s";
            return (level + 1) + ": L" + (level + 1) + "custom " + pathways_name + " (children of L" + level + ")";
        }
        public string[] Get_input_scps_sorted_by_level_and_parent_scp_with_headlines_if_parent_child(bool consider_all_scps, Dictionary<string, int> ownScp_level_dict, params string[] inputChildSCPs)
        {
            if (!Nodes.Direction.Equals(Ontology_direction_enum.Parent_child)) { throw new Exception(); }
            Dictionary<int, string[]> level_ownScp_dict = Dictionary_class.Reverse_dictionary_with_overlapping_values(ownScp_level_dict);
            int[] levels = Get_all_levels();
            levels = levels.OrderBy(l => l).ToArray();
            List<string> mbco_scps_list = new List<string>();
            string[] level_scps;
            string[] children_scps;
            string[] children_scps_in_inputScps;
            string[] own_level_scps;
            string[] own_level_scps_in_inputScps;
            Dictionary<string, string[]> parentSCP_childSCP_dict = Get_parentScp_childScp_dictionary_if_parent_child();
            foreach (int level in levels)
            {
                {
                    level_scps = Get_all_scps_of_indicated_levels(level);
                    level_scps = level_scps.OrderBy(l => l).ToArray();
                    foreach (string level_scp in level_scps)
                    {
                        children_scps = parentSCP_childSCP_dict[level_scp];
                        children_scps = Overlap_class.Get_part_of_list1_but_not_of_list2(children_scps, level_scp);
                        if (!consider_all_scps) { children_scps_in_inputScps = Overlap_class.Get_ordered_intersection(inputChildSCPs, children_scps); }
                        else 
                        { 
                            children_scps_in_inputScps = children_scps;
                            children_scps_in_inputScps = children_scps_in_inputScps.OrderBy(l => l).ToArray();
                        }
                        if (children_scps_in_inputScps.Length > 0)
                        {
                            mbco_scps_list.Add(Get_headline_for_level_and_parent_scp(level, level_scp));
                            mbco_scps_list.AddRange(children_scps_in_inputScps);
                            mbco_scps_list.Add("");
                        }
                    }
                }
                if (level_ownScp_dict.ContainsKey(level + 1))
                {
                    own_level_scps = level_ownScp_dict[level + 1].OrderBy(l => l).ToArray();
                    if (!consider_all_scps) { own_level_scps_in_inputScps = Overlap_class.Get_ordered_intersection(inputChildSCPs, own_level_scps); }
                    else { own_level_scps_in_inputScps = own_level_scps; }
                    mbco_scps_list.Add(Get_headline_for_custom_parent_scp_of_level(level));
                    mbco_scps_list.AddRange(own_level_scps_in_inputScps);
                    mbco_scps_list.Add("");
                }
            }
            return mbco_scps_list.ToArray();
        }

        public Dictionary<string, string[]> Get_parentScp_childScp_dictionary_if_parent_child()
        {
            this.Nodes.Order_by_nw_index();
            int nw_length = this.NW_length;
            Network_line_class nw_line;
            Dictionary<int, string> index_nodeName_dict = new Dictionary<int, string>();
            Dictionary<string, string[]> parentScp_childScp_dict = new Dictionary<string, string[]>();
            string[] childScps;
            int childIndex;
            int targets_length;
            for (int indexNW = 0; indexNW < nw_length; indexNW++)
            {
                nw_line = this.NW[indexNW];
                if (!index_nodeName_dict.ContainsKey(indexNW))
                {
                    index_nodeName_dict.Add(indexNW, this.Nodes.Get_names_of_indexes_without_ordering_by_index(indexNW)[0]);
                }
                targets_length = nw_line.Targets_length;
                childScps = new string[targets_length];
                for (int indexCI = 0; indexCI < targets_length; indexCI++)
                {
                    childIndex = nw_line.Targets[indexCI].NW_index;
                    if (!index_nodeName_dict.ContainsKey(childIndex))
                    {
                        index_nodeName_dict.Add(childIndex, this.Nodes.Get_names_of_indexes_without_ordering_by_index(childIndex)[0]);
                    }
                    childScps[indexCI] = index_nodeName_dict[childIndex];
                }
                parentScp_childScp_dict.Add(index_nodeName_dict[indexNW], childScps);
            }
            return parentScp_childScp_dict;
        }

        public string[] Get_input_scps_sorted_by_level_and_parent_scp_with_headlines_if_child_parent(bool consider_all_scps, Dictionary<string, int> ownScp_level_dict, params string[] inputChildSCPs)
        {
            Dictionary<int, string[]> level_ownScp_dict = Dictionary_class.Reverse_dictionary_with_overlapping_values(ownScp_level_dict);
            if (!Nodes.Direction.Equals(Ontology_direction_enum.Child_parent)) { throw new Exception(); }
            Dictionary<string, List<string>> parentSCP_childScp_dict = new Dictionary<string, List<string>>();
            Dictionary<int, List<string>> level_parentScp_dict = new Dictionary<int, List<string>>();
            List<string> mbco_scps_list = new List<string>();
            string[] current_parent_scps;
            int current_parent_level;
            this.Nodes.Order_by_nw_index();
            foreach (string childScp in inputChildSCPs)
            {
                current_parent_scps = Get_all_parents_if_direction_is_child_parent_without_ordering_nodes_by_index(childScp);
                foreach (string current_parent_scp in current_parent_scps)
                {
                    if (!parentSCP_childScp_dict.ContainsKey(current_parent_scp))
                    {
                        parentSCP_childScp_dict.Add(current_parent_scp, new List<string>());
                        current_parent_level = Nodes.Get_level_of_processName(current_parent_scp);
                        if (!level_parentScp_dict.ContainsKey(current_parent_level))
                        {
                            level_parentScp_dict.Add(current_parent_level, new List<string>());
                        }
                        level_parentScp_dict[current_parent_level].Add(current_parent_scp);
                    }
                    parentSCP_childScp_dict[current_parent_scp].Add(childScp);
                }
            }
            int[] parentLevels = level_parentScp_dict.Keys.OrderBy(l=>l).ToArray();
            string[] current_child_scps;
            foreach (int parentLevel in parentLevels)
            {
                current_parent_scps = level_parentScp_dict[parentLevel].OrderBy(l=>l).ToArray();
                current_parent_scps = current_parent_scps.OrderBy(l=>l).ToArray();
                foreach (string current_parent_scp in current_parent_scps)
                {
                    mbco_scps_list.Add(Get_headline_for_level_and_parent_scp(parentLevel, current_parent_scp));
                    current_child_scps = parentSCP_childScp_dict[current_parent_scp].OrderBy(l=>l).ToArray();
                    mbco_scps_list.AddRange(current_child_scps);
                    mbco_scps_list.Add("");
                }
                if (level_ownScp_dict.ContainsKey(parentLevel + 1))
                {
                    mbco_scps_list.Add(Get_headline_for_custom_parent_scp_of_level(parentLevel));
                    current_child_scps = level_ownScp_dict[parentLevel];
                    mbco_scps_list.AddRange(current_child_scps);
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
            Nodes.Order_by_nw_index();
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

        private string[] Get_children_names_if_direction_is_parent_child_without_ordering_nodes_by_index(params string[] parent_names)
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
            string[] child_names = Nodes.Get_names_of_indexes_without_ordering_by_index(children_indexes_list.ToArray());
            return child_names;
        }

        private string[] Get_ancestor_names_if_direction_is_child_parent_without_ordering_nodes_by_index(params string[] offspring_names)
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
            string[] ancestor_names = Nodes.Get_names_of_indexes_without_ordering_by_index(all_ancestor_indexes_list.ToArray());
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
                if (node_line.Level == -1) { throw new Exception(); }
                processName_level_dict.Add(node_line.Name, node_line.Level);
            }
            return processName_level_dict;
        }
        public Dictionary<string, int> Get_processName_depth_dictionary_without_setting_process_level()
        {
            Dictionary<string, int> processName_depth_dict = new Dictionary<string, int>();
            int nodes_length = Nodes.Nodes_length;
            NetworkNode_line_class node_line;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = Nodes.Nodes[indexN];
                if (node_line.Level == -1) { throw new Exception(); }
                processName_depth_dict.Add(node_line.Name, node_line.Depth);
            }
            return processName_depth_dict;
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

        public Dictionary<string,Namespace_type_enum> Get_processName_namespace_dictionary()
        {
            return Nodes.Get_nodeName_namespace_dictionary();
        }

        public string[] Get_all_descendents_if_direction_is_parent_child_without_ordering_nodes_by_index(params string[] parentProcess_names)
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
                string[] childrenProcess_names = Nodes.Get_names_of_indexes_without_ordering_by_index(childrenNode_indexes);
                return childrenProcess_names;
            }
            else { return new string[0]; }
        }

        public string[] Get_all_children_if_direction_is_parent_child_without_ordering_nodes_by_index(params string[] parentProcess_names)
        {
            if (!Nodes.Direction.Equals(Ontology_direction_enum.Parent_child))
            {
                throw new Exception();
            }
            int[] parentNode_indexes = Nodes.Get_nw_indexes_of_names(parentProcess_names);
            int[] childrenNode_indexes;
            int[] distances;
            Get_direct_neighbors_with_breadth_first_search(out childrenNode_indexes, out distances, 1, parentNode_indexes);
            string[] childrenProcess_names = Nodes.Get_names_of_indexes_without_ordering_by_index(childrenNode_indexes);
            return childrenProcess_names;
        }

        public string[] Get_all_parents_if_direction_is_child_parent_without_ordering_nodes_by_index(params string[] childrenProcess_names)
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
                string[] parentProcess_names = Nodes.Get_names_of_indexes_without_ordering_by_index(parentNode_indexes);
                return parentProcess_names;
            }
            else { return new string[0]; }
        }

        public string[] Get_all_ancestors_if_direction_is_child_parent_without_ordering_nodes_by_index(params string[] childrenProcess_names)
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
                string[] ancestorProcess_names = Nodes.Get_names_of_indexes_without_ordering_by_index(ancestorNode_indexes);
                return ancestorProcess_names;
            }
        }

        #region copy
        public MBCO_obo_network_class Deep_copy_mbco_obo_nw()
        {
            MBCO_obo_network_class copy = (MBCO_obo_network_class)base.Deep_copy();
            copy.Ontology = this.Ontology;
            copy.Scp_hierarchal_interactions = this.Scp_hierarchal_interactions;
            return copy;
        }
        #endregion
    }
}
