Imports CryCry.ConexionEndPoints



Public Class FrmCryCry

    Private Sub BtnConsultar_Click(sender As Object, e As EventArgs) Handles BtnConsultar.Click
        Dim symbol = "ADAUSDT"


        Dim dt = ObtenerOrderstrades(symbol)
        DataGridView1.DataSource = dt
        Label1.Text = DataGridView1.RowCount


        Dim resultado = ObtenerPrecio("ADAUSDT")
        Dim precio = resultado.Item1
        Dim horaServidor = resultado.Item2


        Dim argentinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time")
        Dim horaArgentina = TimeZoneInfo.ConvertTimeFromUtc(horaServidor, argentinaTimeZone)


        Label1.Text = precio.ToString & " " & horaArgentina.ToString("yyyy-MM-dd HH:mm:ss")



    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click


        Dim Fechayhora As String = ObtenerHoraServidor()
        Dim argentinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time")
        Dim horaArgentina = TimeZoneInfo.ConvertTimeFromUtc(Fechayhora, argentinaTimeZone)

        Dim symbol = "ADAUSDT"
        Dim dt = ObtenerLibroDeOrdenesContiempo(symbol, horaArgentina)
        DataGridView2.DataSource = dt




    End Sub


End Class
