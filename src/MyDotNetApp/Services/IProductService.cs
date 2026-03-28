using MyDotNetApp.Models;

namespace MyDotNetApp.Services;
/// <summary>
/// Contract (Interface) for product operations.
/// Controller  depends on this  interface , not  on the concrete class.
///  means that the controller can work with any implementation of this interface, 
/// which promotes loose coupling and makes the code more flexible and testable.
/// This makes  the code easy to test and  swap implementations without changing the controller code.
///</summary>

public interface  IProductService
{
    // Method to get all products, returns an IEnumerable of Product objects means
    //  that it returns a collection of Product objects that can be enumerated (iterated) over.
   IEnumerable<Product> GetAll();

   // Method to get a product by its unique identifier (id), returns a Product object or null if not found means
    // that it returns a single Product object that matches the specified id, or null if no such product exists.
   Product?   GetById(int id);

   // Method to create a new product, takes a CreateProductRequest object as input and returns the created Product object means
    // that it takes a CreateProductRequest object as input, which contains the necessary information to create a new product, 
    // and returns the created Product object after it has been successfully created.
   Product  Create(CreateProductRequest request);

// Method to update an existing product, takes the product's unique identifier (id) and a CreateProductRequest object as input, returns a boolean indicating success or failure means
    // that it takes the product's unique identifier (id) and a CreateProductRequest object as input, which contains the updated information for the product, 
    // and returns a boolean value indicating whether the update operation was successful (true) or not (false).
   bool Update(int id, CreateProductRequest request);

// Method to delete a product by its unique identifier (id), returns a boolean indicating success or failure means
    // that it takes the product's unique identifier (id) as input and returns a boolean value indicating whether the delete operation was successful (true) or not (false).
   bool Delete(int id);
}

