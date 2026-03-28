using System.ComponentModel;
using System.Data.Common;
using MyDotNetApp.Models;

namespace MyDotNetApp.Services;
/// <summary>
/// In-Memory implementation of the IProcducntService Interface.
/// In a real  app this would talk to  a databse (e.g. Entity Framework + SQL Server).
/// For Learning  CI/CD we keep it Simple  - no  databse  needed.
/// This class provides methods to perform CRUD operations on products stored in memory.
/// </summary>

// this line means that the ProductService class implements the IProductService interface,
// which defines the contract for product operations.
public class ProductService : IProductService
{
    //----- Seed  data  -- Pre-loaded when the  app starts-------------------------
    // means that the list of products is initialized with some sample data when the application
    //  starts, allowing us to test the functionality of the service without needing to create products manually.

    //this line means that a private readonly field named _products is declared, which is a list of Product objects. 
    // List<Product> means that it is a list that can hold multiple Product objects, and the new () syntax is used to initialize the list with an empty collection of products.
     private  readonly List<Product> _products = new ()
     {
        // This is a collection initializer that adds several Product objects to the _products list when it is created.
        //  Each Product object is initialized with specific values for its properties,
        //  such as Id, Name, Price, Stock, and Category.

           new Product {    Id = 1, Name = "Laptop",     Price = 999.99m, Stock=10, Category="Electronics" },
           new Product {    Id = 2, Name = "Desk Chair", Price  =  2500.00m, Stock = 5, Category="Furniture" },
           new Product {    Id = 3, Name = "Notebook",   Price =15.99m, Stock = 100, Category = "Stationery"},
           new Product {    Id = 4, Name = "Coffee Mug", Price =30.00m, Stock = 50, Category = "kitchen" },
           new Product {    Id = 5, Name = "USB-C Hub",   Price = 85.99m, Stock = 40, Category = "Electronics" }
     };

     private int _nextId = 6; // This field is used to keep track of the next available unique identifier for new products.

     // Read only Operations --------------------------------------------------
     // => means that the method is implemented as an expression-bodied member, which is a concise way to define a method that consists of a single expression.
        // This method retrieves a product by its unique identifier (id) from the in-memory list of products.
     public IEnumerable<Product> GetAll() => _products; // This method returns the entire list of products stored in memory.

     // 1. The method takes an integer parameter named id, which represents the unique identifier of the product to be retrieved.
     // 2. The method uses the FirstOrDefault LINQ method to search through the _products list for a product that matches the specified id. 
     // The lambda expression p => p.Id == id is used to define the search criteria, where p represents each product in the list.
     // 3. If a matching product is found, it is returned; otherwise, null  is returned.
     //  The return type of the method is Product?, which indicates that it can return either a Product object or null. 
     public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id); 
     // This method searches for a product with the specified id and returns it if found, or null if no such product exists.

// Write Operations --------------------------------------------------

 // this line means that the Create method is defined to create a new product based
 //  on the information provided in a CreateProductRequest object.
    public Product Create(CreateProductRequest request)
    {
        // validation  - throw if invalid (Controller  catches this )

        //  this line means that the method checks if the Name property of the request object is null,
        //  empty, or consists only of whitespace characters.
        // If the Name is invalid, an ArgumentException is thrown with the message "Product Name cannot be empty.
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new  ArgumentException("Product Name cannot be empty.");

        if (request.Price < 0)
            throw new ArgumentException("Product Price cannot be negative.");
        
        if(request.Stock < 0)
            throw new ArgumentException("Product Stock cannot be negative.");
        
        var product = new Product
        {
            Id = _nextId++, 
           // This line assigns a unique identifier to the new product by using 
           // the _nextId field and then increments it for the next product. 
            Name  = request.Name.Trim(),
           // This line assigns the Name property of the new product based on the Name property of the request object,
           //  after trimming any leading or trailing whitespace characters from the Name value.
            Price  = request.Price,
              // This line assigns the Price property of the new product based on the Price property of the request object.
            Stock = request.Stock,
              // This line assigns the Stock property of the new product based on the Stock property of the request object.
            Category = request.Category.Trim(),
               // This line assigns the Category property of the new product based on the Category property of the request object, after trimming any leading or trailing whitespace characters from the Category value.
            CreatedAt  = DateTime.UtcNow
               // This line assigns the CreatedAt property of the new product to the current date and time in Coordinated Universal Time (UTC) format, indicating when the product was created.           
        };

        // This line adds the newly created product to the in-memory list of products, 
        // allowing it to be stored and retrieved later.
        _products.Add(product); 
        return product; 
        // This line returns the newly created product object to the caller, allowing it to be used or returned in a response.
    }

    // this line means that the Update method is defined to update an existing product
    //  based on the information provided in a CreateProductRequest object and the unique identifier (id) of the product to be updated.
    public bool Update(int id, CreateProductRequest request)
    {
        //the method attempts to retrieve the product with the specified id using the GetById method.
         var product = GetById(id);

         // this line checks if the retrieved product is null,
         //  which would indicate that no product with the specified id exists in the in-memory list.
         if (product == null) return false; // Product not found

         //validation
         // this line checks if the Name property of the request object is null, empty, or consists only of whitespace characters.
            // If the Name is invalid, an ArgumentException is thrown with the message "Product Name cannot be empty.
         if(string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Product Name cannot be empty.");
        if(request.Price < 0)
            throw new ArgumentException("Product Price cannot be negative.");
        if(request.Stock < 0)
            throw new ArgumentException("Product Stock cannot be negative.");
           

           //this line updates the properties of the existing product based on the values provided in the request object.
            
            product.Name = request.Name.Trim();
            // this line updates the Name property of the existing product based on the Name property of the request object, after trimming any leading or trailing whitespace characters from the Name value.

            product.Price = request.Price;
            // this line updates the Price property of the existing product based on the Price property of the request object.

            product.Stock = request.Stock;
            // this line updates the Stock property of the existing product based on the Stock property of the request object.

            product.Category =  request.Category.Trim();
            //this line updates the Category property of the existing product based on the Category property of the request object, after trimming any leading or trailing whitespace characters from the Category value.
       
          // this line returns true to indicate that the update operation was successful, 
          // meaning that the product was found and its properties were updated with the new values provided in the request object.

            return true; // Update successful
    }


//this line means that the Delete method is defined to delete an existing product based on the unique identifier (id) of the product to be deleted.
    public bool Delete(int id)
    {

        // this line means that the method 
        // attempts to retrieve the product with the specified id using the GetById method.
         var product  = GetById(id);

          // this line means that the method checks if the retrieved product is null, which would indicate that no product with the specified id exists in the in-memory list. If the product is not found, 
          // the method returns false to indicate that the delete operation was unsuccessful.
         if (product == null) return false; // Product not found

        //this line means that the method removes the retrieved product from the
        //  in-memory list of products using the Remove method of the List<Product> class.
         _products.Remove(product);

        // this line means that the method returns true to indicate that the delete operation was successful,
        //  meaning that the product was found and removed from the in-memory list.
            return true; // Delete successful
    }


    



}