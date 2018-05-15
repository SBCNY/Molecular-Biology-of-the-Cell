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
using Common_functions.Text;

namespace Common_functions.Colors
{

    enum Color_enum { White, Dark_blue, Light_blue, Dark_red, Light_red, Dark_green, Light_green, Bright_green, Black }

    class Hexadecimal_color_class
    {
        private static string Get_hexadecimal_sign(int number)
        {
            string sign = "no value";
            switch (number)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    sign = number.ToString();
                    break;
                case 10:
                    sign = "A";
                    break;
                case 11:
                    sign = "B";
                    break;
                case 12:
                    sign = "C";
                    break;
                case 13:
                    sign = "D";
                    break;
                case 14:
                    sign = "E";
                    break;
                case 15:
                    sign = "F";
                    break;
                default:
                    throw new Exception("not considered");
            }
            return sign;
        }

        private static string Convert_into_two_digit_hexadecimal(int number)
        {
            if ((number > 255) || (number < 0))
            {
                throw new Exception("number is not between0 and 255");
            }
            else
            {
                int multiples_of_16 = (int)Math.Floor((double)number / (double)16);
                int modulus = number % 16;
                return Get_hexadecimal_sign(multiples_of_16) + Get_hexadecimal_sign(modulus);
            }
        }

        public static string Get_hexadecimal_code(int red, int green, int blue)
        {
            return "#" + Convert_into_two_digit_hexadecimal(red) + Convert_into_two_digit_hexadecimal(green) + Convert_into_two_digit_hexadecimal(blue);
        }

        public static string Get_hexadecimial_code_for_color(Color_enum color)
        {
            string color_code;
            switch (color)
            {
                case Color_enum.White:
                    color_code = Get_hexadecimal_code(255, 255, 255);
                    break;
                case Color_enum.Light_blue:
                    color_code = Get_hexadecimal_code(23, 141, 214);
                    break;
                case Color_enum.Dark_blue:
                    color_code = Get_hexadecimal_code(10, 10, 100);
                    break;
                case Color_enum.Light_red:
                    color_code = Get_hexadecimal_code(230, 0, 0);
                    break;
                case Color_enum.Dark_red:
                    color_code = Get_hexadecimal_code(100, 0, 0);
                    break;
                case Color_enum.Light_green:
                    color_code = Get_hexadecimal_code(134, 196, 64);
                    break;
                case Color_enum.Bright_green:
                    color_code = Get_hexadecimal_code(0, 255, 0);
                    break;
                case Color_enum.Dark_green:
                    color_code = Get_hexadecimal_code(0, 100, 0);
                    break;
                case Color_enum.Black:
                    color_code = Get_hexadecimal_code(0, 0, 0);
                    break;
                default:
                    throw new Exception("Color is not considered");
            }
            return color_code;
        }
    }

    //////////////////////////////////////////////////////////

    public class ColorMap_class
    {
        float Min_value { get; set; }
        float Max_value { get; set; }
        float Step_size { get; set; }
        float Mean_value { get; set; }

        public ColorMap_class(float max_value, float min_value)
        {
            this.Max_value = max_value;
            this.Min_value = min_value;
            Step_size = (Math.Abs(Max_value) + Math.Abs(Min_value)) / 3;
            Mean_value = (Max_value - Min_value) / 2;
        }

        private float Interpolate(float val, float y0, float x0, float y1, float x1 ) 
        {
            return (val-x0)*(y1-y0)/(x1-x0) + y0;
        }

        private float Base(float val ) 
        {
            if (val <= -0.75)      { return 0F; }
            else if (val <= -0.25) { return Interpolate(val, 0.0F, -0.75F, 1.0F, -0.25F); }
            else if (val <= 0.25)  { return 1.0F; }
            else if (val <= 0.75)  { return Interpolate(val, 1.0F, 0.25F, 0.0F, 0.75F); }
            else return 0.0F;
        }

        private float Get_red(float gray ) 
        {
            return Base( gray - Step_size );
        }
        
        private float Get_green(float gray ) 
        {
            return Base( gray );
        }

        private float Get_blue(float gray ) 
        {
            return Base( gray + Step_size );
        }

        public string Get_hexadecimal_color_based_on_jet_palette(float value_to_be_colored)
        {
            float relative_value_to_be_colored = -1;
            if (value_to_be_colored<Min_value)
            {
                relative_value_to_be_colored = Min_value;
            }
            else if (value_to_be_colored > Max_value)
            {
                relative_value_to_be_colored = Max_value;
            }
            else
            {
                relative_value_to_be_colored = ((value_to_be_colored - Min_value) / (Max_value - Min_value)) *2 - 1;
            }
            float red_fraction = Get_red(relative_value_to_be_colored);
            float green_fraction = Get_green(relative_value_to_be_colored);
            float blue_fraction = Get_blue(relative_value_to_be_colored);
            int red = (int)Math.Round(255 * red_fraction);
            int green = (int)Math.Round(255 * green_fraction);
            int blue = (int)Math.Round(255 * blue_fraction);
            return Hexadecimal_color_class.Get_hexadecimal_code(red, green, blue);
        }
    }

}
