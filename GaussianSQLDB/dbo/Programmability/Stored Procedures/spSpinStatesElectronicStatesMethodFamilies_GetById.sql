CREATE PROCEDURE [dbo].[spSpinStatesElectronicStatesMethodFamilies_GetById]
	@Id INT
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
		[Id] = @Id AND
		[Archived] = 0
	ORDER BY
		[Name] ASC, [Keyword] ASC;
END
