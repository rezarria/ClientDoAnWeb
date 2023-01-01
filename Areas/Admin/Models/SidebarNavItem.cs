#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Client.Areas.Admin.Models;

public class SidebarNavItem
{
	[Key]
	public Guid Id { get; set; }
	[Required(AllowEmptyStrings = false)]
	public string Name { get; set; } = string.Empty;
	public int Order { get; set; } = -1;
	[Required]
	public bool Active { get; set; } = true;

	public string? Icon { get; set; }

	#region theo AAC
	public bool UseAAC { get; set; } = false;

	public string? Area { get; set; }
	public string? Action { get; set; }
	public string? Controller { get; set; }

	#endregion

	#region theo URL

	public bool UseUrl { get; set; } = false;
	public string? Url { get; set; } = "#";

	#endregion

	public Guid? ParentId { get; set; }

	[ForeignKey(nameof(ParentId))]
	public virtual SidebarNavItem? Parent { get; set; }
	[InverseProperty(nameof(Parent))]
	public virtual List<SidebarNavItem> Childs { get; set; } = new List<SidebarNavItem>();
}