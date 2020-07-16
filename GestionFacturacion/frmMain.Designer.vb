<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.btnFacturacion = New System.Windows.Forms.Button()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FacturasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConsultarFacturasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.SalirToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnFacturacion
        '
        Me.btnFacturacion.Location = New System.Drawing.Point(12, 54)
        Me.btnFacturacion.Name = "btnFacturacion"
        Me.btnFacturacion.Size = New System.Drawing.Size(123, 42)
        Me.btnFacturacion.TabIndex = 5
        Me.btnFacturacion.Text = "Registro de Facturas"
        Me.btnFacturacion.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FacturasToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(784, 24)
        Me.MenuStrip1.TabIndex = 11
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FacturasToolStripMenuItem
        '
        Me.FacturasToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConsultarFacturasToolStripMenuItem, Me.ToolStripSeparator2, Me.SalirToolStripMenuItem})
        Me.FacturasToolStripMenuItem.Name = "FacturasToolStripMenuItem"
        Me.FacturasToolStripMenuItem.Size = New System.Drawing.Size(125, 20)
        Me.FacturasToolStripMenuItem.Text = "Registro de Facturas"
        '
        'ConsultarFacturasToolStripMenuItem
        '
        Me.ConsultarFacturasToolStripMenuItem.Name = "ConsultarFacturasToolStripMenuItem"
        Me.ConsultarFacturasToolStripMenuItem.Size = New System.Drawing.Size(125, 22)
        Me.ConsultarFacturasToolStripMenuItem.Text = "Consultar"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(122, 6)
        '
        'SalirToolStripMenuItem
        '
        Me.SalirToolStripMenuItem.Name = "SalirToolStripMenuItem"
        Me.SalirToolStripMenuItem.Size = New System.Drawing.Size(125, 22)
        Me.SalirToolStripMenuItem.Text = "Salir"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.btnFacturacion)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMain"
        Me.Text = "Gestión de Facturación"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnFacturacion As Button
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FacturasToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConsultarFacturasToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents SalirToolStripMenuItem As ToolStripMenuItem
End Class
