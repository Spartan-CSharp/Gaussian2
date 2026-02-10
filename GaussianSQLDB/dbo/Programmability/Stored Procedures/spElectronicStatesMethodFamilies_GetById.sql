CREATE PROCEDURE [dbo].[spElectronicStatesMethodFamilies_GetById]
	@Id INT
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
		[Id] = @Id AND
		[Archived] = 0
	ORDER BY
		[Name] ASC, [Keyword] ASC;
END
