# Project Plan

## Walk Through the Project

We need an application to store, index, and retrieve the results of electronic structure calculations performed with the Gaussian 16 software. The application should allow users to capture the key input parameters of each calculation, including the Calculation Type, the Method, the Method Family, the Spin State, the Electronic State, the Basis Set, the Density Fitting Set, the Molecule under consideration, and any additional Keywords used in the calculation. The application should also store the key results of each calculation, and allow users to summarize and discuss the importance or significance of the results.

Individual calculations should be grouped into "Experiments", and these should allow users to enter the overall results, significance, importance, and discussion, much as these things would appear in a research paper.

Users should be able to search for an retrieve records based on the above parameters, including a text search within the results/discussions fields.

While there is no requirement for file storage to store input and output files, the application should store references to these files, and for the desktop application, allow users to open these files in their default application.

## Open Up the Requirements

### 1. Users & Usage Context

#### A. Who is this for and how will they use it?

##### I. Who are the intended users (e.g., individual researchers, research groups, labs, students)?

The intended users are individual researchers and research groups in academic or industrial settings who perform electronic structure calculations using Gaussian 16.

##### II. Is this a single-user application or multi-user with shared data?

This will be a multi-user application with shared data, allowing collaboration among researchers within a group or lab.

##### III. Will users need different roles or permissions (e.g., read-only, editor, admin)?

This is a *nice-to-have* feature, but does not need to be implemented in version 1. However, the application should be set up in a way that would allow for future implementation of user roles and permissions.

##### IV. Is this intended for daily active use or occasional archival/reference use?

Storing of new data will be daily, as calculations are performed. Retrieval may be more occasional, depending on the research workflow.

##### V. Do users typically work with many small calculations or fewer large ones?

Both. Also, these calculations are often grouped into larger sets, called "Experiments".

#### B. Environment

##### I. Is this primarily a desktop application, a web application, or both?

As a demonstration app, this will have both a desktop interface and a web interface. If it is eventually rolled out as a production app, it would likely be web-based, but hosted on an internal server using IIS.

##### II. Which operating systems must be supported?

For version 1, Windows only. Eventually, Linux support would be desirable. We do not forsee needing to support macOS or mobile devices.

##### III. Is the application expected to run offline?

No.

### 2. Scope of Calculation Metadata

#### A. Input parameters

##### I. Are the listed parameters (Calculation Type, Method, Basis Set, etc.) fixed, or should users be able to define new ones?

These are fixed, by the Gaussian software. Unless new selections get added in future versions, this list can remain fixed. The app should be built around Gaussian 16, and new versions of Gaussian can be considered in future versions of the app.

##### II. Should these fields be free-text, constrained to controlled vocabularies, or selectable from predefined lists?

These should mostly be selectable from predefined lists, though users should be able to add to those lists as needed (e.g., new keywords, new basis sets, new molecules, etc.). The exception is the charge and spin multiplicity, which should be numeric (integer) fields.

##### III. Are there standard taxonomies we should align with (e.g., Gaussian keyword conventions)?

The application must follow conventions and taxonomies set out in the Gaussian software.

##### IV. Can a calculation have multiple methods, basis sets, or keywords?

As Gaussian 16 supports ONIOM calculations (S. Dapprich, I. Komáromi, K. S. Byun, K. Morokuma, and M. J. Frisch, “A New ONIOM Implementation in Gaussian 98. 1. The Calculation of Energies, Gradients and Vibrational Frequencies and Electric Field Derivatives,” *J. Mol. Struct. (Theochem)*, 462 (1999) 1-21. DOI: [10.1016/S0166-1280(98)00475-8)](https://dx.doi.org/10.1016/S0166-1280\(98\)00475-8), the application should support these calculations, which may have multiple methods and basis sets. However, beyond this, each calculation would only have one method, one basis set, and at most one density fitting set.

##### V. Should parameters be versioned if they change over time?

There is no requirement for versioning.

#### B. Molecules

##### I. How should molecules be identified (name, formula, SMILES, InChI, structure file reference)?

Molecules should be primarily identified by the IUPAC name, but also have fields to store the common name/description, any synonyms, the empirical formula, and the SMILES and InChI strings. The database should also be able to link to an image file that displays the structural formula.

##### II. Can multiple calculations reference the same molecule?

Yes, multiple calculations can reference the same molecule. Indeed, we want to avoid duplication of molecule records where possible.

##### III. Do users want to store molecular metadata (charge, multiplicity, geometry summary)?

The application should store the charge, spin multiplicity, whether the molecule is an an electronic ground state or in an excited state, and whether the geometry is a local minimum or a transition state. Any variations of this would be stored as separate molecule records. Isotopologues would also be considered separate molecule records, but conformers would not.

### 3. Results & Scientific Content

#### A. Results data

##### I. What qualifies as “key results”?

The "key results" will depend on the calculation type and the experimental goals. The application should allow storing of those results that are captured in the archiving entry at the end of the output file into separate fields. There should also be a free-text field into which the user can copy and paste any other relevant results from the output file, as well as a field for notes and discussions of these results.

##### II. Should results be structured (numeric fields) or unstructured (text)?

Both. Some results will be numeric fields (e.g., total energy, optimized geometry) while others may be unstructured text (e.g., discussion of results).

##### III. Are units important, and should the application enforce or convert them?

The application should use the same units as the Gaussian 16 software. There is no requirement for unit conversion.

##### IV. Should derived values or comparisons between calculations be supported?

Not at this time, this is perhaps a Version 3 item.

#### B. Discussion & interpretation

##### I. Are results summaries plain text, rich text, or markdown?

The actual Gaussian output file is plain text, but the discussion and interpretation fields should support rich text formatting (bold, italics, lists, etc.).

##### II. Should users be able to link conclusions to specific calculations or experiments?

Calculations should be linked to experiments, and each level should have its own discussion and interpretation fields. There isn't a need to link specific conclusions to specific calculations beyond this.

##### III. Is citation or reference tracking required?

Not in version 1, but this is a *nice-to-have* feature for future versions.

### 3. Experiments & Grouping

#### A. Experiment structure

##### I. What defines an “Experiment”?

An experiment is a collection of related calculations that together address a specific research question or hypothesis.

##### II. Can calculations belong to more than one experiment?

No, each calculation belongs to only one experiment. If another experiment needs to reference the same calculation, it would need to be duplicated.

##### III. Is there an expected hierarchy (Project → Experiment → Calculation)?

Yes, though "Project" is not a required level in version 1. The hierarchy is Experiment → Calculation. Eventually, Projects could be added as a higher-level grouping.

##### IV. Should experiments support their own metadata (date, goal, hypothesis)?

Yes.

#### B. Scientific workflow

##### I. Do experiments evolve over time, or are they immutable once completed?

Experiments may evolve over time as new calculations are added or interpretations are refined. However, once an experiment is marked as "final", it should be immutable.

##### II. Should users be able to mark experiments as “draft”, “in progress”, or “final”?

Yes, users should be able to mark experiments with these statuses to reflect their current state.

### 5. Search, Indexing & Retrieval

#### A. Search behavior

##### I. What fields must be searchable?

For version 1, the only requirement is to return calculations that reference a specific input parameter (calculation type, method, basis set, spin state, electronic state, molecule). Future versions should include a full-text search of results and discussions.

##### II. Should search be exact match, partial match, or fuzzy?

Not applicable for version 1.

##### III. Should users be able to combine filters (e.g., Method + Basis Set + Spin State)?

Yes.

##### IV. Should text search cover:

###### a. Results

Not applicable for version 1.

###### b. Discussions

Not applicable for version 1.

###### c. Keywords

Not applicable for version 1.

###### d. File references?

Not applicable for version 1.

#### B. Performance

##### I. How large do you expect the database to grow?

The database will likely grow to thousands of calculations over time, depending on the research activity of the users.

##### II. Are search response times critical?

Searches should return results within seconds to minutes, not hours to days.

##### III. Should results be ranked or simply filtered?

For version 1, results should simply be filtered, with the ability of the user to sort based on the result columns.

### 6. File References & Integration

#### A. File handling

##### I. What types of files will be referenced (input files, output logs, checkpoint files)?

We expect the following file types to be supported:
* Gaussian input files (.com, .gjf)
* Gaussian output files (.log, .out)
* Gaussian checkpoint files (.chk)
* Gaussian formatted checkpoint files (.fchk)
* Ampac input files (.data, .dat)
* Ampac output files (.aout, .out)
* Ampac archive files (.aarc, .arc)
* Ampac Vis files (.avis, .vis)
* Cube files (.cub, .cube)
* Protein Data Bank files (.pdb, .pdb1)
* MDL MOL files (.mol)
* MDL RXN files (.rxn)
* MDL SDF files (.sdf)
* GMMX input files (.gmmx)
* Sybyl MOL2 files (.ml2, .mol2)
* Microsoft Office files (.docx, .xlsx, .pptx, etc.)
* Image files (.png, .jpg, .jpeg, .tiff, .bmp, etc.)
* PDF files (.pdf)

##### II. Are file paths local, network-based, or cloud-based?

File paths will primarily be local or network-based (e.g., UNC paths). Cloud-based storage is not a requirement for version 1.

##### III. Can file paths change over time?

Yes, file paths can change over time. The application should allow users to update file references as needed. However this updating would be manual by the user; there is no requirement for automatic path updating.

##### IV. Should broken file references be detected?

That is not a current requirement.

#### B. Desktop integration

##### I. Is “open in default application” sufficient, or are previews desired?

With one exception, "Open in default application" is sufficient for version 1. Previews are a *nice-to-have* feature for future versions.

The exception is for the image file that shows the structural formula of the molecule. This image should be previewed within the application.

##### II.  Should the application remember last-used directories?

For version 1, it will be sufficient if this is rememebered for the lifetime of the application. Eventually, persisting that information would be desireable.

##### III. * Is drag-and-drop support expected?

Not at this time.

### 7. Data Integrity & Validation

#### A. Should the application prevent duplicate calculations?

No, since the same calculation could be run multiple times and there are no easy criteria to determine whether two calculations are truly identical. However, the application should prevent duplicate molecules as well as duplicate input parameters in the predefined lists (methods, basis sets, keywords, etc.).

#### B. What fields are required vs optional?

All of the input parameters should be mandatory on a calculation record. The results and discussion fields can be optional, as users may want to enter these later.

#### C. Should changes to calculations or experiments be tracked (audit/history)?

No, there is no requirement for audit/history tracking.

#### D. Is data export required (CSV, JSON, BibTeX, etc.)?

Not for version 1.

#### E. Is backup or restore functionality expected?

No.

### 8. Non-Functional Requirements

#### A. Usability

##### I. Is ease of data entry a priority over strict validation?

Strict validation is a priority to ensure data integrity, but ease of data entry is also important to encourage user adoption. The application should strike a balance between these two aspects.

##### II. Should there be templates for common calculation types?

This is not needed at this time.

##### III. Do users want batch entry or bulk editing?

Not for version 1. Future versions could consider a feature that allows bulk importing via a CSV or JSON file.

#### B. Extensibility

##### I. Is support for non-Gaussian software anticipated in the future?

The only one that would be considered is Ampac, since Gaussian and Ampac are "sisters". However, this is a *nice-to-have* feature for future versions, not a requirement for version 1.

##### II. Should the system be designed to accommodate additional calculation engines?

No.

#### C. Compliance & Longevity

##### I. Are there data retention or reproducibility requirements?

The database needs to be secure and reliable, to ensure the entered data is retained as long as needed by the user. "Deletions" should be accomplished by setting an archive bit, not by actually deleting the record from the database.

##### II. Is this intended as a prototype, internal tool, or long-term product?

This application is intended as a prototype or proof-of-concept and a demonstration application.

### 9. Success Criteria

#### A. What does success look like for this application?

Success at this stage is a functioning application that allows users to enter, store, index, and retrieve Gaussian 16 calculation results along with their associated metadata, and that meets the outlined requirements for version 1.

#### B. What problem would it replace or improve upon?

The goal of the application is to centralize and consolidate data storage and management for Gaussian 16 calculations. This would improve upon the current situation where researchers may be using disparate methods (spreadsheets, text files, etc.) to track their calculations, leading to inefficiencies and potential data loss.

#### C. Are there existing tools users like or dislike that we should learn from?

None of which we are currently aware.

#### D. How will you decide that version 1 is “done”?

Version 1 only needs to be a Minimum Viable Product (MVP).

### 10. Out-of-Scope Clarification (Very Important)

#### A. Are we explicitly **not**:

##### I. Parsing Gaussian output files?

That is correct; that would be beyond the scope of any version of this application.

##### II. Performing calculations?

That is correct; that would be beyond the scope of any version of this application.

##### III. Visualizing molecular structures?

With the limited exception of displaying a structural formula image file, that is correct; that would be beyond the scope of this application.

##### IV. Providing statistical or comparative analysis tools?

That is correct; that would be beyond the scope of version 1 of this application, though it could be considered in future versions.

#### B. What features are *nice-to-have* vs *must-have*?

The *must-have* features are those outlined in the requirements above. *Nice-to-have* features include user roles and permissions, citation tracking, file previews, support for additional calculation engines, and bulk data import/export capabilities.

## User Interface Design

### The Windows Presentation Foundation Desktop User Interface

The WPF app will use a menu system to load user controls into the main window for the various screens. For each record type, there should be a screen to create a new one (with a "New" button), a screen to select an existing one to view or edit (including "delete"). There should also be a search screen to find calculations based on the input parameters.

The WPF app will access the database using the API that will be part of the MVC web application. It will need to authenticate and authorize the user through the API, so a login page will be needed.

The expected challenges will be around Rich Text Format fields, drop-down select fields, and scrolling, given the size of some of the pages and several one-to-many relationships (such as associated files).

### The ASP.NET Core Model-View-Controller Web Application User Interface

The ASP.NET Core web application will include Model-View-Controller (MVC) pages for each record type, as well as API routing for each record type. We will use the authenticastion model provided in the Microsoft template (using a separate identity database and Entity Framework). This can be eventually replaced with another authentication system such as Azure AD B2C.

The ASP.NET Core app will have direct database access via a Class Library, using Dapper for the ORM.

The expected challenges will be around Rich Text Format fields and drop-down select fields; scrolling should not be an issue since it will be handled by the browser.

## Logic Design

### UI Layer

The UI layer (both UIs and the API) will have the logic for the user interaction, as well as basic data validation (ensuring required fields are filled in, data types are correct, etc.). UI-specific models will be used, mapped to the full models used in the Business Logic Layer.

The desktop UI will have its own Class Library, and the Web application will also have its own Class Library; there will be a third Class Library for shared functionality between the two UIs.

### Business Logic Layer

The Business Logic Layer will contain the core functionality of the application, including the rules for how data can be created, read, updated, and deleted. For now, there will not be a separate Data Access Layer, so this layer will interact with the database.

This layer will include authentication and authorization, as well as full data validation, including duplicate checking where necessary.

### Database Layer

The Database Layer will be a SQL Server database, accessed via Dapper in the Business Logic Layer. The database will include tables for each record type, with appropriate relationships and constraints to ensure data integrity.

Data access will be via stored procedures to encapsulate the SQL logic and improve performance and security.

## Data Design

### Experiments Table
Id
Name
PurposeRtf
PurposeText
ResultsRtf
ResultsText
CreatedDate
LastUpdatedDate
Archived

### Calculations Table
Id
ExperimentId -- Links to Experiments.Id
Name
DescritionRtf
DescriptionText
CalculationTypeId -- Links to CalculationTypes.Id
ONIOMId -- Links to ONIOMs.Id
MoleculeId -- Links to Molecules.Id
TerminationDateTime
ElapsedTime
Success
RouteSection
ArchiveEntry
ResultsRtf
ResultsText
CreatedDate
LastUpdatedDate
Archived

### CalculationTypes Table
Id
Name
Keyword
DescriptionRtf
DescriptionText
CreatedDate
LastUpdatedDate
Archived

### ONIOMs Table
Id
LayerOneId -- Links to ModelChemistries.Id
LayerTwoId -- Links to ModelChemistries.Id
LayerThreeId -- Links to ModelChemistries.Id
Keyword
DescriptionRtf
DescriptionText
CreatedDate
LastUpdatedDate
Archived

### ModelChemistries Table
Id
FullMethodId -- Links to FullMethods.Id
BasisSetId -- Links to BasisSets.Id
DensityFittingSetId -- Links to DensityFittingSets.Id
Keyword
DescriptionRtf
DescriptionText
CreatedDate
LastUpdatedDate
Archived

### FullMethods Table
Id
SpinStateElectronicStateMethodFamilyId -- Links to SpinStateElectronicStateMethodFamilies.Id
BaseMethodId -- Links to BaseMethods.Id
Keyword
DescriptionRtf
DescriptionText
CreatedDate
LastUpdatedDate
Archived

### BaseMethod Table
Id
MethodFamilyId -- Links to MethodFamilies.Id
Keyword
DescriptionRtf
DescriptionText
CreatedDate
LastUpdatedDate
Archived

### SpinStateElectronicStateMethodFamilies Table
Id
ElectronicStateMethodFamilyId -- Links to ElectronicStateMethodFamilies.Id
SpinStateId -- Links to SpinStates.Id
Keyword
DescriptionRtf
DescriptionText
CreatedDate
LastUpdatedDate
Archived

### ElectronicStateMethodFamilies Table
Id
ElectronicStateId -- Links to ElectronicStates.Id
MethodFamilyId -- Links to MethodFamilies.Id
Keyword
DescriptionRtf
DescriptionText
CreatedDate
LastUpdatedDate
Archived

### SpinStates Table
Id
Keyword
DescriptionRtf
DescriptionText
CreatedDate
LastUpdatedDate
Archived

### ElectronicStates Table
Id
Keyword
DescriptionRtf
DescriptionText
CreatedDate
LastUpdatedDate
Archived

### MethodFamilies Table
Id
Keyword
DescriptionRtf
DescriptionText
CreatedDate
LastUpdatedDate
Archived

### BasisSets Table
Id
Keyword
DescriptionRtf
DescriptionText
CreatedDate
LastUpdatedDate
Archived

### DensityFittingSets Table
Id
Keyword
DescriptionRtf
DescriptionText
CreatedDate
LastUpdatedDate
Archived

### Molecules Table
Id
CommonName
IUPACName
Synonyms
EmpiricalFormula
SpeciesTypeId -- Links to SpeciesTypes.Id
ElectricCharge
SpinMultiplicity
ImageFileId -- Links to Files.Id
SMILES
InChI
CreatedDate
LastUpdatedDate
Archived

### SpeciesTypes Table
Id
Name
DescriptionRtf
DescriptionText
CreatedDate
LastUpdatedDate
Archived

### Files Table
Id
FilePath
FileTypeId -- Links to FileTypes.Id
CreatedDate
LastUpdatedDate
Archived

### FileTypes Table
Id
Type
CommandLine
CreatedDate
LastUpdatedDate
Archived

### FileExtensions Table
Id
Extension
FileTypeId -- Links to FileTypes.Id
CreatedDate
LastUpdatedDate
Archived

### CalculationFiles Table
Id
CalculationId -- Links to Calculations.Id
FileId -- Links to Files.Id
CreatedDate
LastUpdatedDate
Archived

### ExperimentFiles Table
Id
ExperimentId -- Links to Experiments.Id
FileId -- Links to Files.Id
CreatedDate
LastUpdatedDate
Archived
