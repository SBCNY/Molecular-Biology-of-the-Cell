using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_functions.Array_own;
using Common_functions.Global_definitions;
using MBCO;
using Network;

namespace ClassLibrary1.Prepare_datasets
{
    class Prepare_NaGlu_TMT_dataset
    {
        public static void Prepare_na_glucose_tmt_ontology()
        {
            //Ontology_type_enum ontology = Ontology_type_enum.Mbco_na_glucose_tm_transport;
            //MBCO_association_class naGlu_association = new MBCO_association_class();
            //naGlu_association.Read_minimum_mbco_association(ontology, new Windows_forms.ProgressReport_interface_class());

            //Obo_networkTable_class naGlu_table = new Obo_networkTable_class();
           // naGlu_table.Read_minimum_networkTables(ontology, new Windows_forms.ProgressReport_interface_class());

            //foreach (Obo_networkTable_line_class obo_line in naGlu_table.NetworkTable)
            //{
            //    obo_line.Child_id = (string)obo_line.Child_name.Clone();
            //    obo_line.Parent_id = (string)obo_line.Parent_name.Clone();
            //}

            //MBCO_obo_network_class naGlu_obo_parent_child = new MBCO_obo_network_class(ontology, Enrichment.SCP_hierarchy_interaction_type_enum.Parent_child);
            //naGlu_obo_parent_child.Add_obo_networkTable(naGlu_table);

        //    MBCO_obo_network_class naGlu_obo_child_parent = naGlu_obo_parent_child.Deep_copy_mbco_obo_nw();
        //    naGlu_obo_child_parent.Transform_into_child_parent_direction();

        //    Dictionary<string,string[]> parentScps_childScps = naGlu_obo_parent_child.Get_sourceNodeName_targetNodeNames_dict();
        //    string[] final_parents = naGlu_obo_child_parent.Get_all_finalParents_leaves_if_child_parent();

        //    final_parents = final_parents.OrderBy(l => l).ToArray();
        //    string final_parent;
        //    int final_parents_length = final_parents.Length;
        //    Dictionary<string, string> scpName_scpID_dic = new Dictionary<string, string>();
        //    int scpID_no = 0;
        //    int scpID_no_nchar = 7;
        //    string scpID_no_string;
              
        //    List<string> nextParents = new List<string>();
        //    while (final_parents_length > 0)
        //    {
        //        nextParents.Clear();
        //        for (int indexFP=0; indexFP<final_parents_length;indexFP++)
        //        {
        //            final_parent = final_parents[indexFP];
        //            if (!scpName_scpID_dic.ContainsKey(final_parent))
        //            {
        //                nextParents.AddRange(naGlu_obo_parent_child.Get_all_children_if_direction_is_parent_child_without_ordering_nodes_by_index(final_parent));
        //                scpID_no++;
        //                scpID_no_string = scpID_no.ToString();
        //                while (scpID_no_string.Length < scpID_no_nchar)
        //                {
        //                    scpID_no_string = "0" + scpID_no_string;
        //                }
        //                scpName_scpID_dic.Add(final_parent, "MBCO_NaGlu_Id:" + scpID_no_string);
        //            }
        //        }
        //        final_parents = nextParents.Distinct().ToArray();
        //        final_parents_length = final_parents.Length;
        //    }

        //    int nwTable_length = naGlu_table.NetworkTable.Length;
        //    Obo_networkTable_line_class nwTable_line;
        //    for (int indexNWT=0; indexNWT<nwTable_length;indexNWT++)
        //    {
        //        nwTable_line = naGlu_table.NetworkTable[indexNWT];
        //        nwTable_line.Child_id = (string)scpName_scpID_dic[nwTable_line.Child_name].Clone();
        //        nwTable_line.Parent_id = (string)scpName_scpID_dic[nwTable_line.Parent_name].Clone();
        //    }

        //    int mbco_association_length = naGlu_association.MBCO_associations.Length;
        //    MBCO_association_line_class mbco_association_line;
        //    for (int indexMbco=0; indexMbco<mbco_association_length; indexMbco++)
        //    {
        //        mbco_association_line = naGlu_association.MBCO_associations[indexMbco];
        //        mbco_association_line.ProcessID = (string)scpName_scpID_dic[mbco_association_line.ProcessName].Clone();
        //    }

        //    MBCO_association_class mbco_association = new MBCO_association_class();
        //    //mbco_association.Generate_after_reading_safed_file(Ontology_type_enum.Mbco_human, new Windows_forms.ProgressReport_interface_class());
        //    List<MBCO_association_line_class> background_genes = new List<MBCO_association_line_class>();
        //    foreach (MBCO_association_line_class mbco_association_line2 in mbco_association.MBCO_associations)
        //    {
        //        if (mbco_association_line2.ProcessName.Equals(Ontology_classification_class.Background_genes_scp))
        //        {
        //            background_genes.Add(mbco_association_line2);
        //        }
        //    }
        //    //naGlu_table.W(ontology, new Windows_forms.ProgressReport_interface_class());
        //    naGlu_association.Add_to_array(background_genes.ToArray());
        //    //naGlu_association.Write_mbco_association(ontology, new Windows_forms.ProgressReport_interface_class());

        //}

        //public static void Prepare_energy_metabolism_ontology()
        //{
        //    Ontology_type_enum ontology = Ontology_type_enum.Mbco_energy_metabolism_human;
        //    MBCO_association_class energy_association = new MBCO_association_class();
        //    energy_association.Read_minimum_mbco_association(ontology);

        //    string[] scps = energy_association.Get_all_distinct_ordered_scps();

        //    Obo_networkTable_class energy_table = new Obo_networkTable_class();
        //    energy_table.Read_minimum_networkTables(ontology);

        //    foreach (Obo_networkTable_line_class obo_line in energy_table.NetworkTable)
        //    {
        //        obo_line.Id = (string)obo_line.Name.Clone();
        //        obo_line.Parent_id = (string)obo_line.Parent_name.Clone();
        //    }

        //    MBCO_obo_network_class energy_obo_parent_child = new MBCO_obo_network_class(ontology);
        //    energy_obo_parent_child.Add_obo_networkTable(energy_table);

        //    MBCO_obo_network_class energy_obo_child_parent = energy_obo_parent_child.Deep_copy_mbco_obo_nw();
        //    energy_obo_child_parent.Transform_into_child_parent_direction();
        //    string[] keep_scps = energy_obo_child_parent.Get_all_ancestors_if_direction_is_child_parent(scps);
        //    keep_scps = Overlap_class.Get_union(keep_scps, scps);
        //    energy_obo_child_parent.Keep_only_input_nodeNames(keep_scps);

        //    energy_obo_child_parent.Keep_only_input_nodeNames(keep_scps);

        //    energy_table.Keep_only_lines_with_parent_and_child_scp_part_of_input_scps(keep_scps);


        //    Dictionary<string, string[]> parentScps_childScps = energy_obo_parent_child.Get_sourceNodeName_targetNodeNames_dict();
        //    string[] final_parents = energy_obo_child_parent.Get_all_finalParents_leaves_if_child_parent();

        //    final_parents = final_parents.OrderBy(l => l).ToArray();
        //    string final_parent;
        //    int final_parents_length = final_parents.Length;
        //    Dictionary<string, string> scpName_scpID_dic = new Dictionary<string, string>();
        //    int scpID_no = 0;
        //    int scpID_no_nchar = 7;
        //    string scpID_no_string;

        //    List<string> nextParents = new List<string>();
        //    while (final_parents_length > 0)
        //    {
        //        nextParents.Clear();
        //        for (int indexFP = 0; indexFP < final_parents_length; indexFP++)
        //        {
        //            final_parent = final_parents[indexFP];
        //            if (!scpName_scpID_dic.ContainsKey(final_parent))
        //            {
        //                nextParents.AddRange(energy_obo_parent_child.Get_all_children_if_direction_is_parent_child(final_parent));
        //                scpID_no++;
        //                scpID_no_string = scpID_no.ToString();
        //                while (scpID_no_string.Length < scpID_no_nchar)
        //                {
        //                    scpID_no_string = "0" + scpID_no_string;
        //                }
        //                scpName_scpID_dic.Add(final_parent, "MBCO_EnergyMet_Id:" + scpID_no_string);
        //            }
        //        }
        //        final_parents = nextParents.Distinct().ToArray();
        //        final_parents_length = final_parents.Length;
        //    }

        //    int nwTable_length = energy_table.NetworkTable.Length;
        //    Obo_networkTable_line_class nwTable_line;
        //    for (int indexNWT = 0; indexNWT < nwTable_length; indexNWT++)
        //    {
        //        nwTable_line = energy_table.NetworkTable[indexNWT];
        //        nwTable_line.Id = (string)scpName_scpID_dic[nwTable_line.Name].Clone();
        //        nwTable_line.Parent_id = (string)scpName_scpID_dic[nwTable_line.Parent_name].Clone();
        //    }

        //    Dictionary<string, List<string>> childScp_parentScps_dict = new Dictionary<string, List<string>>();
        //    for (int indexNWT = 0; indexNWT < nwTable_length; indexNWT++)
        //    {
        //        nwTable_line = energy_table.NetworkTable[indexNWT];
        //        if (!childScp_parentScps_dict.ContainsKey(nwTable_line.Name))
        //        {
        //            childScp_parentScps_dict.Add(nwTable_line.Name, new List<string>());
        //        }
        //        childScp_parentScps_dict[nwTable_line.Name].Add(nwTable_line.Parent_name);
        //    }

        //    energy_association.Populate_parent_scps_with_genes_of_children_scps(energy_obo_parent_child);

        //    int mbco_association_length = energy_association.MBCO_associations.Length;
        //    MBCO_association_line_class mbco_association_line;
        //    string[] parentSCPs;
        //    string parentSCP;
        //    int parentSCPs_length;
        //    StringBuilder sb = new StringBuilder();
        //    for (int indexMbco = 0; indexMbco < mbco_association_length; indexMbco++)
        //    {
        //        mbco_association_line = energy_association.MBCO_associations[indexMbco];
        //        mbco_association_line.ProcessID = (string)scpName_scpID_dic[mbco_association_line.ProcessName].Clone();
        //        if (childScp_parentScps_dict.ContainsKey(mbco_association_line.ProcessName))
        //        {
        //            parentSCPs = childScp_parentScps_dict[mbco_association_line.ProcessName].Distinct().ToArray();
        //            parentSCPs_length = parentSCPs.Length;
        //            sb.Clear();
        //            for (int indexP = 0; indexP < parentSCPs_length; indexP++)
        //            {
        //                parentSCP = parentSCPs[indexP];
        //                if (indexP != 0) { sb.AppendFormat(";"); }
        //                sb.AppendFormat(parentSCP);
        //            }
        //            mbco_association_line.Parent_processName = sb.ToString();
        //        }
        //    }

        //    MBCO_association_class mbco_association = new MBCO_association_class();
        //    mbco_association.Generate_after_reading_safed_file(Ontology_type_enum.Mbco_human, new System.Windows.Forms.Label(), new Common_functions.Form_tools.Form1_default_settings_class());
        //    List<MBCO_association_line_class> background_genes = new List<MBCO_association_line_class>();
        //    foreach (MBCO_association_line_class mbco_association_line2 in mbco_association.MBCO_associations)
        //    {
        //        if (mbco_association_line2.ProcessName.Equals(Global_class.Background_genes_scpName))
        //        {
        //            background_genes.Add(mbco_association_line2);
        //        }
        //    }
        //    energy_table.Write_networkTables(ontology);
        //    energy_association.Add_to_array(background_genes.ToArray());
        //    energy_association.Write_mbco_association(ontology);

        }
    }
}
