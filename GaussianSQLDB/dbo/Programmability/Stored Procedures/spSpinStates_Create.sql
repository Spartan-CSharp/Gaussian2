CREATE PROCEDURE [dbo].[spSpinStates_Create]
	@Name NVARCHAR(50),
	@Keyword NVARCHAR(20),
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(2000),
	@Id INT = 0 OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [dbo].[SpinStates] ([Name], [Keyword], [DescriptionRtf], [DescriptionText])
	VALUES (@Name, @Keyword, @DescriptionRtf, @DescriptionText);
	SELECT @Id = SCOPE_IDENTITY();
END
