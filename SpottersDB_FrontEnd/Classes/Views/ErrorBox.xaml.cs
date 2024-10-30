namespace SpottersDB_FrontEnd.Classes.Views;

public partial class ErrorBox : ContentPage
{
	public ErrorBox(string ErrorNameString, string ErrorBodyString)
	{
		InitializeComponent();
		this.ErrorName.Text = ErrorNameString;
		this.ErrorBody.Text = ErrorBodyString;
	}
}