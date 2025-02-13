Imports CryCry.ConexionEndPoints


Public Class FrmCryCry

    Private Sub BtnConsultar_Click(sender As Object, e As EventArgs) Handles BtnConsultar.Click

        'Dim precio As Decimal = ObtenerPrecio("DOGEUSDT")
        'lblPrecio.Text = "Precio: " & precio.ToString("C6")

        Dim dt As New DataTable
        dt = ObtenerOrderstrades("ADAUSDT", 5000)
        DataGridView1.DataSource = AgruparOrdenesPorMinuto(dt)
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

    End Sub

    Private Sub FrmCryCry_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
