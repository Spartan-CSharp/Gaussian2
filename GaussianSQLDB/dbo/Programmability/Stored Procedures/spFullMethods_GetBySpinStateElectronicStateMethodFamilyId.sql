CREATE PROCEDURE [dbo].[spFullMethods_GetBySpinStateElectronicStateMethodFamilyId]
	@SpinStateElectronicStateMethodFamilyId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Id],
		[Keyword],
		[SpinStateElectronicStateMethodFamilyId],
		[BaseMethodId],
		[DescriptionRtf],
		[DescriptionText],
		[CreatedDate],
		[LastUpdatedDate],
		[Archived]
	FROM 
		[dbo].[FullMethods]
	WHERE
		ISNULL([SpinStateElectronicStateMethodFamilyId],0) = @SpinStateElectronicStateMethodFamilyId AND
		[Archived] = 0
	ORDER BY
		[Keyword] ASC;
END
