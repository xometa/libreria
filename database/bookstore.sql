-- phpMyAdmin SQL Dump
-- version 5.0.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 09-01-2021 a las 00:10:34
-- Versión del servidor: 10.4.11-MariaDB
-- Versión de PHP: 7.4.2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `bookstore`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `abono`
--

CREATE TABLE `abono` (
  `id` int(11) NOT NULL,
  `monto` decimal(10,2) NOT NULL,
  `fechaabono` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `abono`
--

INSERT INTO `abono` (`id`, `monto`, `fechaabono`) VALUES
(1, '3.00', '2020-12-22'),
(2, '3.00', '2020-12-22'),
(3, '0.16', '2020-12-22');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `accion`
--

CREATE TABLE `accion` (
  `id` int(11) NOT NULL,
  `descripcion` varchar(300) COLLATE utf8_spanish_ci NOT NULL,
  `hora` datetime NOT NULL DEFAULT current_timestamp(),
  `idbitacora` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `accion`
--

INSERT INTO `accion` (`id`, `descripcion`, `hora`, `idbitacora`) VALUES
(166, 'inicio sesión como Administrador, la fecha 25/11/2020 a la 11:22:42', '2020-11-25 11:22:42', 146),
(167, 'registro una nueva marca (Oxford), la fecha 25/11/2020 a la 11:29:15', '2020-11-25 11:29:15', 146),
(168, 'registro una nueva marca (Enri), la fecha 25/11/2020 a la 11:29:26', '2020-11-25 11:29:26', 146),
(169, 'registro una nueva marca (Moleskine), la fecha 25/11/2020 a la 11:29:35', '2020-11-25 11:29:35', 146),
(170, 'registro una nueva marca (Leitz), la fecha 25/11/2020 a la 11:29:42', '2020-11-25 11:29:42', 146),
(171, 'registro una nueva marca (Gallery), la fecha 25/11/2020 a la 11:29:57', '2020-11-25 11:29:57', 146),
(172, 'registro una nueva marca (Calipage), la fecha 25/11/2020 a la 11:30:04', '2020-11-25 11:30:04', 146),
(173, 'registro una nueva marca (bic), la fecha 25/11/2020 a la 11:32:43', '2020-11-25 11:32:43', 146),
(174, 'eliminó la información de la marca bic, la fecha 25/11/2020 a la 11:33:04', '2020-11-25 11:33:04', 146),
(175, 'registro una nueva marca (Bic), la fecha 25/11/2020 a la 11:33:09', '2020-11-25 11:33:09', 146),
(176, 'registro una nueva categoría (Lapices), la fecha 25/11/2020 a la 11:34:12', '2020-11-25 11:34:12', 146),
(177, 'registro una nueva categoría (Cuadernos), la fecha 25/11/2020 a la 11:34:18', '2020-11-25 11:34:18', 146),
(178, 'registro una nueva categoría (Sacapuntas), la fecha 25/11/2020 a la 11:34:25', '2020-11-25 11:34:25', 146),
(179, 'registro una nueva categoría (Borradores), la fecha 25/11/2020 a la 11:34:31', '2020-11-25 11:34:31', 146),
(180, 'registro una nueva categoría (Plumones), la fecha 25/11/2020 a la 11:34:38', '2020-11-25 11:34:38', 146),
(181, 'registro una nueva categoría (Colores), la fecha 25/11/2020 a la 11:34:44', '2020-11-25 11:34:44', 146),
(182, 'registro una nueva categoría (Engrapadoras), la fecha 25/11/2020 a la 11:34:53', '2020-11-25 11:34:53', 146),
(183, 'registro una nueva marca (Maped), la fecha 25/11/2020 a la 11:36:47', '2020-11-25 11:36:47', 146),
(184, 'registro una nueva categoría (Marcadores), la fecha 25/11/2020 a la 11:37:13', '2020-11-25 11:37:13', 146),
(185, 'registro una nueva categoría (Resaltadores), la fecha 25/11/2020 a la 11:37:30', '2020-11-25 11:37:30', 146),
(186, 'registro una nueva marca (Stabilo), la fecha 25/11/2020 a la 11:38:54', '2020-11-25 11:38:54', 146),
(187, 'registro un nuevo producto (Stabilo boss), la fecha 25/11/2020', '2020-11-25 11:44:10', 146),
(188, 'registro un nuevo producto (Lapices stabilo point), la fecha 25/11/2020', '2020-11-25 11:49:10', 146),
(189, 'registro un nuevo producto (Pack cuadernos), la fecha 25/11/2020', '2020-11-25 11:54:13', 146),
(190, 'modificó el producto Pack de cuadernos, la fecha 25/11/2020', '2020-11-25 11:54:26', 146),
(191, 'modificó el producto Lapices point, la fecha 25/11/2020', '2020-11-25 11:54:37', 146),
(192, 'registro un nuevo producto (Cuaderno school), la fecha 25/11/2020', '2020-11-25 11:56:37', 146),
(193, 'registro una nueva categoría (Tijeras), la fecha 25/11/2020 a la 11:57:59', '2020-11-25 11:57:59', 146),
(194, 'registro un nuevo producto (Tijera infantil), la fecha 25/11/2020', '2020-11-25 11:58:44', 146),
(195, 'modificó el producto Tijera infantil, la fecha 25/11/2020', '2020-11-25 11:59:18', 146),
(196, 'registro un nuevo cliente (Jorge Alberto García Santos), la fecha 25/11/2020', '2020-11-25 11:59:51', 146),
(197, 'registro un nuevo cliente (Ana Carolina Beltrán Vásquez), la fecha 25/11/2020', '2020-11-25 12:00:17', 146),
(198, 'registro una nueva institución (PAPELCO S.A DE C.V), la fecha 25/11/2020', '2020-11-25 12:05:25', 146),
(199, 'modificó la información de la institución PAPELCO S.A DE C.V, la fecha 25/11/2020', '2020-11-25 12:05:36', 146),
(200, 'registro el número de contacto 7458-9655, la fecha 25/11/2020', '2020-11-25 12:05:44', 146),
(201, 'registro el número de contacto 2222-2555, la fecha 25/11/2020', '2020-11-25 12:05:51', 146),
(202, 'registro un nuevo proveedor (LIBRERÍA CERVANTES S.A DE C.V), la fecha 25/11/2020', '2020-11-25 12:08:25', 146),
(203, 'registro un nuevo proveedor (PAPELERA SALVADOREÑA S.A DE C.V), la fecha 25/11/2020', '2020-11-25 12:10:41', 146),
(204, 'modificó la información del proveedor PAPELERA SALVADOREÑA S.A DE C.V, la fecha 25/11/2020', '2020-11-25 12:10:46', 146),
(205, 'modificó la información del proveedor LIBRERÍA CERVANTES, la fecha 25/11/2020', '2020-11-25 12:10:54', 146),
(206, 'modificó la información del proveedor PAPELERA SALVADOREÑA S.A DE C.V, la fecha 25/11/2020', '2020-11-25 12:11:18', 146),
(207, 'registro el número de contacto 7458-9565, la fecha 25/11/2020', '2020-11-25 12:11:25', 146),
(208, 'registro el número de contacto 2255-4544, la fecha 25/11/2020', '2020-11-25 12:11:29', 146),
(209, 'registro un nuevo cargo (Limpieza), la fecha 25/11/2020 a la 12:14:32', '2020-11-25 12:14:32', 146),
(210, 'registro un nuevo cargo (Comprador), la fecha 25/11/2020 a la 12:14:37', '2020-11-25 12:14:37', 146),
(211, 'registro un nuevo cargo (Secretaria), la fecha 25/11/2020 a la 12:14:54', '2020-11-25 12:14:54', 146),
(212, 'registro el empleado José Saúl Hernández Vásquez, la fecha 25/11/2020', '2020-11-25 12:15:36', 146),
(213, 'creo el usuario jvasquez, la fecha 25/11/2020', '2020-11-25 12:16:46', 146),
(214, 'modificó el estado del usuario jvasquez, la fecha 25/11/2020', '2020-11-25 12:17:04', 146),
(215, 'registro el empleado María Cristina García Ventura, la fecha 25/11/2020', '2020-11-25 12:18:34', 146),
(216, 'cerro sesión, la fecha 25/11/2020 a la 12:19:17', '2020-11-25 12:19:17', 146),
(217, 'inicio sesión como Administrador, la fecha 25/11/2020 a la 12:19:27', '2020-11-24 12:19:27', 147),
(218, 'modificó la información de la marca Oxford, la fecha 25/11/2020 a la 12:19:33', '2020-11-24 12:19:33', 147),
(219, 'modificó el estado de la categoría Lapices, la fecha 25/11/2020', '2020-11-24 12:20:00', 147),
(220, 'cerro sesión, la fecha 25/11/2020 a la 12:21:00', '2020-11-24 12:21:00', 147),
(221, 'inicio sesión como Administrador, la fecha 25/11/2020 a la 12:22:00', '2020-11-25 12:22:00', 148),
(222, 'cerro sesión, la fecha 25/11/2020 a la 12:24:38', '2020-11-25 12:24:38', 148),
(223, 'inicio sesión como Administrador, la fecha 25/11/2020 a la 12:25:27', '2020-11-25 12:25:27', 149),
(224, 'cerro sesión, la fecha 25/11/2020 a la 12:26:41', '2020-11-25 12:26:41', 149),
(225, 'inicio sesión como Administrador, la fecha 25/11/2020 a la 12:27:04', '2020-11-25 12:27:04', 150),
(226, 'cerro sesión, la fecha 25/11/2020 a la 12:28:18', '2020-11-25 12:28:18', 150),
(227, 'inicio sesión como Administrador, la fecha 25/11/2020 a la 12:38:19', '2020-11-25 12:38:19', 151),
(228, 'cerro sesión, la fecha 25/11/2020 a la 12:43:59', '2020-11-25 12:43:59', 151),
(229, 'inicio sesión como Administrador, la fecha 25/11/2020 a la 12:52:17', '2020-11-25 12:52:17', 152),
(230, 'cerro sesión, la fecha 25/11/2020 a la 12:52:52', '2020-11-25 12:52:52', 152),
(231, 'inicio sesión como Administrador, la fecha 25/11/2020 a la 12:53:19', '2020-11-25 12:53:19', 153),
(232, 'modificó el usuario jvasquez, la fecha 25/11/2020', '2020-11-25 13:21:49', 153),
(233, 'cerro sesión, la fecha 25/11/2020 a la 01:22:29', '2020-11-25 13:22:29', 153),
(234, 'modificó la contraseña de acceso, para su usuario jvasquez, la fecha 25/11/2020', '2020-11-25 13:24:15', 154),
(235, 'inicio sesión como Empleado, la fecha 25/11/2020 a la 01:24:15', '2020-11-25 13:24:15', 154),
(236, 'modificó el producto Stabilo marcadores, la fecha 25/11/2020', '2020-11-25 13:25:24', 154),
(237, 'registro un nuevo producto (Borrador de goma), la fecha 25/11/2020', '2020-11-25 13:26:40', 154),
(238, 'eliminó el producto Borrador de goma, la fecha 25/11/2020', '2020-11-25 13:27:16', 154),
(239, 'modificó la información del cliente Jorge Angel García Santos, la fecha 25/11/2020', '2020-11-25 13:28:43', 154),
(240, 'registro un nuevo cliente (José Gerardo Zometa Díaz), la fecha 25/11/2020', '2020-11-25 13:29:37', 154),
(241, 'eliminó el cliente José Gerardo Zometa Díaz, la fecha 25/11/2020', '2020-11-25 13:29:42', 154),
(242, 'registro una nueva institución (DISEÑO S.A DE C.V), la fecha 25/11/2020', '2020-11-25 13:30:48', 154),
(243, 'registro el número de contacto 7458-8888, la fecha 25/11/2020', '2020-11-25 13:31:17', 154),
(244, 'modificó el número de contacto 7458-7888, la fecha 25/11/2020', '2020-11-25 13:31:25', 154),
(245, 'modificó el estado del número de contacto 7458-7888, la fecha 25/11/2020', '2020-11-25 13:31:28', 154),
(246, 'modificó el estado del número de contacto 7458-7888, la fecha 25/11/2020', '2020-11-25 13:31:30', 154),
(247, 'modificó el estado del número de contacto 7458-7888, la fecha 25/11/2020', '2020-11-25 13:31:47', 154),
(248, 'modificó el estado del número de contacto 7458-9655, la fecha 25/11/2020', '2020-11-25 13:31:52', 154),
(249, 'eliminó el número de contacto 2222-2555, la fecha 25/11/2020', '2020-11-25 13:31:56', 154),
(250, 'registro el empleado Jenny Maricela Santos Lopez, la fecha 25/11/2020', '2020-11-25 13:33:25', 154),
(251, 'creo el usuario jmaricela, la fecha 25/11/2020', '2020-11-25 13:34:15', 154),
(252, 'modificó el estado del usuario jmaricela, la fecha 25/11/2020', '2020-11-25 13:34:46', 154),
(253, 'eliminó el usuario jmaricela, la fecha 25/11/2020', '2020-11-25 13:35:57', 154),
(254, 'eliminó el empleado Jenny Maricela Santos Lopez, la fecha 25/11/2020', '2020-11-25 13:35:57', 154),
(255, 'registro un nuevo precio, para un producto; la fecha 25/11/2020', '2020-11-25 13:38:38', 154),
(256, 'modificó el estado del precio del producto Stabilo marcadores, la fecha 25/11/2020', '2020-11-25 13:38:45', 154),
(257, 'modificó el estado del precio del producto Stabilo marcadores, la fecha 25/11/2020', '2020-11-25 13:38:48', 154),
(258, 'eliminó un precio del producto Stabilo marcadores, la fecha 25/11/2020', '2020-11-25 13:38:52', 154),
(259, 'cerro sesión, la fecha 25/11/2020 a la 02:30:47', '2020-11-25 14:30:47', 154),
(260, 'inicio sesión como Administrador, la fecha 01/12/2020 a la 01:35:38', '2020-12-01 13:35:38', 155),
(261, 'modificó el estado de la categoría Tijeras, la fecha 01/12/2020', '2020-12-01 13:38:10', 155),
(262, 'agrego el permiso Inicio, para el usuario José Saúl Hernández Vásquez', '2020-12-01 13:39:05', 155),
(263, 'agrego el permiso Acerca de, para el usuario José Saúl Hernández Vásquez', '2020-12-01 13:39:09', 155),
(264, 'agrego el permiso Proveedores, para el usuario José Saúl Hernández Vásquez', '2020-12-01 13:39:13', 155),
(265, 'eliminó el permiso Inicio del usuario José Saúl Hernández Vásquez', '2020-12-01 13:39:22', 155),
(266, 'eliminó el permiso Acerca de del usuario José Saúl Hernández Vásquez', '2020-12-01 13:39:26', 155),
(267, 'inicio sesión como Administrador, la fecha 01/12/2020 a la 02:17:15', '2020-12-01 14:17:15', 156),
(268, 'inicio sesión como Administrador, la fecha 01/12/2020 a la 03:02:40', '2020-12-01 15:02:40', 157),
(269, 'cerro sesión, la fecha 01/12/2020 a la 03:06:41', '2020-12-01 15:06:41', 157),
(270, 'inicio sesión como Administrador, la fecha 02/12/2020 a la 10:29:34', '2020-12-02 10:29:34', 158),
(271, 'cerro sesión, la fecha 02/12/2020 a la 10:43:01', '2020-12-02 10:43:01', 158),
(272, 'inicio sesión como Administrador, la fecha 02/12/2020 a la 10:54:31', '2020-12-02 10:54:31', 159),
(273, 'cerro sesión, la fecha 02/12/2020 a la 11:48:17', '2020-12-02 11:48:17', 159),
(274, 'inicio sesión como Administrador, la fecha 02/12/2020 a la 08:18:29', '2020-12-02 20:18:29', 160),
(275, 'inicio sesión como Administrador, la fecha 03/12/2020 a la 09:29:27', '2020-12-03 09:29:27', 161),
(276, 'cerro sesión, la fecha 03/12/2020 a la 09:36:32', '2020-12-03 09:36:32', 161),
(277, 'inicio sesión como Administrador, la fecha 03/12/2020 a la 09:37:31', '2020-12-03 09:37:31', 162),
(278, 'cerro sesión, la fecha 03/12/2020 a la 10:00:09', '2020-12-03 10:00:09', 162),
(279, 'inicio sesión como Administrador, la fecha 03/12/2020 a la 10:00:40', '2020-12-03 10:00:40', 163),
(280, 'cerro sesión, la fecha 03/12/2020 a la 10:12:39', '2020-12-03 10:12:39', 163),
(281, 'inicio sesión como Administrador, la fecha 03/12/2020 a la 10:13:13', '2020-12-03 10:13:13', 164),
(282, 'cerro sesión, la fecha 03/12/2020 a la 10:15:01', '2020-12-03 10:15:01', 164),
(283, 'inicio sesión como Administrador, la fecha 03/12/2020 a la 10:16:04', '2020-12-03 10:16:04', 165),
(284, 'registro un nuevo precio, para un producto; la fecha 03/12/2020', '2020-12-03 10:18:15', 165),
(285, 'cerro sesión, la fecha 03/12/2020 a la 10:20:58', '2020-12-03 10:20:58', 165),
(286, 'inicio sesión como Administrador, la fecha 03/12/2020 a la 10:40:25', '2020-12-03 10:40:25', 166),
(287, 'cerro sesión, la fecha 03/12/2020 a la 10:42:30', '2020-12-03 10:42:30', 166),
(288, 'inicio sesión como Administrador, la fecha 03/12/2020 a la 10:43:11', '2020-12-03 10:43:11', 167),
(289, 'registro un nuevo precio, para un producto; la fecha 03/12/2020', '2020-12-03 10:43:47', 167),
(290, 'eliminó un precio del producto Stabilo marcadores, la fecha 03/12/2020', '2020-12-03 10:44:03', 167),
(291, 'registro un nuevo precio, para un producto; la fecha 03/12/2020', '2020-12-03 10:51:46', 167),
(292, 'registro un nuevo precio, para un producto; la fecha 03/12/2020', '2020-12-03 10:51:50', 167),
(293, 'registro un nuevo precio, para un producto; la fecha 03/12/2020', '2020-12-03 10:52:00', 167),
(294, 'inicio sesión como Administrador, la fecha 07/12/2020 a la 03:28:50', '2020-12-07 15:28:50', 168),
(295, 'cerro sesión, la fecha 07/12/2020 a la 04:43:35', '2020-12-07 16:43:35', 168),
(296, 'inicio sesión como Administrador, la fecha 07/12/2020 a la 04:44:37', '2020-12-07 16:44:37', 169),
(297, 'cerro sesión, la fecha 07/12/2020 a la 04:46:47', '2020-12-07 16:46:47', 169),
(298, 'inicio sesión como Administrador, la fecha 07/12/2020 a la 04:47:25', '2020-12-07 16:47:25', 170),
(299, 'cerro sesión, la fecha 07/12/2020 a la 04:53:49', '2020-12-07 16:53:49', 170),
(300, 'inicio sesión como Administrador, la fecha 07/12/2020 a la 04:54:37', '2020-12-07 16:54:37', 171),
(301, 'cerro sesión, la fecha 07/12/2020 a la 05:14:12', '2020-12-07 17:14:12', 171),
(302, 'inicio sesión como Administrador, la fecha 07/12/2020 a la 05:15:09', '2020-12-07 17:15:09', 172),
(303, 'cerro sesión, la fecha 07/12/2020 a la 05:41:57', '2020-12-07 17:41:57', 172),
(304, 'inicio sesión como Administrador, la fecha 07/12/2020 a la 05:42:50', '2020-12-07 17:42:50', 173),
(305, 'inicio sesión como Administrador, la fecha 09/12/2020 a la 05:29:03', '2020-12-09 05:29:03', 174),
(306, 'cerro sesión, la fecha 09/12/2020 a la 05:33:53', '2020-12-09 05:33:53', 174),
(307, 'inicio sesión como Administrador, la fecha 09/12/2020 a la 05:34:23', '2020-12-09 05:34:23', 175),
(308, 'cerro sesión, la fecha 09/12/2020 a la 05:43:27', '2020-12-09 05:43:27', 175),
(309, 'inicio sesión como Administrador, la fecha 09/12/2020 a la 05:44:30', '2020-12-09 05:44:30', 176),
(310, 'modificó el estado del precio del producto Stabilo marcadores, la fecha 09/12/2020', '2020-12-09 05:44:49', 176),
(311, 'modificó el estado del precio del producto Stabilo marcadores, la fecha 09/12/2020', '2020-12-09 05:45:03', 176),
(312, 'modificó el estado del precio del producto Stabilo marcadores, la fecha 09/12/2020', '2020-12-09 05:46:13', 176),
(313, 'modificó el estado del precio del producto Lapices point, la fecha 09/12/2020', '2020-12-09 05:46:20', 176),
(314, 'modificó el estado del precio del producto Stabilo marcadores, la fecha 09/12/2020', '2020-12-09 05:46:45', 176),
(315, 'modificó el estado del precio del producto Lapices point, la fecha 09/12/2020', '2020-12-09 05:46:50', 176),
(316, 'inicio sesión como Administrador, la fecha 10/12/2020 a la 11:35:08', '2020-12-10 11:35:08', 177),
(317, 'cerro sesión, la fecha 10/12/2020 a la 11:36:15', '2020-12-10 11:36:15', 177),
(318, 'inicio sesión como Administrador, la fecha 10/12/2020 a la 11:36:59', '2020-12-10 11:36:59', 178),
(319, 'inicio sesión como Administrador, la fecha 10/12/2020 a la 11:37:50', '2020-12-10 11:37:50', 179),
(320, 'cerro sesión, la fecha 10/12/2020 a la 11:39:20', '2020-12-10 11:39:20', 179),
(321, 'inicio sesión como Administrador, la fecha 10/12/2020 a la 11:39:35', '2020-12-10 11:39:35', 180),
(322, 'registro un nuevo precio, para un producto; la fecha 10/12/2020', '2020-12-10 12:44:29', 180),
(323, 'registro un nuevo precio, para un producto; la fecha 10/12/2020', '2020-12-10 12:44:33', 180),
(324, 'registro un nuevo precio, para un producto; la fecha 10/12/2020', '2020-12-10 12:44:36', 180),
(325, 'modificó el estado del precio del producto Stabilo marcadores, la fecha 10/12/2020', '2020-12-10 12:44:38', 180),
(326, 'modificó el estado del precio del producto Stabilo marcadores, la fecha 10/12/2020', '2020-12-10 12:44:39', 180),
(327, 'modificó el estado del precio del producto Stabilo marcadores, la fecha 10/12/2020', '2020-12-10 12:44:40', 180),
(328, 'eliminó un precio del producto Stabilo marcadores, la fecha 10/12/2020', '2020-12-10 12:59:44', 180),
(329, 'eliminó un precio del producto Stabilo marcadores, la fecha 10/12/2020', '2020-12-10 12:59:46', 180),
(330, 'cerro sesión, la fecha 10/12/2020 a la 01:24:37', '2020-12-10 13:24:37', 180),
(331, 'inicio sesión como Administrador, la fecha 10/12/2020 a la 01:33:53', '2020-12-10 13:33:53', 181),
(332, 'eliminó un precio del producto Lapices point, la fecha 10/12/2020', '2020-12-10 13:36:56', 181),
(333, 'eliminó un precio del producto Lapices point, la fecha 10/12/2020', '2020-12-10 13:36:58', 181),
(334, 'registro un nuevo precio, para un producto; la fecha 10/12/2020', '2020-12-10 13:37:16', 181),
(335, 'modificó el estado del precio del producto Lapices point, la fecha 10/12/2020', '2020-12-10 13:37:27', 181),
(336, 'cerro sesión, la fecha 10/12/2020 a la 01:39:46', '2020-12-10 13:39:46', 181),
(337, 'inicio sesión como Administrador, la fecha 12/12/2020 a la 02:15:47', '2020-12-12 14:15:47', 182),
(338, 'cerro sesión, la fecha 12/12/2020 a la 03:08:22', '2020-12-12 15:08:22', 182),
(339, 'inicio sesión como Administrador, la fecha 12/12/2020 a la 03:11:39', '2020-12-12 15:11:39', 183),
(340, 'inicio sesión como Administrador, la fecha 15/12/2020 a la 10:36:27', '2020-12-15 10:36:27', 184),
(341, 'agrego el permiso Bitácora, para el usuario José Saúl Hernández Vásquez', '2020-12-15 10:59:23', 184),
(342, 'cerro sesión, la fecha 15/12/2020 a la 11:01:26', '2020-12-15 11:01:26', 184),
(343, 'inicio sesión como Administrador, la fecha 15/12/2020 a la 03:21:28', '2020-12-15 15:21:28', 185),
(344, 'cerro sesión, la fecha 15/12/2020 a la 04:07:21', '2020-12-15 16:07:21', 185),
(345, 'inicio sesión como Administrador, la fecha 16/12/2020 a la 05:35:37', '2020-12-16 17:35:37', 186),
(346, 'cerro sesión, la fecha 16/12/2020 a la 06:40:07', '2020-12-16 18:40:07', 186),
(347, 'inicio sesión como Administrador, la fecha 16/12/2020 a la 06:41:46', '2020-12-16 18:41:46', 187),
(348, 'cerro sesión, la fecha 16/12/2020 a la 06:56:41', '2020-12-16 18:56:41', 187),
(349, 'inicio sesión como Administrador, la fecha 16/12/2020 a la 06:57:39', '2020-12-16 18:57:39', 188),
(350, 'modificó el estado de la categoría Cuadernos, la fecha 16/12/2020', '2020-12-16 19:07:02', 188),
(351, 'agrego el permiso Productos, para el usuario José Saúl Hernández Vásquez', '2020-12-16 20:19:45', 188),
(352, 'agrego el permiso Inicio, para el usuario José Saúl Hernández Vásquez', '2020-12-16 20:19:49', 188),
(353, 'eliminó el permiso Bitácora del usuario José Saúl Hernández Vásquez', '2020-12-16 20:19:55', 188),
(354, 'eliminó el permiso Inicio del usuario José Saúl Hernández Vásquez', '2020-12-16 20:19:59', 188),
(355, 'inicio sesión como Administrador, la fecha 16/12/2020 a la 09:48:55', '2020-12-16 21:48:55', 189),
(356, 'inicio sesión como Administrador, la fecha 17/12/2020 a la 09:36:32', '2020-12-17 09:36:32', 190),
(357, 'cerro sesión, la fecha 17/12/2020 a la 09:39:48', '2020-12-17 09:39:48', 190),
(358, 'inicio sesión como Administrador, la fecha 17/12/2020 a la 09:44:22', '2020-12-17 09:44:22', 191),
(359, 'cerro sesión, la fecha 17/12/2020 a la 09:46:22', '2020-12-17 09:46:22', 191),
(360, 'inicio sesión como Administrador, la fecha 17/12/2020 a la 09:47:18', '2020-12-17 09:47:18', 192),
(361, 'inicio sesión como Administrador, la fecha 18/12/2020 a la 08:48:14', '2020-12-18 08:48:14', 193),
(362, 'cerro sesión, la fecha 18/12/2020 a la 08:50:45', '2020-12-18 08:50:45', 193),
(363, 'inicio sesión como Administrador, la fecha 18/12/2020 a la 08:52:09', '2020-12-18 08:52:09', 194),
(364, 'cerro sesión, la fecha 18/12/2020 a la 08:58:10', '2020-12-18 08:58:10', 194),
(365, 'inicio sesión como Administrador, la fecha 18/12/2020 a la 08:58:33', '2020-12-18 08:58:33', 195),
(366, 'inicio sesión como Administrador, la fecha 18/12/2020 a la 09:31:29', '2020-12-18 09:31:29', 196),
(367, 'cerro sesión, la fecha 18/12/2020 a la 09:31:51', '2020-12-18 09:31:51', 196),
(368, 'inicio sesión como Administrador, la fecha 18/12/2020 a la 09:32:33', '2020-12-18 09:32:33', 197),
(369, 'cerro sesión, la fecha 18/12/2020 a la 09:34:11', '2020-12-18 09:34:11', 197),
(370, 'inicio sesión como Administrador, la fecha 18/12/2020 a la 09:34:41', '2020-12-18 09:34:41', 198),
(371, 'registro un nuevo precio, para un producto; la fecha 18/12/2020', '2020-12-18 09:38:24', 198),
(372, 'registro un nuevo precio, para un producto; la fecha 18/12/2020', '2020-12-18 09:38:29', 198),
(373, 'modificó el estado del precio del producto Cuaderno school, la fecha 18/12/2020', '2020-12-18 09:38:30', 198),
(374, 'modificó el estado del precio del producto Pack de cuadernos, la fecha 18/12/2020', '2020-12-18 09:38:33', 198),
(375, 'registro un nuevo precio, para un producto; la fecha 18/12/2020', '2020-12-18 09:38:38', 198),
(376, 'modificó el estado del precio del producto Tijera infantil, la fecha 18/12/2020', '2020-12-18 09:38:39', 198),
(377, 'cerro sesión, la fecha 18/12/2020 a la 09:53:17', '2020-12-18 09:53:17', 198),
(378, 'inicio sesión como Administrador, la fecha 18/12/2020 a la 09:53:43', '2020-12-18 09:53:43', 199),
(379, 'cerro sesión, la fecha 18/12/2020 a la 09:54:38', '2020-12-18 09:54:38', 199),
(380, 'inicio sesión como Administrador, la fecha 18/12/2020 a la 09:55:10', '2020-12-18 09:55:10', 200),
(381, 'agrego el permiso Listado de reportes, para el usuario José Saúl Hernández Vásquez', '2020-12-18 10:19:06', 200),
(382, 'agrego el permiso Compras, para el usuario José Saúl Hernández Vásquez', '2020-12-18 10:19:08', 200),
(383, 'eliminó el permiso Compras del usuario José Saúl Hernández Vásquez', '2020-12-18 10:19:11', 200),
(384, 'eliminó el permiso Productos del usuario José Saúl Hernández Vásquez', '2020-12-18 10:19:12', 200),
(385, 'eliminó el permiso Proveedores del usuario José Saúl Hernández Vásquez', '2020-12-18 10:19:14', 200),
(386, 'cerro sesión, la fecha 18/12/2020 a la 10:37:24', '2020-12-18 10:37:24', 200),
(387, 'inicio sesión como Administrador, la fecha 18/12/2020 a la 10:38:15', '2020-12-18 10:38:15', 201),
(388, 'cerro sesión, la fecha 18/12/2020 a la 10:40:40', '2020-12-18 10:40:40', 201),
(389, 'inicio sesión como Administrador, la fecha 18/12/2020 a la 10:41:40', '2020-12-18 10:41:40', 202),
(390, 'cerro sesión, la fecha 18/12/2020 a la 10:50:15', '2020-12-18 10:50:15', 202),
(391, 'inicio sesión como Administrador, la fecha 19/12/2020 a la 10:49:30', '2020-12-19 10:49:30', 203),
(392, 'inicio sesión como Administrador, la fecha 19/12/2020 a la 11:30:07', '2020-12-19 11:30:07', 204),
(393, 'cerro sesión, la fecha 19/12/2020 a la 11:31:24', '2020-12-19 11:31:24', 204),
(394, 'inicio sesión como Administrador, la fecha 19/12/2020 a la 11:31:44', '2020-12-19 11:31:44', 205),
(395, 'cerro sesión, la fecha 19/12/2020 a la 11:38:39', '2020-12-19 11:38:39', 205),
(396, 'inicio sesión como Administrador, la fecha 19/12/2020 a la 11:39:03', '2020-12-19 11:39:03', 206),
(397, 'cerro sesión, la fecha 19/12/2020 a la 11:40:57', '2020-12-19 11:40:57', 206),
(398, 'inicio sesión como Administrador, la fecha 19/12/2020 a la 11:41:15', '2020-12-19 11:41:15', 207),
(399, 'cerro sesión, la fecha 19/12/2020 a la 12:08:00', '2020-12-19 12:08:00', 207),
(400, 'inicio sesión como Administrador, la fecha 19/12/2020 a la 12:08:24', '2020-12-19 12:08:24', 208),
(401, 'registro una nueva venta, la fecha 19/12/2020 a la 12:08:45', '2020-12-19 12:08:45', 208),
(402, 'cerro sesión, la fecha 19/12/2020 a la 12:22:00', '2020-12-19 12:22:00', 208),
(403, 'inicio sesión como Administrador, la fecha 20/12/2020 a la 03:50:29', '2020-12-20 15:50:29', 209),
(404, 'cerro sesión, la fecha 20/12/2020 a la 03:56:25', '2020-12-20 15:56:25', 209),
(405, 'inicio sesión como Administrador, la fecha 20/12/2020 a la 04:00:15', '2020-12-20 16:00:15', 210),
(406, 'cerro sesión, la fecha 20/12/2020 a la 04:06:22', '2020-12-20 16:06:22', 210),
(407, 'inicio sesión como Administrador, la fecha 20/12/2020 a la 04:12:42', '2020-12-20 16:12:42', 211),
(408, 'cerro sesión, la fecha 20/12/2020 a la 04:14:45', '2020-12-20 16:14:45', 211),
(409, 'inicio sesión como Administrador, la fecha 20/12/2020 a la 04:15:53', '2020-12-20 16:15:53', 212),
(410, 'registro una nueva venta para el cliente Público general, la fecha 20/12/2020 a la 04:16:48', '2020-12-20 16:16:48', 212),
(411, 'inicio sesión como Administrador, la fecha 20/12/2020 a la 04:35:28', '2020-12-20 16:35:28', 213),
(412, 'cerro sesión, la fecha 20/12/2020 a la 04:35:51', '2020-12-20 16:35:51', 213),
(413, 'inicio sesión como Administrador, la fecha 20/12/2020 a la 04:36:43', '2020-12-20 16:36:43', 214),
(414, 'registro una nueva venta para el cliente Público general, la fecha 20/12/2020 a la 04:37:38', '2020-12-20 16:37:38', 214),
(415, 'cerro sesión, la fecha 20/12/2020 a la 04:39:39', '2020-12-20 16:39:39', 214),
(416, 'inicio sesión como Administrador, la fecha 20/12/2020 a la 04:40:47', '2020-12-20 16:40:47', 215),
(417, 'registro una nueva marca (sdgfsdgdsg), la fecha 20/12/2020 a la 04:41:00', '2020-12-20 16:41:00', 215),
(418, 'modificó el estado de la marca sdgfsdgdsg, la fecha 20/12/2020', '2020-12-20 16:41:02', 215),
(419, 'modificó el estado de la marca sdgfsdgdsg, la fecha 20/12/2020', '2020-12-20 16:41:03', 215),
(420, 'eliminó la información de la marca sdgfsdgdsg, la fecha 20/12/2020 a la 04:41:06', '2020-12-20 16:41:06', 215),
(421, 'registro una nueva venta para el cliente Público general, la fecha 20/12/2020 a la 04:41:23', '2020-12-20 16:41:23', 215),
(422, 'registro una nueva venta para el cliente Público general, la fecha 20/12/2020 a la 04:42:47', '2020-12-20 16:42:47', 215),
(423, 'modificó el estado del precio del producto Tijera infantil, la fecha 20/12/2020', '2020-12-20 16:43:41', 215),
(424, 'modificó el estado del precio del producto Cuaderno school, la fecha 20/12/2020', '2020-12-20 16:43:43', 215),
(425, 'modificó el estado del precio del producto Pack de cuadernos, la fecha 20/12/2020', '2020-12-20 16:43:45', 215),
(426, 'modificó el estado del precio del producto Lapices point, la fecha 20/12/2020', '2020-12-20 16:43:56', 215),
(427, 'modificó el estado del precio del producto Stabilo marcadores, la fecha 20/12/2020', '2020-12-20 16:43:58', 215),
(428, 'modificó el estado del precio del producto Stabilo marcadores, la fecha 20/12/2020', '2020-12-20 16:43:59', 215),
(429, 'modificó el estado del precio del producto Stabilo marcadores, la fecha 20/12/2020', '2020-12-20 16:43:59', 215),
(430, 'cerro sesión, la fecha 20/12/2020 a la 04:44:34', '2020-12-20 16:44:34', 215),
(431, 'inicio sesión como Administrador, la fecha 20/12/2020 a la 04:46:33', '2020-12-20 16:46:33', 216),
(432, 'cerro sesión, la fecha 20/12/2020 a la 04:49:00', '2020-12-20 16:49:00', 216),
(433, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 08:52:30', '2020-12-21 08:52:30', 217),
(434, 'modificó el estado del precio del producto Stabilo marcadores, la fecha 21/12/2020', '2020-12-21 08:52:46', 217),
(435, 'modificó el estado del precio del producto Lapices point, la fecha 21/12/2020', '2020-12-21 08:52:49', 217),
(436, 'modificó el estado del precio del producto Pack de cuadernos, la fecha 21/12/2020', '2020-12-21 08:52:51', 217),
(437, 'modificó el estado del precio del producto Stabilo marcadores, la fecha 21/12/2020', '2020-12-21 08:59:18', 217),
(438, 'modificó el estado del precio del producto Stabilo marcadores, la fecha 21/12/2020', '2020-12-21 08:59:19', 217),
(439, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 09:59:15', '2020-12-21 09:59:15', 218),
(440, 'cerro sesión, la fecha 21/12/2020 a la 10:17:09', '2020-12-21 10:17:09', 218),
(441, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 10:17:39', '2020-12-21 10:17:39', 219),
(442, 'cerro sesión, la fecha 21/12/2020 a la 10:20:19', '2020-12-21 10:20:19', 219),
(443, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 10:20:35', '2020-12-21 10:20:35', 220),
(444, 'registro una nueva venta para el cliente María Reyna Sánchez Díaz (Cliente institucional), la fecha 21/12/2020 a la 10:21:10', '2020-12-21 10:21:10', 220),
(445, 'cerro sesión, la fecha 21/12/2020 a la 10:51:17', '2020-12-21 10:51:17', 220),
(446, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 10:52:04', '2020-12-21 10:52:04', 221),
(447, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 10:54:05', '2020-12-21 10:54:05', 222),
(448, 'cerro sesión, la fecha 21/12/2020 a la 10:56:12', '2020-12-21 10:56:12', 222),
(449, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 10:56:29', '2020-12-21 10:56:29', 223),
(450, 'registro una nueva venta para el cliente Público general, la fecha 21/12/2020 a la 11:00:21', '2020-12-21 11:00:21', 223),
(451, 'cerro sesión, la fecha 21/12/2020 a la 11:01:14', '2020-12-21 11:01:14', 223),
(452, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 11:02:54', '2020-12-21 11:02:54', 224),
(453, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 11:08:44', '2020-12-21 11:08:44', 225),
(454, 'cerro sesión, la fecha 21/12/2020 a la 11:12:06', '2020-12-21 11:12:06', 225),
(455, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 11:12:26', '2020-12-21 11:12:26', 226),
(456, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 11:16:29', '2020-12-21 11:16:29', 227),
(457, 'cerro sesión, la fecha 21/12/2020 a la 11:22:38', '2020-12-21 11:22:38', 227),
(458, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 11:22:54', '2020-12-21 11:22:54', 228),
(459, 'cerro sesión, la fecha 21/12/2020 a la 11:24:05', '2020-12-21 11:24:05', 228),
(460, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 11:24:24', '2020-12-21 11:24:24', 229),
(461, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 11:27:11', '2020-12-21 11:27:11', 230),
(462, 'registro una nueva venta para el cliente María Reyna Sánchez Díaz (Cliente institucional), la fecha 21/12/2020 a la 11:29:01', '2020-12-21 11:29:01', 230),
(463, 'registro una nueva venta para el cliente María Reyna Sánchez Díaz (Cliente institucional), la fecha 21/12/2020 a la 11:32:58', '2020-12-21 11:32:58', 230),
(464, 'cerro sesión, la fecha 21/12/2020 a la 11:44:57', '2020-12-21 11:44:57', 230),
(465, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 11:48:09', '2020-12-21 11:48:09', 231),
(466, 'cerro sesión, la fecha 21/12/2020 a la 11:52:01', '2020-12-21 11:52:01', 231),
(467, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 11:52:20', '2020-12-21 11:52:20', 232),
(468, 'cerro sesión, la fecha 21/12/2020 a la 11:55:48', '2020-12-21 11:55:48', 232),
(469, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 11:56:06', '2020-12-21 11:56:06', 233),
(470, 'registro una nueva venta para el cliente Jorge Angel García Santos, la fecha 21/12/2020 a la 11:57:33', '2020-12-21 11:57:33', 233),
(471, 'cerro sesión, la fecha 21/12/2020 a la 12:09:58', '2020-12-21 12:09:58', 233),
(472, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 12:10:12', '2020-12-21 12:10:12', 234),
(473, 'cerro sesión, la fecha 21/12/2020 a la 12:10:21', '2020-12-21 12:10:21', 234),
(474, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 12:12:17', '2020-12-21 12:12:17', 235),
(475, 'cerro sesión, la fecha 21/12/2020 a la 12:13:57', '2020-12-21 12:13:57', 235),
(476, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 12:14:24', '2020-12-21 12:14:24', 236),
(477, 'cerro sesión, la fecha 21/12/2020 a la 12:17:35', '2020-12-21 12:17:35', 236),
(478, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 12:18:07', '2020-12-21 12:18:07', 237),
(479, 'cerro sesión, la fecha 21/12/2020 a la 12:20:20', '2020-12-21 12:20:20', 237),
(480, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 12:22:04', '2020-12-21 12:22:04', 238),
(481, 'cerro sesión, la fecha 21/12/2020 a la 12:32:57', '2020-12-21 12:32:57', 238),
(482, 'inicio sesión como Administrador, la fecha 21/12/2020 a la 12:34:47', '2020-12-21 12:34:47', 239),
(483, 'registro una nueva venta para el cliente Público general, la fecha 21/12/2020 a la 12:41:56', '2020-12-21 12:41:56', 239),
(484, 'inicio sesión como Administrador, la fecha 22/12/2020 a la 04:03:52', '2020-12-22 16:03:52', 240),
(485, 'cerro sesión, la fecha 22/12/2020 a la 04:08:49', '2020-12-22 16:08:49', 240),
(486, 'inicio sesión como Administrador, la fecha 22/12/2020 a la 04:09:19', '2020-12-22 16:09:19', 241),
(487, 'modificó el estado del producto Stabilo marcadores, la fecha 22/12/2020', '2020-12-22 16:09:32', 241),
(488, 'registro una nueva venta para el cliente María Reyna Sánchez Díaz (Cliente institucional), la fecha 22/12/2020 a la 04:15:06', '2020-12-22 16:15:06', 241),
(489, 'cerro sesión, la fecha 22/12/2020 a la 04:30:49', '2020-12-22 16:30:49', 241),
(490, 'inicio sesión como Administrador, la fecha 22/12/2020 a la 04:34:24', '2020-12-22 16:34:24', 242),
(491, 'cerro sesión, la fecha 22/12/2020 a la 05:04:56', '2020-12-22 17:04:56', 242),
(492, 'inicio sesión como Administrador, la fecha 22/12/2020 a la 05:05:21', '2020-12-22 17:05:21', 243),
(493, 'cerro sesión, la fecha 22/12/2020 a la 05:07:38', '2020-12-22 17:07:38', 243),
(494, 'inicio sesión como Administrador, la fecha 22/12/2020 a la 05:08:07', '2020-12-22 17:08:07', 244),
(495, 'cerro sesión, la fecha 22/12/2020 a la 05:15:34', '2020-12-22 17:15:34', 244),
(496, 'inicio sesión como Administrador, la fecha 22/12/2020 a la 05:16:11', '2020-12-22 17:16:11', 245),
(497, 'inicio sesión como Administrador, la fecha 22/12/2020 a la 05:21:18', '2020-12-22 17:21:18', 246),
(498, 'cerro sesión, la fecha 22/12/2020 a la 05:27:44', '2020-12-22 17:27:44', 246),
(499, 'inicio sesión como Administrador, la fecha 22/12/2020 a la 05:28:07', '2020-12-22 17:28:07', 247),
(500, 'cerro sesión, la fecha 22/12/2020 a la 05:32:43', '2020-12-22 17:32:43', 247),
(501, 'inicio sesión como Administrador, la fecha 22/12/2020 a la 05:33:27', '2020-12-22 17:33:27', 248),
(502, 'cerro sesión, la fecha 22/12/2020 a la 05:34:02', '2020-12-22 17:34:02', 248),
(503, 'inicio sesión como Administrador, la fecha 22/12/2020 a la 05:34:21', '2020-12-22 17:34:21', 249),
(504, 'cerro sesión, la fecha 22/12/2020 a la 05:44:00', '2020-12-22 17:44:00', 249),
(505, 'inicio sesión como Administrador, la fecha 22/12/2020 a la 05:44:33', '2020-12-22 17:44:33', 250),
(506, 'cerro sesión, la fecha 22/12/2020 a la 05:53:59', '2020-12-22 17:53:59', 250),
(507, 'inicio sesión como Administrador, la fecha 22/12/2020 a la 05:54:40', '2020-12-22 17:54:40', 251),
(508, 'registro una nueva venta para el cliente Kevin Alexis Jimenez Ruiz (Cliente institucional), la fecha 22/12/2020 a la 06:04:15', '2020-12-22 18:04:15', 251),
(509, 'inicio sesión como Administrador, la fecha 30/12/2020 a la 01:56:03', '2020-12-30 13:56:03', 252),
(510, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 11:19:48', '2021-01-05 11:19:48', 253),
(511, 'agrego el permiso Respaldo de datos, para el usuario José Saúl Hernández Vásquez', '2021-01-05 11:22:56', 253),
(512, 'agrego el permiso Productos, para el usuario José Saúl Hernández Vásquez', '2021-01-05 11:22:59', 253),
(513, 'agrego el permiso Acerca de, para el usuario José Saúl Hernández Vásquez', '2021-01-05 11:23:03', 253),
(514, 'eliminó el permiso Respaldo de datos del usuario José Saúl Hernández Vásquez', '2021-01-05 11:23:17', 253),
(515, 'eliminó el permiso Productos del usuario José Saúl Hernández Vásquez', '2021-01-05 11:23:18', 253),
(516, 'eliminó el permiso Listado de reportes del usuario José Saúl Hernández Vásquez', '2021-01-05 11:23:22', 253),
(517, 'eliminó el permiso Acerca de del usuario José Saúl Hernández Vásquez', '2021-01-05 11:23:24', 253),
(518, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 11:30:50', '2021-01-05 11:30:50', 254),
(519, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 11:37:06', '2021-01-05 11:37:06', 255),
(520, 'cerro sesión, la fecha 05/01/2021 a la 11:38:09', '2021-01-05 11:38:09', 255),
(521, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 11:38:35', '2021-01-05 11:38:35', 256),
(522, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 11:55:46', '2021-01-05 11:55:46', 257),
(523, 'cerro sesión, la fecha 05/01/2021 a la 11:59:38', '2021-01-05 11:59:38', 257),
(524, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 11:59:59', '2021-01-05 11:59:59', 258),
(525, 'cerro sesión, la fecha 05/01/2021 a la 12:05:54', '2021-01-05 12:05:54', 258),
(526, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 12:06:11', '2021-01-05 12:06:11', 259),
(527, 'cerro sesión, la fecha 05/01/2021 a la 12:08:46', '2021-01-05 12:08:46', 259),
(528, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 12:09:15', '2021-01-05 12:09:15', 260),
(529, 'registro un nuevo producto (er), la fecha 05/01/2021', '2021-01-05 12:12:54', 260),
(530, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 01:01:53', '2021-01-05 13:01:53', 261),
(531, 'cerro sesión, la fecha 05/01/2021 a la 01:22:36', '2021-01-05 13:22:36', 261),
(532, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 01:23:07', '2021-01-05 13:23:07', 262),
(533, 'registro una nueva venta para el cliente Público general, la fecha 05/01/2021 a la 01:23:35', '2021-01-05 13:23:35', 262),
(534, 'cerro sesión, la fecha 05/01/2021 a la 01:49:40', '2021-01-05 13:49:40', 262),
(535, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 01:50:08', '2021-01-05 13:50:08', 263),
(536, 'cerro sesión, la fecha 05/01/2021 a la 01:52:07', '2021-01-05 13:52:07', 263),
(537, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 01:52:28', '2021-01-05 13:52:28', 264),
(538, 'cerro sesión, la fecha 05/01/2021 a la 01:53:21', '2021-01-05 13:53:21', 264),
(539, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 01:55:54', '2021-01-05 13:55:54', 265),
(540, 'cerro sesión, la fecha 05/01/2021 a la 01:58:24', '2021-01-05 13:58:24', 265),
(541, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 02:04:53', '2021-01-05 14:04:53', 266),
(542, 'cerro sesión, la fecha 05/01/2021 a la 02:12:07', '2021-01-05 14:12:07', 266),
(543, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 02:12:40', '2021-01-05 14:12:40', 267),
(544, 'cerro sesión, la fecha 05/01/2021 a la 02:17:01', '2021-01-05 14:17:01', 267),
(545, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 02:19:08', '2021-01-05 14:19:08', 268),
(546, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 02:36:34', '2021-01-05 14:36:34', 269),
(547, 'cerro sesión, la fecha 05/01/2021 a la 02:39:45', '2021-01-05 14:39:45', 269),
(548, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 02:40:09', '2021-01-05 14:40:09', 270),
(549, 'cerro sesión, la fecha 05/01/2021 a la 02:41:42', '2021-01-05 14:41:42', 270),
(550, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 02:46:06', '2021-01-05 14:46:06', 271),
(551, 'cerro sesión, la fecha 05/01/2021 a la 02:49:55', '2021-01-05 14:49:55', 271),
(552, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 02:52:01', '2021-01-05 14:52:01', 272),
(553, 'cerro sesión, la fecha 05/01/2021 a la 02:53:50', '2021-01-05 14:53:50', 272),
(554, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 02:54:09', '2021-01-05 14:54:09', 273),
(555, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 02:55:18', '2021-01-05 14:55:18', 274),
(556, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 02:59:28', '2021-01-05 14:59:28', 275),
(557, 'cerro sesión, la fecha 05/01/2021 a la 03:12:44', '2021-01-05 15:12:44', 275),
(558, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 03:15:50', '2021-01-05 15:15:50', 276),
(559, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 03:18:20', '2021-01-05 15:18:20', 277),
(560, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 03:22:22', '2021-01-05 15:22:22', 278),
(561, 'cerro sesión, la fecha 05/01/2021 a la 03:24:47', '2021-01-05 15:24:47', 278),
(562, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 03:25:04', '2021-01-05 15:25:04', 279),
(563, 'cerro sesión, la fecha 05/01/2021 a la 03:25:41', '2021-01-05 15:25:41', 279),
(564, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 03:26:11', '2021-01-05 15:26:11', 280),
(565, 'cerro sesión, la fecha 05/01/2021 a la 03:28:26', '2021-01-05 15:28:26', 280),
(566, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 03:29:16', '2021-01-05 15:29:16', 281),
(567, 'cerro sesión, la fecha 05/01/2021 a la 03:31:26', '2021-01-05 15:31:26', 281),
(568, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 03:32:46', '2021-01-05 15:32:46', 282),
(569, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 03:34:28', '2021-01-05 15:34:28', 283),
(570, 'cerro sesión, la fecha 05/01/2021 a la 03:34:44', '2021-01-05 15:34:44', 283),
(571, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 03:36:34', '2021-01-05 15:36:34', 284),
(572, 'cerro sesión, la fecha 05/01/2021 a la 03:38:55', '2021-01-05 15:38:55', 284),
(573, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 03:39:06', '2021-01-05 15:39:06', 285),
(574, 'cerro sesión, la fecha 05/01/2021 a la 03:39:32', '2021-01-05 15:39:32', 285),
(575, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 03:39:48', '2021-01-05 15:39:48', 286),
(576, 'cerro sesión, la fecha 05/01/2021 a la 03:40:28', '2021-01-05 15:40:28', 286),
(577, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 03:40:52', '2021-01-05 15:40:52', 287),
(578, 'cerro sesión, la fecha 05/01/2021 a la 03:43:08', '2021-01-05 15:43:08', 287),
(579, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 03:43:56', '2021-01-05 15:43:56', 288),
(580, 'cerro sesión, la fecha 05/01/2021 a la 03:45:59', '2021-01-05 15:45:59', 288),
(581, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 03:48:04', '2021-01-05 15:48:04', 289),
(582, 'cerro sesión, la fecha 05/01/2021 a la 03:48:21', '2021-01-05 15:48:21', 289),
(583, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 03:49:08', '2021-01-05 15:49:08', 290),
(584, 'cerro sesión, la fecha 05/01/2021 a la 03:53:44', '2021-01-05 15:53:44', 290),
(585, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 03:54:33', '2021-01-05 15:54:33', 291),
(586, 'cerro sesión, la fecha 05/01/2021 a la 03:58:07', '2021-01-05 15:58:07', 291),
(587, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 04:00:30', '2021-01-05 16:00:30', 292),
(588, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 04:02:25', '2021-01-05 16:02:25', 293),
(589, 'cerro sesión, la fecha 05/01/2021 a la 04:04:11', '2021-01-05 16:04:11', 293),
(590, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 04:04:33', '2021-01-05 16:04:33', 294),
(591, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 04:08:52', '2021-01-05 16:08:52', 295),
(592, 'cerro sesión, la fecha 05/01/2021 a la 04:11:11', '2021-01-05 16:11:11', 295),
(593, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 04:11:52', '2021-01-05 16:11:52', 296),
(594, 'cerro sesión, la fecha 05/01/2021 a la 04:12:08', '2021-01-05 16:12:08', 296),
(595, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 04:12:38', '2021-01-05 16:12:38', 297),
(596, 'cerro sesión, la fecha 05/01/2021 a la 04:14:46', '2021-01-05 16:14:46', 297),
(597, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 04:15:06', '2021-01-05 16:15:06', 298),
(598, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 04:18:02', '2021-01-05 16:18:02', 299),
(599, 'cerro sesión, la fecha 05/01/2021 a la 04:19:30', '2021-01-05 16:19:30', 299),
(600, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 04:19:47', '2021-01-05 16:19:47', 300),
(601, 'cerro sesión, la fecha 05/01/2021 a la 04:25:07', '2021-01-05 16:25:07', 300),
(602, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 04:25:28', '2021-01-05 16:25:28', 301),
(603, 'cerro sesión, la fecha 05/01/2021 a la 04:26:38', '2021-01-05 16:26:38', 301),
(604, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 04:27:22', '2021-01-05 16:27:22', 302),
(605, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 04:30:26', '2021-01-05 16:30:26', 303),
(606, 'cerro sesión, la fecha 05/01/2021 a la 04:34:30', '2021-01-05 16:34:30', 303),
(607, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 04:34:48', '2021-01-05 16:34:48', 304),
(608, 'cerro sesión, la fecha 05/01/2021 a la 04:38:44', '2021-01-05 16:38:44', 304),
(609, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 04:39:02', '2021-01-05 16:39:02', 305),
(610, 'cerro sesión, la fecha 05/01/2021 a la 04:47:43', '2021-01-05 16:47:43', 305),
(611, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 04:48:00', '2021-01-05 16:48:00', 306),
(612, 'cerro sesión, la fecha 05/01/2021 a la 04:49:07', '2021-01-05 16:49:07', 306),
(613, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 04:49:23', '2021-01-05 16:49:23', 307),
(614, 'cerro sesión, la fecha 05/01/2021 a la 04:59:01', '2021-01-05 16:59:01', 307),
(615, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 04:59:53', '2021-01-05 16:59:53', 308),
(616, 'cerro sesión, la fecha 05/01/2021 a la 05:00:26', '2021-01-05 17:00:26', 308),
(617, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 05:00:52', '2021-01-05 17:00:52', 309),
(618, 'cerro sesión, la fecha 05/01/2021 a la 05:01:28', '2021-01-05 17:01:28', 309),
(619, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 05:01:43', '2021-01-05 17:01:43', 310),
(620, 'cerro sesión, la fecha 05/01/2021 a la 05:02:07', '2021-01-05 17:02:07', 310),
(621, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 05:02:22', '2021-01-05 17:02:22', 311),
(622, 'registro una nueva venta para el cliente Público general, la fecha 05/01/2021 a la 05:09:22', '2021-01-05 17:09:22', 311),
(623, 'cerro sesión, la fecha 05/01/2021 a la 05:25:50', '2021-01-05 17:25:50', 311),
(624, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 05:26:20', '2021-01-05 17:26:20', 312),
(625, 'cerro sesión, la fecha 05/01/2021 a la 05:27:08', '2021-01-05 17:27:08', 312),
(626, 'inicio sesión como Administrador, la fecha 05/01/2021 a la 05:27:24', '2021-01-05 17:27:24', 313),
(627, 'cerro sesión, la fecha 05/01/2021 a la 05:28:18', '2021-01-05 17:28:18', 313),
(628, 'inicio sesión como Administrador, la fecha 07/01/2021 a la 03:03:47', '2021-01-07 15:03:47', 314),
(629, 'cerro sesión, la fecha 07/01/2021 a la 03:07:52', '2021-01-07 15:07:52', 314),
(630, 'inicio sesión como Administrador, la fecha 07/01/2021 a la 03:08:59', '2021-01-07 15:08:59', 315),
(631, 'cerro sesión, la fecha 07/01/2021 a la 03:15:50', '2021-01-07 15:15:50', 315),
(632, 'inicio sesión como Administrador, la fecha 07/01/2021 a la 03:18:23', '2021-01-07 15:18:23', 316),
(633, 'cerro sesión, la fecha 07/01/2021 a la 03:21:01', '2021-01-07 15:21:01', 316),
(634, 'inicio sesión como Administrador, la fecha 07/01/2021 a la 03:28:11', '2021-01-07 15:28:11', 317),
(635, 'cerro sesión, la fecha 07/01/2021 a la 03:37:39', '2021-01-07 15:37:39', 317),
(636, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 08:19:31', '2021-01-08 08:19:31', 318),
(637, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 09:28:51', '2021-01-08 09:28:51', 319),
(638, 'cerro sesión, la fecha 08/01/2021 a la 09:39:38', '2021-01-08 09:39:38', 319),
(639, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 09:40:41', '2021-01-08 09:40:41', 320),
(640, 'cerro sesión, la fecha 08/01/2021 a la 09:51:21', '2021-01-08 09:51:21', 320),
(641, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 09:51:41', '2021-01-08 09:51:41', 321),
(642, 'cerro sesión, la fecha 08/01/2021 a la 09:54:28', '2021-01-08 09:54:28', 321),
(643, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 09:55:03', '2021-01-08 09:55:03', 322),
(644, 'cerro sesión, la fecha 08/01/2021 a la 09:58:05', '2021-01-08 09:58:05', 322),
(645, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 09:58:36', '2021-01-08 09:58:36', 323),
(646, 'cerro sesión, la fecha 08/01/2021 a la 10:12:31', '2021-01-08 10:12:31', 323),
(647, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 10:12:53', '2021-01-08 10:12:53', 324),
(648, 'cerro sesión, la fecha 08/01/2021 a la 10:19:26', '2021-01-08 10:19:26', 324),
(649, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 10:19:50', '2021-01-08 10:19:50', 325),
(650, 'cerro sesión, la fecha 08/01/2021 a la 10:47:49', '2021-01-08 10:47:49', 325),
(651, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 10:48:37', '2021-01-08 10:48:37', 326),
(652, 'cerro sesión, la fecha 08/01/2021 a la 10:50:14', '2021-01-08 10:50:14', 326),
(653, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 10:50:27', '2021-01-08 10:50:27', 327),
(654, 'cerro sesión, la fecha 08/01/2021 a la 10:55:14', '2021-01-08 10:55:14', 327),
(655, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 10:56:00', '2021-01-08 10:56:00', 328),
(656, 'cerro sesión, la fecha 08/01/2021 a la 10:58:16', '2021-01-08 10:58:16', 328),
(657, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 10:58:36', '2021-01-08 10:58:36', 329),
(658, 'cerro sesión, la fecha 08/01/2021 a la 10:59:29', '2021-01-08 10:59:29', 329),
(659, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 10:59:58', '2021-01-08 10:59:58', 330),
(660, 'cerro sesión, la fecha 08/01/2021 a la 11:01:04', '2021-01-08 11:01:04', 330),
(661, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:02:52', '2021-01-08 11:02:52', 331),
(662, 'cerro sesión, la fecha 08/01/2021 a la 11:04:24', '2021-01-08 11:04:24', 331),
(663, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:05:53', '2021-01-08 11:05:53', 332),
(664, 'cerro sesión, la fecha 08/01/2021 a la 11:07:24', '2021-01-08 11:07:24', 332);
INSERT INTO `accion` (`id`, `descripcion`, `hora`, `idbitacora`) VALUES
(665, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:08:43', '2021-01-08 11:08:43', 333),
(666, 'cerro sesión, la fecha 08/01/2021 a la 11:11:31', '2021-01-08 11:11:31', 333),
(667, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:11:56', '2021-01-08 11:11:56', 334),
(668, 'cerro sesión, la fecha 08/01/2021 a la 11:13:08', '2021-01-08 11:13:08', 334),
(669, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:14:33', '2021-01-08 11:14:33', 335),
(670, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:17:04', '2021-01-08 11:17:04', 336),
(671, 'cerro sesión, la fecha 08/01/2021 a la 11:18:40', '2021-01-08 11:18:40', 336),
(672, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:19:04', '2021-01-08 11:19:04', 337),
(673, 'cerro sesión, la fecha 08/01/2021 a la 11:21:11', '2021-01-08 11:21:11', 337),
(674, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:21:46', '2021-01-08 11:21:46', 338),
(675, 'cerro sesión, la fecha 08/01/2021 a la 11:23:45', '2021-01-08 11:23:45', 338),
(676, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:24:08', '2021-01-08 11:24:08', 339),
(677, 'cerro sesión, la fecha 08/01/2021 a la 11:25:28', '2021-01-08 11:25:28', 339),
(678, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:26:32', '2021-01-08 11:26:32', 340),
(679, 'cerro sesión, la fecha 08/01/2021 a la 11:28:05', '2021-01-08 11:28:05', 340),
(680, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:28:32', '2021-01-08 11:28:32', 341),
(681, 'cerro sesión, la fecha 08/01/2021 a la 11:29:48', '2021-01-08 11:29:48', 341),
(682, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:31:15', '2021-01-08 11:31:15', 342),
(683, 'cerro sesión, la fecha 08/01/2021 a la 11:32:01', '2021-01-08 11:32:01', 342),
(684, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:35:09', '2021-01-08 11:35:09', 343),
(685, 'cerro sesión, la fecha 08/01/2021 a la 11:35:57', '2021-01-08 11:35:57', 343),
(686, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:37:03', '2021-01-08 11:37:03', 344),
(687, 'cerro sesión, la fecha 08/01/2021 a la 11:39:12', '2021-01-08 11:39:12', 344),
(688, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:40:16', '2021-01-08 11:40:16', 345),
(689, 'cerro sesión, la fecha 08/01/2021 a la 11:41:43', '2021-01-08 11:41:43', 345),
(690, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:45:28', '2021-01-08 11:45:28', 346),
(691, 'cerro sesión, la fecha 08/01/2021 a la 11:46:39', '2021-01-08 11:46:39', 346),
(692, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:47:57', '2021-01-08 11:47:57', 347),
(693, 'cerro sesión, la fecha 08/01/2021 a la 11:49:14', '2021-01-08 11:49:14', 347),
(694, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:51:44', '2021-01-08 11:51:44', 348),
(695, 'cerro sesión, la fecha 08/01/2021 a la 11:57:38', '2021-01-08 11:57:38', 348),
(696, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 11:58:35', '2021-01-08 11:58:35', 349),
(697, 'cerro sesión, la fecha 08/01/2021 a la 12:10:39', '2021-01-08 12:10:39', 349),
(698, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 12:11:01', '2021-01-08 12:11:01', 350),
(699, 'modificó el producto er, la fecha 08/01/2021', '2021-01-08 12:11:17', 350),
(700, 'cerro sesión, la fecha 08/01/2021 a la 12:15:35', '2021-01-08 12:15:35', 350),
(701, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 12:16:11', '2021-01-08 12:16:11', 351),
(702, 'cerro sesión, la fecha 08/01/2021 a la 12:22:16', '2021-01-08 12:22:16', 351),
(703, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 12:22:28', '2021-01-08 12:22:28', 352),
(704, 'cerro sesión, la fecha 08/01/2021 a la 12:23:52', '2021-01-08 12:23:52', 352),
(705, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 12:24:03', '2021-01-08 12:24:03', 353),
(706, 'cerro sesión, la fecha 08/01/2021 a la 12:26:31', '2021-01-08 12:26:31', 353),
(707, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 12:30:04', '2021-01-08 12:30:04', 354),
(708, 'cerro sesión, la fecha 08/01/2021 a la 12:30:58', '2021-01-08 12:30:58', 354),
(709, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 12:31:09', '2021-01-08 12:31:09', 355),
(710, 'modificó el estado del producto Lapices point, la fecha 08/01/2021', '2021-01-08 12:31:56', 355),
(711, 'modificó el estado del producto Pack de cuadernos, la fecha 08/01/2021', '2021-01-08 12:32:00', 355),
(712, 'cerro sesión, la fecha 08/01/2021 a la 12:32:09', '2021-01-08 12:32:09', 355),
(713, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 12:34:15', '2021-01-08 12:34:15', 356),
(714, 'cerro sesión, la fecha 08/01/2021 a la 12:34:26', '2021-01-08 12:34:26', 356),
(715, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 12:36:24', '2021-01-08 12:36:24', 357),
(716, 'cerro sesión, la fecha 08/01/2021 a la 12:38:22', '2021-01-08 12:38:22', 357),
(717, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 12:38:34', '2021-01-08 12:38:34', 358),
(718, 'modificó el estado del producto Pack de cuadernos, la fecha 08/01/2021', '2021-01-08 12:39:50', 358),
(719, 'cerro sesión, la fecha 08/01/2021 a la 12:42:55', '2021-01-08 12:42:55', 358),
(720, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 12:43:05', '2021-01-08 12:43:05', 359),
(721, 'cerro sesión, la fecha 08/01/2021 a la 01:01:50', '2021-01-08 13:01:50', 359),
(722, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 01:02:03', '2021-01-08 13:02:03', 360),
(723, 'modificó el estado del producto er, la fecha 08/01/2021', '2021-01-08 13:08:02', 360),
(724, 'modificó el estado del producto Tijera infantil, la fecha 08/01/2021', '2021-01-08 13:08:04', 360),
(725, 'modificó el estado del producto Cuaderno school, la fecha 08/01/2021', '2021-01-08 13:08:06', 360),
(726, 'modificó el estado del producto Pack de cuadernos, la fecha 08/01/2021', '2021-01-08 13:08:08', 360),
(727, 'cerro sesión, la fecha 08/01/2021 a la 01:11:43', '2021-01-08 13:11:43', 360),
(728, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 01:11:55', '2021-01-08 13:11:55', 361),
(729, 'cerro sesión, la fecha 08/01/2021 a la 01:14:08', '2021-01-08 13:14:08', 361),
(730, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 01:14:20', '2021-01-08 13:14:20', 362),
(731, 'cerro sesión, la fecha 08/01/2021 a la 01:22:53', '2021-01-08 13:22:53', 362),
(732, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 01:23:36', '2021-01-08 13:23:36', 363),
(733, 'cerro sesión, la fecha 08/01/2021 a la 01:54:36', '2021-01-08 13:54:36', 363),
(734, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 01:54:54', '2021-01-08 13:54:54', 364),
(735, 'cerro sesión, la fecha 08/01/2021 a la 01:55:19', '2021-01-08 13:55:19', 364),
(736, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 01:55:40', '2021-01-08 13:55:40', 365),
(737, 'registro un nuevo producto (fgfdg), la fecha 08/01/2021', '2021-01-08 13:56:48', 365),
(738, 'modificó el estado del producto fgfdg, la fecha 08/01/2021', '2021-01-08 13:57:15', 365),
(739, 'cerro sesión, la fecha 08/01/2021 a la 01:58:35', '2021-01-08 13:58:35', 365),
(740, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 02:01:17', '2021-01-08 14:01:17', 366),
(741, 'cerro sesión, la fecha 08/01/2021 a la 02:01:38', '2021-01-08 14:01:38', 366),
(742, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 02:01:53', '2021-01-08 14:01:53', 367),
(743, 'cerro sesión, la fecha 08/01/2021 a la 02:04:32', '2021-01-08 14:04:32', 367),
(744, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 02:04:48', '2021-01-08 14:04:48', 368),
(745, 'cerro sesión, la fecha 08/01/2021 a la 02:07:58', '2021-01-08 14:07:58', 368),
(746, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 02:08:13', '2021-01-08 14:08:13', 369),
(747, 'cerro sesión, la fecha 08/01/2021 a la 02:14:22', '2021-01-08 14:14:22', 369),
(748, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 02:15:14', '2021-01-08 14:15:14', 370),
(749, 'cerro sesión, la fecha 08/01/2021 a la 02:22:15', '2021-01-08 14:22:15', 370),
(750, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 02:22:33', '2021-01-08 14:22:33', 371),
(751, 'cerro sesión, la fecha 08/01/2021 a la 02:27:07', '2021-01-08 14:27:07', 371),
(752, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 02:27:25', '2021-01-08 14:27:25', 372),
(753, 'cerro sesión, la fecha 08/01/2021 a la 02:27:50', '2021-01-08 14:27:50', 372),
(754, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 02:28:14', '2021-01-08 14:28:14', 373),
(755, 'cerro sesión, la fecha 08/01/2021 a la 02:29:36', '2021-01-08 14:29:36', 373),
(756, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 02:29:57', '2021-01-08 14:29:57', 374),
(757, 'cerro sesión, la fecha 08/01/2021 a la 02:33:07', '2021-01-08 14:33:07', 374),
(758, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 02:33:22', '2021-01-08 14:33:22', 375),
(759, 'cerro sesión, la fecha 08/01/2021 a la 02:50:15', '2021-01-08 14:50:15', 375),
(760, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 02:50:40', '2021-01-08 14:50:40', 376),
(761, 'cerro sesión, la fecha 08/01/2021 a la 02:52:02', '2021-01-08 14:52:02', 376),
(762, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 02:53:59', '2021-01-08 14:53:59', 377),
(763, 'cerro sesión, la fecha 08/01/2021 a la 03:08:05', '2021-01-08 15:08:05', 377),
(764, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 03:08:38', '2021-01-08 15:08:38', 378),
(765, 'cerro sesión, la fecha 08/01/2021 a la 03:09:43', '2021-01-08 15:09:43', 378),
(766, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 03:10:00', '2021-01-08 15:10:00', 379),
(767, 'cerro sesión, la fecha 08/01/2021 a la 04:16:27', '2021-01-08 16:16:27', 379),
(768, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 04:17:16', '2021-01-08 16:17:16', 380),
(769, 'cerro sesión, la fecha 08/01/2021 a la 04:19:39', '2021-01-08 16:19:39', 380),
(770, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 04:20:05', '2021-01-08 16:20:05', 381),
(771, 'cerro sesión, la fecha 08/01/2021 a la 04:21:45', '2021-01-08 16:21:45', 381),
(772, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 04:22:02', '2021-01-08 16:22:02', 382),
(773, 'cerro sesión, la fecha 08/01/2021 a la 04:28:22', '2021-01-08 16:28:22', 382),
(774, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 04:28:36', '2021-01-08 16:28:36', 383),
(775, 'cerro sesión, la fecha 08/01/2021 a la 04:48:15', '2021-01-08 16:48:15', 383),
(776, 'inicio sesión como Administrador, la fecha 08/01/2021 a la 04:48:29', '2021-01-08 16:48:29', 384),
(777, 'registro una nueva venta para el proveedor Jorge Luis Pacheco Beltrán, la fecha 08/01/2021 a la 04:49:12', '2021-01-08 16:49:12', 384),
(778, 'modificó el estado del producto fgfdg, la fecha 08/01/2021', '2021-01-08 16:50:12', 384),
(779, 'modificó el estado del producto er, la fecha 08/01/2021', '2021-01-08 16:50:14', 384),
(780, 'modificó el producto Modificación, la fecha 08/01/2021', '2021-01-08 16:50:59', 384);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `bitacora`
--

CREATE TABLE `bitacora` (
  `id` int(11) NOT NULL,
  `iniciosesion` datetime NOT NULL,
  `cierresesion` datetime NOT NULL,
  `idusuario` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `bitacora`
--

INSERT INTO `bitacora` (`id`, `iniciosesion`, `cierresesion`, `idusuario`) VALUES
(146, '2020-11-25 11:22:41', '2020-11-25 12:19:17', 22),
(147, '2020-11-24 12:19:27', '2020-11-24 12:21:42', 22),
(148, '2020-11-25 12:22:00', '2020-11-25 12:24:38', 22),
(149, '2020-11-25 12:25:26', '2020-11-25 12:26:41', 22),
(150, '2020-11-25 12:27:04', '2020-11-25 12:28:18', 22),
(151, '2020-11-25 12:38:19', '2020-11-25 12:43:59', 22),
(152, '2020-11-25 12:52:16', '2020-11-25 12:52:52', 22),
(153, '2020-11-25 12:53:19', '2020-11-25 13:22:28', 22),
(154, '2020-11-25 13:24:15', '2020-11-25 14:30:47', 27),
(155, '2020-12-01 13:35:37', '0001-01-01 00:00:00', 22),
(156, '2020-12-01 14:17:14', '0001-01-01 00:00:00', 22),
(157, '2020-12-01 15:02:40', '2020-12-01 15:06:40', 22),
(158, '2020-12-02 10:29:34', '2020-12-02 10:43:01', 22),
(159, '2020-12-02 10:54:31', '2020-12-02 11:48:16', 22),
(160, '2020-12-02 20:18:28', '0001-01-01 00:00:00', 22),
(161, '2020-12-03 09:29:27', '2020-12-03 09:36:32', 22),
(162, '2020-12-03 09:37:30', '2020-12-03 10:00:09', 22),
(163, '2020-12-03 10:00:40', '2020-12-03 10:12:39', 22),
(164, '2020-12-03 10:13:13', '2020-12-03 10:15:00', 22),
(165, '2020-12-03 10:16:03', '2020-12-03 10:20:58', 22),
(166, '2020-12-03 10:40:25', '2020-12-03 10:42:30', 22),
(167, '2020-12-03 10:43:11', '0001-01-01 00:00:00', 22),
(168, '2020-12-07 15:28:49', '2020-12-07 16:43:34', 22),
(169, '2020-12-07 16:44:36', '2020-12-07 16:46:47', 22),
(170, '2020-12-07 16:47:24', '2020-12-07 16:53:49', 22),
(171, '2020-12-07 16:54:37', '2020-12-07 17:14:12', 22),
(172, '2020-12-07 17:15:08', '2020-12-07 17:41:57', 22),
(173, '2020-12-07 17:42:50', '0001-01-01 00:00:00', 22),
(174, '2020-12-09 05:29:02', '2020-12-09 05:33:53', 22),
(175, '2020-12-09 05:34:22', '2020-12-09 05:43:27', 22),
(176, '2020-12-09 05:44:29', '0001-01-01 00:00:00', 22),
(177, '2020-12-10 11:35:07', '2020-12-10 11:36:15', 22),
(178, '2020-12-10 11:36:59', '0001-01-01 00:00:00', 22),
(179, '2020-12-10 11:37:49', '2020-12-10 11:39:20', 22),
(180, '2020-12-10 11:39:35', '2020-12-10 13:24:37', 22),
(181, '2020-12-10 13:33:53', '2020-12-10 13:39:46', 22),
(182, '2020-12-12 14:15:46', '2020-12-12 15:08:22', 22),
(183, '2020-12-12 15:11:39', '0001-01-01 00:00:00', 22),
(184, '2020-12-15 10:36:27', '2020-12-15 11:01:26', 22),
(185, '2020-12-15 15:21:28', '2020-12-15 16:07:21', 22),
(186, '2020-12-16 17:35:36', '2020-12-16 18:40:07', 22),
(187, '2020-12-16 18:41:45', '2020-12-16 18:56:41', 22),
(188, '2020-12-16 18:57:38', '0001-01-01 00:00:00', 22),
(189, '2020-12-16 21:48:54', '0001-01-01 00:00:00', 22),
(190, '2020-12-17 09:36:31', '2020-12-17 09:39:48', 22),
(191, '2020-12-17 09:44:22', '2020-12-17 09:46:22', 22),
(192, '2020-12-17 09:47:18', '0001-01-01 00:00:00', 22),
(193, '2020-12-18 08:48:13', '2020-12-18 08:50:45', 22),
(194, '2020-12-18 08:52:09', '2020-12-18 08:58:10', 22),
(195, '2020-12-18 08:58:33', '0001-01-01 00:00:00', 22),
(196, '2020-12-18 09:31:29', '2020-12-18 09:31:51', 22),
(197, '2020-12-18 09:32:32', '2020-12-18 09:34:10', 22),
(198, '2020-12-18 09:34:40', '2020-12-18 09:53:17', 22),
(199, '2020-12-18 09:53:43', '2020-12-18 09:54:37', 22),
(200, '2020-12-18 09:55:10', '2020-12-18 10:37:24', 22),
(201, '2020-12-18 10:38:14', '2020-12-18 10:40:40', 22),
(202, '2020-12-18 10:41:39', '2020-12-18 10:50:15', 22),
(203, '2020-12-19 10:49:30', '0001-01-01 00:00:00', 22),
(204, '2020-12-19 11:30:07', '2020-12-19 11:31:24', 22),
(205, '2020-12-19 11:31:44', '2020-12-19 11:38:38', 22),
(206, '2020-12-19 11:39:02', '2020-12-19 11:40:57', 22),
(207, '2020-12-19 11:41:15', '2020-12-19 12:08:00', 22),
(208, '2020-12-19 12:08:24', '2020-12-19 12:22:00', 22),
(209, '2020-12-20 15:50:28', '2020-12-20 15:56:25', 22),
(210, '2020-12-20 16:00:15', '2020-12-20 16:06:21', 22),
(211, '2020-12-20 16:12:42', '2020-12-20 16:14:45', 22),
(212, '2020-12-20 16:15:53', '0001-01-01 00:00:00', 22),
(213, '2020-12-20 16:35:28', '2020-12-20 16:35:51', 22),
(214, '2020-12-20 16:36:42', '2020-12-20 16:39:39', 22),
(215, '2020-12-20 16:40:47', '2020-12-20 16:44:34', 22),
(216, '2020-12-20 16:46:33', '2020-12-20 16:49:00', 22),
(217, '2020-12-21 08:52:29', '0001-01-01 00:00:00', 22),
(218, '2020-12-21 09:59:15', '2020-12-21 10:17:09', 22),
(219, '2020-12-21 10:17:39', '2020-12-21 10:20:19', 22),
(220, '2020-12-21 10:20:35', '2020-12-21 10:51:17', 22),
(221, '2020-12-21 10:52:04', '0001-01-01 00:00:00', 22),
(222, '2020-12-21 10:54:05', '2020-12-21 10:56:12', 22),
(223, '2020-12-21 10:56:29', '2020-12-21 11:01:13', 22),
(224, '2020-12-21 11:02:53', '0001-01-01 00:00:00', 22),
(225, '2020-12-21 11:08:44', '2020-12-21 11:12:06', 22),
(226, '2020-12-21 11:12:26', '0001-01-01 00:00:00', 22),
(227, '2020-12-21 11:16:28', '2020-12-21 11:22:38', 22),
(228, '2020-12-21 11:22:54', '2020-12-21 11:24:05', 22),
(229, '2020-12-21 11:24:24', '0001-01-01 00:00:00', 22),
(230, '2020-12-21 11:27:11', '2020-12-21 11:44:57', 22),
(231, '2020-12-21 11:48:09', '2020-12-21 11:52:01', 22),
(232, '2020-12-21 11:52:20', '2020-12-21 11:55:48', 22),
(233, '2020-12-21 11:56:06', '2020-12-21 12:09:58', 22),
(234, '2020-12-21 12:10:11', '2020-12-21 12:10:20', 22),
(235, '2020-12-21 12:12:17', '2020-12-21 12:13:57', 22),
(236, '2020-12-21 12:14:24', '2020-12-21 12:17:35', 22),
(237, '2020-12-21 12:18:06', '2020-12-21 12:20:20', 22),
(238, '2020-12-21 12:22:03', '2020-12-21 12:32:57', 22),
(239, '2020-12-21 12:34:47', '0001-01-01 00:00:00', 22),
(240, '2020-12-22 16:03:52', '2020-12-22 16:08:49', 22),
(241, '2020-12-22 16:09:19', '2020-12-22 16:30:49', 22),
(242, '2020-12-22 16:34:24', '2020-12-22 17:04:56', 22),
(243, '2020-12-22 17:05:21', '2020-12-22 17:07:38', 22),
(244, '2020-12-22 17:08:06', '2020-12-22 17:15:33', 22),
(245, '2020-12-22 17:16:10', '0001-01-01 00:00:00', 22),
(246, '2020-12-22 17:21:17', '2020-12-22 17:27:44', 22),
(247, '2020-12-22 17:28:06', '2020-12-22 17:32:43', 22),
(248, '2020-12-22 17:33:27', '2020-12-22 17:34:02', 22),
(249, '2020-12-22 17:34:20', '2020-12-22 17:43:59', 22),
(250, '2020-12-22 17:44:33', '2020-12-22 17:53:59', 22),
(251, '2020-12-22 17:54:40', '0001-01-01 00:00:00', 22),
(252, '2020-12-30 13:56:02', '0001-01-01 00:00:00', 22),
(253, '2021-01-05 11:19:47', '0001-01-01 00:00:00', 22),
(254, '2021-01-05 11:30:50', '0001-01-01 00:00:00', 22),
(255, '2021-01-05 11:37:06', '2021-01-05 11:38:09', 22),
(256, '2021-01-05 11:38:35', '0001-01-01 00:00:00', 22),
(257, '2021-01-05 11:55:46', '2021-01-05 11:59:38', 22),
(258, '2021-01-05 11:59:58', '2021-01-05 12:05:54', 22),
(259, '2021-01-05 12:06:10', '2021-01-05 12:08:46', 22),
(260, '2021-01-05 12:09:15', '0001-01-01 00:00:00', 22),
(261, '2021-01-05 13:01:53', '2021-01-05 13:22:36', 22),
(262, '2021-01-05 13:23:07', '2021-01-05 13:49:40', 22),
(263, '2021-01-05 13:50:08', '2021-01-05 13:52:07', 22),
(264, '2021-01-05 13:52:28', '2021-01-05 13:53:20', 22),
(265, '2021-01-05 13:55:54', '2021-01-05 13:58:24', 22),
(266, '2021-01-05 14:04:53', '2021-01-05 14:12:07', 22),
(267, '2021-01-05 14:12:40', '2021-01-05 14:17:01', 22),
(268, '2021-01-05 14:19:08', '0001-01-01 00:00:00', 22),
(269, '2021-01-05 14:36:34', '2021-01-05 14:39:45', 22),
(270, '2021-01-05 14:40:08', '2021-01-05 14:41:42', 22),
(271, '2021-01-05 14:46:05', '2021-01-05 14:49:55', 22),
(272, '2021-01-05 14:52:01', '2021-01-05 14:53:50', 22),
(273, '2021-01-05 14:54:09', '0001-01-01 00:00:00', 22),
(274, '2021-01-05 14:55:18', '0001-01-01 00:00:00', 22),
(275, '2021-01-05 14:59:28', '2021-01-05 15:12:44', 22),
(276, '2021-01-05 15:15:50', '0001-01-01 00:00:00', 22),
(277, '2021-01-05 15:18:20', '0001-01-01 00:00:00', 22),
(278, '2021-01-05 15:22:22', '2021-01-05 15:24:47', 22),
(279, '2021-01-05 15:25:04', '2021-01-05 15:25:40', 22),
(280, '2021-01-05 15:26:11', '2021-01-05 15:28:26', 22),
(281, '2021-01-05 15:29:16', '2021-01-05 15:31:26', 22),
(282, '2021-01-05 15:32:46', '0001-01-01 00:00:00', 22),
(283, '2021-01-05 15:34:28', '2021-01-05 15:34:44', 22),
(284, '2021-01-05 15:36:34', '2021-01-05 15:38:55', 22),
(285, '2021-01-05 15:39:06', '2021-01-05 15:39:32', 22),
(286, '2021-01-05 15:39:48', '2021-01-05 15:40:28', 22),
(287, '2021-01-05 15:40:52', '2021-01-05 15:43:08', 22),
(288, '2021-01-05 15:43:56', '2021-01-05 15:45:59', 22),
(289, '2021-01-05 15:48:04', '2021-01-05 15:48:21', 22),
(290, '2021-01-05 15:49:08', '2021-01-05 15:53:44', 22),
(291, '2021-01-05 15:54:33', '2021-01-05 15:58:07', 22),
(292, '2021-01-05 16:00:30', '0001-01-01 00:00:00', 22),
(293, '2021-01-05 16:02:25', '2021-01-05 16:04:10', 22),
(294, '2021-01-05 16:04:33', '0001-01-01 00:00:00', 22),
(295, '2021-01-05 16:08:52', '2021-01-05 16:11:11', 22),
(296, '2021-01-05 16:11:52', '2021-01-05 16:12:08', 22),
(297, '2021-01-05 16:12:37', '2021-01-05 16:14:46', 22),
(298, '2021-01-05 16:15:06', '0001-01-01 00:00:00', 22),
(299, '2021-01-05 16:18:02', '2021-01-05 16:19:30', 22),
(300, '2021-01-05 16:19:46', '2021-01-05 16:25:07', 22),
(301, '2021-01-05 16:25:27', '2021-01-05 16:26:38', 22),
(302, '2021-01-05 16:27:22', '0001-01-01 00:00:00', 22),
(303, '2021-01-05 16:30:25', '2021-01-05 16:34:30', 22),
(304, '2021-01-05 16:34:47', '2021-01-05 16:38:44', 22),
(305, '2021-01-05 16:39:02', '2021-01-05 16:47:43', 22),
(306, '2021-01-05 16:47:59', '2021-01-05 16:49:07', 22),
(307, '2021-01-05 16:49:23', '2021-01-05 16:59:00', 22),
(308, '2021-01-05 16:59:52', '2021-01-05 17:00:26', 22),
(309, '2021-01-05 17:00:52', '2021-01-05 17:01:28', 22),
(310, '2021-01-05 17:01:42', '2021-01-05 17:02:07', 22),
(311, '2021-01-05 17:02:22', '2021-01-05 17:25:49', 22),
(312, '2021-01-05 17:26:19', '2021-01-05 17:27:08', 22),
(313, '2021-01-05 17:27:23', '2021-01-05 17:28:18', 22),
(314, '2021-01-07 15:03:46', '2021-01-07 15:07:52', 22),
(315, '2021-01-07 15:08:59', '2021-01-07 15:15:50', 22),
(316, '2021-01-07 15:18:23', '2021-01-07 15:21:01', 22),
(317, '2021-01-07 15:28:11', '2021-01-07 15:37:38', 22),
(318, '2021-01-08 08:19:31', '0001-01-01 00:00:00', 22),
(319, '2021-01-08 09:28:51', '2021-01-08 09:39:38', 22),
(320, '2021-01-08 09:40:40', '2021-01-08 09:51:21', 22),
(321, '2021-01-08 09:51:41', '2021-01-08 09:54:28', 22),
(322, '2021-01-08 09:55:02', '2021-01-08 09:58:05', 22),
(323, '2021-01-08 09:58:36', '2021-01-08 10:12:31', 22),
(324, '2021-01-08 10:12:53', '2021-01-08 10:19:26', 22),
(325, '2021-01-08 10:19:49', '2021-01-08 10:47:49', 22),
(326, '2021-01-08 10:48:37', '2021-01-08 10:50:14', 22),
(327, '2021-01-08 10:50:27', '2021-01-08 10:55:14', 22),
(328, '2021-01-08 10:56:00', '2021-01-08 10:58:16', 22),
(329, '2021-01-08 10:58:35', '2021-01-08 10:59:29', 22),
(330, '2021-01-08 10:59:58', '2021-01-08 11:01:03', 22),
(331, '2021-01-08 11:02:52', '2021-01-08 11:04:23', 22),
(332, '2021-01-08 11:05:52', '2021-01-08 11:07:24', 22),
(333, '2021-01-08 11:08:43', '2021-01-08 11:11:31', 22),
(334, '2021-01-08 11:11:56', '2021-01-08 11:13:07', 22),
(335, '2021-01-08 11:14:32', '0001-01-01 00:00:00', 22),
(336, '2021-01-08 11:17:03', '2021-01-08 11:18:40', 22),
(337, '2021-01-08 11:19:04', '2021-01-08 11:21:10', 22),
(338, '2021-01-08 11:21:46', '2021-01-08 11:23:45', 22),
(339, '2021-01-08 11:24:07', '2021-01-08 11:25:28', 22),
(340, '2021-01-08 11:26:31', '2021-01-08 11:28:05', 22),
(341, '2021-01-08 11:28:32', '2021-01-08 11:29:48', 22),
(342, '2021-01-08 11:31:15', '2021-01-08 11:32:01', 22),
(343, '2021-01-08 11:35:08', '2021-01-08 11:35:57', 22),
(344, '2021-01-08 11:37:02', '2021-01-08 11:39:12', 22),
(345, '2021-01-08 11:40:16', '2021-01-08 11:41:43', 22),
(346, '2021-01-08 11:45:28', '2021-01-08 11:46:39', 22),
(347, '2021-01-08 11:47:57', '2021-01-08 11:49:13', 22),
(348, '2021-01-08 11:51:44', '2021-01-08 11:57:38', 22),
(349, '2021-01-08 11:58:35', '2021-01-08 12:10:39', 22),
(350, '2021-01-08 12:11:01', '2021-01-08 12:15:35', 22),
(351, '2021-01-08 12:16:11', '2021-01-08 12:22:16', 22),
(352, '2021-01-08 12:22:28', '2021-01-08 12:23:52', 22),
(353, '2021-01-08 12:24:03', '2021-01-08 12:26:31', 22),
(354, '2021-01-08 12:30:03', '2021-01-08 12:30:58', 22),
(355, '2021-01-08 12:31:08', '2021-01-08 12:32:09', 22),
(356, '2021-01-08 12:34:15', '2021-01-08 12:34:26', 22),
(357, '2021-01-08 12:36:23', '2021-01-08 12:38:22', 22),
(358, '2021-01-08 12:38:34', '2021-01-08 12:42:55', 22),
(359, '2021-01-08 12:43:05', '2021-01-08 13:01:50', 22),
(360, '2021-01-08 13:02:03', '2021-01-08 13:11:43', 22),
(361, '2021-01-08 13:11:55', '2021-01-08 13:14:08', 22),
(362, '2021-01-08 13:14:19', '2021-01-08 13:22:53', 22),
(363, '2021-01-08 13:23:35', '2021-01-08 13:54:36', 22),
(364, '2021-01-08 13:54:54', '2021-01-08 13:55:19', 22),
(365, '2021-01-08 13:55:39', '2021-01-08 13:58:35', 22),
(366, '2021-01-08 14:01:16', '2021-01-08 14:01:38', 22),
(367, '2021-01-08 14:01:52', '2021-01-08 14:04:32', 22),
(368, '2021-01-08 14:04:48', '2021-01-08 14:07:58', 22),
(369, '2021-01-08 14:08:13', '2021-01-08 14:14:22', 22),
(370, '2021-01-08 14:15:14', '2021-01-08 14:22:15', 22),
(371, '2021-01-08 14:22:33', '2021-01-08 14:27:07', 22),
(372, '2021-01-08 14:27:24', '2021-01-08 14:27:50', 22),
(373, '2021-01-08 14:28:13', '2021-01-08 14:29:36', 22),
(374, '2021-01-08 14:29:57', '2021-01-08 14:33:07', 22),
(375, '2021-01-08 14:33:22', '2021-01-08 14:50:15', 22),
(376, '2021-01-08 14:50:40', '2021-01-08 14:52:02', 22),
(377, '2021-01-08 14:53:58', '2021-01-08 15:08:05', 22),
(378, '2021-01-08 15:08:37', '2021-01-08 15:09:43', 22),
(379, '2021-01-08 15:10:00', '2021-01-08 16:16:27', 22),
(380, '2021-01-08 16:17:16', '2021-01-08 16:19:39', 22),
(381, '2021-01-08 16:20:05', '2021-01-08 16:21:45', 22),
(382, '2021-01-08 16:22:01', '2021-01-08 16:28:22', 22),
(383, '2021-01-08 16:28:36', '2021-01-08 16:48:14', 22),
(384, '2021-01-08 16:48:29', '0001-01-01 00:00:00', 22);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `cargo`
--

CREATE TABLE `cargo` (
  `id` int(11) NOT NULL,
  `nombre` varchar(50) COLLATE utf8_spanish_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `cargo`
--

INSERT INTO `cargo` (`id`, `nombre`) VALUES
(1, 'Vendedor'),
(2, 'Limpieza'),
(3, 'Comprador'),
(4, 'Secretaria');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `categoria`
--

CREATE TABLE `categoria` (
  `id` int(11) NOT NULL,
  `nombre` varchar(30) COLLATE utf8_spanish_ci NOT NULL,
  `estado` tinyint(4) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `categoria`
--

INSERT INTO `categoria` (`id`, `nombre`, `estado`) VALUES
(6, 'Lapices', 1),
(7, 'Cuadernos', 1),
(8, 'Sacapuntas', 0),
(9, 'Borradores', 0),
(10, 'Plumones', 0),
(11, 'Colores', 0),
(12, 'Engrapadoras', 0),
(13, 'Marcadores', 0),
(14, 'Resaltadores', 0),
(15, 'Tijeras', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `compra`
--

CREATE TABLE `compra` (
  `id` int(11) NOT NULL,
  `documento` varchar(200) COLLATE utf8_spanish_ci NOT NULL,
  `fecha` date NOT NULL,
  `tipo` varchar(10) COLLATE utf8_spanish_ci NOT NULL,
  `idusuario` int(11) NOT NULL,
  `idproveedor` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `compra`
--

INSERT INTO `compra` (`id`, `documento`, `fecha`, `tipo`, `idusuario`, `idproveedor`) VALUES
(33, '1010', '2020-12-01', 'Contado', 27, 3),
(34, '111', '2021-01-05', 'Contado', 27, 3),
(35, '10000', '2021-01-08', 'Crédito', 22, 2);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `detallecompra`
--

CREATE TABLE `detallecompra` (
  `id` int(11) NOT NULL,
  `cantidad` int(11) NOT NULL,
  `precio` decimal(10,2) NOT NULL,
  `idcompra` int(11) NOT NULL,
  `idproducto` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `detallecompra`
--

INSERT INTO `detallecompra` (`id`, `cantidad`, `precio`, `idcompra`, `idproducto`) VALUES
(41, 100, '1.00', 33, 47),
(42, 150, '1.25', 33, 48),
(43, 200, '5.25', 33, 49),
(44, 200, '1.25', 33, 50),
(45, 200, '3.25', 33, 51),
(46, 10, '1.00', 34, 47),
(47, 1, '1.00', 35, 54),
(48, 1, '1.00', 35, 53),
(49, 1, '1.00', 35, 51),
(50, 1, '1.00', 35, 50),
(51, 1, '1.00', 35, 49),
(52, 1, '1.00', 35, 48),
(53, 1, '1.00', 35, 47);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `detallepermisosusuario`
--

CREATE TABLE `detallepermisosusuario` (
  `id` int(11) NOT NULL,
  `idusuario` int(11) NOT NULL,
  `idpermiso` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `detalleventa`
--

CREATE TABLE `detalleventa` (
  `id` int(11) NOT NULL,
  `cantidad` int(11) NOT NULL,
  `idventa` int(11) NOT NULL,
  `idproducto` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `detalleventa`
--

INSERT INTO `detalleventa` (`id`, `cantidad`, `idventa`, `idproducto`) VALUES
(25, 10, 17, 41),
(26, 11, 17, 44),
(27, 14, 17, 45),
(28, 5, 18, 42),
(29, 1, 19, 43),
(30, 4, 20, 41);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `empleado`
--

CREATE TABLE `empleado` (
  `id` int(11) NOT NULL,
  `sexo` varchar(10) COLLATE utf8_spanish_ci NOT NULL,
  `fechanacimiento` date DEFAULT NULL,
  `idpersona` int(11) NOT NULL,
  `idtelefono` int(11) DEFAULT NULL,
  `idcargo` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `empleado`
--

INSERT INTO `empleado` (`id`, `sexo`, `fechanacimiento`, `idpersona`, `idtelefono`, `idcargo`) VALUES
(28, 'Masculino', NULL, 76, NULL, NULL),
(33, 'Masculino', '1998-07-02', 92, 104, 1),
(34, 'Femenino', '1980-11-03', 93, 105, 4);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `imagen`
--

CREATE TABLE `imagen` (
  `id` int(11) NOT NULL,
  `nombre` varchar(300) COLLATE utf8_spanish_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `imagen`
--

INSERT INTO `imagen` (`id`, `nombre`) VALUES
(55, 'stabiloboss.webp'),
(56, 'lapicesstabilo.webp'),
(57, 'moleskinecua.jpg'),
(58, 'oxfordcuaderno.jpg'),
(60, 'maptijera.jpg'),
(61, 'iconuser.png');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `institucion`
--

CREATE TABLE `institucion` (
  `id` int(11) NOT NULL,
  `nombre` varchar(100) COLLATE utf8_spanish_ci NOT NULL,
  `direccion` varchar(300) COLLATE utf8_spanish_ci NOT NULL,
  `idrepresentante` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `institucion`
--

INSERT INTO `institucion` (`id`, `nombre`, `direccion`, `idrepresentante`) VALUES
(20, 'PAPELCO S.A DE C.V', 'San Salvador,  25 Avenida Sur No. 418, entre 6a 10a y 12 Calle Poniente', 89),
(21, 'DISEÑO S.A DE C.V', 'parque central, san vicente', 95);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inventario`
--

CREATE TABLE `inventario` (
  `id` int(11) NOT NULL,
  `existencia` int(11) NOT NULL,
  `fecha` date NOT NULL,
  `idcompra` int(11) DEFAULT NULL,
  `idventa` int(11) DEFAULT NULL,
  `idproducto` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `inventario`
--

INSERT INTO `inventario` (`id`, `existencia`, `fecha`, `idcompra`, `idventa`, `idproducto`) VALUES
(12, 100, '2020-12-01', 41, NULL, 47),
(13, 150, '2020-12-01', 42, NULL, 48),
(14, 200, '2020-12-01', 43, NULL, 49),
(15, 200, '2020-12-01', 44, NULL, 50),
(16, 200, '2020-12-01', 45, NULL, 51),
(39, 90, '2020-12-22', NULL, 25, 47),
(40, 139, '2020-12-22', NULL, 26, 48),
(41, 186, '2020-12-22', NULL, 27, 49),
(42, 85, '2020-12-22', NULL, 28, 47),
(43, 84, '2020-12-25', NULL, 29, 47),
(44, 80, '2021-01-05', NULL, 30, 47),
(45, 90, '2021-01-05', 46, NULL, 47),
(47, 1, '2021-01-08', 47, NULL, 54),
(48, 1, '2021-01-08', 48, NULL, 53),
(49, 201, '2021-01-08', 49, NULL, 51),
(50, 201, '2021-01-08', 50, NULL, 50),
(51, 187, '2021-01-08', 51, NULL, 49),
(52, 140, '2021-01-08', 52, NULL, 48),
(53, 91, '2021-01-08', 53, NULL, 47);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `marca`
--

CREATE TABLE `marca` (
  `id` int(11) NOT NULL,
  `nombre` varchar(30) COLLATE utf8_spanish_ci NOT NULL,
  `estado` tinyint(4) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `marca`
--

INSERT INTO `marca` (`id`, `nombre`, `estado`) VALUES
(4, 'Oxford', 0),
(5, 'Enri', 0),
(6, 'Moleskine', 0),
(7, 'Leitz', 0),
(8, 'Gallery', 0),
(9, 'Calipage', 0),
(11, 'Bic', 0),
(12, 'Maped', 0),
(13, 'Stabilo', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `id` int(11) NOT NULL,
  `idventa` int(11) NOT NULL,
  `idabono` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`id`, `idventa`, `idabono`) VALUES
(1, 18, 1),
(2, 18, 2),
(3, 18, 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `permisos`
--

CREATE TABLE `permisos` (
  `id` int(11) NOT NULL,
  `nombre` varchar(30) COLLATE utf8_spanish_ci NOT NULL,
  `icono` varchar(200) COLLATE utf8_spanish_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `permisos`
--

INSERT INTO `permisos` (`id`, `nombre`, `icono`) VALUES
(1, 'Inicio', 'uil-home-alt'),
(2, 'Ventas', 'uil-shopping-cart-alt'),
(3, 'Compras', 'uil-cart'),
(4, 'Abonos', 'uil-usd-square'),
(5, 'Marcas', 'uil-pricetag-alt'),
(6, 'Categorías', 'uil-layer-group'),
(7, 'Productos', 'uil-archive'),
(8, 'Clientes', 'uil-user-square'),
(9, 'Proveedores', 'uil-building'),
(10, 'Personal', 'uil-users-alt '),
(11, 'Permisos', 'uil-padlock'),
(12, 'Bitácora', 'uil-clock-ten'),
(13, 'Respaldo de datos', 'uil-database'),
(14, 'Listado de consultas', 'uil-weight'),
(15, 'Listado de reportes', 'uil-file-edit-alt'),
(16, 'Acerca de', 'uil-clock-ten');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `persona`
--

CREATE TABLE `persona` (
  `id` int(11) NOT NULL,
  `nombre` varchar(50) COLLATE utf8_spanish_ci NOT NULL,
  `apellido` varchar(50) COLLATE utf8_spanish_ci NOT NULL,
  `dui` varchar(10) COLLATE utf8_spanish_ci DEFAULT NULL,
  `tipo` varchar(30) COLLATE utf8_spanish_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `persona`
--

INSERT INTO `persona` (`id`, `nombre`, `apellido`, `dui`, `tipo`) VALUES
(76, 'Carlos', 'Villalta', '12345678-9', 'Administrador'),
(86, 'Público general', 'Público general', NULL, 'Público'),
(87, 'Jorge Angel', 'García Santos', '74586154-5', 'Público'),
(88, 'Ana Carolina', 'Beltrán Vásquez', '45874545-1', 'Público'),
(89, 'María Reyna', 'Sánchez Díaz', '45787841-3', 'Institución'),
(90, 'Jorge Luis', 'Pacheco Beltrán', '45548416-5', 'Proveedor'),
(91, 'Kevin Alexis', 'Jimenez Ruíz', '74849445-6', 'Proveedor'),
(92, 'José Saúl', 'Hernández Vásquez', '26654165-1', 'Empleado'),
(93, 'María Cristina', 'García Ventura', '56456415-1', 'Empleado'),
(95, 'Kevin Alexis', 'Jimenez Ruiz', '74585656-5', 'Institución');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `preciosproducto`
--

CREATE TABLE `preciosproducto` (
  `id` int(11) NOT NULL,
  `margen` decimal(10,2) NOT NULL,
  `fecha` date NOT NULL,
  `estado` tinyint(4) NOT NULL DEFAULT 0,
  `idproducto` int(11) NOT NULL,
  `idcompra` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `preciosproducto`
--

INSERT INTO `preciosproducto` (`id`, `margen`, `fecha`, `estado`, `idproducto`, `idcompra`) VALUES
(41, '5.00', '2020-12-10', 1, 47, 33),
(42, '9.00', '2020-12-10', 1, 47, 33),
(43, '13.00', '2020-12-10', 1, 47, 33),
(44, '3.00', '2020-12-10', 1, 48, 33),
(45, '6.00', '2020-12-18', 1, 49, 33),
(46, '4.00', '2020-12-18', 0, 50, 33),
(47, '7.00', '2020-12-18', 0, 51, 33);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `producto`
--

CREATE TABLE `producto` (
  `id` int(11) NOT NULL,
  `codigo` varchar(30) COLLATE utf8_spanish_ci NOT NULL,
  `nombre` varchar(50) COLLATE utf8_spanish_ci NOT NULL,
  `descripcion` varchar(250) COLLATE utf8_spanish_ci DEFAULT NULL,
  `stockminimo` int(11) DEFAULT 0,
  `estado` tinyint(4) NOT NULL DEFAULT 0,
  `idmarca` int(11) DEFAULT NULL,
  `idcategoria` int(11) NOT NULL,
  `idimagen` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `producto`
--

INSERT INTO `producto` (`id`, `codigo`, `nombre`, `descripcion`, `stockminimo`, `estado`, `idmarca`, `idcategoria`, `idimagen`) VALUES
(47, '1 458 89', 'Stabilo marcadores', 'Diferentes presntaciones', 15, 1, 13, 13, 55),
(48, '74 895 658', 'Lapices point', NULL, 12, 1, 13, 6, 56),
(49, '74 874 5', 'Pack de cuadernos', 'Incluye 3 cuadernos de la marca moleskine', 20, 1, 6, 7, 57),
(50, '458 789', 'Cuaderno school', 'Cuaderno microperforado school 80 hijas por color', 25, 1, 4, 7, 58),
(51, '745 895', 'Tijera infantil', 'Tijera en diversas presentaciones', 40, 1, 12, 15, 60),
(53, '1010', 'er', '1010', 101, 0, 5, 8, NULL),
(54, '10', 'Modificación', '10', 10, 0, 12, 14, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `proveedor`
--

CREATE TABLE `proveedor` (
  `id` int(11) NOT NULL,
  `nombre` varchar(250) COLLATE utf8_spanish_ci NOT NULL,
  `direccion` varchar(300) COLLATE utf8_spanish_ci NOT NULL,
  `idrepresentante` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `proveedor`
--

INSERT INTO `proveedor` (`id`, `nombre`, `direccion`, `idrepresentante`) VALUES
(2, 'LIBRERÍA CERVANTES', '9 Av. Sur 118 4-B, Edificio Patricia', 90),
(3, 'PAPELERA SALVADOREÑA S.A DE C.V', '49 Avenida Sur, # 2614, Colonia San Mateo, San Salvador.', 91);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `recuperarcuenta`
--

CREATE TABLE `recuperarcuenta` (
  `id` int(11) NOT NULL,
  `codigo` varchar(8) COLLATE utf8_spanish_ci NOT NULL,
  `fechaenvio` datetime NOT NULL,
  `fecharecuperacion` datetime DEFAULT NULL,
  `estado` tinyint(4) NOT NULL,
  `idusuario` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `recuperarcuenta`
--

INSERT INTO `recuperarcuenta` (`id`, `codigo`, `fechaenvio`, `fecharecuperacion`, `estado`, `idusuario`) VALUES
(5, 'fU7XmL', '2020-11-25 13:23:02', '2020-11-25 13:23:45', 0, 27);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `telefono`
--

CREATE TABLE `telefono` (
  `id` int(11) NOT NULL,
  `telefono` varchar(15) COLLATE utf8_spanish_ci NOT NULL,
  `estado` tinyint(4) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `telefono`
--

INSERT INTO `telefono` (`id`, `telefono`, `estado`) VALUES
(100, '7458-9655', 1),
(102, '7458-9565', 0),
(103, '2255-4544', 0),
(104, '7458-5956', 1),
(105, '4584-4841', 1),
(106, '7458-7888', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `telefonoinstitucion`
--

CREATE TABLE `telefonoinstitucion` (
  `id` int(11) NOT NULL,
  `idinstitucion` int(11) NOT NULL,
  `idtelefono` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `telefonoinstitucion`
--

INSERT INTO `telefonoinstitucion` (`id`, `idinstitucion`, `idtelefono`) VALUES
(75, 20, 100),
(77, 21, 106);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `telefonoproveedor`
--

CREATE TABLE `telefonoproveedor` (
  `id` int(11) NOT NULL,
  `idproveedor` int(11) NOT NULL,
  `idtelefono` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `telefonoproveedor`
--

INSERT INTO `telefonoproveedor` (`id`, `idproveedor`, `idtelefono`) VALUES
(1, 2, 102),
(2, 2, 103);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `id` int(11) NOT NULL,
  `usuario` varchar(50) COLLATE utf8_spanish_ci NOT NULL,
  `contrasena` varchar(300) COLLATE utf8_spanish_ci NOT NULL,
  `correo` varchar(50) COLLATE utf8_spanish_ci DEFAULT NULL,
  `rol` varchar(15) COLLATE utf8_spanish_ci NOT NULL,
  `estado` tinyint(4) NOT NULL DEFAULT 0,
  `idempleado` int(11) NOT NULL,
  `idimagen` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`id`, `usuario`, `contrasena`, `correo`, `rol`, `estado`, `idempleado`, `idimagen`) VALUES
(22, 'Administrador', 'd4584547c7f6a01a40bb8d863ab2c134e0c51ce353c0ca2fd93857961d750658', 'administrador@gmail.com', 'Administrador', 1, 28, NULL),
(27, 'jvasquez', 'ef797c8118f02dfb649607dd5d3f8c7623048c9c063d532cc95c5ed7a898a64f', 'hv17009@ues.edu.sv', 'Empleado', 1, 33, 61);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `venta`
--

CREATE TABLE `venta` (
  `id` int(11) NOT NULL,
  `documento` varchar(200) COLLATE utf8_spanish_ci NOT NULL,
  `fecha` date NOT NULL,
  `tipo` varchar(10) COLLATE utf8_spanish_ci NOT NULL,
  `estado` tinyint(4) DEFAULT NULL,
  `idcliente` int(11) NOT NULL,
  `idusuario` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `venta`
--

INSERT INTO `venta` (`id`, `documento`, `fecha`, `tipo`, `estado`, `idcliente`, `idusuario`) VALUES
(17, '100', '2020-12-22', 'Contado', 0, 86, 22),
(18, '4587-89', '2020-12-22', 'Crédito', 1, 89, 22),
(19, '4512', '2020-12-25', 'Crédito', 1, 95, 22),
(20, '145', '2021-01-05', 'Contado', 0, 86, 22);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `abono`
--
ALTER TABLE `abono`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `accion`
--
ALTER TABLE `accion`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_bitacora_accion` (`idbitacora`);

--
-- Indices de la tabla `bitacora`
--
ALTER TABLE `bitacora`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_usuario_bitacora` (`idusuario`);

--
-- Indices de la tabla `cargo`
--
ALTER TABLE `cargo`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `categoria`
--
ALTER TABLE `categoria`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `compra`
--
ALTER TABLE `compra`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_usuario_compra` (`idusuario`),
  ADD KEY `fk_proveedor_compra` (`idproveedor`);

--
-- Indices de la tabla `detallecompra`
--
ALTER TABLE `detallecompra`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_compra_detallecompra` (`idcompra`),
  ADD KEY `fk_producto_detallecompra` (`idproducto`);

--
-- Indices de la tabla `detallepermisosusuario`
--
ALTER TABLE `detallepermisosusuario`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_usuario_detallepermiso` (`idusuario`),
  ADD KEY `fk_permisos_detallepermiso` (`idpermiso`);

--
-- Indices de la tabla `detalleventa`
--
ALTER TABLE `detalleventa`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_venta_detalleventa` (`idventa`),
  ADD KEY `fk_producto_detalleventa` (`idproducto`);

--
-- Indices de la tabla `empleado`
--
ALTER TABLE `empleado`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_persona_empleado` (`idpersona`),
  ADD KEY `fk_telefono_empleado` (`idtelefono`),
  ADD KEY `fk_cargo_empleado` (`idcargo`);

--
-- Indices de la tabla `imagen`
--
ALTER TABLE `imagen`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `institucion`
--
ALTER TABLE `institucion`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_cliente_institucion` (`idrepresentante`);

--
-- Indices de la tabla `inventario`
--
ALTER TABLE `inventario`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_compra_inventario` (`idcompra`),
  ADD KEY `fk_venta_inventario` (`idventa`),
  ADD KEY `fk_producto_inventario` (`idproducto`);

--
-- Indices de la tabla `marca`
--
ALTER TABLE `marca`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_venta_pago` (`idventa`),
  ADD KEY `fk_abono_pago` (`idabono`);

--
-- Indices de la tabla `permisos`
--
ALTER TABLE `permisos`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `persona`
--
ALTER TABLE `persona`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `preciosproducto`
--
ALTER TABLE `preciosproducto`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_precio_producto` (`idproducto`),
  ADD KEY `fk_compra_precioventa` (`idcompra`);

--
-- Indices de la tabla `producto`
--
ALTER TABLE `producto`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_categoria_producto` (`idcategoria`),
  ADD KEY `fk_marca_producto` (`idmarca`),
  ADD KEY `fk_imagen_producto` (`idimagen`);

--
-- Indices de la tabla `proveedor`
--
ALTER TABLE `proveedor`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_representante_proveedor` (`idrepresentante`);

--
-- Indices de la tabla `recuperarcuenta`
--
ALTER TABLE `recuperarcuenta`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_usuario_recuperarc` (`idusuario`);

--
-- Indices de la tabla `telefono`
--
ALTER TABLE `telefono`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `telefonoinstitucion`
--
ALTER TABLE `telefonoinstitucion`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_institucion_telefono` (`idinstitucion`),
  ADD KEY `fk_telefono_institucion` (`idtelefono`);

--
-- Indices de la tabla `telefonoproveedor`
--
ALTER TABLE `telefonoproveedor`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_telefono_proveedor` (`idproveedor`),
  ADD KEY `fk_proveedor_telefono` (`idtelefono`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_empleado_usuario` (`idempleado`),
  ADD KEY `fk_imagen_usuario` (`idimagen`);

--
-- Indices de la tabla `venta`
--
ALTER TABLE `venta`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_persona_venta` (`idcliente`),
  ADD KEY `fk_usuario_venta` (`idusuario`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `abono`
--
ALTER TABLE `abono`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `accion`
--
ALTER TABLE `accion`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=781;

--
-- AUTO_INCREMENT de la tabla `bitacora`
--
ALTER TABLE `bitacora`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=385;

--
-- AUTO_INCREMENT de la tabla `cargo`
--
ALTER TABLE `cargo`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `categoria`
--
ALTER TABLE `categoria`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT de la tabla `compra`
--
ALTER TABLE `compra`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=36;

--
-- AUTO_INCREMENT de la tabla `detallecompra`
--
ALTER TABLE `detallecompra`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=54;

--
-- AUTO_INCREMENT de la tabla `detallepermisosusuario`
--
ALTER TABLE `detallepermisosusuario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `detalleventa`
--
ALTER TABLE `detalleventa`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=32;

--
-- AUTO_INCREMENT de la tabla `empleado`
--
ALTER TABLE `empleado`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=36;

--
-- AUTO_INCREMENT de la tabla `imagen`
--
ALTER TABLE `imagen`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=63;

--
-- AUTO_INCREMENT de la tabla `institucion`
--
ALTER TABLE `institucion`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- AUTO_INCREMENT de la tabla `inventario`
--
ALTER TABLE `inventario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=54;

--
-- AUTO_INCREMENT de la tabla `marca`
--
ALTER TABLE `marca`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `permisos`
--
ALTER TABLE `permisos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT de la tabla `persona`
--
ALTER TABLE `persona`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=97;

--
-- AUTO_INCREMENT de la tabla `preciosproducto`
--
ALTER TABLE `preciosproducto`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=48;

--
-- AUTO_INCREMENT de la tabla `producto`
--
ALTER TABLE `producto`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=55;

--
-- AUTO_INCREMENT de la tabla `proveedor`
--
ALTER TABLE `proveedor`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `recuperarcuenta`
--
ALTER TABLE `recuperarcuenta`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `telefono`
--
ALTER TABLE `telefono`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=107;

--
-- AUTO_INCREMENT de la tabla `telefonoinstitucion`
--
ALTER TABLE `telefonoinstitucion`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=78;

--
-- AUTO_INCREMENT de la tabla `telefonoproveedor`
--
ALTER TABLE `telefonoproveedor`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT de la tabla `venta`
--
ALTER TABLE `venta`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `accion`
--
ALTER TABLE `accion`
  ADD CONSTRAINT `fk_bitacora_accion` FOREIGN KEY (`idbitacora`) REFERENCES `bitacora` (`id`);

--
-- Filtros para la tabla `bitacora`
--
ALTER TABLE `bitacora`
  ADD CONSTRAINT `fk_usuario_bitacora` FOREIGN KEY (`idusuario`) REFERENCES `usuario` (`id`);

--
-- Filtros para la tabla `compra`
--
ALTER TABLE `compra`
  ADD CONSTRAINT `fk_proveedor_compra` FOREIGN KEY (`idproveedor`) REFERENCES `proveedor` (`id`),
  ADD CONSTRAINT `fk_usuario_compra` FOREIGN KEY (`idusuario`) REFERENCES `usuario` (`id`);

--
-- Filtros para la tabla `detallecompra`
--
ALTER TABLE `detallecompra`
  ADD CONSTRAINT `fk_compra_detallecompra` FOREIGN KEY (`idcompra`) REFERENCES `compra` (`id`),
  ADD CONSTRAINT `fk_producto_detallecompra` FOREIGN KEY (`idproducto`) REFERENCES `producto` (`id`);

--
-- Filtros para la tabla `detallepermisosusuario`
--
ALTER TABLE `detallepermisosusuario`
  ADD CONSTRAINT `fk_permisos_detallepermiso` FOREIGN KEY (`idpermiso`) REFERENCES `permisos` (`id`),
  ADD CONSTRAINT `fk_usuario_detallepermiso` FOREIGN KEY (`idusuario`) REFERENCES `usuario` (`id`);

--
-- Filtros para la tabla `detalleventa`
--
ALTER TABLE `detalleventa`
  ADD CONSTRAINT `fk_producto_detalleventa` FOREIGN KEY (`idproducto`) REFERENCES `preciosproducto` (`id`),
  ADD CONSTRAINT `fk_venta_detalleventa` FOREIGN KEY (`idventa`) REFERENCES `venta` (`id`);

--
-- Filtros para la tabla `empleado`
--
ALTER TABLE `empleado`
  ADD CONSTRAINT `fk_cargo_empleado` FOREIGN KEY (`idcargo`) REFERENCES `cargo` (`id`),
  ADD CONSTRAINT `fk_persona_empleado` FOREIGN KEY (`idpersona`) REFERENCES `persona` (`id`),
  ADD CONSTRAINT `fk_telefono_empleado` FOREIGN KEY (`idtelefono`) REFERENCES `telefono` (`id`);

--
-- Filtros para la tabla `institucion`
--
ALTER TABLE `institucion`
  ADD CONSTRAINT `fk_cliente_institucion` FOREIGN KEY (`idrepresentante`) REFERENCES `persona` (`id`);

--
-- Filtros para la tabla `inventario`
--
ALTER TABLE `inventario`
  ADD CONSTRAINT `fk_compra_inventario` FOREIGN KEY (`idcompra`) REFERENCES `detallecompra` (`id`),
  ADD CONSTRAINT `fk_producto_inventario` FOREIGN KEY (`idproducto`) REFERENCES `producto` (`id`),
  ADD CONSTRAINT `fk_venta_inventario` FOREIGN KEY (`idventa`) REFERENCES `detalleventa` (`id`);

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `fk_abono_pago` FOREIGN KEY (`idabono`) REFERENCES `abono` (`id`),
  ADD CONSTRAINT `fk_venta_pago` FOREIGN KEY (`idventa`) REFERENCES `venta` (`id`);

--
-- Filtros para la tabla `preciosproducto`
--
ALTER TABLE `preciosproducto`
  ADD CONSTRAINT `fk_compra_precioventa` FOREIGN KEY (`idcompra`) REFERENCES `compra` (`id`),
  ADD CONSTRAINT `fk_precio_producto` FOREIGN KEY (`idproducto`) REFERENCES `producto` (`id`);

--
-- Filtros para la tabla `producto`
--
ALTER TABLE `producto`
  ADD CONSTRAINT `fk_categoria_producto` FOREIGN KEY (`idcategoria`) REFERENCES `categoria` (`id`),
  ADD CONSTRAINT `fk_imagen_producto` FOREIGN KEY (`idimagen`) REFERENCES `imagen` (`id`),
  ADD CONSTRAINT `fk_marca_producto` FOREIGN KEY (`idmarca`) REFERENCES `marca` (`id`);

--
-- Filtros para la tabla `proveedor`
--
ALTER TABLE `proveedor`
  ADD CONSTRAINT `fk_representante_proveedor` FOREIGN KEY (`idrepresentante`) REFERENCES `persona` (`id`);

--
-- Filtros para la tabla `recuperarcuenta`
--
ALTER TABLE `recuperarcuenta`
  ADD CONSTRAINT `fk_usuario_recuperarc` FOREIGN KEY (`idusuario`) REFERENCES `usuario` (`id`);

--
-- Filtros para la tabla `telefonoinstitucion`
--
ALTER TABLE `telefonoinstitucion`
  ADD CONSTRAINT `fk_institucion_telefono` FOREIGN KEY (`idinstitucion`) REFERENCES `institucion` (`id`),
  ADD CONSTRAINT `fk_telefono_institucion` FOREIGN KEY (`idtelefono`) REFERENCES `telefono` (`id`);

--
-- Filtros para la tabla `telefonoproveedor`
--
ALTER TABLE `telefonoproveedor`
  ADD CONSTRAINT `fk_proveedor_telefono` FOREIGN KEY (`idtelefono`) REFERENCES `telefono` (`id`),
  ADD CONSTRAINT `fk_telefono_proveedor` FOREIGN KEY (`idproveedor`) REFERENCES `proveedor` (`id`);

--
-- Filtros para la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD CONSTRAINT `fk_empleado_usuario` FOREIGN KEY (`idempleado`) REFERENCES `empleado` (`id`),
  ADD CONSTRAINT `fk_imagen_usuario` FOREIGN KEY (`idimagen`) REFERENCES `imagen` (`id`);

--
-- Filtros para la tabla `venta`
--
ALTER TABLE `venta`
  ADD CONSTRAINT `fk_persona_venta` FOREIGN KEY (`idcliente`) REFERENCES `persona` (`id`),
  ADD CONSTRAINT `fk_usuario_venta` FOREIGN KEY (`idusuario`) REFERENCES `usuario` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
