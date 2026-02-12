CREATE PROCEDURE [dbo].[spSpinStatesElectronicStatesMethodFamilies_GetAll]
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
		[Archived] = 0
	ORDER BY
		[Name] ASC, [Keyword] ASC;
END
