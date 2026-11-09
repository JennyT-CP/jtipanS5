using jtipanS5.Models;

namespace jtipanS5.Views;

public partial class vPersona : ContentPage
{
    private int idPersonaSeleccionada = 0;
    public vPersona()
	{
		InitializeComponent();
        // AL INICIAR SOLO SE PERMITE AGREGAR
        btnAgregar.IsEnabled = true;
        btnActualizar.IsEnabled = false;
    }

    private void btnMostrar_Clicked(object sender, EventArgs e)
    {
        List<Persona> people = App._personRepo.GetAllPerson();
        listaPersonas.ItemsSource = people;
        lblStatusMessage.Text = "";
    }

    private void btnAgregar_Clicked(object sender, EventArgs e)
    {
        lblStatusMessage.Text = "";
        App._personRepo.AddNewPerson(txtName.Text);
        lblStatusMessage.Text = App._personRepo.StatusMessage;
        txtName.Text = "";
        lblStatusMessage.TextColor = 
            lblStatusMessage.Text.StartsWith("Error")
            ? Colors.Red
            : Colors.Green;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        // NO PERMITIR ELIMINAR MIENTRAS SE EDITA
        if (idPersonaSeleccionada != 0)
        {
            await DisplayAlertAsync(
                "Advertencia",
                "Primero debe actualizar o cancelar la edición",
                "Aceptar");

            return;
        }

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
        lblStatusMessage.TextColor =
           lblStatusMessage.Text.StartsWith("Error")
           ? Colors.Red
           : Colors.Green;

        // ACTUALIZAR LA LISTA
        listaPersonas.ItemsSource = App._personRepo.GetAllPerson();
    }

    private async void btnActualizar_Clicked(object sender, EventArgs e)
    {
        // VALIDAR QUE SE HAYA SELECCIONADO UNA PERSONA
        if (idPersonaSeleccionada == 0)
        {
            await DisplayAlertAsync(
                "Advertencia",
                "Primero debe seleccionar una persona para editar.",
                "Aceptar");
            return;
        }

        // OBTENER EL NUEVO NOMBRE
        string nombre = txtName.Text?.Trim() ?? "";

        // VALIDAR QUE EL NOMBRE NO ESTÉ VACÍO
        if (string.IsNullOrWhiteSpace(nombre))
        {
            await DisplayAlertAsync(
                "Error",
                "Debe ingresar el nombre de la persona.",
                "Aceptar");
            return;
        }

        // ACTUALIZAR EN SQLITE
        App._personRepo.UpdatePerson(idPersonaSeleccionada, nombre);

        // MOSTRAR MENSAJE DEL REPOSITORIO
        lblStatusMessage.Text = App._personRepo.StatusMessage;

        // ACTUALIZAR EL COLLECTIONVIEW
        listaPersonas.ItemsSource = App._personRepo.GetAllPerson();

        // LIMPIAR LOS CAMPOS
        txtName.Text = "";
        idPersonaSeleccionada = 0;

        // RESTABLECER BOTONES
        btnAgregar.IsEnabled = true;
        btnActualizar.IsEnabled = false;
        btnCancelar.IsEnabled = false;

        // MOSTRAR MENSAJE
        lblStatusMessage.TextColor = Colors.Green;

    }

    private void btnEditar_Clicked(object sender, EventArgs e)
    {
        // OBTENER EL BOTÓN PRESIONADO
        Button boton = (Button)sender;

        // OBTENER LA PERSONA SELECCIONADA
        Persona persona = (Persona)boton.CommandParameter;

        // GUARDAR EL ID
        idPersonaSeleccionada = persona.Id;

        // MOSTRAR EL NOMBRE EN EL ENTRY
        txtName.Text = persona.Name;

        // CONTROLAR LOS BOTONES
        btnAgregar.IsEnabled = false;
        btnActualizar.IsEnabled = true;
        btnCancelar.IsEnabled = true;

        // MOSTRAR MENSAJE
        lblStatusMessage.Text =
            "Modifique el nombre y presione Actualizar Persona";
        lblStatusMessage.TextColor = Colors.DarkBlue;
        txtName.Focus();
    }

    private void btnCancelar_Clicked(object sender, EventArgs e)
    {
        // CANCELAR LA EDICIÓN
        idPersonaSeleccionada = 0;

        // LIMPIAR EL CAMPO
        txtName.Text = "";

        // RESTABLECER LOS BOTONES
        btnAgregar.IsEnabled = true;
        btnActualizar.IsEnabled = false;
        btnCancelar.IsEnabled = false;

        // MOSTRAR MENSAJE
        lblStatusMessage.Text = "Edición cancelada.";
        lblStatusMessage.TextColor = Colors.DarkOrange;
    }
}