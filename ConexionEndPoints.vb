Imports System.IO
Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq


Public Class ConexionEndPoints
    Public Class PreciosActuales
        Public Property symbol As String
        Public Property price As Decimal
    End Class

    'Obtener el ptecio por crypto
    Public Shared Function ObtenerPrecio(ByVal Symbolo As String) As Decimal
        Try
            Dim request As WebRequest = WebRequest.Create("https://api.binance.com/api/v3/ticker/price?symbol=" & Symbolo)
            request.Credentials = CredentialCache.DefaultCredentials

            Dim response As WebResponse = request.GetResponse()
            Dim dataStream As Stream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()

            ' Print the response for debugging
            Console.WriteLine(responseFromServer)

            ' Deserialize the JSON response
            Dim precios As PreciosActuales = JsonConvert.DeserializeObject(Of PreciosActuales)(responseFromServer)

            reader.Close()
            response.Close()

            Return precios.price
        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
            Return 0 ' Return a default value or handle the error as needed
        End Try
    End Function



    Public Shared Function ObtenerOrderstrades(symbol As String, Optional limit As Integer = 500) As DataTable
        Dim url As String = $"https://api.binance.com/api/v3/trades?symbol={symbol}&limit={limit}"

        Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
        request.Method = "GET"

        Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
        Dim responseStream As Stream = response.GetResponseStream()
        Dim reader As New StreamReader(responseStream)
        Dim responseJson As String = reader.ReadToEnd()

        Dim parsedResponse As JArray = JArray.Parse(responseJson)

        ' Crear DataTable para almacenar los datos de operaciones recientes
        Dim tradesTable As New DataTable()
        tradesTable.Columns.Add("ID", GetType(Long))
        tradesTable.Columns.Add("Precio", GetType(Decimal))
        tradesTable.Columns.Add("Cantidad", GetType(Decimal))
        tradesTable.Columns.Add("QuoteQty", GetType(Decimal))
        tradesTable.Columns.Add("Marca de tiempo", GetType(String))
        tradesTable.Columns.Add("Tipo", GetType(String))

        ' Agregar los datos de operaciones recientes al DataTable
        For Each trade As JObject In parsedResponse
            Dim tradeId As Long = CLng(trade("id"))
            Dim price As Decimal = CDec(trade("price"))
            Dim quantity As Decimal = CDec(trade("qty"))
            Dim quoteQty As Decimal = CDec(trade("quoteQty"))
            Dim timestamp As Long = CLng(trade("time"))

            ' Convertir la marca de tiempo a la hora local de Argentina con segundos y milisegundos
            Dim utcDateTime As DateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime
            Dim argentinaTimeZone As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time")
            Dim argentinaDateTime As DateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, argentinaTimeZone)

            ' Formatear la fecha con segundos y milisegundos
            Dim timestampFormatted As String = argentinaDateTime.ToString("yyyy-MM-dd HH:mm:ss")

            Dim tradeType As String = If(CBool(trade("isBuyerMaker")), "Compra", "Venta")

            tradesTable.Rows.Add(tradeId, price, quantity, quoteQty, timestampFormatted, tradeType)
        Next

        Return tradesTable
    End Function

    Public Shared Function AgruparOrdenesPorMinuto(tradesTable As DataTable) As DataTable
        Dim resultadoTable As New DataTable()
        resultadoTable.Columns.Add("Tipo", GetType(String))
        resultadoTable.Columns.Add("CantidadOrdenes", GetType(Integer))
        resultadoTable.Columns.Add("SumaQuoteQty", GetType(Decimal))
        resultadoTable.Columns.Add("Marca de tiempo", GetType(String))

        Dim grupos As New Dictionary(Of String, Dictionary(Of String, (Integer, Decimal)))

        For Each row As DataRow In tradesTable.Rows
            Dim timestamp As DateTime = DateTime.ParseExact(row("Marca de tiempo").ToString(), "yyyy-MM-dd HH:mm:ss", Nothing)
            Dim tipo As String = row("Tipo").ToString()
            Dim quoteQty As Decimal = CDec(row("QuoteQty"))

            ' Agrupar por minuto completo (ignorando segundos)
            Dim key As String = timestamp.ToString("yyyy-MM-dd HH:mm")

            If Not grupos.ContainsKey(key) Then
                grupos(key) = New Dictionary(Of String, (Integer, Decimal)) From {
                {"Compra", (0, 0D)},
                {"Venta", (0, 0D)}
            }
            End If

            grupos(key)(tipo) = (grupos(key)(tipo).Item1 + 1, grupos(key)(tipo).Item2 + quoteQty)
        Next

        ' Convertir los datos agrupados a DataTable
        For Each kvp In grupos
            Dim timestampGrupo As String = kvp.Key
            For Each tipo In {"Compra", "Venta"}
                If kvp.Value(tipo).Item1 > 0 Then
                    resultadoTable.Rows.Add(tipo, kvp.Value(tipo).Item1, kvp.Value(tipo).Item2, timestampGrupo)
                End If
            Next
        Next

        Return resultadoTable
    End Function



End Class
