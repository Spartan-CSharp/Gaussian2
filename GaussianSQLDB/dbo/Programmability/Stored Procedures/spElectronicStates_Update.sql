CREATE PROCEDURE [dbo].[spElectronicStates_Update]
	@Id INT,
	@Name NVARCHAR(200),
	@Keyword NVARCHAR(50),
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(4000)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE 
		[dbo].[ElectronicStates]
	SET
		[Name] = @Name,
		[Keyword] = @Keyword,
		[DescriptionRtf] = @DescriptionRtf,
		[DescriptionText] = @DescriptionText,
		[LastUpdatedDate] = GETUTCDATE()
	WHERE
		[Id] = @Id;
END
