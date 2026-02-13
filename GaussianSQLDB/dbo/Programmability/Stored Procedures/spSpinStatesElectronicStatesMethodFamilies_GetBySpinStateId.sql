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
		ISNULL([SpinStateId],0) = @SpinStateId AND
		[Archived] = 0
	ORDER BY
		[Name] ASC, [Keyword] ASC;
END
