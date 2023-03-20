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
using Common_functions.Global_definitions;
using Common_functions.Text;
using Common_functions.Form_tools;
using Common_functions.ReadWrite;

namespace Network
{
    enum NWedge_type_enum { E_m_p_t_y, Arrow, Thick_dotted_line, Dotted_line, Dashed_line, Line };

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
        public int Level { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Parent_id { get; set; }
        public string Parent_name { get; set; }
        public Obo_interaction_type_enum Interaction_type { get; set; }
        public Namespace_type_enum Namespace_type { get; set; }
        #endregion

        public Obo_networkTable_line_class()
        {
            this.Id = "";
            this.Name = "";
            this.Level = -1;
            this.Parent_id = (string)Global_class.Empty_entry.Clone();
            this.Parent_name = (string)Global_class.Empty_entry.Clone();
        }
        public Obo_networkTable_line_class(string id, string name, int level) : this()
        {
            this.Id = (string)id.Clone();
            this.Name = (string)name.Clone();
            this.Level = level;
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

    class Obo_networkTable_oboFile_readOptions_class
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
        public const string Level_marker = "level";

        public string Complete_file_name { get; set; }
        #endregion

        public Obo_networkTable_oboFile_readOptions_class(Ontology_type_enum ontology)
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            switch (ontology)
            {
                case Ontology_type_enum.Mbco_human:
                case Ontology_type_enum.Mbco_mouse:
                case Ontology_type_enum.Mbco_rat:
                    Complete_file_name = global_dirFile.Complete_mbco_v11_obo_fileName;
                    break;
                case Ontology_type_enum.Go_bp_human:
                case Ontology_type_enum.Go_mf_human:
                case Ontology_type_enum.Go_cc_human:
                    Complete_file_name = global_dirFile.Complete_go_obo_fileName;
                    break;
                default:
                    throw new Exception();
            }
        }
    }

    class Obo_networkTable_readOptions_class : ReadWriteOptions_base
    {
        public Obo_networkTable_readOptions_class(Ontology_type_enum ontology)
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            switch (ontology)
            {
                case Ontology_type_enum.Mbco_na_glucose_tm_transport_human:
                    this.File = global_dirFile.Complete_na_glucose_tm_transport_parentChild_fileName;
                    break;
                default:
                    throw new Exception();
            }
            this.Key_propertyNames = new string[] { "Id", "Name", "Parent_id", "Parent_name" };
            this.Key_columnNames = new string[] { "Child_id", "Child_scp", "Parent_id", "Parent_scp" };
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.File_has_headline = true;
            this.Report = ReadWrite_report_enum.Report_nothing;
        }
    }
    class Obo_networkTable_minimum_readOptions_class : ReadWriteOptions_base
    {
        public Obo_networkTable_minimum_readOptions_class(Ontology_type_enum ontology)
        {
            Global_directory_and_file_class global_dirFile = new Global_directory_and_file_class();
            switch (ontology)
            {
                case Ontology_type_enum.Mbco_na_glucose_tm_transport_human:
                    this.File = global_dirFile.Complete_na_glucose_tm_transport_parentChild_minimum_fileName;
                    break;
                default:
                    throw new Exception();
            }
            this.Key_propertyNames = new string[] { "Name", "Parent_name" };
            this.Key_columnNames = new string[] { "Child_scp", "Parent_scp" };
            this.HeadlineDelimiters = new char[] { Global_class.Tab };
            this.LineDelimiters = new char[] { Global_class.Tab };
            this.File_has_headline = true;
            this.Report = ReadWrite_report_enum.Report_nothing;
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

        private Obo_networkTable_line_class[] Read_obo_file(Ontology_type_enum ontology, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            Obo_networkTable_oboFile_readOptions_class readOptions = new Obo_networkTable_oboFile_readOptions_class(ontology);
            StreamReader reader = Common_functions.ReadWrite.ReadWriteClass.Get_new_stream_reader_and_sent_notice_if_file_in_use(readOptions.Complete_file_name, error_report_label, form_default_settings);

            string input_line;
            bool new_term = false;
            bool inside_term = false;
            string id = "error";
            string name = "error";
            string parent_id = "error";
            string parent_name = "error";
            Namespace_type_enum namespace_type = Namespace_type_enum.E_m_p_t_y;

            string[] splitStrings;
            string[] parent_id_parent_name;
            Obo_interaction_type_enum interaction_type;
            StringBuilder sb;
            int splitString_length;
            bool continue_reading = true;
            bool new_parent = false;
            bool at_least_one_line_added = false;
            bool end_of_term = false;
            bool is_obsolete = false;
            int level = -1;
            List<Obo_networkTable_line_class> networkTable_list = new List<Obo_networkTable_line_class>();
            Obo_networkTable_line_class new_networkTable_line;
            Dictionary<string, bool> added_scps_dict = new Dictionary<string, bool>();
            string[] second_splitStrings;
            string second_splitString;
            int second_splitStrings_length;
            StringBuilder sb_splitStringTwo = new StringBuilder();
            int parent_childRelationShips_count = 0;
            int part_of_relationships_count = 0;
            int is_a_relationships_count = 0;
            bool part_of_in_input_line_recognized_by_alogorithm = false;
            bool is_a_in_input_line_recognized_by_alogorithm = false;
            List<string> missed_part_ofs_list = new List<string>();
            List<string> missed_is_list = new List<string>();
            List<Obo_networkTable_line_class> lines_without_parents = new List<Obo_networkTable_line_class>();
            while (((input_line = reader.ReadLine()) != null) && (continue_reading))
            {
                is_a_in_input_line_recognized_by_alogorithm = input_line.IndexOf(Obo_networkTable_oboFile_readOptions_class.Is_a_marker) != -1;
                part_of_in_input_line_recognized_by_alogorithm = input_line.IndexOf(Obo_networkTable_oboFile_readOptions_class.Part_of_marker) != -1;

                #region Get split stings
                splitStrings = input_line.Split(':');
                splitString_length = splitStrings.Length;
                if ((splitString_length > 2) && (!splitStrings[0].Equals("synonym")) && (!splitStrings[0].Equals("xref")) && (!splitStrings[0].Equals("def")))
                {
                    sb = new StringBuilder();
                    for (int indexS = 1; indexS < splitString_length; indexS++)
                    {
                        splitStrings[indexS] = splitStrings[indexS].Replace("{", "").Replace("}", "");
                        if (indexS > 1) { sb.AppendFormat(":"); }
                        sb.AppendFormat(splitStrings[indexS]);
                    }
                    splitStrings = new string[] { splitStrings[0], sb.ToString() };
                }
                if (splitStrings[0].Equals("relationship"))
                {
                    second_splitStrings = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(splitStrings[1]).Split(' ');
                    splitStrings[0] = second_splitStrings[0];
                    sb_splitStringTwo.Clear();
                    second_splitStrings_length = second_splitStrings.Length;
                    for (int indexSecondSplit=1;indexSecondSplit < second_splitStrings_length; indexSecondSplit++)
                    {
                        second_splitString = second_splitStrings[indexSecondSplit];
                        if (indexSecondSplit>1) { sb_splitStringTwo.AppendFormat(" "); }
                        sb_splitStringTwo.AppendFormat(second_splitString);
                    }
                    splitStrings[1] = sb_splitStringTwo.ToString();
                }
                #endregion

                #region Set every turn default values
                parent_id = Global_class.Empty_entry;
                parent_name = Global_class.Empty_entry;
                interaction_type = Obo_interaction_type_enum.E_m_p_t_y;
                new_parent = false;
                #endregion

                if ((String.IsNullOrEmpty(splitStrings[0]))&&(inside_term))
                {
                    end_of_term = true;
                    inside_term = false;
                }
                else
                {
                    switch (splitStrings[0])
                    {
                        case Obo_networkTable_oboFile_readOptions_class.New_term_marker:
                            new_term = true;
                            is_obsolete = false;
                            break;
                        case Obo_networkTable_oboFile_readOptions_class.Id_marker:
                            id = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(splitStrings[1]);
                            break;
                        case Obo_networkTable_oboFile_readOptions_class.Namespace_marker:
                            namespace_type = (Namespace_type_enum)Enum.Parse(typeof(Namespace_type_enum),Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(splitStrings[1]));
                            break;
                        case Obo_networkTable_oboFile_readOptions_class.Name_marker:
                            name = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(splitStrings[1]);
                            break;
                        case Obo_networkTable_oboFile_readOptions_class.Is_a_marker:
                            parent_id_parent_name = splitStrings[1].Split(Obo_networkTable_oboFile_readOptions_class.Parent_id_parent_name_separator);
                            parent_id = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(parent_id_parent_name[0]);
                            if (parent_id_parent_name.Length == 2) { parent_name = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(parent_id_parent_name[1]); }
                            interaction_type = Obo_interaction_type_enum.Is_a;
                            new_parent = true;
                            break;
                        case Obo_networkTable_oboFile_readOptions_class.Part_of_marker:
                            parent_id_parent_name = splitStrings[1].Split(Obo_networkTable_oboFile_readOptions_class.Parent_id_parent_name_separator);
                            parent_id = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(parent_id_parent_name[0]);
                            parent_name = Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(parent_id_parent_name[1]);
                            interaction_type = Obo_interaction_type_enum.Part_of;
                            new_parent = true;
                            break;
                        case Obo_networkTable_oboFile_readOptions_class.End_reading_marker:
                            continue_reading = false;
                            break;
                        case Obo_networkTable_oboFile_readOptions_class.Is_obsolete_marker:
                            is_obsolete = true;
                            break;
                        case Obo_networkTable_oboFile_readOptions_class.Level_marker:
                            level = int.Parse(splitStrings[1]);
                            break;
                        default:
                            break;
                    }
                }
                if (new_term)
                {
                    inside_term = true;
                    new_term = false;
                    id = (string)Global_class.Empty_entry.Clone();
                    name = (string)Global_class.Empty_entry.Clone();
                    namespace_type = Namespace_type_enum.E_m_p_t_y;
                    at_least_one_line_added = false;
                    end_of_term = false;
                    level = -1;
                }
                else if ((new_parent) && (!is_obsolete) && (inside_term))
                {
                    new_networkTable_line = new Obo_networkTable_line_class(id, name, level);
                    new_networkTable_line.Parent_id = (string)parent_id.Clone();
                    new_networkTable_line.Parent_name = (string)parent_name.Clone();
                    new_networkTable_line.Interaction_type = interaction_type;
                    new_networkTable_line.Namespace_type = namespace_type;
                    networkTable_list.Add(new_networkTable_line);
                    at_least_one_line_added = true;
                    parent_childRelationShips_count++;
                    switch (interaction_type)
                    {
                        case Obo_interaction_type_enum.Is_a:
                            is_a_relationships_count++;
                            is_a_in_input_line_recognized_by_alogorithm = false;
                            break;
                        case Obo_interaction_type_enum.Part_of:
                            part_of_relationships_count++;
                            part_of_in_input_line_recognized_by_alogorithm = false;
                            break;
                        default:
                            throw new Exception();
                    }
                }
                else if ((end_of_term) && (!at_least_one_line_added) && (!is_obsolete))
                {
                    if (!added_scps_dict.ContainsKey(name))
                    {
                        new_networkTable_line = new Obo_networkTable_line_class(id, name, level);
                        new_networkTable_line.Namespace_type = namespace_type;
                        networkTable_list.Add(new_networkTable_line);
                        at_least_one_line_added = true;
                        lines_without_parents.Add(new_networkTable_line);
                    }
                    else
                    {
                        throw new Exception("Process name already exists");   
                    }
                }
                if (is_a_in_input_line_recognized_by_alogorithm)
                {
                    //missed_is_list.Add(input_line);
                }
                if (part_of_in_input_line_recognized_by_alogorithm)
                {
                    //missed_part_ofs_list.Add(input_line);
                }
            }
            reader.Close();
            return networkTable_list.ToArray();
        }

        private Obo_networkTable_line_class[] Read_parentChild_networkTable_lines(Ontology_type_enum ontology, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            Obo_networkTable_readOptions_class readWriteOptions = new Obo_networkTable_readOptions_class(ontology);
            Obo_networkTable_line_class[] add_nwTable = ReadWriteClass.Read_data_fill_array_and_complain_if_error_message<Obo_networkTable_line_class>(readWriteOptions, error_report_label, form_default_settings);
            return add_nwTable;
        }

        public void Generate_for_mbco_by_reading_obo_file(Ontology_type_enum ontology, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            Obo_networkTable_line_class[] add_sigNW;
            switch (ontology)
            {
                case Ontology_type_enum.Mbco_human:
                case Ontology_type_enum.Mbco_mouse:
                case Ontology_type_enum.Mbco_rat:
                case Ontology_type_enum.Go_cc_human:
                case Ontology_type_enum.Go_bp_human:
                case Ontology_type_enum.Go_mf_human:
                    add_sigNW = Read_obo_file(ontology, error_report_label, form_default_settings);
                    Add_to_networkTable(add_sigNW);
                    break;
                case Ontology_type_enum.Mbco_na_glucose_tm_transport_human:
                    add_sigNW = Read_parentChild_networkTable_lines(ontology, error_report_label, form_default_settings);
                    Add_to_networkTable(add_sigNW);
                    break;
                default:
                    throw new Exception();
            }
            Remove_duplicates();
        }

        public void Read_minimum_networkTables(Ontology_type_enum ontology)
        {
            Obo_networkTable_minimum_readOptions_class readOptions = new Obo_networkTable_minimum_readOptions_class(ontology);
            this.NetworkTable = ReadWriteClass.Read_data_fill_array_and_complain_if_error_message<Obo_networkTable_line_class>(readOptions, new System.Windows.Forms.Label(), new Form1_default_settings_class());
        }
        public void Write_networkTables(Ontology_type_enum ontology)
        {
            Obo_networkTable_readOptions_class readOptions = new Obo_networkTable_readOptions_class(ontology);
            ReadWriteClass.WriteData(this.NetworkTable,readOptions, new System.Windows.Forms.Label(), new Form1_default_settings_class());
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
