CREATE TABLE [dbo].[SpinStatesElectronicStatesMethodFamilies]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(200) NULL,
	[Keyword] NVARCHAR(50) NULL,
	[ElectronicStateMethodFamilyId] INT NOT NULL,
	[SpinStateId] INT NULL,
	[DescriptionRtf] NVARCHAR(MAX) NULL,
	[DescriptionText] NVARCHAR(4000) NULL,
	[CreatedDate] DATETIME2(7) NOT NULL DEFAULT (GETUTCDATE()),
	[LastUpdatedDate] DATETIME2(7) NOT NULL DEFAULT (GETUTCDATE()),
	[Archived] BIT NOT NULL DEFAULT 0,
	CONSTRAINT [PK_SpinStatesElectronicStatesMethodFamilies] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UQ_SpinStatesElectronicStatesMethodFamilies_Name_or_Keyword] UNIQUE NONCLUSTERED ([Name] ASC, [Keyword] ASC),
	CONSTRAINT [CK_SpinStatesElectronicStatesMethodFamilies_Name_or_Keyword] CHECK ([Name] IS NOT NULL OR [Keyword] IS NOT NULL),
	CONSTRAINT [FK_SpinStatesElectronicStatesMethodFamilies_ElectronicStatesMethodFamilies] FOREIGN KEY ([ElectronicStateMethodFamilyId]) REFERENCES [dbo].[ElectronicStatesMethodFamilies] ([Id]),
	CONSTRAINT [FK_SpinStatesElectronicStatesMethodFamilies_SpinStates] FOREIGN KEY ([SpinStateId]) REFERENCES [dbo].[SpinStates] ([Id])
)
