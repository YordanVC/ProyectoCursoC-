using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GestionProductos
{

    class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}\nNombre: {Nombre}\nPrecio: ${Precio.ToString("F2", CultureInfo.InvariantCulture)}";
        }
    }

    class Program
    {
        static List<Producto> productos = new List<Producto>();
        static int siguienteId = 1;

        static void Main(string[] args)
        {
            bool salir = false;

            while (!salir)
            {
                try
                {
                    MostrarMenu();
                    Console.Write("Seleccione una opción: ");
                    string opcion = Console.ReadLine();

                    Console.WriteLine();

                    switch (opcion)
                    {
                        case "1":
                            AgregarProducto();
                            break;
                        case "2":
                            EliminarProducto();
                            break;
                        case "3":
                            BuscarProducto();
                            break;
                        case "4":
                            MostrarProductos();
                            break;
                        case "5":
                            salir = true;
                            break;
                        default:
                            throw new ProductoException("Error. Por favor, seleccione una opcion valida.");
                    }
                }
                catch (ProductoException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error de formato: Verifique los datos ingresados.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Error: El valor ingresado es demasiado grande o pequeño.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado: {ex.Message}");
                }

                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void MostrarMenu()
        {
            Console.WriteLine("===== GESTION DE PRODUCTOS =====");
            Console.WriteLine("| 1. Agregar Producto          |");
            Console.WriteLine("| 2. Eliminar Producto         |");
            Console.WriteLine("| 3. Buscar Producto           |");
            Console.WriteLine("| 4. Mostrar Productos         |");
            Console.WriteLine("| 5. Salir                     |");
            Console.WriteLine("===============================");
        }

        static void AgregarProducto()
        {
            try
            {
                Console.WriteLine("=== AGREGAR PRODUCTO ===");

                Console.Write("Ingrese el nombre del producto: ");
                string nombre = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(nombre))
                {
                    throw new ProductoException("El nombre del producto no puede estar vacío.");
                }

                Console.Write("Ingrese el precio del producto: $");
                string precioStr = Console.ReadLine();

                precioStr = precioStr.Replace(",", ".");

                if (!double.TryParse(precioStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double precio))
                {
                    Console.WriteLine("El precio ingresado no es un numero valido.");
                    return;
                }

                if (precio < 0)
                {
                    throw new ProductoException("El precio no puede ser negativo.");
                }

                Producto nuevoProducto = new Producto
                {
                    Id = siguienteId++,
                    Nombre = nombre,
                    Precio = Math.Round(precio, 2)
                };

                productos.Add(nuevoProducto);
                Console.WriteLine($"Producto agregado con éxito\n ID asignado: {nuevoProducto.Id}");
            }
            catch (Exception ex)
            {
                throw new ProductoException($"Error al agregar producto: {ex.Message}");
            }
        }

        static void EliminarProducto()
        {
            try
            {
                Console.WriteLine("=== ELIMINAR PRODUCTO ===");

                if (productos.Count == 0)
                {
                    throw new ProductoException("No hay productos registrados para eliminar.");
                }

                Console.Write("Ingrese el ID del producto a eliminar: ");
                string idStr = Console.ReadLine();

                if (!int.TryParse(idStr, out int id))
                {
                    throw new ProductoException("El ID debe ser un número entero.");
                }

                Producto producto = productos.FirstOrDefault(p => p.Id == id);

                if (producto == null)
                {
                    throw new ProductoException($"No se encontro ningun producto con ID {id}.");
                }

                productos.Remove(producto);
                Console.WriteLine($"Producto con ID {id} eliminado correctamente.");
            }
            catch (ProductoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ProductoException($"Error al eliminar producto: {ex.Message}");
            }
        }

        static void BuscarProducto()
        {
            try
            {
                Console.WriteLine("=== BUSCAR PRODUCTO ===");

                if (productos.Count == 0)
                {
                    throw new ProductoException("No hay productos registrados para buscar.");
                }

                Console.Write("Ingrese el nombre del producto a buscar: ");
                string nombreBusqueda = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(nombreBusqueda))
                {
                    throw new ProductoException("El criterio de busqueda no puede estar vacio.");
                }

                var resultados = productos.Where(p => p.Nombre.ToLower().Contains(nombreBusqueda.ToLower())).ToList();

                if (resultados.Count > 0)
                {
                    Console.WriteLine($"Se encontraron {resultados.Count} productos:");
                    foreach (var producto in resultados)
                    {
                        Console.WriteLine(producto);
                    }
                }
                else
                {
                    Console.WriteLine("No se encontraron productos con ese nombre.");
                }
            }
            catch (ProductoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ProductoException($"Error al buscar producto: {ex.Message}");
            }
        }

        static void MostrarProductos()
        {
            try
            {
                Console.WriteLine("=== LISTA DE PRODUCTOS ===");

                if (productos.Count == 0)
                {
                    Console.WriteLine("No hay productos registrados.");
                    return;
                }

                Console.WriteLine($"Total de productos: {productos.Count}");
                foreach (var producto in productos)
                {
                    Console.WriteLine(producto);
                }
            }
            catch (Exception ex)
            {
                throw new ProductoException($"Error al mostrar productos: {ex.Message}");
            }
        }
    }


    public class ProductoException : Exception
    {
        public ProductoException(string mensaje) : base(mensaje){}
    }
}
