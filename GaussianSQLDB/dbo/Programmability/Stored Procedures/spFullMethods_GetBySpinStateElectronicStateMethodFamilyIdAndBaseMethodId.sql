CREATE PROCEDURE [dbo].[spFullMethods_GetBySpinStateElectronicStateMethodFamilyIdAndBaseMethodId]
	@SpinStateElectronicStateMethodFamilyId INT,
	@BaseMethodId INT
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
		ISNULL([BaseMethodId],0) = @BaseMethodId AND
		[Archived] = 0
	ORDER BY
		[Keyword] ASC;
END
