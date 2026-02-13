CREATE PROCEDURE [dbo].[spSpinStatesElectronicStatesMethodFamilies_GetBySpinStateIdAndElectronicStateMethodFamilyId]
	@ElectronicStateMethodFamilyId INT,
	@SpinStateId INT
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
		ISNULL([SpinStateId],0) = @SpinStateId AND
		[Archived] = 0
	ORDER BY
		[Name] ASC, [Keyword] ASC;
END
