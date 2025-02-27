Imports CryCry.ConexionEndPoints



Public Class FrmCryCry



    Private Sub FrmCryCry_Load(sender As Object, e As EventArgs) Handles MyBase.Load


    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs)


        Dim Fechayhora As String = ObtenerHoraServidor()
        Dim argentinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time")
        Dim horaArgentina = TimeZoneInfo.ConvertTimeFromUtc(Fechayhora, argentinaTimeZone)

        Dim symbol = "ADAUSDT"
        Dim dt = ObtenerLibroDeOrdenesContiempo(symbol, horaArgentina)
        DataGridView2.DataSource = dt


    End Sub



    Public Shared Sub ProcesarPrecios()
        Dim listaDeMonedas = ObtenerListaDeMonedas()
        For Each moneda In listaDeMonedas
            Dim nombreMoneda As String = moneda.Item1
            Dim estadoMoneda As String = moneda.Item2
            If estadoMoneda = "Activo" Then
                Dim symbol As String = $"{nombreMoneda}USDT"
                Dim resultado = ObtenerPrecio(symbol)
                Dim precio As Double = resultado.Item1
                Dim horaServidor As DateTime = resultado.Item2
                Dim argentinaTimeZone As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time")
                Dim horaArgentina As DateTime = TimeZoneInfo.ConvertTimeFromUtc(horaServidor, argentinaTimeZone)
                Dim horaArgentinaFormatted As String = horaArgentina.ToString("yyyy-MM-dd HH:mm:ss")
                InsertarPrecioEnBD(nombreMoneda, horaArgentinaFormatted, precio)
            End If
        Next
    End Sub

    Private Function ObtenerTiempoYprocesarAmboTiposdeOrdenes()

        Dim horaServidor As DateTime = ObtenerHoraServidor()
        Dim argentinaTimeZone As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time")
        Dim horaArgentina As DateTime = TimeZoneInfo.ConvertTimeFromUtc(horaServidor, argentinaTimeZone)
        Dim horaArgentinaFormatted As String = horaArgentina.ToString("yyyy-MM-dd HH:mm:ss")
        ProcesarDatosOrdenesfinalizadas(horaArgentinaFormatted)
        ProcesarLibroDeOrdenes(horaArgentinaFormatted)
    End Function

    Private Shared Sub ProcesarDatosOrdenesfinalizadas(ByVal tiempoObtenido As String)
        Dim listaDeMonedas = ObtenerListaDeMonedas()

        For Each moneda In listaDeMonedas
            Dim nombreMoneda As String = moneda.Item1
            Dim estadoMoneda As String = moneda.Item2
            If estadoMoneda = "Activo" Then
                Dim symbol As String = $"{nombreMoneda}USDT"
                Dim dt = ObtenerOrderstrades(symbol)
                For Each row As DataRow In dt.Rows
                    Dim id As Integer = CInt(row("ID"))
                    Dim precio As Double = CDbl(row("Precio"))
                    Dim cantidad As Double = CDbl(row("Cantidad"))
                    Dim quoteQty As Double = CDbl(row("QuoteQty"))
                    Dim tipo As String = row("Tipo").ToString()
                    InsertarOrdenFinalizadaEnBD(nombreMoneda, id, precio, cantidad, quoteQty, tiempoObtenido, tipo)
                Next
            End If
        Next
    End Sub
    Private Shared Sub ProcesarLibroDeOrdenes(ByVal tiempoObtenido As String)

        Dim listaDeMonedas = ObtenerListaDeMonedas()

        For Each moneda In listaDeMonedas
            Dim nombreMoneda As String = moneda.Item1
            Dim estadoMoneda As String = moneda.Item2

            If estadoMoneda = "Activo" Then
                Dim symbol As String = $"{nombreMoneda}USDT"

                Dim dt As DataTable = ObtenerLibroDeOrdenes(symbol)
                For Each row As DataRow In dt.Rows
                    Dim precio As Decimal = CDec(row("Precio"))
                    Dim cantidad As Decimal = CDec(row("Cantidad"))

                    Dim tipo As String = row("Tipo").ToString()
                    InsertarOrdenPendienteEnBD(nombreMoneda, tiempoObtenido, precio, cantidad, tipo)
                Next
            End If
        Next
    End Sub



End Class
