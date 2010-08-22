Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Module Module1
    Sub Main()
        Dim serverSocket As New TcpListener(8888)
        Dim clientSocket As TcpClient
        Dim counter As Integer
        ' spuštení serveru
        serverSocket.Start()
        msg("Server Started")
        ' nastavení počítání clientů na 0
        counter = 0
        While (True)
            counter += 1
            clientSocket = serverSocket.AcceptTcpClient()
            msg("Client Number:" + Convert.ToString(counter) + " connected!")
            Dim client As New handleClinet
            client.startClient(clientSocket, Convert.ToString(counter))
        End While

        clientSocket.Close()
        serverSocket.Stop()
        msg("Server Konci")
        Console.ReadLine()
    End Sub
    ' uprava textu
    Sub msg(ByVal mesg As String)
        mesg.Trim()
        Console.WriteLine(" >> " + mesg)
    End Sub

    Public Class handleClinet
        Dim clientSocket As TcpClient
        Dim clNum As String

        Public Sub startClient(ByVal ClientsSocket As TcpClient, ByVal clineNumber As String)
            Me.clientSocket = ClientsSocket
            Me.clNum = clineNumber
            Dim ctThread As Thread = New Thread(AddressOf doChat)
            ctThread.Start()
        End Sub

        Private Sub doChat()
            Dim requestCount As Integer
            Dim bytesFrom(10024) As Byte
            Dim dataFromClient As String
            Dim sendBytes As [Byte]()
            Dim serverResponse As String
            Dim rCount As String

            requestCount = 0

            While (True)
                Try
                    requestCount = requestCount + 1
                    Dim networkStream As NetworkStream = clientSocket.GetStream()
                    networkStream.Read(bytesFrom, 0, CInt(clientSocket.ReceiveBufferSize))

                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom)
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"))
                    msg("Od clienta cislo - " + clNum + " Text: " + dataFromClient)

                    rCount = Convert.ToString(requestCount)
                    serverResponse = "Server clientu c.(" + clNum + ") " + rCount
                    sendBytes = Encoding.ASCII.GetBytes(serverResponse)

                    networkStream.Write(sendBytes, 0, sendBytes.Length)
                    networkStream.Flush()
                    msg(serverResponse)

                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try

            End While

        End Sub
    End Class
End Module