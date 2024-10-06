using System.Runtime.Serialization;
using StoreGate.Common.Models;

namespace StoreGate.GitHub.Models;

[DataContract]
public record Release : BaseIntModel
{
    public Release()
    {
    }

    public Release(Version version)
    {
        Name = $"v{version}";
        TagName = Name;
    }

    [DataMember]
    public string? Name { get; set; }
    [DataMember(Name = "tag_name")]
    public string? TagName { get; set; }
    [DataMember(Name = "generate_release_notes")]
    public bool GenerateReleaseNotes { get; set; } = true;
    [DataMember(Name = "prerelease")]
    public bool PreRelease { get; set; }
    [DataMember(Name = "upload_url")]
    public string? UploadUrl { get; set; }
}