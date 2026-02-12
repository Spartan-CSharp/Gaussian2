CREATE PROCEDURE [dbo].[spSpinStatesElectronicStatesMethodFamilies_Create]
	@Name NVARCHAR(200),
	@Keyword NVARCHAR(50),
	@ElectronicStateMethodFamilyId INT,
	@SpinStateId INT,
	@DescriptionRtf NVARCHAR(MAX),
	@DescriptionText NVARCHAR(4000),
	@Id INT = 0 OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [dbo].[SpinStatesElectronicStatesMethodFamilies] ([Name], [Keyword], [ElectronicStateMethodFamilyId], [SpinStateId], [DescriptionRtf], [DescriptionText])
	VALUES (@Name, @Keyword, @ElectronicStateMethodFamilyId, @SpinStateId, @DescriptionRtf, @DescriptionText);
	SELECT @Id = SCOPE_IDENTITY();
END
