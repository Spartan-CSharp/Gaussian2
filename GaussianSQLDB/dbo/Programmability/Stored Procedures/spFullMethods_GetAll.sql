CREATE PROCEDURE [dbo].[spFullMethods_GetAll]
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
		[Archived] = 0
	ORDER BY
		[Keyword] ASC;
END
