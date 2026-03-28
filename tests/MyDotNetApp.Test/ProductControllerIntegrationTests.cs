using  System .Net;
using  System.Net.Http.Json;
using  Microsoft.AspNetCore.Mvc.Testing;
using  MyDotNetApp.Models;
using  Xunit;

namespace MyDotNetApp.Tests;

/// <summary>
/// INTERGRATION TESTS - spins up a real  in-memory http server and tests the full Request / Response cycle,
/// just like real client would.
/// No Network needed - everthing run inside the test process.
/// </summary>

public class ProductControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
 
    public ProductControllerIntegrationTests(WebApplicationFactory<Program> factory) // this is the constructor
    {
        // Create a  real test server running  our  app  in memory
        _client  = factory.CreateClient();
    }

//==============================================================================================
// GET /api/products
//==============================================================================================

[Fact]
public async Task GetAll_Returns200OK()
    {
        var response = await _client.GetAsync("/api/products");

         Assert.Equal(HttpStatusCode.OK, response.StatusCode);   

    }
    
[Fact]
public async Task GetAll_ReturnsJsonContentType()
    {
        var response = await _client.GetAsync("/api/products");
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }

[Fact]
public async Task GetAll_ReturnsSeedProducts()
    {
        var products  = await _client.GetFromJsonAsync<List<Product>>("/api/products");
        
        Assert.NotNull(products);
        Assert.True(products.Count >= 5); // we seeded 5 Products
    }


    //================================================================================================
    // GET /api/products/{id}
    //================================================================================================

    [Fact]
    public async Task GetById_ExistingId_Returns200WithProduct()
    {
        var  response = await _client.GetAsync("/api/products/1");
        var product   = await response.Content.ReadFromJsonAsync<Product>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(product);
        Assert.Equal(1, product.Id);
        Assert.Equal("Laptop", product.Name);
    }


    [Fact]
    public async Task GetById_NonExistingId_Returns404()
    {
        var response = await _client.GetAsync("/api/products/9999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }


    //=========================================================================================
    // POST /api/products
    //=========================================================================================

    [Fact]
    public async Task Create_ValidProduct_Returns201Created()
    {
        var Request = new CreateProductRequest
        {
            Name = "Wireless Mouse",
            Price = 50.00m,
            Stock = 50,
            Category = "Electronics"
        };

        var response = await _client.PostAsJsonAsync("/api/products", Request);
        var created  = await response.Content.ReadFromJsonAsync<Product>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(created);
        Assert.Equal("Wireless Mouse", created.Name);
        Assert.Equal(50.00m, created.Price);
        Assert.True(created.Id > 0);
    }

    [Fact]
    public async Task Create_ValidProduct_LocationHeaderPointsToNewProduct()
    {
        var request = new CreateProductRequest
        {
            Name = "Headphones",
            Price = 80.00m,
            Stock = 25,
            Category = "Electronics"
        };

        var response = await _client.PostAsJsonAsync("/api/Products", request); // problem is p to P
        var created  = await response.Content.ReadFromJsonAsync<Product>();

        // Location header should point to /api/products/{newId}
        Assert.NotNull(response.Headers.Location);
        Assert.Contains($"/api/Products/{created!.Id}", response.Headers.Location.ToString());
    }


    [Fact]
    public async Task Create_EmptyName_Returns400BadRequest()
    {
        var  request = new CreateProductRequest
        {
            Name = "",
            Price = 30.00m,
            Stock = 15,
            Category = "Test"
        };

        var response = await _client.PostAsJsonAsync("/api/products", request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

    }

    [Fact]
    public async Task Create_NegativePrice_Returns400BadRequest()
    {
        var request = new CreateProductRequest
        {
            Name = "Widget",
            Price = -5m,
            Stock = 5,
            Category = "Test"
        };

        var response = await  _client.PostAsJsonAsync("/api/products", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    //============================================================================================
    // PUT /api/products/{id}
    //============================================================================================

    [Fact]
    public async Task Update_ExistingProduct_Returns204NoContent()
    {
        var request = new CreateProductRequest
        {
            Name = "Updated Chair",
            Price = 25000.00m,
            Stock = 50,
            Category = "Furniture"
        };
    }

    [Fact]
    public async Task Update_NonExistingProduct_Returns404()
    {
        var request = new CreateProductRequest
        {
            Name = "X",
            Price = 100m, 
            Stock = 1,
            Category = "Y"
        };
        var response = await _client.PutAsJsonAsync("/api/products/9999", request);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }

    //===============================================================================================
    // DELETE /api/products/{id}
    //===============================================================================================
    
    [Fact]
    public async Task Delete_ExistingProduct_Returns204NoContent()
    {
        var response = await _client.DeleteAsync("/api/products/5");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ExistingProduct_ProductNoLongerFound()
    {
         await _client.DeleteAsync("/api/products/4");

         var getResponse = await _client.GetAsync("/api/products/4");

         Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);

    }

    [Fact]
    public async Task Delete_NonExistingProduct_Returns404()
    {
        var response = await _client.DeleteAsync("/api/products/9999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

} 
