USE [uid2_selfserve]
GO

SET IDENTITY_INSERT [ParticipantApi].[ClientTypes] ON;
INSERT INTO [ParticipantApi].[ClientTypes] ([Id], [Name]) VALUES (1, 'DSP')
INSERT INTO [ParticipantApi].[ClientTypes] ([Id], [Name]) VALUES (2, 'PUBLISHER')
INSERT INTO [ParticipantApi].[ClientTypes] ([Id], [Name]) VALUES (3, 'DATA_PROVIDER')
INSERT INTO [ParticipantApi].[ClientTypes] ([Id], [Name]) VALUES (4, 'ADVERTISER')
GO
SET IDENTITY_INSERT [ParticipantApi].[ClientTypes] OFF;


SET IDENTITY_INSERT [ParticipantApi].[Participants] ON;
GO
INSERT INTO [ParticipantApi].[Participants] ([Id], [Name], [Description], [Enabled], [Visible]) VALUES (1, N'TestParticipant', N'Test Participant Description', 1, 1)
SET IDENTITY_INSERT [ParticipantApi].[Participants] OFF;


INSERT INTO [ParticipantApi].[ParticipantClientType] (ParticipantId, ClientTypeId) VALUES (1, 1)
INSERT INTO [ParticipantApi].[ParticipantClientType] (ParticipantId, ClientTypeId) VALUES (1, 2)
INSERT INTO [ParticipantApi].[ParticipantClientType] (ParticipantId, ClientTypeId) VALUES (1, 3)

GO
