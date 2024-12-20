using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views;

public partial class EditAircraftTypeModal : ContentPage
{
    Picker ManufactorerPicker = null;
    public List<Manufactorer> Manufactorers = new List<Manufactorer>();
    bool IsEditing;
    AircraftType aircraftType;

    public EditAircraftTypeModal()
	{
		InitializeComponent();
        IsEditing = false;
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
        int Manufactorer = Manufactorers[ManufactorerPicker.SelectedIndex].id;
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

    public async void GetAllManufactorers()
    {
        if (ManufactorerPicker != null)
        {
            GridMain.Children.Remove(ManufactorerPicker);
        }

        Manufactorers = await HTTP_Controller.GetManufactorers();
        ManufactorerPicker = new Picker();
        List<string> manufactorerNames = new List<string>();

        foreach (Manufactorer country in Manufactorers)
        {
            manufactorerNames.Add(country.name + " - " + country.id);
        }

        manufactorerNames.Add("Create New");

        ManufactorerPicker.ItemsSource = manufactorerNames;

        ManufactorerPicker.Title = "Select a Manufactorer";

        if (IsEditing)
        {
            int ID = Manufactorers.FindIndex(c => c.id == aircraftType.manufactorerID);
            ManufactorerPicker.SelectedIndex = ID;
        }

        ManufactorerPicker.SelectedIndexChanged += ManufactorerPicker_SelectedIndexChanged;

        GridMain.Add(ManufactorerPicker, 1, 3);
    }

    private void ManufactorerPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ManufactorerPicker.SelectedItem)
        {
            case "Create New":
                CreateNewManufactorer();
                break;
        }
    }

    private void CreateNewManufactorer()
    {
        EditManufactorerModal editManufactorerModal = new EditManufactorerModal();
        Navigation.PushAsync(editManufactorerModal);
    }
}