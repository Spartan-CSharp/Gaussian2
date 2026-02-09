CREATE PROCEDURE [dbo].[spSpinStates_Update]
	@Id INT,
	@Name NVARCHAR(50),
	@Keyword NVARCHAR(20),
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(2000)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE 
		[dbo].[SpinStates]
	SET
		[Name] = @Name,
		[Keyword] = @Keyword,
		[DescriptionRtf] = @DescriptionRtf,
		[DescriptionText] = @DescriptionText,
		[LastUpdatedDate] = GETUTCDATE()
	WHERE
		[Id] = @Id;
END
