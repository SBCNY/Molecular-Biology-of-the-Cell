//The code was written by Jens Hansen working for the Ravi Iyengar Lab
//The copyright holder for this preprint is the author/funder. It is made available under a creative commons 4.0 International license (CC BY 4.0).
//Please acknowledge Molecular Biology of the Cell Ontology (MBCO) in your publications by citing the following reference:
//Jens Hansen, David Meretzky, Simeneh Woldesenbet, Gustavo Stolovitzky, Ravi Iyengar.
//A flexible ontology for inference of emergent whole cell function from relationships between subcellular processes.
//Sci Rep. 2017 Dec 18; 7(1):17689. PMID: 29255142

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel.Security;
using System.Web;
using System.Windows.Forms;
using ClassLibrary1.Read_interface;
using Common_functions.Global_definitions;

namespace Common_functions.Array_own
{
    class Array_class
    {
        public static bool Arrays_are_equal<T>(T[] array0, T[] array1)
        {
            bool equal = true;
            int array0_length = array0.Length;
            int array1_length = array1.Length;
            if (array0_length != array1_length) {  equal = false; }
            else
            {
                for (int indexArray=0; indexArray<array0.Length; indexArray++) 
                {
                    if (!array0[indexArray].Equals(array1[indexArray])) { equal = false; break;}
                }
            }
            return equal;
        }
        public static string[] Deep_copy_string_array(string[] array)
        {
            int array_length = array.Length;
            string[] copy = new string[array_length];
            for (int indexA = 0; indexA < array_length; indexA++)
            {
                copy[indexA] = (string)array[indexA].Clone();
            }
            return copy;
        }
        public static T[] Deep_copy_array<T>(T[] array)
        {
            T[] copy = new T[0];
            if (typeof(T).Equals(typeof(string))) { throw new Exception(); }
            if (array != null)
            {
                int array_length = array.Length;
                copy = new T[array_length];
                for (int indexA = 0; indexA < array_length; indexA++)
                {
                    copy[indexA] = array[indexA];
                }
            }
            else
            {
                copy = array;
            }
            return copy;
        }

        #region Deep copy dictionaries
        public static Dictionary<T1, T2[]> Deep_copy_dictionary_that_contains_no_strings<T1, T2>(Dictionary<T1, T2[]> dictionary)
        {
            if (typeof(T1).Equals(typeof(string))) { throw new Exception(); }
            if (typeof(T2).Equals(typeof(string))) { throw new Exception(); }
            Dictionary<T1, T2[]> copy = new Dictionary<T1, T2[]>();
            T1[] t1_keys = dictionary.Keys.ToArray();
            T1 t1_key;
            int t1_keys_length = t1_keys.Length;
            for (int indexT1Key = 0; indexT1Key < t1_keys_length; indexT1Key++)
            {
                t1_key = t1_keys[indexT1Key];
                copy.Add(t1_key, new T2[0]);
                copy[t1_key] = Array_class.Deep_copy_array(dictionary[t1_key]);
            }
            return copy;
        }
        public static Dictionary<T1, T2> Deep_copy_dictionary_that_contains_no_strings<T1, T2>(Dictionary<T1, T2> dictionary)
        {
            if (typeof(T1).Equals(typeof(string))) { throw new Exception(); }
            if (typeof(T2).Equals(typeof(string))) { throw new Exception(); }
            Dictionary<T1, T2> copy = new Dictionary<T1, T2>();
            T1[] t1_keys = dictionary.Keys.ToArray();
            T1 t1_key;
            int t1_keys_length = t1_keys.Length;
            for (int indexT1Key = 0; indexT1Key < t1_keys_length; indexT1Key++)
            {
                t1_key = t1_keys[indexT1Key];
                copy.Add(t1_key, dictionary[t1_key]);
            }
            return copy;
        }
        public static Dictionary<string, string[]> Deep_copy_dictionary_with_stringKey_stringValueArray(Dictionary<string, string[]> dictionary)
        {
            Dictionary<string, string[]> copy = new Dictionary<string, string[]>();
            string[] keys = dictionary.Keys.ToArray();
            string key;
            int keys_length = keys.Length;
            for (int indexKey = 0; indexKey < keys_length; indexKey++)
            {
                key = keys[indexKey];
                copy.Add((string)key.Clone(), new string[0]);
                copy[key] = Deep_copy_string_array(dictionary[key]);
            }
            return copy;
        }
        public static Dictionary<string, string> Deep_copy_dictionary_with_stringKeysValues(Dictionary<string, string> dictionary)
        {
            Dictionary<string, string> copy = new Dictionary<string, string>();
            string[] keys = dictionary.Keys.ToArray();
            string key;
            int keys_length = keys.Length;
            for (int indexKey = 0; indexKey < keys_length; indexKey++)
            {
                key = keys[indexKey];
                copy.Add((string)key.Clone(), (string)dictionary[key]);
            }
            return copy;
        }
        public static Dictionary<T, string> Deep_copy_dictionary<T>(Dictionary<T, string> dictionary)
        {
            if (typeof(T) == typeof(string)) throw new InvalidOperationException("T must not be a string.");
            Dictionary<T, string> copy = new Dictionary<T, string>();
            T[] keys = dictionary.Keys.ToArray();
            T key;
            int keys_length = keys.Length;
            for (int indexKey = 0; indexKey < keys_length; indexKey++)
            {
                key = keys[indexKey];
                copy.Add(key, (string)dictionary[key].Clone());
            }
            return copy;
        }
        public static Dictionary<T1, T2> Deep_copy_dictionary<T1,T2>(Dictionary<T1, T2> dictionary)
        {
            if (typeof(T1) == typeof(string)) throw new InvalidOperationException("T1 must not be a string.");
            if (typeof(T2) == typeof(string)) throw new InvalidOperationException("T2 must not be a string.");
            Dictionary<T1, T2> copy = new Dictionary<T1, T2>();
            T1[] keys = dictionary.Keys.ToArray();
            T1 key;
            int keys_length = keys.Length;
            for (int indexKey = 0; indexKey < keys_length; indexKey++)
            {
                key = keys[indexKey];
                copy.Add(key, dictionary[key]);
            }
            return copy;
        }
        public static Dictionary<T, string[]> Deep_copy_dictionary_with_array_stringValue<T>(Dictionary<T, string[]> dictionary)
        {
            if (typeof(T) == typeof(string)) throw new InvalidOperationException("T must not be a string.");
            Dictionary<T, string[]> copy = new Dictionary<T, string[]>();
            T[] keys = dictionary.Keys.ToArray();
            T key;
            int keys_length = keys.Length;
            string[] values;
            for (int indexKey = 0; indexKey < keys_length; indexKey++)
            {
                key = keys[indexKey];
                values = Deep_copy_string_array(dictionary[key]);
                copy.Add(key, values);
            }
            return copy;
        }
        public static Dictionary<T1, T2[]> Deep_copy_dictionary_with_array_value<T1,T2>(Dictionary<T1, T2[]> dictionary)
        {
            if (typeof(T1) == typeof(string)) throw new InvalidOperationException("T1 must not be a string.");
            if (typeof(T2) == typeof(string)) throw new InvalidOperationException("T2 must not be a string.");
            Dictionary<T1, T2[]> copy = new Dictionary<T1, T2[]>();
            T1[] t1_keys = dictionary.Keys.ToArray();
            T1 t1_key;
            int keys_length = t1_keys.Length;
            T2[] values;
            for (int indexKey = 0; indexKey < keys_length; indexKey++)
            {
                t1_key = t1_keys[indexKey];
                values = Deep_copy_array(dictionary[t1_key]);
                copy.Add(t1_key, values);
            }
            return copy;
        }
        public static Dictionary<T, float> Deep_copy_dictionary<T>(Dictionary<T, float> dictionary)
        {
            if (typeof(T) == typeof(string)) throw new InvalidOperationException("T must not be a string.");
            Dictionary<T, float> copy = new Dictionary<T, float>();
            T[] keys = dictionary.Keys.ToArray();
            T key;
            int keys_length = keys.Length;
            for (int indexKey = 0; indexKey < keys_length; indexKey++)
            {
                key = keys[indexKey];
                copy.Add(key, dictionary[key]);
            }
            return copy;
        }
        public static Dictionary<T, Dictionary<string, string[]>> Deep_copy_nested_dictionary_with_final_stringKeys_stringValueArrays<T>(Dictionary<T, Dictionary<string, string[]>> dictionary)
        {
            if (typeof(T).Equals(typeof(string))) { throw new Exception(); }
            Dictionary<T, Dictionary<string, string[]>> copy = new Dictionary<T, Dictionary<string, string[]>>();
            T[] t_keys = dictionary.Keys.ToArray();
            T t_key;
            int t_keys_length = t_keys.Length;
            for (int indexTKey = 0; indexTKey < t_keys_length; indexTKey++)
            {
                t_key = t_keys[indexTKey];
                copy.Add(t_key, new Dictionary<string, string[]>());
                copy[t_key] = Deep_copy_dictionary_with_stringKey_stringValueArray(copy[t_key]);
            }
            return copy;
        }
        public static Dictionary<T1, Dictionary<T2, string[]>> Deep_copy_nested_dictionary_with_final_stringArrayValues<T1, T2>(Dictionary<T1, Dictionary<T2, string[]>> dictionary)
        {
            if (typeof(T1).Equals(typeof(string))) { throw new Exception("T1 must not be a string"); }
            if (typeof(T2).Equals(typeof(string))) { throw new Exception("T2 must not be a string"); }
            Dictionary<T1, Dictionary<T2, string[]>> copy = new Dictionary<T1, Dictionary<T2, string[]>>();
            T1[] t1_keys = dictionary.Keys.ToArray();
            T1 t1_key;
            int t1_keys_length = t1_keys.Length;
            for (int indexTKey = 0; indexTKey < t1_keys_length; indexTKey++)
            {
                t1_key = t1_keys[indexTKey];
                copy.Add(t1_key, new Dictionary<T2, string[]>());
                copy[t1_key] = Deep_copy_dictionary_with_array_stringValue(dictionary[t1_key]);
            }
            return copy;
        }
        public static Dictionary<T1, Dictionary<T2, T3>> Deep_copy_nested_dictionary_with_final_value<T1, T2, T3>(Dictionary<T1, Dictionary<T2, T3>> dictionary)
        {
            if (typeof(T1).Equals(typeof(string))) { throw new Exception("T1 must not be a string"); }
            if (typeof(T2).Equals(typeof(string))) { throw new Exception("T2 must not be a string"); }
            if (typeof(T3).Equals(typeof(string))) { throw new Exception("T3 must not be a string"); }
            Dictionary<T1, Dictionary<T2, T3>> copy = new Dictionary<T1, Dictionary<T2, T3>>();
            T1[] t1_keys = dictionary.Keys.ToArray();
            T1 t1_key;
            int t1_keys_length = t1_keys.Length;
            for (int indexTKey = 0; indexTKey < t1_keys_length; indexTKey++)
            {
                t1_key = t1_keys[indexTKey];
                copy.Add(t1_key, new Dictionary<T2, T3>());
                copy[t1_key] = Deep_copy_dictionary(dictionary[t1_key]);
            }
            return copy;
        }
        #endregion
        public static T[] Get_ordered_union<T>(T[] array1, params T[] array2) where T : IComparable<T>
        {
            int array1_length = array1.Length;
            int array2_length = array2.Length;
            int combined_array_length = array1_length + array2_length;
            int indexC = -1;
            T[] combined_array = new T[combined_array_length];
            for (int index1 = 0; index1 < array1_length; index1++)
            {
                indexC++;
                combined_array[indexC] = array1[index1];
            }
            for (int index2 = 0; index2 < array2_length; index2++)
            {
                indexC++;
                combined_array[indexC] = array2[index2];
            }
            return combined_array.Distinct().OrderBy(l => l).ToArray();

        }
    }

    class Dictionary_class
    {
        public static Dictionary<T2, T1> Reverse_dictionary<T1, T2>(Dictionary<T1, T2> toBeReversed_dict)
        {
            Dictionary<T2, T1> reversed_dictionary = new Dictionary<T2, T1>();
            T1[] toBeReversed_keys = toBeReversed_dict.Keys.ToArray();
            int topBeReversed_keys_length = toBeReversed_keys.Length;
            T1 toBeReversed_key;
            T2 new_key;
            for (int indexToBeRev = 0; indexToBeRev < topBeReversed_keys_length; indexToBeRev++)
            {
                toBeReversed_key = toBeReversed_keys[indexToBeRev];
                new_key = toBeReversed_dict[toBeReversed_key];
                reversed_dictionary.Add(new_key, toBeReversed_key);
            }
            return reversed_dictionary;
        }

        public static Dictionary<T2, T1[]> Reverse_dictionary_with_overlapping_values<T1, T2>(Dictionary<T1, T2> toBeReversed_dict)
        {
            Dictionary<T2, List<T1>> reversed_dictionary = new Dictionary<T2, List<T1>>();
            T1[] toBeReversed_keys = toBeReversed_dict.Keys.ToArray();
            int topBeReversed_keys_length = toBeReversed_keys.Length;
            T1 toBeReversed_key;
            T2 new_key;
            for (int indexToBeRev = 0; indexToBeRev < topBeReversed_keys_length; indexToBeRev++)
            {
                toBeReversed_key = toBeReversed_keys[indexToBeRev];
                new_key = toBeReversed_dict[toBeReversed_key];
                if (!reversed_dictionary.ContainsKey(new_key)) {  reversed_dictionary.Add(new_key, new List<T1>()); }
                reversed_dictionary[new_key].Add(toBeReversed_key);
            }
            T2[] new_keys = reversed_dictionary.Keys.ToArray();
            Dictionary<T2, T1[]> final_reversed_dictionary = new Dictionary<T2, T1[]>();
            foreach (T2 new_key2 in new_keys)
            {
                final_reversed_dictionary.Add(new_key2, reversed_dictionary[new_key2].ToArray());
            }

            return final_reversed_dictionary;
        }

        public static Dictionary<string, string[]> Reverse_dictionary(Dictionary<string, string[]> toBeReversed_dict)
        {
            Dictionary<string, List<string>> reversed_list_dictionary = new Dictionary<string, List<string>>();
            string[] toBeReversed_keys = toBeReversed_dict.Keys.ToArray();
            int topBeReversed_keys_length = toBeReversed_keys.Length;
            string toBeReversed_key;
            string[] new_keys;
            string new_key;
            int new_keys_length;
            for (int indexToBeRev = 0; indexToBeRev < topBeReversed_keys_length; indexToBeRev++)
            {
                toBeReversed_key = toBeReversed_keys[indexToBeRev];
                new_keys = toBeReversed_dict[toBeReversed_key];
                new_keys_length = new_keys.Length;
                for (int indexKey =0; indexKey<new_keys_length;indexKey++)
                {
                    new_key = new_keys[indexKey];
                    if (!reversed_list_dictionary.ContainsKey(new_key))
                    { reversed_list_dictionary.Add(new_key, new List<string>()); }
                    reversed_list_dictionary[new_key].Add(toBeReversed_key);
                }
            }
            new_keys = reversed_list_dictionary.Keys.ToArray();
            Dictionary<string, string[]> reversed_dictionary = new Dictionary<string, string[]>();
            new_keys_length = new_keys.Length;
            for (int indexNewKey=0; indexNewKey<new_keys_length; indexNewKey++)
            {
                new_key = new_keys[indexNewKey];
                reversed_dictionary.Add(new_key, reversed_list_dictionary[new_key].ToArray());
            }
            return reversed_dictionary;
        }

    }

    class Overlap_class
    {
        public static string[] Get_ordered_intersection(string[] list1, params string[] list2)
        {
            Dictionary<string, bool> list1_dict = new Dictionary<string, bool>();
            foreach (string list1_string in list1)
            {
                if (!list1_dict.ContainsKey(list1_string))
                {
                    list1_dict.Add(list1_string, true);
                }
            }
            List<string> intersection = new List<string>();
            foreach (string list2_string in list2)
            {
                if (list1_dict.ContainsKey(list2_string))
                {
                    intersection.Add(list2_string);
                }
            }
            return intersection.OrderBy(l => l).ToArray();
        }
        public static T[] Get_ordered_intersection<T>(T[] list1, params T[] list2) where T : IComparable
        {
            Dictionary<T, bool> list1_dict = new Dictionary<T, bool>();
            foreach (T list1_string in list1)
            {
                if (!list1_dict.ContainsKey(list1_string))
                {
                    list1_dict.Add(list1_string, true);
                }
            }
            List<T> intersection = new List<T>();
            foreach (T list2_string in list2)
            {
                if (list1_dict.ContainsKey(list2_string))
                {
                    intersection.Add(list2_string);
                }
            }
            return intersection.OrderBy(l => l).ToArray();
        }
        public static string[] Get_union_of_string_arrays_keeping_the_order(string[] list1, params string[] list2)
        {
            Dictionary<string, bool> string_dict = new Dictionary<string, bool>();
            foreach (string string1 in list1)
            {
                if (!string_dict.ContainsKey(string1)) { string_dict.Add(string1, true); }
            }
            foreach (string string2 in list2)
            {
                if (!string_dict.ContainsKey(string2)) { string_dict.Add(string2, true); }
            }
            return string_dict.Keys.ToArray();
        }
        public static string[] Get_ordered_union_of_string_arrays(string[] list1, params string[] list2)
        {
            return Get_union_of_string_arrays_keeping_the_order(list1, list2).OrderBy(l => l).ToArray();
        }
        public static T[] Get_union_of_T_arrays<T>(T[] list1, params T[] list2) where T: IComparable
        {
            Dictionary<T, bool> var_dict = new Dictionary<T, bool>();
            foreach (T var1 in list1)
            {
                if (!var_dict.ContainsKey(var1)) { var_dict.Add(var1, true); }
            }
            foreach (T string2 in list2)
            {
                if (!var_dict.ContainsKey(string2)) { var_dict.Add(string2, true); }
            }
            return var_dict.Keys.OrderBy(l => l).ToArray();
        }
        public static string[] Get_part_of_list1_but_not_of_list2(string[] list1, params string[] list2)
        {
            list1 = list1.Distinct().OrderBy(l => l).ToArray();
            list2 = list2.Distinct().OrderBy(l => l).ToArray();
            List<string> not = new List<string>();
            int list1_length = list1.Length;
            int list2_length = list2.Length;
            int index2 = 0;
            int stringCompare;
            for (int index1 = 0; index1 < list1_length; index1++)
            {
                stringCompare = -2;
                while ((index2 < list2_length) && (stringCompare < 0))
                {
                    stringCompare = list2[index2].CompareTo(list1[index1]);
                    if (stringCompare < 0) { index2++; }
                }
                if ((stringCompare > 0) || (index2 == list2_length))
                {
                    not.Add(list1[index1]);
                }
            }
            return not.ToArray();
        }
        public static string[] Get_part_of_list1_but_not_of_list2_while_keeping_the_order(string[] list1, params string[] list2)
        {
            Dictionary<string, bool> list2_dict = new Dictionary<string, bool>();
            foreach (string list2_string in list2)
            {
                if (!list2_dict.ContainsKey(list2_string))
                {
                    list2_dict.Add(list2_string, true);
                }
            }
            List<string> keep_list1 = new List<string>();
            foreach (string list1_string in list1)
            {
                if (!list2_dict.ContainsKey(list1_string))
                {
                    keep_list1.Add(list1_string);
                }
            }
            return keep_list1.ToArray();
        }
        public static T[] Get_part_of_list1_but_not_of_list2<T>(T[] list1, params T[] list2) where T : IComparable
        {
            Dictionary<T, bool> list2_dict = new Dictionary<T, bool>();
            foreach (T list2_string in list2)
            {
                if (!list2_dict.ContainsKey(list2_string))
                {
                    list2_dict.Add(list2_string, true);
                }
            }
            List<T> keep_list1 = new List<T>();
            foreach (T list1_string in list1)
            {
                if (!list2_dict.ContainsKey(list1_string))
                {
                    keep_list1.Add(list1_string);
                }
            }
            return keep_list1.ToArray();
        }
    }
}
