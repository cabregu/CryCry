<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmCryCry
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        BtnConsultar = New Button()
        DataGridView1 = New DataGridView()
        lblPrecio = New Label()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' BtnConsultar
        ' 
        BtnConsultar.Location = New Point(586, 12)
        BtnConsultar.Name = "BtnConsultar"
        BtnConsultar.Size = New Size(75, 23)
        BtnConsultar.TabIndex = 0
        BtnConsultar.Text = "Consultar"
        BtnConsultar.UseVisualStyleBackColor = True
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(12, 79)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.Size = New Size(728, 431)
        DataGridView1.TabIndex = 1
        ' 
        ' lblPrecio
        ' 
        lblPrecio.AutoSize = True
        lblPrecio.Location = New Point(12, 12)
        lblPrecio.Name = "lblPrecio"
        lblPrecio.Size = New Size(10, 15)
        lblPrecio.TabIndex = 2
        lblPrecio.Text = "."
        ' 
        ' FrmCryCry
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(752, 522)
        Controls.Add(lblPrecio)
        Controls.Add(DataGridView1)
        Controls.Add(BtnConsultar)
        Name = "FrmCryCry"
        Text = "CryCry"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents BtnConsultar As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents lblPrecio As Label

End Class
