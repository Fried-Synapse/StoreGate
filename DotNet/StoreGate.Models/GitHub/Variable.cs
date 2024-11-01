using System.Runtime.Serialization;

namespace StoreGate.Models.GitHub;

[DataContract]
public record Variable
{
    [DataMember]
    public string? Name { get; set; }
    [DataMember]
    public string? Value { get; set; }
}