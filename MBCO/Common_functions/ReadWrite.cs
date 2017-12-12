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
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Text;
using Common_functions.Text;
using Common_functions.Global_definitions;
using Common_functions.Report;

namespace Common_functions.ReadWrite
{

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
        public string[] RemoveFromHeadline { get; set; }
        public char[] LineDelimiters { get; set; }
        public char[] HeadlineDelimiters { get; set; }
        public int Skip_lines { get; set; }
        public int Empty_integer_value { get; set; }
        public string Empty_string_value { get; set; }
        public string[] Invalid_line_defining_columnNames { get; set; }
        public ReadWrite_report_enum Report { get; set; }
        public bool Report_unhandled_null_entries { get; set; }
        #endregion

        public ReadWriteOptions_base()
        {
            Report_unhandled_null_entries = true;
            Invalid_line_defining_columnNames = new string[0];
            Empty_string_value = "";
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

    public abstract class DictionaryReadWriteOptions_base : ReadWriteOptions_base
    {
        public string DictionaryKey_columnName { get; set; }
        public int DictionaryKey_columnIndex { get; set; }
    }

    public class ReadWriteOptions_standard : ReadWriteOptions_base
    {
        public ReadWriteOptions_standard(string file_name)
        {
            Set_parameter(file_name);
        }

        public ReadWriteOptions_standard(string subdirectory, string file_name)
        {
            ReadWriteClass.Create_directory_if_it_does_not_exist(Global_directory_and_file_class.Results_directory + subdirectory);
            Set_parameter(subdirectory + file_name);
        }

        private void Set_parameter(string complete_add_file_name)
        {
            File = Global_directory_and_file_class.Results_directory + complete_add_file_name;
            LineDelimiters = new char[] { '\t' };
            HeadlineDelimiters = new char[] { '\t' };
        }
    }

    class ReadWriteClass
    {
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
                Report_class.Write_error_line("{0}: no columnNames to search for", typeof(T).Name);
            }
            int[] columnIndexes = new int[given_length];
            for (int i = 0; i < given_length; i++)
            {
                int index = Array.IndexOf(columnNames, given_columnNames[i]);
                if (index >= 0) { columnIndexes[i] = index; }
                else
                {
                    Report_class.Write_error_line("{0}: columnName \"{1}\" does not exist", typeof(T).Name, given_columnNames[i]);
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
                Report_class.Write_error_line("{0}: no search columnNames to search for", typeof(T).Name);
                throw new Exception();
            }
            for (int i = 0; i < search_length; i++)
            {
                int index = Array.IndexOf(given_columnNames, search_given_columnNames[i]);
                if (index >= 0) { columnNames_indexes[i] = index; }
                else
                {
                    Report_class.Write_error_line("{0}: given_columnName \"{1}\" does not exist", typeof(T).Name, given_columnNames[i]);
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
                    Report_class.Write_error_line("{0}: propertyName \"{1}\" does not exist", typeof(T).Name, key_propertyNames[i]);
                    throw new Exception();
                }
            }
            return propertyIndexes;
        }

        public static void Create_subdirectory_if_it_does_not_exist(string actDirectory, string sub_directory_name)
        {
            if (!Directory.Exists(actDirectory + sub_directory_name))
            {
                Report_class.WriteLine("{0}: Create subdirectory {1} in directory {2}", typeof(ReadWriteClass).Name, sub_directory_name, actDirectory);
                DirectoryInfo dir = new DirectoryInfo(actDirectory);
                dir.CreateSubdirectory(sub_directory_name);
            }
        }

        public static void Create_subdirectory_in_results_directory_if_it_does_not_exist(string sub_directory_name)
        {
            Create_subdirectory_if_it_does_not_exist(Global_directory_and_file_class.Results_directory, sub_directory_name);
        }

        public static List<T> ReadRawData_and_FillList<T>(ReadWriteOptions_base options) where T : class
        {
            FileInfo file = new FileInfo(options.File);
            StreamReader stream = file.OpenText();
            List<T> Data = ReadRawData_and_FillList<T>(stream, options, options.File);
            return Data;
        }

        public static List<T> ReadRawData_and_FillList<T>(StreamReader stream, ReadWriteOptions_base options, string file_name, params string[] columName_with_only_unique_entries) where T : class
        {
            if (options.Report != ReadWrite_report_enum.Report_nothing)
            {
                Report_class.WriteLine("{0}:\nRead file: {1}", typeof(T).Name, file_name);
            }
            Stopwatch timer = new Stopwatch();
            timer.Start();

            PropertyInfo[] propInfo = typeof(T).GetProperties();
            FileInfo file = new FileInfo(options.File);

            #region Determine columns to be safed and invalidLine_defining columns and properties
            //Read headline, if it exists, determine indexes of columns to be safed in list
            //Begin
            string[] columnNames = { Global_class.Empty_entry };
            int[] columnIndexes;
            int[] invalidLine_defining_columnIndexes = new int[0];
            int[] invalidLine_defining_popertyIndexes = new int[0];
            int[] propertyIndexes;

            if (options.File_has_headline)
            {
                string headline = stream.ReadLine();
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
                Report_class.Write_error_line("{0}: Length columnIndexes (Key_columnNames/columnIndexes) != propertyIndexes (Key_propertyNames)", typeof(T).Name);
                throw new Exception();
            }
            //End
            #endregion

            #region Skip lines
            //Skip lines
            for (int indexSkip = 0; indexSkip < options.Skip_lines; indexSkip++)
            {
                stream.ReadLine();
            }
            #endregion

            #region Determine indexes of columns which contain a safecondition, if safeconditions exist
            bool safeConditions_exist = options.SafeCondition_entries != null;
            int[] safeConditions_columnIndexes = new int[0];
            string[] safeConditions_entries = options.SafeCondition_entries;
            int safeConditions_length = -1;

            if (safeConditions_exist == true)
            {
                safeConditions_length = options.SafeCondition_entries.Length;
                if (options.File_has_headline)
                {
                    safeConditions_columnIndexes = Get_columnIndexes_of_given_columnNames<T>(columnNames, options.SafeCondition_columnNames);
                }
                else
                {
                    safeConditions_columnIndexes = options.SafeCondition_columnIndexes;
                }
                if (safeConditions_columnIndexes.Length != safeConditions_entries.Length) { Report_class.WriteLine("{0}: length safeConditions_columnIndexes (_columnNames/columnIndexes) != length safeConditions_columnEntries", typeof(T).Name); }
            }
            #endregion

            #region Generate and fill list
            List<T> Data = new List<T>();
            var TType = typeof(T);

            int invalidLine_defining_columnIndexes_length = invalidLine_defining_columnIndexes.Length;
            string inputLine;
            int readLines = 0;
            int safedLines = 0;
            int colIndex;
            int propIndex;
            bool safeLine;
            bool report_check_lineDelimiters = false;
            bool valid;
            string invalidLineDefiningColumnEntry;
            int line_count = 0;

            while ((inputLine = stream.ReadLine()) != null)
            {
                if ((inputLine.Length > 0) && ((inputLine.Length<6) || (!inputLine.Substring(0, 5).Equals("-----"))))
                {
                    line_count++;
                    string[] columnEntries = inputLine.Split(options.LineDelimiters);
                    if (columnEntries.Length == 1)
                    {
                        report_check_lineDelimiters = true;
                    }
                    safeLine = true;
                    if (safeConditions_exist)
                    {
                        for (int indexSC = 0; indexSC < safeConditions_length; indexSC++)
                        {
                            columnEntries[safeConditions_columnIndexes[indexSC]] = char.ToUpper(columnEntries[safeConditions_columnIndexes[indexSC]][0]) + columnEntries[safeConditions_columnIndexes[indexSC]].ToLower().Substring(1);  ///transient solution!!!!!!
                            if (safeConditions_entries[indexSC] != columnEntries[safeConditions_columnIndexes[indexSC]])
                            {
                                safeLine = false;
                            }
                        }
                    }
                    valid = true;
                    for (int indexIndex = 0; indexIndex < invalidLine_defining_columnIndexes_length; indexIndex++)
                    {
                        invalidLineDefiningColumnEntry = columnEntries[invalidLine_defining_columnIndexes[indexIndex]];
                        try
                        {
                            var obj = Convert.ChangeType(invalidLineDefiningColumnEntry, propInfo[invalidLine_defining_popertyIndexes[indexIndex]].PropertyType);
                            valid = true;
                        }
                        catch (InvalidCastException)
                        {
                            valid = false;
                        }
                        catch (FormatException)
                        {
                            valid = false;
                        }
                        catch (OverflowException)
                        {
                            valid = false;
                        }
                        catch (ArgumentNullException)
                        {
                            valid = false;
                        }
                    }
                    if ((safeLine) && (valid))
                    {
                        T newLine = (T)Activator.CreateInstance(TType);
                        for (int i = 0; i < columnIndexes.Length; i++)
                        {
                            colIndex = columnIndexes[i];
                            propIndex = propertyIndexes[i];
                            if (columnEntries[colIndex] == "#DIV/0!") { columnEntries[colIndex] = "NaN"; }
                            if (propInfo[propIndex].PropertyType.IsEnum)
                            {
                                columnEntries[colIndex] = char.ToUpper(columnEntries[colIndex][0]) + columnEntries[colIndex].ToLower().Substring(1);
                                propInfo[propIndex].SetValue(newLine, Enum.Parse(propInfo[propIndex].PropertyType, columnEntries[colIndex]), null);
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
                                    Report_class.Write_error_line("{0}: ReadRawData_and_FillList: {1} unhandled null entry", typeof(ReadWriteClass).Name, options.File);
                                    throw new Exception();
                                }
                            }
                            else
                            {
                                if ((columnEntries[colIndex] != "") && ((columnEntries[colIndex] != "NA") || (propInfo[propIndex].PropertyType == typeof(string))))
                                {
                                    propInfo[propIndex].SetValue(newLine, Convert.ChangeType(columnEntries[colIndex], propInfo[propIndex].PropertyType), null);
                                }
                            }
                        }
                        Data.Add(newLine);
                        safedLines = safedLines + 1;
                    }
                    readLines = readLines + 1;
                    if ((options.Report == ReadWrite_report_enum.Report_everything) && (readLines % 2000000 == 0)) { Report_class.WriteLine("{0}: Read lines: {1} Mio, \tSafed lines: {2} Mio", typeof(T).Name, (double)readLines / 1000000, (double)safedLines / 1000000); }
                }
            }
            #endregion

            #region Final report
            if (report_check_lineDelimiters)
            {
                Report_class.Write_error_line("{0}: only one column entry: Check lineDelimiters", typeof(ReadWriteClass).Name);
            }
            timer.Stop();
            if (options.Report == ReadWrite_report_enum.Report_everything)
            {
                Report_class.WriteLine("{0}: Read lines: {1} Mio, Safed lines: {2} Mio", typeof(T).Name, (double)readLines / 1000000, (double)safedLines / 1000000);
                Report_class.WriteLine("{0}: Time: {1}", typeof(T).Name, timer.Elapsed);
            }
            if (options.Report != ReadWrite_report_enum.Report_nothing)
            {
                Report_class.WriteLine();
            }
            stream.Close();
            if (Data.Count == 0)
            {
                Report_class.Write_error_line("{0}: ReadFile {1} data.count == 0, no lines filled!", typeof(ReadWriteClass).Name, options.File);
            }
            #endregion

            return Data;
        }

        public static Dictionary<TD, T_line> ReadRawData_and_fillDictionary<TD, T_line>(StreamReader stream, DictionaryReadWriteOptions_base options, string file_name) where T_line : class
        {
            if ((options.Report == ReadWrite_report_enum.Report_main)||(options.Report == ReadWrite_report_enum.Report_everything))
            {
                Report_class.WriteLine("{0}:\nRead file for dictionary: {1}", typeof(T_line).Name, file_name);
            }
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Read headline, if it exists, determine indexes of columns to be safed in list
            //Begin
            FileInfo file = new FileInfo(options.File);
            PropertyInfo[] propInfo = typeof(T_line).GetProperties();
            PropertyInfo dict_key_propInfo = typeof(TD).GetProperties()[0];
            string[] columnNames = { Global_class.Empty_entry };
            int dictionaryKey_index;
            int[] columnIndexes;
            int[] invalidLine_defining_columnIndexes = new int[0];
            int[] invalidLine_defining_popertyIndexes = new int[0];
            int[] propertyIndexes;

            if (options.File_has_headline)
            {
                string headline = stream.ReadLine();
                columnNames = Get_and_modify_columnNames(headline, options);
                columnIndexes = Get_columnIndexes_of_given_columnNames<T_line>(columnNames, options.Key_columnNames);
                dictionaryKey_index = Get_columnIndexes_of_given_columnNames<T_line>(columnNames, options.DictionaryKey_columnName)[0];
                if (options.Invalid_line_defining_columnNames.Length > 0)
                {
                    invalidLine_defining_columnIndexes = Get_columnIndexes_of_given_columnNames<T_line>(columnNames, options.Invalid_line_defining_columnNames);
                    invalidLine_defining_popertyIndexes = Get_propertyIndexes_of_corresponding_given_columnNames<T_line>(propInfo, options.Key_propertyNames, options.Key_columnNames, options.Invalid_line_defining_columnNames);
                }
            }
            else
            {
                columnIndexes = options.Key_columnIndexes;
                dictionaryKey_index = options.DictionaryKey_columnIndex;
            }
            propertyIndexes = Get_propertyIndexes<T_line>(propInfo, options.Key_propertyNames);
            if (columnIndexes.Length != propertyIndexes.Length)
            {
                Report_class.Write_error_line("{0}: Length columnIndexes (Key_columnNames/columnIndexes) != propertyIndexes (Key_propertyNames)", typeof(T_line).Name);
            }
            //End

            //Skip lines
            for (int indexSkip = 0; indexSkip < options.Skip_lines; indexSkip++)
            {
                stream.ReadLine();
            }

            //Determine indexes of columns which contain a safecondition, if safeconditions exist
            //Begin
            bool safeConditions_exist = options.SafeCondition_entries != null;
            int[] safeConditions_columnIndexes = new int[0];
            string[] safeConditions_entries = options.SafeCondition_entries;
            int safeConditions_length = -1;

            if (safeConditions_exist == true)
            {
                safeConditions_length = options.SafeCondition_entries.Length;
                if (options.File_has_headline)
                {
                    safeConditions_columnIndexes = Get_columnIndexes_of_given_columnNames<T_line>(columnNames, options.SafeCondition_columnNames);
                }
                else
                {
                    safeConditions_columnIndexes = options.SafeCondition_columnIndexes;
                }
                if (safeConditions_columnIndexes.Length != safeConditions_entries.Length) { Report_class.WriteLine("{0}: length safeConditions_columnIndexes (_columnNames/columnIndexes) != length safeConditions_columnEntries", typeof(T_line).Name); }
            }
            //End

            //Generate and fill list
            //Begin
            Dictionary<TD, T_line> dictionary = new Dictionary<TD, T_line>();

            var TD_Type = typeof(TD);
            var T_line_Type = typeof(T_line);

            int invalidLine_defining_columnIndexes_length = invalidLine_defining_columnIndexes.Length;
            string inputLine;
            int readLines = 0;
            int safedLines = 0;
            int colIndex;
            int propIndex;
            bool safeLine;
            bool report_check_lineDelimiters = false;
            bool valid;
            string invalidLineDefiningColumnEntry;
            int line_count = 0;

            while ((inputLine = stream.ReadLine()) != null)
            {
                if ((inputLine.Length > 0) && (!inputLine.Substring(0, 5).Equals("-----")))
                {
                    line_count++;
                    string[] columnEntries = inputLine.Split(options.LineDelimiters);
                    if (columnEntries.Length == 1)
                    {
                        report_check_lineDelimiters = true;
                    }
                    safeLine = true;
                    if (safeConditions_exist)
                    {
                        for (int indexSC = 0; indexSC < safeConditions_length; indexSC++)
                        {
                            columnEntries[safeConditions_columnIndexes[indexSC]] = char.ToUpper(columnEntries[safeConditions_columnIndexes[indexSC]][0]) + columnEntries[safeConditions_columnIndexes[indexSC]].ToLower().Substring(1);  ///transient solution!!!!!!
                            if (safeConditions_entries[indexSC] != columnEntries[safeConditions_columnIndexes[indexSC]])
                            {
                                safeLine = false;
                            }
                        }
                    }
                    valid = true;
                    for (int indexIndex = 0; indexIndex < invalidLine_defining_columnIndexes_length; indexIndex++)
                    {
                        invalidLineDefiningColumnEntry = columnEntries[invalidLine_defining_columnIndexes[indexIndex]];
                        try
                        {
                            var obj = Convert.ChangeType(invalidLineDefiningColumnEntry, propInfo[invalidLine_defining_popertyIndexes[indexIndex]].PropertyType);
                            valid = true;
                        }
                        catch (InvalidCastException)
                        {
                            valid = false;
                        }
                        catch (FormatException)
                        {
                            valid = false;
                        }
                        catch (OverflowException)
                        {
                            valid = false;
                        }
                        catch (ArgumentNullException)
                        {
                            valid = false;
                        }
                    }
                    if ((safeLine) && (valid))
                    {
                        //TD new_dictionary_key = new TD();//)Activator.CreateInstance(TD_Type);
                        TD new_dictionary_key = (TD)Convert.ChangeType(columnEntries[dictionaryKey_index], TD_Type);

                        T_line newLine = (T_line)Activator.CreateInstance(T_line_Type);
                        for (int i = 0; i < columnIndexes.Length; i++)
                        {
                            colIndex = columnIndexes[i];
                            propIndex = propertyIndexes[i];
                            if (columnEntries[colIndex] == "#DIV/0!") { columnEntries[colIndex] = "NaN"; }
                            if (propInfo[propIndex].PropertyType.IsEnum)
                            {
                                columnEntries[colIndex] = char.ToUpper(columnEntries[colIndex][0]) + columnEntries[colIndex].ToLower().Substring(1);
                                propInfo[propIndex].SetValue(newLine, Enum.Parse(propInfo[propIndex].PropertyType, columnEntries[colIndex]), null);
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
                                else
                                {
                                    Report_class.Write_error_line("{0}: ReadRawData_and_FillList: {1} unhandled null entry", typeof(ReadWriteClass).Name, options.File);
                                }
                            }
                            else
                            {
                                if ((columnEntries[colIndex] != "") && ((columnEntries[colIndex] != "NA") || (propInfo[propIndex].PropertyType == typeof(string))))
                                {
                                    propInfo[propIndex].SetValue(newLine, Convert.ChangeType(columnEntries[colIndex], propInfo[propIndex].PropertyType), null);
                                }
                            }
                        }
                        dictionary.Add(new_dictionary_key, newLine);
                        safedLines = safedLines + 1;
                    }
                    readLines = readLines + 1;
                    if ((options.Report == ReadWrite_report_enum.Report_everything) && (readLines % 2000000 == 0)) { Report_class.WriteLine("{0}: Read lines: {1} Mio, \tSafed lines: {2} Mio", typeof(T_line).Name, (double)readLines / 1000000, (double)safedLines / 1000000); }
                }
            }
            if (report_check_lineDelimiters)
            {
                Report_class.Write_error_line("{0}: only one column entry: Check lineDelimiters", typeof(ReadWriteClass).Name);
            }
            timer.Stop();
            if (options.Report == ReadWrite_report_enum.Report_everything)
            {
                Report_class.WriteLine("{0}: Read lines: {1} Mio, Safed lines: {2} Mio", typeof(T_line).Name, (double)readLines / 1000000, (double)safedLines / 1000000);
                Report_class.WriteLine("{0}: Time: {1}", typeof(T_line).Name, timer.Elapsed);
            }
            if (options.Report >= ReadWrite_report_enum.Report_main)
            {
                Report_class.WriteLine();
            }
            stream.Close();
            if (dictionary.Count == 0)
            {
                Report_class.Write_error_line("{0}: ReadFile {1} data.count == 0, no lines filled!", typeof(ReadWriteClass).Name, options.File);
            }
            return dictionary;
        }

        public static Dictionary<TD, T_line> ReadRawData_and_fillDictionary<TD, T_line>(DictionaryReadWriteOptions_base options) where T_line : class
        {
            FileInfo file = new FileInfo(options.File);
            StreamReader stream = file.OpenText();
            Dictionary<TD, T_line> dictionary = ReadRawData_and_fillDictionary<TD, T_line>(stream, options, options.File);
            return dictionary;
        }

        public static T[] ReadRawData_and_FillArray<T>(ReadWriteOptions_base Options) where T : class
        {
            return ReadRawData_and_FillList<T>(Options).ToArray();
        }

        public static string[] Read_string_array_and_remove_non_letters_from_beginning_and_end_of_each_line(string filename)
        {
            string directory = Global_directory_and_file_class.Custom_data_directory;
            string full_file_name = directory + filename;
            StreamReader reader = new StreamReader(full_file_name);
            List<string> string_list = new List<string>();
            string inputLine;
            while ((inputLine= reader.ReadLine()) != null)
            {
                string_list.Add(Text_class.Remove_space_comma_semicolon_colon_from_end_and_beginning_of_text(inputLine));
            }
            return string_list.ToArray();
        }

        public static void Delete_file_if_it_exists(string complete_file_name)
        {
            if (File.Exists(complete_file_name))
            {
                Report_class.Write("Delete file: {0}?", complete_file_name);
                Console.ReadLine();
                FileInfo delete_file = new FileInfo(complete_file_name);
                delete_file.Delete();
                Report_class.WriteLine(" deleted");
            }
        }

        public static void Delete_files_in_directory(string directory)
        {
            string[] complete_file_names = Directory.GetFiles(directory);
            if (complete_file_names.Length > 0)
            {
                Report_class.Write("Delete files in directory {0}:", directory);
                Console.ReadLine();
                foreach (string complete_file_name in complete_file_names)
                {
                    FileInfo delete_file = new FileInfo(complete_file_name);
                    delete_file.Delete();
                }
            }
        }

        public static void Delete_files_in_directory_that_start_with_startString(string directory, string startString)
        {
            string[] complete_file_names = Directory.GetFiles(directory);
            int complete_file_names_length = complete_file_names.Length;
            if (complete_file_names_length > 0)
            {
                Report_class.Write("Delete files in directory {0}:", directory);
                Console.ReadLine();
                string[] file_names = new string[complete_file_names_length];
                for (int indexC = 0; indexC < complete_file_names_length; indexC++)
                {
                    file_names[indexC] = Path.GetFileName(complete_file_names[indexC]);
                }

                foreach (string file_name in file_names)
                {
                    if (file_name.IndexOf(startString) == 0)
                    {
                        FileInfo delete_file = new FileInfo(directory + file_name);
                        delete_file.Delete();
                    }
                }
            }
        }

        public static void Write_one_type_array_as_single_line<T>(T[] Array, ReadWriteOptions_base Options)
        {
            Report_class.WriteLine("{0}: Write file {1}", typeof(T).Name, Options.File);
            StreamWriter writer = new StreamWriter(Options.File, false);

            //Generate and write lines
            char line_delimiter = Options.LineDelimiters[0];
            StringBuilder line = new StringBuilder();
            int array_length = Array.Length;
            for (int index = 0; index < array_length; index++)
            {
                line.AppendFormat("{0}{1}", Array[index], line_delimiter);
            }
            writer.WriteLine(line);
            writer.Close();
            Report_class.WriteLine();
        }

        public static void Write_one_type_array_as_single_column<T>(T[] Array, ReadWriteOptions_base Options)
        {
            Report_class.WriteLine("{0}: Write file {1}", typeof(T).Name, Options.File);
            StreamWriter writer = new StreamWriter(Options.File, false);

            //Generate and write lines
            char line_delimiter = Options.LineDelimiters[0];
            StringBuilder line = new StringBuilder();
            int array_length = Array.Length;
            for (int index = 0; index < array_length; index++)
            {
                writer.WriteLine(Array[index]);
            }
            writer.Close();
            Report_class.WriteLine();
        }

        public static void Write_one_type_array_as_single_column<T>(T[] Array, string file_name)
        {
            ReadWriteOptions_standard options = new ReadWriteOptions_standard(file_name);
            Write_one_type_array_as_single_column<T>(Array, options);
        }

        #region Write data
        public static void WriteData<T>(List<T> Data, ReadWriteOptions_base Options) where T : class
        {
            Report_class.WriteLine("{0}: Write file {1}", typeof(T).Name, Options.File);
            StreamWriter writer = new StreamWriter(Options.File, false);
            WriteData(Data, Options, writer);
        }

        public static void WriteData<T>(T[] Data, ReadWriteOptions_base Options) where T : class
        {
            if (Options.Report != ReadWrite_report_enum.Report_nothing) { Report_class.WriteLine("{0}: Write file {1}", typeof(T).Name, Options.File); }
            StreamWriter writer = new StreamWriter(Options.File, false);
            WriteData(Data, Options, writer);
        }

        #region For parallelization
        public static bool Is_file_in_use(string complete_file_name)
        {
            FileStream stream = null;
            FileInfo file = new FileInfo(complete_file_name);
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
        #endregion

        public static void WriteData<T>(T[] Data, ReadWriteOptions_base Options, StreamWriter writer) where T : class
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

        public static void WriteData<T>(List<T> Data, ReadWriteOptions_base Options, StreamWriter writer) where T : class
        {
            WriteData_headline<T>(Data, Options, writer);
            WriteData_body<T>(Data, Options, writer);
            writer.Close();
            if (Options.Report != ReadWrite_report_enum.Report_nothing) { Report_class.WriteLine(); } 
        }

        public static void WriteArray<T>(T[] array, string file_name)
        {
            Report_class.WriteLine("{0}: Write array {1}", typeof(T).Name, file_name);
            StreamWriter writer = new StreamWriter(file_name, false);
            PropertyInfo[] propInfo = typeof(T).GetProperties();

            //Generate and write lines
            StringBuilder line = new StringBuilder();
            int array_length = array.Length;
            for (int indexA = 0; indexA < array_length; indexA++)
            {
                writer.WriteLine(array[indexA]);
            }
            writer.Close();
            Report_class.WriteLine();
        }

        public static void WriteArray_as_row<T>(T[] array, char delimiter, string file_name)
        {
            Report_class.WriteLine("{0}: Write array {1} as row", typeof(T).Name, file_name);
            StreamWriter writer = new StreamWriter(file_name, false);
            PropertyInfo[] propInfo = typeof(T).GetProperties();

            //Generate and write lines
            StringBuilder line = new StringBuilder();
            int array_length = array.Length;
            for (int indexA = 0; indexA < array_length; indexA++)
            {
                if (indexA > 0)
                {
                    writer.Write(delimiter);
                }
                writer.Write(array[indexA]);
            }
            writer.Close();
            Report_class.WriteLine();

        }

        public static void WriteArray_in_results_directory<T>(T[] array, string file_name)
        {
            file_name = Global_directory_and_file_class.Results_directory + file_name;
            WriteArray(array, file_name);
        }

        public static void WriteArray_as_row_in_results_directory<T>(T[] array, char delimiter, string file_name)
        {
            file_name = Global_directory_and_file_class.Results_directory + file_name;
            WriteArray_as_row_in_results_directory(array, delimiter, file_name);
        }

        public static void Write_jaggedArray<T>(T[][] array, string file_name)
        {
            StreamWriter writer = new StreamWriter(file_name);
            int array_length = array.Length;
            T[] inner_array;
            int inner_array_length;
            for (int indexA = 0; indexA < array_length; indexA++)
            {
                inner_array = array[indexA];
                inner_array_length = inner_array.Length;
                for (int indexInner = 0; indexInner < inner_array_length; indexInner++)
                {
                    if (indexInner != 0)
                    {
                        writer.Write(Global_class.Tab);
                    }
                    writer.Write(inner_array[indexInner]);
                }
                if (indexA!=array_length-1) { writer.WriteLine(); }
            }
            writer.Close();
        }
        #endregion

        public static T[] Get_array_from_readLine<T>(string readLine, char delimiter)
        {
            string[] split = readLine.Split(delimiter);
            int split_length = split.Length;
            if (string.IsNullOrEmpty(split[split_length - 1])) { split_length--; }
            var TType = typeof(T);
            T[] array = new T[split_length];
            for (int i = 0; i < split_length; i++)
            {
                if (typeof(T).IsEnum)
                {
                    split[i] = char.ToUpper(split[i][0]) + split[i].ToLower().Substring(1);
                    array[i] = (T)Enum.Parse(typeof(T), split[i]);
                }
                else
                {
                    array[i] = (T)Convert.ChangeType(split[i], TType);
                }
            }
            return array;
        }

        public static T[][] Get_jagged_array_from_readLine<T>(string readLine, char row_delimiter, char col_delimiter)
        {
            string[] rows = readLine.Split(row_delimiter);
            int rows_length = rows.Length;
            var TType = typeof(T);
            T[][] jagged_array = new T[rows_length][];
            for (int indexRow = 0; indexRow < rows_length; indexRow++)
            {
                string[] cols = rows[indexRow].Split(col_delimiter);
                int cols_length = cols.Length;
                T[] array = new T[cols_length];
                for (int indexCol = 0; indexCol < cols_length; indexCol++)
                {
                    array[indexCol] = (T)Convert.ChangeType(cols[indexCol], TType);
                }
                jagged_array[indexRow] = array;
            }
            return jagged_array;
        }

        public static List<T> Get_list_from_readLine<T>(string readLine, char delimiter)
        {
            string[] split = readLine.Split(delimiter);
            int split_length = split.Length;
            if (string.IsNullOrEmpty(split[split_length - 1])) { split_length--; }
            var TType = typeof(T);
            List<T> list = new List<T>(split_length);
            for (int i = 0; i < split_length; i++)
            {
                list.Add((T)Convert.ChangeType(split[i], TType));
            }
            return list;
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

        public static string Get_writeLine_with_label_from_array<T>(T[] array, string label, char delimiter)
        {
            StringBuilder stringBuild = new StringBuilder();
            int array_length = array.Length;
            for (int i = 0; i < array_length; i++)
            {
                if (i == 0) { stringBuild.AppendFormat("{0}: {1}", label, array[i]); }
                else { stringBuild.AppendFormat("{0}{1}", delimiter, array[i]); }
            }
            return stringBuild.ToString();
        }

        public static string Get_writeLine_from_list<T>(List<T> list, char delimiter)
        {
            StringBuilder stringBuild = new StringBuilder();
            int list_count = list.Count;
            for (int i = 0; i < list_count; i++)
            {
                if (i == 0) { stringBuild.AppendFormat("{0}", list[i]); }
                else { stringBuild.AppendFormat("{0}{1}", delimiter, list[i]); }
            }
            return stringBuild.ToString();
        }

        public static bool File_exists(string file_name)
        {
            return File.Exists(file_name);
        }

        #region Directory
        public static void Create_directory_if_it_does_not_exist(string complete_directory_name)
        {
            if (!Directory.Exists(complete_directory_name))
            {
                Directory.CreateDirectory(complete_directory_name);
            }
        }
        #endregion
    }

}

