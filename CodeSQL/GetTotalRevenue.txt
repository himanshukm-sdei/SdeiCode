USE [smartHealthAgencyDB]
GO
/****** Object:  StoredProcedure [dbo].[ADM_GetTotalRevenue]    Script Date: 13-05-2025 14:39:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Get total revenue of the organization
-- ADM_GetTotalRevenue 1
-- =============================================
ALTER PROCEDURE [dbo].[ADM_GetTotalRevenue] 
	-- Add the parameters for the stored procedure here
	@OrganizationId int = 0	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Declare @FromDate nvarchar(500) = '01/01/' + CONVERT(VARCHAR(4), DATEPART(yy, getUTCDate())) ,@ToDate nvarchar(500) =  '12/31/' + CONVERT(VARCHAR(4), DATEPART(yy, getUTCDate()))
	Select Sum(CLS.TotalAmount) AS TotalRevenue from Claims CLA inner join ClaimServiceLine CLS on CLA.id=CLS.ClaimID
	Where CLA.DOS between @FromDate AND @ToDate AND CLA.OrganizationID=@OrganizationId AND CLA.IsActive=1 AND CLA.IsDeleted=0

END
