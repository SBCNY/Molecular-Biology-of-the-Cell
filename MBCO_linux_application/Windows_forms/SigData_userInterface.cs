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
using System.Threading.Tasks;
using Windows_forms_customized_tools;
using System.Windows.Forms;
using Data;
using Common_functions.Global_definitions;
using Common_functions.Form_tools;


namespace ClassLibrary1.SigData_userInterface
{
    class SigData_text_class
    {
        public static string Changes_significance_using_both_cutoffs_text { get { return "Adjust significance\nusing both cutoffs"; } }
        public static string Set_all_genes_to_significant_text { get { return "Define all genes\nas significant"; } }
        public static string Eg_pvalue_text { get { return "eg for p-values"; } }
        public static string Eg_log2foldchange_text { get { return "eg for log2(FCs)"; } }
    }

    class SigData_userInterface_class
    {
        public MyPanel Overall_panel { get; set; }
        public MyPanel SigSelection_panel { get; set; }
        public Label Value1st_headline_label { get; set; }
        public MyCheckBox_button Value1st_higherSig_cbButton { get; set; }
        public Label Value1st_higherSig_cbLabel { get; set; }
        public Label Value1st_higherSig_explanation_label { get; set; }
        public MyCheckBox_button Value1st_lowerSig_cbButton { get; set; }
        public Label Value1st_lowerSig_cbLabel { get; set; }
        public Label Value1st_lowerSig_explanation_label { get; set; }
        public Label Value2nd_headline_label { get; set; }
        public MyCheckBox_button Value2nd_higherSig_cbButton { get; set; }
        public Label Value2nd_higherSig_cbLabel { get; set; }
        public Label Value2nd_higherSig_explanation_label { get; set; }
        public MyCheckBox_button Value2nd_lowerSig_cbButton { get; set; }
        public Label Value2nd_lowerSig_cbLabel { get; set; }
        public Label Value2nd_lowerSig_explanation_label { get; set; }
        public Label SigData_1st_sigCutoff_headline_label { get; set; }
        public Label Value1st_cutoff_label { get; set; }
        public OwnTextBox Value1st_cutoff_ownTextBox { get; set; }
        public Label Value1st_cutoff_expl_label { get; set; }
        public Label Value2nd_cutoff_label { get; set; }
        public OwnTextBox Value2nd_cutoff_ownTextBox { get; set; }
        public Label Value2nd_cutoff_expl_label { get; set; }
        public Label SigData_2nd_sigCutoff_headline_label { get; set; }
        public Label RankBy_top_label { get; set; }
        public Label RankBy_left_label { get; set; }
        public MyCheckBox_button RankBy_1stValue_cbButton { get; set; }
        public Label RankBy_1stValue_cbLabel { get; set; }
        public MyCheckBox_button RankBy_2ndValue_cbButton { get; set; }
        public Label RankBy_2ndValue_cbLabel { get; set; }
        public Label TieBreaker_explanation_label { get; set; }
        public Label KeepTopRanks_left_label { get; set; }
        public OwnTextBox KeepTopRanks_ownTextBox { get; set; }
        public Label KeepTopRanks_right_label { get; set; }
        public MyCheckBox_button Keep_eachDataset_cbButton { get; set; }
        public Label Keep_eachDataset_cbLabel { get; set; }
        public MyCheckBox_button Keep_mergeUpDown_cbButton { get; set; }
        public Label Keep_mergeUpDown_cbLabel { get; set; }
        public MyCheckBox_button DeleteNotSignGenes_cbButton { get; set; }
        public Label DeleteNotSignGenes_cbLabel { get; set; }
        public Label AllGenesSignificant_headline_label { get; set; }
        public MyCheckBox_button AllGenesSignificant_cbButton { get; set; }
        public Label AllGenesSignificant_cbLabel { get; set; }
        public Button ResetSig_button { get; set; }
        public Button ResetParameter_button { get; set; }
        public Label SigSubject_explanation_label { get; set; }
        public User_data_options_class Visualized_options { get; set; }
        private Form1_default_settings_class Form_default_settings { get; set; }


        public SigData_userInterface_class(MyPanel overall_panel,
                                           MyPanel sigSelection_panel,
                                           Label value1st_headline_label,
                                           MyCheckBox_button value1st_higherSig_cbButton,
                                           Label value1st_higherSig_cbLabel,
                                           Label value1st_higherSig_explanation_label,
                                           MyCheckBox_button value1st_lowerSig_cbButton,
                                           Label value1st_lowerSig_cbLabel,
                                           Label value1st_lowerSig_explanation_label,
                                           Label value2nd_headline_label,
                                           MyCheckBox_button value2nd_higherSig_cbButton,
                                           Label value2nd_higherSig_cbLabel,
                                           Label value2nd_higherSig_explanation_label,
                                           MyCheckBox_button value2nd_lowerSig_cbButton,
                                           Label value2nd_lowerSig_cbLabel,
                                           Label value2nd_lowerSig_explanation_label,
                                           Label sigData_1st_sigCutoff_headline_label,
                                           Label value1st_cutoff_label,
                                           OwnTextBox value1st_cutoff_ownTextBox,
                                           Label value1st_cutoff_expl_label,
                                           Label value2nd_cutoff_label,
                                           OwnTextBox value2nd_cutoff_ownTextBox,
                                           Label value2nd_cutoff_expl_label,
                                           Label sigData_2nd_sigCutoff_headline_label,
                                           Label rankBy_top_label,
                                           Label rankBy_left_label,
                                           MyCheckBox_button rankBy_1stValue_cbButton,
                                           Label rankBy_1stValue_cbLabel,
                                           MyCheckBox_button rankBy_2ndValue_cbButton,
                                           Label rankBy_2ndValue_cbLabel,
                                           Label tieBreaker_explanation_label,
                                           Label keepTopRanks_left_label,
                                           OwnTextBox keepTopRanks_ownTextBox,
                                           Label keepTopRanks_right_label,
                                           MyCheckBox_button keep_eachDataset_cbButton,
                                           Label keep_eachDataset_cbLabel,
                                           MyCheckBox_button keep_mergeUpDown_cbButton,
                                           Label keep_mergeUpDown_cbLabel,
                                           MyCheckBox_button deleteNotSignGenes_cbButton,
                                           Label deleteNotSignGenes_cbLabel,
                                           Label allGenesSignificant_headline_label,
                                           MyCheckBox_button allGenesSignificant_cbButton,
                                           Label allGenesSignificant_cbLabel,
                                           Button resetSig_button,
                                           Button resetParameter_button,
                                           Label sigSubject_explanation_label,
                                           User_data_options_class custom_data_options,
                                           Form1_default_settings_class form_default_settings)
        {
            Form_default_settings = form_default_settings;

            this.Overall_panel = overall_panel;
            this.Value1st_cutoff_ownTextBox = value1st_cutoff_ownTextBox;
            this.Value2nd_cutoff_ownTextBox = value2nd_cutoff_ownTextBox;
            this.Value1st_cutoff_label = value1st_cutoff_label;
            this.Value1st_cutoff_expl_label = value1st_cutoff_expl_label;
            this.Value2nd_cutoff_label = value2nd_cutoff_label;
            this.Value2nd_cutoff_expl_label = value2nd_cutoff_expl_label;
            this.SigData_1st_sigCutoff_headline_label = sigData_1st_sigCutoff_headline_label;
            this.RankBy_1stValue_cbButton = rankBy_1stValue_cbButton;
            this.RankBy_1stValue_cbLabel = rankBy_1stValue_cbLabel;
            this.RankBy_2ndValue_cbButton = rankBy_2ndValue_cbButton;
            this.RankBy_2ndValue_cbLabel = rankBy_2ndValue_cbLabel;
            this.Overall_panel = overall_panel;
            this.SigSelection_panel = sigSelection_panel;
            this.KeepTopRanks_ownTextBox = keepTopRanks_ownTextBox;
            this.KeepTopRanks_left_label = keepTopRanks_left_label;
            this.KeepTopRanks_right_label = keepTopRanks_right_label;
            this.SigData_2nd_sigCutoff_headline_label = sigData_2nd_sigCutoff_headline_label;
            this.Keep_mergeUpDown_cbButton = keep_mergeUpDown_cbButton;
            this.Keep_mergeUpDown_cbLabel = keep_mergeUpDown_cbLabel;
            this.Keep_eachDataset_cbButton = keep_eachDataset_cbButton;
            this.Keep_eachDataset_cbLabel = keep_eachDataset_cbLabel;
            this.TieBreaker_explanation_label = tieBreaker_explanation_label;
            this.RankBy_top_label = rankBy_top_label;
            this.RankBy_left_label = rankBy_left_label;

            this.ResetSig_button = resetSig_button;
            this.ResetParameter_button = resetParameter_button;
            this.Value1st_higherSig_explanation_label = value1st_higherSig_explanation_label;
            this.Value1st_lowerSig_explanation_label = value1st_lowerSig_explanation_label;
            this.Value2nd_higherSig_explanation_label = value2nd_higherSig_explanation_label;
            this.Value2nd_lowerSig_explanation_label = value2nd_lowerSig_explanation_label;
            this.AllGenesSignificant_headline_label = allGenesSignificant_headline_label;
            this.AllGenesSignificant_cbButton = allGenesSignificant_cbButton;
            this.AllGenesSignificant_cbLabel = allGenesSignificant_cbLabel;
            this.SigSubject_explanation_label = sigSubject_explanation_label;
            this.DeleteNotSignGenes_cbButton = deleteNotSignGenes_cbButton;
            this.DeleteNotSignGenes_cbLabel = deleteNotSignGenes_cbLabel;
            this.Value1st_higherSig_explanation_label = value1st_higherSig_explanation_label;
            this.Value1st_lowerSig_explanation_label = value1st_lowerSig_explanation_label;
            this.Value2nd_higherSig_explanation_label = value2nd_higherSig_explanation_label;
            this.Value2nd_lowerSig_explanation_label = value2nd_lowerSig_explanation_label;
            this.Value1st_higherSig_cbButton = value1st_higherSig_cbButton;
            this.Value1st_higherSig_cbLabel = value1st_higherSig_cbLabel;
            this.Value1st_lowerSig_cbButton = value1st_lowerSig_cbButton;
            this.Value1st_lowerSig_cbLabel = value1st_lowerSig_cbLabel;
            this.Value2nd_higherSig_cbButton = value2nd_higherSig_cbButton;
            this.Value2nd_higherSig_cbLabel = value2nd_higherSig_cbLabel;
            this.Value2nd_lowerSig_cbButton = value2nd_lowerSig_cbButton;
            this.Value2nd_lowerSig_cbLabel = value2nd_lowerSig_cbLabel;
            this.Value1st_headline_label = value1st_headline_label;
            this.Value2nd_headline_label = value2nd_headline_label;

            this.Value1st_higherSig_explanation_label.Text = SigData_text_class.Eg_log2foldchange_text;
            this.Value1st_lowerSig_explanation_label.Text = SigData_text_class.Eg_pvalue_text;
            this.Value2nd_higherSig_explanation_label.Text = SigData_text_class.Eg_log2foldchange_text;
            this.Value2nd_lowerSig_explanation_label.Text = SigData_text_class.Eg_pvalue_text;

            Update_all_graphic_elements(custom_data_options);
        }

        public void Update_all_graphic_elements(User_data_options_class custom_data_options)
        { 
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;
            MyPanel my_panel;
            Label my_label;
            MyCheckBox_button my_cbButton;
            OwnTextBox my_textBox;
            Button my_button;

            this.Overall_panel = Form_default_settings.MyPanelOverallMenu_add_default_parameters(Overall_panel);
            int shared_distance_from_leftRightSide = (int)Math.Round(0.01 * this.Overall_panel.Width);


            #region SigSelection panel
            left_referenceBorder = shared_distance_from_leftRightSide;
            right_referenceBorder = this.Overall_panel.Width - shared_distance_from_leftRightSide;
            top_referenceBorder = 0;
            bottom_referenceBorder = (int)Math.Round(0.77 * this.Overall_panel.Height);
            my_panel = this.SigSelection_panel;
            Form_default_settings.MyPanelDefaultTransparentFrame_add_default_parameters(my_panel, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            int space_for_headline = (int)Math.Round(0.07F * SigSelection_panel.Height);
            int shared_row_height = (int)Math.Round((float)(SigSelection_panel.Height - 4 * space_for_headline)/ 13);
            int shared_height_of_textBoxes = Math.Min(shared_row_height,(int)Math.Round(0.07F * SigSelection_panel.Height));
            int shared_heightWidth_of_checkBoxButtons = Math.Min(shared_row_height, (int)Math.Round(0.05F * SigSelection_panel.Height));
            int half_extra_height_for_cbLabels = (int)Math.Round(0.5F * (shared_row_height - shared_heightWidth_of_checkBoxButtons));
            int running_top_referenceBorder = 0;
            int shared_right_referenceBorder_for_sigIncDec_checkBoxes = (int)Math.Round(0.5F * Overall_panel.Height);

            #region Significance increases/decreases with higher/lower 1st value - headline label
            left_referenceBorder = 0;
            right_referenceBorder = this.SigSelection_panel.Width;
            top_referenceBorder = running_top_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + space_for_headline;
            my_label = this.Value1st_headline_label;
            Form_default_settings.LabelUnderlinedHeadline_adjust_to_given_positions_attach_to_leftPosition_and_center_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += space_for_headline;

            #region Significance increases with higher 1st value - cbButton and cbLabel
            left_referenceBorder = 0;
            right_referenceBorder = left_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            top_referenceBorder = running_top_referenceBorder + half_extra_height_for_cbLabels;
            bottom_referenceBorder = top_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            my_cbButton = Value1st_higherSig_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = Value1st_higherSig_cbButton.Location.X + Value1st_higherSig_cbButton.Width;
            right_referenceBorder = shared_right_referenceBorder_for_sigIncDec_checkBoxes;
            top_referenceBorder = Value1st_higherSig_cbButton.Location.Y - half_extra_height_for_cbLabels;
            bottom_referenceBorder = Value1st_higherSig_cbButton.Location.Y + Value1st_higherSig_cbButton.Height + half_extra_height_for_cbLabels;
            my_label = Value1st_higherSig_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += shared_row_height;

            #region Significance increases with lower 1st value - cbButton and cbLabel
            left_referenceBorder = 0;
            right_referenceBorder = left_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            top_referenceBorder = running_top_referenceBorder + half_extra_height_for_cbLabels;
            bottom_referenceBorder = top_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            my_cbButton = Value1st_lowerSig_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = Value1st_lowerSig_cbButton.Location.X + Value1st_lowerSig_cbButton.Width;
            right_referenceBorder = shared_right_referenceBorder_for_sigIncDec_checkBoxes;
            top_referenceBorder = Value1st_lowerSig_cbButton.Location.Y - half_extra_height_for_cbLabels;
            bottom_referenceBorder = Value1st_lowerSig_cbButton.Location.Y + Value1st_lowerSig_cbButton.Height + half_extra_height_for_cbLabels;
            my_label = Value1st_lowerSig_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += shared_row_height;

            #region Significance increases/decreases with higher/lower 2nd value - headline label
            left_referenceBorder = 0;
            right_referenceBorder = this.SigSelection_panel.Width;
            top_referenceBorder = running_top_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + space_for_headline;
            my_label = this.Value2nd_headline_label;
            Form_default_settings.LabelUnderlinedHeadline_adjust_to_given_positions_attach_to_leftPosition_and_center_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += space_for_headline;

            #region Significance increases with higher 2nd value - cbButton and cbLabel
            left_referenceBorder = 0;
            right_referenceBorder = left_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            top_referenceBorder = running_top_referenceBorder + half_extra_height_for_cbLabels;
            bottom_referenceBorder = top_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            my_cbButton = Value2nd_higherSig_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = Value2nd_higherSig_cbButton.Location.X + Value2nd_higherSig_cbButton.Width;
            right_referenceBorder = shared_right_referenceBorder_for_sigIncDec_checkBoxes;
            top_referenceBorder = Value2nd_higherSig_cbButton.Location.Y - half_extra_height_for_cbLabels;
            bottom_referenceBorder = Value2nd_higherSig_cbButton.Location.Y + Value2nd_higherSig_cbButton.Height + half_extra_height_for_cbLabels;
            my_label = Value2nd_higherSig_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += shared_row_height;

            #region Significance increases with lower 2nd value - cbButton and cbLabel
            left_referenceBorder = 0;
            right_referenceBorder = left_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            top_referenceBorder = running_top_referenceBorder + half_extra_height_for_cbLabels;
            bottom_referenceBorder = top_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            my_cbButton = Value2nd_lowerSig_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = Value2nd_lowerSig_cbButton.Location.X + Value2nd_lowerSig_cbButton.Width;
            right_referenceBorder = shared_right_referenceBorder_for_sigIncDec_checkBoxes;
            top_referenceBorder = Value2nd_lowerSig_cbButton.Location.Y - half_extra_height_for_cbLabels;
            bottom_referenceBorder = Value2nd_lowerSig_cbButton.Location.Y + Value2nd_lowerSig_cbButton.Height + half_extra_height_for_cbLabels;
            my_label = Value2nd_lowerSig_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += shared_row_height;

            #region Significance increases/decreases with higher/lower values - explanation labels
            Dictionary<Label, Label> cbLabel_explLabel_dict = new Dictionary<Label, Label>();
            cbLabel_explLabel_dict.Add(this.Value1st_higherSig_cbLabel, this.Value1st_higherSig_explanation_label);
            cbLabel_explLabel_dict.Add(this.Value1st_lowerSig_cbLabel, this.Value1st_lowerSig_explanation_label);
            cbLabel_explLabel_dict.Add(this.Value2nd_higherSig_cbLabel, this.Value2nd_higherSig_explanation_label);
            cbLabel_explLabel_dict.Add(this.Value2nd_lowerSig_cbLabel, this.Value2nd_lowerSig_explanation_label);

            Label[] labels = cbLabel_explLabel_dict.Keys.ToArray();
            Label current_label;
            foreach (Label current_cbLabel in labels)
            {
                current_label = cbLabel_explLabel_dict[current_cbLabel];
                left_referenceBorder = current_cbLabel.Location.X + current_cbLabel.Width;
                right_referenceBorder = this.SigSelection_panel.Width;
                top_referenceBorder = current_cbLabel.Location.Y;
                bottom_referenceBorder = current_cbLabel.Location.Y + current_cbLabel.Height;
                my_label = current_label;
                Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            }
            #endregion

            #region Significance cutoff headline
            left_referenceBorder = 0;
            right_referenceBorder = SigSelection_panel.Width;
            top_referenceBorder = running_top_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + space_for_headline;
            my_label = this.SigData_1st_sigCutoff_headline_label;
            Form_default_settings.LabelUnderlinedHeadline_adjust_to_given_positions_attach_to_leftPosition_and_center_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += space_for_headline;

            int shared_leftSide_for_cutoff_textBoxes = (int)Math.Round(0.52F * this.SigSelection_panel.Width);
            int shared_topSide_for_cutoff_textBoxes = (int)Math.Round(0.69F * this.SigSelection_panel.Width);

            #region Significance 1stvalue cutoff textBox and label
            left_referenceBorder = shared_leftSide_for_cutoff_textBoxes;
            right_referenceBorder = shared_topSide_for_cutoff_textBoxes;
            top_referenceBorder = running_top_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + shared_height_of_textBoxes;
            my_textBox = this.Value1st_cutoff_ownTextBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += shared_row_height;

            #region Significance 2ndvalue cutoff textBox and label
            left_referenceBorder = shared_leftSide_for_cutoff_textBoxes;
            right_referenceBorder = shared_topSide_for_cutoff_textBoxes;
            top_referenceBorder = running_top_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + shared_height_of_textBoxes;
            my_textBox = this.Value2nd_cutoff_ownTextBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += shared_row_height;

            #region Significance cutoff textBox labels and explanation labels
            Set_value1st_and_2nd_label_and_expl_label_fontSize_and_size();
            #endregion

            #region Rank headline
            left_referenceBorder = 0;
            right_referenceBorder = SigSelection_panel.Width;
            top_referenceBorder = running_top_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + space_for_headline;
            my_label = this.SigData_2nd_sigCutoff_headline_label;
            Form_default_settings.LabelUnderlinedHeadline_adjust_to_given_positions_attach_to_leftPosition_and_center_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += space_for_headline;

            #region Rank by first or second value top label
            left_referenceBorder = 0;
            right_referenceBorder = SigSelection_panel.Width;
            top_referenceBorder = running_top_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + shared_row_height;
            my_label = this.RankBy_top_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += shared_row_height;

            int shared_leftBorder_for_rankBy_cbButtons = (int)Math.Round(0.35F * Overall_panel.Width);

            #region Rank by 1st value cbButton and cbLabel
            left_referenceBorder = shared_leftBorder_for_rankBy_cbButtons;
            right_referenceBorder = left_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            top_referenceBorder = running_top_referenceBorder + half_extra_height_for_cbLabels;
            bottom_referenceBorder = top_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            my_cbButton = this.RankBy_1stValue_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.RankBy_1stValue_cbButton.Location.X + this.RankBy_1stValue_cbButton.Width;
            right_referenceBorder = (int)Math.Round(0.7F * this.SigSelection_panel.Width);
            top_referenceBorder = this.RankBy_1stValue_cbButton.Location.Y - half_extra_height_for_cbLabels;
            bottom_referenceBorder = this.RankBy_1stValue_cbButton.Location.Y + this.RankBy_1stValue_cbButton.Height + half_extra_height_for_cbLabels;
            my_label = this.RankBy_1stValue_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += shared_row_height;

            #region Rank by 2nd value cbButton and cbLabel
            left_referenceBorder = shared_leftBorder_for_rankBy_cbButtons;
            right_referenceBorder = left_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            top_referenceBorder = running_top_referenceBorder + half_extra_height_for_cbLabels;
            bottom_referenceBorder = top_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            my_cbButton = this.RankBy_2ndValue_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.RankBy_2ndValue_cbButton.Location.X + this.RankBy_2ndValue_cbButton.Width;
            right_referenceBorder = (int)Math.Round(0.7F * this.SigSelection_panel.Width);
            top_referenceBorder = this.RankBy_2ndValue_cbButton.Location.Y - half_extra_height_for_cbLabels;
            bottom_referenceBorder = this.RankBy_2ndValue_cbButton.Location.Y + this.RankBy_2ndValue_cbButton.Height + half_extra_height_for_cbLabels;
            my_label = this.RankBy_2ndValue_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += shared_row_height;
            int middle_between_both_rank_cbButtons = RankBy_1stValue_cbButton.Location.Y + RankBy_1stValue_cbButton.Height + (int)Math.Round(0.5F * (this.RankBy_2ndValue_cbButton.Location.Y - this.RankBy_1stValue_cbButton.Location.Y - this.RankBy_1stValue_cbButton.Height));

            #region Rank by first or second value left label
            left_referenceBorder = 0;
            right_referenceBorder = RankBy_1stValue_cbButton.Location.X;
            top_referenceBorder = middle_between_both_rank_cbButtons - (int)Math.Round(0.5F*shared_row_height);
            bottom_referenceBorder = top_referenceBorder + shared_row_height;
            my_label = this.RankBy_left_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Rank by first or second value tie breaker label
            Set_tie_breaker_explanation_label_fontSize_and_size();
            #endregion

            #region Rank with or without merging upDown datasets check boxes
            top_referenceBorder = running_top_referenceBorder + (int)Math.Round(0.5F*shared_row_height) + half_extra_height_for_cbLabels;

            bottom_referenceBorder = top_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            left_referenceBorder = 0;
            right_referenceBorder = left_referenceBorder + shared_heightWidth_of_checkBoxButtons; 
            my_cbButton = this.Keep_mergeUpDown_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            top_referenceBorder = Keep_mergeUpDown_cbButton.Location.Y - half_extra_height_for_cbLabels;
            bottom_referenceBorder = Keep_mergeUpDown_cbButton.Location.Y + Keep_mergeUpDown_cbButton.Height + half_extra_height_for_cbLabels;
            left_referenceBorder = Keep_mergeUpDown_cbButton.Location.X + Keep_mergeUpDown_cbButton.Width;
            right_referenceBorder = (int)Math.Round(0.35F * SigSelection_panel.Width);
            my_label = this.Keep_mergeUpDown_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            top_referenceBorder = running_top_referenceBorder + (int)Math.Round(0.5F * shared_row_height) + half_extra_height_for_cbLabels;
            bottom_referenceBorder = top_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            left_referenceBorder = Keep_mergeUpDown_cbLabel.Location.X + Keep_mergeUpDown_cbLabel.Width;
            right_referenceBorder = left_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            my_cbButton = this.Keep_eachDataset_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            top_referenceBorder = Keep_eachDataset_cbButton.Location.Y - (int)Math.Round(0.4F * shared_row_height);
            bottom_referenceBorder = Keep_eachDataset_cbButton.Location.Y + Keep_eachDataset_cbButton.Height + (int)Math.Round(0.4F * shared_row_height);
            left_referenceBorder = Keep_eachDataset_cbButton.Location.X + Keep_eachDataset_cbButton.Width;
            right_referenceBorder = this.SigSelection_panel.Width - shared_distance_from_leftRightSide;
            my_label = this.Keep_eachDataset_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += 2*shared_row_height;

            #region Rank keep top ranks text box, left and right labels
            top_referenceBorder = running_top_referenceBorder;
            bottom_referenceBorder = top_referenceBorder + shared_height_of_textBoxes;
            left_referenceBorder = (int)Math.Round(0.23F * Overall_panel.Width);
            right_referenceBorder = (int)Math.Round(0.38F * Overall_panel.Width);
            my_textBox = this.KeepTopRanks_ownTextBox;
            Form_default_settings.MyTextBoxSingleLine_adjustCoordinatesToBorders_add_default_parameter(my_textBox, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = 0;
            right_referenceBorder = this.KeepTopRanks_ownTextBox.Location.X;
            top_referenceBorder = this.KeepTopRanks_ownTextBox.Location.Y;
            bottom_referenceBorder = this.KeepTopRanks_ownTextBox.Location.Y + this.KeepTopRanks_ownTextBox.Height;
            my_label = this.KeepTopRanks_left_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_rightXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = this.KeepTopRanks_ownTextBox.Location.X + this.KeepTopRanks_ownTextBox.Width;
            right_referenceBorder = this.SigSelection_panel.Width;
            top_referenceBorder = this.KeepTopRanks_ownTextBox.Location.Y;
            bottom_referenceBorder = this.KeepTopRanks_ownTextBox.Location.Y + this.KeepTopRanks_ownTextBox.Height;
            my_label = this.KeepTopRanks_right_label;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            running_top_referenceBorder += shared_row_height;

            #region Delete not significant check box
            left_referenceBorder = 0;
            right_referenceBorder = left_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            top_referenceBorder = running_top_referenceBorder + half_extra_height_for_cbLabels;
            bottom_referenceBorder = top_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            my_cbButton = this.DeleteNotSignGenes_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = DeleteNotSignGenes_cbButton.Location.X + DeleteNotSignGenes_cbButton.Width;
            right_referenceBorder = SigSelection_panel.Width;
            top_referenceBorder = DeleteNotSignGenes_cbButton.Location.Y - half_extra_height_for_cbLabels;
            bottom_referenceBorder = DeleteNotSignGenes_cbButton.Location.Y + DeleteNotSignGenes_cbButton.Height + half_extra_height_for_cbLabels;
            my_label = this.DeleteNotSignGenes_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region All genes are significant healine label
            left_referenceBorder = this.SigSelection_panel.Location.X;
            right_referenceBorder = this.SigSelection_panel.Width;
            top_referenceBorder = this.SigSelection_panel.Location.Y + this.SigSelection_panel.Height;
            bottom_referenceBorder = top_referenceBorder + space_for_headline;
            my_label = this.AllGenesSignificant_headline_label;
            Form_default_settings.LabelUnderlinedHeadline_adjust_to_given_positions_attach_to_leftPosition_and_center_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region All Genes are significant checkbox
            left_referenceBorder = this.SigSelection_panel.Location.X;
            right_referenceBorder = left_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            top_referenceBorder = (int)SigSelection_panel.Location.Y + SigSelection_panel.Height + space_for_headline + half_extra_height_for_cbLabels;
            bottom_referenceBorder = top_referenceBorder + shared_heightWidth_of_checkBoxButtons;
            my_cbButton= this.AllGenesSignificant_cbButton;
            Form_default_settings.MyCheckBoxButton_without_text_add_default_and_adjust_to_referenceBorders(my_cbButton, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = AllGenesSignificant_cbButton.Location.X + AllGenesSignificant_cbButton.Width;
            right_referenceBorder = SigSelection_panel.Width;
            top_referenceBorder = AllGenesSignificant_cbButton.Location.Y - half_extra_height_for_cbLabels;
            bottom_referenceBorder = AllGenesSignificant_cbButton.Location.Y + AllGenesSignificant_cbButton.Height + half_extra_height_for_cbLabels;
            my_label = this.AllGenesSignificant_cbLabel;
            Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Apply and reset significance criteria buttons
            top_referenceBorder = AllGenesSignificant_cbButton.Location.Y + AllGenesSignificant_cbButton.Height;
            bottom_referenceBorder = (int)Math.Round(0.94 * this.Overall_panel.Height);

            left_referenceBorder = shared_distance_from_leftRightSide;
            right_referenceBorder = (int)Math.Round(0.5 * this.Overall_panel.Width - 0.5F * shared_distance_from_leftRightSide);
            my_button = this.ResetSig_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);

            left_referenceBorder = (int)Math.Round(0.5 * this.Overall_panel.Width + 0.5F * shared_distance_from_leftRightSide);
            right_referenceBorder = this.Overall_panel.Width - shared_distance_from_leftRightSide;
            my_button = this.ResetParameter_button;
            Form_default_settings.Button_standard_add_default_values_and_adjust_to_referenceBorders(my_button, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

            #region Subject only significant genes to enrichment explanation label
            left_referenceBorder = shared_distance_from_leftRightSide;
            right_referenceBorder = Overall_panel.Width - shared_distance_from_leftRightSide;
            top_referenceBorder = this.ResetParameter_button.Location.Y + this.ResetParameter_button.Height;
            bottom_referenceBorder = this.Overall_panel.Height;
            my_label = this.SigSubject_explanation_label;
            Form_default_settings.LabelExplanation_adjust_to_given_referenceBorders_and_center_x_and_y(my_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            #endregion

           Set_visualized_options_to_custom_data_options_and_update_boxes(custom_data_options);
        }

        public void Set_visualized_options_to_custom_data_options_and_update_boxes(User_data_options_class custom_data_options)
        {
            this.Visualized_options = custom_data_options.Deep_copy();
            Update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
        }

        private void Set_tie_breaker_explanation_label_fontSize_and_size()
        {
            int left_referenceBorder = RankBy_1stValue_cbLabel.Location.X + RankBy_1stValue_cbLabel.Width;
            int right_referenceBorder = this.SigSelection_panel.Width;
            int top_referenceBorder = RankBy_1stValue_cbLabel.Location.Y;
            int bottom_referenceBorder = RankBy_2ndValue_cbLabel.Location.Y + RankBy_2ndValue_cbLabel.Height;
            Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(TieBreaker_explanation_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
        }


        private void Set_value1st_and_2nd_label_and_expl_label_fontSize_and_size()
        {
            int left_referenceBorder;
            int right_referenceBorder;
            int top_referenceBorder;
            int bottom_referenceBorder;

            Dictionary<OwnTextBox, Label> textBox_defaultLabel_dict = new Dictionary<OwnTextBox, Label>();
            textBox_defaultLabel_dict.Add(this.Value1st_cutoff_ownTextBox, this.Value1st_cutoff_label);
            textBox_defaultLabel_dict.Add(this.Value2nd_cutoff_ownTextBox, this.Value2nd_cutoff_label);

            Dictionary<OwnTextBox, Label> textBox_explLabel_dict = new Dictionary<OwnTextBox, Label>();
            textBox_explLabel_dict.Add(this.Value1st_cutoff_ownTextBox, this.Value1st_cutoff_expl_label);
            textBox_explLabel_dict.Add(this.Value2nd_cutoff_ownTextBox, this.Value2nd_cutoff_expl_label);

            OwnTextBox[] textBoxes = textBox_defaultLabel_dict.Keys.ToArray();
            Label current_label;
            foreach (OwnTextBox textBox in textBoxes)
            {
                current_label = textBox_defaultLabel_dict[textBox];
                left_referenceBorder = 0;
                right_referenceBorder = textBox.Location.X;
                top_referenceBorder = textBox.Location.Y;
                bottom_referenceBorder = textBox.Location.Y + textBox.Height;
                Form_default_settings.LabelDefaultBold_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(current_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            }

            foreach (OwnTextBox textBox in textBoxes)
            {
                current_label = textBox_explLabel_dict[textBox];
                left_referenceBorder = textBox.Location.X + textBox.Width;
                right_referenceBorder = this.SigSelection_panel.Width;
                top_referenceBorder = textBox.Location.Y;
                bottom_referenceBorder = textBox.Location.Y + textBox.Height;
                Form_default_settings.LabelExplanation_adjust_to_given_positions_and_attach_to_leftXPosition_and_centerYPosition(current_label, left_referenceBorder, right_referenceBorder, top_referenceBorder, bottom_referenceBorder);
            }
        }

        public void Set_to_visible(User_data_options_class custom_data_options)
        {
            Update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
        }

        private void Update_boxes_based_on_custom_data_and_visualized_options(User_data_options_class custom_data_options)
        {
            switch (Visualized_options.Significance_definition_value_1st)
            {
                case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                    this.Value1st_higherSig_cbButton.SilentChecked = true;
                    this.Value1st_lowerSig_cbButton.SilentChecked = false;
                    this.Value1st_cutoff_label.Text = "Min absolute 1st value:";
                    this.Value1st_cutoff_expl_label.Text = SigData_text_class.Eg_log2foldchange_text;
                    break;
                case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                    this.Value1st_higherSig_cbButton.SilentChecked = false;
                    this.Value1st_lowerSig_cbButton.SilentChecked = true;
                    this.Value1st_cutoff_label.Text = "Max absolute 1st value:";
                    this.Value1st_cutoff_expl_label.Text = SigData_text_class.Eg_pvalue_text;
                    break;
                default:
                    throw new Exception();
            }
            switch (Visualized_options.Significance_definition_value_2nd)
            {
                case Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant:
                    this.Value2nd_higherSig_cbButton.SilentChecked = true;
                    this.Value2nd_lowerSig_cbButton.SilentChecked = false;
                    this.Value2nd_cutoff_label.Text = "Min absolute 2nd value:";
                    this.Value2nd_cutoff_expl_label.Text = SigData_text_class.Eg_log2foldchange_text;
                    break;
                case Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant:
                    this.Value2nd_higherSig_cbButton.SilentChecked = false;
                    this.Value2nd_lowerSig_cbButton.SilentChecked = true;
                    this.Value2nd_cutoff_label.Text = "Max absolute 2nd value:";
                    this.Value2nd_cutoff_expl_label.Text = SigData_text_class.Eg_pvalue_text; 
                    break;
                default:
                    throw new Exception();
            }
            Set_value1st_and_2nd_label_and_expl_label_fontSize_and_size();
            this.Value1st_cutoff_ownTextBox.SilentText = Visualized_options.Value_1st_cutoff.ToString();
            this.Value1st_cutoff_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            this.Value2nd_cutoff_ownTextBox.SilentText = Visualized_options.Value_2nd_cutoff.ToString();
            this.Value2nd_cutoff_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            switch (Visualized_options.Value_importance_order)
            {
                case Value_importance_order_enum.Value_1st_2nd:
                    this.RankBy_1stValue_cbButton.SilentChecked = true;
                    this.RankBy_2ndValue_cbButton.SilentChecked = false;
                    this.TieBreaker_explanation_label.Text = "2nd value\nwill be tie breaker";
                    this.TieBreaker_explanation_label.Refresh();
                    break;
                case Value_importance_order_enum.Value_2nd_1st:
                    this.RankBy_1stValue_cbButton.SilentChecked = false;
                    this.RankBy_2ndValue_cbButton.SilentChecked = true;
                    this.TieBreaker_explanation_label.Text = "1st value\nwill be tie breaker";
                    this.TieBreaker_explanation_label.Refresh();
                    break;
                default:
                    throw new Exception();
            }

            this.KeepTopRanks_ownTextBox.SilentText = Visualized_options.Keep_top_ranks.ToString();
            this.KeepTopRanks_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            this.Keep_mergeUpDown_cbButton.SilentChecked = Visualized_options.Merge_upDown_before_ranking;
            this.Keep_eachDataset_cbButton.SilentChecked = !Visualized_options.Merge_upDown_before_ranking;
            AllGenesSignificant_cbButton.SilentChecked = Visualized_options.All_genes_significant;
            SigSelection_panel.Visible = !Visualized_options.All_genes_significant;
            if (!Visualized_options.Equals_other(custom_data_options))
            {
                if (Visualized_options.All_genes_significant)
                {
                    ResetSig_button.Text = (string)SigData_text_class.Set_all_genes_to_significant_text.Clone();
                }
                else
                {
                    ResetSig_button.Text = (string)SigData_text_class.Changes_significance_using_both_cutoffs_text.Clone();
                }
                ResetSig_button.Visible = true;
                ResetParameter_button.Visible = true;

            }
            else
            {
                ResetSig_button.Visible = false;
                ResetParameter_button.Visible = false;
            }
            DeleteNotSignGenes_cbButton.SilentChecked = Visualized_options.Delete_all_not_significant_genes;
            if (Visualized_options.Delete_all_not_significant_genes)
            {
                DeleteNotSignGenes_cbLabel.BackColor = Form_default_settings.Warnings_back_color;
                DeleteNotSignGenes_cbLabel.ForeColor = Form_default_settings.Warnings_fore_color;
            }
            else
            {
                DeleteNotSignGenes_cbLabel.BackColor = Form_default_settings.Color_checkBox_backColor;
                DeleteNotSignGenes_cbLabel.ForeColor = Form_default_settings.Color_checkBox_foreColor;
            }
            DeleteNotSignGenes_cbLabel.Refresh();
        }

        public void Value1st_higherSig_ownCheckBox_CheckedChanged(User_data_options_class custom_data_options)
        {
            if (Value1st_higherSig_cbButton.Checked)
            {
                Visualized_options.Significance_definition_value_1st = Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant;
            }
            else
            {
                Visualized_options.Significance_definition_value_1st = Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant;
            }
            Update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
        }
        public void Value1st_lowerSig_ownCheckBox_CheckedChanged(User_data_options_class custom_data_options)
        {
            if (Value1st_lowerSig_cbButton.Checked)
            {
                Visualized_options.Significance_definition_value_1st = Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant;
            }
            else
            {
                Visualized_options.Significance_definition_value_1st = Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant; ;
            }
            Update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
        }
        public void Value2nd_higherSig_ownCheckBox_CheckedChanged(User_data_options_class custom_data_options)
        {
            if (Value2nd_higherSig_cbButton.Checked)
            {
                Visualized_options.Significance_definition_value_2nd = Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant;
            }
            else
            {
                Visualized_options.Significance_definition_value_2nd = Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant;
            }
            Update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
        }
        public void Value2nd_lowerSig_ownCheckBox_CheckedChanged(User_data_options_class custom_data_options)
        {
            if (Value2nd_lowerSig_cbButton.Checked)
            {
                Visualized_options.Significance_definition_value_2nd = Order_of_values_for_signficance_enum.Lower_abs_values_are_more_significant;
            }
            else
            {
                Visualized_options.Significance_definition_value_2nd = Order_of_values_for_signficance_enum.Higher_abs_values_are_more_significant;
            }
            Update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
        }
        public void Value1st_cutoff_textBox_TextChanged(User_data_options_class custom_data_options)
        {
            float cutoff;
            if  (  (Value1st_cutoff_ownTextBox.Text.Length>0)
                 &&(!Value1st_cutoff_ownTextBox.Text[Value1st_cutoff_ownTextBox.Text.Length-1].Equals('.'))
                 &&(float.TryParse(Value1st_cutoff_ownTextBox.Text, out cutoff)) && ((cutoff >= 0)))
            {
                Visualized_options.Value_1st_cutoff = cutoff;
                Update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
                Value1st_cutoff_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            }
            else
            {
                Value1st_cutoff_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value;
            }
        }
        public void Value2nd_cutoff_textBox_TextChanged(User_data_options_class custom_data_options)
        {
            float cutoff;
            if (  (Value2nd_cutoff_ownTextBox.Text.Length>0)
                &&(!Value2nd_cutoff_ownTextBox.Text[Value2nd_cutoff_ownTextBox.Text.Length - 1].Equals('.'))
                &&(float.TryParse(Value2nd_cutoff_ownTextBox.Text, out cutoff)) && ((cutoff >= 0)))
            {
                Visualized_options.Value_2nd_cutoff = cutoff;
                Update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
                Value2nd_cutoff_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            }
            else
            {
                Value2nd_cutoff_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value;
            }
        }
        public void RankBy_1stValue_ownCheckBox_CheckedChanged(User_data_options_class custom_data_options)
        {
            if (RankBy_1stValue_cbButton.Checked)
            {
                Visualized_options.Value_importance_order = Value_importance_order_enum.Value_1st_2nd;
            }
            else
            {
                Visualized_options.Value_importance_order = Value_importance_order_enum.Value_2nd_1st;
            }
            Update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
        }
        public void RankBy_2ndValue_ownCheckBox_CheckedChanged(User_data_options_class custom_data_options)
        {
            if (RankBy_2ndValue_cbButton.Checked)
            {
                Visualized_options.Value_importance_order = Value_importance_order_enum.Value_2nd_1st;
            }
            else
            {
                Visualized_options.Value_importance_order = Value_importance_order_enum.Value_1st_2nd;
            }
            Update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
        }
        public void Keep_eachDataset_ownCheckBox_CheckedChanged(User_data_options_class custom_data_options)
        {
            if (Keep_eachDataset_cbButton.Checked)
            {
                Visualized_options.Merge_upDown_before_ranking = false;
            }
            else
            {
                Visualized_options.Merge_upDown_before_ranking = true;
            }
            Update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
        }
        public void Keep_mergeUpDown_ownCheckBox_CheckedChanged(User_data_options_class custom_data_options)
        {
            if (Keep_mergeUpDown_cbButton.Checked)
            {
                Visualized_options.Merge_upDown_before_ranking = true;
            }
            else
            {
                Visualized_options.Merge_upDown_before_ranking = false;
            }
            Update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
        }
        public void KeepTopRanks_ownTextBox_TextChanged(User_data_options_class custom_data_options)
        {
            int top_ranks;
            if (int.TryParse(KeepTopRanks_ownTextBox.Text, out top_ranks) && (top_ranks > 0))
            {
                Visualized_options.Keep_top_ranks = top_ranks;
                Update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
                KeepTopRanks_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor;
            }
            else
            {
                KeepTopRanks_ownTextBox.BackColor = Form_default_settings.Color_textBox_backColor_invalid_value;
            }
        }
        public void DeleteNotSignGenes_ownCheckBox_CheckedChanged(User_data_options_class custom_data_options)
        {
            Visualized_options.Delete_all_not_significant_genes = DeleteNotSignGenes_cbButton.Checked;
            Update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
        }

        public void AllGeneSignificant_ownCheckBox_CheckedChanged(User_data_options_class custom_data_options)
        {
            Visualized_options.All_genes_significant = AllGenesSignificant_cbButton.Checked;
            Update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
        }
        public Custom_data_class ResetSig_button_Click(Custom_data_class custom_data)
        {
            custom_data.Options = Visualized_options.Deep_copy();
            custom_data.Update_significance_after_calculation_of_fractional_ranks_based_on_options();
            Update_boxes_based_on_custom_data_and_visualized_options(custom_data.Options);
            return custom_data;
        }
        public void ResetParameter_button_Click(User_data_options_class custom_data_options)
        {
            this.Visualized_options = custom_data_options.Deep_copy();
            Update_boxes_based_on_custom_data_and_visualized_options(custom_data_options);
        }

    }
}
