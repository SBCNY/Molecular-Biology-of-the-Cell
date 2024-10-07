To use MBCO PathNet with other ontologies, please follow the following instructions.

Reactome
Please download 'NCBI2Reactome_All_Levels.txt' ('Identifier mapping files': 'NCBI to All pathways'),
'ReactomePathwaysRelation.txt' ('Pathways': 'Pathways hierarchy relationship')
and 'ReactomePathways.txt' ('Pathways': 'Complete List of Pathways') from reactome.org.
Please download 'gene_info' from 'ftp.ncbi.nih.gov/gene/DATA/'.
Unzip and copy the files into the folder 'Other_ontologies'.

Gene Ontology
Please download  the annotation file 'goa_human.gaf' and  the ontology file 'go-basic.obo' from
geneontology.org. Unzip and copy both files into the folder 'Other_ontologies'.

Custom ontologies:
Please generate the tab-delimited file 'Custom_1_scpGeneAssociations.txt' with the columns
'Scp' and 'Symbol' that contain the pathway names and annotated NCBI official gene symbols,
respectively. Column names are case-sensitive. Scp is an abbreviation of 'Subcellular Process'.
Gene symbols can be capitalized or in lower case letters, since our application will transform
all symbol letters into upper case letters.
Please generate a second tab-delimieted file, 'Custom_1_scpHierarchy.txt' that contains the columns
'Child_scp' and 'Parent_scp'. This file allows specification of hierarchical parent-child relationships.
Scps in both files are case-sensitive. It is not necessary to add parent-child relationships for all
SCPs specified in 'Custom_1_scpGeneAssociations.txt' or to add any at all. The only requirement is
that the file 'Custom_1_scpHierarchy.txt' has a headline that contains both column names, even if
no rows will follow.
Please note that parent SCPs will NOT be populated with the genes of their child SCPs, in case of
the custom ontologies.
To genereate the files for the 2nd custom ontology, replace '1' by '2' in both file names.

The application will save the results of the initial processing for each ontology for faster
future accesses.
