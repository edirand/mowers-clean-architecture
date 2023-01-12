namespace Mowers.CleanArchitecture.Api.Models;

public class FileProcessing
{
    public string Id { get; set; }
    
    public bool Completed { get; set; }
    
    public bool Success { get; set; }
    
    public IEnumerable<string> Mowers { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}