CREATE PROCEDURE [dbo].[spElectronicStates_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Id],
		[Name],
		[Keyword],
		[DescriptionRtf],
		[DescriptionText],
		[CreatedDate],
		[LastUpdatedDate],
		[Archived]
	FROM 
		[dbo].[ElectronicStates]
	WHERE
		[Archived] = 0
	ORDER BY
		[Name] ASC;
END
