CREATE PROCEDURE [dbo].[spMethodFamilies_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Id],
		[Name],
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
		[Name] ASC;
END
