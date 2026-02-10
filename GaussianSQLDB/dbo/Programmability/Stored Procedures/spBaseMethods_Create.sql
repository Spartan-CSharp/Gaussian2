CREATE PROCEDURE [dbo].[spBaseMethods_Create]
	@Keyword NVARCHAR(50),
	@MethodFamilyId INT,
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(4000),
	@Id INT = 0 OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [dbo].[BaseMethods] ([Keyword], [MethodFamilyId], [DescriptionRtf], [DescriptionText])
	VALUES (@Keyword, @MethodFamilyId, @DescriptionRtf, @DescriptionText);
	SELECT @Id = SCOPE_IDENTITY();
END
