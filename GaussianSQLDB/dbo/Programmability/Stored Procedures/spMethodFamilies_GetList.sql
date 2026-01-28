CREATE PROCEDURE [dbo].[spMethodFamilies_GetList]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Id],
		[Name]
	FROM 
		[dbo].[MethodFamilies]
	WHERE
		[Archived] = 0
	ORDER BY
		[Name] ASC;
END
