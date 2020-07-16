Imports System
Imports System.Xml.Serialization
Imports System.Xml
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Imports System.Security.Cryptography
Imports System.Security.Permissions
Imports System.Security.Cryptography.X509Certificates
Imports System.Security.Cryptography.xml
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports System.Reflection

Imports FirmaXadesNet
Imports FirmaXadesNet.Clients
Imports FirmaXadesNet.Crypto
Imports FirmaXadesNet.Signature
Imports FirmaXadesNet.Signature.Parameters
Imports FirmaXadesNet.Upgraders
Imports FirmaXadesNet.Upgraders.Parameters
Imports FirmaXadesNet.Utils
Imports FirmaXadesNet.Validation
Imports System.Collections.Generic

Imports System.ServiceModel
Imports System.ServiceModel.Channels
Imports System.ServiceModel.Security
Imports System.Text

'Imports System.ComponentModel
'Imports System.Diagnostics
'Imports System.Web.Services
'Imports System.Web.Services.Protocols

Public Class frmFacturacion
    Private bDEBUG As Boolean = True
    Private myDataGridViewBindingSource As BindingSource
    Private Const LB_GETCURSEL As Int32 = &H188 ' message used to get item index the mouse is over in drop list.

    <DllImport("user32.dll")>
    Private Shared Function WindowFromPoint(ByVal Point As Point) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Int32, ByVal wParam As Int32, ByVal lParam As Int32) As Int32
    End Function

    ''' <summary>
    ''' Se ejecuta al cargar la ventana de facturación
    ''' Carga los datos de la base de datos, establece el título de la ventana, inicializa los filtros, rellena certificados
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub frmFacturacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Me.DataSetPresentacionFacturacionListado.EnforceConstraints = False

        ' DATAGRIDVIEW
        'TODO: esta línea de código carga datos en la tabla 'DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION' Puede moverla o quitarla según sea necesario.
        Me.PRESENTACIONFACTURACIONTableAdapter.Fill(Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION)

        myDataGridViewBindingSource = Me.PRESENTACIONFACTURACIONBindingSource
        myDataGridViewBindingSource.Filter = "FECHA_ENVIO_SII Is NULL"

        ' SERIES
        Me.DataSetSeriesListado.EnforceConstraints = False
        'TODO: esta línea de código carga datos en la tabla 'DataSetSeriesListado.SERIES' Puede moverla o quitarla según sea necesario.
        Me.SERIESTableAdapter.Fill(Me.DataSetSeriesListado.SERIES)

        ' ZONAS
        'TODO: esta línea de código carga datos en la tabla 'DataSetZONASListado.ZONAS' Puede moverla o quitarla según sea necesario.
        Me.ZONASTableAdapter.Fill(Me.DataSetZONASListado.ZONAS)

        ' PERIODOS
        'TODO: esta línea de código carga datos en la tabla 'DataSetPRESENTACIONFACTURACIONPeriodos.PRESENTACIONFACTURACION' Puede moverla o quitarla según sea necesario.
        Me.PRESENTACIONFACTURACIONTableAdapter2.Fill(Me.DataSetPRESENTACIONFACTURACIONPeriodos.PRESENTACIONFACTURACION)

        ' Título de la ventana
        Me.Text = "Registro de facturas con AAPP (" & String.Format("{0:#,##0}", Me.dgvPRESENTACIONFACTURACION.Rows.Count) & ")"

        ' Inicializar Filtro AÑO
        Me.ClearAnoFiltro()

        ' Inicializar Filtro PRESENTACION SII
        Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Checked = False
        Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Format = DateTimePickerFormat.Custom
        Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.CustomFormat = " "
        Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Checked = False
        Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Format = DateTimePickerFormat.Custom
        Me.FechaPresentacionSIIFiltroHastaDateTimePicker.CustomFormat = " "
        Me.FechaPresentacionSIIFiltroCheckBox.Checked = True
        ' Inicializar Filtro PRESENTACION FACE
        Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Checked = False
        Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Format = DateTimePickerFormat.Custom
        Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.CustomFormat = " "
        Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Checked = False
        Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Format = DateTimePickerFormat.Custom
        Me.FechaPresentacionFACEFiltroHastaDateTimePicker.CustomFormat = " "
        Me.FechaPresentacionFACEFiltroCheckBox.Checked = True

        ' Rellenar listado de certificados
        Dim repositorioCertificados As Security.Cryptography.X509Certificates.X509Store = New Security.Cryptography.X509Certificates.X509Store(Security.Cryptography.X509Certificates.StoreName.My) ', Security.Cryptography.X509Certificates.StoreLocation.LocalMachine)
        repositorioCertificados.Open(Security.Cryptography.X509Certificates.OpenFlags.ReadOnly)

        Dim nIndex As Integer = 0
        Dim certificadosComboSource As New Dictionary(Of Integer, String)()
        certificadosComboSource.Add(-2, "Seleccionar certificado")
        certificadosComboSource.Add(-1, "Seleccionar archivo PFX")
        For Each certificado In repositorioCertificados.Certificates
            certificadosComboSource.Add(nIndex, certificado.SubjectName.Name)
            nIndex = nIndex + 1
        Next
        Me.CertificadosComboBox.Items.Clear()
        Me.CertificadosComboBox.DataSource = New BindingSource(certificadosComboSource, Nothing)
        Me.CertificadosComboBox.DisplayMember = "Value"
        Me.CertificadosComboBox.ValueMember = "Key"

        ' Inicializar Tooltip
        With ToolTip1
            .SetToolTip(CertificadosComboBox, "Seleccionar certificado para presentación")
            .IsBalloon = False
            .ShowAlways = True
        End With
        ' Inicializar listado de certificados para Tooltip
        With CertificadosComboBox
            .DrawMode = DrawMode.OwnerDrawFixed
            .BackColor = Color.White 'Color.LightYellow
            .ForeColor = Color.Black
        End With

        ' *** TEST ***
        bDEBUG = Me.DEBUGCheckBox.Checked
        If bDEBUG Then
            Me.CertificadosComboBox.SelectedIndex = 1
            Me.CertificadoTextBox.Enabled = True
            Me.CertificadoTextBox.Text = ""
            Me.CertificadoClaveTextBox.Enabled = True
            Me.CertificadoClaveTextBox.Text = ""
        End If

        ' Maximizar ventana
        Me.WindowState = FormWindowState.Maximized

    End Sub
    ' CERTIFICADOS COMBOBOX
    ''' <summary>
    ''' Simula el efecto 'Tooltip' en la lista de certificados. Se ejecuta al dibujarse la lista
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub CertificadosComboBox_DrawItem(sender As Object, e As System.Windows.Forms.DrawItemEventArgs) Handles CertificadosComboBox.DrawItem
        Static lastindex As Integer = -1
        ' Si no hay nada seleccionado, salir!
        If e.Index < 0 Then Return
        ' Dibujo el fondo del control de lista para cada elemento
        e.DrawBackground()
        ' Devuelve el combo que originó el evento
        Dim cb = DirectCast(sender, ComboBox)
        ' Dibujo el texto del elemento usando los colores por defecto
        Using brsh As New SolidBrush(e.ForeColor)
            e.Graphics.DrawString(cb.Items(e.Index).ToString, e.Font, brsh, e.Bounds)
        End Using
        '
        ' Devuelve la ventana bajo el ratón
        Dim hWnd = WindowFromPoint(MousePosition)
        ' Devuelve el índice del elemento de la lista
        Dim index As Int32 = SendMessage(hWnd, LB_GETCURSEL, 0, 0)
        If index > -1 AndAlso lastindex <> index Then ' mostrar una sola vez o parpadeará, desaparecerá, etc.
            lastindex = index
            ' Devuelve la posición del ratón para establecer la localización del tooltip
            Dim mousePos = cb.PointToClient(MousePosition)
            ' Offset de la localización para mostrar el tooltipo justo por debajo del cursor
            mousePos.Offset(-2, 22) '+22 debería ser suficiente para cursores de tamaños normales
            ' Muestra el texto del elemento en el tooltip
            ToolTip1.Show(cb.Items(index).ToString, cb, mousePos.X, mousePos.Y)
        End If
        '
        ' Dibuja el rectángulo del foco si es necesario
        e.DrawFocusRectangle()
    End Sub
    ''' <summary>
    ''' Oculta el tooltip al cerrar la lista de certificados
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub CertificadosComboBox_DropDownClosed(sender As Object, e As System.EventArgs) Handles CertificadosComboBox.DropDownClosed
        ToolTip1.Hide(CertificadosComboBox)
    End Sub
    ''' <summary>
    ''' Evento al cambiar el elemento seleccionado en la lista de certificados
    ''' Si se selecciona el índice -1, se abre el diálogo para seleccionar el archivo con el certificado
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub CertificadosComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CertificadosComboBox.SelectedIndexChanged
        Dim ncertificadoSeleccionadoIndex As Integer = DirectCast(Me.CertificadosComboBox.SelectedItem, KeyValuePair(Of Integer, String)).Key
        If ncertificadoSeleccionadoIndex = -1 And Not bDEBUG Then
            Me.CertificadoTextBox.Enabled = True
            Me.CertificadoClaveTextBox.Enabled = True

            Dim fd As OpenFileDialog = New OpenFileDialog()

            fd.Title = "Open File Dialog"
            If Me.CertificadoTextBox.Text.Length > 0 Then
                fd.InitialDirectory = System.IO.Path.GetDirectoryName(Me.CertificadoTextBox.Text)
            Else
                fd.InitialDirectory = "C:\"
            End If
            fd.Filter = "Todos los archivos (*.*)|*.*|Certificados (*.pfx,*.p12) |*.pfx;*.p12"
            fd.FilterIndex = 2
            fd.RestoreDirectory = True

            If fd.ShowDialog() = DialogResult.OK Then
                Me.CertificadoTextBox.Text = fd.FileName
            Else
                Me.CertificadosComboBox.SelectedIndex = 0
                Me.CertificadoTextBox.Enabled = False
                Me.CertificadoClaveTextBox.Enabled = False
            End If
        Else
            Me.CertificadoTextBox.Enabled = False
            Me.CertificadoClaveTextBox.Enabled = False
        End If
    End Sub
    ''' <summary>
    ''' Evento al hacer click en el campo de la ruta y nombre del certificado
    ''' Al pulsar se abre el diálogo de archivos para seleccionar el archivo
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub CertificadoTextBox_Click(sender As Object, e As EventArgs) Handles CertificadoTextBox.Click
        Dim fd As OpenFileDialog = New OpenFileDialog()

        fd.Title = "Open File Dialog"
        If Me.CertificadoTextBox.Text.Length > 0 Then
            fd.InitialDirectory = System.IO.Path.GetDirectoryName(Me.CertificadoTextBox.Text)
        Else
            fd.InitialDirectory = "C:\"
        End If

        fd.Filter = "Todos los archivos (*.*)|*.*|Certificados (*.pfx,*.p12) |*.pfx;*.p12"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            Me.CertificadoTextBox.Text = fd.FileName
        End If
    End Sub
    ' ANO
    ''' <summary>
    ''' Borra el filtro del año, el campo 'desde', el campo 'hasta', o ambos
    ''' </summary>
    ''' <param name="anoDesdeHasta">Texto para especificar qué se borra: 'desde', 'hasta', o todo, por defecto vacío, que borra todo</param>
    Private Sub ClearAnoFiltro(Optional anoDesdeHasta As String = "")
        Try
            If anoDesdeHasta = "desde" Or anoDesdeHasta = "" Then
                Me.AnoFiltroDesdeDateTimePicker.Format = DateTimePickerFormat.Custom
                Me.AnoFiltroDesdeDateTimePicker.CustomFormat = " "
                Me.AnoFiltroDesdeDateTimePicker.ShowCheckBox = True
                Me.AnoFiltroDesdeDateTimePicker.Checked = False
                Me.AnoFiltroDesdeDateTimePicker.MinDate = DateTime.Now.AddYears(-5)
                Me.AnoFiltroDesdeDateTimePicker.MaxDate = DateTime.Now.AddYears(+1)
                If Me.AnoFiltroHastaDateTimePicker.Checked Or anoDesdeHasta = "" Then
                    Me.AnoFiltroHastaDateTimePicker.MinDate = DateTime.Now.AddYears(-5)
                End If
            End If
        Catch ex As Exception
            frmMain.AddToLog("ERROR al limpiar el filtro del año (desde): " & ex.Message)
            MessageBox.Show("ERROR al limpiar el filtro del año (desde): " & ex.Message)
        End Try

        Try
            If anoDesdeHasta = "hasta" Or anoDesdeHasta = "" Then
                Me.AnoFiltroHastaDateTimePicker.Format = DateTimePickerFormat.Custom
                Me.AnoFiltroHastaDateTimePicker.CustomFormat = " "
                Me.AnoFiltroHastaDateTimePicker.ShowCheckBox = True
                Me.AnoFiltroHastaDateTimePicker.Checked = False
                Me.AnoFiltroHastaDateTimePicker.MinDate = DateTime.Now.AddYears(-5)
                Me.AnoFiltroHastaDateTimePicker.MaxDate = DateTime.Now.AddYears(+1)
                If Me.AnoFiltroDesdeDateTimePicker.Checked Or anoDesdeHasta = "" Then
                    Me.AnoFiltroDesdeDateTimePicker.MaxDate = DateTime.Now.AddYears(+1)
                End If
            End If
        Catch ex As Exception
            frmMain.AddToLog("ERROR al limpiar el filtro del año (hasta): " & ex.Message)
            MessageBox.Show("ERROR al limpiar el filtro del año (hasta): " & ex.Message)
        End Try

    End Sub
    ''' <summary>
    ''' Evento al pulsar con el ratón para poner el foco en el filtro del año
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub AnoFiltroDesdeDateTimePicker_MouseDown(sender As Object, e As MouseEventArgs) Handles AnoFiltroDesdeDateTimePicker.MouseDown
        Try
            If Me.AnoFiltroDesdeDateTimePicker.Checked Then
                Me.AnoFiltroDesdeDateTimePicker.Focus()
            End If
        Catch ex As Exception
            frmMain.AddToLog("ERROR al poner el foco en el filtro del año (desde): " & ex.Message)
            MessageBox.Show("ERROR al poner el foco en el filtro del año (desde): " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Evento al pulsar con el ratón para poner el foco en el filtro del año
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub AnoFiltroHastaDateTimePicker_MouseDown(sender As Object, e As MouseEventArgs) Handles AnoFiltroHastaDateTimePicker.MouseDown
        Try
            If Me.AnoFiltroHastaDateTimePicker.Checked Then
                Me.AnoFiltroHastaDateTimePicker.Focus()
            End If
        Catch ex As Exception
            frmMain.AddToLog("ERROR al poner el foco en el filtro del año (hasta): " & ex.Message)
            MessageBox.Show("ERROR al poner el foco en el filtro del año (hasta): " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Evento al cambiar el valor seleccionado del filtro del año (desde) que cambia el formato del datetimepicker y ejecuta el filtro
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub AnoFiltroDesdeDateTimePicker_ValueChanged(sender As Object, e As EventArgs) Handles AnoFiltroDesdeDateTimePicker.ValueChanged
        Try
            If Me.AnoFiltroDesdeDateTimePicker.Checked = False Then
                ClearAnoFiltro("desde")
            Else
                Me.AnoFiltroDesdeDateTimePicker.Format = DateTimePickerFormat.Custom
                Me.AnoFiltroDesdeDateTimePicker.CustomFormat = "yyyy"
                Me.AnoFiltroDesdeDateTimePicker.ShowUpDown = True
                Me.AnoFiltroDesdeDateTimePicker.Checked = True
                If Me.AnoFiltroHastaDateTimePicker.Checked Then
                    Me.AnoFiltroHastaDateTimePicker.MinDate = Me.AnoFiltroDesdeDateTimePicker.Value
                End If
                Me.AnoFiltroDesdeDateTimePicker.Focus()
            End If
            FiltrarListado()
        Catch ex As Exception
            frmMain.AddToLog("ERROR al cambiar el valor en el filtro del año (desde): " & ex.Message)
            MessageBox.Show("ERROR al cambiar el valor en el filtro del año (desde): " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Evento al cambiar el valor seleccionado del filtro del año (hasta) que cambia el formato del datetimepicker y ejecuta el filtro
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub AnoFiltroHastaDateTimePicker_ValueChanged(sender As Object, e As EventArgs) Handles AnoFiltroHastaDateTimePicker.ValueChanged
        Try
            If Me.AnoFiltroHastaDateTimePicker.Checked = False Then
                Me.ClearAnoFiltro("hasta")
            Else
                Me.AnoFiltroHastaDateTimePicker.Format = DateTimePickerFormat.Custom
                Me.AnoFiltroHastaDateTimePicker.CustomFormat = "yyyy"
                Me.AnoFiltroHastaDateTimePicker.ShowUpDown = True
                Me.AnoFiltroHastaDateTimePicker.Checked = True
                If Me.AnoFiltroDesdeDateTimePicker.Checked Then
                    Me.AnoFiltroDesdeDateTimePicker.MaxDate = Me.AnoFiltroHastaDateTimePicker.Value
                End If
                Me.AnoFiltroHastaDateTimePicker.Focus()
            End If
            FiltrarListado()
        Catch ex As Exception
            frmMain.AddToLog("ERROR al cambiar el valor en el filtro del año (hasta): " & ex.Message)
            MessageBox.Show("ERROR al cambiar el valor en el filtro del año (hasta): " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Evento al pulsar sobre el botón de borrado del filtro del año que ejecuta el borrado del campo y ejecuta el filtro
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub AnoFiltroDeleteButton_Click(sender As Object, e As EventArgs) Handles AnoFiltroDeleteButton.Click
        Me.ClearAnoFiltro()
        Me.FiltrarListado()
    End Sub
    ' SERIES
    ''' <summary>
    ''' Evento al cambiar el valor seleccionado del filtro de la serie que ejecuta el filtro
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub SeriesFiltroComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SeriesFiltroComboBox.SelectedIndexChanged
        FiltrarListado()
    End Sub
    ' ZONAS
    ''' <summary>
    ''' Evento al cambiar el valor seleccionado del filtro de la zona que ejecuta el filtro
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub ZonasFiltroComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ZonasFiltroComboBox.SelectedIndexChanged
        FiltrarListado()
    End Sub
    ' PERIODOS
    ''' <summary>
    ''' Evento al cambiar el valor seleccionado del filtro de los periodos que ejecuta el filtro
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub PeriodosFiltroComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PeriodosFiltroComboBox.SelectedIndexChanged
        FiltrarListado()
    End Sub
    ' FECHA PRESENTACION SII
    ''' <summary>
    ''' Borra el filtro de la fecha de presentación del SII, el campo 'desde', el campo 'hasta', o ambos
    ''' </summary>
    ''' <param name="anoDesdeHasta">Texto para especificar qué se borra: 'desde', 'hasta', o todo, por defecto vacío, que borra todo</param>
    Private Sub ClearFechaPresentacionSIIFiltro(Optional anoDesdeHasta As String = "")
        Try
            If anoDesdeHasta = "desde" Or anoDesdeHasta = "" Then
                Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Format = DateTimePickerFormat.Custom
                Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.CustomFormat = " "
                Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.ShowCheckBox = True
                Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Checked = False
                Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.MinDate = DateTime.Now.AddYears(-5)
                Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.MaxDate = DateTime.Now.AddYears(+1)
                If Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Checked Or anoDesdeHasta = "" Then
                    Me.FechaPresentacionSIIFiltroHastaDateTimePicker.MinDate = DateTime.Now.AddYears(-5)
                End If
            End If
        Catch ex As Exception
            frmMain.AddToLog("ERROR al limpiar el filtro de la presentación SII (desde): " & ex.Message)
            MessageBox.Show("ERROR al limpiar el filtro de la presentación SII (desde): " & ex.Message)
        End Try

        Try
            If anoDesdeHasta = "hasta" Or anoDesdeHasta = "" Then
                Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Format = DateTimePickerFormat.Custom
                Me.FechaPresentacionSIIFiltroHastaDateTimePicker.CustomFormat = " "
                Me.FechaPresentacionSIIFiltroHastaDateTimePicker.ShowCheckBox = True
                Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Checked = False
                Me.FechaPresentacionSIIFiltroHastaDateTimePicker.MinDate = DateTime.Now.AddYears(-5)
                Me.FechaPresentacionSIIFiltroHastaDateTimePicker.MaxDate = DateTime.Now.AddYears(+1)
                If Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Checked Or anoDesdeHasta = "" Then
                    Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.MaxDate = DateTime.Now.AddYears(+1)
                End If
            End If
        Catch ex As Exception
            frmMain.AddToLog("ERROR al limpiar el filtro de la presentación SII (hasta): " & ex.Message)
            MessageBox.Show("ERROR al limpiar el filtro de la presentación SII (hasta): " & ex.Message)
        End Try

    End Sub
    ''' <summary>
    ''' Evento al pulsar con el ratón para poner el foco en el filtro de la fecha
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub FechaPresentacionSIIFiltroDesdeDateTimePicker_MouseDown(sender As Object, e As MouseEventArgs) Handles FechaPresentacionSIIFiltroDesdeDateTimePicker.MouseDown
        Try
            If Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Checked Then
                Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Focus()
            End If
        Catch ex As Exception
            frmMain.AddToLog("ERROR al poner el foco en el filtro de la presentación SII (desde): " & ex.Message)
            MessageBox.Show("ERROR al poner el foco en el filtro de la presentación SII (desde): " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Evento al pulsar con el ratón para poner el foco en el filtro de la fecha
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub FechaPresentacionSIIFiltroHastaDateTimePicker_MouseDown(sender As Object, e As MouseEventArgs) Handles FechaPresentacionSIIFiltroHastaDateTimePicker.MouseDown
        Try
            If Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Checked Then
                Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Focus()
            End If
        Catch ex As Exception
            frmMain.AddToLog("ERROR al poner el foco en el filtro de la presentación SII (hasta): " & ex.Message)
            MessageBox.Show("ERROR al poner el foco en el filtro de la presentación SII (hasta): " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Evento al cambiar el valor seleccionado del filtro de la fecha (desde) que cambia el formato del datetimepicker y ejecuta el filtro
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub FechaPresentacionSIIFiltroDesdeDateTimePicker_ValueChanged(sender As Object, e As EventArgs) Handles FechaPresentacionSIIFiltroDesdeDateTimePicker.ValueChanged
        Try
            If Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Checked = False Then
                ClearFechaPresentacionSIIFiltro("desde")
            Else
                Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Format = DateTimePickerFormat.Custom
                Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.CustomFormat = "dd/MM/yyyy"
                'Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.ShowUpDown = True
                Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Checked = True
                If Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Checked Then
                    Me.FechaPresentacionSIIFiltroHastaDateTimePicker.MinDate = Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Value
                End If
                Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Focus()
            End If
            FiltrarListado()
        Catch ex As Exception
            frmMain.AddToLog("ERROR al cambiar el valor en el filtro de la presentación SII (desde): " & ex.Message)
            MessageBox.Show("ERROR al cambiar el valor en el filtro de la presentación SII (desde): " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Evento al cambiar el valor seleccionado del filtro de la fecha (hasta) que cambia el formato del datetimepicker y ejecuta el filtro
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub FechaPresentacionSIIFiltroHastaDateTimePicker_ValueChanged(sender As Object, e As EventArgs) Handles FechaPresentacionSIIFiltroHastaDateTimePicker.ValueChanged
        Try
            If Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Checked = False Then
                Me.ClearFechaPresentacionSIIFiltro("hasta")
            Else
                Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Format = DateTimePickerFormat.Custom
                Me.FechaPresentacionSIIFiltroHastaDateTimePicker.CustomFormat = "dd/MM/yyyy"
                'Me.FechaPresentacionSIIFiltroHastaDateTimePicker.ShowUpDown = True
                Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Checked = True
                If Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Checked Then
                    Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.MaxDate = Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Value
                End If
                Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Focus()
            End If
            FiltrarListado()
        Catch ex As Exception
            frmMain.AddToLog("ERROR al cambiar el valor en el filtro de la presentación SII (hasta): " & ex.Message)
            MessageBox.Show("ERROR al cambiar el valor en el filtro de la presentación SII (hasta): " & ex.Message)
        End Try

    End Sub
    ''' <summary>
    ''' Evento al pulsar sobre el botón de borrado del filtro de la fecha que ejecuta el borrado del campo y ejecuta el filtro
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub FechaPresentacionSIIFiltroClearButton_Click(sender As Object, e As EventArgs) Handles FechaPresentacionSIIFiltroClearButton.Click
        Me.ClearFechaPresentacionSIIFiltro()
        Me.FiltrarListado()
    End Sub
    ''' <summary>
    ''' Evento al pulsar sobre el checkbox 'sin fecha de presentación' del filtro activa/desactiva los campos de las fechas y ejecuta el filtro
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub FechaPresentacionSIIFiltroCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles FechaPresentacionSIIFiltroCheckBox.CheckedChanged
        If FechaPresentacionSIIFiltroCheckBox.Checked Then
            Me.FechaPresentacionSIIFiltroDesdeLabel.Enabled = False
            Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Enabled = False
            Me.FechaPresentacionSIIFiltroHastaLabel.Enabled = False
            Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Enabled = False
            Me.FechaPresentacionSIIFiltroClearButton.Enabled = False
        Else
            Me.FechaPresentacionSIIFiltroDesdeLabel.Enabled = True
            Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Enabled = True
            Me.FechaPresentacionSIIFiltroHastaLabel.Enabled = True
            Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Enabled = True
            Me.FechaPresentacionSIIFiltroClearButton.Enabled = True

            Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Format = DateTimePickerFormat.Custom
            Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.CustomFormat = "dd/MM/yyyy"
            'Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.ShowUpDown = True
            Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Checked = True
            If Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Checked Then
                Me.FechaPresentacionSIIFiltroHastaDateTimePicker.MinDate = Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Value
            End If
            Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Focus()

            Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Format = DateTimePickerFormat.Custom
            Me.FechaPresentacionSIIFiltroHastaDateTimePicker.CustomFormat = "dd/MM/yyyy"
            'Me.FechaPresentacionSIIFiltroHastaDateTimePicker.ShowUpDown = True
            Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Checked = True
            If Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Checked Then
                Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.MaxDate = Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Value
            End If
            ClearFechaPresentacionSIIFiltro()
        End If
        Me.FiltrarListado()
    End Sub
    ' ERRORES SII
    ''' <summary>
    ''' Evento al pulsar sobre el checkbox 'con errores' del filtro que ejecuta el filtro
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub ErroresSIIFiltroCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ErroresSIIFiltroCheckBox.CheckedChanged
        Me.FiltrarListado()
    End Sub
    ' FACE
    Private Sub FACEFiltroCheckBox_CheckedChanged(sender As Object, e As EventArgs)
        Me.FiltrarListado()
    End Sub
    ''' <summary>
    ''' Borra el filtro de la fecha de presentación del FACE, el campo 'desde', el campo 'hasta', o ambos
    ''' </summary>
    ''' <param name="anoDesdeHasta">Texto para especificar qué se borra: 'desde', 'hasta', o todo, por defecto vacío, que borra todo</param>
    Private Sub ClearFechaPresentacionFACEFiltro(Optional anoDesdeHasta As String = "")
        Try
            If anoDesdeHasta = "desde" Or anoDesdeHasta = "" Then
                Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Format = DateTimePickerFormat.Custom
                Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.CustomFormat = " "
                Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.ShowCheckBox = True
                Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Checked = False
                Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.MinDate = DateTime.Now.AddYears(-5)
                Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.MaxDate = DateTime.Now.AddYears(+1)
                If Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Checked Or anoDesdeHasta = "" Then
                    Me.FechaPresentacionFACEFiltroHastaDateTimePicker.MinDate = DateTime.Now.AddYears(-5)
                End If
            End If
        Catch ex As Exception
            frmMain.AddToLog("ERROR al limpiar el filtro de la presentación FACE (desde): " & ex.Message)
            MessageBox.Show("ERROR al limpiar el filtro de la presentación FACE (desde): " & ex.Message)
        End Try

        Try
            If anoDesdeHasta = "hasta" Or anoDesdeHasta = "" Then
                Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Format = DateTimePickerFormat.Custom
                Me.FechaPresentacionFACEFiltroHastaDateTimePicker.CustomFormat = " "
                Me.FechaPresentacionFACEFiltroHastaDateTimePicker.ShowCheckBox = True
                Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Checked = False
                Me.FechaPresentacionFACEFiltroHastaDateTimePicker.MinDate = DateTime.Now.AddYears(-5)
                Me.FechaPresentacionFACEFiltroHastaDateTimePicker.MaxDate = DateTime.Now.AddYears(+1)
                If Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Checked Or anoDesdeHasta = "" Then
                    Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.MaxDate = DateTime.Now.AddYears(+1)
                End If
            End If
        Catch ex As Exception
            frmMain.AddToLog("ERROR al limpiar el filtro de la presentación FACE (hasta): " & ex.Message)
            MessageBox.Show("ERROR al limpiar el filtro de la presentación FACE (hasta): " & ex.Message)
        End Try

    End Sub
    ''' <summary>
    ''' Evento al pulsar con el ratón para poner el foco en el filtro de la fecha
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub FechaPresentacionFACEFiltroDesdeDateTimePicker_MouseDown(sender As Object, e As MouseEventArgs) Handles FechaPresentacionFACEFiltroDesdeDateTimePicker.MouseDown
        Try
            If Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Checked Then
                Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Focus()
            End If
        Catch ex As Exception
            frmMain.AddToLog("ERROR al poner el foco en el filtro de la presentación FACE (desde): " & ex.Message)
            MessageBox.Show("ERROR al poner el foco en el filtro de la presentación FACE (desde): " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Evento al pulsar con el ratón para poner el foco en el filtro de la fecha
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub FechaPresentacionFACEFiltroHastaDateTimePicker_MouseDown(sender As Object, e As MouseEventArgs) Handles FechaPresentacionFACEFiltroHastaDateTimePicker.MouseDown
        Try
            If Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Checked Then
                Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Focus()
            End If
        Catch ex As Exception
            frmMain.AddToLog("ERROR al poner el foco en el filtro de la presentación FACE (hasta): " & ex.Message)
            MessageBox.Show("ERROR al poner el foco en el filtro de la presentación FACE (hasta): " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Evento al cambiar el valor seleccionado del filtro de la fecha (desde) que cambia el formato del datetimepicker y ejecuta el filtro
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub FechaPresentacionFACEFiltroDesdeDateTimePicker_ValueChanged(sender As Object, e As EventArgs) Handles FechaPresentacionFACEFiltroDesdeDateTimePicker.ValueChanged
        Try
            If Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Checked = False Then
                ClearFechaPresentacionFACEFiltro("desde")
            Else
                Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Format = DateTimePickerFormat.Custom
                Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.CustomFormat = "dd/MM/yyyy"
                'Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.ShowUpDown = True
                Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Checked = True
                If Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Checked Then
                    Me.FechaPresentacionFACEFiltroHastaDateTimePicker.MinDate = Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Value
                End If
                Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Focus()
            End If
            FiltrarListado()
        Catch ex As Exception
            frmMain.AddToLog("ERROR al cambiar el valor en el filtro de la presentación FACE (desde): " & ex.Message)
            MessageBox.Show("ERROR al cambiar el valor en el filtro de la presentación FACE (desde): " & ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' Evento al cambiar el valor seleccionado del filtro de la fecha (hasta) que cambia el formato del datetimepicker y ejecuta el filtro
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub FechaPresentacionFACEFiltroHastaDateTimePicker_ValueChanged(sender As Object, e As EventArgs) Handles FechaPresentacionFACEFiltroHastaDateTimePicker.ValueChanged
        Try
            If Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Checked = False Then
                Me.ClearFechaPresentacionFACEFiltro("hasta")
            Else
                Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Format = DateTimePickerFormat.Custom
                Me.FechaPresentacionFACEFiltroHastaDateTimePicker.CustomFormat = "dd/MM/yyyy"
                'Me.FechaPresentacionFACEFiltroHastaDateTimePicker.ShowUpDown = True
                Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Checked = True
                If Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Checked Then
                    Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.MaxDate = Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Value
                End If
                Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Focus()
            End If
            FiltrarListado()
        Catch ex As Exception
            frmMain.AddToLog("ERROR al cambiar el valor en el filtro de la presentación FACE (hasta): " & ex.Message)
            MessageBox.Show("ERROR al cambiar el valor en el filtro de la presentación FACE (hasta): " & ex.Message)
        End Try

    End Sub
    ''' <summary>
    ''' Evento al pulsar sobre el botón de borrado del filtro de la fecha que ejecuta el borrado del campo y ejecuta el filtro
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub FechaPresentacionFACEFiltroClearButton_Click(sender As Object, e As EventArgs) Handles FechaPresentacionFACEFiltroClearButton.Click
        Me.ClearFechaPresentacionFACEFiltro()
        Me.FiltrarListado()
    End Sub
    ''' <summary>
    ''' Evento al pulsar sobre el checkbox 'sin fecha de presentación' del filtro activa/desactiva los campos de las fechas y ejecuta el filtro
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub FechaPresentacionFACEFiltroCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles FechaPresentacionFACEFiltroCheckBox.CheckedChanged
        If FechaPresentacionFACEFiltroCheckBox.Checked Then
            Me.FechaPresentacionFACEFiltroDesdeLabel.Enabled = False
            Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Enabled = False
            Me.FechaPresentacionFACEFiltroHastaLabel.Enabled = False
            Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Enabled = False
            Me.FechaPresentacionFACEFiltroClearButton.Enabled = False
        Else
            Me.FechaPresentacionFACEFiltroDesdeLabel.Enabled = True
            Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Enabled = True
            Me.FechaPresentacionFACEFiltroHastaLabel.Enabled = True
            Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Enabled = True
            Me.FechaPresentacionFACEFiltroClearButton.Enabled = True

            Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Format = DateTimePickerFormat.Custom
            Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.CustomFormat = "dd/MM/yyyy"
            'Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.ShowUpDown = True
            Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Checked = True
            If Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Checked Then
                Me.FechaPresentacionFACEFiltroHastaDateTimePicker.MinDate = Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Value
            End If
            Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Focus()

            Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Format = DateTimePickerFormat.Custom
            Me.FechaPresentacionFACEFiltroHastaDateTimePicker.CustomFormat = "dd/MM/yyyy"
            'Me.FechaPresentacionFACEFiltroHastaDateTimePicker.ShowUpDown = True
            Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Checked = True
            If Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Checked Then
                Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.MaxDate = Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Value
            End If
            ClearFechaPresentacionFACEFiltro()
        End If
        Me.FiltrarListado()
    End Sub
    ' ERRORES FACE
    ''' <summary>
    ''' Evento al pulsar sobre el checkbox 'con errores' del filtro que ejecuta el filtro
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub ErroresFACEFiltroCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ErroresFACEFiltroCheckBox.CheckedChanged
        Me.FiltrarListado()
    End Sub
    ''' <summary>
    ''' Evento al pulsar sobre las pestañas del filtro de SII y FACE que a su vez ejecuta el filtro
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub ServicioTabControl_Selected(sender As Object, e As TabControlEventArgs) Handles ServicioTabControl.Selected
        'MessageBox.Show(e.TabPage.ToString)
        Me.FiltrarListado()
    End Sub
    ' FILTRAR TODO
    ''' <summary>
    ''' Genera la consulta de filtrado según los campos seleccionados en los filtros y la ejecuta como filtro en la lista
    ''' </summary>
    Public Sub FiltrarListado()
        Dim sFiltro As String = ""
        Try
            If Not IsDBNull(Me.AnoFiltroDesdeDateTimePicker.Value) Then
                If Me.AnoFiltroDesdeDateTimePicker.Checked Then
                    If sFiltro.Length > 0 Then
                        sFiltro = sFiltro & " AND "
                    End If
                    sFiltro = sFiltro & "[ANOFACTURA]>='" & Me.AnoFiltroDesdeDateTimePicker.Value.ToString("yyyy") & "'"
                End If
            End If
            If Not IsDBNull(Me.AnoFiltroHastaDateTimePicker.Value) Then
                If Me.AnoFiltroHastaDateTimePicker.Checked Then
                    If sFiltro.Length > 0 Then
                        sFiltro = sFiltro & " AND "
                    End If
                    sFiltro = sFiltro & "[ANOFACTURA]<='" & Me.AnoFiltroHastaDateTimePicker.Value.ToString("yyyy") & "'"
                End If
            End If
            If Not IsDBNull(Me.SeriesFiltroComboBox.SelectedValue) Then
                If Not IsNothing(Me.SeriesFiltroComboBox.SelectedValue) Then
                    If Me.SeriesFiltroComboBox.SelectedValue.ToString.Trim() <> "Todas" And Me.SeriesFiltroComboBox.SelectedValue.length > 0 Then
                        If sFiltro.Length > 0 Then
                            sFiltro = sFiltro & " AND "
                        End If
                        sFiltro = sFiltro & "[SERIE]='" & Me.SeriesFiltroComboBox.SelectedValue & "'"
                    End If
                End If
            End If
            If Not IsDBNull(Me.ZonasFiltroComboBox.SelectedValue) Then
                If Not IsNothing(Me.ZonasFiltroComboBox.SelectedValue) Then
                    If Me.ZonasFiltroComboBox.SelectedValue.ToString.Trim() <> "Todas" And Me.ZonasFiltroComboBox.SelectedValue.length > 0 Then
                        If sFiltro.Length > 0 Then
                            sFiltro = sFiltro & " AND "
                        End If
                        sFiltro = sFiltro & "[CODIGOZONA]='" & Me.ZonasFiltroComboBox.SelectedValue & "'"
                    End If
                End If
            End If
            If Not IsDBNull(Me.PeriodosFiltroComboBox.SelectedValue) Then
                If Not IsNothing(Me.PeriodosFiltroComboBox.SelectedValue) Then
                    If Me.PeriodosFiltroComboBox.SelectedValue.ToString.Trim() <> "Todos" And Me.PeriodosFiltroComboBox.SelectedValue.length > 0 Then
                        If sFiltro.Length > 0 Then
                            sFiltro = sFiltro & " AND "
                        End If
                        sFiltro = sFiltro & "[PERIODO]='" & Me.PeriodosFiltroComboBox.SelectedValue & "'"
                    End If
                End If
            End If
            ' SII o FACE TAB
            'If Me.ServicioTabControl.SelectedTab.Text.ToString = "SII" Then
            '    If sFiltro.Length > 0 Then
            '        sFiltro = sFiltro & " AND "
            '    End If
            '    sFiltro = sFiltro & "([ORGANO_GESTOR_FACE] Is NULL OR [ORGANO_GESTOR_FACE]='')"
            'End If
            If Me.ServicioTabControl.SelectedTab.Text.ToString = "FACE" Then
                If sFiltro.Length > 0 Then
                    sFiltro = sFiltro & " AND "
                End If
                sFiltro = sFiltro & "(NOT [ORGANO_GESTOR_FACE] Is NULL AND NOT [ORGANO_GESTOR_FACE]='')"
            End If
            ' SII
            If FechaPresentacionSIIFiltroCheckBox.Checked Then
                If sFiltro.Length > 0 Then
                    sFiltro = sFiltro & " AND "
                End If
                sFiltro = sFiltro & "([FECHA_ENVIO_SII] Is NULL OR [FECHA_ENVIO_SII]='')"
            Else
                If Not IsDBNull(Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Value) Then
                    If Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Checked Then
                        If sFiltro.Length > 0 Then
                            sFiltro = sFiltro & " AND "
                        End If
                        sFiltro = sFiltro & "([FECHA_ENVIO_SII]>='" & Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Value.ToString("yyyyMMdd") & "' AND NOT [FECHA_ENVIO_SII] Is NULL AND NOT [FECHA_ENVIO_SII]='')"
                    End If
                End If
                If Not IsDBNull(Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Value) Then
                    If Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Checked Then
                        If sFiltro.Length > 0 Then
                            sFiltro = sFiltro & " AND "
                        End If
                        sFiltro = sFiltro & "([FECHA_ENVIO_SII]<='" & Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Value.ToString("yyyyMMdd") & "' AND NOT [FECHA_ENVIO_SII] Is NULL AND NOT [FECHA_ENVIO_SII]='')"
                    End If
                End If
            End If
            If ErroresSIIFiltroCheckBox.Checked Then
                If sFiltro.Length > 0 Then
                    sFiltro = sFiltro & " AND "
                End If
                sFiltro = sFiltro & "LEN([ERROR_PRESENTACION_SII]) > 0"
            End If
            ' FACE
            'If Me.FACEFiltroCheckBox.Checked Then
            '    If sFiltro.Length > 0 Then
            '        sFiltro = sFiltro & " AND "
            '    End If
            '    sFiltro = sFiltro & "ESFACE=1"
            'End If
            If FechaPresentacionFACEFiltroCheckBox.Checked Then
                If sFiltro.Length > 0 Then
                    sFiltro = sFiltro & " AND "
                End If
                sFiltro = sFiltro & "([FECHA_PRESENTACION_FACE] Is NULL OR [FECHA_PRESENTACION_FACE]='')"
            Else
                If Not IsDBNull(Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Value) Then
                    If Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Checked Then
                        If sFiltro.Length > 0 Then
                            sFiltro = sFiltro & " AND "
                        End If
                        sFiltro = sFiltro & "[FECHA_PRESENTACION_FACE]>='" & Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Value.ToString("yyyyMMdd") & "'"
                    End If
                End If
                If Not IsDBNull(Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Value) Then
                    If Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Checked Then
                        If sFiltro.Length > 0 Then
                            sFiltro = sFiltro & " AND "
                        End If
                        sFiltro = sFiltro & "[FECHA_PRESENTACION_FACE]<='" & Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Value.ToString("yyyyMMdd") & "'"
                    End If
                End If
            End If
            If ErroresFACEFiltroCheckBox.Checked Then
                If sFiltro.Length > 0 Then
                    sFiltro = sFiltro & " AND "
                End If
                sFiltro = sFiltro & "LEN([ERROR_PRESENTACION_FACE]) > 0"
            End If

            'MessageBox.Show(sFiltro)
            myDataGridViewBindingSource.Filter = sFiltro
            Me.Text = "Registro de facturas con AAPP (" & String.Format("{0:#,##0}", Me.dgvPRESENTACIONFACTURACION.Rows.Count) & ")"
        Catch ex As Exception
            frmMain.AddToLog("ERROR al generar el filtro: " & ex.Message)
            MessageBox.Show("ERROR al generar el filtro: " & ex.Message)
        End Try
    End Sub
    ' GENERAR Y PRESENTAR SII
    ''' <summary>
    ''' Evento del botón para iniciar el proceso de presentación en el servicio SII
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub PresentarSIIButton_Click(sender As Object, e As EventArgs) Handles PresentarSIIButton.Click
        Dim sMessageText As String
        Dim sNombreArchivo As String = ""
        Dim sRutaYNombreArchivo As String = ""
        Dim nFacturasCount As Long = 0
        Dim sNregistro As String
        Dim sNFactura As String
        Dim sAno As String
        Dim sZona As String
        Dim sNIF_PRESENTADOR_Main As String
        Dim sNOMBRE_PRESENTADOR_Main As String
        Dim sNIF_PRESENTADOR As String
        Dim sNOMBRE_PRESENTADOR As String
        Dim sSQL As String = ""
        Dim sSelect As String = ""
        Dim sFrom As String = ""
        Dim sWhere As String = ""

        ' NOMBRE DEL ARCHIVO XML
        sNombreArchivo = sNombreArchivo & "SII"
        sNombreArchivo = sNombreArchivo & "_SERIES_" & Me.SeriesFiltroComboBox.SelectedValue.Trim()
        Dim sZonas As String = ""
        If IsDBNull(Me.ZonasFiltroComboBox.SelectedValue) Then
            sZonas = "Todas"
        Else
            sZonas = Me.ZonasFiltroComboBox.SelectedValue.trim()
            Dim rowView As DataRowView = TryCast(Me.ZonasFiltroComboBox.SelectedItem, DataRowView)
            If (Not rowView Is Nothing) Then
                Dim row As DataRow = rowView.Row
                sZonas = DirectCast(row.Item("CODIGOZONA"), String).Trim().Replace(".", "_")
            End If
        End If
        sNombreArchivo = sNombreArchivo & "_ZONAS_" & sZonas
        Dim sPeriodos As String = ""
        sPeriodos = TryCast(Me.PeriodosFiltroComboBox.SelectedValue.Replace(" ", "_").Replace("º", "").Replace("ª", "").Trim(), String)
        For Each c In Path.GetInvalidFileNameChars()
            If sPeriodos.Contains(c) Then
                sPeriodos = sPeriodos.Replace(c, "")
            End If
        Next
        sNombreArchivo = sNombreArchivo & "_PERIODOS_" & sPeriodos
        sNombreArchivo = sNombreArchivo & "_" & Format(Now(), "ddMMyyyy") & "_" & Format(Now(), "Hmmss")
        sNombreArchivo = sNombreArchivo & ".xml"
        sRutaYNombreArchivo = Path.Combine(frmMain.sApplicationPath, sNombreArchivo)

        ' NUMERO DE FACTURAS A PROCESAR
        nFacturasCount = Me.dgvPRESENTACIONFACTURACION.Rows.Count

        If nFacturasCount > 0 Then
            sMessageText = "Te dispones a iniciar el proceso de presentación de facturas emitidas mediante el servicio SII. A continuación podrás decidir si generar el fichero XML con la factura mostrada actualmente en el listado y/o presentar dicha factura telemáticamente. ¿Estás seguro de querer continuar?"
            If nFacturasCount > 1 Then
                sMessageText = "Te dispones a iniciar el proceso de presentación de facturas emitidas mediante el servicio SII. A continuación podrás decidir si generar el fichero XML con las " & String.Format("{0:#,##0}", Me.dgvPRESENTACIONFACTURACION.Rows.Count) & " facturas mostradas actualmente en el listado y/o presentar dicha factura telemáticamente. ¿Estás seguro de querer continuar?"
            End If
            If MessageBox.Show(sMessageText, "SERVICIO SII", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = vbOK Then

                sSelect = "SELECT PRESENTACIONFACTURACION.*,TRIM(CONCAT(REGANTES.NOMBRE,CONCAT(' ',REGANTES.APELLIDOS))) AS NOMBREYAPELLIDOS,ZONAS.DESCRIPCION AS ZONA, CONCAT(CONCAT(CONCAT(SUBSTR(PRESENTACIONFACTURACION.FECHA, - 2), '-'), CONCAT(SUBSTR(PRESENTACIONFACTURACION.FECHA, 5, 2), '-')), SUBSTR(PRESENTACIONFACTURACION.FECHA, 1, 4)) AS FECHA_FORMATTED, DECODE(NVL(PRESENTACIONFACTURACION.FECHA_ENVIO_SII, ''), '', '', CONCAT(CONCAT(CONCAT(SUBSTR(PRESENTACIONFACTURACION.FECHA_ENVIO_SII, -2), '-'), CONCAT(SUBSTR(PRESENTACIONFACTURACION.FECHA_ENVIO_SII, 5, 2), '-')), SUBSTR(PRESENTACIONFACTURACION.FECHA_ENVIO_SII, 1, 4))) As FECHA_ENVIO_SII_FORMATTED"
                sFrom = " FROM (PRESENTACIONFACTURACION LEFT OUTER JOIN ZONAS On ZONAS.CODIGOZONA = PRESENTACIONFACTURACION.CODIGOZONA) INNER JOIN REGANTES ON REGANTES.NIF_CIF = PRESENTACIONFACTURACION.NIF_CIF"
                sWhere = " WHERE " & myDataGridViewBindingSource.Filter.Replace("[", "PRESENTACIONFACTURACION.").Replace("]", "").Replace("LEN(", "LENGTH(")
                sWhere = sWhere.Replace("PRESENTACIONFACTURACION.ORGANO_GESTOR_FACE", "REGANTES.ORGANO_GESTOR_FACE")
                sSQL = sSelect & sFrom & sWhere
                Dim dtFacturas As DataTable = Nothing
                dtFacturas = frmMain.DameRegistros(sSQL)

                sNIF_PRESENTADOR_Main = frmMain.sSIIPresentadorNIF
                sNOMBRE_PRESENTADOR_Main = frmMain.sSIIPresentadorNombre


                Dim bPRUEBA As Boolean = Me.DEBUGCheckBox.Checked


                ' /////////////////////////////////
                '   org.gobiernodecanarias.www
                ' /////////////////////////////////

                Dim oSuministroFacturasEmitidas As org.gobiernodecanarias.www.SuministroLRFacturasEmitidas = New org.gobiernodecanarias.www.SuministroLRFacturasEmitidas

                ' CABECERA
                Dim oCabecera As org.gobiernodecanarias.www.CabeceraSii = New org.gobiernodecanarias.www.CabeceraSii
                Dim oTitular As org.gobiernodecanarias.www.PersonaFisicaJuridicaESType = New org.gobiernodecanarias.www.PersonaFisicaJuridicaESType
                oTitular.NombreRazon = sNOMBRE_PRESENTADOR_Main
                oTitular.NIF = sNIF_PRESENTADOR_Main
                oCabecera.Titular = oTitular
                'oTitularmiType.NIFRepresentante = ""
                ' Version
                Dim oVersionSII As org.gobiernodecanarias.www.VersionSiiType = New org.gobiernodecanarias.www.VersionSiiType
                oVersionSII = org.gobiernodecanarias.www.VersionSiiType.Item10
                oCabecera.IDVersionSii = oVersionSII
                ' TipoComunicacion
                Dim oTipoComunicacion As org.gobiernodecanarias.www.ClaveTipoComunicacionType = New org.gobiernodecanarias.www.ClaveTipoComunicacionType
                oTipoComunicacion = org.gobiernodecanarias.www.ClaveTipoComunicacionType.A0
                oCabecera.TipoComunicacion = oTipoComunicacion

                Dim nCountIndex As Long = 0
                Dim nMaxNFacturas As Long = Me.dgvPRESENTACIONFACTURACION.Rows.Count

                ' REGISTROLRFACTURASEMITIDAS
                Dim oLRFacturasEmitidas(nMaxNFacturas) As org.gobiernodecanarias.www.LRfacturasEmitidasType


                ' INICIAR BUCLE POR LOS REGISTROS
                For Each thisRow As DataRow In dtFacturas.Rows

                    ' Recuperar datos de PRESENTADOR
                    sNregistro = thisRow("NREGISTRO")
                    sNFactura = thisRow("NFACTURA")
                    sAno = thisRow("ANOFACTURA")
                    sZona = thisRow("CODIGOZONA")
                    If sZona = "Sin zona" Then
                        sZona = "0"
                    End If
                    sNIF_PRESENTADOR = frmMain.DameRegistro("SELECT NIF_PRESENTADOR FROM PRESENTADOR WHERE CODIGOZONA = '" & sZona & "'", "NIF_PRESENTADOR")
                    sNOMBRE_PRESENTADOR = frmMain.DameRegistro("SELECT PRESENTADOR FROM PRESENTADOR WHERE CODIGOZONA = '" & sZona & "'", "PRESENTADOR")
                    If sNIF_PRESENTADOR = "" Then
                        sNIF_PRESENTADOR = frmMain.DameRegistro("SELECT NIF_PRESENTADOR FROM PRESENTADOR WHERE CODIGOZONA = '1'", "NIF_PRESENTADOR")
                        sNOMBRE_PRESENTADOR = frmMain.DameRegistro("SELECT PRESENTADOR FROM PRESENTADOR WHERE CODIGOZONA = '1'", "PRESENTADOR")
                    End If

                    oLRFacturasEmitidas(nCountIndex) = New org.gobiernodecanarias.www.LRfacturasEmitidasType

                    ' PeriodoLiquidacion
                    Dim oPeriodoLiquidacion As org.gobiernodecanarias.www.RegistroSiiPeriodoLiquidacion = New org.gobiernodecanarias.www.RegistroSiiPeriodoLiquidacion
                    oPeriodoLiquidacion.Ejercicio = Mid(thisRow("FECHA"), 1, 4)
                    ' Periodo
                    Dim oPeriodo As org.gobiernodecanarias.www.TipoPeriodoType = New org.gobiernodecanarias.www.TipoPeriodoType
                    Dim sTipoPeriodo As String = Mid(thisRow("FECHA"), 5, 2)
                    Select Case sTipoPeriodo
                        Case "01"
                            oPeriodo = org.gobiernodecanarias.www.TipoPeriodoType.Item01
                        Case "02"
                            oPeriodo = org.gobiernodecanarias.www.TipoPeriodoType.Item02
                        Case "03"
                            oPeriodo = org.gobiernodecanarias.www.TipoPeriodoType.Item03
                        Case "04"
                            oPeriodo = org.gobiernodecanarias.www.TipoPeriodoType.Item04
                        Case "05"
                            oPeriodo = org.gobiernodecanarias.www.TipoPeriodoType.Item05
                        Case "06"
                            oPeriodo = org.gobiernodecanarias.www.TipoPeriodoType.Item06
                        Case "07"
                            oPeriodo = org.gobiernodecanarias.www.TipoPeriodoType.Item07
                        Case "08"
                            oPeriodo = org.gobiernodecanarias.www.TipoPeriodoType.Item08
                        Case "09"
                            oPeriodo = org.gobiernodecanarias.www.TipoPeriodoType.Item09
                        Case "10"
                            oPeriodo = org.gobiernodecanarias.www.TipoPeriodoType.Item10
                        Case "11"
                            oPeriodo = org.gobiernodecanarias.www.TipoPeriodoType.Item11
                        Case "12"
                            oPeriodo = org.gobiernodecanarias.www.TipoPeriodoType.Item12
                        Case Else
                            oPeriodo = Nothing
                    End Select
                    oPeriodoLiquidacion.Periodo = oPeriodo
                    oLRFacturasEmitidas(nCountIndex).PeriodoLiquidacion = oPeriodoLiquidacion

                    'IDEmisorFactura
                    Dim oIDEmisorFactura As org.gobiernodecanarias.www.IDFacturaExpedidaTypeIDEmisorFactura = New org.gobiernodecanarias.www.IDFacturaExpedidaTypeIDEmisorFactura
                    oIDEmisorFactura.NIF = sNIF_PRESENTADOR
                    'IDFactura
                    Dim oIDFacturaExpedidaType As org.gobiernodecanarias.www.IDFacturaExpedidaType = New org.gobiernodecanarias.www.IDFacturaExpedidaType
                    oIDFacturaExpedidaType.IDEmisorFactura = oIDEmisorFactura
                    oIDFacturaExpedidaType.FechaExpedicionFacturaEmisor = thisRow("FECHA_FORMATTED") '"dd-MM-YY"
                    oIDFacturaExpedidaType.NumSerieFacturaEmisor = thisRow("SERIE") & " " & thisRow("NFACTURA") & "/" & thisRow("ANOFACTURA")
                    'oIDFacturaExpedidaType.NumSerieFacturaEmisorResumenFin = ""
                    oLRFacturasEmitidas(nCountIndex).IDFactura = oIDFacturaExpedidaType

                    ' FacturaExpedida
                    Dim oFacturaExpedidaType As org.gobiernodecanarias.www.FacturaExpedidaType = New org.gobiernodecanarias.www.FacturaExpedidaType
                    ' TipoFactura
                    Dim oTipoFactura As org.gobiernodecanarias.www.ClaveTipoFacturaType = New org.gobiernodecanarias.www.ClaveTipoFacturaType

                    Dim bFacturaRectificada As Boolean = False
                    Dim sSERVICIO As String
                    If thisRow("SERIE") = "FR" Then
                        ' FACTURA RECTIFICADA
                        bFacturaRectificada = True
                        oTipoFactura = org.gobiernodecanarias.www.ClaveTipoFacturaType.R1
                        ' TipoRectificativa
                        Dim oTipoRectificativa As org.gobiernodecanarias.www.ClaveTipoRectificativaType
                        oTipoRectificativa = org.gobiernodecanarias.www.ClaveTipoRectificativaType.S
                        oFacturaExpedidaType.TipoRectificativaSpecified = vbTrue
                        oFacturaExpedidaType.TipoRectificativa = oTipoRectificativa
                        ' FacturasRectificadas
                        Dim oFacturasRectificadas(1) As org.gobiernodecanarias.www.IDFacturaARType
                        oFacturasRectificadas(0) = New org.gobiernodecanarias.www.IDFacturaARType
                        oFacturasRectificadas(0).NumSerieFacturaEmisor = Trim(thisRow("MOTIVO_DEVOLUCION"))
                        ' Fecha
                        Dim sSERIE_RECTIFICADA As String = Mid(thisRow("MOTIVO_DEVOLUCION"), 1, 2)
                        Dim sNFACTURA_RECTIFICADA As String = Mid(thisRow("MOTIVO_DEVOLUCION"), 4, InStr(1, thisRow("MOTIVO_DEVOLUCION"), "/") - 4)
                        Dim sANO_RECTIFICADA As String = Mid(thisRow("MOTIVO_DEVOLUCION"), InStr(4, thisRow("MOTIVO_DEVOLUCION"), "/") + 1, 4)
                        sSERVICIO = frmMain.DameRegistro("SELECT FECHA FROM PRESENTACIONFACTURACION WHERE SERIE = '" & sSERIE_RECTIFICADA & "' AND NFACTURA = " & sNFACTURA_RECTIFICADA & " AND ANOFACTURA = " & sANO_RECTIFICADA, "FECHA")
                        If IsDBNull(sSERVICIO) Then
                            sSERVICIO = frmMain.DameRegistro("SELECT FECHA FROM PRESENTACIONFACTURACION3AN WHERE SERIE = '" & sSERIE_RECTIFICADA & "' AND NFACTURA = " & sNFACTURA_RECTIFICADA & " AND ANOFACTURA = " & sANO_RECTIFICADA, "FECHA")
                        Else
                            If sSERVICIO.Length = 0 Then
                                sSERVICIO = frmMain.DameRegistro("SELECT FECHA FROM PRESENTACIONFACTURACION3AN WHERE SERIE = '" & sSERIE_RECTIFICADA & "' AND NFACTURA = " & sNFACTURA_RECTIFICADA & " AND ANOFACTURA = " & sANO_RECTIFICADA, "FECHA")
                            End If
                        End If
                        If IsDBNull(sSERVICIO) Then
                            MsgBox("La factura " & thisRow("MOTIVO_DEVOLUCION") & " (factura a rectificar por " & thisRow("SERIE") & " " & thisRow("NFACTURA") & "/" & thisRow("ANOFACTURA") & ") no se puede localizar. Proceso abortado.", vbOKOnly + vbExclamation, "¡Atención!")
                            Exit Sub
                        Else
                            If sSERVICIO.Length = 0 Then
                                MsgBox("La factura " & thisRow("MOTIVO_DEVOLUCION") & " (factura a rectificar por " & thisRow("SERIE") & " " & thisRow("NFACTURA") & "/" & thisRow("ANOFACTURA") & ") no se puede localizar. Proceso abortado.", vbOKOnly + vbExclamation, "¡Atención!")
                                Exit Sub
                            Else
                                sSERVICIO = Mid(sSERVICIO, 7, 2) & "-" & Mid(sSERVICIO, 5, 2) & "-" & Mid(sSERVICIO, 1, 4)
                            End If
                        End If
                        ' FechaExpedicionFacturaEmisor
                        oFacturasRectificadas(0).FechaExpedicionFacturaEmisor = sSERVICIO '"dd-MM-YY"
                        oFacturaExpedidaType.FacturasRectificadas = oFacturasRectificadas
                        ' ImporteRectificacion
                        Dim oDesgloseRectificacionEmitType As org.gobiernodecanarias.www.DesgloseRectificacionEmitType = New org.gobiernodecanarias.www.DesgloseRectificacionEmitType
                        oDesgloseRectificacionEmitType.BaseRectificada = Replace(thisRow("TOTAL"), ",", ".")
                        oDesgloseRectificacionEmitType.CuotaRectificada = "0"
                        oFacturaExpedidaType.ImporteRectificacion = oDesgloseRectificacionEmitType
                        ' FechaOperacion
                        oFacturaExpedidaType.FechaOperacion = sSERVICIO '"dd-MM-YY"
                    Else
                        oTipoFactura = org.gobiernodecanarias.www.ClaveTipoFacturaType.F1
                    End If
                    oFacturaExpedidaType.TipoFactura = oTipoFactura

                    ' ClaveRegimenEspecialOTrascendencia
                    Dim oClaveRegimenEspecialOTranscendencia As org.gobiernodecanarias.www.IdOperacionesTrascendenciaTributariaType = New org.gobiernodecanarias.www.IdOperacionesTrascendenciaTributariaType
                    oClaveRegimenEspecialOTranscendencia = org.gobiernodecanarias.www.IdOperacionesTrascendenciaTributariaType.Item01
                    oFacturaExpedidaType.ClaveRegimenEspecialOTrascendencia = oClaveRegimenEspecialOTranscendencia
                    If bFacturaRectificada Then
                        ' ImporteTotal
                        oFacturaExpedidaType.ImporteTotal = "-" & Replace(thisRow("TOTAL"), ",", ".")
                        ' DescripcionOperacion
                        sSERVICIO = "FACTURA RECTIFICATIVA DE " & thisRow("MOTIVO_DEVOLUCION")
                        oFacturaExpedidaType.DescripcionOperacion = sSERVICIO '"FACTURA RECTIFICATIVA DE ..."
                        ' EmitidaPorTercerosODestinatario
                        Dim oEmitidaPorTercerosODestinatario As org.gobiernodecanarias.www.EmitidaPorTercerosType = New org.gobiernodecanarias.www.EmitidaPorTercerosType
                        oEmitidaPorTercerosODestinatario = org.gobiernodecanarias.www.EmitidaPorTercerosType.N
                        oFacturaExpedidaType.EmitidaPorTercerosODestinatario = oEmitidaPorTercerosODestinatario
                    Else
                        ' ImporteTotal
                        oFacturaExpedidaType.ImporteTotal = Replace(thisRow("TOTAL"), ",", ".")
                        ' DescripcionOperacion
                        sSERVICIO = frmMain.DameRegistro("SELECT DESCRIPCION FROM SERIES_FACTURAS WHERE SERIE = '" & thisRow("SERIE") & "'", "DESCRIPCION")
                        oFacturaExpedidaType.DescripcionOperacion = sSERVICIO '"FACTURA RECTIFICATIVA DE AG 2181/2020"
                        ' EmitidaPorTercerosODestinatario   OJO!!!
                        'Dim oEmitidaPorTercerosODestinatario As EmitidaPorTercerosType = New EmitidaPorTercerosType
                        'oEmitidaPorTercerosODestinatario = EmitidaPorTercerosType.N
                        'oFacturaExpedidaType.EmitidaPorTercerosODestinatario = oEmitidaPorTercerosODestinatario
                    End If

                    'oFacturaExpedidaType.FacturaSimplificadaArticulos72_73 = SimplificadaCualificadaType.N
                    'oFacturaExpedidaType.FacturaSimplificadaArticulos72_73Specified = False

                    ' Contraparte
                    Dim oContraparte As org.gobiernodecanarias.www.PersonaFisicaJuridicaType = New org.gobiernodecanarias.www.PersonaFisicaJuridicaType
                    oContraparte.NombreRazon = Replace(Replace(Replace(thisRow("NOMBREYAPELLIDOS"), "D. ", ""), "Da. ", ""), "Dª. ", "")
                    Dim oContraparteNIF As Object = thisRow("NIF_CIF")
                    oContraparte.Item = oContraparteNIF
                    oFacturaExpedidaType.Contraparte = oContraparte

                    ' TipoDesglose

                    ' TipoDesglose -> DesgloseFactura -> Sujeta -> Exenta
                    Dim oExentaType(1) As org.gobiernodecanarias.www.DetalleExentaType
                    oExentaType(0) = New org.gobiernodecanarias.www.DetalleExentaType

                    If IsDBNull(thisRow("BASEIGIC1")) Then
                        oExentaType(0).BaseImponible = "0.00"
                    Else
                        oExentaType(0).BaseImponible = Replace(thisRow("BASEIGIC1"), ",", ".")
                    End If

                    ' CausaExencion
                    Dim oCausaExencion As org.gobiernodecanarias.www.CausaExencionType = New org.gobiernodecanarias.www.CausaExencionType
                    oCausaExencion = org.gobiernodecanarias.www.CausaExencionType.E1
                    oExentaType(0).CausaExencion = oCausaExencion
                    oExentaType(0).CausaExencionSpecified = True


                    ' TipoDesglose -> DesgloseFactura -> Sujeta -> NoExenta
                    Dim oNoExentaType As org.gobiernodecanarias.www.SujetaTypeNoExenta = New org.gobiernodecanarias.www.SujetaTypeNoExenta
                    ' TipoDesglose -> DesgloseFactura -> Sujeta -> NoExenta -> TipoNoExenta   OJO!!!
                    Dim oTipoNoExenta As org.gobiernodecanarias.www.TipoOperacionSujetaNoExentaType = New org.gobiernodecanarias.www.TipoOperacionSujetaNoExentaType
                    oTipoNoExenta = org.gobiernodecanarias.www.TipoOperacionSujetaNoExentaType.S1
                    oNoExentaType.TipoNoExenta = oTipoNoExenta

                    ' TipoDesglose -> DesgloseFactura -> Sujeta -> NoExenta -> DesgloseIGIC -> DetalleIGIC
                    ' Base e impuestos desglosados por tipo
                    Dim nDetallesIGIC As Integer = 0
                    If Not (IsDBNull(thisRow("BASEIGIC1"))) Then
                        nDetallesIGIC += 1
                    End If
                    If Not (IsDBNull(thisRow("BASEIGIC2"))) Then
                        nDetallesIGIC += 1
                    End If
                    If Not (IsDBNull(thisRow("BASEIGIC3"))) Then
                        nDetallesIGIC += 1
                    End If
                    If Not (IsDBNull(thisRow("BASEIGIC4"))) Then
                        nDetallesIGIC += 1
                    End If
                    If Not (IsDBNull(thisRow("BASEIGIC5"))) Then
                        nDetallesIGIC += 1
                    End If
                    Dim oDetalleIGICEmitida() As org.gobiernodecanarias.www.DetalleIGICEmitidaType = Nothing
                    If nDetallesIGIC > 0 Then
                        ReDim oDetalleIGICEmitida(nDetallesIGIC) 'As org.gobiernodecanarias.www.DetalleIGICEmitidaType
                        Dim nIndiceDetalleIGIC As Integer = 0
                        If Not (IsDBNull(thisRow("BASEIGIC1"))) Then
                            If thisRow("BASEIGIC1").Length > 6 Then
                                If thisRow("BASEIGIC1").Substring(6, 1) = "," Or thisRow("BASEIGIC1").Substring(6, 1) = "." Then
                                    thisRow("BASEIGIC1") = thisRow("BASEIGIC1").Substring(0, 6) & "0" & thisRow("BASEIGIC1").Substring(6)
                                End If
                            End If
                            oDetalleIGICEmitida(nIndiceDetalleIGIC) = New org.gobiernodecanarias.www.DetalleIGICEmitidaType With {
                                .TipoImpositivo = Trim(Replace(Replace(Replace(thisRow("TEXTOIGIC1"), "IGIC", ""), "%", ""), ",", ".")),
                                .BaseImponible = Replace(Mid(thisRow("BASEIGIC1"), 7, Len(thisRow("BASEIGIC1")) - 6), ",", "."),
                                .CuotaRepercutida = Replace(thisRow("VALORIGIC1"), ",", ".")
                            }
                            nIndiceDetalleIGIC += 1
                        End If
                        If Not (IsDBNull(thisRow("BASEIGIC2"))) Then
                            If thisRow("BASEIGIC2").Length > 6 Then
                                If thisRow("BASEIGIC2").Substring(6, 1) = "," Or thisRow("BASEIGIC2").Substring(6, 1) = "." Then
                                    thisRow("BASEIGIC2") = thisRow("BASEIGIC2").Substring(0, 6) & "0" & thisRow("BASEIGIC2").Substring(6)
                                End If
                            End If
                            oDetalleIGICEmitida(nIndiceDetalleIGIC) = New org.gobiernodecanarias.www.DetalleIGICEmitidaType With {
                                .TipoImpositivo = Trim(Replace(Replace(Replace(thisRow("TEXTOIGIC2"), "IGIC", ""), "%", ""), ",", ".")),
                                .BaseImponible = Replace(Mid(thisRow("BASEIGIC2"), 7, Len(thisRow("BASEIGIC2")) - 6), ",", "."),
                                .CuotaRepercutida = Replace(thisRow("VALORIGIC2"), ",", ".")
                            }
                            nIndiceDetalleIGIC += 1
                        End If
                        If Not (IsDBNull(thisRow("BASEIGIC3"))) Then
                            If thisRow("BASEIGIC3").Length > 6 Then
                                If thisRow("BASEIGIC3").Substring(6, 1) = "," Or thisRow("BASEIGIC3").Substring(6, 1) = "." Then
                                    thisRow("BASEIGIC3") = thisRow("BASEIGIC3").Substring(0, 6) & "0" & thisRow("BASEIGIC3").Substring(6)
                                End If
                            End If
                            oDetalleIGICEmitida(nIndiceDetalleIGIC) = New org.gobiernodecanarias.www.DetalleIGICEmitidaType With {
                                .TipoImpositivo = Trim(Replace(Replace(Replace(thisRow("TEXTOIGIC3"), "IGIC", ""), "%", ""), ",", ".")),
                                .BaseImponible = Replace(Mid(thisRow("BASEIGIC3"), 7, Len(thisRow("BASEIGIC3")) - 6), ",", "."),
                                .CuotaRepercutida = Replace(thisRow("VALORIGIC3"), ",", ".")
                            }
                            nIndiceDetalleIGIC += 1
                        End If
                        If Not (IsDBNull(thisRow("BASEIGIC4"))) Then
                            If thisRow("BASEIGIC4").Length > 6 Then
                                If thisRow("BASEIGIC4").Substring(6, 1) = "," Or thisRow("BASEIGIC4").Substring(6, 1) = "." Then
                                    thisRow("BASEIGIC4") = thisRow("BASEIGIC4").Substring(0, 6) & "0" & thisRow("BASEIGIC4").Substring(6)
                                End If
                            End If
                            oDetalleIGICEmitida(nIndiceDetalleIGIC) = New org.gobiernodecanarias.www.DetalleIGICEmitidaType With {
                                .TipoImpositivo = Trim(Replace(Replace(Replace(thisRow("TEXTOIGIC4"), "IGIC", ""), "%", ""), ",", ".")),
                                .BaseImponible = Replace(Mid(thisRow("BASEIGIC4"), 7, Len(thisRow("BASEIGIC4")) - 6), ",", "."),
                                .CuotaRepercutida = Replace(thisRow("VALORIGIC4"), ",", ".")
                            }
                            nIndiceDetalleIGIC += 1
                        End If
                        If Not (IsDBNull(thisRow("BASEIGIC5"))) Then
                            If thisRow("BASEIGIC5").Length > 6 Then
                                If thisRow("BASEIGIC5").Substring(6, 1) = "," Or thisRow("BASEIGIC5").Substring(6, 1) = "." Then
                                    thisRow("BASEIGIC5") = thisRow("BASEIGIC5").Substring(0, 6) & "0" & thisRow("BASEIGIC5").Substring(6)
                                End If
                            End If
                            oDetalleIGICEmitida(nIndiceDetalleIGIC) = New org.gobiernodecanarias.www.DetalleIGICEmitidaType With {
                                .TipoImpositivo = Trim(Replace(Replace(Replace(thisRow("TEXTOIGIC5"), "IGIC", ""), "%", ""), ",", ".")),
                                .BaseImponible = Replace(Mid(thisRow("BASEIGIC5"), 7, Len(thisRow("BASEIGIC5")) - 6), ",", "."),
                                .CuotaRepercutida = Replace(thisRow("VALORIGIC5"), ",", ".")
                            }
                            nIndiceDetalleIGIC += 1
                        End If
                    End If


                    ' TipoDesglose -> DesgloseFactura -> Sujeta -> NoExenta -> DesgloseIGIC
                    oNoExentaType.DesgloseIGIC = oDetalleIGICEmitida

                    ' TipoDesglose -> DesgloseFactura -> Sujeta
                    Dim oSujetaType As org.gobiernodecanarias.www.SujetaType = New org.gobiernodecanarias.www.SujetaType
                    'oSujetaType.Exenta = miExentaType
                    oSujetaType.NoExenta = oNoExentaType

                    ' TipoDesglose -> DesgloseFactura -> NoSujeta
                    Dim oNoSujetaType As org.gobiernodecanarias.www.NoSujetaType = New org.gobiernodecanarias.www.NoSujetaType

                    ' TipoDesglose
                    Dim oDesgloseFactura As org.gobiernodecanarias.www.TipoSinDesgloseType = New org.gobiernodecanarias.www.TipoSinDesgloseType
                    oDesgloseFactura.Sujeta = oSujetaType
                    'oDesgloseFactura.NoSujeta = miNoSujetaType
                    Dim oConDesgloseFactura As org.gobiernodecanarias.www.TipoConDesgloseType = New org.gobiernodecanarias.www.TipoConDesgloseType
                    Dim oSinDesglosePrestacion As org.gobiernodecanarias.www.TipoSinDesglosePrestacionType = New org.gobiernodecanarias.www.TipoSinDesglosePrestacionType
                    oConDesgloseFactura.PrestacionServicios = oSinDesglosePrestacion
                    oConDesgloseFactura.Entrega = oDesgloseFactura
                    Dim oDesglose As org.gobiernodecanarias.www.FacturaExpedidaTypeTipoDesglose = New org.gobiernodecanarias.www.FacturaExpedidaTypeTipoDesglose
                    'oDesglose.Item = oConDesgloseFactura
                    oDesglose.Item = oDesgloseFactura
                    oFacturaExpedidaType.TipoDesglose = oDesglose


                    oLRFacturasEmitidas(nCountIndex).FacturaExpedida = oFacturaExpedidaType

                    nCountIndex += 1
                    ' FIN DE BUCLE DE REGISTROS
                Next


                oSuministroFacturasEmitidas.Cabecera = oCabecera
                oSuministroFacturasEmitidas.RegistroLRFacturasEmitidas = oLRFacturasEmitidas

                Dim soapAttrs As SoapAttributes = New SoapAttributes

                sMessageText = "¿Quieres guardar un archivo XML con la factura seleccionada?"
                If nFacturasCount > 1 Then
                    sMessageText = "¿Quieres guardar un archivo XML con las facturas seleccionadas?"
                End If
                If MessageBox.Show(sMessageText, "Guardar XML", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = vbOK Then
                    ' ***************************************
                    ' *** GUARDAR XML DE PRESENTACION SII ***
                    ' ***************************************
                    Dim oImporter As New SoapReflectionImporter
                    Dim oTypeMapping As XmlTypeMapping = oImporter.ImportTypeMapping(oSuministroFacturasEmitidas.GetType)
                    ' Especificar namespace
                    Dim oNamespaces As XmlSerializerNamespaces = New XmlSerializerNamespaces()
                    oNamespaces.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/")
                    oNamespaces.Add("sii", "https://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/ssii/igic/ws/SuministroInformacion.xsd")
                    oNamespaces.Add("siiLR", "https://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/ssii/igic/ws/SuministroLR.xsd")
                    Dim oSerializer As XmlSerializer
                    'oSerializer = New XmlSerializer(oSuministroFacturasEmitidas.GetType())
                    oSerializer = New XmlSerializer(oSuministroFacturasEmitidas.GetType, "https://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/ssii/fact/ws/SuministroLR.xsd")
                    'oSerializer = New XmlSerializer(oTypeMapping)
                    Dim oWriter As New XmlTextWriter(sRutaYNombreArchivo, System.Text.Encoding.UTF8)
                    oWriter.WriteStartElement("soapenv:Envelope")
                    oWriter.WriteAttributeString("xmlns", "soapenv", Nothing, "http://schemas.xmlsoap.org/soap/envelope/")
                    oWriter.WriteAttributeString("xmlns", "siiLR", Nothing, "https://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/ssii/igic/ws/SuministroLR.xsd")
                    oWriter.WriteAttributeString("xmlns", "sii", Nothing, "https://www2.agenciatributaria.gob.es/static_files/common/internet/dep/aplicaciones/es/aeat/ssii/igic/ws/SuministroInformacion.xsd")
                    oWriter.WriteStartElement("soapenv:Header")
                    oWriter.WriteEndElement()
                    oWriter.WriteStartElement("soapenv:Body")
                    oSerializer.Serialize(oWriter, oSuministroFacturasEmitidas)
                    'oSerializer.Serialize(oWriter, oSuministroFacturasEmitidas, oNamespaces)
                    oWriter.WriteEndElement()
                    oWriter.WriteEndElement()
                    oWriter.Close()

                    MessageBox.Show("Se ha guardado el archivo XML en " & sRutaYNombreArchivo, "Archivo guardado", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                sMessageText = "¿Quieres presentar la factura telemáticamente?"
                If nFacturasCount > 1 Then
                    sMessageText = "¿Quieres presentar las facturas telemáticamente?"
                End If
                If MessageBox.Show(sMessageText, "Procesar presentación", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = vbOK Then
                    ' ****************************************
                    ' *** REALIZAR PRESENTACION SII ONLINE ***
                    ' ****************************************
                    Dim oSIIServicioCliente As org.gobiernodecanarias.www.siiService = New org.gobiernodecanarias.www.siiService

                    ' PRUEBA
                    If bPRUEBA Then
                        oSIIServicioCliente.Url = "https://sede.gobcan.es/tributos/middlewarecaut/services/sii/SiiFactFEV1SOAP" ' Prueba
                    End If

                    Try
                        ' Cargar certificado para autenticación con servicio web
                        Dim oCertificado As New Security.Cryptography.X509Certificates.X509Certificate2
                        oCertificado = DameCertificado()
                        If Not IsNothing(oCertificado) Then
                            oSIIServicioCliente.ClientCertificates.Add(oCertificado)

                            ' < .NET 4.5
                            'ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 'TLS 1.2
                            'ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 'TLS 1.1
                            ' .NET 4.5
                            System.Net.ServicePointManager.Expect100Continue = True
                            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls Or System.Net.SecurityProtocolType.Tls11 Or System.Net.SecurityProtocolType.Tls12 Or System.Net.SecurityProtocolType.Ssl3
                            'System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) >= True)

                            ' Derivar/Saltar comprobación de validación de certificado SSL
                            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                                Function(se As Object,
                                cert As System.Security.Cryptography.X509Certificates.X509Certificate,
                                chain As System.Security.Cryptography.X509Certificates.X509Chain,
                                sslerror As System.Net.Security.SslPolicyErrors) True


                            ' Enviar solicitud y recibir respuesta
                            oSIIServicioCliente.InitializeLifetimeService()
                            Dim oRespuesta As org.gobiernodecanarias.www.RespuestaLRFEmitidasType
                            oRespuesta = oSIIServicioCliente.SuministroLRFacturasEmitidas(oSuministroFacturasEmitidas)

                            ' Restaurar comprobación de validación de certificado SSL
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = Nothing

                            oCertificado.Dispose()


                            ' **************************************************
                            ' *** GUARDAR RESULTADO  PRESENTACION SII ONLINE ***
                            ' **************************************************
                            ' Guardar respuesta como XML
                            sNombreArchivo = sNombreArchivo.Replace(".xml", "_Respuesta.xml")
                            sRutaYNombreArchivo = sRutaYNombreArchivo.Replace(".xml", "_Respuesta.xml")
                            Dim oWriterResponse As TextWriter = New StreamWriter(sRutaYNombreArchivo)
                            Dim oSerializerResponse As New XmlSerializer(oRespuesta.GetType())
                            oSerializerResponse.Serialize(oWriterResponse, oRespuesta)
                            oWriterResponse.Close()

                            ' Actualizar campos de base de datos en función de la respuesta
                            Dim sFechaAhora As String = String.Format("{0:yyyyMMdd}", Now)
                            Dim sFechaAhoraFormatted As String = String.Format("{0:dd/MM/yyyy}", Now)
                            Dim sRespuestaFactura As String
                            Dim sRespuestaFacturaSerie As String
                            Dim sRespuestaFacturaNumero As String
                            Dim sRespuestaFacturaAno As String
                            Dim bRespuestaFacturaFound As Boolean = False

                            Dim nErroresCount As Integer = 0
                            Dim sRespuestaError As String
                            Dim sRespuestaErrores As String = ""

                            Dim nAceptadoErroresCount As Integer = 0
                            Dim sRespuestaAceptadoError As String
                            Dim sRespuestaAceptadoErrores As String = ""

                            Dim sRespuestaInfo As String = ""
                            For Each oRespuestaLinea In oRespuesta.RespuestaLinea
                                sRespuestaFactura = oRespuestaLinea.IDFactura.NumSerieFacturaEmisor
                                ' Formato de factura >> serie numerofactura/anofactura <<
                                Dim rxPatron As String = "(\w+)\s+(\d+)/(\d{4})"
                                Dim rxExpresion As Regex = New Regex(rxPatron, RegexOptions.IgnoreCase)
                                Dim rxCoincidencia As Match = rxExpresion.Match(sRespuestaFactura)
                                If (rxCoincidencia.Success) Then
                                    sRespuestaFacturaSerie = rxCoincidencia.Groups(1).Value
                                    sRespuestaFacturaNumero = rxCoincidencia.Groups(2).Value
                                    sRespuestaFacturaAno = rxCoincidencia.Groups(3).Value
                                    bRespuestaFacturaFound = True
                                Else
                                    ' No es un número de factura válido
                                    bRespuestaFacturaFound = False
                                    Continue For
                                End If
                                Try
                                    sSQL = "SELECT NREGISTRO FROM PRESENTACIONFACTURACION WHERE NFACTURA=" & sRespuestaFacturaNumero & " AND ANOFACTURA=" & sRespuestaFacturaAno & " AND SERIE='" & sRespuestaFacturaSerie & "'"
                                    Dim nNREGISTRO As Long
                                    nNREGISTRO = frmMain.DameRegistro(sSQL, "NREGISTRO")
                                    Dim drCurrent As DataRow
                                    drCurrent = Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Rows.Find(nNREGISTRO)

                                    If oRespuestaLinea.EstadoRegistro = SII_GobCan_WS.EstadoRegistroType.Correcto Then
                                        ' CORRECTO
                                        drCurrent.BeginEdit()
                                        drCurrent("FECHA_ENVIO_SII") = sFechaAhora
                                        'drCurrent("FECHA_PRESENTACION_SII") = sFechaAhora
                                        'drCurrent("FECHA_ERROR_PRESENTACION_SII") = DBNull.Value
                                        'drCurrent("ERROR_PRESENTACION_SII") = ""

                                        Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_ENVIO_SII_FORMATTED").ReadOnly = False
                                        drCurrent("FECHA_ENVIO_SII_FORMATTED") = sFechaAhoraFormatted
                                        Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_ENVIO_SII_FORMATTED").ReadOnly = True

                                        'Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_PRESENTACION_SII_FORMAT").ReadOnly = False
                                        'drCurrent("FECHA_PRESENTACION_SII_FORMAT") = sFechaAhoraFormatted
                                        'Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_PRESENTACION_SII_FORMAT").ReadOnly = True

                                        'Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_ERROR_SII_FORMAT").ReadOnly = False
                                        'drCurrent("FECHA_ERROR_SII_FORMAT") = DBNull.Value
                                        'Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_ERROR_SII_FORMAT").ReadOnly = True

                                        drCurrent.EndEdit()
                                    Else
                                        ' CON ERRORES
                                        If oRespuestaLinea.EstadoRegistro = SII_GobCan_WS.EstadoRegistroType.AceptadoConErrores Then
                                            ' ACEPTADO CON ERRORES
                                            sRespuestaAceptadoError = oRespuestaLinea.CodigoErrorRegistro & ": " & oRespuestaLinea.DescripcionErrorRegistro
                                            nAceptadoErroresCount += 1
                                            If sRespuestaAceptadoErrores.Length > 0 Then
                                                sRespuestaAceptadoErrores &= vbCrLf & vbCrLf
                                            End If
                                            sRespuestaAceptadoErrores &= nAceptadoErroresCount & ") " & sRespuestaFactura & ": " & sRespuestaAceptadoError

                                            If bRespuestaFacturaFound Then
                                                drCurrent.BeginEdit()
                                                drCurrent("FECHA_ENVIO_SII") = sFechaAhora
                                                'drCurrent("FECHA_PRESENTACION_SII") = sFechaAhora
                                                drCurrent("FECHA_ERROR_PRESENTACION_SII") = sFechaAhora
                                                drCurrent("ERROR_PRESENTACION_SII") = sRespuestaAceptadoError

                                                Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_ENVIO_SII_FORMATTED").ReadOnly = False
                                                drCurrent("FECHA_ENVIO_SII_FORMATTED") = sFechaAhoraFormatted
                                                Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_ENVIO_SII_FORMATTED").ReadOnly = True

                                                'Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_PRESENTACION_SII_FORMAT").ReadOnly = False
                                                'drCurrent("FECHA_PRESENTACION_SII_FORMAT") = sFechaAhoraFormatted
                                                'Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_PRESENTACION_SII_FORMAT").ReadOnly = True

                                                'Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_ERROR_SII_FORMAT").ReadOnly = False
                                                'drCurrent("FECHA_ERROR_SII_FORMAT") = DBNull.Value
                                                'Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_ERROR_SII_FORMAT").ReadOnly = True

                                                drCurrent.EndEdit()
                                            End If

                                        Else
                                            ' NO ACEPTADO / INCORRECTO
                                            sRespuestaError = oRespuestaLinea.CodigoErrorRegistro & ": " & oRespuestaLinea.DescripcionErrorRegistro
                                            nErroresCount += 1
                                            If sRespuestaErrores.Length > 0 Then
                                                sRespuestaErrores &= vbCrLf & vbCrLf
                                            End If
                                            sRespuestaErrores &= nErroresCount & ") " & sRespuestaFactura & ": " & sRespuestaError
                                            If bRespuestaFacturaFound Then
                                                drCurrent.BeginEdit()
                                                drCurrent("FECHA_ENVIO_SII") = DBNull.Value
                                                ' Una vez que se haya presentado, aunque de error, la fecha de presentación no se cambia
                                                'drCurrent("FECHA_PRESENTACION_SII") = ""
                                                drCurrent("FECHA_ERROR_PRESENTACION_SII") = sFechaAhora
                                                drCurrent("ERROR_PRESENTACION_SII") = sRespuestaError

                                                Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_ENVIO_SII_FORMATTED").ReadOnly = False
                                                drCurrent("FECHA_ENVIO_SII_FORMATTED") = ""
                                                Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_ENVIO_SII_FORMATTED").ReadOnly = True

                                                'Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_PRESENTACION_SII_FORMAT").ReadOnly = False
                                                'drCurrent("FECHA_PRESENTACION_SII_FORMAT") = ""
                                                'Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_PRESENTACION_SII_FORMAT").ReadOnly = True

                                                Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_ERROR_SII_FORMAT").ReadOnly = False
                                                drCurrent("FECHA_ERROR_SII_FORMAT") = sFechaAhoraFormatted
                                                Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_ERROR_SII_FORMAT").ReadOnly = True

                                                drCurrent.EndEdit()
                                            End If
                                        End If
                                    End If
                                Catch exc As Exception
                                    MsgBox("ERROR: " & exc.Message)
                                End Try
                            Next

                            If sRespuestaErrores.Length > 0 Then
                                sRespuestaErrores = vbCrLf & vbCrLf & "Se han encontrado los siguientes errores (para más información, consulta el archivo XML de la respuesta):" & vbCrLf & vbCrLf & sRespuestaErrores
                            Else
                                If sRespuestaAceptadoErrores.Length > 0 Then
                                    sRespuestaInfo = vbCrLf & vbCrLf & "NO se han encontrado errores que impidieran la presentación, sin embargo se encontraron las siguientes deficiencias:" & vbCrLf & vbCrLf & sRespuestaAceptadoErrores
                                Else
                                    sRespuestaInfo = vbCrLf & vbCrLf & "NO se han encontrado errores."
                                End If
                            End If

                            Try
                                Me.Validate()
                                Me.PRESENTACIONFACTURACIONBindingSource.EndEdit()
                                Me.PRESENTACIONFACTURACIONTableAdapter.Update(Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION)
                                sRespuestaInfo = vbCrLf & vbCrLf & "Se han actualizado los registros de la base de datos conforme a la respuesta del servicio." & vbCrLf & vbCrLf & sRespuestaInfo
                                Me.dgvPRESENTACIONFACTURACION.Refresh()
                            Catch exc As Exception
                                sRespuestaErrores = vbCrLf & vbCrLf & "ERROR al actualizar los registros de la base de datos: " & exc.Message & vbCrLf & vbCrLf & sRespuestaErrores
                            End Try
                            If sRespuestaErrores.Length > 0 Then
                                MessageBox.Show("Se ha procesado la presentación telemática y se ha generado el archivo XML de respuesta en " & sRutaYNombreArchivo & sRespuestaErrores, "Respuesta del servicio", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Else
                                MessageBox.Show("Se ha procesado la presentación telemática y se ha generado el archivo XML de respuesta en " & sRutaYNombreArchivo & sRespuestaInfo, "Respuesta del servicio", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If

                        Else
                            MessageBox.Show("No se ha cargado el certificado seleccionado ", "ERROR DE CERTIFICADO", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                        End If
                    Catch ex As Exception
                        MessageBox.Show("ERROR: " & ex.Message)
                    End Try

                    Try
                        oSIIServicioCliente.Dispose()
                    Catch ex As Exception

                    End Try
                    Try
                        oSIIServicioCliente.Abort()
                    Catch ex As Exception

                    End Try
                End If

                MessageBox.Show("Proceso terminado", "SII", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If
        Else
            sMessageText = "No hay registros para realizar la acción. Por favor cambia los filtros para que se muestren los registros que quieras cambiar."
            MessageBox.Show(sMessageText, "Generar XML", MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End If
    End Sub
    ' GENERAR Y PRESENTAR FACE
    ''' <summary>
    ''' Evento del botón para iniciar el proceso de presentación en el servicio FACE
    ''' </summary>
    ''' <param name="sender">Iniciador del evento</param>
    ''' <param name="e">Evento</param>
    Private Sub PresentarFACEButton_Click(sender As Object, e As EventArgs) Handles PresentarFACEButton.Click
        Dim sSelect As String
        Dim sNombreArchivo As String = ""
        Dim sRutaYNombreArchivo As String = ""
        Dim oArchivosFacturas(1, 2) As String
        Dim sZonas As String = ""
        Dim sPeriodos As String = ""
        Dim sFrom As String
        Dim sWhere As String
        Dim sSQL As String
        Dim sZona As String = ""
        Dim sNIF_PRESENTADOR As String = ""
        Dim sNOMBRE_PRESENTADOR As String = ""
        Dim sFACEemail As String = ""
        Dim sMessageText As String = ""
        Dim nCountIndex As Long = 0
        Dim nCountFacturas As Long = 0
        Dim bPRUEBA As Boolean = False
        Dim bXMLporFactura As Boolean = True


        ' NOMBRE DEL ARCHIVO XML
        sNombreArchivo = sNombreArchivo & "SII"
        sNombreArchivo = sNombreArchivo & "_SERIES_" & Me.SeriesFiltroComboBox.SelectedValue.Trim()

        If IsDBNull(Me.ZonasFiltroComboBox.SelectedValue) Then
            sZonas = "Todas"
        Else
            sZonas = Me.ZonasFiltroComboBox.SelectedValue.trim()
            Dim rowView As DataRowView = TryCast(Me.ZonasFiltroComboBox.SelectedItem, DataRowView)
            If (Not rowView Is Nothing) Then
                Dim row As DataRow = rowView.Row
                sZonas = DirectCast(row.Item("CODIGOZONA"), String).Trim().Replace(".", "_")
            End If
        End If
        sNombreArchivo = sNombreArchivo & "_ZONAS_" & sZonas

        sPeriodos = TryCast(Me.PeriodosFiltroComboBox.SelectedValue.Replace(" ", "_").Replace("º", "").Replace("ª", "").Trim(), String)
        For Each c In Path.GetInvalidFileNameChars()
            If sPeriodos.Contains(c) Then
                sPeriodos = sPeriodos.Replace(c, "")
            End If
        Next
        sNombreArchivo = sNombreArchivo & "_PERIODOS_" & sPeriodos
        sNombreArchivo = sNombreArchivo & "_" & Format(Now(), "ddMMyyyy") & "_" & Format(Now(), "Hmmss")
        sNombreArchivo = sNombreArchivo & ".xml"
        sRutaYNombreArchivo = Path.Combine(frmMain.sApplicationPath, sNombreArchivo)

        bPRUEBA = Me.DEBUGCheckBox.Checked

        bXMLporFactura = True

        nCountFacturas = Me.dgvPRESENTACIONFACTURACION.Rows.Count

        If nCountFacturas > 0 Then
            sMessageText = "Te dispones a iniciar el proceso de presentación de facturas electrónicas mediante el servicio FACE. Si continúas se generará un fichero XSIG con la factura mostrada actualmente en el listado y posteriormente podrás presentar dicha factura telemáticamente. ¿Estás seguro de querer continuar?"
            If bXMLporFactura Then
                If Me.dgvPRESENTACIONFACTURACION.Rows.Count > 1 Then
                    sMessageText = "Te dispones a iniciar el proceso de presentación de facturas electrónicas mediante el servicio FACE. Si continúas se generará un fichero XSIG para cada una de las " & String.Format("{0:#,##0}", Me.dgvPRESENTACIONFACTURACION.Rows.Count) & " facturas mostradas actualmente en el listado y posteriormente podrás presentar dichas facturas telemáticamente. ¿Estás seguro de querer continuar?"
                End If
            Else
                If Me.dgvPRESENTACIONFACTURACION.Rows.Count > 1 Then
                    sMessageText = "Te dispones a iniciar el proceso de presentación de facturas electrónicas mediante el servicio FACE. Si continúas se generará un fichero XSIG con las " & String.Format("{0:#,##0}", Me.dgvPRESENTACIONFACTURACION.Rows.Count) & " facturas mostradas actualmente en el listado y posteriormente podrás presentar dichas facturas telemáticamente. ¿Estás seguro de querer continuar?"
                End If
            End If

            If MessageBox.Show(sMessageText, "SERVICIO FACE", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = vbOK Then

                If nCountFacturas > 5 Then
                    If MessageBox.Show("Has seleccionado más de 5 facturas, cuantas más facturas proceses de golpe, más tardará el programa y más posibilidades existen de que la conexión con FACe de problemas. ¿Realmente queires continuar?", "SERVICIO FACE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = vbCancel Then
                        Exit Sub
                    End If
                End If

                sSelect = "SELECT PRESENTACIONFACTURACION.*, TRIM(CONCAT(REGANTES.NOMBRE,CONCAT(' ',REGANTES.APELLIDOS))) AS NOMBREYAPELLIDOS,ZONAS.DESCRIPCION AS ZONA, CONCAT(CONCAT(CONCAT(SUBSTR(PRESENTACIONFACTURACION.FECHA, - 2), '-'), CONCAT(SUBSTR(PRESENTACIONFACTURACION.FECHA, 5, 2), '-')), SUBSTR(PRESENTACIONFACTURACION.FECHA, 1, 4)) AS FECHA_FORMATTED, DECODE(NVL(PRESENTACIONFACTURACION.FECHA_ENVIO_SII, ''), '', '', CONCAT(CONCAT(CONCAT(SUBSTR(PRESENTACIONFACTURACION.FECHA_ENVIO_SII, -2), '-'), CONCAT(SUBSTR(PRESENTACIONFACTURACION.FECHA_ENVIO_SII, 5, 2), '-')), SUBSTR(PRESENTACIONFACTURACION.FECHA_ENVIO_SII, 1, 4))) As FECHA_ENVIO_SII_FORMATTED"
                sSelect &= ", REGANTES.DOMICILIO, REGANTES.CODIGO_POSTAL, MUNICIPIOS.NOMBMUNI AS MUNICIPIO, REGANTES.PROVINCIA, REGANTES.ORGANO_GESTOR_FACE, REGANTES.UDAD_TRAMITADORA_FACE, REGANTES.OFICINA_CONTABLE_FACE"
                sFrom = " FROM (PRESENTACIONFACTURACION LEFT OUTER JOIN ZONAS On ZONAS.CODIGOZONA = PRESENTACIONFACTURACION.CODIGOZONA) INNER JOIN REGANTES ON REGANTES.NIF_CIF = PRESENTACIONFACTURACION.NIF_CIF"
                sFrom &= " LEFT JOIN MUNICIPIOS ON MUNICIPIOS.CODMUNI=REGANTES.MUNICIPIO"
                sWhere = " WHERE " & myDataGridViewBindingSource.Filter.Replace("[", "PRESENTACIONFACTURACION.").Replace("]", "").Replace("LEN(", "LENGTH(")
                sWhere = sWhere.Replace("PRESENTACIONFACTURACION.ORGANO_GESTOR_FACE", "REGANTES.ORGANO_GESTOR_FACE")
                sSQL = sSelect & sFrom & sWhere
                Dim dtFacturas As DataTable = Nothing
                Try

                Catch ex As Exception

                End Try
                dtFacturas = frmMain.DameRegistros(sSQL)


                Dim nTotalXMLFiles As Integer = 0
                If bXMLporFactura Then
                    ' Crear bucle para crear un único archivo XML para cada una de las facturas del listado
                    nTotalXMLFiles = nCountFacturas
                    ReDim oArchivosFacturas(nCountFacturas, 2)
                Else
                    ' Crear un único buble para crear un único fichero XML para todas las facturas
                    nTotalXMLFiles = 1
                End If


                Dim nXMLFileIndex As Integer = 0
                For Each thisRow As DataRow In dtFacturas.Rows
                    If bXMLporFactura Then
                        sNombreArchivo = thisRow("SERIE") & thisRow("ANOFACTURA") & thisRow("NFACTURA") & ".xml"
                        sRutaYNombreArchivo = Path.Combine(frmMain.sApplicationPath, sNombreArchivo)
                    End If
                    If Not bXMLporFactura And nXMLFileIndex > 0 Then
                        Exit For
                    End If

                    sZona = thisRow("CODIGOZONA") 'thisrow.Cells(3).Value.ToString
                    If sZona = "Sin zona" Then
                        sZona = "0"
                    End If

                    sNIF_PRESENTADOR = frmMain.DameRegistro("SELECT NIF_PRESENTADOR FROM PRESENTADOR WHERE CODIGOZONA = '" & sZona & "'", "NIF_PRESENTADOR")
                    sNOMBRE_PRESENTADOR = frmMain.DameRegistro("SELECT PRESENTADOR FROM PRESENTADOR WHERE CODIGOZONA = '" & sZona & "'", "PRESENTADOR")
                    sSQL = "SELECT * FROM PRESENTADOR WHERE CODIGOZONA = '" & sZona & "'"
                    If sNIF_PRESENTADOR = "" Then
                        sNIF_PRESENTADOR = frmMain.DameRegistro("SELECT NIF_PRESENTADOR FROM PRESENTADOR WHERE CODIGOZONA = '1'", "NIF_PRESENTADOR")
                        sNOMBRE_PRESENTADOR = frmMain.DameRegistro("SELECT PRESENTADOR FROM PRESENTADOR WHERE CODIGOZONA = '1'", "PRESENTADOR")
                        sSQL = "SELECT * FROM PRESENTADOR WHERE CODIGOZONA = '1'"
                    End If

                    Dim dtPresentador As DataTable
                    dtPresentador = frmMain.DameRegistros(sSQL)
                    Dim oPresentador As DataRow = Nothing
                    If dtPresentador.Rows.Count > 0 Then
                        oPresentador = dtPresentador.Rows(0)
                    End If

                    If IsDBNull(oPresentador("PROVINCIA")) Then
                        oPresentador("PROVINCIA") = "S/C de Tenerife"
                    ElseIf oPresentador("PROVINCIA").ToString.Length = 0 Then
                        oPresentador("PROVINCIA") = "S/C de Tenerife"
                    Else
                        oPresentador("PROVINCIA") = oPresentador("PROVINCIA").Replace("SANTA CRUZ DE TENERIFE", "S/C de Tenerife").Replace("Santa Cruz de Tenerife", "S/C de Tenerife").Replace("LAS PALMAS DE GRAN CANARIA", "Las Palmas de G.C.").Replace("Las Palmas de Gran Canaria", "Las Palmas de G.C.")
                    End If

                    If Not IsDBNull(oPresentador("EMAIL")) Then
                        sFACEemail = oPresentador("EMAIL")
                    End If


                    ' Generar XML de factura  /  Elemento ORIGEN de Fichero de Facturas Facturae
                    Dim oFacturaFACEService As New facturaeService.Facturae

                    ' FILEHEADER  /  Cabecera del fichero xml
                    ' SchemaVersion
                    Dim oFileHeaderType As facturaeService.SchemaVersionType
                    oFileHeaderType = facturaeService.SchemaVersionType.Item322  ' Item322: Version 3.2.2
                    ' Modality
                    Dim oFileHeaderModality As facturaeService.ModalityType
                    oFileHeaderModality = facturaeService.ModalityType.L   ' L: Lote  ;  I: Individual
                    If bXMLporFactura Or nCountFacturas = 1 Then
                        oFileHeaderModality = facturaeService.ModalityType.I   ' L: Lote  ;  I: Individual
                    End If
                    ' InvoiceIssuerType
                    Dim oFileHeaderInvoiceIssuerType As facturaeService.InvoiceIssuerTypeType
                    oFileHeaderInvoiceIssuerType = facturaeService.InvoiceIssuerTypeType.EM  ' EM: Emisor/Proveedor  ;  DE: Destinatario  ;   TE: Tercero
                    Dim oFileHeader As New facturaeService.FileHeaderType With {
                        .SchemaVersion = oFileHeaderType,  ' Código que indica versión utilizada. Puede tomar los valores 3.2, 3.2.1, 3.2.2
                        .Modality = oFileHeaderModality,  '   ' Modalidad. Individual o Lote. Si es "individual" (I) los importes de los campos del grupo Batch coincidirán con sus correspondientes campos del grupo InvoiceTotals y el campo InvoicesCount tendrá siempre el valor "1". Si es "lote" (L), el valor del campo InvoicesCount será siempre > "1"
                        .InvoiceIssuerType = oFileHeaderInvoiceIssuerType  ' Tipo Emisor Factura. Puede tomar 3 valores (EM, RE y TE), que se describen como “Proveedor (antes denominado emisor)”, “Destinatario (antes denominado cliente o receptor)” y “Tercero”, respectivamente. Si toma el valor "TE" el grupo ThirdParty será obligatorio cumplimentarlo en todos sus apartados
                    }
                    ' Si la factura la emite un Tercero
                    If oFileHeaderInvoiceIssuerType = facturaeService.InvoiceIssuerTypeType.TE Then
                        Dim oFileHeaderThirdParty As New facturaeService.ThirdPartyType
                        Dim oFileHeaderPersonTypeCode As facturaeService.PersonTypeCodeType
                        oFileHeaderPersonTypeCode = facturaeService.PersonTypeCodeType.F  ' F: Física  ;  J: Jurídica
                        Dim oFileheaderResidenceTypeCode As facturaeService.ResidenceTypeCodeType
                        oFileheaderResidenceTypeCode = facturaeService.ResidenceTypeCodeType.R  ' E: Extranjero  ;  R: Residente  ;  U: Unión Europea
                        Dim oFileHeaderTaxIdentificationtype As New facturaeService.TaxIdentificationType With {
                            .TaxIdentificationNumber = "",  ' CIF/NIF
                            .PersonTypeCode = oFileHeaderPersonTypeCode,  ' F: Física  ;  J: Jurídica
                            .ResidenceTypeCode = oFileheaderResidenceTypeCode  ' E: Extranjero  ;  R: Residente  ;  U: Unión Europea
                        }
                        oFileHeaderThirdParty.TaxIdentification = oFileHeaderTaxIdentificationtype

                        ' Datos comunes Persona jurídica / Persona física
                        ' Dirección en España
                        Dim oFileHeaderAddressInSpain As New facturaeService.AddressType With {
                            .Address = "",
                            .Town = "",
                            .PostCode = "",
                            .Province = "",
                            .CountryCode = ""
                        }
                        ' Dirección fuera de España
                        Dim oFileHeaderAddressOutOfSpain As New facturaeService.OverseasAddressType With {
                            .Address = "",
                            .PostCodeAndTown = "",
                            .Province = "",
                            .CountryCode = ""
                        }
                        ' Datos de contacto
                        Dim oFileHeaderContactDetails As New facturaeService.ContactDetailsType With {
                            .ContactPersons = "",  ' Contactos. Apellidos y Nombre/Razón Social
                            .CnoCnae = "",  ' CNO/CNAE. Código Asignado por el INE
                            .INETownCode = "",  ' Código de población asignado por el INE
                            .ElectronicMail = "",
                            .Telephone = "",  ' Teléfono. Número de teléfono completo con prefijos del país
                            .WebAddress = "",
                            .TeleFax = "",  ' Fax. Número de fax completo con prefijos del país
                            .AdditionalContactDetails = ""
                        }

                        If oFileHeaderTaxIdentificationtype.PersonTypeCode = facturaeService.PersonTypeCodeType.F Then
                            ' Persona física
                            Dim oFileHeaderIndividual As New facturaeService.IndividualType With {
                                .Name = "",
                                .FirstSurname = "",
                                .SecondSurname = "",
                                .ContactDetails = oFileHeaderContactDetails
                            }
                            oFileHeaderIndividual.Item = oFileHeaderAddressInSpain
                            'oFileHeaderIndividual.Item = oFileHeaderAddressOutOfSpain
                            oFileHeaderThirdParty.Item = oFileHeaderIndividual  ' Persona física
                        Else
                            ' Persona jurídica y otras
                            Dim oFileHeaderRegistrationDataType As New facturaeService.RegistrationDataType With {
                                .Book = "",  ' Libro
                                .Folio = "",  ' Folio
                                .Sheet = "",  ' Hoja
                                .Section = "",  ' Sección
                                .Volume = "",  ' Tomo
                                .RegisterOfCompaniesLocation = "",  ' Registro Mercantil
                                .AdditionalRegistrationData = ""  ' Otros datos registrales
                            }
                            Dim oFileHeaderLegalEntity As New facturaeService.LegalEntityType With {
                                .CorporateName = "",  ' Razón Social
                                .TradeName = "",  ' Nombre Comercial
                                .RegistrationData = oFileHeaderRegistrationDataType,
                                .ContactDetails = oFileHeaderContactDetails
                            }
                            oFileHeaderLegalEntity.Item = oFileHeaderAddressInSpain
                            'oFileHeaderLegalEntity.Item = oFileHeaderAddressOutOfSpain
                            oFileHeaderThirdParty.Item = oFileHeaderLegalEntity  ' Persona jurídica y otras
                        End If

                        oFileHeader.ThirdParty = oFileHeaderThirdParty
                    End If

                    ' BATCH
                    Dim nInvoicesCount As Integer
                    If bXMLporFactura Then
                        nInvoicesCount = 1
                    Else
                        nInvoicesCount = nCountFacturas
                    End If
                    Dim oCurrencyCode As facturaeService.CurrencyCodeType
                    oCurrencyCode = facturaeService.CurrencyCodeType.EUR
                    Dim oTotalInvoicesAmount As New facturaeService.AmountType With {
                        .TotalAmount = CDbl(thisRow("TOTAL")),  'Importe en la moneda original de la facturación (Total facturas. Suma de los importes InvoiceTotal del Fichero). 2 a 8 decimales
                        .EquivalentInEurosSpecified = vbFalse,
                        .EquivalentInEuros = 0.0  ' Importe equivalente en Euros. Siempre con dos decimales
                    }
                    Dim oTotalOutstandingAmount As New facturaeService.AmountType With {
                        .TotalAmount = CDbl(thisRow("TOTAL")),  'Importe en la moneda original de la facturación (Total a pagar. Suma de los importes TotalOutstandingAmount del Fichero. Es el importe que efectivamente se adeuda, una vez descontados los anticipos y sin tener en cuenta las retenciones)
                        .EquivalentInEurosSpecified = vbFalse,
                        .EquivalentInEuros = 0.0  ' Importe equivalente en Euros. Siempre con dos decimales
                    }
                    Dim oTotalExecutableAmount As New facturaeService.AmountType With {
                        .TotalAmount = CDbl(thisRow("TOTAL")),  'Importe en la moneda original de la facturación (Total a Ejecutar. Sumatorio de los Importes TotalExecutableAmount del fichero)
                        .EquivalentInEurosSpecified = vbFalse,
                        .EquivalentInEuros = 0.0  ' Importe equivalente en Euros. Siempre con dos decimales
                    }
                    If Not oCurrencyCode = facturaeService.CurrencyCodeType.EUR Then

                    End If
                    Dim oFileHeaderBatch As New facturaeService.BatchType With {
                        .InvoicesCount = nInvoicesCount,  ' Número total de facturas
                        .InvoiceCurrencyCode = oCurrencyCode,  ' Código ISO 4217:2001 Alpha-3 de la moneda en la que se emite la factura
                        .BatchIdentifier = thisRow("NIF_CIF") & thisRow("NFACTURA") & thisRow("SERIE") & thisRow("ANOFACTURA"), '"Q8855008B793AG2020",  ' Identificador del lote. Concatenación del nº de documento del emisor con el número de la primera factura y el número de SERIE caso de existir
                        .TotalInvoicesAmount = oTotalInvoicesAmount,  ' Total facturas. Suma de los importes InvoiceTotal del Fichero
                        .TotalOutstandingAmount = oTotalOutstandingAmount,  ' Total a pagar. Suma de los importes TotalOutstandingAmount del Fichero. Es el importe que efectivamente se adeuda, una vez descontados los anticipos y sin tener en cuenta las retenciones
                        .TotalExecutableAmount = oTotalExecutableAmount  ' Total a Ejecutar. Sumatorio de los Importes TotalExecutableAmount del fichero
                    }
                    oFileHeader.Batch = oFileHeaderBatch

                    Dim bUseCesionario As Boolean = False
                    If bUseCesionario Then
                        ' FACTORINGASSIGNMENTDATA / CESIONARIO
                        Dim oFileHeaderAssigneePersonTypeCode As facturaeService.PersonTypeCodeType
                        oFileHeaderAssigneePersonTypeCode = facturaeService.PersonTypeCodeType.J  ' F: Física  ;  J: Jurídica
                        Dim oFileheaderAssigneeResidenceTypeCode As facturaeService.ResidenceTypeCodeType
                        oFileheaderAssigneeResidenceTypeCode = facturaeService.ResidenceTypeCodeType.R  ' E: Extranjero  ;  R: Residente  ;  U: Unión Europea
                        Dim oFileHeaderTaxIdentification As New facturaeService.TaxIdentificationType With {
                            .TaxIdentificationNumber = "",  ' Código de Identificación Fiscal del sujeto. Se trata de las composiciones de NIF/CIF que marca la Administración correspondiente
                            .PersonTypeCode = oFileHeaderAssigneePersonTypeCode,  ' Tipo de persona: F: Física  ;  J: Jurídica
                            .ResidenceTypeCode = oFileheaderAssigneeResidenceTypeCode  ' Identificación del tipo de residencia:  E: Extranjero  ;  R: Residente  ;  U: Unión Europea
                        }
                        ' Datos comunes Persona jurídica / Persona física
                        ' Dirección en España
                        Dim oFileHeaderAssigneeAddressInSpain As New facturaeService.AddressType With {
                            .Address = "",
                            .Town = "",
                            .PostCode = "",
                            .Province = "",
                            .CountryCode = ""
                        }
                        ' Dirección fuera de España
                        Dim oFileHeaderAssigneeAddressOutOfSpain As New facturaeService.OverseasAddressType With {
                            .Address = "",
                            .PostCodeAndTown = "",
                            .Province = "",
                            .CountryCode = ""
                        }
                        ' Datos de contacto
                        Dim oFileHeaderAssigneeContactDetails As New facturaeService.ContactDetailsType With {
                            .ContactPersons = "",  ' Contactos. Apellidos y Nombre/Razón Social
                            .CnoCnae = "",  ' CNO/CNAE. Código Asignado por el INE
                            .INETownCode = "",  ' Código de población asignado por el INE
                            .ElectronicMail = "",
                            .Telephone = "",  ' Teléfono. Número de teléfono completo con prefijos del país
                            .WebAddress = "",
                            .TeleFax = "",  ' Fax. Número de fax completo con prefijos del país
                            .AdditionalContactDetails = ""
                        }

                        ' Persona jurídica y otras
                        Dim oFileHeaderAssigneeRegistrationDataType As New facturaeService.RegistrationDataType With {
                            .Book = "",  ' Libro
                            .Folio = "",  ' Folio
                            .Sheet = "",  ' Hoja
                            .Section = "",  ' Sección
                            .Volume = "",  ' Tomo
                            .RegisterOfCompaniesLocation = "",  ' Registro Mercantil
                            .AdditionalRegistrationData = ""  ' Otros datos registrales
                        }
                        Dim oFileHeaderAssigneeLegalEntity As New facturaeService.LegalEntityType With {
                            .CorporateName = "",  ' Razón Social
                            .TradeName = "",  ' Nombre Comercial
                            .RegistrationData = oFileHeaderAssigneeRegistrationDataType,
                            .ContactDetails = oFileHeaderAssigneeContactDetails
                        }
                        oFileHeaderAssigneeLegalEntity.Item = oFileHeaderAssigneeAddressInSpain
                        'oFileHeaderLegalEntity.Item = oFileHeaderAddressOutOfSpain

                        ' Persona física
                        Dim oFileHeaderAssigneeIndividual As New facturaeService.IndividualType With {
                            .Name = "",
                            .FirstSurname = "",
                            .SecondSurname = "",
                            .ContactDetails = oFileHeaderAssigneeContactDetails
                        }
                        oFileHeaderAssigneeIndividual.Item = oFileHeaderAssigneeAddressInSpain
                        'oFileHeaderAssigneeIndividual.Item = oFileHeaderAssigneeAddressOutOfSpain
                        Dim oFileHeaderAssignee As New facturaeService.AssigneeType With {
                            .TaxIdentification = oFileHeaderTaxIdentification,
                            .Item = oFileHeaderAssigneeLegalEntity ' oFileHeaderAssigneeIndividual
                        }

                        Dim oFileHeaderPaymentMeans As facturaeService.PaymentMeansType
                        oFileHeaderPaymentMeans = facturaeService.PaymentMeansType.Item01

                        Dim oFileHeaderInstallment(1) As facturaeService.InstallmentType
                        oFileHeaderInstallment(0) = New facturaeService.InstallmentType With {  ' Vencimiento
                            .InstallmentDueDate = "",  ' Fechas en las que se deben atender los pagos
                            .InstallmentAmount = "",  ' Importe a satisfacer en cada plazo. Siempre con dos decimales
                            .PaymentMeans = oFileHeaderPaymentMeans,  ' Cada vencimiento/importe podrá tener un medio de pago concreto. p.ej. [01], [02], [03], [04].
                            .PaymentReconciliationReference = "",  ' Referencia expresa del pago. Dato que precisa el Emisor para conciliar los pagos con cada factura
                            .CollectionAdditionalInformation = "",  ' Observaciones de cobro. Libre para uso del Emisor
                            .RegulatoryReportingData = "",  ' Código Estadístico. Usado en las operaciones transfronterizas
                            .DebitReconciliationReference = ""  ' Referencia del cliente pagador, similar a la utilizada por el emisor para la conciliación de los pagos
                        }

                        If oFileHeaderPaymentMeans = facturaeService.PaymentMeansType.Item01 Or oFileHeaderPaymentMeans = facturaeService.PaymentMeansType.Item02 Then
                            Dim oFileHeaderAccount As New facturaeService.AccountType With {
                                .BankCode = "",  ' Código de la entidad financiera
                                .BranchCode = "",  ' Código de la oficina de la entidad financiera
                                .BIC = ""  ' Código SWIFT. Será obligatorio rellenar las 11 posiciones, utilizando los caracteres XXX cuando no se informe de la sucursal
                            }
                            If oFileHeaderAccount.ItemElementName = "IBAN" Then
                                oFileHeaderAccount.Item = ""  ' IBAN
                            ElseIf oFileHeaderAccount.ItemElementName = "AccountNumber" Then
                                oFileHeaderAccount.Item = ""  ' Número de cuenta
                            End If
                            ' Dirección de la sucursal/oficina en España  /  Dirección en España
                            Dim oFileHeaderPaymentAddressInSpain As New facturaeService.AddressType With {
                                .Address = "",  ' Dirección. Tipo de vía, nombre, número, piso…
                                .Town = "",  ' Población. Correspondiente al C.P.
                                .PostCode = "",  ' Código Postal asignado por Correos
                                .Province = "",  ' Provincia. Donde está situada la Población
                                .CountryCode = ""  ' Código País. Al ser un domicilio ubicado en España siempre será "ESP"
                            }
                            ' Dirección de la sucursal/oficina en el extranjero  /  Dirección fuera de España
                            Dim oFileHeaderPaymentAddressOutOfSpain As New facturaeService.OverseasAddressType With {
                                .Address = "",  ' Dirección. Tipo de vía, nombre, número, piso…
                                .PostCodeAndTown = "",  ' Población y Código Postal en el extranjero
                                .Province = "",  ' Provincia, Estado, etc.
                                .CountryCode = ""  ' Código País. Código según la ISO 3166-1:2006 Alpha-3. p.ej.[AFG], [ALB], [DZA], [ASM]
                            }
                            oFileHeaderAccount.Item1 = oFileHeaderPaymentAddressInSpain  ' OR oFileHeaderAccount.Item1 = oFileHeaderPaymentAddressOutOfSpain
                            If oFileHeaderPaymentMeans = facturaeService.PaymentMeansType.Item01 Then
                                ' Transferencia
                                oFileHeaderInstallment(0).AccountToBeCredited = oFileHeaderAccount  ' Cuenta de abono. Único formato admitido. Cuando la forma de pago (PaymentMeans) sea "transferencia" este dato será obligatorio
                            ElseIf oFileHeaderPaymentMeans = facturaeService.PaymentMeansType.Item02 Then
                                ' Recibo domiciliado
                                oFileHeaderInstallment(0).AccountToBeDebited = oFileHeaderAccount  ' Cuenta de cargo. Único formato admitido. Cuando la forma de pago (PaymentMeans) sea "recibo domiciliado" este dato será obligatorio
                            End If
                        End If
                        Dim oFileHeaderRepositoryType As New facturaeService.RepositoryType With {
                            .RepositoryName = "",  ' Archivo electrónico en el que estuviera anotado: [CGN]1, [ROLECE]2, [REA]3, [otros]
                            .Reference = ""  ' Referencia electrónica o código de verificación en el archivo electrónico
                        }
                        If oFileHeaderRepositoryType.RepositoryName = "otros" Then
                            oFileHeaderRepositoryType.URL = ""  ' URL del archivo electrónico no definido. Es obligatorio en el caso de que RepositoryName tenga el valor “otros”
                        End If
                        ' REVISAR:   OJO!!!!  Múltiples entradas: ARRAY
                        Dim oFileHeaderFactoringAssignmentDocument(1) As facturaeService.FactoringAssignmentDocumentType
                        oFileHeaderFactoringAssignmentDocument(0) = New facturaeService.FactoringAssignmentDocumentType With {  ' Datos para identificar la referencia electrónica de los documentos de cesión
                            .DocumentCharacter = "",  ' Naturaleza del documento. Puede tomar los valores [acuerdo de cesión], [poder acreditativo de representación],[otros]
                            .DocumentType = "",  ' Tipo de documento. Puede tomar los valores: [escritura pública], [documento privado]
                            .Repository = oFileHeaderRepositoryType  ' Datos del archivo electrónico utilizado
                        }
                        If oFileHeaderFactoringAssignmentDocument(0).DocumentCharacter = "poder acreditativo de representación" Then
                            oFileHeaderFactoringAssignmentDocument(0).RepresentationIdentity = ""  ' Obligatorio en caso de que DocumentCharacter tenga el valor “poder acreditativo de representación”. Puede tomar los valores [del cedente en el acuerdo de cesión], [del cesionario en el acuerdo de cesión], [de quien efectúa la notificación en nombre de cedente o cesionario]
                        End If
                        Dim oFileHeaderFactoringData As New facturaeService.FactoringAssignmentDataType With {
                            .Assignee = oFileHeaderAssignee,
                            .PaymentDetails = oFileHeaderInstallment,  ' Datos de pago
                            .FactoringAssignmentClauses = "",  ' Texto de la cláusula de cesión
                            .FactoringAssignmentDocument = oFileHeaderFactoringAssignmentDocument
                        }
                        oFileHeader.FactoringAssignmentData = oFileHeaderFactoringData
                    End If

                    oFacturaFACEService.FileHeader = oFileHeader


                    'PARTIES
                    ' SELLER
                    Dim oPartiesSellerPersonType As facturaeService.PersonTypeCodeType
                    oPartiesSellerPersonType = facturaeService.PersonTypeCodeType.J
                    Dim oPartiesSellerResidence As facturaeService.ResidenceTypeCodeType
                    oPartiesSellerResidence = facturaeService.ResidenceTypeCodeType.R
                    Dim oPartiesSellerTaxIdentification As New facturaeService.TaxIdentificationType With {
                        .PersonTypeCode = oPartiesSellerPersonType,  ' Tipo de persona. Física o Jurídica. "F" - Física; "J" - Jurídica
                        .ResidenceTypeCode = oPartiesSellerResidence,  ' Identificación del tipo de residencia y/o extranjería. "E" - Extranjero; "R" - Residente; "U" - Residente en la Unión Europea.
                        .TaxIdentificationNumber = oPresentador("NIF_PRESENTADOR") '  Código de Identificación Fiscal del sujeto. Se trata de las composiciones de NIF/CIF que marca la Administración correspondiente(precedidas de las dos letras del país en el caso de operaciones intracomunitarias, es decir, cuando comprador y vendedor tienen domicilio fiscal en estados miembros de la UE distintos)                      
                    }
                    Dim oPartiesSellerRoleTypeCode As facturaeService.RoleTypeCodeType
                    ' 01: Fiscal
                    ' 02: Receptor
                    ' 03: Pagador
                    ' 04: Comprador
                    ' 05: Cobrador
                    ' 06: Vendedor
                    ' 07: Receptor del pago
                    ' 08: Receptor del cobro
                    ' 09: Emisor
                    oPartiesSellerRoleTypeCode = facturaeService.RoleTypeCodeType.Item01
                    Dim oPartiesSellerAdmCentreAdress As New facturaeService.AddressType With {
                        .Address = Nothing,  ' Dirección. Tipo de vía, nombre, número, piso
                        .PostCode = Nothing,  ' Código Postal asignado por Correos.
                        .Town = Nothing,  ' Población. Correspondiente al C.P.
                        .Province = Nothing,  ' Provincia. Donde está situada la Población
                        .CountryCode = facturaeService.CountryType.ESP  ' Código País. Código según la ISO 3166-1:2006 Alpha-3. Al ser un domicilio ubicado en España siempre será "ESP".
                    }
                    Dim oPartiesSellerAdmCentreContact As New facturaeService.ContactDetailsType With {
                        .Telephone = Nothing,  ' Teléfono. Número de teléfono completo con prefijos del país.
                        .TeleFax = Nothing,  ' Fax. Número de fax completo con prefijos del país
                        .WebAddress = Nothing,  ' Página web. URL de la dirección de Internet
                        .ElectronicMail = Nothing,  ' Correo electrónico. Dirección de correo electrónico
                        .ContactPersons = Nothing,  ' Contactos. Apellidos y Nombre/Razón Social
                        .CnoCnae = Nothing,  ' CNO/CNAE. Código Asignado por el INE
                        .INETownCode = Nothing,  ' Código de población asignado por el INE
                        .AdditionalContactDetails = Nothing  ' Otros datos de contacto.
                    }
                    Dim oPartiesSellerAdmCentres(1) As facturaeService.AdministrativeCentreType
                    oPartiesSellerAdmCentres(0) = New facturaeService.AdministrativeCentreType With {
                        .CentreCode = Nothing,  ' Número del Departamento Emisor
                        .RoleTypeCodeSpecified = vbFalse,  ' Tipo rol especificado?  true / false
                        .RoleTypeCode = oPartiesSellerRoleTypeCode,  ' Tipo rol. Indica la función de un Punto Operacional (P.O.) definido como Centro/Departamento. Estas funciones son: "Receptor" - Centro del NIF receptor destinatario de la factura. "Pagador" - Centro del NIF receptor responsable de pagar la factura. "Comprador" - Centro del NIF receptor que emitió el pedido. "Cobrador" - Centro del NIF emisor responsable de gestionar el cobro. "Fiscal" - Centro del NIF receptor de las facturas, cuando un P.O. buzón es compartido por varias empresas clientes con diferentes NIF.s y es necesario diferenciar el receptor del mensaje (buzón común) del lugar donde debe depositarse (empresa destinataria). Algunos valores posibles serian:  [01], [02], [03], [04].
                        .Name = Nothing,  ' Nombre de la persona responsable o de relación del centro
                        .FirstSurname = Nothing,  ' Primer apellido de la persona responsable o de relación del centro
                        .SecondSurname = Nothing,  ' Segundo apellido de la persona responsable o de relación del centro
                        .ContactDetails = oPartiesSellerAdmCentreContact,  ' Datos de contacto
                        .PhysicalGLN = Nothing,  ' GLN Físico. Identificación del punto de conexión a la VAN EDI(Global Location Number).Código de barras de 13 posiciones estándar.Valores registrados por AECOC
                        .LogicalOperationalPoint = Nothing,  ' Punto Lógico Operacional. Código identificativo de la Empresa.Código de barras de 13 posiciones estándar. Valores registrados por AECOC. Recoge el código de País (2p) España es "84" + Empresa (5p) + los restantes - el último es el producto + dígito de control
                        .CentreDescription = Nothing    ' Descripción del centro.                                  
                    }
                    oPartiesSellerAdmCentres(0).Item = Nothing 'oPartiesSellerAdmCentreAdress,  ' Dirección nacional. Dirección en España

                    Dim oPartiesSellerLegalEntityRegistration As New facturaeService.RegistrationDataType With {
                        .Book = Nothing,  ' Libro
                        .Folio = Nothing,  ' Folio
                        .Sheet = Nothing,  ' Hoja
                        .Section = Nothing,  ' Sección
                        .Volume = Nothing,  ' Tomo
                        .RegisterOfCompaniesLocation = Nothing,  ' Registro Mercantil
                        .AdditionalRegistrationData = Nothing  ' Otros datos registrales
                    }
                    Dim oPartiesSellerAdress As New facturaeService.AddressType With {
                        .Address = oPresentador("DOMICILIO"),  ' Dirección. Tipo de vía, nombre, número, piso
                        .PostCode = oPresentador("CP"),  ' Código Postal asignado por Correos.
                        .Town = oPresentador("MUNICIPIO"),  ' Población. Correspondiente al C.P.
                        .Province = oPresentador("PROVINCIA").Replace("Santa Cruz de Tenerife", "S/C de Tenerife").Replace("Las Palmas de Gran Canaria", "Las Palmas de G.C."),  ' Provincia. Donde está situada la Población
                        .CountryCode = facturaeService.CountryType.ESP  ' Código País. Código según la ISO 3166-1:2006 Alpha-3. Al ser un domicilio ubicado en España siempre será "ESP".
                    }
                    Dim oPartiesSellerLegalEntityContact As New facturaeService.ContactDetailsType With {
                        .Telephone = oPresentador("TLFNO"),  ' Teléfono. Número de teléfono completo con prefijos del país.
                        .TeleFax = "",  ' Fax. Número de fax completo con prefijos del país
                        .WebAddress = oPresentador("WEB"),  ' Página web. URL de la dirección de Internet
                        .ElectronicMail = oPresentador("EMAIL"),  ' Correo electrónico. Dirección de correo electrónico
                        .ContactPersons = oPresentador("PERS_CONTACTO"),  ' Contactos. Apellidos y Nombre/Razón Social
                        .CnoCnae = Nothing,  ' CNO/CNAE. Código Asignado por el INE
                        .INETownCode = Nothing,  ' Código de población asignado por el INE
                        .AdditionalContactDetails = ""  ' Otros datos de contacto.
                    }
                    Dim oPartiesSellerLegalEntity As New facturaeService.LegalEntityType With {
                        .CorporateName = oPresentador("PRESENTADOR"), '"Entidad Publica Empresarial Local Balsas de Tenerife",  ' Razón Social
                        .TradeName = oPresentador("PRESENTADOR"),  ' Nombre Comercial
                        .RegistrationData = Nothing, 'oPartiesSellerLegalEntityRegistration,  ' Datos Registrales: Inscripción Registro, Tomo, Folio,…
                        .Item = oPartiesSellerAdress,  ' Dirección Nacional. Dirección en España
                        .ContactDetails = oPartiesSellerLegalEntityContact  ' Datos de contacto                     
                    }
                    Dim oPartiesSellerIndividual As New facturaeService.IndividualType With {
                        .Name = Nothing,  ' Nombre de la persona física.
                        .FirstSurname = Nothing,  ' Primer apellido de la persona física
                        .SecondSurname = Nothing,  ' Segundo apellido de la persona física
                        .Item = oPartiesSellerAdress  ' Dirección nacional. Dirección en España
                    }
                    Dim oPartiesSeller As New facturaeService.BusinessType With {
                        .TaxIdentification = oPartiesSellerTaxIdentification,  ' Identificación fiscal
                        .PartyIdentification = Nothing,  ' Identificación de la entidad; Rellenar con el número de referencia de la entidad del programa de facturación que utilice
                        .AdministrativeCentres = Nothing, 'oPartiesSellerAdmCentres,  ' Centros
                        .Item = oPartiesSellerLegalEntity  ' Persona jurídica y otras
                    }
                    If oPartiesSellerPersonType = facturaeService.PersonTypeCodeType.F Then
                        oPartiesSeller.Item = oPartiesSellerIndividual  ' Persona física
                    Else
                        oPartiesSeller.Item = oPartiesSellerLegalEntity  ' Persona jurídica y otras
                    End If

                    ' BUYER
                    thisRow("NOMBREYAPELLIDOS") = thisRow("NOMBREYAPELLIDOS").Replace("D. ", "").Replace("Da. ", "").Replace("Dª. ", "")
                    If thisRow("NOMBREYAPELLIDOS").length > 40 Then
                        thisRow("NOMBREYAPELLIDOS") = thisRow("NOMBREYAPELLIDOS").Substring(0, 39)
                    End If

                    If IsDBNull(thisRow("CODIGO_POSTAL")) Then
                        thisRow("CODIGO_POSTAL") = "38001"
                    End If

                    If IsDBNull(thisRow("PROVINCIA")) Then
                        thisRow("PROVINCIA") = "S/C de Tenerife"
                    ElseIf thisrow("PROVINCIA").ToString.Length = 0 Then
                        thisRow("PROVINCIA") = "S/C de Tenerife"
                    End If
                    thisRow("PROVINCIA") = thisRow("PROVINCIA").Replace("Santa Cruz de Tenerife", "S/C de Tenerife").Replace("Las Palmas de Gran Canaria", "Las Palmas de G.C.")

                    Dim oPartiesBuyerPersonType As facturaeService.PersonTypeCodeType
                    oPartiesBuyerPersonType = facturaeService.PersonTypeCodeType.J
                    Dim oPartiesBuyerResidence As facturaeService.ResidenceTypeCodeType
                    oPartiesBuyerResidence = facturaeService.ResidenceTypeCodeType.R
                    Dim oPartiesBuyerTaxIdentification As New facturaeService.TaxIdentificationType With {
                        .PersonTypeCode = oPartiesBuyerPersonType,  ' Tipo de persona. Física o Jurídica. "F" - Física; "J" - Jurídica
                        .ResidenceTypeCode = oPartiesBuyerResidence,  ' Identificación del tipo de residencia y/o extranjería. "E" - Extranjero; "R" - Residente; "U" - Residente en la Unión Europea.
                        .TaxIdentificationNumber = thisRow("NIF_CIF") '"Q8855008B"  '  Código de Identificación Fiscal del sujeto. Se trata de las composiciones de NIF/CIF que marca la Administración correspondiente(precedidas de las dos letras del país en el caso de operaciones intracomunitarias, es decir, cuando comprador y vendedor tienen domicilio fiscal en estados miembros de la UE distintos)                      
                    }
                    Dim oPartiesBuyerRoleTypeCode As facturaeService.RoleTypeCodeType
                    ' 01: Fiscal
                    ' 02: Receptor
                    ' 03: Pagador
                    ' 04: Comprador
                    ' 05: Cobrador
                    ' 06: Vendedor
                    ' 07: Receptor del pago
                    ' 08: Receptor del cobro
                    ' 09: Emisor
                    oPartiesBuyerRoleTypeCode = facturaeService.RoleTypeCodeType.Item02
                    Dim oPartiesBuyerAdmCentreAdress As New facturaeService.AddressType With {
                        .Address = thisRow("DOMICILIO"),  ' Dirección. Tipo de vía, nombre, número, piso
                        .PostCode = thisRow("CODIGO_POSTAL"),  ' Código Postal asignado por Correos.
                        .Town = thisRow("MUNICIPIO"),  ' Población. Correspondiente al C.P.
                        .Province = thisRow("PROVINCIA"),  ' Provincia. Donde está situada la Población
                        .CountryCode = facturaeService.CountryType.ESP  ' Código País. Código según la ISO 3166-1:2006 Alpha-3. Al ser un domicilio ubicado en España siempre será "ESP".
                    }
                    Dim oPartiesBuyerAdmCentreContact As New facturaeService.ContactDetailsType With {
                        .Telephone = Nothing,  ' Teléfono. Número de teléfono completo con prefijos del país.
                        .TeleFax = Nothing,  ' Fax. Número de fax completo con prefijos del país
                        .WebAddress = Nothing,  ' Página web. URL de la dirección de Internet
                        .ElectronicMail = Nothing,  ' Correo electrónico. Dirección de correo electrónico
                        .ContactPersons = Nothing,  ' Contactos. Apellidos y Nombre/Razón Social
                        .CnoCnae = Nothing,  ' CNO/CNAE. Código Asignado por el INE
                        .INETownCode = Nothing,  ' Código de población asignado por el INE
                        .AdditionalContactDetails = Nothing  ' Otros datos de contacto.
                    }
                    Dim oPartiesBuyerAdmCentres(3) As facturaeService.AdministrativeCentreType
                    ' 01: Fiscal
                    ' 02: Receptor
                    ' 03: Pagador
                    ' 04: Comprador
                    ' 05: Cobrador
                    ' 06: Vendedor
                    ' 07: Receptor del pago
                    ' 08: Receptor del cobro
                    ' 09: Emisor
                    oPartiesBuyerRoleTypeCode = facturaeService.RoleTypeCodeType.Item02
                    oPartiesBuyerAdmCentres(0) = New facturaeService.AdministrativeCentreType With {
                        .CentreCode = thisRow("ORGANO_GESTOR_FACE"), ' Número del Departamento Emisor
                        .RoleTypeCodeSpecified = vbTrue,  ' Tipo rol especificado?  true / false
                        .RoleTypeCode = oPartiesBuyerRoleTypeCode,  ' Tipo rol. Indica la función de un Punto Operacional (P.O.) definido como Centro/Departamento. Estas funciones son: "Receptor" - Centro del NIF receptor destinatario de la factura. "Pagador" - Centro del NIF receptor responsable de pagar la factura. "Comprador" - Centro del NIF receptor que emitió el pedido. "Cobrador" - Centro del NIF emisor responsable de gestionar el cobro. "Fiscal" - Centro del NIF receptor de las facturas, cuando un P.O. buzón es compartido por varias empresas clientes con diferentes NIF.s y es necesario diferenciar el receptor del mensaje (buzón común) del lugar donde debe depositarse (empresa destinataria). Algunos valores posibles serian:  [01], [02], [03], [04].
                        .Name = thisRow("NOMBREYAPELLIDOS"), ' Nombre de la persona responsable o de relación del centro
                        .FirstSurname = Nothing,  ' Primer apellido de la persona responsable o de relación del centro
                        .SecondSurname = Nothing,  ' Segundo apellido de la persona responsable o de relación del centro
                        .Item = oPartiesBuyerAdmCentreAdress,  ' Dirección nacional. Dirección en España
                        .ContactDetails = oPartiesBuyerAdmCentreContact,  ' Datos de contacto
                        .PhysicalGLN = Nothing,  ' GLN Físico. Identificación del punto de conexión a la VAN EDI(Global Location Number).Código de barras de 13 posiciones estándar.Valores registrados por AECOC
                        .LogicalOperationalPoint = Nothing,  ' Punto Lógico Operacional. Código identificativo de la Empresa.Código de barras de 13 posiciones estándar. Valores registrados por AECOC. Recoge el código de País (2p) España es "84" + Empresa (5p) + los restantes - el último es el producto + dígito de control
                        .CentreDescription = thisRow("NOMBREYAPELLIDOS") ' Descripción del centro.
                    }
                    ' 01: Fiscal
                    ' 02: Receptor
                    ' 03: Pagador
                    ' 04: Comprador
                    ' 05: Cobrador
                    ' 06: Vendedor
                    ' 07: Receptor del pago
                    ' 08: Receptor del cobro
                    ' 09: Emisor 
                    oPartiesBuyerRoleTypeCode = facturaeService.RoleTypeCodeType.Item03
                    oPartiesBuyerAdmCentres(1) = New facturaeService.AdministrativeCentreType With {
                        .CentreCode = thisRow("UDAD_TRAMITADORA_FACE"),  ' Número del Departamento Emisor
                        .RoleTypeCodeSpecified = vbTrue,  ' Tipo rol especificado?  true / false
                        .RoleTypeCode = oPartiesBuyerRoleTypeCode,  ' Tipo rol. Indica la función de un Punto Operacional (P.O.) definido como Centro/Departamento. Estas funciones son: "Receptor" - Centro del NIF receptor destinatario de la factura. "Pagador" - Centro del NIF receptor responsable de pagar la factura. "Comprador" - Centro del NIF receptor que emitió el pedido. "Cobrador" - Centro del NIF emisor responsable de gestionar el cobro. "Fiscal" - Centro del NIF receptor de las facturas, cuando un P.O. buzón es compartido por varias empresas clientes con diferentes NIF.s y es necesario diferenciar el receptor del mensaje (buzón común) del lugar donde debe depositarse (empresa destinataria). Algunos valores posibles serian:  [01], [02], [03], [04].
                        .Name = thisRow("NOMBREYAPELLIDOS"), ' Nombre de la persona responsable o de relación del centro
                        .FirstSurname = Nothing,  ' Primer apellido de la persona responsable o de relación del centro
                        .SecondSurname = Nothing,  ' Segundo apellido de la persona responsable o de relación del centro
                        .Item = oPartiesBuyerAdmCentreAdress,  ' Dirección nacional. Dirección en España
                        .ContactDetails = oPartiesBuyerAdmCentreContact,  ' Datos de contacto
                        .PhysicalGLN = Nothing,  ' GLN Físico. Identificación del punto de conexión a la VAN EDI(Global Location Number).Código de barras de 13 posiciones estándar.Valores registrados por AECOC
                        .LogicalOperationalPoint = Nothing,  ' Punto Lógico Operacional. Código identificativo de la Empresa.Código de barras de 13 posiciones estándar. Valores registrados por AECOC. Recoge el código de País (2p) España es "84" + Empresa (5p) + los restantes - el último es el producto + dígito de control
                        .CentreDescription = thisRow("NOMBREYAPELLIDOS") ' Descripción del centro.
                    }
                    ' 01: Fiscal
                    ' 02: Receptor
                    ' 03: Pagador
                    ' 04: Comprador
                    ' 05: Cobrador
                    ' 06: Vendedor
                    ' 07: Receptor del pago
                    ' 08: Receptor del cobro
                    ' 09: Emisor 
                    oPartiesBuyerRoleTypeCode = facturaeService.RoleTypeCodeType.Item01
                    oPartiesBuyerAdmCentres(2) = New facturaeService.AdministrativeCentreType With {
                        .CentreCode = thisRow("OFICINA_CONTABLE_FACE"),  ' Número del Departamento Emisor
                        .RoleTypeCodeSpecified = vbTrue,  ' Tipo rol especificado?  true / false
                        .RoleTypeCode = oPartiesBuyerRoleTypeCode,  ' Tipo rol. Indica la función de un Punto Operacional (P.O.) definido como Centro/Departamento. Estas funciones son: "Receptor" - Centro del NIF receptor destinatario de la factura. "Pagador" - Centro del NIF receptor responsable de pagar la factura. "Comprador" - Centro del NIF receptor que emitió el pedido. "Cobrador" - Centro del NIF emisor responsable de gestionar el cobro. "Fiscal" - Centro del NIF receptor de las facturas, cuando un P.O. buzón es compartido por varias empresas clientes con diferentes NIF.s y es necesario diferenciar el receptor del mensaje (buzón común) del lugar donde debe depositarse (empresa destinataria). Algunos valores posibles serian:  [01], [02], [03], [04].
                        .Name = thisRow("NOMBREYAPELLIDOS"), ' Nombre de la persona responsable o de relación del centro
                        .FirstSurname = Nothing,  ' Primer apellido de la persona responsable o de relación del centro
                        .SecondSurname = Nothing,  ' Segundo apellido de la persona responsable o de relación del centro
                        .Item = oPartiesBuyerAdmCentreAdress,  ' Dirección nacional. Dirección en España
                        .ContactDetails = oPartiesBuyerAdmCentreContact,  ' Datos de contacto
                        .PhysicalGLN = Nothing,  ' GLN Físico. Identificación del punto de conexión a la VAN EDI(Global Location Number).Código de barras de 13 posiciones estándar.Valores registrados por AECOC
                        .LogicalOperationalPoint = Nothing,  ' Punto Lógico Operacional. Código identificativo de la Empresa.Código de barras de 13 posiciones estándar. Valores registrados por AECOC. Recoge el código de País (2p) España es "84" + Empresa (5p) + los restantes - el último es el producto + dígito de control
                        .CentreDescription = thisRow("NOMBREYAPELLIDOS") ' Descripción del centro.
                    }
                    Dim oPartiesBuyerLegalEntityRegistration As New facturaeService.RegistrationDataType With {
                        .Book = Nothing,  ' Libro
                        .Folio = Nothing,  ' Folio
                        .Sheet = Nothing,  ' Hoja
                        .Section = Nothing,  ' Sección
                        .Volume = Nothing,  ' Tomo
                        .RegisterOfCompaniesLocation = Nothing,  ' Registro Mercantil
                        .AdditionalRegistrationData = Nothing  ' Otros datos registrales
                    }
                    Dim oPartiesBuyerAdress As New facturaeService.AddressType With {
                        .Address = thisRow("DOMICILIO"), ' Dirección. Tipo de vía, nombre, número, piso
                        .PostCode = thisRow("CODIGO_POSTAL"), ' Código Postal asignado por Correos.
                        .Town = thisRow("MUNICIPIO"), ' Población. Correspondiente al C.P.
                        .Province = thisRow("PROVINCIA"), ' Provincia. Donde está situada la Población
                        .CountryCode = facturaeService.CountryType.ESP  ' Código País. Código según la ISO 3166-1:2006 Alpha-3. Al ser un domicilio ubicado en España siempre será "ESP".
                    }
                    Dim oPartiesBuyerLegalEntityContact As New facturaeService.ContactDetailsType With {
                        .Telephone = Nothing,  ' Teléfono. Número de teléfono completo con prefijos del país.
                        .TeleFax = Nothing,  ' Fax. Número de fax completo con prefijos del país
                        .WebAddress = Nothing,  ' Página web. URL de la dirección de Internet
                        .ElectronicMail = Nothing,  ' Correo electrónico. Dirección de correo electrónico
                        .ContactPersons = Nothing,  ' Contactos. Apellidos y Nombre/Razón Social
                        .CnoCnae = Nothing,  ' CNO/CNAE. Código Asignado por el INE
                        .INETownCode = Nothing,  ' Código de población asignado por el INE
                        .AdditionalContactDetails = Nothing  ' Otros datos de contacto.
                    }
                    Dim oPartiesBuyerLegalEntity As New facturaeService.LegalEntityType With {
                        .CorporateName = thisRow("NOMBREYAPELLIDOS"), ' Razón Social
                        .TradeName = thisRow("NOMBREYAPELLIDOS"), ' Nombre Comercial
                        .RegistrationData = Nothing, 'oPartiesBuyerLegalEntityRegistration,  ' Datos Registrales: Inscripción Registro, Tomo, Folio,…
                        .Item = oPartiesBuyerAdress,  ' Dirección Nacional. Dirección en España
                        .ContactDetails = Nothing 'oPartiesBuyerLegalEntityContact  ' Datos de contacto                     
                    }
                    If bDEBUG Then
                        oPartiesBuyerTaxIdentification.TaxIdentificationNumber = "S3511001D"

                        oPartiesBuyerAdmCentreAdress.Address = "Av. Dr de la Rosa Perdomo"  ' Dirección. Tipo de vía, nombre, número, piso
                        oPartiesBuyerAdmCentreAdress.PostCode = "38010"  ' Código Postal asignado por Correos.
                        oPartiesBuyerAdmCentreAdress.Town = "Santa Cruz de Tenerife"  ' Población. Correspondiente al C.P.
                        oPartiesBuyerAdmCentreAdress.Province = "S/C de Tenerife"  ' Provincia. Donde está situada la Población

                        oPartiesBuyerAdmCentres(0).CentreCode = "A05003420"
                        oPartiesBuyerAdmCentres(0).Name = "SECRETARÍA GENERAL TÉCNICA DE ECONOMÍA, HACIENDA Y SEGURIDAD".Substring(0, 39)
                        oPartiesBuyerAdmCentres(0).Item = oPartiesBuyerAdmCentreAdress
                        oPartiesBuyerAdmCentres(0).CentreDescription = "SECRETARÍA GENERAL TÉCNICA DE ECONOMÍA, HACIENDA Y SEGURIDAD".Substring(0, 39)

                        oPartiesBuyerAdmCentres(1).CentreCode = "A05003420"
                        oPartiesBuyerAdmCentres(1).Name = "SECRETARÍA GENERAL TÉCNICA DE ECONOMÍA, HACIENDA Y SEGURIDAD".Substring(0, 39)
                        oPartiesBuyerAdmCentres(1).Item = oPartiesBuyerAdmCentreAdress
                        oPartiesBuyerAdmCentres(1).CentreDescription = "SECRETARÍA GENERAL TÉCNICA DE ECONOMÍA, HACIENDA Y SEGURIDAD".Substring(0, 39)

                        oPartiesBuyerAdmCentres(2).CentreCode = "A05003420"
                        oPartiesBuyerAdmCentres(2).Name = "SECRETARÍA GENERAL TÉCNICA DE ECONOMÍA, HACIENDA Y SEGURIDAD".Substring(0, 39)
                        oPartiesBuyerAdmCentres(2).Item = oPartiesBuyerAdmCentreAdress
                        oPartiesBuyerAdmCentres(2).CentreDescription = "SECRETARÍA GENERAL TÉCNICA DE ECONOMÍA, HACIENDA Y SEGURIDAD".Substring(0, 39)

                        oPartiesBuyerAdress.Address = "Av. Dr de la Rosa Perdomo"  ' Dirección. Tipo de vía, nombre, número, piso
                        oPartiesBuyerAdress.PostCode = "38010"  ' Código Postal asignado por Correos.
                        oPartiesBuyerAdress.Town = "Santa Cruz de Tenerife"  ' Población. Correspondiente al C.P.
                        oPartiesBuyerAdress.Province = "S/C de Tenerife"  ' Provincia. Donde está situada la Población

                        oPartiesBuyerLegalEntity.CorporateName = "SECRETARÍA GENERAL TÉCNICA DE ECONOMÍA, HACIENDA Y SEGURIDAD".Substring(0, 39)
                        oPartiesBuyerLegalEntity.TradeName = "SECRETARÍA GENERAL TÉCNICA DE ECONOMÍA, HACIENDA Y SEGURIDAD".Substring(0, 39)
                    End If
                    Dim oPartiesBuyerIndividual As New facturaeService.IndividualType With {
                        .Name = Nothing,  ' Nombre de la persona física.
                        .FirstSurname = Nothing,  ' Primer apellido de la persona física
                        .SecondSurname = Nothing,  ' Segundo apellido de la persona física
                        .Item = oPartiesBuyerAdress  ' Dirección nacional. Dirección en España
                    }
                    Dim oPartiesBuyer As New facturaeService.BusinessType With {
                        .TaxIdentification = oPartiesBuyerTaxIdentification,  ' Identificación fiscal
                        .PartyIdentification = Nothing,  ' Identificación de la entidad; Rellenar con el número de referencia de la entidad del programa de facturación que utilice
                        .AdministrativeCentres = oPartiesBuyerAdmCentres  ' Centros
                    }
                    If oPartiesSellerPersonType = facturaeService.PersonTypeCodeType.F Then
                        oPartiesBuyer.Item = oPartiesBuyerIndividual  ' Persona física
                    Else
                        oPartiesBuyer.Item = oPartiesBuyerLegalEntity  ' Persona jurídica y otras
                    End If

                    Dim oParties As New facturaeService.PartiesType With {
                        .SellerParty = oPartiesSeller,  ' Emisor. Datos básicos del fichero. Son comunes a la factura o facturas que se incluyen
                        .BuyerParty = oPartiesBuyer  ' Identificación de la entidad; Rellenar con el número de referencia de la entidad del programa de facturación que utilice
                    }
                    oFacturaFACEService.Parties = oParties


                    ' INVOICES
                    Dim oFacturas(nCountFacturas) As facturaeService.InvoiceType
                    Dim nFacturaEnXMLIndex As Integer = 0
                    For Each thisRowLine As DataRow In dtFacturas.Rows
                        If bXMLporFactura Then
                            thisRowLine = thisRow
                            If nFacturaEnXMLIndex > 0 Then
                                Exit For
                            End If
                        End If
                        Dim oInvoideDocumentType As facturaeService.InvoiceDocumentTypeType
                        oInvoideDocumentType = facturaeService.InvoiceDocumentTypeType.FC  ' FC: Factura completa u ordinaria  ;  FA: Factura simplificada  ;  AF: Código sin uso
                        Dim oInvoiceClass As facturaeService.InvoiceClassType
                        ' OO: Original
                        ' OR: Original rectificativa
                        ' OC: Original recapitulativa
                        ' CO: Duplicado original
                        ' CR: Duplicado rectificativa 
                        ' CC: Duplicado recapitulativa
                        oInvoiceClass = facturaeService.InvoiceClassType.OO
                        If thisRowLine("SERIE") = "FR" Then
                            oInvoiceClass = facturaeService.InvoiceClassType.OR
                        End If
                        Dim oInvoiceHeader As New facturaeService.InvoiceHeaderType With {
                            .InvoiceSeriesCode = thisRowLine("SERIE") & thisRowLine("ANOFACTURA"), ' Número de serie asignado por el Emisor
                            .InvoiceNumber = thisRowLine("NFACTURA"), '"793",  ' Número de factura. Número asignado por el Emisor
                            .InvoiceDocumentType = oInvoideDocumentType,  ' Tipo documento factura. FC: Factura completa u ordinaria  ;  FA: Factura simplificada  ;  AF: Código sin uso
                            .InvoiceClass = oInvoiceClass  ' Clase de Factura. OO: Original  ;  OR: Original rectificativa  ;  OC: Original recapitulativa  ;  CO: Duplicado original  ;  CR: Duplicado rectificativa  ;  CC: Duplicado recapitulativa
                        }
                        ' Si es rectificativa
                        If oInvoiceClass = facturaeService.InvoiceClassType.OR Or oInvoiceClass = facturaeService.InvoiceClassType.CR Then
                            ' FACTURA RECTIFICATIVA
                            ' CorrectionMethodType:
                            ' 01: Rectificación íntegra
                            ' 02: Rectificación por diferencias
                            ' 03: Rectificación por descuento por volumen de operaciones durante un periodo
                            ' 04: Autorizadas por la Agencia Tributaria
                            Dim oInvoiceCorrectionMethod As facturaeService.CorrectionMethodType
                            oInvoiceCorrectionMethod = facturaeService.CorrectionMethodType.Item01  ' "01" - se reflejan todos los detalles a rectificar de la factura original. "02" – solo se anotan los detalles ya rectificados. "03" - Rectificación por descuento por volumen de operaciones durante un periodo. - "04" - Autorizadas por la Agencia Tributaria"

                            Dim oInvoiceCorrectionMethodDescription As facturaeService.CorrectionMethodDescriptionType
                            ' CorrectionMethodDescriptionType:
                            ' 01: Rectificación modelo íntegro
                            ' 02: Rectificación modelo por diferencias
                            ' 03: Rectificación por descuento por volumen de operaciones durante un período
                            ' 04: Autorizadas por la Agencia Tributaria
                            oInvoiceCorrectionMethodDescription = facturaeService.CorrectionMethodDescriptionType.Rectificacióníntegra

                            Dim oInvoiceCorrectiveReasonCode As facturaeService.ReasonCodeType
                            ' ReasonCodeType:
                            ' 01: Número de la factura
                            ' 02: Serie de la factura
                            ' 03: Fecha expedición
                            ' 04: Nombre y apellidos/Razón social - Emisor
                            ' 05: Nombre y apellidos/Razón social - Receptor
                            ' 06: Identificación fiscal Emisor/Obligado
                            ' 07: Identificación fiscal Receptor
                            ' 08: Domicilio Emisor/Obligado
                            ' 09: Domicilio Receptor
                            ' 10: Detalle Operación
                            ' 11: Porcentaje impositivo a aplicar
                            ' 12: Cuota tributaria a aplicar
                            ' 13: Fecha/Periodo a aplicar
                            ' 14: Clase de factura
                            ' 15: Literales legales
                            ' 16: Base imponible
                            ' 80: Cálculo de cuotas repercutidas
                            ' 81: Cálculo de cuotas retenidas
                            ' 82: Base imponible modificada por devolución de envases/embalajes
                            ' 83: Base imponible modificada por descuentos y bonificaciones
                            ' 84: Base imponible modificada por resolución firme, judicial o administrativa
                            ' 85: Base imponible modificada cuotas repercutidas no satisfechas. Auto de declaración de concurso
                            oInvoiceCorrectiveReasonCode = facturaeService.ReasonCodeType.Item01  ' Código del motivo. Código numérico del motivo de rectificación. "01" a "16" errores según reglamento

                            Dim oInvoiceCorrectiveReasonDescriptionType As facturaeService.ReasonDescriptionType
                            oInvoiceCorrectiveReasonDescriptionType = facturaeService.ReasonDescriptionType.Baseimponible  ' Descripción motivo. Descripción del motivo de rectificación y que se corresponde con cada código
                            Dim oInvoiceCorrectivePeriodDates As facturaeService.PeriodDates
                            oInvoiceCorrectivePeriodDates = New facturaeService.PeriodDates With {
                                .StartDate = "",  ' Fecha de inicio
                                .EndDate = ""  ' Fecha final
                            }
                            Dim oInvoiceCorrective As New facturaeService.CorrectiveType With {
                                .InvoiceSeriesCode = "",
                                .InvoiceNumber = "",
                                .InvoiceIssueDateSpecified = False,
                                .ReasonCode = oInvoiceCorrectiveReasonCode,  ' Código del motivo. Código numérico del motivo de rectificación. "01" a "16" errores según reglamento
                                .ReasonDescription = oInvoiceCorrectiveReasonDescriptionType,  ' Descripción motivo. Descripción del motivo de rectificación y que se corresponde con cada código
                                .TaxPeriod = oInvoiceCorrectivePeriodDates,  ' Período natural en el que se produjeron los efectos fiscales de la factura a rectificar; y, por lo tanto, se tributó, y que ahora, es objeto de rectificación
                                .CorrectionMethod = oInvoiceCorrectionMethod,  ' Código numérico que identifica el criterio empleado en cada caso para una rectificación. "01" - se reflejan todos los detalles a rectificar de la factura original. "02" – solo se anotan los detalles ya rectificados. "03" - Rectificación por descuento por volumen de operaciones durante un periodo. - "04" - Autorizadas por la Agencia Tributaria"
                                .CorrectionMethodDescription = oInvoiceCorrectionMethodDescription,  ' Descripción asociada a CorrectionMethod. 01: Rectificación modelo íntegro  ;  02: Rectificación modelo por diferencias  ;   03: Rectificación por descuento por volumen de operaciones durante un período  ;  04: Autorizadas por la Agencia Tributaria
                                .AdditionalReasonDescription = ""  ' Ampliación motivo de la rectificación
                            }
                            If oInvoiceCorrectionMethod = facturaeService.CorrectionMethodType.Item01 Or oInvoiceCorrectionMethod = facturaeService.CorrectionMethodType.Item02 Then
                                oInvoiceCorrective.InvoiceIssueDateSpecified = True
                                oInvoiceCorrective.InvoiceIssueDate = ""  ' Fecha de expedición de la factura rectificada. Valor obligatorio en el supuesto de que CorrectionMethod = “01” o “02”
                            End If
                            oInvoiceHeader.Corrective = oInvoiceCorrective
                        End If
                        oFacturas(nCountIndex) = New facturaeService.InvoiceType
                        oFacturas(nCountIndex).InvoiceHeader = oInvoiceHeader


                        Dim oInvoicePlaceOfIssue As New facturaeService.PlaceOfIssueType With {
                            .PostCode = Nothing,  ' Código postal. Asignado por Correos
                            .PlaceOfIssueDescription = ""  ' Texto del nombre del lugar
                        }
                        Dim oInvoiceCurrencyCode As facturaeService.CurrencyCodeType
                        oInvoiceCurrencyCode = facturaeService.CurrencyCodeType.EUR
                        Dim oInvoiceTaxCurrencyCode As facturaeService.CurrencyCodeType
                        oInvoiceTaxCurrencyCode = facturaeService.CurrencyCodeType.EUR
                        Dim oInvoiceLanguage As facturaeService.LanguageCodeType
                        oInvoiceLanguage = facturaeService.LanguageCodeType.es
                        Dim oInvoiceIssueData As New facturaeService.InvoiceIssueDataType With {  ' Datos de la emisión de la factura
                            .IssueDate = Date.ParseExact(thisRowLine("FECHA"), "yyyyMMdd", Nothing),' Date.ParseExact("2020-01-27", "yyyy-MM-dd", Nothing),  ' Fecha de expedición. Fecha en la que se genera la factura con efectos fiscales
                            .OperationDateSpecified = vbFalse,
                            .OperationDate = Nothing,  ' Fecha de Operación. Fecha en la que se realiza el servicio o se entrega el bien. Esta fecha solo será obligatoria si es distinta de la fecha de expedición
                            .PlaceOfIssue = Nothing, 'oInvoicePlaceOfIssue,  ' Lugar de expedición. Plaza en la que se expide el documento
                            .InvoiceCurrencyCode = oInvoiceCurrencyCode,  ' Moneda de la operación. Código de la moneda en la que se emite la factura. Si la moneda de la operación difiere de la moneda del impuesto(EURO), los campos del contravalor ExchangeRate y ExchangeRateDate deberán cumplimentarse
                            .TaxCurrencyCode = oInvoiceTaxCurrencyCode,  ' Moneda del Impuesto. Código de la moneda en la que se liquida el impuesto.
                            .LanguageName = oInvoiceLanguage,  ' Lengua. Código ISO 639-1:2002 Alpha-2 de la lengua en la que se emite el documento
                            .InvoiceDescription = Nothing,  ' Descripción general de la factura
                            .ReceiverTransactionReference = Nothing,  ' Referencia de pedido
                            .FileReference = Nothing,  ' Código del expediente de contratación
                            .ReceiverContractReference = Nothing  ' Referencia del contrato del receptor
                        }
                        If oInvoiceClass = facturaeService.InvoiceClassType.OO Or oInvoiceClass = facturaeService.InvoiceClassType.OC Or oInvoiceClass = facturaeService.InvoiceClassType.CC Then
                            Dim oInvoiceInvoicingPeriods As facturaeService.PeriodDates = Nothing
                            If bDEBUG Then
                                oInvoiceInvoicingPeriods = New facturaeService.PeriodDates With {
                                    .StartDate = Date.ParseExact("2019-10-31", "yyyy-MM-dd", Nothing),  ' Fecha de inicio
                                    .EndDate = Date.ParseExact("2019-12-31", "yyyy-MM-dd", Nothing)  ' Fecha final
                                }
                            Else
                                If Not IsDBNull(thisRowLine("FECHA1")) And Not IsDBNull(thisRowLine("FECHA2")) Then
                                    If thisRowLine("FECHA1").length > 0 And thisRowLine("FECHA2").length > 0 Then
                                        oInvoiceInvoicingPeriods = New facturaeService.PeriodDates With {
                                            .StartDate = Date.ParseExact(thisRowLine("FECHA1"), "yyyyMMdd", Nothing),  ' Fecha de inicio
                                            .EndDate = Date.ParseExact(thisRowLine("FECHA2"), "yyyyMMdd", Nothing)  ' Fecha final
                                        }
                                    ElseIf thisRowLine("FECHA1").length > 0 Then
                                        oInvoiceInvoicingPeriods = New facturaeService.PeriodDates With {
                                            .StartDate = Date.ParseExact(thisRowLine("FECHA1"), "yyyyMMdd", Nothing)  ' Fecha de inicio
                                        }
                                    Else
                                        oInvoiceInvoicingPeriods = New facturaeService.PeriodDates With {
                                            .EndDate = Date.ParseExact(thisRowLine("FECHA2"), "yyyyMMdd", Nothing)  ' Fecha final
                                        }
                                    End If
                                Else
                                    If Not IsDBNull(thisRowLine("FECHA1")) Then
                                        If thisRowLine("FECHA1").length > 0 Then
                                            oInvoiceInvoicingPeriods = New facturaeService.PeriodDates With {
                                                .StartDate = Date.ParseExact(thisRowLine("FECHA1"), "yyyyMMdd", Nothing)  ' Fecha de inicio
                                            }
                                        End If
                                    Else
                                        If thisRowLine("FECHA2").length > 0 Then
                                            oInvoiceInvoicingPeriods = New facturaeService.PeriodDates With {
                                                .EndDate = Date.ParseExact(thisRowLine("FECHA2"), "yyyyMMdd", Nothing)  ' Fecha final
                                            }
                                        End If
                                    End If
                                End If
                            End If
                            oInvoiceIssueData.InvoicingPeriod = oInvoiceInvoicingPeriods  ' Periodo de facturación. Sólo cuando se requiera: Servicio prestado temporalmente o Factura Recapitulativa. Esta información será obligatoria cuando el dato InvoiceClass (Clase) contenga alguno de los valores: "OC" ó "CC"
                        End If
                        If Not oInvoiceCurrencyCode = facturaeService.CurrencyCodeType.EUR Then
                            ' Si la moneda de la operación difiere de la moneda del impuesto(EURO), los campos del contravalor ExchangeRate y ExchangeRateDate deberán cumplimentarse
                            Dim oInvoiceExchangeRateDetails As New facturaeService.ExchangeRateDetailsType With {
                                .ExchangeRate = Nothing,  ' Tipo de Cambio. Artº 79.once de la Ley 37/92 de 28 de diciembre del Impuesto sobre el Valor Añadido. Cambio vendedor fijado por el Banco de España y vigente en el momento del devengo
                                .ExchangeRateDate = Nothing  ' Fecha de publicación del tipo de cambio aplicado
                            }
                            oInvoiceIssueData.ExchangeRateDetails = oInvoiceExchangeRateDetails  ' Detalles del tipo de cambio
                        End If
                        oFacturas(nCountIndex).InvoiceIssueData = oInvoiceIssueData


                        ' TAXESOUTPUTS  /  Impuestos repercutidos
                        Dim oInvoiceTaxTypeCode As facturaeService.TaxTypeCodeType
                        ' TaxTypeCodeType:
                        ' 01: IVA: Impuesto sobre el valor añadido
                        ' 02: IPSI: Impuesto sobre la producción, los servicios y la importación
                        ' 03: IGIC: Impuesto general indirecto de Canarias
                        ' 04: IRPF: Impuesto sobre la Renta de las personas físicas
                        ' 05: Otro
                        oInvoiceTaxTypeCode = facturaeService.TaxTypeCodeType.Item03
                        Dim nDetallesIGIC As Integer = 0
                        If Not (IsDBNull(thisRowLine("BASEIGIC1"))) Then
                            nDetallesIGIC += 1
                        End If
                        If Not (IsDBNull(thisRowLine("BASEIGIC2"))) Then
                            nDetallesIGIC += 1
                        End If
                        If Not (IsDBNull(thisRowLine("BASEIGIC3"))) Then
                            nDetallesIGIC += 1
                        End If
                        If Not (IsDBNull(thisRowLine("BASEIGIC4"))) Then
                            nDetallesIGIC += 1
                        End If
                        If Not (IsDBNull(thisRowLine("BASEIGIC5"))) Then
                            nDetallesIGIC += 1
                        End If

                        Dim oInvoiceTaxableBaseAmount As New facturaeService.AmountType With {
                                .TotalAmount = 0.0, ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                                .EquivalentInEurosSpecified = vbFalse
                            }
                        If Not oInvoiceCurrencyCode = facturaeService.CurrencyCodeType.EUR Then
                            oInvoiceTaxableBaseAmount.EquivalentInEurosSpecified = vbFalse
                            oInvoiceTaxableBaseAmount.EquivalentInEuros = Nothing  ' Importe equivalente en Euros
                        End If
                        Dim oInvoiceTaxAmount As New facturaeService.AmountType With {
                                .TotalAmount = 0.0,  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                                .EquivalentInEurosSpecified = False
                            }
                        If Not oInvoiceTaxCurrencyCode = facturaeService.CurrencyCodeType.EUR Then
                            oInvoiceTaxAmount.EquivalentInEurosSpecified = vbFalse
                            oInvoiceTaxAmount.EquivalentInEuros = Nothing  ' Importe equivalente en Euros
                        End If
                        Dim oInvoiceSpecialTaxableBaseAmount As New facturaeService.AmountType With {
                                .TotalAmount = Nothing,  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                                .EquivalentInEurosSpecified = vbFalse
                            }
                        If Not oInvoiceCurrencyCode = facturaeService.CurrencyCodeType.EUR Then
                            oInvoiceSpecialTaxableBaseAmount.EquivalentInEurosSpecified = vbFalse
                            oInvoiceSpecialTaxableBaseAmount.EquivalentInEuros = Nothing  ' Importe equivalente en Euros
                        End If
                        Dim oInvoiceSpecialTaxAmount As New facturaeService.AmountType With {
                                .TotalAmount = Nothing,  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                                .EquivalentInEurosSpecified = vbFalse
                            }
                        If Not oInvoiceTaxCurrencyCode = facturaeService.CurrencyCodeType.EUR Then
                            oInvoiceSpecialTaxAmount.EquivalentInEurosSpecified = vbFalse
                            oInvoiceSpecialTaxAmount.EquivalentInEuros = Nothing  ' Importe equivalente en Euros
                        End If
                        Dim oInvoiceEquivalenceSurchargeAmount As New facturaeService.AmountType With {
                                .TotalAmount = 0.0,  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                                .EquivalentInEurosSpecified = vbFalse
                            }
                        If Not oInvoiceCurrencyCode = facturaeService.CurrencyCodeType.EUR Then
                            oInvoiceEquivalenceSurchargeAmount.EquivalentInEurosSpecified = vbFalse
                            oInvoiceEquivalenceSurchargeAmount.EquivalentInEuros = Nothing  ' Importe equivalente en Euros
                        End If

                        Dim oInvoiceTaxes(nDetallesIGIC) As facturaeService.TaxOutputType

                        Dim nTotalBASE As Double = 0.0
                        Dim nTotalIGIC As Double = 0.0
                        Dim nIndiceDetalleIGIC As Integer = 0
                        If Not IsDBNull(thisRowLine("BASEIGIC1")) Then
                            If thisRowLine("BASEIGIC1").ToString.Length > 6 Then
                                If thisRowLine("BASEIGIC1").Substring(6, 1) = "," Or thisRowLine("BASEIGIC1").Substring(6, 1) = "." Then
                                    thisRowLine("BASEIGIC1") = thisRowLine("BASEIGIC1").Substring(0, 6) & "0" & thisRowLine("BASEIGIC1").Substring(6)
                                End If
                                thisRowLine("BASEIGIC1") = Mid(thisRowLine("BASEIGIC1"), 7, Len(thisRowLine("BASEIGIC1")) - 6)
                            Else
                                thisRowLine("BASEIGIC1") = 0.0
                            End If

                            If Not IsDBNull(thisRowLine("VALORIGIC1")) Then
                                If thisRowLine("VALORIGIC1").ToString.Length = 0 Then
                                    thisRowLine("VALORIGIC1") = 0.0
                                Else
                                    thisRowLine("VALORIGIC1") = CDbl(thisRowLine("VALORIGIC1")) '"0"
                                End If
                            Else
                                thisRowLine("VALORIGIC1") = 0.0
                            End If

                            If Not IsDBNull(thisRowLine("TEXTOIGIC1")) Then
                                If thisRowLine("TEXTOIGIC1").ToString.Length = 0 Then
                                    thisRowLine("TEXTOIGIC1") = 0.0
                                Else
                                    thisRowLine("TEXTOIGIC1") = CDbl(thisRowLine("TEXTOIGIC1").Replace("IGIC ", "").Replace("%", "")) '"0"
                                End If
                            Else
                                thisRowLine("TEXTOIGIC1") = 0.0
                            End If


                            oInvoiceTaxableBaseAmount.TotalAmount = CDbl(thisRowLine("BASEIGIC1"))  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                            oInvoiceTaxAmount.TotalAmount = CDbl(thisRowLine("VALORIGIC1"))  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse

                            nTotalBASE += CDbl(thisRowLine("BASEIGIC1"))
                            nTotalIGIC += CDbl(thisRowLine("VALORIGIC1"))

                            oInvoiceTaxes(nIndiceDetalleIGIC) = New facturaeService.TaxOutputType With {
                                .TaxTypeCode = oInvoiceTaxTypeCode,  ' Identificador del impuesto por el que se tributa. 
                                .TaxRate = thisRowLine("TEXTOIGIC1"),  ' Tipo impositivo
                                .TaxableBase = oInvoiceTaxableBaseAmount,  ' Base imponible
                                .TaxAmount = oInvoiceTaxAmount,  ' Cuota
                                .SpecialTaxableBase = Nothing, 'oInvoiceSpecialTaxableBaseAmount,  ' Base imponible del régimen especial del grupo de entidades
                                .SpecialTaxAmount = Nothing, 'oInvoiceSpecialTaxAmount,  ' Cuota especial
                                .EquivalenceSurcharge = Nothing,  ' Tipo de recargo de Equivalencia. Siempre con dos decimales
                                .EquivalenceSurchargeAmount = Nothing, 'oInvoiceEquivalenceSurchargeAmount,  ' Cuota. Importe resultante de aplicar a la Base Imponible, la misma que para el IVA, el porcentaje indicado en “EquivalenceSurchage”
                                .EquivalenceSurchargeSpecified = vbFalse  ' true / false
                            }

                            nIndiceDetalleIGIC += 1
                        End If
                        If Not IsDBNull(thisRowLine("BASEIGIC2")) Then
                            If thisRowLine("BASEIGIC2").Length > 6 Then
                                If thisRowLine("BASEIGIC2").Substring(6, 1) = "," Or thisRowLine("BASEIGIC2").Substring(6, 1) = "." Then
                                    thisRowLine("BASEIGIC2") = thisRowLine("BASEIGIC2").Substring(0, 6) & "0" & thisRowLine("BASEIGIC2").Substring(6)
                                End If
                                thisRowLine("BASEIGIC2") = Mid(thisRowLine("BASEIGIC2"), 7, Len(thisRowLine("BASEIGIC2")) - 6)
                            Else
                                thisRowLine("BASEIGIC2") = 0.0
                            End If

                            If Not IsDBNull(thisRowLine("VALORIGIC2")) Then
                                If thisRowLine("VALORIGIC2").ToString.Length = 0 Then
                                    thisRowLine("VALORIGIC2") = 0.0
                                Else
                                    thisRowLine("VALORIGIC2") = CDbl(thisRowLine("VALORIGIC2"))
                                End If
                            Else
                                thisRowLine("VALORIGIC2") = 0.0
                            End If

                            If Not IsDBNull(thisRowLine("TEXTOIGIC2")) Then
                                If thisRowLine("TEXTOIGIC2").ToString.Length = 0 Then
                                    thisRowLine("TEXTOIGIC2") = 0.0
                                Else
                                    thisRowLine("TEXTOIGIC2") = CDbl(thisRowLine("TEXTOIGIC2").Replace("IGIC ", "").Replace("%", ""))
                                End If
                            Else
                                thisRowLine("TEXTOIGIC2") = 0.0
                            End If

                            oInvoiceTaxableBaseAmount.TotalAmount = CDbl(thisRowLine("BASEIGIC2"))  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                            oInvoiceTaxAmount.TotalAmount = CDbl(thisRowLine("VALORIGIC2"))  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse

                            nTotalBASE += CDbl(thisRowLine("BASEIGIC2"))
                            nTotalIGIC += CDbl(thisRowLine("VALORIGIC2"))

                            oInvoiceTaxes(nIndiceDetalleIGIC) = New facturaeService.TaxOutputType With {
                                .TaxTypeCode = oInvoiceTaxTypeCode,  ' Identificador del impuesto por el que se tributa. 
                                .TaxRate = thisRowLine("TEXTOIGIC2"),  ' Tipo impositivo
                                .TaxableBase = oInvoiceTaxableBaseAmount,  ' Base imponible
                                .TaxAmount = oInvoiceTaxAmount,  ' Cuota
                                .SpecialTaxableBase = Nothing, 'oInvoiceSpecialTaxableBaseAmount,  ' Base imponible del régimen especial del grupo de entidades
                                .SpecialTaxAmount = Nothing, 'oInvoiceSpecialTaxAmount,  ' Cuota especial
                                .EquivalenceSurcharge = Nothing,  ' Tipo de recargo de Equivalencia. Siempre con dos decimales
                                .EquivalenceSurchargeAmount = Nothing, 'oInvoiceEquivalenceSurchargeAmount,  ' Cuota. Importe resultante de aplicar a la Base Imponible, la misma que para el IVA, el porcentaje indicado en “EquivalenceSurchage”
                                .EquivalenceSurchargeSpecified = vbFalse  ' true / false
                            }

                            nIndiceDetalleIGIC += 1
                        End If
                        If Not IsDBNull(thisRowLine("BASEIGIC3")) Then
                            If thisRowLine("BASEIGIC3").Length > 6 Then
                                If thisRowLine("BASEIGIC3").Substring(6, 1) = "," Or thisRowLine("BASEIGIC3").Substring(6, 1) = "." Then
                                    thisRowLine("BASEIGIC3") = thisRowLine("BASEIGIC3").Substring(0, 6) & "0" & thisRowLine("BASEIGIC3").Substring(6)
                                End If
                                thisRowLine("BASEIGIC3") = Replace(Mid(thisRowLine("BASEIGIC3"), 7, Len(thisRowLine("BASEIGIC3")) - 6), ",", ".")
                            Else
                                thisRowLine("BASEIGIC3") = 0.0
                            End If

                            If Not IsDBNull(thisRowLine("VALORIGIC3")) Then
                                If thisRowLine("VALORIGIC3").ToString.Length = 0 Then
                                    thisRowLine("VALORIGIC3") = 0.0
                                Else
                                    thisRowLine("VALORIGIC3") = CDbl(thisRowLine("VALORIGIC3"))
                                End If
                            Else
                                thisRowLine("VALORIGIC3") = 0.0
                            End If

                            If Not IsDBNull(thisRowLine("TEXTOIGIC3")) Then
                                If thisRowLine("TEXTOIGIC3").ToString.Length = 0 Then
                                    thisRowLine("TEXTOIGIC3") = 0.0
                                Else
                                    thisRowLine("TEXTOIGIC3") = CDbl(thisRowLine("TEXTOIGIC3").Replace("IGIC ", "").Replace("%", ""))
                                End If
                            Else
                                thisRowLine("TEXTOIGIC3") = 0.0
                            End If

                            oInvoiceTaxableBaseAmount.TotalAmount = CDbl(thisRowLine("BASEIGIC3"))  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                            oInvoiceTaxAmount.TotalAmount = CDbl(thisRowLine("VALORIGIC3"))  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse

                            nTotalBASE += CDbl(thisRowLine("BASEIGIC3"))
                            nTotalIGIC += CDbl(thisRowLine("VALORIGIC3"))

                            oInvoiceTaxes(nIndiceDetalleIGIC) = New facturaeService.TaxOutputType With {
                                .TaxTypeCode = oInvoiceTaxTypeCode,  ' Identificador del impuesto por el que se tributa. 
                                .TaxRate = thisRowLine("TEXTOIGIC3"),  ' Tipo impositivo
                                .TaxableBase = oInvoiceTaxableBaseAmount,  ' Base imponible
                                .TaxAmount = oInvoiceTaxAmount,  ' Cuota
                                .SpecialTaxableBase = Nothing, 'oInvoiceSpecialTaxableBaseAmount,  ' Base imponible del régimen especial del grupo de entidades
                                .SpecialTaxAmount = Nothing, 'oInvoiceSpecialTaxAmount,  ' Cuota especial
                                .EquivalenceSurcharge = Nothing,  ' Tipo de recargo de Equivalencia. Siempre con dos decimales
                                .EquivalenceSurchargeAmount = Nothing, 'oInvoiceEquivalenceSurchargeAmount,  ' Cuota. Importe resultante de aplicar a la Base Imponible, la misma que para el IVA, el porcentaje indicado en “EquivalenceSurchage”
                                .EquivalenceSurchargeSpecified = vbFalse  ' true / false
                            }

                            nIndiceDetalleIGIC += 1
                        End If
                        If Not IsDBNull(thisRowLine("BASEIGIC4")) Then
                            If thisRowLine("BASEIGIC4").Length > 6 Then
                                If thisRowLine("BASEIGIC4").Substring(6, 1) = "," Or thisRowLine("BASEIGIC4").Substring(6, 1) = "." Then
                                    thisRowLine("BASEIGIC4") = thisRowLine("BASEIGIC4").Substring(0, 6) & "0" & thisRowLine("BASEIGIC4").Substring(6)
                                End If
                                thisRowLine("BASEIGIC4") = Replace(Mid(thisRowLine("BASEIGIC4"), 7, Len(thisRowLine("BASEIGIC4")) - 6), ",", ".")
                            Else
                                thisRowLine("BASEIGIC4") = 0.0
                            End If

                            If Not IsDBNull(thisRowLine("VALORIGIC4")) Then
                                If thisRowLine("VALORIGIC4").ToString.Length = 0 Then
                                    thisRowLine("VALORIGIC4") = 0.0
                                Else
                                    thisRowLine("VALORIGIC4") = CDbl(thisRowLine("VALORIGIC4"))
                                End If
                            Else
                                thisRowLine("VALORIGIC4") = 0.0
                            End If

                            If Not IsDBNull(thisRowLine("TEXTOIGIC4")) Then
                                If thisRowLine("TEXTOIGIC4").ToString.Length = 0 Then
                                    thisRowLine("TEXTOIGIC4") = 0.0
                                Else
                                    thisRowLine("TEXTOIGIC4") = CDbl(thisRowLine("TEXTOIGIC4").Replace("IGIC ", "").Replace("%", ""))
                                End If
                            Else
                                thisRowLine("TEXTOIGIC4") = 0.0
                            End If

                            oInvoiceTaxableBaseAmount.TotalAmount = CDbl(thisRowLine("BASEIGIC4"))  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                            oInvoiceTaxAmount.TotalAmount = CDbl(thisRowLine("VALORIGIC4"))  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse

                            nTotalBASE += CDbl(thisRowLine("BASEIGIC4"))
                            nTotalIGIC += CDbl(thisRowLine("VALORIGIC4"))

                            oInvoiceTaxes(nIndiceDetalleIGIC) = New facturaeService.TaxOutputType With {
                                .TaxTypeCode = oInvoiceTaxTypeCode,  ' Identificador del impuesto por el que se tributa. 
                                .TaxRate = thisRowLine("TEXTOIGIC4"),  ' Tipo impositivo
                                .TaxableBase = oInvoiceTaxableBaseAmount,  ' Base imponible
                                .TaxAmount = oInvoiceTaxAmount,  ' Cuota
                                .SpecialTaxableBase = Nothing, 'oInvoiceSpecialTaxableBaseAmount,  ' Base imponible del régimen especial del grupo de entidades
                                .SpecialTaxAmount = Nothing, 'oInvoiceSpecialTaxAmount,  ' Cuota especial
                                .EquivalenceSurcharge = Nothing,  ' Tipo de recargo de Equivalencia. Siempre con dos decimales
                                .EquivalenceSurchargeAmount = Nothing, 'oInvoiceEquivalenceSurchargeAmount,  ' Cuota. Importe resultante de aplicar a la Base Imponible, la misma que para el IVA, el porcentaje indicado en “EquivalenceSurchage”
                                .EquivalenceSurchargeSpecified = vbFalse  ' true / false
                            }

                            nIndiceDetalleIGIC += 1
                        End If
                        If Not IsDBNull(thisRowLine("BASEIGIC5")) Then
                            If thisRowLine("BASEIGIC5").Length > 6 Then
                                If thisRowLine("BASEIGIC5").Substring(6, 1) = "," Or thisRowLine("BASEIGIC5").Substring(6, 1) = "." Then
                                    thisRowLine("BASEIGIC5") = thisRowLine("BASEIGIC5").Substring(0, 6) & "0" & thisRowLine("BASEIGIC5").Substring(6)
                                End If
                                thisRowLine("BASEIGIC5") = Replace(Mid(thisRowLine("BASEIGIC5"), 7, Len(thisRowLine("BASEIGIC5")) - 6), ",", ".")
                            Else
                                thisRowLine("BASEIGIC5") = 0.0
                            End If

                            If Not IsDBNull(thisRowLine("VALORIGIC5")) Then
                                If thisRowLine("VALORIGIC5").ToString.Length = 0 Then
                                    thisRowLine("VALORIGIC5") = 0.0
                                Else
                                    thisRowLine("VALORIGIC5") = CDbl(thisRowLine("VALORIGIC5"))
                                End If
                            Else
                                thisRowLine("VALORIGIC5") = 0.0
                            End If

                            If Not IsDBNull(thisRowLine("TEXTOIGIC5")) Then
                                If thisRowLine("TEXTOIGIC5").ToString.Length = 0 Then
                                    thisRowLine("TEXTOIGIC5") = 0.0
                                Else
                                    thisRowLine("TEXTOIGIC5") = CDbl(thisRowLine("TEXTOIGIC4").Replace("IGIC ", "").Replace("%", ""))
                                End If
                            Else
                                thisRowLine("TEXTOIGIC5") = 0.0
                            End If

                            oInvoiceTaxableBaseAmount.TotalAmount = CDbl(thisRowLine("BASEIGIC5"))  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                            oInvoiceTaxAmount.TotalAmount = CDbl(thisRowLine("VALORIGIC5"))  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse

                            nTotalBASE += CDbl(thisRowLine("BASEIGIC5"))
                            nTotalIGIC += CDbl(thisRowLine("VALORIGIC5"))

                            oInvoiceTaxes(nIndiceDetalleIGIC) = New facturaeService.TaxOutputType With {
                                .TaxTypeCode = oInvoiceTaxTypeCode,  ' Identificador del impuesto por el que se tributa. 
                                .TaxRate = thisRowLine("TEXTOIGIC5"),  ' Tipo impositivo
                                .TaxableBase = oInvoiceTaxableBaseAmount,  ' Base imponible
                                .TaxAmount = oInvoiceTaxAmount,  ' Cuota
                                .SpecialTaxableBase = Nothing, 'oInvoiceSpecialTaxableBaseAmount,  ' Base imponible del régimen especial del grupo de entidades
                                .SpecialTaxAmount = Nothing, 'oInvoiceSpecialTaxAmount,  ' Cuota especial
                                .EquivalenceSurcharge = Nothing,  ' Tipo de recargo de Equivalencia. Siempre con dos decimales
                                .EquivalenceSurchargeAmount = Nothing, 'oInvoiceEquivalenceSurchargeAmount,  ' Cuota. Importe resultante de aplicar a la Base Imponible, la misma que para el IVA, el porcentaje indicado en “EquivalenceSurchage”
                                .EquivalenceSurchargeSpecified = vbFalse  ' true / false
                            }

                            nIndiceDetalleIGIC += 1
                        End If


                        If oInvoiceTaxTypeCode = facturaeService.TaxTypeCodeType.Item05 Then
                            'AditionalLineItemInformation = ""
                        End If
                        oFacturas(nCountIndex).TaxesOutputs = oInvoiceTaxes


                        ' TAXESWITHHELD  /  Impuestos retenidos
                        Dim bConRetenciones As Boolean = False
                        If bConRetenciones Then
                            Dim oInvoiceWithheldTaxTypeCode As facturaeService.TaxTypeCodeType
                            ' TaxTypeCodeType:
                            ' 01: IVA: Impuesto sobre el valor añadido
                            ' 02: IPSI: Impuesto sobre la producción, los servicios y la importación
                            ' 03: IGIC: Impuesto general indirecto de Canarias
                            ' 04: IRPF: Impuesto sobre la Renta de las personas físicas
                            ' 05: Otro 
                            oInvoiceWithheldTaxTypeCode = facturaeService.TaxTypeCodeType.Item01
                            Dim oInvoiceWithheldTaxableBaseAmount As New facturaeService.AmountType With {
                                .TotalAmount = "",  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                                .EquivalentInEurosSpecified = vbFalse
                            }
                            If Not oInvoiceCurrencyCode = facturaeService.CurrencyCodeType.EUR Then
                                oInvoiceWithheldTaxableBaseAmount.EquivalentInEurosSpecified = True
                                oInvoiceWithheldTaxableBaseAmount.EquivalentInEuros = ""  ' Importe equivalente en Euros
                            End If
                            Dim oInvoiceWithheldTaxAmount As New facturaeService.AmountType With {
                                .TotalAmount = "",  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                                .EquivalentInEurosSpecified = vbFalse
                            }
                            If Not oInvoiceTaxCurrencyCode = facturaeService.CurrencyCodeType.EUR Then
                                oInvoiceWithheldTaxAmount.EquivalentInEurosSpecified = True
                                oInvoiceWithheldTaxAmount.EquivalentInEuros = ""  ' Importe equivalente en Euros
                            End If
                            Dim oInvoiceTaxesWithheld(1) As facturaeService.TaxType
                            oInvoiceTaxesWithheld(0) = New facturaeService.TaxType With {
                                .TaxTypeCode = oInvoiceWithheldTaxTypeCode,  ' Identificador del impuesto por cuenta del cual se retiene
                                .TaxRate = "",  ' Tipo impositivo
                                .TaxableBase = oInvoiceWithheldTaxableBaseAmount,  ' Base de retención
                                .TaxAmount = oInvoiceWithheldTaxAmount  ' Importe de la retención
                            }
                            oFacturas(nCountIndex).TaxesWithheld = oInvoiceTaxesWithheld
                        End If



                        ' INVOICETOTALS  /  Totales de factura
                        ' Descuentos
                        Dim oInvoiceDiscounts(1) As facturaeService.DiscountType
                        oInvoiceDiscounts(0) = New facturaeService.DiscountType With {
                            .DiscountReason = Nothing,  ' Concepto por el que se aplica descuento
                            .DiscountRateSpecified = vbFalse,
                            .DiscountRate = Nothing,  ' Porcentaje a descontar del Total Importe Bruto (TIB)
                            .DiscountAmount = Nothing ' Importe a descontar sobre el TIB
                        }
                        ' Cargos
                        Dim oInvoiceSurcharges(1) As facturaeService.ChargeType
                        oInvoiceSurcharges(0) = New facturaeService.ChargeType With {
                            .ChargeReason = Nothing,  ' Concepto por el que se aplica el cargo
                            .ChargeRateSpecified = vbFalse,
                            .ChargeRate = Nothing,  ' Porcentaje a cargar sobre el TIB
                            .ChargeAmount = Nothing  ' Importe a cargar sobre el TIB
                        }
                        ' Subvenciones
                        Dim oInvoiceSubsidies(1) As facturaeService.SubsidyType
                        oInvoiceSubsidies(0) = New facturaeService.SubsidyType With {
                            .SubsidyDescription = Nothing,  ' Detalle de la Subvención aplicada
                            .SubsidyRateSpecified = vbFalse,
                            .SubsidyRate = Nothing,  ' Porcentaje de la Subvención. Porcentaje a aplicar al Total Factura
                            .SubsidyAmount = Nothing  ' Importe de la Subvención. Importe a aplicar al Total Factura
                        }
                        ' Anticipos
                        Dim oInvoicePaymentsOnAccount(1) As facturaeService.PaymentOnAccountType
                        oInvoicePaymentsOnAccount(0) = New facturaeService.PaymentOnAccountType With {
                            .PaymentOnAccountDate = Nothing,  ' Fecha/s del/os anticipo/s
                            .PaymentOnAccountAmount = Nothing  ' Importe de cada anticipo
                        }
                        ' Suplidos
                        ' Seller
                        Dim oInvoiceReimbursableExpensesSellerType As facturaeService.PersonTypeCodeType
                        oInvoiceReimbursableExpensesSellerType = facturaeService.PersonTypeCodeType.J
                        Dim oInvoiceReimbursableExpensesSellerResidence As facturaeService.ResidenceTypeCodeType
                        oInvoiceReimbursableExpensesSellerResidence = facturaeService.ResidenceTypeCodeType.R
                        Dim oInvoiceReimbursableExpensesSeller As New facturaeService.TaxIdentificationType With {
                            .PersonTypeCode = oInvoiceReimbursableExpensesSellerType,  ' Tipo de persona. Física o Jurídica. "F" - Física; "J" - Jurídica
                            .ResidenceTypeCode = oInvoiceReimbursableExpensesSellerResidence,  ' Identificación del tipo de residencia y/o extranjería. "E" - Extranjero; "R" - Residente; "U" - Residente en la Unión Europea
                            .TaxIdentificationNumber = Nothing  ' Código de Identificación Fiscal del sujeto. Se trata de las composiciones de NIF/CIF que marca la Administración correspondiente, precedidas de las dos letras del país
                        }
                        ' Buyer
                        Dim oInvoiceReimbursableExpensesBuyerType As facturaeService.PersonTypeCodeType
                        oInvoiceReimbursableExpensesBuyerType = facturaeService.PersonTypeCodeType.J
                        Dim oInvoiceReimbursableExpensesBuyerResidence As facturaeService.ResidenceTypeCodeType
                        oInvoiceReimbursableExpensesBuyerResidence = facturaeService.ResidenceTypeCodeType.R
                        Dim oInvoiceReimbursableExpensesBuyer As New facturaeService.TaxIdentificationType With {
                            .PersonTypeCode = oInvoiceReimbursableExpensesBuyerType,  ' Tipo de persona. Física o Jurídica. "F" - Física; "J" - Jurídica
                            .ResidenceTypeCode = oInvoiceReimbursableExpensesBuyerResidence,  ' Identificación del tipo de residencia y/o extranjería. "E" - Extranjero; "R" - Residente; "U" - Residente en la Unión Europea
                            .TaxIdentificationNumber = Nothing  ' Código de Identificación Fiscal del sujeto. Se trata de las composiciones de NIF/CIF que marca la Administración correspondiente, precedidas de las dos letras del país
                        }
                        Dim oInvoiceReimbursableExpenses(1) As facturaeService.ReimbursableExpensesType
                        oInvoiceReimbursableExpenses(0) = New facturaeService.ReimbursableExpensesType With {
                            .ReimbursableExpensesSellerParty = oInvoiceReimbursableExpensesSeller,  ' Seller
                            .ReimbursableExpensesBuyerParty = oInvoiceReimbursableExpensesBuyer,  ' Buyer
                            .IssueDateSpecified = vbFalse,  ' Especificar IssueDate?
                            .IssueDate = Nothing,  ' Fecha
                            .InvoiceNumber = Nothing,  ' Número de factura
                            .InvoiceSeriesCode = Nothing,  ' Serie
                            .ReimbursableExpensesAmount = Nothing  ' Importe reembolsable
                        }
                        ' Retención
                        Dim oInvoiceAmountsWithheld As New facturaeService.AmountsWithheldType With {
                            .WithholdingReason = Nothing,  ' Motivo de Retención. Descripción de la finalidad de la Retención
                            .WithholdingRateSpecified = vbFalse, ' true / false
                            .WithholdingRate = Nothing,  ' Porcentaje de Retención. Porcentaje sobre el Total a Pagar
                            .WithholdingAmount = Nothing  ' Importe de Retención. Importe a retener sobre el Total a Pagar
                        }
                        ' Pagos en especies
                        Dim oInvoicePaymentInKind As New facturaeService.PaymentInKindType With {
                            .PaymentInKindReason = Nothing,  ' Descripción del motivo por el que existe un pago en especie
                            .PaymentInKindAmount = Nothing  ' Importe de los pagos en especie
                        }
                        Dim oInvoiceTotals As New facturaeService.InvoiceTotalsType With {
                            .TotalGrossAmount = CDbl(thisRowLine("TOTAL")),   ' Total Importe Bruto. Suma total de importes brutos de los detalles de la factura
                            .TotalGeneralDiscountsSpecified = vbTrue, ' true / false
                            .TotalGeneralDiscounts = 0.00,  ' Total general de descuentos. Sumatorio de los importes de los diferentes campos GeneralDiscounts
                            .TotalGeneralSurchargesSpecified = vbTrue, ' true / false
                            .TotalGeneralSurcharges = 0.00,  ' Total general de cargos. Sumatorio de los importes de los diferentes campos GeneralSurcharges
                            .TotalGrossAmountBeforeTaxes = CDbl(thisRowLine("TOTAL")),  ' Total importe bruto antes de impuestos. Resultado de: TotalGrossAmount - TotalGeneralDiscounts + TotalGeneralSurcharges
                            .TotalTaxOutputs = nTotalIGIC,  ' Total impuestos repercutidos. Sumatorio de todas las Cuotas y Recargos de Equivalencia
                            .TotalTaxesWithheld = Nothing,  ' Total impuestos retenidos
                            .InvoiceTotal = CDbl(thisRowLine("TOTAL")),  ' Total factura. Resultado de: TotalGrossAmountBeforeTaxes +TotalTaxOutputs - TotalTaxesWithheld
                            .Subsidies = Nothing, 'oInvoiceSubsidies,  ' Subvenciones por adquisición de determinados bienes. Habrá tantos bloques de campos Subsidies como subvenciones se apliquen. En el caso de que la subvención se aplique solo a parte de las operaciones facturadas, en el subelemento SubsidyDescription se especificará también a qué operación corresponde
                            .PaymentsOnAccount = Nothing, 'oInvoicePaymentsOnAccount,  ' Anticipos. Pagos anticipados sobre el Total Facturas. Habrá tantos bloques PaymentsOnAccount como clases de anticipos se apliquen a nivel factura
                            .ReimbursableExpenses = Nothing, 'oInvoiceReimbursableExpenses,  ' Suplidos incorporados en la factura
                            .TotalFinancialExpensesSpecified = vbFalse, ' true / false
                            .TotalFinancialExpenses = Nothing,  ' Total de gastos financieros. Siempre con dos decimales
                            .TotalOutstandingAmount = CDbl(thisRowLine("TOTAL")),  ' Total a pagar. Resultado de: InvoiceTotal - (Total subvenciones + TotalPaymentsOnAccount). En Total subvenciones se sumarán las cantidades especificadas en los bloques Subsidies
                            .TotalPaymentsOnAccountSpecified = vbFalse, ' true / false
                            .TotalPaymentsOnAccount = Nothing,  ' Total de anticipos. Sumatorio de los campos PaymentOnAccountAmount
                            .AmountsWithheld = Nothing, 'oInvoiceAmountsWithheld,  ' Cantidades que retiene el pagador hasta el buen fin de la operación
                            .TotalExecutableAmount = CDbl(thisRowLine("TOTAL")),  ' Total a ejecutar. Resultado de: TotalOutstandingAmount - TOTAL de Cantidades retenidas - PaymentInKindAmount+ TotalReimbursableExpenses +TotalFinancialExpenses.En TOTAL de Cantidades retenidas se sumaran las cantidades especificadas en los bloques AmountsWithheld
                            .TotalReimbursableExpensesSpecified = vbFalse, ' true / false
                            .TotalReimbursableExpenses = Nothing,  ' Total de suplidos
                            .PaymentInKind = Nothing 'oInvoicePaymentInKind  ' Pagos en especie
                        }
                        If oInvoiceTotals.TotalGeneralDiscounts > 0 Then
                            oInvoiceTotals.GeneralDiscounts = oInvoiceDiscounts  ' Descuentos sobre el Total Importe Bruto. Habrá tantos bloques de campos GeneralDiscounts como clases de descuentos diferentes se apliquen a nivel de factura
                        End If
                        If oInvoiceTotals.TotalGeneralSurcharges > 0 Then
                            oInvoiceTotals.GeneralSurcharges = oInvoiceSurcharges  ' Cargos sobre el Total Importe Bruto. Habrá tantos bloques de campos GeneralSurcharges como clases de cargos/ recargos se apliquen a nivel de factura
                        End If

                        oFacturas(nCountIndex).InvoiceTotals = oInvoiceTotals


                        ' ITEMS  /  Líneas de detalle de la factura
                        Dim oInvoiceItemUnitOfMeasure As facturaeService.UnitOfMeasureType
                        ' REVISAR:  OJO!!!!   Item01, Item02, ...   Qué es cada elemento????????????
                        oInvoiceItemUnitOfMeasure = facturaeService.UnitOfMeasureType.Item33
                        ' Albaranes / Información del albarán
                        Dim oInvoiceItemDeliveryNotes(1) As facturaeService.DeliveryNoteType
                        oInvoiceItemDeliveryNotes(0) = New facturaeService.DeliveryNoteType With {
                            .DeliveryNoteNumber = Nothing,  ' Número de referencia del albarán
                            .DeliveryNoteDateSpecified = vbFalse,
                            .DeliveryNoteDate = Nothing  ' Fecha del albarán
                        }
                        ' Descuentos
                        Dim oInvoiceItemDiscountsAndRebates(1) As facturaeService.DiscountType
                        oInvoiceItemDiscountsAndRebates(0) = New facturaeService.DiscountType With {
                            .DiscountReason = Nothing,  ' Concepto por el que se aplica descuento
                            .DiscountRateSpecified = vbFalse,  ' Introducir porcentaje? true/false
                            .DiscountRate = Nothing,  ' Porcentaje a descontar del Total Importe Bruto (TIB) - 4 a 8 decimales
                            .DiscountAmount = Nothing  ' Importe a descontar sobre el TIB - 6 a 8 decimales
                        }
                        ' Cargos
                        Dim oInvoiceItemCharges(1) As facturaeService.ChargeType
                        oInvoiceItemCharges(0) = New facturaeService.ChargeType With {
                            .ChargeReason = Nothing,  ' Concepto por el que se aplica el cargo
                            .ChargeRateSpecified = vbFalse,  ' Introducir porcentaje? true/false
                            .ChargeRate = Nothing,  ' Porcentaje a cargar sobre el TIB. Los porcentajes se reflejan - 4 a 8 decimales
                            .ChargeAmount = Nothing  ' Importe a cargar sobre el TIB - 6 a 8 decimales
                        }
                        ' IMPUESTOS
                        Dim oInvoiceItemTaxCurrencyCode As facturaeService.CurrencyCodeType
                        oInvoiceItemTaxCurrencyCode = facturaeService.CurrencyCodeType.EUR
                        ' TAXESWITHHELD  /  Impuestos retenidos
                        Dim oInvoiceItemWithheldTaxTypeCode As facturaeService.TaxTypeCodeType
                        ' REVISAR:  OJO!!!!   Item01, Item02, ...   Qué es cada elemento????????????  
                        oInvoiceItemWithheldTaxTypeCode = facturaeService.TaxTypeCodeType.Item01
                        Dim oInvoiceItemWithheldTaxableBaseAmount As New facturaeService.AmountType With {
                            .TotalAmount = Nothing,  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                            .EquivalentInEurosSpecified = vbFalse
                        }
                        If Not oInvoiceItemTaxCurrencyCode = facturaeService.CurrencyCodeType.EUR Then
                            oInvoiceItemWithheldTaxableBaseAmount.EquivalentInEurosSpecified = True
                            oInvoiceItemWithheldTaxableBaseAmount.EquivalentInEuros = ""  ' Importe equivalente en Euros. Siempre con dos decimales.
                        End If
                        Dim oInvoiceItemWithheldTaxAmount As New facturaeService.AmountType With {
                            .TotalAmount = Nothing,  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                            .EquivalentInEurosSpecified = vbFalse
                        }
                        If Not oInvoiceItemTaxCurrencyCode = facturaeService.CurrencyCodeType.EUR Then
                            oInvoiceItemWithheldTaxAmount.EquivalentInEurosSpecified = vbFalse
                            oInvoiceItemWithheldTaxAmount.EquivalentInEuros = 0.0  ' Importe equivalente en Euros. Siempre con dos decimales.
                        End If
                        Dim oInvoiceItemTaxesWithheld(1) As facturaeService.TaxType
                        oInvoiceItemTaxesWithheld(0) = New facturaeService.TaxType With {
                            .TaxTypeCode = oInvoiceItemWithheldTaxTypeCode,  ' Identificador del impuesto por cuenta del cual se retiene
                            .TaxRate = Nothing,  ' Tipo impositivo - 2 a 8 decimales
                            .TaxableBase = oInvoiceItemWithheldTaxableBaseAmount,  ' Base de retención
                            .TaxAmount = oInvoiceItemWithheldTaxAmount  ' Importe de la retención - 2 a 8 decimales
                        }
                        ' TAXESOUTPUTS  /  Impuestos repercutidos
                        Dim oInvoiceItemTaxTypeCode As facturaeService.TaxTypeCodeType
                        ' REVISAR:  OJO!!!!   Item01, Item02, ...   Qué es cada elemento????????????  
                        oInvoiceItemTaxTypeCode = facturaeService.TaxTypeCodeType.Item03  ' 03: IGIC
                        Dim oInvoiceItemTaxableBaseAmount As New facturaeService.AmountType With {
                            .TotalAmount = CDbl(thisRowLine("TOTAL")),  '"279",  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                            .EquivalentInEurosSpecified = vbFalse
                        }
                        If Not oInvoiceItemTaxCurrencyCode = facturaeService.CurrencyCodeType.EUR Then
                            oInvoiceItemTaxableBaseAmount.EquivalentInEurosSpecified = vbFalse
                            oInvoiceItemTaxableBaseAmount.EquivalentInEuros = ""  ' Importe equivalente en Euros. Siempre con dos decimales.
                        End If
                        Dim oInvoiceItemTaxAmount As New facturaeService.AmountType With {
                            .TotalAmount = Nothing,  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                            .EquivalentInEurosSpecified = vbFalse
                        }
                        If Not oInvoiceItemTaxCurrencyCode = facturaeService.CurrencyCodeType.EUR Then
                            oInvoiceItemTaxAmount.EquivalentInEurosSpecified = vbFalse
                            oInvoiceItemTaxAmount.EquivalentInEuros = Nothing  ' Importe equivalente en Euros. Siempre con dos decimales.
                        End If
                        Dim oInvoiceItemSpecialTaxableBaseAmount As New facturaeService.AmountType With {
                            .TotalAmount = Nothing,  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                            .EquivalentInEurosSpecified = vbFalse
                        }
                        If Not oInvoiceItemTaxCurrencyCode = facturaeService.CurrencyCodeType.EUR Then
                            oInvoiceItemSpecialTaxableBaseAmount.EquivalentInEurosSpecified = vbFalse
                            oInvoiceItemSpecialTaxableBaseAmount.EquivalentInEuros = Nothing  ' Importe equivalente en Euros. Siempre con dos decimales.
                        End If
                        Dim oInvoiceItemSpecialTaxAmount As New facturaeService.AmountType With {
                            .TotalAmount = Nothing,  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                            .EquivalentInEurosSpecified = vbFalse
                        }
                        If Not oInvoiceItemTaxCurrencyCode = facturaeService.CurrencyCodeType.EUR Then
                            oInvoiceItemSpecialTaxAmount.EquivalentInEurosSpecified = vbFalse
                            oInvoiceItemSpecialTaxAmount.EquivalentInEuros = Nothing  ' Importe equivalente en Euros
                        End If
                        Dim oInvoiceItemEquivalenceSurchargeAmount As New facturaeService.AmountType With {
                            .TotalAmount = Nothing,  ' Importe en la moneda original de la facturación. Siempre que la divisa de facturación sea distinta de EURO, el elemento EquivalentInEuros deberá cumplimentarse
                            .EquivalentInEurosSpecified = vbFalse
                        }
                        If Not oInvoiceItemTaxCurrencyCode = facturaeService.CurrencyCodeType.EUR Then
                            oInvoiceItemEquivalenceSurchargeAmount.EquivalentInEurosSpecified = vbFalse
                            oInvoiceItemEquivalenceSurchargeAmount.EquivalentInEuros = Nothing  ' Importe equivalente en Euros
                        End If
                        Dim oInvoiceItemTaxesOutputs(1) As facturaeService.InvoiceLineTypeTax
                        oInvoiceItemTaxesOutputs(0) = New facturaeService.InvoiceLineTypeTax With {
                            .TaxTypeCode = oInvoiceItemTaxTypeCode,  ' Identificador del impuesto por el que se tributa. 
                            .TaxRate = nTotalIGIC,  ' Tipo impositivo - 2 a 8 decimales
                            .TaxableBase = oInvoiceItemTaxableBaseAmount,  ' Base imponible
                            .TaxAmount = oInvoiceItemTaxAmount,  ' Cuota - 2 a 8 decimales
                            .SpecialTaxableBase = Nothing, 'oInvoiceItemSpecialTaxableBaseAmount,  ' Base imponible del régimen especial del grupo de entidades
                            .SpecialTaxAmount = Nothing, 'oInvoiceItemSpecialTaxAmount,  ' Cuota especial
                            .EquivalenceSurchargeSpecified = vbFalse,  ' true / false
                            .EquivalenceSurcharge = Nothing,  ' Tipo de recargo de Equivalencia. Siempre con dos decimales
                            .EquivalenceSurchargeAmount = Nothing 'oInvoiceItemEquivalenceSurchargeAmount,  ' Cuota. Importe resultante de aplicar a la Base Imponible, la misma que para el IVA, el porcentaje indicado en “EquivalenceSurchage”
                        }
                        If oInvoiceItemTaxTypeCode = facturaeService.TaxTypeCodeType.Item05 Then
                            'oInvoiceItemTaxes(0).AditionalLineItemInformation = ""
                        End If
                        ' Periodos de prestación de servicios
                        Dim oInvoiceItemLineItemPeriod As New facturaeService.PeriodDates With {
                            .StartDate = Nothing,  ' Fecha de inicio. ISO 8601:2004.
                            .EndDate = Nothing  ' Fecha final. ISO 8601:2004
                        }
                        ' Obligatoriedad de los impuestos
                        Dim oInvoiceItemSpecialTaxableEventCode As facturaeService.SpecialTaxableEventCodeType
                        oInvoiceItemSpecialTaxableEventCode = facturaeService.SpecialTaxableEventCodeType.Item01
                        Dim oInvoiceItemSpecialTaxableEvent As New facturaeService.SpecialTaxableEventType With {
                               .SpecialTaxableEventCode = oInvoiceItemSpecialTaxableEventCode,  ' Código de fiscalidad especial. Cuando un hecho imponible (taxable event) presenta una fiscalidad especial. No se informará este elemento cuando no exista fiscalidad especial. Posibles valores:  [01], [02]
                               .SpecialTaxableEventReason = ""  ' Justificación de la fiscalidad especial que se aplica en esta operación.Como este elemento se define a nivel de línea, no de impuesto, es necesario especificar a qué impuesto se refiere.Para establecer esta relación, al comienzo de este elemento se indicará el código del impuesto al que corresponde según la lista de código de impuestos del tipo “TaxTypeCodeType”. Si hubiera varios impuestos con el código “05” (“Otro”), se añadirán los valores de sus campos “TaxRate”, “TaxableBase” y “TaxAmount”, en este orden, hasta que sea posible discernirlos; es decir:  05 [valor “TaxRate”] [valor “TaxableBase”] [valor “TaxAmount”]… 
                        }
                        ' Extensiones
                        'Dim oInvoiceItemXMLElement(1) As XmlAnyElement
                        'oInvoiceItemXMLElement(0) = New XmlAnyElement With {

                        '}
                        Dim oInvoiceItemExtensions As New facturaeService.ExtensionsType
                        'oInvoiceItemExtensions.Any = oInvoiceItemXMLElement ' 

                        Dim oInvoiceItems(1) As facturaeService.InvoiceLineType
                        oInvoiceItems(0) = New facturaeService.InvoiceLineType With {  ' Líneas de detalle de la factura
                            .IssuerContractReference = Nothing,  ' Referencia del contrato del Emisor
                            .IssuerContractDateSpecified = vbFalse,
                            .IssuerContractDate = Nothing,  ' Fecha del contrato del emisor
                            .IssuerTransactionReference = Nothing,  ' Referencia de la Operación, Número de Pedido, Contrato, etc. del Emisor
                            .IssuerTransactionDateSpecified = vbFalse,
                            .IssuerTransactionDate = Nothing,  ' Fecha de operación / pedido del emisor
                            .ReceiverContractReference = Nothing,  ' Referencia del contrato del Receptor
                            .ReceiverContractDateSpecified = vbFalse,
                            .ReceiverContractDate = Nothing,  ' Fecha del contrato del receptor
                            .ReceiverTransactionReference = Nothing,  ' Referencia de la Operación, Número de Pedido, Contrato, etc.del Receptor
                            .ReceiverTransactionDateSpecified = vbFalse,
                            .ReceiverTransactionDate = Nothing,  ' Fecha de operación / pedido del receptor
                            .FileReference = Nothing,  ' Referencia del expediente
                            .FileDateSpecified = vbFalse,
                            .FileDate = Nothing,  ' Fecha del expediente
                            .SequenceNumberSpecified = vbFalse,
                            .SequenceNumber = Nothing,  ' Número de secuencia o línea del pedido
                            .DeliveryNotesReferences = Nothing, 'oInvoiceItemDeliveryNotes,  ' Referencias de albaranes
                            .ItemDescription = "Agua de riego",  ' Descripción del bien o servicio
                            .Quantity = thisRowLine("CONSUMOM3"),  ' Cantidad. Número de Unidades servidas/prestadas
                            .UnitOfMeasureSpecified = vbTrue,
                            .UnitOfMeasure = oInvoiceItemUnitOfMeasure,  ' Unidad en que está referida la Cantidad. Recomendación 20, Revisión 4 y Recomendación 21, Revisión 5 de UN/ CEFACT
                            .UnitPriceWithoutTax = 0.6,  ' Precio de la unidad de bien o servicio servido/prestado, en la moneda indicada en la Cabecera de la Factura. Siempre sin Impuestos
                            .TotalCost = CDbl(thisRowLine("TOTAL")),  ' Coste Total. Resultado: Quantity x UnitPriceWithoutTax - 6 a 8 decimales
                            .DiscountsAndRebates = Nothing, 'oInvoiceItemDiscountsAndRebates,  ' Descuentos
                            .Charges = Nothing, 'oInvoiceItemCharges,  ' Cargos.
                            .GrossAmount = CDbl(thisRowLine("TOTAL")),  ' Importe bruto. Resultado: TotalCost - DiscountAmount + ChargeAmount - 6 a 8 decimales
                            .TaxesWithheld = Nothing, 'oInvoiceItemTaxesWithheld,  ' Impuestos retenidos. Es una secuencia de elementos, cada uno de los cuales contiene la información de un impuesto retenido
                            .TaxesOutputs = oInvoiceItemTaxesOutputs,  ' Impuestos repercutidos. Es una secuencia de elementos, cada uno de los cuales contiene la información de un impuesto repercutido
                            .LineItemPeriod = Nothing, 'oInvoiceItemLineItemPeriod,  ' Información sobre el periodo de prestación de un servicio
                            .TransactionDateSpecified = vbFalse,
                            .TransactionDate = Nothing,  ' Fecha concreta de prestación o entrega del bien o servicio
                            .AdditionalLineItemInformation = Nothing,  ' Información adicional. Libre para el emisor por cada detalle
                            .SpecialTaxableEvent = Nothing, 'oInvoiceItemSpecialTaxableEvent,  ' Este campo indica la obligatoriedad de los impuestos
                            .ArticleCode = Nothing,  ' Código de artículo
                            .Extensions = Nothing 'oInvoiceItemExtensions  ' Extensiones. Podrán incorporarse nuevas definiciones estructuradas cuando sean de interés conjunto para emisores y receptores, y no estén ya definidas en el esquema de la factura
                        }
                        oFacturas(nCountIndex).Items = oInvoiceItems


                        ' Datos de pago / Vencimientos
                        Dim bConPagos As Boolean = False
                        If bConPagos Then
                            Dim oInvoicePaymentMeans As facturaeService.PaymentMeansType
                            ' REVISAR:  OJO!!!!   Item01, Item02, ...   Qué es cada elemento????????????  
                            oInvoicePaymentMeans = facturaeService.PaymentMeansType.Item01
                            ' Dirección de la sucursal/oficina en España
                            Dim oInvoicePaymentAccountCountry As facturaeService.CountryType
                            oInvoicePaymentAccountCountry = facturaeService.CountryType.ESP
                            Dim oInvoicePaymentAccountInSpain As New facturaeService.AddressType With {
                                .Address = Nothing,  ' Dirección. Tipo de vía, nombre, número, piso
                                .PostCode = Nothing,  ' Código Postal asignado por Correos.
                                .Town = Nothing,  ' Población. Correspondiente al C.P.
                                .Province = Nothing,  ' Provincia. Donde está situada la Población.
                                .CountryCode = oInvoicePaymentAccountCountry  ' Código País. Código según la ISO 3166-1:2006 Alpha-3. Al ser un domicilio ubicado en España siempre será "ESP"
                            }
                            ' Cuenta
                            Dim oInvoicePaymentAccount As New facturaeService.AccountType With {
                                .ItemElementName = 1, '"IBAN",  ' IBAN. Único formato admitido para identificar la cuenta. (Recomendado)
                                .BankCode = Nothing,  ' Código de la entidad financiera
                                .BranchCode = Nothing,  ' Código de la oficina de la entidad financiera
                                .BIC = Nothing,  ' Código SWIFT. Será obligatorio rellenar las 11 posiciones, utilizando los caracteres XXX cuando no se informe de la sucursal
                                .Item1 = oInvoicePaymentAccountInSpain  ' Dirección de la sucursal/oficina en España
                            }
                            If oInvoicePaymentAccount.ItemElementName = 1 Then '"IBAN" Then
                                oInvoicePaymentAccount.Item = Nothing  ' IBAN
                            ElseIf oInvoicePaymentAccount.ItemElementName = "AccountNumber" Then
                                oInvoicePaymentAccount.Item = Nothing  ' Número de cuenta
                            End If
                            Dim oInvoicePayment(1) As facturaeService.InstallmentType
                            oInvoicePayment(0) = New facturaeService.InstallmentType With {
                                .InstallmentDueDate = Nothing,  ' Fechas en las que se deben atender los pagos. ISO 8601: 2004
                                .InstallmentAmount = Nothing,  ' Importe a satisfacer en cada plazo. Siempre con dos decimales
                                .PaymentMeans = oInvoicePaymentMeans,  ' Cada vencimiento/importe podrá tener un medio de pago concreto. Posibles valores: [01], [02], [03], [04]
                                .AccountToBeCredited = oInvoicePaymentAccount,  ' Cuenta de abono. Único formato admitido. Cuando la forma de pago (PaymentMeans) sea "transferencia" este dato será obligatorio
                                .PaymentReconciliationReference = Nothing,  ' Referencia expresa del pago. Dato que precisa el Emisor para conciliar los pagos con cada factura
                                .CollectionAdditionalInformation = Nothing,  ' Observaciones de cobro. Libre para uso del Emisor.
                                .RegulatoryReportingData = Nothing,  ' Código Estadístico. Usado en las operaciones transfronterizas según las especificaciones de la circular del Banco España 15/1992
                                .DebitReconciliationReference = Nothing  ' Referencia del cliente pagador, similar a la utilizada por elemisor para la conciliación de los pagos.
                            }
                            ' REVISAR:  OJO!!!!   Item01, Item02, ...   Cuál corresponde a "recibo domiciliado" ????????????  
                            If oInvoicePaymentMeans = facturaeService.PaymentMeansType.Item01 Then  ' "recibo domiciliado"
                                ' Dirección de la sucursal/oficina en España
                                Dim oInvoicePaymentDebitedAccountCountry As facturaeService.CountryType
                                oInvoicePaymentDebitedAccountCountry = facturaeService.CountryType.ESP
                                Dim oInvoicePaymentDebitedAccountInSpain As New facturaeService.AddressType With {
                                    .Address = Nothing,  ' Dirección. Tipo de vía, nombre, número, piso
                                    .PostCode = Nothing,  ' Código Postal asignado por Correos.
                                    .Town = Nothing,  ' Población. Correspondiente al C.P.
                                    .Province = Nothing,  ' Provincia. Donde está situada la Población.
                                    .CountryCode = oInvoicePaymentDebitedAccountCountry  ' Código País. Código según la ISO 3166-1:2006 Alpha-3. Al ser un domicilio ubicado en España siempre será "ESP"
                                }
                                ' Cuenta
                                Dim oInvoicePaymentDebitedAccount As New facturaeService.AccountType With {
                                    .ItemElementName = 1, '"IBAN",  ' IBAN. Único formato admitido para identificar la cuenta. (Recomendado)
                                    .BankCode = Nothing,  ' Código de la entidad financiera
                                    .BranchCode = Nothing,  ' Código de la oficina de la entidad financiera
                                    .BIC = Nothing,  ' Código SWIFT. Será obligatorio rellenar las 11 posiciones, utilizando los caracteres XXX cuando no se informe de la sucursal
                                    .Item1 = oInvoicePaymentDebitedAccountInSpain  ' Dirección de la sucursal/oficina en España
                                }
                                If oInvoicePaymentDebitedAccount.ItemElementName = 1 Then '"IBAN" Then
                                    oInvoicePaymentDebitedAccount.Item = Nothing  ' IBAN
                                ElseIf oInvoicePaymentDebitedAccount.ItemElementName = "AccountNumber" Then
                                    oInvoicePaymentDebitedAccount.Item = Nothing  ' Número de cuenta
                                End If
                                oInvoicePayment(0).AccountToBeDebited = oInvoicePaymentDebitedAccount  ' Cuenta de cargo. Único formato admitido. Cuando la forma de pago (PaymentMeans) sea "recibo domiciliado" este dato será obligatorio
                            End If

                            oFacturas(nCountIndex).PaymentDetails = oInvoicePayment
                        End If


                        Dim bConTextosLegales As Boolean = False
                        If bConTextosLegales Then
                            'Textos literales que deben figurar obligatoriamente en
                            'determinadas facturas.Los textos establecidos en la
                            'legislación vigente son: Operación exenta por aplicación del
                            'artículo [indicar el artículo] de la Ley 37/1992, de 28 de
                            'diciembre, del Impuesto sobre el Valor Añadido; Medio de
                            'transporte [describir el medio, por ejemplo automóvil turismo
                            'Seat Ibiza TDI 2.0] fecha 1ª puesta en servicio [indicar la
                            'fecha] distancias/horas recorridas [indicar la distancia o las
                            'horas, por ejemplo, 5.9 km o 48 horas]; Facturación por el
                            'destinatario;Inversión del sujeto pasivo; Régimen especial de
                            'las agencias de viajes; Régimen especial de los bienes usados;
                            'Régimen especial de los objetos de arte; Régimen especial de
                            'las antigüedades y objetos de colección; Régimen especial del
                            'criterio de caja; Operación exenta por aplicación del artículo
                            '11.27º del Real Decreto 2538/1994 por el que se dictan las
                            'normas de desarrollo del Impuesto General Indirecto Canario;
                            'Comerciante Minorista.Operación exenta por aplicación del
                            'artículo 11.27º del Real Decreto 2538/1994 por el que se
                            'dictan las normas de desarrollo del Impuesto General
                            'Indirecto Canario.NOTA 1: Salvo el texto “Inversión del sujeto
                            'pasivo”, los demás se refieren no a la factura en su conjunto,
                            'sino a una determinada operación (línea) de la factura. Se
                            'deberá especificar a cuál corresponde. NOTA 2: Debe
                            'permitirse la posibilidad de cumplimentar este campo con
                            'cualquier cadena alfanumérica de hasta 250 caracteres
                            'introducida por el usuario por si se establecen nuevos textos
                            'literales obligatorios en el futuro.
                            Dim oInvoiceLegalLiterals(1) As String
                            oInvoiceLegalLiterals(0) = ""
                            oFacturas(nCountIndex).LegalLiterals = oInvoiceLegalLiterals
                        End If



                        ' Datos adicionales
                        Dim bAgregarDatosAdicionales As Boolean = False
                        If bAgregarDatosAdicionales Then
                            ' Tipo de compresión: GZIP, ZIP, NONE
                            Dim oInvoiceAdditionalDataAttachmentCompressionAlgorithm As facturaeService.AttachmentCompressionAlgorithmType
                            oInvoiceAdditionalDataAttachmentCompressionAlgorithm = facturaeService.AttachmentCompressionAlgorithmType.NONE
                            ' Formato: bmp, doc, gif, html, jpg, pdf, rtf, tiff, xds, xml
                            Dim oInvoiceAdditionalDataAttachmentFormat As facturaeService.AttachmentFormatType
                            oInvoiceAdditionalDataAttachmentFormat = facturaeService.AttachmentFormatType.pdf
                            ' Codificación: BASE64, BER, DER, NONE
                            Dim oInvoiceAdditionalDataAttachmentEncoding As facturaeService.AttachmentEncodingType
                            oInvoiceAdditionalDataAttachmentEncoding = facturaeService.AttachmentEncodingType.NONE
                            ' Extensiones
                            'Dim doc As XmlDocument = New XmlDocument()
                            'Dim oInvoiceAdditionalDataExtensionsXMLElements(1) As XmlElement
                            ''oInvoiceAdditionalDataExtensionsXMLElements(0) = doc.CreateElement("Prefix")
                            'oInvoiceAdditionalDataExtensionsXMLElements(0).Prefix = ""
                            'oInvoiceAdditionalDataExtensionsXMLElements(0).InnerText = ""
                            'oInvoiceAdditionalDataExtensionsXMLElements(0).InnerXml = ""
                            'oInvoiceAdditionalDataExtensionsXMLElements(0).Value = ""
                            'oInvoiceAdditionalDataExtensionsXMLElements(0).IsEmpty = vbTrue

                            Dim oInvoiceAdditionalDataExtensions As facturaeService.ExtensionsType
                            oInvoiceAdditionalDataExtensions = New facturaeService.ExtensionsType With {
                                .Any = Nothing 'oInvoiceAdditionalDataExtensionsXMLElements
                            }
                            ' Documentos adjuntos
                            Dim oInvoiceAdditionalDataDocuments(1) As facturaeService.AttachmentType
                            oInvoiceAdditionalDataDocuments(0) = New facturaeService.AttachmentType With {
                                .AttachmentCompressionAlgorithmSpecified = vbTrue,  ' Algoritmo especificado? true / false
                                .AttachmentCompressionAlgorithm = oInvoiceAdditionalDataAttachmentCompressionAlgorithm,  ' Algoritmo usado para comprimir el documento adjunto. Ampliar restricciones según convenga. Posibles valores: [ZIP], [GZIP], [NONE]
                                .AttachmentFormat = oInvoiceAdditionalDataAttachmentFormat,  ' Formato del documento adjunto. Ampliar restricciones según convenga. Posibles valores: [Xml], [doc], [gif], [rtf]...
                                .AttachmentEncodingSpecified = vbFalse,  ' Codificación especificada? true / false
                                .AttachmentEncoding = oInvoiceAdditionalDataAttachmentEncoding,  ' Algoritmo usado para codificar el documento adjunto. Posibles valores: [BASE64], [BER], [DER], [NONE]
                                .AttachmentDescription = "",  ' Descripción del documento
                                .AttachmentData = ""  ' Stream de datos del documento adjunto
                            }
                            Dim oInvoiceAdditionalData As New facturaeService.AdditionalDataType With {
                                .RelatedInvoice = "",  ' Factura asociada. Número y Serie de acuerdo Emisor / Receptor.
                                .RelatedDocuments = oInvoiceAdditionalDataDocuments,  ' Documento adjunto. En [BASE-64]. Contiene los documentos relacionados con la factura en el formato deseado(imagen, PDF, Xml, etc.)
                                .InvoiceAdditionalInformation = "",  ' Información adicional. Lo que considere oportuno el Emisor. En este elemento se recogerá el motivo por lo que el impuesto correspondiente está "no sujeto" o "exento", cuando se produzca esta situación
                                .Extensions = oInvoiceAdditionalDataExtensions  ' Extensiones. Podrán incorporarse nuevas definiciones estructuradas cuando sean de interés conjunto para emisores y receptores, y no estén ya definidas en el esquema de la factura
                            }
                            oFacturas(nCountIndex).AdditionalData = oInvoiceAdditionalData
                        End If


                        nFacturaEnXMLIndex += 1
                        nCountIndex += 1
                    Next
                    oFacturaFACEService.Invoices = oFacturas



                    'EXTENSIONES
                    Dim bConExtensiones As Boolean = False
                    If bConExtensiones Then
                        'Dim oExtensionsXMLElements(1) As System.Xml.XmlElement
                        'oExtensionsXMLElements(0).Prefix = ""
                        'oExtensionsXMLElements(0).InnerText = ""
                        'oExtensionsXMLElements(0).InnerXml = ""
                        'oExtensionsXMLElements(0).Value = ""
                        'oExtensionsXMLElements(0).IsEmpty = vbTrue
                        Dim oExtensions As facturaeService.ExtensionsType
                        oExtensions = New facturaeService.ExtensionsType With {
                        .Any = Nothing 'oExtensionsXMLElements
                    }
                        'Extensiones. Podrán incorporarse nuevas definiciones estructuradas 
                        'cuando sean de interés conjunto para emisores y receptores, y no estén 
                        'ya definidas en el esquema de la factura.
                        oFacturaFACEService.Extensions = oExtensions  ' 
                    End If



                    ' SIGNATURE
                    ' Cargar certificado para autenticación con servicio web
                    Dim oCertificado As Security.Cryptography.X509Certificates.X509Certificate2
                    oCertificado = DameCertificado()
                    Dim bConFirma As Boolean = False
                    If bConFirma Then
                        If Not IsNothing(oCertificado) Then

                            Dim oSignatureCanonicalizationMethod As New facturaeService.CanonicalizationMethodType With {
                                .Algorithm = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315"
                            }
                            Dim oSignatureSignatureMethod As New facturaeService.SignatureMethodType With {
                                .Algorithm = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256" '"http://www.w3.org/2000/09/xmldsig#rsa-sha1"
                            }

                            ' REFERENCES
                            Dim oSignatureDigestMethod As New facturaeService.DigestMethodType With {
                                .Algorithm = "http://www.w3.org/2001/04/xmlenc#sha512" '"http://www.w3.org/2000/09/xmldsig#sha1"
                            }
                            ' REVISAR: OJO!!!
                            Dim oSignatureDigestValue As Byte() = Nothing

                            Dim oSignatureTransformType(1) As facturaeService.TransformType
                            oSignatureTransformType(0) = New facturaeService.TransformType With {
                                .Algorithm = "http://www.w3.org/2000/09/xmldsig#enveloped-signature"
                            }
                            Dim oSignatureReference(3) As facturaeService.ReferenceType
                            oSignatureReference(0) = New facturaeService.ReferenceType With {
                                .Id = "",
                                .Type = "http://www.w3.org/2000/09/xmldsig#Object",
                                .URI = "",
                                .DigestMethod = oSignatureDigestMethod,
                                .DigestValue = oSignatureDigestValue,
                                .Transforms = oSignatureTransformType
                            }

                            oSignatureReference(1) = New facturaeService.ReferenceType With {
                                .Id = "",
                                .Type = "http://uri.etsi.org/01903#SignedProperties",
                                .URI = "",
                                .DigestMethod = oSignatureDigestMethod,
                                .DigestValue = oSignatureDigestValue
                            }

                            oSignatureReference(2) = New facturaeService.ReferenceType With {
                                .Id = "",
                                .Type = "", '"http://uri.etsi.org/01903/v1.3.2#SignedProperties"
                                .URI = "#Signature-26be912e-e768-4db3-94f6-3122256207fe-KeyInfo",
                                .DigestMethod = oSignatureDigestMethod,
                                .DigestValue = oSignatureDigestValue
                            }
                            Dim oSignatureSignedInfo As New facturaeService.SignedInfoType With {
                                .Id = "",
                                .CanonicalizationMethod = oSignatureCanonicalizationMethod,
                                .SignatureMethod = oSignatureSignatureMethod,
                                .Reference = oSignatureReference
                            }
                            Dim oSignatureValueType As New facturaeService.SignatureValueType With {
                                .Value = oCertificado.GetPublicKey,
                                .Id = ""
                            }

                            Dim oSignatureText(1) As String
                            oSignatureText(0) = ""
                            Dim oSignatureKeyItemsElement(1) As facturaeService.ItemsChoiceType2
                            oSignatureKeyItemsElement(0) = facturaeService.ItemsChoiceType2.X509Data
                            Dim oSignatureKeyX509ItemsElement(1) As facturaeService.ItemsChoiceType
                            oSignatureKeyX509ItemsElement(0) = facturaeService.ItemsChoiceType.X509Certificate
                            Dim oSignatureKeyX509base64(1) As Object
                            oSignatureKeyX509base64(0) = Convert.ToBase64String(oCertificado.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks) 'bytes2String(Base64.encodeBase64(oCertificado.ToString().getBytes("UTF-8")))
                            Dim oSignatureKeyX509(1) As facturaeService.X509DataType
                            oSignatureKeyX509(0) = New facturaeService.X509DataType With {
                                .ItemsElementName = oSignatureKeyX509ItemsElement,
                                .Items = oSignatureKeyX509base64
                            }
                            Dim oSignatureKeyInfo As New facturaeService.KeyInfoType With {
                                .Id = "",
                                .Text = oSignatureText,
                                .ItemsElementName = oSignatureKeyItemsElement,
                                .Items = oSignatureKeyX509
                            }

                            Dim oSignature As New facturaeService.SignatureType With {
                                .Id = "",
                                .SignedInfo = oSignatureSignedInfo,
                                .SignatureValue = oSignatureValueType,
                                .KeyInfo = oSignatureKeyInfo
                            }
                            'Conjunto de datos asociados a la factura que garantizarán
                            'la autoría y la integridad del mensaje. Se define como
                            'opcional para facilitar la verificación y el tránsito del fichero.
                            'No obstante, debe cumplimentarse este bloque de firma
                            'electrónica para que se considere una factura electrónica
                            'válida legalmente frente a terceros

                            oFacturaFACEService.Signature = oSignature  ' 
                        End If
                    End If



                    ' GUARDAR XML 
                    ' Specify our namespace
                    Dim oNamespaces As XmlSerializerNamespaces = New XmlSerializerNamespaces()
                    oNamespaces.Add("ds", "http://www.w3.org/2000/09/xmldsig#")
                    'Dim oImporter As New SoapReflectionImporter
                    'Dim oTypeMapping As XmlTypeMapping = oImporter.ImportTypeMapping(oFacturaXMLFile.GetType)
                    Dim oSerializer As XmlSerializer = New XmlSerializer(oFacturaFACEService.GetType)
                    'oSerializer = New XmlSerializer(oTypeMapping)
                    Dim oWriter As New XmlTextWriter(sRutaYNombreArchivo, System.Text.Encoding.UTF8)
                    'oSerializer.Serialize(oWriter, oFacturaXMLFile)
                    oSerializer.Serialize(oWriter, oFacturaFACEService, oNamespaces)
                    oWriter.Close()


                    ' ****** FIRMAR XML A XSIG ******
                    FirmarXMLFace(oCertificado, sRutaYNombreArchivo)
                    sNombreArchivo = sNombreArchivo.Replace(".xml", ".xsig")
                    sRutaYNombreArchivo = sRutaYNombreArchivo.Replace(".xml", ".xsig")

                    ' Generate a signing key.
                    'Dim Key As RSACryptoServiceProvider = New RSACryptoServiceProvider()
                    'SignXmlFile(Path.Combine(frmMain.sApplicationPath, "FacturaFACE.xml"), Path.Combine(frmMain.sApplicationPath, "FacturaFACE_signed.xml"), Key)


                    ' Create a New CspParameters object to specify
                    '' a key container.
                    'Dim cspParams As CspParameters = New CspParameters()
                    'cspParams.KeyContainerName = "XML_DSIG_RSA_KEY"

                    '' Create a New RSA signing key And save it in the container. 
                    'Dim rsaKey As RSACryptoServiceProvider = New RSACryptoServiceProvider(cspParams)

                    '' Create a New XML document.
                    'Dim xmlDoc As XmlDocument = New XmlDocument()

                    '' Load an XML file into the XmlDocument object.
                    'xmlDoc.PreserveWhitespace = True
                    'xmlDoc.Load(Path.Combine(frmMain.sApplicationPath, "FacturaFACE.xml"))

                    '' Sign the XML document. 
                    'SignXml(xmlDoc, rsaKey)

                    ''Console.WriteLine("XML file signed.")

                    '' Save the document.
                    'xmlDoc.Save(Path.Combine(frmMain.sApplicationPath, "FacturaFACE_signed.xml"))

                    oArchivosFacturas(nXMLFileIndex, 0) = sRutaYNombreArchivo
                    oArchivosFacturas(nXMLFileIndex, 1) = sNombreArchivo
                    nXMLFileIndex += 1
                Next

                Dim sListaArchivosFacturas As String = ""
                'sListaArchivosFacturas = String.Join(vbCrLf, Enumerable.Range(0, oArchivosFacturas.GetLength(1)).[Select](Function(column) oArchivosFacturas(row, column)))
                Dim i As Integer
                For i = 0 To oArchivosFacturas.GetLength(0) - 1
                    If Not IsNothing(oArchivosFacturas(i, 1)) Then
                        If Convert.ToString(oArchivosFacturas(i, 1)).Length > 0 Then
                            If sListaArchivosFacturas.Length > 0 Then
                                sListaArchivosFacturas &= vbCrLf
                            End If
                            sListaArchivosFacturas &= Convert.ToString(oArchivosFacturas(i, 1))
                        End If
                    End If
                Next
                sMessageText = "Se ha guardado la factura firmada en formato XSIG con el siguiente nombre: " & vbCrLf & vbCrLf & oArchivosFacturas(0, 1)
                If bXMLporFactura And nCountFacturas > 1 Then
                    sMessageText = "Se han guardado las facturas firmadas en formato XSIG con los siguientes nombres: " & vbCrLf & vbCrLf & sListaArchivosFacturas
                ElseIf Not bXMLporFactura And nCountFacturas > 1 Then
                    sMessageText = "Se han guardado las facturas firmadas en formato XSIG con el siguiente nombre: " & vbCrLf & vbCrLf & sListaArchivosFacturas
                End If

                MessageBox.Show(sMessageText, "Resultado factura firmada", MessageBoxButtons.OK, MessageBoxIcon.Information)



                ' ********** ENVIAR FACTURA A FACE **********
                sMessageText = "¿Presentar la siguiente factura XSIG a través del servicio web de FACE?" & vbCrLf & vbCrLf & oArchivosFacturas(0, 1)
                If bXMLporFactura And nCountFacturas > 1 Then
                    sMessageText = "¿Presentar las siguientes facturas XSIG a través del servicio web de FACE?" & vbCrLf & vbCrLf & sListaArchivosFacturas
                ElseIf Not bXMLporFactura And nCountFacturas > 1 Then
                    sMessageText = "¿Presentar las facturas XSIG contenidas en el archivo detallado a continuación a través del servicio web de FACE?" & vbCrLf & vbCrLf & oArchivosFacturas(0, 1)
                End If
                If MessageBox.Show(sMessageText, "Presentar factura(s)", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = vbOK Then

                    Dim oCertificado As Security.Cryptography.X509Certificates.X509Certificate2
                    oCertificado = DameCertificado()
                    If Not IsNothing(oCertificado) Then

                        Dim oCertificadoPEM As String
                        Dim oStringBuilder As StringBuilder = New StringBuilder()
                        oStringBuilder.AppendLine("-----BEGIN CERTIFICATE-----")
                        oStringBuilder.AppendLine(Convert.ToBase64String(oCertificado.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks))
                        oStringBuilder.AppendLine("-----End CERTIFICATE-----")
                        oCertificadoPEM = oStringBuilder.ToString()

                        For i = 0 To oArchivosFacturas.GetLength(0) - 1
                            If Not IsNothing(oArchivosFacturas(i, 1)) Then
                                If Convert.ToString(oArchivosFacturas(i, 1)).Length > 0 Then
                                    Dim sRutaYNombreArchivoFactura As String = Convert.ToString(oArchivosFacturas(i, 0))
                                    Dim sNombreArchivoFactura As String = Convert.ToString(oArchivosFacturas(i, 1))
                                    sRutaYNombreArchivo = Convert.ToString(oArchivosFacturas(i, 0))
                                    'For Each sArchivoFactura In oArchivosFacturas
                                    ' Enviar archivos de facturas XSIG

                                    Dim sRutaYNombreArchivoBase64 As String
                                    sRutaYNombreArchivoBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(sRutaYNombreArchivo)) '.Replace("SP20192.xsig", "SP20192.xml_signed.xsig")))


                                    ' FacturaRequest
                                    Dim oFacturaRequest = Nothing
                                    ' FacturaResponse
                                    Dim oRespuesta = Nothing

                                    Dim oFACEServiceCertificado As X509Certificate2
                                    Dim sFACEServiceURL As String = "https://webservice.face.gob.es/facturasspp2"
                                    Dim sDnsIdentity As String

                                    ' PRUEBA
                                    If bPRUEBA Then
                                        sFACEServiceURL = "https://se-face-webservice.redsara.es/facturasspp2" ' Prueba
                                        oFACEServiceCertificado = New X509Certificate2(Path.Combine(frmMain.sApplicationPath, "SELLO-ENTIDAD-SGAD-PRUEBAS.crt"))
                                        sDnsIdentity = "SELLO ENTIDAD SGAD PRUEBAS"
                                    Else
                                        sFACEServiceURL = "https://webservice.face.gob.es/facturasspp2"
                                        oFACEServiceCertificado = New X509Certificate2(Path.Combine(frmMain.sApplicationPath, "SELLO-DE-ENTIDAD-SGAD.crt"))
                                        sDnsIdentity = "SELLO DE ENTIDAD SGAD"
                                    End If


                                    ' < .NET 4.5
                                    'ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 'TLS 1.2
                                    'ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 'TLS 1.1
                                    ' .NET 4.5
                                    'System.Net.ServicePointManager.Expect100Continue = True
                                    'System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 Or System.Net.SecurityProtocolType.Tls12 Or System.Net.SecurityProtocolType.Ssl3
                                    ''ByPass SSL Certificate Validation Checking
                                    'System.Net.ServicePointManager.ServerCertificateValidationCallback =
                                    '            Function(se As Object,
                                    '            cert As System.Security.Cryptography.X509Certificates.X509Certificate,
                                    '            chain As System.Security.Cryptography.X509Certificates.X509Chain,
                                    '            sslerror As System.Net.Security.SslPolicyErrors) True


                                    Dim bUseCustomBinding As Boolean = True

                                    If bUseCustomBinding Then
                                        ' ********** SERVICIO FACE MEDIANTE CHANNEL FACTORY - CUSTOM BINDING **********

                                        ' CUSTOM BINDING


                                        ' AÑADIR EL SIGUIENTE PARAMETRO A "ServiceContractAttribute"
                                        ' EN EL ARCHIVO REFERENCE.VB DE LA CARPETA DEL SERVICIO EN SERVICE REFERENCES
                                        ' ProtectionLevel := System.Net.Security.ProtectionLevel.Sign


                                        ' ACTUALIZAR CERTIFICADOS DEL SERVIDOR FACE EN
                                        ' https://administracionelectronica.gob.es/ctt/face/descargas
                                        ' "Documentación para integradores"
                                        ' Actualmente: 
                                        ' SELLO_ENTIDAD_SGAD_PRUUEBAS.crt : Fecha de modificación:  14/01/2019
                                        ' SELLO_DE_ENTIDAD_SGAD.crt:        Fecha de modificación:  11/03/2020

                                        Dim oFACEWSRespuesta = Nothing
                                        Dim oFACEWSFacturaRequest = Nothing
                                        Dim oFACEWSEstados = Nothing
                                        Dim oServicioFACE = Nothing

                                        If bPRUEBA Then
                                            ' FacturaRequest
                                            Dim oFACEWSFactura As New FACE_Gob_WS_pruebas.FacturaFile With {
                                                .factura = sRutaYNombreArchivoBase64,
                                                .nombre = sNombreArchivo,
                                                .mime = "application/xml"
                                            }
                                            Dim oFACEWSAnexo(1) As FACE_Gob_WS_pruebas.AnexoFile
                                            oFACEWSAnexo(0) = New FACE_Gob_WS_pruebas.AnexoFile With {
                                                .anexo = "",
                                                .nombre = "",
                                                .mime = ""
                                            }
                                            Dim oFACEWSFacturaRequest_pruebas As FACE_Gob_WS_pruebas.EnviarFacturaRequest
                                            oFACEWSFacturaRequest_pruebas = New FACE_Gob_WS_pruebas.EnviarFacturaRequest With {
                                                .correo = sFACEemail,
                                                .factura = oFACEWSFactura,
                                                .anexos = Nothing 'oFACEWSAnexo
                                            }
                                            oFACEWSFacturaRequest = oFACEWSFacturaRequest_pruebas

                                            oFACEWSRespuesta = New FACE_Gob_WS_pruebas.EnviarFacturaResponse

                                            oFACEWSEstados = New FACE_Gob_WS_pruebas.ConsultarEstadosResponse


                                            'CustomBinding for interop with FACE java web service
                                            Dim oCustomBinding As CustomBinding = New CustomBinding()

                                            ' Security
                                            Dim oSecurity = SecurityBindingElement.CreateMutualCertificateDuplexBindingElement(MessageSecurityVersion.WSSecurity10WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10)
                                            oSecurity.AllowSerializedSigningTokenOnReply = True
                                            oSecurity.MessageProtectionOrder = MessageProtectionOrder.SignBeforeEncrypt
                                            oSecurity.SecurityHeaderLayout = SecurityHeaderLayout.LaxTimestampLast
                                            oSecurity.IncludeTimestamp = False
                                            ' FaceB2B
                                            'oSecurity.DefaultAlgorithmSuite = New FaceB2BCustomAlgorithmSuite()

                                            ' Message
                                            Dim textBindingElement = New TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8)

                                            ' Transport
                                            Dim httpsTransport = New HttpsTransportBindingElement()
                                            httpsTransport.RequireClientCertificate = True

                                            ' Bind in order (Security layer, message layer, transport layer)
                                            oCustomBinding.Elements.Add(oSecurity)
                                            oCustomBinding.Elements.Add(textBindingElement)
                                            oCustomBinding.Elements.Add(httpsTransport)

                                            Dim oIdentity As EndpointIdentity = EndpointIdentity.CreateDnsIdentity(sDnsIdentity)
                                            Dim oEndpoint As EndpointAddress = New EndpointAddress(New Uri(sFACEServiceURL), oIdentity)

                                            'Dim oFactory As ChannelFactory(Of es.gob.face.webservice.FacturaSSPPWebServiceProxyService) = New ChannelFactory(Of es.gob.face.webservice.FacturaSSPPWebServiceProxyService)(oCustomBinding, oEndpoint)
                                            'Dim oFactory As ChannelFactory(Of es.gob.face.webservice.Service) = New ChannelFactory(Of es.gob.face.webservice.Service)(oCustomBinding, oEndpoint)
                                            Dim oFactory As ChannelFactory(Of FACE_Gob_WS_pruebas.FacturaSSPPWebServiceProxyPort) = New ChannelFactory(Of FACE_Gob_WS_pruebas.FacturaSSPPWebServiceProxyPort)(oCustomBinding, oEndpoint)

                                            oFactory.Credentials.ClientCertificate.Certificate = oCertificado
                                            oFactory.Credentials.ServiceCertificate.DefaultCertificate = oFACEServiceCertificado
                                            oFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None
                                            ' FaceB2B
                                            'oFactory.Endpoint.Behaviors.Add(New FaceB2BCustomEndpointBehavior())

                                            oServicioFACE = oFactory.CreateChannel()
                                        Else
                                            ' FacturaRequest
                                            Dim oFACEWSFactura As New FACE_Gob_WS.FacturaFile With {
                                                .factura = sRutaYNombreArchivoBase64,
                                                .nombre = sNombreArchivo,
                                                .mime = "application/xml"
                                            }
                                            Dim oFACEWSAnexo(1) As FACE_Gob_WS.AnexoFile
                                            oFACEWSAnexo(0) = New FACE_Gob_WS.AnexoFile With {
                                                .anexo = "",
                                                .nombre = "",
                                                .mime = ""
                                            }
                                            Dim oFACEWSFacturaRequest_produccion As FACE_Gob_WS.EnviarFacturaRequest
                                            oFACEWSFacturaRequest_produccion = New FACE_Gob_WS.EnviarFacturaRequest With {
                                                .correo = sFACEemail,
                                                .factura = oFACEWSFactura,
                                                .anexos = Nothing 'oFACEWSAnexo
                                            }
                                            oFACEWSFacturaRequest = oFACEWSFacturaRequest_produccion

                                            oFACEWSRespuesta = New FACE_Gob_WS.EnviarFacturaResponse

                                            oFACEWSEstados = New FACE_Gob_WS.ConsultarEstadosResponse

                                            'CustomBinding for interop with FACE java web service
                                            Dim oCustomBinding As CustomBinding = New CustomBinding()

                                            ' Security
                                            Dim oSecurity = SecurityBindingElement.CreateMutualCertificateDuplexBindingElement(MessageSecurityVersion.WSSecurity10WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10)
                                            oSecurity.AllowSerializedSigningTokenOnReply = True
                                            oSecurity.MessageProtectionOrder = MessageProtectionOrder.SignBeforeEncrypt
                                            oSecurity.SecurityHeaderLayout = SecurityHeaderLayout.LaxTimestampLast
                                            oSecurity.IncludeTimestamp = False
                                            ' FaceB2B
                                            'oSecurity.DefaultAlgorithmSuite = New FaceB2BCustomAlgorithmSuite()

                                            ' Message
                                            Dim textBindingElement = New TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8)

                                            ' Transport
                                            Dim httpsTransport = New HttpsTransportBindingElement()
                                            httpsTransport.RequireClientCertificate = True

                                            ' Bind in order (Security layer, message layer, transport layer)
                                            oCustomBinding.Elements.Add(oSecurity)
                                            oCustomBinding.Elements.Add(textBindingElement)
                                            oCustomBinding.Elements.Add(httpsTransport)

                                            Dim oIdentity As EndpointIdentity = EndpointIdentity.CreateDnsIdentity(sDnsIdentity)
                                            Dim oEndpoint As EndpointAddress = New EndpointAddress(New Uri(sFACEServiceURL), oIdentity)

                                            'Dim oFactory As ChannelFactory(Of es.gob.face.webservice.FacturaSSPPWebServiceProxyService) = New ChannelFactory(Of es.gob.face.webservice.FacturaSSPPWebServiceProxyService)(oCustomBinding, oEndpoint)
                                            'Dim oFactory As ChannelFactory(Of es.gob.face.webservice.Service) = New ChannelFactory(Of es.gob.face.webservice.Service)(oCustomBinding, oEndpoint)
                                            Dim oFactory As ChannelFactory(Of FACE_Gob_WS.FacturaSSPPWebServiceProxyPort) = New ChannelFactory(Of FACE_Gob_WS.FacturaSSPPWebServiceProxyPort)(oCustomBinding, oEndpoint)

                                            oFactory.Credentials.ClientCertificate.Certificate = oCertificado
                                            oFactory.Credentials.ServiceCertificate.DefaultCertificate = oFACEServiceCertificado
                                            oFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None
                                            ' FaceB2B
                                            'oFactory.Endpoint.Behaviors.Add(New FaceB2BCustomEndpointBehavior())

                                            oServicioFACE = oFactory.CreateChannel()
                                        End If


                                        'oServicioFACE.ClientCertificates.Add(oCertificado)
                                        'oServicioFACE.RequestEncoding = Encoding.UTF8
                                        'oServicioFACE.InitializeLifetimeService()

                                        'oFACEWSEstados = oServicioFACE.consultarEstados()

                                        '' Guardar respuesta como XML
                                        'sRutaYNombreArchivo = sRutaYNombreArchivo.Replace(".xsig", ".xml")
                                        'sRutaYNombreArchivo = sRutaYNombreArchivo.Replace(".xml", "_Estados.xml")
                                        'Dim oWriterResponse As TextWriter = New StreamWriter(sRutaYNombreArchivo)
                                        'Dim oSerializerResponse As New XmlSerializer(oFACEWSEstados.GetType())
                                        'oSerializerResponse.Serialize(oWriterResponse, oFACEWSEstados)
                                        'oWriterResponse.Close()

                                        oFacturaRequest = oFACEWSFacturaRequest

                                        ' Enviar solicitud y recibir respuesta
                                        Try
                                            oFACEWSRespuesta = oServicioFACE.enviarFactura(oFACEWSFacturaRequest)
                                            oRespuesta = oFACEWSRespuesta
                                        Catch ex As Exception
                                            MessageBox.Show("Ha ocurrido un error al tratar de conectar con el servidor al cual enviar la factura " & ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        End Try

                                    Else
                                        ' ********** SERVICIO FACE WCF PROXY **********

                                        ' FacturaRequest
                                        Dim oFACEFactura As New es.gob.face.webservice.FacturaFile With {
                                            .factura = sRutaYNombreArchivoBase64,
                                            .nombre = sNombreArchivo,
                                            .mime = "application/xml"
                                        }
                                        Dim oFACEAnexo(1) As es.gob.face.webservice.AnexoFile
                                        oFACEAnexo(0) = New es.gob.face.webservice.AnexoFile With {
                                            .anexo = "",
                                            .nombre = "",
                                            .mime = ""
                                        }
                                        Dim oFACEFacturaRequest As es.gob.face.webservice.EnviarFacturaRequest
                                        oFACEFacturaRequest = New es.gob.face.webservice.EnviarFacturaRequest With {
                                            .correo = sFACEemail,
                                            .factura = oFACEFactura,
                                            .anexos = Nothing 'oFACEAnexo
                                        }

                                        Dim oFACERespuesta As es.gob.face.webservice.EnviarFacturaResponse

                                        '' CABECERA SOAP
                                        '' Creamos los headers, necesarios para la petición SOAP
                                        ''Dim sphdr = TSOAPHEADER.create
                                        'Dim oAddressHeader As AddressHeader = AddressHeader.CreateAddressHeader("SessionType", sFACEServiceURL, "None")
                                        'Dim oAddressHeaders() As AddressHeader = {oAddressHeader}

                                        '' Endpoint address constructor with URI and address headers
                                        'Dim endpointAddressWithHeaders As New EndpointAddress(New Uri(sFACEServiceURL), oAddressHeaders)
                                        'Dim MyService As New es.gob.face.webservice.FacturaSSPPWebServiceProxyService(es.gob.face.webservice.FacturaSSPPWebServiceProxyService, endpointAddressWithHeaders)

                                        ' SERVICIO
                                        Dim oFACEServicioCliente As New es.gob.face.webservice.FacturaSSPPWebServiceProxyService
                                        'Dim oFACEServicioCliente As New es.gob.face.webservice.Service
                                        'Dim oFACEServicioCliente As New FACE_Gob_WS.FacturaSSPPWebServiceProxyPort

                                        'oFACEServicioCliente.UseDefaultCredentials = True
                                        oFACEServicioCliente.PreAuthenticate = True
                                        oFACEServicioCliente.Url = sFACEServiceURL
                                        'oFACEServicioCliente.Credentials = oCertificado.SubjectName
                                        oFACEServicioCliente.SoapVersion = Web.Services.Protocols.SoapProtocolVersion.Soap11
                                        oFACEServicioCliente.RequestEncoding = Encoding.UTF8

                                        oFACEServicioCliente.EnableDecompression = False

                                        oFACEServicioCliente.ClientCertificates.Add(oCertificado)
                                        oFACEServicioCliente.InitializeLifetimeService()

                                        ' Security
                                        'Dim oSecurity = SecurityBindingElement.CreateMutualCertificateDuplexBindingElement(MessageSecurityVersion.WSSecurity10WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10)
                                        'oSecurity.AllowSerializedSigningTokenOnReply = True
                                        'oSecurity.MessageProtectionOrder = MessageProtectionOrder.SignBeforeEncrypt
                                        'oSecurity.SecurityHeaderLayout = SecurityHeaderLayout.LaxTimestampLast
                                        'oSecurity.IncludeTimestamp = False

                                        'Dim oSecurityHeader As MessageHeader = MessageHeader.CreateHeader("Security", sFACEServiceURL, oSecurityToken, False)
                                        ''oSecurity.Headers.Add(oSecurityHeader)
                                        'oFACEServicioCliente.Headers.Add(oSecurityHeader)

                                        oFacturaRequest = oFACEFacturaRequest

                                        ' Enviar solicitud y recibir respuesta
                                        Try
                                            oFACERespuesta = oFACEServicioCliente.enviarFactura(oFACEFacturaRequest)
                                            oRespuesta = oFACERespuesta
                                        Catch ex As Exception
                                            MessageBox.Show("Ha ocurrido un error al tratar de conectar con el servidor al cual enviar la factura " & ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        End Try
                                    End If

                                    Try
                                        '' Guardar envio como XML
                                        Dim sRutaYNombreArchivo_tmp As String
                                        sRutaYNombreArchivo_tmp = sRutaYNombreArchivo.Replace(".xsig", "_Envio.xml")
                                        Dim oWriterResponse_tmp As TextWriter = New StreamWriter(sRutaYNombreArchivo_tmp)
                                        Dim oSerializerResponse_tmp As New XmlSerializer(oFacturaRequest.GetType(), "https//webservice.face.gob.es/facturasspp2?wsdl")
                                        oSerializerResponse_tmp.Serialize(oWriterResponse_tmp, oFacturaRequest)
                                        oWriterResponse_tmp.Close()
                                    Catch exc As Exception

                                    End Try


                                    If IsNothing(oRespuesta) Then
                                        MessageBox.Show("No se ha recibido respuesta del servidor", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Else
                                        ' Evaluar respuesta y guardar en base de datos
                                        sRutaYNombreArchivo = sRutaYNombreArchivo.Replace(".xsig", ".xml")
                                        sRutaYNombreArchivo = sRutaYNombreArchivo.Replace("_Estados.xml", ".xml")
                                        sRutaYNombreArchivo = sRutaYNombreArchivo.Replace(".xml", "_Respuesta.xml")
                                        Try
                                            ' Actualizar campos de base de datos en función de la respuesta
                                            Dim sFechaAhora As String = String.Format("{0:yyyyMMdd}", Now)
                                            Dim sFechaAhoraFormatted As String = String.Format("{0:dd/MM/yyyy}", Now)

                                            Dim nErroresCount As Integer = 0
                                            Dim sRespuestaError As String = ""
                                            Dim sRespuestaErrores As String = ""

                                            Dim sRespuestaInfo As String = ""

                                            Dim sRespuestaCodigo As String = ""
                                            Dim sRespuestaDescripcion As String = ""
                                            Dim sRespuestaCodigoSeguimiento As String = ""
                                            Dim sRespuestaFactura As String = ""
                                            Dim sRespuestaFacturaNumero As String = ""
                                            Dim sRespuestaFacturaSerie As String = ""

                                            Try
                                                sRespuestaCodigo = oRespuesta.resultado.codigo
                                            Catch ex As Exception

                                            End Try
                                            Try
                                                sRespuestaDescripcion = oRespuesta.resultado.descripcion
                                            Catch ex As Exception

                                            End Try
                                            Try
                                                sRespuestaCodigoSeguimiento = oRespuesta.resultado.codigoSeguimiento
                                            Catch ex As Exception

                                            End Try
                                            Try
                                                sRespuestaFactura = oRespuesta.factura
                                            Catch ex As Exception

                                            End Try
                                            Try
                                                sRespuestaFacturaNumero = oRespuesta.factura.numeroFactura
                                            Catch ex As Exception

                                            End Try
                                            Try
                                                sRespuestaFacturaSerie = oRespuesta.factura.serieFactura
                                            Catch ex As Exception

                                            End Try
                                            Dim sRespuestaFacturaAno As String = ""
                                            Dim bRespuestaFacturaFound As Boolean = False
                                            ' Formato de factura >> serie numerofactura/anofactura <<
                                            ' Dim rxPatron As String = "(\w+)\s+(\d+)/(\d{4})"
                                            Dim rxPatron As String = "(^[a-zA-Z]+)(\d{4})(\d+)(.*)"
                                            Dim rxExpresion As Regex = New Regex(rxPatron, RegexOptions.IgnoreCase)
                                            Dim rxCoincidencia As Match = rxExpresion.Match(sNombreArchivoFactura)
                                            If (rxCoincidencia.Success) Then
                                                sRespuestaFacturaSerie = rxCoincidencia.Groups(1).Value
                                                sRespuestaFacturaAno = rxCoincidencia.Groups(2).Value
                                                sRespuestaFacturaNumero = rxCoincidencia.Groups(3).Value
                                                bRespuestaFacturaFound = True
                                            Else
                                                ' No es un número de factura válido
                                                bRespuestaFacturaFound = False
                                                'Continue For
                                            End If
                                            sSQL = "SELECT NREGISTRO FROM PRESENTACIONFACTURACION WHERE NFACTURA=" & sRespuestaFacturaNumero & " AND ANOFACTURA=" & sRespuestaFacturaAno & " AND SERIE='" & sRespuestaFacturaSerie & "'"
                                            Dim nNREGISTRO As Long
                                            nNREGISTRO = frmMain.DameRegistro(sSQL, "NREGISTRO")
                                            If IsDBNull(nNREGISTRO) Then
                                                sRespuestaErrores = "Se ha producido un error y no se ha podido encontrar la factura en la base de datos para poder actualizar los datos de la presentación."
                                            Else

                                            End If
                                            Dim drCurrent As DataRow
                                            drCurrent = Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Rows.Find(nNREGISTRO)


                                            If sRespuestaCodigo = 0 Then
                                                ' OK
                                                sRespuestaInfo = sRespuestaDescripcion
                                                Try
                                                    drCurrent.BeginEdit()

                                                    drCurrent("FECHA_PRESENTACION_FACE") = sFechaAhora
                                                    'drCurrent("FECHA_ERROR_PRESENTACION_FACE") = DBNull.Value
                                                    'drCurrent("ERROR_PRESENTACION_FACE") = ""

                                                    Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_PRESENTACION_FACE_FORMAT").ReadOnly = False
                                                    drCurrent("FECHA_PRESENTACION_FACE_FORMAT") = sFechaAhoraFormatted
                                                    Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_PRESENTACION_FACE_FORMAT").ReadOnly = True

                                                    'Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_ERROR_FACE_FORMAT").ReadOnly = False
                                                    'drCurrent("FECHA_ERROR_FACE_FORMAT") = DBNull.Value
                                                    'Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_ERROR_FACE_FORMAT").ReadOnly = True

                                                    drCurrent.EndEdit()
                                                Catch exce As Exception

                                                End Try
                                            Else
                                                ' ERROR
                                                If sRespuestaErrores.Length > 0 Then
                                                    sRespuestaErrores &= vbCrLf
                                                End If
                                                sRespuestaErrores = sRespuestaDescripcion
                                                Try
                                                    drCurrent.BeginEdit()

                                                    drCurrent("FECHA_PRESENTACION_FACE") = DBNull.Value
                                                    drCurrent("FECHA_ERROR_PRESENTACION_FACE") = sFechaAhora
                                                    drCurrent("ERROR_PRESENTACION_FACE") = sRespuestaDescripcion

                                                    Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_PRESENTACION_FACE_FORMAT").ReadOnly = False
                                                    drCurrent("FECHA_PRESENTACION_FACE_FORMAT") = DBNull.Value
                                                    Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_PRESENTACION_FACE_FORMAT").ReadOnly = True

                                                    Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_ERROR_FACE_FORMAT").ReadOnly = False
                                                    drCurrent("FECHA_ERROR_FACE_FORMAT") = sFechaAhoraFormatted
                                                    Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION.Columns("FECHA_ERROR_FACE_FORMAT").ReadOnly = True

                                                    drCurrent.EndEdit()
                                                Catch exce As Exception

                                                End Try

                                            End If

                                            Try
                                                Me.Validate()
                                                Me.PRESENTACIONFACTURACIONBindingSource.EndEdit()
                                                Me.PRESENTACIONFACTURACIONTableAdapter.Update(Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION)
                                                sRespuestaInfo = vbCrLf & vbCrLf & "Se han actualizado los registros de la base de datos conforme a la respuesta del servicio." & vbCrLf & vbCrLf & sRespuestaInfo
                                                Me.dgvPRESENTACIONFACTURACION.Refresh()
                                            Catch exce As Exception
                                                sRespuestaErrores = vbCrLf & vbCrLf & "ERROR al actualizar los registros de la base de datos: " & exce.Message & vbCrLf & vbCrLf & sRespuestaErrores
                                            End Try

                                            If sRespuestaErrores.Length > 0 Then
                                                sRespuestaErrores = vbCrLf & vbCrLf & "Se han encontrado los siguientes errores (para más información, consulta el archivo XML de la respuesta):" & vbCrLf & vbCrLf & sRespuestaErrores
                                            Else
                                                sRespuestaInfo = vbCrLf & vbCrLf & "NO se han encontrado errores."
                                            End If

                                            Try
                                                Me.Validate()
                                                Me.PRESENTACIONFACTURACIONBindingSource.EndEdit()
                                                Me.PRESENTACIONFACTURACIONTableAdapter.Update(Me.DataSetPresentacionFacturacionListado.PRESENTACIONFACTURACION)
                                                sRespuestaInfo = vbCrLf & vbCrLf & "Se han actualizado los registros de la base de datos conforme a la respuesta del servicio." & vbCrLf & vbCrLf & sRespuestaInfo
                                                Me.dgvPRESENTACIONFACTURACION.Refresh()
                                            Catch exce As Exception
                                                sRespuestaErrores = vbCrLf & vbCrLf & "ERROR al actualizar los registros de la base de datos: " & exce.Message & vbCrLf & vbCrLf & sRespuestaErrores
                                            End Try

                                            If sRespuestaErrores.Length > 0 Then
                                                MessageBox.Show("Se ha procesado la presentación telemática y se ha generado el archivo XML de respuesta en " & sRutaYNombreArchivo & sRespuestaErrores, "Respuesta del servicio", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            Else
                                                MessageBox.Show("Se ha procesado la presentación telemática y se ha generado el archivo XML de respuesta en " & sRutaYNombreArchivo & sRespuestaInfo, "Respuesta del servicio", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                            End If

                                        Catch exc As Exception
                                            MessageBox.Show("No se ha podido evaluar la respuesta del servidor: " & exc.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        End Try


                                        ' Guardar respuesta como XML
                                        Try
                                            Dim oWriterResponse As TextWriter = New StreamWriter(sRutaYNombreArchivo)
                                            Dim oSerializerResponse As New XmlSerializer(oRespuesta.GetType())
                                            oSerializerResponse.Serialize(oWriterResponse, oRespuesta)
                                            oWriterResponse.Close()
                                        Catch exc As Exception
                                            MessageBox.Show("Se ha producido un error al guardar la respuesta del servidor de FACe tras enviar el archivo de factura " & sRutaYNombreArchivoFactura, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        End Try
                                    End If
                                End If
                            End If
                        Next
                    Else
                        MessageBox.Show("No se ha encontrado certificado o no se ha podido usar", "ERROR DE CERTIFICADO", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If
                MessageBox.Show("Proceso terminado", "FACE", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            sMessageText = "No hay registros para realizar la acción. Por favor cambia los filtros para que se muestren los registros que quieras cambiar."
            MessageBox.Show(sMessageText, "Generar XML", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    ' CARGAR CERTIFICADO SEGUN SELECCION
    ''' <summary>
    ''' Evalúa la selección del listado de certificados para comprobar si se ha seleccionado alguno del
    ''' repositorio de certificados del navegador, un archivo con el certificado o nada
    ''' </summary>
    ''' <returns>Devuelve el certificado seleccionado como objeto X509Certificate2</returns>
    Public Function DameCertificado() As X509Certificate2
        Dim oCertificado As Security.Cryptography.X509Certificates.X509Certificate2 = Nothing
        Dim nCertificadoSeleccionadoIndex As Integer = DirectCast(Me.CertificadosComboBox.SelectedItem, KeyValuePair(Of Integer, String)).Key
        Dim bExisteCertificado As Boolean = False
        If nCertificadoSeleccionadoIndex > -1 Then
            ' Carga certificado desde repositorio del navegador
            Dim repositorioCertificados As Security.Cryptography.X509Certificates.X509Store = New Security.Cryptography.X509Certificates.X509Store(Security.Cryptography.X509Certificates.StoreName.My) ', Security.Cryptography.X509Certificates.StoreLocation.LocalMachine)
            repositorioCertificados.Open(Security.Cryptography.X509Certificates.OpenFlags.ReadOnly)
            'miCertificado = New System.Security.Cryptography.X509Certificates.X509Certificate2
            oCertificado = repositorioCertificados.Certificates(nCertificadoSeleccionadoIndex)
            bExisteCertificado = True
        ElseIf nCertificadoSeleccionadoIndex = -1 Then
            ' Carga certificado desde archivo
            Dim sCertFile As String
            Dim sCertFileClave As String
            sCertFile = Me.CertificadoTextBox.Text
            sCertFileClave = Me.CertificadoClaveTextBox.Text
            If sCertFile.Length > 0 And File.Exists(sCertFile) And sCertFileClave.Length > 0 Then
                Try
                    oCertificado = New Security.Cryptography.X509Certificates.X509Certificate2(sCertFile, sCertFileClave)
                    bExisteCertificado = True
                Catch ex As Exception
                    MessageBox.Show("No se ha podido cargar el certificado '" & sCertFile & "'. Comprueba que se trate de un certificado correcto y que la contraseña sea la correspondiente", "ERROR DE CERTIFICADO", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End Try
            Else
                If sCertFile.Length = 0 Or Not File.Exists(sCertFile) Then
                    MessageBox.Show("No se ha encontrado el archivo seleccionado", "ERROR DE CERTIFICADO", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                If sCertFileClave.Length = 0 Then
                    MessageBox.Show("La contraseña del certificado no puede estar vacía", "ERROR DE CERTIFICADO", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        Else
            MessageBox.Show("No se ha seleccionado certificado", "ERROR DE CERTIFICADO", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        Return oCertificado
    End Function
    ''' <summary>
    ''' Firma una archivo XML de acuerdo a las especificaciones de FACe (Xades)
    ''' </summary>
    ''' <param name="oCertificate">Certificado con el que firmar el archivo XML</param>
    ''' <param name="xmlFile">Ruta y nombre del archivo XML que se va a firmar</param>
    Private Sub FirmarXMLFace(oCertificate As X509Certificate2, xmlFile As String)
        Dim oXadesService As XadesService = New XadesService()
        Dim oParametros As SignatureParameters = New SignatureParameters()

        Dim ficheroFactura As String = xmlFile

        ' Política de firma de factura-e 3.1
        oParametros.SignaturePolicyInfo = New SignaturePolicyInfo()
        oParametros.SignaturePolicyInfo.PolicyIdentifier = "http://www.facturae.es/politica_de_firma_formato_facturae/politica_de_firma_formato_facturae_v3_1.pdf"
        oParametros.SignaturePolicyInfo.PolicyHash = "Ohixl6upD6av8N7pEvDABhEL6hM="
        oParametros.SignaturePackaging = SignaturePackaging.ENVELOPED
        oParametros.DataFormat = New DataFormat()
        oParametros.DataFormat.MimeType = "text/xml"
        oParametros.SignerRole = New SignerRole()
        oParametros.SignerRole.ClaimedRoles.Add("emisor")

        oParametros.Signer = New Signer(oCertificate) 'Signer(CertUtil.SelectCertificate())
        Using fs As FileStream = New FileStream(ficheroFactura, FileMode.Open)
            Dim docFirmado = oXadesService.Sign(fs, oParametros)
            docFirmado.Save(xmlFile.Replace(".xml", ".xsig"))
        End Using
    End Sub
End Class
