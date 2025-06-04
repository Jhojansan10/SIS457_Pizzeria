INSERT INTO REPARTIDOR
  (usuario_id, nro_licencia)
VALUES
  (3, 'ABC-1234-DC');
--   REPARTIDOR con usuario_id = 3
GO

/* ------------------------------------------------
   6) Insertar PEDIDO
   ------------------------------------------------ */
INSERT INTO PEDIDO
  (usuario_id, estado, total, direccion_id)
VALUES
  (2, 'pendiente', 18.50, 1),
  (2, 'pendiente',  9.00, 1);
--   PEDIDO_ID = 1 y 2 (ambos de Juan Pérez, cliente)

/*
  Descripción de totales:
   - Pedido 1:  2x Pepperoni (2 * 8.50 = 17.00) + 1x Coca-Cola (1.50) = 18.50
   - Pedido 2:  1x Cuatro Quesos (9.00) = 9.00
*/
GO

/* ------------------------------------------------
   7) Insertar DETALLE_PEDIDO
   ------------------------------------------------ */
INSERT INTO DETALLE_PEDIDO
  (pedido_id, platillo_id, cantidad, precio_unitario)
VALUES
  (1, 1, 2, 8.50),   -- Pedido 1: 2 pizzas Pepperoni a 8.50 c/u
  (1, 3, 1, 1.50),   -- Pedido 1: 1 Coca-Cola a 1.50
  (2, 2, 1, 9.00);   -- Pedido 2: 1 pizza Cuatro Quesos a 9.00
GO

/* ------------------------------------------------
   8) Insertar PAGO
   ------------------------------------------------ */
INSERT INTO PAGO
  (pedido_id, monto_pagado, metodo)
VALUES
  (1, 18.50, 'efectivo'),
  (2,  9.00, 'tarjeta');
--   Ambos pedidos ya pagados
GO

/* ------------------------------------------------
   9) Insertar RESEÑA
   ------------------------------------------------ */
INSERT INTO RESENA
  (usuario_id, pedido_id, calificacion, comentario)
VALUES
  (2, 1, 5, 'Excelente pizza, muy sabrosa y a tiempo!'),
  (2, 2, 4, 'La pizza de 4 quesos muy buena, aunque un poco fría.');
--   RESEÑA_ID = 1 y 2 (Juan Pérez opina de sus pedidos)
GO