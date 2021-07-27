using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SignalRDemo.Data;
using SignalRDemo.Models;

namespace SignalRDemo.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<AppUser> _userManager;

		public HomeController(ILogger<HomeController> logger,
								ApplicationDbContext context,
								UserManager<AppUser> userManager)
		{
			_context = context;
			_userManager = userManager;
			_logger = logger;
		}

		public async Task<IActionResult> Index()
		{
			var currentUser = await _userManager.GetUserAsync(User);
			var messages = await _context.Messages.ToListAsync();

			if (User.Identity.IsAuthenticated)
			{
				ViewBag.CurrentUserName = currentUser.UserName;
			}

			return View(messages);
		}

		[HttpPost]
		public async Task<IActionResult> Create(Message message)
		{
			if (ModelState.IsValid)
			{
				message.UserName = User.Identity.Name;
				var sender = await _userManager.GetUserAsync(User);
				message.UserId = sender.Id;

				await _context.Messages.AddAsync(message);
				await _context.SaveChangesAsync();
				return Ok();
			}
			return Error();
		}


		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
