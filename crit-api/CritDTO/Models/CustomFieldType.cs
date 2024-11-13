namespace CritDTO.Models;

public class CustomFieldType
{
    public CustomFieldType(string name, Guid projectId)
    {
        Name = name;
        ProjectId = projectId;
    }

    private CustomFieldType()
    {
        Name = "";
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid ProjectId { get; set; }
}
