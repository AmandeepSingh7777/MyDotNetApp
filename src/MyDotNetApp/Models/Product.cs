namespace MyDotNetApp.Models;
/// <summary>
/// Represents a product stored in the system
/// </summary>


// product model with properties and default values 
// Each property has a comment describing its purpose, whether it's required, and its default value
//public means that the property can be accessed from outside the class, and set means that it can be modified
//get means that the property can be read, and set means that it can be modified
//private means that the property can only be accessed from within the class, 
//and public means that it can be accessed from outside the class
public class Product
{
    // Unique identifier for the product // Required field // Default value is 0
    
    public int Id {get; set;}
    
    // Name of the product // Required field // Default value is an empty string
    public string Name {get; set;} = string.Empty;     

    // Price of the product // Required field // Default value is 0.0
    public decimal Price {get; set;}

    // Quantity of the product in stock // Required field // Default value is 0
    public  int Stock {get; set;}
    
    // Category of the product // Required field // Default value is an empty string
    public  String Category {get; set;} = string.Empty;

    // Date and time when the product was created // Required field 
    // // Default value is the current date and time
    //utcnow is used to get the current date and time in Coordinated Universal Time (UTC) format, 
    // which is a standard time format that is not affected by time zones or daylight saving time
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

}

/// <summary>
/// The data a client send  when creating or updating a product
/// </summary>

// create product request model with properties and default values
public class CreateProductRequest
{
    // Name of the product // Required field // Default value is an empty string
    public string Name {get; set;} = string.Empty;

    // Price of the product // Required field // Default value is 0.0
    public decimal Price {get; set;}

    // Quantity of the product in stock // Required field // Default value is 0
    public int Stock {get; set;}

    // Category of the product // Required field // Default value is an empty string
    public string Category {get; set;} = string.Empty;
    
} 
