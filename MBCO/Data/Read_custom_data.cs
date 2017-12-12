#region Author information
/*
The code was written by Jens Hansen working for the Ravi Iyengar Lab
The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
Please acknowledge the MBC Ontology in your publications by citing the following reference:
Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar: 
A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes
Sci Rep. 2017 Dec18th
*/
#endregion

using System;
using System.Linq;
using Common_functions.Global_definitions;
using Common_functions.ReadWrite;


namespace Data
{
    class Custom_data_line_class : IAdd_to_data
    {
        public Entry_type_enum EntryType { get; set; }
        public int Timepoint { get; set; }
        public string SampleName { get; set; }
        public string NCBI_official_symbol { get; set; }
        public float Value { get; set; }

        public string NCBI_official_symbol_for_data { get { return NCBI_official_symbol; } }
        public Entry_type_enum EntryType_for_data { get { return EntryType; } }
        public int Timepoint_for_data { get { return Timepoint; } }
        public string SampleName_for_data { get { return SampleName; } }
        public float Value_for_data { get { return Value; } }

        public Custom_data_line_class Deep_copy()
        {
            Custom_data_line_class copy = (Custom_data_line_class)this.MemberwiseClone();
            copy.SampleName = (string)this.SampleName.Clone();
            return copy;
        }
    }

    class Custom_data_readWriteOptions_class : ReadWriteOptions_base
    {
        public Custom_data_readWriteOptions_class(string file_name)
        {
            string directory = Global_directory_and_file_class.Custom_data_directory;
            this.File = directory + file_name;
            Key_propertyNames = new string[] { "EntryType", "Timepoint", "SampleName", "NCBI_official_symbol", "Value" };
            Key_columnNames = Key_propertyNames;
            HeadlineDelimiters = new char[] { Global_class.Tab };
            LineDelimiters = new char[] { Global_class.Tab };
            File_has_headline = true;
            Report = ReadWrite_report_enum.Report_nothing;
        }
    }

    class Custom_data_class
    {
        public Custom_data_line_class[] Custom_data { get; set; }

        private void Check_for_duplicates()
        {
            int custom_data_length = Custom_data.Length;
            Custom_data_line_class custom_data_line;
            Custom_data_line_class previous_custom_data_line;
            this.Custom_data = this.Custom_data.OrderBy(l => l.SampleName).ThenBy(l => l.EntryType).ThenBy(l => l.Timepoint).ThenBy(l => l.NCBI_official_symbol).ToArray();
            for (int indexC=1; indexC<custom_data_length;indexC++)
            {
                custom_data_line = this.Custom_data[indexC];
                previous_custom_data_line = this.Custom_data[indexC - 1];
                if (  (custom_data_line.SampleName.Equals(previous_custom_data_line.SampleName))
                    && (custom_data_line.EntryType.Equals(previous_custom_data_line.EntryType))
                    && (custom_data_line.Timepoint.Equals(previous_custom_data_line.Timepoint))
                    && (custom_data_line.NCBI_official_symbol.Equals(previous_custom_data_line.NCBI_official_symbol)))
                {
                    throw new Exception();
                }
            }
        }

        public void Generate_custom_data_instance(Custom_data_readWriteOptions_class readWriteOptions)
        {
            Read(readWriteOptions);
            Check_for_duplicates();
        }

        public Data_class Generate_new_data_instance()
        {
            Check_for_duplicates();
            Data_class data = new Data_class();
            data.Add_to_data_instance(this.Custom_data);
            return data;
        }

        private void Read(Custom_data_readWriteOptions_class readWriteOptions)
        {
            this.Custom_data = ReadWriteClass.ReadRawData_and_FillArray<Custom_data_line_class>(readWriteOptions);
        }
    }
}
