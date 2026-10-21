
--Insertar Suplidores
INSERT INTO Suplidores (nombreEmpresa, nombreContacto, telefono, email, fechaCreacion, estado) 
VALUES
(N'Cecomsa SRL', N'Pedro Martínez', '809-555-0101', 'ventas@gmail.com', GETDATE(), 1),
(N'Omega Tech Dominicana', N'Laura Sánchez', '829-555-0202', 'contacto@gamil.com.do', GETDATE(), 1),
(N'CompuCentro RD', N'Carlos Gómez', '849-555-0303', 'cgomez@gmail.do', GETDATE(), 1),
(N'Suplidores Tecnológicos ST', N'Ana Rodríguez', '809-555-0404', 'arodriguez@gmail.com', GETDATE(), 1),
(N'Distribuidora Global Tech', N'Manuel Peralta', '829-555-0505', 'ventas@gmail.com.do', GETDATE(), 1);
GO


--Insertar Categorias

INSERT INTO Categorias (nombre, descripcion, idImagen, fechaCreacion, estado) 
VALUES
(N'Laptops y Computadoras', N'Equipos portátiles para trabajo, estudio y alto rendimiento gaming.', NULL, GETDATE(), 1),
(N'Monitores y Pantallas', N'Monitores LED, OLED y ultraanchos para productividad y juegos.', NULL, GETDATE(), 1),
(N'Periféricos y Accesorios', N'Teclados mecánicos, ratones ergonómicos, audífonos y accesorios.', NULL, GETDATE(), 1),
(N'Componentes de PC', N'Tarjetas gráficas, procesadores, memorias RAM y fuentes de poder.', NULL, GETDATE(), 1),
(N'Almacenamiento', N'Unidades SSD NVMe, discos duros externos y memorias flash USB.', NULL, GETDATE(), 1);
GO



--Insertar Productos

INSERT INTO Productos (nombre, precio, idCategoria, idSuplidor, fechaCreacion, estado) 
VALUES

('Asus ROG Zephyrus G16', 125000.00, 1, 1, GETDATE(), 1),
('Lenovo Legion Slim 5', 89000.00, 1, 2, GETDATE(), 1),
('Dell XPS 13 Ultra', 95000.00, 1, 3, GETDATE(), 1),
('HP Pavilion 15', 45000.00, 1, 4, GETDATE(), 1),

('Monitor LG UltraGear 27" 165Hz', 18500.00, 2, 1, GETDATE(), 1),
('Monitor Samsung Odyssey G5 32"', 24000.00, 2, 2, GETDATE(), 1),
('Monitor Dell 24" IPS FHD', 11500.00, 2, 3, GETDATE(), 1),
('Monitor Asus TUF Gaming 24.5"', 16000.00, 2, 5, GETDATE(), 1),


('Teclado Mecánico Logitech G Pro X', 7500.00, 3, 1, GETDATE(), 1),
('Mouse Inalámbrico Razer DeathAdder V3', 5200.00, 3, 2, GETDATE(), 1),
('Audífonos HyperX Cloud II', 4800.00, 3, 4, GETDATE(), 1),
('Combo Teclado y Mouse VSG', 2500.00, 3, 5, GETDATE(), 1),


('Procesador AMD Ryzen 7 5700X', 14500.00, 4, 1, GETDATE(), 1),
('Tarjeta de Video Nvidia RTX 4060 8GB', 26500.00, 4, 2, GETDATE(), 1),
('Memoria RAM Corsair Vengeance 16GB DDR4', 3800.00, 4, 3, GETDATE(), 1),
('Fuente de Poder EVGA 600W 80 Plus', 4200.00, 4, 4, GETDATE(), 1),


('SSD NVMe Kingston NV2 1TB', 4100.00, 5, 1, GETDATE(), 1),
('SSD NVMe Samsung 980 Pro 2TB', 9800.00, 5, 2, GETDATE(), 1),
('Disco Duro Externo WD Elements 2TB', 4600.00, 5, 3, GETDATE(), 1),
('Memoria USB SanDisk 64GB USB 3.0', 750.00, 5, 5, GETDATE(), 1);
GO