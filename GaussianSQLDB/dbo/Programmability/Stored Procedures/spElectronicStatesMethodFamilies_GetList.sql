CREATE PROCEDURE [dbo].[spElectronicStatesMethodFamilies_GetList]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Id],
		[Name],
		[Keyword]
	FROM 
		[dbo].[ElectronicStatesMethodFamilies]
	WHERE
		[Archived] = 0
	ORDER BY
		[Name] ASC, [Keyword] ASC;
END
