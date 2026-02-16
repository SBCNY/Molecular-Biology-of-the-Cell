using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Windows_forms;
using Windows_forms_customized_tools;

namespace ClassLibrary1.Windows_forms
{
    /// <summary>
    /// ///////////////////////// is inactive
    /// </summary>
    class ExplanationErrorReport_userInterface
    {
        private OwnTextBox Explanation_error_reports_ownTextBox { get; set; }

        private ProgressReport_interface_class ProgressReport { get; set; }

        public ExplanationErrorReport_userInterface(OwnTextBox explanation_error_reports_ownTextBox,
                                                    ProgressReport_interface_class progress_report)
        {
            this.Explanation_error_reports_ownTextBox = explanation_error_reports_ownTextBox;
            this.ProgressReport = progress_report;
        }
        public void Update_text_and_set_to_visible(string text)
        {
            Explanation_error_reports_ownTextBox.SilentText = (string)text.Clone();
            Explanation_error_reports_ownTextBox.Visible = true;
            ProgressReport.Update_progressReport_text_and_visualization("After clicking on the text box, the mouse wheel, arrow keys and page up/down keys can be used for scrolling.");
        }
        public void Set_to_invisible()
        {
            Explanation_error_reports_ownTextBox.Visible = true;
            ProgressReport.Update_progressReport_text_and_visualization("");
        }

    }
}
