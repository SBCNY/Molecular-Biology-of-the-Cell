using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows_forms_customized_tools;
using Common_functions.Form_tools;
using Network;
using Common_functions.Global_definitions;
using Enrichment;
using Windows_forms;
using yed_network;
using System.Windows.Forms.VisualStyles;

namespace ClassLibrary1.Tips_userInterface
{
    class Tips_userInterface_class
    {
        private MyPanel Overall_panel { get; set; }
        private Label OverallHeadline_label { get; set; }
        private MyPanel_textBox Tips_myPanelTextBox { get; set; }
        private Label Demonstration_headline_label { get; set; }
        private MyCheckBox_button Demonstration_cbButton { get; set; }
        private MyPanel_label Demonstration_cbMyPanelLabel { get; set; }
        private Button Forward_button { get; set; }
        private Button Backward_button { get; set; }
        private Button Write_mbco_hierarchy_button { get; set; }
        private Form1_default_settings_class Form_default_settings { get; set; }
        private MBCO_obo_network_class Mbco_parent_child_network { get; set; }
        private ProgressReport_interface_class ProgressReport { get; set; }
        private int Shown_page { get; set; }

        public Tips_userInterface_class(MyPanel overall_panel,
                                        Label overallHeadline_label,
                                        MyPanel_textBox tips_myPaneTextBox,
                                        Label demonstration_headline_label,
                                        MyCheckBox_button demonstration_cbButton,
                                        MyPanel_label demonstration_cbMyPanelLabel,
                                        Button forward_button,
                                        Button backward_button,
                                        Button write_mbco_hierarchy_button,
                                        MBCO_enrichment_pipeline_options_class mbco_options, 
                                        ProgressReport_interface_class progressReport,
                                        Form1_default_settings_class form_default_settings,
                                        MBCO_obo_network_class mbco_parent_child_nw)
        {
            this.Form_default_settings = form_default_settings;
            this.Overall_panel = overall_panel;
            this.OverallHeadline_label = overallHeadline_label;
            this.Tips_myPanelTextBox = tips_myPaneTextBox;
            this.Demonstration_headline_label = demonstration_headline_label;
            this.Demonstration_cbButton = demonstration_cbButton;
            this.Demonstration_cbMyPanelLabel = demonstration_cbMyPanelLabel;
            this.Forward_button = forward_button;
            this.Backward_button = backward_button;
            this.Write_mbco_hierarchy_button = write_mbco_hierarchy_button;
            this.ProgressReport = progressReport;
            Shown_page = 0;
            Update_all_graphic_elements();
            Update_mbco_parent_child_and_child_parent_obo_networks(mbco_parent_child_nw);
        }

        public bool Are_parentChild_hierarchy_networks_upToDate(MBCO_enrichment_pipeline_options_class options)
        {
            return (Mbco_parent_child_network.Ontology.Equals(options.Next_ontology))
                   && (Mbco_parent_child_network.Organism.Equals(options.Next_organism));
        }

        public void Update_mbco_parent_child_and_child_parent_obo_networks(MBCO_obo_network_class parent_child_nw)
        {
            if (!parent_child_nw.Nodes.Direction.Equals(Ontology_direction_enum.Parent_child)) { throw new Exception(); }
            if (!parent_child_nw.Scp_hierarchal_interactions.Equals(SCP_hierarchy_interaction_type_enum.Parent_child)) { throw new Exception(); }
            Mbco_parent_child_network = parent_child_nw.Deep_copy_mbco_obo_nw();
        }

        private void Update_tips_text_and_forward_and_backward_buttons()
        {
            int bottom_reference_position;
            int top_reference_position;
            int left_reference_position;
            int right_reference_position;
            left_reference_position = (int)Math.Round(0.01 * Overall_panel.Width);
            right_reference_position = (int)Math.Round(0.99 * Overall_panel.Width);

            float fraction_for_tips = 0.7F;
            int total_y_space = (int)Math.Round((float)(fraction_for_tips * Overall_panel.Height - OverallHeadline_label.Location.Y - OverallHeadline_label.Height));
            bottom_reference_position = (int)Math.Round(0.04 * Overall_panel.Height + OverallHeadline_label.Location.Y + OverallHeadline_label.Height);
            top_reference_position = bottom_reference_position;
            bottom_reference_position = top_reference_position + total_y_space;
            Tips_myPanelTextBox.BackColor = Overall_panel.BackColor;
            Tips_myPanelTextBox.Font_style = System.Drawing.FontStyle.Regular;
            Tips_myPanelTextBox.Set_left_top_right_bottom_position(left_reference_position, top_reference_position, right_reference_position, bottom_reference_position, Form_default_settings);
            Tips_myPanelTextBox.FullSize_OwnTextBox.TextAlign = HorizontalAlignment.Center;
            Forward_button.Visible = true;
            Backward_button.Visible = true;
            switch (Shown_page)
            {
                case 0:
                    Tips_myPanelTextBox.Set_silent_text_adjustFontSize_and_refresh(""
                    + "Within yED Graph Editor you can zoom out 5 times from the 1:1 level, before pie charts are visualized as gray squares and node labels disappear. To increase the number of scp nodes shown on the screen at the same zoom level, decrease node diameters and label sizes within the 'SCP networks' menu. Alternatively, you can export any network into a PDF."
                    + "\r\n" + "\r\n"
                    + "C# color maps can be found online. Colors in hexadecimal code can be uploaded using the menu 'Read data' and will be mapped to the closest C# color.",
                    Form_default_settings
                    );
                    Backward_button.Visible = false;
                    break;
                case 1:
                    string add_for_ubuntu = "";
                    if (Form_default_settings.Is_mono)
                    {
                        add_for_ubuntu = " or text is missing in labels or buttons";
                    }
                    Tips_myPanelTextBox.Set_silent_text_adjustFontSize_and_refresh(""
                    + "If list boxes are collapsed and shown as lines" + add_for_ubuntu + ", increase the height of the application by changing the window setttings."
                    + "\r\n" + "\r\n"
                    + "Select list box entires via mouse click, arrow or letter keys. The latter will jump to the next item (e.g., color) starting with the pressed letter. Scroll bars only change the items that are shown. Selected items are always highlighted."
                    + "\r\n" + "\r\n"
                    + "Decimal numbers can be specified using dots or commas, depending on your area.",
                    Form_default_settings
                    );
                    break;
                case 2:
                    Tips_myPanelTextBox.Set_silent_text_adjustFontSize_and_refresh(""
                    + "Independent of your area, we enforce points as decimal separators when writing graphml or xgmml files for network visualization. This allowed error-free data import using any English version of the yED Graph Editor and Cytoscape we tested. If networks cannot be loaded, double check that your version uses point and not comma separators."
                    + "\r\n" + "\r\n"
                    + "Please check www.mbc-ontology.org for recent updates."
                    + "\r\n" + "\r\n"
                    + "See www.iyengarlab.org/mbcpathnet for additional information.",
                    Form_default_settings);
                    Forward_button.Visible = false;
                    break;
                default:
                    throw new Exception();
            }
        }

        public void Update_all_graphic_elements()
        { 
            int left_reference_position;
            int right_reference_position;
            int top_reference_position;
            int bottom_reference_position;
            Label my_label;
            MyCheckBox_button my_cbButton;
            Button my_button;

            Overall_panel = Form_default_settings.MyPanelOverallMenu_add_default_parameters(Overall_panel);

            left_reference_position = (int)Math.Round(0.01 * Overall_panel.Width);
            right_reference_position = (int)Math.Round(0.99 * Overall_panel.Width);

            top_reference_position = 0;
            bottom_reference_position = (int)Math.Round(0.06 * Overall_panel.Height);
            my_label = OverallHeadline_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);
            float fraction_for_checkBox_button = 0.07F;
            float fraction_for_forward_backward_buttons_height = 0.06F;
            float fraction_for_write_mbco_button_height = 0.1F;
            float fraction_distance_all_buttons_from_bottom_border = 0.02F;
            float fraction_for_all_buttons = fraction_for_forward_backward_buttons_height + fraction_for_write_mbco_button_height + fraction_distance_all_buttons_from_bottom_border;

            Update_tips_text_and_forward_and_backward_buttons();

            int sizeOfCbButton = (int)Math.Round(0.05F * Overall_panel.Height);

            top_reference_position = (int)Math.Round((float)(Overall_panel.Height - fraction_for_checkBox_button * Overall_panel.Height));
            bottom_reference_position = top_reference_position + sizeOfCbButton;
            right_reference_position = left_reference_position + sizeOfCbButton;
            my_cbButton = this.Demonstration_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            top_reference_position = Demonstration_cbButton.Location.Y - Demonstration_cbButton.Height;
            bottom_reference_position = Demonstration_cbButton.Location.Y;
            right_reference_position = (int)Math.Round(0.5F * this.Overall_panel.Width);
            my_label = this.Demonstration_headline_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            top_reference_position = (int)Math.Round((float)(Overall_panel.Height - fraction_for_all_buttons * Overall_panel.Height));
            bottom_reference_position = (int)Math.Round((float)(top_reference_position + fraction_for_forward_backward_buttons_height * Overall_panel.Height));
            left_reference_position = (int)Math.Round(0.55F * this.Overall_panel.Width);
            right_reference_position = (int)Math.Round(0.74F * this.Overall_panel.Width);
            my_button = this.Backward_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            top_reference_position = (int)Math.Round((float)(Overall_panel.Height - fraction_for_all_buttons * Overall_panel.Height));
            bottom_reference_position = (int)Math.Round((float)(top_reference_position + fraction_for_forward_backward_buttons_height * Overall_panel.Height));
            left_reference_position = (int)Math.Round(0.76F * this.Overall_panel.Width);
            right_reference_position = (int)Math.Round(0.95F * this.Overall_panel.Width);
            my_button = this.Forward_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            top_reference_position = bottom_reference_position;
            bottom_reference_position = (int)Math.Round((float)(top_reference_position + fraction_for_write_mbco_button_height * Overall_panel.Height));
            if (Form_default_settings.Is_mono)
            {
                bottom_reference_position -= (int)Math.Round(0.05 * Overall_panel.Height);
            }
            left_reference_position = (int)Math.Round(0.55F * this.Overall_panel.Width);
            right_reference_position = (int)Math.Round(0.95F * this.Overall_panel.Width);
            my_button = this.Write_mbco_hierarchy_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_reference_position, right_reference_position, top_reference_position, bottom_reference_position);

            Update_demonstration_cbLabel();
        }

        public void Update_demonstration_cbLabel()
        {
            bool cbButton_pressed = this.Demonstration_cbButton.Checked;
            if (cbButton_pressed)
            {
                this.Demonstration_cbMyPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Item is selected");
            }
            else
            {
                this.Demonstration_cbMyPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Item is not selected");
            }
            int left_reference_position = Demonstration_cbButton.Location.X + Demonstration_cbButton.Width;
            int top_reference_position = Demonstration_cbButton.Location.Y;
            int bottom_reference_position = Demonstration_cbButton.Location.Y + Demonstration_cbButton.Height;
            int right_reference_position = Write_mbco_hierarchy_button.Location.X;
            Demonstration_cbMyPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_reference_position, top_reference_position, right_reference_position, bottom_reference_position, Form_default_settings);
        }

        public void Forward_or_backward_button_pressed(int add_to_page_no)
        {
            Shown_page += add_to_page_no;
            if (Shown_page > 3) { Shown_page = 3; }
            else if (Shown_page < 0) { Shown_page = 0; }
            Update_tips_text_and_forward_and_backward_buttons();
        }
        public void Write_mbco_hierarchy_button_pressed(Graph_editor_enum graphEditor)
        {
            Write_mbco_hierarchy_button.BackColor = Form_default_settings.Color_button_pressed_back;
            Write_mbco_hierarchy_button.ForeColor = Form_default_settings.Color_button_pressed_fore;
            Write_mbco_hierarchy_button.Refresh();
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            string ontology_subdirectory = gdf.Ontology_hierarchy_subdirectory;
            string ontology_fileName = gdf.Get_name_for_ontology_hierarchy(Mbco_parent_child_network.Ontology);
            string[] legend_dataset_nodes = new string[0];
            Mbco_parent_child_network.Write_yED_nw_in_results_directory_with_nodes_colored_by_level_and_return_if_interrupted(Mbco_parent_child_network.Ontology, Ontology_overview_network_enum.Parent_child_hierarchy, ontology_subdirectory, ontology_fileName, legend_dataset_nodes, graphEditor, ProgressReport);
            Write_mbco_hierarchy_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            Write_mbco_hierarchy_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
        }
    }
}
