using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.UI_Elements.Cards;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views.Home.Details;

public partial class AircraftTypeDetails : ContentPage
{
    AircraftType Type;
    Manufactorer manufactorer;
    List<SpottingPicture> pictures;
    HashSet<int> Aircrafts = new HashSet<int>();
    public AircraftTypeDetails(AircraftType type)
    {
        this.Type = type;
        InitializeComponent();
        GetData(type);
    }

    private EventHandler OpenClicked(SpottingPicture spottingPicture)
    {
        SpottingPictureDetails picDetails = new SpottingPictureDetails(spottingPicture);
        Navigation.PushAsync(picDetails);
        return null;
    }

    private async void GetData(AircraftType type)
    {
        manufactorer = await HTTP_Controller.GetManufactorerByID(type.manufactorerID);
        List<Aircraft> aircrafts = await HTTP_Controller.GetAircraftsByTypeID(type.id);
        pictures = new List<SpottingPicture>();
        foreach (Aircraft aircraft in aircrafts)
        {
            if (!Aircrafts.Contains(aircraft.id))
            {
                Aircrafts.Add(aircraft.id);
            }

            pictures.AddRange(await HTTP_Controller.GetSpottingPicturesByAircraft(aircraft.id));
        }

        FillInformation();
    }

    public async void FillInformation()
    {
        Title = "Aircrafttype Details: " + Type.icaoCode;
        LBL_ICAOCode.Text = Type.icaoCode;
        LBL_Name.Text = Type.fullName;
        LBL_Nickname.Text = Type.nickName;
        LBL_Manufactorer.Text = manufactorer.name;
        foreach (int aircraftID in Aircrafts)
        {
            bool HasAircraft = false;
            Aircraft aircraft = await HTTP_Controller.GetAircraft(aircraftID);
            VerticalStackLayout parent = new VerticalStackLayout();
            Label lbl = UI_Utilities.CreateLabel(parent, aircraft.registration, 50, FontAttributes.Bold);
            FlexLayout Test = new FlexLayout();
            Test.Wrap = Microsoft.Maui.Layouts.FlexWrap.Wrap;
            Test.JustifyContent = Microsoft.Maui.Layouts.FlexJustify.SpaceEvenly;
            Test.Direction = Microsoft.Maui.Layouts.FlexDirection.Row;
            foreach (SpottingPicture pic in pictures.Where(p => p.aircraftID == aircraft.id))
            {
                HasAircraft = true;
                SpottingPictureCard picCard = new SpottingPictureCard();
                Border b = await picCard.CardHome(pic);
                picCard.EditClicked += OpenClicked;
                Test.Add(b);
            }
            parent.Add(Test);
            if(HasAircraft)
            {
                AircraftParent.Add(parent);
            }
        }
    }
}