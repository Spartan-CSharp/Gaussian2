CREATE PROCEDURE [dbo].[spBaseMethods_Update]
	@Id INT,
	@Keyword NVARCHAR(50),
	@MethodFamilyId INT,
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(4000)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE 
		[dbo].[BaseMethods]
	SET
		[Keyword] = @Keyword,
		[MethodFamilyId] = @MethodFamilyId,
		[DescriptionRtf] = @DescriptionRtf,
		[DescriptionText] = @DescriptionText,
		[LastUpdatedDate] = GETUTCDATE()
	WHERE
		[Id] = @Id;
END
