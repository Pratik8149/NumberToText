using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NumberToText.Helpers;
using NumberToText.Models;

namespace NumberToText.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;

	public HomeController(ILogger<HomeController> logger)
	{
		_logger = logger;
	}

	public IActionResult Index()
	{
		var homeView = new IndexViewModel();
		return View(homeView);
	}

	public IActionResult Privacy()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
	}

	/// <summary>
	/// Index - post
	/// </summary>
	/// <param name="model"></param>
	/// <returns></returns>
	[HttpPost]
	public IActionResult Index(IndexViewModel model)
	{
		var convertedText = TextConversionHelper.CovertNumberToTextFormat(model.Amount);

		return View(new IndexViewModel
		{
			Amount = model.Amount,
			Text = convertedText.ToUpper()
		});
	}
}