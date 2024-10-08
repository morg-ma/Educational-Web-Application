USE [master]
GO
/****** Object:  Database [EducationalWebAppDB]    Script Date: 20/09/2024 07:13:12 ******/
CREATE DATABASE [EducationalWebAppDB]
GO
ALTER DATABASE [EducationalWebAppDB] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EducationalWebAppDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EducationalWebAppDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EducationalWebAppDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EducationalWebAppDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [EducationalWebAppDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EducationalWebAppDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET RECOVERY FULL 
GO
ALTER DATABASE [EducationalWebAppDB] SET  MULTI_USER 
GO
ALTER DATABASE [EducationalWebAppDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EducationalWebAppDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EducationalWebAppDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EducationalWebAppDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [EducationalWebAppDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'EducationalWebAppDB', N'ON'
GO
ALTER DATABASE [EducationalWebAppDB] SET QUERY_STORE = OFF
GO
USE [EducationalWebAppDB]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [EducationalWebAppDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 20/09/2024 07:13:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseResults]    Script Date: 20/09/2024 07:13:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseResults](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Score] [int] NOT NULL,
	[CourseID] [int] NOT NULL,
	[TraineeID] [int] NOT NULL,
 CONSTRAINT [PK_CourseResults] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Courses]    Script Date: 20/09/2024 07:13:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Courses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Degree] [int] NOT NULL,
	[MinDegree] [int] NOT NULL,
	[Credits] [int] NOT NULL,
	[DepartmentID] [int] NOT NULL,
 CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Departments]    Script Date: 20/09/2024 07:13:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ManagerName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Instructors]    Script Date: 20/09/2024 07:13:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Instructors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ImageURL] [nvarchar](max) NOT NULL,
	[Salary] [float] NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[DepartmentID] [int] NOT NULL,
	[CourseID] [int] NOT NULL,
 CONSTRAINT [PK_Instructors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trainees]    Script Date: 20/09/2024 07:13:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trainees](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ImageURL] [nvarchar](max) NOT NULL,
	[Grade] [int] NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[DepartmentID] [int] NOT NULL,
 CONSTRAINT [PK_Trainees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_CourseResults_CourseID]    Script Date: 20/09/2024 07:13:12 ******/
CREATE NONCLUSTERED INDEX [IX_CourseResults_CourseID] ON [dbo].[CourseResults]
(
	[CourseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CourseResults_TraineeID]    Script Date: 20/09/2024 07:13:12 ******/
CREATE NONCLUSTERED INDEX [IX_CourseResults_TraineeID] ON [dbo].[CourseResults]
(
	[TraineeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Courses_DepartmentID]    Script Date: 20/09/2024 07:13:12 ******/
CREATE NONCLUSTERED INDEX [IX_Courses_DepartmentID] ON [dbo].[Courses]
(
	[DepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Instructors_CourseID]    Script Date: 20/09/2024 07:13:12 ******/
CREATE NONCLUSTERED INDEX [IX_Instructors_CourseID] ON [dbo].[Instructors]
(
	[CourseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Instructors_DepartmentID]    Script Date: 20/09/2024 07:13:12 ******/
CREATE NONCLUSTERED INDEX [IX_Instructors_DepartmentID] ON [dbo].[Instructors]
(
	[DepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Trainees_DepartmentID]    Script Date: 20/09/2024 07:13:12 ******/
CREATE NONCLUSTERED INDEX [IX_Trainees_DepartmentID] ON [dbo].[Trainees]
(
	[DepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CourseResults]  WITH CHECK ADD  CONSTRAINT [FK_CourseResults_Courses_CourseID] FOREIGN KEY([CourseID])
REFERENCES [dbo].[Courses] ([Id])
GO
ALTER TABLE [dbo].[CourseResults] CHECK CONSTRAINT [FK_CourseResults_Courses_CourseID]
GO
ALTER TABLE [dbo].[CourseResults]  WITH CHECK ADD  CONSTRAINT [FK_CourseResults_Trainees_TraineeID] FOREIGN KEY([TraineeID])
REFERENCES [dbo].[Trainees] ([Id])
GO
ALTER TABLE [dbo].[CourseResults] CHECK CONSTRAINT [FK_CourseResults_Trainees_TraineeID]
GO
ALTER TABLE [dbo].[Courses]  WITH CHECK ADD  CONSTRAINT [FK_Courses_Departments_DepartmentID] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Departments] ([Id])
GO
ALTER TABLE [dbo].[Courses] CHECK CONSTRAINT [FK_Courses_Departments_DepartmentID]
GO
ALTER TABLE [dbo].[Instructors]  WITH CHECK ADD  CONSTRAINT [FK_Instructors_Courses_CourseID] FOREIGN KEY([CourseID])
REFERENCES [dbo].[Courses] ([Id])
GO
ALTER TABLE [dbo].[Instructors] CHECK CONSTRAINT [FK_Instructors_Courses_CourseID]
GO
ALTER TABLE [dbo].[Instructors]  WITH CHECK ADD  CONSTRAINT [FK_Instructors_Departments_DepartmentID] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Departments] ([Id])
GO
ALTER TABLE [dbo].[Instructors] CHECK CONSTRAINT [FK_Instructors_Departments_DepartmentID]
GO
ALTER TABLE [dbo].[Trainees]  WITH CHECK ADD  CONSTRAINT [FK_Trainees_Departments_DepartmentID] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Departments] ([Id])
GO
ALTER TABLE [dbo].[Trainees] CHECK CONSTRAINT [FK_Trainees_Departments_DepartmentID]
GO
USE [master]
GO
ALTER DATABASE [EducationalWebAppDB] SET  READ_WRITE 
GO
