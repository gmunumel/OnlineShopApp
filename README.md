# OnlineShopApp
Autor: Gabriel Muñumel

## Detalles Generales

En el sistema hay dos clases de Usuarios:
1. Admin
2. Client

Para acceder como administrador se puede utilizar el usuario: `admin` y password: `Admin1!`. Para acceder como cliente se puede utilizar cualquier nombre de usuario de prueba: `test1`, `test2`, .... La contraseña será siempre `Admin1!`. 

El usuario `Admin` puede realizar todas las operaciones en el sistema excepto ver el carrito (Checkout) o comprar. Adicionalmente puede visualizar la API con un link en la parte superior derecha.

El usuario `Client` puede Comprar y gestionar su Carrito (Checkout). Sin embargo, no puede crear nuevos productos o categorias de productos. Puede crear clientes pero solo con el Rol `Client`.

## Contenido de Tablas

La aplicación cuenta con dos archivos `.mdf`:
1. `aspnet-OnlineShopApp-20180219085138`: encargado del manejo de usuario y su administración. Estas tablas son estandar. Contiene las siguientes tablas:
* AspNetRoles: contiene los roles que puede ser asignados a los usuarios. Se han definido dos: `Admin` y `Client`.
* AspNetUserClaims: contiene información de claims para los usuarios. _No se utiliza_.
* AspNetUserLogins: contiene información adicional del log in del usuario. _No se utiliza_.
* AspNetUserRoles: contiene la relación entre usuario y roles. 
* AspNetUsers: contiene la información del usuario. 
2. `Online Shop`: encargada de almacenar la lógica de la aplicación. Contiene las siguientes tablas:
* Product: encargada de almacenar todos los productos.
* ProductCategory: contiene las posible categorías de productos que se puede crear.
* Purchase: contiene la información asociada a la compra de un producto y la persona quien lo ha hecho.

# Contenido Aplicación

Esta aplicación contiene la implementación de todos los aspectos solicitados para la entrega. Tango los obligatorios como los opcionales.

 
