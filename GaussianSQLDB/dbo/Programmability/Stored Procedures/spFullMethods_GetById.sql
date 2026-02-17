CREATE PROCEDURE [dbo].[spFullMethods_GetById]
	@Id INT
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
		[Id] = @Id AND
		[Archived] = 0
	ORDER BY
		[Keyword] ASC;
END
