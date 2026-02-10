CREATE TABLE [dbo].[BaseMethods]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[Keyword] NVARCHAR(50) NOT NULL,
	[MethodFamilyId] INT NOT NULL,
	[DescriptionRtf] NVARCHAR(MAX) NULL,
	[DescriptionText] NVARCHAR(4000) NULL,
	[CreatedDate] DATETIME2(7) NOT NULL DEFAULT (GETUTCDATE()),
	[LastUpdatedDate] DATETIME2(7) NOT NULL DEFAULT (GETUTCDATE()),
	[Archived] BIT NOT NULL DEFAULT 0,
	CONSTRAINT [PK_BaseMethods] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UQ_BaseMethods_Keyword] UNIQUE NONCLUSTERED ([Keyword] ASC),
	CONSTRAINT [FK_BaseMethods_MethodFamilies] FOREIGN KEY ([MethodFamilyId]) REFERENCES [dbo].[MethodFamilies] ([Id])
)
