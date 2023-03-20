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

namespace yed_network
{
    enum Shape_enum { E_mp_t_y, Ellipse, Rectangle, Diamond, Octagon, Triangle_pointing_up, Triangle_pointing_down, Star_five_pointed }
    enum EdgeArrow_type_enum { E_m_p_t_y, Arrow, Thick_dotted_line, Dotted_line, Dashed_line }
    enum Yed_network_node_size_determinant_enum { E_m_p_t_y, No_of_sets, Standard, No_of_different_colors }

    //////////////////////////////////////////////////////////

    class yed_node_color_line_class
    {
        public string NodeName { get; set; }
        public Color_specification_line_class[] Color_specifications { get; set; }

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
        public int Geometry_heigth { get; set; }
        public int Geometry_width { get; set; }
        public Color_specification_line_class[] Fill_colorSpecifications { get; set; }
        public int FontSize { get; set; }
        public string FontStyle { get; set; }
        public string LabelHasLineColor { get; set; }
        public string TextColor { get; set; }
        public string Model_name { get; set; }
        public string Model_position { get; set; }
        public string Shape_type { get; set; }
        public string Transparent { get; set; }
        public string Border_style_color { get; set; }
        public bool Border_style_has_color { get; set; }
        public float Border_style_width { get; set; }
        public string Label_alignement { get; set; }
        public string Box_label { get; set; }
        public string Resource_id { get; set; }
        #endregion

        public Visualization_of_nw_node_line()
        {
            Geometry_heigth = 30;
            Geometry_width = 30;
            FontSize = 26;
            FontStyle = "plain";
            Model_name = "sides";
            Model_position = "s";
            Transparent = "false";
            LabelHasLineColor = "false";
            TextColor = "#000000";
            Border_style_color = "#000000";
            Border_style_width = 0.5F;
            Label_alignement = "side";
            Box_label = "";
            Shape_type = "rectangle";
            Resource_id = "";
            Border_style_has_color = true;
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
            copy.Border_style_color = (string)this.Border_style_color.Clone();
            copy.Label_alignement = (string)this.Label_alignement.Clone();
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
        public string Arrow_source_end { get; set; }
        public string Arrow_target_end { get; set; }
        public string Arrow_color { get; set; }
        public string Arrow_type { get; set; }
        public string Arrow_width { get; set; }
        public string Arrow_label { get; set; }
        public string Arrow_label_font_size { get; set; }
        #endregion

        public Visualization_of_nw_edge_line()
        {
            Arrow_source_end = "";
            Arrow_target_end = "standard";
            Arrow_color = "#000000";
            Arrow_type = "line";
            Arrow_width = "1.0";
            Arrow_label = "";
            Arrow_label_font_size = "15";
        }

        public Visualization_of_nw_edge_line Deep_copy()
        {
            Visualization_of_nw_edge_line copy = (Visualization_of_nw_edge_line)this.MemberwiseClone();
            copy.Target_id = (string)this.Target_id.Clone();
            copy.Source_id = (string)this.Source_id.Clone();
            copy.Edge_id = (string)this.Edge_id.Clone();
            copy.Arrow_source_end = (string)this.Arrow_source_end.Clone();
            copy.Arrow_color = (string)this.Arrow_color.Clone();
            copy.Arrow_target_end = (string)this.Arrow_target_end.Clone();
            copy.Arrow_type = (string)this.Arrow_type.Clone();
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
        public Dictionary<string, Shape_enum> NodeLabel_nodeShape_dict { get; set; }
        public bool Group_same_level_processes { get; set; }
        public int Max_signs_per_line_in_nodeName { get; set; }
        public Yed_network_node_size_determinant_enum Node_size_determinant { get; set; }

        public yED_options_class()
        {
            NodeLabel_nodeShape_dict = new Dictionary<string, Shape_enum>();
            Edge_color = Color.Black;
            Node_fill_color = Color.CornflowerBlue;
            Node_shape = Shape_enum.Rectangle;
            Group_same_level_processes = false;
            Max_signs_per_line_in_nodeName = 20;
            Node_size_determinant = Yed_network_node_size_determinant_enum.Standard;
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

        private bool Check_if_all_nodes_are_conncected_to_at_least_one_edge(Visualization_of_nw_node_line[] nodes, Visualization_of_nw_edge_line[] edges)
        {
            bool ok = true;

            #region Get ordered nodeIds in edges
            int edges_length = edges.Length;
            edges = edges.OrderBy(l => l.Edge_id).ToArray();
            List<string> allEdge_node_ids_list = new List<string>();
            Visualization_of_nw_edge_line edge_line;
            for (int indexE = 0; indexE < edges_length; indexE++)
            {
                edge_line = edges[indexE];
                allEdge_node_ids_list.Add(edge_line.Source_id);
                allEdge_node_ids_list.Add(edge_line.Target_id);
            }
            string[] allEdge_node_ids = allEdge_node_ids_list.Distinct().OrderBy(l => l).ToArray();
            #endregion

            #region Get all nodeIds in nodes
            int nodes_length = nodes.Length;
            string[] allNode_node_ids = new string[nodes_length];
            for (int indexN = 0; indexN < nodes_length; indexN++)
            {
                allNode_node_ids[indexN] = nodes[indexN].Id;
            }
            allNode_node_ids = allNode_node_ids.Distinct().OrderBy(l => l).ToArray();
            #endregion

            #region Get nodeIds that are not in edges or nodes
            int indexEdgeNodeIds = 0;
            int indexNodeNodeIds = 0;
            int node_nodeIds_length = allNode_node_ids.Length;
            int edge_nodeIds_length = allEdge_node_ids.Length;
            int stringCompare = -2;
            string node_nodeId;
            string edge_nodeId;
            List<string> nodeNodeIds_not_in_edges = new List<string>();
            List<string> edgeNodeIds_not_in_nodes = new List<string>();

            while ((indexEdgeNodeIds < node_nodeIds_length) || (indexNodeNodeIds < edge_nodeIds_length))
            {
                edge_nodeId = "error_edge";
                node_nodeId = "error_node";
                if ((indexEdgeNodeIds < node_nodeIds_length) || (indexNodeNodeIds < edge_nodeIds_length))
                {
                    node_nodeId = allNode_node_ids[indexNodeNodeIds];
                    edge_nodeId = allEdge_node_ids[indexEdgeNodeIds];
                    stringCompare = node_nodeId.CompareTo(edge_nodeId);
                }
                else if (indexEdgeNodeIds < node_nodeIds_length)
                {
                    edge_nodeId = allEdge_node_ids_list[indexEdgeNodeIds];
                    stringCompare = 2;
                }
                else
                {
                    node_nodeId = allNode_node_ids[indexNodeNodeIds];
                    stringCompare = -2;
                }

                if (stringCompare == 0)
                {
                    indexEdgeNodeIds++;
                    indexNodeNodeIds++;
                }
                else if (stringCompare > 0)
                {
                    indexEdgeNodeIds++;
                    edgeNodeIds_not_in_nodes.Add(edge_nodeId);
                }
                else
                {
                    indexNodeNodeIds++;
                    nodeNodeIds_not_in_edges.Add(node_nodeId);
                }
            }
            #endregion

            #region Report error, if error
            if (nodeNodeIds_not_in_edges.Count > 0)
            {
                string error_text = "The following nodes are not connected to any edge: " + string.Join(", ", nodeNodeIds_not_in_edges);
                throw new Exception(error_text);
            }
            if (edgeNodeIds_not_in_nodes.Count > 0)
            {
                string error_text = "The following edgeNodes are not listed as nodes: " + string.Join(", ", edgeNodeIds_not_in_nodes);
                throw new Exception(error_text);
            }
            #endregion
            return ok;
        }

        #region Write head and botton
        private void Write_file_head()
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
                + "  <graph id=\"G\" edgedefault=\"undirected\">";
            Writer.WriteLine(file_head);
            Shift_text_right += 4;
        }

        private void Write_file_bottom_for_nodes_and_edges()
        {
            Shift_text_right -= 2;
            string file_bottom =
                  "</graph>\n";
            Writer.Write(file_bottom);
        }

        private void Write_final_file_bottom()
        {
            string file_bottom =
                 "</graphml>";
            Writer.Write(file_bottom);
        }
        #endregion

        #region Write Nodes
        private void Write_individual_node(Visualization_of_nw_node_line node)
        {
            StringBuilder sb_spaces = new StringBuilder();
            string spaces = Get_spaces_string();
            StringBuilder node_label = new StringBuilder();
            string[] nodeLabel_words = node.Label.Split(' ');
            string current_nodeLabel_word;
            int indexN;
            int nodeLabel_words_length = nodeLabel_words.Length;
            node_label.AppendFormat("{0}", nodeLabel_words[0]);
            if (nodeLabel_words_length > 1)
            {
                int length_of_current_line = node_label.Length;
                for (int indexNW = 1; indexNW < nodeLabel_words_length; indexNW++)
                {
                    current_nodeLabel_word = nodeLabel_words[indexNW];
                    if (current_nodeLabel_word.IndexOf('@') != -1)
                    {
                        while ((indexN = current_nodeLabel_word.IndexOf('@')) != -1)
                        {
                            node_label.AppendFormat("\n{0}", (string)current_nodeLabel_word.Substring(0, indexN));
                            current_nodeLabel_word = current_nodeLabel_word.Substring(indexN + 1, current_nodeLabel_word.Length - indexN - 1);
                        }
                        length_of_current_line = current_nodeLabel_word.Length;
                    }
                    if (length_of_current_line + 1 + current_nodeLabel_word.Length <= this.Options.Max_signs_per_line_in_nodeName)
                    {
                        node_label.AppendFormat(" {0}", (string)current_nodeLabel_word.Clone());
                        length_of_current_line += 1 + current_nodeLabel_word.Length;
                    }
                    else
                    {
                        node_label.AppendFormat("\n{0}", (string)current_nodeLabel_word.Clone());
                        length_of_current_line = current_nodeLabel_word.Length;
                    }
                }
            }

            string text_node = "error";
            if (node.Fill_colorSpecifications.Length == 0) { throw new Exception(); }
            else if (node.Fill_colorSpecifications.Length == 1)
            {
                string hexadecimal_color = Hexadecimal_color_class.Get_hexadecimal_code_for_color(node.Fill_colorSpecifications[0].Fill_color);
                if (node.Border_style_has_color)
                {
                    text_node =
                      spaces + "<node id=\"" + node.Id + "\">\n"
                    + spaces + "  <data key=\"" + Key.Node_url + "\"/>\n"
                    + spaces + "  <data key=\"" + Key.Node_graphics + "\">\n"
                    + spaces + "    <y:ShapeNode>\n"
                    + spaces + "      <y:Geometry height=\"" + node.Geometry_heigth + "\" width=\"" + node.Geometry_width + "\" x=\"94.3427734375\" y=\"208.0\"/>\n"
                    + spaces + "      <y:Fill color=\"" + hexadecimal_color + "\" transparent=\"" + node.Transparent + "\"/>\n"
                    + spaces + "      <y:BorderStyle color=\"" + node.Border_style_color + "\" type=\"line\" width=\"" + node.Border_style_width + "\"/>\n"
                    + spaces + "      <y:NodeLabel alignment=\"" + node.Label_alignement + "\" autoSizePolicy=\"content\" fontFamily=\"Dialog\" fontSize=\"" + node.FontSize + "\" fontStyle=\"" + node.FontStyle + "\" hasBackgroundColor=\"false\" hasLineColor=\"" + node.LabelHasLineColor + "\" height=\"18.701171875\" modelName=\"" + node.Model_name + "\" modelPosition=\"" + node.Model_position + "\" textColor=\"" + node.TextColor + "\" visible=\"true\" width=\"64.703125\" x=\"-17.3515625\" y=\"5.6494140625\">" + node_label.ToString() + "</y:NodeLabel>\n"
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
                    + spaces + "      <y:Geometry height=\"" + node.Geometry_heigth + "\" width=\"" + node.Geometry_width + "\" x=\"94.3427734375\" y=\"208.0\"/>\n"
                    + spaces + "      <y:Fill color=\"" + hexadecimal_color + "\" transparent=\"" + node.Transparent + "\"/>\n"
                    + spaces + "      <y:BorderStyle hasColor=\"false\" raised=\"false\" type=\"line\" width=\"0.0\"/>\n"
                    + spaces + "      <y:NodeLabel alignment=\"" + node.Label_alignement + "\" autoSizePolicy=\"content\" fontFamily=\"Dialog\" fontSize=\"" + node.FontSize + "\" fontStyle=\"" + node.FontStyle + "\" hasBackgroundColor=\"false\" hasLineColor=\"" + node.LabelHasLineColor + "\" height=\"18.701171875\" modelName=\"" + node.Model_name + "\" modelPosition=\"" + node.Model_position + "\" textColor=\"" + node.TextColor + "\" visible=\"true\" width=\"64.703125\" x=\"-17.3515625\" y=\"5.6494140625\">" + node_label.ToString() + "</y:NodeLabel>\n"
                    + spaces + "      <y:Shape type=\"" + node.Shape_type + "\"/>\n"
                    + spaces + "    </y:ShapeNode>\n"
                    + spaces + "  </data>\n"
                    + spaces + "</node>";
                }
            }
            else
            {
                if (String.IsNullOrEmpty(node.Resource_id)) { throw new Exception(); }
                text_node =
                  spaces + "<node id=\"" + node.Id + "\">\n"
                + spaces + "  <data key=\"" + Key.Node_url + "\"/>\n"
                + spaces + "  <data key=\"" + Key.Node_graphics + "\">\n"
                + spaces + "    <y:ImageNode>\n"
                + spaces + "      <y:Geometry height=\"" + node.Geometry_heigth * 1.2 + "\" width=\"" + node.Geometry_width * 1.2 + "\" x=\"812.0\" y=\"187.0\"/>\n"
                + spaces + "      <y:Fill color=\"#CCCCFF\" transparent=\"" + node.Transparent + "\"/>\n"
                + spaces + "      <y:BorderStyle color=\"" + node.Border_style_color + "\" type=\"line\" width=\"" + node.Border_style_width + "\" />\n"
                + spaces + "      <y:NodeLabel alignment=\"" + node.Label_alignement + "\" autoSizePolicy=\"content\" fontFamily=\"Dialog\" fontSize=\"" + node.FontSize + "\" fontStyle=\"" + node.FontStyle + "\" hasLineColor=\"" + node.LabelHasLineColor + "\" height=\"18.701171875\" horizontalTextPosition=\"center\" iconTextGap=\"4\" modelName=\"" + node.Model_name + "\" modelPosition=\"" + node.Model_position + "\" textColor=\"" + node.TextColor + "\" verticalTextPosition=\"bottom\" visible=\"true\" width=\"41.34765625\" x=\"129.326171875\" xml:space=\"preserve\" y=\"304.0\">" + node_label.ToString() + "</y:NodeLabel>\n"
                + spaces + "      <y:Image alphaImage=\"true\" refid=\"" + node.Resource_id + "\"/>\n"
                + spaces + "    </y:ImageNode>\n"
                + spaces + "  </data>\n"
                + spaces + "</node>";
            }
            Writer.WriteLine(text_node);
        }

        private void Write_all_nodes(Visualization_of_nw_node_line[] nodes, bool group_same_level_nodes)
        {
            int nodes_length = nodes.Length;
            Visualization_of_nw_node_line node_line;
            if (group_same_level_nodes)
            {
                nodes = nodes.OrderBy(l => l.Box_label).ThenBy(l => l.Id).ToArray();
                for (int indexN = 0; indexN < nodes_length; indexN++)
                {
                    node_line = nodes[indexN];
                    if (node_line.Box_label.Equals("-1")) { throw new Exception(); }
                    if ((indexN == 0) || (!node_line.Box_label.Equals(nodes[indexN - 1].Box_label)))
                    {
                        Write_box_group_start_and_boxes(node_line.Box_label);
                    }
                    Write_individual_node(node_line);
                    if ((indexN == nodes_length - 1) || (!node_line.Box_label.Equals(nodes[indexN + 1].Box_label)))
                    {
                        Write_box_group_end();
                    }
                }
            }
            else
            {
                nodes = nodes.OrderBy(l => l.Box_label).ThenBy(l => l.Id).ToArray();
                for (int indexN = 0; indexN < nodes_length; indexN++)
                {
                    node_line = nodes[indexN];
                    if ((indexN == 0) || (!node_line.Box_label.Equals(nodes[indexN - 1].Box_label)))
                    {
                        if (node_line.Box_label.Equals(Global_class.Network_legend_box_label))
                        {
                            Write_box_group_start_and_boxes(node_line.Box_label);
                        }
                    }
                    Write_individual_node(node_line);
                    if ((indexN == nodes_length - 1) || (!node_line.Box_label.Equals(nodes[indexN + 1].Box_label)))
                    {
                        if (node_line.Box_label.Equals(Global_class.Network_legend_box_label))
                        {
                            Write_box_group_end();
                        }
                    }
                }
            }
        }
        #endregion

        #region Write Resources
        private void Write_individual_resource(Visualization_of_nw_resource_line_class resource)
        {
            StringBuilder sb_spaces = new StringBuilder();
            string spaces = Get_spaces_string();

            string text_resource = "error";
            //text_resource = spaces + "<data key=\"" + Key.Resource_graphml + "\">\n"
            text_resource = spaces + "  <y:Resource id=\"" + resource.Resource_id + "\" type=\"java.awt.image.BufferedImage\" xml:space=\"preserve\">" + resource.Base64String
                                     + "</y:Resource>\n";
            Writer.WriteLine(text_resource);
        }

        private void Write_all_resources(Visualization_of_nw_resource_line_class[] resources)
        {
            int resources_length = resources.Length;
            Visualization_of_nw_resource_line_class resource_line;
            resources = resources.OrderBy(l => l.Resource_id).ToArray();
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
        }
        #endregion

        #region Write Edges
        private void Write_individual_edge(Visualization_of_nw_edge_line edge)
        {
            string spaces = Get_spaces_string();
            string text_edge =
                   spaces + "<edge id=\"" + edge.Edge_id + "\" source=\"" + edge.Source_id + "\" target=\"" + edge.Target_id + "\">\n"
                 + spaces + "  <data key=\"" + Key.Edge_description + "\"/>\n"
                 + spaces + "  <data key=\"" + Key.Edge_graphics + "\">\n"
                 + spaces + "    <y:PolyLineEdge>\n"
                 + spaces + "      <y:Path sx=\"0.0\" sy=\"0.0\" tx=\"0.0\" ty=\"0.0\"/>\n"
                 + spaces + "      <y:LineStyle color=\"" + edge.Arrow_color + "\" type=\"" + edge.Arrow_type + "\" width=\"" + edge.Arrow_width + "\"/>\n"
                 + spaces + "      <y:Arrows source=\"" + edge.Arrow_source_end + "\" target=\"" + edge.Arrow_target_end + "\"/>\n"
                 + spaces + "      <y:EdgeLabel alignment=\"center\" configuration=\"AutoFlippingLabel\" distance=\"2.0\" fontFamily=\"Dialog\" fontSize=\"" + edge.Arrow_label_font_size + "\" fontStyle=\"plain\" hasBackgroundColor=\"false\" hasLineColor=\"false\" height=\"28.501953125\" modelName=\"two_pos\" modelPosition=\"head\" preferredPlacement=\"anywhere\" ratio=\"0.5\" textColor=\"#000000\" visible=\"true\" width=\"25.123046875\" x=\"-72.580322265625\" y=\"-84.35603932425425\">" + edge.Arrow_label + "<y:PreferredPlacementDescriptor angle=\"0.0\" angleOffsetOnRightSide=\"0\" angleReference=\"absolute\" angleRotationOnRightSide=\"co\" distance=\"-1.0\" frozen=\"true\" placement=\"anywhere\" side=\"anywhere\" sideReference=\"relative_to_edge_flow\"/>"
                 + spaces + "      </y:EdgeLabel>"
                 + spaces + "      <y:BendStyle smoothed=\"false\"/>\n"
                 + spaces + "    </y:PolyLineEdge>\n"
                 + spaces + "  </data>\n"
                 + spaces + "</edge>";
            Writer.WriteLine(text_edge);
        }

        private void Write_all_edges(Visualization_of_nw_edge_line[] edges)
        {
            int edges_length = edges.Length;
            edges = edges.OrderBy(l => l.Edge_id).ToArray();
            Visualization_of_nw_edge_line edge_line;
            for (int indexE = 0; indexE < edges_length; indexE++)
            {
                edge_line = edges[indexE];
                Write_individual_edge(edge_line);
            }
        }
        #endregion

        #region Write level boxes
        private void Write_box_group_start_and_boxes(string box_name)
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
              + spaces + "        <y:NodeLabel alignment=\"right\" autoSizePolicy=\"node_width\" backgroundColor=\"#EBEBEB\" borderDistance=\"0.0\" fontFamily=\"Dialog\" fontSize=\"15\" fontStyle=\"plain\" hasLineColor=\"false\" height=\"22.37646484375\" modelName=\"internal\" modelPosition=\"t\" textColor=\"#000000\" visible=\"true\" width=\"353.5269841269842\" x=\"0.0\" y=\"0.0\">" + name_open_box + "</y:NodeLabel>\n"
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

        private void Write_box_group_end()
        {
            Shift_text_right -= 2;
            string space = Get_spaces_string();
            string text_group_end =
                  space + "  </graph>\n"
                + space + "</node>\n";
            Writer.WriteLine(text_group_end);
        }
        #endregion

        #region Generate edges
        private Visualization_of_nw_edge_line Generate_individual_edge(string source, Visualization_nw_edge_characterisation_line_class target)
        {
            Visualization_of_nw_edge_line new_edge_line = new Visualization_of_nw_edge_line();
            new_edge_line.Arrow_color = Hexadecimal_color_class.Get_hexadecimal_code_for_color(Options.Edge_color);
            new_edge_line.Edge_id = source + "_to_" + target.Target;
            new_edge_line.Source_id = source;
            new_edge_line.Target_id = target.Target;
            new_edge_line.Arrow_width = target.Edge_width.ToString();
            new_edge_line.Arrow_label = (string)target.Edge_label.Clone();
            switch (target.EdgeArrow_type)
            {
                case EdgeArrow_type_enum.Arrow:
                    new_edge_line.Arrow_type = "line";
                    new_edge_line.Arrow_source_end = "none";
                    new_edge_line.Arrow_target_end = "standard";
                    break;
                case EdgeArrow_type_enum.Dotted_line:
                    new_edge_line.Arrow_type = "dotted";
                    new_edge_line.Arrow_source_end = "none";
                    new_edge_line.Arrow_target_end = "none";
                    break;
                case EdgeArrow_type_enum.Thick_dotted_line:
                    new_edge_line.Arrow_type = "dotted";
                    new_edge_line.Arrow_source_end = "none";
                    new_edge_line.Arrow_target_end = "none";
                    new_edge_line.Arrow_width = "2.0";
                    break;
                case EdgeArrow_type_enum.Dashed_line:
                    new_edge_line.Arrow_type = "dashed";
                    new_edge_line.Arrow_source_end = "none";
                    new_edge_line.Arrow_target_end = "none";
                    break;
                default:
                    throw new Exception();
            }
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

        #region Generate nodes
        private string Get_shape_code(Visualization_of_nw_node_line node_line)
        {
            string shape_code;
            Shape_enum node_shape = Options.Node_shape;
            if ((!String.IsNullOrEmpty(node_line.Label)) && (Options.NodeLabel_nodeShape_dict.ContainsKey(node_line.Label)))
            {
                node_shape = Options.NodeLabel_nodeShape_dict[node_line.Label];
            }
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

        private Visualization_of_nw_node_line Generate_individual_node(string nodeName)
        {
            Visualization_of_nw_node_line node_line = new Visualization_of_nw_node_line();
            Color_specification_line_class color_specification_line = new Color_specification_line_class();
            color_specification_line.Fill_color = Options.Node_fill_color;
            node_line.Fill_colorSpecifications = new Color_specification_line_class[] { color_specification_line };
            node_line.Shape_type = Get_shape_code(node_line);
            node_line.Id = nodeName;
            node_line.Label = nodeName;
            return node_line;
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
                node.Shape_type = Get_shape_code(node);
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

        private Visualization_of_nw_node_line[] Adjust_node_colors_and_sizes(Visualization_of_nw_node_line[] nodes, params yed_node_color_line_class[] node_colors)
        {
            nodes = nodes.OrderBy(l => l.Id).ToArray();
            Visualization_of_nw_node_line node_line;
            int nodes_length = nodes.Length;
            node_colors = node_colors.OrderBy(l => l.NodeName).ToArray();
            int node_colors_length = node_colors.Length;
            int indexNode = 0;
            int stringCompare;
            yed_node_color_line_class node_color;
            int color_specifications_length;
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
                                node_line.Geometry_width = (int)Math.Round((double)node_line.Geometry_width * Math.Sqrt(node_line.Fill_colorSpecifications.Length));
                                node_line.Geometry_heigth = (int)Math.Round((double)node_line.Geometry_heigth * Math.Sqrt(node_line.Fill_colorSpecifications.Length));
                                break;
                            case Yed_network_node_size_determinant_enum.No_of_different_colors:
                                List<Color> colors = new List<Color>();
                                foreach (Color_specification_line_class fill_color_specification_line in node_line.Fill_colorSpecifications)
                                {
                                    colors.Add(fill_color_specification_line.Fill_color);
                                }
                                int distinct_colors_length = colors.Distinct().ToArray().Length;
                                node_line.Geometry_width = (int)Math.Round((double)node_line.Geometry_width * distinct_colors_length);
                                node_line.Geometry_heigth = (int)Math.Round((double)node_line.Geometry_heigth * distinct_colors_length);
                                break;
                            case Yed_network_node_size_determinant_enum.Standard:
                                if (node_line.Fill_colorSpecifications.Length==1)
                                {
                                    node_line.Geometry_width = (int)Math.Round((double)node_line.Geometry_width * 1.5);
                                    node_line.Geometry_heigth = (int)Math.Round((double)node_line.Geometry_heigth * 1.5);
                                }
                                else
                                {
                                    node_line.Geometry_width = (int)Math.Round((double)node_line.Geometry_width * 2);
                                    node_line.Geometry_heigth = (int)Math.Round((double)node_line.Geometry_heigth * 2);
                                }
                                break;
                            default:
                                throw new Exception();
                        }
                        if (node_line.Box_label.Equals(Global_class.Network_genes_box_label))
                        {
                            node_line.Geometry_width = (int)Math.Round(0.7F * (float)node_line.Geometry_width);
                            node_line.Geometry_heigth = (int)Math.Round(0.7F * (float)node_line.Geometry_heigth);
                        }

                    }
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
            for (int indexColor = colors_length - 1; indexColor < 0; indexColor--)
            {
                sameFillColorsLength_nodes = sameFillColorsLength_nodes.OrderBy(l => l.Fill_colorSpecifications[indexColor]).ToList();
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
                    Color_specification_line_class[] current_colorSpecifications = visualization_node_line.Fill_colorSpecifications.OrderBy(l => l.Fill_color.GetBrightness()).ToArray();
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
                    pieChart.Chart.Fill.Type = FillType.None;
                    pieChart.Chart.Fill.Color = Color.Transparent;
                    pieChart.Chart.Border.Color = Color.Transparent;
                    pieChart.Margin.All=0;
                    pieChart.Fill.Color = Color.Transparent;
                    pieChart.Fill.IsVisible = false;
                    pieChart.Fill.Type = FillType.None;
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
            nodes = nodes.OrderBy(l => l.Fill_colorSpecifications.Length).ToArray();
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

        #region Generate Legend
        private Visualization_of_nw_node_line[] Generate_legend(yed_node_color_line_class[] legend_node_colors)
        {
            int legend_length = legend_node_colors.Length;
            yed_node_color_line_class legend_node_color;
            Visualization_of_nw_node_line legend_visualization_line;
            List<Visualization_of_nw_node_line> legend_visualization_list = new List<Visualization_of_nw_node_line>();
            int colorSpecifications_length;
            for (int indexL = 0; indexL < legend_length; indexL++)
            {
                legend_node_color = legend_node_colors[indexL];
                legend_visualization_line = new Visualization_of_nw_node_line();
                colorSpecifications_length = legend_node_color.Color_specifications.Length;
                legend_visualization_line.Fill_colorSpecifications = new Color_specification_line_class[colorSpecifications_length];
                for (int indexCS=0;indexCS<colorSpecifications_length;indexCS++)
                {
                    legend_visualization_line.Fill_colorSpecifications[indexCS] = legend_node_color.Color_specifications[indexCS].Deep_copy();
                }
                legend_visualization_line.Shape_type = Get_shape_code(legend_visualization_line);
                legend_visualization_line.Label_alignement = "center";
                legend_visualization_line.Model_name = "sides";
                legend_visualization_line.Model_position = "e";
                legend_visualization_line.Id = (string)legend_node_color.NodeName.Clone();
                legend_visualization_line.Label = (string)legend_node_color.NodeName.Clone();
                legend_visualization_list.Add(legend_visualization_line);
            }
            return legend_visualization_list.ToArray();
        }
        #endregion

        #region Write legend
        private void Write_legend(Visualization_of_nw_node_line[] visu_Nodes_legend)
        {
            int all_nodes_length = visu_Nodes_legend.Length;
            Visualization_of_nw_node_line visu_node_legend_line;
            Write_box_group_start_and_boxes("Legend (-log10(p))");
            for (int indexN = 0; indexN < all_nodes_length; indexN++)
            {
                visu_node_legend_line = visu_Nodes_legend[indexN];
                Write_individual_node(visu_node_legend_line);
            }
            Write_box_group_end();
        }
        #endregion

        public bool Write_yED_file_and_return_if_error(NetworkNode_line_class[] input_nodes, Dictionary<string, Visualization_nw_edge_characterisation_line_class[]> nw, string complete_file_name_without_extension, yed_node_color_line_class[] node_colors, System.Windows.Forms.Label error_report_label, Common_functions.Form_tools.Form1_default_settings_class form_default_settings, params yed_node_color_line_class[] legend_node_colors)
        {
            bool exception_thrown;
            string complete_file_name = complete_file_name_without_extension + ".graphml";
            ReadWriteClass.Create_directory_if_it_does_not_exist(complete_file_name);
            Visualization_of_nw_node_line[] nodes = Generate_nodes(input_nodes);
            Visualization_of_nw_edge_line[] edges = Generate_edges(nw);
            if (node_colors.Length == 0) { }
            else
            {
                nodes = Adjust_node_colors_and_sizes(nodes, node_colors);
            }
            Visualization_of_nw_resource_line_class[] resources = Generate_resource_lines_and_connect_to_nodes(ref nodes, out exception_thrown);

            //Check_if_all_nodes_are_conncected_to_at_least_one_edge(nodes, edges);
            if (exception_thrown)
            {
                error_report_label.Visible = true;
                error_report_label.Text = "Network generation interrupted, probably due to insufficient memory. Please restart the application and try again or switch of in menu 'SCP-networks'.";
                error_report_label.Refresh();
                form_default_settings.LabelProgressReport_set_sizes_and_fontSize(error_report_label, 0);
                System.Threading.Thread.Sleep(4000);
                error_report_label.Text = "";
                error_report_label.Refresh();
            }
            else if (complete_file_name.Length > 260) { }
            else
            {
                Writer = ReadWriteClass.Get_new_stream_writer_and_sent_notice_if_file_in_use(complete_file_name, error_report_label, form_default_settings);
                Write_file_head();
                Write_all_nodes(nodes, Options.Group_same_level_processes);
                Write_all_edges(edges);
                if (legend_node_colors.Length > 0)
                {
                    Visualization_of_nw_node_line[] legend = Generate_legend(legend_node_colors);
                    Write_legend(legend);
                }
                Write_file_bottom_for_nodes_and_edges();
                if (resources.Length > 0) { Write_all_resources(resources); }
                Write_final_file_bottom();
                Writer.Close();
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
