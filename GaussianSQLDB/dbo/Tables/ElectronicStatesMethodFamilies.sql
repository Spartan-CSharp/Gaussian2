CREATE TABLE [dbo].[ElectronicStatesMethodFamilies]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(100) NULL,
	[Keyword] NVARCHAR(20) NULL,
	[ElectronicStateId] INT NOT NULL,
	[MethodFamilyId] INT NULL,
	[DescriptionRtf] NVARCHAR(MAX) NULL,
	[DescriptionText] NVARCHAR(2000) NULL,
	[CreatedDate] DATETIME2(7) NOT NULL DEFAULT (GETUTCDATE()),
	[LastUpdatedDate] DATETIME2(7) NOT NULL DEFAULT (GETUTCDATE()),
	[Archived] BIT NOT NULL DEFAULT 0,
	CONSTRAINT [PK_ElectronicStatesMethodFamilies] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UQ_ElectronicStatesMethodFamilies_Name_or_Keyword] UNIQUE NONCLUSTERED ([Name] ASC, [Keyword] ASC),
	CONSTRAINT [CK_ElectronicStatesMethodFamilies_Name_or_Keyword] CHECK ([Name] IS NOT NULL OR [Keyword] IS NOT NULL),
	CONSTRAINT [FK_ElectronicStatesMethodFamilies_ElectronicStates] FOREIGN KEY ([ElectronicStateId]) REFERENCES [dbo].[ElectronicStates] ([Id]),
	CONSTRAINT [FK_ElectronicStatesMethodFamilies_MethodFamilies] FOREIGN KEY ([MethodFamilyId]) REFERENCES [dbo].[MethodFamilies] ([Id])
)
