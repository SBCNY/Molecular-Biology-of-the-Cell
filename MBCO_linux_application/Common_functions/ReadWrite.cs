//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Text;
using Common_functions.Text;
using Common_functions.Global_definitions;
using Common_functions.Form_tools;

namespace Common_functions.ReadWrite
{

    enum Read_file_delimiter_enum {  E_m_p_t_y, Tab, Comma, Semicolon, Space }

    class ReadWrite_settings_class
    {
        public static char Get_delimiter_char_from_enum(Read_file_delimiter_enum delimiter)
        {
            switch (delimiter)
            {
                case Read_file_delimiter_enum.Tab:
                    return '\t';
                case Read_file_delimiter_enum.Comma:
                    return ',';
                case Read_file_delimiter_enum.Semicolon:
                    return ';';
                case Read_file_delimiter_enum.Space:
                    return ' ';
                default:
                    throw new Exception();
            }
        }
    }

    enum Read_error_message_enum { E_m_p_t_y, Not_an_accepted_color, Not_an_integer, No_error, Not_a_float_or_double, Not_part_of_enum, Delimiter_seems_wrong, Length_column_entries_not_equal_length_column_names, Timeunit_not_recognized, File_does_not_exist, File_does_not_contain_text, Directory_does_not_exist, Invalid_spelling_of_directory_or_file_name, Defined_columnNames_are_missing, Duplicated_bggenes_dataset, Parameter_file_read, BgGenes_file_read, File_name_already_uploaded, Wrong_ending_of_directoryFileName, Maximum_number_of_datasets_exceeded, Reading_of_file_stopped, Duplicated_entry, All_values_in_column_specified_for_1st_value_are_zero, Multiple_color_assignments_for_dataset, Multiple_integration_group_assignments_for_dataset, Custom_data_array_too_long }
    enum Read_file_type { E_m_p_t_y, Background_genes, Data, Parameter_settings }

    class Read_error_message_line_class
    {
        public string Complete_fileName { get; set; }
        public string ColumnName { get; set; }
        public int LineIndex { get; set; }
        public string Value { get; set; }
        public Read_file_type File_type { get; set; }
        public Read_error_message_enum Error_message { get; set; }
    }

    public abstract class ReadWriteOptions_base
    {
        #region Fields
        public string File { get; set; }
        public string[] Key_columnNames { get; set; }
        public string[] Key_propertyNames { get; set; }
        public int[] Key_columnIndexes { get; set; }
        public string[] SafeCondition_columnNames { get; set; }
        public int[] SafeCondition_columnIndexes { get; set; }
        public string[] SafeCondition_entries { get; set; }
        public bool File_has_headline { get; set; }
        public bool Remove_not_existing_columnNames_and_corresponding_properties { get; set; }
        public string[] RemoveFromHeadline { get; set; }
        public char[] LineDelimiters { get; set; }
        public char[] HeadlineDelimiters { get; set; }
        public int Skip_lines { get; set; }
        public int Empty_integer_value { get; set; }
        public string Empty_string_value { get; set; }
        public string[] Invalid_line_defining_columnNames { get; set; }
        public ReadWrite_report_enum Report { get; set; }
        public bool Report_unhandled_null_entries { get; set; }
        public bool Read_only { get; set; }
        public static string Save_file_in_use_warning_text_start { get { return "File cannot be saved, please stop using:\r\n"; } }
        public static string Read_file_in_use_warning_text_start { get { return "File cannot be read, please stop using:\r\n"; } }
        public static string File_does_not_exist_in_directory { get { return "File does not exist in directory: "; } }
        public static string Directory_does_not_exist { get { return "Directory does not exist: "; } }
        public int Max_error_messages { get; set; }
        #endregion

        public ReadWriteOptions_base()
        {
            Report_unhandled_null_entries = true;
            Invalid_line_defining_columnNames = new string[0];
            Empty_string_value = "";
            Read_only = false;
            Max_error_messages = 50;
            Remove_not_existing_columnNames_and_corresponding_properties = false;
        }

        public int Get_index_of_propertyName(string propertyName)
        {
            int propertyNames_of_interest_length = this.Key_propertyNames.Length;

            for (int indexPropertyName = 0; indexPropertyName < propertyNames_of_interest_length; indexPropertyName++)
            {
                if (propertyName.Equals(this.Key_propertyNames[indexPropertyName]))
                {
                    return indexPropertyName;
                }
            }
            return -1;
        }
    }

    class ReadWriteClass
    {

        #region Column and property indexes
        public static string[] Get_and_modify_columnNames(string headline, ReadWriteOptions_base Options)
        {
            List<string> columnNamesList = new List<string>();
            columnNamesList.AddRange(headline.Split(Options.HeadlineDelimiters));
            if (Options.RemoveFromHeadline != null)
            {
                int removeFromHeadline_length = Options.RemoveFromHeadline.Length;
                for (int i = 0; i < removeFromHeadline_length; i++)
                {
                    columnNamesList.Remove(Options.RemoveFromHeadline[i]);
                }
            }

            string[] columnNames = columnNamesList.ToArray();
            return columnNames;
        }
        public static int[] Get_columnIndexes_of_given_columnNames<T>(string[] columnNames, params string[] given_columnNames)
        {
            int given_length = given_columnNames.Length;
            if (given_length == 0)
            {
                //Report_class.Write_error_line("{0}: no columnNames to search for", typeof(T).Name);
                throw new Exception();
            }
            int[] columnIndexes = new int[given_length];
            for (int i = 0; i < given_length; i++)
            {
                int index = Array.IndexOf(columnNames, given_columnNames[i]);
                if (index >= 0) { columnIndexes[i] = index; }
                else
                {
                    //Report_class.Write_error_line("{0}: columnName \"{1}\" does not exist", typeof(T).Name, given_columnNames[i]);
                    throw new Exception();
                }
            }
            return columnIndexes;
        }
        public static int[] Get_propertyIndexes_of_corresponding_given_columnNames<T>(PropertyInfo[] propInfo, string[] propertyNames, string[] given_columnNames, string[] search_given_columnNames)
        {
            int search_length = search_given_columnNames.Length;
            int[] columnNames_indexes = new int[search_length];
            if (search_length == 0)
            {
                //Report_class.Write_error_line("{0}: no search columnNames to search for", typeof(T).Name);
                throw new Exception();
            }
            for (int i = 0; i < search_length; i++)
            {
                int index = Array.IndexOf(given_columnNames, search_given_columnNames[i]);
                if (index >= 0) { columnNames_indexes[i] = index; }
                else
                {
                    //Report_class.Write_error_line("{0}: given_columnName \"{1}\" does not exist", typeof(T).Name, given_columnNames[i]);
                    throw new Exception();
                }
            }
            string[] corresponding_propertyNames = new string[search_length];
            for (int indexS = 0; indexS < search_length; indexS++)
            {
                corresponding_propertyNames[indexS] = propertyNames[columnNames_indexes[indexS]];
            }
            int[] propertyIndexes = Get_propertyIndexes<T>(propInfo, corresponding_propertyNames);
            return propertyIndexes;
        }
        public static int[] Get_propertyIndexes<T>(PropertyInfo[] propInfo, string[] key_propertyNames)
        {
            int key_length = key_propertyNames.Length;
            int[] propertyIndexes = new int[key_length];
            string[] propInfo_names = new string[propInfo.Length];

            for (int i = 0; i < propInfo.Length; i++)
            {
                propInfo_names[i] = propInfo[i].Name;
            }

            for (int i = 0; i < key_length; i++)
            {
                int index = Array.IndexOf(propInfo_names, key_propertyNames[i]);
                if (index >= 0) { propertyIndexes[i] = index; }
                if (index < 0)
                {
                    string error = "propertyName :" +  typeof(T).Name + key_propertyNames[i];
                    throw new Exception();
                }
            }
            return propertyIndexes;
        }
        #endregion

        #region Read data
        public static StreamReader Get_new_stream_reader_and_sent_notice_if_file_in_use(string complete_fileName, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            string report_label_text = (string)error_report_label.Text.Clone();
            bool report_label_visible = error_report_label.Visible;
            int report_label_x_location = error_report_label.Location.X;
            bool file_opened = false;
            StreamReader reader = null;
            string directory = Path.GetDirectoryName(complete_fileName);
            if (!Directory.Exists(directory))
            {
                error_report_label.Text = ReadWriteOptions_base.Directory_does_not_exist + directory;
                form_default_settings.LabelProgressReport_set_sizes_and_fontSize(error_report_label,0);
                error_report_label.Visible = true;
                error_report_label.Refresh();
                System.Threading.Thread.Sleep(5000);
            }
            else if (!File.Exists(complete_fileName))
            {
                error_report_label.Text = ReadWriteOptions_base.File_does_not_exist_in_directory + Path.GetFileName(complete_fileName);
                form_default_settings.LabelProgressReport_set_sizes_and_fontSize(error_report_label, 0);
                error_report_label.Visible = true;
                error_report_label.Refresh();
                System.Threading.Thread.Sleep(5000);
            }
            else
            { 
                while (!file_opened)
                {
                    try
                    {
                        reader = new StreamReader(complete_fileName);
                        file_opened = true;
                    }
                    catch
                    {
                        error_report_label.Text = ReadWriteOptions_base.Read_file_in_use_warning_text_start + complete_fileName;
                        form_default_settings.LabelProgressReport_set_sizes_and_fontSize(error_report_label,0);
                        error_report_label.Visible = true;
                        error_report_label.Refresh();
                    }
                }
            }
            error_report_label.Visible = report_label_visible;
            error_report_label.Text = (string)report_label_text.Clone();
            error_report_label.Location = new System.Drawing.Point(report_label_x_location, error_report_label.Location.Y);
            return reader;
        }
        public static bool Analyze_if_file_contains_characters(ReadWriteOptions_base options, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            StreamReader reader = Get_new_stream_reader_and_sent_notice_if_file_in_use(options.File, error_report_label, form_default_settings);
            string headline = reader.ReadLine();
            reader.Close();
            return !String.IsNullOrEmpty(headline);
        }
        private static Read_error_message_line_class[] Analzye_if_all_columnNames_exist_and_return_likely_error_message(ReadWriteOptions_base options, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings, string completeFileName)
        {
            Read_error_message_line_class new_error_message_line;
            List<Read_error_message_line_class> new_error_messages = new List<Read_error_message_line_class>();
            StreamReader reader = Get_new_stream_reader_and_sent_notice_if_file_in_use(options.File, error_report_label, form_default_settings);
            string headline = reader.ReadLine();
            reader.Close();
            string[] columnNames = headline.Split(options.HeadlineDelimiters);
            StringBuilder sb_missing_columnNames = new StringBuilder();
            int key_columnNames_length = options.Key_columnNames.Length;
            bool all_columnNames_exist = true;
            for (int indexKey=0; indexKey<key_columnNames_length;indexKey++)
            {
                if (!columnNames.Contains(options.Key_columnNames[indexKey]))
                {
                    all_columnNames_exist = false;
                    if (sb_missing_columnNames.Length>0) { sb_missing_columnNames.AppendFormat(", "); }
                    sb_missing_columnNames.AppendFormat(options.Key_columnNames[indexKey]);
                }
            }
            if (!all_columnNames_exist)
            {
                if (columnNames.Length==1)
                {
                    new_error_message_line = new Read_error_message_line_class();
                    new_error_message_line.File_type = Read_file_type.Data;
                    new_error_message_line.Error_message = Read_error_message_enum.Delimiter_seems_wrong;
                    new_error_message_line.Complete_fileName = (string)completeFileName.Clone();
                    new_error_message_line.Value = sb_missing_columnNames.ToString();
                    new_error_messages.Add(new_error_message_line);
                }
                else
                {
                    new_error_message_line = new Read_error_message_line_class();
                    new_error_message_line.File_type = Read_file_type.Data;
                    new_error_message_line.Error_message = Read_error_message_enum.Defined_columnNames_are_missing;
                    new_error_message_line.Complete_fileName = (string)completeFileName.Clone();
                    new_error_message_line.Value = sb_missing_columnNames.ToString();
                    new_error_messages.Add(new_error_message_line);
                }
            }
            return new_error_messages.ToArray();
        }

        private static string Get_next_accepted_inputLine(StreamReader reader)
        {
            string inputLine="";
            bool readNextLine = true;
            while (readNextLine)
            {
                inputLine = reader.ReadLine();
                if (  (inputLine==null)
                    ||(   (inputLine.Length > 0)
                       && (!inputLine[0].Equals('!'))))
                {
                    readNextLine=false;
                }
            }
            return inputLine;
        }
        public static T[] Read_data_fill_array_and_return_error_messages<T>(out Read_error_message_line_class[] error_messages, ReadWriteOptions_base options, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings) where T : class
        {
            Read_error_message_line_class new_error_message_line;
            Read_error_message_line_class[] new_error_message_lines;
            List<Read_error_message_line_class> error_messages_list = new List<Read_error_message_line_class>();
            List<T> Data = new List<T>();
            string directory = Path.GetDirectoryName(options.File);
            if (!Directory.Exists(directory))
            {
                new_error_message_line = new Read_error_message_line_class();
                new_error_message_line.File_type = Read_file_type.Data;
                new_error_message_line.Complete_fileName = (string)directory.Clone();
                new_error_message_line.Error_message = Read_error_message_enum.Directory_does_not_exist;
                error_messages_list.Add(new_error_message_line);
            }
            else if (!File.Exists(options.File))
            {
                new_error_message_line = new Read_error_message_line_class();
                new_error_message_line.File_type = Read_file_type.Data;
                new_error_message_line.Complete_fileName = (string)options.File.Clone();
                new_error_message_line.Error_message = Read_error_message_enum.File_does_not_exist;
                error_messages_list.Add(new_error_message_line);
            }
            else if (!Analyze_if_file_contains_characters(options, error_report_label, form_default_settings))
            {
                new_error_message_line = new Read_error_message_line_class();
                new_error_message_line.File_type = Read_file_type.Data;
                new_error_message_line.Complete_fileName = (string)options.File.Clone();
                new_error_message_line.Error_message = Read_error_message_enum.File_does_not_contain_text;
                error_messages_list.Add(new_error_message_line);
            }
            else
            {
                new_error_message_lines = new Read_error_message_line_class[0];
                if (options.File_has_headline) { new_error_message_lines = Analzye_if_all_columnNames_exist_and_return_likely_error_message(options, error_report_label, form_default_settings, options.File); }
                if (new_error_message_lines.Length>0)
                {
                    error_messages_list.AddRange(new_error_message_lines);
                }
                else
                {
                    StreamReader stream = Get_new_stream_reader_and_sent_notice_if_file_in_use(options.File, error_report_label, form_default_settings);
                    if (stream != null)
                    {
                        PropertyInfo[] propInfo = typeof(T).GetProperties();
                        FileInfo file = new FileInfo(options.File);

                        #region Determine columns to be safed and invalidLine_defining columns and properties
                        string[] columnNames = { Global_class.Empty_entry };
                        int[] columnIndexes;
                        int[] invalidLine_defining_columnIndexes = new int[0];
                        int[] invalidLine_defining_popertyIndexes = new int[0];
                        int[] propertyIndexes;

                        string inputLine;

                        if (options.File_has_headline)
                        {
                            string headline = Get_next_accepted_inputLine(stream);
                            columnNames = Get_and_modify_columnNames(headline, options);
                            columnIndexes = Get_columnIndexes_of_given_columnNames<T>(columnNames, options.Key_columnNames);
                            if (options.Invalid_line_defining_columnNames.Length > 0)
                            {
                                invalidLine_defining_columnIndexes = Get_columnIndexes_of_given_columnNames<T>(columnNames, options.Invalid_line_defining_columnNames);
                                invalidLine_defining_popertyIndexes = Get_propertyIndexes_of_corresponding_given_columnNames<T>(propInfo, options.Key_propertyNames, options.Key_columnNames, options.Invalid_line_defining_columnNames);
                            }
                        }
                        else { columnIndexes = options.Key_columnIndexes; }
                        propertyIndexes = Get_propertyIndexes<T>(propInfo, options.Key_propertyNames);
                        if (columnIndexes.Length != propertyIndexes.Length)
                        {
                            //Report_class.Write_error_line("{0}: Length columnIndexes (Key_columnNames/columnIndexes) != propertyIndexes (Key_propertyNames)", typeof(T).Name);
                            throw new Exception();
                        }
                        //End
                        #endregion

                        #region Generate and fill list
                        var TType = typeof(T);

                        int invalidLine_defining_columnIndexes_length = invalidLine_defining_columnIndexes.Length;
                        int readLines = 0;
                        int safedLines = 0;
                        int colIndex;
                        int propIndex;
                        bool safeLine;
                        string columnName;
                        int lineIndex = 1; //1, because headline was already read
                        int max_error_messages = options.Max_error_messages;

                        while ((inputLine = Get_next_accepted_inputLine(stream)) != null)
                        {
                            if (   (inputLine.Length > 0)
                                 &&((inputLine.Length < 6) || (!inputLine.Substring(0, 5).Equals("-----"))))
                            {
                                lineIndex++;
                                string[] columnEntries = inputLine.Split(options.LineDelimiters);
                                if ((options.File_has_headline)&&(columnEntries.Length != columnNames.Length))
                                {
                                    safeLine = false;
                                    new_error_message_line = new Read_error_message_line_class();
                                    new_error_message_line.File_type = Read_file_type.Data;
                                    new_error_message_line.Complete_fileName = (string)options.File.Clone();
                                    new_error_message_line.ColumnName = "";
                                    new_error_message_line.LineIndex = lineIndex;
                                    new_error_message_line.Error_message = Read_error_message_enum.Length_column_entries_not_equal_length_column_names;
                                    error_messages_list.Add(new_error_message_line);
                                }
                                else
                                {
                                    safeLine = true;
                                }
                                if (safeLine)
                                {
                                    T newLine = (T)Activator.CreateInstance(TType);
                                    for (int i = 0; i < columnIndexes.Length; i++)
                                    {
                                        colIndex = columnIndexes[i];
                                        if (options.File_has_headline)
                                        {
                                            columnName = columnNames[colIndex];
                                        }
                                        else
                                        {
                                            columnName = "Column index " + colIndex + " (zero based)";
                                        }
                                        propIndex = propertyIndexes[i];
                                        if (columnEntries[colIndex] == "#DIV/0!") { columnEntries[colIndex] = "NaN"; }
                                        if (propInfo[propIndex].PropertyType.IsEnum)
                                        {
                                            columnEntries[colIndex] = char.ToUpper(columnEntries[colIndex][0]) + columnEntries[colIndex].ToLower().Substring(1);
                                            try
                                            {
                                                propInfo[propIndex].SetValue(newLine, Enum.Parse(propInfo[propIndex].PropertyType, columnEntries[colIndex]), null);
                                            }
                                            catch
                                            {
                                                new_error_message_line = new Read_error_message_line_class();
                                                new_error_message_line.File_type = Read_file_type.Data;
                                                new_error_message_line.Complete_fileName = (string)options.File.Clone();
                                                new_error_message_line.ColumnName = (string)columnName.Clone();
                                                new_error_message_line.LineIndex = lineIndex;
                                                new_error_message_line.Value = (string)columnEntries[colIndex].Clone();
                                                new_error_message_line.Error_message = Read_error_message_enum.Not_part_of_enum;
                                                error_messages_list.Add(new_error_message_line);
                                            }
                                        }
                                        else if (string.IsNullOrEmpty(columnEntries[colIndex]))
                                        {
                                            if (propInfo[propIndex].PropertyType == typeof(int))
                                            {
                                                propInfo[propIndex].SetValue(newLine, options.Empty_integer_value, null);
                                            }
                                            else if (propInfo[propIndex].PropertyType == typeof(string))
                                            {
                                                propInfo[propIndex].SetValue(newLine, options.Empty_string_value, null);
                                            }
                                            else if (options.Report_unhandled_null_entries)
                                            {
                                                //Report_class.Write_error_line("{0}: ReadRawData_and_FillList: {1} unhandled null entry", typeof(ReadWriteClass).Name, options.File);
                                                throw new Exception();
                                            }
                                        }
                                        else
                                        {
                                            if ((columnEntries[colIndex] != "") && ((columnEntries[colIndex] != "NA") || (propInfo[propIndex].PropertyType == typeof(string))))
                                            {
                                                if (  (columnEntries[colIndex].Equals("Infinity"))
                                                    || (columnEntries[colIndex].Equals("∞")))
                                                {
                                                    if (propInfo[propIndex].PropertyType.Equals(typeof(double)))
                                                    {
                                                        columnEntries[colIndex] = (0.999 * Double.MaxValue).ToString();
                                                    }
                                                    else if (propInfo[propIndex].PropertyType.Equals(typeof(float)))
                                                    {
                                                        columnEntries[colIndex] = (0.999 * float.MaxValue).ToString();
                                                    }
                                                }
                                                else if (  (columnEntries[colIndex].Equals("-Infinity"))
                                                         ||(columnEntries[colIndex].Equals("-∞")))
                                                {
                                                    if (propInfo[propIndex].PropertyType.Equals(typeof(double)))
                                                    {
                                                        columnEntries[colIndex] = (-0.999 * Double.MaxValue).ToString();
                                                    }
                                                    else if (propInfo[propIndex].PropertyType.Equals(typeof(float)))
                                                    {
                                                        columnEntries[colIndex] = (-0.999 * float.MaxValue).ToString();
                                                    }
                                                }
                                                try
                                                {
                                                    propInfo[propIndex].SetValue(newLine, Convert.ChangeType(columnEntries[colIndex], propInfo[propIndex].PropertyType), null);
                                                }
                                                catch
                                                {
                                                    new_error_message_line = new Read_error_message_line_class();
                                                    new_error_message_line.File_type = Read_file_type.Data;
                                                    new_error_message_line.Complete_fileName = (string)options.File.Clone();
                                                    new_error_message_line.ColumnName = (string)columnName.Clone();
                                                    new_error_message_line.LineIndex = lineIndex;
                                                    new_error_message_line.Value = (string)columnEntries[colIndex].Clone();
                                                    if (propInfo[propIndex].PropertyType.Equals(typeof(int)))
                                                    {
                                                        new_error_message_line.Error_message = Read_error_message_enum.Not_an_integer;
                                                    }
                                                    else if ((propInfo[propIndex].PropertyType.Equals(typeof(float)))
                                                             || (propInfo[propIndex].PropertyType.Equals(typeof(double))))
                                                    {
                                                        new_error_message_line.Error_message = Read_error_message_enum.Not_a_float_or_double;
                                                    }
                                                    else { throw new Exception(); }
                                                    error_messages_list.Add(new_error_message_line);
                                                }
                                            }
                                        }
                                    }
                                    Data.Add(newLine);
                                    safedLines = safedLines + 1;
                                }
                                readLines = readLines + 1;
                            }
                            if (error_messages_list.Count >= max_error_messages)
                            {
                                new_error_message_line = new Read_error_message_line_class();
                                new_error_message_line.File_type = Read_file_type.Data;
                                new_error_message_line.Complete_fileName = (string)options.File.Clone();
                                new_error_message_line.ColumnName = "";
                                new_error_message_line.LineIndex = lineIndex;
                                new_error_message_line.Error_message = Read_error_message_enum.Reading_of_file_stopped;
                                error_messages_list.Add(new_error_message_line);
                                break;
                            }
                        }
                        stream.Close();
                        #endregion
                    }
                }
            }

            error_messages = error_messages_list.ToArray();
            return Data.ToArray();
        }
        public static T[] Read_data_fill_array_and_complain_if_error_message<T>(ReadWriteOptions_base readWriteOptions, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings) where T : class
        {
            Read_error_message_line_class[] error_messages = new Read_error_message_line_class[1];
            T[] data = new T[0];
            data = Read_data_fill_array_and_return_error_messages<T>(out error_messages, readWriteOptions, error_report_label, form_default_settings);
            if (error_messages.Length > 0)
            {
                string shared_response = "\nPlease download MBCO windows application from mbc-ontology.org again.";
                string file = Path.GetFileName(readWriteOptions.File);
                string directory = Path.GetDirectoryName(readWriteOptions.File);
                switch (error_messages[0].Error_message)
                {
                    case Read_error_message_enum.Defined_columnNames_are_missing:
                        error_report_label.Text = "Columns " + error_messages[0].Value + " are missing in " + file + shared_response; ;
                        break;
                    case Read_error_message_enum.Directory_does_not_exist:
                        error_report_label.Text = "Directory \"" + directory + "\" does not exist." + shared_response;
                        break;
                    case Read_error_message_enum.File_does_not_exist:
                        error_report_label.Text = "File \"" + file + "\" does not exist in directory." + shared_response;
                        break;
                    default:
                        error_report_label.Text = "Reading of file \"" + file + "\" not possible." + shared_response;
                        break;
                }
                error_report_label.Refresh();
                System.Threading.Thread.Sleep(15000);
            }
            return data;
        }
        public static string[] Read_string_array_and_remove_non_letters_from_beginning_and_end_of_each_line(string completeFileName, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            StreamReader reader = Get_new_stream_reader_and_sent_notice_if_file_in_use(completeFileName, error_report_label, form_default_settings);
            List<string> string_list = new List<string>();
            string inputLine;
            while ((inputLine = reader.ReadLine()) != null)
            {
                string_list.Add(Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(inputLine));
            }
            reader.Close();
            return string_list.ToArray();
        }
        #endregion

        #region Write data
        public static StreamWriter Get_new_stream_writer_and_sent_notice_if_file_in_use(string complete_fileName, System.Windows.Forms.Label error_report_label, Form1_default_settings_class form_default_settings)
        {
            Create_directory_if_it_does_not_exist(complete_fileName);
            string report_label_text = (string)error_report_label.Text.Clone();
            bool report_label_visible = error_report_label.Visible;
            int report_label_x_location = error_report_label.Location.X;
            bool file_opened = false;
            StreamWriter writer = null;
            while (!file_opened)
            {
                try
                {
                    writer = new StreamWriter(complete_fileName);
                    file_opened = true;
                }
                catch
                {
                    error_report_label.Text = ReadWriteOptions_base.Save_file_in_use_warning_text_start + complete_fileName;
                    form_default_settings.LabelProgressReport_set_sizes_and_fontSize(error_report_label,0);
                    error_report_label.Visible = true;
                    error_report_label.Refresh();
                }
            }
            error_report_label.Visible = report_label_visible;
            error_report_label.Text = (string)report_label_text.Clone();
            error_report_label.Location = new System.Drawing.Point(report_label_x_location, error_report_label.Location.Y);
            return writer;
        }
        public static void WriteData<T>(T[] Data, ReadWriteOptions_base Options, System.Windows.Forms.Label reportLabel, Form1_default_settings_class form_default_settings) where T : class
        {
            if (Options.Read_only==true) { throw new Exception(); }
            StreamWriter writer = Get_new_stream_writer_and_sent_notice_if_file_in_use(Options.File, reportLabel, form_default_settings);
            WriteData(Data, Options, writer);
            writer.Close();
        }
        private static void WriteData<T>(T[] Data, ReadWriteOptions_base Options, StreamWriter writer) where T : class
        {
            WriteData(Data.ToList(), Options, writer);
        }
        private static void WriteData_headline<T>(List<T> Data, ReadWriteOptions_base Options, StreamWriter writer) where T : class
        {
            PropertyInfo[] propInfo = typeof(T).GetProperties();
            int[] propertyIndexes = Get_propertyIndexes<T>(propInfo, Options.Key_propertyNames);

            //Generate and write Headline
            int propertyIndexes_length = propertyIndexes.Length;
            if (Options.File_has_headline == true)
            {
                char headline_delimiter = Options.HeadlineDelimiters[0];
                StringBuilder headline = new StringBuilder();
                for (int index = 0; index < propertyIndexes_length; index++)
                {
                    if (index < propertyIndexes_length - 1)
                    {
                        headline.AppendFormat("{0}{1}", Options.Key_columnNames[index], headline_delimiter);
                    }
                    else
                    {
                        headline.AppendFormat("{0}", Options.Key_columnNames[index]);
                    }
                }
                writer.WriteLine(headline);
            }
        }
        private static void WriteData_body<T>(List<T> Data, ReadWriteOptions_base Options, StreamWriter writer) where T : class
        {
            PropertyInfo[] propInfo = typeof(T).GetProperties();
            PropertyInfo prop;

            int[] propertyIndexes = Get_propertyIndexes<T>(propInfo, Options.Key_propertyNames);
            int propertyIndexes_length = propertyIndexes.Length;

            //Generate and write lines
            char line_delimiter = Options.LineDelimiters[0];
            StringBuilder line = new StringBuilder();
            int data_count = Data.Count;
            for (int lineIndex = 0; lineIndex < data_count; lineIndex++)
            {
                line.Clear();
                for (int index = 0; index < propertyIndexes_length; index++)
                {
                    prop = propInfo[propertyIndexes[index]];
                    if (index < propertyIndexes_length - 1) { line.AppendFormat("{0}{1}", prop.GetValue(Data[lineIndex], null), line_delimiter); }
                    else { line.AppendFormat("{0}", prop.GetValue(Data[lineIndex], null)); }
                }
                writer.WriteLine(line);
            }
        }
        private static void WriteData<T>(List<T> Data, ReadWriteOptions_base Options, StreamWriter writer) where T : class
        {
            if (Options.Read_only == true) { throw new Exception(); }
            WriteData_headline<T>(Data, Options, writer);
            WriteData_body<T>(Data, Options, writer);
            writer.Close();
        }
        public static void WriteArray<T>(T[] array, string file_name, System.Windows.Forms.Label reportLabel, Form1_default_settings_class form_default_settings)
        {
            StreamWriter writer = Get_new_stream_writer_and_sent_notice_if_file_in_use(file_name, reportLabel, form_default_settings);
            int array_length = array.Length;
            for (int indexA = 0; indexA < array_length; indexA++)
            {
                writer.WriteLine(array[indexA]);
            }
            writer.Close();
        }
        #endregion

        #region Get array or dictionary from readLine and vice verse
        public static T Get_value_from_string<T>(string read_string)
        {
            if (typeof(T).IsEnum)
            {
                read_string = char.ToUpper(read_string[0]) + read_string.ToLower().Substring(1);
                return (T)Enum.Parse(typeof(T), read_string);
            }
            else
            {
                return (T)Convert.ChangeType(read_string, typeof(T));
            }

        }
        public static T[] Get_array_from_readLine<T>(string readLine, char delimiter)
        {
            string[] split = readLine.Split(delimiter);
            int split_length = split.Length;
            if (string.IsNullOrEmpty(split[split_length - 1])) { split_length--; }
            var TType = typeof(T);
            T[] array = new T[split_length];
            for (int i = 0; i < split_length; i++)
            {
                array[i] = Get_value_from_string<T>(split[i]);
            }
            return array;
        }
        public static string Get_writeLine_from_array<T>(T[] array, char delimiter)
        {
            StringBuilder stringBuild = new StringBuilder();
            if (array != null)
            {
                int array_length = array.Length;
                for (int i = 0; i < array_length; i++)
                {
                    if (i == 0) { stringBuild.AppendFormat("{0}", array[i]); }
                    else { stringBuild.AppendFormat("{0}{1}", delimiter, array[i]); }
                }
                return stringBuild.ToString();
            }
            else return "";
        }
        public static Dictionary<T1, T2> Get_dictionary_from_readLine<T1, T2>(string readLIne, char delimiter1, char delimiter2)
        {
            Dictionary<T1, T2> dict = new Dictionary<T1, T2>();
            string[] splitStrings1 = readLIne.Split(delimiter1);
            string splitString1;
            int splitStrings1_length = splitStrings1.Length;
            string[] splitStrings2;
            T2 current_value;
            T1 current_key;
            var T1Type = typeof(T1);
            var T2Type = typeof(T2);
            for (int indexSplit1 = 0; indexSplit1 < splitStrings1_length; indexSplit1++)
            {
                splitString1 = splitStrings1[indexSplit1];
                splitStrings2 = splitString1.Split(delimiter2);
                if (splitStrings2.Length!=2) { throw new Exception(); }
                current_value = Get_value_from_string<T2>(splitStrings2[1]);
                current_key = Get_value_from_string<T1>(splitStrings2[0]);
                dict.Add(current_key, current_value);
            }
            return dict;
        }
        public static string Get_writeLine_from_dictionary<T1, T2>(Dictionary<T1, T2> dict, char delimiter1, char delimiter2)
        {
            T1[] dict_keys = dict.Keys.ToArray();
            T2 current_value;
            StringBuilder sb = new StringBuilder();
            foreach (T1 dict_key in dict_keys)
            {
                if (sb.Length > 0) { sb.AppendFormat(delimiter1.ToString()); }
                sb.AppendFormat("{0}", dict_key.ToString(), delimiter1);
                current_value = dict[dict_key];
                sb.AppendFormat("{0}{1}", delimiter2.ToString(),current_value.ToString());
            }
            return sb.ToString();
        }
        public static Dictionary<T1, T2[]> Get_dictionary_with_array_values_from_readLine<T1, T2>(string readLIne, char delimiter1, char delimiter2)
        {
            Dictionary<T1, T2[]> dict = new Dictionary<T1, T2[]>();
            string[] splitStrings1 = readLIne.Split(delimiter1);
            string splitString1;
            int splitStrings1_length = splitStrings1.Length;
            string[] splitStrings2;
            T2[] current_values;
            T1 current_key;
            var T1Type = typeof(T1);
            var T2Type = typeof(T2);
            int splitStrings2_length;
            for (int indexSplit1 = 0; indexSplit1 < splitStrings1_length; indexSplit1++)
            {
                splitString1 = splitStrings1[indexSplit1];
                splitStrings2 = splitString1.Split(delimiter2);
                splitStrings2_length = splitStrings2.Length;
                current_values = new T2[splitStrings2_length - 1];
                for (int indexSplit2 = 1; indexSplit2 < splitStrings2_length; indexSplit2++)
                {
                    current_values[indexSplit2-1] = Get_value_from_string<T2>(splitStrings2[indexSplit2]);
                }
                current_key = Get_value_from_string<T1>(splitStrings2[0]);
                dict.Add(current_key, current_values);
            }
            return dict;
        }
        public static string Get_writeLine_from_dictionary_with_array_values<T1, T2>(Dictionary<T1, T2[]> dict, char delimiter1, char delimiter2)
        {
            T1[] dict_keys = dict.Keys.ToArray();
            T2[] current_values;
            T2 current_value;
            int current_values_length;
            StringBuilder sb = new StringBuilder();
            foreach (T1 dict_key in dict_keys)
            {
                if (sb.Length > 0) { sb.AppendFormat(delimiter1.ToString()); }
                sb.AppendFormat("{0}", dict_key.ToString(), delimiter1);
                current_values = dict[dict_key];
                current_values_length = current_values.Length;
                for (int indexCV = 0; indexCV < current_values_length; indexCV++)
                {
                    current_value = current_values[indexCV];
                    sb.AppendFormat("{0}{1}",delimiter2.ToString(), current_value.ToString());
                }
            }
            return sb.ToString();
        }
        #endregion

        #region Directory
        public static void Create_directory_if_it_does_not_exist(string complete_directory_name)
        {
            string directory = System.IO.Path.GetDirectoryName(complete_directory_name);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
        #endregion
    }

}

