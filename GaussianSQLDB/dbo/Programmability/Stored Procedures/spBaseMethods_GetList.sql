CREATE PROCEDURE [dbo].[spBaseMethods_GetList]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Id],
		[Keyword]
	FROM 
		[dbo].[BaseMethods]
	WHERE
		[Archived] = 0
	ORDER BY
		[Keyword] ASC;
END
