CREATE TABLE [dbo].[ElectronicStates]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(200) NULL,
	[Keyword] NVARCHAR(50) NULL,
	[DescriptionRtf] NVARCHAR(MAX) NULL,
	[DescriptionText] NVARCHAR(4000) NULL,
	[CreatedDate] DATETIME2(7) NOT NULL DEFAULT (GETUTCDATE()),
	[LastUpdatedDate] DATETIME2(7) NOT NULL DEFAULT (GETUTCDATE()),
	[Archived] BIT NOT NULL DEFAULT 0,
	CONSTRAINT [PK_ElectronicStates] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UQ_ElectronicStates_Name_or_Keyword] UNIQUE NONCLUSTERED ([Name] ASC, [Keyword] ASC),
	CONSTRAINT [CK_ElectronicStates_Name_or_Keyword] CHECK ([Name] IS NOT NULL OR [Keyword] IS NOT NULL)
)
