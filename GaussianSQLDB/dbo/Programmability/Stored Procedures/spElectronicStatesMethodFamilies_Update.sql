CREATE PROCEDURE [dbo].[spElectronicStatesMethodFamilies_Update]
	@Id INT,
	@Name NVARCHAR(200),
	@Keyword NVARCHAR(50),
	@ElectronicStateId INT,
	@MethodFamilyId INT,
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(4000)
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
