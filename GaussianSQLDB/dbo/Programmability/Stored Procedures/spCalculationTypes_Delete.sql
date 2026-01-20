CREATE PROCEDURE [dbo].[spCalculationTypes_Delete]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE 
		[dbo].[CalculationTypes]
	SET
		[LastUpdatedDate] = GETUTCDATE(),
		[Archived] = 1
	WHERE
		[Id] = @Id;
END
