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
        components = New ComponentModel.Container()
        TmrOrdenes = New Timer(components)
        TmrPrecios = New Timer(components)
        SuspendLayout()
        ' 
        ' TmrOrdenes
        ' 
        TmrOrdenes.Interval = 60000
        ' 
        ' TmrPrecios
        ' 
        TmrPrecios.Interval = 1000
        ' 
        ' FrmCryCry
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1001, 785)
        Name = "FrmCryCry"
        Text = "CryCry"
        ResumeLayout(False)
    End Sub
    Friend WithEvents TmrOrgenes As Timer
    Friend WithEvents TmrPrecio As Timer
    Friend WithEvents TmrOrdenes As Timer
    Friend WithEvents TmrPrecios As Timer

End Class
