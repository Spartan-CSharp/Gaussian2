CREATE PROCEDURE [dbo].[spElectronicStatesMethodFamilies_GetByElectronicStateIdAndMethodFamilyId]
	@ElectronicStateId INT,
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
		[ElectronicStateId] = @ElectronicStateId AND
		[MethodFamilyId] = @MethodFamilyId AND
		[Archived] = 0
	ORDER BY
		[Name] ASC, [Keyword] ASC;
END
