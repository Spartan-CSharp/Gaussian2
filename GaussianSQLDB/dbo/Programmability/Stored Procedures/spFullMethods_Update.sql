CREATE PROCEDURE [dbo].[spFullMethods_Update]
	@Id INT,
	@Keyword NVARCHAR(50),
	@SpinStateElectronicStateMethodFamilyId INT,
	@BaseMethodId INT,
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(4000)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE 
		[dbo].[FullMethods]
	SET
		[Keyword] = @Keyword,
		[SpinStateElectronicStateMethodFamilyId] = @SpinStateElectronicStateMethodFamilyId,
		[BaseMethodId] = @BaseMethodId,
		[DescriptionRtf] = @DescriptionRtf,
		[DescriptionText] = @DescriptionText,
		[LastUpdatedDate] = GETUTCDATE()
	WHERE
		[Id] = @Id;
END
