using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Client.Areas.Api.DTOs;

public static class SidebarNavItem
{
	public class Get
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int Order { get; set; } = -1;
		public bool Active { get; set; } = true;
		public string? Icon { get; set; }
		public bool UseAAC { get; set; } = false;
		public bool UseUrl { get; set; } = false;
		public string? Url { get; set; }
		public string? Area { get; set; }
		public string? Controller { get; set; }
		public string? Action { get; set; }
		public Guid? ParentId { get; set; }

		public static Expression<Func<Admin.Models.SidebarNavItem, dynamic>> ExpressionToiThieu { get; } = model => new
		{
			model.Id,
			model.Name,
			model.Order,
			model.Active,
			model.UseAAC,
			model.UseUrl
		};

		public static Expression<Func<Admin.Models.SidebarNavItem, Get>> Expression { get; } = model => new()
		{
			Id = model.Id,
			Name = model.Name,
			Order = model.Order,
			Active = model.Active,
			Icon = model.Icon,
			UseUrl = model.UseUrl,
			UseAAC = model.UseAAC,
			ParentId = model.ParentId,
			Url = model.Url,
			Area = model.Area,
			Controller = model.Controller,
			Action = model.Action
		};
	}

	public class Post
	{
		public string Name { get; set; } = string.Empty;
		public int Order { get; set; } = -1;
		public bool Active { get; set; } = true;
		public string? Icon { get; set; }
		public bool UseAAC { get; set; } = false;
		public bool UseUrl { get; set; } = false;
		public string? Url { get; set; }
		public string? Area { get; set; }
		public string? Controller { get; set; }
		public string? Action { get; set; }
		public Guid? ParentId { get; set; }

		public static Expression<Func<Post, Admin.Models.SidebarNavItem>> expression = dto => new()
		{
			Name = dto.Name,
			Order = dto.Order,
			Active = dto.Active,
			Icon = dto.Icon,
			UseUrl = dto.UseUrl,
			Url = dto.Url,
			Area = dto.Area,
			Controller = dto.Controller,
			Action = dto.Action,
			ParentId = dto.ParentId,
			UseAAC = dto.UseAAC
		};
	}
}