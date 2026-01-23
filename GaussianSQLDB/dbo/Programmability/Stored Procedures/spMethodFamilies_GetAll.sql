CREATE PROCEDURE [dbo].[spMethodFamilies_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Id],
		[Keyword],
		[DescriptionRtf],
		[DescriptionText],
		[CreatedDate],
		[LastUpdatedDate],
		[Archived]
	FROM 
		[dbo].[MethodFamilies]
	WHERE
		[Archived] = 0
	ORDER BY
		[Keyword] ASC;
END
