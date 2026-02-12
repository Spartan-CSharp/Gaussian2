CREATE PROCEDURE [dbo].[spSpinStateslectronicStatesMethodFamilies_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE 
		[dbo].[SpinStatesElectronicStatesMethodFamilies]
	SET
		[LastUpdatedDate] = GETUTCDATE(),
		[Archived] = 1
	WHERE
		[Id] = @Id;
END
