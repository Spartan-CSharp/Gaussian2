CREATE PROCEDURE [dbo].[spMethodFamilies_Update]
	@Id INT,
	@Name NVARCHAR(200),
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(4000)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE 
		[dbo].[MethodFamilies]
	SET
		[Name] = @Name,
		[DescriptionRtf] = @DescriptionRtf,
		[DescriptionText] = @DescriptionText,
		[LastUpdatedDate] = GETUTCDATE()
	WHERE
		[Id] = @Id;
END
