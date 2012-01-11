Public Class xmlhf

    Private Sub XMLヘッダフッター編集ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        TextBox1.Text = My.Settings.xmlhead
        TextBox2.Text = My.Settings.xmlfoot
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Try
            Dim sw As New System.IO.StreamWriter("test,xml", False, System.Text.Encoding.GetEncoding(65001))
            sw.Write(TextBox1.Text & TextBox2.Text)
            sw.Close()
            Dim xmlDoc As New System.Xml.XmlDocument
            xmlDoc.Load("test,xml")

            My.Settings.xmlhead = TextBox1.Text
            My.Settings.xmlfoot = TextBox2.Text
            Me.Close()
        Catch ex As System.Xml.XmlException
            Label1.Text = ex.Message
        Catch ex As Exception
            Label1.Text = ex.Message
        End Try

    End Sub

    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox1.TextChanged, TextBox1.ModifiedChanged, TextBox1.Click, TextBox1.KeyPress
        Label4.Text = CountLine(TextBox1.Text, TextBox1.SelectionStart + 1)
    End Sub

    Private Sub TextBox2_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox2.TextChanged, TextBox2.ModifiedChanged, TextBox2.Click, TextBox2.KeyPress
        Label4.Text = CountLine(TextBox1.Text & TextBox2.Text, TextBox1.Text.Length + TextBox2.SelectionStart + 1)
    End Sub


    Private Sub TextBox1_TextChanged1(sender As System.Object, e As System.EventArgs) Handles TextBox1.KeyDown
        Label4.Text = CountLine(TextBox1.Text, TextBox1.SelectionStart + 1)
    End Sub

    Private Sub TextBox2_TextChanged2(sender As System.Object, e As System.EventArgs) Handles TextBox2.KeyDown
        Label4.Text = CountLine(TextBox1.Text & TextBox2.Text, TextBox1.Text.Length + TextBox2.SelectionStart + 1)
    End Sub

    Public Function CountLine(ByVal s As String, ByVal stat As Long) As String
        Try
            Dim line As Long = 0
            Dim len As Long = 1
            Dim lens As Long = 1
            Dim lines As String() = s.Split(CChar(vbLf))
            While True
                line += 1
                len += lines(CInt(line - 1)).Length + 1
                If len > stat Then
                    len -= lines(CInt(line - 1)).Length
                    lens = stat - len + 1
                    If lens > 1 Then

                    ElseIf lens = 1 Then
                        lens = 2
                    Else
                        lens = len - stat
                    End If
                    Exit While
                End If
            End While

            Return line.ToString & "行"
        Catch ex As Exception
            Return ""
        End Try
        Return ""
    End Function
End Class