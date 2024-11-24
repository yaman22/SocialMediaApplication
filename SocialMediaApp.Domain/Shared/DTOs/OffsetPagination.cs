using System.ComponentModel;

namespace SocialMediaApp.Domain.Shared.DTOs;

public class OffsetPagination
{
    [DefaultValue(1)] public int PageIndex { get; set; }
    [DefaultValue(10)] public int PageSize { get; set; }
}