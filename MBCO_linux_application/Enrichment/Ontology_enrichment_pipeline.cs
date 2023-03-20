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
using Data;
using MBCO;
using Network;
using Leave_out;
using Common_functions.Array_own;
using Common_functions.Global_definitions;
using Common_functions.ReadWrite;
using System.Drawing;
using Common_functions.Statistic;
using yed_network;
using System.Windows.Forms;
using Common_functions.Options_base;
using Common_functions.Form_tools;

namespace Enrichment
{
    enum Data_value_signs_of_interest_enum { E_m_p_t_y, Upregulated, Downregulated, Combined }
    enum Integration_entity_for_definition_of_sample_sets_enum { E_m_p_t_y, No_integration, EntryType_and_timepoint, EntryType_and_sampleName, EntryType_only, Integration_group }

    class Mbc_add_inferred_parentSCP_line_class
    {
        public string Inferred_parentSCP { get; set; }
        public string[] Children_scps { get; set; }
        public int ParentScp_processLevel { get; set; }

        public Mbc_add_inferred_parentSCP_line_class Deep_copy()
        {
            Mbc_add_inferred_parentSCP_line_class copy = (Mbc_add_inferred_parentSCP_line_class)this.MemberwiseClone();
            copy.Inferred_parentSCP = (string)this.Inferred_parentSCP.Clone();
            copy.Children_scps = Array_class.Deep_copy_string_array(this.Children_scps);
            return copy;
        }
    }

    class MBCO_enrichment_pipeline_options_class : Options_readWrite_base_class
    {
        #region Min max values
        private const float Const_min_maxPvalue = 0;
        private const float Const_max_maxPvalue = 1;
        public const float Const_min_top_quantile_of_scp_interactions = 0.05F;
        public const float Const_max_top_quantile_of_scp_interactions = 1;
        private const int Const_min_keep_top_predictions = 0;
        #endregion


        #region Fields for general Options
        public Ontology_type_enum Ontology { get; private set; }
        public Ontology_type_enum Next_ontology { get; set; }
        public int Max_columns_per_analysis { get; set; }
        public bool Write_results { get; set; }
        #endregion

        #region Fields for standard enrichment analysis
        private Dictionary<Ontology_type_enum, int[]> Ontology_keep_top_predictions_standardEnrichment_per_level { get; set; }
        private Dictionary<Ontology_type_enum, float> Ontology_max_pvalue_for_standardEnrichment { get; set; }
        public int[] Keep_top_predictions_standardEnrichment_per_level
        {
            get
            {
                Ontology_type_enum next_simplified_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_keep_top_predictions_standardEnrichment_per_level[next_simplified_ontology];
            }
            set
            {
                Ontology_type_enum next_simplified_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                int length_value = value.Length;
                for (int indexValue = 0; indexValue < length_value; indexValue++)
                {
                    Ontology_keep_top_predictions_standardEnrichment_per_level[next_simplified_ontology][indexValue] = Math.Max(Const_min_keep_top_predictions, value[indexValue]);
                }
            }
        }
        public float Max_pvalue_for_standardEnrichment
        {
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_max_pvalue_for_standardEnrichment[human_ontology];
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                Ontology_max_pvalue_for_standardEnrichment[human_ontology] = Math.Min(Const_max_maxPvalue,Math.Max(Const_min_maxPvalue, value));
            }
        }
        #endregion

        #region Fields for dynamic enrichment analysis
        private Dictionary<Ontology_type_enum, float[]> Ontology_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level { get; set; }
        private Dictionary<Ontology_type_enum, float[]> Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level { get; set; }
        private Dictionary<Ontology_type_enum, int[]> Ontology_keep_top_predictions_dynamicEnrichment_per_level { get; set; }
        private Dictionary<Ontology_type_enum, float> Ontology_max_pvalue_for_dynamicEnrichment { get; set; }
        private Dictionary<Ontology_type_enum, int[][]> Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level { get; set; }

        public float[] Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level
        {
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[human_ontology];
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                int length_value = value.Length;
                for (int indexValue = 0; indexValue < length_value; indexValue++)
                {
                    switch (indexValue)
                    {
                        case 0:
                        case 1:
                        case 4:
                            Ontology_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[human_ontology][indexValue] = -1;
                            break;
                        case 2:
                        case 3:
                            Ontology_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[human_ontology][indexValue] = Math.Max(Const_min_top_quantile_of_scp_interactions, Math.Min(Const_max_top_quantile_of_scp_interactions, value[indexValue]));
                            break;
                        default:
                            throw new Exception();
                    }
                }

            }
        }
        public float[] Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level
        {
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[human_ontology];
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                int length_value = value.Length;
                for (int indexValue = 0; indexValue < length_value; indexValue++)
                {
                    switch (indexValue)
                    {
                        case 0:
                        case 1:
                        case 4:
                            Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[human_ontology][indexValue] = -1;
                            break;
                        case 2:
                        case 3:
                            Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[human_ontology][indexValue] = Math.Max(Const_min_top_quantile_of_scp_interactions, Math.Min(Const_max_top_quantile_of_scp_interactions, value[indexValue]));
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
        }

        public int[][] Numbers_of_merged_scps_for_dynamicEnrichment_per_level 
        { 
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level[human_ontology];
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                int values_length = value.Length;
                for (int indexV=0; indexV< values_length;indexV++)
                {
                    switch (indexV)
                    {
                        case 0:
                        case 1:
                        case 4:
                            Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level[human_ontology][indexV] = new int[0];
                            break;
                        case 2:
                        case 3:
                            Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level[human_ontology][indexV] = Array_class.Deep_copy_array(value[indexV]);
                            break;
                        default:
                            throw new Exception();

                    }
                }
            }
        }
        public int[] Keep_top_predictions_dynamicEnrichment_per_level
        {
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_keep_top_predictions_dynamicEnrichment_per_level[human_ontology];
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                int length_value = value.Length;
                for (int indexValue = 0; indexValue < length_value; indexValue++)
                {
                    switch (indexValue)
                    {
                        case 0:
                        case 1:
                        case 4:
                            Ontology_keep_top_predictions_dynamicEnrichment_per_level[human_ontology][indexValue] = -1;
                            break;
                        case 2:
                        case 3:
                            Ontology_keep_top_predictions_dynamicEnrichment_per_level[human_ontology][indexValue] = Math.Max(Const_min_keep_top_predictions, value[indexValue]);
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
        }

        public float Max_pvalue_for_dynamicEnrichment
        {
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_max_pvalue_for_dynamicEnrichment[human_ontology];
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                Ontology_max_pvalue_for_dynamicEnrichment[human_ontology] = Math.Max(Const_min_maxPvalue, Math.Min(Const_max_maxPvalue, value));
            }
        }
        #endregion

        #region Fields for timeline
        private Dictionary<Ontology_type_enum,float> Ontology_pvalue_cutoff_for_timeline { get; set; }
        public float Timeline_pvalue_cutoff
        {
            get 
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_pvalue_cutoff_for_timeline[human_ontology]; 
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                Ontology_pvalue_cutoff_for_timeline[human_ontology] = Math.Max(Const_min_maxPvalue, Math.Min(Const_max_maxPvalue, value));
            }
        }
        #endregion

        #region Fields for selected SCPs
        private Dictionary<Ontology_type_enum, Dictionary<string, string[]>> Ontology_group_selectedScps_dict { get; set; }
        public Dictionary<string, string[]> Group_selectedScps_dict
        {
            get
            {
                Ontology_type_enum next_simplified_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                Dictionary<string, string[]> group_selected_Scps_dict = new Dictionary<string, string[]>();
                if (Ontology_group_selectedScps_dict.ContainsKey(next_simplified_ontology))
                {
                    group_selected_Scps_dict = Ontology_group_selectedScps_dict[next_simplified_ontology];
                }
                return group_selected_Scps_dict;
            }
            set
            {
                Ontology_type_enum next_simplified_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                if (!Ontology_group_selectedScps_dict.ContainsKey(next_simplified_ontology))
                {
                    Ontology_group_selectedScps_dict.Add(next_simplified_ontology, new Dictionary<string, string[]>());
                }
                Ontology_group_selectedScps_dict[next_simplified_ontology] = value;
            }
        }
        public void Group_selectedScps_dict_add(string dict_key, string[] dict_values)
        {
            Ontology_type_enum next_simplified_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
            if (!Ontology_group_selectedScps_dict.ContainsKey(next_simplified_ontology))
            {
                Ontology_group_selectedScps_dict.Add(next_simplified_ontology, new Dictionary<string, string[]>());
            }
            Ontology_group_selectedScps_dict[next_simplified_ontology].Add(dict_key, dict_values);
        }
        public bool Show_all_and_only_selected_scps { get; set; }
        #endregion

        #region Fields for add own SCPs
        public Dictionary<Ontology_type_enum, Dictionary<string, string[]>> Ontology_ownScp_mbcoSubScps_dict { get; set; }
        public Dictionary<Ontology_type_enum, Dictionary<string, int>> Ontology_ownScp_level_dict { get; set; }
        public Dictionary<string, string[]> OwnScp_mbcoSubScps_dict
        {
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                Dictionary<string, string[]> ownScp_mbcoSubScps_dict = new Dictionary<string, string[]>();
                if (Ontology_ownScp_mbcoSubScps_dict.ContainsKey(human_ontology))
                {
                    ownScp_mbcoSubScps_dict = Ontology_ownScp_mbcoSubScps_dict[human_ontology];
                }
                return ownScp_mbcoSubScps_dict;
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                if (!Ontology_ownScp_mbcoSubScps_dict.ContainsKey(human_ontology))
                {
                    Ontology_ownScp_mbcoSubScps_dict.Add(human_ontology, new Dictionary<string, string[]>());
                }
                Ontology_ownScp_mbcoSubScps_dict[human_ontology] = value;
            }
        }
        public void OwnScp_mbcoSubScps_dict_add(string dict_key, string[] dict_values)
        {
            Ontology_type_enum next_simplified_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
            if (!Ontology_ownScp_mbcoSubScps_dict.ContainsKey(next_simplified_ontology))
            {
                Ontology_ownScp_mbcoSubScps_dict.Add(next_simplified_ontology, new Dictionary<string, string[]>());
            }
            Ontology_ownScp_mbcoSubScps_dict[next_simplified_ontology].Add(dict_key, dict_values);
        }
        public Dictionary<string, int> OwnScp_level_dict
        {
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                Dictionary<string, int> ownScp_level_dict = new Dictionary<string, int>();
                if (Ontology_ownScp_level_dict.ContainsKey(human_ontology))
                {
                    ownScp_level_dict = Ontology_ownScp_level_dict[human_ontology];
                }
                return ownScp_level_dict;
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                if (!Ontology_ownScp_level_dict.ContainsKey(human_ontology))
                {
                    Ontology_ownScp_level_dict.Add(human_ontology, new Dictionary<string, int>());
                }
                Ontology_ownScp_level_dict[human_ontology] = value;
            }
        }
        public void OwnScp_level_dict_add(string dict_key, int dict_value)
        {
            Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
            if (!Ontology_ownScp_level_dict.ContainsKey(human_ontology))
            {
                Ontology_ownScp_level_dict.Add(human_ontology, new Dictionary<string, int>());
            }
            Ontology_ownScp_level_dict[human_ontology].Add(dict_key,dict_value);
        }
        public bool Update_scps_in_select_SCPs_interface { get; set; }
        #endregion

        public void Set_ontology_to_next_ontology_to_indicate_that_datasets_were_updated()
        {
            this.Ontology = this.Next_ontology;
        }

        public bool Is_at_least_one_scp_selected_as_part_of_any_group()
        {
            string[] groups = Group_selectedScps_dict.Keys.ToArray();
            bool atLeast_one_scp_selected = false;
            foreach (string group in groups)
            {
                if (Group_selectedScps_dict[group].Length > 0)
                {
                    atLeast_one_scp_selected = true;
                }
            }
            return atLeast_one_scp_selected;
        }

        public void Remove_scps_from_group_selectedScps_dictionary(params string[] remove_scps)
        {
            string[] scpGroups = Group_selectedScps_dict.Keys.ToArray();
            foreach (string group in scpGroups)
            {
                Group_selectedScps_dict[group] = Overlap_class.Get_part_of_list1_but_not_of_list2(Group_selectedScps_dict[group], remove_scps);
            }
        }

        public bool Is_necessary_to_update_pipeline_for_all_runs()
        {
            bool update_pipeline = false;
            int selected_top_quantiles_length = this.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level.Length;
            int real_top_quantiles_length = this.Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level.Length;
            if (!Next_ontology.Equals(Ontology)) { update_pipeline = true; }
            else if (selected_top_quantiles_length != real_top_quantiles_length) { update_pipeline = true; }
            else
            {
                for (int indexQ = 0; indexQ < selected_top_quantiles_length; indexQ++)
                {
                    if (Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[indexQ] != Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[indexQ]) { update_pipeline = true; }
                }
            }
            return update_pipeline;
        }

        public void Set_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level_to_selected_to_indicate_leaveOutNetworkUpdate()
        {
            this.Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level = Array_class.Deep_copy_array(this.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level);
        }

        public MBCO_enrichment_pipeline_options_class(Ontology_type_enum ontology)
        {
            #region Parameter for general options
            this.Ontology = Ontology_type_enum.E_m_p_t_y;
            this.Next_ontology = ontology;
            Max_columns_per_analysis = 100;
            Write_results = true;
            #endregion

            #region Initialize dictionaries for standard, dynamic enrichment and timelines
            Ontology_keep_top_predictions_standardEnrichment_per_level = new Dictionary<Ontology_type_enum, int[]>();
            Ontology_max_pvalue_for_standardEnrichment = new Dictionary<Ontology_type_enum, float>();
            Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level = new Dictionary<Ontology_type_enum, float[]>();
            Ontology_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level = new Dictionary<Ontology_type_enum, float[]>();
            Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level = new Dictionary<Ontology_type_enum, int[][]>();
            Ontology_keep_top_predictions_dynamicEnrichment_per_level = new Dictionary<Ontology_type_enum, int[]>();
            Ontology_max_pvalue_for_dynamicEnrichment = new Dictionary<Ontology_type_enum, float>();
            Ontology_pvalue_cutoff_for_timeline = new Dictionary<Ontology_type_enum, float>();
            #endregion

            Set_default_parameters_for_mbco();
            Set_default_parameters_other_ontologies();

            #region Fields for selected SCPs
            Ontology_group_selectedScps_dict = new Dictionary<Ontology_type_enum, Dictionary<string, string[]>>();
            Show_all_and_only_selected_scps = false;
            Update_scps_in_select_SCPs_interface = false;
            #endregion

            #region Fields for add own SCPs
            Ontology_ownScp_mbcoSubScps_dict = new Dictionary<Ontology_type_enum, Dictionary<string, string[]>>();
            Ontology_ownScp_level_dict = new Dictionary<Ontology_type_enum, Dictionary<string, int>>();
            #endregion

        }
        private void Set_default_parameters_for_mbco()
        {
            Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Ontology_type_enum.Mbco_human);

            #region Fields for standard enrichment analysis
            Ontology_keep_top_predictions_standardEnrichment_per_level.Add(human_ontology, new int[5]);
            Ontology_keep_top_predictions_standardEnrichment_per_level[human_ontology] = new int[] { -1, 5, 5, 10, 5 };
            Ontology_max_pvalue_for_standardEnrichment.Add(human_ontology, -1);
            Ontology_max_pvalue_for_standardEnrichment[human_ontology] = 0.05F;
            #endregion

            #region Fields for dynamic enrichment analysis
            Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level.Add(human_ontology, new float[5]);
            Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[human_ontology] = new float[] { -1, -1, 0.2F, 0.25F, -1 };
            Ontology_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level.Add(human_ontology, new float[5]);
            Ontology_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[human_ontology] = new float[] { -1, -1, 0.2F, 0.25F, -1 };
            Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level.Add(human_ontology, new int[5][]);
            Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level[human_ontology][2] = new int[] { 2, 3 };
            Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level[human_ontology][3] = new int[] { 2, 3 };
            Ontology_keep_top_predictions_dynamicEnrichment_per_level.Add(human_ontology, new int[0]);
            Ontology_keep_top_predictions_dynamicEnrichment_per_level[human_ontology] = new int[] { -1, -1, 3, 5, -1 };
            Ontology_max_pvalue_for_dynamicEnrichment.Add(human_ontology, -1);
            Ontology_max_pvalue_for_dynamicEnrichment[human_ontology] = 0.05F;
            #endregion

            #region Fields for timeline
            Ontology_pvalue_cutoff_for_timeline.Add(human_ontology, 0.05F);
            #endregion
        }
        private void Set_default_parameters_other_ontologies()
        {
            Ontology_type_enum[] go_ontoogies = new Ontology_type_enum[] { Ontology_type_enum.Go_bp_human, Ontology_type_enum.Go_mf_human, Ontology_type_enum.Go_cc_human, Ontology_type_enum.Mbco_na_glucose_tm_transport_human };
            foreach (Ontology_type_enum go_ontology in go_ontoogies)
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(go_ontology);

                #region Fields for standard enrichment analysis
                Ontology_keep_top_predictions_standardEnrichment_per_level.Add(human_ontology, new int[5]);
                Ontology_keep_top_predictions_standardEnrichment_per_level[human_ontology] = new int[] { -1, 15, -1, -1, -1 };
                Ontology_max_pvalue_for_standardEnrichment.Add(human_ontology, -1);
                Ontology_max_pvalue_for_standardEnrichment[human_ontology] = 0.05F;
                #endregion

                #region Fields for dynamic enrichment analysis
                Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level.Add(human_ontology, new float[5]);
                Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[human_ontology] = new float[] { -1, -1, Const_min_top_quantile_of_scp_interactions, Const_min_top_quantile_of_scp_interactions, -1 };
                Ontology_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level.Add(human_ontology, new float[5]);
                Ontology_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[human_ontology] = new float[] { -1, -1, Const_min_top_quantile_of_scp_interactions, Const_min_top_quantile_of_scp_interactions, -1 };
                Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level.Add(human_ontology, new int[5][]);
                Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level[human_ontology][2] = new int[] { -1, -1 };
                Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level[human_ontology][3] = new int[] { -1, -1 };
                Ontology_keep_top_predictions_dynamicEnrichment_per_level.Add(human_ontology, new int[0]);
                Ontology_keep_top_predictions_dynamicEnrichment_per_level[human_ontology] = new int[] { -1, -1, -1, -1, -1 };
                Ontology_max_pvalue_for_dynamicEnrichment.Add(human_ontology, -1);
                Ontology_max_pvalue_for_dynamicEnrichment[human_ontology] = -1F;
                #endregion

                #region Fields for timeline
                if (Ontology_classification_class.Is_go_ontology(go_ontology))
                { Ontology_pvalue_cutoff_for_timeline.Add(human_ontology, 0.001F); }
                else if (go_ontology.Equals(Ontology_type_enum.Mbco_na_glucose_tm_transport_human))
                { Ontology_pvalue_cutoff_for_timeline.Add(human_ontology, 0.05F); }
                #endregion
            }
        }
        public override void Write_option_entries(System.IO.StreamWriter writer)
        {
            //base.Write_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), "Update_scps_in_select_SCPs_interface", "Write_results", "Max_columns_per_analysis");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_keep_top_predictions_standardEnrichment_per_level, "Keep_top_standard_SCPs");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_max_pvalue_for_standardEnrichment, "Max_pvalue_standard_enrichment");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_keep_top_predictions_dynamicEnrichment_per_level, "Keep_top_dynamic_SCPs");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_max_pvalue_for_dynamicEnrichment, "Max_pvalue_dynamic_enrichment");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level, "Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_pvalue_cutoff_for_timeline, "Pvalue_cutoff_for_timeline");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_group_selectedScps_dict, "User_selected_scps");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_ownScp_mbcoSubScps_dict, "User_defined_scps");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_ownScp_level_dict, "User_defined_scpLevels");
        }

        public void Clear_all_deNovo_dictionaries()
        {
            this.Ontology_ownScp_level_dict.Clear();
            this.Ontology_ownScp_mbcoSubScps_dict.Clear();
            this.Ontology_group_selectedScps_dict.Clear();
        }
        public override void Add_read_entry_to_options(string readLine)
        {
            string[] splitStrings = readLine.Split(Global_class.Tab);
            switch (splitStrings[1])
            {
                case "Keep_top_standard_SCPs":
                    Ontology_keep_top_predictions_standardEnrichment_per_level = base.Add_to_dictionary_entries(readLine, Ontology_keep_top_predictions_standardEnrichment_per_level);
                    break;
                case "Max_pvalue_standard_enrichment":
                    Ontology_max_pvalue_for_standardEnrichment = base.Add_to_dictionary_entries(readLine, Ontology_max_pvalue_for_standardEnrichment);
                    break;
                case "Keep_top_dynamic_SCPs":
                    Ontology_keep_top_predictions_dynamicEnrichment_per_level = base.Add_to_dictionary_entries(readLine, Ontology_keep_top_predictions_dynamicEnrichment_per_level);
                    break;
                case "Max_pvalue_dynamic_enrichment":
                    Ontology_max_pvalue_for_dynamicEnrichment = base.Add_to_dictionary_entries(readLine, Ontology_max_pvalue_for_dynamicEnrichment);
                    break;
                case "Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment":
                    Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level = base.Add_to_dictionary_entries(readLine, Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level);
                    break;
                case "Pvalue_cutoff_for_timeline":
                    Ontology_pvalue_cutoff_for_timeline = base.Add_to_dictionary_entries(readLine, Ontology_pvalue_cutoff_for_timeline);
                    break;
                case "User_selected_scps":
                    Ontology_group_selectedScps_dict = base.Add_to_dictionary_entries(readLine, Ontology_group_selectedScps_dict);
                    break;
                case "User_defined_scps":
                    Ontology_ownScp_mbcoSubScps_dict = base.Add_to_dictionary_entries(readLine, Ontology_ownScp_mbcoSubScps_dict);
                    break;
                case "User_defined_scpLevels":
                    Ontology_ownScp_level_dict = base.Add_to_dictionary_entries(readLine, Ontology_ownScp_level_dict);
                    break;
                default:
                    throw new Exception();
                    //base.Add_read_entry(readLine, typeof(MBCO_enrichment_pipeline_options_class));
            }
        }
        public MBCO_enrichment_pipeline_options_class Deep_copy()
        {
            MBCO_enrichment_pipeline_options_class copy = (MBCO_enrichment_pipeline_options_class)this.MemberwiseClone();
            copy.Ontology_keep_top_predictions_standardEnrichment_per_level = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_keep_top_predictions_standardEnrichment_per_level);
            copy.Ontology_keep_top_predictions_dynamicEnrichment_per_level = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_keep_top_predictions_dynamicEnrichment_per_level);
            copy.Ontology_max_pvalue_for_standardEnrichment = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_max_pvalue_for_standardEnrichment);
            copy.Ontology_max_pvalue_for_dynamicEnrichment = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_max_pvalue_for_dynamicEnrichment);
            copy.Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level);
            copy.Ontology_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level);
            copy.Ontology_ownScp_mbcoSubScps_dict = Array_class.Deep_copy_dictionary_with_stringKeysValues(this.Ontology_ownScp_mbcoSubScps_dict);
            copy.Ontology_group_selectedScps_dict = Array_class.Deep_copy_dictionary_with_stringKeysValues(this.Ontology_group_selectedScps_dict);
            copy.Ontology_ownScp_level_dict = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_ownScp_level_dict);
            copy.Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level);
            int numbers_of_merged_scps_length = this.Numbers_of_merged_scps_for_dynamicEnrichment_per_level.Length;
            copy.Numbers_of_merged_scps_for_dynamicEnrichment_per_level = new int[numbers_of_merged_scps_length][];
            for (int indexNumber = 0; indexNumber < numbers_of_merged_scps_length; indexNumber++)
            {
                copy.Numbers_of_merged_scps_for_dynamicEnrichment_per_level[indexNumber] = Array_class.Deep_copy_array(this.Numbers_of_merged_scps_for_dynamicEnrichment_per_level[indexNumber]);
            }
            return copy;
        }
    }

    class Mbc_enrichment_fast_pipeline_class
    {
        #region Fields
        public MBCO_association_class MBCO_association_unmodified { get; set; }
        public MBCO_association_class MBCO_association { get; set; }
        public Leave_out_scp_scp_network_class Leave_out_scp_network_for_dynamicEnrichment_analysis { get; set; }
        public Leave_out_scp_scp_network_class Leave_out_scp_network_all_scps { get; set; }
        public MBCO_enrichment_pipeline_options_class Options { get; set; }
        public MBCO_enrichment_pipeline_options_class Options_default { get; set; }
        public MBCO_obo_network_class Mbco_parentChild_nw { get; set; }
        public MBCO_obo_network_class Mbco_childParent_nw { get; set; }
        public Fisher_exact_test_class Fisher { get; set; }
        public string[] Exp_bg_genes { get; set; }
        public string[] Final_bg_genes { get; set; }
        public string Base_file_name { get; set; }
        #endregion

        public Mbc_enrichment_fast_pipeline_class(Ontology_type_enum ontology)
        {
            Options_default = new MBCO_enrichment_pipeline_options_class(ontology);
            Options = Options_default.Deep_copy();
        }

        #region Generate
        private void Generate_parent_child_and_child_parent_MBCO_networks(System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            Mbco_parentChild_nw = new MBCO_obo_network_class(this.Options.Ontology);
            Mbco_parentChild_nw.Generate_by_reading_safed_obo_file(error_report_label, form_default_settings);
            Mbco_childParent_nw = new MBCO_obo_network_class(this.Options.Ontology);
            Mbco_childParent_nw = Mbco_parentChild_nw.Deep_copy_mbco_obo_nw();
            Mbco_childParent_nw.Reverse_direction();
        }

        private void Generate_mbco_association(System.Windows.Forms.Label errorReport_label, Form1_default_settings_class form_default_settings)
        {
            MBCO_association_unmodified = new MBCO_association_class();
            MBCO_association_unmodified.Generate_after_reading_safed_file(this.Options.Ontology, errorReport_label, form_default_settings);
        }

        private void Generate_leave_out_scp_network(System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            Dictionary<string, int> processName_processLevel_dict = Mbco_parentChild_nw.Get_processName_level_dictionary_without_setting_process_level();

            Leave_out_class leave_out = new Leave_out_class(Options.Ontology);
            leave_out.Generate_by_reading_safed_file(error_report_label, form_default_settings);
            Leave_out_scp_network_for_dynamicEnrichment_analysis = new Leave_out_scp_scp_network_class(Options.Ontology);
            this.Options.Set_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level_to_selected_to_indicate_leaveOutNetworkUpdate();
            Leave_out_scp_network_for_dynamicEnrichment_analysis.Options.Top_quantile_of_considered_SCP_interactions_per_level = this.Options.Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level;
            Leave_out_scp_network_for_dynamicEnrichment_analysis.Generate_scp_scp_network_from_leave_out(leave_out, error_report_label, form_default_settings);
            Leave_out_scp_network_for_dynamicEnrichment_analysis.Scp_nw.Nodes.Set_processLevel_for_all_nodes_based_on_dictionary(processName_processLevel_dict);

            Leave_out_scp_network_all_scps = new Leave_out_scp_scp_network_class(this.Options.Ontology);
            Leave_out_scp_network_all_scps.Options.Top_quantile_of_considered_SCP_interactions_per_level = new float[] { 1F, 1F, 1F, 1F, 1F };
            Leave_out_scp_network_all_scps.Generate_scp_scp_network_from_leave_out(leave_out, error_report_label, form_default_settings);
            Leave_out_scp_network_all_scps.Scp_nw.Transform_into_undirected_single_network_and_set_all_widths_to_one();
            Leave_out_scp_network_all_scps.Scp_nw.Nodes.Set_processLevel_for_all_nodes_based_on_dictionary(processName_processLevel_dict);
        }

        public void Generate_for_all_runs_after_setting_ontology_to_next_ontology(System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            this.Options.Set_ontology_to_next_ontology_to_indicate_that_datasets_were_updated();
            Generate_parent_child_and_child_parent_MBCO_networks(error_report_label, form_default_settings);
            Generate_mbco_association(error_report_label, form_default_settings);
            Generate_leave_out_scp_network(error_report_label, form_default_settings);

        }

        public void Reset_mbco_and_adjust_bg_genes_and_set_to_upperCase(string[] bg_genes)
        {
            if (!Options.Ontology.Equals(Options.Next_ontology)) { throw new Exception(); }
            this.MBCO_association = this.MBCO_association_unmodified.Deep_copy();
            string[] all_mbco_genes = MBCO_association.Get_all_distinct_ordered_symbols();
            if (bg_genes.Length > 0)
            {
                this.Exp_bg_genes = Array_class.Deep_copy_string_array(bg_genes);
                int exp_bg_genes_length = this.Exp_bg_genes.Length;
                for (int indexBg=0; indexBg<exp_bg_genes_length;indexBg++)
                {
                    this.Exp_bg_genes[indexBg] = this.Exp_bg_genes[indexBg].ToUpper();
                }
                this.Final_bg_genes = Overlap_class.Get_intersection(this.Exp_bg_genes, all_mbco_genes);
            }
            else
            {
                this.Exp_bg_genes = new string[0];
                this.Final_bg_genes = Array_class.Deep_copy_string_array(all_mbco_genes);
            }
            this.MBCO_association.Keep_only_bg_symbols(this.Final_bg_genes);
            this.MBCO_association.Remove_background_genes_scp();
            Fisher = new Fisher_exact_test_class(Final_bg_genes.Length, false);
        }
        #endregion

        public bool Is_necessary_to_update_pipeline_for_all_runs()
        {
            return Options.Is_necessary_to_update_pipeline_for_all_runs();
        }

        public void Remove_all_existing_custom_scps_from_mbco_association_unmodified_and_add_new_ones()
        {
            Dictionary<string, string[]> customSCP_mbcoSubSCP_dict = this.Options.OwnScp_mbcoSubScps_dict;
            Dictionary<string, int> customSCP_level_dict = this.Options.OwnScp_level_dict;
            this.MBCO_association_unmodified.Remove_all_custom_SCPs_from_mbco_association_unmodified();
            this.MBCO_association_unmodified.Add_custom_scps_as_combination_of_selected_scps_to_mbco_association_unmodified(customSCP_mbcoSubSCP_dict, customSCP_level_dict);
        }

        #region Check
        private void Check_if_no_duplicates_and_in_bg_genes(string[] input_overlap_genes)
        {
            string[] overlap_genes = input_overlap_genes.Distinct().OrderBy(l => l).ToArray();
            this.Final_bg_genes = this.Final_bg_genes.OrderBy(l => l).ToArray();
            string bg_gene;
            int bg_genes_length = this.Final_bg_genes.Length;
            int indexBG = 0;
            int stringCompare2 = -2;
            if (overlap_genes.Length != input_overlap_genes.Length) { throw new Exception(); }
            foreach (string gene in overlap_genes)
            {
                stringCompare2 = -2;
                while (stringCompare2 < 0)
                {
                    bg_gene = this.Final_bg_genes[indexBG];
                    stringCompare2 = bg_gene.CompareTo(gene);
                    if (stringCompare2 < 0)
                    {
                        indexBG++;
                    }
                }
                if (stringCompare2 != 0)
                {
                    throw new Exception();
                }
            }
        }

        public void Check_for_duplicated_enrichment_lines(Ontology_enrichment_line_class[] onto_enrich_lines)
        {
            int onto_enrich_length = onto_enrich_lines.Length;
            onto_enrich_lines = onto_enrich_lines.OrderBy(l=>l.Integration_group).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.Sample_name).ThenBy(l => l.Scp_name).ToArray();
            Ontology_enrichment_line_class previous_onto_enrich_line;
            Ontology_enrichment_line_class onto_enrich_line;
            for (int indexO = 1; indexO < onto_enrich_length; indexO++)
            {
                previous_onto_enrich_line = onto_enrich_lines[indexO - 1];
                onto_enrich_line = onto_enrich_lines[indexO];
                if ((previous_onto_enrich_line.EntryType.Equals(onto_enrich_line.EntryType))
                    && (previous_onto_enrich_line.Integration_group.Equals(onto_enrich_line.Integration_group))
                    && (previous_onto_enrich_line.Timepoint.Equals(onto_enrich_line.Timepoint))
                    && (previous_onto_enrich_line.Sample_name.Equals(onto_enrich_line.Sample_name))
                    && (previous_onto_enrich_line.Scp_name.Equals(onto_enrich_line.Scp_name)))
                {
                    throw new Exception();
                }
            }
        }
        #endregion

        #region Faster data analysis
        private Dictionary<string, List<string>[]> Generate_processName_colIndex_overlapSymbols_dictionary_and_count_column_entries_in_data(ref Data_class data)
        {
            int data_empty_value = 0;
            Dictionary<string, List<string>[]> processName_overlapSymbols_dict = new Dictionary<string, List<string>[]>();
            int col_count = data.ColChar.Columns.Length;
            Data_line_class[] data_lines = data.Data;
            data_lines = data_lines.OrderBy(l => l.NCBI_official_symbol.Length).ThenBy(l => l.NCBI_official_symbol).ToArray();
            Data_line_class data_line;
            int data_length = data.Data.Length;

            Data_line_class col_entries_count_line = new Data_line_class("Column entries count", col_count);

            int indexMbco = 0;
            MBCO_association_line_class[] mbco_association_lines = MBCO_association.MBCO_associations;
            mbco_association_lines = mbco_association_lines.OrderBy(l => l.Symbol.Length).ThenBy(l => l.Symbol).ToArray();
            MBCO_association_line_class mbco_association_line;
            int mbco_associations_length = mbco_association_lines.Length;
            int valueCompare;
            for (int indexData = 0; indexData < data_length; indexData++)
            {
                data_line = data_lines[indexData]; 
                valueCompare = -2;
                while ((indexMbco < mbco_associations_length) && (valueCompare <= 0))
                {
                    mbco_association_line = mbco_association_lines[indexMbco];
                    valueCompare = mbco_association_line.Symbol.Length.CompareTo(data_line.NCBI_official_symbol.Length);
                    if (valueCompare == 0)
                    {
                        valueCompare = mbco_association_line.Symbol.CompareTo(data_line.NCBI_official_symbol);
                    }
                    if (valueCompare < 0)
                    {
                        indexMbco++;
                    }
                    else if (valueCompare == 0)
                    {
                        if (!processName_overlapSymbols_dict.ContainsKey(mbco_association_line.ProcessName))
                        {
                            processName_overlapSymbols_dict.Add(mbco_association_line.ProcessName, new List<string>[col_count]);
                            for (int indexCol = 0; indexCol < col_count; indexCol++)
                            {
                                processName_overlapSymbols_dict[mbco_association_line.ProcessName][indexCol] = new List<string>();
                            }
                        }
                        for (int indexCol = 0; indexCol < col_count; indexCol++)
                        {
                            if (data_line.Columns[indexCol] != data_empty_value)
                            {
                                processName_overlapSymbols_dict[mbco_association_line.ProcessName][indexCol].Add(data_line.NCBI_official_symbol);
                            }
                        }
                        indexMbco++;
                    }
                }
                for (int indexCol = 0; indexCol < col_count; indexCol++)
                {
                    if (data_line.Columns[indexCol] != data_empty_value)
                    {
                        col_entries_count_line.Columns[indexCol]++;
                    }
                }
            }
            data.Column_entries_count_line = col_entries_count_line;
            return processName_overlapSymbols_dict;
        }

        private Ontology_enrichment_line_class[] Generate_enrichment_lines(Dictionary<string, List<string>[]> processName_colIndex_overlapSymbols_dict, Data_class data)
        {
            List<Ontology_enrichment_line_class> enrichment_lines = new List<Ontology_enrichment_line_class>();
            string[] processNames = processName_colIndex_overlapSymbols_dict.Keys.ToArray();
            string processName;
            int processNames_length = processNames.Length;
            Ontology_enrichment_line_class new_enrichment_line;
            List<Ontology_enrichment_line_class> enrichment_lines_list = new List<Ontology_enrichment_line_class>();

            Colchar_column_line_class[] columns = data.ColChar.Columns;
            Colchar_column_line_class column_line;
            Data_line_class data_column_entries_line = data.Column_entries_count_line;
            int col_length = columns.Length;
            List<string>[] overlap_symbols_of_each_column;
            for (int indexPN = 0; indexPN < processNames_length; indexPN++)
            {
                processName = processNames[indexPN];
                overlap_symbols_of_each_column = processName_colIndex_overlapSymbols_dict[processName];
                for (int indexCol = 0; indexCol < col_length; indexCol++)
                {
                    if (overlap_symbols_of_each_column[indexCol].Count > 0)
                    {
                        column_line = columns[indexCol];
                        new_enrichment_line = new Ontology_enrichment_line_class();
                        new_enrichment_line.Ontology_type = this.Options.Ontology;
                        new_enrichment_line.Scp_name = (string)processName.Clone();
                        new_enrichment_line.Integration_group = (string)column_line.IntegrationGroup.Clone();
                        new_enrichment_line.Unique_dataset_name = (string)column_line.UniqueDataset_name.Clone();
                        new_enrichment_line.Sample_name = column_line.SampleName;
                        new_enrichment_line.EntryType = column_line.EntryType;
                        new_enrichment_line.Timepoint = column_line.Timepoint;
                        new_enrichment_line.Timeunit = column_line.Timeunit;
                        new_enrichment_line.Sample_color = column_line.SampleColor;
                        new_enrichment_line.Overlap_symbols = overlap_symbols_of_each_column[indexCol].ToArray();
                        new_enrichment_line.Overlap_count = new_enrichment_line.Overlap_symbols.Length;
                        new_enrichment_line.Experimental_symbols_count = (int)data_column_entries_line.Columns[indexCol];
                        new_enrichment_line.Results_number = column_line.Results_no;
                        enrichment_lines_list.Add(new_enrichment_line);
                    }
                }
            }
            return enrichment_lines_list.ToArray();
        }

        private void Add_missing_process_information_and_backgroundGenes_count_for_standard_scps(ref Ontology_enrichment_line_class[] enrichment_lines)
        {
            int background_genes_length = this.Final_bg_genes.Length;
            enrichment_lines = enrichment_lines.OrderBy(l => l.Scp_name).ToArray();
            int enrich_length = enrichment_lines.Length;
            Ontology_enrichment_line_class enrich_line;
            MBCO_association.Order_by_processName_symbol();
            int onto_length = MBCO_association.MBCO_associations.Length;
            int indexOnto = 0;
            MBCO_association_line_class mbco_association_line;
            int stringCompare;
            int process_symbols_count = 1;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = enrichment_lines[indexE];
                stringCompare = -2;
                while ((indexOnto < onto_length) && (stringCompare < 0))
                {
                    mbco_association_line = MBCO_association.MBCO_associations[indexOnto];
                    stringCompare = mbco_association_line.ProcessName.CompareTo(enrich_line.Scp_name);
                    if (stringCompare < 0)
                    {
                        indexOnto++;
                        process_symbols_count = 1;
                    }
                    else if (stringCompare == 0)
                    {
                        while ((indexOnto < onto_length - 1) && (mbco_association_line.ProcessName.Equals(MBCO_association.MBCO_associations[indexOnto + 1].ProcessName)))
                        {
                            process_symbols_count++;
                            indexOnto++;
                        }
                        if (!string.IsNullOrEmpty(mbco_association_line.ProcessID)) { enrich_line.Scp_id = (string)mbco_association_line.ProcessID.Clone(); }
                        enrich_line.ProcessLevel = mbco_association_line.ProcessLevel;
                        if (!string.IsNullOrEmpty(mbco_association_line.Parent_processName)) { enrich_line.Parent_scp_name = (string)mbco_association_line.Parent_processName.Clone(); }
                        enrich_line.Process_symbols_count = process_symbols_count;
                        enrich_line.Bg_symbol_count = background_genes_length;
                    }
                }
            }
        }

        private void Add_missing_process_information_and_backgroundGenes_count_for_scp_unions(ref Ontology_enrichment_line_class[] enrichment_lines, Dictionary<string, List<string>> scp_scpUnion_dict)
        {
            MBCO_association.Order_by_symbol_processName();
            Dictionary<string, int> scpUnion_symbols_length_dict = new Dictionary<string, int>();
            MBCO_association_line_class mbco_association_line;
            int mbco_association_length = MBCO_association.MBCO_associations.Length;
            List<string> scp_unions_of_current_scp;
            List<string> add_one_to_gene_count_of_scp_unions = new List<string>();
            Dictionary<string, int> scpUnion_genes_count_dict = new Dictionary<string, int>();
            Dictionary<string, int> scpUnion_level_dict = new Dictionary<string, int>();
            for (int indexMBCO = 0; indexMBCO < mbco_association_length; indexMBCO++)
            {
                mbco_association_line = MBCO_association.MBCO_associations[indexMBCO];
                if ((indexMBCO == 0) || (!mbco_association_line.Symbol.Equals(MBCO_association.MBCO_associations[indexMBCO - 1].Symbol)))
                {
                    add_one_to_gene_count_of_scp_unions.Clear();
                }
                if (scp_scpUnion_dict.ContainsKey(mbco_association_line.ProcessName))
                {
                    scp_unions_of_current_scp = scp_scpUnion_dict[mbco_association_line.ProcessName];
                    add_one_to_gene_count_of_scp_unions.AddRange(scp_unions_of_current_scp);
                    foreach (string scp_union_of_curren_scp in scp_unions_of_current_scp)
                    {
                        if (!scpUnion_level_dict.ContainsKey(scp_union_of_curren_scp))
                        {
                            scpUnion_level_dict.Add(scp_union_of_curren_scp, mbco_association_line.ProcessLevel);
                        }
                    }
                }
                if ((indexMBCO == mbco_association_length - 1) || (!mbco_association_line.Symbol.Equals(MBCO_association.MBCO_associations[indexMBCO + 1].Symbol)))
                {
                    add_one_to_gene_count_of_scp_unions = add_one_to_gene_count_of_scp_unions.Distinct().ToList();
                    foreach (string scp_union in add_one_to_gene_count_of_scp_unions)
                    {
                        if (!scpUnion_genes_count_dict.ContainsKey(scp_union)) { scpUnion_genes_count_dict.Add(scp_union, 0); }
                        scpUnion_genes_count_dict[scp_union]++;
                    }
                }
            }

            int background_symbols_count = this.Final_bg_genes.Length;
            int enrichment_lines_length = enrichment_lines.Length;
            Ontology_enrichment_line_class enrichment_line;
            for (int indexE = 0; indexE < enrichment_lines_length; indexE++)
            {
                enrichment_line = enrichment_lines[indexE];
                if (scpUnion_genes_count_dict.ContainsKey(enrichment_line.Scp_name))
                {
                    enrichment_line.Process_symbols_count = scpUnion_genes_count_dict[enrichment_line.Scp_name];
                    enrichment_line.ProcessLevel = scpUnion_level_dict[enrichment_line.Scp_name];
                    enrichment_line.Bg_symbol_count = background_symbols_count;
                    enrichment_line.Parent_scp_name = "No annotated parent SCP";
                    enrichment_line.Scp_id = "Dynamic SCP union";
                }
            }
        }

        private Dictionary<string, List<string>> Generate_all_scp_unions_and_add_them_with_overlap_symbols_to_scp_colIndex_overlapSymbols_dict(ref Dictionary<string, List<string>[]> processName_colIndex_overlapSymbols_dict)
        {
            string[] experimental_processNames = processName_colIndex_overlapSymbols_dict.Keys.ToArray();
            Dictionary<string, List<string>> scp_scpUnion_dict = new Dictionary<string, List<string>>();
            if (experimental_processNames.Length > 0)
            {
                string[][] array_of_scps_in_one_union = Leave_out_scp_network_for_dynamicEnrichment_analysis.Generate_array_of_scp_unions_between_any_combination_between_two_or_three_neighboring_selected_scps(experimental_processNames);
                string[] consideredSCP_names = processName_colIndex_overlapSymbols_dict.Keys.ToArray();
                int data_col_length = processName_colIndex_overlapSymbols_dict[consideredSCP_names[0]].Length;

                int unions_length = array_of_scps_in_one_union.Length;
                string[] scps_of_one_union;
                string scp;
                int scps_of_one_union_length;
                StringBuilder sb = new StringBuilder();
                string name_of_scp_union;
                List<string>[] unionScpOverlapSymbols_of_each_column = new List<string>[data_col_length];
                List<string>[] currentScpOverlapSymbols_of_each_column;
                bool[] consider_column = new bool[data_col_length];
                for (int indexUnion = 0; indexUnion < unions_length; indexUnion++)
                {
                    scps_of_one_union = array_of_scps_in_one_union[indexUnion];
                    scps_of_one_union_length = scps_of_one_union.Length;

                    #region Clear stringbuilder and reset unionScpOverlapSymbols_of_each_column and consider scp
                    sb.Clear();
                    unionScpOverlapSymbols_of_each_column = new List<string>[data_col_length];
                    for (int indexCol = 0; indexCol < data_col_length; indexCol++)
                    {
                        unionScpOverlapSymbols_of_each_column[indexCol] = new List<string>();
                        consider_column[indexCol] = true;
                    }
                    #endregion

                    for (int indexScp = 0; indexScp < scps_of_one_union_length; indexScp++)
                    {
                        scp = scps_of_one_union[indexScp];
                        if (indexScp != 0) { sb.AppendFormat("$"); }
                        sb.AppendFormat(scp);
                        currentScpOverlapSymbols_of_each_column = processName_colIndex_overlapSymbols_dict[scp];
                        for (int indexCol = 0; indexCol < data_col_length; indexCol++)
                        {
                            if (currentScpOverlapSymbols_of_each_column[indexCol].Count == 0)
                            {
                                consider_column[indexCol] = false;
                                unionScpOverlapSymbols_of_each_column[indexCol].Clear();
                            }
                            else if (consider_column[indexCol])
                            {
                                unionScpOverlapSymbols_of_each_column[indexCol].AddRange(currentScpOverlapSymbols_of_each_column[indexCol]);
                            }
                        }
                    }
                    for (int indexCol = 0; indexCol < data_col_length; indexCol++)
                    {
                        unionScpOverlapSymbols_of_each_column[indexCol] = unionScpOverlapSymbols_of_each_column[indexCol].Distinct().OrderBy(l => l).ToList();
                    }
                    name_of_scp_union = sb.ToString();
                    for (int indexScp = 0; indexScp < scps_of_one_union_length; indexScp++)
                    {
                        scp = scps_of_one_union[indexScp];
                        if (!scp_scpUnion_dict.ContainsKey(scp)) { scp_scpUnion_dict.Add(scp, new List<string>()); }
                        scp_scpUnion_dict[scp].Add(name_of_scp_union);
                    }
                    for (int indexCol = 0; indexCol < data_col_length; indexCol++)
                    {
                        if (consider_column[indexCol])
                        {
                            processName_colIndex_overlapSymbols_dict.Add(name_of_scp_union, unionScpOverlapSymbols_of_each_column);
                            break;
                        }
                    }
                }
            }
            return scp_scpUnion_dict;
        }

        private void Calculate_missing_pvalues(ref Ontology_enrichment_line_class[] enrichment_lines)
        {
            int enrich_length = enrichment_lines.Length;
            Ontology_enrichment_line_class enrich_line;
            int a; int b; int c; int d;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = enrichment_lines[indexE];
                if (enrich_line.Pvalue == -1)
                {
                    a = enrich_line.Overlap_count;
                    b = enrich_line.Experimental_symbols_count - a;
                    c = enrich_line.Process_symbols_count - a;
                    d = enrich_line.Bg_symbol_count - a - b - c;
                    if ((a < 0) || (b < 0) || (c < 0) || (d < 0))
                    {
                        throw new Exception();
                    }
                    enrich_line.Pvalue = Fisher.Get_rightTailed_p_value(a, b, c, d);
                    if (enrich_line.Pvalue > 1)
                    {
                        if (enrich_line.Pvalue < 1.0001)
                        {
                            enrich_line.Pvalue = 1;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
            }
        }

        private void Calculate_minusLog10pvalues(ref Ontology_enrichment_line_class[] enrichment_lines)
        {
            int enrich_length = enrichment_lines.Length;
            Ontology_enrichment_line_class enrich_line;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = enrichment_lines[indexE];
                enrich_line.Minus_log10_pvalue = -(float)Math.Log10(enrich_line.Pvalue);
            }
        }

        private void Generate_standard_and_dynamic_enrichment_result_lines(Ontology_enrichment_line_class[] combined_enrichment_lines, out Ontology_enrichment_line_class[] standard_enrichment_lines, out Ontology_enrichment_line_class[] dynamic_enrichment_lines)
        {
            List<Ontology_enrichment_line_class> standard_enrichment_analysis_list = new List<Ontology_enrichment_line_class>();
            List<Ontology_enrichment_line_class> dynamic_enrichment_analysis_list = new List<Ontology_enrichment_line_class>();
            int combined_length = combined_enrichment_lines.Length;
            Ontology_enrichment_line_class combined_line;
            for (int indexC = 0; indexC < combined_length; indexC++)
            {
                combined_line = combined_enrichment_lines[indexC];
                if ((combined_line.ProcessLevel != 2) && (combined_line.ProcessLevel != 3))
                {
                    standard_enrichment_analysis_list.Add(combined_line);
                }
                else if (combined_line.Scp_name.IndexOf("$") != -1)
                {
                    dynamic_enrichment_analysis_list.Add(combined_line);
                }
                else
                {
                    standard_enrichment_analysis_list.Add(combined_line.Deep_copy());
                    dynamic_enrichment_analysis_list.Add(combined_line);
                }
            }
            standard_enrichment_lines = standard_enrichment_analysis_list.ToArray();
            dynamic_enrichment_lines = dynamic_enrichment_analysis_list.ToArray();
        }

        public void Analyse_data_instance_fast(Data_class data_input, string base_file_name, string fileName_addition, out Ontology_enrichment_class standard_enrichment_unfiltered, out Ontology_enrichment_class dynamic_enrichment_unfiltered, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            if (!Options.Ontology.Equals(Options.Next_ontology)) { throw new Exception(); }
            //Generate_leave_out_scp_network(error_report_label, error_report_label_x_location);
            this.Base_file_name = base_file_name;
            int columns_length = data_input.ColChar.Columns_length;
            int runs = (int)Math.Ceiling((float)columns_length / (float)Options.Max_columns_per_analysis);
            int currentFirstColumn = -1;
            int currentLastColumn = -1;
            List<int> keep_columns = new List<int>();
            List<Ontology_enrichment_line_class> standard_enrichment_lines_list = new List<Ontology_enrichment_line_class>();
            List<Ontology_enrichment_line_class> dynamic_enrichment_lines_list = new List<Ontology_enrichment_line_class>();
            for (int indexRun = 0; indexRun < runs; indexRun++)
            {
                currentFirstColumn = indexRun * Options.Max_columns_per_analysis;
                currentLastColumn = Math.Min((indexRun + 1) * Options.Max_columns_per_analysis - 1, columns_length - 1);
                keep_columns.Clear();
                for (int keepColumn = currentFirstColumn; keepColumn <= currentLastColumn; keepColumn++)
                {
                    keep_columns.Add(keepColumn);
                }
                Data_class data = data_input.Deep_copy();
                data.Keep_only_input_columns_and_remove_all_rows_that_are_left_over_with_only_zero_values(keep_columns.ToArray());
                data.Keep_only_input_rowNames(this.Final_bg_genes);
                if (data.Data.Length > 0)
                {
                    Dictionary<string, List<string>[]> scp_colIndex_overlapSymbols_dict = Generate_processName_colIndex_overlapSymbols_dictionary_and_count_column_entries_in_data(ref data);
                    Dictionary<string, List<string>> scp_scpUnion_dict = Generate_all_scp_unions_and_add_them_with_overlap_symbols_to_scp_colIndex_overlapSymbols_dict(ref scp_colIndex_overlapSymbols_dict);
                    Ontology_enrichment_line_class[] current_combined_enrichment_lines = Generate_enrichment_lines(scp_colIndex_overlapSymbols_dict, data);
                    Add_missing_process_information_and_backgroundGenes_count_for_standard_scps(ref current_combined_enrichment_lines);
                    Add_missing_process_information_and_backgroundGenes_count_for_scp_unions(ref current_combined_enrichment_lines, scp_scpUnion_dict);

                    Calculate_missing_pvalues(ref current_combined_enrichment_lines);
                    Check_for_duplicated_enrichment_lines(current_combined_enrichment_lines);

                    Ontology_enrichment_line_class[] current_standard_enrichment_lines;
                    Ontology_enrichment_line_class[] current_dynamic_enrichment_lines;

                    Generate_standard_and_dynamic_enrichment_result_lines(current_combined_enrichment_lines, out current_standard_enrichment_lines, out current_dynamic_enrichment_lines);
                    standard_enrichment_lines_list.AddRange(current_standard_enrichment_lines);
                    dynamic_enrichment_lines_list.AddRange(current_dynamic_enrichment_lines);
                }
                else
                {
                    //not analyzed notice
                }
            }
            Ontology_enrichment_line_class[] standard_enrichment_lines = standard_enrichment_lines_list.ToArray();
            Ontology_enrichment_line_class[] dynamic_enrichment_lines = dynamic_enrichment_lines_list.ToArray();

            Calculate_minusLog10pvalues(ref standard_enrichment_lines);
            Check_for_duplicated_enrichment_lines(standard_enrichment_lines);
            Calculate_minusLog10pvalues(ref dynamic_enrichment_lines);
            Check_for_duplicated_enrichment_lines(dynamic_enrichment_lines);
            standard_enrichment_unfiltered = new Ontology_enrichment_class();
            standard_enrichment_unfiltered.Add_other_lines_without_resetting_unique_datasetNames(standard_enrichment_lines);
            standard_enrichment_unfiltered.Calculate_fractional_ranks_for_SCPs_within_each_integrationGroup_sampleName_timepoint_timeunit_entryType_processLevel();
            dynamic_enrichment_unfiltered = new Ontology_enrichment_class();
            dynamic_enrichment_unfiltered.Add_other_lines_without_resetting_unique_datasetNames(dynamic_enrichment_lines);
            dynamic_enrichment_unfiltered.Calculate_fractional_ranks_for_SCPs_within_each_integrationGroup_sampleName_timepoint_timeunit_entryType_processLevel();
        }
        #endregion

        public string[] Get_all_symbols_of_scp_names_after_updating_instance_if_ontology_unequals_next_ontology(System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings, params string[] scp_names)
        {
            if (!Options.Ontology.Equals(Options.Next_ontology))
            {
                Generate_for_all_runs_after_setting_ontology_to_next_ontology(error_report_label, form_default_settings);
            }
            return MBCO_association_unmodified.Get_all_symbols_of_process_names(scp_names);
        }
        public string[] Get_all_symbols_of_any_SCPs_after_updating_instance_if_ontology_unequals_next_ontology(System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings, params string[] scp_names)
        {
            if (!Options.Ontology.Equals(Options.Next_ontology))
            {
                Generate_for_all_runs_after_setting_ontology_to_next_ontology(error_report_label, form_default_settings);
            }
            return MBCO_association_unmodified.Get_all_distinct_ordered_symbols();
        }

        private string Get_results_subdirectory_for_indicated_ontology(Ontology_type_enum ontology)
        {
            return this.Base_file_name + "_" + ontology + "//";
        }

        #region Integrative dynamic networks
        private Network_class Generate_network_from_ontology_enrichment_lines(Ontology_enrichment_line_class[] ontology_enrichment_lines_input)
        {
            int ontology_enrichment_lines_length = ontology_enrichment_lines_input.Length;
            Ontology_enrichment_line_class[] ontology_enrichment_lines = new Ontology_enrichment_line_class[ontology_enrichment_lines_length];
            Ontology_enrichment_line_class ontology_enrichment_line;

            for (int indexOE = 0; indexOE < ontology_enrichment_lines_length; indexOE++)
            {
                ontology_enrichment_lines[indexOE] = ontology_enrichment_lines_input[indexOE].Deep_copy();
            }

            ontology_enrichment_lines = ontology_enrichment_lines.OrderByDescending(l => l.ProcessLevel).ToArray();

            string current_scp;
            string[] current_ancestors;
            string current_ancestor;
            int current_ancestors_length;
            string[] genes_in_current_scp;
            string[] remove_genes;
            Dictionary<string, List<string>> scp_genesToBeRemoved_dictionary = new Dictionary<string, List<string>>();

            for (int indexOE = 0; indexOE < ontology_enrichment_lines_length; indexOE++)
            {
                ontology_enrichment_line = ontology_enrichment_lines[indexOE];
                current_scp = ontology_enrichment_line.Scp_name;

                if (scp_genesToBeRemoved_dictionary.ContainsKey(current_scp))
                {
                    remove_genes = scp_genesToBeRemoved_dictionary[current_scp].ToArray();
                    ontology_enrichment_line.Overlap_symbols = Overlap_class.Get_part_of_list1_but_not_of_list2(ontology_enrichment_line.Overlap_symbols, remove_genes);
                }
                current_ancestors = Mbco_childParent_nw.Get_all_ancestors_if_direction_is_child_parent(current_scp);
                genes_in_current_scp = ontology_enrichment_line.Overlap_symbols;
                current_ancestors_length = current_ancestors.Length;
                for (int indexAncestor = 0; indexAncestor < current_ancestors_length; indexAncestor++)
                {
                    current_ancestor = current_ancestors[indexAncestor];
                    if (!scp_genesToBeRemoved_dictionary.ContainsKey(current_ancestor))
                    {
                        scp_genesToBeRemoved_dictionary.Add(current_ancestor, new List<string>());
                    }
                    scp_genesToBeRemoved_dictionary[current_ancestor].AddRange(genes_in_current_scp);
                }
            }

            Network_class nw = new Network_class();

            NetworkTable_line_class new_network_table_line;
            List<NetworkTable_line_class> new_network_table_list = new List<NetworkTable_line_class>();
            string[] target_symbols;
            string target_symbol;
            int target_symbols_length;
            int indexFirstOpenBracket;
            for (int indexO = 0; indexO < ontology_enrichment_lines_length; indexO++)
            {
                ontology_enrichment_line = ontology_enrichment_lines[indexO];
                target_symbols = ontology_enrichment_line.Overlap_symbols;
                target_symbols_length = target_symbols.Length;
                for (int indexT = 0; indexT < target_symbols_length; indexT++)
                {
                    target_symbol = target_symbols[indexT];
                    indexFirstOpenBracket = target_symbol.IndexOf('(');
                    if (indexFirstOpenBracket != -1)
                    {
                        target_symbol = target_symbol.Substring(0, indexFirstOpenBracket - 1);
                    }
                    new_network_table_line = new NetworkTable_line_class();
                    new_network_table_line.Source = (string)ontology_enrichment_line.Scp_name.Clone();
                    new_network_table_line.Target = (string)target_symbol.Clone();
                    new_network_table_list.Add(new_network_table_line);
                }
            }
            nw.Add_from_networkTable_lines(new_network_table_list.ToArray());
            return nw;
        }

        private void Get_technology_setName_and_legendName(Dictionary<string, string> technology_predefinedSetName_dict, string sample_name, out string technology, out string legend_name, out string set_name)
        {
            technology = sample_name.Split('$')[0];
            string cluster_full_name = sample_name.Split('$')[1];
            int indexNo = cluster_full_name.IndexOf("no");
            if (indexNo != -1)
            {
                string cluster_no_string = cluster_full_name.Substring(indexNo, cluster_full_name.Length - indexNo);
                legend_name = technology + " " + cluster_no_string;
                set_name = technology_predefinedSetName_dict[technology] + " (" + cluster_no_string + ")";
            }
            else
            {
                legend_name = technology;
                set_name = technology_predefinedSetName_dict[technology];
            }
        }
        #endregion
    }

    class MBCO_network_based_integration_options_class : Options_readWrite_base_class
    {
        #region Const min max values
        private const float const_min_top_quantile_probability_of_scp_interactions = MBCO_enrichment_pipeline_options_class.Const_min_top_quantile_of_scp_interactions;
        private const float const_max_top_quantile_probability_of_scp_interactions = MBCO_enrichment_pipeline_options_class.Const_max_top_quantile_of_scp_interactions;
        #endregion

        #region private dictionaries
        private Dictionary<Ontology_type_enum, bool> Ontology_add_genes_to_standard_networks { get; set; }
        private Dictionary<Ontology_type_enum, bool> Ontology_add_genes_to_dynamic_networks { get; set; }
        private Dictionary<Ontology_type_enum, bool> Ontology_add_edges_that_connect_standard_scps { get; set; }
        private Dictionary<Ontology_type_enum, bool> Ontology_add_additional_edges_that_connect_dynamic_scps { get; set; }
        private Dictionary<Ontology_type_enum, bool> Ontology_add_parent_child_relationships_to_dynamic_SCP_networks { get; set; }
        private Dictionary<Ontology_type_enum, bool> Ontology_add_parent_child_relationships_to_standard_SCP_networks { get; set; }
        private Dictionary<Ontology_type_enum, float[]> Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level { get; set; }
        private Dictionary<Ontology_type_enum, float[]> Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level { get; set; }
        private Dictionary<Ontology_type_enum, bool> Ontology_generate_scp_networks { get; set; }
        private Dictionary<Ontology_type_enum, Yed_network_node_size_determinant_enum> Ontology_node_size_determinant { get; set; }
        private Dictionary<Ontology_type_enum, bool> Ontology_box_sameLevel_scps_for_standard_enrichment { get; set; }
        private Dictionary<Ontology_type_enum, bool> Ontology_box_sameLevel_scps_for_dynamic_enrichment { get; set; }
        #endregion


        public Ontology_type_enum _ontology;
        public Ontology_type_enum Next_ontology { get; set; }
        public Ontology_type_enum Ontology { get { return _ontology; } private set {  _ontology = value; } }
        public bool Add_genes_to_standard_networks
        { 
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_add_genes_to_standard_networks[human_ontology];
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                Ontology_add_genes_to_standard_networks[human_ontology] = value;
            }
        }
        public bool Add_genes_to_dynamic_networks
        { 
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_add_genes_to_dynamic_networks[human_ontology];
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                Ontology_add_genes_to_dynamic_networks[human_ontology] = value;
            }
        }
        public bool Add_edges_that_connect_standard_scps
        {
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_add_edges_that_connect_standard_scps[human_ontology];
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                Ontology_add_edges_that_connect_standard_scps[human_ontology] = value;
            }
        }
        public bool Add_additional_edges_that_connect_dynamic_scps
        {
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_add_additional_edges_that_connect_dynamic_scps[human_ontology];
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                Ontology_add_additional_edges_that_connect_dynamic_scps[human_ontology] = value;
            }
        }
        public bool Add_parent_child_relationships_to_dynamic_SCP_networks
        {
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_add_parent_child_relationships_to_dynamic_SCP_networks[human_ontology];
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                Ontology_add_parent_child_relationships_to_dynamic_SCP_networks[human_ontology] = value;
            }
        }
        public bool Add_parent_child_relationships_to_standard_SCP_networks
        {
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_add_parent_child_relationships_to_standard_SCP_networks[human_ontology];
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                Ontology_add_parent_child_relationships_to_standard_SCP_networks[human_ontology] = value;
            }
        }
        public float[] Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level 
        { 
          get 
          {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level[human_ontology];
          }
          set 
          {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                int values_length = value.Length;
                for (int indexV=0; indexV < values_length; indexV++)
                {
                    switch (indexV)
                    {
                        case 0:
                        case 1:
                        case 4:
                            Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level[human_ontology][indexV] = -1;
                            break;
                        case 2:
                        case 3:
                            Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level[human_ontology][indexV] = Math.Min(const_max_top_quantile_probability_of_scp_interactions,Math.Max(const_min_top_quantile_probability_of_scp_interactions, value[indexV]));
                            break;
                        default:
                            throw new Exception();
                    }
                }
          }
        }
        public float[] Top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level
        {
            get 
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level[human_ontology]; 
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                int values_length = value.Length;
                for (int indexV = 0; indexV < values_length; indexV++)
                {
                    switch (indexV)
                    {
                        case 0:
                        case 1:
                        case 4:
                            Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level[human_ontology][indexV] = -1;
                            break;
                        case 2:
                        case 3:
                            Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level[human_ontology][indexV] = Math.Min(const_max_top_quantile_probability_of_scp_interactions, Math.Max(const_min_top_quantile_probability_of_scp_interactions, value[indexV]));
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
        }
        public bool Generate_scp_networks
        {
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_generate_scp_networks[human_ontology];
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                Ontology_generate_scp_networks[human_ontology] = value;
            }
        }
        public Yed_network_node_size_determinant_enum Node_size_determinant
        {
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_node_size_determinant[human_ontology];
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                Ontology_node_size_determinant[human_ontology] = value;
            }
        }
        public bool Box_sameLevel_scps_for_standard_enrichment
        {
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_box_sameLevel_scps_for_standard_enrichment[human_ontology];
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                Ontology_box_sameLevel_scps_for_standard_enrichment[human_ontology] = value;
            }
        }
        public bool Box_sameLevel_scps_for_dynamic_enrichment
        {
            get
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                return Ontology_box_sameLevel_scps_for_dynamic_enrichment[human_ontology];
            }
            set
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Next_ontology);
                Ontology_box_sameLevel_scps_for_dynamic_enrichment[human_ontology] = value;
            }
        }

        public MBCO_network_based_integration_options_class(Ontology_type_enum next_ontology)
        {
            Next_ontology = next_ontology;
            Ontology = Ontology_type_enum.E_m_p_t_y;

            #region Initialize dictionaries
            Ontology_generate_scp_networks = new Dictionary<Ontology_type_enum, bool>();
            Ontology_add_genes_to_standard_networks = new Dictionary<Ontology_type_enum, bool>();
            Ontology_add_genes_to_dynamic_networks = new Dictionary<Ontology_type_enum, bool>();
            Ontology_add_parent_child_relationships_to_standard_SCP_networks = new Dictionary<Ontology_type_enum, bool>();
            Ontology_add_edges_that_connect_standard_scps = new Dictionary<Ontology_type_enum, bool>();
            Ontology_add_additional_edges_that_connect_dynamic_scps = new Dictionary<Ontology_type_enum, bool>();
            Ontology_add_parent_child_relationships_to_dynamic_SCP_networks = new Dictionary<Ontology_type_enum, bool>();
            Ontology_node_size_determinant = new Dictionary<Ontology_type_enum, Yed_network_node_size_determinant_enum>();
            Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level = new Dictionary<Ontology_type_enum, float[]>();
            Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level = new Dictionary<Ontology_type_enum, float[]>();
            Ontology_box_sameLevel_scps_for_dynamic_enrichment = new Dictionary<Ontology_type_enum, bool>();
            Ontology_box_sameLevel_scps_for_standard_enrichment = new Dictionary<Ontology_type_enum, bool>();
            #endregion

            Set_default_mbco_parameters();
            Set_default_parameters_for_other_ontologies();
        }

        private void Set_default_mbco_parameters()
        {
            Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(Ontology_type_enum.Mbco_human);
            Ontology_generate_scp_networks.Add(human_ontology, true);
            Ontology_add_genes_to_standard_networks.Add(human_ontology, false);
            Ontology_add_genes_to_dynamic_networks.Add(human_ontology, false);
            Ontology_add_parent_child_relationships_to_standard_SCP_networks.Add(human_ontology, true);
            Ontology_add_edges_that_connect_standard_scps.Add(human_ontology, false);
            Ontology_add_parent_child_relationships_to_dynamic_SCP_networks.Add(human_ontology, false);
            Ontology_add_additional_edges_that_connect_dynamic_scps.Add(human_ontology, true);
            Ontology_node_size_determinant.Add(human_ontology, Yed_network_node_size_determinant_enum.Standard);
            Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level.Add(human_ontology, new float[] { });
            Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level[human_ontology] = new float[] { -1, -1, 0, 0, -1 };
            Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level.Add(human_ontology, new float[5]);
            Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level[human_ontology] = new float[] { -1, -1, 0.2F, 0.25F, -1 };
            Ontology_box_sameLevel_scps_for_dynamic_enrichment.Add(human_ontology, true);
            Ontology_box_sameLevel_scps_for_standard_enrichment.Add(human_ontology, false);
        }

        private void Set_default_parameters_for_other_ontologies()
        {
            Ontology_type_enum[] go_ontologies = new Ontology_type_enum[] { Ontology_type_enum.Go_bp_human, Ontology_type_enum.Go_cc_human, Ontology_type_enum.Go_mf_human, Ontology_type_enum.Mbco_na_glucose_tm_transport_human };
            foreach (Ontology_type_enum go_ontology in go_ontologies)
            {
                Ontology_type_enum human_ontology = Ontology_classification_class.Get_related_human_ontology(go_ontology);
                Ontology_generate_scp_networks.Add(human_ontology, true);
                Ontology_add_genes_to_standard_networks.Add(human_ontology, false);
                Ontology_add_genes_to_dynamic_networks.Add(human_ontology, false);
                Ontology_add_parent_child_relationships_to_standard_SCP_networks.Add(human_ontology, true);
                Ontology_add_edges_that_connect_standard_scps.Add(human_ontology, false);
                Ontology_add_parent_child_relationships_to_dynamic_SCP_networks.Add(human_ontology, false);
                Ontology_add_additional_edges_that_connect_dynamic_scps.Add(human_ontology, false);
                Ontology_node_size_determinant.Add(human_ontology, Yed_network_node_size_determinant_enum.Standard);
                Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level.Add(human_ontology, new float[] { });
                Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level[human_ontology] = new float[] { -1, -1, -1, -1, -1 };
                Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level.Add(human_ontology, new float[5]);
                Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level[human_ontology] = new float[] { -1, -1, -1, -1, -1 };
                Ontology_box_sameLevel_scps_for_dynamic_enrichment.Add(human_ontology, true);
                Ontology_box_sameLevel_scps_for_standard_enrichment.Add(human_ontology, false);
            }
        }

        public MBCO_network_based_integration_options_class Deep_copy()
        {
            MBCO_network_based_integration_options_class copy = (MBCO_network_based_integration_options_class)this.MemberwiseClone();
            copy.Ontology_add_genes_to_standard_networks = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_add_genes_to_standard_networks);
            copy.Ontology_add_genes_to_dynamic_networks = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_add_genes_to_dynamic_networks);
            copy.Ontology_add_edges_that_connect_standard_scps = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_add_edges_that_connect_standard_scps);
            copy.Ontology_add_additional_edges_that_connect_dynamic_scps = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_add_additional_edges_that_connect_dynamic_scps);
            copy.Ontology_add_parent_child_relationships_to_dynamic_SCP_networks = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_add_parent_child_relationships_to_dynamic_SCP_networks);
            copy.Ontology_add_parent_child_relationships_to_standard_SCP_networks = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_add_parent_child_relationships_to_standard_SCP_networks);
            copy.Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level);
            copy.Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level);
            copy.Ontology_generate_scp_networks = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_generate_scp_networks);
            copy.Ontology_node_size_determinant = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_node_size_determinant);
            copy.Ontology_box_sameLevel_scps_for_standard_enrichment = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_box_sameLevel_scps_for_standard_enrichment);
            copy.Ontology_box_sameLevel_scps_for_dynamic_enrichment = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_box_sameLevel_scps_for_dynamic_enrichment);
            return copy;
        }
        public bool Add_parent_child_relationships(Enrichment_type_enum enrichment_type)
        {
            switch (enrichment_type)
            {
                case Enrichment_type_enum.Dynamic:
                    return Add_parent_child_relationships_to_dynamic_SCP_networks;
                case Enrichment_type_enum.Standard:
                    return Add_parent_child_relationships_to_standard_SCP_networks;
                default:
                    throw new Exception();
            }
        }

        public bool Add_genes_to_networks(Enrichment_type_enum enrichment_type)
        {
            switch (enrichment_type)
            {
                case Enrichment_type_enum.Dynamic:
                    return Add_genes_to_dynamic_networks;
                case Enrichment_type_enum.Standard:
                    return Add_genes_to_standard_networks;
                default:
                    throw new Exception();
            }
        }

        public bool Add_edges_that_connect_SCPs_within_and_between_sets(Enrichment_type_enum enrichment_type)
        {
            switch (enrichment_type)
            {
                case Enrichment_type_enum.Dynamic:
                    return true;
                case Enrichment_type_enum.Standard:
                    return Add_edges_that_connect_standard_scps;
                default:
                    throw new Exception();
            }
        }

        public bool Box_sameLevel_scps(Enrichment_type_enum enrichment_type)
        {
            switch (enrichment_type)
            {
                case Enrichment_type_enum.Standard:
                    return Box_sameLevel_scps_for_standard_enrichment;
                case Enrichment_type_enum.Dynamic:
                    return Box_sameLevel_scps_for_dynamic_enrichment;
                default:
                    throw new Exception();
            }
        }
        public float[] Get_top_quantile_probability_of_scp_interactions_for_dynamic_or_standard_enrichment_per_level(Enrichment_type_enum enrichment_type)
        {
            switch (enrichment_type)
            {
                case Enrichment_type_enum.Dynamic:
                    return Top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level;
                case Enrichment_type_enum.Standard:
                    return Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level;
                default:
                    throw new Exception();
            }
        }

        public override void Write_option_entries(System.IO.StreamWriter writer)
        {
           // base.Write_entries(writer, typeof(MBCO_network_based_integration_options_class));

            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_generate_scp_networks,"Generate_scp_networks");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_add_genes_to_standard_networks, "Add_genes_to_standard_networks");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_add_genes_to_dynamic_networks, "Add_genes_to_dynamic_networks");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_add_parent_child_relationships_to_standard_SCP_networks, "Add_parent_child_relationships_to_standard_SCP_networks");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_add_edges_that_connect_standard_scps, "Add_edges_that_connect_standard_scps");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_add_additional_edges_that_connect_dynamic_scps, "Add_additional_edges_that_connect_dynamic_scps");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_add_parent_child_relationships_to_dynamic_SCP_networks, "Add_parent_child_relationships_to_dynamic_SCP_networks");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_node_size_determinant, "Node_size_determinant");
            //base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level, "Top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level, "Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_box_sameLevel_scps_for_dynamic_enrichment, "Box_sameLevel_scps_for_dynamic_enrichment");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_box_sameLevel_scps_for_standard_enrichment, "Box_sameLevel_scps_for_standard_enrichment");
        }
        public override void Add_read_entry_to_options(string readLine)
        {
            string[] splitStrings = readLine.Split(Global_class.Tab);
            switch (splitStrings[1])
            {
                case "Generate_scp_networks":
                    Ontology_generate_scp_networks = base.Add_to_dictionary_entries(readLine, Ontology_generate_scp_networks);
                    break;
                case "Add_genes_to_standard_networks":
                    Ontology_add_genes_to_standard_networks = base.Add_to_dictionary_entries(readLine, Ontology_add_genes_to_standard_networks);
                    break;
                case "Add_genes_to_dynamic_networks":
                    Ontology_add_genes_to_dynamic_networks = base.Add_to_dictionary_entries(readLine, Ontology_add_genes_to_dynamic_networks);
                    break;
                case "Add_parent_child_relationships_to_standard_SCP_networks":
                    Ontology_add_parent_child_relationships_to_standard_SCP_networks = base.Add_to_dictionary_entries(readLine, Ontology_add_parent_child_relationships_to_standard_SCP_networks);
                    break;
                case "Add_edges_that_connect_standard_scps":
                    Ontology_add_edges_that_connect_standard_scps = base.Add_to_dictionary_entries(readLine, Ontology_add_edges_that_connect_standard_scps);
                    break;
                case "Add_additional_edges_that_connect_dynamic_scps":
                    Ontology_add_additional_edges_that_connect_dynamic_scps = base.Add_to_dictionary_entries(readLine, Ontology_add_additional_edges_that_connect_dynamic_scps);
                    break;
                case "Add_parent_child_relationships_to_dynamic_SCP_networks":
                    Ontology_add_parent_child_relationships_to_dynamic_SCP_networks = base.Add_to_dictionary_entries(readLine, Ontology_add_parent_child_relationships_to_dynamic_SCP_networks);
                    break;
                case "Node_size_determinant":
                    Ontology_node_size_determinant = base.Add_to_dictionary_entries(readLine, Ontology_node_size_determinant);
                    break;
                case "Top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level":
                    Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level = base.Add_to_dictionary_entries(readLine, Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level);
                    break;
                case "Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level":
                    Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level = base.Add_to_dictionary_entries(readLine, Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level);
                    break;
                case "Box_sameLevel_scps_for_dynamic_enrichment":
                    Ontology_box_sameLevel_scps_for_dynamic_enrichment = base.Add_to_dictionary_entries(readLine, Ontology_box_sameLevel_scps_for_dynamic_enrichment);
                    break;
                case "Box_sameLevel_scps_for_standard_enrichment":
                    Ontology_box_sameLevel_scps_for_standard_enrichment = base.Add_to_dictionary_entries(readLine, Ontology_box_sameLevel_scps_for_standard_enrichment);
                    break;
                default:
                    throw new Exception();
            }
        }

        public void Override_ontology(Ontology_type_enum selected_ontology)
        {
            _ontology = selected_ontology;
        }
    }

    class Mbc_network_based_integration_class
    {
        #region Fields
        public Leave_out_scp_scp_network_class Leave_out_background_scp_network { get; set; }
        public MBCO_network_based_integration_options_class Options { get; set; }
        private MBCO_network_based_integration_options_class Default_options { get; set; }
        public MBCO_obo_network_class Mbco_parentChild_nw { get; set; }
        public MBCO_obo_network_class Mbco_childParent_nw { get; set; }
        public bool Generated_for_all_runs { get; set; }
        #endregion

        public Mbc_network_based_integration_class(Ontology_type_enum next_ontology)
        {
            this.Default_options = new MBCO_network_based_integration_options_class(next_ontology);
            this.Options = Default_options.Deep_copy();
            Generated_for_all_runs = false;
        }

        #region Generate
        private void Generate_parent_child_and_child_parent_MBCO_networks(System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            Mbco_childParent_nw = new MBCO_obo_network_class(this.Options.Ontology);
            Mbco_parentChild_nw = new MBCO_obo_network_class(this.Options.Ontology);
            Mbco_parentChild_nw.Generate_by_reading_safed_obo_file(error_report_label, form_default_settings);
            if (Ontology_classification_class.Is_go_ontology(this.Options.Ontology))
            {
                Mbco_parentChild_nw.Keep_only_scps_of_selected_namespace_if_gene_ontology();
            }

            Mbco_childParent_nw = Mbco_parentChild_nw.Deep_copy_mbco_obo_nw();
            Mbco_childParent_nw.Reverse_direction();
        }

        public void Generate_for_all_runs_after_resetting_ontology(Ontology_type_enum ontology, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            this.Options.Override_ontology(ontology);
            Generate_parent_child_and_child_parent_MBCO_networks(error_report_label, form_default_settings);
            Generated_for_all_runs = true;
        }
        #endregion

        public void Replace_options_by_default_options_and_reset_generated_for_all_runs()
        {
            Options = Default_options.Deep_copy();
            Generated_for_all_runs = false;
        }

        #region Scp gene network
        private Network_class Generate_network_from_ontology_enrichment_lines_and_gene_colors_to_scpGeneLegend_color_dict(Ontology_enrichment_line_class[] sameUniqueDataset_ontology_enrichment_lines_input, ref Dictionary<string,List<Color>> scpGeneLegend_colors_dict)
        {
            string uniqueDatasetName = sameUniqueDataset_ontology_enrichment_lines_input[0].Unique_dataset_name;
            int ontology_enrichment_lines_length = sameUniqueDataset_ontology_enrichment_lines_input.Length;
            Ontology_enrichment_line_class[] sameUniqueDataset_ontology_enrichment_lines = new Ontology_enrichment_line_class[ontology_enrichment_lines_length];
            Ontology_enrichment_line_class ontology_enrichment_line;

            for (int indexOE = 0; indexOE < ontology_enrichment_lines_length; indexOE++)
            {
                sameUniqueDataset_ontology_enrichment_lines[indexOE] = sameUniqueDataset_ontology_enrichment_lines_input[indexOE].Deep_copy();
            }


            #region Remove genes from higher level SCPs, if they are part of lower level SCP children
            bool remove_genes_parent_SCPs_if_part_of_child_SCPs = true;
            Dictionary<string, Dictionary<string, bool>> scp_genesToBeRemoved_dictionary = new Dictionary<string, Dictionary<string, bool>>();
            if (remove_genes_parent_SCPs_if_part_of_child_SCPs)
            {
                sameUniqueDataset_ontology_enrichment_lines = sameUniqueDataset_ontology_enrichment_lines.OrderByDescending(l => l.ProcessLevel).ToArray();

                string current_scp;
                string[] current_ancestors;
                string current_ancestor;
                int current_ancestors_length;
                string[] genes_in_current_scp;
                //string[] remove_genes;

                for (int indexOE = 0; indexOE < ontology_enrichment_lines_length; indexOE++)
                {
                    ontology_enrichment_line = sameUniqueDataset_ontology_enrichment_lines[indexOE];
                    if (!ontology_enrichment_line.Unique_dataset_name.Equals(ontology_enrichment_line.Unique_dataset_name)) { throw new Exception(); }
                    current_scp = ontology_enrichment_line.Scp_name;

                    //if (scp_genesToBeRemoved_dictionary.ContainsKey(current_scp))
                    //{
                    //    remove_genes = scp_genesToBeRemoved_dictionary[current_scp].Keys.ToArray();
                    //    ontology_enrichment_line.Overlap_symbols = Overlap_class.Get_part_of_list1_but_not_of_list2(ontology_enrichment_line.Overlap_symbols, remove_genes);
                    //}
                    current_ancestors = Mbco_childParent_nw.Get_all_ancestors_if_direction_is_child_parent(current_scp);
                    genes_in_current_scp = ontology_enrichment_line.Overlap_symbols;
                    current_ancestors_length = current_ancestors.Length;
                    for (int indexAncestor = 0; indexAncestor < current_ancestors_length; indexAncestor++)
                    {
                        current_ancestor = current_ancestors[indexAncestor];
                        if (!scp_genesToBeRemoved_dictionary.ContainsKey(current_ancestor))
                        {
                            scp_genesToBeRemoved_dictionary.Add(current_ancestor, new Dictionary<string, bool>());
                        }
                        foreach (string gene_in_current_scp in genes_in_current_scp)
                        {
                            if (!scp_genesToBeRemoved_dictionary[current_ancestor].ContainsKey(gene_in_current_scp))
                            { scp_genesToBeRemoved_dictionary[current_ancestor].Add(gene_in_current_scp,true); }
                        }
                    }
                }
            }
            #endregion

            Network_class nw = new Network_class();

            NetworkTable_line_class new_network_table_line;
            List<NetworkTable_line_class> new_network_table_list = new List<NetworkTable_line_class>();
            string[] target_symbols;
            string target_symbol;
            string scp;
            int target_symbols_length;
            int indexFirstOpenBracket;
            Dictionary<string, bool> genesToBeRemoved_dict = new Dictionary<string, bool>();
            Dictionary<string, bool> added_gene_dict = new Dictionary<string, bool>();
            for (int indexO = 0; indexO < ontology_enrichment_lines_length; indexO++)
            {
                ontology_enrichment_line = sameUniqueDataset_ontology_enrichment_lines[indexO];
                scp = ontology_enrichment_line.Scp_name;
                if (scp_genesToBeRemoved_dictionary.ContainsKey(scp))
                { genesToBeRemoved_dict = scp_genesToBeRemoved_dictionary[scp]; }
                else { genesToBeRemoved_dict = new Dictionary<string, bool>(); }
                target_symbols = ontology_enrichment_line.Overlap_symbols;
                target_symbols_length = target_symbols.Length;
                for (int indexT = 0; indexT < target_symbols_length; indexT++)
                {
                    target_symbol = target_symbols[indexT];
                    indexFirstOpenBracket = target_symbol.IndexOf('(');
                    if (indexFirstOpenBracket != -1)
                    {
                        target_symbol = target_symbol.Substring(0, indexFirstOpenBracket - 1);
                    }
                    if (!genesToBeRemoved_dict.ContainsKey(target_symbol))
                    {
                        new_network_table_line = new NetworkTable_line_class();
                        new_network_table_line.Source = (string)ontology_enrichment_line.Scp_name.Clone();
                        new_network_table_line.Target = (string)target_symbol.Clone();
                        new_network_table_list.Add(new_network_table_line);
                        if (!added_gene_dict.ContainsKey(target_symbol))
                        {
                            added_gene_dict.Add(target_symbol, true);
                            if (!scpGeneLegend_colors_dict.ContainsKey(target_symbol)) { scpGeneLegend_colors_dict.Add(target_symbol, new List<Color>()); }
                            scpGeneLegend_colors_dict[target_symbol].Add(ontology_enrichment_line.Sample_color);
                        }
                    }
                }
            }
            nw.Add_from_networkTable_lines(new_network_table_list.ToArray());
            return nw;
        }
        #endregion

        #region Integrative networks
        private void Generate_leave_out_background_scp_network(Enrichment_type_enum enrichment_type, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            ///Leave out background network will be regenerated every enrichment cycle
            Dictionary<string, int> processName_processLevel_dict = Mbco_parentChild_nw.Get_processName_level_dictionary_without_setting_process_level();

            Leave_out_class leave_out = new Leave_out_class(Options.Ontology);
            leave_out.Generate_by_reading_safed_file(error_report_label, form_default_settings);

            Leave_out_background_scp_network = new Leave_out_scp_scp_network_class(this.Options.Ontology);
            Leave_out_background_scp_network.Options.Top_quantile_of_considered_SCP_interactions_per_level = Options.Get_top_quantile_probability_of_scp_interactions_for_dynamic_or_standard_enrichment_per_level(enrichment_type);
            Leave_out_background_scp_network.Generate_scp_scp_network_from_leave_out(leave_out, error_report_label, form_default_settings);
            Leave_out_background_scp_network.Scp_nw.Transform_into_undirected_single_network_and_set_all_widths_to_one();
            Leave_out_background_scp_network.Scp_nw.Nodes.Set_processLevel_for_all_nodes_based_on_dictionary(processName_processLevel_dict);
        }

        public bool Generate_and_write_integrative_network_for_indicated_enrichment_results_of_each_integrationGroupName_only_defined_sets_and_return_if_interrupted(Ontology_enrichment_class onto_enrich_for_network, Ontology_enrichment_class standard_onto_enrich_unfiltered_input, string results_directory, string baseFile_name, Enrichment_type_enum enrichmentType, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            if (!Options.Next_ontology.Equals(Options.Ontology)) { throw new Exception(); }
            Generate_leave_out_background_scp_network(enrichmentType, error_report_label, form_default_settings);

            #region Check and prepare input enrichment files
            Ontology_enrichment_class standard_onto_enrich_unfiltered = standard_onto_enrich_unfiltered_input.Deep_copy();
            Ontology_enrichment_condition_line_class[] keep_conditions = onto_enrich_for_network.Get_all_conditions();
            standard_onto_enrich_unfiltered.Keep_only_indicated_conditions(keep_conditions);
            string[] all_scps = onto_enrich_for_network.Get_all_scps_after_spliting_scp_unions();
            standard_onto_enrich_unfiltered.Keep_only_input_scpNames(all_scps);
            standard_onto_enrich_unfiltered.Check_for_correctness();
            standard_onto_enrich_unfiltered.Check_if_one_integrationGroup();
            onto_enrich_for_network.Check_for_correctness();
            onto_enrich_for_network.Check_if_one_integrationGroup();
            standard_onto_enrich_unfiltered.Order_by_uniqueDatasetName_scpName();
            onto_enrich_for_network.Order_by_uniqueDatasetName_scpName();
            #endregion

            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();

            int standard_onto_enrich_unfiletered_length = standard_onto_enrich_unfiltered.Enrich.Length;
            string subdirectory = results_directory + global_dirFile.Get_results_subdirectory_for_integrative_dynamic_scps_networks(onto_enrich_for_network.Get_ontology_and_check_if_only_one_ontology(), "SCP_networks");
            Ontology_enrichment_line_class onto_enrich_unfiltered_line = new Ontology_enrichment_line_class();
            int indexStandardUnfiltered_first = 0;
            int enrich_compare = -2;
            ReadWriteClass.Create_directory_if_it_does_not_exist(subdirectory);
            int standard_onto_enrich_length = onto_enrich_for_network.Enrich.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            Dictionary<string, int> legend_node_level_dict = new Dictionary<string, int>();

            Network_class current_uniqueDataset_scp_network = new Network_class(); 
            Leave_out_scp_scp_network_class current_enrichmentLine_scp_network;
            string[] scps;

            #region Variables for same integration group
            Network_class combined_scpNetwork = new Network_class();
            Dictionary<string, List<Color>> scpGeneLegend_colors_dict = new Dictionary<string, List<Color>>();
            List<string> sets_ofCurrentIntegrationGroup = new List<string>();
            #endregion

            Dictionary<string, int> processName_processLevel_dict = Mbco_parentChild_nw.Get_processName_level_dictionary_without_setting_process_level();
            Dictionary<string, bool> processName_added_for_uniqueDataset_dict = new Dictionary<string, bool>();
            string[] missing_scps;

            for (int indexO = 0; indexO < standard_onto_enrich_length; indexO++)
            {
                onto_enrich_line = onto_enrich_for_network.Enrich[indexO];
                if (!processName_processLevel_dict.ContainsKey(onto_enrich_line.Scp_name))
                {
                    processName_processLevel_dict.Add(onto_enrich_line.Scp_name, onto_enrich_line.ProcessLevel);
                }
                if ((indexO == 0)
                    || (!onto_enrich_line.Unique_dataset_name.Equals(onto_enrich_for_network.Enrich[indexO - 1].Unique_dataset_name)))
                {
                    current_uniqueDataset_scp_network = new Network_class();
                    scpGeneLegend_colors_dict.Add(onto_enrich_line.Unique_dataset_name, new List<Color>());
                    scpGeneLegend_colors_dict[onto_enrich_line.Unique_dataset_name].Add(onto_enrich_line.Sample_color);
                    processName_added_for_uniqueDataset_dict.Clear();
                    legend_node_level_dict.Add(onto_enrich_line.Unique_dataset_name,Global_class.Network_legend_level);
                }
                scps = onto_enrich_line.Scp_name.Split('$');
                scps = scps.OrderBy(l => l).ToArray();
                foreach (string scp in scps)
                {
                    if (!processName_added_for_uniqueDataset_dict.ContainsKey(scp))
                    {
                        if (!scpGeneLegend_colors_dict.ContainsKey(scp)) { scpGeneLegend_colors_dict.Add(scp, new List<Color>()); }
                        scpGeneLegend_colors_dict[scp].Add(onto_enrich_line.Sample_color);
                        processName_added_for_uniqueDataset_dict.Add(scp, true);
                    }
                }
                switch (enrichmentType)
                {
                    case Enrichment_type_enum.Dynamic:
                        current_enrichmentLine_scp_network = Leave_out_background_scp_network.Deep_copy_scp_network();
                        current_enrichmentLine_scp_network.Scp_nw.Keep_only_input_nodeNames(scps.ToArray());
                        missing_scps = Overlap_class.Get_part_of_list1_but_not_of_list2(scps.ToArray(), current_enrichmentLine_scp_network.Scp_nw.Nodes.Get_all_nodeNames());
                        current_enrichmentLine_scp_network.Scp_nw.Add_single_nodes(missing_scps);
                        current_uniqueDataset_scp_network.Merge_this_network_with_other_network(current_enrichmentLine_scp_network.Scp_nw);
                        break;
                    case Enrichment_type_enum.Standard:
                        current_uniqueDataset_scp_network.Add_single_nodes(scps.ToArray());
                        break;
                    default:
                        throw new Exception();
                }

                if ((indexO == standard_onto_enrich_length - 1)
                    || (!onto_enrich_line.Unique_dataset_name.Equals(onto_enrich_for_network.Enrich[indexO + 1].Unique_dataset_name)))
                {
                    if (Options.Add_genes_to_networks(enrichmentType))
                    {
                        enrich_compare = -2;
                        List<Ontology_enrichment_line_class> current_uniqueDataset_standard = new List<Ontology_enrichment_line_class>();
                        while ((indexStandardUnfiltered_first < standard_onto_enrich_unfiletered_length) && (enrich_compare <= 0))
                        {
                            onto_enrich_unfiltered_line = standard_onto_enrich_unfiltered.Enrich[indexStandardUnfiltered_first];
                            enrich_compare = onto_enrich_unfiltered_line.Unique_dataset_name.CompareTo(onto_enrich_line.Unique_dataset_name);
                            if (enrich_compare < 0)
                            {
                                indexStandardUnfiltered_first++;
                            }
                            else if (enrich_compare == 0)
                            {
                                if (processName_added_for_uniqueDataset_dict.ContainsKey(onto_enrich_unfiltered_line.Scp_name))
                                {
                                    current_uniqueDataset_standard.Add(onto_enrich_unfiltered_line);
                                }
                                indexStandardUnfiltered_first++;
                            }
                        }
                        Network_class gene_scp_network = Generate_network_from_ontology_enrichment_lines_and_gene_colors_to_scpGeneLegend_color_dict(current_uniqueDataset_standard.ToArray(), ref scpGeneLegend_colors_dict);
                        current_uniqueDataset_scp_network.Merge_this_network_with_other_network(gene_scp_network);
                    }
                    current_uniqueDataset_scp_network.Add_single_nodes(onto_enrich_line.Unique_dataset_name);
                    combined_scpNetwork.Merge_this_network_with_other_network(current_uniqueDataset_scp_network);
                }
            }

            if (Options.Add_parent_child_relationships(enrichmentType))
            {
                MBCO_obo_network_class scp_parent_child_network = this.Mbco_parentChild_nw.Deep_copy_mbco_obo_nw();
                string[] network_scps = combined_scpNetwork.Nodes.Get_all_nodeNames();
                string[] all_ancestor_scps = this.Mbco_childParent_nw.Get_all_ancestors_if_direction_is_child_parent(network_scps);
                all_ancestor_scps = Overlap_class.Get_union(all_ancestor_scps, network_scps);
                string[] intermediate_scps = Overlap_class.Get_part_of_list1_but_not_of_list2(all_ancestor_scps, network_scps);
                intermediate_scps = Overlap_class.Get_part_of_list1_but_not_of_list2(intermediate_scps, "Molecular Biology of the Cell");
                if (intermediate_scps.Length > 0)
                {
                    foreach (string intermediate_scp in intermediate_scps)
                    {
                        scpGeneLegend_colors_dict.Add(intermediate_scp, new List<Color>());
                        scpGeneLegend_colors_dict[intermediate_scp].Add(Global_class.Intermediate_network_scps_color);
                    }
                    string unpredicted_intermediate_scps_legend = "Not predicted intermediate SCPs";
                    scpGeneLegend_colors_dict.Add(unpredicted_intermediate_scps_legend, new List<Color>());
                    scpGeneLegend_colors_dict[unpredicted_intermediate_scps_legend].Add(Global_class.Intermediate_network_scps_color);
                    legend_node_level_dict.Add(unpredicted_intermediate_scps_legend, Global_class.Network_legend_level);
                    combined_scpNetwork.Add_single_nodes(unpredicted_intermediate_scps_legend);
                }
                scp_parent_child_network.Keep_only_input_nodeNames(all_ancestor_scps);
                combined_scpNetwork.Merge_this_network_with_other_network(scp_parent_child_network);
            }

            string comleteFileName = subdirectory + baseFile_name;

            if (  ((enrichmentType.Equals(Enrichment_type_enum.Standard))&&(Options.Add_edges_that_connect_standard_scps))
                ||((enrichmentType.Equals(Enrichment_type_enum.Dynamic))&&(Options.Add_additional_edges_that_connect_dynamic_scps)))
            {
                Network_class all_scp_network = Leave_out_background_scp_network.Deep_copy_scp_network().Scp_nw;
                all_scp_network.Keep_only_input_nodeNames(all_scps);
                all_scp_network.Merge_this_network_with_other_network(combined_scpNetwork);
                combined_scpNetwork = all_scp_network;
            }
            if (processName_processLevel_dict.Values.ToArray().Contains(Global_class.Network_genes_level)) { throw new Exception(); }
            if (processName_processLevel_dict.Values.ToArray().Contains(Global_class.Network_legend_level)) { throw new Exception(); }
            combined_scpNetwork.Nodes.Set_level_for_all_nodes(Global_class.Network_genes_level);
            combined_scpNetwork.Nodes.Set_processLevel_for_all_nodes_based_on_dictionary(legend_node_level_dict);
            combined_scpNetwork.Nodes.Set_processLevel_for_all_nodes_based_on_dictionary(processName_processLevel_dict);
            combined_scpNetwork.Keep_only_nodes_with_indicated_levels(new int[] { -1, 1, 2, 3, 4, Global_class.Network_genes_level, Global_class.Network_legend_level });

            bool group_scps_by_level = Options.Box_sameLevel_scps(enrichmentType);

            Yed_network_node_size_determinant_enum node_size_determinant = Options.Node_size_determinant;
            bool nw_generation_interrupted = combined_scpNetwork.Write_yED_nw_in_results_directory_with_nodes_colored_by_set_and_return_if_interrupted(comleteFileName, Shape_enum.Ellipse, scpGeneLegend_colors_dict, group_scps_by_level, node_size_determinant, error_report_label, form_default_settings);
            return nw_generation_interrupted;
        }
        #endregion
    }
}
