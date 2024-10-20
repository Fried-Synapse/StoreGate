using System.Runtime.Serialization;

namespace StoreGate.GitHub.Models;

[DataContract]
public record Variable
{
    [DataMember]
    public string? Name { get; set; }
    [DataMember]
    public string? Value { get; set; }
}