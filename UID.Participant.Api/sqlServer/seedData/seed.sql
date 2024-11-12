USE [uid2_selfserve]
GO

INSERT INTO [ParticipantApi].[ClientTypes] ([Id], [Name]) VALUES (1, "DSP")
INSERT INTO [ParticipantApi].[ClientTypes] ([Id], [Name]) VALUES (2, "PUBLISHER")
INSERT INTO [ParticipantApi].[ClientTypes] ([Id], [Name]) VALUES (3, "DATA_PROVIDER")
INSERT INTO [ParticipantApi].[ClientTypes] ([Id], [Name]) VALUES (4, "ADVERTISER")

INSERT INTO [ParticipantApi].[Sites] ([Id], [Name], [Description], [Enabled], [Visible]) VALUES (1, "TestSite", "Test Site Description", 1, 1)

-- INSERT INTO [ParticipantApi].[SiteClientType] ([SiteId] ,[ClientTypeId]) VALUES ()



GO
