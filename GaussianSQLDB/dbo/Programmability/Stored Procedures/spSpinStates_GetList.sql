CREATE PROCEDURE [dbo].[spSpinStates_GetList]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Id],
		[Name],
		[Keyword]
	FROM 
		[dbo].[SpinStates]
	WHERE
		[Archived] = 0
	ORDER BY
		[Name] ASC, [Keyword] ASC;
END
