CREATE PROCEDURE [dbo].[spMethodFamilies_GetById]
	@Id int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Id],
		[Name],
		[DescriptionRtf],
		[DescriptionText],
		[CreatedDate],
		[LastUpdatedDate],
		[Archived]
	FROM 
		[dbo].[MethodFamilies]
	WHERE
		[Id] = @Id AND
		[Archived] = 0
	ORDER BY
		[Name] ASC;
END
