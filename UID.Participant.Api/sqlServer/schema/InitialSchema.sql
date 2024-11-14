USE [uid2_selfserve]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE SCHEMA [ParticipantApi];
GO

CREATE TABLE [ParticipantApi].[Participants](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Description] [nvarchar](max) NULL,
	[Enabled] [bit] NOT NULL,
	[Visible] [bit] NOT NULL,
 CONSTRAINT [PK_Participants] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [ParticipantApi].[ClientTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
 CONSTRAINT [PK_ClientTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TABLE [ParticipantApi].[ParticipantClientType](
	[ParticipantId] [int] NOT NULL,
	[ClientTypeId] [int] NOT NULL,
 CONSTRAINT [PK_ParticipantClientType] PRIMARY KEY CLUSTERED 
(
	[ParticipantId] ASC,
	[ClientTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [ParticipantApi].[ParticipantClientType]  WITH CHECK ADD  CONSTRAINT [FK_ParticipantClientType_ClientTypes] FOREIGN KEY([ClientTypeId])
REFERENCES [ParticipantApi].[ClientTypes] ([Id])
GO

ALTER TABLE [ParticipantApi].[ParticipantClientType] CHECK CONSTRAINT [FK_ParticipantClientType_ClientTypes]
GO

ALTER TABLE [ParticipantApi].[ParticipantClientType]  WITH CHECK ADD  CONSTRAINT [FK_ParticipantClientType_Participants] FOREIGN KEY([ParticipantId])
REFERENCES [ParticipantApi].[Participants] ([Id])
GO

ALTER TABLE [ParticipantApi].[ParticipantClientType] CHECK CONSTRAINT [FK_ParticipantClientType_Participants]
GO

