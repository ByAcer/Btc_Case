
USE [master]
GO
CREATE DATABASE [Btc]

GO
USE [Btc]
GO

CREATE TABLE [dbo].[InstructionOrder](
	[Id] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[UserId] [uniqueidentifier] NULL,
	[NotificationType] [int] NULL,
	[OrderStatusType] [int] NULL,
	[Amount] [decimal](18, 2) NULL,
	[ActionTime] [datetime] NULL,
 CONSTRAINT [PK_InstructionOrder_] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[OutboxMessage](
	[Id] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[ProcessedDate] [datetime] NULL,
	[NotificationType] [int] NULL,
	[Payload] [nvarchar](max) NULL,
 CONSTRAINT [PK_OutboxMessage_] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[InstructionOrder] ADD  CONSTRAINT [DF_InstructionOrder__CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[OutboxMessage] ADD  CONSTRAINT [DF_OutboxMessage__CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
