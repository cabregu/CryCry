Imports CryCry.ConexionEndPoints

Public Class FrmCryCry

    Private Sub FrmCryCry_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'BorrarDatosDeTablas()

        Timer1.Start()
        Timer2.Start()
    End Sub

    Public Shared Sub ProcesarPrecios()

        Dim listaDeMonedas = ObtenerListaDeMonedasActivas()

        For Each moneda In listaDeMonedas

            Dim resultado = ObtenerPrecio(moneda)
            Dim precio As Double = resultado.Item1
            Dim horaServidor As DateTime = resultado.Item2


            Dim argentinaTimeZone As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time")
            Dim horaArgentina As DateTime = TimeZoneInfo.ConvertTimeFromUtc(horaServidor, argentinaTimeZone)
            Dim horaArgentinaFormatted As String = horaArgentina.ToString("yyyy-MM-dd HH:mm:ss")


            InsertarPrecioEnBD(moneda, horaArgentinaFormatted, precio)
        Next
    End Sub

    Private Sub ObtenerTiempoYprocesarAmboTiposdeOrdenes()

        Dim horaServidor As DateTime = ObtenerHoraServidor()

        Dim argentinaTimeZone As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time")
        Dim horaArgentina As DateTime = TimeZoneInfo.ConvertTimeFromUtc(horaServidor, argentinaTimeZone)
        Dim horaArgentinaFormatted As String = horaArgentina.ToString("yyyy-MM-dd HH:mm:ss")

        ProcesarDatosOrdenesfinalizadas()
        ProcesarLibroDeOrdenes(horaArgentinaFormatted)

    End Sub

    Private Shared Sub ProcesarDatosOrdenesfinalizadas()

        Dim listaDeMonedas = ObtenerListaDeMonedasActivas()
        For Each moneda In listaDeMonedas
            Dim dt = ObtenerOrderstrades(moneda)
            For Each row As DataRow In dt.Rows

                Dim id As Integer = CInt(row("ID"))
                Dim precio As Double = CDbl(row("Precio"))
                Dim cantidad As Double = CDbl(row("Cantidad"))
                Dim quoteQty As Double = CDbl(row("QuoteQty"))
                Dim tiempo As String = row("Tiempo").ToString()
                Dim tipo As String = row("Tipo").ToString()
                InsertarOrdenFinalizadaEnBD(moneda, id, precio, cantidad, quoteQty, tiempo, tipo)

            Next
        Next

    End Sub

    Private Shared Sub ProcesarLibroDeOrdenes(ByVal tiempoObtenido As String)

        Dim listaDeMonedas = ObtenerListaDeMonedasActivas()

        For Each moneda In listaDeMonedas

            Dim dt As DataTable = ObtenerLibroDeOrdenes(moneda)

            For Each row As DataRow In dt.Rows
                Dim precio As Decimal = CDec(row("Precio"))
                Dim cantidad As Decimal = CDec(row("Cantidad"))
                Dim tipo As String = row("Tipo").ToString()


                InsertarOrdenPendienteEnBD(moneda, tiempoObtenido, precio, cantidad, tipo)
            Next
        Next
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        ObtenerTiempoYprocesarAmboTiposdeOrdenes()
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick

        ProcesarPrecios()
    End Sub

End Class