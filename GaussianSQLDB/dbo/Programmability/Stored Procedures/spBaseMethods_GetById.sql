CREATE PROCEDURE [dbo].[spBaseMethods_GetById]
	@Id int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Id],
		[Keyword],
		[MethodFamilyId],
		[DescriptionRtf],
		[DescriptionText],
		[CreatedDate],
		[LastUpdatedDate],
		[Archived]
	FROM 
		[dbo].[BaseMethods]
	WHERE
		[Id] = @Id AND
		[Archived] = 0
	ORDER BY
		[Keyword] ASC;
END
