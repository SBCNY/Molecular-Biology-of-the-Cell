using Common_functions.Form_tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using Windows_forms_customized_tools;

namespace Windows_forms
{
    class ProgressReport_interface_class
    {
        private MyPanel_textBox Overall_panel_textBox { get; set; }
        private Form1_default_settings_class Form_default_settings { get; set; }
        private string Last_entry { get; set; }

        public ProgressReport_interface_class()
        { }
        public ProgressReport_interface_class(MyPanel_textBox overall_panel_textBox,
                                              Form1_default_settings_class form_default_settings)
        {
            Overall_panel_textBox = overall_panel_textBox;
            Form_default_settings = form_default_settings;
            Update_all_graphic_elements();
        }

        public void Update_progressReport_text_and_visualization(string new_text)
        {
            Overall_panel_textBox.Set_silent_text_adjustFontSize_and_refresh(new_text, Form_default_settings);
            Overall_panel_textBox.Refresh();
        }
        public string Get_deep_copy_of_progressReport_text()
        {
            return Overall_panel_textBox.Get_deep_copy_of_text();
        }
        public void Clear_progressReport_text_and_last_entry()
        {
            Overall_panel_textBox.Set_silent_text_adjustFontSize_and_refresh("", Form_default_settings);
            Overall_panel_textBox.Refresh();
            this.Last_entry = "";
        }
        public void Update_all_graphic_elements()
        {
            Form_default_settings.Get_myPanelTextBoxProgressReport_default_parameters(out int left_position, out int top_position, out int right_position, out int bottom_position, out Color backColor, out Color textColor);
            Overall_panel_textBox.Set_left_top_right_bottom_position(left_position, top_position, right_position, bottom_position, Form_default_settings);
            Overall_panel_textBox.Back_color = backColor;
            Overall_panel_textBox.TextColor = textColor;
            Overall_panel_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            Overall_panel_textBox.Set_silent_text_adjustFontSize_and_refresh("", Form_default_settings);
        }
        public void Write_temporary_warning_and_restore_progressReport(string warning_text, int wait_time_in_seconds)
        {
            Write_warning_and_save_current_entry(warning_text);
            if (!Form_default_settings.Is_mono)
            {
                //System.Threading.Thread.Sleep(wait_time_in_seconds * 1000);
                //Restore_and_delete_lastEntry_if_not_emtpy();
            }
        }
        public void Write_temporary_announcement_and_restore_progressReport(string warning_text, int wait_time_in_seconds)
        {
            Write_warning_and_save_current_entry(warning_text);
            if (!Form_default_settings.Is_mono)
            {
               //System.Threading.Thread.Sleep(wait_time_in_seconds * 1000);
               //Restore_and_delete_lastEntry_if_not_emtpy();
            }
        }

        public void Restore_and_delete_lastEntry_if_not_emtpy()
        {
            if (!String.IsNullOrEmpty(Last_entry))
            {
                Update_progressReport_text_and_visualization(Last_entry);
                Last_entry = "";
            }
        }
        public void Write_warning_and_save_current_entry(string warning_text)
        {
            Last_entry = Overall_panel_textBox.Get_deep_copy_of_text(); 
            Update_progressReport_text_and_visualization(warning_text);
        }
        public void Write_announcement_and_save_current_entry(string announcement_text)
        {
            Last_entry = Overall_panel_textBox.Get_deep_copy_of_text();
            Update_progressReport_text_and_visualization(announcement_text);
        }

    }
}
