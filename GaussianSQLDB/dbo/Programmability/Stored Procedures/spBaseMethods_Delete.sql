CREATE PROCEDURE [dbo].[spBaseMethods_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE 
		[dbo].[BaseMethods]
	SET
		[LastUpdatedDate] = GETUTCDATE(),
		[Archived] = 1
	WHERE
		[Id] = @Id;
END
