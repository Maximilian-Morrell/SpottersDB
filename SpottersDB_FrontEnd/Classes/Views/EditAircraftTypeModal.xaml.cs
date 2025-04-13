using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class EditAircraftTypeModal : ContentPage
{
    Picker ManufactorerPicker = null;
    public List<Manufactorer> Manufactorers = new List<Manufactorer>();
    bool IsEditing;
    AircraftType aircraftType;
    bool IsLoaded = false;

    public EditAircraftTypeModal()
	{
		InitializeComponent();
        IsEditing = false;
        TypeICAO.Text = "";
        TypeName.Text = "";
        Submit.Clicked += Submit_Clicked;
	}

    public EditAircraftTypeModal(AircraftType aircraftType)
    {
        InitializeComponent();
        IsEditing = true;
        Submit.Clicked += Submit_Clicked;
        this.aircraftType = aircraftType;
        TypeICAO.Text = aircraftType.icaoCode;
        TypeName.Text = aircraftType.fullName;
        NickName.Text = aircraftType.nickName;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        GetAllManufactorers();
        base.OnNavigatedTo(args);
    }

    private async void Submit_Clicked(object sender, EventArgs e)
    {
        try
        {
            int Manufactorer = Manufactorers[ManufactorerPicker.SelectedIndex -1].id;
            if (IsEditing)
            {
                int id = aircraftType.id;
                AircraftType newaircraftType = new AircraftType(id, TypeICAO.Text, TypeName.Text, NickName.Text, Manufactorer);
                await HTTP_Controller.UpdateAircraftType(newaircraftType);
                Navigation.RemovePage(this);
            }
            else
            {
                aircraftType = new AircraftType(TypeICAO.Text, TypeName.Text, NickName.Text, Manufactorer);
                await HTTP_Controller.AddNewAircraftType(aircraftType);
                Navigation.RemovePage(this);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Something has gone wrong with saving", ex.Message, "OK");
        }
    }

    public async void GetAllManufactorers()
    {
        if (ManufactorerPicker != null)
        {
            GridMain.Children.Remove(ManufactorerPicker);
        }

        Manufactorers = await HTTP_Controller.GetManufactorers();
        ManufactorerPicker = new Picker();
        List<string> manufactorerNames = new List<string>();

        manufactorerNames.Add("Create New");
        foreach (Manufactorer country in Manufactorers)
        {
            manufactorerNames.Add(country.name + " - " + country.id);
        }


        ManufactorerPicker.ItemsSource = manufactorerNames;

        ManufactorerPicker.Title = "Select a Manufactorer";

        if (IsEditing)
        {
            int ID = Manufactorers.FindIndex(c => c.id == aircraftType.manufactorerID);
            ManufactorerPicker.SelectedIndex = ID;
        }

        ManufactorerPicker.SelectedIndexChanged += ManufactorerPicker_SelectedIndexChanged;

        GridMain.Add(ManufactorerPicker, 1, 3);
        IsLoaded = true;
    }

    private void ManufactorerPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ManufactorerPicker.SelectedItem)
        {
            case "Create New":
                CreateNewManufactorer();
                break;
            default:
                CheckIfValid();
                break;
        }
    }

    private void CreateNewManufactorer()
    {
        EditManufactorerModal editManufactorerModal = new EditManufactorerModal();
        Navigation.PushAsync(editManufactorerModal);
    }

    private void CheckIfValid()
    {
        if(IsLoaded)
        {
            Submit.IsEnabled = ManufactorerPicker.SelectedIndex >= 1 && TypeICAO.Text.Length > 0 && TypeName.Text.Length > 0;
        }
    }

    private void Entry_TextChange(object sender, TextChangedEventArgs e)
    {
        CheckIfValid();
    }
}