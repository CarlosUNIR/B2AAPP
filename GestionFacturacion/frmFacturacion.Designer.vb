<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFacturacion
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFacturacion))
        Me.PresentarSIIButton = New System.Windows.Forms.Button()
        Me.dgvPRESENTACIONFACTURACION = New System.Windows.Forms.DataGridView()
        Me.PRESENTACIONFACTURACIONBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DataSetPresentacionFacturacionListado = New GestionFacturacion.DataSetPresentacionFacturacionListado()
        Me.DataSetPresentacionFacturacionListado1 = New GestionFacturacion.DataSetPresentacionFacturacionListado()
        Me.gbFiltro = New System.Windows.Forms.GroupBox()
        Me.SeriesFiltroComboBox = New System.Windows.Forms.ComboBox()
        Me.SERIESBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DataSetSeriesListado = New GestionFacturacion.DataSetSeriesListado()
        Me.SerieLabel = New System.Windows.Forms.Label()
        Me.ServicioTabControl = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.ErroresSIIFiltroLabel = New System.Windows.Forms.Label()
        Me.ErroresSIIFiltroCheckBox = New System.Windows.Forms.CheckBox()
        Me.SIIFiltroDelimiter2PictureBox = New System.Windows.Forms.PictureBox()
        Me.FechaPresentacionSIIFiltroClearButton = New System.Windows.Forms.Button()
        Me.FechaPresentacionSIIFiltroDesdeLabel = New System.Windows.Forms.Label()
        Me.FechaPresentacionSIIFiltroCheckBox = New System.Windows.Forms.CheckBox()
        Me.FechaPresentacionSIIFiltroHastaDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.FechaPresentacionSIIFiltroHastaLabel = New System.Windows.Forms.Label()
        Me.FechaPresentacionSIIFiltroDesdeDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.FechaPresentacionSIIFiltroLabel = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.FACEFiltroDelimiter2PictureBox = New System.Windows.Forms.PictureBox()
        Me.ErroresFACEFiltroLabel = New System.Windows.Forms.Label()
        Me.ErroresFACEFiltroCheckBox = New System.Windows.Forms.CheckBox()
        Me.FechaPresentacionFACEFiltroClearButton = New System.Windows.Forms.Button()
        Me.FechaPresentacionFACEFiltroDesdeLabel = New System.Windows.Forms.Label()
        Me.FechaPresentacionFACEFiltroCheckBox = New System.Windows.Forms.CheckBox()
        Me.FechaPresentacionFACEFiltroHastaDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.FechaPresentacionFACEFiltroHastaLabel = New System.Windows.Forms.Label()
        Me.FechaPresentacionFACEFiltroDesdeDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.FechaPresentacionFACEFiltroLabel = New System.Windows.Forms.Label()
        Me.AnoFlitroLabel = New System.Windows.Forms.Label()
        Me.PeriodoLabel = New System.Windows.Forms.Label()
        Me.PeriodosFiltroComboBox = New System.Windows.Forms.ComboBox()
        Me.PRESENTACIONFACTURACIONBindingSource3 = New System.Windows.Forms.BindingSource(Me.components)
        Me.DataSetPRESENTACIONFACTURACIONPeriodos = New GestionFacturacion.DataSetPRESENTACIONFACTURACIONPeriodos()
        Me.AnoFiltroDeleteButton = New System.Windows.Forms.Button()
        Me.AnoFiltroHastaDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.AnoFiltroHastaLabel = New System.Windows.Forms.Label()
        Me.AnoFiltroDesdeLabel = New System.Windows.Forms.Label()
        Me.AnoFiltroDesdeDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.ZonasFiltroComboBox = New System.Windows.Forms.ComboBox()
        Me.ZONASBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DataSetZONASListado = New GestionFacturacion.DataSetZONASListado()
        Me.FiltroZonasLabel = New System.Windows.Forms.Label()
        Me.PresentarFACEButton = New System.Windows.Forms.Button()
        Me.PRESENTACIONFACTURACIONTableAdapter2 = New GestionFacturacion.DataSetPRESENTACIONFACTURACIONPeriodosTableAdapters.PRESENTACIONFACTURACIONTableAdapter()
        Me.CertificadosComboBox = New System.Windows.Forms.ComboBox()
        Me.CertificadoTextBox = New System.Windows.Forms.TextBox()
        Me.CertificadoClaveTextBox = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.DEBUGCheckBox = New System.Windows.Forms.CheckBox()
        Me.SERIESTableAdapter = New GestionFacturacion.DataSetSeriesListadoTableAdapters.SERIESTableAdapter()
        Me.ZONASTableAdapter = New GestionFacturacion.DataSetZONASListadoTableAdapters.ZONASTableAdapter()
        Me.PRESENTACIONFACTURACIONBindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
        Me.PRESENTACIONFACTURACIONTableAdapter = New GestionFacturacion.DataSetPresentacionFacturacionListadoTableAdapters.PRESENTACIONFACTURACIONTableAdapter()
        Me.TableAdapterManager1 = New GestionFacturacion.DataSetPresentacionFacturacionListadoTableAdapters.TableAdapterManager()
        Me.PRESENTACIONFACTURACIONBindingSource2 = New System.Windows.Forms.BindingSource(Me.components)
        Me.gbCertificado = New System.Windows.Forms.GroupBox()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SERIE = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ZONA = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn24 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FECHA_FORMATTED = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NOMBREYAPELLIDOS = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NIF_CIF = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FECHA_ENVIO_SII_FORMATTED = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FECHA_PRESENTACION_SII_FORMAT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FECHA_ERROR_SII_FORMAT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ERROR_PRESENTACION_SII = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FECHA_PRESENTACION_FACE_FORMAT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FECHA_ERROR_FACE_FORMAT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FECHA_ERROR_PRESENTACION_FACE = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ERROR_PRESENTACION_FACE = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn70 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FINCA = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TOTAL = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TIPOPERIODO = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FECHA_PRESENTACION_SII = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ORGANO_GESTOR_FACE = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvPRESENTACIONFACTURACION, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PRESENTACIONFACTURACIONBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataSetPresentacionFacturacionListado, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataSetPresentacionFacturacionListado1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbFiltro.SuspendLayout()
        CType(Me.SERIESBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataSetSeriesListado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ServicioTabControl.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.SIIFiltroDelimiter2PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        CType(Me.FACEFiltroDelimiter2PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PRESENTACIONFACTURACIONBindingSource3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataSetPRESENTACIONFACTURACIONPeriodos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ZONASBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataSetZONASListado, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PRESENTACIONFACTURACIONBindingSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PRESENTACIONFACTURACIONBindingSource2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbCertificado.SuspendLayout()
        Me.SuspendLayout()
        '
        'PresentarSIIButton
        '
        Me.PresentarSIIButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PresentarSIIButton.Location = New System.Drawing.Point(710, 12)
        Me.PresentarSIIButton.Name = "PresentarSIIButton"
        Me.PresentarSIIButton.Size = New System.Drawing.Size(130, 28)
        Me.PresentarSIIButton.TabIndex = 2
        Me.PresentarSIIButton.Text = "Presentar SII"
        Me.PresentarSIIButton.UseVisualStyleBackColor = True
        '
        'dgvPRESENTACIONFACTURACION
        '
        Me.dgvPRESENTACIONFACTURACION.AllowUserToAddRows = False
        Me.dgvPRESENTACIONFACTURACION.AllowUserToDeleteRows = False
        Me.dgvPRESENTACIONFACTURACION.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvPRESENTACIONFACTURACION.AutoGenerateColumns = False
        Me.dgvPRESENTACIONFACTURACION.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPRESENTACIONFACTURACION.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.SERIE, Me.DataGridViewTextBoxColumn5, Me.DataGridViewTextBoxColumn6, Me.DataGridViewTextBoxColumn2, Me.ZONA, Me.DataGridViewTextBoxColumn24, Me.FECHA_FORMATTED, Me.NOMBREYAPELLIDOS, Me.NIF_CIF, Me.FECHA_ENVIO_SII_FORMATTED, Me.FECHA_PRESENTACION_SII_FORMAT, Me.FECHA_ERROR_SII_FORMAT, Me.ERROR_PRESENTACION_SII, Me.FECHA_PRESENTACION_FACE_FORMAT, Me.FECHA_ERROR_FACE_FORMAT, Me.FECHA_ERROR_PRESENTACION_FACE, Me.ERROR_PRESENTACION_FACE, Me.DataGridViewTextBoxColumn7, Me.DataGridViewTextBoxColumn70, Me.FINCA, Me.TOTAL, Me.TIPOPERIODO, Me.FECHA_PRESENTACION_SII, Me.ORGANO_GESTOR_FACE})
        Me.dgvPRESENTACIONFACTURACION.DataSource = Me.PRESENTACIONFACTURACIONBindingSource
        Me.dgvPRESENTACIONFACTURACION.Location = New System.Drawing.Point(12, 206)
        Me.dgvPRESENTACIONFACTURACION.Name = "dgvPRESENTACIONFACTURACION"
        Me.dgvPRESENTACIONFACTURACION.ReadOnly = True
        Me.dgvPRESENTACIONFACTURACION.Size = New System.Drawing.Size(828, 343)
        Me.dgvPRESENTACIONFACTURACION.TabIndex = 3
        '
        'PRESENTACIONFACTURACIONBindingSource
        '
        Me.PRESENTACIONFACTURACIONBindingSource.DataMember = "PRESENTACIONFACTURACION"
        Me.PRESENTACIONFACTURACIONBindingSource.DataSource = Me.DataSetPresentacionFacturacionListado
        '
        'DataSetPresentacionFacturacionListado
        '
        Me.DataSetPresentacionFacturacionListado.DataSetName = "DataSetPresentacionFacturacionListado"
        Me.DataSetPresentacionFacturacionListado.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'DataSetPresentacionFacturacionListado1
        '
        Me.DataSetPresentacionFacturacionListado1.DataSetName = "DataSetPresentacionFacturacionListado"
        Me.DataSetPresentacionFacturacionListado1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'gbFiltro
        '
        Me.gbFiltro.Controls.Add(Me.SeriesFiltroComboBox)
        Me.gbFiltro.Controls.Add(Me.SerieLabel)
        Me.gbFiltro.Controls.Add(Me.ServicioTabControl)
        Me.gbFiltro.Controls.Add(Me.AnoFlitroLabel)
        Me.gbFiltro.Controls.Add(Me.PeriodoLabel)
        Me.gbFiltro.Controls.Add(Me.PeriodosFiltroComboBox)
        Me.gbFiltro.Controls.Add(Me.AnoFiltroDeleteButton)
        Me.gbFiltro.Controls.Add(Me.AnoFiltroHastaDateTimePicker)
        Me.gbFiltro.Controls.Add(Me.AnoFiltroHastaLabel)
        Me.gbFiltro.Controls.Add(Me.AnoFiltroDesdeLabel)
        Me.gbFiltro.Controls.Add(Me.AnoFiltroDesdeDateTimePicker)
        Me.gbFiltro.Controls.Add(Me.ZonasFiltroComboBox)
        Me.gbFiltro.Controls.Add(Me.FiltroZonasLabel)
        Me.gbFiltro.Location = New System.Drawing.Point(12, 12)
        Me.gbFiltro.Name = "gbFiltro"
        Me.gbFiltro.Size = New System.Drawing.Size(630, 188)
        Me.gbFiltro.TabIndex = 14
        Me.gbFiltro.TabStop = False
        Me.gbFiltro.Text = "Filtro"
        '
        'SeriesFiltroComboBox
        '
        Me.SeriesFiltroComboBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SeriesFiltroComboBox.DataSource = Me.SERIESBindingSource
        Me.SeriesFiltroComboBox.DisplayMember = "SERIE"
        Me.SeriesFiltroComboBox.FormattingEnabled = True
        Me.SeriesFiltroComboBox.Location = New System.Drawing.Point(55, 93)
        Me.SeriesFiltroComboBox.Name = "SeriesFiltroComboBox"
        Me.SeriesFiltroComboBox.Size = New System.Drawing.Size(206, 21)
        Me.SeriesFiltroComboBox.TabIndex = 31
        Me.SeriesFiltroComboBox.ValueMember = "SERIE"
        '
        'SERIESBindingSource
        '
        Me.SERIESBindingSource.DataMember = "SERIES"
        Me.SERIESBindingSource.DataSource = Me.DataSetSeriesListado
        '
        'DataSetSeriesListado
        '
        Me.DataSetSeriesListado.DataSetName = "DataSetSeriesListado"
        Me.DataSetSeriesListado.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'SerieLabel
        '
        Me.SerieLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SerieLabel.AutoSize = True
        Me.SerieLabel.Location = New System.Drawing.Point(6, 96)
        Me.SerieLabel.Name = "SerieLabel"
        Me.SerieLabel.Size = New System.Drawing.Size(34, 13)
        Me.SerieLabel.TabIndex = 32
        Me.SerieLabel.Text = "Serie:"
        '
        'ServicioTabControl
        '
        Me.ServicioTabControl.Controls.Add(Me.TabPage1)
        Me.ServicioTabControl.Controls.Add(Me.TabPage2)
        Me.ServicioTabControl.Location = New System.Drawing.Point(287, 14)
        Me.ServicioTabControl.Name = "ServicioTabControl"
        Me.ServicioTabControl.SelectedIndex = 0
        Me.ServicioTabControl.Size = New System.Drawing.Size(337, 168)
        Me.ServicioTabControl.TabIndex = 30
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.ErroresSIIFiltroLabel)
        Me.TabPage1.Controls.Add(Me.ErroresSIIFiltroCheckBox)
        Me.TabPage1.Controls.Add(Me.SIIFiltroDelimiter2PictureBox)
        Me.TabPage1.Controls.Add(Me.FechaPresentacionSIIFiltroClearButton)
        Me.TabPage1.Controls.Add(Me.FechaPresentacionSIIFiltroDesdeLabel)
        Me.TabPage1.Controls.Add(Me.FechaPresentacionSIIFiltroCheckBox)
        Me.TabPage1.Controls.Add(Me.FechaPresentacionSIIFiltroHastaDateTimePicker)
        Me.TabPage1.Controls.Add(Me.FechaPresentacionSIIFiltroHastaLabel)
        Me.TabPage1.Controls.Add(Me.FechaPresentacionSIIFiltroDesdeDateTimePicker)
        Me.TabPage1.Controls.Add(Me.FechaPresentacionSIIFiltroLabel)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(329, 142)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "SII"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'ErroresSIIFiltroLabel
        '
        Me.ErroresSIIFiltroLabel.AutoSize = True
        Me.ErroresSIIFiltroLabel.Location = New System.Drawing.Point(6, 108)
        Me.ErroresSIIFiltroLabel.Name = "ErroresSIIFiltroLabel"
        Me.ErroresSIIFiltroLabel.Size = New System.Drawing.Size(40, 13)
        Me.ErroresSIIFiltroLabel.TabIndex = 32
        Me.ErroresSIIFiltroLabel.Text = "Errores"
        '
        'ErroresSIIFiltroCheckBox
        '
        Me.ErroresSIIFiltroCheckBox.AutoSize = True
        Me.ErroresSIIFiltroCheckBox.Location = New System.Drawing.Point(123, 108)
        Me.ErroresSIIFiltroCheckBox.Name = "ErroresSIIFiltroCheckBox"
        Me.ErroresSIIFiltroCheckBox.Size = New System.Drawing.Size(79, 17)
        Me.ErroresSIIFiltroCheckBox.TabIndex = 31
        Me.ErroresSIIFiltroCheckBox.Text = "con errores"
        Me.ErroresSIIFiltroCheckBox.UseVisualStyleBackColor = True
        '
        'SIIFiltroDelimiter2PictureBox
        '
        Me.SIIFiltroDelimiter2PictureBox.BackColor = System.Drawing.Color.DimGray
        Me.SIIFiltroDelimiter2PictureBox.Location = New System.Drawing.Point(3, 88)
        Me.SIIFiltroDelimiter2PictureBox.Name = "SIIFiltroDelimiter2PictureBox"
        Me.SIIFiltroDelimiter2PictureBox.Size = New System.Drawing.Size(320, 1)
        Me.SIIFiltroDelimiter2PictureBox.TabIndex = 30
        Me.SIIFiltroDelimiter2PictureBox.TabStop = False
        '
        'FechaPresentacionSIIFiltroClearButton
        '
        Me.FechaPresentacionSIIFiltroClearButton.AutoSize = True
        Me.FechaPresentacionSIIFiltroClearButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FechaPresentacionSIIFiltroClearButton.Image = CType(resources.GetObject("FechaPresentacionSIIFiltroClearButton.Image"), System.Drawing.Image)
        Me.FechaPresentacionSIIFiltroClearButton.Location = New System.Drawing.Point(300, 48)
        Me.FechaPresentacionSIIFiltroClearButton.Name = "FechaPresentacionSIIFiltroClearButton"
        Me.FechaPresentacionSIIFiltroClearButton.Size = New System.Drawing.Size(22, 22)
        Me.FechaPresentacionSIIFiltroClearButton.TabIndex = 26
        Me.FechaPresentacionSIIFiltroClearButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.FechaPresentacionSIIFiltroClearButton.UseVisualStyleBackColor = True
        '
        'FechaPresentacionSIIFiltroDesdeLabel
        '
        Me.FechaPresentacionSIIFiltroDesdeLabel.AutoSize = True
        Me.FechaPresentacionSIIFiltroDesdeLabel.Location = New System.Drawing.Point(7, 57)
        Me.FechaPresentacionSIIFiltroDesdeLabel.Name = "FechaPresentacionSIIFiltroDesdeLabel"
        Me.FechaPresentacionSIIFiltroDesdeLabel.Size = New System.Drawing.Size(39, 13)
        Me.FechaPresentacionSIIFiltroDesdeLabel.TabIndex = 28
        Me.FechaPresentacionSIIFiltroDesdeLabel.Text = "desde:"
        '
        'FechaPresentacionSIIFiltroCheckBox
        '
        Me.FechaPresentacionSIIFiltroCheckBox.AutoSize = True
        Me.FechaPresentacionSIIFiltroCheckBox.Location = New System.Drawing.Point(123, 13)
        Me.FechaPresentacionSIIFiltroCheckBox.Name = "FechaPresentacionSIIFiltroCheckBox"
        Me.FechaPresentacionSIIFiltroCheckBox.Size = New System.Drawing.Size(148, 17)
        Me.FechaPresentacionSIIFiltroCheckBox.TabIndex = 27
        Me.FechaPresentacionSIIFiltroCheckBox.Text = "sin fecha de presentación"
        Me.FechaPresentacionSIIFiltroCheckBox.UseVisualStyleBackColor = True
        '
        'FechaPresentacionSIIFiltroHastaDateTimePicker
        '
        Me.FechaPresentacionSIIFiltroHastaDateTimePicker.CustomFormat = "yyyy"
        Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Location = New System.Drawing.Point(197, 51)
        Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Name = "FechaPresentacionSIIFiltroHastaDateTimePicker"
        Me.FechaPresentacionSIIFiltroHastaDateTimePicker.Size = New System.Drawing.Size(97, 20)
        Me.FechaPresentacionSIIFiltroHastaDateTimePicker.TabIndex = 25
        '
        'FechaPresentacionSIIFiltroHastaLabel
        '
        Me.FechaPresentacionSIIFiltroHastaLabel.AutoSize = True
        Me.FechaPresentacionSIIFiltroHastaLabel.Location = New System.Drawing.Point(157, 57)
        Me.FechaPresentacionSIIFiltroHastaLabel.Name = "FechaPresentacionSIIFiltroHastaLabel"
        Me.FechaPresentacionSIIFiltroHastaLabel.Size = New System.Drawing.Size(36, 13)
        Me.FechaPresentacionSIIFiltroHastaLabel.TabIndex = 24
        Me.FechaPresentacionSIIFiltroHastaLabel.Text = "hasta:"
        '
        'FechaPresentacionSIIFiltroDesdeDateTimePicker
        '
        Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.CustomFormat = "yyyy"
        Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Location = New System.Drawing.Point(52, 51)
        Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Name = "FechaPresentacionSIIFiltroDesdeDateTimePicker"
        Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.Size = New System.Drawing.Size(100, 20)
        Me.FechaPresentacionSIIFiltroDesdeDateTimePicker.TabIndex = 23
        '
        'FechaPresentacionSIIFiltroLabel
        '
        Me.FechaPresentacionSIIFiltroLabel.AutoSize = True
        Me.FechaPresentacionSIIFiltroLabel.Location = New System.Drawing.Point(6, 14)
        Me.FechaPresentacionSIIFiltroLabel.Name = "FechaPresentacionSIIFiltroLabel"
        Me.FechaPresentacionSIIFiltroLabel.Size = New System.Drawing.Size(69, 13)
        Me.FechaPresentacionSIIFiltroLabel.TabIndex = 22
        Me.FechaPresentacionSIIFiltroLabel.Text = "Presentación"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.FACEFiltroDelimiter2PictureBox)
        Me.TabPage2.Controls.Add(Me.ErroresFACEFiltroLabel)
        Me.TabPage2.Controls.Add(Me.ErroresFACEFiltroCheckBox)
        Me.TabPage2.Controls.Add(Me.FechaPresentacionFACEFiltroClearButton)
        Me.TabPage2.Controls.Add(Me.FechaPresentacionFACEFiltroDesdeLabel)
        Me.TabPage2.Controls.Add(Me.FechaPresentacionFACEFiltroCheckBox)
        Me.TabPage2.Controls.Add(Me.FechaPresentacionFACEFiltroHastaDateTimePicker)
        Me.TabPage2.Controls.Add(Me.FechaPresentacionFACEFiltroHastaLabel)
        Me.TabPage2.Controls.Add(Me.FechaPresentacionFACEFiltroDesdeDateTimePicker)
        Me.TabPage2.Controls.Add(Me.FechaPresentacionFACEFiltroLabel)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(329, 142)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "FACE"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'FACEFiltroDelimiter2PictureBox
        '
        Me.FACEFiltroDelimiter2PictureBox.BackColor = System.Drawing.Color.DimGray
        Me.FACEFiltroDelimiter2PictureBox.Location = New System.Drawing.Point(3, 88)
        Me.FACEFiltroDelimiter2PictureBox.Name = "FACEFiltroDelimiter2PictureBox"
        Me.FACEFiltroDelimiter2PictureBox.Size = New System.Drawing.Size(320, 1)
        Me.FACEFiltroDelimiter2PictureBox.TabIndex = 50
        Me.FACEFiltroDelimiter2PictureBox.TabStop = False
        '
        'ErroresFACEFiltroLabel
        '
        Me.ErroresFACEFiltroLabel.AutoSize = True
        Me.ErroresFACEFiltroLabel.Location = New System.Drawing.Point(6, 108)
        Me.ErroresFACEFiltroLabel.Name = "ErroresFACEFiltroLabel"
        Me.ErroresFACEFiltroLabel.Size = New System.Drawing.Size(40, 13)
        Me.ErroresFACEFiltroLabel.TabIndex = 49
        Me.ErroresFACEFiltroLabel.Text = "Errores"
        '
        'ErroresFACEFiltroCheckBox
        '
        Me.ErroresFACEFiltroCheckBox.AutoSize = True
        Me.ErroresFACEFiltroCheckBox.Location = New System.Drawing.Point(123, 108)
        Me.ErroresFACEFiltroCheckBox.Name = "ErroresFACEFiltroCheckBox"
        Me.ErroresFACEFiltroCheckBox.Size = New System.Drawing.Size(79, 17)
        Me.ErroresFACEFiltroCheckBox.TabIndex = 48
        Me.ErroresFACEFiltroCheckBox.Text = "con errores"
        Me.ErroresFACEFiltroCheckBox.UseVisualStyleBackColor = True
        '
        'FechaPresentacionFACEFiltroClearButton
        '
        Me.FechaPresentacionFACEFiltroClearButton.AutoSize = True
        Me.FechaPresentacionFACEFiltroClearButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FechaPresentacionFACEFiltroClearButton.Image = CType(resources.GetObject("FechaPresentacionFACEFiltroClearButton.Image"), System.Drawing.Image)
        Me.FechaPresentacionFACEFiltroClearButton.Location = New System.Drawing.Point(300, 48)
        Me.FechaPresentacionFACEFiltroClearButton.Name = "FechaPresentacionFACEFiltroClearButton"
        Me.FechaPresentacionFACEFiltroClearButton.Size = New System.Drawing.Size(22, 22)
        Me.FechaPresentacionFACEFiltroClearButton.TabIndex = 44
        Me.FechaPresentacionFACEFiltroClearButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.FechaPresentacionFACEFiltroClearButton.UseVisualStyleBackColor = True
        '
        'FechaPresentacionFACEFiltroDesdeLabel
        '
        Me.FechaPresentacionFACEFiltroDesdeLabel.AutoSize = True
        Me.FechaPresentacionFACEFiltroDesdeLabel.Location = New System.Drawing.Point(7, 57)
        Me.FechaPresentacionFACEFiltroDesdeLabel.Name = "FechaPresentacionFACEFiltroDesdeLabel"
        Me.FechaPresentacionFACEFiltroDesdeLabel.Size = New System.Drawing.Size(39, 13)
        Me.FechaPresentacionFACEFiltroDesdeLabel.TabIndex = 46
        Me.FechaPresentacionFACEFiltroDesdeLabel.Text = "desde:"
        '
        'FechaPresentacionFACEFiltroCheckBox
        '
        Me.FechaPresentacionFACEFiltroCheckBox.AutoSize = True
        Me.FechaPresentacionFACEFiltroCheckBox.Location = New System.Drawing.Point(123, 13)
        Me.FechaPresentacionFACEFiltroCheckBox.Name = "FechaPresentacionFACEFiltroCheckBox"
        Me.FechaPresentacionFACEFiltroCheckBox.Size = New System.Drawing.Size(148, 17)
        Me.FechaPresentacionFACEFiltroCheckBox.TabIndex = 45
        Me.FechaPresentacionFACEFiltroCheckBox.Text = "sin fecha de presentación"
        Me.FechaPresentacionFACEFiltroCheckBox.UseVisualStyleBackColor = True
        '
        'FechaPresentacionFACEFiltroHastaDateTimePicker
        '
        Me.FechaPresentacionFACEFiltroHastaDateTimePicker.CustomFormat = "yyyy"
        Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Location = New System.Drawing.Point(197, 51)
        Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Name = "FechaPresentacionFACEFiltroHastaDateTimePicker"
        Me.FechaPresentacionFACEFiltroHastaDateTimePicker.Size = New System.Drawing.Size(97, 20)
        Me.FechaPresentacionFACEFiltroHastaDateTimePicker.TabIndex = 43
        '
        'FechaPresentacionFACEFiltroHastaLabel
        '
        Me.FechaPresentacionFACEFiltroHastaLabel.AutoSize = True
        Me.FechaPresentacionFACEFiltroHastaLabel.Location = New System.Drawing.Point(157, 57)
        Me.FechaPresentacionFACEFiltroHastaLabel.Name = "FechaPresentacionFACEFiltroHastaLabel"
        Me.FechaPresentacionFACEFiltroHastaLabel.Size = New System.Drawing.Size(36, 13)
        Me.FechaPresentacionFACEFiltroHastaLabel.TabIndex = 42
        Me.FechaPresentacionFACEFiltroHastaLabel.Text = "hasta:"
        '
        'FechaPresentacionFACEFiltroDesdeDateTimePicker
        '
        Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.CustomFormat = "yyyy"
        Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Location = New System.Drawing.Point(52, 51)
        Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Name = "FechaPresentacionFACEFiltroDesdeDateTimePicker"
        Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.Size = New System.Drawing.Size(100, 20)
        Me.FechaPresentacionFACEFiltroDesdeDateTimePicker.TabIndex = 41
        '
        'FechaPresentacionFACEFiltroLabel
        '
        Me.FechaPresentacionFACEFiltroLabel.AutoSize = True
        Me.FechaPresentacionFACEFiltroLabel.Location = New System.Drawing.Point(6, 14)
        Me.FechaPresentacionFACEFiltroLabel.Name = "FechaPresentacionFACEFiltroLabel"
        Me.FechaPresentacionFACEFiltroLabel.Size = New System.Drawing.Size(69, 13)
        Me.FechaPresentacionFACEFiltroLabel.TabIndex = 40
        Me.FechaPresentacionFACEFiltroLabel.Text = "Presentación"
        '
        'AnoFlitroLabel
        '
        Me.AnoFlitroLabel.AutoSize = True
        Me.AnoFlitroLabel.Location = New System.Drawing.Point(6, 25)
        Me.AnoFlitroLabel.Name = "AnoFlitroLabel"
        Me.AnoFlitroLabel.Size = New System.Drawing.Size(47, 13)
        Me.AnoFlitroLabel.TabIndex = 21
        Me.AnoFlitroLabel.Text = "Ejercicio"
        '
        'PeriodoLabel
        '
        Me.PeriodoLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PeriodoLabel.AutoSize = True
        Me.PeriodoLabel.Location = New System.Drawing.Point(6, 159)
        Me.PeriodoLabel.Name = "PeriodoLabel"
        Me.PeriodoLabel.Size = New System.Drawing.Size(43, 13)
        Me.PeriodoLabel.TabIndex = 13
        Me.PeriodoLabel.Text = "Periodo"
        '
        'PeriodosFiltroComboBox
        '
        Me.PeriodosFiltroComboBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PeriodosFiltroComboBox.DataSource = Me.PRESENTACIONFACTURACIONBindingSource3
        Me.PeriodosFiltroComboBox.DisplayMember = "PERIODO"
        Me.PeriodosFiltroComboBox.FormattingEnabled = True
        Me.PeriodosFiltroComboBox.Location = New System.Drawing.Point(55, 156)
        Me.PeriodosFiltroComboBox.Name = "PeriodosFiltroComboBox"
        Me.PeriodosFiltroComboBox.Size = New System.Drawing.Size(206, 21)
        Me.PeriodosFiltroComboBox.TabIndex = 12
        Me.PeriodosFiltroComboBox.ValueMember = "PERIODO"
        '
        'PRESENTACIONFACTURACIONBindingSource3
        '
        Me.PRESENTACIONFACTURACIONBindingSource3.DataMember = "PRESENTACIONFACTURACION"
        Me.PRESENTACIONFACTURACIONBindingSource3.DataSource = Me.DataSetPRESENTACIONFACTURACIONPeriodos
        '
        'DataSetPRESENTACIONFACTURACIONPeriodos
        '
        Me.DataSetPRESENTACIONFACTURACIONPeriodos.DataSetName = "DataSetPRESENTACIONFACTURACIONPeriodos"
        Me.DataSetPRESENTACIONFACTURACIONPeriodos.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'AnoFiltroDeleteButton
        '
        Me.AnoFiltroDeleteButton.AutoSize = True
        Me.AnoFiltroDeleteButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.AnoFiltroDeleteButton.Image = CType(resources.GetObject("AnoFiltroDeleteButton.Image"), System.Drawing.Image)
        Me.AnoFiltroDeleteButton.Location = New System.Drawing.Point(259, 41)
        Me.AnoFiltroDeleteButton.Name = "AnoFiltroDeleteButton"
        Me.AnoFiltroDeleteButton.Size = New System.Drawing.Size(22, 22)
        Me.AnoFiltroDeleteButton.TabIndex = 11
        Me.AnoFiltroDeleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.AnoFiltroDeleteButton.UseVisualStyleBackColor = True
        '
        'AnoFiltroHastaDateTimePicker
        '
        Me.AnoFiltroHastaDateTimePicker.CustomFormat = "yyyy"
        Me.AnoFiltroHastaDateTimePicker.Location = New System.Drawing.Point(178, 43)
        Me.AnoFiltroHastaDateTimePicker.Name = "AnoFiltroHastaDateTimePicker"
        Me.AnoFiltroHastaDateTimePicker.Size = New System.Drawing.Size(75, 20)
        Me.AnoFiltroHastaDateTimePicker.TabIndex = 8
        '
        'AnoFiltroHastaLabel
        '
        Me.AnoFiltroHastaLabel.AutoSize = True
        Me.AnoFiltroHastaLabel.Location = New System.Drawing.Point(136, 48)
        Me.AnoFiltroHastaLabel.Name = "AnoFiltroHastaLabel"
        Me.AnoFiltroHastaLabel.Size = New System.Drawing.Size(36, 13)
        Me.AnoFiltroHastaLabel.TabIndex = 7
        Me.AnoFiltroHastaLabel.Text = "hasta:"
        '
        'AnoFiltroDesdeLabel
        '
        Me.AnoFiltroDesdeLabel.AutoSize = True
        Me.AnoFiltroDesdeLabel.Location = New System.Drawing.Point(10, 49)
        Me.AnoFiltroDesdeLabel.Name = "AnoFiltroDesdeLabel"
        Me.AnoFiltroDesdeLabel.Size = New System.Drawing.Size(39, 13)
        Me.AnoFiltroDesdeLabel.TabIndex = 6
        Me.AnoFiltroDesdeLabel.Text = "desde:"
        '
        'AnoFiltroDesdeDateTimePicker
        '
        Me.AnoFiltroDesdeDateTimePicker.CustomFormat = "yyyy"
        Me.AnoFiltroDesdeDateTimePicker.Location = New System.Drawing.Point(55, 43)
        Me.AnoFiltroDesdeDateTimePicker.Name = "AnoFiltroDesdeDateTimePicker"
        Me.AnoFiltroDesdeDateTimePicker.Size = New System.Drawing.Size(75, 20)
        Me.AnoFiltroDesdeDateTimePicker.TabIndex = 5
        '
        'ZonasFiltroComboBox
        '
        Me.ZonasFiltroComboBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ZonasFiltroComboBox.DataSource = Me.ZONASBindingSource
        Me.ZonasFiltroComboBox.DisplayMember = "DESCRIPCION"
        Me.ZonasFiltroComboBox.FormattingEnabled = True
        Me.ZonasFiltroComboBox.Location = New System.Drawing.Point(55, 124)
        Me.ZonasFiltroComboBox.Name = "ZonasFiltroComboBox"
        Me.ZonasFiltroComboBox.Size = New System.Drawing.Size(206, 21)
        Me.ZonasFiltroComboBox.TabIndex = 3
        Me.ZonasFiltroComboBox.ValueMember = "CODIGOZONA"
        '
        'ZONASBindingSource
        '
        Me.ZONASBindingSource.DataMember = "ZONAS"
        Me.ZONASBindingSource.DataSource = Me.DataSetZONASListado
        '
        'DataSetZONASListado
        '
        Me.DataSetZONASListado.DataSetName = "DataSetZONASListado"
        Me.DataSetZONASListado.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'FiltroZonasLabel
        '
        Me.FiltroZonasLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FiltroZonasLabel.AutoSize = True
        Me.FiltroZonasLabel.Location = New System.Drawing.Point(6, 127)
        Me.FiltroZonasLabel.Name = "FiltroZonasLabel"
        Me.FiltroZonasLabel.Size = New System.Drawing.Size(35, 13)
        Me.FiltroZonasLabel.TabIndex = 4
        Me.FiltroZonasLabel.Text = "Zona:"
        '
        'PresentarFACEButton
        '
        Me.PresentarFACEButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PresentarFACEButton.Location = New System.Drawing.Point(710, 46)
        Me.PresentarFACEButton.Name = "PresentarFACEButton"
        Me.PresentarFACEButton.Size = New System.Drawing.Size(130, 26)
        Me.PresentarFACEButton.TabIndex = 15
        Me.PresentarFACEButton.Text = "Presentar FACE"
        Me.PresentarFACEButton.UseVisualStyleBackColor = True
        '
        'PRESENTACIONFACTURACIONTableAdapter2
        '
        Me.PRESENTACIONFACTURACIONTableAdapter2.ClearBeforeFill = True
        '
        'CertificadosComboBox
        '
        Me.CertificadosComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CertificadosComboBox.FormattingEnabled = True
        Me.CertificadosComboBox.Location = New System.Drawing.Point(6, 19)
        Me.CertificadosComboBox.MaximumSize = New System.Drawing.Size(500, 0)
        Me.CertificadosComboBox.Name = "CertificadosComboBox"
        Me.CertificadosComboBox.Size = New System.Drawing.Size(180, 21)
        Me.CertificadosComboBox.TabIndex = 16
        '
        'CertificadoTextBox
        '
        Me.CertificadoTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CertificadoTextBox.Location = New System.Drawing.Point(6, 46)
        Me.CertificadoTextBox.MaximumSize = New System.Drawing.Size(500, 4)
        Me.CertificadoTextBox.MinimumSize = New System.Drawing.Size(4, 20)
        Me.CertificadoTextBox.Name = "CertificadoTextBox"
        Me.CertificadoTextBox.Size = New System.Drawing.Size(180, 20)
        Me.CertificadoTextBox.TabIndex = 17
        '
        'CertificadoClaveTextBox
        '
        Me.CertificadoClaveTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CertificadoClaveTextBox.Location = New System.Drawing.Point(6, 72)
        Me.CertificadoClaveTextBox.MaximumSize = New System.Drawing.Size(500, 4)
        Me.CertificadoClaveTextBox.MinimumSize = New System.Drawing.Size(4, 20)
        Me.CertificadoClaveTextBox.Name = "CertificadoClaveTextBox"
        Me.CertificadoClaveTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.CertificadoClaveTextBox.Size = New System.Drawing.Size(180, 20)
        Me.CertificadoClaveTextBox.TabIndex = 18
        '
        'DEBUGCheckBox
        '
        Me.DEBUGCheckBox.AutoSize = True
        Me.DEBUGCheckBox.Checked = True
        Me.DEBUGCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.DEBUGCheckBox.Location = New System.Drawing.Point(716, 183)
        Me.DEBUGCheckBox.Name = "DEBUGCheckBox"
        Me.DEBUGCheckBox.Size = New System.Drawing.Size(124, 17)
        Me.DEBUGCheckBox.TabIndex = 19
        Me.DEBUGCheckBox.Text = "MODO DE PRUEBA"
        Me.DEBUGCheckBox.UseVisualStyleBackColor = True
        '
        'SERIESTableAdapter
        '
        Me.SERIESTableAdapter.ClearBeforeFill = True
        '
        'ZONASTableAdapter
        '
        Me.ZONASTableAdapter.ClearBeforeFill = True
        '
        'PRESENTACIONFACTURACIONBindingSource1
        '
        Me.PRESENTACIONFACTURACIONBindingSource1.DataMember = "PRESENTACIONFACTURACION"
        Me.PRESENTACIONFACTURACIONBindingSource1.DataSource = Me.DataSetPresentacionFacturacionListado
        '
        'PRESENTACIONFACTURACIONTableAdapter
        '
        Me.PRESENTACIONFACTURACIONTableAdapter.ClearBeforeFill = True
        '
        'TableAdapterManager1
        '
        Me.TableAdapterManager1.BackupDataSetBeforeUpdate = False
        Me.TableAdapterManager1.Connection = Nothing
        Me.TableAdapterManager1.PRESENTACIONFACTURACIONTableAdapter = Nothing
        Me.TableAdapterManager1.UpdateOrder = GestionFacturacion.DataSetPresentacionFacturacionListadoTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete
        '
        'PRESENTACIONFACTURACIONBindingSource2
        '
        Me.PRESENTACIONFACTURACIONBindingSource2.DataMember = "PRESENTACIONFACTURACION"
        Me.PRESENTACIONFACTURACIONBindingSource2.DataSource = Me.DataSetPresentacionFacturacionListado
        '
        'gbCertificado
        '
        Me.gbCertificado.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbCertificado.Controls.Add(Me.CertificadosComboBox)
        Me.gbCertificado.Controls.Add(Me.CertificadoTextBox)
        Me.gbCertificado.Controls.Add(Me.CertificadoClaveTextBox)
        Me.gbCertificado.Location = New System.Drawing.Point(648, 72)
        Me.gbCertificado.MaximumSize = New System.Drawing.Size(500, 105)
        Me.gbCertificado.MinimumSize = New System.Drawing.Size(192, 105)
        Me.gbCertificado.Name = "gbCertificado"
        Me.gbCertificado.Size = New System.Drawing.Size(192, 105)
        Me.gbCertificado.TabIndex = 20
        Me.gbCertificado.TabStop = False
        Me.gbCertificado.Text = "Certificado"
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "NREGISTRO"
        Me.DataGridViewTextBoxColumn1.HeaderText = "NREGISTRO"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Visible = False
        '
        'SERIE
        '
        Me.SERIE.DataPropertyName = "SERIE"
        Me.SERIE.HeaderText = "Serie"
        Me.SERIE.Name = "SERIE"
        Me.SERIE.ReadOnly = True
        Me.SERIE.Width = 50
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "NFACTURA"
        Me.DataGridViewTextBoxColumn5.HeaderText = "Nº Fac."
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Width = 50
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "ANOFACTURA"
        Me.DataGridViewTextBoxColumn6.HeaderText = "Año"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Width = 50
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "CODIGOZONA"
        Me.DataGridViewTextBoxColumn2.HeaderText = "CODIGOZONA"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Visible = False
        '
        'ZONA
        '
        Me.ZONA.DataPropertyName = "ZONA"
        Me.ZONA.HeaderText = "Zona"
        Me.ZONA.Name = "ZONA"
        Me.ZONA.ReadOnly = True
        Me.ZONA.Width = 140
        '
        'DataGridViewTextBoxColumn24
        '
        Me.DataGridViewTextBoxColumn24.DataPropertyName = "PERIODO"
        Me.DataGridViewTextBoxColumn24.HeaderText = "Periodo"
        Me.DataGridViewTextBoxColumn24.Name = "DataGridViewTextBoxColumn24"
        Me.DataGridViewTextBoxColumn24.ReadOnly = True
        Me.DataGridViewTextBoxColumn24.Width = 140
        '
        'FECHA_FORMATTED
        '
        Me.FECHA_FORMATTED.DataPropertyName = "FECHA_FORMATTED"
        Me.FECHA_FORMATTED.HeaderText = "Fecha"
        Me.FECHA_FORMATTED.Name = "FECHA_FORMATTED"
        Me.FECHA_FORMATTED.ReadOnly = True
        Me.FECHA_FORMATTED.Width = 80
        '
        'NOMBREYAPELLIDOS
        '
        Me.NOMBREYAPELLIDOS.DataPropertyName = "NOMBREYAPELLIDOS"
        Me.NOMBREYAPELLIDOS.HeaderText = "Titular"
        Me.NOMBREYAPELLIDOS.Name = "NOMBREYAPELLIDOS"
        Me.NOMBREYAPELLIDOS.ReadOnly = True
        Me.NOMBREYAPELLIDOS.Width = 150
        '
        'NIF_CIF
        '
        Me.NIF_CIF.DataPropertyName = "NIF_CIF"
        Me.NIF_CIF.HeaderText = "NIF/CIF"
        Me.NIF_CIF.Name = "NIF_CIF"
        Me.NIF_CIF.ReadOnly = True
        Me.NIF_CIF.Width = 75
        '
        'FECHA_ENVIO_SII_FORMATTED
        '
        Me.FECHA_ENVIO_SII_FORMATTED.DataPropertyName = "FECHA_ENVIO_SII_FORMATTED"
        Me.FECHA_ENVIO_SII_FORMATTED.HeaderText = "Fecha presentación SII"
        Me.FECHA_ENVIO_SII_FORMATTED.Name = "FECHA_ENVIO_SII_FORMATTED"
        Me.FECHA_ENVIO_SII_FORMATTED.ReadOnly = True
        Me.FECHA_ENVIO_SII_FORMATTED.Width = 145
        '
        'FECHA_PRESENTACION_SII_FORMAT
        '
        Me.FECHA_PRESENTACION_SII_FORMAT.DataPropertyName = "FECHA_PRESENTACION_SII_FORMAT"
        Me.FECHA_PRESENTACION_SII_FORMAT.HeaderText = "FECHA_PRESENTACION_SII_FORMAT"
        Me.FECHA_PRESENTACION_SII_FORMAT.Name = "FECHA_PRESENTACION_SII_FORMAT"
        Me.FECHA_PRESENTACION_SII_FORMAT.ReadOnly = True
        Me.FECHA_PRESENTACION_SII_FORMAT.Visible = False
        Me.FECHA_PRESENTACION_SII_FORMAT.Width = 145
        '
        'FECHA_ERROR_SII_FORMAT
        '
        Me.FECHA_ERROR_SII_FORMAT.DataPropertyName = "FECHA_ERROR_SII_FORMAT"
        Me.FECHA_ERROR_SII_FORMAT.HeaderText = "Fecha error SII"
        Me.FECHA_ERROR_SII_FORMAT.Name = "FECHA_ERROR_SII_FORMAT"
        Me.FECHA_ERROR_SII_FORMAT.ReadOnly = True
        Me.FECHA_ERROR_SII_FORMAT.Width = 110
        '
        'ERROR_PRESENTACION_SII
        '
        Me.ERROR_PRESENTACION_SII.DataPropertyName = "ERROR_PRESENTACION_SII"
        Me.ERROR_PRESENTACION_SII.HeaderText = "Error SII"
        Me.ERROR_PRESENTACION_SII.Name = "ERROR_PRESENTACION_SII"
        Me.ERROR_PRESENTACION_SII.ReadOnly = True
        Me.ERROR_PRESENTACION_SII.Width = 140
        '
        'FECHA_PRESENTACION_FACE_FORMAT
        '
        Me.FECHA_PRESENTACION_FACE_FORMAT.DataPropertyName = "FECHA_PRESENTACION_FACE_FORMAT"
        Me.FECHA_PRESENTACION_FACE_FORMAT.HeaderText = "Fecha presentación FACE"
        Me.FECHA_PRESENTACION_FACE_FORMAT.Name = "FECHA_PRESENTACION_FACE_FORMAT"
        Me.FECHA_PRESENTACION_FACE_FORMAT.ReadOnly = True
        Me.FECHA_PRESENTACION_FACE_FORMAT.Width = 160
        '
        'FECHA_ERROR_FACE_FORMAT
        '
        Me.FECHA_ERROR_FACE_FORMAT.DataPropertyName = "FECHA_ERROR_FACE_FORMAT"
        Me.FECHA_ERROR_FACE_FORMAT.HeaderText = "Fecha error FACE"
        Me.FECHA_ERROR_FACE_FORMAT.Name = "FECHA_ERROR_FACE_FORMAT"
        Me.FECHA_ERROR_FACE_FORMAT.ReadOnly = True
        Me.FECHA_ERROR_FACE_FORMAT.Width = 120
        '
        'FECHA_ERROR_PRESENTACION_FACE
        '
        Me.FECHA_ERROR_PRESENTACION_FACE.DataPropertyName = "FECHA_ERROR_PRESENTACION_FACE"
        Me.FECHA_ERROR_PRESENTACION_FACE.HeaderText = "FECHA_ERROR_PRESENTACION_FACE"
        Me.FECHA_ERROR_PRESENTACION_FACE.Name = "FECHA_ERROR_PRESENTACION_FACE"
        Me.FECHA_ERROR_PRESENTACION_FACE.ReadOnly = True
        Me.FECHA_ERROR_PRESENTACION_FACE.Visible = False
        Me.FECHA_ERROR_PRESENTACION_FACE.Width = 120
        '
        'ERROR_PRESENTACION_FACE
        '
        Me.ERROR_PRESENTACION_FACE.DataPropertyName = "ERROR_PRESENTACION_FACE"
        Me.ERROR_PRESENTACION_FACE.HeaderText = "Error FACE"
        Me.ERROR_PRESENTACION_FACE.Name = "ERROR_PRESENTACION_FACE"
        Me.ERROR_PRESENTACION_FACE.ReadOnly = True
        Me.ERROR_PRESENTACION_FACE.Width = 140
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.DataPropertyName = "FECHA"
        Me.DataGridViewTextBoxColumn7.HeaderText = "FECHA"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        Me.DataGridViewTextBoxColumn7.Visible = False
        '
        'DataGridViewTextBoxColumn70
        '
        Me.DataGridViewTextBoxColumn70.DataPropertyName = "FECHA_ENVIO_SII"
        Me.DataGridViewTextBoxColumn70.HeaderText = "FECHA_ENVIO_SII"
        Me.DataGridViewTextBoxColumn70.Name = "DataGridViewTextBoxColumn70"
        Me.DataGridViewTextBoxColumn70.ReadOnly = True
        Me.DataGridViewTextBoxColumn70.Visible = False
        '
        'FINCA
        '
        Me.FINCA.DataPropertyName = "FINCA"
        Me.FINCA.HeaderText = "FINCA"
        Me.FINCA.Name = "FINCA"
        Me.FINCA.ReadOnly = True
        Me.FINCA.Visible = False
        '
        'TOTAL
        '
        Me.TOTAL.DataPropertyName = "TOTAL"
        Me.TOTAL.HeaderText = "TOTAL"
        Me.TOTAL.Name = "TOTAL"
        Me.TOTAL.ReadOnly = True
        Me.TOTAL.Visible = False
        '
        'TIPOPERIODO
        '
        Me.TIPOPERIODO.DataPropertyName = "TIPOPERIODO"
        Me.TIPOPERIODO.HeaderText = "TIPOPERIODO"
        Me.TIPOPERIODO.Name = "TIPOPERIODO"
        Me.TIPOPERIODO.ReadOnly = True
        Me.TIPOPERIODO.Visible = False
        '
        'FECHA_PRESENTACION_SII
        '
        Me.FECHA_PRESENTACION_SII.DataPropertyName = "FECHA_PRESENTACION_SII"
        Me.FECHA_PRESENTACION_SII.HeaderText = "FECHA_PRESENTACION_SII"
        Me.FECHA_PRESENTACION_SII.Name = "FECHA_PRESENTACION_SII"
        Me.FECHA_PRESENTACION_SII.ReadOnly = True
        Me.FECHA_PRESENTACION_SII.Visible = False
        '
        'ORGANO_GESTOR_FACE
        '
        Me.ORGANO_GESTOR_FACE.DataPropertyName = "ORGANO_GESTOR_FACE"
        Me.ORGANO_GESTOR_FACE.HeaderText = "ORGANO_GESTOR_FACE"
        Me.ORGANO_GESTOR_FACE.Name = "ORGANO_GESTOR_FACE"
        Me.ORGANO_GESTOR_FACE.ReadOnly = True
        Me.ORGANO_GESTOR_FACE.Visible = False
        '
        'frmFacturacion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(852, 561)
        Me.Controls.Add(Me.gbCertificado)
        Me.Controls.Add(Me.DEBUGCheckBox)
        Me.Controls.Add(Me.PresentarFACEButton)
        Me.Controls.Add(Me.gbFiltro)
        Me.Controls.Add(Me.dgvPRESENTACIONFACTURACION)
        Me.Controls.Add(Me.PresentarSIIButton)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmFacturacion"
        Me.Text = "Facturación"
        CType(Me.dgvPRESENTACIONFACTURACION, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PRESENTACIONFACTURACIONBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataSetPresentacionFacturacionListado, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataSetPresentacionFacturacionListado1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbFiltro.ResumeLayout(False)
        Me.gbFiltro.PerformLayout()
        CType(Me.SERIESBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataSetSeriesListado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ServicioTabControl.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        CType(Me.SIIFiltroDelimiter2PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        CType(Me.FACEFiltroDelimiter2PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PRESENTACIONFACTURACIONBindingSource3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataSetPRESENTACIONFACTURACIONPeriodos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ZONASBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataSetZONASListado, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PRESENTACIONFACTURACIONBindingSource1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PRESENTACIONFACTURACIONBindingSource2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbCertificado.ResumeLayout(False)
        Me.gbCertificado.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PresentarSIIButton As Button
    Friend WithEvents DataSetPresentacionFacturacionListado As DataSetPresentacionFacturacionListado
    Friend WithEvents PRESENTACIONFACTURACIONBindingSource1 As BindingSource
    Friend WithEvents PRESENTACIONFACTURACIONTableAdapter As DataSetPresentacionFacturacionListadoTableAdapters.PRESENTACIONFACTURACIONTableAdapter
    Friend WithEvents TableAdapterManager1 As DataSetPresentacionFacturacionListadoTableAdapters.TableAdapterManager
    Friend WithEvents dgvPRESENTACIONFACTURACION As DataGridView
    Friend WithEvents gbFiltro As GroupBox
    Friend WithEvents ZonasFiltroComboBox As ComboBox
    Friend WithEvents FiltroZonasLabel As Label
    Friend WithEvents PresentarFACEButton As Button
    Friend WithEvents AnoFiltroHastaDateTimePicker As DateTimePicker
    Friend WithEvents AnoFiltroHastaLabel As Label
    Friend WithEvents AnoFiltroDesdeLabel As Label
    Friend WithEvents AnoFiltroDesdeDateTimePicker As DateTimePicker
    Friend WithEvents AnoFiltroDeleteButton As Button
    Friend WithEvents PeriodoLabel As Label
    Friend WithEvents PeriodosFiltroComboBox As ComboBox
    Friend WithEvents PRESENTACIONFACTURACIONBindingSource2 As BindingSource
    Friend WithEvents DataSetPRESENTACIONFACTURACIONPeriodos As DataSetPRESENTACIONFACTURACIONPeriodos
    Friend WithEvents PRESENTACIONFACTURACIONBindingSource3 As BindingSource
    Friend WithEvents PRESENTACIONFACTURACIONTableAdapter2 As DataSetPRESENTACIONFACTURACIONPeriodosTableAdapters.PRESENTACIONFACTURACIONTableAdapter
    Friend WithEvents AnoFlitroLabel As Label
    Friend WithEvents CertificadosComboBox As ComboBox
    Friend WithEvents CertificadoTextBox As TextBox
    Friend WithEvents CertificadoClaveTextBox As TextBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents DataSetPresentacionFacturacionListado1 As DataSetPresentacionFacturacionListado
    Friend WithEvents PRESENTACIONFACTURACIONBindingSource As BindingSource
    Friend WithEvents FechaPresentacionSIIFiltroDesdeLabel As Label
    Friend WithEvents FechaPresentacionSIIFiltroCheckBox As CheckBox
    Friend WithEvents FechaPresentacionSIIFiltroClearButton As Button
    Friend WithEvents FechaPresentacionSIIFiltroHastaDateTimePicker As DateTimePicker
    Friend WithEvents FechaPresentacionSIIFiltroHastaLabel As Label
    Friend WithEvents FechaPresentacionSIIFiltroDesdeDateTimePicker As DateTimePicker
    Friend WithEvents FechaPresentacionSIIFiltroLabel As Label
    Friend WithEvents ServicioTabControl As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents ErroresSIIFiltroLabel As Label
    Friend WithEvents ErroresSIIFiltroCheckBox As CheckBox
    Friend WithEvents SIIFiltroDelimiter2PictureBox As PictureBox
    Friend WithEvents ErroresFACEFiltroLabel As Label
    Friend WithEvents ErroresFACEFiltroCheckBox As CheckBox
    Friend WithEvents FechaPresentacionFACEFiltroClearButton As Button
    Friend WithEvents FechaPresentacionFACEFiltroDesdeLabel As Label
    Friend WithEvents FechaPresentacionFACEFiltroCheckBox As CheckBox
    Friend WithEvents FechaPresentacionFACEFiltroHastaDateTimePicker As DateTimePicker
    Friend WithEvents FechaPresentacionFACEFiltroHastaLabel As Label
    Friend WithEvents FechaPresentacionFACEFiltroDesdeDateTimePicker As DateTimePicker
    Friend WithEvents FechaPresentacionFACEFiltroLabel As Label
    Friend WithEvents FACEFiltroDelimiter2PictureBox As PictureBox
    Friend WithEvents DEBUGCheckBox As CheckBox
    Friend WithEvents SeriesFiltroComboBox As ComboBox
    Friend WithEvents SerieLabel As Label
    Friend WithEvents DataSetSeriesListado As DataSetSeriesListado
    Friend WithEvents SERIESBindingSource As BindingSource
    Friend WithEvents SERIESTableAdapter As DataSetSeriesListadoTableAdapters.SERIESTableAdapter
    Friend WithEvents DataSetZONASListado As DataSetZONASListado
    Friend WithEvents ZONASBindingSource As BindingSource
    Friend WithEvents ZONASTableAdapter As DataSetZONASListadoTableAdapters.ZONASTableAdapter
    Friend WithEvents gbCertificado As GroupBox
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents SERIE As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents ZONA As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn24 As DataGridViewTextBoxColumn
    Friend WithEvents FECHA_FORMATTED As DataGridViewTextBoxColumn
    Friend WithEvents NOMBREYAPELLIDOS As DataGridViewTextBoxColumn
    Friend WithEvents NIF_CIF As DataGridViewTextBoxColumn
    Friend WithEvents FECHA_ENVIO_SII_FORMATTED As DataGridViewTextBoxColumn
    Friend WithEvents FECHA_PRESENTACION_SII_FORMAT As DataGridViewTextBoxColumn
    Friend WithEvents FECHA_ERROR_SII_FORMAT As DataGridViewTextBoxColumn
    Friend WithEvents ERROR_PRESENTACION_SII As DataGridViewTextBoxColumn
    Friend WithEvents FECHA_PRESENTACION_FACE_FORMAT As DataGridViewTextBoxColumn
    Friend WithEvents FECHA_ERROR_FACE_FORMAT As DataGridViewTextBoxColumn
    Friend WithEvents FECHA_ERROR_PRESENTACION_FACE As DataGridViewTextBoxColumn
    Friend WithEvents ERROR_PRESENTACION_FACE As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn70 As DataGridViewTextBoxColumn
    Friend WithEvents FINCA As DataGridViewTextBoxColumn
    Friend WithEvents TOTAL As DataGridViewTextBoxColumn
    Friend WithEvents TIPOPERIODO As DataGridViewTextBoxColumn
    Friend WithEvents FECHA_PRESENTACION_SII As DataGridViewTextBoxColumn
    Friend WithEvents ORGANO_GESTOR_FACE As DataGridViewTextBoxColumn
End Class
