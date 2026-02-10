CREATE PROCEDURE [dbo].[spCalculationTypes_Create]
	@Name NVARCHAR(200),
	@Keyword NVARCHAR(50),
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(4000),
	@Id INT = 0 OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [dbo].[CalculationTypes] ([Name], [Keyword], [DescriptionRtf], [DescriptionText])
	VALUES (@Name, @Keyword, @DescriptionRtf, @DescriptionText);
	SELECT @Id = SCOPE_IDENTITY();
END
