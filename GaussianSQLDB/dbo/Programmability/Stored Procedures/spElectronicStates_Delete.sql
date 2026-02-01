CREATE PROCEDURE [dbo].[spElectronicStates_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE 
		[dbo].[ElectronicStates]
	SET
		[LastUpdatedDate] = GETUTCDATE(),
		[Archived] = 1
	WHERE
		[Id] = @Id;
END
