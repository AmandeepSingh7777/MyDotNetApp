using System.Reflection;
using MyDotNetApp.Models;
using MyDotNetApp.Services;
using Xunit;

namespace MyDotNetApp.Test;
/// <summary> Unit Test - test  the productService in isolation without depending on the actual implementation of the service,
/// (no HTTP requests, no database, no external dependencies, no server).
/// Each test is fast, reliable, and focused on  one behaviour.
/// </summary

public class ProductServiceTests
{
    //  Create a fresh service for every test
    private  readonly ProductService _service = new ();

    // Test 1 --------------------------------------------------------------
    //Does GetAll() return products ?
    [Fact]
    public void GetAll_WhenCalled_ReturnsAllProducts()
    {
        // Arrange - nothing to setup,  service already  has  seed data

        // Act - call the method we want to test

        var result = _service.GetAll().ToList();
        /// Assert - verify that the result is what we expect
        
        // Assert - check  we got  Products back

        Assert.NotNull(result); // check that the result is not null, which means that we got a collection of products back from the GetAll() method.

        Assert.Equal(5, result.Count); // we seeded 5 products in the service constructor exactly 5 products should be returned.
    }


    // Test 2 --------------------------------------------------------------
    //==================================================================================================
    // GetById() returns a product when given a valid id
    // Does GetById() return the correct product when given a valid id?
    //====================================================================================================
    [Fact] // fact attribute indicates that this method is a test method that should be executed by the test runner.
    public void GetById_ExistingId_ReturnsCorrectProduct() // this test method is named GetById_ExistingId_ReturnsCorrectProduct, 
    // which follows the naming convention of MethodName_Condition_ExpectedResult.
    {
        // Arrange - we know that the product with id 1 exists in the seed data, so we can use that id for our test.
         var expectedName = "Laptop"; // we define a variable expectedName and assign it the value "Laptop",
 
         // Act - call the method we want to test
        var product = _service.GetById(1); // we know that the product with id 1 exists in the seed data, 
        // so we call the GetById method with id 1 to retrieve that product.

        // Assert - verify that the result is what we expect
        Assert.NotNull(product); // we assert that the product is not null,
        //  which means that we successfully retrieved a product Existed.

        Assert.Equal(1, product.Id); // we assert that the id of the retrieved product is 1,
        // which confirms that we got the correct product based on the {id} we requested.

        Assert.Equal(expectedName, product.Name); // we assert that the name of the retrieved product existed is "Laptop",
        // which confirms that we got the correct product based on the expected {Name}.

    }

    // Test 3--------------------------------------------------------------
    // Does GetById() return null for missing product?
    [Fact]
    public void GetById_NonExistingId_ReturnsNull()
    {
        // Act - call the method we want to test with an id that does not exist in the seed data (e.g., 999)
        var product = _service.GetById(999); // we call the GetById method with an id (999) that we know does not exist in the seed data to test how the method handles a request for a non-existing product.
       
       // Assert - verify that the result is what we expect - in this case, we expect the result to be null since there is no product with id 999 in the seed data.
        Assert.Null(product); // we assert that the product is null, which means that the GetById method correctly returns null when a product with the specified id does not exist in the system.

    }

    // Test 4--------------------------------------------------------------
    // Does Create() add a new product Correctly?
    [Fact]
    public void Create_ValidRequest_ReturnsProductWithGeneratedId()
    {
        // Arrange - create a new product request with valid data
        var request = new CreateProductRequest
        {
            Name = "Mechanical Keyboard",
            Price = 1500.00m,
            Stock = 20,
            Category = "Electronics"
        };

        // Act - call the Create method to add the new product
        var CreatedProduct = _service.Create(request); // we call the Create method of the product service with the valid product request to create a new product in the system.

        // Assert - verify that the created product has the expected properties
        Assert.NotNull(CreatedProduct); // we assert that the created product is not null, which means that the Create method successfully created a new product based on the provided request.
        Assert.True(CreatedProduct.Id > 0); // we assert that the id of the created product is greater than 0, which confirms that the Create method assigned a unique identifier to the new product.
        Assert.Equal("Mechanical Keyboard", CreatedProduct.Name); // we assert that the name of the created product matches the name provided in the request, which confirms that the Create method correctly set the product's name based on the input data.
        Assert.Equal(1500.00m, CreatedProduct.Price); // we assert that the price of the created product matches the price provided in the request, which confirms that the Create method correctly set the product's price based on the input data.
        Assert.Equal(20, CreatedProduct.Stock); // we assert that the stock quantity of the created product matches the stock provided in the request, which confirms that the Create method correctly set the product's stock based on the input data.
    }

// Test 5--------------------------------------------------------------
    [Fact]
    public void Create_ValidRequest_ProductAppearsGetAll()
    {
        var request = new CreateProductRequest
        {
            Name = "Monitor",
            Price = 15000.00m,
            Stock = 20, 
            Category = "Electronics"
        };

        var created = _service.Create(request); // we call the Create method of the product service with the valid product request to create a new product in the system.
        var found = _service.GetById(created.Id); // we call the GetById method of the product service with the id of the created product to retrieve that product from the system.

        Assert.NotNull(found); // we assert that the found product is not null, which means that we successfully retrieved the product that was just created.
        Assert.Equal("Monitor", found.Name); // we assert that the name of the found product matches the name provided in the request, which confirms that the product we retrieved is indeed the one we created.
    }

// Test 6-------------------------------------------------------------
    [Fact]
    public void Create_WhiteSpaceName_TrimsAndSaves()
    {
        var request  = new CreateProductRequest
        {
               Name = "   Mouse   ", // we create a new product request with a name that contains leading and trailing whitespace to test how the Create method handles such input.
                Price = 150.00m,
                Stock = 50,
                Category = "Electronics"   
        };
          // act - call the Create method to add the new product with a name that has leading and trailing whitespace
        var created = _service.Create(request); // we call the Create method of the product service with the product request that contains a name with leading and trailing whitespace to create a new product in the system.
        
        Assert.Equal("Mouse", created.Name); // we assert that the name of the created product is "Mouse" without the leading and trailing whitespace, which confirms that the Create method correctly trims the whitespace from the product name before saving it to the system.
    }

    //============================================================================================
    // Create - Validation failures
    //============================================================================================
    // Test 7--------------------------------------------------------------
    [Fact]
    public void Create_EmptyName_ThrowsArgumentException()
    {
        var request = new CreateProductRequest
        {
            Name ="", // we create a new product request with an empty name to test how the Create method handles invalid input.
            Price = 10.00m,    
            Stock = 50,
            Category = "Test"
        };

        Assert.Throws<ArgumentException>(() => _service.Create(request)); // we assert that calling the Create method with the product request that has an empty name throws an ArgumentException,
        //  which confirms that the Create method correctly validates the input and does not allow creating a product with an empty name.
    }

// Test 8--------------------------------------------------------------
    [Fact]
    public void Create_WhiteSpaceName_ThrowsArgumentException()
    {
        var request = new CreateProductRequest
        {
            Name = "   ", // we create a new product request with a name that contains only whitespace to test how the Create method handles such invalid input.
            Price = 10.00m,
            Stock = 50,
            Category = "Test"
        };

        Assert.Throws<ArgumentException>( () => _service.Create(request)); // we assert that calling the Create method with the product request that has a name containing only whitespace throws an ArgumentException,
        // which confirms that the Create method correctly validates the input and does not allow creating a product. with a name that is empty or contains only whitespace.
    }
    // Test 9--------------------------------------------------------------
    [Fact]
    public void Create_NegativePrice_ThrowsArgumentException()
    {
        var request = new CreateProductRequest
        {
            Name = "Widget",// we create a new product request with a valid name to isolate the test case to only the price validation.
            Price = -10.00m, // we create a new product request with a negative price to test how the Create method handles such invalid input.
            Stock = 5, // we create a new product request with a valid stock quantity to isolate the test case to only the price validation.
            Category = "Test" // we create a new product request with a valid category to isolate the test case to only the price validation.
        };
    }
// Test 10--------------------------------------------------------------
    [Fact]
    public void Create_NegativeStock_ThrowsArgumentException()
    {
        var request = new CreateProductRequest
        {
            Name = "Widget", // we create a new product request with a valid name to isolate the test case to only the stock validation.
            Price = 10.00m, // we create a new product request with a valid price to isolate the test case to only the stock validation.
            Stock = -1, // we create a new product request with a negative stock quantity to test how the Create method handles such invalid input.
            Category = "Test" // we create a new product request with a valid category to isolate the test case to only the stock validation.
        };

        Assert.Throws<ArgumentException>(() => _service.Create(request)); // we assert that calling the Create method with the product request that has a negative stock quantity throws an ArgumentException,
        // which confirms that the Create method correctly validates the input and does not allow creating a product with a negative stock quantity.
    }

// Test 11--------------------------------------------------------------
    [Fact]
    public void Create_ZeroPrice_ThrowsArgumentException()
    {
        var request = new CreateProductRequest
        {
            Name = "Free Sample", // we create a new product request with a valid name to isolate the test case to only the price validation.
            Price = 0.00m, // we create a new product request with a price of zero to test how the Create method handles such invalid input.
            Stock = 100, // we create a new product request with a valid stock quantity to isolate the test case to only the price validation.
            Category = "Promo" // category is valid to isolate the test case to only the price validation.
        };

        var created = _service.Create(request); // we call the Create method of the product service with the product request that has a price of zero to create a new product in the system.

        Assert.Equal(0.00m, created.Price); // we assert that the price of the created product is 0.00, which confirms that the Create method allows creating a product with a price of zero and correctly sets the product's price based on the input data.)
    }


    //====================================================================================================
    // UPDATE
    //====================================================================================================
// Test 12--------------------------------------------------------------
    [Fact]
    public void Update_ExistingProduct_ReturnsTrueAndUpdatesFields()
    {
        var request = new CreateProductRequest
        {
            Name = "Gaming Laptop",
            Price = 150000.00m,
            Stock = 5,
            Category = "Electronics"
        };

        var result = _service.Update(1, request);
        var updated = _service.GetById(1);

        Assert.True(result);
        Assert.Equal("Gaming Laptop", updated!.Name);
        Assert.Equal(150000.00m, updated.Price);
        Assert.Equal(5, updated.Stock);
    }

// Test 13--------------------------------------------------------------
    [Fact]
    public void Update_NonExistingProduct_ReturnsFalse()
    {
        var request = new CreateProductRequest
        {
            Name = "X",
            Price = 1m,
            Stock = 1,
            Category = "Y"
        };

        var result = _service.Update(9999, request);
        Assert.False(result);
    }


    //=============================================================================================
    // DELETE
    //=============================================================================================
// Test 14--------------------------------------------------------------
    [Fact]
    public void Delete_ExistingProduct_ReturnsTrueAndRemovesIt()
    {
        var result = _service.Delete(2);

        Assert.True(result);

        Assert.Null(_service.GetById(2)); // Gone
    }
    
// Test 15--------------------------------------------------------------
    [Fact]
    public void Delete_NonExistingProduct_ReturnsFalse()
    {
        var result = _service.Delete(9999);

        Assert.False(result);
    }

// Test 16--------------------------------------------------------------
    [Fact]
    public void Delete_ProductTwice_SecondDeleteReturnsFalse()
    {
        _service.Delete(3); ///  first delete - Success
        var result = _service.Delete(3); // Second delete product gone.

        Assert.False(result);
    }


}