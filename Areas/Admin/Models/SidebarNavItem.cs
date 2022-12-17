#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Client.Areas.Admin.Models;

public class SidebarNavItem
{
	[Key] public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public int Order { get; set; } = -1;
	public bool Active { get; set; } = true;

	public string? Icon { get; set; }

	#region theo AAC

	public string? Area { get; set; }
	public string Action { get; set; } = string.Empty;
	public string Controller { get; set; } = string.Empty;

	#endregion

	#region theo URL

	public bool UseUrl { get; set; } = false;
	public string Url { get; set; } = "#";

	#endregion

	public Guid? ParentId { get; set; }

	[ForeignKey(nameof(ParentId))]
	// public virtual SidebarNavItem? Parent { get; set; }
	// [InverseProperty(nameof(Parent))]
	public virtual ICollection<SidebarNavItem>? Childs { get; set; }
}