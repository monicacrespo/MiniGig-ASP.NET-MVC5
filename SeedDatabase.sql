USE [MiniGigContext]
GO

INSERT INTO [dbo].[MusicGenres]
           ([Category])
     VALUES
           ('Pop'),('Rock'), ('Punk'), ('Jazz'), ('Reggaeton'), ('Flamenco')
GO

INSERT INTO [dbo].[Gigs]
           ([Name]
           ,[GigDate]
           ,[MusicGenreId])
     VALUES
           ('The Chacer Live Gig','2017-11-17',3),
		   ('Ed Sheeran Pop','2017-11-17',1),
		   ('Luis Fonsi & Daddy Yankee','2017-11-17',5),
		   ('Alejando Sanz','2017-11-17',1),
		   ('Adele Concert','2017-11-17',1),
		   ('Shakira Concert','2017-11-17',1),
		   ('Amy Winehouse','2017-11-17',1),
		   ('Lady Gaga','2017-11-17',1)
GO
