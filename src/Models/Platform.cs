using System.ComponentModel.DataAnnotations;

namespace GraphQLPoC.Models;

[GraphQLDescription("Represents any software or service that has a command line interface")]
public class Platform
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [GraphQLDescription("Represents a purchased, valid licence for the platform")]
    public string LicenseKey { get; set; }
    public ICollection<Command> Commands { get; set; } = new List<Command>();
}