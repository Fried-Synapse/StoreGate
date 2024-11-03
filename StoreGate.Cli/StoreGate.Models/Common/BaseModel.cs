using System.Runtime.Serialization;

namespace StoreGate.Models.Common;

[DataContract]
public record BaseIntModel
{
    [DataMember]
    public int Id { get; set; }
}