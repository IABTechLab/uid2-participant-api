USE [uid2_selfserve]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE SCHEMA [ParticipantApi];
GO

CREATE TABLE [ParticipantApi].[Sites](
	[Id] [int] NOT NULL,
	[Name] [varchar](200) NULL,
	[Description] [varchar](max) NULL,
	[Enabled] [bit] NOT NULL,
	[Visible] [bit] NOT NULL,
 CONSTRAINT [PK_Sites] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [ParticipantApi].[ClientTypes](
	[Id] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
 CONSTRAINT [PK_ClientTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TABLE [ParticipantApi].[SiteClientType](
	[SiteId] [int] NOT NULL,
	[ClientTypeId] [int] NOT NULL,
 CONSTRAINT [PK_SiteClientType] PRIMARY KEY CLUSTERED 
(
	[SiteId] ASC,
	[ClientTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [ParticipantApi].[SiteClientType]  WITH CHECK ADD  CONSTRAINT [FK_SiteClientType_ClientTypes] FOREIGN KEY([ClientTypeId])
REFERENCES [ParticipantApi].[ClientTypes] ([Id])
GO

ALTER TABLE [ParticipantApi].[SiteClientType] CHECK CONSTRAINT [FK_SiteClientType_ClientTypes]
GO

ALTER TABLE [ParticipantApi].[SiteClientType]  WITH CHECK ADD  CONSTRAINT [FK_SiteClientType_Sites] FOREIGN KEY([SiteId])
REFERENCES [ParticipantApi].[Sites] ([Id])
GO

ALTER TABLE [ParticipantApi].[SiteClientType] CHECK CONSTRAINT [FK_SiteClientType_Sites]
GO

