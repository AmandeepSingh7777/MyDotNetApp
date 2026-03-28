using Microsoft.AspNetCore.Mvc;
using MyDotNetApp.Models;
using MyDotNetApp.Services;
// this line means that the code in this file is using the Microsoft.AspNetCore.Mvc
// namespace, which contains classes and interfaces for building web APIs and handling 
// HTTP requests and responses. It also means that the code is using the MyDotnetApp.Models namespace,
// which likely contains the Product model class, and the MyDotnetApp.Services namespace,
// which likely contains the IProductService interface for managing products.


namespace MyDotNetApp.Controllers;
/// <summary>
/// REST API for managing products.
/// Base Route: /api/products
/// Controller for managing products. 
/// It handles HTTP requests related to products and interacts with the 
/// IProductService to perform operations.
/// The controller is decorated with the [ApiController] attribute,
///  which enables API-specific behaviors and 
/// the [Route] attribute, which defines the base route for all actions in this controller.
/// </summary>

/// api controller means that this class is an API controller,
///  which is a type of controller in ASP.NET Core that is designed to
///  handle HTTP requests and return data in a format such as JSON.
[ApiController]

/// Route ("api/[controller]") means that the base route for all actions in this
///  controller will be "api/products",
/// where [controller] is a placeholder that will be replaced with the 
/// name of the controller ( without the "Controller" suffix).
[Route("api/[controller]")]

/// Produces ("application/json") means that this controller will return responses in JSON format by default.
/// This attribute indicates that the controller's actions will produce responses with the "application/json"
///  content type, which is a common format for APIs to return data.
[Produces("application/json")]


// this line means that the ProductsController class is a controller that inherits from ControllerBase,
// which is a base class for API controllers in ASP.NET Core. The controller has a private
// readonly field of type IProductService, which is used to interact with the product service for performing operations related to products. 
// The constructor of the controller will likely take an IProductService as a parameter and assign it to the _productService field for use in the controller's actions.
public class ProductsController : ControllerBase
{
     private readonly IProductService _service;

     // .Net injects the IProductService  automatically (Dependency Injection)

     // this line means that the constructor of the ProductsController class takes an IProductService as a parameter and assigns it to the _productService field.
     // This allows the controller to use the product service for performing operations related to products, such as retrieving, creating, updating, and deleting products. The comment indicates that
     //  .NET will automatically inject the IProductService implementation when the controller is instantiated, which is a feature of dependency injection in ASP.NET Core.
     public ProductsController(IProductService service)
    {
        _service = service;
    }

    // ------GET /api/products------------------------------------------------------------
    /// <summary> Get all the products stored in the system. </summary>
    /// 
    /// this line means that the GetAll method is an HTTP GET action that returns an IEnumerable of Product objects.
    /// The [HttpGet] attribute indicates that this action will respond to HTTP GET requests.
    [HttpGet]

    /// this line means that the GetAll method will return a response with a status code of 200 OK 
    /// and a body containing an IEnumerable of Product objects.
    /// The [ProducesResponseType] attribute specifies the type of response that the action can return,
    ///  in this case, an IEnumerable of Product objects with a status code of 200 OK.
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    
    /// this line means that the GetAll method is an action method that will handle HTTP
    ///  GET requests to the /api/products endpoint. 
    /// The method will return an IActionResult, which is a common return type for action
    ///  methods in ASP.NET Core that allows for flexibility in returning different types
    ///  of responses (e.g., JSON, status codes, etc.). The implementation of the method is
    ///  currently empty and will need to be filled in to retrieve and return the list of products from the product service.
    public IActionResult GetAll()
    { 
        /// this line means that the GetAll method is calling the GetAll method of the _service
        ///  (which is an instance of IProductService) to retrieve a list of all products.
        ///  The result is stored in the variable products.
        var products = _service.GetAll();

        /// this line means that the GetAll method is returning an HTTP 200 OK response with the list of products in the response body.
        /// The Ok() method is a helper method that creates an OkObjectResult, which is a type of IActionResult that represents a successful response with a status code of 200 OK 
        /// and includes the specified object (in this case, the list of products) in the response body.
        /// So, when this line is executed, it will send an HTTP response back to the client with a status code of 200 OK and the list of products as the content of the response.
        return Ok(products);
    }

// ---- Get /api/products/{id} --------------------------------------------------------------
/// <summary> Get a single  product by its unique identifier (id). </summary>
/// 
/// this line means that the GetById method is an HTTP GET action that takes an integer parameter named id and returns an IActionResult.
/// The [HttpGet("{id:int}")] attribute indicates that this action will respond to HTTP GET requests to the /api/products/{id} endpoint,
///  where {id} is a placeholder for an integer value representing the unique identifier of a product. The method will need to be implemented to retrieve and return the product with the specified id from the product service.
[HttpGet("{id:int}")]

// this line means that the GetById method will return a response with a status code of 200 OK 
//and a body containing a Product object if the product is found, or a status code of 404 Not Found
// if the product is not found.
[ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]

// this line means that the GetById method will return a response with a status code of 404 Not Found
/// if the product with the specified id is not found in the system.
[ProducesResponseType(StatusCodes.Status404NotFound)]

//  public IActionResult GetById(int id) 
///means that the GetById method is an action method that will handle HTTP GET
///  requests to the /api/products/{id} endpoint,
///  where {id} is an integer representing the unique identifier of a product.
///  The method will return an IActionResult, which allows for flexibility in returning 
/// different types of responses (e.g., JSON, status codes, etc.). 
/// The implementation of the method is currently empty and will need to be filled 
/// in to retrieve and return the product with the specified id from the product service,
/// or return a 404 Not Found response if the product is not found.
public IActionResult GetById(int id)
    {

        /// this line means that the GetById method is calling the GetById method of the _service (which is an instance of IProductService) to retrieve a product by its unique identifier (id).
        /// The result is stored in the variable product. The method will need to check if the product is null (i.e., not found) and return a 404 Not Found response if it is,
        ///  or return a 200 OK response with the product in the response body if it is found.
        var product = _service.GetById(id);

        /// this line means that the GetById method is checking if the product variable is null,
        ///  which would indicate that a product with the specified id was not found in the system. 
        /// If the product is null, it returns a 404 Not Found response with a message indicating that the
        ///  product with the specified id was not found. If the product is not null (i.e., it was found),
        ///  it returns a 200 OK response with the product in the response body.
        if(product == null)
            return NotFound(new { Message = $"Product with id {id} not found."});
        return Ok(product);

    }

//----- POST /api/products --------------------------------------------------------------
/// <summary> Create a new product. </summary>
/// 
[HttpPost]
[ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]

/// this line means that the Create method is an action method that will handle HTTP POST requests to the /api/products endpoint.
/// The method takes a parameter of type CreateProductRequest,
///  which is expected to be provided in the body of the HTTP request (as indicated by the [FromBody] attribute).
///  The method will return an IActionResult, which allows for flexibility in returning different
///  types of responses (e.g., JSON, status codes, etc.). The implementation of the method is currently empty and will
///  need to be filled in to create a new product using the product service and return an appropriate response (e.g., 201 Created with the created product in the response body, or 400 Bad Request if the input is invalid).

public IActionResult Create([FromBody] CreateProductRequest request)
    {

        /// this line means that the Create method is using a 
        /// try-catch block to handle potential exceptions that may occur 
        /// during the creation of a new product.
        try
        {
            /// this line means that the Create method is calling the Create method of the _service
            ///  (which is an instance of IProductService) to create a new product using
            ///  the data provided in the request object.
            var product = _service.Create(request);
             
            // Return 201 Created  with location  header pointing the new resource.
            /// this line means that if the product is successfully created, 
            /// the Create method will return a 201 Created response with a Location 
            /// header pointing to the newly created product resource.
            /// 
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            /// this line means that the CreatedAtAction
            ///  method is a helper method that creates a CreatedAtActionResult, 
            /// which is a type of IActionResult that represents a successful response with
            ///  a status code of 201 Created.
            /// 
        }catch(ArgumentException ex)
        {
            /// this line means that if an ArgumentException
            ///  is thrown during the creation of the product (e.g., due to invalid input),
            
            // Return 400 Bad Request with error message
            return BadRequest(new { Message = ex.Message });
            /// this line means that the BadRequest method is a helper method that creates a BadRequestObjectResult, which is a type of IActionResult that represents a client error
            ///  response with a status code of 400 Bad Request and includes the specified 
            /// object (in this case, an anonymous object containing the error message)
            ///  in the response body.
        }
    }

// ----- PUT /api/products/{id} --------------------------------------------------------------
/// <summary> Update an existing product by its unique identifier (id). </summary>
/// 
/// this line means that the Update method is an action method that will handle 
/// HTTP PUT requests to the /api/products/{id} endpoint,
///  where {id} is an integer representing the unique identifier of a product.
///  The method takes an integer parameter named id and a CreateProductRequest
///  object from the body of the HTTP request. 
/// The method will return an IActionResult, which allows for flexibility
///  in returning different types of responses (e.g., JSON, status codes, etc.). 
/// The implementation of the method is currently empty and will need to be filled 
/// in to update an existing product using the product service and return an
///  appropriate response (e.g., 204 No Content if the update is successful,
///  404 Not Found if the product with the specified id is not found, or 
/// 400 Bad Request if the input is invalid).
[HttpPut("{id:int}")]
[ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]

/// this line means that the Update method is an action method 
/// that will handle HTTP PUT requests to the /api/products/{id} endpoint,
///  where {id} is an integer representing the unique identifier of a product.
///  The method takes an integer parameter named id and a CreateProductRequest 
/// object from the body of the HTTP request (as indicated by the [FromBody] attribute). 
/// The method will return an IActionResult, which allows for flexibility in returning different types of responses 
/// (e.g., JSON, status codes, etc.). 
/// The implementation of the method is currently empty and will need to be filled in to update an existing product using the product service
///  and return an appropriate response 
/// (e.g., 204 No Content if the update is successful,
///  404 Not Found if the product with the specified id is not found, or 400 Bad Request if the input is invalid).
public IActionResult Update(int id, [FromBody] CreateProductRequest request)
/// this line means that the Update method is 
/// using a try-catch block to handle potential 
/// exceptions that may occur during the update of an existing product.
    {
        try
        {

            /// this line means that the Update method is calling the Update method of the
            ///  _service (which is an instance of IProductService) to update an existing product with the specified
            ///  id using the data provided in the request object.
            var updated = _service.Update(id, request);

            /// this line means that the Update method is checking 
            /// if the update operation was successful by evaluating the updated variable.
            if(!updated)
            /// this line means that if the update 
            /// operation was not successful (i.e., updated is false), 
            /// it returns a 404 Not Found response with a message indicating that the product with the specified id was not found. 
                return NotFound(new { Message = $"Product with id {id} not found."});

                /// this line means that if the update operation was successful 
                /// (i.e., updated is true), 
                /// it returns a 204 No Content response, which indicates that the request was successful 
                /// but there is no content to return in the response body.
            return NoContent(); // 204 = Success but no body returned or content to return.
        }
        catch(ArgumentException ex)
        {
            /// this line means that if an ArgumentException is thrown during the update of the product (e.g., due to invalid input),
            /// it returns a 400 Bad Request response with a message containing the error details.
            return BadRequest(new {Message = ex.Message});
        }
    }

/// ----- DELETE /api/products/{id} --------------------------------------------------------------
/// <summary> Delete a product by its unique identifier (id). </summary>

[HttpDelete("{id:int}")]
[ProducesResponseType(StatusCodes.Status204NoContent)]  
[ProducesResponseType(StatusCodes.Status404NotFound)]
public IActionResult Delete(int id)
    {
        /// this line means that the Delete method is calling the Delete method of the _service (which is an instance of IProductService) to delete a product by its unique identifier (id).
        /// The result is stored in the variable deleted, which is likely a boolean indicating whether the
        var deleted = _service.Delete(id);
        /// this line means that the Delete method is checking if the delete operation was successful by evaluating the deleted variable.
        if(!deleted)
        /// this line means that if the delete operation was not successful (i.e., deleted is false), it returns a 404 Not Found response with a message indicating that the product with the specified id was not found.
            return NotFound(new { Message = $"Product with id {id} not found."});
        /// this line means that if the delete operation was successful (i.e., deleted is true), it returns a 204 No Content response, which indicates that the request was successful but there is no content to return in the response body.
        return NoContent();
    }

}