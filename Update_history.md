<b>Update history</b><br>
<br>
December 26th, 2023
- Adjustment of text sizes in bardiagram figures
- Update of Tips menu panel

May 25th, 2023
- Dataset-specific colors can be uploaded using the functionalities of the menu 'Read data'. It is now possible to define colors in hexadecimal numbers that will be mapped to the closest C# colors by the application. This allows visualization of MBCO enrichment results in the same colors used within other languages, e.g. R or Python. Simply convert the color names to hexadecimal numbers (e.g. using 'col2hex' that is part of the R-'gplots' library) and add these as a new column to the dataset file.

May 23rd, 2023
- The menu panel 'Set data cutoffs' was redesigned.

March 20th, 2023
- Heatmaps allow quick comparison of -log10(p-values) or ranks between the different datasets. If a heatmap contains too many rows (SCPs) and/or columns (datasets), it is split into multiple heatmap blocks. The blocks are now indexed by their position and first sorted by columns, then by rows, allowing easier comparison of the results for the same SCP accross all datasets.
- If the option to visualize only SCPs of interest is selected, the user can now define significance criteria for the heatmap. Fields containing significant -log10(p-values) or ranks will be colored in red or blue. All other fields will be colored in white.

January 13th, 2023
- Updates in the menu panel 'Read data' and the explanatory descriptions of the included functionalities.

December 23rd, 2022
- Additional explanation buttons were added that open descriptions of the included functionalities.

December 20th, 2022
- The MBCO application has been compiled on a Linux system using Mono ('mono-project.com') that is "an open source implementation of Microsoft's .NET Framework", "sponsored by Microsoft". To run the linux version, download the compiled 'mono-xsp4' package from 'mono-project.com' and open the application in the terminal with the command 'mono Molecular_Biology_of_the_Cell_Ontology.exe'.
- The graphical library that generates bardiagrams, heatmaps, timelines and network piecharts was switched to ZedGraph. Besides Windows ZedGraph also supports Linux (and Mac) operating systems.
- A new interface was generated that allows investigation of the results within the desktop application.
- The sodium-glucose transmembrane transport ontology was added as a specialized MBCO dataset.
- Pathway networks can also be generated using Gene Ontology. Download and unzip the files 'go-basic.obo' and 'goa_human.gaf' from 'geneontology.org' and copy them into the folder 'GO_datasets'.
- User-defined parameter settings are be specified, saved and re-imported independently for each ontology.
- More explanation buttons were added to the application.

December 5th, 2022
- The graphical interface of the application was redesigned, so that all elements adapt to the current screen resolution. In addition, height, width and color scheme can also be adjusted by the user.
- The menu panel 'Organize data' now also allows addition of the dataset order numbers to the dataset names.

October 11th, 2022
- The functionalities of the menu panel ‘Read data’ were updated. It is not necessary any longer to define a column that specifies a dataset name.

October 1st, 2022
- Besides saving datasets and background genes in the results subfolder ‘Input_data’ we now save the user-selected parameter settings as well. Use of the   functionalities in the menu panel ‘Read data’ will now also re-import the saved parameter settings into the application, allowing quick reproduction of generated   results and recapitulation of parameter settings. 

September 27th, 2022
- In the menu panel ‘Select SCPs’ we introduced the option to automatically add or remove all ancestor and/or offspring SCPs of user-selected SCPs. This allows easy   and quick investigation of how experimental gene lists map to complete MBCO parent-child SCP branches.
- The menu panel ‘Define own SCPs’ was updated, including an increase in the size of the list box ‘Genes of the following SCPs will be added to SCP [My SCP]’.
- The menu panel ‘Tips’ was updated.


September 24th, 2022
- If the checkbox ‘Read all files in directory’ in the menu panel ‘Read data’ is selected, pressing the ‘Read’-button will read all data files in a given directory. We   extended this functionality, so that all files that contain background genes (i.e., files that end with the label “_bgGenes”) will also be read. Additionally, the   lists of background genes will be automatically assigned to those datasets that contain the same file name without the “_bgGenes” addition. This automatic extension   mimics the manual addition of the background genes, using the ‘Read’- and ‘Automatically assign background genes’-buttons in the menu panel ‘Background genes’.
- The menu panel ‘Background genes’ was updated.
- Memory usage during the generation of PDFs was optimized.


September 20th, 2022
- The menu panel 'Example data' was updated.
- A new checkbox was introduced in the menu panel 'Set data cutoffs' that allows permanent removal of not significant genes from the user-supplied
  datasets. Removal of long lists of not significant genes (that will be ignored for enrichment analysis) can speed up the time needed for data organization or analysis.
- Subcellular processes (SCPs) in the network diagrams are visualized as circles to document their integration into the MBCO hierarchy or inferred MBCO networks. If  the same SCP is predicted based on multiple datasets, each dataset will be represented by a different slice. The thickness of the black lines that separate these slices from each other will now be adjusted to the number of slices. This ensures that the color of each slice can still be identified and will not be overlaid by the separation lines, even if the number of slices is very high (>20 or >30).
- The output in the 'Selected parameter' file was updated.
- Empty entries in the text boxes for specification of substring indexes (panel 'Organize data') will be ignored.
- The menu panel 'Select SCPs' allows selection of SCPs that will be shown in all result files (bar diagrams, heatmaps, timelines and SCP-networks), independently of their significance. We updated this functionality, so that the user can specify multiple groups of SCPs at the same time. Our pipeline will generate one set of result files for each group and add the group name to the file names. 
- Since a main purpose of the 'Select SCPs' functionality lies in the visualization of the experimental genes that map to the selected SCPs, we introduced the new checkbox 'Add genes' to this panel. This checkbox becomes visible, once the checkbox 'Ignore significance cutoffs' is selected. The new checkbox is identical with the checkbox 'Add genes' in the menu panel 'SCP-networks', section 'Standard enrichment analysis'. If the new checkbox is checked, all experimental genes that map to the selected SCPs will be added to the network.

July 25th, 2022
- The menu panel 'Tips' was updated.
- Two rare conditions that caused an interruption during the generation of bardiagrams or SCP networks were fixed.

June 30th, 2022
- To quickly assign integration groups or colors in the menu panel ‘Organize Data’ the user can group multiple datasets together based on shared characteristics,  such as Up/Down statuses, time points or substrings within their names. It is now possible to define multiple substrings by defining multiple substring indexes. The indexes need to be separated by commas. All datasets with the same substrings at the indexed positions will be grouped together.
- The script now writes the data submitted to enrichment analysis as multiple files into the results folder 'Input_data' using the file names specified in the column 'Source'. Similarly, the lists of experimental background genes are written into that folder as well. To repeat a previous analysis, the user can simply read those files using the regular read pipeline in the menu ‘Read data’ (using the ‘MBCO’ column names). The list of background genes can be added using the read and assignment pipeline in the menu panel ‘Background genes’. This way the scripts also avoids automatic screening for some columns that are part of the saved input data files (e.g., column ‘Source’).
- Any rows in an uploaded data file that contain zeros in the column assigned to the 1st value will be removed right after the upload.
- A dataset in an uploaded file is now defined by the same name, Up/Down status, timepoint and integration group. Consequently, rows with the same name, Up/Down status and timepoint combination but different integration groups will be assigned to different datasets.
- Additional buttons will change their color to light blue when pressed. Color will be set back to dark blue, once the requested action is finished.

June 19th, 2022
- The pipeline now allows analysis of datasets with the same name, Up/Down status and timepoint, as long as they are assigned to different integration groups.
- The menu panel ‘Tips’ was introduced.
- If a file that assigns multiple integration groups or colors to the same dataset (as defined by a unique dataset name, Up/Down status and timepoint combination) is uploaded, the script will warn the user with an error message and ignore the upload (instead of throwing an exception during an internal correctness check). A similar warning already exists, if the same dataset contains the same gene symbol multiple times.
- The menu panel ‘SCP networks’ was updated.
- The output within the file ‘Selected_parameter’ was updated.
- If an uploaded dataset file contains only zero values within the column that is assigned to the 1st Value in the ‘Read data’ menu, the script will warn the user with an error message and ignore the upload.
- If no column name is specified for the 1st values, all 1st values in the uploaded data files will be set to 1.
- If MBCO dataset files are missing or corrupted, the script will ask the user to download the MBCO application again.

June 11th, 2022
- The new check box 'Connect all related SCPs' under ‘Dynamic enrichment analysis’ in the menu panel 'SCP-networks' allows the user to specify visualized SCP-connections in the networks derived from dynamic enrichment analysis. Unchecked, only those SCPs will be connected that were combined to form a context-specific higher level-SCP that meets the significance criteria, defined in the menu panel ‘Enrichment’. These combined SCPs label the same bar in the bardiagram charts for dynamic enrichment analysis. The results text file that documents the significant predictions for dynamic enrichment analysis, lists these SCPs as one entry in the column ‘SCP’, separated by dollar signs. Since an SCP can be part of multiple higher-level SCPs, it can be connected to more than two other SCPs. If the check box is checked, any SCPs will be connected, as long as their interaction is among the top percentages of considered SCP interactions, no matter if they are part of the same significant context-specific higher level-SCP or not. If checked, SCPs will also be connected across different datasets. The top percentages of considered SCP interactions are the same as those used for the dynamic enrichment analysis algorithm and can be modified in the menu panel ‘Enrichment’. 

June 08th, 2022
- Any user input for an enrichment, data significance or network parameter that lies outside of the available parameter ranges (e.g., a maximum p-value > 1) will change the text box color to orchid, indicating that those entries will be ignored.
- The text boxes labeled "Top % of SCP interactions for dynamic enrichment analysis" in the menu panel 'Enrichment' allow optimization of the dynamic enrichment algorithm. Changing these values will update the background infered SCP-networks for the next analysis that form the basis of the dynamic enrichment algorithm.

May 15th, 2022
- The dataset ‘KPMP reference tissue atlas’ was added as a new example dataset.
