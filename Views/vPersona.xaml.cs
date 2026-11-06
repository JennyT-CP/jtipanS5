using jtipanS5.Models;

namespace jtipanS5.Views;

public partial class vPersona : ContentPage
{
	public vPersona()
	{
		InitializeComponent();
	}

    private void btnMostrar_Clicked(object sender, EventArgs e)
    {
        List<Persona> people = App._personRepo.GetAllPerson();
        listaPersonas.ItemsSource = people;
    }

    private void btnAgregar_Clicked(object sender, EventArgs e)
    {
        lblStatusMessage.Text = "";
        App._personRepo.AddNewPerson(txtName.Text);
        lblStatusMessage.Text = App._personRepo.StatusMessage;
    }
}