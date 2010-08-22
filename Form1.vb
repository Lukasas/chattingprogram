Imports System.Net.Sockets
Imports System.Text
Public Class Form1
    Dim clientSocket As New System.Net.Sockets.TcpClient()
    Dim serverStream As NetworkStream

    Private Sub Button1_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles Button1.Click
        Dim serverStream As NetworkStream = clientSocket.GetStream()
        Dim buffSize As Integer
        Dim outStream As Byte() = System.Text.Encoding.ASCII.GetBytes(TextBox1.Text + "$")

        serverStream.Write(outStream, 0, outStream.Length)
        serverStream.Flush()

        Dim inStream(10024) As Byte
        buffSize = clientSocket.ReceiveBufferSize
        serverStream.Read(inStream, 0, buffSize)
        Dim returndata As String = System.Text.Encoding.ASCII.GetString(inStream)
        msg("Zprava od serveru : " + returndata)
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        msg("Client Started")
        clientSocket.Connect("127.0.0.1", 8888)
        Label1.Text = "Client - Server Connected ..."
    End Sub
    'updava textu
    Sub msg(ByVal mesg As String)
        TextBox1.Text = TextBox1.Text + Environment.NewLine + " >> " + mesg
    End Sub
End Class