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
        txtName.Text = "";
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        // OBTENER EL BOTÓN QUE SE PRESIONÓ
        Button boton = (Button)sender;

        // OBTENER EL ID DE LA PERSONA
        int id = Convert.ToInt32(boton.CommandParameter);

        // CONFIRMAR ANTES DE ELIMINAR
        bool confirmar = await DisplayAlertAsync(
            "Confirmar eliminación",
            "¿Está seguro de eliminar esta persona?",
            "Sí",
            "No"
        );

        if (!confirmar)
        {
            return;
        }

        // ELIMINAR PERSONA DE SQLITE
        App._personRepo.DeletePerson(id);

        // MOSTRAR MENSAJE DEL REPOSITORIO
        lblStatusMessage.Text = App._personRepo.StatusMessage;

        // ACTUALIZAR LA LISTA
        listaPersonas.ItemsSource = App._personRepo.GetAllPerson();
    }
}