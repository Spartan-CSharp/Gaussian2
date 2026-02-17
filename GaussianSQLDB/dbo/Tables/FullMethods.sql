CREATE TABLE [dbo].[FullMethods]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[Keyword] NVARCHAR(50) NOT NULL,
	[SpinStateElectronicStateMethodFamilyId] INT NOT NULL,
	[BaseMethodId] INT NOT NULL,
	[DescriptionRtf] NVARCHAR(MAX) NULL,
	[DescriptionText] NVARCHAR(4000) NULL,
	[CreatedDate] DATETIME2(7) NOT NULL DEFAULT (GETUTCDATE()),
	[LastUpdatedDate] DATETIME2(7) NOT NULL DEFAULT (GETUTCDATE()),
	[Archived] BIT NOT NULL DEFAULT 0,
	CONSTRAINT [PK_FullMethods] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UQ_FullMethods_Keyword] UNIQUE NONCLUSTERED ([Keyword] ASC),
	CONSTRAINT [UQ_FullMethods_SpinStateElectronicStateMethodFamilyId_and_BaseMethodId] UNIQUE NONCLUSTERED ([SpinStateElectronicStateMethodFamilyId] ASC, [BaseMethodId] ASC),
	CONSTRAINT [FK_FullMethods_SpinStateElectronicStateMethodFamilies] FOREIGN KEY ([SpinStateElectronicStateMethodFamilyId]) REFERENCES [dbo].[SpinStatesElectronicStatesMethodFamilies] ([Id]),
	CONSTRAINT [FK_FullMethods_BaseMethods] FOREIGN KEY ([BaseMethodId]) REFERENCES [dbo].[BaseMethods] ([Id])
)
