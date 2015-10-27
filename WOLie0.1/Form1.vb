Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Looks for the argument during execution
        Dim returnArgument As String()
        returnArgument = Environment.GetCommandLineArgs()
        If returnArgument.Length > 1 Then
            'MessageBox.Show(returnArgument(1).ToString())

            ' Read txt from from argument into MacArrary
            ' Note - only the first argument is read
            Dim fStream As New System.IO.FileStream(returnArgument(1), IO.FileMode.Open)
            Dim sReader As New System.IO.StreamReader(fStream)

            Dim MacArray As String()
            Dim Index As Integer = 0
            Do While sReader.Peek >= 0
                ReDim Preserve MacArray(Index)
                MacArray(Index) = sReader.ReadLine
                Index += 1
            Loop
            fStream.Close()
            sReader.Close()

            ' Loop through the elements in the MacArray from the text file
            ' Ignore the error the next line throws
            For Each Element As String In MacArray
                'MsgBox(Element)
                SendMagicPacket(Element)
                Application.Exit()
            Next

        Else
            ' Grab the list of Mac Addresses from the MacList.textbox
            Dim MacArray As String() = MacList.Lines
        End If

    End Sub

    Private Function GetIP(ByVal DNSName As String) As String

        Try

            Return Net.Dns.GetHostEntry(DNSName).AddressList.GetLowerBound(0).ToString

        Catch ex As Exception

            Return String.Empty

        End Try

    End Function


    Private Sub SendMagicPacket(ByVal currentMac As String)
        'currentMac is the passed Mac Addess to process

        'SET THESE VARIABLES TO REAL VALUES

        'Dim MacAddress As String = String.Empty
        'Dim WANIPAddr As String = String.Empty
        'Dim LanSubnet As String = String.Empty

        ' Old string of MacAddress
        'Dim MacAddress As String = "2C-41-38-62-8A-3E"
        Dim WANIPAddr As String = "192.186.0.0"
        Dim LanSubnet As String = "255.255.255.0"

        Dim Port As Integer = 9

        Dim udpClient As New System.Net.Sockets.UdpClient

        Dim buf(101) As Char

        Dim sendBytes As [Byte]() = System.Text.Encoding.ASCII.GetBytes(buf)

        For x As Integer = 0 To 5

            sendBytes(x) = CByte("&HFF")

        Next

        'MacAddress = MacAddress.Replace("-", "").Replace(":", "")
        currentMac = currentMac.Replace("-", "").Replace(":", "")

        Dim i As Integer = 6

        For x As Integer = 1 To 16

            'sendBytes(i) = CByte("&H" + MacAddress.Substring(0, 2))
            'sendBytes(i + 1) = CByte("&H" + MacAddress.Substring(2, 2))
            'sendBytes(i + 2) = CByte("&H" + MacAddress.Substring(4, 2))
            'sendBytes(i + 3) = CByte("&H" + MacAddress.Substring(6, 2))
            'sendBytes(i + 4) = CByte("&H" + MacAddress.Substring(8, 2))
            'sendBytes(i + 5) = CByte("&H" + MacAddress.Substring(10, 2))

            sendBytes(i) = CByte("&H" + currentMac.Substring(0, 2))
            sendBytes(i + 1) = CByte("&H" + currentMac.Substring(2, 2))
            sendBytes(i + 2) = CByte("&H" + currentMac.Substring(4, 2))
            sendBytes(i + 3) = CByte("&H" + currentMac.Substring(6, 2))
            sendBytes(i + 4) = CByte("&H" + currentMac.Substring(8, 2))
            sendBytes(i + 5) = CByte("&H" + currentMac.Substring(10, 2))

            i += 6

        Next

        Dim myAddress As String

        Try

            myAddress = Net.IPAddress.Parse(WANIPAddr).ToString

        Catch ex As Exception

            myAddress = GetIP(WANIPAddr)

        End Try

        If myAddress = String.Empty Then

            MessageBox.Show("Invalid IP address/Host Name given")

            Return

        End If

        Dim mySubnetArray() As String

        Dim sm1, sm2, sm3, sm4 As Int64

        mySubnetArray = LanSubnet.Split("."c)

        For i = 0 To mySubnetArray.GetUpperBound(0)

            Select Case i

                Case Is = 0

                    sm1 = Convert.ToInt64(mySubnetArray(i))

                Case Is = 1

                    sm2 = Convert.ToInt64(mySubnetArray(i))

                Case Is = 2

                    sm3 = Convert.ToInt64(mySubnetArray(i))

                Case Is = 3

                    sm4 = Convert.ToInt64(mySubnetArray(i))

            End Select

        Next

        myAddress = WANIPAddr

        udpClient.Send(sendBytes, sendBytes.Length, myAddress, Port)

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        'SendMagicPacket()

        For Each Line As String In MacList.Lines
            MsgBox(Line)
            'SendMagicPacket(Line)
        Next

    End Sub
End Class
