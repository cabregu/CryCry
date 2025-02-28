<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmCryCry
    Inherits System.Windows.Forms.Form


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

    Private components As System.ComponentModel.IContainer


    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Timer1 = New Timer(components)
        Timer2 = New Timer(components)
        SuspendLayout()
        ' 
        ' Timer1
        ' 
        Timer1.Interval = 60000
        ' 
        ' Timer2
        ' 
        Timer2.Interval = 1000
        ' 
        ' FrmCryCry
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(355, 306)
        Name = "FrmCryCry"
        Text = "CryCry"
        ResumeLayout(False)
    End Sub
    Friend WithEvents TmrOrgenes As Timer
    Friend WithEvents TmrPrecio As Timer
    Friend WithEvents TmrOrdenes As Timer
    Friend WithEvents TmrPrecios As Timer
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Timer2 As Timer

End Class
