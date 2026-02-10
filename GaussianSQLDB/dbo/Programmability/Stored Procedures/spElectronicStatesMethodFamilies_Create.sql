CREATE PROCEDURE [dbo].[spElectronicStatesMethodFamilies_Create]
	@Name NVARCHAR(200),
	@Keyword NVARCHAR(50),
	@ElectronicStateId INT,
	@MethodFamilyId INT,
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(4000),
	@Id INT = 0 OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [dbo].[ElectronicStatesMethodFamilies] ([Name], [Keyword], [ElectronicStateId], [MethodFamilyId], [DescriptionRtf], [DescriptionText])
	VALUES (@Name, @Keyword, @ElectronicStateId, @MethodFamilyId, @DescriptionRtf, @DescriptionText);
	SELECT @Id = SCOPE_IDENTITY();
END
