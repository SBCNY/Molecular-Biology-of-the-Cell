using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using Common_functions.Global_definitions;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections;
using System.Reflection;
using yed_network;
using System.CodeDom;
using Enrichment;
using Common_functions.Array_own;
using System.Threading;

namespace Common_functions.Options_base
{
    abstract class Options_readWrite_base_class
    {
        public void Write_entries_excluding_dictionaries(System.IO.StreamWriter writer, Type class_type, params string[] ignore_fields)
        {
            ignore_fields = ignore_fields.Distinct().ToArray();
            List<string> ignored_fields = new List<string>();
            System.Reflection.PropertyInfo[] properties = class_type.GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                if (   (!property.PropertyType.IsArray)
                         && (!typeof(Dictionary<,>).Name.Equals(property.PropertyType.Name))
                         && (property.GetSetMethod() != null)
                         && (property.GetGetMethod() != null))
                {
                    if ((ignore_fields.Contains(property.Name)))
                    {
                        ignored_fields.Add(property.Name);
                    }
                    else
                    {
                        writer.WriteLine(class_type.Name + Global_class.Tab + property.Name + Global_class.Tab + property.GetValue(this));
                    }
                }
            }
            string[] notFound_fields = Overlap_class.Get_part_of_list1_but_not_of_list2(ignore_fields, ignored_fields.ToArray());
            if (ignore_fields.Length!=ignored_fields.Count) { throw new Exception(); }
        }
        public void Write_dictionary_entries(System.IO.StreamWriter writer, Type class_type, Dictionary<Ontology_type_enum, Dictionary<string, string[]>> ontology_classification_members_dict, string parameterSetName)
        {
            char delimiter = Global_class.Tab;
            char array_delimiter = ';';
            Ontology_type_enum[] ontologies = ontology_classification_members_dict.Keys.ToArray();
            int ontologies_length = ontologies.Length;
            Ontology_type_enum ontology_type;
            Dictionary<string, string[]> classification_members_dict;
            string[] classifications;
            string classification;
            int classifications_length;
            string[] members;
            string member;
            int members_length;

            for (int indexO = 0; indexO < ontologies_length; indexO++)
            {
                ontology_type = ontologies[indexO];
                classification_members_dict = ontology_classification_members_dict[ontology_type];
                classifications = classification_members_dict.Keys.ToArray();
                classifications_length = classifications.Length;
                for (int indexClass = 0; indexClass < classifications_length; indexClass++)
                {
                    classification = classifications[indexClass];
                    members = classification_members_dict[classification];
                    writer.Write("{1}{0}{2}{0}{3}{0}{4}", delimiter, class_type.Name, parameterSetName, ontology_type.ToString(), classification);
                    members_length = members.Length;
                    for (int indexMember = 0; indexMember < members_length; indexMember++)
                    {
                        member = members[indexMember];
                        if (indexMember == 0) { writer.Write(delimiter); }
                        else { writer.Write(array_delimiter); }
                        writer.Write(member);
                    }
                    writer.WriteLine();
                }
            }
        }
        public void Write_dictionary_entries(System.IO.StreamWriter writer, Type class_type, Dictionary<Ontology_type_enum, Dictionary<string, int>> ontology_classification_integerNo_dict, string parameterSetName)
        {
            char delimiter = Global_class.Tab;
            Ontology_type_enum[] ontologies = ontology_classification_integerNo_dict.Keys.ToArray();
            int ontologies_length = ontologies.Length;
            Ontology_type_enum ontology_type;
            Dictionary<string, int> classification_integerNo_dict;
            string[] classifications;
            string classification;
            int classifications_length;
            int integerNo;

            for (int indexO = 0; indexO < ontologies_length; indexO++)
            {
                ontology_type = ontologies[indexO];
                classification_integerNo_dict = ontology_classification_integerNo_dict[ontology_type];
                classifications = classification_integerNo_dict.Keys.ToArray();
                classifications_length = classifications.Length;
                for (int indexClass = 0; indexClass < classifications_length; indexClass++)
                {
                    classification = classifications[indexClass];
                    integerNo = classification_integerNo_dict[classification];
                    writer.Write("{1}{0}{2}{0}{3}{0}{4}{0}{5}", delimiter, class_type.Name, parameterSetName, ontology_type.ToString(), classification, integerNo);
                    writer.WriteLine();
                }
            }
        }
        public void Write_dictionary_entries(System.IO.StreamWriter writer, Type class_type, Dictionary<Ontology_type_enum, Dictionary<Yed_network_node_size_determinant_enum, int>> ontology_classification_integerNo_dict, string parameterSetName)
        {
            char delimiter = Global_class.Tab;
            Ontology_type_enum[] ontologies = ontology_classification_integerNo_dict.Keys.ToArray();
            int ontologies_length = ontologies.Length;
            Ontology_type_enum ontology_type;
            Dictionary<Yed_network_node_size_determinant_enum, int> classification_integerNo_dict;
            Yed_network_node_size_determinant_enum[] classifications;
            Yed_network_node_size_determinant_enum classification;
            int classifications_length;
            int integerNo;

            for (int indexO = 0; indexO < ontologies_length; indexO++)
            {
                ontology_type = ontologies[indexO];
                classification_integerNo_dict = ontology_classification_integerNo_dict[ontology_type];
                classifications = classification_integerNo_dict.Keys.ToArray();
                classifications_length = classifications.Length;
                for (int indexClass = 0; indexClass < classifications_length; indexClass++)
                {
                    classification = classifications[indexClass];
                    integerNo = classification_integerNo_dict[classification];
                    writer.Write("{1}{0}{2}{0}{3}{0}{4}{0}{5}", delimiter, class_type.Name, parameterSetName, ontology_type.ToString(), classification, integerNo);
                    writer.WriteLine();
                }
            }
        }
        public void Write_dictionary_entries(System.IO.StreamWriter writer, Type class_type, Dictionary<Yed_network_node_size_determinant_enum, int> nodeSizeDeterminatn_integerNo_dict, string parameterSetName)
        {
            char delimiter = Global_class.Tab;
            Yed_network_node_size_determinant_enum[] nodeSizeDeterminants;
            Yed_network_node_size_determinant_enum nodeSizeDeterminant;
            int integerNo;

            nodeSizeDeterminants = nodeSizeDeterminatn_integerNo_dict.Keys.ToArray();
            int nodeSizeDeterminants_length = nodeSizeDeterminants.Length;
            for (int indexClass = 0; indexClass < nodeSizeDeterminants_length; indexClass++)
            {
                nodeSizeDeterminant = nodeSizeDeterminants[indexClass];
                integerNo = nodeSizeDeterminatn_integerNo_dict[nodeSizeDeterminant];
                writer.Write("{1}{0}{2}{0}{3}{0}{4}", delimiter, class_type.Name, parameterSetName, nodeSizeDeterminant, integerNo);
                writer.WriteLine();
            }
        }
        public void Write_dictionary_entries(System.IO.StreamWriter writer, Type class_type, Dictionary<Ontology_type_enum, float[]> ontology_floatNOs_dict, string parameterSetName)
        {
            char delimiter = Global_class.Tab;
            char array_delimiter = ';';
            Ontology_type_enum[] ontologies = ontology_floatNOs_dict.Keys.ToArray();
            int ontologies_length = ontologies.Length;
            Ontology_type_enum ontology_type;
            float[] floatNOs;
            float floatNO;
            float floatNOs_length;

            for (int indexO = 0; indexO < ontologies_length; indexO++)
            {
                ontology_type = ontologies[indexO];
                floatNOs = ontology_floatNOs_dict[ontology_type];
                floatNOs_length = floatNOs.Length;
                writer.Write("{1}{0}{2}{0}{3}", delimiter, class_type.Name, parameterSetName, ontology_type.ToString());
                for (int indexFloat = 0; indexFloat < floatNOs_length; indexFloat++)
                {
                    floatNO = floatNOs[indexFloat];
                    if (indexFloat == 0) { writer.Write(delimiter); }
                    else { writer.Write(array_delimiter); }
                    writer.Write(floatNO);
                }
                writer.WriteLine();
            }
        }
        public void Write_dictionary_entries(System.IO.StreamWriter writer, Type class_type, Dictionary<Ontology_type_enum, int[]> ontology_integerNOs_dict, string parameterSetName)
        {
            char delimiter = Global_class.Tab;
            char array_delimiter = ';';
            Ontology_type_enum[] ontologies = ontology_integerNOs_dict.Keys.ToArray();
            int ontologies_length = ontologies.Length;
            Ontology_type_enum ontology_type;
            int[] integerNOs;
            int integerNO;
            int integerNOs_length;

            for (int indexO = 0; indexO < ontologies_length; indexO++)
            {
                ontology_type = ontologies[indexO];
                integerNOs = ontology_integerNOs_dict[ontology_type];
                integerNOs_length = integerNOs.Length;
                writer.Write("{1}{0}{2}{0}{3}", delimiter, class_type.Name, parameterSetName, ontology_type.ToString());
                for (int indexFloat = 0; indexFloat < integerNOs_length; indexFloat++)
                {
                    integerNO = integerNOs[indexFloat];
                    if (indexFloat == 0) { writer.Write(delimiter); }
                    else { writer.Write(array_delimiter); }
                    writer.Write(integerNO);
                }
                writer.WriteLine();
            }
        }
        public void Write_dictionary_entries(System.IO.StreamWriter writer, Type class_type, Dictionary<Ontology_type_enum, float> ontology_floatNO_dict, string parameterSetName)
        {
            char delimiter = Global_class.Tab;
            Ontology_type_enum[] ontologies = ontology_floatNO_dict.Keys.ToArray();
            int ontologies_length = ontologies.Length;
            Ontology_type_enum ontology_type;
            float floatNO;

            for (int indexO = 0; indexO < ontologies_length; indexO++)
            {
                ontology_type = ontologies[indexO];
                floatNO = ontology_floatNO_dict[ontology_type];
                writer.Write("{1}{0}{2}{0}{3}{0}{4}", delimiter, class_type.Name, parameterSetName, ontology_type.ToString(), floatNO);
                writer.WriteLine();
            }
        }
        public void Write_dictionary_entries(System.IO.StreamWriter writer, Type class_type, Dictionary<Ontology_type_enum, Dictionary<GO_hyperParameter_enum, int>> ontology_hyperparamerter_intValue_dict, string parameterSetName)
        {
            char delimiter = Global_class.Tab;
            Ontology_type_enum[] ontologies = ontology_hyperparamerter_intValue_dict.Keys.ToArray();
            int ontologies_length = ontologies.Length;
            Ontology_type_enum ontology_type;
            Dictionary<GO_hyperParameter_enum, int> hyperparameter_integerNo_dict;
            GO_hyperParameter_enum[] hyperparameters;
            GO_hyperParameter_enum hyperparameter;
            int hyperparameters_length;
            int integerNo;

            for (int indexO = 0; indexO < ontologies_length; indexO++)
            {
                ontology_type = ontologies[indexO];
                hyperparameter_integerNo_dict = ontology_hyperparamerter_intValue_dict[ontology_type];
                hyperparameters = hyperparameter_integerNo_dict.Keys.ToArray();
                hyperparameters_length = hyperparameters.Length;
                for (int indexHP = 0; indexHP < hyperparameters_length; indexHP++)
                {
                    hyperparameter = hyperparameters[indexHP];
                    integerNo = hyperparameter_integerNo_dict[hyperparameter];
                    writer.Write("{1}{0}{2}{0}{3}{0}{4}{0}{5}", delimiter, class_type.Name, parameterSetName, ontology_type.ToString(), hyperparameter, integerNo);
                    writer.WriteLine();
                }
            }
        }
        public void Write_dictionary_entries(System.IO.StreamWriter writer, Type class_type, Dictionary<Ontology_type_enum, int> ontology_integerNO_dict, string parameterSetName)
        {
            char delimiter = Global_class.Tab;
            Ontology_type_enum[] ontologies = ontology_integerNO_dict.Keys.ToArray();
            int ontologies_length = ontologies.Length;
            Ontology_type_enum ontology_type;
            int integerNO;

            for (int indexO = 0; indexO < ontologies_length; indexO++)
            {
                ontology_type = ontologies[indexO];
                integerNO = ontology_integerNO_dict[ontology_type];
                writer.Write("{1}{0}{2}{0}{3}{0}{4}", delimiter, class_type.Name, parameterSetName, ontology_type.ToString(), integerNO);
                writer.WriteLine();
            }
        }
        public void Write_dictionary_entries(System.IO.StreamWriter writer, Type class_type, Dictionary<Ontology_type_enum, Yed_network_node_size_determinant_enum> ontology_nodeSize_dict, string parameterSetName)
        {
            char delimiter = Global_class.Tab;
            Ontology_type_enum[] ontologies = ontology_nodeSize_dict.Keys.ToArray();
            int ontologies_length = ontologies.Length;
            Ontology_type_enum ontology_type;
            Yed_network_node_size_determinant_enum nodeSize;

            for (int indexO = 0; indexO < ontologies_length; indexO++)
            {
                ontology_type = ontologies[indexO];
                nodeSize = ontology_nodeSize_dict[ontology_type];
                writer.Write("{1}{0}{2}{0}{3}{0}{4}", delimiter, class_type.Name, parameterSetName, ontology_type.ToString(), nodeSize);
                writer.WriteLine();
            }
        }
        public void Write_dictionary_entries(System.IO.StreamWriter writer, Type class_type, Dictionary<Ontology_type_enum, Predicted_scp_hierarchy_integration_strategy_enum> ontology_parentChildScpNetworkGeneration_dict, string parameterSetName)
        {
            char delimiter = Global_class.Tab;
            Ontology_type_enum[] ontologies = ontology_parentChildScpNetworkGeneration_dict.Keys.ToArray();
            int ontologies_length = ontologies.Length;
            Ontology_type_enum ontology_type;
            Predicted_scp_hierarchy_integration_strategy_enum pcsn_generation;

            for (int indexO = 0; indexO < ontologies_length; indexO++)
            {
                ontology_type = ontologies[indexO];
                pcsn_generation = ontology_parentChildScpNetworkGeneration_dict[ontology_type];
                writer.Write("{1}{0}{2}{0}{3}{0}{4}", delimiter, class_type.Name, parameterSetName, ontology_type.ToString(), pcsn_generation);
                writer.WriteLine();
            }
        }
        public void Write_dictionary_entries(System.IO.StreamWriter writer, Type class_type, Dictionary<Ontology_type_enum, SCP_hierarchy_interaction_type_enum> ontology_parentChildScpNetworkGeneration_dict, string parameterSetName)
        {
            char delimiter = Global_class.Tab;
            Ontology_type_enum[] ontologies = ontology_parentChildScpNetworkGeneration_dict.Keys.ToArray();
            int ontologies_length = ontologies.Length;
            Ontology_type_enum ontology_type;
            SCP_hierarchy_interaction_type_enum hierarchical_interaction;

            for (int indexO = 0; indexO < ontologies_length; indexO++)
            {
                ontology_type = ontologies[indexO];
                hierarchical_interaction = ontology_parentChildScpNetworkGeneration_dict[ontology_type];
                writer.Write("{1}{0}{2}{0}{3}{0}{4}", delimiter, class_type.Name, parameterSetName, ontology_type.ToString(), hierarchical_interaction);
                writer.WriteLine();
            }
        }
        public void Write_dictionary_entries(System.IO.StreamWriter writer, Type class_type, Dictionary<Ontology_type_enum, bool> ontology_boolVar_dict, string parameterSetName)
        {
            char delimiter = Global_class.Tab;
            Ontology_type_enum[] ontologies = ontology_boolVar_dict.Keys.ToArray();
            int ontologies_length = ontologies.Length;
            Ontology_type_enum ontology_type;
            bool boolVar;

            for (int indexO = 0; indexO < ontologies_length; indexO++)
            {
                ontology_type = ontologies[indexO];
                boolVar = ontology_boolVar_dict[ontology_type];
                writer.Write("{1}{0}{2}{0}{3}{0}{4}", delimiter, class_type.Name, parameterSetName, ontology_type.ToString(), boolVar);
                writer.WriteLine();
            }
        }
        public bool Add_read_entry_and_return_if_succesful(string readLine, Type class_type)
        {
            string[] splitStrings = readLine.Split(Global_class.Tab);
            string classType_string = splitStrings[0];
            string valueType_string = splitStrings[1];
            string value_string = splitStrings[2];
            bool successful = false;
            if (classType_string.Equals(class_type.Name))
            {
                foreach (System.Reflection.PropertyInfo property in class_type.GetProperties())
                {
                    if (property.Name.Equals(valueType_string))
                    {
                        if (!String.IsNullOrEmpty(value_string))
                        {
                            if (property.PropertyType.IsEnum)
                            {
                                try
                                {
                                    property.SetValue(this, Enum.Parse(property.PropertyType, value_string), null);
                                    successful = true;
                                }
                                catch
                                {
                                    throw new Exception();
                                }
                            }
                            else if (property.PropertyType.Equals(typeof(ChartImageFormat)))
                            {
                                ChartImageFormat[] suggested_image_formats = new ChartImageFormat[] { ChartImageFormat.Gif, ChartImageFormat.Jpeg, ChartImageFormat.Png, ChartImageFormat.Gif, ChartImageFormat.Tiff };
                                bool value_set = false;
                                foreach (ChartImageFormat suggested_image_format in suggested_image_formats)
                                {
                                    if (value_string.Equals(suggested_image_format.ToString()))
                                    {
                                        property.SetValue(this, suggested_image_format, null);
                                        value_set = true;
                                        successful = true;
                                        break;
                                    }
                                }
                                if (!value_set)
                                {
                                    throw new Exception();
                                }
                            }
                            else
                            {
                                try
                                { 
                                    property.SetValue(this, Convert.ChangeType(value_string, property.PropertyType));
                                    successful = true;
                                }
                                catch
                                {
                                    throw new Exception();
                                }
                            }
                        }
                    }
                }
            }
            return successful;
        }
        public Dictionary<Ontology_type_enum, Dictionary<string, string[]>> Add_to_dictionary_entries(string readLine, Dictionary<Ontology_type_enum, Dictionary<string, string[]>> ontology_classification_members_dict)
        {
            char array_delimiter = ';';
            char delimiter = Global_class.Tab;
            string[] splitStrings = readLine.Split(delimiter);
            Ontology_type_enum ontology = (Ontology_type_enum)Enum.Parse(typeof(Ontology_type_enum), splitStrings[2]);
            string classification = splitStrings[3];
            string[] members = new string[0];
            if (splitStrings.Length == 5)
            { 
                string members_string = splitStrings[4];
                members = members_string.Split(array_delimiter);
            }
            if (!ontology_classification_members_dict.ContainsKey(ontology))
            { ontology_classification_members_dict.Add(ontology, new Dictionary<string, string[]>()); }
            if (!ontology_classification_members_dict[ontology].ContainsKey(classification)) { ontology_classification_members_dict[ontology].Add(classification, members); }
            ontology_classification_members_dict[ontology][classification] = members;
            return ontology_classification_members_dict;
        }
        public Dictionary<Ontology_type_enum, Dictionary<string, int>> Add_to_dictionary_entries(string readLine, Dictionary<Ontology_type_enum, Dictionary<string, int>> ontology_classification_integerNo_dict)
        {
            char delimiter = Global_class.Tab;
            string[] splitStrings = readLine.Split(delimiter);
            Ontology_type_enum ontology = (Ontology_type_enum)Enum.Parse(typeof(Ontology_type_enum), splitStrings[2]);
            string classification = splitStrings[3];
            if (!ontology_classification_integerNo_dict.ContainsKey(ontology))
            { ontology_classification_integerNo_dict.Add(ontology, new Dictionary<string, int>()); }
            string integerNo_string = splitStrings[4];
            int integerNo;
            if (int.TryParse(integerNo_string, out integerNo))
            {
                if (!ontology_classification_integerNo_dict[ontology].ContainsKey(classification))
                {
                    ontology_classification_integerNo_dict[ontology].Add(classification, integerNo);
                }
                ontology_classification_integerNo_dict[ontology][classification] = integerNo;
            }
            return ontology_classification_integerNo_dict;
        }
        public Dictionary<Ontology_type_enum, Dictionary<Yed_network_node_size_determinant_enum, int>> Add_to_dictionary_entries(string readLine, Dictionary<Ontology_type_enum, Dictionary<Yed_network_node_size_determinant_enum, int>> ontology_classification_integerNo_dict)
        {
            char delimiter = Global_class.Tab;
            string[] splitStrings = readLine.Split(delimiter);
            Ontology_type_enum ontology = (Ontology_type_enum)Enum.Parse(typeof(Ontology_type_enum), splitStrings[2]);
            Yed_network_node_size_determinant_enum classification = (Yed_network_node_size_determinant_enum)Enum.Parse(typeof(Yed_network_node_size_determinant_enum), splitStrings[3]);
            if (!ontology_classification_integerNo_dict.ContainsKey(ontology))
            { ontology_classification_integerNo_dict.Add(ontology, new Dictionary<Yed_network_node_size_determinant_enum, int>()); }
            string integerNo_string = splitStrings[4];
            int integerNo;
            if (int.TryParse(integerNo_string, out integerNo))
            {
                if (!ontology_classification_integerNo_dict[ontology].ContainsKey(classification))
                {
                    ontology_classification_integerNo_dict[ontology].Add(classification, integerNo);
                }
                ontology_classification_integerNo_dict[ontology][classification] = integerNo;
            }
            return ontology_classification_integerNo_dict;
        }
        public Dictionary<Ontology_type_enum, Dictionary<GO_hyperParameter_enum, int>> Add_to_dictionary_entries(string readLine, Dictionary<Ontology_type_enum, Dictionary<GO_hyperParameter_enum, int>> ontology_hyperparameter_integerNo_dict)
        {
            char delimiter = Global_class.Tab;
            string[] splitStrings = readLine.Split(delimiter);
            Ontology_type_enum ontology = (Ontology_type_enum)Enum.Parse(typeof(Ontology_type_enum), splitStrings[2]);
            GO_hyperParameter_enum hyperparameter = (GO_hyperParameter_enum)Enum.Parse(typeof(GO_hyperParameter_enum), splitStrings[3]);
            if (!ontology_hyperparameter_integerNo_dict.ContainsKey(ontology))
            { ontology_hyperparameter_integerNo_dict.Add(ontology, new Dictionary<GO_hyperParameter_enum, int>()); }
            string integerNo_string = splitStrings[4];
            int integerNo;
            if (int.TryParse(integerNo_string, out integerNo))
            {
                if (!ontology_hyperparameter_integerNo_dict[ontology].ContainsKey(hyperparameter))
                {
                    ontology_hyperparameter_integerNo_dict[ontology].Add(hyperparameter, integerNo);
                }
                ontology_hyperparameter_integerNo_dict[ontology][hyperparameter] = integerNo;
            }
            return ontology_hyperparameter_integerNo_dict;
        }
        public Dictionary<Yed_network_node_size_determinant_enum, int> Add_to_dictionary_entries(string readLine, Dictionary<Yed_network_node_size_determinant_enum, int> nodeSizeDeterminant_integerNo_dict)
        {
            char delimiter = Global_class.Tab;
            string[] splitStrings = readLine.Split(delimiter);
            Yed_network_node_size_determinant_enum nodeSizeDeterminant = (Yed_network_node_size_determinant_enum)Enum.Parse(typeof(Yed_network_node_size_determinant_enum), splitStrings[2]);
            string integerNo_string = splitStrings[3];
            int integerNo;
            if (int.TryParse(integerNo_string, out integerNo))
            {
                if (!nodeSizeDeterminant_integerNo_dict.ContainsKey(nodeSizeDeterminant))
                {
                    nodeSizeDeterminant_integerNo_dict.Add(nodeSizeDeterminant, integerNo);
                }
                nodeSizeDeterminant_integerNo_dict[nodeSizeDeterminant] = integerNo;
            }
            return nodeSizeDeterminant_integerNo_dict;
        }
        public Dictionary<Ontology_type_enum, int[]> Add_to_dictionary_entries(string readLine, Dictionary<Ontology_type_enum, int[]> ontology_integerNOs_dict)
        {
            char array_delimiter = ';';
            char delimiter = Global_class.Tab;
            string[] splitStrings = readLine.Split(delimiter);
            Ontology_type_enum ontology = (Ontology_type_enum)Enum.Parse(typeof(Ontology_type_enum), splitStrings[2]);
            string integerNoCombinedString = splitStrings[3];
            string[] integerNosStrings = integerNoCombinedString.Split(array_delimiter);
            string integerNoString;
            int integerNOs_length = integerNosStrings.Length;
            int[] integerNOs = new int[integerNOs_length];
            int integerNo;
            for (int indexNO = 0; indexNO < integerNOs_length; indexNO++)
            {
                integerNoString = integerNosStrings[indexNO];
                if (int.TryParse(integerNoString, out integerNo))
                {
                    if (!ontology_integerNOs_dict.ContainsKey(ontology))
                    {
                        ontology_integerNOs_dict.Add(ontology, new int[5]);
                    }
                    ontology_integerNOs_dict[ontology][indexNO] = integerNo;
                }
            }
            return ontology_integerNOs_dict;
        }
        public Dictionary<Ontology_type_enum, float[]> Add_to_dictionary_entries(string readLine, Dictionary<Ontology_type_enum, float[]> ontology_floatNOs_dict)
        {
            char array_delimiter = ';';
            char delimiter = Global_class.Tab;
            string[] splitStrings = readLine.Split(delimiter);
            Ontology_type_enum ontology = (Ontology_type_enum)Enum.Parse(typeof(Ontology_type_enum), splitStrings[2]);
            string floatNoCombinedString = splitStrings[3];
            string[] floatNosStrings = floatNoCombinedString.Split(array_delimiter);
            string floatNoString;
            int floatNOs_length = floatNosStrings.Length;
            float[] flaotNOs = new float[floatNOs_length];
            float floatNo;
            for (int indexNO = 0; indexNO < floatNOs_length; indexNO++)
            {
                floatNoString = floatNosStrings[indexNO];
                if (float.TryParse(floatNoString, out floatNo))
                {
                    if (!ontology_floatNOs_dict.ContainsKey(ontology))
                    {
                        ontology_floatNOs_dict.Add(ontology, new float[5]);
                    }
                    ontology_floatNOs_dict[ontology][indexNO] = floatNo;
                }
            }
            return ontology_floatNOs_dict;
        }
        public Dictionary<Ontology_type_enum, float> Add_to_dictionary_entries(string readLine, Dictionary<Ontology_type_enum, float> ontology_floatNO_dict)
        {
            char delimiter = Global_class.Tab;
            string[] splitStrings = readLine.Split(delimiter);
            Ontology_type_enum ontology = (Ontology_type_enum)Enum.Parse(typeof(Ontology_type_enum), splitStrings[2]);
            string floatNoString = splitStrings[3];
            float floatNo;
            if (float.TryParse(floatNoString, out floatNo))
            {
                if (!ontology_floatNO_dict.ContainsKey(ontology))
                {
                    ontology_floatNO_dict.Add(ontology, floatNo);
                }
                ontology_floatNO_dict[ontology] = floatNo;
            }
            return ontology_floatNO_dict;
        }
        public Dictionary<Ontology_type_enum, bool> Add_to_dictionary_entries(string readLine, Dictionary<Ontology_type_enum, bool> ontology_boolValue_dict)
        {
            char delimiter = Global_class.Tab;
            string[] splitStrings = readLine.Split(delimiter);
            Ontology_type_enum ontology = (Ontology_type_enum)Enum.Parse(typeof(Ontology_type_enum), splitStrings[2]);
            bool boolValue = bool.Parse(splitStrings[3]);
            if (!ontology_boolValue_dict.ContainsKey(ontology))
            {
                ontology_boolValue_dict.Add(ontology, boolValue);
            }
            ontology_boolValue_dict[ontology] = boolValue;
            return ontology_boolValue_dict;
        }
        public Dictionary<Ontology_type_enum, Yed_network_node_size_determinant_enum> Add_to_dictionary_entries(string readLine, Dictionary<Ontology_type_enum, Yed_network_node_size_determinant_enum> ontology_nodeSizeDeterminant_dict)
        {
            char delimiter = Global_class.Tab;
            string[] splitStrings = readLine.Split(delimiter);
            Ontology_type_enum ontology = (Ontology_type_enum)Enum.Parse(typeof(Ontology_type_enum), splitStrings[2]);
            Yed_network_node_size_determinant_enum nodeSizeDeterminant = (Yed_network_node_size_determinant_enum)Enum.Parse(typeof(Yed_network_node_size_determinant_enum), splitStrings[3]);
            if (!ontology_nodeSizeDeterminant_dict.ContainsKey(ontology))
            {
                ontology_nodeSizeDeterminant_dict.Add(ontology, nodeSizeDeterminant);
            }
            ontology_nodeSizeDeterminant_dict[ontology] = nodeSizeDeterminant;
            return ontology_nodeSizeDeterminant_dict;
        }
        public Dictionary<Ontology_type_enum, Predicted_scp_hierarchy_integration_strategy_enum> Add_to_dictionary_entries(string readLine, Dictionary<Ontology_type_enum, Predicted_scp_hierarchy_integration_strategy_enum> ontology_parentChildScpNetworkGeneration_dict)
        {
            char delimiter = Global_class.Tab;
            string[] splitStrings = readLine.Split(delimiter);
            Ontology_type_enum ontology = (Ontology_type_enum)Enum.Parse(typeof(Ontology_type_enum), splitStrings[2]);
            Predicted_scp_hierarchy_integration_strategy_enum pcsn_generation = (Predicted_scp_hierarchy_integration_strategy_enum)Enum.Parse(typeof(Predicted_scp_hierarchy_integration_strategy_enum), splitStrings[3]);
            if (!ontology_parentChildScpNetworkGeneration_dict.ContainsKey(ontology))
            {
                ontology_parentChildScpNetworkGeneration_dict.Add(ontology, pcsn_generation);
            }
            ontology_parentChildScpNetworkGeneration_dict[ontology] = pcsn_generation;
            return ontology_parentChildScpNetworkGeneration_dict;
        }
        public Dictionary<Ontology_type_enum, SCP_hierarchy_interaction_type_enum> Add_to_dictionary_entries(string readLine, Dictionary<Ontology_type_enum, SCP_hierarchy_interaction_type_enum> ontology_parentChildScpNetworkGeneration_dict)
        {
            char delimiter = Global_class.Tab;
            string[] splitStrings = readLine.Split(delimiter);
            Ontology_type_enum ontology = (Ontology_type_enum)Enum.Parse(typeof(Ontology_type_enum), splitStrings[2]);
            SCP_hierarchy_interaction_type_enum hierarchical_interaction = (SCP_hierarchy_interaction_type_enum)Enum.Parse(typeof(SCP_hierarchy_interaction_type_enum), splitStrings[3]);
            if (!ontology_parentChildScpNetworkGeneration_dict.ContainsKey(ontology))
            {
                ontology_parentChildScpNetworkGeneration_dict.Add(ontology, hierarchical_interaction);
            }
            ontology_parentChildScpNetworkGeneration_dict[ontology] = hierarchical_interaction;
            return ontology_parentChildScpNetworkGeneration_dict;
        }



        public abstract void Write_option_entries(System.IO.StreamWriter writer);
        public abstract bool Add_read_entry_to_options_and_return_if_successful(string readLine);
    }

}
