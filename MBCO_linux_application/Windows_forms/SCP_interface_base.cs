////The code was written by Jens Hansen working for the Ravi Iyengar Lab
////The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
////Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
////Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
////A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
////Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows_forms_customized_tools;
//using System.Windows.Forms;
//using Common_functions.Array_own;
//using Network;
//using Enrichment;

//namespace ClassLibrary1.Scp_interface_base
//{
//    class Scps_userInterface_base_class
//    {
//        const string No_selected_scps_message = "No SCPs selected";
//        private MyPanel Overall_panel { get; set; }
//        private OwnListBox Mbco_scps_checkBox { get; set; }
//        private OwnListBox Selected_scps_listBox { get; set; }
//        private OwnCheckBox Sort_alphabetically_checkBox { get; set; }
//        private OwnCheckBox Sort_byLevel_checkBox { get; set; }
//        private OwnCheckBox Sort_byLevelParentSCP_checkBox { get; set; }
//        private bool Update_mbco_scps { get; set; }
//        private bool Update_selected_scps { get; set; }
//        private Button Add_button { get; set; }
//        private Button Remove_button { get; set; }
//        private Button Write_mbcoHierarchy_button { get; set; }
//        private MBCO_obo_network_class Mbco_parent_child_network { get; set; }

//        public Scps_userInterface_base_class(MyPanel overall_panel,
//                                             OwnListBox mbco_scps_textBox,
//                                             OwnListBox selected_scps_listBox,
//                                             OwnCheckBox sort_alphabetically_checkBox,
//                                             OwnCheckBox sort_byLevel_checkBox,
//                                             OwnCheckBox sort_byLevelParentSCP_checkBox,
//                                             Button add_button,
//                                             Button remove_button,
//                                             Button write_mbcoHierarchy_button
//                                             )
//        {
//            Overall_panel = overall_panel;
//            Mbco_scps_checkBox = mbco_scps_textBox;
//            Selected_scps_listBox = selected_scps_listBox;
//            Sort_alphabetically_checkBox = sort_alphabetically_checkBox;
//            Sort_byLevel_checkBox = sort_byLevel_checkBox;
//            Sort_byLevelParentSCP_checkBox = sort_byLevelParentSCP_checkBox;
//            Add_button = add_button;
//            Remove_button = remove_button;
//            Mbco_parent_child_network = null;
//            Update_mbco_scps = true;
//            Update_selected_scps = true;
//            Write_mbcoHierarchy_button = write_mbcoHierarchy_button;
//            Set_to_default();
//        }

//        private void Set_to_default()
//        {
//            Add_button.BackColor = Form1_default_settings_class.Color_notPressed_button;
//            Add_button.ForeColor = Form1_default_settings_class.Color_button_text;
//            Add_button.Visible = false;
//            Remove_button.BackColor = Form1_default_settings_class.Color_notPressed_button;
//            Remove_button.ForeColor = Form1_default_settings_class.Color_button_text;
//            Remove_button.Visible = false;
//            Write_mbcoHierarchy_button.BackColor = Form1_default_settings_class.Color_notPressed_button;
//            Write_mbcoHierarchy_button.ForeColor = Form1_default_settings_class.Color_button_text;
//            Sort_alphabetically_checkBox.SilentChecked = false;
//            Sort_byLevelParentSCP_checkBox.SilentChecked = false;
//            Sort_byLevel_checkBox.SilentChecked = true;
//        }

//        public void Set_to_visible(Mbc_enrichment_pipeline_options_class options, Label error_repor_label, int error_report_label_x_location)
//        {
//            Update_mbcoSCPs_and_selectedSCPs_listBox(options, error_repor_label, error_report_label_x_location);
//            this.Overall_panel.Visible = true;
//        }

//        private void Update_mbcoSCPs_and_selectedSCPs_listBox(Mbc_enrichment_pipeline_options_class options, string selectedScpGroup, Label error_repor_label, int error_report_label_x_location)
//        {
//            if (Update_mbco_scps)
//            {
//                if (Mbco_parent_child_network == null)
//                {
//                    Mbco_parent_child_network = new MBCO_obo_network_class();
//                    Mbco_parent_child_network.Generate_by_reading_safed_obo_file(Common_functions.Global_definitions.Ontology_type_enum.Mbco_human, error_repor_label, error_report_label_x_location);
//                }
//                string[] add_mbco_scps = new string[0];
//                List<string> add_mbco_scps_list = new List<string>();
//                if (Sort_alphabetically_checkBox.Checked)
//                {
//                    string[] all_scps = Mbco_parent_child_network.Get_all_scps();
//                    add_mbco_scps = all_scps.OrderBy(l => l).ToArray();
//                }
//                else if (Sort_byLevel_checkBox.Checked)
//                {
//                    int[] levels = Mbco_parent_child_network.Get_all_levels();
//                    levels = levels.OrderBy(l => l).ToArray();
//                    add_mbco_scps_list.Clear();
//                    string[] level_scps;
//                    foreach (int level in levels)
//                    {
//                        if (level != 0)
//                        {
//                            level_scps = Mbco_parent_child_network.Get_all_scps_of_indicated_levels(level);
//                            level_scps = level_scps.OrderBy(l => l).ToArray();
//                            add_mbco_scps_list.Add("Level " + level + "SCPs:");
//                            add_mbco_scps_list.AddRange(level_scps);
//                            add_mbco_scps_list.Add("");
//                        }
//                    }
//                    add_mbco_scps = add_mbco_scps_list.ToArray();
//                }
//                else if (Sort_byLevelParentSCP_checkBox.Checked)
//                {
//                    int[] levels = Mbco_parent_child_network.Get_all_levels();
//                    levels = levels.OrderBy(l => l).ToArray();
//                    add_mbco_scps_list.Clear();
//                    string[] level_scps;
//                    string[] children_scps;
//                    foreach (int level in levels)
//                    {
//                        if (level < 4)
//                        {
//                            level_scps = Mbco_parent_child_network.Get_all_scps_of_indicated_levels(level);
//                            foreach (string level_scp in level_scps)
//                            {
//                                children_scps = Mbco_parent_child_network.Get_all_children_if_direction_is_parent_child(level_scp);
//                                children_scps = Overlap_class.Get_part_of_list1_but_not_of_list2(children_scps, level_scp);
//                                if (children_scps.Length > 0)
//                                {
//                                    add_mbco_scps_list.Add("Level " + (level + 1) + " children of " + level_scp);
//                                    children_scps = children_scps.OrderBy(l => l).ToArray();
//                                    add_mbco_scps_list.AddRange(children_scps);
//                                    add_mbco_scps_list.Add("");
//                                }
//                            }
//                        }
//                    }
//                    add_mbco_scps = add_mbco_scps_list.ToArray();
//                }
//                Mbco_scps_checkBox.Items.Clear();
//                Mbco_scps_checkBox.Items.AddRange(add_mbco_scps);
//                Update_mbco_scps = false;
//            }
//            if (Update_selected_scps)
//            {
//                int mbco_scps_length = Mbco_scps_checkBox.Items.Count;
//                string[] mbco_scps = new string[mbco_scps_length];
//                for (int indexMbco = 0; indexMbco < mbco_scps_length; indexMbco++)
//                {
//                    mbco_scps[indexMbco] = Mbco_scps_checkBox.Items[indexMbco].ToString();
//                }
//                string[] selected_scps = options.Group_selectedScps_dict[selectedScpGroup];
//                Dictionary<string, bool> selectedScps_dict = new Dictionary<string, bool>();
//                foreach (string scp in selected_scps)
//                {
//                    selectedScps_dict.Add(scp, true);
//                }

//                List<string> add_selected_scps = new List<string>();
//                List<string> add_next_scpBlock = new List<string>();
//                int add_mbco_scps_length = mbco_scps.Length;
//                string add_mbco_scp;
//                bool lastEmptyLine_passed = true;
//                string current_headline = "";
//                for (int indexMbco = 0; indexMbco < add_mbco_scps_length; indexMbco++)
//                {
//                    add_mbco_scp = mbco_scps[indexMbco];
//                    if (lastEmptyLine_passed)
//                    {
//                        add_next_scpBlock.Clear();
//                        current_headline = (string)add_mbco_scp.Clone();
//                        add_next_scpBlock.Add(current_headline);
//                        lastEmptyLine_passed = false;
//                    }
//                    else if (selectedScps_dict.ContainsKey(add_mbco_scp))
//                    {
//                        add_next_scpBlock.Add((string)add_mbco_scp.Clone());
//                    }
//                    if (String.IsNullOrEmpty(add_mbco_scp))
//                    {
//                        lastEmptyLine_passed = true;
//                        if (add_next_scpBlock.Count > 1)
//                        {
//                            add_selected_scps.AddRange(add_next_scpBlock);
//                            add_selected_scps.Add("");
//                        }
//                    }
//                }
//                Selected_scps_listBox.Items.Clear();
//                Selected_scps_listBox.Items.AddRange(add_selected_scps.ToArray());
//                Update_selected_scps = false;
//            }
//        }
//        public void Add_button_pressed(Mbc_enrichment_pipeline_options_class options, Label error_report_label, int error_report_label_x_location)
//        {
//            int add_scps_length = Mbco_scps_checkBox.SelectedItems.Count;
//            string[] add_to_selected_scps = new string[add_scps_length];
//            for (int indexS = 0; indexS < add_scps_length; indexS++)
//            {
//                add_to_selected_scps[indexS] = (string)Mbco_scps_checkBox.SelectedItems[indexS];
//            }
//            //options.Selected_scps = Overlap_class.Get_union(options.Selected_scps, add_to_selected_scps);
//            Update_selected_scps = true;
//            Update_mbcoSCPs_and_selectedSCPs_listBox(options, error_report_label, error_report_label_x_location);
//            Mbco_scps_checkBox.SelectedItems.Clear();
//            Add_button.Visible = false;
//        }

//        public void Remove_button_pressed(Mbc_enrichment_pipeline_options_class options, Label error_report_label, int error_report_label_x_location)
//        {
//            int remove_scps_length = Selected_scps_listBox.SelectedItems.Count;
//            string[] remove_from_selected_scps = new string[remove_scps_length];
//            for (int indexS = 0; indexS < remove_scps_length; indexS++)
//            {
//                remove_from_selected_scps[indexS] = (string)Selected_scps_listBox.SelectedItems[indexS];
//            }
//           // options.Selected_scps = Overlap_class.Get_part_of_list1_but_not_of_list2(options.Selected_scps, remove_from_selected_scps);
//           // if (options.Selected_scps.Length == 0) { options.Show_all_and_only_selected_scps = false; }
//            Update_selected_scps = true;
//            Update_mbcoSCPs_and_selectedSCPs_listBox(options, error_report_label, error_report_label_x_location);
//            Selected_scps_listBox.SelectedItems.Clear();
//            Remove_button.Visible = false;
//        }
//        public void Sort_alphabetically_pressed(Mbc_enrichment_pipeline_options_class options, Label error_report_label, int error_report_label_x_location)
//        {
//            Update_mbco_scps = true;
//            Update_selected_scps = true;
//            Sort_byLevel_checkBox.SilentChecked = !Sort_alphabetically_checkBox.Checked;
//            Sort_byLevelParentSCP_checkBox.SilentChecked = !Sort_alphabetically_checkBox.Checked;
//            Update_mbcoSCPs_and_selectedSCPs_listBox(options, error_report_label, error_report_label_x_location);
//        }
//        public void Sort_byLevel_pressed(Mbc_enrichment_pipeline_options_class options, Label error_report_label, int error_report_label_x_location)
//        {
//            Update_mbco_scps = true;
//            Update_selected_scps = true;
//            Sort_alphabetically_checkBox.SilentChecked = !Sort_byLevel_checkBox.Checked;
//            Sort_byLevelParentSCP_checkBox.SilentChecked = !Sort_byLevel_checkBox.Checked;
//            Update_mbcoSCPs_and_selectedSCPs_listBox(options, error_report_label, error_report_label_x_location);
//        }
//        public void Sort_byLevelParentScp_pressed(Mbc_enrichment_pipeline_options_class options, Label error_report_label, int error_report_label_x_location)
//        {
//            Update_mbco_scps = true;
//            Update_selected_scps = true;
//            Sort_alphabetically_checkBox.SilentChecked = !Sort_byLevelParentSCP_checkBox.Checked;
//            Sort_byLevel_checkBox.SilentChecked = !Sort_byLevelParentSCP_checkBox.Checked;
//            Update_mbcoSCPs_and_selectedSCPs_listBox(options, error_report_label, error_report_label_x_location);
//        }
//        public void MBCO_listBox_changed()
//        {
//            if (Mbco_scps_checkBox.SelectedItems.Count > 0)
//            {
//                Add_button.Visible = true;
//            }
//            else
//            {
//                Add_button.Visible = false;
//            }
//            Add_button.Refresh();
//        }
//        public void Selected_listBox_changed()
//        {
//            if ((Selected_scps_checkBox.SelectedItems.Count == 1)
//                && (!Selected_scps_checkBox.SelectedItems[0].Equals(No_selected_scps_message)))
//            {
//                Remove_button.Visible = true;
//            }
//            else if (Selected_scps_checkBox.SelectedItems.Count > 0)
//            {
//                Remove_button.Visible = true;
//            }
//            else
//            {
//                Remove_button.Visible = false;
//            }
//            Remove_button.Refresh();
//        }

//        public void Write_mbco_yed_network(Label progress_report, int error_report_label_x_location)
//        {
//            progress_report.Text = "Write MBCO hierarchy into " + Common_functions.Global_definitions.Global_directory_and_file_class.Results_directory + "MBCO_hierarchy\\" + "\r\nOpen with graph editor (e.g. yED graph editor, here use layout circular)";
//            progress_report.Visible = true;
//            progress_report.Refresh();
//            Write_mbcoHierarchy_button.BackColor = Form1_default_settings_class.Color_pressed_button;
//            Mbco_parent_child_network.Write_yED_nw_in_results_directory_with_nodes_colored_by_level("MBCO_hierarchy\\", "MBCO_parent_child", yed_network.Shape_enum.Rectangle, progress_report, error_report_label_x_location);
//            System.Threading.Thread.Sleep(5000);
//            progress_report.Text = "";
//            progress_report.Refresh();
//            Write_mbcoHierarchy_button.BackColor = Form1_default_settings_class.Color_notPressed_button;
//        }
//    }
//}
