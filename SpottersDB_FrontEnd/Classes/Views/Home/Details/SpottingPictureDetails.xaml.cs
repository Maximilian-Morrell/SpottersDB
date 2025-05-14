using SpottersDB_FrontEnd.Classes.Structure;
using SpottersDB_FrontEnd.Classes.Utilities;

namespace SpottersDB_FrontEnd.Classes.Views.Home.Details;

public partial class SpottingPictureDetails : ContentPage
{
	SpottingPicture pic;
	Airport airport;
	Country country;
	SpottingTrip trip;
	Aircraft aircraft;
	AircraftType aircraftType;
	Manufactorer manufactorer;
	Airline airline;
	public SpottingPictureDetails(SpottingPicture pic)
	{
		this.pic = pic;
		InitializeComponent();
		GetData();
	}

	private async void GetData()
	{
		Dictionary<string, int> tmp = await HTTP_Controller.GetSpottingTripAirport(pic.spottingTripAirportID);
		airport = await HTTP_Controller.GetAirport(tmp["Airport"]);
		country = await HTTP_Controller.GetCountryByID(airport.countryID);
		trip = await HTTP_Controller.GetSpottingTrip(tmp["SpottingTrip"]);
		aircraft = await HTTP_Controller.GetAircraft(pic.aircraftID);
		aircraftType = await HTTP_Controller.GetAircraftTypeByID(aircraft.typeID);
		manufactorer = await HTTP_Controller.GetManufactorerByID(aircraftType.manufactorerID);
		airline = await HTTP_Controller.GetAirlineByID(aircraft.airlineID);

		FillData();
	}

	private void FillData()
	{
		ImgUI.Source = pic.pictureUrl;
        Title = "Spotting Picture: " + pic.name;
		LBL_Title.Text = pic.name;
		LBL_Description.Text = pic.description;
		LBL_Airport.Text = airport.icaO_Code + "/" + airport.iatA_Code + " - " + airport.name;
		LBL_Country.Text = country.name;
		LBL_SpottingTrip.Text = trip.name;
		LBL_SpottingTripDate.Text = trip.start.ToString("dd.MM.yyyy - HH:mm") + " / " + trip.end.ToString("dd.MM.yyyy - HH:mm");
		LBL_Registration.Text = aircraft.registration;
		LBL_Type.Text = aircraftType.fullName;
		LBL_Manufactorer.Text = manufactorer.name;
		LBL_Airline.Text = airline.name;
    }
}