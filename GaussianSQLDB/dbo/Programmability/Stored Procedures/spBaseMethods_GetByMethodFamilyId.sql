CREATE PROCEDURE [dbo].[spBaseMethods_GetByMethodFamilyId]
	@MethodFamilyId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Id],
		[Keyword],
		[MethodFamilyId],
		[DescriptionRtf],
		[DescriptionText],
		[CreatedDate],
		[LastUpdatedDate],
		[Archived]
	FROM 
		[dbo].[BaseMethods]
	WHERE
		ISNULL([MethodFamilyId],0) = @MethodFamilyId AND
		[Archived] = 0
	ORDER BY
		[Keyword] ASC;
END
