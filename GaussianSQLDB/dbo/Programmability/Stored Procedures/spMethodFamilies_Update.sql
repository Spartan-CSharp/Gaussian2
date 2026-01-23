CREATE PROCEDURE [dbo].[spMethodFamilies_Update]
	@Id INT,
	@Keyword NVARCHAR(200),
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(2000)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE 
		[dbo].[MethodFamilies]
	SET
		[Keyword] = @Keyword,
		[DescriptionRtf] = @DescriptionRtf,
		[DescriptionText] = @DescriptionText,
		[LastUpdatedDate] = GETUTCDATE()
	WHERE
		[Id] = @Id;
END
