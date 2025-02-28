Imports System.Data.SQLite
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq


Public Class ConexionEndPoints
    Public Class PreciosActuales
        Public Property symbol As String
        Public Property price As Decimal
    End Class


    Public Shared Sub CrearTablasSeguro()
        Using connection As New SQLiteConnection(ObtenerConexion())
            connection.Open()

            ' Verificar si la tabla "ordenesfinalizadas" existe
            Dim checkOrdersTableExists As String = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='ordenesfinalizadas'"
            Using command As New SQLiteCommand(checkOrdersTableExists, connection)
                Dim tableExists As Boolean = Convert.ToInt32(command.ExecuteScalar()) > 0
                If Not tableExists Then
                    ' Crear tabla "ordenesfinalizadas"
                    Dim createOrdersTable As String = "CREATE TABLE ordenesfinalizadas (" &
                                                 "moneda TEXT, " &
                                                 "id INTEGER, " &
                                                 "precio REAL, " &
                                                 "cantidad REAL, " &
                                                 "quoteqty REAL, " &
                                                 "tiempo TEXT, " &
                                                 "tipo TEXT)"
                    Using createCommand As New SQLiteCommand(createOrdersTable, connection)
                        createCommand.ExecuteNonQuery()
                    End Using
                End If
            End Using

            ' Verificar si la tabla "precios" existe
            Dim checkPricesTableExists As String = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='precios'"
            Using command As New SQLiteCommand(checkPricesTableExists, connection)
                Dim tableExists As Boolean = Convert.ToInt32(command.ExecuteScalar()) > 0
                If Not tableExists Then
                    ' Crear tabla "precios"
                    Dim createPricesTable As String = "CREATE TABLE precios (" &
                                                 "moneda TEXT, " &
                                                 "tiempo TEXT, " &
                                                 "precio REAL)"
                    Using createCommand As New SQLiteCommand(createPricesTable, connection)
                        createCommand.ExecuteNonQuery()
                    End Using
                End If
            End Using

            ' Verificar si la tabla "ordenespendientes" existe
            Dim checkPendingOrdersTableExists As String = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='ordenespendientes'"
            Using command As New SQLiteCommand(checkPendingOrdersTableExists, connection)
                Dim tableExists As Boolean = Convert.ToInt32(command.ExecuteScalar()) > 0
                If Not tableExists Then
                    ' Crear tabla "ordenespendientes"
                    Dim createPendingOrdersTable As String = "CREATE TABLE ordenespendientes (" &
                                                        "moneda TEXT, " &
                                                        "tiempo TEXT, " &
                                                        "precio REAL, " &
                                                        "cantidad REAL, " &
                                                        "tipo TEXT)"
                    Using createCommand As New SQLiteCommand(createPendingOrdersTable, connection)
                        createCommand.ExecuteNonQuery()
                    End Using
                End If
            End Using
        End Using
    End Sub

    Public Shared Sub BorrarDatosDeTablas()
        Using connection As New SQLiteConnection(ObtenerConexion())
            connection.Open()

            ' Lista de tablas de las que se eliminarán los datos
            Dim tablas As String() = {"ordenesfinalizadas", "precios", "ordenespendientes"}

            For Each tabla In tablas
                ' Verificar si la tabla existe
                Dim checkTableExists As String = $"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='{tabla}'"
                Using command As New SQLiteCommand(checkTableExists, connection)
                    Dim tableExists As Boolean = Convert.ToInt32(command.ExecuteScalar()) > 0
                    If tableExists Then
                        ' Eliminar todos los registros de la tabla
                        Dim deleteQuery As String = $"DELETE FROM {tabla}"
                        Using deleteCommand As New SQLiteCommand(deleteQuery, connection)
                            deleteCommand.ExecuteNonQuery()

                        End Using
                    Else

                    End If
                End Using
            Next
        End Using
    End Sub

    Private Shared Function ObtenerConexion() As String
        Dim connectionString As String
        connectionString = "Data Source=C:\BaseBina\BaseBina.db;Version=3;"
        Return connectionString
    End Function

    Public Shared Function ObtenerListaDeMonedasActivas() As List(Of String)
        Dim monedas As New List(Of String)()

        Using connection As New SQLiteConnection(ObtenerConexion())
            connection.Open()

            ' Consulta SQL para obtener solo las monedas con estado "Activo"
            Dim selectQuery As String = "SELECT moneda FROM monedas WHERE estado = 'Activo'"
            Using command As New SQLiteCommand(selectQuery, connection)
                Using reader As SQLiteDataReader = command.ExecuteReader()
                    While reader.Read()
                        ' Agregar solo la moneda a la lista
                        Dim moneda As String = reader("moneda").ToString()
                        monedas.Add(moneda)
                    End While
                End Using
            End Using
        End Using

        Return monedas
    End Function

    ' Función para agregar una nueva moneda
    Public Shared Sub AgregarMoneda(moneda As String, estado As String)
        Using connection As New SQLiteConnection(ObtenerConexion())
            connection.Open()

            Dim insertQuery As String = "INSERT INTO monedas (moneda, estado) VALUES (@moneda, @estado)"
            Using command As New SQLiteCommand(insertQuery, connection)
                command.Parameters.AddWithValue("@moneda", moneda)
                command.Parameters.AddWithValue("@estado", estado)
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ' Función para actualizar el estado de una moneda por su nombre
    Public Shared Sub ActualizarEstadoMoneda(moneda As String, nuevoEstado As String)
        Using connection As New SQLiteConnection(ObtenerConexion())
            connection.Open()

            Dim updateQuery As String = "UPDATE monedas SET estado = @estado WHERE moneda = @moneda"
            Using command As New SQLiteCommand(updateQuery, connection)
                command.Parameters.AddWithValue("@estado", nuevoEstado)
                command.Parameters.AddWithValue("@moneda", moneda)
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub



    Public Shared Sub InsertarOrdenFinalizadaEnBD(moneda As String, id As Integer, precio As Double, cantidad As Double, quoteqty As Double, tiempo As String, tipo As String)
        Using connection As New SQLiteConnection(ObtenerConexion())
            connection.Open()

            Dim checkQuery As String = "SELECT COUNT(*) FROM ordenesfinalizadas WHERE id = @id"
            Using checkCommand As New SQLiteCommand(checkQuery, connection)
                checkCommand.Parameters.AddWithValue("@id", id)
                Dim count As Integer = Convert.ToInt32(checkCommand.ExecuteScalar())

                If count > 0 Then

                    Dim logPath As String = "C:\BaseBina\log.txt"
                    Dim currentTime As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

                    Dim countQuery As String = "SELECT COUNT(*) FROM ordenesfinalizadas"
                    Using countCommand As New SQLiteCommand(countQuery, connection)
                        Dim totalExisting As Integer = Convert.ToInt32(countCommand.ExecuteScalar())


                        Dim logMessage As String = $"[{currentTime}] Cantidad de IDs existentes: {totalExisting}"

                        If Not Directory.Exists("C:\BaseBina") Then
                            Directory.CreateDirectory("C:\BaseBina")
                        End If
                        File.WriteAllText(logPath, logMessage)
                    End Using


                    Return
                End If
            End Using

            Dim insertQuery As String = "INSERT INTO ordenesfinalizadas (moneda, id, precio, cantidad, quoteqty, tiempo, tipo) VALUES (@moneda, @id, @precio, @cantidad, @quoteqty, @tiempo, @tipo)"
            Using command As New SQLiteCommand(insertQuery, connection)
                command.Parameters.AddWithValue("@moneda", moneda)
                command.Parameters.AddWithValue("@id", id)
                command.Parameters.AddWithValue("@precio", precio)
                command.Parameters.AddWithValue("@cantidad", cantidad)
                command.Parameters.AddWithValue("@quoteqty", quoteqty)
                command.Parameters.AddWithValue("@tiempo", tiempo)
                command.Parameters.AddWithValue("@tipo", tipo)
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Shared Sub InsertarPrecioEnBD(moneda As String, tiempo As String, precio As Double)
        Using connection As New SQLiteConnection(ObtenerConexion())
            connection.Open()

            Dim insertQuery As String = "INSERT INTO precios (moneda, tiempo, precio) VALUES (@moneda, @tiempo, @precio)"
            Using command As New SQLiteCommand(insertQuery, connection)
                command.Parameters.AddWithValue("@moneda", moneda)
                command.Parameters.AddWithValue("@tiempo", tiempo)
                command.Parameters.AddWithValue("@precio", precio)
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Shared Sub InsertarOrdenPendienteEnBD(moneda As String, tiempo As String, precio As Double, cantidad As Double, tipo As String)
        Using connection As New SQLiteConnection(ObtenerConexion())
            connection.Open()

            Dim insertQuery As String = "INSERT INTO ordenespendientes (moneda, tiempo, precio, cantidad, tipo) VALUES (@moneda, @tiempo, @precio, @cantidad, @tipo)"
            Using command As New SQLiteCommand(insertQuery, connection)
                command.Parameters.AddWithValue("@moneda", moneda)
                command.Parameters.AddWithValue("@tiempo", tiempo)
                command.Parameters.AddWithValue("@precio", precio)
                command.Parameters.AddWithValue("@cantidad", cantidad)
                command.Parameters.AddWithValue("@tipo", tipo)
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub


    'PUBLICAS PARA CONSULTAS A BINANCE

    Public Shared Function ObtenerHoraServidor() As DateTime
        Try
            ' URL para obtener la hora del servidor
            Dim horaUrl As String = "https://api.binance.com/api/v3/time"
            Dim horaRequest As WebRequest = WebRequest.Create(horaUrl)
            Dim horaResponse As WebResponse = horaRequest.GetResponse()
            Dim horaStream As Stream = horaResponse.GetResponseStream()
            Dim horaReader As New StreamReader(horaStream)
            Dim horaJson As String = horaReader.ReadToEnd()

            ' Deserializar la hora del servidor
            Dim horaData As JObject = JObject.Parse(horaJson)
            Dim serverTime As Long = CLng(horaData("serverTime"))
            Dim serverDateTime As DateTime = DateTimeOffset.FromUnixTimeMilliseconds(serverTime).UtcDateTime

            ' Cerrar recursos
            horaReader.Close()
            horaResponse.Close()

            ' Devolver la hora del servidor
            Return serverDateTime
        Catch ex As Exception
            Console.WriteLine("Error al obtener la hora del servidor: " & ex.Message)
            ' En caso de error, devolver un valor por defecto
            Return DateTime.MinValue
        End Try
    End Function

    Public Shared Function ObtenerPrecio(ByVal Symbolo As String) As (Decimal, DateTime)

        Dim precioUrl As String = "https://api.binance.com/api/v3/ticker/price?symbol=" & Symbolo
            Dim precioRequest As WebRequest = WebRequest.Create(precioUrl)
            Dim precioResponse As WebResponse = precioRequest.GetResponse()
            Dim precioStream As Stream = precioResponse.GetResponseStream()
            Dim precioReader As New StreamReader(precioStream)
            Dim precioJson As String = precioReader.ReadToEnd()

        Dim precios As PreciosActuales = JsonConvert.DeserializeObject(Of PreciosActuales)(precioJson)
            Dim precio As Decimal = precios.price

        precioReader.Close()
        precioResponse.Close()
        Dim serverDateTime As DateTime = ObtenerHoraServidor()
        Return (precio, serverDateTime)

    End Function
    Public Shared Function ObtenerOrderstrades(symbol As String, Optional limit As Integer = 200) As DataTable
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
        tradesTable.Columns.Add("Tiempo", GetType(String)) ' Convertir el timestamp a hora local en formato String
        tradesTable.Columns.Add("Tipo", GetType(String))

        ' Zona horaria de Argentina
        Dim argentinaTimeZone As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time")

        ' Agregar los datos de operaciones recientes al DataTable
        For Each trade As JObject In parsedResponse
            Dim tradeId As Long = CLng(trade("id"))
            Dim price As Decimal = CDec(trade("price"))
            Dim quantity As Decimal = CDec(trade("qty"))
            Dim quoteQty As Decimal = CDec(trade("quoteQty"))
            Dim timestamp As Long = CLng(trade("time")) ' Timestamp en milisegundos

            ' Convertir el timestamp a DateTime en UTC
            Dim tradeTimeUtc As DateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime

            ' Convertir la hora UTC a la hora local de Argentina
            Dim tradeTimeArgentina As DateTime = TimeZoneInfo.ConvertTimeFromUtc(tradeTimeUtc, argentinaTimeZone)

            ' Formatear la hora en un formato legible
            Dim tradeTimeFormatted As String = tradeTimeArgentina.ToString("yyyy-MM-dd HH:mm:ss")

            Dim tradeType As String = If(CBool(trade("isBuyerMaker")), "Compra", "Venta")

            tradesTable.Rows.Add(tradeId, price, quantity, quoteQty, tradeTimeFormatted, tradeType)
        Next

        Return tradesTable
    End Function

    Public Shared Function ObtenerLibroDeOrdenes(symbol As String, Optional limit As Integer = 200) As DataTable

        Dim url As String = $"https://api.binance.com/api/v3/depth?symbol={symbol}&limit={limit}"

        Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
        request.Method = "GET"

        Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
        Dim responseStream As Stream = response.GetResponseStream()
        Dim reader As New StreamReader(responseStream)
        Dim responseJson As String = reader.ReadToEnd()

        Dim parsedResponse As JObject = JObject.Parse(responseJson)


        Dim ordersTable As New DataTable()
        ordersTable.Columns.Add("Precio", GetType(Decimal))
        ordersTable.Columns.Add("Cantidad", GetType(Decimal))
        ordersTable.Columns.Add("Tipo", GetType(String))


        For Each bid As JArray In parsedResponse("bids")
            Dim price As Decimal = CDec(bid(0))
            Dim quantity As Decimal = CDec(bid(1))
            ordersTable.Rows.Add(price, quantity, "Compra")
        Next

        For Each ask As JArray In parsedResponse("asks")
            Dim price As Decimal = CDec(ask(0))
            Dim quantity As Decimal = CDec(ask(1))
            ordersTable.Rows.Add(price, quantity, "Venta")
        Next

        Return ordersTable


    End Function

    Public Shared Function ObtenerDatosVelasPublicos(symbol As String, interval As String, Optional limit As Integer = 100) As DataTable
        Try
            Dim url As String = $"https://api.binance.com/api/v3/klines?symbol={symbol}&interval={interval}&limit={limit}"

            Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            request.Method = "GET"

            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            Dim responseStream As Stream = response.GetResponseStream()
            Dim reader As New StreamReader(responseStream)
            Dim responseJson As String = reader.ReadToEnd()

            Dim parsedResponse As JArray = JArray.Parse(responseJson)

            Dim candlesTable As New DataTable()
            candlesTable.Columns.Add("OpenTime", GetType(DateTime))
            candlesTable.Columns.Add("Open", GetType(Decimal))
            candlesTable.Columns.Add("High", GetType(Decimal))
            candlesTable.Columns.Add("Low", GetType(Decimal))
            candlesTable.Columns.Add("Close", GetType(Decimal))
            candlesTable.Columns.Add("Volume", GetType(Decimal))
            candlesTable.Columns.Add("CloseTime", GetType(DateTime))

            ' Obtener la zona horaria de Argentina
            Dim argentinaTimeZone As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time")

            For Each candle As JArray In parsedResponse
                Dim openTime As Long = CLng(candle(0))
                Dim open As Decimal = CDec(candle(1))
                Dim high As Decimal = CDec(candle(2))
                Dim low As Decimal = CDec(candle(3))
                Dim close As Decimal = CDec(candle(4))
                Dim volume As Decimal = CDec(candle(5))
                Dim closeTime As Long = CLng(candle(6))

                ' Convertir de UTC a hora de Argentina
                Dim openDateTimeUtc As DateTime = DateTimeOffset.FromUnixTimeMilliseconds(openTime).UtcDateTime
                Dim closeDateTimeUtc As DateTime = DateTimeOffset.FromUnixTimeMilliseconds(closeTime).UtcDateTime

                Dim openDateTimeArgentina As DateTime = TimeZoneInfo.ConvertTimeFromUtc(openDateTimeUtc, argentinaTimeZone)
                Dim closeDateTimeArgentina As DateTime = TimeZoneInfo.ConvertTimeFromUtc(closeDateTimeUtc, argentinaTimeZone)

                candlesTable.Rows.Add(openDateTimeArgentina, open, high, low, close, volume, closeDateTimeArgentina)
            Next

            Return candlesTable
        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
            Return Nothing
        End Try
    End Function





    'SIN USAR AUN
    Public Shared Function ObtenerOrdenesPendientes(apiKey As String, apiSecret As String, symbol As String) As DataTable
        Try
            ' URL para obtener las órdenes abiertas
            Dim url As String = "https://api.binance.com/api/v3/openOrders?symbol=" & symbol

            ' Crear la solicitud HTTP
            Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            request.Method = "GET"
            request.Headers.Add("X-MBX-APIKEY", apiKey)

            ' Obtener la respuesta
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            Dim responseStream As Stream = response.GetResponseStream()
            Dim reader As New StreamReader(responseStream)
            Dim responseJson As String = reader.ReadToEnd()

            ' Parsear la respuesta JSON
            Dim parsedResponse As JArray = JArray.Parse(responseJson)

            ' Crear DataTable para almacenar las órdenes pendientes
            Dim ordersTable As New DataTable()
            ordersTable.Columns.Add("ID", GetType(Long))
            ordersTable.Columns.Add("Símbolo", GetType(String))
            ordersTable.Columns.Add("Precio", GetType(Decimal))
            ordersTable.Columns.Add("Cantidad", GetType(Decimal))
            ordersTable.Columns.Add("Estado", GetType(String))
            ordersTable.Columns.Add("Fecha", GetType(String))

            ' Agregar las órdenes al DataTable
            For Each order As JObject In parsedResponse
                Dim orderId As Long = CLng(order("orderId"))
                Dim orderSymbol As String = order("symbol").ToString()
                Dim price As Decimal = CDec(order("price"))
                Dim quantity As Decimal = CDec(order("origQty"))
                Dim status As String = order("status").ToString()
                Dim timestamp As Long = CLng(order("time"))

                ' Convertir la marca de tiempo a la hora local de Argentina
                Dim utcDateTime As DateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime
                Dim argentinaTimeZone As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time")
                Dim argentinaDateTime As DateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, argentinaTimeZone)

                ' Formatear la fecha
                Dim formattedDate As String = argentinaDateTime.ToString("yyyy-MM-dd HH:mm:ss")

                ordersTable.Rows.Add(orderId, orderSymbol, price, quantity, status, formattedDate)
            Next

            Return ordersTable
        Catch ex As Exception
            Console.WriteLine("Error al obtener las órdenes pendientes: " & ex.Message)
            Return Nothing ' Manejar el error según sea necesario
        End Try
    End Function
    Public Shared Function ObtenerTodasLasOrdenes(apiKey As String, apiSecret As String, symbol As String, Optional limit As Integer = 500) As DataTable
        Try
            ' Generar el timestamp
            Dim timestamp As Long = CLng((DateTime.UtcNow - New DateTime(1970, 1, 1)).TotalMilliseconds)

            ' Construir la query string y generar la firma
            Dim queryString As String = $"symbol={symbol}&limit={limit}&timestamp={timestamp}"
            Dim signature As String = GenerarFirma(apiSecret, queryString)

            ' Construir la URL completa
            Dim url As String = $"https://api.binance.com/api/v3/allOrders?{queryString}&signature={signature}"

            ' Crear la solicitud HTTP
            Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            request.Method = "GET"
            request.Headers.Add("X-MBX-APIKEY", apiKey)

            ' Obtener la respuesta
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            Dim responseStream As Stream = response.GetResponseStream()
            Dim reader As New StreamReader(responseStream)
            Dim responseJson As String = reader.ReadToEnd()

            ' Parsear la respuesta JSON
            Dim parsedResponse As JArray = JArray.Parse(responseJson)

            ' Crear un DataTable para almacenar las órdenes
            Dim ordersTable As New DataTable()
            ordersTable.Columns.Add("ID", GetType(Long))
            ordersTable.Columns.Add("Symbol", GetType(String))
            ordersTable.Columns.Add("Price", GetType(Decimal))
            ordersTable.Columns.Add("Quantity", GetType(Decimal))
            ordersTable.Columns.Add("Status", GetType(String))
            ordersTable.Columns.Add("Type", GetType(String))
            ordersTable.Columns.Add("Side", GetType(String))
            ordersTable.Columns.Add("Time", GetType(DateTime))

            ' Agregar las órdenes al DataTable
            For Each order As JObject In parsedResponse
                Dim orderId As Long = CLng(order("orderId"))
                Dim orderSymbol As String = order("symbol").ToString()
                Dim price As Decimal = CDec(order("price"))
                Dim quantity As Decimal = CDec(order("origQty"))
                Dim status As String = order("status").ToString()
                Dim type As String = order("type").ToString()
                Dim side As String = order("side").ToString()
                Dim time As Long = CLng(order("time"))

                ' Convertir el timestamp a DateTime
                Dim orderTime As DateTime = DateTimeOffset.FromUnixTimeMilliseconds(time).UtcDateTime

                ordersTable.Rows.Add(orderId, orderSymbol, price, quantity, status, type, side, orderTime)
            Next

            Return ordersTable
        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
            Return Nothing
        End Try
    End Function
    Public Shared Function GenerarFirma(apiSecret As String, queryString As String) As String

        Dim secretKeyBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(apiSecret)
        Dim queryStringBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(queryString)
        Using hmac As New System.Security.Cryptography.HMACSHA256(secretKeyBytes)
            Dim hashBytes As Byte() = hmac.ComputeHash(queryStringBytes)
            Dim signature As String = BitConverter.ToString(hashBytes).Replace("-", "").ToLower()
            Return signature
        End Using
    End Function




    'COMPRA Y VENTA REAL
    Public Shared Function EnviarOrden(apiKey As String, apiSecret As String, symbol As String, side As String, orderType As String, quantity As Decimal, Optional price As Decimal = 0, Optional timeInForce As String = "GTC") As String
        ' URL del endpoint
        Dim baseUrl As String = "https://api.binance.com"
        Dim endpoint As String = "/api/v3/order"
        Dim url As String = baseUrl & endpoint

        ' Crear los parámetros de la orden
        Dim timestamp As Long = CLng((DateTime.UtcNow - New DateTime(1970, 1, 1)).TotalMilliseconds)
        Dim parameters As New Dictionary(Of String, String) From {
        {"symbol", symbol},
        {"side", side}, ' "BUY" o "SELL"
        {"type", orderType}, ' "LIMIT", "MARKET", etc.
        {"quantity", quantity.ToString()},
        {"timestamp", timestamp.ToString()}
    }

        ' Agregar parámetros opcionales para órdenes LIMIT
        If orderType = "LIMIT" Then
            parameters.Add("price", price.ToString())
            parameters.Add("timeInForce", timeInForce) ' "GTC" (Good Till Cancelled) por defecto
        End If

        ' Crear la query string
        Dim queryString As String = String.Join("&", parameters.Select(Function(kvp) $"{kvp.Key}={kvp.Value}"))

        ' Generar la firma usando la función existente
        Dim signature As String = GenerarFirma(apiSecret, queryString)
        parameters.Add("signature", signature)

        ' Crear el cliente HTTP
        Using client As New HttpClient()
            ' Agregar la API Key al encabezado
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey)

            ' Crear el contenido de la solicitud
            Dim content As New FormUrlEncodedContent(parameters)

            ' Enviar la solicitud POST
            Dim response As HttpResponseMessage = client.PostAsync(url, content).Result

            ' Procesar la respuesta
            If response.IsSuccessStatusCode Then
                Dim responseData As String = response.Content.ReadAsStringAsync().Result
                Return responseData ' Devuelve la respuesta JSON de la API
            Else
                Dim errorData As String = response.Content.ReadAsStringAsync().Result
                Throw New Exception($"Error al enviar la orden: {response.StatusCode} - {errorData}")
            End If
        End Using
    End Function



End Class
