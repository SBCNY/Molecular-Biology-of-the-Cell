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
using System.Drawing;
using Network;
using Common_functions.Global_definitions;
using Common_functions.ReadWrite;
using Common_functions.Array_own;
using Common_functions.Form_tools;
using yed_network;

namespace Leave_out
{

    class Leave_out_line_class
    {
        public int ProcessLevel {get;set;}
        public string ProcessID { get; set; }
        public string ProcessName { get; set; }
        public string Left_out_processName { get; set; }
        public int Original_process_size { get; set; }
        public int Process_size_after_left_out { get; set; }
        public int New_symbols_count { get; set; }
        public float New_symbols_count_divided_by_original_process_size { get; set; }
        public float New_symbols_rank_increase { get; set; }
        public float New_symbols_rank_increase_divided_by_original_process_size { get; set; }
        public float Edge_weight { get; set; }

        public Leave_out_line_class Deep_copy()
        {
            Leave_out_line_class copy = (Leave_out_line_class)this.MemberwiseClone();
            copy.ProcessName = (string)this.ProcessName.Clone();
            copy.Left_out_processName = (string)this.Left_out_processName.Clone();
            return copy;
        }

    }

    class Leave_out_input_readWriteOptions_class : ReadWriteOptions_base
    {
        public Leave_out_input_readWriteOptions_class(Ontology_type_enum ontology)
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            switch (ontology)
            {
                case Ontology_type_enum.Mbco_human:
                case Ontology_type_enum.Mbco_mouse:
                case Ontology_type_enum.Mbco_rat:
                    File = global_dirFile.Complete_mbco_inferred_scp_relationships_v11_fileName;
                    break;
                default:
                    throw new Exception();
            }
            Key_propertyNames = new string[] { "ProcessLevel", "Left_out_processName", "ProcessID", "ProcessName", "Original_process_size", "Process_size_after_left_out", "New_symbols_count", "New_symbols_count_divided_by_original_process_size", "New_symbols_rank_increase", "New_symbols_rank_increase_divided_by_original_process_size" };
            Key_columnNames = Key_propertyNames;
            File_has_headline = true;
            HeadlineDelimiters = new char[] { Global_class.Tab };
            LineDelimiters = new char[] { Global_class.Tab };
            Report = ReadWrite_report_enum.Report_main;
        }
    }

    class LeaveOut_options_class
    {
        public Ontology_type_enum Ontology { get; private set; }

        public void Set_new_ontology(Ontology_type_enum ontology)
        {
            this.Ontology = ontology;
        }

        public LeaveOut_options_class(Ontology_type_enum ontology)
        {
            this.Ontology = ontology;
        }
    }

    class Leave_out_class
    {
        #region Fields
        public Leave_out_line_class[] Leave_out_lines { get; set; }
        public LeaveOut_options_class Options { get; set; }
        #endregion

        public Leave_out_class(Ontology_type_enum ontology)
        {
            Options = new LeaveOut_options_class(ontology);
        }

        #region Order
        public void Order_by_processLevel()
        {
            this.Leave_out_lines = this.Leave_out_lines.OrderBy(l => l.ProcessLevel).ToArray();
        }

        public void Order_by_processLevel_descending_newSymbolsRankIncreaseDividedByOriginalProcessSize()
        {
            this.Leave_out_lines = this.Leave_out_lines.OrderBy(l=>l.ProcessLevel).ThenByDescending(l => l.New_symbols_rank_increase_divided_by_original_process_size).ToArray();
        }

        public void Order_by_left_out_processName_processName()
        {
            this.Leave_out_lines = this.Leave_out_lines.OrderBy(l => l.Left_out_processName).ThenBy(l => l.ProcessName).ToArray();
        }

        public void Order_by_processName()
        {
            this.Leave_out_lines = this.Leave_out_lines.OrderBy(l => l.ProcessName).ToArray();
        }
        #endregion

        #region Check
        private void Check_for_duplicated_scp_scp_connections()
        {
            int leave_out_length = this.Leave_out_lines.Length;
            Leave_out_line_class leave_out_line;
            this.Leave_out_lines = this.Leave_out_lines.OrderBy(l => l.Left_out_processName).ThenBy(l => l.ProcessName).ToArray();
            for (int indexL = 0; indexL < leave_out_length; indexL++)
            {
                leave_out_line = this.Leave_out_lines[indexL];
                if ((indexL == leave_out_length - 1)
                    || (!leave_out_line.Left_out_processName.Equals(this.Leave_out_lines[indexL + 1].Left_out_processName))
                    || (!leave_out_line.ProcessName.Equals(this.Leave_out_lines[indexL + 1].ProcessName)))
                {
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        #endregion

        public void Keep_only_process_process_associations_that_only_contain_processNames_of_interest(params string[] processNames_of_interest)
        {
            int leave_out_length = this.Leave_out_lines.Length;
            Leave_out_line_class leave_out_line;
            List<Leave_out_line_class> keep_leave_out_list = new List<Leave_out_line_class>();
            for (int indexL = 0; indexL < leave_out_length; indexL++)
            {
                leave_out_line = this.Leave_out_lines[indexL];
                if ((processNames_of_interest.Contains(leave_out_line.ProcessName))
                    && (processNames_of_interest.Contains(leave_out_line.Left_out_processName)))
                {
                    keep_leave_out_list.Add(leave_out_line);
                }
            }
            this.Leave_out_lines = keep_leave_out_list.ToArray();
        }

        public void Keep_only_process_process_associations_of_indicated_levels(params int[] levels)
        {
            int leave_out_length = this.Leave_out_lines.Length;
            Leave_out_line_class leave_out_line;
            List<Leave_out_line_class> keep_leave_out_list = new List<Leave_out_line_class>();
            for (int indexL = 0; indexL < leave_out_length; indexL++)
            {
                leave_out_line = this.Leave_out_lines[indexL];
                if (levels.Contains(leave_out_line.ProcessLevel))
                {
                    keep_leave_out_list.Add(leave_out_line);
                }
            }
            this.Leave_out_lines = keep_leave_out_list.ToArray();
        }

        public void Remove_all_process_process_associations_between_input_processNames(params string[] removeSCP_names)
        {
            int leave_out_length = this.Leave_out_lines.Length;
            Leave_out_line_class leave_out_line;
            List<Leave_out_line_class> keep_leave_out_list = new List<Leave_out_line_class>();
            for (int indexL = 0; indexL < leave_out_length; indexL++)
            {
                leave_out_line = this.Leave_out_lines[indexL];
                if (  (!removeSCP_names.Contains(leave_out_line.Left_out_processName))
                    ||(!removeSCP_names.Contains(leave_out_line.ProcessName)))
                {
                    keep_leave_out_list.Add(leave_out_line);
                }
            }
            this.Leave_out_lines = keep_leave_out_list.ToArray();
        }

        public void Remove_all_process_process_associations_that_contain_processNames(params string[] remove_processNames)
        {
            string[] all_processNames = Get_all_ordered_processNames();
            string[] keep_processNames = Overlap_class.Get_part_of_list1_but_not_of_list2(all_processNames, remove_processNames);
            Keep_only_process_process_associations_that_only_contain_processNames_of_interest(keep_processNames);
        }

        public string[] Get_all_ordered_processNames()
        {
            int leave_out_length = this.Leave_out_lines.Length;
            Leave_out_line_class leave_out_line;
            List<string> processNames = new List<string>();

            this.Leave_out_lines = this.Leave_out_lines.OrderBy(l => l.Left_out_processName).ToArray();
            for (int indexL = 0; indexL < leave_out_length; indexL++)
            {
                leave_out_line = this.Leave_out_lines[indexL];
                if ((indexL == 0) || (!leave_out_line.Left_out_processName.Equals(this.Leave_out_lines[indexL - 1].Left_out_processName)))
                {
                    processNames.Add(leave_out_line.Left_out_processName);
                }
            }

            this.Leave_out_lines = this.Leave_out_lines.OrderBy(l => l.ProcessName).ToArray();
            for (int indexL = 0; indexL < leave_out_length; indexL++)
            {
                leave_out_line = this.Leave_out_lines[indexL];
                if ((indexL == 0) || (!leave_out_line.ProcessName.Equals(this.Leave_out_lines[indexL - 1].ProcessName)))
                {
                    processNames.Add(leave_out_line.ProcessName);
                }
            }
            return processNames.Distinct().OrderBy(l=>l).ToArray();
        }

        public int Get_level_and_check_if_all_levels_are_the_same()
        {
            int level = this.Leave_out_lines[0].ProcessLevel;
            int leave_out_length = this.Leave_out_lines.Length;
            Leave_out_line_class leave_out_line;
            for (int indexL=0; indexL<leave_out_length; indexL++)
            {
                leave_out_line = this.Leave_out_lines[indexL];
                if (leave_out_line.ProcessLevel != level)
                {
                    throw new Exception("Different levels in array");
                }
            }
            return level;
        }

        private void Calculate_edge_weights_as_highest_fractional_new_symbols_rank_increase()
        {
            int leave_out_length = this.Leave_out_lines.Length;
            Leave_out_line_class leave_out_line;
            for (int indexLeaveOut=0; indexLeaveOut<leave_out_length; indexLeaveOut++)
            {
                leave_out_line = this.Leave_out_lines[indexLeaveOut];
                leave_out_line.Edge_weight = leave_out_line.New_symbols_rank_increase_divided_by_original_process_size;
            }
        }

        public void Generate_by_reading_safed_file(System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            Read_if_available_or_set_leave_out_lines_to_zero_lines_array_otherwise(error_report_label, form_default_settings);
            Check_for_duplicated_scp_scp_connections();
            Calculate_edge_weights_as_highest_fractional_new_symbols_rank_increase();
        }

        public void Set_new_ontology(Ontology_type_enum ontology)
        {
            this.Options.Set_new_ontology(ontology);
        }

        #region Read copy
        private void Read_if_available_or_set_leave_out_lines_to_zero_lines_array_otherwise(System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            switch (Options.Ontology)
            {
                case Ontology_type_enum.Mbco_human:
                case Ontology_type_enum.Mbco_mouse:
                case Ontology_type_enum.Mbco_rat:
                    Leave_out_input_readWriteOptions_class readWriteOptions = new Leave_out_input_readWriteOptions_class(this.Options.Ontology);
                    this.Leave_out_lines = ReadWriteClass.Read_data_fill_array_and_complain_if_error_message<Leave_out_line_class>(readWriteOptions, error_report_label, form_default_settings);
                    break;
                case Ontology_type_enum.Go_bp_human:
                case Ontology_type_enum.Go_cc_human:
                case Ontology_type_enum.Go_mf_human:
                case Ontology_type_enum.Mbco_na_glucose_tm_transport_human:
                    this.Leave_out_lines = new Leave_out_line_class[0];
                    break;
                default:
                    throw new Exception();
            }
        }

        public Leave_out_class Deep_copy()
        {
            Leave_out_class copy = (Leave_out_class)this.MemberwiseClone();
            int leave_out_length = this.Leave_out_lines.Length;
            copy.Leave_out_lines = new Leave_out_line_class[leave_out_length];
            for (int indexL = 0; indexL < leave_out_length; indexL++)
            {
                copy.Leave_out_lines[indexL] = this.Leave_out_lines[indexL].Deep_copy();
            }
            return copy;
        }
        #endregion
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    class Leave_out_scp_scp_network_options_class
    {
        public Ontology_type_enum Ontology { get; set; }
        public float[] Top_quantile_of_considered_SCP_interactions_per_level { get; set;}
        public bool Consider_scp_interactions_between_signaling_processes { get; }

        public Leave_out_scp_scp_network_options_class(Ontology_type_enum ontology)
        {
            Top_quantile_of_considered_SCP_interactions_per_level = new float[] { 1F, 1F, 1F, 1F, 1F };
            Consider_scp_interactions_between_signaling_processes = false;
            Ontology = ontology;
        }

        public Leave_out_scp_scp_network_options_class Deep_copy()
        {
            Leave_out_scp_scp_network_options_class copy = (Leave_out_scp_scp_network_options_class)this.MemberwiseClone();
            copy.Top_quantile_of_considered_SCP_interactions_per_level = Array_class.Deep_copy_array(this.Top_quantile_of_considered_SCP_interactions_per_level);
            return copy;
        }
    }

    class Leave_out_generate_node_colors_class
    {
        #region Fields
        Dictionary<int, int> NodeIndex_colorIndex_dict { get; set; }
        Dictionary<string, string[]> ProcessName_siblingNames_dict { get; set; }
        Color[] Rotation_colors { get; set; }
        int Rotation_colors_length { get; set; }
        List<yed_node_color_line_class> Color_lines_list { get; set; }
        Network_class Process_nw { get; set; }
        Dictionary<int, bool> Already_considered_neighbors { get; set; }
        int Start_indexColor { get; set; }
        private Ontology_type_enum Ontology { get; set; }
        #endregion

        #region Initialize and generate
        public Leave_out_generate_node_colors_class(Ontology_type_enum ontology)
        {
            NodeIndex_colorIndex_dict = new Dictionary<int, int>();
            ProcessName_siblingNames_dict = new Dictionary<string, string[]>();
            Ontology = ontology;
        }

        public void Generate(Network_class process_nw,Color[] rotation_colors, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            this.Process_nw = process_nw.Deep_copy();
            this.Process_nw.Transform_into_undirected_double_network();
            this.Rotation_colors = rotation_colors;
            this.Rotation_colors_length = this.Rotation_colors.Length;
            this.Already_considered_neighbors = new Dictionary<int, bool>();

            NodeIndex_colorIndex_dict = new Dictionary<int, int>();
            Color_lines_list = new List<yed_node_color_line_class>();
            MBCO_obo_network_class mbc_obo_parent_child = new MBCO_obo_network_class(this.Ontology);
            mbc_obo_parent_child.Generate_by_reading_safed_obo_file(error_report_label, form_default_settings);
            ProcessName_siblingNames_dict = mbc_obo_parent_child.Get_processName_siblings_dictionary_if_direction_is_parent_child();
        }
        #endregion

        private int Identify_free_colorIndex_that_is_not_used_by_neighbors(int[] sibling_indexes)
        {
            int sibling_indexes_length = sibling_indexes.Length;
            int sibling_index;
            List<int> used_colors = new List<int>();
            Network_target_line_class[] target_lines;
            int target_lines_length;
            int target_line_nwIndex;
            for (int indexS = 0; indexS < sibling_indexes_length; indexS++)
            {
                sibling_index = sibling_indexes[indexS];
                target_lines = Process_nw.NW[sibling_index].Targets;
                target_lines_length = target_lines.Length;
                for (int indexT = 0; indexT < target_lines_length; indexT++)
                {
                    target_line_nwIndex = target_lines[indexT].NW_index;
                    if (NodeIndex_colorIndex_dict.ContainsKey(target_line_nwIndex))
                    {
                        used_colors.Add(NodeIndex_colorIndex_dict[target_line_nwIndex]);
                    }
                }
            }
            used_colors = used_colors.Distinct().OrderBy(l => l).ToList();
            int colorIndex = Start_indexColor;
            do
            {
                if (!used_colors.Contains(colorIndex))
                {
                    break;
                }
                else
                {
                    colorIndex++;
                }
                if (colorIndex == Rotation_colors_length)
                {
                    colorIndex = 0;
                }
            } while (colorIndex != Start_indexColor);
            Start_indexColor++;
            if (Start_indexColor == Rotation_colors_length) { Start_indexColor = 0; }
            return colorIndex;
        }

        private void Generate_new_yed_node_color_line_and_add_to_dictionary_and_list(int indexNW,int indexColor)
        {
            Color current_rotation_color = Rotation_colors[indexColor];

            NetworkNode_line_class node_line = Process_nw.Nodes.Get_indexed_node_line_if_index_is_correct(indexNW);
            yed_node_color_line_class new_node_color_line = new yed_node_color_line_class();
            Color_specification_line_class color_specification_line = new Color_specification_line_class();
            color_specification_line.Fill_color = current_rotation_color;
            new_node_color_line.Color_specifications = new Color_specification_line_class[] { color_specification_line };
            new_node_color_line.NodeName = (string)node_line.Name.Clone();
            NodeIndex_colorIndex_dict.Add(indexNW, indexColor);
            Color_lines_list.Add(new_node_color_line);
        }

        private int[] Identify_all_neighborIndexes_that_are_siblings_recursive(int indexNW)
        {
            Already_considered_neighbors.Add(indexNW, true);
            Network_line_class nw_line = Process_nw.NW[indexNW];
            NetworkNode_line_class this_node_line = Process_nw.Nodes.Get_indexed_node_line_if_index_is_correct(indexNW);
            NetworkNode_line_class target_node_line;
            int targets_length = nw_line.Targets_length;
            Network_target_line_class target_line;
            List<int> all_neighborIndexes_that_are_siblings_list = new List<int>();
            for (int indexT = 0; indexT < targets_length; indexT++)
            {
                target_line = nw_line.Targets[indexT];
                if (!Already_considered_neighbors.ContainsKey(target_line.NW_index))
                {
                    target_node_line = Process_nw.Nodes.Get_indexed_node_line_if_index_is_correct(target_line.NW_index);

                    if ((ProcessName_siblingNames_dict.ContainsKey(this_node_line.Name)) && (ProcessName_siblingNames_dict[this_node_line.Name].Contains(target_node_line.Name)))
                    {
                        all_neighborIndexes_that_are_siblings_list.Add(target_line.NW_index);
                        all_neighborIndexes_that_are_siblings_list.AddRange(Identify_all_neighborIndexes_that_are_siblings_recursive(target_line.NW_index));
                    }
                }
            }
            return all_neighborIndexes_that_are_siblings_list.ToArray();
        }

        private void Generate_and_add_yed_node_color_lines_for_all_neighbors_that_are_siblings(int indexNW)
        {
            Network_line_class nw_line = Process_nw.NW[indexNW];
            NetworkNode_line_class this_node_line = Process_nw.Nodes.Get_indexed_node_line_if_index_is_correct(indexNW);
            if (!NodeIndex_colorIndex_dict.ContainsKey(indexNW))
            {
                Already_considered_neighbors.Clear();
                int[] all_neighborIndexes_siblings = Identify_all_neighborIndexes_that_are_siblings_recursive(indexNW);
                int indexColor = Identify_free_colorIndex_that_is_not_used_by_neighbors(all_neighborIndexes_siblings); 
                Generate_new_yed_node_color_line_and_add_to_dictionary_and_list(indexNW, indexColor);
                int all_neighborIndexes_siblings_length = all_neighborIndexes_siblings.Length;
                int neighborIndex_sibling;
                for (int indexS = 0; indexS < all_neighborIndexes_siblings_length; indexS++)
                {
                    neighborIndex_sibling = all_neighborIndexes_siblings[indexS];
                    if (!NodeIndex_colorIndex_dict.ContainsKey(neighborIndex_sibling))
                    {
                        Generate_new_yed_node_color_line_and_add_to_dictionary_and_list(neighborIndex_sibling, indexColor);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
        }

        public yed_node_color_line_class[] Generate_new_yed_node_color_lines()
        {
            int nw_length = Process_nw.NW_length;
            for (int indexNW = 0; indexNW < nw_length; indexNW++)
            {
                Generate_and_add_yed_node_color_lines_for_all_neighbors_that_are_siblings(indexNW);
            }
            return Color_lines_list.ToArray();
        }

    }

    class Leave_out_scp_scp_combination_line_class
    {
        public string[] Processes { get; set; }
        public string StringCombine_processes
        {
            get { return ReadWriteClass.Get_writeLine_from_array(this.Processes, '-'); }
        }
    }

    class Leave_out_scp_scp_network_class
    {
        public Leave_out_scp_scp_network_options_class Options { get; set; }
        public Network_class Scp_nw {get;set;}
 
        public Leave_out_scp_scp_network_class(Ontology_type_enum ontology)
        {
            this.Options = new Leave_out_scp_scp_network_options_class(ontology);
            this.Scp_nw = new Network_class();
        }

        public void Replace_old_scp_name_by_new_scp_name(Dictionary<string,string> oldScp_to_newScp_dict)
        {
            Scp_nw.Nodes.Replace_old_scpNames_by_new_scpNames(oldScp_to_newScp_dict);
        }

        #region Generate from leave out instance
        private Dictionary<string,bool> Generate_dictionary_with_all_signaling_processes(System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            Dictionary<string, bool> signaling_processes_dict = new Dictionary<string, bool>();
            MBCO_obo_network_class mbco_obo_nw = new MBCO_obo_network_class(this.Options.Ontology);
            mbco_obo_nw.Generate_by_reading_safed_obo_file(error_report_label, form_default_settings);
            string[] signaling_processes = mbco_obo_nw.Get_all_offspring_if_direction_is_parent_child("Cellular communication");
            signaling_processes = signaling_processes.OrderBy(l => l).ToArray();
            foreach (string signaling_process in signaling_processes)
            {
                signaling_processes_dict.Add(signaling_process, true);
            }
            return signaling_processes_dict;
        }

        private NetworkTable_line_class[] Generate_networkTable_lines_for_one_level(Leave_out_line_class[] leave_out_lines, Dictionary<string,bool> scps_that_shall_not_be_connected_with_eachOther_dict)
        {
            int level = leave_out_lines[0].ProcessLevel;
            List<NetworkTable_line_class> networkTable_list = new List<NetworkTable_line_class>();
            NetworkTable_line_class networkTable_line;
            leave_out_lines = leave_out_lines.OrderByDescending(l => l.Edge_weight).ToArray();
            Leave_out_line_class leave_out_line;
            int leave_out_length = leave_out_lines.Length;
            float top_quantile_for_cutoff = this.Options.Top_quantile_of_considered_SCP_interactions_per_level[level];
            int top_quantile_lines_count = (int)Math.Round((float)leave_out_length * (top_quantile_for_cutoff));
            if (top_quantile_lines_count == leave_out_length) { top_quantile_lines_count = leave_out_length - 1; }
            for (int indexL = 0; indexL <= top_quantile_lines_count; indexL++)
            {
                leave_out_line = leave_out_lines[indexL];
                if (leave_out_line.ProcessLevel != level) { throw new Exception("Leave out contains more than one level"); }
                if (   (!scps_that_shall_not_be_connected_with_eachOther_dict.ContainsKey(leave_out_line.ProcessName))
                    || (!scps_that_shall_not_be_connected_with_eachOther_dict.ContainsKey(leave_out_line.Left_out_processName)))
                {
                    networkTable_line = new NetworkTable_line_class();
                    networkTable_line.Source = (string)leave_out_line.Left_out_processName.Clone();
                    networkTable_line.Target = (string)leave_out_line.ProcessName.Clone();
                    networkTable_line.Width = leave_out_line.Edge_weight * 2;
                    networkTable_line.Edge_type = NWedge_type_enum.Dashed_line;
                    networkTable_list.Add(networkTable_line);
                    networkTable_line = new NetworkTable_line_class();
                    networkTable_line.Target = (string)leave_out_line.Left_out_processName.Clone();
                    networkTable_line.Source = (string)leave_out_line.ProcessName.Clone();
                    networkTable_line.Width = leave_out_line.Edge_weight * 2;
                    networkTable_line.Edge_type = NWedge_type_enum.Dashed_line;
                    networkTable_list.Add(networkTable_line);
                }
            }

            List<NetworkTable_line_class> final_networkTable_list = new List<NetworkTable_line_class>();
            #region Remove duplicates
            networkTable_list = networkTable_list.OrderBy(l => l.Source).ThenBy(l => l.Target).ThenByDescending(l => l.Width).ToList();
            int networkTable_count = networkTable_list.Count;
            for (int indexNT=0; indexNT<networkTable_count; indexNT++)
            {
                networkTable_line = networkTable_list[indexNT];
                if (  (indexNT==0)
                    || (!networkTable_line.Source.Equals(networkTable_list[indexNT - 1].Source))
                    || (!networkTable_line.Target.Equals(networkTable_list[indexNT - 1].Target)))
                {
                    final_networkTable_list.Add(networkTable_line);
                }
            }
            #endregion

            return final_networkTable_list.ToArray();
        }

        private NetworkTable_line_class[] Generate_networkTable_lines(Leave_out_class leave_out, Dictionary<string, bool> scps_that_shall_not_be_connected_with_eachOther_dict)
        {
            leave_out.Order_by_processLevel_descending_newSymbolsRankIncreaseDividedByOriginalProcessSize();
            int leave_out_length = leave_out.Leave_out_lines.Length;
            Leave_out_line_class leave_out_line;
            List<Leave_out_line_class> sameLevel_leave_out_list = new List<Leave_out_line_class>();
            NetworkTable_line_class[] new_networkTable_lines;
            List<NetworkTable_line_class> networkTable_list = new List<NetworkTable_line_class>();
            for (int indexL=0; indexL < leave_out_length; indexL++)
            {
                leave_out_line = leave_out.Leave_out_lines[indexL];
                if ((indexL==0) || (!leave_out_line.ProcessLevel.Equals(leave_out.Leave_out_lines[indexL-1].ProcessLevel)))
                {
                    sameLevel_leave_out_list.Clear();
                }
                sameLevel_leave_out_list.Add(leave_out_line);
                if ((indexL == leave_out_length-1) || (!leave_out_line.ProcessLevel.Equals(leave_out.Leave_out_lines[indexL + 1].ProcessLevel)))
                {
                    new_networkTable_lines = Generate_networkTable_lines_for_one_level(sameLevel_leave_out_list.ToArray(), scps_that_shall_not_be_connected_with_eachOther_dict);
                    networkTable_list.AddRange(new_networkTable_lines);
                }
            }
            return networkTable_list.ToArray();
        }

        public void Generate_scp_scp_network_from_leave_out(Leave_out_class leave_out, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            Dictionary<string, bool> scps_that_shall_not_be_connected_with_eachOther_dict = new Dictionary<string, bool>();
            if (!Options.Consider_scp_interactions_between_signaling_processes) { scps_that_shall_not_be_connected_with_eachOther_dict = Generate_dictionary_with_all_signaling_processes(error_report_label, form_default_settings); }
            NetworkTable_line_class[] networkTable_lines = Generate_networkTable_lines(leave_out, scps_that_shall_not_be_connected_with_eachOther_dict);

            Scp_nw = new Network_class();
            Scp_nw.Add_from_networkTable_lines(networkTable_lines);
        }
        #endregion

        #region Generate Scp unions for dynamic enrichment analysis
        private Dictionary<string,string[]> Add_scpNames_combination_to_dictionary(Dictionary<string, string[]> scpNames_scpUnionNames_dict, string[] scpNames)
        {
            scpNames = scpNames.OrderBy(l => l).ToArray();
            string scpName;
            int scpNames_length = scpNames.Length;
            StringBuilder sb = new StringBuilder();
            for (int indexScp=0; indexScp<scpNames_length; indexScp++)
            {
                scpName = scpNames[indexScp];
                if (indexScp!=0) { sb.AppendFormat("{0}", Global_class.Scp_delimiter); }
                sb.AppendFormat(scpName);
            }
            string scpUnion_name = sb.ToString();
            List<string> scpUnions = new List<string>();
            for (int indexScp = 0; indexScp < scpNames_length; indexScp++)
            {
                scpName = scpNames[indexScp];
                if (!scpNames_scpUnionNames_dict.ContainsKey(scpName))
                {
                    scpNames_scpUnionNames_dict.Add(scpName, new string[] { (string)scpUnion_name.Clone() });
                }
                else
                {
                    scpUnions.Clear();
                    scpUnions.AddRange(scpNames_scpUnionNames_dict[scpName]);
                    scpUnions.Add(scpUnion_name);
                    scpNames_scpUnionNames_dict[scpName] = scpUnions.Distinct().OrderBy(l => l).ToArray();
                }
            }
            return scpNames_scpUnionNames_dict;
        }

        public Dictionary<string, string[]> Generate_scpNames_scpUnionNames_dict_with_indicated_numbers_of_combined_scps(int[] numbers_of_combined_scps, string[] consideredSCP_names)
        {
            consideredSCP_names = consideredSCP_names.Distinct().OrderBy(l => l).ToArray();
            Network_class scp_nw_considered = this.Scp_nw.Deep_copy();
            scp_nw_considered.Keep_only_input_nodeNames(consideredSCP_names);

            int nw_length = scp_nw_considered.NW_length;
            scp_nw_considered.Transform_into_undirected_double_network();
            scp_nw_considered.Nodes.Order_by_nw_index();
            Network_line_class nw_line0;
            Network_line_class nw_line1;
            Network_line_class nw_line2;
            Network_line_class nw_line3;
            Network_line_class nw_line4;
            Network_line_class nw_line5;
            Network_target_line_class nw_target1_line;
            Network_target_line_class nw_target2_line;
            Network_target_line_class nw_target3_line;
            Network_target_line_class nw_target4_line;
            Network_target_line_class nw_target5_line;
            int targets0_length;
            int targets1_length;
            int targets2_length;
            int targets3_length;
            int targets4_length;
            int targets5_length;
            NetworkNode_line_class current_node_line;
            List<string[]> all_target_processNames_list = new List<string[]>();
            Dictionary<string, string[]> scpNames_scpUnionNames_dict = new Dictionary<string, string[]>();
            Dictionary<int, bool> Included_nw_indexes = new Dictionary<int, bool>();

            int numbers_of_combined_scps_length = numbers_of_combined_scps.Length;
            int number_of_combined_scps;
            for (int indexCombined = 0; indexCombined < numbers_of_combined_scps_length; indexCombined++)
            {
                number_of_combined_scps = numbers_of_combined_scps[indexCombined];
                string[] target_processNames = new string[number_of_combined_scps];
                for (int indexNW = 0; indexNW < nw_length; indexNW++)
                {
                    nw_line0 = scp_nw_considered.NW[indexNW];
                    Included_nw_indexes.Add(indexNW, true);
                    current_node_line = scp_nw_considered.Nodes.Get_indexed_node_line_if_index_is_correct(indexNW);
                    target_processNames[0] = (string)current_node_line.Name.Clone();
                    targets0_length = nw_line0.Targets_length;

                    #region Loop1
                    for (int indexT_process1 = 0; indexT_process1 < targets0_length; indexT_process1++)
                    {
                        nw_target1_line = nw_line0.Targets[indexT_process1];
                        if (!Included_nw_indexes.ContainsKey(nw_target1_line.NW_index))
                        {
                            Included_nw_indexes.Add(nw_target1_line.NW_index, true);
                            nw_line1 = scp_nw_considered.NW[nw_target1_line.NW_index];
                            current_node_line = scp_nw_considered.Nodes.Get_indexed_node_line_if_index_is_correct(nw_target1_line.NW_index);
                            target_processNames[1] = (string)current_node_line.Name.Clone();
                            targets1_length = nw_line1.Targets_length;
                            if (number_of_combined_scps == 2)
                            {
                                Add_scpNames_combination_to_dictionary(scpNames_scpUnionNames_dict, target_processNames);
                            }
                            else
                            {
                                #region Loop2
                                for (int indexT_process2 = 0; indexT_process2 < targets1_length; indexT_process2++)
                                {
                                    nw_target2_line = nw_line1.Targets[indexT_process2];
                                    if (!Included_nw_indexes.ContainsKey(nw_target2_line.NW_index))
                                    {
                                        Included_nw_indexes.Add(nw_target2_line.NW_index, true);
                                        nw_line2 = scp_nw_considered.NW[nw_target2_line.NW_index];
                                        current_node_line = scp_nw_considered.Nodes.Get_indexed_node_line_if_index_is_correct(nw_target2_line.NW_index);
                                        target_processNames[2] = (string)current_node_line.Name.Clone();
                                        targets2_length = nw_line2.Targets_length;
                                        if (number_of_combined_scps == 3)
                                        {
                                            Add_scpNames_combination_to_dictionary(scpNames_scpUnionNames_dict, target_processNames);
                                        }
                                        else
                                        {
                                            #region Loop3
                                            for (int indexT_process3 = 0; indexT_process3 < targets2_length; indexT_process3++)
                                            {
                                                nw_target3_line = nw_line2.Targets[indexT_process3];
                                                if (!Included_nw_indexes.ContainsKey(nw_target3_line.NW_index))
                                                {
                                                    Included_nw_indexes.Add(nw_target3_line.NW_index, true);
                                                    nw_line3 = scp_nw_considered.NW[nw_target3_line.NW_index];
                                                    current_node_line = scp_nw_considered.Nodes.Get_indexed_node_line_if_index_is_correct(nw_target3_line.NW_index);
                                                    target_processNames[3] = (string)current_node_line.Name.Clone();
                                                    targets3_length = nw_line3.Targets_length;
                                                    if (number_of_combined_scps == 4)
                                                    {
                                                        Add_scpNames_combination_to_dictionary(scpNames_scpUnionNames_dict, target_processNames);
                                                    }
                                                    else
                                                    {
                                                        #region Loop4
                                                        for (int indexT_process4 = 0; indexT_process4 < targets3_length; indexT_process4++)
                                                        {
                                                            nw_target4_line = nw_line3.Targets[indexT_process4];
                                                            if (!Included_nw_indexes.ContainsKey(nw_target4_line.NW_index))
                                                            {
                                                                Included_nw_indexes.Add(nw_target4_line.NW_index, true);
                                                                nw_line4 = scp_nw_considered.NW[nw_target4_line.NW_index];
                                                                current_node_line = scp_nw_considered.Nodes.Get_indexed_node_line_if_index_is_correct(nw_target4_line.NW_index);
                                                                target_processNames[4] = (string)current_node_line.Name.Clone();
                                                                targets4_length = nw_line4.Targets_length;
                                                                if (number_of_combined_scps == 5)
                                                                {
                                                                    Add_scpNames_combination_to_dictionary(scpNames_scpUnionNames_dict, target_processNames);
                                                                }
                                                                else
                                                                {
                                                                    #region Loop5
                                                                    for (int indexT_process5 = 0; indexT_process5 < targets4_length; indexT_process5++)
                                                                    {
                                                                        nw_target5_line = nw_line4.Targets[indexT_process5];
                                                                        nw_line5 = scp_nw_considered.NW[nw_target5_line.NW_index];
                                                                        current_node_line = scp_nw_considered.Nodes.Get_indexed_node_line_if_index_is_correct(nw_target5_line.NW_index);
                                                                        target_processNames[5] = (string)current_node_line.Name.Clone();
                                                                        targets5_length = nw_line5.Targets_length;
                                                                        if (number_of_combined_scps == 6)
                                                                        {
                                                                            Add_scpNames_combination_to_dictionary(scpNames_scpUnionNames_dict, target_processNames);
                                                                        }
                                                                        else
                                                                        {
                                                                            throw new Exception();
                                                                        }
                                                                    }
                                                                    #endregion
                                                                }
                                                                Included_nw_indexes.Remove(nw_target4_line.NW_index);

                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                    Included_nw_indexes.Remove(nw_target3_line.NW_index);
                                                }
                                            }
                                            #endregion
                                        }
                                        Included_nw_indexes.Remove(nw_target2_line.NW_index);
                                    }
                                }
                                #endregion
                            }
                            Included_nw_indexes.Remove(nw_target1_line.NW_index);
                        }
                    }
                    #endregion

                    Included_nw_indexes.Remove(indexNW);
                }
            }
           return scpNames_scpUnionNames_dict;
        }

        public string[][] Generate_array_of_scp_unions_between_any_combination_between_two_or_three_neighboring_selected_scps(string[] consideredSCP_names)
        {
            consideredSCP_names = consideredSCP_names.Distinct().OrderBy(l => l).ToArray();
            Network_class scp_nw_considered = this.Scp_nw.Deep_copy();
            scp_nw_considered.Keep_only_input_nodeNames(consideredSCP_names);
            scp_nw_considered.Transform_into_undirected_double_network();
            int nw_length = scp_nw_considered.NW_length;
            Network_line_class nw_line;
            Network_target_line_class target1_line;
            Network_target_line_class target2_line;
            NetworkNode_line_class current_source_node_line;
            NetworkNode_line_class target1_node_line;
            NetworkNode_line_class target2_node_line;
            string current_source_scp;
            string current_target1_scp;
            string current_target2_scp;
            int target_length;
            List<string[]> list_of_scps_of_each_union = new List<string[]>();
            List<string> scps_of_one_union_list = new List<string>();
            for (int indexNW=0;indexNW<nw_length; indexNW++)
            {
                nw_line = scp_nw_considered.NW[indexNW];
                target_length = nw_line.Targets.Length;
                current_source_node_line = scp_nw_considered.Nodes.Get_indexed_node_line_if_index_is_correct(indexNW);
                current_source_scp = current_source_node_line.Name;
                for (int indexT1=0; indexT1<target_length;indexT1++)
                {
                    target1_line = nw_line.Targets[indexT1];
                    target1_node_line = scp_nw_considered.Nodes.Get_indexed_node_line_if_index_is_correct(target1_line.NW_index);
                    current_target1_scp = target1_node_line.Name;
                    scps_of_one_union_list.Clear();
                    scps_of_one_union_list.Add(current_source_scp);
                    scps_of_one_union_list.Add(current_target1_scp);
                    scps_of_one_union_list.Add("ZZZZZZZZZZZZZZZZZZZZZ-Remove SCP");
                    list_of_scps_of_each_union.Add(scps_of_one_union_list.OrderBy(l => l).ToArray());
                    for (int indexT2=indexT1+1; indexT2<target_length;indexT2++)
                    {
                        target2_line = nw_line.Targets[indexT2];
                        target2_node_line = scp_nw_considered.Nodes.Get_indexed_node_line_if_index_is_correct(target2_line.NW_index);
                        current_target2_scp = target2_node_line.Name;
                        scps_of_one_union_list.Clear();
                        scps_of_one_union_list.Add(current_source_scp);
                        scps_of_one_union_list.Add(current_target1_scp);
                        scps_of_one_union_list.Add(current_target2_scp);
                        list_of_scps_of_each_union.Add(scps_of_one_union_list.OrderBy(l => l).ToArray());
                    }
                }
            }
            list_of_scps_of_each_union = list_of_scps_of_each_union.OrderBy(l => l.Length).ThenBy(l=>l[0]).ThenBy(l => l[1]).ThenBy(l=>l[2]).ToList();
            int list_count = list_of_scps_of_each_union.Count;
            List<string[]> unique_list_of_scps_of_each_union = new List<string[]>();
            string[] current_scp_union;
            for (int indexList=0; indexList<list_count;indexList++)
            {
                current_scp_union = list_of_scps_of_each_union[indexList];
                if (   (indexList==0)
                     ||(!current_scp_union[0].Equals(list_of_scps_of_each_union[indexList - 1][0]))
                     ||(!current_scp_union[1].Equals(list_of_scps_of_each_union[indexList - 1][1]))
                     ||(!current_scp_union[2].Equals(list_of_scps_of_each_union[indexList - 1][2])))
                    {
                        if (current_scp_union[2].IndexOf("ZZZZZZZZZZZZZZZZZZZZZ-Remove SCP") ==0)
                        {
                            unique_list_of_scps_of_each_union.Add(new string[] { current_scp_union[0], current_scp_union[1] });
                        }
                        else
                        { 
                            unique_list_of_scps_of_each_union.Add(current_scp_union);
                        }
                    }
            }
            return unique_list_of_scps_of_each_union.ToArray();
        }
        #endregion

        #region Add 
        public void Set_all_edges_width_to_1()
        {
            int scp_nw_length = this.Scp_nw.NW_length;
            Network_line_class nw_line;
            int targets_length;
            for (int indexNW=0; indexNW<scp_nw_length; indexNW++)
            {
                nw_line = this.Scp_nw.NW[indexNW];
                targets_length = nw_line.Targets.Length;
                for (int indexT=0; indexT<targets_length; indexT++)
                {
                    nw_line.Targets[indexT].Width = 1;
                }
            }
        }
        #endregion

        public void Add_ancestors_of_missing_levels_and_connect_parent_child_scps(MBCO_obo_network_class mbco_obo_network_parent_child)
        {
            MBCO_obo_network_class mbco_obo_network_child_parent = mbco_obo_network_parent_child.Deep_copy_mbco_obo_nw();
            mbco_obo_network_child_parent.Transform_into_child_parent_direction();

            MBCO_obo_network_class current_mbco_obo_network;
            int[] levels_in_scp = this.Scp_nw.Nodes.Get_all_levels();
            int levels_in_scp_length = levels_in_scp.Length;
            int level_in_scp;
            int max_level_in_scp = -1;
            for (int indexLevelScp=0; indexLevelScp < levels_in_scp_length; indexLevelScp++)
            {
                level_in_scp = levels_in_scp[indexLevelScp];
                if (  (max_level_in_scp==-1)
                    ||(max_level_in_scp<level_in_scp))
                {
                    max_level_in_scp = level_in_scp;
                }
            }
            int[] levels_in_mbco = mbco_obo_network_child_parent.Nodes.Get_all_levels();
            int[] add_levels_preliminary = Overlap_class.Get_part_of_list1_but_not_of_list2(levels_in_mbco, levels_in_scp);
            List<int> add_levels_list = new List<int>();
            foreach (int add_level_preliminary in add_levels_preliminary)
            {
                if ((add_level_preliminary<max_level_in_scp)&&(add_level_preliminary!=0))
                {
                    add_levels_list.Add(add_level_preliminary);
                }
            }
            int[] add_levels = add_levels_list.ToArray();

            add_levels = add_levels.OrderByDescending(l => l).ToArray();
            int add_length = add_levels.Length;
            int add_level;
            string[] add_parents;
            string[] children;
            string[] keep_nodes;
            for (int indexAdd=0; indexAdd < add_length; indexAdd++)
            {
                add_level = add_levels[indexAdd];
                level_in_scp = add_level+1;
                children = this.Scp_nw.Nodes.Get_all_nodeNames_of_indicated_levels(level_in_scp);
                add_parents = mbco_obo_network_child_parent.Get_all_parents_if_direction_is_child_parent(children);
                current_mbco_obo_network = mbco_obo_network_parent_child.Deep_copy_mbco_obo_nw();
                keep_nodes = Array_class.Get_ordered_union(add_parents, children);
                current_mbco_obo_network.Keep_only_input_nodeNames(keep_nodes);
                this.Scp_nw.Merge_this_network_with_other_network(current_mbco_obo_network);
            }
            MBCO_obo_network_class scp_parent_child_network = mbco_obo_network_parent_child.Deep_copy_mbco_obo_nw();
            scp_parent_child_network.Keep_only_input_nodeNames(this.Scp_nw.Nodes.Get_all_nodeNames());
            this.Scp_nw.Merge_this_network_with_other_network(scp_parent_child_network);
        }

        public void Connect_parent_child_scps(MBCO_obo_network_class mbco_obo_network_parent_child)
        {
            MBCO_obo_network_class scp_parent_child_network = mbco_obo_network_parent_child.Deep_copy_mbco_obo_nw();
            scp_parent_child_network.Keep_only_input_nodeNames(this.Scp_nw.Nodes.Get_all_nodeNames());
            this.Scp_nw.Merge_this_network_with_other_network(scp_parent_child_network);
        }

        public Leave_out_scp_scp_network_class Deep_copy_scp_network()
        {
            Leave_out_scp_scp_network_class copy = (Leave_out_scp_scp_network_class)this.MemberwiseClone();
            copy.Options = this.Options.Deep_copy();
            copy.Scp_nw = this.Scp_nw.Deep_copy();
            return copy;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}
