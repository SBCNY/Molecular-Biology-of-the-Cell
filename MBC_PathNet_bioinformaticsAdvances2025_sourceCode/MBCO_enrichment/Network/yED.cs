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
using Network;
//using System.Windows.Forms.DataVisualization.Charting;
using ZedGraph;
using System.Drawing;
using Common_functions.Global_definitions;
using Common_functions.ReadWrite;
using System.Drawing.Imaging;
using PdfSharp.Pdf;
using Windows_forms;
using System.Web;
using System.CodeDom;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms.VisualStyles;
using System.Globalization;
using ClassLibrary1.Properties;
using System.Resources;
using static System.Net.Mime.MediaTypeNames;
using System.Data.SqlTypes;
using System.Windows.Forms;
using PdfSharp.Drawing;
using System.Windows.Documents;

namespace yed_network
{
    enum Shape_enum { E_mp_t_y, Ellipse, Rectangle, Diamond, Octagon, Triangle_pointing_up, Triangle_pointing_down, Star_five_pointed }
    enum EdgeArrow_type_enum { E_m_p_t_y, Arrow, Thick_dotted_line, Dotted_line, Dashed_line, Solid_line }
    enum Yed_network_node_size_determinant_enum { E_m_p_t_y, No_of_sets, Uniform, No_of_different_colors, Minus_log10_pvalue }
    enum Node_size_scaling_across_plots_enum {  E_m_p_t_y, Unique, Consitent }
    enum Graph_editor_enum {  E_m_p_t_y, yED, Cytoscape }
    enum Visu_node_field_enum { E_m_p_t_y, Resource_id, Resource_size, Height, Width, Fill_color, Shape_type, Border_color, Border_width, Name, Shared_name, Box_label, Label, Suid, Label_y_offset, FontStyle, FontSize };

    //////////////////////////////////////////////////////////

    class yed_node_color_line_class
    {
        public string NodeName { get; set; }
        public Color_specification_line_class[] Color_specifications { get; set; }


        public static yed_node_color_line_class[] Order_by_nodeName(yed_node_color_line_class[] node_color_lines)
        {
            //node_colors = node_colors.OrderBy(l => l.NodeName).ToArray();
            Dictionary<string, List<yed_node_color_line_class>> nodeName_dict = new Dictionary<string, List<yed_node_color_line_class>>();
            int node_lines_length = node_color_lines.Length;
            yed_node_color_line_class node_color_line;
            for (int indexNL = 0; indexNL < node_lines_length; indexNL++)
            {
                node_color_line = node_color_lines[indexNL];
                if (!nodeName_dict.ContainsKey(node_color_line.NodeName))
                {
                    nodeName_dict.Add(node_color_line.NodeName, new List<yed_node_color_line_class>());
                }
                nodeName_dict[node_color_line.NodeName].Add(node_color_line);
            }
            List<yed_node_color_line_class> ordered_lines = new List<yed_node_color_line_class>();
            node_color_lines = null;
            string[] nodeNames;
            string nodeName;
            int nodeNames_length;
            nodeNames = nodeName_dict.Keys.ToArray();
            nodeNames = nodeNames.OrderBy(l => l).ToArray();
            nodeNames_length = nodeNames.Length;
            for (int indexNN = 0; indexNN < nodeNames_length; indexNN++)
            {
                nodeName = nodeNames[indexNN];
                ordered_lines.AddRange(nodeName_dict[nodeName]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != node_lines_length) { throw new Exception(); }
                yed_node_color_line_class previous_line;
                yed_node_color_line_class this_line;
                for (int indexO = 1; indexO < ordered_lines.Count; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.NodeName.CompareTo(previous_line.NodeName) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();


        }

        public yed_node_color_line_class Deep_copy()
        {
            yed_node_color_line_class copy = (yed_node_color_line_class)this.MemberwiseClone();
            int color_speci_length = Color_specifications.Length;
            copy.Color_specifications = new Color_specification_line_class[color_speci_length];
            for (int indexC=0; indexC<color_speci_length; indexC++)
            {
                copy.Color_specifications[indexC] = this.Color_specifications[indexC].Deep_copy();
            }
            return copy;
        }
    }

    //////////////////////////////////////////////////////////

    class Visualization_nw_node_characterisation_line_class
    {
        public string NodeName { get; set; }
        public int Level { get; set; }

        public Visualization_nw_node_characterisation_line_class Deep_copy()
        {
            Visualization_nw_node_characterisation_line_class copy = (Visualization_nw_node_characterisation_line_class)this.MemberwiseClone();
            copy.NodeName = (string)this.NodeName.Clone();
            return copy;
        }
    }

    class Visualization_nw_edge_characterisation_line_class
    {
        public string Target { get; set; }
        public float Edge_width { get; set; }
        public string Edge_label { get; set; }
        public EdgeArrow_type_enum EdgeArrow_type { get; set; }

        public Visualization_nw_edge_characterisation_line_class()
        {
            Target = "";
            Edge_label = "";
        }

        public Visualization_nw_edge_characterisation_line_class Deep_copy()
        {
            Visualization_nw_edge_characterisation_line_class copy = (Visualization_nw_edge_characterisation_line_class)this.MemberwiseClone();
            copy.Target = (string)this.Target.Clone();
            copy.Edge_label = (string)this.Edge_label.Clone();
            return copy;
        }
    }

    //////////////////////////////////////////////////////////

    class Visualization_of_nw_node_line
    {
        #region Fields
        public string Id { get; set; }
        public string Label { get; set; }
        public float Geometry_height { get; set; }
        public float Geometry_width { get; set; }
        public Color_specification_line_class[] Fill_colorSpecifications { get; set; }
        public int FontSize { get; set; }
        public string FontStyle { get; set; }
        public string LabelHasLineColor { get; set; }
        public string TextColor { get; set; }
        public string Model_name { get; set; }
        public string Model_position { get; set; }
        public string Shape_type { get; set; }
        public string Transparent { get; set; }
        public string Border_color { get; set; }
        public bool Border_has_color { get; set; }
        public float Border_width_cytoscape { get; set; }
        public float Border_width_yED { get; set; }
        public string Label_alignment { get; set; }
        public string Box_label { get; set; }
        public string Resource_id { get; set; }
        #endregion

        #region Order
        public static Visualization_of_nw_node_line[] Order_by_id(Visualization_of_nw_node_line[] node_lines)
        {
            ////nodes = nodes.OrderBy(l => l.Id).ToArray();
            Dictionary<string, List<Visualization_of_nw_node_line>> id_dict = new Dictionary<string, List<Visualization_of_nw_node_line>>();
            int node_lines_length = node_lines.Length;
            Visualization_of_nw_node_line node_line;
            for (int indexNL=0; indexNL < node_lines_length; indexNL++)
            {
                node_line = node_lines[indexNL];
                if (!id_dict.ContainsKey(node_line.Id))
                {
                    id_dict.Add(node_line.Id, new List<Visualization_of_nw_node_line>());
                }
                id_dict[node_line.Id].Add(node_line);
            }
            List<Visualization_of_nw_node_line> ordered_lines = new List<Visualization_of_nw_node_line>();
            node_lines = null;
            string[] ids;
            string id;
            int ids_length;
            ids = id_dict.Keys.ToArray();
            ids = ids.OrderBy(l => l).ToArray();
            ids_length = ids.Length;
            for (int indexId=0; indexId < ids_length;indexId++)
            {
                id = ids[indexId];
                ordered_lines.AddRange(id_dict[id]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count!=node_lines_length) { throw new Exception(); }
                Visualization_of_nw_node_line previous_line;
                Visualization_of_nw_node_line this_line;
                for (int indexO=1; indexO<ordered_lines.Count;indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.Id.CompareTo(previous_line.Id)<0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }

        public static Visualization_of_nw_node_line[] Order_by_fillColorSpecificationLength(Visualization_of_nw_node_line[] node_lines)
        {
            //nodes = nodes.OrderBy(l => l.Fill_colorSpecifications.Length).ToArray();
            Dictionary<int, List<Visualization_of_nw_node_line>> fillColorSpecificationLength_dict = new Dictionary<int, List<Visualization_of_nw_node_line>>();
            int node_lines_length = node_lines.Length;
            Visualization_of_nw_node_line node_line;
            int fillColorSpecification_length;
            for (int indexNL = 0; indexNL < node_lines_length; indexNL++)
            {
                node_line = node_lines[indexNL];
                fillColorSpecification_length = node_line.Fill_colorSpecifications.Length;
                if (!fillColorSpecificationLength_dict.ContainsKey(fillColorSpecification_length))
                {
                    fillColorSpecificationLength_dict.Add(fillColorSpecification_length, new List<Visualization_of_nw_node_line>());
                }
                fillColorSpecificationLength_dict[fillColorSpecification_length].Add(node_line);
            }
            List<Visualization_of_nw_node_line> ordered_lines = new List<Visualization_of_nw_node_line>();
            node_lines = null;
            int[] fillColorSpecificationLengthes;
            int fillColorSpecificaitonLength;
            int fillColorSpecificationLengthes_length;
            fillColorSpecificationLengthes = fillColorSpecificationLength_dict.Keys.ToArray();
            fillColorSpecificationLengthes = fillColorSpecificationLengthes.OrderBy(l => l).ToArray();
            fillColorSpecificationLengthes_length = fillColorSpecificationLengthes.Length;
            for (int indexFCSL = 0; indexFCSL < fillColorSpecificationLengthes_length; indexFCSL++)
            {
                fillColorSpecificaitonLength = fillColorSpecificationLengthes[indexFCSL];
                ordered_lines.AddRange(fillColorSpecificationLength_dict[fillColorSpecificaitonLength]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != node_lines_length) { throw new Exception(); }
                Visualization_of_nw_node_line previous_line;
                Visualization_of_nw_node_line this_line;
                for (int indexO = 1; indexO < ordered_lines.Count; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.Fill_colorSpecifications.Length.CompareTo(previous_line.Fill_colorSpecifications.Length) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();

        }

        public static Visualization_of_nw_node_line[] Order_by_boxLabel_id(Visualization_of_nw_node_line[] node_lines)
        {
            //nodes = nodes.OrderBy(l => l.Box_label).ThenBy(l => l.Id).ToArray();
            Dictionary<string, Dictionary<string, List<Visualization_of_nw_node_line>>> boxLabel_id_dict = new Dictionary<string, Dictionary<string, List<Visualization_of_nw_node_line>>>();
            Dictionary<string, List<Visualization_of_nw_node_line>> id_dict = new Dictionary<string, List<Visualization_of_nw_node_line>>();
            int node_lines_length = node_lines.Length;
            Visualization_of_nw_node_line node_line;
            for (int indexNL = 0; indexNL < node_lines_length; indexNL++)
            {
                node_line = node_lines[indexNL];
                if (!boxLabel_id_dict.ContainsKey(node_line.Box_label))
                {
                    boxLabel_id_dict.Add(node_line.Box_label, new Dictionary<string, List<Visualization_of_nw_node_line>>());
                }
                if (!boxLabel_id_dict[node_line.Box_label].ContainsKey(node_line.Box_label))
                {
                    boxLabel_id_dict[node_line.Box_label].Add(node_line.Id, new List<Visualization_of_nw_node_line>());
                }
                boxLabel_id_dict[node_line.Box_label][node_line.Id].Add(node_line);
            }
            List<Visualization_of_nw_node_line> ordered_lines = new List<Visualization_of_nw_node_line>();
            node_lines = null;
            string[] boxLabels;
            string boxLabel;
            int boxLabels_length;
            string[] ids;
            string id;
            int ids_length;
            boxLabels = boxLabel_id_dict.Keys.ToArray();
            boxLabels = boxLabels.OrderBy(l => l).ToArray();
            boxLabels_length = boxLabels.Length;
            for (int indexBL=0; indexBL<boxLabels_length;indexBL++)
            {
                boxLabel = boxLabels[indexBL];
                id_dict = boxLabel_id_dict[boxLabel];
                ids = id_dict.Keys.ToArray();
                ids = ids.OrderBy(l => l).ToArray();
                ids_length = ids.Length;
                for (int indexId = 0; indexId < ids_length; indexId++)
                {
                    id = ids[indexId];
                    ordered_lines.AddRange(id_dict[id]);
                }
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != node_lines_length) { throw new Exception(); }
                Visualization_of_nw_node_line previous_line;
                Visualization_of_nw_node_line this_line;
                for (int indexO = 1; indexO < ordered_lines.Count; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.Box_label.CompareTo(previous_line.Box_label) < 0) { throw new Exception(); }
                    if (  (this_line.Box_label.Equals(previous_line.Box_label))
                        &&(this_line.Id.CompareTo(previous_line.Id)<0)) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();

        }
        #endregion



        public Visualization_of_nw_node_line()
        {
            Geometry_height = 30;
            Geometry_width = 30;
            FontSize = 35;
            FontStyle = "arial";
            Model_name = "sides";
            Model_position = "s";
            Transparent = "false";
            LabelHasLineColor = "false";
            TextColor = "#000000";
            Border_color = "#000000";
            Border_width_cytoscape = 5F;//
            Border_width_yED = 0.5F;
            Label_alignment = "side";
            Box_label = "";
            Shape_type = "rectangle";
            Resource_id = "";
            Border_has_color = true;
            Fill_colorSpecifications = new Color_specification_line_class[0];
        }

        public bool SameFillColors_in_same_order(Visualization_of_nw_node_line other)
        {
            int this_fill_colors_length = this.Fill_colorSpecifications.Length;
            int other_fill_colors_length = other.Fill_colorSpecifications.Length;
            bool sameFillColors = true;
            if (this_fill_colors_length != other_fill_colors_length)
            {
                sameFillColors = false;
            }
            else
            {
                for (int indexC = 0; indexC < this_fill_colors_length; indexC++)
                {
                    if ((!this.Fill_colorSpecifications[indexC].Fill_color.Equals(other.Fill_colorSpecifications[indexC].Fill_color)))
                    {
                        sameFillColors = false;
                        break;
                    }
                    if ((!this.Fill_colorSpecifications[indexC].Size.Equals(other.Fill_colorSpecifications[indexC].Size)))
                    {
                        sameFillColors = false;
                        break;
                    }
                    if ((!this.Fill_colorSpecifications[indexC].Dataset_order_no.Equals(other.Fill_colorSpecifications[indexC].Dataset_order_no)))
                    {
                        sameFillColors = false;
                        break;
                    }
                }
            }
            return sameFillColors;
        }

        public Visualization_of_nw_node_line Deep_copy()
        {
            Visualization_of_nw_node_line copy = (Visualization_of_nw_node_line)this.MemberwiseClone();
            copy.Id = (string)this.Id.Clone();
            copy.Label = (string)this.Label.Clone();
            int color_specifications_length = this.Fill_colorSpecifications.Length;
            copy.Fill_colorSpecifications = new Color_specification_line_class[color_specifications_length];
            for (int indexCS=0; indexCS<color_specifications_length; indexCS++)
            {
                copy.Fill_colorSpecifications[indexCS] = this.Fill_colorSpecifications[indexCS].Deep_copy();
            }
            copy.FontStyle = (string)this.FontStyle.Clone();
            copy.LabelHasLineColor = (string)this.LabelHasLineColor.Clone();
            copy.TextColor = (string)this.TextColor.Clone();
            copy.Model_name = (string)this.Model_name.Clone();
            copy.Shape_type = (string)this.Shape_type.Clone();
            copy.Transparent = (string)this.Transparent.Clone();
            copy.Border_color = (string)this.Border_color.Clone();
            copy.Label_alignment = (string)this.Label_alignment.Clone();
            return copy;
        }
    }

    class Visualization_of_nw_edge_line
    {
        #region Fields
        public string Target_id { get; set; }
        public string Edge_id { get; set; }
        public string Reaction_summary_index { get; set; }
        public string Source_id { get; set; }
        public string Arrow_source_end_yEd
        {
            get
            {
                switch (EdgeArrow_type)
                {
                    case EdgeArrow_type_enum.Arrow:
                    case EdgeArrow_type_enum.Dotted_line:
                    case EdgeArrow_type_enum.Solid_line:
                    case EdgeArrow_type_enum.Thick_dotted_line:
                    case EdgeArrow_type_enum.Dashed_line:
                        return "none";
                    default:
                        throw new Exception();
                }

            }
        }
        public string Arrow_source_end_cytoscape
        {
            get
            {
                switch (EdgeArrow_type)
                {
                    case EdgeArrow_type_enum.Arrow:
                    case EdgeArrow_type_enum.Solid_line:
                    case EdgeArrow_type_enum.Dotted_line:
                    case EdgeArrow_type_enum.Thick_dotted_line:
                    case EdgeArrow_type_enum.Dashed_line:
                        return "NONE";
                    default:
                        throw new Exception();
                }

            }
        }
        public string Arrow_target_end_yEd
        {
            get
            {
                switch (EdgeArrow_type)
                {
                    case EdgeArrow_type_enum.Arrow:
                        return "standard";
                    case EdgeArrow_type_enum.Dotted_line:
                    case EdgeArrow_type_enum.Thick_dotted_line:
                    case EdgeArrow_type_enum.Dashed_line:
                    case EdgeArrow_type_enum.Solid_line:
                        return "none";
                    default:
                        throw new Exception();
                }
            }
        }
        public string Arrow_target_end_cytoscape
        {
            get
            {
                switch (EdgeArrow_type)
                {
                    case EdgeArrow_type_enum.Arrow:
                        return "ARROW";
                    case EdgeArrow_type_enum.Dotted_line:
                    case EdgeArrow_type_enum.Thick_dotted_line:
                    case EdgeArrow_type_enum.Dashed_line:
                    case EdgeArrow_type_enum.Solid_line:
                        return "NONE";
                    default:
                        throw new Exception();
                }
            }
        }
        public string Arrow_color { get; set; }
        public string Arrow_type_yED
        {
            get
            {
                switch (EdgeArrow_type)
                {
                    case EdgeArrow_type_enum.Arrow:
                    case EdgeArrow_type_enum.Solid_line:
                        return "line";
                    case EdgeArrow_type_enum.Dotted_line:
                    case EdgeArrow_type_enum.Thick_dotted_line:
                        return "dotted";
                    case EdgeArrow_type_enum.Dashed_line:
                        return "dashed";
                    default:
                        throw new Exception();
                }
            }
        }
        public string Arrow_type_cytoscape
        {
            get
            {
                switch (EdgeArrow_type)
                {
                    case EdgeArrow_type_enum.Arrow:
                    case EdgeArrow_type_enum.Solid_line:
                        return "SOLID";
                    case EdgeArrow_type_enum.Dotted_line:
                    case EdgeArrow_type_enum.Thick_dotted_line:
                        return "DOTTED";
                    case EdgeArrow_type_enum.Dashed_line:
                        return "DASHED";
                    default:
                        throw new Exception();
                }
            }
        }

        public string Arrow_width
        { get; set; }
        public string Arrow_label { get; set; }
        public string Arrow_label_font_size { get; set; }
        public EdgeArrow_type_enum EdgeArrow_type { get; set; }
        #endregion

        #region Order
        public static Visualization_of_nw_edge_line[] Order_edgeId(Visualization_of_nw_edge_line[] edge_lines)
        {
            //edges = edges.OrderBy(l => l.Edge_id).ToArray();
            Dictionary<string, List<Visualization_of_nw_edge_line>> edgeId_dict = new Dictionary<string, List<Visualization_of_nw_edge_line>>();
            int edge_lines_length = edge_lines.Length;
            Visualization_of_nw_edge_line edge_line;
            for (int indexEdge = 0; indexEdge < edge_lines_length; indexEdge++)
            {
                edge_line = edge_lines[indexEdge];
                if (!edgeId_dict.ContainsKey(edge_line.Edge_id))
                {
                    edgeId_dict.Add(edge_line.Edge_id, new List<Visualization_of_nw_edge_line>());
                }
                edgeId_dict[edge_line.Edge_id].Add(edge_line);
            }
            List<Visualization_of_nw_edge_line> ordered_lines = new List<Visualization_of_nw_edge_line>();
            edge_lines = null;
            string[] edgeIds;
            string edgeId;
            int edgeIds_length;
            edgeIds = edgeId_dict.Keys.ToArray();
            edgeIds = edgeIds.OrderBy(l => l).ToArray();
            edgeIds_length = edgeIds.Length;
            for (int indexEdgeId = 0; indexEdgeId < edgeIds_length; indexEdgeId++)
            {
                edgeId = edgeIds[indexEdgeId];
                ordered_lines.AddRange(edgeId_dict[edgeId]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != edge_lines_length) { throw new Exception(); }
                Visualization_of_nw_edge_line previous_line;
                Visualization_of_nw_edge_line this_line;
                for (int indexO = 1; indexO < ordered_lines.Count; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.Edge_id.CompareTo(previous_line.Edge_id) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();
        }

        #endregion


        public Visualization_of_nw_edge_line()
        {
            Arrow_color = "#000000";
            Arrow_width = "4.0";
            Arrow_label = "";
            Arrow_label_font_size = "15";
        }

        public Visualization_of_nw_edge_line Deep_copy()
        {
            Visualization_of_nw_edge_line copy = (Visualization_of_nw_edge_line)this.MemberwiseClone();
            copy.Target_id = (string)this.Target_id.Clone();
            copy.Source_id = (string)this.Source_id.Clone();
            copy.Edge_id = (string)this.Edge_id.Clone();
            copy.Arrow_color = (string)this.Arrow_color.Clone();
            copy.Arrow_width = (string)this.Arrow_width.Clone();
            copy.Arrow_label = (string)this.Arrow_label.Clone();
            copy.Arrow_label_font_size = (string)this.Arrow_label_font_size.Clone();
            return copy;
        }
    }

    class Visualization_of_nw_resource_line_class
    {
        public string Resource_id { get; set; }
        public string Base64String { get; set; }

        public static Visualization_of_nw_resource_line_class[] Order_by_resourceId(Visualization_of_nw_resource_line_class[] resource_lines)
        {
            Dictionary<string, List<Visualization_of_nw_resource_line_class>> resourceId_dict = new Dictionary<string, List<Visualization_of_nw_resource_line_class>>();
            int resource_lines_length = resource_lines.Length;
            Visualization_of_nw_resource_line_class resource_line;
            for (int indexResource = 0; indexResource < resource_lines_length; indexResource++)
            {
                resource_line = resource_lines[indexResource];
                if (!resourceId_dict.ContainsKey(resource_line.Resource_id))
                {
                    resourceId_dict.Add(resource_line.Resource_id, new List<Visualization_of_nw_resource_line_class>());
                }
                resourceId_dict[resource_line.Resource_id].Add(resource_line);
            }
            List<Visualization_of_nw_resource_line_class> ordered_lines = new List<Visualization_of_nw_resource_line_class>();
            resource_lines = null;
            string[] resourceIds;
            string resourceId;
            int resourceIds_length;
            resourceIds = resourceId_dict.Keys.ToArray();
            resourceIds = resourceIds.OrderBy(l => l).ToArray();
            resourceIds_length = resourceIds.Length;
            for (int indexResourceId = 0; indexResourceId < resourceIds_length; indexResourceId++)
            {
                resourceId = resourceIds[indexResourceId];
                ordered_lines.AddRange(resourceId_dict[resourceId]);
            }
            if (Global_class.Check_ordering)
            {
                if (ordered_lines.Count != resource_lines_length) { throw new Exception(); }
                Visualization_of_nw_resource_line_class previous_line;
                Visualization_of_nw_resource_line_class this_line;
                for (int indexO = 1; indexO < ordered_lines.Count; indexO++)
                {
                    this_line = ordered_lines[indexO];
                    previous_line = ordered_lines[indexO - 1];
                    if (this_line.Resource_id.CompareTo(previous_line.Resource_id) < 0) { throw new Exception(); }
                }
            }
            return ordered_lines.ToArray();

        }


        public Visualization_of_nw_resource_line_class Deep_copy()
        {
            Visualization_of_nw_resource_line_class copy = (Visualization_of_nw_resource_line_class)this.MemberwiseClone();
            copy.Resource_id = (string)this.Resource_id.Clone();
            copy.Base64String = (string)this.Base64String.Clone();
            return copy;
        }
    }

    //////////////////////////////////////////////////////////

    class yED_options_class
    {
        public Color Edge_color { get; set; }
        public Color Node_fill_color { get; set; }
        public Shape_enum Node_shape { get; set; }
        public Shape_enum Legend_dataset_node_shape { get; set; }
        public Dictionary<string,bool> Legend_dataset_node_dict { get; set; }
        public bool Group_same_level_processes { get; set; }
        public int Max_signs_per_line_in_nodeName { get; set; }
        public int Max_node_diameter { get; set; }
        public int Cytoscape_label_width { get; set; }
        public int Max_number_of_lines_in_nodeName { get; set; }
        public int Min_node_diameter { get; set; }
        public int Min_label_size { get; set; }
        public int Max_label_size { get; set; }
        private float Circle_to_pie_chart_factor_yED { get; set; }
        private float Circle_to_pie_chart_factor_cytoscape { get; set; }
        public float Circle_to_pie_chart_factor
        { 
            get 
            {  
                switch (Graph_editor)
                {
                    case Graph_editor_enum.yED:
                        return Circle_to_pie_chart_factor_yED;
                    case Graph_editor_enum.Cytoscape:
                        return Circle_to_pie_chart_factor_cytoscape;
                    default:
                        throw new Exception();
                }
            }
        }
        public Yed_network_node_size_determinant_enum Node_size_determinant { get; set; }
        public Graph_editor_enum Graph_editor { get; set; }

        public yED_options_class()
        {
            Graph_editor = Graph_editor_enum.E_m_p_t_y;
            Circle_to_pie_chart_factor_yED = 1.315F;
            Circle_to_pie_chart_factor_cytoscape = 1.54F;
            Max_number_of_lines_in_nodeName = 4;
            Edge_color = Color.Black;
            Node_fill_color = Color.CornflowerBlue;
            Node_shape = Shape_enum.Ellipse;
            Legend_dataset_node_shape = Shape_enum.Rectangle;
            Group_same_level_processes = false;
            Max_signs_per_line_in_nodeName = 16;
            Cytoscape_label_width = 500;
            Min_label_size = 10;
            Max_label_size = 10;
            Max_node_diameter = 100;
            Min_node_diameter = 1;
            Node_size_determinant = Yed_network_node_size_determinant_enum.Uniform;
            Legend_dataset_node_dict = new Dictionary<string, bool>();
        }
    }

    class yED_key_id_class
    {
        #region Fields
        public string Graphml { get; private set; }
        public string Portgraphics { get; private set; }
        public string Portgeometry { get; private set; }
        public string Portuserdata { get; private set; }
        public string Node_url { get; private set; }
        public string Node_description { get; private set; }
        public string Node_graphics { get; private set; }
        public string Resource_graphml { get; set; }
        public string Edge_url { get; private set; }
        public string Edge_description { get; private set; }
        public string Edge_graphics { get; private set; }
        #endregion

        public yED_key_id_class()
        {
            Portgraphics = "d1";
            Portgeometry = "d2";
            Portuserdata = "d3";
            Node_url = "d4";
            Node_description = "d5";
            Node_graphics = "d6";
            Edge_url = "d7";
            Edge_description = "d8";
            Edge_graphics = "d9";
            Resource_graphml = "d10";
        }
    }

    class yED_class
    {
        #region Fields
        private StreamWriter Writer { get; set; }
        private int Shift_text_right { get; set; }
        public yED_options_class Options { get; set; }
        private yED_key_id_class Key { get; set; }
        #endregion

        public yED_class()
        {
            Shift_text_right = 0;
            Key = new yED_key_id_class();
            Options = new yED_options_class();
        }

        #region Write head and botton
        private void Write_file_head_in_graphml_for_yED(string graph_label)
        {
            string file_head =
                  "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>\n"
                + "<graphml xmlns=\"http://graphml.graphdrawing.org/xmlns\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:y=\"http://www.yworks.com/xml/graphml\" xmlns:yed=\"http://www.yworks.com/xml/yed/3\" xsi:schemaLocation=\"http://graphml.graphdrawing.org/xmlns http://www.yworks.com/xml/schema/graphml/1.1/ygraphml.xsd\">\n"
                + "  <key for=\"graphml\" id=\"" + Key.Graphml + "\" yfiles.type=\"resources\"/>\n"
                + "  <key attr.name=\"url\" attr.type=\"string\" for=\"node\" id=\"" + Key.Node_url + "\"/>\n"
                + "  <key attr.name=\"description\" attr.type=\"string\" for=\"node\" id=\"" + Key.Node_description + "\"/>\n"
                + "  <key for=\"node\" id=\"" + Key.Node_graphics + "\" yfiles.type=\"nodegraphics\"/>\n"
                + "  <key attr.name=\"url\" attr.type=\"string\" for=\"edge\" id=\"" + Key.Edge_url + "\"/>\n"
                + "  <key attr.name=\"description\" attr.type=\"string\" for=\"edge\" id=\"" + Key.Edge_description + "\"/>\n"
                + "  <key for=\"edge\" id=\"" + Key.Edge_graphics + "\" yfiles.type=\"edgegraphics\"/>\n"
                + "  <key for=\"graphml\" id=\"" + Key.Resource_graphml + "\" yfiles.type=\"resources\"/>\n"
                + "  <graph id=\"" + graph_label + "\" edgedefault=\"undirected\">";
            Writer.WriteLine(file_head);
            Shift_text_right += 4;
        }
        private void Write_file_head_in_xgmml_for_cytoscape(string graph_label)
        {
            string fileHead =
          "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\n"
        + "<graph label=\"" + graph_label + "\" xmlns=\"http://www.cs.rpi.edu/XGMML\""
        + " xmlns:dc=\"http://purl.org/dc/elements/1.1/\""
        + " xmlns:xlink=\"http://www.w3.org/1999/xlink\""
        + " xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\""
        + " xmlns:cy=\"http://www.cytoscape.org\"\n"
        + "   directed=\"1\">\n";

            //    string fileHead =
            //  "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\n"
            //+ "<graph label=\"Cytoscape Network\""
            //+ " directed=\"1\">\n";

            Writer.WriteLine(fileHead);
                Shift_text_right += 4;
        }
        private void Write_file_head(string graph_label)
        {
            switch (Options.Graph_editor)
            {
                case Graph_editor_enum.Cytoscape:
                    Write_file_head_in_xgmml_for_cytoscape(graph_label);
                    break;
                case Graph_editor_enum.yED:
                    Write_file_head_in_graphml_for_yED(graph_label);
                    break;
                default:
                    throw new Exception();
            }
        }

        private void Write_file_bottom_for_nodes_and_edges_in_graphml_for_yED()
        {
            Shift_text_right -= 2;
            string file_bottom =
                  "</graph>\n";
            Writer.Write(file_bottom);
        }
        private void Write_file_bottom_for_nodes_and_edges_if_necessary()
        {
            switch (Options.Graph_editor)
            {
                case Graph_editor_enum.Cytoscape:
                    break;
                case Graph_editor_enum.yED:
                    Write_file_bottom_for_nodes_and_edges_in_graphml_for_yED();
                    break;
                default:
                    throw new Exception();
            }
        }

        private void Write_final_file_bottom_in_graphml_for_yED()
        {
            string file_bottom =
                 "</graphml>\n";
            Writer.Write(file_bottom);
            Shift_text_right -= 4;
        }
        private void Write_final_file_bottom_in_xgmml_for_cytoscape()
        {
            string file_bottom =
                 "</graph>\n";
            Writer.Write(file_bottom);
            Shift_text_right -= 4;
        }
        private void Write_final_file_bottom()
        {
            switch (Options.Graph_editor)
            {
                case Graph_editor_enum.Cytoscape:
                    Write_final_file_bottom_in_xgmml_for_cytoscape();
                    break;
                case Graph_editor_enum.yED:
                    Write_final_file_bottom_in_graphml_for_yED();
                    break;
                default:
                    throw new Exception();
            }
        }
        #endregion

        #region Write yED Nodes
        private string Split_label_over_multiple_lines_if_too_many_signs(string node_label)
        {
            int length_of_current_line = node_label.Length;
            if (length_of_current_line > Options.Max_signs_per_line_in_nodeName)
            {
                int optimal_number_of_lines = (int)Math.Ceiling((double)length_of_current_line / (double)Options.Max_signs_per_line_in_nodeName);
                int number_of_lines = Math.Min(Options.Max_number_of_lines_in_nodeName, optimal_number_of_lines);
                int signs_per_line = (int)Math.Ceiling((double)length_of_current_line / (double)number_of_lines);
                char[] split_characters = new char[] { ' ', '-' };
                string[] words = node_label.Split(split_characters);
                string word;
                int words_length = words.Length;

                #region Identify split_characters_in_label
                char[] split_characters_in_label = new char[words_length - 1];
                int indexStartSearch = 0;
                int indexNextSpace;
                int indexNextHypen;
                for (int indexSplit = 0; indexSplit < words_length - 1; indexSplit++)
                {
                    indexNextSpace = node_label.IndexOf(' ', indexStartSearch);
                    indexNextHypen = node_label.IndexOf('-', indexStartSearch);
                    if ((indexNextHypen == -1)
                        || ((indexNextSpace != -1)
                           && (indexNextSpace < indexNextHypen)))
                    {
                        if (indexNextSpace == -1) { throw new Exception(); }
                        split_characters_in_label[indexSplit] = ' ';
                        indexStartSearch = indexNextSpace + 1;
                    }
                    else
                    {
                        if (indexNextHypen == -1) { throw new Exception(); }
                        split_characters_in_label[indexSplit] = '-';
                        indexStartSearch = indexNextHypen + 1;
                    }

                }
                #endregion

                #region Calculate word_signsCounts
                int[] word_signsCounts = new int[words_length];
                int word_signsCount;
                for (int indexNLW = 0; indexNLW < words_length; indexNLW++)
                {
                    word = words[indexNLW];
                    word_signsCounts[indexNLW] = word.Length;
                }
                #endregion

                #region Identify indexFirst_word_in_lines and calculate lineSigns_count by not exceeding lineSigns_count for each line except the last
                int[] indexFirst_word_in_lines = new int[number_of_lines];
                int[] lineSigns_count = new int[number_of_lines];
                int indexLine = 0;
                int potential_one_for_space;
                for (int indexNLW = 0; indexNLW < words_length; indexNLW++)
                {
                    word_signsCount = word_signsCounts[indexNLW];
                    potential_one_for_space = 0;
                    if (lineSigns_count[indexLine] > 0) { potential_one_for_space = 1; }

                    if ((lineSigns_count[indexLine] + word_signsCount + potential_one_for_space > signs_per_line)
                        && (indexLine < number_of_lines - 1))
                    {
                        indexLine++;
                        indexFirst_word_in_lines[indexLine] = indexNLW;
                    }
                    lineSigns_count[indexLine] += word_signsCount + potential_one_for_space;
                }
                #endregion

                if (lineSigns_count[number_of_lines - 1] > signs_per_line)
                {
                    int indexLineMaxSigns = number_of_lines - 1;
                    int movedWordSignsCount;
                    int[] new_lineSigns_count = new int[number_of_lines];
                    int[] new_indexFirst_word_in_lines = new int[number_of_lines];
                    bool try_again = true;
                    while (try_again)
                    {
                        try_again = false;
                        for (int indexStartLine = number_of_lines - 1; indexStartLine >= 0; indexStartLine--)
                        {
                            for (int indexNewLine = 0; indexNewLine < number_of_lines; indexNewLine++)
                            {
                                new_lineSigns_count[indexNewLine] = lineSigns_count[indexNewLine];
                                new_indexFirst_word_in_lines[indexNewLine] = indexFirst_word_in_lines[indexNewLine];
                            }
                            for (int indexThisLine = indexStartLine; indexThisLine > 0; indexThisLine--)
                            {
                                if (new_indexFirst_word_in_lines[indexThisLine] < words_length - 1)
                                {
                                    movedWordSignsCount = word_signsCounts[new_indexFirst_word_in_lines[indexThisLine]];
                                    new_lineSigns_count[indexThisLine] = new_lineSigns_count[indexThisLine] - movedWordSignsCount - 1;
                                    new_lineSigns_count[indexThisLine - 1] = new_lineSigns_count[indexThisLine - 1] + movedWordSignsCount + 1;
                                    new_indexFirst_word_in_lines[indexThisLine] = new_indexFirst_word_in_lines[indexThisLine] + 1;
                                    bool words_better_distributed = true;
                                    for (int indexInternal = indexStartLine - 1; indexInternal >= indexThisLine - 1; indexInternal--)
                                    {
                                        if (Math.Max(0, lineSigns_count[indexStartLine] - signs_per_line)
                                            < Math.Max(0, new_lineSigns_count[indexInternal] - signs_per_line))
                                        {
                                            words_better_distributed = false;
                                            break;
                                        }
                                    }
                                    if (words_better_distributed)
                                    {
                                        for (int indexNewLine = 0; indexNewLine < number_of_lines; indexNewLine++)
                                        {
                                            lineSigns_count[indexNewLine] = new_lineSigns_count[indexNewLine];
                                            indexFirst_word_in_lines[indexNewLine] = new_indexFirst_word_in_lines[indexNewLine];
                                        }
                                        try_again = true;
                                    }
                                }
                            }
                        }
                    }
                }
                StringBuilder sb_node_label = new StringBuilder();
                sb_node_label.Append(words[0]);
                int indexIndex = 1;
                int current_line_length = word_signsCounts[0];
                for (int indexWord = 1; indexWord < words_length; indexWord++)
                {
                    word = words[indexWord];
                    word_signsCount = word_signsCounts[indexWord];
                    if ((indexIndex < number_of_lines) && (indexWord == indexFirst_word_in_lines[indexIndex]))
                    {
                        if (split_characters_in_label[indexWord - 1].Equals('-'))
                        {
                            sb_node_label.AppendFormat("-\n{0}", word);
                        }
                        else
                        {
                            sb_node_label.AppendFormat("\n{0}", word);
                        }
                        current_line_length = word_signsCount;
                        indexIndex++;
                    }
                    else
                    {
                        if (current_line_length > 0)
                        {
                            sb_node_label.AppendFormat("{0}", split_characters_in_label[indexWord - 1]);
                        }
                        sb_node_label.AppendFormat("{0}", word);
                        current_line_length += word_signsCount;
                    }

                }
                node_label = sb_node_label.ToString();
            }
            return node_label;
        }
        private Dictionary<Visu_node_field_enum, string> Get_visuNode_attributeCytoscapeName_dict()
        {
            Dictionary<Visu_node_field_enum, string> attribute_attributeCytoscapeName_dict = new Dictionary<Visu_node_field_enum, string>();
            attribute_attributeCytoscapeName_dict.Add(Visu_node_field_enum.Box_label, "Group label");
            attribute_attributeCytoscapeName_dict.Add(Visu_node_field_enum.Suid, "SUID"); // do not change
            attribute_attributeCytoscapeName_dict.Add(Visu_node_field_enum.Resource_id, "Pie chart");
            attribute_attributeCytoscapeName_dict.Add(Visu_node_field_enum.Resource_size, "Pie chart size");
            attribute_attributeCytoscapeName_dict.Add(Visu_node_field_enum.Shared_name, "shared name");
            attribute_attributeCytoscapeName_dict.Add(Visu_node_field_enum.Name, "name");
            attribute_attributeCytoscapeName_dict.Add(Visu_node_field_enum.Height, "Node height");
            attribute_attributeCytoscapeName_dict.Add(Visu_node_field_enum.Label, "Label text");
            attribute_attributeCytoscapeName_dict.Add(Visu_node_field_enum.Width, "Node width");
            attribute_attributeCytoscapeName_dict.Add(Visu_node_field_enum.Fill_color, "Node fill color");
            attribute_attributeCytoscapeName_dict.Add(Visu_node_field_enum.Shape_type, "Node shape");
            attribute_attributeCytoscapeName_dict.Add(Visu_node_field_enum.Border_color, "Node border color");
            attribute_attributeCytoscapeName_dict.Add(Visu_node_field_enum.Border_width, "Node border width");
            attribute_attributeCytoscapeName_dict.Add(Visu_node_field_enum.Label_y_offset, "Label y offset");
            attribute_attributeCytoscapeName_dict.Add(Visu_node_field_enum.FontStyle, "Label font style");
            attribute_attributeCytoscapeName_dict.Add(Visu_node_field_enum.FontSize, "Label font size");

            return attribute_attributeCytoscapeName_dict;
        }
        private void Write_individual_node_in_xgmml_for_cytoscape(Visualization_of_nw_node_line node, Dictionary<string, string> resourceID_base64String_dict, int current_node_suid)
        {
            string spaces = Get_spaces_string();
            string nodeLabel = Split_label_over_multiple_lines_if_too_many_signs(node.Label);

            Dictionary<Visu_node_field_enum, string> nf_aC_dict = Get_visuNode_attributeCytoscapeName_dict();


            string nodeId = node.Id;
            string nodeName = node.Label; // Escape special XML characters
            string nodeFillColor = Hexadecimal_color_class.Get_hexadecimal_code_for_color(node.Fill_colorSpecifications[0].Fill_color);
            string nodeShape = node.Shape_type.ToLower(); // Cytoscape uses lowercase terms (e.g., "ellipse", "rectangle")
            string width_string = node.Geometry_width.ToString(CultureInfo.InvariantCulture);
            string height_string = node.Geometry_height.ToString(CultureInfo.InvariantCulture);
            if ((node.Geometry_height < 0)||(node.Geometry_width < 0)) { throw new Exception(); }
            int ellipse_width_for_piechart = ((int)Math.Round(node.Geometry_width / Options.Circle_to_pie_chart_factor));
            int ellipse_height_for_piechart = ((int)Math.Round(node.Geometry_height / Options.Circle_to_pie_chart_factor));
            string ellipse_width_for_piechart_string = ellipse_width_for_piechart.ToString(CultureInfo.InvariantCulture);
            string ellipse_height_for_piechart_string = ellipse_height_for_piechart.ToString(CultureInfo.InvariantCulture);
            string node_border_width_string = node.Border_width_cytoscape.ToString(CultureInfo.InvariantCulture);
            string label_y_offset_string = "0.00";
            string label_x_offset_string = "0.00";
            int index = 0;
            int label_lines_count = 1;
            string line_break_string = "&#10;";
            string model_label_position = node.Model_position.ToUpper();
            string label_alignment = "C";
            if (model_label_position.Equals("S"))
            {
                label_alignment = "N";
            }
            else if (model_label_position.Equals("E"))
            {
                label_alignment = "W";
                label_x_offset_string = "10.0";
            }
            else { throw new Exception(); }

            while ((index = nodeLabel.IndexOf(line_break_string, index, StringComparison.Ordinal)) != -1)
            {
                label_lines_count++;
                index += line_break_string.Length; // move past the last match
            }

            float label_y_offset = 0;// (float)Math.Round(0.5F * node.Geometry_heigth + 0.5F * 1F * 2 * node.FontSize * Math.Pow(label_lines_count,1.4) * 100)/100;
            double node_area = Math.Pow(node.Geometry_height, 2);
            double pieChart_area = Math.Pow(ellipse_height_for_piechart, 2);
            if (node.Fill_colorSpecifications.Length>=2)
            {
                label_y_offset = (int)Math.Round(0.5/1000 * (ellipse_height_for_piechart - node.Geometry_height))*1000;
                //label_y_offset = (float)Math.Round(0.235F * node.Geometry_height);
            }
            else
            {
                label_y_offset = 0;
            }
            label_y_offset_string = label_y_offset.ToString(CultureInfo.InvariantCulture);

            StringBuilder xgmmlNode = new StringBuilder();

            xgmmlNode.AppendLine(spaces + $"<node id=\"{nodeId}\" label=\"{nodeLabel}\">");
            bool write_attributes_instead_of_graphics = true;
            if (write_attributes_instead_of_graphics)
            {
                //xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Suid]}\" type=\"integer\" value=\"{current_node_suid}\" cy:type=\"Long\"/>");
                xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Shared_name]}\" type=\"string\" value=\"{nodeLabel}\" cy:type=\"String\"/>");
                xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Name]}\" type=\"string\" value=\"{nodeLabel}\" cy:type=\"String\"/>");
                xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Shape_type]}\" type=\"string\" value=\"{nodeShape}\"/>");
                xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Box_label]}\" type=\"string\" value=\"{node.Box_label}\"/>");
                xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Label]}\" type=\"string\" value=\"{nodeLabel}\"/>");
                xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Label_y_offset]}\" type=\"double\" value=\"{model_label_position},{label_alignment},c,{label_x_offset_string},{label_y_offset_string}\"/>");
                xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.FontSize]}\" type=\"double\" value=\"{node.FontSize}\"/>");
                xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.FontStyle]}\" type=\"string\" value=\"{node.FontStyle}\"/>");
                if (node.Fill_colorSpecifications.Length >= 2)
                {
                    xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Resource_id]}\" type=\"string\" value=\"data:image/png;base64,{resourceID_base64String_dict[node.Resource_id]}\"/>");
                    xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Resource_size]}\" type=\"string\" value=\"{height_string}\"/>");
                    xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Border_width]}\" type = \"double\" value = \"0.0\"/>");
                    xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Width]}\" type=\"double\" value=\"{ellipse_width_for_piechart_string}\"/>");
                    xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Height]}\" type=\"double\" value=\"{ellipse_height_for_piechart_string}\"/>");
                }
                else
                {
                    xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Fill_color]}\" type=\"string\" value=\"{nodeFillColor}\"/>");
                    xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Border_color]}\" type = \"string\" value = \"#000000\"/>");
                    xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Border_width]}\" type = \"double\" value = \"{node_border_width_string}\"/>");
                    xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Width]}\" type=\"double\" value=\"{width_string}\"/>");
                    xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Height]}\" type=\"double\" value=\"{height_string}\"/>");
                }
            }
            else
            {
                xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Shared_name]}\" type=\"string\" value=\"{nodeLabel}\" cy:type=\"String\"/>");
                xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Name]}\" type=\"string\" value=\"{nodeLabel}\" cy:type=\"String\"/>");
                xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Box_label]}\" type=\"string\" value=\"{node.Box_label}\"/>");
                xgmmlNode.AppendLine(spaces + $"  <att name = \"{nf_aC_dict[Visu_node_field_enum.Shape_type]}\" type=\"string\" value=\"{nodeShape}\"/>");
                xgmmlNode.AppendLine(spaces + $"  <graphics z=\"0.0\" fill=\"{nodeFillColor}\" type=\"{node.Shape_type}\" outline=\"#000000\"");
                if (node.Fill_colorSpecifications.Length > 0)
                { xgmmlNode.AppendLine(spaces + $"            w=\"{ellipse_width_for_piechart_string}\" h=\"{ellipse_height_for_piechart_string}\">"); }
                else { xgmmlNode.AppendLine(spaces + $"            w=\"{width_string}\" h=\"{height_string}\">"); }
                xgmmlNode.AppendLine(spaces + $"    <att name=\"NODE_LABEL_POSITION\" value=\"S,N,c,0.00,{label_y_offset_string}\" type=\"string\" cy:type=\"String\"/>");
                xgmmlNode.AppendLine(spaces + $"    <att name=\"NODE_LABEL_WIDTH\" value=\"{Options.Cytoscape_label_width}\" type=\"string\" cy:type=\"String\"/>");
                xgmmlNode.AppendLine(spaces + $"    <att name=\"NODE_LABEL_FONT_FACE\" value=\"{node.FontStyle},plain,{node.FontSize}\" type=\"string\" cy:type=\"String\"/>");
                xgmmlNode.AppendLine(spaces + $"    <att name=\"NODE_LABEL_FONT_SIZE\" value=\"{node.FontSize}\" type=\"string\" cy:type=\"String\"/>");
                xgmmlNode.AppendLine(spaces + $"    <att name=\"NODE_BORDER_WIDTH\" value=\"{node_border_width_string}\" type=\"string\" cy:type=\"String\"/>");
                xgmmlNode.AppendLine(spaces + $"    <att name=\"NODE_BORDER_COLOR\" value=\"#000000\" type=\"string\" cy:type=\"String\"/>");
                xgmmlNode.AppendLine(spaces + $"  </graphics>");
            }

            if (node.Border_has_color)
            {
               // xgmmlNode.AppendLine(spaces + $"            outline=\"{node.Border_style_color}\"");
              //  xgmmlNode.AppendLine(spaces + $"            width=\"{node.Border_style_width.ToString(CultureInfo.InvariantCulture)}\"");
            }

            //xgmmlNode.AppendLine(spaces + $"            font=\"{node.FontStyle}\"");
            //xgmmlNode.AppendLine(spaces + $"            fontSize=\"{node.FontSize}\"");
            //xgmmlNode.AppendLine(spaces + $"            label=\"{nodeName}\"");
            //xgmmlNode.AppendLine(spaces + $"            visible=\"true\"/>");
            xgmmlNode.AppendLine(spaces + "</node>");
            string write_line = xgmmlNode.ToString();
            if (write_line.IndexOf(" &amp;#10;") != -1) { throw new Exception(); }


            Writer.WriteLine(write_line);
        }
        private void Write_individual_node_in_graphml_for_yED(Visualization_of_nw_node_line node)
        {
            StringBuilder sb_spaces = new StringBuilder();
            string spaces = Get_spaces_string();
            string node_label = (string)node.Label.Clone();
            int length_of_current_line = node_label.Length;
            node_label = Split_label_over_multiple_lines_if_too_many_signs(node_label);

            string text_node;
            string node_geometry_height_string;
            string node_geometry_width_string;
            string node_border_style_width;
            if (node.Fill_colorSpecifications.Length == 0) { throw new Exception(); }
            else if (node.Fill_colorSpecifications.Length == 1)
            {
                node_geometry_height_string = node.Geometry_height.ToString(CultureInfo.InvariantCulture);
                node_geometry_width_string = node.Geometry_width.ToString(CultureInfo.InvariantCulture);
                string hexadecimal_color = Hexadecimal_color_class.Get_hexadecimal_code_for_color(node.Fill_colorSpecifications[0].Fill_color);
                if (node.Border_has_color)
                {
                    node_border_style_width = node.Border_width_yED.ToString(CultureInfo.InvariantCulture);
                    text_node =
                      spaces + " <node id=\"" + node.Id + "\">\n"
                    + spaces + "  <data key=\"" + Key.Node_url + "\"/>\n"
                    + spaces + "  <data key=\"" + Key.Node_graphics + "\">\n"
                    + spaces + "    <y:ShapeNode>\n"
                    + spaces + "      <y:Geometry height=\"" + node_geometry_height_string + "\" width=\"" + node_geometry_width_string + "\" x=\"94.3427734375\" y=\"208.0\"/>\n"
                    + spaces + "      <y:Fill color=\"" + hexadecimal_color + "\" transparent=\"" + node.Transparent + "\"/>\n"
                    + spaces + "      <y:BorderStyle color=\"" + node.Border_color + "\" type=\"line\" width=\"" + node_border_style_width + "\"/>\n"
                    + spaces + "      <y:NodeLabel alignment=\"" + node.Label_alignment + "\" autoSizePolicy=\"content\" fontFamily=\"Arial\" fontSize=\"" + node.FontSize + "\" fontStyle=\"" + node.FontStyle + "\" hasBackgroundColor=\"false\" hasLineColor=\"" + node.LabelHasLineColor + "\" height=\"18.701171875\" modelName=\"" + node.Model_name + "\" modelPosition=\"" + node.Model_position + "\" textColor=\"" + node.TextColor + "\" visible=\"true\" width=\"64.703125\" x=\"-17.3515625\" y=\"5.6494140625\">" + node_label + "</y:NodeLabel>\n"
                    + spaces + "      <y:Shape type=\"" + node.Shape_type + "\"/>\n"
                    + spaces + "    </y:ShapeNode>\n"
                    + spaces + "  </data>\n"
                    + spaces + "</node>";
                }
                else
                {
                    text_node =
                      spaces + "<node id=\"" + node.Id + "\">\n"
                    + spaces + "  <data key=\"" + Key.Node_url + "\"/>\n"
                    + spaces + "  <data key=\"" + Key.Node_graphics + "\">\n"
                    + spaces + "    <y:ShapeNode>\n"
                    + spaces + "      <y:Geometry height=\"" + node_geometry_height_string + "\" width=\"" + node_geometry_width_string + "\" x=\"94.3427734375\" y=\"208.0\"/>\n"
                    + spaces + "      <y:Fill color=\"" + hexadecimal_color + "\" transparent=\"" + node.Transparent + "\"/>\n"
                    + spaces + "      <y:BorderStyle hasColor=\"false\" raised=\"false\" type=\"line\" width=\"0.0\"/>\n"
                    + spaces + "      <y:NodeLabel alignment=\"" + node.Label_alignment + "\" autoSizePolicy=\"content\" fontFamily=\"Arial\" fontSize=\"" + node.FontSize + "\" fontStyle=\"" + node.FontStyle + "\" hasBackgroundColor=\"false\" hasLineColor=\"" + node.LabelHasLineColor + "\" height=\"18.701171875\" modelName=\"" + node.Model_name + "\" modelPosition=\"" + node.Model_position + "\" textColor=\"" + node.TextColor + "\" visible=\"true\" width=\"64.703125\" x=\"-17.3515625\" y=\"5.6494140625\">" + node_label + "</y:NodeLabel>\n"
                    + spaces + "      <y:Shape type=\"" + node.Shape_type + "\"/>\n"
                    + spaces + "    </y:ShapeNode>\n"
                    + spaces + "  </data>\n"
                    + spaces + "</node>";
                }
            }
            else
            {
                node_geometry_height_string = (node.Geometry_height * 1.2).ToString(CultureInfo.InvariantCulture);
                node_geometry_width_string = (node.Geometry_width * 1.2).ToString(CultureInfo.InvariantCulture);
                node_border_style_width = node.Border_width_yED.ToString(CultureInfo.InvariantCulture);
                if (String.IsNullOrEmpty(node.Resource_id)) { throw new Exception(); }
                text_node =
                  spaces + "<node id=\"" + node.Id + "\">\n"
                + spaces + "  <data key=\"" + Key.Node_url + "\"/>\n"
                + spaces + "  <data key=\"" + Key.Node_graphics + "\">\n"
                + spaces + "    <y:ImageNode>\n"
                + spaces + "      <y:Geometry height=\"" + node_geometry_height_string + "\" width=\"" + node_geometry_width_string + "\" x=\"812.0\" y=\"187.0\"/>\n"
                + spaces + "      <y:Fill color=\"#CCCCFF\" transparent=\"" + node.Transparent + "\"/>\n"
                + spaces + "      <y:BorderStyle color=\"" + node.Border_color + "\" type=\"line\" width=\"" + node_border_style_width + "\" />\n"
                + spaces + "      <y:NodeLabel alignment=\"" + node.Label_alignment + "\" autoSizePolicy=\"content\" fontFamily=\"Arial\" fontSize=\"" + node.FontSize + "\" fontStyle=\"" + node.FontStyle + "\" hasLineColor=\"" + node.LabelHasLineColor + "\" height=\"18.701171875\" horizontalTextPosition=\"center\" iconTextGap=\"4\" modelName=\"" + node.Model_name + "\" modelPosition=\"" + node.Model_position + "\" textColor=\"" + node.TextColor + "\" verticalTextPosition=\"bottom\" visible=\"true\" width=\"41.34765625\" x=\"129.326171875\" xml:space=\"preserve\" y=\"304.0\">" + node_label + "</y:NodeLabel>\n"
                + spaces + "      <y:Image alphaImage=\"true\" refid=\"" + node.Resource_id + "\"/>\n"
                + spaces + "    </y:ImageNode>\n"
                + spaces + "  </data>\n"
                + spaces + "</node>";
            }
            Writer.WriteLine(text_node);
        }
        private void Write_individual_node(Visualization_of_nw_node_line node_line, Dictionary<string, string> resourceID_base64String_dict, int current_node_suid)
        {
            switch (Options.Graph_editor)
            {
                case Graph_editor_enum.Cytoscape:
                    Write_individual_node_in_xgmml_for_cytoscape(node_line, resourceID_base64String_dict, current_node_suid);
                    break;
                case Graph_editor_enum.yED:
                    Write_individual_node_in_graphml_for_yED(node_line);
                    break;
                default:
                    throw new Exception();
            }
        }
        private void Write_all_nodes(Visualization_of_nw_node_line[] nodes, bool group_same_level_nodes, Visualization_of_nw_resource_line_class[] resources)
        {
            Dictionary<string, string> resourceID_base64String_dict = new Dictionary<string, string>();
            foreach (Visualization_of_nw_resource_line_class resource_line in resources)
            {
                resourceID_base64String_dict.Add(resource_line.Resource_id, resource_line.Base64String);
            }


            int nodes_length = nodes.Length;
            Visualization_of_nw_node_line node_line;
            Dictionary<string, int> boxedNode_SUID_dict = new Dictionary<string, int>();
            int current_node_suid;
            if (group_same_level_nodes)
            {
                //nodes = nodes.OrderBy(l => l.Box_label).ThenBy(l => l.Id).ToArray();
                nodes = Visualization_of_nw_node_line.Order_by_boxLabel_id(nodes);
                current_node_suid = 0;
                for (int indexN = 0; indexN < nodes_length; indexN++)
                {
                    node_line = nodes[indexN];
                    current_node_suid++;
                    if (node_line.Box_label.Equals("-1")) { throw new Exception(); }
                    if ((indexN == 0) || (!node_line.Box_label.Equals(nodes[indexN - 1].Box_label)))
                    {
                        Write_box_group_start_and_boxes(node_line.Box_label);
                        boxedNode_SUID_dict.Clear();
                    }
                    boxedNode_SUID_dict.Add(node_line.Id, current_node_suid);
                    Write_individual_node(node_line, resourceID_base64String_dict, current_node_suid);
                    if ((indexN == nodes_length - 1) || (!node_line.Box_label.Equals(nodes[indexN + 1].Box_label)))
                    {
                        Write_box_group_end(node_line.Box_label, boxedNode_SUID_dict);
                    }
                }
            }
            else
            {
                //nodes = nodes.OrderBy(l => l.Box_label).ThenBy(l => l.Id).ToArray();
                nodes = Visualization_of_nw_node_line.Order_by_boxLabel_id(nodes);
                current_node_suid = 0;
                for (int indexN = 0; indexN < nodes_length; indexN++)
                {
                    node_line = nodes[indexN];
                    current_node_suid++;
                    if ((indexN == 0) || (!node_line.Box_label.Equals(nodes[indexN - 1].Box_label)))
                    {
                        if (node_line.Box_label.Equals(Global_class.Network_legend_box_label))
                        {
                            Write_box_group_start_and_boxes(node_line.Box_label);
                            boxedNode_SUID_dict.Clear();
                        }
                    }
                    boxedNode_SUID_dict.Add(node_line.Id, current_node_suid);
                    Write_individual_node(node_line, resourceID_base64String_dict, current_node_suid);
                    if ((indexN == nodes_length - 1) || (!node_line.Box_label.Equals(nodes[indexN + 1].Box_label)))
                    {
                        if (node_line.Box_label.Equals(Global_class.Network_legend_box_label))
                        {
                            Write_box_group_end(node_line.Box_label, boxedNode_SUID_dict);
                        }
                    }
                }
            }
        }
        #endregion

        #region Write yED Resources
        private void Write_individual_resource_in_graphml_for_yED(Visualization_of_nw_resource_line_class resource)
        {
            StringBuilder sb_spaces = new StringBuilder();
            string spaces = Get_spaces_string();

            string text_resource = "error";
            //text_resource = spaces + "<data key=\"" + Key.Resource_graphml + "\">\n"
            text_resource = spaces + "  <y:Resource id=\"" + resource.Resource_id + "\" type=\"java.awt.image.BufferedImage\" xml:space=\"preserve\">" + resource.Base64String
                                     + "</y:Resource>\n";
            Writer.WriteLine(text_resource);
        }
        private void Write_individual_resource_in_xgmml_for_cytoscape(Visualization_of_nw_resource_line_class resource)
        {
            string spaces = Get_spaces_string();
            string textResource =
                spaces + $"<att name=\"image\" cytoscapeType=\"nodeImage\" value=\"{resource.Base64String}\""
                      + $" xlink:href=\"embedded\" id=\"{resource.Resource_id}\" type=\"java.awt.image.BufferedImage\" />";
            Writer.WriteLine(textResource);
        }
        private void Write_individual_resource(Visualization_of_nw_resource_line_class resource)
        {
            switch (Options.Graph_editor)
            {
                case Graph_editor_enum.Cytoscape:
                    //Write_individual_resource_in_xgmml_for_cytoscape(resource);
                    break;
                case Graph_editor_enum.yED:
                    Write_individual_resource_in_graphml_for_yED(resource);
                    break;
                default:
                    throw new Exception();
            }
        }

        private void Write_all_resources_if_necessary(Visualization_of_nw_resource_line_class[] resources)
        {
            switch (Options.Graph_editor)
            {
                case Graph_editor_enum.yED:
                    int resources_length = resources.Length;
                    Visualization_of_nw_resource_line_class resource_line;
                    //resources = resources.OrderBy(l => l.Resource_id).ToArray();
                    resources = Visualization_of_nw_resource_line_class.Order_by_resourceId(resources);
                    string text = "<data key=\"" + Key.Resource_graphml + "\">\n"
                                  + "<y:Resources>\n";
                    Writer.WriteLine(text);
                    Shift_text_right = Shift_text_right + 2;

                    for (int indexN = 0; indexN < resources_length; indexN++)
                    {
                        resource_line = resources[indexN];
                        Write_individual_resource(resource_line);
                    }
                    text = "</y:Resources>\n"
                           + "</data>";
                    Shift_text_right = Shift_text_right - 2;
                    Writer.WriteLine(text);
                    break;
                case Graph_editor_enum.Cytoscape:
                    break;
                default:
                    throw new Exception();
            }
        }
        #endregion

        #region Write yED Edges
        private void Write_individual_edge_in_graphml_for_yED(Visualization_of_nw_edge_line edge)
        {
            string spaces = Get_spaces_string();
            string text_edge =
                   spaces + "<edge id=\"" + edge.Edge_id + "\" source=\"" + edge.Source_id + "\" target=\"" + edge.Target_id + "\">\n"
                 + spaces + "  <data key=\"" + Key.Edge_description + "\"/>\n"
                 + spaces + "  <data key=\"" + Key.Edge_graphics + "\">\n"
                 + spaces + "    <y:PolyLineEdge>\n"
                 + spaces + "      <y:Path sx=\"0.0\" sy=\"0.0\" tx=\"0.0\" ty=\"0.0\"/>\n"
                 + spaces + "      <y:LineStyle color=\"" + edge.Arrow_color + "\" type=\"" + edge.Arrow_type_yED + "\" width=\"" + edge.Arrow_width + "\"/>\n"
                 + spaces + "      <y:Arrows source=\"" + edge.Arrow_source_end_yEd + "\" target=\"" + edge.Arrow_target_end_yEd + "\"/>\n"
                 + spaces + "      <y:EdgeLabel alignment=\"center\" configuration=\"AutoFlippingLabel\" distance=\"2.0\" fontFamily=\"Dialog\" fontSize=\"" + edge.Arrow_label_font_size + "\" fontStyle=\"plain\" hasBackgroundColor=\"false\" hasLineColor=\"false\" height=\"28.501953125\" modelName=\"two_pos\" modelPosition=\"head\" preferredPlacement=\"anywhere\" ratio=\"0.5\" textColor=\"#000000\" visible=\"true\" width=\"25.123046875\" x=\"-72.580322265625\" y=\"-84.35603932425425\">" + edge.Arrow_label + "<y:PreferredPlacementDescriptor angle=\"0.0\" angleOffsetOnRightSide=\"0\" angleReference=\"absolute\" angleRotationOnRightSide=\"co\" distance=\"-1.0\" frozen=\"true\" placement=\"anywhere\" side=\"anywhere\" sideReference=\"relative_to_edge_flow\"/>"
                 + spaces + "      </y:EdgeLabel>"
                 + spaces + "      <y:BendStyle smoothed=\"false\"/>\n"
                 + spaces + "    </y:PolyLineEdge>\n"
                 + spaces + "  </data>\n"
                 + spaces + "</edge>";
            Writer.WriteLine(text_edge);
        }
        private void Write_individual_edge_in_xgmml_for_cytoscape(Visualization_of_nw_edge_line edge)
        {
            Dictionary<Visu_node_field_enum, string> attribute_attributeCytoscapeName_dict = new Dictionary<Visu_node_field_enum, string>();

            string spaces = Get_spaces_string();
            string label = edge.Arrow_label;

            StringBuilder xgmmlEdge = new StringBuilder();

            xgmmlEdge.AppendLine(spaces + $"<edge id=\"{edge.Edge_id}\" source=\"{edge.Source_id}\" target=\"{edge.Target_id}\" label=\"{label}\">");
            xgmmlEdge.AppendLine(spaces + $"  <att name=\"interaction\" value=\"{edge.Arrow_type_cytoscape}\" type=\"string\"/>");
            xgmmlEdge.AppendLine(spaces + $"  <att name=\"EdgeLabel\" value=\"{label}\" type=\"string\"/>");
            xgmmlEdge.AppendLine(spaces + $"  <graphics width=\"{edge.Arrow_width.ToString(CultureInfo.InvariantCulture)}\" fill=\"{edge.Arrow_color}\"");
            xgmmlEdge.AppendLine(spaces + $"            edgeLinetype=\"{edge.Arrow_type_cytoscape}\"");
            xgmmlEdge.AppendLine(spaces + $"            targetArrow=\"{edge.Arrow_target_end_cytoscape}\"");
            xgmmlEdge.AppendLine(spaces + $"            sourceArrow=\"{edge.Arrow_source_end_cytoscape}\"");
            xgmmlEdge.AppendLine(spaces + $"            fontSize=\"{edge.Arrow_label_font_size}\" label=\"{label}\"/>");
            xgmmlEdge.AppendLine(spaces + $"</edge>");

            Writer.WriteLine(xgmmlEdge.ToString());
        }
        private void Write_individual_edge(Visualization_of_nw_edge_line edge)
        {
            switch (Options.Graph_editor)
            {
                case Graph_editor_enum.Cytoscape:
                    Write_individual_edge_in_xgmml_for_cytoscape(edge);
                    break;
                case Graph_editor_enum.yED:
                    Write_individual_edge_in_graphml_for_yED(edge);
                    break;
                default:
                    throw new Exception();
            }
        }
        private void Write_all_edges(Visualization_of_nw_edge_line[] edges)
        {
            int edges_length = edges.Length;
            //edges = edges.OrderBy(l => l.Edge_id).ToArray();
            edges = Visualization_of_nw_edge_line.Order_edgeId(edges);
            Visualization_of_nw_edge_line edge_line;
            string[] scps;
            Dictionary<string, Dictionary<string,bool>> scp_scp_dict = new Dictionary<string, Dictionary<string, bool>>();
            for (int indexE = 0; indexE < edges_length; indexE++)
            {
                edge_line = edges[indexE];
                scps = new string[] {edge_line.Source_id, edge_line.Target_id};
                scps = scps.OrderBy(l=>l).ToArray();
                if (!scp_scp_dict.ContainsKey(scps[0]))
                {
                    scp_scp_dict.Add(scps[0], new Dictionary<string, bool>());
                }
                scp_scp_dict[scps[0]].Add(scps[1], true);
                Write_individual_edge(edge_line);
            }
        }
        #endregion

        #region Write yED level boxes
        private void Write_box_group_start_and_boxes_in_graphml_for_yED(string box_name)
        {
            string box_id = box_name;
            string graph_id = box_name + "_graph_id";
            string name_open_box = "MBCO " + box_name.Replace("_", " ");
            string name_closed_box = "MBCO " + box_name.Replace("_", " ");

            string spaces = Get_spaces_string();
            string text_group_start = spaces + "<node id=\"" + box_id + "\" yfiles.foldertype=\"group\">\n";
            Writer.Write(text_group_start);

            Shift_text_right += 2;
            spaces = Get_spaces_string();
            string text_group_boxes =
                spaces + "<data key=\"" + Key.Node_url + "\"/>\n"
              + spaces + "<data key=\"" + Key.Node_graphics + "\">\n"
              + spaces + "  <y:ProxyAutoBoundsNode>\n"
              + spaces + "    <y:Realizers active=\"0\">\n"
              + spaces + "      <y:GroupNode>\n"
              + spaces + "        <y:Geometry height=\"191.2364196849619\" width=\"353.5269841269842\" x=\"431.6801587301587\" y=\"101.4834899974619\"/>\n"
              + spaces + "        <y:Fill color=\"#F5F5F5\" transparent=\"false\"/>\n"
              + spaces + "        <y:BorderStyle color=\"#000000\" type=\"dashed\" width=\"1.0\"/>\n"
              + spaces + "        <y:NodeLabel alignment=\"right\" autoSizePolicy=\"node_width\" backgroundColor=\"#EBEBEB\" borderDistance=\"0.0\" fontFamily=\"Dialog\" fontSize=\"50\" fontStyle=\"plain\" hasLineColor=\"false\" height=\"22.37646484375\" modelName=\"internal\" modelPosition=\"t\" textColor=\"#000000\" visible=\"true\" width=\"353.5269841269842\" x=\"0.0\" y=\"0.0\">" + name_open_box + "</y:NodeLabel>\n"
              + spaces + "        <y:Shape type=\"roundrectangle\"/>\n"
              + spaces + "        <y:State closed=\"false\" closedHeight=\"50.0\" closedWidth=\"50.0\" innerGraphDisplayEnabled=\"false\"/>\n"
              + spaces + "        <y:Insets bottom=\"15\" bottomF=\"15.0\" left=\"15\" leftF=\"15.0\" right=\"15\" rightF=\"15.0\" top=\"15\" topF=\"15.0\"/>\n"
              + spaces + "        <y:BorderInsets bottom=\"9\" bottomF=\"8.57568359375\" left=\"112\" leftF=\"112.0976190476191\" right=\"104\" rightF=\"103.7509765625\" top=\"0\" topF=\"0.0\"/>\n"
              + spaces + "      </y:GroupNode>\n"
              + spaces + "      <y:GroupNode>\n"
              + spaces + "        <y:Geometry height=\"50.0\" width=\"50.0\" x=\"431.6801587301587\" y=\"101.4834899974619\"/>\n"
              + spaces + "        <y:Fill color=\"#F2F0D8\" transparent=\"false\"/>\n"
              + spaces + "        <y:BorderStyle color=\"#000000\" type=\"line\" width=\"1.0\"/>\n"
              + spaces + "        <y:NodeLabel alignment=\"right\" autoSizePolicy=\"node_width\" backgroundColor=\"#B7B69E\" borderDistance=\"0.0\" fontFamily=\"Dialog\" fontSize=\"15\" fontStyle=\"plain\" hasLineColor=\"false\" height=\"22.37646484375\" modelName=\"internal\" modelPosition=\"t\" textColor=\"#000000\" visible=\"true\" width=\"75.69677734375\" x=\"-12.848388671875\" y=\"0.0\">" + name_closed_box + "</y:NodeLabel>\n"
              + spaces + "        <y:Shape type=\"rectangle\"/>\n"
              + spaces + "        <y:DropShadow color=\"#D2D2D2\" offsetX=\"4\" offsetY=\"4\"/>\n"
              + spaces + "        <y:State closed=\"true\" closedHeight=\"50.0\" closedWidth=\"50.0\" innerGraphDisplayEnabled=\"false\"/>\n"
              + spaces + "        <y:Insets bottom=\"5\" bottomF=\"5.0\" left=\"5\" leftF=\"5.0\" right=\"5\" rightF=\"5.0\" top=\"5\" topF=\"5.0\"/>\n"
              + spaces + "        <y:BorderInsets bottom=\"0\" bottomF=\"0.0\" left=\"0\" leftF=\"0.0\" right=\"0\" rightF=\"0.0\" top=\"0\" topF=\"0.0\"/>\n"
              + spaces + "      </y:GroupNode>\n"
              + spaces + "    </y:Realizers>\n"
              + spaces + "</y:ProxyAutoBoundsNode>\n"
              + spaces + "</data>\n"
              + spaces + "<graph edgedefault=\"directed\" id=\"" + graph_id + ":\">";
            Writer.WriteLine(text_group_boxes);
        }
        private void Write_box_group_start_and_boxes_in_xgmml_for_cytoscape(string box_name)
        {
            Shift_text_right += 6;
        }
        private void Write_box_group_end_in_xgmml_for_cytoscape(string box_name, Dictionary<string,int> linkedNodeIds_nodeSUIDs_dict)
        {
            string box_id = box_name;
            string label = "MBCO " + box_name.Replace("_", " ");
            string spaces = Get_spaces_string();

            Shift_text_right -= 6;

            Writer.WriteLine(spaces + "<node id=\"{0}\" label=\"{1}\">", box_name, label);
            Writer.WriteLine(spaces + "  <att>");
            Writer.WriteLine(spaces + "    <graph id=\"{0}_graphId\" label=\"{1}\" cy:registered=\"0\">", box_name, label);
            Writer.WriteLine(spaces + "    <att name=\"__groupShown.SUID\" type=\"list\" cy:elementType=\"Long\" cy:hidden=\"1\">");
            string[] linkedNodeIds = linkedNodeIds_nodeSUIDs_dict.Keys.ToArray();
            string linkedNodeId;
            int linkedNodeIds_length = linkedNodeIds.Length;
            int nodeSUID;
            for (int indexN=0; indexN<linkedNodeIds_length;indexN++)
            {
                linkedNodeId = linkedNodeIds[indexN];
                nodeSUID = linkedNodeIds_nodeSUIDs_dict[linkedNodeId];
                Writer.WriteLine(spaces + "      <att name=\"__groupShown.SUID\" value=\"{0}\" type=\"integer\" cy:type=\"Long\" cy:hidden=\"1\"/>", nodeSUID);
            }
            Writer.WriteLine(spaces + "    </att>");
            //string linked_node_id;
            for (int indexN = 0; indexN < linkedNodeIds_length; indexN++)
            {
                linkedNodeId = linkedNodeIds[indexN];
                nodeSUID = linkedNodeIds_nodeSUIDs_dict[linkedNodeId];
                Writer.WriteLine(spaces + "      <node xlink:href=\"#{0}\"/>", nodeSUID);
            }
            Writer.WriteLine(spaces + "    </graph>");
            Writer.WriteLine(spaces + "  </att>");
            Writer.WriteLine(spaces + "</node>");
        }

        private void Write_box_group_start_and_boxes(string box_name)
        {
            switch (Options.Graph_editor)
            {
                case Graph_editor_enum.Cytoscape:
                    //Write_box_group_start_and_boxes_in_xgmml_for_cytoscape(box_name);
                    break;
                case Graph_editor_enum.yED:
                    Write_box_group_start_and_boxes_in_graphml_for_yED(box_name);
                    break;
                default:
                    throw new Exception();
            }
        }
        private void Write_box_group_end_in_graphml_for_yED()
        {
            Shift_text_right -= 2;
            string space = Get_spaces_string();
            string text_group_end =
                  space + "  </graph>\n"
                + space + "</node>\n";
            Writer.WriteLine(text_group_end);
        }
        private void Write_box_group_end(string box_name, Dictionary<string, int> linkedNodeIds_nodeSUIDs_dict)
        {
            switch (Options.Graph_editor)
            {
                case Graph_editor_enum.Cytoscape:
                    //Write_box_group_end_in_xgmml_for_cytoscape(box_name, linkedNodeIds_nodeSUIDs_dict);
                    break;
                case Graph_editor_enum.yED:
                    Write_box_group_end_in_graphml_for_yED();
                    break;
                default:
                    throw new Exception();
            }
        }
        #endregion

        #region Generate yED edges
        private Visualization_of_nw_edge_line Generate_individual_edge(string source, Visualization_nw_edge_characterisation_line_class target)
        {
            Visualization_of_nw_edge_line new_edge_line = new Visualization_of_nw_edge_line();
            new_edge_line.Arrow_color = Hexadecimal_color_class.Get_hexadecimal_code_for_color(Options.Edge_color);
            new_edge_line.Edge_id = source + "_to_" + target.Target;
            new_edge_line.Source_id = source;
            new_edge_line.Target_id = target.Target;
            new_edge_line.Arrow_width = target.Edge_width.ToString();
            new_edge_line.Arrow_label = (string)target.Edge_label.Clone();
            new_edge_line.EdgeArrow_type = target.EdgeArrow_type;
            return new_edge_line;
        }
        private Visualization_of_nw_edge_line[] Generate_edges(Dictionary<string, Visualization_nw_edge_characterisation_line_class[]> nw)
        {
            List<Visualization_of_nw_edge_line> edges_list = new List<Visualization_of_nw_edge_line>();
            Visualization_of_nw_edge_line new_edge_line;
            string[] sources = nw.Keys.ToArray();
            Visualization_nw_edge_characterisation_line_class[] targets;
            int targets_length;
            Visualization_nw_edge_characterisation_line_class target;
            string source;
            int sources_length = sources.Length;
            for (int indexS = 0; indexS < sources_length; indexS++)
            {
                source = sources[indexS];
                targets = nw[source].ToArray();
                targets_length = targets.Length;
                for (int indexT = 0; indexT < targets_length; indexT++)
                {
                    target = targets[indexT];
                    new_edge_line = Generate_individual_edge(source, target);
                    edges_list.Add(new_edge_line);
                }
            }
            return edges_list.ToArray();
        }
        #endregion

        #region Generate yED nodes
        private string Get_shape_code(Visualization_of_nw_node_line node_line, Shape_enum node_shape)
        {
            string shape_code;
            switch (node_shape)
            {
                case Shape_enum.Ellipse:
                    shape_code = "ellipse";
                    break;
                case Shape_enum.Rectangle:
                    shape_code = "rectangle";
                    break;
                case Shape_enum.Diamond:
                    shape_code = "diamond";
                    break;
                case Shape_enum.Octagon:
                    shape_code = "octagon";
                    break;
                case Shape_enum.Triangle_pointing_up:
                    shape_code = "triangle";
                    break;
                case Shape_enum.Triangle_pointing_down:
                    shape_code = "triangle2";
                    break;
                case Shape_enum.Star_five_pointed:
                    shape_code = "star5";
                    break;
                default:
                    throw new Exception("Shape type is not considered");
            }
            return shape_code;
        }

        private Visualization_of_nw_node_line[] Generate_nodes(NetworkNode_line_class[] input_nodes)
        {
            NetworkNode_line_class input_node;
            int input_nodes_length = input_nodes.Length;
            Visualization_of_nw_node_line[] nodes = new Visualization_of_nw_node_line[input_nodes_length];
            Visualization_of_nw_node_line node;
            for (int indexI = 0; indexI < input_nodes_length; indexI++)
            {
                input_node = input_nodes[indexI];
                node = new Visualization_of_nw_node_line();
                node.Id = (string)input_node.Name.Clone();
                node.Label = (string)input_node.Name.Clone();
                if (Options.Legend_dataset_node_dict.ContainsKey(node.Id))
                {
                    node.Shape_type = Get_shape_code(node, Options.Legend_dataset_node_shape);
                }
                else
                {
                    node.Shape_type = Get_shape_code(node, Options.Node_shape);
                }
                node.FontSize = Options.Min_label_size;
                node.Model_name = "sides";
                if (input_node.Level==Global_class.Network_legend_level)
                {
                    node.Label_alignment = "left";
                    node.Model_position = "e";
                }
                else
                {
                    node.Label_alignment = "center";
                    node.Model_position = "s";
                }
                if (input_node.Level.Equals(Global_class.Network_legend_level))
                {
                    node.Box_label = (string)Global_class.Network_legend_box_label.Clone();
                }
                else if (input_node.Level.Equals(Global_class.Network_genes_level))
                {
                    node.Box_label = (string)Global_class.Network_genes_box_label.Clone();
                }
                else
                {
                    node.Box_label = "Level " + input_node.Level;
                }
                nodes[indexI] = node;
            }
            return nodes;
        }

        private void Remove_illegal_characters(ref Visualization_of_nw_node_line[] nodes, ref Visualization_of_nw_edge_line[] edges, ref yed_node_color_line_class[] node_colors)
        {
            foreach (Visualization_of_nw_node_line node_line in nodes)
            {
                node_line.Id = node_line.Id.Replace("&", "and");
                node_line.Label = node_line.Label.Replace("&", "and");
            }
            foreach (Visualization_of_nw_edge_line edge_line in edges)
            {
                edge_line.Edge_id = edge_line.Edge_id.Replace("&", "and");
                edge_line.Target_id = edge_line.Target_id.Replace("&", "and");
                edge_line.Source_id = edge_line.Source_id.Replace("&", "and");
            }
            foreach (yed_node_color_line_class node_color in node_colors)
            {
                node_color.NodeName = node_color.NodeName.Replace("&", "and");
            }
        }

        private void Get_max_and_min_labelFontSize_for_given_max_and_min_nodeSizes(out int max_labelSize, out int min_labelSize, out int label_size_for_diameter_100, float max_nodeDiameter, float min_nodeDiameter)
        {
            label_size_for_diameter_100 = 50;
            int minimum_min_labelSize = 35;
            int maximum_max_labelSize = 100;
            min_labelSize = (int)Math.Round(label_size_for_diameter_100 * (float)Math.Pow(min_nodeDiameter / 100F, 2));
            max_labelSize = (int)Math.Round(label_size_for_diameter_100 * (float)Math.Pow(max_nodeDiameter / 100F, 2));
            if ((min_labelSize < minimum_min_labelSize) && (max_labelSize <= maximum_max_labelSize))
            {
                min_labelSize = minimum_min_labelSize;
                max_labelSize = min_labelSize + (int)Math.Round((label_size_for_diameter_100 - min_labelSize) * (float)Math.Pow((max_nodeDiameter - min_nodeDiameter) / (100F - min_nodeDiameter), 2));
            }
            else if ((max_labelSize > maximum_max_labelSize) && (min_labelSize >= minimum_min_labelSize))
            {
                max_labelSize = maximum_max_labelSize;
                min_labelSize = max_labelSize - (int)Math.Round((max_labelSize - label_size_for_diameter_100) * (float)Math.Pow((max_nodeDiameter - min_nodeDiameter) / (max_nodeDiameter - 100F), 2));
            }
            else if ((max_labelSize > maximum_max_labelSize) && (min_labelSize < minimum_min_labelSize))
            {
                min_labelSize = minimum_min_labelSize;
                max_labelSize = maximum_max_labelSize;
            }
            if (Global_class.Do_internal_checks)
            {
                if (max_nodeDiameter == 100 && max_labelSize != label_size_for_diameter_100) { throw new Exception(); }
                if (max_labelSize<min_labelSize) {  throw new Exception(); }
            }
        }

        private Visualization_of_nw_node_line[] Adjust_node_colors_and_sizes(Visualization_of_nw_node_line[] nodes, params yed_node_color_line_class[] node_colors)
        {
            float max_observed_node_area = -1;
            float min_observed_node_area = -1;

            //nodes = nodes.OrderBy(l => l.Id).ToArray();
            nodes = Visualization_of_nw_node_line.Order_by_id(nodes);
            Visualization_of_nw_node_line node_line;
            int nodes_length = nodes.Length;
            //node_colors = node_colors.OrderBy(l => l.NodeName).ToArray();
            node_colors = yed_node_color_line_class.Order_by_nodeName(node_colors);
            int node_colors_length = node_colors.Length;
            int indexNode = 0;
            int stringCompare;
            yed_node_color_line_class node_color;
            int color_specifications_length;
            float current_node_area = -1;
            int indexMaxObservedArea = -1;
            for (int indexColor = 0; indexColor < node_colors_length; indexColor++)
            {
                node_color = node_colors[indexColor];
                stringCompare = -2;
                while ((indexNode < nodes_length) && (stringCompare < 0))
                {
                    node_line = nodes[indexNode];
                    stringCompare = node_line.Id.CompareTo(node_color.NodeName);
                    if (stringCompare < 0)
                    {
                        indexNode++;
                    }
                    else if (stringCompare == 0)
                    {
                        color_specifications_length = node_color.Color_specifications.Length;
                        node_line.Fill_colorSpecifications = new Color_specification_line_class[color_specifications_length];
                        for (int indexFillColorSpeci = 0; indexFillColorSpeci<color_specifications_length;indexFillColorSpeci++)
                        {
                            node_line.Fill_colorSpecifications[indexFillColorSpeci] = node_color.Color_specifications[indexFillColorSpeci].Deep_copy();
                        }
                        switch (Options.Node_size_determinant)
                        {
                            case Yed_network_node_size_determinant_enum.No_of_sets:
                                current_node_area = node_line.Fill_colorSpecifications.Length;
                                break;
                            case Yed_network_node_size_determinant_enum.Minus_log10_pvalue:
                                float sum_minusLog10Pvalues = 0;
                                foreach (Color_specification_line_class fill_color_specification_line in node_line.Fill_colorSpecifications)
                                {
                                    sum_minusLog10Pvalues += fill_color_specification_line.Size;
                                }
                                current_node_area = sum_minusLog10Pvalues;
                                break;
                            case Yed_network_node_size_determinant_enum.No_of_different_colors:
                                Dictionary<Color, bool> colors_dict = new Dictionary<Color, bool>();
                                foreach (Color_specification_line_class fill_color_specification_line in node_line.Fill_colorSpecifications)
                                {
                                    if (!colors_dict.ContainsKey(fill_color_specification_line.Fill_color))
                                    {
                                        colors_dict.Add(fill_color_specification_line.Fill_color, true);
                                    }
                                }
                                current_node_area = colors_dict.Keys.Count;
                                break;
                            case Yed_network_node_size_determinant_enum.Uniform:
                                current_node_area = Options.Max_node_diameter;
                                break;
                            default:
                                throw new Exception();
                        }
                        node_line.Geometry_height = current_node_area; //transient save of value under misleading name
                        node_line.Geometry_width = current_node_area; //transient save of value under misleading name
                        if (node_line.Box_label.Equals(Global_class.Network_genes_box_label)) //make gene nodes smaller
                        {
                            node_line.Geometry_width = 0.7F * node_line.Geometry_width;
                            node_line.Geometry_height = 0.7F * node_line.Geometry_height;
                        }
                        if ((max_observed_node_area == -1) || (node_line.Geometry_width > max_observed_node_area))
                        { 
                            max_observed_node_area = node_line.Geometry_width;
                            indexMaxObservedArea = indexNode;
                        }
                        if ((min_observed_node_area == -1) || (node_line.Geometry_width < min_observed_node_area))
                        { 
                            min_observed_node_area = node_line.Geometry_width;
                        }
                    }
                }
            }
            switch (Options.Node_size_determinant)
            {
                case Yed_network_node_size_determinant_enum.Uniform:
                case Yed_network_node_size_determinant_enum.No_of_different_colors:
                case Yed_network_node_size_determinant_enum.No_of_sets:
                case Yed_network_node_size_determinant_enum.Minus_log10_pvalue:
                    float max_anticipated_span_area = (float)Math.PI * (float)Math.Pow((Options.Max_node_diameter/2), 2);
                    float ratio_max_anticipated_to_observed_span_area = max_anticipated_span_area / max_observed_node_area;
                    bool max_height_is_there = false;
                    float max_data_height = -1;
                    int indexMaxHeight = -1;
                    for (int indexNode2 = 0; indexNode2 < nodes_length; indexNode2++)
                    {
                        node_line = nodes[indexNode2];
                        current_node_area = node_line.Geometry_height; //see above: transient save of value under misleading name
                        current_node_area = current_node_area * ratio_max_anticipated_to_observed_span_area;
                        node_line.Geometry_height = (float)Math.Sqrt((float)current_node_area / Math.PI) * 2F;
                        node_line.Geometry_width = node_line.Geometry_height;
                        if (Global_class.Do_internal_checks)
                        {
                            int rounded_geometry_height = (int)Math.Round((Math.Round(100F * node_line.Geometry_height) / 100F));
                            if (rounded_geometry_height > Options.Max_node_diameter) { throw new Exception(); }
                            if (rounded_geometry_height == Options.Max_node_diameter)
                            {
                                max_height_is_there = true;
                            }
                            if (max_data_height<node_line.Geometry_height)
                            {
                                max_data_height = node_line.Geometry_height;
                                indexMaxHeight = indexNode2;
                            }
                        }
                    }
                    if (Global_class.Do_internal_checks)
                    {
                        node_line = nodes[indexMaxHeight];
                        if (!max_height_is_there)
                        { 
                            throw new Exception();
                        }
                    }
                    break;
                default:
                    throw new Exception();
            }

            for (int indexNode2 = 0; indexNode2 < nodes_length; indexNode2++)
            {
                node_line = nodes[indexNode2];
                if (node_line.Fill_colorSpecifications.Length > 1)
                {
                    node_line.Geometry_width = Options.Circle_to_pie_chart_factor * node_line.Geometry_width;
                    node_line.Geometry_height = Options.Circle_to_pie_chart_factor * node_line.Geometry_height;
                }
                if (node_line.Geometry_width < 1) { node_line.Geometry_width = 1; }
                if (node_line.Geometry_height < 1) { node_line.Geometry_height = 1; }
            }
            return nodes;
        }

        private Visualization_of_nw_node_line[] Add_labelSizes_to_nodeSizes(Visualization_of_nw_node_line[] nodes)
        {
            float max_node_diameter = -1;
            float min_node_diameter = -1;
            float corrected_diameter;
            foreach (Visualization_of_nw_node_line node_line in nodes)
            {
                corrected_diameter = node_line.Geometry_width;
                if (node_line.Fill_colorSpecifications.Length>1)
                {
                    corrected_diameter /= Options.Circle_to_pie_chart_factor;
                }
                if (corrected_diameter > max_node_diameter)
                {
                    max_node_diameter = corrected_diameter;
                }
                if (  (min_node_diameter==-1)
                    ||(corrected_diameter < min_node_diameter))
                {
                    min_node_diameter = corrected_diameter;
                }
            }
            if (Global_class.Do_internal_checks)
            {
                if (Math.Round(Math.Round(100F*max_node_diameter)/100F)!=Options.Max_node_diameter)
                {
                    throw new Exception();
                }
            }
            //Get_max_and_min_labelFontSize_for_given_max_and_min_nodeSizes(out int max_labelSize, out int min_labelSize, out int labelSize_for_diameter_100, max_node_diameter, min_node_diameter);
            int max_labelSize = Options.Max_label_size;
            int min_labelSize = Options.Min_label_size;
            if (min_labelSize != max_labelSize)
            {
                foreach (Visualization_of_nw_node_line node_line in nodes)
                {
                    corrected_diameter = node_line.Geometry_width;
                    if (node_line.Fill_colorSpecifications.Length > 1)
                    {
                        corrected_diameter /= Options.Circle_to_pie_chart_factor;
                    }
                    if (max_labelSize==min_labelSize)
                    {
                        node_line.FontSize = max_labelSize;
                    }
                    else if (max_node_diameter==min_node_diameter)
                    {
                        node_line.FontSize = min_labelSize + (int)Math.Round((float)0.5F*(max_labelSize - min_labelSize));
                    }
                    else
                    {
                        node_line.FontSize = min_labelSize + (int)Math.Round((float)(max_labelSize - min_labelSize) * (float)Math.Pow((corrected_diameter - min_node_diameter) / (max_node_diameter - min_node_diameter), 2));
                    }
                    if (Global_class.Do_internal_checks)
                    {
                        if (corrected_diameter.Equals(100))
                        {
                            //if (node_line.FontSize != labelSize_for_diameter_100)
                            //{
                            //    throw new Exception();
                            //}
                        }
                    }
                }
            }
            else
            {
                foreach (Visualization_of_nw_node_line node_line in nodes)
                {
                    node_line.FontSize = min_labelSize;
                }
            }
            return nodes;
        }
        private Visualization_of_nw_resource_line_class[] Generate_resource_lines_with_pieChart_and_connect_to_nodes_for_same_fill_colors_length(ref List<Visualization_of_nw_node_line> sameFillColorsLength_nodes, out bool exception_thrown)
        {
            int colors_length = sameFillColorsLength_nodes[0].Fill_colorSpecifications.Length;
            int sameFillColorsLength_nodes_length = sameFillColorsLength_nodes.Count;
            Visualization_of_nw_resource_line_class resource_line;
            List<Visualization_of_nw_resource_line_class> new_resources = new List<Visualization_of_nw_resource_line_class>();
            Visualization_of_nw_node_line visualization_node_line;
            Visualization_of_nw_node_line inner_visualization_node_line;
            int pieChart_no = -1;
            string pieChart_id = "";
            exception_thrown = false;
            for (int indexSFN = 0; indexSFN < sameFillColorsLength_nodes_length; indexSFN++)
            {
                visualization_node_line = sameFillColorsLength_nodes[indexSFN];
                visualization_node_line.Fill_colorSpecifications = visualization_node_line.Fill_colorSpecifications.OrderBy(l => l.Dataset_order_no).ThenByDescending(l => l.Size).ToArray();
            }
            for (int indexColor = colors_length - 1; indexColor < 0; indexColor--)
            {
                sameFillColorsLength_nodes = sameFillColorsLength_nodes.OrderBy(l => l.Fill_colorSpecifications[indexColor].Dataset_order_no).ToList();
            }
            int firstIndexSameResource = -1;
            for (int indexSFN = 0; indexSFN < sameFillColorsLength_nodes_length; indexSFN++)
            {
                visualization_node_line = sameFillColorsLength_nodes[indexSFN];
                if ((indexSFN == 0) || (!visualization_node_line.SameFillColors_in_same_order(sameFillColorsLength_nodes[indexSFN - 1])))
                {
                    firstIndexSameResource = indexSFN;
                }
                if ((indexSFN == sameFillColorsLength_nodes_length - 1) || (!visualization_node_line.SameFillColors_in_same_order(sameFillColorsLength_nodes[indexSFN + 1])))
                {
                    pieChart_no++;
                    pieChart_id = "PieChart" + colors_length + "_no" + pieChart_no;
                    for (int indexInner = firstIndexSameResource; indexInner <= indexSFN; indexInner++)
                    {
                        inner_visualization_node_line = sameFillColorsLength_nodes[indexInner];
                        inner_visualization_node_line.Resource_id = (string)pieChart_id.Clone();
                    }
                    
                    resource_line = new Visualization_of_nw_resource_line_class();
                    resource_line.Resource_id = (string)pieChart_id.Clone();

                    int baseBorderWidth = 40;
                    baseBorderWidth = (int)Math.Round(baseBorderWidth / Math.Sqrt((double)colors_length));

                    GraphPane pieChart = new GraphPane();
                    Color_specification_line_class[] current_colorSpecifications = visualization_node_line.Fill_colorSpecifications.OrderBy(l => l.Dataset_order_no).ThenByDescending(l=>l.Size).ToArray();
                    Color_specification_line_class cS_line;
                    for (int indexColor = 0; indexColor < colors_length; indexColor++)
                    {
                        cS_line = current_colorSpecifications[indexColor];
                        PieItem pieItem = pieChart.AddPieSlice(cS_line.Size, cS_line.Fill_color, 0, "");
                        pieItem.LabelType = PieLabelType.None;
                        pieItem.Border.Color = Color.Black;
                        pieItem.Border.Width = baseBorderWidth;
                    }

                    pieChart.Rect = new RectangleF(0, 0, 2000, 2000);
                    pieChart.Chart.Fill.IsVisible = false;
                    pieChart.Chart.Fill.Type = ZedGraph.FillType.None;
                    pieChart.Chart.Fill.Color = Color.Transparent;
                    pieChart.Chart.Border.Color = Color.Transparent;
                    pieChart.Margin.All=0;
                    pieChart.Fill.Color = Color.Transparent;
                    pieChart.Fill.IsVisible = false;
                    pieChart.Fill.Type = ZedGraph.FillType.None;
                    pieChart.Border.Color = Color.Transparent;
                    pieChart.Border.IsVisible = false;
                    pieChart.XAxis.IsVisible = false;
                    pieChart.YAxis.IsVisible = false;

                    try
                    {
                        using (MemoryStream memory = new MemoryStream())
                        {
                            using (Bitmap bmp = pieChart.GetImage())
                            {
                                bmp.Save(memory, ImageFormat.Png);
                                // bmp.MakeTransparent();
                                byte[] imageBytes = memory.ToArray();
                                string base64String = Convert.ToBase64String(imageBytes);
                                resource_line.Base64String = (string)base64String.Clone();
                                new_resources.Add(resource_line);
                            }
                        }
                    }
                    catch 
                    { 
                        new_resources.Clear();
                        exception_thrown = true;
                    }
                }
            }
            return new_resources.ToArray();
        }

        private Visualization_of_nw_resource_line_class[] Generate_resource_lines_and_connect_to_nodes(ref Visualization_of_nw_node_line[] nodes, out bool exeption_thrown)
        {
            List<Visualization_of_nw_resource_line_class> resources = new List<Visualization_of_nw_resource_line_class>();
            //nodes = nodes.OrderBy(l => l.Fill_colorSpecifications.Length).ToArray();
            nodes = Visualization_of_nw_node_line.Order_by_fillColorSpecificationLength(nodes);
            int nodes_length = nodes.Length;
            Visualization_of_nw_node_line node_line;
            List<Visualization_of_nw_node_line> sameColorLength_node_lines = new List<Visualization_of_nw_node_line>();
            exeption_thrown=false;
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                node_line = nodes[indexN];
                if (node_line.Fill_colorSpecifications.Length > 1)
                {
                    if ((indexN == 0) || (!node_line.Fill_colorSpecifications.Length.Equals(nodes[indexN - 1].Fill_colorSpecifications.Length)))
                    {
                        sameColorLength_node_lines.Clear();
                    }
                    sameColorLength_node_lines.Add(node_line);
                    if ((indexN == nodes_length - 1) || (!node_line.Fill_colorSpecifications.Length.Equals(nodes[indexN + 1].Fill_colorSpecifications.Length)))
                    {
                        resources.AddRange(Generate_resource_lines_with_pieChart_and_connect_to_nodes_for_same_fill_colors_length(ref sameColorLength_node_lines, out exeption_thrown));
                    }
                }
            }
            return resources.ToArray();
        }
        #endregion

        private void Write_cytoscape_styles_xml_file_into_given_directory(string complete_directoryName)
        {
            Dictionary<Visu_node_field_enum, string> nf_aC_dict = Get_visuNode_attributeCytoscapeName_dict();
            string complete_style_fileName = complete_directoryName + "/Cytoscape_styles.xml";
            StreamWriter writer = new StreamWriter(complete_style_fileName);
            StringBuilder cytoscape_style_sb = new StringBuilder();
            writer.WriteLine("<vizmap id=\"VizMap-2025_05_07-14_59\" documentVersion=\"3.1\">");
            writer.WriteLine("    <visualStyle name=\"MBC PathNet\">");
            writer.WriteLine("        <network>");
            writer.WriteLine("            <visualProperty default=\"0.0\" name=\"NETWORK_CENTER_X_LOCATION\"/>");
            writer.WriteLine("            <visualProperty default=\"false\" name=\"NETWORK_NODE_LABEL_SELECTION\"/>");
            writer.WriteLine("            <visualProperty default=\"true\" name=\"NETWORK_NODE_SELECTION\"/>");
            writer.WriteLine("            <visualProperty default=\"#FFFFFF\" name=\"NETWORK_BACKGROUND_PAINT\"/>");
            writer.WriteLine("            <visualProperty default=\"\" name=\"NETWORK_TITLE\"/>");
            writer.WriteLine("            <visualProperty default=\"false\" name=\"NETWORK_ANNOTATION_SELECTION\"/>");
            writer.WriteLine("            <visualProperty default=\"true\" name=\"NETWORK_EDGE_SELECTION\"/>");
            writer.WriteLine("            <visualProperty default=\"400.0\" name=\"NETWORK_HEIGHT\"/>");
            writer.WriteLine("            <visualProperty default=\"550.0\" name=\"NETWORK_WIDTH\"/>");
            writer.WriteLine("            <visualProperty default=\"1.0\" name=\"NETWORK_SCALE_FACTOR\"/>");
            writer.WriteLine("            <visualProperty default=\"false\" name=\"NETWORK_FORCE_HIGH_DETAIL\"/>");
            writer.WriteLine("            <visualProperty default=\"0.0\" name=\"NETWORK_CENTER_Y_LOCATION\"/>");
            writer.WriteLine("            <visualProperty default=\"0.0\" name=\"NETWORK_CENTER_Z_LOCATION\"/>");
            writer.WriteLine("            <visualProperty default=\"0.0\" name=\"NETWORK_DEPTH\"/>");
            writer.WriteLine("            <visualProperty default=\"550.0\" name=\"NETWORK_SIZE\"/>");
            writer.WriteLine("        </network>");
            writer.WriteLine("        <node>");
            writer.WriteLine("            <dependency value=\"false\" name=\"nodeCustomGraphicsSizeSync\"/>");
            writer.WriteLine("            <dependency value=\"false\" name=\"nodeSizeLocked\"/>");
            writer.WriteLine("            <visualProperty default=\"10.0\" name=\"COMPOUND_NODE_PADDING\"/>");
            writer.WriteLine("            <visualProperty default=\"org.cytoscape.cg.model.NullCustomGraphics,0,[ Remove Graphics ],\" name=\"NODE_CUSTOMGRAPHICS_9\"/>");
            writer.WriteLine("            <visualProperty default=\"true\" name=\"NODE_NESTED_NETWORK_IMAGE_VISIBLE\"/>");
            writer.WriteLine("            <visualProperty default=\"255\" name=\"NODE_LABEL_TRANSPARENCY\"/>");
            writer.WriteLine("            <visualProperty default=\"0.0\" name=\"NODE_DEPTH\"/>");
            writer.WriteLine("            <visualProperty default=\"#C80000\" name=\"NODE_FILL_COLOR\">");
            writer.WriteLine("                <passthroughMapping attributeName=\"{0}\" attributeType=\"string\"/>", nf_aC_dict[Visu_node_field_enum.Fill_color]);
            writer.WriteLine("            </visualProperty>");
            writer.WriteLine("            <visualProperty default=\"50.0\" name=\"NODE_CUSTOMGRAPHICS_SIZE_5\"/>");
            writer.WriteLine("            <visualProperty default=\"#1E90FF\" name=\"NODE_PAINT\"/>");
            writer.WriteLine("            <visualProperty default=\"org.cytoscape.cg.model.NullCustomGraphics,0,[ Remove Graphics ],\" name=\"NODE_CUSTOMGRAPHICS_2\"/>");
            writer.WriteLine("            <visualProperty default=\"org.cytoscape.cg.model.NullCustomGraphics,0,[ Remove Graphics ],\" name=\"NODE_CUSTOMGRAPHICS_4\"/>");
            writer.WriteLine("            <visualProperty default=\"C,C,c,0.00,0.00\" name=\"NODE_CUSTOMGRAPHICS_POSITION_4\"/>");
            writer.WriteLine("            <visualProperty default=\"50.0\" name=\"NODE_CUSTOMGRAPHICS_SIZE_3\"/>");
            writer.WriteLine("            <visualProperty default=\"DefaultVisualizableVisualProperty(id=NODE_CUSTOMPAINT_4, name=Node Custom Paint 4)\" name=\"NODE_CUSTOMPAINT_4\"/>");
            writer.WriteLine("            <visualProperty default=\"50.0\" name=\"NODE_CUSTOMGRAPHICS_SIZE_8\"/>");
            writer.WriteLine("            <visualProperty default=\"C,C,c,0.00,0.00\" name=\"NODE_CUSTOMGRAPHICS_POSITION_5\"/>");
            writer.WriteLine("            <visualProperty default=\"#B6B6B6\" name=\"NODE_LABEL_BACKGROUND_COLOR\"/>");
            writer.WriteLine("            <visualProperty default=\"C,C,c,0.00,0.00\" name=\"NODE_CUSTOMGRAPHICS_POSITION_9\"/>");
            writer.WriteLine("            <visualProperty default=\"\" name=\"NODE_LABEL\">");
            writer.WriteLine("                <passthroughMapping attributeName=\"{0}\" attributeType=\"string\"/>", nf_aC_dict[Visu_node_field_enum.Label]);
            writer.WriteLine("            </visualProperty>");
            writer.WriteLine("            <visualProperty default=\"75.0\" name=\"NODE_WIDTH\">");
            writer.WriteLine("                <passthroughMapping attributeName=\"{0}\" attributeType=\"double\"/>", nf_aC_dict[Visu_node_field_enum.Width]);
            writer.WriteLine("            </visualProperty>");
            writer.WriteLine("            <visualProperty default=\"org.cytoscape.cg.model.NullCustomGraphics,0,[ Remove Graphics ],\" name=\"NODE_CUSTOMGRAPHICS_3\"/>");
            writer.WriteLine("            <visualProperty default=\"DefaultVisualizableVisualProperty(id=NODE_CUSTOMPAINT_3, name=Node Custom Paint 3)\" name=\"NODE_CUSTOMPAINT_3\"/>");
            writer.WriteLine("            <visualProperty default=\"255\" name=\"NODE_TRANSPARENCY\"/>");
            writer.WriteLine("            <visualProperty default=\"50.0\" name=\"NODE_CUSTOMGRAPHICS_SIZE_2\"/>");
            writer.WriteLine("            <visualProperty default=\"DefaultVisualizableVisualProperty(id=NODE_CUSTOMPAINT_9, name=Node Custom Paint 9)\" name=\"NODE_CUSTOMPAINT_9\"/>");
            writer.WriteLine("            <visualProperty name=\"NODE_LABEL_FONT_SIZE\" default=\"12\">");
            writer.WriteLine("                <passthroughMapping attributeName=\"{0}\" attributeType=\"double\"/>", nf_aC_dict[Visu_node_field_enum.FontSize]);
            writer.WriteLine("            </visualProperty>");
            writer.WriteLine("            <visualProperty default=\"0.0\" name=\"NODE_Y_LOCATION\"/>");
            writer.WriteLine("            <visualProperty default=\"50.0\" name=\"NODE_CUSTOMGRAPHICS_SIZE_4\"/>");
            writer.WriteLine("            <visualProperty default=\"0.0\" name=\"NODE_LABEL_ROTATION\"/>");
            writer.WriteLine("            <visualProperty default=\"35.0\" name=\"NODE_HEIGHT\">");
            writer.WriteLine("                <passthroughMapping attributeName=\"{0}\" attributeType=\"double\"/>", nf_aC_dict[Visu_node_field_enum.Height]);
            writer.WriteLine("            </visualProperty>");
            writer.WriteLine("            <visualProperty default=\"#CCCCCC\" name=\"NODE_BORDER_PAINT\">");
            writer.WriteLine("                <passthroughMapping attributeName=\"{0}\" attributeType=\"string\"/>", nf_aC_dict[Visu_node_field_enum.Border_color]);
            writer.WriteLine("            </visualProperty>\r\n            <visualProperty default=\"0.0\" name=\"NODE_BORDER_WIDTH\">");
            writer.WriteLine("                <passthroughMapping attributeName=\"{0}\" attributeType=\"double\"/>", nf_aC_dict[Visu_node_field_enum.Border_width]);
            writer.WriteLine("            </visualProperty>\r\n            <visualProperty name=\"NODE_CUSTOMGRAPHICS_1\">");
            writer.WriteLine("                <passthroughMapping attributeName=\"{0}\" attributeType=\"string\"/>", nf_aC_dict[Visu_node_field_enum.Resource_id]);
            writer.WriteLine("            </visualProperty>");
            writer.WriteLine("            <visualProperty name=\"NODE_CUSTOMGRAPHICS_SIZE_1\">");
            writer.WriteLine("                <passthroughMapping attributeName=\"{0}\" attributeType=\"double\"/>", nf_aC_dict[Visu_node_field_enum.Resource_size]);
            writer.WriteLine("            </visualProperty>");
            writer.WriteLine("            <visualProperty default=\"org.cytoscape.cg.model.NullCustomGraphics,0,[ Remove Graphics ],\" name=\"NODE_CUSTOMGRAPHICS_7\"/>");
            writer.WriteLine("            <visualProperty default=\"C,C,c,0.00,0.00\" name=\"NODE_CUSTOMGRAPHICS_POSITION_8\"/>");
            writer.WriteLine("            <visualProperty default=\"{0}\" name=\"NODE_LABEL_WIDTH\"/>", Options.Cytoscape_label_width);
            writer.WriteLine("            <visualProperty default=\"50.0\" name=\"NODE_CUSTOMGRAPHICS_SIZE_7\"/>");
            writer.WriteLine("            <visualProperty default=\"ROUND_RECTANGLE\" name=\"COMPOUND_NODE_SHAPE\"/>");
            writer.WriteLine("            <visualProperty default=\"NONE\" name=\"NODE_LABEL_BACKGROUND_SHAPE\"/>");
            writer.WriteLine("            <visualProperty default=\"DefaultVisualizableVisualProperty(id=NODE_CUSTOMPAINT_7, name=Node Custom Paint 7)\" name=\"NODE_CUSTOMPAINT_7\"/>");
            writer.WriteLine("            <visualProperty default=\"0.0\" name=\"NODE_Z_LOCATION\"/>");
            writer.WriteLine("            <visualProperty default=\"C,C,c,0.00,0.00\" name=\"NODE_CUSTOMGRAPHICS_POSITION_3\"/>");
            writer.WriteLine("            <visualProperty default=\"DefaultVisualizableVisualProperty(id=NODE_CUSTOMPAINT_1, name=Node Custom Paint 1)\" name=\"NODE_CUSTOMPAINT_1\"/>");
            writer.WriteLine("            <visualProperty default=\"50.0\" name=\"NODE_CUSTOMGRAPHICS_SIZE_9\"/>");
            writer.WriteLine("            <visualProperty default=\"\" name=\"NODE_TOOLTIP\"/>");
            writer.WriteLine("            <visualProperty default=\"DefaultVisualizableVisualProperty(id=NODE_CUSTOMPAINT_5, name=Node Custom Paint 5)\" name=\"NODE_CUSTOMPAINT_5\"/>");
            writer.WriteLine("            <visualProperty default=\"C,C,c,0.00,0.00\" name=\"NODE_CUSTOMGRAPHICS_POSITION_1\"/>");
            writer.WriteLine("            <visualProperty default=\"org.cytoscape.cg.model.NullCustomGraphics,0,[ Remove Graphics ],\" name=\"NODE_CUSTOMGRAPHICS_5\"/>");
            writer.WriteLine("            <visualProperty default=\"DefaultVisualizableVisualProperty(id=NODE_CUSTOMPAINT_6, name=Node Custom Paint 6)\" name=\"NODE_CUSTOMPAINT_6\"/>");
            writer.WriteLine("            <visualProperty default=\"C,C,c,0.00,0.00\" name=\"NODE_CUSTOMGRAPHICS_POSITION_6\"/>");
            writer.WriteLine("            <visualProperty default=\"#FFFF00\" name=\"NODE_SELECTED_PAINT\"/>");
            writer.WriteLine("            <visualProperty default=\"org.cytoscape.cg.model.NullCustomGraphics,0,[ Remove Graphics ],\" name=\"NODE_CUSTOMGRAPHICS_8\"/>");
            writer.WriteLine("            <visualProperty default=\"35.0\" name=\"NODE_SIZE\"/>");
            writer.WriteLine("            <visualProperty default=\"false\" name=\"NODE_SELECTED\"/>");
            writer.WriteLine("            <visualProperty default=\"C,C,c,0.00,0.00\" name=\"NODE_CUSTOMGRAPHICS_POSITION_2\"/>");
            writer.WriteLine("            <visualProperty default=\"ROUND_RECTANGLE\" name=\"NODE_SHAPE\">");
            writer.WriteLine("                <passthroughMapping attributeName=\"{0}\" attributeType=\"string\"/>", nf_aC_dict[Visu_node_field_enum.Shape_type]);
            writer.WriteLine("            </visualProperty>");
            writer.WriteLine("            <visualProperty default=\"DefaultVisualizableVisualProperty(id=NODE_CUSTOMPAINT_2, name=Node Custom Paint 2)\" name=\"NODE_CUSTOMPAINT_2\"/>");
            writer.WriteLine("            <visualProperty default=\"true\" name=\"NODE_VISIBLE\"/>");
            writer.WriteLine("            <visualProperty default=\"S,C,c,0.00,0.00\" name=\"NODE_LABEL_POSITION\">");
            writer.WriteLine("                <passthroughMapping attributeName=\"{0}\" attributeType=\"double\"/>", nf_aC_dict[Visu_node_field_enum.Label_y_offset]);
            writer.WriteLine("            </visualProperty>");
            writer.WriteLine("            <visualProperty default=\"#000000\" name=\"NODE_LABEL_COLOR\"/>");
            writer.WriteLine("            <visualProperty default=\"50.0\" name=\"NODE_CUSTOMGRAPHICS_SIZE_6\"/>");
            writer.WriteLine("            <visualProperty default=\"0.0\" name=\"NODE_X_LOCATION\"/>");
            writer.WriteLine("            <visualProperty default=\"SOLID\" name=\"NODE_BORDER_STROKE\"/>");
            writer.WriteLine("            <visualProperty default=\"255\" name=\"NODE_LABEL_BACKGROUND_TRANSPARENCY\"/>");
            writer.WriteLine("            <visualProperty default=\"C,C,c,0.00,0.00\" name=\"NODE_CUSTOMGRAPHICS_POSITION_7\"/>");
            writer.WriteLine("            <visualProperty default=\"org.cytoscape.cg.model.NullCustomGraphics,0,[ Remove Graphics ],\" name=\"NODE_CUSTOMGRAPHICS_1\"/>");
            writer.WriteLine("            <visualProperty default=\"Arial,plain,12\" name=\"NODE_LABEL_FONT_FACE\"/>");
            writer.WriteLine("            <visualProperty default=\"DefaultVisualizableVisualProperty(id=NODE_CUSTOMPAINT_8, name=Node Custom Paint 8)\" name=\"NODE_CUSTOMPAINT_8\"/>");
            writer.WriteLine("            <visualProperty default=\"org.cytoscape.cg.model.NullCustomGraphics,0,[ Remove Graphics ],\" name=\"NODE_CUSTOMGRAPHICS_6\"/>");
            writer.WriteLine("            <visualProperty default=\"255\" name=\"NODE_BORDER_TRANSPARENCY\"/>");
            writer.WriteLine("        </node>");
            writer.WriteLine("        <edge>");
            writer.WriteLine("            <dependency value=\"false\" name=\"arrowColorMatchesEdge\"/>");
            writer.WriteLine("            <visualProperty default=\"AUTO_BEND\" name=\"EDGE_STACKING\"/>");
            writer.WriteLine("            <visualProperty default=\"C,C,c,0.00,0.00\" name=\"EDGE_LABEL_POSITION\"/>");
            writer.WriteLine("            <visualProperty default=\"6.0\" name=\"EDGE_TARGET_ARROW_SIZE\"/>");
            writer.WriteLine("            <visualProperty default=\"false\" name=\"EDGE_SELECTED\"/>");
            writer.WriteLine("            <visualProperty default=\"0.0\" name=\"EDGE_LABEL_ROTATION\"/>");
            writer.WriteLine("            <visualProperty default=\"\" name=\"EDGE_TOOLTIP\"/>");
            writer.WriteLine("            <visualProperty default=\"#FFFF00\" name=\"EDGE_TARGET_ARROW_SELECTED_PAINT\"/>");
            writer.WriteLine("            <visualProperty default=\"true\" name=\"EDGE_CURVED\"/>");
            writer.WriteLine("            <visualProperty default=\"Dialog.plain,plain,10\" name=\"EDGE_LABEL_FONT_FACE\"/>");
            writer.WriteLine("            <visualProperty default=\"NONE\" name=\"EDGE_TARGET_ARROW_SHAPE\"/>");
            writer.WriteLine("            <visualProperty default=\"0.5\" name=\"EDGE_STACKING_DENSITY\"/>");
            writer.WriteLine("            <visualProperty default=\"#FF0000\" name=\"EDGE_SELECTED_PAINT\"/>");
            writer.WriteLine("            <visualProperty default=\"255\" name=\"EDGE_LABEL_TRANSPARENCY\"/>");
            writer.WriteLine("            <visualProperty default=\"#000000\" name=\"EDGE_LABEL_COLOR\"/>");
            writer.WriteLine("            <visualProperty default=\"NONE\" name=\"EDGE_LABEL_BACKGROUND_SHAPE\"/>");
            writer.WriteLine("            <visualProperty default=\"#000000\" name=\"EDGE_SOURCE_ARROW_UNSELECTED_PAINT\"/>");
            writer.WriteLine("            <visualProperty default=\"NONE\" name=\"EDGE_SOURCE_ARROW_SHAPE\"/>");
            writer.WriteLine("            <visualProperty default=\"SOLID\" name=\"EDGE_LINE_TYPE\"/>");
            writer.WriteLine("            <visualProperty default=\"255\" name=\"EDGE_LABEL_BACKGROUND_TRANSPARENCY\"/>");
            writer.WriteLine("            <visualProperty default=\"#000000\" name=\"EDGE_TARGET_ARROW_UNSELECTED_PAINT\"/>");
            writer.WriteLine("            <visualProperty default=\"2.0\" name=\"EDGE_WIDTH\"/>");
            writer.WriteLine("            <visualProperty default=\"6.0\" name=\"EDGE_SOURCE_ARROW_SIZE\"/>");
            writer.WriteLine("            <visualProperty default=\"0.0\" name=\"EDGE_Z_ORDER\"/>");
            writer.WriteLine("            <visualProperty default=\"#848484\" name=\"EDGE_STROKE_UNSELECTED_PAINT\"/>");
            writer.WriteLine("            <visualProperty default=\"10\" name=\"EDGE_LABEL_FONT_SIZE\"/>");
            writer.WriteLine("            <visualProperty default=\"\" name=\"EDGE_BEND\"/>");
            writer.WriteLine("            <visualProperty default=\"#404040\" name=\"EDGE_UNSELECTED_PAINT\"/>");
            writer.WriteLine("            <visualProperty default=\"200.0\" name=\"EDGE_LABEL_WIDTH\"/>");
            writer.WriteLine("            <visualProperty default=\"true\" name=\"EDGE_VISIBLE\"/>");
            writer.WriteLine("            <visualProperty default=\"#FFFF00\" name=\"EDGE_SOURCE_ARROW_SELECTED_PAINT\"/>");
            writer.WriteLine("            <visualProperty default=\"false\" name=\"EDGE_LABEL_AUTOROTATE\"/>");
            writer.WriteLine("            <visualProperty default=\"#B6B6B6\" name=\"EDGE_LABEL_BACKGROUND_COLOR\"/>");
            writer.WriteLine("            <visualProperty default=\"#323232\" name=\"EDGE_PAINT\"/>");
            writer.WriteLine("            <visualProperty default=\"255\" name=\"EDGE_TRANSPARENCY\"/>");
            writer.WriteLine("            <visualProperty default=\"\" name=\"EDGE_LABEL\"/>");
            writer.WriteLine("            <visualProperty default=\"#FF0000\" name=\"EDGE_STROKE_SELECTED_PAINT\"/>");
            writer.WriteLine("        </edge>");
            writer.WriteLine("    </visualStyle>");
            writer.WriteLine("</vizmap>");
            writer.Close();
        }


        public bool Write_yED_file_and_return_if_error(NetworkNode_line_class[] input_nodes, Dictionary<string, Visualization_nw_edge_characterisation_line_class[]> nw, string complete_file_name_without_extension, yed_node_color_line_class[] node_colors, ProgressReport_interface_class progressReport)
        {
            bool exception_thrown;
            string extension = ".error";
            switch (Options.Graph_editor)
            {
                case Graph_editor_enum.Cytoscape:
                    extension = ".xgmml";
                    break;
                case Graph_editor_enum.yED:
                    extension = ".graphml";
                    break;
                default:
                    throw new Exception();
            }

            string complete_file_name = complete_file_name_without_extension + extension;
            string graph_label = System.IO.Path.GetFileName(complete_file_name_without_extension);
            ReadWriteClass.Create_directory_if_it_does_not_exist(complete_file_name);
            Visualization_of_nw_node_line[] nodes = Generate_nodes(input_nodes);
            Visualization_of_nw_edge_line[] edges = Generate_edges(nw);
            //Remove_illegal_characters(ref nodes, ref edges, ref node_colors);
            if (node_colors.Length == 0) { }
            else
            {
                nodes = Adjust_node_colors_and_sizes(nodes, node_colors);
            }
            nodes = Add_labelSizes_to_nodeSizes(nodes);
            Visualization_of_nw_resource_line_class[] resources = Generate_resource_lines_and_connect_to_nodes(ref nodes, out exception_thrown);

            //Check_if_all_nodes_are_conncected_to_at_least_one_edge(nodes, edges);
            if (exception_thrown)
            {
                string exception_text = "Network generation interrupted, probably due to insufficient memory. Please restart the application and try again or switch of in menu 'SCP-networks'.";
                progressReport.Write_temporary_warning_and_restore_progressReport(exception_text, 5);
            }
            else if (complete_file_name.Length > 260) { }
            else
            {
                Writer = ReadWriteClass.Get_new_stream_writer_and_return_if_successful_or_write_warning_to_progress_report(complete_file_name, progressReport, out bool file_opened);
                if (file_opened)
                {
                    Write_file_head(graph_label);
                    Write_all_nodes(nodes, Options.Group_same_level_processes, resources);
                    Write_all_edges(edges);
                    Write_file_bottom_for_nodes_and_edges_if_necessary();
                    if (resources.Length > 0) { Write_all_resources_if_necessary(resources); }
                    Write_final_file_bottom();
                    Writer.Close();

                    switch (Options.Graph_editor)
                    {
                        case Graph_editor_enum.Cytoscape:
                            string complete_directory = System.IO.Path.GetDirectoryName(complete_file_name);
                            Write_cytoscape_styles_xml_file_into_given_directory(complete_directory);
                            break;
                        case Graph_editor_enum.yED:
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
            return exception_thrown;
        }

        private string Get_spaces_string()
        {
            StringBuilder sb_spaces = new StringBuilder();
            for (int i = 0; i < Shift_text_right; i++) { sb_spaces.Append(" "); }
            return sb_spaces.ToString();
        }
    }
}
