using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.UI_Elements.Cards;
using SpottersDB_FrontEnd.Classes.Utilities;
using SpottersDB_FrontEnd.Classes.Views.Home.Details;

namespace SpottersDB_FrontEnd.Classes.Views.Home;

public partial class HomeAircraftType : ContentPage
{
    List<Manufactorer> manufactorers;
    List<AircraftType> AllAircraftTypes;
    bool IsLoaded = false;
	public HomeAircraftType()
	{
		InitializeComponent();
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        LoadEverything();
        base.OnNavigatedTo(args);
    }

    public async void LoadEverything()
    {
        LoadFilters();
    }

    private EventHandler OpenClicked(AircraftType type)
    {
        AircraftTypeDetails aircraftTypeDetails = new AircraftTypeDetails(type);
        Navigation.PushAsync(aircraftTypeDetails);
        return null;
    }

    private async void LoadFilters()
    {
        manufactorers = await HTTP_Controller.GetManufactorers();
        ManufactorerPicker.Items.Clear();
        ManufactorerPicker.Items.Add("");
        if (manufactorers != null)
        {
            foreach (Manufactorer manufactorer in manufactorers)
            {
                ManufactorerPicker.Items.Add(manufactorer.name);
            }
        }
        IsLoaded = true;
        LoadAircraftTypes();
    }

    private async void LoadAircraftTypes()
    {
        try
        {
            AllAircraftTypes = await HTTP_Controller.GetAircraftTypes();
            ContentParent.Opacity = 1;
            await Microsoft.Maui.Controls.ViewExtensions.FadeTo(ContentParent, 0, 100, Easing.CubicInOut);
            ContentParent.Clear();
            foreach (AircraftType type in FilterAircraftType())
            {
                AircraftTypeCard card = new AircraftTypeCard();
                Border b = await card.CardHome(type);
                card.EditClicked += OpenClicked;
                //Cards.Add(b);
                ContentParent.Children.Add(b);
            }
            await Microsoft.Maui.Controls.ViewExtensions.FadeTo(ContentParent, 1, 100, Easing.CubicInOut);

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error: ", ex.Message, "Ok");
        }
    }

    private List<AircraftType> FilterAircraftType()
    {
        string SearchParams = Search.Text.ToLower();
        List<AircraftType> AircraftTypeFiltered = AllAircraftTypes.FindAll(air => air.fullName.ToLower().Contains(SearchParams));

        if (ManufactorerPicker.SelectedIndex > 0)
        {
            int ManufactorerID = manufactorers[ManufactorerPicker.SelectedIndex - 1].id;
            AircraftTypeFiltered = AircraftTypeFiltered.FindAll(air => air.manufactorerID == ManufactorerID);
        }

        return AircraftTypeFiltered;
    }

    private void Search_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (IsLoaded)
        {
            LoadAircraftTypes();
        }

    }

    private void SelectionChanged(object sender, EventArgs e)
    {
        if (IsLoaded)
        {
            LoadAircraftTypes();
        }
    }
}