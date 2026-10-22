using Microsoft.Extensions.DependencyInjection;
using OneThreeFive.ViewModels;

namespace OneThreeFive;

public partial class App : Application
{
	private readonly MainViewModel _viewModel;

	public App(MainViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var window = new Window(new AppShell());

		window.Deactivated += (_, _) => _viewModel.Save();
		window.Stopped += (_, _) => _viewModel.Save();

		return window;
	}
}