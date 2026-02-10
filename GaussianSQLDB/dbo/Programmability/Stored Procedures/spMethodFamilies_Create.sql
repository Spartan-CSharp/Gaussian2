CREATE PROCEDURE [dbo].[spMethodFamilies_Create]
	@Name NVARCHAR(200),
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(4000),
	@Id INT = 0 OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [dbo].[MethodFamilies] ([Name], [DescriptionRtf], [DescriptionText])
	VALUES (@Name, @DescriptionRtf, @DescriptionText);
	SELECT @Id = SCOPE_IDENTITY();
END
