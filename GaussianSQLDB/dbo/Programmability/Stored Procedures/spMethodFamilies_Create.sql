CREATE PROCEDURE [dbo].[spMethodFamilies_Create]
	@Keyword NVARCHAR(200),
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(2000),
	@Id INT = 0 OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [dbo].[MethodFamilies] ([Keyword], [DescriptionRtf], [DescriptionText])
	VALUES (@Keyword, @DescriptionRtf, @DescriptionText);
	SELECT @Id = SCOPE_IDENTITY();
END
