using System.Runtime.Serialization;

namespace StoreGate.Common.Models;

[DataContract]
public record BaseIntModel
{
    [DataMember]
    public int Id { get; set; }
}