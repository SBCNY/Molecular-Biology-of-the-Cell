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
using Windows_forms;
using System.Windows.Controls;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Data;
using System.IO.Ports;
using ZedGraph;

namespace Enrichment
{
    enum GO_hyperParameter_enum { E_m_p_t_y, Max_size, Min_size }//, Max_level, Min_level, Max_depth, Min_depth}
    enum Predicted_scp_hierarchy_integration_strategy_enum {  E_m_p_t_y, All_ancestors, Intermediate_nodes, First_shared_parent }
    enum SCP_hierarchy_interaction_type_enum {  E_m_p_t_y, Parent_child, Parent_child_regulatory }


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
        private const int Const_go_max_maxSize = 9999;
        private const int Const_go_min_maxSize = -1;
        private const int Const_go_max_minSize = 100;
        private const int Const_go_min_minSize = -1;
        #endregion


        #region Fields for general Options
        public Ontology_type_enum Ontology { get; private set; }
        public Ontology_type_enum Next_ontology { get; set; }
        public Organism_enum Organism { get; private set; }
        public Organism_enum Next_organism { get; set; }
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
                return Ontology_keep_top_predictions_standardEnrichment_per_level[Next_ontology];
            }
            set
            {
                int length_value = value.Length;
                for (int indexValue = 0; indexValue < length_value; indexValue++)
                {
                    if (value[indexValue] >= 0)
                    {
                        Ontology_keep_top_predictions_standardEnrichment_per_level[Next_ontology][indexValue] = Math.Max(Const_min_keep_top_predictions, value[indexValue]);
                    }
                }
            }
        }
        public float Max_pvalue_for_standardEnrichment
        {
            get
            {
                return Ontology_max_pvalue_for_standardEnrichment[Next_ontology];
            }
            set
            {
                Ontology_max_pvalue_for_standardEnrichment[Next_ontology] = Math.Min(Const_max_maxPvalue,Math.Max(Const_min_maxPvalue, value));
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
                return Ontology_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[Next_ontology];
            }
            set
            {
                int length_value = value.Length;
                for (int indexValue = 0; indexValue < length_value; indexValue++)
                {
                    switch (indexValue)
                    {
                        case 0:
                        case 1:
                        case 4:
                            Ontology_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[Next_ontology][indexValue] = -1;
                            break;
                        case 2:
                        case 3:
                            Ontology_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[Next_ontology][indexValue] = Math.Max(Const_min_top_quantile_of_scp_interactions, Math.Min(Const_max_top_quantile_of_scp_interactions, value[indexValue]));
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
                return Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[Next_ontology];
            }
            set
            {
                int length_value = value.Length;
                for (int indexValue = 0; indexValue < length_value; indexValue++)
                {
                    switch (indexValue)
                    {
                        case 0:
                        case 1:
                        case 4:
                            Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[Next_ontology][indexValue] = -1;
                            break;
                        case 2:
                        case 3:
                            Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[Next_ontology][indexValue] = Math.Max(Const_min_top_quantile_of_scp_interactions, Math.Min(Const_max_top_quantile_of_scp_interactions, value[indexValue]));
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
                return Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level[Next_ontology];
            }
            set
            {
                int values_length = value.Length;
                for (int indexV=0; indexV< values_length;indexV++)
                {
                    switch (indexV)
                    {
                        case 0:
                        case 1:
                        case 4:
                            Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level[Next_ontology][indexV] = new int[0];
                            break;
                        case 2:
                        case 3:
                            Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level[Next_ontology][indexV] = Array_class.Deep_copy_array(value[indexV]);
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
                return Ontology_keep_top_predictions_dynamicEnrichment_per_level[Next_ontology];
            }
            set
            {
                int length_value = value.Length;
                for (int indexValue = 0; indexValue < length_value; indexValue++)
                {
                    switch (indexValue)
                    {
                        case 0:
                        case 1:
                        case 4:
                            Ontology_keep_top_predictions_dynamicEnrichment_per_level[Next_ontology][indexValue] = -1;
                            break;
                        case 2:
                        case 3:
                            Ontology_keep_top_predictions_dynamicEnrichment_per_level[Next_ontology][indexValue] = Math.Max(Const_min_keep_top_predictions, value[indexValue]);
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
                return Ontology_max_pvalue_for_dynamicEnrichment[Next_ontology];
            }
            set
            {
                Ontology_max_pvalue_for_dynamicEnrichment[Next_ontology] = Math.Max(Const_min_maxPvalue, Math.Min(Const_max_maxPvalue, value));
            }
        }
        #endregion

        #region Fields for timeline
        private Dictionary<Ontology_type_enum,float> Ontology_pvalue_cutoff_for_timeline { get; set; }
        public float Timeline_pvalue_cutoff
        {
            get 
            {
                return Ontology_pvalue_cutoff_for_timeline[Next_ontology]; 
            }
            set
            {
                Ontology_pvalue_cutoff_for_timeline[Next_ontology] = Math.Max(Const_min_maxPvalue, Math.Min(Const_max_maxPvalue, value));
            }
        }
        #endregion

        #region Fields for selected SCPs
        private Dictionary<Ontology_type_enum, Dictionary<string, string[]>> Ontology_group_selectedScps_dict { get; set; }
        public Dictionary<string, string[]> Group_selectedScps_dict
        {
            get
            {
                Dictionary<string, string[]> group_selected_Scps_dict = new Dictionary<string, string[]>();
                if (Ontology_group_selectedScps_dict.ContainsKey(Next_ontology))
                {
                    group_selected_Scps_dict = Ontology_group_selectedScps_dict[Next_ontology];
                }
                return group_selected_Scps_dict;
            }
            set
            {
                if (!Ontology_group_selectedScps_dict.ContainsKey(Next_ontology))
                {
                    Ontology_group_selectedScps_dict.Add(Next_ontology, new Dictionary<string, string[]>());
                }
                Ontology_group_selectedScps_dict[Next_ontology] = value;
            }
        }
        public void Group_selectedScps_dict_clear()
        {
            if (Ontology_group_selectedScps_dict.ContainsKey(Next_ontology))
            {
                Ontology_group_selectedScps_dict[Next_ontology].Clear();
            }
        }
        public void Group_selectedScps_dict_add(string dict_key, string[] dict_values)
        {
            if (!Ontology_group_selectedScps_dict.ContainsKey(Next_ontology))
            {
                Ontology_group_selectedScps_dict.Add(Next_ontology, new Dictionary<string, string[]>());
            }
            Ontology_group_selectedScps_dict[Next_ontology].Add(dict_key, dict_values);
        }
        private Dictionary<Ontology_type_enum, bool> Ontology_showAllAndOnlySelectedScps_dict { get; set; }
        public bool Show_all_and_only_selected_scps
        {
            get 
            {
                return Ontology_showAllAndOnlySelectedScps_dict[Next_ontology];
            }
            set
            {
                Ontology_showAllAndOnlySelectedScps_dict[Next_ontology] = value; 
            }
        }
        #endregion

        #region Fields for add own SCPs
        public Dictionary<Ontology_type_enum, Dictionary<string, string[]>> Ontology_ownScp_mbcoSubScps_dict { get; set; }
        public Dictionary<Ontology_type_enum, Dictionary<string, int>> Ontology_ownScp_level_dict { get; set; }
        public Dictionary<string, string[]> OwnScp_mbcoSubScps_dict
        {
            get
            {
                Dictionary<string, string[]> ownScp_mbcoSubScps_dict = new Dictionary<string, string[]>();
                if (Ontology_ownScp_mbcoSubScps_dict.ContainsKey(Next_ontology))
                {
                    ownScp_mbcoSubScps_dict = Ontology_ownScp_mbcoSubScps_dict[Next_ontology];
                }
                return ownScp_mbcoSubScps_dict;
            }
            set
            {
                if (!Ontology_ownScp_mbcoSubScps_dict.ContainsKey(Next_ontology))
                {
                    Ontology_ownScp_mbcoSubScps_dict.Add(Next_ontology, new Dictionary<string, string[]>());
                }
                Ontology_ownScp_mbcoSubScps_dict[Next_ontology] = value;
            }
        }
        public void OwnScp_mbcoSubScps_dict_add(string dict_key, string[] dict_values)
        {
            if (!Ontology_ownScp_mbcoSubScps_dict.ContainsKey(Next_ontology))
            {
                Ontology_ownScp_mbcoSubScps_dict.Add(Next_ontology, new Dictionary<string, string[]>());
            }
            Ontology_ownScp_mbcoSubScps_dict[Next_ontology].Add(dict_key, dict_values);
        }
        public void OwnScp_mbcoSubScps_dict_clear()
        {
            if (Ontology_ownScp_mbcoSubScps_dict.ContainsKey(Next_ontology))
            {
                Ontology_ownScp_mbcoSubScps_dict[Next_ontology].Clear();
            }
        }
        public void OwnScp_mbcoSubScps_dict_of_given_ownSCP_clear(string own_scp)
        {
            if (  (Ontology_ownScp_mbcoSubScps_dict.ContainsKey(Next_ontology))
                && (Ontology_ownScp_mbcoSubScps_dict[Next_ontology].ContainsKey(own_scp)))
            {
                Ontology_ownScp_mbcoSubScps_dict[Next_ontology][own_scp] = new string[0];
            }
        }
        public Dictionary<string, int> OwnScp_level_dict
        {
            get
            {
                Dictionary<string, int> ownScp_level_dict = new Dictionary<string, int>();
                if (Ontology_ownScp_level_dict.ContainsKey(Next_ontology))
                {
                    ownScp_level_dict = Ontology_ownScp_level_dict[Next_ontology];
                }
                return ownScp_level_dict;
            }
            set
            {
                if (!Ontology_ownScp_level_dict.ContainsKey(Next_ontology))
                {
                    Ontology_ownScp_level_dict.Add(Next_ontology, new Dictionary<string, int>());
                }
                Ontology_ownScp_level_dict[Next_ontology] = value;
            }
        }
        public void OwnScp_level_dict_clear()
        {
            if (Ontology_ownScp_level_dict.ContainsKey(Next_ontology))
            {
                Ontology_ownScp_level_dict[Next_ontology].Clear();
            }
        }
        private Dictionary<Organism_enum, string> OrganismEnum_organismString_dict { get; set; }
        public void OwnScp_level_dict_add(string dict_key, int dict_value)
        {
            if (!Ontology_ownScp_level_dict.ContainsKey(Next_ontology))
            {
                Ontology_ownScp_level_dict.Add(Next_ontology, new Dictionary<string, int>());
            }
            Ontology_ownScp_level_dict[Next_ontology].Add(dict_key,dict_value);
        }
        #endregion

        #region Fields for GO
        public Dictionary<Ontology_type_enum, Dictionary<GO_hyperParameter_enum, int>> GoOntology_hyperParameter_cutoff_dict { get; set; }
        public Dictionary<Ontology_type_enum, Dictionary<GO_hyperParameter_enum, int>> Next_GoOntology_hyperParameter_cutoff_dict { get; set; }
        #endregion

        public void Set_next_ontology_and_organism(Ontology_type_enum next_ontology, Organism_enum next_organism)
        {
            this.Next_ontology = next_ontology;
            this.Next_organism = next_organism;
        }

        public void Set_ontology_to_next_ontology_organism_to_next_organism_and_current_goHyperparameter_to_next_goHyperparameter_to_indicate_that_datasets_were_updated()
        {
            this.Ontology = this.Next_ontology;
            this.Organism = this.Next_organism;
            if (Ontology_classification_class.Is_go_ontology(this.Ontology))
            {
                if (!GoOntology_hyperParameter_cutoff_dict.ContainsKey(this.Ontology))
                {
                    GoOntology_hyperParameter_cutoff_dict.Add(this.Ontology, new Dictionary<GO_hyperParameter_enum, int>());
                }

                GO_hyperParameter_enum[] hyperparameters = Next_GoOntology_hyperParameter_cutoff_dict[this.Ontology].Keys.ToArray();
                foreach (GO_hyperParameter_enum hyperparameter in hyperparameters)
                {
                    if (!GoOntology_hyperParameter_cutoff_dict[this.Ontology].ContainsKey(hyperparameter))
                    {
                        GoOntology_hyperParameter_cutoff_dict[this.Ontology].Add(hyperparameter, Next_GoOntology_hyperParameter_cutoff_dict[this.Ontology][hyperparameter]);
                    }
                    else
                    {
                        GoOntology_hyperParameter_cutoff_dict[this.Ontology][hyperparameter] = Next_GoOntology_hyperParameter_cutoff_dict[this.Ontology][hyperparameter];
                    }
                }
            }

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

        private bool Is_Next_GoOntology_hyperParameter_cutoff_dict_equal_GoOntology_hyperParameter_cutoff_dict()
        {
            bool is_equal = true;
            Dictionary<GO_hyperParameter_enum, int> next_hyperparameter_cutoff_dict = Next_GoOntology_hyperParameter_cutoff_dict[this.Next_ontology];
            if (!GoOntology_hyperParameter_cutoff_dict.ContainsKey(this.Next_ontology))
            {
                GoOntology_hyperParameter_cutoff_dict.Add(this.Next_ontology, new Dictionary<GO_hyperParameter_enum, int>());
            }
            Dictionary<GO_hyperParameter_enum, int> current_hyperparameter_cutoff_dict = GoOntology_hyperParameter_cutoff_dict[this.Next_ontology];
            GO_hyperParameter_enum[] hyperparameters = next_hyperparameter_cutoff_dict.Keys.ToArray();
            foreach (GO_hyperParameter_enum hyperparameter in hyperparameters)
            {
                if (!current_hyperparameter_cutoff_dict.ContainsKey(hyperparameter))
                {
                    current_hyperparameter_cutoff_dict.Add(hyperparameter, next_hyperparameter_cutoff_dict[hyperparameter]);
                    is_equal = false;
                }
                else if (!current_hyperparameter_cutoff_dict[hyperparameter].Equals(next_hyperparameter_cutoff_dict[hyperparameter]))
                {
                    is_equal = false;
                }
            }
            GoOntology_hyperParameter_cutoff_dict[this.Next_ontology] = current_hyperparameter_cutoff_dict;
            return is_equal;
        }

        public bool Is_necessary_to_update_pipeline_for_all_runs_and_out_give_update_network_integration(out bool update_network_integration)
        {
            bool update_pipeline = false;
            update_network_integration = false;
            int selected_top_quantiles_length = this.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level.Length;
            int real_top_quantiles_length = this.Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level.Length;
            if (  (!Next_ontology.Equals(Ontology))
                ||(!Next_organism.Equals(Organism)))
            { 
                update_pipeline = true;
                update_network_integration = true;
            }
            else if (selected_top_quantiles_length != real_top_quantiles_length)
            { 
                update_pipeline = true;
                update_network_integration = true;
            }
            else if (  (Ontology_classification_class.Is_go_ontology(Next_ontology))
                     &&(!Is_Next_GoOntology_hyperParameter_cutoff_dict_equal_GoOntology_hyperParameter_cutoff_dict()))
            { 
                update_pipeline = true;
                update_network_integration = true;
            }
            else if (Ontology_classification_class.Is_mbco_ontology(Next_ontology))
            {
                for (int indexQ = 0; indexQ < selected_top_quantiles_length; indexQ++)
                {
                    if (Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[indexQ] != Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[indexQ])
                    { update_pipeline = true; }
                }
            }
            return update_pipeline;
        }
        public void Set_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level_to_selected_to_indicate_leaveOutNetworkUpdate()
        {
            this.Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level = Array_class.Deep_copy_array(this.Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level);
        }
        public void Set_go_min_size(Ontology_type_enum go_ontology, int min_size)
        {
            if (  (min_size==-1)
                || (Next_GoOntology_hyperParameter_cutoff_dict[go_ontology][GO_hyperParameter_enum.Max_size] ==-1)
                || (Next_GoOntology_hyperParameter_cutoff_dict[go_ontology][GO_hyperParameter_enum.Max_size] >= min_size))
            { Next_GoOntology_hyperParameter_cutoff_dict[go_ontology][GO_hyperParameter_enum.Min_size] = Math.Min(Const_go_max_minSize, Math.Max(Const_go_min_minSize, min_size)); }
        }
        public int Get_go_min_size(Ontology_type_enum go_ontology)
        {
            return Next_GoOntology_hyperParameter_cutoff_dict[go_ontology][GO_hyperParameter_enum.Min_size];
        }
        public string Get_go_min_size_string(Ontology_type_enum go_ontology)
        {
            int min_size = Next_GoOntology_hyperParameter_cutoff_dict[go_ontology][GO_hyperParameter_enum.Min_size];
            if (min_size==-1) { return ""; }
            else { return min_size.ToString(); }
        }
        public void Set_go_max_size(Ontology_type_enum go_ontology, int max_size)
        {
            if (  (max_size==-1)
                || (Next_GoOntology_hyperParameter_cutoff_dict[go_ontology][GO_hyperParameter_enum.Min_size] == -1)
                || (Next_GoOntology_hyperParameter_cutoff_dict[go_ontology][GO_hyperParameter_enum.Min_size] <= max_size))
            { Next_GoOntology_hyperParameter_cutoff_dict[go_ontology][GO_hyperParameter_enum.Max_size] = Math.Min(Const_go_max_maxSize, Math.Max(Const_go_min_maxSize, max_size)); }
        }
        public int Get_go_max_size(Ontology_type_enum go_ontology)
        {
            return Next_GoOntology_hyperParameter_cutoff_dict[go_ontology][GO_hyperParameter_enum.Max_size];
        }
        public string Get_go_max_size_string(Ontology_type_enum go_ontology)
        {
            int max_size = Next_GoOntology_hyperParameter_cutoff_dict[go_ontology][GO_hyperParameter_enum.Max_size];
            if (max_size == -1) { return ""; }
            else { return max_size.ToString(); }
        }

        public MBCO_enrichment_pipeline_options_class(Ontology_type_enum ontology, Organism_enum organism)
        {
            OrganismEnum_organismString_dict = Ontology_classification_class.Get_organismEnum_organismString_dict();
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();

            #region Parameter for general options
            this.Ontology = Ontology_type_enum.E_m_p_t_y;
            this.Set_next_ontology_and_organism(ontology, organism);
            this.Organism = Organism_enum.E_m_p_t_y;
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

            #region Fields for selected SCPs
            Ontology_group_selectedScps_dict = new Dictionary<Ontology_type_enum, Dictionary<string, string[]>>();
            Ontology_showAllAndOnlySelectedScps_dict = new Dictionary<Ontology_type_enum, bool>();
            #endregion

            Set_default_parameters_for_mbco();
            Set_default_parameters_specialized_mbco_ontologies();
            Set_default_parameters_other_ontologies();

            #region Fields for add own SCPs
            Ontology_ownScp_mbcoSubScps_dict = new Dictionary<Ontology_type_enum, Dictionary<string, string[]>>();
            Ontology_ownScp_level_dict = new Dictionary<Ontology_type_enum, Dictionary<string, int>>();
            #endregion

            #region Fields for Gene Ontology
            Next_GoOntology_hyperParameter_cutoff_dict = new Dictionary<Ontology_type_enum, Dictionary<GO_hyperParameter_enum, int>>();
            GoOntology_hyperParameter_cutoff_dict = new Dictionary<Ontology_type_enum, Dictionary<GO_hyperParameter_enum, int>>();
            Ontology_type_enum[] go_ontologies = new Ontology_type_enum[] { Ontology_type_enum.Go_bp, Ontology_type_enum.Go_cc, Ontology_type_enum.Go_mf};
            foreach (Ontology_type_enum go_ontology in go_ontologies)
            {
                Next_GoOntology_hyperParameter_cutoff_dict.Add(go_ontology, new Dictionary<GO_hyperParameter_enum, int>());
                Next_GoOntology_hyperParameter_cutoff_dict[go_ontology].Add(GO_hyperParameter_enum.Max_size, 350);
                Next_GoOntology_hyperParameter_cutoff_dict[go_ontology].Add(GO_hyperParameter_enum.Min_size, 5);
                //Next_GoOntology_hyperParameter_cutoff_dict[go_ontology].Add(GO_hyperParameter_enum.Max_level, -1);
                //Next_GoOntology_hyperParameter_cutoff_dict[go_ontology].Add(GO_hyperParameter_enum.Min_level, -1);
                //Next_GoOntology_hyperParameter_cutoff_dict[go_ontology].Add(GO_hyperParameter_enum.Max_depth, -1);
                //Next_GoOntology_hyperParameter_cutoff_dict[go_ontology].Add(GO_hyperParameter_enum.Min_depth, -1);
            }
            #endregion
        }
        private void Set_default_parameters_for_mbco()
        {
            Ontology_type_enum human_ontology = Ontology_type_enum.Mbco;

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

            #region Fields for select SCPs
            Ontology_showAllAndOnlySelectedScps_dict.Add(human_ontology, false);
            #endregion
        }
        private void Set_default_parameters_other_ontologies()
        {
            Ontology_type_enum[] other_ontoogies = new Ontology_type_enum[] { Ontology_type_enum.Go_bp, Ontology_type_enum.Go_mf, Ontology_type_enum.Go_cc, Ontology_type_enum.Reactome, Ontology_type_enum.Custom_1, Ontology_type_enum.Custom_2 };
            foreach (Ontology_type_enum other_ontology in other_ontoogies)
            {
                #region Fields for standard enrichment analysis
                Ontology_keep_top_predictions_standardEnrichment_per_level.Add(other_ontology, new int[5]);
                Ontology_keep_top_predictions_standardEnrichment_per_level[other_ontology] = new int[] { -1, 25, -1, -1, -1 };
                Ontology_max_pvalue_for_standardEnrichment.Add(other_ontology, -1);
                Ontology_max_pvalue_for_standardEnrichment[other_ontology] = 0.05F;
                #endregion

                #region Fields for dynamic enrichment analysis
                Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level.Add(other_ontology, new float[5]);
                Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[other_ontology] = new float[] { -1, -1, Const_min_top_quantile_of_scp_interactions, Const_min_top_quantile_of_scp_interactions, -1 };
                Ontology_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level.Add(other_ontology, new float[5]);
                Ontology_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level[other_ontology] = new float[] { -1, -1, Const_min_top_quantile_of_scp_interactions, Const_min_top_quantile_of_scp_interactions, -1 };
                Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level.Add(other_ontology, new int[5][]);
                Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level[other_ontology][2] = new int[] { -1, -1 };
                Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level[other_ontology][3] = new int[] { -1, -1 };
                Ontology_keep_top_predictions_dynamicEnrichment_per_level.Add(other_ontology, new int[0]);
                Ontology_keep_top_predictions_dynamicEnrichment_per_level[other_ontology] = new int[] { -1, -1, -1, -1, -1 };
                Ontology_max_pvalue_for_dynamicEnrichment.Add(other_ontology, -1);
                Ontology_max_pvalue_for_dynamicEnrichment[other_ontology] = -1F;
                #endregion

                #region Fields for timeline
                switch (other_ontology)
                {
                    case Ontology_type_enum.Go_bp:
                    case Ontology_type_enum.Go_cc:
                    case Ontology_type_enum.Go_mf:
                        Ontology_pvalue_cutoff_for_timeline.Add(other_ontology, 0.001F);
                        break;
                    case Ontology_type_enum.Reactome:
                    case Ontology_type_enum.Custom_1:
                    case Ontology_type_enum.Custom_2:
                        Ontology_pvalue_cutoff_for_timeline.Add(other_ontology, 0.05F);
                        break;
                    default:
                        throw new Exception();
                }
                #endregion

                #region Fields for select SCPs
                Ontology_showAllAndOnlySelectedScps_dict.Add(other_ontology, false);
                #endregion
            }
        }
        private void Set_default_parameters_specialized_mbco_ontologies()
        {
            Ontology_type_enum[] go_ontoogies = new Ontology_type_enum[] { Ontology_type_enum.Mbco_na_glucose_tm_transport};
            foreach (Ontology_type_enum go_ontology in go_ontoogies)
            {
                Ontology_type_enum human_ontology = go_ontology;

                #region Fields for standard enrichment analysis
                Ontology_keep_top_predictions_standardEnrichment_per_level.Add(human_ontology, new int[5]);
                Ontology_keep_top_predictions_standardEnrichment_per_level[human_ontology] = new int[] { -1, 999, -1, -1, -1 };
                Ontology_max_pvalue_for_standardEnrichment.Add(human_ontology, -1);
                Ontology_max_pvalue_for_standardEnrichment[human_ontology] = 1F;
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
                else if (go_ontology.Equals(Ontology_type_enum.Mbco_na_glucose_tm_transport))
                { Ontology_pvalue_cutoff_for_timeline.Add(human_ontology, 0.05F); }
                else { throw new Exception(); }
                #endregion

                #region Fields for select SCPs
                Ontology_showAllAndOnlySelectedScps_dict.Add(human_ontology, false);
                #endregion

            }
        }
        public override void Write_option_entries(System.IO.StreamWriter writer)
        {
            base.Write_entries_excluding_dictionaries(writer, typeof(MBCO_enrichment_pipeline_options_class), "Max_columns_per_analysis", "Write_results", "Max_columns_per_analysis", "Max_pvalue_for_standardEnrichment", "Max_pvalue_for_dynamicEnrichment", "Timeline_pvalue_cutoff", "Show_all_and_only_selected_scps");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_keep_top_predictions_standardEnrichment_per_level, "Keep_top_standard_SCPs");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_max_pvalue_for_standardEnrichment, "Max_pvalue_standard_enrichment");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_keep_top_predictions_dynamicEnrichment_per_level, "Keep_top_dynamic_SCPs");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_max_pvalue_for_dynamicEnrichment, "Max_pvalue_dynamic_enrichment");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Next_GoOntology_hyperParameter_cutoff_dict, "Next_GO_hyperparameter_cutoff");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_selected_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level, "Selected_top_quantile_of_scp_interactions_for_dynamicEnrichment");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_pvalue_cutoff_for_timeline, "Pvalue_cutoff_for_timeline");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_group_selectedScps_dict, "User_selected_scps");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_showAllAndOnlySelectedScps_dict, "Show_all_and_only_selected_scps");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_ownScp_mbcoSubScps_dict, "User_defined_scps");
            base.Write_dictionary_entries(writer, typeof(MBCO_enrichment_pipeline_options_class), Ontology_ownScp_level_dict, "User_defined_scpLevels");
        }

        public void Clear_all_dictionaries_with_selected_scps_for_next_ontology()
        {
            this.Group_selectedScps_dict = new Dictionary<string, string[]>();
            this.OwnScp_level_dict = new Dictionary<string, int>();
            this.OwnScp_mbcoSubScps_dict = new Dictionary<string, string[]>();
        }
        public void Clear_all_deNovo_dictionaries()
        {
            this.Ontology_ownScp_level_dict.Clear();
            this.Ontology_ownScp_mbcoSubScps_dict.Clear();
            this.Ontology_group_selectedScps_dict.Clear();
        }
        public override bool Add_read_entry_to_options_and_return_if_successful(string readLine)
        {
            string[] splitStrings = readLine.Split(Global_class.Tab);
            List<string> missing_entries = new List<string>();
            bool successful = true;
            switch (splitStrings[1])
            {
                case "Next_ontology":
                    Next_ontology = (Ontology_type_enum)Enum.Parse(typeof(Ontology_type_enum), splitStrings[2]);
                    break;
                case "Next_organism":
                    Next_organism = (Organism_enum)Enum.Parse(typeof(Organism_enum), splitStrings[2]);
                    break;
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
                case "Next_GO_hyperparameter_cutoff":
                    Next_GoOntology_hyperParameter_cutoff_dict = base.Add_to_dictionary_entries(readLine, Next_GoOntology_hyperParameter_cutoff_dict);
                    break;
                case "User_selected_scps":
                    Ontology_group_selectedScps_dict = base.Add_to_dictionary_entries(readLine, Ontology_group_selectedScps_dict);
                    break;
                case "Show_all_and_only_selected_scps":
                    Ontology_showAllAndOnlySelectedScps_dict = base.Add_to_dictionary_entries(readLine, Ontology_showAllAndOnlySelectedScps_dict);
                    break;
                case "User_defined_scps":
                    Ontology_ownScp_mbcoSubScps_dict = base.Add_to_dictionary_entries(readLine, Ontology_ownScp_mbcoSubScps_dict);
                    break;
                case "User_defined_scpLevels":
                    Ontology_ownScp_level_dict = base.Add_to_dictionary_entries(readLine, Ontology_ownScp_level_dict);
                    break;
                default:
                    missing_entries.Add(splitStrings[1]);
                    break;
                    //base.Add_read_entry(readLine, typeof(MBCO_enrichment_pipeline_options_class));
            }
            if ((Global_class.Do_internal_checks)&&(missing_entries.Count>0)) { throw new Exception(); }
            if (missing_entries.Count>0) { successful = false; }
            return successful;
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
            copy.Ontology_ownScp_mbcoSubScps_dict = Array_class.Deep_copy_nested_dictionary_with_final_stringKeys_stringValueArrays(this.Ontology_ownScp_mbcoSubScps_dict);
            copy.Ontology_group_selectedScps_dict = Array_class.Deep_copy_nested_dictionary_with_final_stringKeys_stringValueArrays(this.Ontology_group_selectedScps_dict);
            copy.Ontology_ownScp_level_dict = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_ownScp_level_dict);
            copy.Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_numbers_of_merged_scps_for_dynamicEnrichment_per_level);
            int numbers_of_merged_scps_length = this.Numbers_of_merged_scps_for_dynamicEnrichment_per_level.Length;
            copy.Numbers_of_merged_scps_for_dynamicEnrichment_per_level = new int[numbers_of_merged_scps_length][];
            copy.Ontology_pvalue_cutoff_for_timeline = Array_class.Deep_copy_dictionary(this.Ontology_pvalue_cutoff_for_timeline);
            copy.Ontology_group_selectedScps_dict = Array_class.Deep_copy_nested_dictionary_with_final_stringKeys_stringValueArrays(this.Ontology_group_selectedScps_dict);
            copy.Ontology_showAllAndOnlySelectedScps_dict = Array_class.Deep_copy_dictionary(this.Ontology_showAllAndOnlySelectedScps_dict);
            copy.Ontology_ownScp_mbcoSubScps_dict = Array_class.Deep_copy_nested_dictionary_with_final_stringKeys_stringValueArrays(this.Ontology_ownScp_mbcoSubScps_dict);
            copy.OrganismEnum_organismString_dict = Array_class.Deep_copy_dictionary(this.OrganismEnum_organismString_dict);
            copy.GoOntology_hyperParameter_cutoff_dict = Array_class.Deep_copy_nested_dictionary_with_final_value(this.GoOntology_hyperParameter_cutoff_dict);
            copy.Next_GoOntology_hyperParameter_cutoff_dict = Array_class.Deep_copy_nested_dictionary_with_final_value(this.Next_GoOntology_hyperParameter_cutoff_dict);



            Organism_enum[] organisms = OrganismEnum_organismString_dict.Keys.ToArray();
            Organism_enum organism;
            int organisms_length = organisms.Length;
            copy.OrganismEnum_organismString_dict = new Dictionary<Organism_enum, string>();
            for (int indexO=0; indexO<organisms_length;indexO++)
            {
                organism = organisms[indexO];
                copy.OrganismEnum_organismString_dict.Add(organism, (string)this.OrganismEnum_organismString_dict[organism].Clone());
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
        public Fisher_exact_test_class Fisher { get; set; }
        public string[] Exp_bg_genes { get; set; }
        public string[] Final_bg_genes { get; set; }
        public string Base_file_name { get; set; }
        #endregion

        public Mbc_enrichment_fast_pipeline_class(Ontology_type_enum ontology, Organism_enum organism)
        {
            Options_default = new MBCO_enrichment_pipeline_options_class(ontology, organism);
            Options = Options_default.Deep_copy();
        }

        #region Generate
        private void Generate_parent_child_and_child_parent_MBCO_networks(ProgressReport_interface_class progressReport)
        {
            Mbco_parentChild_nw = new MBCO_obo_network_class(this.Options.Ontology, SCP_hierarchy_interaction_type_enum.Parent_child, Options.Organism);
            Mbco_parentChild_nw.Generate_by_reading_safed_spreadsheet_file_or_obo_file_add_missing_scps_if_custom_add_human_processSizes_and_return_if_not_interrupted(progressReport, out bool not_interrupted);
        }

        private void Generate_mbco_association(ProgressReport_interface_class progressReport)
        {
            MBCO_association_unmodified = new MBCO_association_class();
            MBCO_association_unmodified.Generate_after_reading_safed_file_or_de_novo_and_save(this.Options.Ontology, this.Options.Organism,
                                                                                              this.Options.GoOntology_hyperParameter_cutoff_dict, progressReport);
        }

        private void Generate_leave_out_scp_network(ProgressReport_interface_class progressReport)
        {
            Dictionary<string, int> processName_processLevel_dict = Mbco_parentChild_nw.Get_processName_level_dictionary_without_setting_process_level();

            Leave_out_class leave_out = new Leave_out_class(Options.Ontology);
            leave_out.Generate_by_reading_safed_file(progressReport);
            Leave_out_scp_network_for_dynamicEnrichment_analysis = new Leave_out_scp_scp_network_class(Options.Ontology);
            this.Options.Set_top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level_to_selected_to_indicate_leaveOutNetworkUpdate();
            Leave_out_scp_network_for_dynamicEnrichment_analysis.Options.Top_quantile_of_considered_SCP_interactions_per_level = this.Options.Top_quantile_of_scp_interactions_for_dynamicEnrichment_per_level;
            Leave_out_scp_network_for_dynamicEnrichment_analysis.Generate_scp_scp_network_from_leave_out(leave_out, progressReport);
            Leave_out_scp_network_for_dynamicEnrichment_analysis.Scp_nw.Nodes.Set_processLevel_for_all_nodes_based_on_dictionary(processName_processLevel_dict);

            Leave_out_scp_network_all_scps = new Leave_out_scp_scp_network_class(this.Options.Ontology);
            Leave_out_scp_network_all_scps.Options.Top_quantile_of_considered_SCP_interactions_per_level = new float[] { 1F, 1F, 1F, 1F, 1F };
            Leave_out_scp_network_all_scps.Generate_scp_scp_network_from_leave_out(leave_out, progressReport);
            Leave_out_scp_network_all_scps.Scp_nw.Transform_into_undirected_single_network_and_set_all_widths_to_default();
            Leave_out_scp_network_all_scps.Scp_nw.Nodes.Set_processLevel_for_all_nodes_based_on_dictionary(processName_processLevel_dict);
        }

        public void Generate_for_all_runs_after_setting_ontology_to_next_ontology(ProgressReport_interface_class progressReport)
        {
            this.Options.Set_ontology_to_next_ontology_organism_to_next_organism_and_current_goHyperparameter_to_next_goHyperparameter_to_indicate_that_datasets_were_updated();
            Generate_parent_child_and_child_parent_MBCO_networks(progressReport);
            Generate_mbco_association(progressReport);
            Generate_leave_out_scp_network(progressReport);
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
                this.Final_bg_genes = Overlap_class.Get_ordered_intersection(this.Exp_bg_genes, all_mbco_genes);
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

        public bool Is_necessary_to_update_pipeline_for_all_runs_and_out_give_update_network_integration(out bool update_network_integration)
        {
            bool update_based_on_options = Options.Is_necessary_to_update_pipeline_for_all_runs_and_out_give_update_network_integration(out update_network_integration);
            bool update_because_no_array = (this.MBCO_association == null) || (this.MBCO_association.MBCO_associations.Length == 0);
            return update_based_on_options || update_because_no_array;
        }
        public bool Is_data_complete_for_analysis()
        {
            bool is_data_complete = true;
            if (this.MBCO_association_unmodified.MBCO_associations.Length == 0) { is_data_complete = false; }
            if (Ontology_classification_class.Is_mbco_ontology(this.Options.Ontology)&&(this.Leave_out_scp_network_for_dynamicEnrichment_analysis.Scp_nw.NW_length==0)) {  is_data_complete=false; }
            return is_data_complete;
        }
        public void Remove_all_existing_custom_scps_from_mbco_association_unmodified_and_add_new_ones()
        {
            Dictionary<string, string[]> customSCP_mbcoSubSCP_dict = this.Options.OwnScp_mbcoSubScps_dict;
            Dictionary<string, int> customSCP_level_dict = this.Options.OwnScp_level_dict;
            this.MBCO_association_unmodified.Remove_all_custom_SCPs_from_mbco_association_unmodified();
            this.MBCO_association_unmodified.Add_custom_scps_as_combination_of_selected_scps_to_mbco_association_unmodified(customSCP_mbcoSubSCP_dict, customSCP_level_dict);
        }

        public void Write_orthologue_identification_if_done(string results_directory)
        {
            Global_directory_and_file_class gdf = new Global_directory_and_file_class();
            bool write_orthologue_identification_text = false;
            switch (gdf.Ontology_species_selectionOrder_dict[Options.Ontology][Options.Organism])
            {
                case Species_selection_order_enum.Always_generate_orthologues:
                    write_orthologue_identification_text = true;
                    break;
                case Species_selection_order_enum.Insist_on_file:
                    write_orthologue_identification_text = false;
                    break;
                case Species_selection_order_enum.Generate_orthologues_if_missing:
                    string complete_input_fileName = gdf.Ontology_inputDirectory_dict[Options.Ontology] + gdf.Ontology_organism_geneAssociationInputFileName_dict[Options.Ontology][Options.Organism];
                    if (System.IO.File.Exists(complete_input_fileName))
                    {
                        write_orthologue_identification_text = false;
                    }
                    else
                    {
                        write_orthologue_identification_text = true;
                    }
                    break;
                default:
                    throw new Exception();
            }
            if (write_orthologue_identification_text)
            {
                string human_species = Ontology_classification_class.Get_organismString_for_enum(Organism_enum.Homo_sapiens);
                string selected_species = Ontology_classification_class.Get_organismString_for_enum(Options.Organism);
                string complete_fileName = results_directory + Ontology_classification_class.Get_name_of_ontology_plus_organism(Options.Ontology, Options.Organism) + "_ortholgue_mappings.txt";
                StreamWriter writer = new StreamWriter(complete_fileName);
                writer.WriteLine("Human genes were mapped to their {0} orthologues using NCBI orthologues", selected_species);
                writer.WriteLine("(file: '{0}', downloaded on ...) and, if existent, orthologues", gdf.Ncbi_orthologs_download_fileName);
                writer.WriteLine("annotated within the related Mouse Genome Informatics (MGI) database (file:,");
                writer.WriteLine("'{0}', downloaded on ...). If both databases contained", gdf.Mgi_orthologs_download_fileName);
                writer.WriteLine("orthologues for the same human gene, mappings of the MGI database");
                writer.WriteLine("were prioritized and NCBI mappings ignored. Official NCBI gene symbols");
                writer.WriteLine("extracted from the NCBI file '{0}' (downloaded on ...) were mapped", gdf.GeneInfo_download_fileName);
                writer.WriteLine("to human and {0} orthologues based on matching Gene IDs.", selected_species);
                writer.Close();
            }
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
            Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<string, bool>>>>> integrationGroup_entryType_timpointInDays_sampleName_scpName_dict = new Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<string, bool>>>>>();
            Ontology_enrichment_line_class onto_enrich_line;
            float timepointInDays;
            for (int indexO = 0; indexO < onto_enrich_length; indexO++)
            {
                onto_enrich_line = onto_enrich_lines[indexO];
                timepointInDays = onto_enrich_line.TimepointInDays;
                if (!integrationGroup_entryType_timpointInDays_sampleName_scpName_dict.ContainsKey(onto_enrich_line.IntegrationGroup))
                {
                    integrationGroup_entryType_timpointInDays_sampleName_scpName_dict.Add(onto_enrich_line.IntegrationGroup, new Dictionary<Entry_type_enum, Dictionary<float, Dictionary<string, Dictionary<string, bool>>>>());
                }
                if (!integrationGroup_entryType_timpointInDays_sampleName_scpName_dict[onto_enrich_line.IntegrationGroup].ContainsKey(onto_enrich_line.EntryType))
                {
                    integrationGroup_entryType_timpointInDays_sampleName_scpName_dict[onto_enrich_line.IntegrationGroup].Add(onto_enrich_line.EntryType, new Dictionary<float, Dictionary<string, Dictionary<string, bool>>>());
                }
                if (!integrationGroup_entryType_timpointInDays_sampleName_scpName_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType].ContainsKey(timepointInDays))
                {
                    integrationGroup_entryType_timpointInDays_sampleName_scpName_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType].Add(timepointInDays, new Dictionary<string, Dictionary<string, bool>>());
                }
                if (!integrationGroup_entryType_timpointInDays_sampleName_scpName_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType][timepointInDays].ContainsKey(onto_enrich_line.SampleName))
                {
                    integrationGroup_entryType_timpointInDays_sampleName_scpName_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType][timepointInDays].Add(onto_enrich_line.SampleName, new Dictionary<string, bool>());
                }
                integrationGroup_entryType_timpointInDays_sampleName_scpName_dict[onto_enrich_line.IntegrationGroup][onto_enrich_line.EntryType][timepointInDays][onto_enrich_line.SampleName].Add(onto_enrich_line.Scp_name, true);
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
            //data_lines = data_lines.OrderBy(l => l.NCBI_official_symbol.Length).ThenBy(l => l.NCBI_official_symbol).ToArray();
            data_lines = Data_line_class.Order_by_lengthOfNcbiOfficialSymbol_and_then_by_ncbiOfficialSymbol(data_lines);
            Data_line_class data_line;
            int data_length = data.Data.Length;

            Data_line_class col_entries_count_line = new Data_line_class("Column entries count", col_count);

            int indexMbco = 0;
            MBCO_association_line_class[] mbco_association_lines = MBCO_association.MBCO_associations;
            //mbco_association_lines = mbco_association_lines.OrderBy(l => l.Symbol.Length).ThenBy(l => l.Symbol).ToArray();
            mbco_association_lines = MBCO_association_line_class.Order_by_lengthOfSymbol_symbol(mbco_association_lines);
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
                        if (!processName_overlapSymbols_dict.ContainsKey(mbco_association_line.SCP_name))
                        {
                            processName_overlapSymbols_dict.Add(mbco_association_line.SCP_name, new List<string>[col_count]);
                            for (int indexCol = 0; indexCol < col_count; indexCol++)
                            {
                                processName_overlapSymbols_dict[mbco_association_line.SCP_name][indexCol] = new List<string>();
                            }
                        }
                        for (int indexCol = 0; indexCol < col_count; indexCol++)
                        {
                            if (data_line.Columns[indexCol] != data_empty_value)
                            {
                                processName_overlapSymbols_dict[mbco_association_line.SCP_name][indexCol].Add(data_line.NCBI_official_symbol);
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
                        new_enrichment_line.Organism = this.Options.Organism;
                        new_enrichment_line.Scp_name = (string)processName.Clone();
                        new_enrichment_line.IntegrationGroup = (string)column_line.IntegrationGroup.Clone();
                        new_enrichment_line.Unique_dataset_name = (string)column_line.UniqueDataset_name.Clone();
                        new_enrichment_line.SampleName = column_line.SampleName;
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
            //enrichment_lines = enrichment_lines.OrderBy(l => l.Scp_name).ToArray();
            enrichment_lines = Ontology_enrichment_line_class.Order_by_scpName(enrichment_lines);
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
                    stringCompare = mbco_association_line.SCP_name.CompareTo(enrich_line.Scp_name);
                    if (stringCompare < 0)
                    {
                        indexOnto++;
                        process_symbols_count = 1;
                    }
                    else if (stringCompare == 0)
                    {
                        while ((indexOnto < onto_length - 1) && (mbco_association_line.SCP_name.Equals(MBCO_association.MBCO_associations[indexOnto + 1].SCP_name)))
                        {
                            process_symbols_count++;
                            indexOnto++;
                        }
                        if (!string.IsNullOrEmpty(mbco_association_line.SCP_id)) { enrich_line.Scp_id = (string)mbco_association_line.SCP_id.Clone(); }
                        enrich_line.ProcessLevel = mbco_association_line.SCP_level;
                        enrich_line.ProcessDepth = mbco_association_line.SCP_depth;
                        if (!string.IsNullOrEmpty(mbco_association_line.Parent_scpName)) { enrich_line.Parent_scp_name = (string)mbco_association_line.Parent_scpName.Clone(); }
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
                if (scp_scpUnion_dict.ContainsKey(mbco_association_line.SCP_name))
                {
                    scp_unions_of_current_scp = scp_scpUnion_dict[mbco_association_line.SCP_name];
                    add_one_to_gene_count_of_scp_unions.AddRange(scp_unions_of_current_scp);
                    foreach (string scp_union_of_curren_scp in scp_unions_of_current_scp)
                    {
                        if (!scpUnion_level_dict.ContainsKey(scp_union_of_curren_scp))
                        {
                            scpUnion_level_dict.Add(scp_union_of_curren_scp, mbco_association_line.SCP_level);
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
                    enrichment_line.ProcessDepth = enrichment_line.ProcessLevel;
                    enrichment_line.Bg_symbol_count = background_symbols_count;
                    enrichment_line.Parent_scp_name = Ontology_classification_class.No_annotated_parent_scp;
                    enrichment_line.Scp_id = Ontology_classification_class.Dynamic_scp_combination;
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

        private void Calculate_minusLog10pvalues_and_set_to_max_number_if_infinity(ref Ontology_enrichment_line_class[] enrichment_lines)
        {
            int enrich_length = enrichment_lines.Length;
            Ontology_enrichment_line_class enrich_line;
            for (int indexE = 0; indexE < enrich_length; indexE++)
            {
                enrich_line = enrichment_lines[indexE];
                enrich_line.Minus_log10_pvalue = -(float)Math.Log10(enrich_line.Pvalue);
                if (float.IsInfinity(enrich_line.Minus_log10_pvalue)) { enrich_line.Minus_log10_pvalue = float.MaxValue; }
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

        public void Analyse_data_instance_fast(Data_class data_input, string base_file_name, string fileName_addition, out Ontology_enrichment_class standard_enrichment_unfiltered, out Ontology_enrichment_class dynamic_enrichment_unfiltered)
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

            Calculate_minusLog10pvalues_and_set_to_max_number_if_infinity(ref standard_enrichment_lines);
            Check_for_duplicated_enrichment_lines(standard_enrichment_lines);
            Calculate_minusLog10pvalues_and_set_to_max_number_if_infinity(ref dynamic_enrichment_lines);
            Check_for_duplicated_enrichment_lines(dynamic_enrichment_lines);
            standard_enrichment_unfiltered = new Ontology_enrichment_class();
            standard_enrichment_unfiltered.Add_other_lines_without_resetting_unique_datasetNames(standard_enrichment_lines);
            standard_enrichment_unfiltered.Calculate_fractional_ranks_for_SCPs_within_each_integrationGroup_sampleName_timepoint_timeunit_entryType_processLevel();
            dynamic_enrichment_unfiltered = new Ontology_enrichment_class();
            dynamic_enrichment_unfiltered.Add_other_lines_without_resetting_unique_datasetNames(dynamic_enrichment_lines);
            dynamic_enrichment_unfiltered.Calculate_fractional_ranks_for_SCPs_within_each_integrationGroup_sampleName_timepoint_timeunit_entryType_processLevel();
        }
        #endregion

        public string[] Get_all_symbols_of_any_SCPs_after_updating_instance_if_ontology_unequals_next_ontology(ProgressReport_interface_class progressReport)
        {
            if (!Options.Ontology.Equals(Options.Next_ontology))
            {
                Generate_for_all_runs_after_setting_ontology_to_next_ontology(progressReport);
            }
            return MBCO_association_unmodified.Get_all_distinct_ordered_symbols();
        }

        private string Get_results_subdirectory_for_indicated_ontology(Ontology_type_enum ontology)
        {
            return this.Base_file_name + "_" + ontology + "//";
        }

    }

    class MBCO_network_based_integration_options_class : Options_readWrite_base_class
    {
        #region Const min max values
        private const float const_min_top_quantile_probability_of_scp_interactions = MBCO_enrichment_pipeline_options_class.Const_min_top_quantile_of_scp_interactions;
        private const float const_max_top_quantile_probability_of_scp_interactions = MBCO_enrichment_pipeline_options_class.Const_max_top_quantile_of_scp_interactions;
        private const int const_max_node_diameter = 500;
        private const int const_min_node_diameter = 20;
        private const int const_max_label_size = 150;
        private const int const_min_label_size = 5;
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
        private Dictionary<Ontology_type_enum, bool> Ontology_box_sameLevel_scps_for_standard_enrichment { get; set; }
        private Dictionary<Ontology_type_enum, bool> Ontology_box_sameLevel_scps_for_dynamic_enrichment { get; set; }
        private Dictionary<Ontology_type_enum, Predicted_scp_hierarchy_integration_strategy_enum> Ontology_predictedScpHierarchyIntegrationStrategy_dict { get; set; }
        private Dictionary<Ontology_type_enum, SCP_hierarchy_interaction_type_enum> Next_ontology_scpHierarchyInteractionType_dict { get; set; }
        private Dictionary<Ontology_type_enum, SCP_hierarchy_interaction_type_enum> Ontology_scpHierarchyInteractionType_dict { get; set; }
        #endregion

        public Graph_editor_enum Graph_editor { get; set; }

        #region Node Sizes
        public Node_size_scaling_across_plots_enum Node_size_scaling_across_plots { get; set; }
        public Dictionary<Yed_network_node_size_determinant_enum, int> NodeSizeDeterminant_diameterMax_dict { get; set; }
        public Dictionary<Yed_network_node_size_determinant_enum, int> NodeSizeDeterminant_labelUniqueSize_dict { get; set; }
        public Dictionary<Yed_network_node_size_determinant_enum, int> NodeSizeDeterminant_labelMinSize_dict { get; set; }
        public Dictionary<Yed_network_node_size_determinant_enum, int> NodeSizeDeterminant_labelMaxSize_dict { get; set; }
        public bool Adjust_labelSizes_to_nodeSizes { get; set; }
        public int Node_size_diameterMax_for_current_nodeSize_determinant
        {
            get
            {
                return NodeSizeDeterminant_diameterMax_dict[Node_size_determinant];
            }
            set
            {
                if ((value <= const_max_node_diameter)
                    && (value >= const_min_node_diameter))
                {
                    NodeSizeDeterminant_diameterMax_dict[Node_size_determinant] = value;
                }
            }
        }
        public int Label_minSize_for_current_nodeSize_determinant
        {
            get
            {
                return NodeSizeDeterminant_labelMinSize_dict[Node_size_determinant];
            }
            set
            {
                if (   (value <= const_max_label_size)
                    && (value >= const_min_label_size)
                    && (value <= Label_maxSize_for_current_nodeSize_determinant))
                {
                    NodeSizeDeterminant_labelMinSize_dict[Node_size_determinant] = value;
                }
            }
        }
        public int Label_maxSize_for_current_nodeSize_determinant
        {
            get
            {
                return NodeSizeDeterminant_labelMaxSize_dict[Node_size_determinant];
            }
            set
            {
                if (   (value <= const_max_label_size)
                    && (value >= const_min_label_size)
                    && (value >= Label_minSize_for_current_nodeSize_determinant))
                {
                    NodeSizeDeterminant_labelMaxSize_dict[Node_size_determinant] = value;
                }
            }
        }
        public int Label_uniqueSize_for_current_nodeSize_determinant
        {
            get
            {
                return NodeSizeDeterminant_labelUniqueSize_dict[Node_size_determinant];
            }
            set
            {
                if ((value <= const_max_label_size)
                    && (value >= const_min_label_size))
                {
                    NodeSizeDeterminant_labelUniqueSize_dict[Node_size_determinant] = value;
                }
            }
        }
        #endregion

        public Ontology_type_enum _ontology;
        public Ontology_type_enum Next_ontology { get; set; }
        public Organism_enum Next_organism { get; set; }
        public Ontology_type_enum Ontology { get { return _ontology; } private set {  _ontology = value; } }
        public Organism_enum Organism { get; private set; }
        public bool Add_genes_to_standard_networks
        { 
            get
            {
                return Ontology_add_genes_to_standard_networks[Next_ontology];
            }
            set
            {
                Ontology_add_genes_to_standard_networks[Next_ontology] = value;
            }
        }
        public bool Add_genes_to_dynamic_networks
        { 
            get
            {
                return Ontology_add_genes_to_dynamic_networks[Next_ontology];
            }
            set
            {
                Ontology_add_genes_to_dynamic_networks[Next_ontology] = value;
            }
        }
        public bool Add_edges_that_connect_standard_scps
        {
            get
            {
                return Ontology_add_edges_that_connect_standard_scps[Next_ontology];
            }
            set
            {
                Ontology_add_edges_that_connect_standard_scps[Next_ontology] = value;
            }
        }
        public bool Add_additional_edges_that_connect_dynamic_scps
        {
            get
            {
                return Ontology_add_additional_edges_that_connect_dynamic_scps[Next_ontology];
            }
            set
            {
                Ontology_add_additional_edges_that_connect_dynamic_scps[Next_ontology] = value;
            }
        }
        public bool Add_parent_child_relationships_to_dynamic_SCP_networks
        {
            get
            {
                return Ontology_add_parent_child_relationships_to_dynamic_SCP_networks[Next_ontology];
            }
            set
            {
                Ontology_add_parent_child_relationships_to_dynamic_SCP_networks[Next_ontology] = value;
            }
        }
        public bool Add_parent_child_relationships_to_standard_SCP_networks
        {
            get
            {
                return Ontology_add_parent_child_relationships_to_standard_SCP_networks[Next_ontology];
            }
            set
            {
                Ontology_add_parent_child_relationships_to_standard_SCP_networks[Next_ontology] = value;
            }
        }
        public float[] Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level 
        { 
          get 
          {
                return Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level[Next_ontology];
          }
          set 
          {
                int values_length = value.Length;
                for (int indexV=0; indexV < values_length; indexV++)
                {
                    switch (indexV)
                    {
                        case 0:
                        case 1:
                        case 4:
                            Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level[Next_ontology][indexV] = -1;
                            break;
                        case 2:
                        case 3:
                            Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level[Next_ontology][indexV] = Math.Min(const_max_top_quantile_probability_of_scp_interactions,Math.Max(const_min_top_quantile_probability_of_scp_interactions, value[indexV]));
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
                return Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level[Next_ontology]; 
            }
            set
            {
                int values_length = value.Length;
                for (int indexV = 0; indexV < values_length; indexV++)
                {
                    switch (indexV)
                    {
                        case 0:
                        case 1:
                        case 4:
                            Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level[Next_ontology][indexV] = -1;
                            break;
                        case 2:
                        case 3:
                            Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level[Next_ontology][indexV] = Math.Min(const_max_top_quantile_probability_of_scp_interactions, Math.Max(const_min_top_quantile_probability_of_scp_interactions, value[indexV]));
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
        }
        public Predicted_scp_hierarchy_integration_strategy_enum Predicted_scp_hierarchy_integration_strategy
        {
            get
            {
                return Ontology_predictedScpHierarchyIntegrationStrategy_dict[Next_ontology];
            }
            set
            {
                Ontology_predictedScpHierarchyIntegrationStrategy_dict[Next_ontology] = value;
            }
        }
        public SCP_hierarchy_interaction_type_enum Next_scp_hierachical_interactions
        {
            get
            {
                return Next_ontology_scpHierarchyInteractionType_dict[Next_ontology];
            }
            set
            {
                Next_ontology_scpHierarchyInteractionType_dict[Next_ontology] = value;
            }
        }
        public SCP_hierarchy_interaction_type_enum Scp_hierachical_interactions
        {
            get
            {
                if (Ontology_scpHierarchyInteractionType_dict.ContainsKey(Next_ontology))
                {
                    return Ontology_scpHierarchyInteractionType_dict[Next_ontology];
                }
                else
                {
                    return SCP_hierarchy_interaction_type_enum.E_m_p_t_y;
                }
            }
            set
            {
                Ontology_scpHierarchyInteractionType_dict[Next_ontology] = value;
            }
        }
        public bool Generate_scp_networks { get; set; }
        public Yed_network_node_size_determinant_enum Node_size_determinant { get; set; }


        public bool Box_sameLevel_scps_for_standard_enrichment
        {
            get
            {
                return Ontology_box_sameLevel_scps_for_standard_enrichment[Next_ontology];
            }
            set
            {
                Ontology_box_sameLevel_scps_for_standard_enrichment[Next_ontology] = value;
            }
        }
        public bool Box_sameLevel_scps_for_dynamic_enrichment
        {
            get
            {
                return Ontology_box_sameLevel_scps_for_dynamic_enrichment[Next_ontology];
            }
            set
            {
                Ontology_box_sameLevel_scps_for_dynamic_enrichment[Next_ontology] = value;
            }
        }

        public MBCO_network_based_integration_options_class(Ontology_type_enum next_ontology, Organism_enum next_organism)
        {
            Graph_editor = Graph_editor_enum.yED;

            Next_ontology = next_ontology;
            Next_organism = next_organism;
            Ontology = Ontology_type_enum.E_m_p_t_y;
            Organism = Organism_enum.E_m_p_t_y;
            Generate_scp_networks = true;
            Node_size_determinant = Yed_network_node_size_determinant_enum.Minus_log10_pvalue;
            Node_size_scaling_across_plots = Node_size_scaling_across_plots_enum.Unique;
            Adjust_labelSizes_to_nodeSizes = true;
            NodeSizeDeterminant_diameterMax_dict = new Dictionary<Yed_network_node_size_determinant_enum, int>();
            NodeSizeDeterminant_diameterMax_dict.Add(Yed_network_node_size_determinant_enum.Uniform, 50);
            NodeSizeDeterminant_diameterMax_dict.Add(Yed_network_node_size_determinant_enum.No_of_sets, 150);
            NodeSizeDeterminant_diameterMax_dict.Add(Yed_network_node_size_determinant_enum.No_of_different_colors, 150);
            NodeSizeDeterminant_diameterMax_dict.Add(Yed_network_node_size_determinant_enum.Minus_log10_pvalue, 150);

            NodeSizeDeterminant_labelUniqueSize_dict = new Dictionary<Yed_network_node_size_determinant_enum, int>();
            NodeSizeDeterminant_labelUniqueSize_dict.Add(Yed_network_node_size_determinant_enum.Uniform, 50);
            NodeSizeDeterminant_labelUniqueSize_dict.Add(Yed_network_node_size_determinant_enum.No_of_sets, 50);
            NodeSizeDeterminant_labelUniqueSize_dict.Add(Yed_network_node_size_determinant_enum.No_of_different_colors, 50);
            NodeSizeDeterminant_labelUniqueSize_dict.Add(Yed_network_node_size_determinant_enum.Minus_log10_pvalue, 50);

            NodeSizeDeterminant_labelMinSize_dict = new Dictionary<Yed_network_node_size_determinant_enum, int>();
            NodeSizeDeterminant_labelMinSize_dict.Add(Yed_network_node_size_determinant_enum.Uniform, 50);
            NodeSizeDeterminant_labelMinSize_dict.Add(Yed_network_node_size_determinant_enum.No_of_sets, 50);
            NodeSizeDeterminant_labelMinSize_dict.Add(Yed_network_node_size_determinant_enum.No_of_different_colors, 50);
            NodeSizeDeterminant_labelMinSize_dict.Add(Yed_network_node_size_determinant_enum.Minus_log10_pvalue, 50);

            NodeSizeDeterminant_labelMaxSize_dict = new Dictionary<Yed_network_node_size_determinant_enum, int>();
            NodeSizeDeterminant_labelMaxSize_dict.Add(Yed_network_node_size_determinant_enum.Uniform, 70);
            NodeSizeDeterminant_labelMaxSize_dict.Add(Yed_network_node_size_determinant_enum.No_of_sets, 70);
            NodeSizeDeterminant_labelMaxSize_dict.Add(Yed_network_node_size_determinant_enum.No_of_different_colors, 70);
            NodeSizeDeterminant_labelMaxSize_dict.Add(Yed_network_node_size_determinant_enum.Minus_log10_pvalue, 70);



            #region Initialize dictionaries
            Ontology_add_genes_to_standard_networks = new Dictionary<Ontology_type_enum, bool>();
            Ontology_add_genes_to_dynamic_networks = new Dictionary<Ontology_type_enum, bool>();
            Ontology_add_parent_child_relationships_to_standard_SCP_networks = new Dictionary<Ontology_type_enum, bool>();
            Ontology_add_edges_that_connect_standard_scps = new Dictionary<Ontology_type_enum, bool>();
            Ontology_add_additional_edges_that_connect_dynamic_scps = new Dictionary<Ontology_type_enum, bool>();
            Ontology_add_parent_child_relationships_to_dynamic_SCP_networks = new Dictionary<Ontology_type_enum, bool>();
            Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level = new Dictionary<Ontology_type_enum, float[]>();
            Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level = new Dictionary<Ontology_type_enum, float[]>();
            Ontology_box_sameLevel_scps_for_dynamic_enrichment = new Dictionary<Ontology_type_enum, bool>();
            Ontology_box_sameLevel_scps_for_standard_enrichment = new Dictionary<Ontology_type_enum, bool>();
            Ontology_predictedScpHierarchyIntegrationStrategy_dict = new Dictionary<Ontology_type_enum, Predicted_scp_hierarchy_integration_strategy_enum>();
            Ontology_scpHierarchyInteractionType_dict = new Dictionary<Ontology_type_enum, SCP_hierarchy_interaction_type_enum>();
            Next_ontology_scpHierarchyInteractionType_dict = new Dictionary<Ontology_type_enum, SCP_hierarchy_interaction_type_enum>();
            #endregion

            Node_size_scaling_across_plots = Node_size_scaling_across_plots_enum.Unique;

            Set_default_mbco_parameters();
            Set_default_parameters_for_other_ontologies();
        }

        public void Set_next_ontology_and_organism(Ontology_type_enum next_ontology, Organism_enum next_organism)
        {
            this.Next_ontology = next_ontology;
            this.Next_organism = next_organism;
        }
        public void Set_ontology_and_organism_to_next_ontology_and_organism()
        {
            this.Ontology = this.Next_ontology;
            this.Organism = this.Next_organism;
        }
        private int Get_standard_diameter()
        {
            return 50;
        }

        private void Set_default_mbco_parameters()
        {
            Ontology_add_genes_to_standard_networks.Add(Next_ontology, false);
            Ontology_add_genes_to_dynamic_networks.Add(Next_ontology, false);
            Ontology_add_parent_child_relationships_to_standard_SCP_networks.Add(Next_ontology, true);
            Ontology_add_edges_that_connect_standard_scps.Add(Next_ontology, false);
            Ontology_add_parent_child_relationships_to_dynamic_SCP_networks.Add(Next_ontology, false);
            Ontology_add_additional_edges_that_connect_dynamic_scps.Add(Next_ontology, true);
            int standard_diameter = Get_standard_diameter();
            Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level.Add(Next_ontology, new float[] { });
            Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level[Next_ontology] = new float[] { -1, -1, 0, 0, -1 };
            Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level.Add(Next_ontology, new float[5]);
            Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level[Next_ontology] = new float[] { -1, -1, 0.2F, 0.25F, -1 };
            Ontology_box_sameLevel_scps_for_dynamic_enrichment.Add(Next_ontology, true);
            Ontology_box_sameLevel_scps_for_standard_enrichment.Add(Next_ontology, false);
            Ontology_predictedScpHierarchyIntegrationStrategy_dict.Add(Next_ontology, Predicted_scp_hierarchy_integration_strategy_enum.All_ancestors);
            Next_ontology_scpHierarchyInteractionType_dict.Add(Next_ontology, SCP_hierarchy_interaction_type_enum.Parent_child);
        }

        private void Set_default_parameters_for_other_ontologies()
        {
            Ontology_type_enum[] go_ontologies = new Ontology_type_enum[] { Ontology_type_enum.Go_bp, Ontology_type_enum.Go_cc, Ontology_type_enum.Go_mf};
            Ontology_type_enum[] non_go_ontologies = new Ontology_type_enum[] { Ontology_type_enum.Mbco_na_glucose_tm_transport, Ontology_type_enum.Reactome, Ontology_type_enum.Custom_1, Ontology_type_enum.Custom_2 };
            Ontology_type_enum[] other_ontologies = Overlap_class.Get_union_of_T_arrays(go_ontologies, non_go_ontologies);
            Ontology_type_enum human_ontology;
            foreach (Ontology_type_enum other_ontology in other_ontologies)
            {
                human_ontology = other_ontology;
                Ontology_add_genes_to_standard_networks.Add(human_ontology, false);
                Ontology_add_genes_to_dynamic_networks.Add(human_ontology, false);
                Ontology_add_parent_child_relationships_to_standard_SCP_networks.Add(human_ontology, true);
                Ontology_add_edges_that_connect_standard_scps.Add(human_ontology, false);
                Ontology_add_parent_child_relationships_to_dynamic_SCP_networks.Add(human_ontology, false);
                Ontology_add_additional_edges_that_connect_dynamic_scps.Add(human_ontology, false);
                Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level.Add(human_ontology, new float[] { });
                Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level[human_ontology] = new float[] { -1, -1, -1, -1, -1 };
                Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level.Add(human_ontology, new float[5]);
                Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level[human_ontology] = new float[] { -1, -1, -1, -1, -1 };
                Ontology_box_sameLevel_scps_for_dynamic_enrichment.Add(human_ontology, true);
                Ontology_box_sameLevel_scps_for_standard_enrichment.Add(human_ontology, false);
            }
            foreach (Ontology_type_enum ontology in go_ontologies)
            {
                human_ontology = ontology;
                Ontology_predictedScpHierarchyIntegrationStrategy_dict.Add(human_ontology, Predicted_scp_hierarchy_integration_strategy_enum.All_ancestors);
                Next_ontology_scpHierarchyInteractionType_dict.Add(human_ontology, SCP_hierarchy_interaction_type_enum.Parent_child_regulatory);
            }
            foreach (Ontology_type_enum ontology in non_go_ontologies)
            {
                human_ontology = ontology;
                Ontology_predictedScpHierarchyIntegrationStrategy_dict.Add(human_ontology, Predicted_scp_hierarchy_integration_strategy_enum.All_ancestors);
                Next_ontology_scpHierarchyInteractionType_dict.Add(human_ontology, SCP_hierarchy_interaction_type_enum.Parent_child);
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
            copy.Ontology_predictedScpHierarchyIntegrationStrategy_dict = Array_class.Deep_copy_dictionary_that_contains_no_strings(this.Ontology_predictedScpHierarchyIntegrationStrategy_dict);
            Yed_network_node_size_determinant_enum[] nodeSize_determinants = NodeSizeDeterminant_diameterMax_dict.Keys.ToArray();
            copy.NodeSizeDeterminant_diameterMax_dict = new Dictionary<Yed_network_node_size_determinant_enum, int>();
            foreach (Yed_network_node_size_determinant_enum nodeSize_determinant in nodeSize_determinants)
            {
                copy.NodeSizeDeterminant_diameterMax_dict.Add(nodeSize_determinant, this.NodeSizeDeterminant_diameterMax_dict[nodeSize_determinant]);
            }
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

            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_add_genes_to_standard_networks, "Add_genes_to_standard_networks");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_add_genes_to_dynamic_networks, "Add_genes_to_dynamic_networks");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_add_parent_child_relationships_to_standard_SCP_networks, "Add_parent_child_relationships_to_standard_SCP_networks");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_add_edges_that_connect_standard_scps, "Add_edges_that_connect_standard_scps");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_add_additional_edges_that_connect_dynamic_scps, "Add_additional_edges_that_connect_dynamic_scps");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_add_parent_child_relationships_to_dynamic_SCP_networks, "Add_parent_child_relationships_to_dynamic_SCP_networks");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level, "Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_box_sameLevel_scps_for_dynamic_enrichment, "Box_sameLevel_scps_for_dynamic_enrichment");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_box_sameLevel_scps_for_standard_enrichment, "Box_sameLevel_scps_for_standard_enrichment");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), NodeSizeDeterminant_diameterMax_dict, "Max_diameter");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), NodeSizeDeterminant_labelMinSize_dict, "Label_minSize");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), NodeSizeDeterminant_labelMaxSize_dict, "Label_maxSize");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), NodeSizeDeterminant_labelUniqueSize_dict, "Label_uniqueSize");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Next_ontology_scpHierarchyInteractionType_dict, "Considered_interactions_for_parentChildNetworks");
            base.Write_dictionary_entries(writer, typeof(MBCO_network_based_integration_options_class), Ontology_predictedScpHierarchyIntegrationStrategy_dict, "ParentChildScpNetworkGeneration");
            base.Write_entries_excluding_dictionaries(writer, typeof(MBCO_network_based_integration_options_class), "Node_size_diameterMax_for_current_nodeSize_determinant",
                                        ///"Next_ontology", "Next_organism",  both are private set and will be ignored
                                        "Next_scp_hierachical_interactions", "Scp_hierachical_interactions",
                                        "Next_scp_hierachical_interactions", "Predicted_scp_hierarchy_integration_strategy",
                                        "Label_minSize_for_current_nodeSize_determinant", "Label_maxSize_for_current_nodeSize_determinant", "Label_uniqueSize_for_current_nodeSize_determinant",
                                        "Add_genes_to_standard_networks", "Add_genes_to_dynamic_networks", "Add_edges_that_connect_standard_scps", "Add_additional_edges_that_connect_dynamic_scps",
                                        "Add_parent_child_relationships_to_standard_SCP_networks", "Add_parent_child_relationships_to_dynamic_SCP_networks",
                                        "Box_sameLevel_scps_for_standard_enrichment", "Box_sameLevel_scps_for_dynamic_enrichment");
        }
        public override bool Add_read_entry_to_options_and_return_if_successful(string readLine)
        {
            string[] splitStrings = readLine.Split(Global_class.Tab);
            List<string> missing_entries = new List<string>();
            bool successful = true;
            switch (splitStrings[1])
            {
                case "Generate_scp_networks":
                case "Node_size_determinant":
                case "Node_size_scaling_across_plots":
                case "Adjust_labelSizes_to_nodeSizes":
                case "Graph_editor":
                case "Next_ontology":
                case "Next_organism":
                    successful = base.Add_read_entry_and_return_if_succesful(readLine, typeof(MBCO_network_based_integration_options_class));
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
                //case "Top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level":
                //    Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level = base.Add_to_dictionary_entries(readLine, Ontology_top_quantile_probability_of_scp_interactions_for_dynamic_enrichment_per_level);
                //    break;
                case "Top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level":
                    Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level = base.Add_to_dictionary_entries(readLine, Ontology_top_quantile_probability_of_scp_interactions_to_connect_standard_scp_predictions_per_level);
                    break;
                case "Box_sameLevel_scps_for_dynamic_enrichment":
                    Ontology_box_sameLevel_scps_for_dynamic_enrichment = base.Add_to_dictionary_entries(readLine, Ontology_box_sameLevel_scps_for_dynamic_enrichment);
                    break;
                case "Box_sameLevel_scps_for_standard_enrichment":
                    Ontology_box_sameLevel_scps_for_standard_enrichment = base.Add_to_dictionary_entries(readLine, Ontology_box_sameLevel_scps_for_standard_enrichment);
                    break;
                case "Max_diameter":
                    NodeSizeDeterminant_diameterMax_dict = base.Add_to_dictionary_entries(readLine, NodeSizeDeterminant_diameterMax_dict);
                    break;
                case "Label_minSize":
                    NodeSizeDeterminant_labelMinSize_dict = base.Add_to_dictionary_entries(readLine, NodeSizeDeterminant_labelMinSize_dict);
                    break;
                case "Label_maxSize":
                    NodeSizeDeterminant_labelMaxSize_dict = base.Add_to_dictionary_entries(readLine, NodeSizeDeterminant_labelMaxSize_dict);
                    break;
                case "Label_uniqueSize":
                    NodeSizeDeterminant_labelUniqueSize_dict = base.Add_to_dictionary_entries(readLine, NodeSizeDeterminant_labelUniqueSize_dict);
                    break;
                case "ParentChildScpNetworkGeneration":
                    Ontology_predictedScpHierarchyIntegrationStrategy_dict = base.Add_to_dictionary_entries(readLine, Ontology_predictedScpHierarchyIntegrationStrategy_dict);
                    break;
                case "Considered_interactions_for_parentChildNetworks":
                    Next_ontology_scpHierarchyInteractionType_dict = base.Add_to_dictionary_entries(readLine, Ontology_scpHierarchyInteractionType_dict);
                    break;
                default:
                    missing_entries.Add(splitStrings[1]);
                    break;
            }
            if ((Global_class.Do_internal_checks)&&(missing_entries.Count>0)) { throw new Exception(); }
            if (missing_entries.Count > 0) { successful = false; }
            return successful;
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
        private MBCO_association_class Ontology_association { get; set; }
        public MBCO_obo_network_class Mbco_parentChild_nw { get; set; }
        public MBCO_obo_network_class Mbco_childParent_nw { get; set; }
        public bool Generated_for_all_runs { get; set; }
        #endregion

        public Mbc_network_based_integration_class(Ontology_type_enum next_ontology, Organism_enum next_organism)
        {
            this.Default_options = new MBCO_network_based_integration_options_class(next_ontology, next_organism);
            this.Options = Default_options.Deep_copy();
            Generated_for_all_runs = false;
        }

        #region Generate
        private void Generate_parent_child_and_child_parent_MBCO_networks(ProgressReport_interface_class progressReport, string[] keep_go_scps)
        {
            Mbco_childParent_nw = new MBCO_obo_network_class(this.Options.Ontology, this.Options.Scp_hierachical_interactions, this.Options.Organism);
            Mbco_parentChild_nw = new MBCO_obo_network_class(this.Options.Ontology, this.Options.Scp_hierachical_interactions, this.Options.Organism);
            Mbco_parentChild_nw.Generate_by_reading_safed_spreadsheet_file_or_obo_file_add_missing_scps_if_custom_add_human_processSizes_and_return_if_not_interrupted(progressReport, out bool not_interrupted);
            if (not_interrupted)
            {
                if (Ontology_classification_class.Is_go_ontology(this.Options.Ontology))
                {
                    Mbco_parentChild_nw.Keep_only_scps_of_selected_namespace_if_gene_ontology();
                }
                Mbco_parentChild_nw.Keep_only_input_nodeNames(keep_go_scps);
                Mbco_childParent_nw = Mbco_parentChild_nw.Deep_copy_mbco_obo_nw();
                Mbco_childParent_nw.Reverse_direction();
            }
        }
        private void Generate_ontology_association_file(ProgressReport_interface_class progressReport, string[] keep_go_scps)
        {
            Ontology_association = new MBCO_association_class();
            Ontology_association.Generate_after_reading_safed_file_or_de_novo_and_save(this.Options.Ontology, this.Options.Organism, new Dictionary<Ontology_type_enum, Dictionary<GO_hyperParameter_enum, int>>(), progressReport);
            Ontology_association.Keep_only_indicated_scps(keep_go_scps);
        }

        public void Generate_for_all_runs_after_setting_ontology_and_organism_to_next_ontology_and_organism(Ontology_type_enum next_ontology, Organism_enum next_organism, string[] keep_go_scps, ProgressReport_interface_class progressReport)
        {
            if (!this.Options.Next_ontology.Equals(next_ontology)) { throw new Exception(); }
            if (!this.Options.Next_organism.Equals(next_organism)) { throw new Exception(); }
            this.Options.Set_ontology_and_organism_to_next_ontology_and_organism();
            this.Options.Scp_hierachical_interactions = this.Options.Next_scp_hierachical_interactions;
            Generate_parent_child_and_child_parent_MBCO_networks(progressReport, keep_go_scps);
            Generate_ontology_association_file(progressReport, keep_go_scps);
            Generated_for_all_runs = true;
        }
        #endregion

        public void Replace_options_by_default_options_but_keep_nextOntology_nextOrganism_and_reset_generated_for_all_runs()
        {
            Ontology_type_enum next_ontology = Options.Next_ontology;
            Options = Default_options.Deep_copy();
            Options.Set_next_ontology_and_organism(next_ontology, Options.Next_organism);
            Generated_for_all_runs = false;
        }

        #region Scp gene network
        private List<string> Identify_youngest_existing_scps_for_each_gene_recursive(Dictionary<string, List<string>> exisistingParent_existingChildren_dict, Dictionary<string, Dictionary<string, bool>> scp_genes_dict, string gene, string lastMapped_scp)
        {
            List<string> mapped_scps = new List<string>();
            if (exisistingParent_existingChildren_dict.ContainsKey(lastMapped_scp))
            {
                string[] childScps = exisistingParent_existingChildren_dict[lastMapped_scp].ToArray();
                foreach (string child_scp in childScps)
                {
                    if (scp_genes_dict[child_scp].ContainsKey(gene))
                    {
                        mapped_scps.AddRange(Identify_youngest_existing_scps_for_each_gene_recursive(exisistingParent_existingChildren_dict, scp_genes_dict, gene, child_scp));
                    }
                }
            }
            if (mapped_scps.Count ==0) { mapped_scps.Add(lastMapped_scp); }
            return mapped_scps;
        }


        private Network_class Generate_network_from_ontology_enrichment_lines_and_gene_colors_to_scpGeneLegend_color_dict(Ontology_enrichment_line_class[] ontology_enrichment_lines, ref Dictionary<string,List<Color_specification_line_class>> scpGeneLegend_colorSpecifications_dict, bool remove_genes_parent_SCPs_if_part_of_child_SCPs, string[] all_parentChild_scps, float minimum_node_size)
        {
            float gene_node_size = 0.75F * minimum_node_size;
            int ontology_enrichment_lines_length = ontology_enrichment_lines.Length;
            Ontology_enrichment_line_class ontology_enrichment_line;
            Dictionary<string, Dictionary<string,bool>> scp_genes_dict = this.Ontology_association.Get_scp_targetGene_dictionary();
            Dictionary<string, Dictionary<string, List<string>>> ancestorScp_gene_mappToScps_dict = new Dictionary<string, Dictionary<string, List<string>>>();

            #region Generate existingParentScp_existingChildScp_dict
            Dictionary<string, bool> networkScp_dict = new Dictionary<string, bool>();
            foreach (string parent_child_scp in all_parentChild_scps)
            {
                networkScp_dict.Add(parent_child_scp, true);
            }
            Dictionary<string, string[]> parentScp_childScp_dict = Mbco_parentChild_nw.Get_parentScp_childScp_dictionary_if_parent_child();
            Dictionary<string, List<string>> existingParentScp_existingChildScp_dict = new Dictionary<string, List<string>>();
            string[] parents = parentScp_childScp_dict.Keys.ToArray();
            string[] children;
            foreach (string parent in parents)
            {
                if (networkScp_dict.ContainsKey(parent))
                {
                    children = parentScp_childScp_dict[parent];
                    existingParentScp_existingChildScp_dict.Add(parent, new List<string>());
                    if (remove_genes_parent_SCPs_if_part_of_child_SCPs)
                    {
                        foreach (string child in children)
                        {
                            if (networkScp_dict.ContainsKey(child))
                            {
                                existingParentScp_existingChildScp_dict[parent].Add(child);
                            }
                        }
                    }
                }
            }
            #endregion

            Network_class nw = new Network_class();

            Color_specification_line_class color_specification_line;
            NetworkTable_line_class new_network_table_line;
            List<NetworkTable_line_class> new_network_table_list = new List<NetworkTable_line_class>();
            string[] target_symbols;
            string target_symbol;
            int target_symbols_length;
            string[] mapToScps;
            string mapToScp;
            int mapToScps_length;
            Dictionary<string, Dictionary<string, Dictionary<string, bool>>> uniqueDataset_scp_addedGene_dict = new Dictionary<string, Dictionary<string, Dictionary<string, bool>>>();
            Dictionary<string, Dictionary<string, bool>> uniqueDataset_gene_colorAdded_dict = new Dictionary<string, Dictionary<string, bool>>();
            for (int indexO = 0; indexO < ontology_enrichment_lines_length; indexO++)
            {
                ontology_enrichment_line = ontology_enrichment_lines[indexO];
                if (!uniqueDataset_scp_addedGene_dict.ContainsKey(ontology_enrichment_line.Unique_dataset_name))
                {
                    uniqueDataset_scp_addedGene_dict.Add(ontology_enrichment_line.Unique_dataset_name, new Dictionary<string, Dictionary<string, bool>>());
                    uniqueDataset_gene_colorAdded_dict.Add(ontology_enrichment_line.Unique_dataset_name, new Dictionary<string, bool>());
                }
                target_symbols = ontology_enrichment_line.Overlap_symbols;
                target_symbols_length = target_symbols.Length;
                for (int indexT = 0; indexT < target_symbols_length; indexT++)
                {
                    target_symbol = target_symbols[indexT];
                    mapToScps = Identify_youngest_existing_scps_for_each_gene_recursive(existingParentScp_existingChildScp_dict, scp_genes_dict, target_symbol, ontology_enrichment_line.Scp_name).ToArray();
                    mapToScps_length = mapToScps.Length;
                    for (int indexMap = 0; indexMap < mapToScps_length; indexMap++)
                    {
                        mapToScp = mapToScps[indexMap];
                        if (!uniqueDataset_scp_addedGene_dict[ontology_enrichment_line.Unique_dataset_name].ContainsKey(mapToScp))
                        {
                            uniqueDataset_scp_addedGene_dict[ontology_enrichment_line.Unique_dataset_name].Add(mapToScp, new Dictionary<string, bool>());
                        }
                        if (!uniqueDataset_scp_addedGene_dict[ontology_enrichment_line.Unique_dataset_name][mapToScp].ContainsKey(target_symbol))
                        {
                            uniqueDataset_scp_addedGene_dict[ontology_enrichment_line.Unique_dataset_name][mapToScp].Add(target_symbol, true);
                            new_network_table_line = new NetworkTable_line_class();
                            new_network_table_line.Source = (string)mapToScp.Clone();
                            new_network_table_line.Target = (string)target_symbol.Clone();
                            new_network_table_list.Add(new_network_table_line);
                        }
                    }
                    if (!uniqueDataset_gene_colorAdded_dict[ontology_enrichment_line.Unique_dataset_name].ContainsKey(target_symbol))
                    {
                        uniqueDataset_gene_colorAdded_dict[ontology_enrichment_line.Unique_dataset_name].Add(target_symbol, true);
                        if (!scpGeneLegend_colorSpecifications_dict.ContainsKey(target_symbol))
                        {
                            scpGeneLegend_colorSpecifications_dict.Add(target_symbol, new List<Color_specification_line_class>());
                        }
                        color_specification_line = new Color_specification_line_class();
                        color_specification_line.Fill_color = ontology_enrichment_line.Sample_color;
                        color_specification_line.Size = gene_node_size;
                        color_specification_line.Dataset_order_no = ontology_enrichment_line.Results_number;
                        scpGeneLegend_colorSpecifications_dict[target_symbol].Add(color_specification_line);
                    }
                }
            }
            nw.Add_from_networkTable_lines(new_network_table_list.ToArray());
            return nw;
        }
        #endregion

        #region Integrative networks
        private void Generate_leave_out_background_scp_network(Enrichment_type_enum enrichment_type, ProgressReport_interface_class progressReport)
        {
            ///Leave out background network will be regenerated every enrichment cycle
            Dictionary<string, int> processName_processLevel_dict = Mbco_parentChild_nw.Get_processName_level_dictionary_without_setting_process_level();

            Leave_out_class leave_out = new Leave_out_class(Options.Ontology);
            leave_out.Generate_by_reading_safed_file(progressReport);

            Leave_out_background_scp_network = new Leave_out_scp_scp_network_class(this.Options.Ontology);
            Leave_out_background_scp_network.Options.Top_quantile_of_considered_SCP_interactions_per_level = Options.Get_top_quantile_probability_of_scp_interactions_for_dynamic_or_standard_enrichment_per_level(enrichment_type);
            Leave_out_background_scp_network.Generate_scp_scp_network_from_leave_out(leave_out, progressReport);
            Leave_out_background_scp_network.Scp_nw.Transform_into_undirected_single_network_and_set_all_widths_to_default();
            Leave_out_background_scp_network.Scp_nw.Nodes.Set_processLevel_for_all_nodes_based_on_dictionary(processName_processLevel_dict);
        }

        private float Get_nodeSize_value(float minusLog10Pvalue)
        {
            switch (Options.Node_size_determinant)
            {
                case Yed_network_node_size_determinant_enum.No_of_different_colors:
                case Yed_network_node_size_determinant_enum.No_of_sets:
                case Yed_network_node_size_determinant_enum.Uniform:
                    return 1;
                case Yed_network_node_size_determinant_enum.Minus_log10_pvalue:
                    return minusLog10Pvalue;
                default:
                    throw new Exception();
            }

        }
        private float Get_add_nodeSize_value(float minusLog10Pvalue)
        {
            switch (Options.Node_size_determinant)
            {
                case Yed_network_node_size_determinant_enum.No_of_different_colors:
                case Yed_network_node_size_determinant_enum.No_of_sets:
                case Yed_network_node_size_determinant_enum.Uniform:
                    return 0;
                case Yed_network_node_size_determinant_enum.Minus_log10_pvalue:
                    return minusLog10Pvalue;
                default:
                    throw new Exception();
            }

        }
        private void Write_shared_information(StreamWriter writer)
        {
            writer.WriteLine("Legend");
            writer.WriteLine("If node and pie chart slice areas represent the sum of all and single -log10(p-values),");
            writer.WriteLine("respectively, the application will generate multiple legend nodes spanning the range of");
            writer.WriteLine("the visualized sums of -log10(p-values).");
            writer.WriteLine("Since pie charts and circles visualizing predicted sub-cellular processes (SCPs) are generated");
            writer.WriteLine("by two different algorithms, the applicationn will generate legend nodes using both algorithms");
            writer.WriteLine("for quality control. Since both node types have the same size, simply delete the nodes labeled");
            writer.WriteLine("with '(pie charts)'.");
            writer.WriteLine("For an optimal visualization of the legend nodes, arrange them on top of or next to each other. The");
            writer.WriteLine("node with the headline that describes what these values mean can then be moved on top or left of");
            writer.WriteLine("the legend nodes. To allow easier handling of this node in the graph editor, the border of headline");
            writer.WriteLine("node is black. For the final figure, change to color of the border to white or no color.");
            writer.WriteLine("");
            writer.WriteLine("Addition of genes");
            writer.WriteLine("If selected, the application will add genes that are expressed within a dataset and annotated to");
            writer.WriteLine("a predicted SCP or pathway as child nodes of that SCP or pathway. Only genes that are annotated");
            writer.WriteLine("to SCPs / pathways predicted by the same dataset (i.e., listed in the related significantPredictions");
            writer.WriteLine("file) will be included.");
            writer.WriteLine("When SCPs / pathways are connected via parent–child relationships, genes are always added to the");
            writer.WriteLine("youngest predicted child of any dataset to improve clarity. If multiple datasets are analyzed,");
            writer.WriteLine("it may occur that a parent and its child SCP/pathway are predicted by different datasets. In this");
            writer.WriteLine("case, both datasets will be connected, and genes expressed in the dataset that predicted the parent");
            writer.WriteLine("SCP/pathway will also be mapped to the corresponding child, even if that child was not predicted");
            writer.WriteLine("in that dataset. Please not that genes of a dataset of interest that map to an SCP/pathway predicted");
            writer.WriteLine("for another dataset will not be added, if none of the ancestor SCPs/pathways were predicted for the");
            writer.WriteLine("dataset of interest.");
            writer.WriteLine("To determine which predicted SCP(s)/pathway(s) a gene in a dataset is assigned to, follow the");
            writer.WriteLine("parent-child hierarchy upward until reaching an SCP/pathway colored the same as the gene.");
            writer.WriteLine("Alternatively, visualize each dataset in a separate network. Within the 'Automatically set'-");
            writer.WriteLine("panel in 'Organize data'-menu, press the 'Integration groups'- and the poping up");
            writer.WriteLine("'Update changes'-button and redo the analysis.");
        }
        private void Write_readMe_file_for_yED_networks(string complete_network_directory)
        {
            string complete_fileName = complete_network_directory + "ReadMe_yED_graph_editor.txt";
            StreamWriter writer = new StreamWriter(complete_fileName);
            writer.WriteLine("yED Graph Editor");
            writer.WriteLine("Graphml network files can be opened with yED Graph Editor (www.yworks.com/products/yED).");
            writer.WriteLine("A double click is sufficient to open the network.");
            writer.WriteLine("");
            writer.WriteLine("Network layout");
            writer.WriteLine("Within yED, open the 'Layout' menu. The layout option 'Tree' - 'Directed' is a good starting");
            writer.WriteLine("point for initial node arrangements. Once selected, a menu will pop up for the definition");
            writer.WriteLine("of additional parameters. Select the tab 'Directed' and activate 'Consider Node Labels' to");
            writer.WriteLine("ensure that labels and nodes are not spatially overalapping.");
            writer.WriteLine("The 'Consider Node Labels' checkbox can be activated for other selected layouts as well.");
            writer.WriteLine();
            writer.WriteLine("Network zoom in/out");
            writer.WriteLine("yED Graph Editor allows selection of the zoom levels using related buttons at the top panel or the mouse");
            writer.WriteLine("wheel. After zooming out more than 5 times from the 1:1 zoom level, pie charts will be visualized as gray");
            writer.WriteLine("squares and node labels will disappear. Depending on the size of your screen you might want to");
            writer.WriteLine("decrease the SCP node diameters and label reference font sizes, using the 'SCP networks' menu in our");
            writer.WriteLine("application. Decreasing the size of all network elements will allow investigation of larger network");
            writer.WriteLine("areas at those zoom levels that still show pie charts and node labels.");
            writer.WriteLine();
            writer.WriteLine("Paint nodes over edges");
            writer.WriteLine("A useful selection is 'Paint Nodes over Edges' that can be found within the 'File' menu, 'Preferences',");
            writer.WriteLine("'Display' tab.");
            writer.WriteLine("");
            writer.WriteLine("Network edges");
            writer.WriteLine("To distinguish between parent-child and inferred functional interactions between subcellular processes");
            writer.WriteLine("(SCPs), the application uses solid and dashed lines, respectively. Since, if not selected otherwise,");
            writer.WriteLine("SCP networks generated by dynamic enrichment analysis do not show parent child interactions, it is");
            writer.WriteLine("recommended to change the edge style to solid within yED Graph Editor. Simply, select one edge and");
            writer.WriteLine("press 'Ctrl' plus 'A' to select all edges. Now change the line style using the 'Properties View'");
            writer.WriteLine("in the bottom right corner of yED Graph Editor.");
            writer.WriteLine("");
            writer.WriteLine("Adjust distances between nodes and node labels");
            writer.WriteLine("Distances between nodes and node labels can be specified after node selection by changing the");
            writer.WriteLine("'Distance'-property within the 'Properties View' at the lower left corner.");
            writer.WriteLine("");
            writer.WriteLine("Export network to PDF");
            writer.WriteLine("Within the yED Graph Editor you can export your network to a PDF using the 'Export' functionality");
            writer.WriteLine("as part of the 'File' menu. Exported networks always show pie charts and node labels, independently");
            writer.WriteLine("of the current yED zoom level.");
            writer.WriteLine("");
            writer.WriteLine("Area-independent use of decimal points instead of commas");
            writer.WriteLine("When writing graphml files for network visualization, we enforce points as decimal separators.");
            writer.WriteLine("This allowed error-free data import using any English version of the yED graph editor we tested.");
            writer.WriteLine("If networks cannot be loaded, double check that your version uses point and not comma separators.");
            writer.WriteLine("");
            Write_shared_information(writer);
            writer.Close();
        }
        private void Write_readMe_file_for_cytoscape_networks(string complete_network_directory)
        {
            string complete_fileName = complete_network_directory + "ReadMe_cytoscape.txt";
            StreamWriter writer = new StreamWriter(complete_fileName);
            writer.WriteLine("Cytoscape");
            writer.WriteLine("Xgmml network files and the xml style file can be opened with Cytoscape (https://cytoscape.org/).");
            writer.WriteLine("Within Cytoscape, open the 'File'-menu and select 'Import' 'Network from file' and 'Styles from file'");
            writer.WriteLine("to upload a network and the 'Cytoscape_styles.xml'-file. Open the 'Styles'-panel using the tab at the left side");
            writer.WriteLine("and select 'MBC pathNet' as a 'Style'.");
            writer.WriteLine("When uploading additional networks, ensure to unselect each uploaded network to prevent that the new");
            writer.WriteLine("network will be merged with the existing ones. The 'Cytoscape_styles'-file needs only to be uploaded");
            writer.WriteLine("once. If accidentally merged, node duplicates from multiple networks will be overlaid.");
            writer.WriteLine();
            writer.WriteLine("Group legend nodes and/or SCPs of the same level");
            writer.WriteLine("To group SCPs of the same level or level nodes, open the 'Layout'-menu, select 'Group Attributes Layout'");
            writer.WriteLine("and select the attribute 'Group label'. If the nodes are not separated by group, try to first use another");
            writer.WriteLine("layout. Once the nodes of the same level and the legend nodes can be separately selected, right click on");
            writer.WriteLine("selected nodes to group them.");
            writer.WriteLine();
            writer.WriteLine("Network edges");
            writer.WriteLine("To distinguish between parent-child and inferred functional interactions between subcellular processes");
            writer.WriteLine("(SCPs), the application uses solid and dashed line styles, respectively. Since, if not selected otherwise,");
            writer.WriteLine("SCP networks generated by dynamic enrichment analysis do not show parent child interactions, it is");
            writer.WriteLine("recommended to change the edge style to solid within Cytoscape.");
            writer.WriteLine("");
            writer.WriteLine("Area-independent use of decimal points instead of commas");
            writer.WriteLine("When writing xgmml files for network visualization, we enforce points as decimal separators.");
            writer.WriteLine("This allowed error-free data import using any English version of the Cytoscape we tested.");
            writer.WriteLine("If networks cannot be loaded, double check that your version uses point and not comma separators.");
            writer.WriteLine();
            Write_shared_information(writer);
            writer.Close();
        }

        public bool Generate_and_write_integrative_network_for_indicated_enrichment_results_of_each_integrationGroupName_only_defined_sets_and_return_if_interrupted(Ontology_enrichment_class onto_enrich_for_network, Ontology_enrichment_class standard_onto_enrich_unfiltered_input, string results_directory, string baseFile_name, Enrichment_type_enum enrichmentType, Dictionary<Yed_network_node_size_determinant_enum, float> networkNodeSizeDeterminant_maxValue_dict, Dictionary<Yed_network_node_size_determinant_enum, float> networkNodeSizeDeterminant_minValue_dict, float lowest_scp_pvalue_cutoff, ProgressReport_interface_class progressReport)
        {
            if (!Options.Next_ontology.Equals(Options.Ontology)) { throw new Exception(); }
            Generate_leave_out_background_scp_network(enrichmentType, progressReport);
            if (Options.Add_parent_child_relationships(enrichmentType))
            {
                Leave_out_background_scp_network.Set_all_edge_line_types_to_given_line_type(NWedge_type_enum.Dashed_line);
            }
            onto_enrich_for_network.Get_networkNodeSizeDeterminant_max_and_min_values_dictionaries(out Dictionary<Yed_network_node_size_determinant_enum, float> local_networkNodeSizeDeterminant_maxValue_dict, out Dictionary<Yed_network_node_size_determinant_enum, float> local_networkNodeSizeDeterminant_minValue_dict);

            Dictionary<string, bool> scp_dict = standard_onto_enrich_unfiltered_input.Get_all_scps_dict();
            standard_onto_enrich_unfiltered_input.Adjust_unique_dataset_name_if_it_overlaps_with_scp(scp_dict);
            onto_enrich_for_network.Adjust_unique_dataset_name_if_it_overlaps_with_scp(scp_dict);

            #region Check and prepare input enrichment files
            Ontology_enrichment_class standard_onto_enrich_unfiltered = standard_onto_enrich_unfiltered_input.Deep_copy();
            Dictionary<string, Dictionary<string, Dictionary<Entry_type_enum, Dictionary<float, Dictionary<int, bool>>>>> integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict = onto_enrich_for_network.Get_all_conditions_dict();
            standard_onto_enrich_unfiltered.Keep_only_indicated_conditions(integrationGroup_sampleName_entryType_timepointInDays_processLevel_dict);
            string[] all_scps = onto_enrich_for_network.Get_all_scps_after_spliting_scp_unions();
            standard_onto_enrich_unfiltered.Keep_only_input_scpNames(all_scps);
            standard_onto_enrich_unfiltered.Check_for_correctness();
            standard_onto_enrich_unfiltered.Check_if_one_integrationGroup();
            onto_enrich_for_network.Check_for_correctness();
            onto_enrich_for_network.Check_if_one_integrationGroup();
            standard_onto_enrich_unfiltered.Order_by_uniqueDatasetName_descendingMinusLog10pvalue_scpName();
            onto_enrich_for_network.Order_by_uniqueDatasetName_descendingMinusLog10pvalue_scpName();
            #endregion

            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();

            string complete_subdirectory = results_directory + global_dirFile.Get_results_subdirectory_for_integrative_dynamic_scps_networks(onto_enrich_for_network.Get_ontology_and_check_if_only_one_ontology(), "SCP_networks");
            Ontology_enrichment_line_class onto_enrich_unfiltered_line = new Ontology_enrichment_line_class();
            ReadWriteClass.Create_directory_if_it_does_not_exist(complete_subdirectory);
            int standard_onto_enrich_length = onto_enrich_for_network.Enrich.Length;
            Ontology_enrichment_line_class onto_enrich_line;
            Dictionary<string, int> legend_node_level_dict = new Dictionary<string, int>();
            List<string> legend_dataset_nodes = new List<string>();

            Network_class current_uniqueDataset_scp_network = new Network_class(); 
            Leave_out_scp_scp_network_class current_enrichmentLine_scp_network;
            string[] scps;
            string scp;
            int scps_length;

            #region Variables for same integration group
            Network_class combined_scpNetwork = new Network_class();
            Dictionary<string, List<Color_specification_line_class>> scpGeneLegend_colorSpecification_dict = new Dictionary<string, List<Color_specification_line_class>>();
            Color_specification_line_class color_specification_line;
            List<string> sets_ofCurrentIntegrationGroup = new List<string>();
            #endregion

            Dictionary<string, int> processName_processLevel_dict = Mbco_parentChild_nw.Get_processName_level_dictionary_without_setting_process_level();
            Dictionary<string, Color_specification_line_class> processNameOfUniqueDataset_colorSpecificationLine_dict = new Dictionary<string, Color_specification_line_class>();
            Dictionary<string, Dictionary<string, bool>> uniqueDataset_singleSigScp_dict = new Dictionary<string, Dictionary<string, bool>>();
            string[] missing_scps;

            float local_min_referenceValue = local_networkNodeSizeDeterminant_minValue_dict[Options.Node_size_determinant];
            float local_max_referenceValue = local_networkNodeSizeDeterminant_maxValue_dict[Options.Node_size_determinant];
            if (local_max_referenceValue<local_min_referenceValue) { throw new Exception(); }
            float legend_node_size = local_min_referenceValue + 0.2F * (local_max_referenceValue - local_min_referenceValue);

            float max_referenceValue;
            float min_referenceValue;
            int current_min_font_size = -1;
            int current_max_font_size = -1;
            switch (Options.Node_size_scaling_across_plots)
            {
                case Node_size_scaling_across_plots_enum.Unique:
                    min_referenceValue = local_min_referenceValue;
                    max_referenceValue = local_max_referenceValue;
                    current_min_font_size = Options.Label_minSize_for_current_nodeSize_determinant;
                    current_max_font_size = Options.Label_maxSize_for_current_nodeSize_determinant;
                    break;
                case Node_size_scaling_across_plots_enum.Consitent:
                    max_referenceValue = networkNodeSizeDeterminant_maxValue_dict[Options.Node_size_determinant];
                    min_referenceValue = networkNodeSizeDeterminant_minValue_dict[Options.Node_size_determinant];
                    //current_min_font_size = (int)Math.Round(Options.Label_minSize_for_current_nodeSize_determinant * local_min_referenceValue / min_referenceValue);
                    //current_max_font_size = (int)Math.Round(Options.Label_maxSize_for_current_nodeSize_determinant * local_max_referenceValue / max_referenceValue);
                    current_min_font_size = Options.Label_minSize_for_current_nodeSize_determinant;
                    current_max_font_size = Options.Label_maxSize_for_current_nodeSize_determinant;
                    break;
                default:
                    throw new Exception();
            }
            if (max_referenceValue < min_referenceValue) { throw new Exception(); }

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
                    color_specification_line = new Color_specification_line_class();
                    color_specification_line.Fill_color = onto_enrich_line.Sample_color;
                    color_specification_line.Size = legend_node_size;
                    color_specification_line.Dataset_order_no = 0;
                    processNameOfUniqueDataset_colorSpecificationLine_dict.Clear();
                    processNameOfUniqueDataset_colorSpecificationLine_dict[onto_enrich_line.Unique_dataset_name] = color_specification_line;
                    legend_node_level_dict.Add(onto_enrich_line.Unique_dataset_name,Global_class.Network_legend_level);
                    legend_dataset_nodes.Add(onto_enrich_line.Unique_dataset_name);
                    uniqueDataset_singleSigScp_dict.Add(onto_enrich_line.Unique_dataset_name, new Dictionary<string, bool>());
                }
                switch (enrichmentType)
                {
                    case Enrichment_type_enum.Dynamic:
                        scps = onto_enrich_line.Scp_name.Split('$');
                        scps = scps.OrderBy(l => l).ToArray();
                        scps_length = scps.Length;
                        float current_minusLog10Pvalue_for_each_scp = onto_enrich_line.Minus_log10_pvalue / scps_length;
                        for (int indexScp=0; indexScp<scps_length; indexScp++)
                        {
                            scp = scps[indexScp];
                            if (!uniqueDataset_singleSigScp_dict[onto_enrich_line.Unique_dataset_name].ContainsKey(scp))
                            { uniqueDataset_singleSigScp_dict[onto_enrich_line.Unique_dataset_name].Add(scp, true); }
                            if (!processNameOfUniqueDataset_colorSpecificationLine_dict.ContainsKey(scp))
                            {
                                color_specification_line = new Color_specification_line_class();
                                color_specification_line.Fill_color = onto_enrich_line.Sample_color;
                                color_specification_line.Size = Get_nodeSize_value(current_minusLog10Pvalue_for_each_scp);
                                color_specification_line.Dataset_order_no = onto_enrich_line.Results_number;
                                processNameOfUniqueDataset_colorSpecificationLine_dict.Add(scp, color_specification_line);
                            }
                            else
                            {
                                processNameOfUniqueDataset_colorSpecificationLine_dict[scp].Size += Get_add_nodeSize_value(current_minusLog10Pvalue_for_each_scp);
                            }
                        }
                        current_enrichmentLine_scp_network = Leave_out_background_scp_network.Deep_copy_scp_network();
                        current_enrichmentLine_scp_network.Scp_nw.Keep_only_input_nodeNames(scps);
                        missing_scps = Overlap_class.Get_part_of_list1_but_not_of_list2(scps, current_enrichmentLine_scp_network.Scp_nw.Nodes.Get_all_ordered_nodeNames());
                        current_enrichmentLine_scp_network.Scp_nw.Add_single_nodes(missing_scps);
                        current_uniqueDataset_scp_network.Merge_this_network_with_other_network(current_enrichmentLine_scp_network.Scp_nw);
                        break;
                    case Enrichment_type_enum.Standard:
                        scp = onto_enrich_line.Scp_name;
                        uniqueDataset_singleSigScp_dict[onto_enrich_line.Unique_dataset_name].Add(scp, true);
                        current_minusLog10Pvalue_for_each_scp = onto_enrich_line.Minus_log10_pvalue;
                        color_specification_line = new Color_specification_line_class();
                        color_specification_line.Fill_color = onto_enrich_line.Sample_color;
                        color_specification_line.Size = Get_nodeSize_value(onto_enrich_line.Minus_log10_pvalue);
                        color_specification_line.Dataset_order_no = onto_enrich_line.Results_number;
                        processNameOfUniqueDataset_colorSpecificationLine_dict.Add(scp, color_specification_line);
                        current_uniqueDataset_scp_network.Add_single_nodes(scp);
                        break;
                    default:
                        throw new Exception();
                }

                if ((indexO == standard_onto_enrich_length - 1)
                    || (!onto_enrich_line.Unique_dataset_name.Equals(onto_enrich_for_network.Enrich[indexO + 1].Unique_dataset_name)))
                {
                    current_uniqueDataset_scp_network.Add_single_nodes(onto_enrich_line.Unique_dataset_name);
                    combined_scpNetwork.Merge_this_network_with_other_network(current_uniqueDataset_scp_network);
                    scps = processNameOfUniqueDataset_colorSpecificationLine_dict.Keys.ToArray();
                    scps_length = scps.Length;
                    for (int indexScp = 0; indexScp < scps_length; indexScp++)
                    {
                        scp = scps[indexScp];
                        if (!scpGeneLegend_colorSpecification_dict.ContainsKey(scp))
                        {
                            scpGeneLegend_colorSpecification_dict.Add(scp, new List<Color_specification_line_class>());
                        }
                        scpGeneLegend_colorSpecification_dict[scp].Add(processNameOfUniqueDataset_colorSpecificationLine_dict[scp]);
                    }
                }
            }
            bool parent_scps_added = false;
            bool legend_nodes_generated = false;
            float minimum_data_scp_size = -1;

            #region Identify minimum data scp size
            if (parent_scps_added) { throw new Exception("Intermediate nodes must not be added when identifying minimum data scp size"); }
            if (legend_nodes_generated) { throw new Exception("Legend nodes must not be added when identifying minimum data scp size"); }
            string[] processNameOfUniqueDataset_array = scpGeneLegend_colorSpecification_dict.Keys.ToArray();
            List<Color_specification_line_class> min_color_specification_lines;
            foreach (string processNameOfUniqueDataset in processNameOfUniqueDataset_array)
            {
                min_color_specification_lines = scpGeneLegend_colorSpecification_dict[processNameOfUniqueDataset];
                foreach (Color_specification_line_class min_color_specification_line in min_color_specification_lines)
                {
                    if ((minimum_data_scp_size == -1)
                        || (minimum_data_scp_size > min_color_specification_line.Size))
                    {
                        minimum_data_scp_size = min_color_specification_line.Size;
                    }
                }
            }
            if (minimum_data_scp_size == -1) { minimum_data_scp_size = 1; }
            if (minimum_data_scp_size < -(float)Math.Log10(lowest_scp_pvalue_cutoff)) { minimum_data_scp_size = -(float)Math.Log10(lowest_scp_pvalue_cutoff); }
            #endregion




            if (Options.Add_parent_child_relationships(enrichmentType))
            {
                if (minimum_data_scp_size==-1) { throw new Exception(); }
                MBCO_obo_network_class scp_parent_child_network = this.Mbco_parentChild_nw.Deep_copy_mbco_obo_nw();
                string[] network_scps = combined_scpNetwork.Nodes.Get_all_ordered_nodeNames();
                string[] all_ancestor_scps = new string[0];
                string[] all_descendent_scps = new string[0];
                string[] intermediate_scps = new string[0];
                string[] scpsInParentChildNetwork;
                switch (Options.Predicted_scp_hierarchy_integration_strategy)
                {
                    case Predicted_scp_hierarchy_integration_strategy_enum.All_ancestors:
                        Mbco_childParent_nw.Nodes.Order_by_nw_index();
                        all_ancestor_scps = this.Mbco_childParent_nw.Get_all_ancestors_if_direction_is_child_parent_without_ordering_nodes_by_index(network_scps);
                        intermediate_scps = Overlap_class.Get_part_of_list1_but_not_of_list2(all_ancestor_scps, network_scps);
                        break;
                    case Predicted_scp_hierarchy_integration_strategy_enum.Intermediate_nodes:
                        Mbco_childParent_nw.Nodes.Order_by_nw_index();
                        Mbco_parentChild_nw.Nodes.Order_by_nw_index();
                        all_ancestor_scps = this.Mbco_childParent_nw.Get_all_ancestors_if_direction_is_child_parent_without_ordering_nodes_by_index(network_scps);
                        all_descendent_scps = this.Mbco_parentChild_nw.Get_all_descendents_if_direction_is_parent_child_without_ordering_nodes_by_index(network_scps);
                        Dictionary<string, bool> scp_isAncestor_dict = new Dictionary<string, bool>();
                        List<string> scps_visited_from_both_directions = new List<string>();
                        foreach (string ancestor_scp in all_ancestor_scps)
                        {
                            scp_isAncestor_dict.Add(ancestor_scp, true);
                        }
                        foreach (string descendent_scp in all_descendent_scps)
                        {
                            if (scp_isAncestor_dict.ContainsKey(descendent_scp))
                            {
                                scps_visited_from_both_directions.Add(descendent_scp);
                            }
                        }
                        intermediate_scps = Overlap_class.Get_part_of_list1_but_not_of_list2(scps_visited_from_both_directions.ToArray(), network_scps);
                        break;
                    case Predicted_scp_hierarchy_integration_strategy_enum.First_shared_parent:
                    default:
                        throw new Exception();
                }
                intermediate_scps = Overlap_class.Get_part_of_list1_but_not_of_list2(intermediate_scps, "Molecular Biology of the Cell");
                scpsInParentChildNetwork = Overlap_class.Get_union_of_string_arrays_keeping_the_order(network_scps, intermediate_scps);
                if (intermediate_scps.Length > 0)
                {
                    foreach (string intermediate_scp in intermediate_scps)
                    {
                        scpGeneLegend_colorSpecification_dict.Add(intermediate_scp, new List<Color_specification_line_class>());
                        color_specification_line = new Color_specification_line_class();
                        color_specification_line.Fill_color = Global_class.Intermediate_network_scps_color;
                        color_specification_line.Size = minimum_data_scp_size;
                        scpGeneLegend_colorSpecification_dict[intermediate_scp].Add(color_specification_line);
                    }
                    string unpredicted_intermediate_scps_legend = "Not predicted intermediate SCPs";
                    scpGeneLegend_colorSpecification_dict.Add(unpredicted_intermediate_scps_legend, new List<Color_specification_line_class>());
                    color_specification_line = new Color_specification_line_class();
                    color_specification_line.Fill_color = Global_class.Intermediate_network_scps_color;
                    color_specification_line.Size = legend_node_size;
                    scpGeneLegend_colorSpecification_dict[unpredicted_intermediate_scps_legend].Add(color_specification_line);
                    legend_node_level_dict.Add(unpredicted_intermediate_scps_legend, Global_class.Network_legend_level);
                    combined_scpNetwork.Add_single_nodes(unpredicted_intermediate_scps_legend);
                }
                scp_parent_child_network.Keep_only_input_nodeNames(scpsInParentChildNetwork);
                combined_scpNetwork.Merge_this_network_with_other_network(scp_parent_child_network);
                parent_scps_added = true;
            }

            if (Options.Add_genes_to_networks(enrichmentType))
            {
                if (minimum_data_scp_size == -1) { throw new Exception(); }
                if ((Options.Add_parent_child_relationships(enrichmentType)) && (!parent_scps_added)) { throw new Exception("Add genes to networks HAS TO BE CALLED AFTER add parent child relationships"); }

                int standard_onto_enrich_unfiletered_length = standard_onto_enrich_unfiltered.Enrich.Length;
                List<Ontology_enrichment_line_class> current_uniqueDataset_standard = new List<Ontology_enrichment_line_class>();
                for (int indexUnfiltered = 0; indexUnfiltered < standard_onto_enrich_unfiletered_length; indexUnfiltered++)
                {
                    onto_enrich_unfiltered_line = standard_onto_enrich_unfiltered.Enrich[indexUnfiltered];
                    if ((uniqueDataset_singleSigScp_dict.ContainsKey(onto_enrich_unfiltered_line.Unique_dataset_name))
                        && (uniqueDataset_singleSigScp_dict[onto_enrich_unfiltered_line.Unique_dataset_name].ContainsKey(onto_enrich_unfiltered_line.Scp_name)))
                    {
                        current_uniqueDataset_standard.Add(onto_enrich_unfiltered_line);
                    }
                }
                bool remove_genes_parent_SCPs_if_part_of_child_SCPs = Options.Add_parent_child_relationships(enrichmentType);
                string[] all_parentChild_scps = combined_scpNetwork.Get_all_scps();
                Network_class gene_scp_network = Generate_network_from_ontology_enrichment_lines_and_gene_colors_to_scpGeneLegend_color_dict(current_uniqueDataset_standard.ToArray(), ref scpGeneLegend_colorSpecification_dict, remove_genes_parent_SCPs_if_part_of_child_SCPs, all_parentChild_scps, minimum_data_scp_size);
                combined_scpNetwork.Merge_this_network_with_other_network(gene_scp_network);
            }


            string comleteFileName = complete_subdirectory + baseFile_name;

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

            #region Add legend
            string current_value_headline="";

            switch (Options.Node_size_determinant)
            {
                case Yed_network_node_size_determinant_enum.Uniform:
                    break;
                case Yed_network_node_size_determinant_enum.Minus_log10_pvalue:
                    //string value_legend_nameStart = "sum(-log10(p-values)) = ";
                    current_value_headline = "sum(-log10(p-values))";
                    string current_value_legendName;
                    string value_legend_nameStart = "";
                    string value_legend_nameEnd = "";
                    float max_nodeSize = max_referenceValue;
                    float min_nodeSize = min_referenceValue;
                    if (this.Options.Add_genes_to_networks(enrichmentType)) { current_value_headline += " (not for genes)"; }
                    //float current_nodeSize;
                    //scps = scpGeneLegend_colorSpecification_dict.Keys.ToArray();
                    //List<Color_specification_line_class> color_secification_lines;
                    //foreach (string scp in scps)
                    //{
                    //    color_secification_lines = scpGeneLegend_colorSpecification_dict[scp];
                    //    current_nodeSize = 0;
                    //    foreach (Color_specification_line_class local_color_specification_line in color_secification_lines)
                    //    {
                    //        current_nodeSize += local_color_specification_line.Size;
                    //    }
                    //    if (current_nodeSize > max_nodeSize)
                    //    {
                    //        max_nodeSize = current_nodeSize;
                    //    }
                    //}
                    int legend_values_length = 5;
                    float[] legend_values = new float[legend_values_length];
                    float legend_value;
                    float rounding_factor;
                    float[] rounding_factor_array = new float[] { 10, 1, 0.2F, 0.1F, 0.02F };
                    int indexRoundingFactor_array = -1;
                    if (max_nodeSize < 10) { indexRoundingFactor_array = 0; }
                    else if (max_nodeSize < 25) { indexRoundingFactor_array = 1; }
                    else if (max_nodeSize < 100) { indexRoundingFactor_array = 2; }
                    else if (max_nodeSize < 200) { indexRoundingFactor_array = 3; }
                    else { indexRoundingFactor_array = 4; }
                    rounding_factor = rounding_factor_array[indexRoundingFactor_array];
                    float distance_between_legend_values = (max_nodeSize - min_nodeSize) / (legend_values_length - 1);
                    distance_between_legend_values = (float)Math.Round(distance_between_legend_values * rounding_factor) / rounding_factor;
                    float ref_min_nodeSize = (float)Math.Round(min_nodeSize * rounding_factor) / rounding_factor;
                    float smallest_legend_nodeSize = ref_min_nodeSize;
                    int indexRoundingFactor_downscale = indexRoundingFactor_array;
                    while (smallest_legend_nodeSize == 0)
                    {
                        indexRoundingFactor_downscale--;
                        if (indexRoundingFactor_downscale >= 0)
                        {
                            rounding_factor = rounding_factor_array[indexRoundingFactor_downscale];
                            smallest_legend_nodeSize = (float)Math.Round(min_nodeSize * rounding_factor) / rounding_factor;
                        }
                        else
                        {
                            smallest_legend_nodeSize = 2;
                        }
                    }
                    if ((indexRoundingFactor_array==0)&&(smallest_legend_nodeSize<=1.3F)) { smallest_legend_nodeSize = 1.3F; }
                    else if ((indexRoundingFactor_array > 0) && (smallest_legend_nodeSize <= 1.3F)) { smallest_legend_nodeSize = 2F; }
                    for (int indexL = 0; indexL < legend_values_length; indexL++)
                    {
                        legend_value = ref_min_nodeSize + indexL * distance_between_legend_values;
                        if (legend_value == 0) { legend_value = smallest_legend_nodeSize; }
                        legend_values[indexL] = legend_value;
                    }


                    List<float> keep_legend_values = new List<float>();
                    for (int indexLegend2 = 0; indexLegend2 < legend_values_length; indexLegend2++)
                    {
                        legend_value = legend_values[indexLegend2];
                        if (legend_value>0) { keep_legend_values.Add(legend_value); }
                    }
                    legend_values = keep_legend_values.Distinct().ToArray();
                    legend_values_length = legend_values.Length;
                    for (int indexLegend = 0; indexLegend < 2; indexLegend++)
                    {
                        if (indexLegend == 0)
                        {
                            for (int indexLegend2 = 0; indexLegend2 < legend_values_length; indexLegend2++)
                            {
                                legend_value = legend_values[indexLegend2];
                                current_value_legendName = value_legend_nameStart + legend_value + value_legend_nameEnd;
                                scpGeneLegend_colorSpecification_dict.Add(current_value_legendName, new List<Color_specification_line_class>());
                                color_specification_line = new Color_specification_line_class();
                                color_specification_line.Fill_color = Global_class.Legend_node_size_color;
                                color_specification_line.Size = legend_value;
                                color_specification_line.Dataset_order_no = 0;
                                scpGeneLegend_colorSpecification_dict[current_value_legendName].Add(color_specification_line);
                                legend_node_level_dict.Add(current_value_legendName, Global_class.Network_legend_level);
                                combined_scpNetwork.Add_single_nodes(current_value_legendName);
                            }
                        }
                        else
                        {
                            for (int indexLegend2 = 0; indexLegend2 < legend_values_length; indexLegend2++)
                            {
                                legend_value = legend_values[indexLegend2];
                                current_value_legendName = value_legend_nameStart + legend_value + value_legend_nameEnd + " (Pie chart)";
                                scpGeneLegend_colorSpecification_dict.Add(current_value_legendName, new List<Color_specification_line_class>());
                                color_specification_line = new Color_specification_line_class();
                                color_specification_line.Fill_color = Global_class.Legend_node_size_color;
                                color_specification_line.Size = legend_value * 0.5F;
                                color_specification_line.Dataset_order_no = 0;
                                scpGeneLegend_colorSpecification_dict[current_value_legendName].Add(color_specification_line);
                                color_specification_line = new Color_specification_line_class();
                                color_specification_line.Fill_color = Global_class.Legend_node_size_color;
                                color_specification_line.Size = legend_value * 0.5F;
                                color_specification_line.Dataset_order_no = 1;
                                scpGeneLegend_colorSpecification_dict[current_value_legendName].Add(color_specification_line);
                                legend_node_level_dict.Add(current_value_legendName, Global_class.Network_legend_level);
                                combined_scpNetwork.Add_single_nodes(current_value_legendName);
                            }
                        }
                    }
                    break;
                case Yed_network_node_size_determinant_enum.No_of_different_colors:
                    current_value_headline = "node sizes ~ # different colors";
                    break;
                case Yed_network_node_size_determinant_enum.No_of_sets:
                    current_value_headline = "node sizes ~ # datasets";
                    break;
                default:
                    throw new Exception();
            }

            if (!String.IsNullOrEmpty(current_value_headline))
            {
                scpGeneLegend_colorSpecification_dict.Add(current_value_headline, new List<Color_specification_line_class>());
                color_specification_line = new Color_specification_line_class();
                color_specification_line.Fill_color = Global_class.Legend_node_size_color;
                color_specification_line.Size = Get_nodeSize_value(min_referenceValue);
                color_specification_line.Dataset_order_no = 0;
                scpGeneLegend_colorSpecification_dict[current_value_headline].Add(color_specification_line);
                legend_node_level_dict.Add(current_value_headline, Global_class.Network_legend_level);
                combined_scpNetwork.Add_single_nodes(current_value_headline);
            }

            #endregion

            combined_scpNetwork.Nodes.Set_level_for_all_nodes(Global_class.Network_genes_level);
            combined_scpNetwork.Nodes.Set_processLevel_for_all_nodes_based_on_dictionary(legend_node_level_dict);
            combined_scpNetwork.Nodes.Set_processLevel_for_all_nodes_based_on_dictionary(processName_processLevel_dict);
           // combined_scpNetwork.Keep_only_nodes_with_indicated_levels(new int[] { -1, 1, 2, 3, 4, Global_class.Network_genes_level, Global_class.Network_legend_level });

            bool group_scps_by_level = Options.Box_sameLevel_scps(enrichmentType);

            Yed_network_node_size_determinant_enum node_size_determinant = Options.Node_size_determinant;
            int max_node_diameter = Options.Node_size_diameterMax_for_current_nodeSize_determinant;
            int min_label_size = -1;
            int max_label_size = -1;
            switch (Options.Node_size_determinant)
            {
                case Yed_network_node_size_determinant_enum.Uniform:
                    min_label_size = Options.Label_uniqueSize_for_current_nodeSize_determinant;
                    max_label_size = min_label_size;
                    break;
                case Yed_network_node_size_determinant_enum.No_of_different_colors:
                case Yed_network_node_size_determinant_enum.Minus_log10_pvalue:
                case Yed_network_node_size_determinant_enum.No_of_sets:
                    if (Options.Adjust_labelSizes_to_nodeSizes)
                    {
                        min_label_size = current_min_font_size;
                        max_label_size = current_max_font_size;
                    }
                    else
                    {
                        min_label_size = Options.Label_uniqueSize_for_current_nodeSize_determinant;
                        max_label_size = min_label_size;
                    }
                    break;
                default:
                    throw new Exception();
            }
            bool nw_generation_interrupted = combined_scpNetwork.Write_yED_nw_in_results_directory_with_nodes_colored_by_set_and_return_if_interrupted(comleteFileName, legend_dataset_nodes.ToArray(), scpGeneLegend_colorSpecification_dict, group_scps_by_level, node_size_determinant, max_node_diameter, min_label_size, max_label_size, Options.Graph_editor, progressReport);
            if (!nw_generation_interrupted)
            {
                switch (Options.Graph_editor)
                {
                    case Graph_editor_enum.Cytoscape:
                        Write_readMe_file_for_cytoscape_networks(complete_subdirectory);
                        break;
                    case Graph_editor_enum.yED:
                        Write_readMe_file_for_yED_networks(complete_subdirectory);
                        break;
                    default:
                        throw new Exception();
                }
            }
            return nw_generation_interrupted;
        }
        #endregion
    }
}
