#region Author information
/*
The code was written by Jens Hansen working for the Ravi Iyengar Lab
The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
Please acknowledge the MBC Ontology in your publications by citing the following reference:
Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar: 
A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes
Sci Rep. 2017 Dec 18;7(1):17689. doi: 10.1038/s41598-017-16627-4.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common_functions.Global_definitions;
using Common_functions.Report;
using Common_functions.Text;
using Common_functions.ReadWrite;

namespace Network
{
    enum NWedge_type_enum { E_m_p_t_y, Arrow, Dotted_line, Dashed_line };

    class NetworkTable_line_class
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public string Edge_label { get; set; }
        public float Width { get; set; }
        public NWedge_type_enum Edge_type { get; set; }

        public NetworkTable_line_class()
        {
            Width = 1;
            Edge_label = "";
            Edge_type = NWedge_type_enum.Arrow;
        }

        public NetworkTable_line_class Deep_copy()
        {
            NetworkTable_line_class copy = (NetworkTable_line_class)this.MemberwiseClone();
            copy.Source = (string)this.Source.Clone();
            copy.Target = (string)this.Target.Clone();
            copy.Edge_label = (string)this.Edge_label.Clone();
            return copy;
        }
    }

    class Obo_networkTable_line_class
    {
        #region Fields
        public string Id { get; set; }
        public string Name { get; set; }
        public string Parent_id { get; set; }
        public string Parent_name { get; set; }
        public Obo_interaction_type_enum Interaction_type { get; set; }
        #endregion

        public Obo_networkTable_line_class(string id, string name)
        {
            this.Id = (string)id.Clone();
            this.Name = (string)name.Clone();
            this.Parent_id = (string)Global_class.Empty_entry.Clone();
            this.Parent_name = (string)Global_class.Empty_entry.Clone();
        }

        #region Standard way
        public static Obo_networkTable_line_class[] Order_in_standard_way(Obo_networkTable_line_class[] networkTable)
        {
            networkTable = networkTable.OrderBy(l => l.Id).ThenBy(l => l.Name).ThenBy(l => l.Parent_id).ThenBy(l => l.Parent_name).ToArray();
            return networkTable;
        }

        public bool Equal_in_standard_way(Obo_networkTable_line_class other)
        {
            bool equal = ((this.Id.Equals(other.Id))
                          && (this.Parent_id.Equals(other.Parent_id)));
            return equal;
        }
        #endregion

        public Obo_networkTable_line_class Deep_copy()
        {
            Obo_networkTable_line_class copy = (Obo_networkTable_line_class)this.MemberwiseClone();
            copy.Id = (string)this.Id.Clone();
            copy.Name = (string)this.Name.Clone();
            copy.Parent_id = (string)this.Parent_id.Clone();
            copy.Parent_name = (string)this.Parent_name.Clone();
            return copy;
        }
    }

    class Obo_networkTable_private_readOptions_class
    {
        #region Fields
        public const string End_reading_marker = "[Typedef]";
        public const string Id_marker = "id";
        public const string Name_marker = "name";
        public const string Is_a_marker = "is_a";
        public const string Part_of_marker = "part_of";
        public const string Namespace_marker = "namespace";
        public const char Parent_id_parent_name_separator = '!';
        public const string New_term_marker = "[Term]";
        public const string Is_obsolete_marker = "is_obsolete";

        public string Complete_file_name { get; set; }
        #endregion

        public Obo_networkTable_private_readOptions_class()
        {
            Complete_file_name = Global_directory_and_file_class.Complete_mbco_obo_fileName;
        }
    }

    class Obo_networkTable_class
    {
        public Obo_networkTable_line_class[] NetworkTable { get; set; }

        public Obo_networkTable_class()
        {
            NetworkTable = new Obo_networkTable_line_class[0];
        }

        private void Remove_duplicates()
        {
            NetworkTable = Obo_networkTable_line_class.Order_in_standard_way(NetworkTable);
            int sigNW_length = NetworkTable.Length;
            Obo_networkTable_line_class obo_networkTable_line;
            List<Obo_networkTable_line_class> new_sigNW = new List<Obo_networkTable_line_class>();
            for (int indexSigNW = 0; indexSigNW < sigNW_length; indexSigNW++)
            {
                obo_networkTable_line = NetworkTable[indexSigNW];
                if ((indexSigNW == 0)
                    || (!obo_networkTable_line.Equal_in_standard_way(NetworkTable[indexSigNW - 1])))
                {
                    new_sigNW.Add(obo_networkTable_line);
                }
            }
        }

        private void Add_to_networkTable(Obo_networkTable_line_class[] add_networkTable)
        {
            int add_length = add_networkTable.Length;
            int this_length = this.NetworkTable.Length;
            int new_length = add_length + this_length;
            Obo_networkTable_line_class[] new_networkTable = new Obo_networkTable_line_class[new_length];
            int indexNew = -1;
            for (int indexThis = 0; indexThis < this_length; indexThis++)
            {
                indexNew++;
                new_networkTable[indexNew] = this.NetworkTable[indexThis].Deep_copy();
            }
            for (int indexAdd = 0; indexAdd < add_length; indexAdd++)
            {
                indexNew++;
                new_networkTable[indexNew] = add_networkTable[indexAdd].Deep_copy();
            }
            this.NetworkTable = new_networkTable;
        }

        private Obo_networkTable_line_class[] Read_obo_file()
        {
            Obo_networkTable_private_readOptions_class readOptions = new Obo_networkTable_private_readOptions_class();
            StreamReader reader = new StreamReader(readOptions.Complete_file_name);
            string input_line;
            bool new_term = false;
            string id = "error";
            string name = "error";
            string parent_id = "error";
            string parent_name = "error";

            string[] splitString;
            string[] parent_id_parent_name;
            Obo_interaction_type_enum interaction_type;
            StringBuilder sb;
            int splitString_length;
            bool continue_reading = true;
            bool new_parent = false;
            bool at_least_one_line_added = false;
            bool end_of_term = false;
            bool is_obsolete = false;

            List<Obo_networkTable_line_class> networkTable_list = new List<Obo_networkTable_line_class>();
            Obo_networkTable_line_class new_networkTable_line;
            Dictionary<string, bool> added_scps_dict = new Dictionary<string, bool>();
            while (((input_line = reader.ReadLine()) != null) && (continue_reading))
            {
                #region Get split stings
                splitString = input_line.Split(':');
                splitString_length = splitString.Length;
                if ((splitString_length > 2) && (!splitString[0].Equals("synonym")) && (!splitString[0].Equals("xref")) && (!splitString[0].Equals("def")))
                {
                    sb = new StringBuilder();
                    for (int indexS = 1; indexS < splitString_length; indexS++)
                    {
                        splitString[indexS] = splitString[indexS].Replace("{", "").Replace("}", "");
                        if (indexS > 1) { sb.AppendFormat(":"); }
                        sb.AppendFormat(splitString[indexS]);
                    }
                    splitString = new string[] { splitString[0], sb.ToString() };
                }
                #endregion

                #region Set every turn default values
                parent_id = Global_class.Empty_entry;
                parent_name = Global_class.Empty_entry;
                interaction_type = Obo_interaction_type_enum.E_m_p_t_y;
                new_parent = false;
                #endregion

                if (String.IsNullOrEmpty(splitString[0]))
                {
                    end_of_term = true;
                }
                else
                {
                    switch (splitString[0])
                    {
                        case Obo_networkTable_private_readOptions_class.New_term_marker:
                            new_term = true;
                            is_obsolete = false;
                            break;
                        case Obo_networkTable_private_readOptions_class.Id_marker:
                            id = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(splitString[1]);
                            break;
                        case Obo_networkTable_private_readOptions_class.Namespace_marker:
                            break;
                        case Obo_networkTable_private_readOptions_class.Name_marker:
                            name = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(splitString[1]);
                            break;
                        case Obo_networkTable_private_readOptions_class.Is_a_marker:
                            parent_id_parent_name = splitString[1].Split(Obo_networkTable_private_readOptions_class.Parent_id_parent_name_separator);
                            parent_id = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(parent_id_parent_name[0]);
                            if (parent_id_parent_name.Length == 2) { parent_name = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(parent_id_parent_name[1]); }
                            interaction_type = Obo_interaction_type_enum.Is_a;
                            new_parent = true;
                            break;
                        case Obo_networkTable_private_readOptions_class.Part_of_marker:
                            parent_id_parent_name = splitString[1].Split(Obo_networkTable_private_readOptions_class.Parent_id_parent_name_separator);
                            parent_id = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(parent_id_parent_name[0]);
                            parent_name = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(parent_id_parent_name[1]);
                            interaction_type = Obo_interaction_type_enum.Part_of;
                            new_parent = true;
                            break;
                        case Obo_networkTable_private_readOptions_class.End_reading_marker:
                            continue_reading = false;
                            break;
                        case Obo_networkTable_private_readOptions_class.Is_obsolete_marker:
                            is_obsolete = true;
                            break;
                        default:
                            break;
                    }
                }
                if (new_term)
                {
                    new_term = false;
                    id = (string)Global_class.Empty_entry.Clone();
                    name = (string)Global_class.Empty_entry.Clone();
                    at_least_one_line_added = false;
                    end_of_term = false;
                }
                else if ((new_parent) && (!is_obsolete))
                {
                    new_networkTable_line = new Obo_networkTable_line_class(id, name);
                    new_networkTable_line.Parent_id = (string)parent_id.Clone();
                    new_networkTable_line.Parent_name = (string)parent_name.Clone();
                    new_networkTable_line.Interaction_type = interaction_type;
                    networkTable_list.Add(new_networkTable_line);
                    at_least_one_line_added = true;
                }
                else if ((end_of_term) && (!at_least_one_line_added) && (!is_obsolete))
                {
                    if (!added_scps_dict.ContainsKey(name))
                    {
                        new_networkTable_line = new Obo_networkTable_line_class(id, name);
                        networkTable_list.Add(new_networkTable_line);
                        at_least_one_line_added = true;
                    }
                    else
                    {
                        throw new Exception("Process name already exists");   
                    }
                }
            }
            return networkTable_list.ToArray();
        }

        public void Generate_for_mbco_by_reading_obo_file()
        {
            Obo_networkTable_line_class[] add_sigNW = Read_obo_file();
            Add_to_networkTable(add_sigNW);
            Remove_duplicates();
        }

        public Obo_networkTable_class Deep_copy()
        {
            Obo_networkTable_class copy = (Obo_networkTable_class)this.MemberwiseClone();
            int nw_length = this.NetworkTable.Length;
            copy.NetworkTable = new Obo_networkTable_line_class[nw_length];
            for (int indexSigNW = 0; indexSigNW < nw_length; indexSigNW++)
            {
                copy.NetworkTable[indexSigNW] = this.NetworkTable[indexSigNW].Deep_copy();
            }
            return copy;
        }
    }
}
