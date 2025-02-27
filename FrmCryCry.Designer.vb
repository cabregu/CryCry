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
        Label1 = New Label()
        DataGridView2 = New DataGridView()
        Button1 = New Button()
        Button2 = New Button()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        CType(DataGridView2, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' BtnConsultar
        ' 
        BtnConsultar.Location = New Point(648, 12)
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
        DataGridView1.Location = New Point(12, 70)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.Size = New Size(711, 151)
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
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(12, 224)
        Label1.Name = "Label1"
        Label1.Size = New Size(41, 15)
        Label1.TabIndex = 3
        Label1.Text = "Label1"
        ' 
        ' DataGridView2
        ' 
        DataGridView2.AllowUserToAddRows = False
        DataGridView2.AllowUserToDeleteRows = False
        DataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView2.Location = New Point(12, 368)
        DataGridView2.Name = "DataGridView2"
        DataGridView2.Size = New Size(711, 151)
        DataGridView2.TabIndex = 5
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(648, 310)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 23)
        Button1.TabIndex = 4
        Button1.Text = "Consultar"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(310, 599)
        Button2.Name = "Button2"
        Button2.Size = New Size(75, 23)
        Button2.TabIndex = 6
        Button2.Text = "Button2"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' FrmCryCry
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(735, 663)
        Controls.Add(Button2)
        Controls.Add(DataGridView2)
        Controls.Add(Button1)
        Controls.Add(Label1)
        Controls.Add(lblPrecio)
        Controls.Add(DataGridView1)
        Controls.Add(BtnConsultar)
        Name = "FrmCryCry"
        Text = "45"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        CType(DataGridView2, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents BtnConsultar As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents lblPrecio As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents DataGridView2 As DataGridView
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button

End Class
