CREATE PROCEDURE [dbo].[spElectronicStatesMethodFamilies_GetByElectronicStateId]
	@ElectronicStateId INT
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
		[ElectronicStateId] = @ElectronicStateId AND
		[Archived] = 0
	ORDER BY
		[Name] ASC, [Keyword] ASC;
END
