using System.ComponentModel;

namespace SocialMediaApp.Domain.Shared.DTOs;

public class CursorPagination
{
    [DefaultValue(null)] public Guid? NextCursor { get; set; }
    [DefaultValue(10)] public int PageSize { get; set; }
}