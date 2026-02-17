CREATE PROCEDURE [dbo].[spFullMethods_GetByBaseMethodId]
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
		ISNULL([BaseMethodId],0) = @BaseMethodId AND
		[Archived] = 0
	ORDER BY
		[Keyword] ASC;
END
