CREATE PROCEDURE [dbo].[spElectronicStatesMethodFamilies_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Id],
		[Name],
		[Keyword],
		[ElectronicStateId],
		[MethodFamilyId],
		[DescriptionRtf],
		[DescriptionText],
		[CreatedDate],
		[LastUpdatedDate],
		[Archived]
	FROM 
		[dbo].[ElectronicStatesMethodFamilies]
	WHERE
		[Archived] = 0
	ORDER BY
		[Name] ASC, [Keyword] ASC;
END
