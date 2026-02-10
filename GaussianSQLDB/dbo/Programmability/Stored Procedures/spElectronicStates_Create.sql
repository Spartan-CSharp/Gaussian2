CREATE PROCEDURE [dbo].[spElectronicStates_Create]
	@Name NVARCHAR(200),
	@Keyword NVARCHAR(50),
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(4000),
	@Id INT = 0 OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [dbo].[ElectronicStates] ([Name], [Keyword], [DescriptionRtf], [DescriptionText])
	VALUES (@Name, @Keyword, @DescriptionRtf, @DescriptionText);
	SELECT @Id = SCOPE_IDENTITY();
END
