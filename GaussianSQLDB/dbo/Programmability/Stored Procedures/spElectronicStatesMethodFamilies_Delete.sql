CREATE PROCEDURE [dbo].[spElectronicStatesMethodFamilies_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE 
		[dbo].[ElectronicStatesMethodFamilies]
	SET
		[LastUpdatedDate] = GETUTCDATE(),
		[Archived] = 1
	WHERE
		[Id] = @Id;
END
