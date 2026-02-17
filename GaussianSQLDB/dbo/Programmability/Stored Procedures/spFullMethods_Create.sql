CREATE PROCEDURE [dbo].[spFullMethods_Create]
	@Keyword NVARCHAR(50),
	@SpinStateElectronicStateMethodFamilyId INT,
	@BaseMethodId INT,
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(4000),
	@Id INT = 0 OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [dbo].[FullMethods] ([Keyword], [SpinStateElectronicStateMethodFamilyId], [BaseMethodId], [DescriptionRtf], [DescriptionText])
	VALUES (@Keyword, @SpinStateElectronicStateMethodFamilyId, @BaseMethodId, @DescriptionRtf, @DescriptionText);
	SELECT @Id = SCOPE_IDENTITY();
END
