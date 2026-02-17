CREATE PROCEDURE [dbo].[spFullMethods_GetList]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Id],
		[Keyword]
	FROM 
		[dbo].[FullMethods]
	WHERE
		[Archived] = 0
	ORDER BY
		[Keyword] ASC;
END
