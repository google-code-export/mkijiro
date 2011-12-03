Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim fs As New System.IO.FileStream(Application.StartupPath & "\pmp2", _
            System.IO.FileMode.Open, _
            System.IO.FileAccess.Read)
        Dim bs(fs.Length - 1) As Byte
        fs.Read(bs, 0, bs.Length)
        fs.Close()
        Dim i As Integer = 0
        Dim k As Integer = 0
        Dim ss As Byte() = Nothing
        Dim s As New System.Text.StringBuilder
        While i < bs.Length
            If (i And 1) = 0 AndAlso bs(i) = 0 AndAlso bs(i + 1) = 0 Then
                Array.Resize(ss, k)
                Array.ConstrainedCopy(bs, i - k, ss, 0, k)
                s.AppendLine(System.Text.Encoding.GetEncoding(1200).GetString(ss))
                i += 6
                k = 0
            End If
            i += 1
            k += 1
        End While

        Dim sw As New System.IO.StreamWriter(Application.StartupPath & "\pmp.txt", _
    False, _
    System.Text.Encoding.GetEncoding(932))
        sw.Write(s.ToString)
        sw.Close()
    End Sub

    Private Sub form_DragEnter(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.DragEventArgs) _
        Handles Me.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub form_DragDrop(ByVal sender As Object, _
            ByVal e As System.Windows.Forms.DragEventArgs) _
            Handles Me.DragDrop
        Dim fileName As String() = CType( _
            e.Data.GetData(DataFormats.FileDrop, False), _
            String())

        If fileName(0).Contains("pmp2") Then
            Button1_Click(sender, e)
        End If
    End Sub

End Class
