CREATE PROCEDURE [dbo].[spSpinStatesElectronicStatesMethodFamilies_Update]
	@Id INT,
	@Name NVARCHAR(200),
	@Keyword NVARCHAR(50),
	@ElectronicStateMethodFamilyId INT,
	@SpinStateId INT,
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(4000)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE 
		[dbo].[SpinStatesElectronicStatesMethodFamilies]
	SET
		[Name] = @Name,
		[Keyword] = @Keyword,
		[ElectronicStateMethodFamilyId] = @ElectronicStateMethodFamilyId,
		[SpinStateId] = @SpinStateId,
		[DescriptionRtf] = @DescriptionRtf,
		[DescriptionText] = @DescriptionText,
		[LastUpdatedDate] = GETUTCDATE()
	WHERE
		[Id] = @Id;
END
