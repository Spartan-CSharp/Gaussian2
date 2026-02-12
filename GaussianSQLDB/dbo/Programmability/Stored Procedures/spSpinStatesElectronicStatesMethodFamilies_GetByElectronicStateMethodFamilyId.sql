CREATE PROCEDURE [dbo].[spSpinStatesElectronicStatesMethodFamilies_GetByElectronicStateMethodFamilyId]
	@ElectronicStateMethodFamilyId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Id],
		[Name],
		[Keyword],
		[ElectronicStateMethodFamilyId],
		[SpinStateId],
		[DescriptionRtf],
		[DescriptionText],
		[CreatedDate],
		[LastUpdatedDate],
		[Archived]
	FROM 
		[dbo].[SpinStatesElectronicStatesMethodFamilies]
	WHERE
		[ElectronicStateMethodFamilyId] = @ElectronicStateMethodFamilyId AND
		[Archived] = 0
	ORDER BY
		[Name] ASC, [Keyword] ASC;
END
