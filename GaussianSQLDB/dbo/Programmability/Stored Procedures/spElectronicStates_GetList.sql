CREATE PROCEDURE [dbo].[spElectronicStates_GetList]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Id],
		[Name],
		[Keyword]
	FROM 
		[dbo].[ElectronicStates]
	WHERE
		[Archived] = 0
	ORDER BY
		[Name] ASC, [Keyword] ASC;
END
