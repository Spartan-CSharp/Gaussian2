CREATE PROCEDURE [dbo].[spElectronicStatesMethodFamilies_Update]
	@Id INT,
	@Name NVARCHAR(100),
	@Keyword NVARCHAR(20),
	@ElectronicStateId INT,
	@MethodFamilyId INT,
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(2000)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE 
		[dbo].[ElectronicStatesMethodFamilies]
	SET
		[Name] = @Name,
		[Keyword] = @Keyword,
		[ElectronicStateId] = @ElectronicStateId,
		[MethodFamilyId] = @MethodFamilyId,
		[DescriptionRtf] = @DescriptionRtf,
		[DescriptionText] = @DescriptionText,
		[LastUpdatedDate] = GETUTCDATE()
	WHERE
		[Id] = @Id;
END
