using Common_functions.Array_own;
using Common_functions.Form_tools;
using Common_functions.Global_definitions;
using Common_functions.Text;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows_forms;
using Windows_forms_customized_tools;
using ZedGraph;

namespace ClassLibrary1.Results_userInterface
{
    enum Enrichment_results_enum { E_m_p_t_y, Bardiagram_standard, Bardiagram_dynamic, Heatmap_standard, Heatmap_dynamic, Timeline_standard }

    class Results_userInterface_class
    {
        MyPanel Overall_panel { get; set; }
        System.Windows.Forms.Label Overall_headline_label { get; set; }
        System.Windows.Forms.Label IntegrationGroup_label { get; set; }
        MyPanel ControlCommandPanel { get; set; }
        OwnListBox IntegrationGroup_listBox { get; set; }
        Dictionary<Enrichment_results_enum, System.Windows.Forms.Label> Enrichment_showLabel_dict { get; set; }
        Dictionary<Enrichment_results_enum, MyCheckBox_button> EnrichmentResults_cbButton_dict { get; set; }
        Dictionary<Enrichment_results_enum, System.Windows.Forms.Label> EnrichmentResults_cbLabel_dict { get; set; }
        System.Windows.Forms.Label Directory_headline_label { get; set; }
        MyPanel_textBox Directory_myPanelTextBox { get; set; }
        System.Windows.Forms.Label Directory_expl_label { get; set; }
        private MyCheckBox_button AddResultsToControl_cbButton { get; set; }
        private System.Windows.Forms.Label AddResultsToControl_cbLabel { get; set; }

        ZedGraphControl Results_control { get; set; }
        MyPanel Results_visualization_panel { get; set; }
        MyPanel_label Visualization_integrationGroup_myPanelLabel { get; set; }
        Button Previous_button { get; set; }
        Button Next_button { get; set; }
        MyPanel_label Position_myPanelLabel { get; set; }
        Form1_default_settings_class Form_default_settings { get; set; }
        Dictionary<string,Dictionary<Enrichment_results_enum, GraphPane[]>> IntegrationGroup_enrichmentResults_graphPane_dict { get; set; }
        Dictionary<string,Dictionary<Enrichment_results_enum, int[]>> IntegrationGroup_enrichmentResults_graphPaneSelectedIndexes_dict { get; set; }
        Dictionary<Enrichment_results_enum, int> EnrichmentResults_maxIndexCountGraphPanel_dict { get; set; }
        private string Selected_integrationGroup { get; set; }
        private Enrichment_results_enum Activated_enrichment_results { get; set; }
        private ProgressReport_interface_class ProgressReport { get; set; }

        public Results_userInterface_class(MyPanel overall_panel,
                                           System.Windows.Forms.Label overall_headline_label,
                                           MyPanel controlCommandPanel,
                                           System.Windows.Forms.Label integrationGroup_label,
                                           OwnListBox integrationGroup_listBox,
                                           System.Windows.Forms.Label bardiagram_show_label,
                                           MyCheckBox_button bardiagram_standard_cbButton,
                                           System.Windows.Forms.Label bardiagram_standard_cbLabel,
                                           MyCheckBox_button bardiagram_dynamic_cbButton,
                                           System.Windows.Forms.Label bardiagram_dynamic_cbLabel,
                                           System.Windows.Forms.Label heatmap_show_label,
                                           MyCheckBox_button heatmap_standard_cbButton,
                                           System.Windows.Forms.Label heatmap_standard_cbLabel,
                                           MyCheckBox_button heatmap_dynamic_cbButton,
                                           System.Windows.Forms.Label heatmap_dynamic_cbLabel,
                                           System.Windows.Forms.Label timeline_show_label,
                                           MyCheckBox_button timeline_standard_cbButton,
                                           System.Windows.Forms.Label timeline_standard_cbLabel,
                                           System.Windows.Forms.Label directory_headline_label,
                                           MyPanel_textBox directory_myPanelTextBox,
                                           System.Windows.Forms.Label directory_expl_label,
                                           MyCheckBox_button addResultsToControl_cbButton,
                                           System.Windows.Forms.Label addResultsToControl_cbLabel,
                                           MyPanel results_visualization_panel,
                                           MyPanel_label visualization_integrationGroup_myPanelLabel,
                                           ZedGraphControl results_control,
                                           Button previous_button,
                                           Button next_button,
                                           MyPanel_label position_myPanelLabel,
                                           ProgressReport_interface_class progressReport,
                                           Form1_default_settings_class form_default_settings
                                          )
        {
            IntegrationGroup_enrichmentResults_graphPane_dict = new Dictionary<string, Dictionary<Enrichment_results_enum, GraphPane[]>>();
            IntegrationGroup_enrichmentResults_graphPaneSelectedIndexes_dict = new Dictionary<string, Dictionary<Enrichment_results_enum, int[]>>();
            this.ControlCommandPanel = controlCommandPanel;
            this.Overall_panel = overall_panel;
            this.Overall_headline_label = overall_headline_label;
            this.IntegrationGroup_label = integrationGroup_label;
            this.IntegrationGroup_listBox = integrationGroup_listBox;
            this.EnrichmentResults_cbButton_dict = new Dictionary<Enrichment_results_enum, MyCheckBox_button>();
            this.EnrichmentResults_cbButton_dict.Add(Enrichment_results_enum.Bardiagram_standard, bardiagram_standard_cbButton);
            this.EnrichmentResults_cbButton_dict.Add(Enrichment_results_enum.Bardiagram_dynamic, bardiagram_dynamic_cbButton);
            this.EnrichmentResults_cbButton_dict.Add(Enrichment_results_enum.Heatmap_standard, heatmap_standard_cbButton);
            this.EnrichmentResults_cbButton_dict.Add(Enrichment_results_enum.Heatmap_dynamic, heatmap_dynamic_cbButton);
            this.EnrichmentResults_cbButton_dict.Add(Enrichment_results_enum.Timeline_standard, timeline_standard_cbButton);
            this.EnrichmentResults_cbLabel_dict = new Dictionary<Enrichment_results_enum, System.Windows.Forms.Label>();
            this.EnrichmentResults_cbLabel_dict.Add(Enrichment_results_enum.Bardiagram_standard, bardiagram_standard_cbLabel);
            this.EnrichmentResults_cbLabel_dict.Add(Enrichment_results_enum.Bardiagram_dynamic, bardiagram_dynamic_cbLabel);
            this.EnrichmentResults_cbLabel_dict.Add(Enrichment_results_enum.Heatmap_standard, heatmap_standard_cbLabel);
            this.EnrichmentResults_cbLabel_dict.Add(Enrichment_results_enum.Heatmap_dynamic, heatmap_dynamic_cbLabel);
            this.EnrichmentResults_cbLabel_dict.Add(Enrichment_results_enum.Timeline_standard, timeline_standard_cbLabel);
            this.Enrichment_showLabel_dict = new Dictionary<Enrichment_results_enum, System.Windows.Forms.Label>();
            this.Enrichment_showLabel_dict.Add(Enrichment_results_enum.Bardiagram_standard, bardiagram_show_label);
            this.Enrichment_showLabel_dict.Add(Enrichment_results_enum.Heatmap_standard, heatmap_show_label);
            this.Enrichment_showLabel_dict.Add(Enrichment_results_enum.Timeline_standard, timeline_show_label);
            this.AddResultsToControl_cbButton = addResultsToControl_cbButton;
            this.AddResultsToControl_cbLabel = addResultsToControl_cbLabel;

            this.Visualization_integrationGroup_myPanelLabel = visualization_integrationGroup_myPanelLabel;

            this.Results_control = results_control;
            this.Results_control.IsEnableHEdit = false;
            this.Results_control.IsEnableHPan= false;
            this.Results_control.IsEnableHZoom = false;
            this.Results_control.IsEnableSelection= false;
            this.Results_control.IsEnableVEdit= false;
            this.Results_control.IsEnableVPan = false;
            this.Results_control.IsEnableVZoom= false;
            this.Results_control.IsEnableWheelZoom= false;
            this.Results_control.IsEnableZoom= false;
            this.Results_control.IsShowVScrollBar = false;
            this.Results_visualization_panel = results_visualization_panel;
            this.Previous_button = previous_button;
            this.Next_button = next_button;
            this.Position_myPanelLabel = position_myPanelLabel;
            this.Form_default_settings = form_default_settings;
            this.Directory_headline_label = directory_headline_label;
            this.Directory_myPanelTextBox = directory_myPanelTextBox;
            this.Directory_expl_label = directory_expl_label;

            EnrichmentResults_maxIndexCountGraphPanel_dict = new Dictionary<Enrichment_results_enum, int>();
            EnrichmentResults_maxIndexCountGraphPanel_dict.Add(Enrichment_results_enum.Bardiagram_standard, 0);
            EnrichmentResults_maxIndexCountGraphPanel_dict.Add(Enrichment_results_enum.Bardiagram_dynamic, 0);
            EnrichmentResults_maxIndexCountGraphPanel_dict.Add(Enrichment_results_enum.Heatmap_standard, 0);
            EnrichmentResults_maxIndexCountGraphPanel_dict.Add(Enrichment_results_enum.Heatmap_dynamic, 0);
            EnrichmentResults_maxIndexCountGraphPanel_dict.Add(Enrichment_results_enum.Timeline_standard, 3);

            ProgressReport = progressReport;

            AddResultsToControl_cbButton.SilentChecked = true;
            Visualization_integrationGroup_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("");
            Selected_integrationGroup = "";
            Activated_enrichment_results = Enrichment_results_enum.E_m_p_t_y;

            Update_all_graphic_elements();
        }

        #region Upadate graphical elements
        public void Update_all_graphic_elements()
        {
            Update_optionsPanel();
            Update_results_window();
        }
        private void Update_optionsPanel()
        {
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            System.Windows.Forms.Label my_label;
            MyCheckBox_button cbButton;
            OwnListBox my_listBox;
            MyPanel myPanel;

            Form_default_settings.MyPanelOverallMenu_add_default_parameters(this.Overall_panel);
            int shared_overall_left_reference_border = (int)Math.Round(0.05F * this.Overall_panel.Width);
            int shared_ovarlll_right_reference_border = this.Overall_panel.Width - shared_overall_left_reference_border;

            left_referenceBorder = 0;
            right_referenceBorder = this.Overall_panel.Width;
            top_referenceBorder = (int)Math.Round(0.05F * this.Overall_panel.Height);
            bottom_referenceBorder = (int)Math.Round(0.65F * this.Overall_panel.Height);
            myPanel = ControlCommandPanel;
            Form_default_settings.MyPanelDefaultBlackFrame_add_default_parameters(myPanel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = shared_overall_left_reference_border;
            right_referenceBorder = shared_ovarlll_right_reference_border;
            top_referenceBorder = 0;
            bottom_referenceBorder = ControlCommandPanel.Location.Y;
            my_label = Overall_headline_label;
            Form_default_settings.LabelHeadline_adjust_to_given_positions_and_center_x_and_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            #region Set graphical objectes within control command panel
            int shared_left_reference_border = (int)Math.Round(0.02F * this.ControlCommandPanel.Width);
            int shared_right_reference_border = this.ControlCommandPanel.Width - shared_left_reference_border;
            int shared_cbButton_widthHeight = (int)Math.Round(0.1F * this.ControlCommandPanel.Height);

            Dictionary<Enrichment_results_enum, int> enrichmentAlgorith_locationY_dict = new Dictionary<Enrichment_results_enum, int>();
            enrichmentAlgorith_locationY_dict.Add(Enrichment_results_enum.Bardiagram_standard, (int)Math.Round(0.3F*ControlCommandPanel.Height));
            enrichmentAlgorith_locationY_dict.Add(Enrichment_results_enum.Bardiagram_dynamic, (int)Math.Round(0.4F * ControlCommandPanel.Height));
            enrichmentAlgorith_locationY_dict.Add(Enrichment_results_enum.Heatmap_standard, (int)Math.Round(0.575F * ControlCommandPanel.Height));
            enrichmentAlgorith_locationY_dict.Add(Enrichment_results_enum.Heatmap_dynamic, (int)Math.Round(0.675F * ControlCommandPanel.Height));
            enrichmentAlgorith_locationY_dict.Add(Enrichment_results_enum.Timeline_standard, (int)Math.Round(0.85F * ControlCommandPanel.Height));

            left_referenceBorder = shared_left_reference_border;
            right_referenceBorder = shared_right_reference_border;
            top_referenceBorder = (int)Math.Round(0.08F * ControlCommandPanel.Height);
            bottom_referenceBorder = (int)Math.Round(0.18F * ControlCommandPanel.Height);
            my_listBox = IntegrationGroup_listBox;
            Form_default_settings.MyListBoxOneLine_add_default_parameter_and_adjust_to_referenceBorders(my_listBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = shared_left_reference_border;
            right_referenceBorder = shared_right_reference_border;
            top_referenceBorder = 0;
            bottom_referenceBorder = IntegrationGroup_listBox.Location.Y;
            my_label = IntegrationGroup_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_lowerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            Enrichment_results_enum[] enrichmentAlgorithms = enrichmentAlgorith_locationY_dict.Keys.ToArray();
            Enrichment_results_enum enrichmentAlgorithm;
            int enrichment_algorithm_length = enrichmentAlgorithms.Length;
            for (int indexEA=0; indexEA < enrichment_algorithm_length; indexEA++)
            {
                enrichmentAlgorithm = enrichmentAlgorithms[indexEA];
                cbButton = EnrichmentResults_cbButton_dict[enrichmentAlgorithm];
                my_label = EnrichmentResults_cbLabel_dict[enrichmentAlgorithm];
                
                left_referenceBorder = shared_left_reference_border;
                right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight;
                top_referenceBorder = enrichmentAlgorith_locationY_dict[enrichmentAlgorithm];
                bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight;
                Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

                left_referenceBorder = cbButton.Location.X + cbButton.Width;
                right_referenceBorder = shared_right_reference_border;
                top_referenceBorder = cbButton.Location.Y;
                bottom_referenceBorder = cbButton.Location.Y + cbButton.Height;
                Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

                if (Enrichment_showLabel_dict.ContainsKey(enrichmentAlgorithm))
                {
                    my_label = Enrichment_showLabel_dict[enrichmentAlgorithm];
                    left_referenceBorder = cbButton.Location.X;
                    right_referenceBorder = shared_right_reference_border;
                    top_referenceBorder = cbButton.Location.Y - cbButton.Height;
                    bottom_referenceBorder = cbButton.Location.Y;
                    Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
                }
            }
            #endregion

            left_referenceBorder = shared_overall_left_reference_border;
            right_referenceBorder = left_referenceBorder + shared_cbButton_widthHeight;
            top_referenceBorder = (int)Math.Round(0.94F * this.Overall_panel.Height);
            bottom_referenceBorder = top_referenceBorder + shared_cbButton_widthHeight;
            cbButton = AddResultsToControl_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            int topBorder_for_directoryInfo = ControlCommandPanel.Location.Y + ControlCommandPanel.Height;
            int bottomBorder_for_directoryInfo = AddResultsToControl_cbButton.Location.Y;
            int height_of_each_directory_row = (int)Math.Round(0.25F * (bottomBorder_for_directoryInfo - topBorder_for_directoryInfo));

            left_referenceBorder = shared_overall_left_reference_border;
            right_referenceBorder = shared_ovarlll_right_reference_border;
            top_referenceBorder = topBorder_for_directoryInfo;
            bottom_referenceBorder = top_referenceBorder + height_of_each_directory_row;
            my_label = Directory_headline_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            Set_results_directory_label("");
        }
        private void Update_results_window()
        {
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            Button my_button;

            this.Form_default_settings.MyPanelResultsVisualization_add_default_parameters(Results_visualization_panel);
            left_referenceBorder = (int)Math.Round(0.05F * Results_visualization_panel.Width);
            right_referenceBorder = (int)Math.Round(1F * Results_visualization_panel.Width);
            top_referenceBorder = (int)Math.Round(0.06F * Results_visualization_panel.Height);
            bottom_referenceBorder = (int)Math.Round(0.92F * Results_visualization_panel.Height);
            this.Results_control.Location = new System.Drawing.Point(left_referenceBorder, top_referenceBorder);
            this.Results_control.MasterPane.Border.Color = Color.White;
            this.Results_control.MasterPane.Border.IsVisible = true;
            this.Results_control.MasterPane.PaneList.Clear();
            this.Results_control.MasterPane.Fill.Color = Color.White;
            this.Results_control.Size = new System.Drawing.Size(right_referenceBorder - left_referenceBorder, bottom_referenceBorder - top_referenceBorder);

            top_referenceBorder = (int)Math.Round(0.94F * Results_visualization_panel.Height);
            bottom_referenceBorder = (int)Math.Round(0.99F * Results_visualization_panel.Height);
            if (Form_default_settings.Is_mono)
            {
                int move_height = (int)Math.Round(0.015F * Results_visualization_panel.Height);
                top_referenceBorder -= move_height;
                bottom_referenceBorder -= move_height;
            }

            left_referenceBorder = (int)Math.Round(0.17F * Results_visualization_panel.Width);
            right_referenceBorder = (int)Math.Round(0.298F * Results_visualization_panel.Width);
            my_button = Previous_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = (int)Math.Round(0.302F * Results_visualization_panel.Width);
            right_referenceBorder = (int)Math.Round(0.43F * Results_visualization_panel.Width);
            my_button = Next_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            Adjust_fontSize_of_position_label();
            Update_addResultsToControl_cbLabel();
        }
        #endregion

        public void Set_visibility(bool visible)
        {
            this.Overall_panel.Visible = visible;
            Set_visibility_of_checkBoxes_and_labels();
            Update_masterPane();
            this.Overall_panel.Refresh();
        }

        public void IntegrationGroup_listBox_selection_changed()
        {
            this.Selected_integrationGroup = IntegrationGroup_listBox.SelectedItem.ToString();
            Set_visibility_of_checkBoxes_and_labels();
            Update_masterPane();
        }
        public void Set_visibility_of_checkBoxes_and_labels()
        {
            Enrichment_results_enum[] all_enrichment_results = EnrichmentResults_cbButton_dict.Keys.ToArray();
            foreach (Enrichment_results_enum enrichment_results in all_enrichment_results)
            {
                if (   (IntegrationGroup_enrichmentResults_graphPane_dict.ContainsKey(Selected_integrationGroup))
                    && (IntegrationGroup_enrichmentResults_graphPane_dict[Selected_integrationGroup].ContainsKey(enrichment_results))
                    && (IntegrationGroup_enrichmentResults_graphPane_dict[Selected_integrationGroup][enrichment_results].Length > 0))
                {
                    EnrichmentResults_cbButton_dict[enrichment_results].Visible = true;
                    EnrichmentResults_cbLabel_dict[enrichment_results].Visible = true;
                    Visualization_integrationGroup_myPanelLabel.Visible = true;
                }
                else
                {
                    EnrichmentResults_cbButton_dict[enrichment_results].Visible = false;
                    EnrichmentResults_cbLabel_dict[enrichment_results].Visible = false;
                    Visualization_integrationGroup_myPanelLabel.Visible = false;
                }
            }
            ControlCommandPanel.Visible = true;// AddResultsToControl_cbButton.Checked;
            //Update_addResultsToControl_cbLabel();
        }

        #region Buttons in options panel and visualization panel
        public void Enrichment_results_checkBox_button_pressed(Enrichment_results_enum activated_enrichment_results)
        {
            Activated_enrichment_results = activated_enrichment_results;
            Enrichment_results_enum[] all_enrichment_results = EnrichmentResults_cbButton_dict.Keys.ToArray();
            foreach (Enrichment_results_enum enrichment_results in all_enrichment_results)
            {
                if (!enrichment_results.Equals(Activated_enrichment_results))
                {
                    EnrichmentResults_cbButton_dict[enrichment_results].SilentChecked = false;
                }
                else
                {
                    EnrichmentResults_cbButton_dict[enrichment_results].SilentChecked = true;
                }
            }
            Update_masterPane();
        }
        public void Next_button_pressed()
        {
            this.Next_button.BackColor = Form_default_settings.Color_button_pressed_back;
            this.Next_button.ForeColor = Form_default_settings.Color_button_pressed_fore;

            int indexSteps = EnrichmentResults_maxIndexCountGraphPanel_dict[Activated_enrichment_results] + 1;
            int[] nextIndexes = IntegrationGroup_enrichmentResults_graphPaneSelectedIndexes_dict[Selected_integrationGroup][Activated_enrichment_results];
            if (nextIndexes[0] + indexSteps < IntegrationGroup_enrichmentResults_graphPane_dict[Selected_integrationGroup][Activated_enrichment_results].Length)
            {
                int oldIndexes_length = nextIndexes.Length;
                for (int indexIndex = 0; indexIndex < oldIndexes_length; indexIndex++)
                {
                    nextIndexes[indexIndex] += indexSteps;
                }
                IntegrationGroup_enrichmentResults_graphPaneSelectedIndexes_dict[Selected_integrationGroup][Activated_enrichment_results] = nextIndexes;
            }
            Update_masterPane();
            this.Next_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Next_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
        }
        public void Previous_button_pressed()
        {
            this.Previous_button.BackColor = Form_default_settings.Color_button_pressed_back;
            this.Previous_button.ForeColor = Form_default_settings.Color_button_pressed_fore;

            int indexSteps = EnrichmentResults_maxIndexCountGraphPanel_dict[Activated_enrichment_results] + 1;
            int[] nextIndexes = IntegrationGroup_enrichmentResults_graphPaneSelectedIndexes_dict[Selected_integrationGroup][Activated_enrichment_results];
            if (nextIndexes[0] - indexSteps >= 0)
            {
                int oldIndexes_length = nextIndexes.Length;
                for (int indexIndex = 0; indexIndex < oldIndexes_length; indexIndex++)
                {
                    nextIndexes[indexIndex] -= indexSteps;
                }
                IntegrationGroup_enrichmentResults_graphPaneSelectedIndexes_dict[Selected_integrationGroup][Activated_enrichment_results] = nextIndexes;
            }
            Update_masterPane();
            this.Previous_button.BackColor = Form_default_settings.Color_button_notPressed_back;
            this.Previous_button.ForeColor = Form_default_settings.Color_button_notPressed_fore;
        }
        #endregion

        #region Update labels and graphic elements of labels
        public void Set_results_directory_label(string results_directory_label)
        {
            Global_directory_and_file_class gd = new Global_directory_and_file_class();
            Directory_myPanelTextBox.Back_color = Form_default_settings.Color_panel_backColor;
            Directory_myPanelTextBox.TextColor = Form_default_settings.Color_label_foreColor;
            Directory_myPanelTextBox.Set_silent_text_adjustFontSize_and_refresh(results_directory_label.Replace(gd.Delimiter.ToString(), gd.Delimiter + " "),Form_default_settings);

            int available_height_space = this.AddResultsToControl_cbButton.Location.Y - Directory_headline_label.Location.Y - Directory_headline_label.Height;

            int left_referenceBorder = (int)Math.Round(0.05F * this.Overall_panel.Width);
            int right_referenceBorder = this.Overall_panel.Width - left_referenceBorder;
            int top_referenceBorder = Directory_headline_label.Location.Y + Directory_headline_label.Height;
            int bottom_referenceBorder = top_referenceBorder + (int)Math.Round(0.66F*available_height_space);
            Directory_myPanelTextBox.Set_left_top_right_bottom_position(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);

            top_referenceBorder = bottom_referenceBorder;
            bottom_referenceBorder = this.AddResultsToControl_cbButton.Location.Y;
            System.Windows.Forms.Label my_label = Directory_expl_label;
            Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

        }
        private void Adjust_fontSize_of_position_label()
        {
            int top_referenceBorder = Previous_button.Location.Y;
            int bottom_referenceBorder = Previous_button.Location.Y + Previous_button.Height;
            int left_referenceBorder = Next_button.Location.X + Next_button.Width;
            int right_referenceBorder = Results_visualization_panel.Width;
            Position_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);
        }
        private void Update_integrationGroup_in_visualization_window()
        {
            if (String.IsNullOrEmpty(Selected_integrationGroup))
            {
                this.Visualization_integrationGroup_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("");
            }
            else
            {
                this.Visualization_integrationGroup_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize("Integration group: " + (string)this.Selected_integrationGroup.Clone());
            }
            int top_referenceBorder = 0;
            int bottom_referenceBorder = this.Results_control.Location.Y;
            int left_referenceBorder = 0;
            int right_referenceBorder = Results_visualization_panel.Width;
            Visualization_integrationGroup_myPanelLabel.TextAlign = ContentAlignment.MiddleCenter;
            Visualization_integrationGroup_myPanelLabel.Font_style = FontStyle.Bold;
            Visualization_integrationGroup_myPanelLabel.Set_left_top_right_bottom_position_considere_distanceReferenceBorder_and_colors_and_adjust_fontSize(left_referenceBorder, top_referenceBorder, right_referenceBorder, bottom_referenceBorder, Form_default_settings);
        }

        public void Update_addResultsToControl_cbLabel()
        {
            if (AddResultsToControl_cbButton.Checked)
            {
                AddResultsToControl_cbLabel.Text = "Add next results to this user interface";
            }
            else
            {
                AddResultsToControl_cbLabel.Text = "Add next results to this user interface";
            }
            AddResultsToControl_cbLabel.Refresh();
            int top_referenceBorder = AddResultsToControl_cbButton.Location.Y;
            int bottom_referenceBorder = AddResultsToControl_cbButton.Location.Y + AddResultsToControl_cbButton.Height;
            int left_referenceBorder = AddResultsToControl_cbButton.Location.X + AddResultsToControl_cbButton.Width;
            int right_referenceBorder = Overall_panel.Width;
            System.Windows.Forms.Label my_label = AddResultsToControl_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
        }
        #endregion

        #region Update graphPanes and masterPane
        protected GraphPane Generate_empty_graphPane()
        {
            GraphPane empty_graphPane = new GraphPane();
            empty_graphPane.XAxis.IsVisible = false;
            empty_graphPane.X2Axis.IsVisible = false;
            empty_graphPane.YAxis.IsVisible = false;
            empty_graphPane.Y2Axis.IsVisible = false;
            empty_graphPane.XAxis.MajorGrid.IsVisible = false;
            empty_graphPane.XAxis.MinorGrid.IsVisible = false;
            empty_graphPane.X2Axis.MajorGrid.IsVisible = false;
            empty_graphPane.X2Axis.MinorGrid.IsVisible = false;
            empty_graphPane.YAxis.MajorGrid.IsVisible = false;
            empty_graphPane.YAxis.MinorGrid.IsVisible = false;
            empty_graphPane.Y2Axis.MajorGrid.IsVisible = false;
            empty_graphPane.Y2Axis.MinorGrid.IsVisible = false;
            empty_graphPane.Border.IsVisible = false;
            empty_graphPane.Chart.Border.IsVisible = false;
            return empty_graphPane;
        }
        public void Update_masterPane()
        {
            this.Results_control.MasterPane.PaneList.Clear();
            int max_index = -1;
            if (  (IntegrationGroup_enrichmentResults_graphPane_dict.ContainsKey(Selected_integrationGroup))
                &&(IntegrationGroup_enrichmentResults_graphPane_dict[Selected_integrationGroup].ContainsKey(Activated_enrichment_results)))
            { max_index = IntegrationGroup_enrichmentResults_graphPane_dict[Selected_integrationGroup][Activated_enrichment_results].Length - 1; }
            if (max_index < 0)
            {
                this.Next_button.Visible = false;
                this.Previous_button.Visible = false;
                this.Position_myPanelLabel.Visible = false;
            }
            else
            {
                int[] indexesSelected = this.IntegrationGroup_enrichmentResults_graphPaneSelectedIndexes_dict[Selected_integrationGroup][Activated_enrichment_results];
                StringBuilder index_sb = new StringBuilder();
                List<int> shownIndexes_plus_one = new List<int>();
                foreach (int indexSelected in indexesSelected)
                {
                    if (indexSelected <= max_index)
                    {
                        this.Results_control.MasterPane.PaneList.Add(this.IntegrationGroup_enrichmentResults_graphPane_dict[Selected_integrationGroup][Activated_enrichment_results][indexSelected]);
                        shownIndexes_plus_one.Add(indexSelected + 1);
                    }
                }
                int emptyGraphPanes_count = this.EnrichmentResults_maxIndexCountGraphPanel_dict[Activated_enrichment_results] - shownIndexes_plus_one.Count;
                for (int indexEmpty = 0; indexEmpty <= emptyGraphPanes_count;indexEmpty++)
                {
                    this.Results_control.MasterPane.PaneList.Add(Generate_empty_graphPane());
                }
                this.Results_control.MasterPane.Rect = new RectangleF(0, 0, this.Results_control.Width, this.Results_control.Height);
                int graphPanes_length = this.Results_control.MasterPane.PaneList.Count;
                int rows_count = 1;
                int cols_count = 1;
                while (rows_count * cols_count < graphPanes_length)
                {
                    if (rows_count<=cols_count) { rows_count++; }
                    else { cols_count++; }
                }
                List<float> cols_proportions = new List<float>();
                List<float> rows_proportions = new List<float>();
                float empty_row_proportion = 0.1F;
                float single_data_row_proportion = (1F - empty_row_proportion) / rows_count;
                float single_data_col_proportion = 1F / cols_count;
                for (int indexRow = 0; indexRow < rows_count; indexRow++)
                {
                    rows_proportions.Add(single_data_row_proportion);
                }
                rows_proportions.Add(empty_row_proportion);
                List<int> cols_each_row = new List<int>();
                for (int indexCol = 0; indexCol < cols_count; indexCol++)
                {
                    cols_proportions.Add(single_data_col_proportion);
                    cols_each_row.Add(cols_count);
                }
                cols_each_row.Add(cols_count);
                using (Graphics g = this.Results_control.CreateGraphics())
                {
                    this.Results_control.MasterPane.ReSize(g);
                    this.Results_control.MasterPane.Rect = new RectangleF(0, 0, (int)Math.Round(0.9F * Results_visualization_panel.Width), (int)Math.Round(0.9F * Results_visualization_panel.Height));
                    this.Results_control.MasterPane.SetLayout(g, true, cols_each_row.ToArray(), rows_proportions.ToArray());
                    this.Results_control.MasterPane.DoLayout(g);
                }
                int shown_indexes_length = shownIndexes_plus_one.Count;
                if (shown_indexes_length == 1)
                { index_sb.AppendFormat("Figure {0}", shownIndexes_plus_one[0]); }
                else { index_sb.AppendFormat("Figures {0}-{1}", shownIndexes_plus_one[0], shownIndexes_plus_one[shownIndexes_plus_one.Count - 1]); }
                index_sb.AppendFormat(" of {0} figures", max_index + 1);
                this.Next_button.Visible = true;
                this.Previous_button.Visible = true;
                this.Position_myPanelLabel.Visible = true;
                switch (Activated_enrichment_results)
                {
                    case Enrichment_results_enum.Bardiagram_standard:
                    case Enrichment_results_enum.Bardiagram_dynamic:
                    case Enrichment_results_enum.Timeline_standard:
                    case Enrichment_results_enum.Heatmap_standard:
                    case Enrichment_results_enum.Heatmap_dynamic:
                        index_sb.AppendFormat(" (Legend at the end)");
                        break;
                    default:
                        throw new Exception();
                }
                this.Position_myPanelLabel.Set_silent_text_without_adjustment_of_fontSize(index_sb.ToString());
                Adjust_fontSize_of_position_label();
            }
            this.Results_control.MasterPane.Border.IsVisible = true;
            this.Results_control.Invalidate();
            Update_integrationGroup_in_visualization_window();
        }
        public void Clear_all_enrichmentResults_graphPanes()
        {
            this.Directory_myPanelTextBox.Set_silent_text_adjustFontSize_and_refresh("",Form_default_settings);
            this.IntegrationGroup_listBox.Items.Clear();
            IntegrationGroup_enrichmentResults_graphPane_dict.Clear();
            IntegrationGroup_enrichmentResults_graphPaneSelectedIndexes_dict.Clear();
        }
        private GraphPane[] Adjust_graphical_parameter_to_control(Enrichment_results_enum enrichment_results, GraphPane[] graphPanes)
        {
            foreach (GraphPane graphPane in graphPanes)
            {
                switch (enrichment_results)
                {
                    case Enrichment_results_enum.Bardiagram_standard:
                        graphPane.XAxis.Scale.FontSpec = new FontSpec("Arial", 10, Color.Black, false, false, false);
                        graphPane.XAxis.Scale.FontSpec.Border.IsVisible = false;
                        graphPane.XAxis.Scale.IsPreventLabelOverlap = false;
                        graphPane.YAxis.Scale.FontSpec = new FontSpec("Arial", 10, Color.Black, false, false, false);
                        graphPane.YAxis.Scale.FontSpec.Angle = 90;
                        graphPane.YAxis.Scale.FontSpec.Border.IsVisible = false;
                        graphPane.YAxis.Scale.IsPreventLabelOverlap= false;
                        break;
                    case Enrichment_results_enum.Bardiagram_dynamic:
                        graphPane.XAxis.Scale.FontSpec = new FontSpec("Arial", 10, Color.Black, false, false, false);
                        graphPane.XAxis.Scale.FontSpec.Border.IsVisible = false;
                        graphPane.XAxis.Scale.IsPreventLabelOverlap = false;
                        graphPane.YAxis.Scale.FontSpec = new FontSpec("Arial", 8.2F, Color.Black, false, false, false);
                        graphPane.YAxis.Scale.FontSpec.Angle = 90;
                        graphPane.YAxis.Scale.FontSpec.Border.IsVisible = false;
                        graphPane.YAxis.Scale.IsPreventLabelOverlap = false;
                        break;
                    case Enrichment_results_enum.Heatmap_standard:
                    case Enrichment_results_enum.Heatmap_dynamic:
                        float textObjectFontSize = -1;
                        if (graphPane.Title.Text.Equals("Legend"))
                        {
                            graphPane.YAxis.Scale.FontSpec = new FontSpec("Arial", 14, Color.Black, true, false, false);
                            textObjectFontSize = 14;
                        }
                        else
                        {
                            graphPane.YAxis.Scale.FontSpec = new FontSpec("Arial", 10, Color.Black, false, false, false);
                            textObjectFontSize = 11;
                        }
                        graphPane.YAxis.Scale.TextLabels = Text_class.Split_texts_over_multiple_lines(graphPane.YAxis.Scale.TextLabels, 99999, 1);
                        graphPane.YAxis.Scale.FontSpec.Angle = 90;
                        graphPane.YAxis.Scale.FontSpec.StringAlignment = StringAlignment.Far;

                        graphPane.XAxis.Scale.FontSpec = new FontSpec("Arial", 10, Color.Black, false, false, false);
                        graphPane.XAxis.Scale.FontSpec.Border.IsVisible = false;
                        graphPane.XAxis.Scale.IsPreventLabelOverlap = false;
                        graphPane.XAxis.Scale.FontSpec.Angle = 90;
                        graphPane.XAxis.Scale.FontSpec.StringAlignment = StringAlignment.Far;

                        graphPane.YAxis.Scale.FontSpec.StringAlignment = StringAlignment.Far;
                        graphPane.YAxis.Scale.FontSpec.Border.IsVisible = false;
                        graphPane.YAxis.Scale.IsPreventLabelOverlap = false;
                        int obj_list_count = graphPane.GraphObjList.Count;
                        GraphObj obj;



                        for (int indexObj=0; indexObj<obj_list_count;indexObj++)
                        {
                            obj = graphPane.GraphObjList[indexObj];
                            try
                            {
                                TextObj textBox = (TextObj)obj;
                                textBox.FontSpec = new FontSpec("Arial", textObjectFontSize, Color.Black, false, false, false);
                                textBox.FontSpec.Border.IsVisible = false;
                                BoxObj underlying_box = ((BoxObj)graphPane.GraphObjList[indexObj + 1]);
                                textBox.FontSpec.Fill.Color = underlying_box.Fill.Color;
                            }
                            catch { };
                        }
                        break;
                    case Enrichment_results_enum.Timeline_standard:
                        graphPane.Title.FontSpec = new FontSpec("Arial", 28, Color.Black, true, false, false);
                        graphPane.Title.FontSpec.Border.IsVisible = false;
                        graphPane.XAxis.Title.FontSpec = new FontSpec("Arial", 22, Color.Black, false, false, false);
                        graphPane.XAxis.Title.FontSpec.Border.IsVisible = false;
                        graphPane.YAxis.Title.FontSpec = new FontSpec("Arial", 22, Color.Black, false, false, false);
                        graphPane.YAxis.Title.FontSpec.Border.IsVisible = false;
                        graphPane.YAxis.Title.FontSpec.Angle = 180;
                        graphPane.XAxis.Scale.FontSpec = new FontSpec("Arial", 22, Color.Black, false, false, false);
                        graphPane.XAxis.Scale.FontSpec.Border.IsVisible = false;
                        graphPane.XAxis.Scale.IsPreventLabelOverlap = false;
                        graphPane.YAxis.Scale.FontSpec = new FontSpec("Arial", 22, Color.Black, false, false, false);
                        graphPane.YAxis.Scale.FontSpec.Border.IsVisible = false;
                        graphPane.YAxis.Scale.IsPreventLabelOverlap = false;
                        graphPane.YAxis.Scale.FontSpec.Angle = 90;
                        foreach (LineItem line in graphPane.CurveList)
                        {
                            if (  (line.Label.Text.Equals("Positive cutoff line"))
                                || (line.Label.Text.Equals("Negative cutoff line"))
                                || (line.Label.Text.Equals("X-axis")))
                            {
                                line.Line.Width = 1;
                            }
                            else
                            {
                                line.Line.Width = 2;
                                line.Symbol.Size = 7;
                            }
                        }
                        break;
                    default:
                        throw new Exception();
                }
            }
            return graphPanes;
        }
        public void Add_enrichmentResults_graphPanes(Enrichment_results_enum enrichment_results, Dictionary<string,GraphPane[]> integrationGroup_add_graphPanes_dict, ref bool light_up_results)
        {
            if (integrationGroup_add_graphPanes_dict.Keys.Count > 0)
            {

                #region Update Progress report
                string figureSet_name = "";
                switch (enrichment_results)
                {
                    case Enrichment_results_enum.Bardiagram_standard:
                        figureSet_name = "Bardiagrams for standard enrichment analysis";
                        break;
                    case Enrichment_results_enum.Bardiagram_dynamic:
                        figureSet_name = "Bardiagrams for dynamic enrichment analysis";
                        break;
                    case Enrichment_results_enum.Heatmap_standard:
                        figureSet_name = "Heatmaps for standard enrichment analysis";
                        break;
                    case Enrichment_results_enum.Heatmap_dynamic:
                        figureSet_name = "Heatmaps for standard enrichment analysis";
                        break;
                    case Enrichment_results_enum.Timeline_standard:
                        figureSet_name = "Timelines";
                        break;
                    default:
                        throw new Exception();
                }
                string progress_text = "Resizing and adding " + figureSet_name + " to results user interface (Step can be switched off in menu 'Results')";
                ProgressReport.Update_progressReport_text_and_visualization(progress_text);
                #endregion

                string[] integrationGroups = integrationGroup_add_graphPanes_dict.Keys.ToArray();
                string integrationGroup;
                int integrationGroups_length = integrationGroups.Length;
                GraphPane[] add_graphPanes;

                #region Add new integration groups to integration group list box and select first index
                List<string> new_integrationGroups = new List<string>();
                new_integrationGroups.AddRange(integrationGroups);
                int listBox_items_length = IntegrationGroup_listBox.Items.Count;
                for (int indexLB = 0; indexLB < listBox_items_length; indexLB++)
                {
                    new_integrationGroups.Add(IntegrationGroup_listBox.Items[indexLB].ToString());
                }
                IntegrationGroup_listBox.Items.Clear();
                IntegrationGroup_listBox.Items.AddRange(new_integrationGroups.Distinct().OrderBy(l => l).ToArray());
                Selected_integrationGroup = (string)new_integrationGroups[0].Clone();
                IntegrationGroup_listBox.SilentSelectedIndex = IntegrationGroup_listBox.Items.IndexOf(Selected_integrationGroup);
                #endregion

                for (int indexIG = 0; indexIG < integrationGroups_length; indexIG++)
                {
                    integrationGroup = integrationGroups[indexIG];
                    add_graphPanes = integrationGroup_add_graphPanes_dict[integrationGroup];
                    add_graphPanes = Adjust_graphical_parameter_to_control(enrichment_results, add_graphPanes);
                    int maxIndex = Math.Min(add_graphPanes.Length - 1, EnrichmentResults_maxIndexCountGraphPanel_dict[enrichment_results]);
                    int[] selected_indexes = new int[maxIndex + 1];
                    for (int selectedIndex = 0; selectedIndex <= maxIndex; selectedIndex++)
                    {
                        selected_indexes[selectedIndex] = selectedIndex;
                    }
                    if (!IntegrationGroup_enrichmentResults_graphPane_dict.ContainsKey(integrationGroup))
                    {
                        IntegrationGroup_enrichmentResults_graphPane_dict.Add(integrationGroup, new Dictionary<Enrichment_results_enum, GraphPane[]>());
                        IntegrationGroup_enrichmentResults_graphPaneSelectedIndexes_dict.Add(integrationGroup, new Dictionary<Enrichment_results_enum, int[]>());
                    }
                    if (!IntegrationGroup_enrichmentResults_graphPane_dict[integrationGroup].ContainsKey(enrichment_results))
                    {
                        IntegrationGroup_enrichmentResults_graphPane_dict[integrationGroup].Add(enrichment_results, add_graphPanes.ToArray());
                        IntegrationGroup_enrichmentResults_graphPaneSelectedIndexes_dict[integrationGroup].Add(enrichment_results, selected_indexes);
                    }
                    else
                    {
                        IntegrationGroup_enrichmentResults_graphPane_dict[integrationGroup][enrichment_results] = add_graphPanes.ToArray();
                        IntegrationGroup_enrichmentResults_graphPaneSelectedIndexes_dict[integrationGroup][enrichment_results] = selected_indexes;
                    }
                }

                #region Select checkbox with data
                Enrichment_results_enum[] eResults = EnrichmentResults_cbButton_dict.Keys.ToArray();
                foreach (Enrichment_results_enum eResult in eResults)
                {
                    if ((IntegrationGroup_enrichmentResults_graphPane_dict.ContainsKey(Selected_integrationGroup))
                        && (IntegrationGroup_enrichmentResults_graphPane_dict[Selected_integrationGroup].ContainsKey(eResult))
                        && (IntegrationGroup_enrichmentResults_graphPane_dict[Selected_integrationGroup][eResult].Length > 0))
                    {
                        Enrichment_results_checkBox_button_pressed(eResult);
                        break;
                    }
                }

                #endregion

                ProgressReport.Clear_progressReport_text_and_last_entry();
                light_up_results = true;
            }
        }
        #endregion
    }
}
