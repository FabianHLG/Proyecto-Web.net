USE [Restaurante_Web]
GO
/****** Object:  Table [dbo].[Promociones]    Script Date: 11/11/2024 22:58:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Promociones](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RutaId] [int] NOT NULL,
	[Descuento] [decimal](5, 2) NULL,
	[FechaInicio] [datetime2](7) NOT NULL,
	[FechaFin] [datetime2](7) NOT NULL,
	[Descripcion] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Promociones] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reservas]    Script Date: 11/11/2024 22:58:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reservas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RutaId] [int] NOT NULL,
	[AsientoSeleccionado] [nvarchar](max) NOT NULL,
	[PrecioTotal] [int] NOT NULL,
	[FechaReserva] [datetime2](7) NOT NULL,
	[UsuarioId] [int] NOT NULL,
	[EstadoPago] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Reservas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rutas]    Script Date: 11/11/2024 22:58:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rutas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Origen] [nvarchar](50) NOT NULL,
	[Destino] [nvarchar](50) NOT NULL,
	[Horario] [datetime2](7) NOT NULL,
	[Precio] [int] NOT NULL,
	[AsientosDisponibles] [int] NOT NULL,
 CONSTRAINT [PK_Rutas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 11/11/2024 22:58:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Contraseña] [nvarchar](50) NOT NULL,
	[FechaNacimiento] [date] NOT NULL,
	[DireccionExacta] [nvarchar](100) NOT NULL,
	[Telefono] [nvarchar](50) NOT NULL,
	[FechaRegistro] [datetime2](7) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Promociones]  WITH CHECK ADD  CONSTRAINT [FK_Promociones_Rutas] FOREIGN KEY([RutaId])
REFERENCES [dbo].[Rutas] ([Id])
GO
ALTER TABLE [dbo].[Promociones] CHECK CONSTRAINT [FK_Promociones_Rutas]
GO
ALTER TABLE [dbo].[Reservas]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_Rutas] FOREIGN KEY([RutaId])
REFERENCES [dbo].[Rutas] ([Id])
GO
ALTER TABLE [dbo].[Reservas] CHECK CONSTRAINT [FK_Reservas_Rutas]
GO
ALTER TABLE [dbo].[Reservas]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_Usuario] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuario] ([Id])
GO
ALTER TABLE [dbo].[Reservas] CHECK CONSTRAINT [FK_Reservas_Usuario]
GO
