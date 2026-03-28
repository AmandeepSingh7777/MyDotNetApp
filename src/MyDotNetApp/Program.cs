using MyDotNetApp.Services;
var builder = WebApplication.CreateBuilder(args);

//Register Services (Dependency Injection) -------------------------


builder.Services.AddControllers();
/// the AddControllers method is used to register the necessary services for handling HTTP requests and responses in an ASP.NET Core application.
///   it can create  probelm details schema for error responses,
///  and it can also handle model validation errors by automatically returning a 400 Bad Request response with details about the validation errors.

builder.Services.AddEndpointsApiExplorer();
/// the AddEndpointsApiExplorer method is used to enable the generation of API documentation for minimal APIs, which are a simplified way to define API endpoints in ASP.NET Core.
builder.Services.AddSwaggerGen(c =>
/// the AddSwaggerGen method is used to register the Swagger generator services, which are responsible for generating the Swagger documentation for the API. The c parameter is a configuration object that allows you to customize the generated Swagger document.

{
    // this method is used to configure the Swagger document with specific information about the API, such as its title, version, and description.
    // the c parameter represents the SwaggerGenOptions object, which allows you to customize the generated Swagger document.
    // the SwaggerDoc method is used to define a Swagger document with a specific name (in this case, "v1") and provide metadata about the API.
    /// the SwaggerDoc method takes two parameters: the first is the name of the document (in this case, "v1"), and the second is an instance of OpenApiInfo that contains metadata about the API, such as its title, version, and description.
    c.SwaggerDoc("v1", new ()
    /// the OpenApiInfo class is used to provide metadata about the API, such as its title, version, and description. In this case,
    
    {
        /// the Title property sets the title of the API in the Swagger documentation.
        Title ="MyDotNetApp - Product API",
        /// the Version property sets the version of the API in the Swagger documentation.
        Version = "v1",
        /// the Description property provides a brief description of the API in the Swagger documentation.
        Description = "A simple Product Api to learn  Ci/Cd  with .Net"
    });
});

// Register our ProductService  so  Controllers can use it via  Dependency Injection
builder.Services.AddSingleton<IProductService, ProductService>();
/// the AddSingleton method is used to register a service with a singleton lifetime,
///  meaning that only one instance of the service will be created and shared throughout 
/// the application's lifetime. In this case, we are registering the 
/// IProductService interface with its implementation ProductService,
///  allowing controllers to use it via dependency injection.

var app = builder.Build();

// Middleware Pipeline Configuration ---------------------------------
//if (app.Environment.IsDevelopment())  // this use for  development environment when i start project.
/// the IsDevelopment method is used to check if the application is running in a development environment. If it returns true, the code inside the if block will be executed, which typically includes middleware and configurations that are specific to the development environment, such as enabling Swagger UI for API documentation.
//{  // open this comment when  if condition work '{'
     app.UseSwagger();
     /// the UseSwagger method is used to enable the Swagger middleware, which generates the Swagger JSON document for the API. This document describes the API endpoints, request/response models, and other relevant information about the API.
     app.UseSwaggerUI(c =>
     /// the UseSwaggerUI method is used to enable the Swagger UI middleware, 
     /// which provides a user-friendly interface for exploring and testing 
     /// the API endpoints defined in the Swagger document. 
     /// The c parameter is a configuration object that allows you to customize 
     /// the Swagger UI.
     {
         c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyDotNetApp API v1");
         /// the SwaggerEndpoint method is used to specify the endpoint where the 
         /// Swagger JSON document can be accessed. The first parameter is the URL
         ///  path to the Swagger JSON document (in this case, "/swagger/v1/swagger.json"),
         ///  and the second parameter is a name that will be displayed in the Swagger UI
         ///  for this endpoint (in this case, "MyDotNetApp API v1").
         
         c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root "/"
         /// the RoutePrefix property is used to specify the route prefix
         ///  for the Swagger UI. By setting it to an empty string, 
         /// we are configuring the Swagger UI to be accessible at the root URL 
         /// of the application ("/"). This means that when you navigate to the application's 
         /// base URL, you will see the Swagger UI instead of a default landing page.
     });

//} // open this comment when  if condition work  same here '}'

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();


// Required so integration tests can reference this class
public partial class Program { }