CREATE PROCEDURE [dbo].[spElectronicStatesMethodFamilies_GetByMethodFamilyId]
	@MethodFamilyId INT
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
		[MethodFamilyId] = @MethodFamilyId AND
		[Archived] = 0
	ORDER BY
		[Name] ASC, [Keyword] ASC;
END
