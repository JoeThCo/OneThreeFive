using OneThreeFive.ViewModels;

namespace OneThreeFive
{
	public partial class MainPage : ContentPage
	{
		public MainPage(MainViewModel viewModel)
		{
			InitializeComponent();

			BindingContext = viewModel;
		}
	}
}