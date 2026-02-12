CREATE PROCEDURE [dbo].[spSpinStatesElectronicStatesMethodFamilies_GetBySpinStateId]
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
		[SpinStateId] = @SpinStateId AND
		[Archived] = 0
	ORDER BY
		[Name] ASC, [Keyword] ASC;
END
