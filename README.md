# SIS257_Restaurante

## Descripción del proyecto
**“Hot Pizzas”** es un restaurante innovador que no cuenta con un sistema web para la administración de pedidos. Fusiona los sabores de la comida rápida con un ambiente cuidadosamente diseñado para brindar calidez, elegancia y comodidad.

El concepto central de la pizzería es elaborar platos con ingredientes frescos, locales y de temporada, presentados con una estética moderna y un enfoque en el sabor auténtico. Se prioriza la atención al detalle tanto en la preparación de los alimentos como en la atención al cliente.

## Objetivos del Proyecto
- Establecer una página web para el restaurante que sea reconocido por su calidad y buen servicio al cliente.  
- Promover el uso de productos locales y sostenibles en la elaboración de los platos.  
- Crear un entorno digital atractivo que complemente la experiencia en sala, atrayendo a una clientela diversa.  
- Lograr rentabilidad sostenida mediante operaciones eficientes, fidelización de clientes y futura expansión.

## Servicios Ofrecidos
- Servicio de pedidos en línea.

## Ubicación ideal
La pizzería está ubicada en Sucre, en la calle René Calvo Arana frente al Estadio Patria.

---

## Entidades

### USUARIO
- **id** (PK)  
- **nombre**  
- **email** (único)  
- **contraseña**  
- **rol** (ENUM: `cliente`, `repartidor`, `admin`)  
- **fecha_registro**

### CATEGORÍA
- **id** (PK)  
- **nombre** (p. ej. “Pizzas Clásicas”, “Bebidas”)  
- **descripción**

### PLATILLO
- **id** (PK)  
- **nombre** (p. ej. “Pepperoni”, “Cuatro quesos”)  
- **descripción**  
- **precio** (DECIMAL)  
- **tiempo_preparación** (minutos)  
- **imagen_url**  
- **categoria_id** (FK → CATEGORÍA.id)

### PEDIDO
- **id** (PK)  
- **usuario_id** (FK → USUARIO.id)  
- **fecha**  
- **estado** (ENUM: `pendiente`, `en_preparación`, `en_reparto`, `entregado`, `cancelado`)  
- **total**  
- **direccion_id** (FK → DIRECCIÓN.id)

### DETALLE_PEDIDO
- **id** (PK)  
- **pedido_id** (FK → PEDIDO.id)  
- **platillo_id** (FK → PLATILLO.id)  
- **cantidad**  
- **precio_unitario**

### PAGO
- **id** (PK)  
- **pedido_id** (FK → PEDIDO.id)  
- **monto_pagado**  
- **metodo** (ENUM: `efectivo`, `tarjeta`, `paypal`, …)  
- **fecha_pago**

### DIRECCIÓN
- **id** (PK)  
- **usuario_id** (FK → USUARIO.id)  
- **calle**  
- **ciudad**  
- **código_postal**  
- **indicaciones**  
- **fecha_registro**

### RESEÑA (opcional)
- **id** (PK)  
- **usuario_id** (FK → USUARIO.id)  
- **pedido_id** (FK → PEDIDO.id)  
- **calificación** (1–5)  
- **comentario**  
- **fecha**

### REPARTIDOR (opcional)
- **usuario_id** (PK, FK → USUARIO.id)  
- **nro_licencia**  
- **fecha_ingreso**

---
