Proc_Insert
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Proc_Insert]
	@UID nvarchar(50)=null ,
	@elmUrlBar varchar(max)=null,
	@elmTitleBar varchar(max)=null,
	@Date_Time datetime=null,
	@Other1 varchar(max)=null,
	@Other2 varchar(max)=null,
	@Other3 varchar(max)=null
AS
BEGIN
	
    -- Insert statements for procedure here
	Insert into table_Chrome(UID,elmUrlBar,elmTitleBar,Date_Time,Other1,Other2,Other3)VALUES(@UID,@elmUrlBar,@elmTitleBar,@Date_Time,@Other1,@Other2,@Other3) 
END