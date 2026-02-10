CREATE PROCEDURE [dbo].[spElectronicStates_GetById]
	@Id INT
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
		[Id] = @Id AND
		[Archived] = 0
	ORDER BY
		[Name] ASC, [Keyword] ASC;
END
