CREATE PROCEDURE [dbo].[spBaseMethods_GetAll]
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
		[Archived] = 0
	ORDER BY
		[Keyword] ASC;
END
