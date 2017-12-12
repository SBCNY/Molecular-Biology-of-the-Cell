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
using Common_functions.Report;

namespace Common_functions.Statistic
{
    class Fisher_exact_test_class
    {
        private double[] log_factorials;
        private int max_size;
        public bool Report;

        //         a     b
        //         c     d

        public Fisher_exact_test_class(int input_max_size, bool report)
        {
            max_size = input_max_size;
            Report = report;
            if (Report)
            {
                Report_class.WriteLine("{0}: Initialize array of factorials with max_size = {1}", typeof(Fisher_exact_test_class).Name, max_size);
            }
            log_factorials = new double[max_size + 1];
            log_factorials[0] = 0;
            for (int i = 1; i < max_size + 1; i++)
            {
                log_factorials[i] = log_factorials[i - 1] + Math.Log(i);
            }
        }

        protected bool Check_if_n_not_larger_than_max_size(int a, int b, int c, int d)
        {
            bool smaller = true;
            int n = a + b + c + d;
            if (n > max_size + 1)
            {
                Report_class.Write_error_line("{0}: n ({1}) is larger than max_size ({2}): initialize new fisher exact test instance", typeof(Fisher_exact_test_class).Name, n, max_size);
                smaller = false;
                throw new Exception("see above");
            }
            return smaller;
        }

        protected double Get_specific_p_value(int a, int b, int c, int d)
        {
            int n = a + b + c + d;
            double log_p = log_factorials[a + b] + log_factorials[c + d] + log_factorials[a + c] + log_factorials[b + d] - log_factorials[n] - log_factorials[a] - log_factorials[b] - log_factorials[c] - log_factorials[d];
            return Math.Exp(log_p);
        }

        protected double Get_specific_minuslog10_pvalue(int a, int b, int c, int d)
        {
            int n = a + b + c + d;
            double log_p = log_factorials[a + b] + log_factorials[c + d] + log_factorials[a + c] + log_factorials[b + d] - log_factorials[n] - log_factorials[a] - log_factorials[b] - log_factorials[c] - log_factorials[d];
            return log_p;
        }

        public double Get_rightTailed_p_value(int a, int b, int c, int d)
        {
            if (Report) { Report_class.WriteLine("{0}: Get right tailed p-value", typeof(Fisher_exact_test_class).Name); }
            double p;
            if (Check_if_n_not_larger_than_max_size(a, b, c, d))
            {
                p = Get_specific_p_value(a, b, c, d);
                int min = (c < b) ? c : b;
                for (int i = 0; i < min; i++)
                {
                    p += Get_specific_p_value(++a, --b, --c, ++d);
                }
            }
            else { p = -1; };
            if (Report)
            {
                for (int i = 0; i < typeof(Fisher_exact_test_class).Name.Length + 2; i++) { Report_class.Write(" "); }
                Report_class.WriteLine("p_value: {0}", p);
            }
            return p;
        }

        private double Get_specific_p_value_times_10_to_the_power_of_x(int a, int b, int c, int d, int exponent_x)
        {
            int n = a + b + c + d;
            double log_p = log_factorials[a + b] + log_factorials[c + d] + log_factorials[a + c] + log_factorials[b + d] - log_factorials[n] - log_factorials[a] - log_factorials[b] - log_factorials[c] - log_factorials[d];
            return Math.Exp(log_p + exponent_x);
        }

        public double Get_righTailed_p_value_times_10_to_the_power_of_x(int a, int b, int c, int d, int exponent_x)
        {
            if (Report) { Report_class.WriteLine("{0}: Get right tailed p-value", typeof(Fisher_exact_test_class).Name); }
            double p;
            if (Check_if_n_not_larger_than_max_size(a, b, c, d))
            {
                p = Get_specific_p_value_times_10_to_the_power_of_x(a, b, c, d, exponent_x);
                int min = (c < b) ? c : b;
                for (int i = 0; i < min; i++)
                {
                    p += Get_specific_p_value_times_10_to_the_power_of_x(++a, --b, --c, ++d, exponent_x);
                }
            }
            else { p = -1; };
            if (Report)
            {
                for (int i = 0; i < typeof(Fisher_exact_test_class).Name.Length + 2; i++) { Report_class.Write(" "); }
                Report_class.WriteLine("p_value: {0}", p);
            }
            return p;
        }

        public double Get_leftTailed_p_value(int a, int b, int c, int d)
        {
            if (Report) { Report_class.WriteLine("{0}: Get left tailed p-value", typeof(Fisher_exact_test_class).Name); }
            double p;
            if (Check_if_n_not_larger_than_max_size(a, b, c, d))
            {
                p = Get_specific_p_value(a, b, c, d);
                int min = (a < d) ? a : d;
                for (int i = 0; i < min; i++)
                {
                    p += Get_specific_p_value(--a, ++b, ++c, --d);
                }
            }
            else { p = -1; };
            if (Report)
            {
                for (int i = 0; i < typeof(Fisher_exact_test_class).Name.Length + 2; i++) { Report_class.Write(" "); }
                Report_class.WriteLine("p_value: {0}", p);
            }
            return p;
        }

        //private float Get_specific_minus_log10_pvalue(int a, int b, int c, int d)
        //{
        //    int n = a + b + c + d;
        //    double log_p = log_factorials[a + b] + log_factorials[c + d] + log_factorials[a + c] + log_factorials[b + d] - log_factorials[n] - log_factorials[a] - log_factorials[b] - log_factorials[c] - log_factorials[d];
        //    return (float)log_p;
        //}

        //public float Get_rightTailed_minus_log10_p_value(int a, int b, int c, int d)
        //{
        //    if (Report) { Report_class.WriteLine("{0}: Get right tailed p-value", typeof(Fisher_exact_test_class).Name); }
        //    double minus_log10;
        //    if (Check_if_n_not_larger_than_max_size(a, b, c, d))
        //    {
        //        minus_log10 = Get_specific_minus_log10_pvalue(a, b, c, d);
        //        int min = (c < b) ? c : b;
        //        for (int i = 0; i < min; i++)
        //        {
        //            minus_log10 *= Get_specific_minus_log10_pvalue(++a, --b, --c, ++d);
        //        }
        //    }
        //    else { minus_log10 = -1; };
        //    if (Report)
        //    {
        //        for (int i = 0; i < typeof(Fisher_exact_test_class).Name.Length + 2; i++) { Report_class.Write(" "); }
        //        Report_class.WriteLine("minus log 10 p_value: {0}", minus_log10);
        //    }
        //    return (float)p;
        //}

     }
}
