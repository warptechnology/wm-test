﻿CREATE PROCEDURE [dbo].[GetOrAdd]
	@Name nvarchar(50)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
	BEGIN TRANSACTION
		DECLARE @id AS INT
		SELECT @id = ID FROM [dbo].[FirstTaskModels] WHERE Name=@Name
		IF @id IS NULL
		BEGIN
		   INSERT INTO [dbo].[FirstTaskModels] (Name) VALUES (@Name)
		   SELECT @id = SCOPE_IDENTITY()
		END
	COMMIT TRANSACTION
	RETURN @id
END