Public Class Form1

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim sr As New System.IO.StreamReader("sjis.txt", System.Text.Encoding.GetEncoding("shift_jis"))
        Dim fs As New System.IO.FileStream("sjis", System.IO.FileMode.Create, System.IO.FileAccess.Write)
        Dim fss As New System.IO.FileStream("utf8", System.IO.FileMode.Create, System.IO.FileAccess.Write)
        Dim s As String
        Dim sjis(1000) As Byte
        Dim utf8(1000) As Byte
        Dim i As Integer = 0
        Dim bb As Byte()
        Dim bbb As Byte()
        While sr.Peek() > -1
            s = sr.ReadLine()
            bb = System.Text.Encoding.GetEncoding(932).GetBytes(s)
            bbb = System.Text.Encoding.GetEncoding(65001).GetBytes(s)
            Array.Resize(bb, 2)
            Array.Resize(bbb, 4)
            fs.Write(bb, 0, 2)
            fss.Write(bbb, 0, 4)
        End While

        sr.Close()
        fs.Close()
        fss.Close()
        Beep()
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click

        Dim s As String = TextBox1.Text
        Dim bb As Byte() = System.Text.Encoding.GetEncoding(65001).GetBytes(s)
       Dim bs(2047) As Byte
        Dim stm(256) As Byte
        Dim dummy As Byte() = {32, 0}
        Dim seek As UInteger
        Dim i As Integer = 0
        Dim k As Integer = 0
        Dim kk As Integer = 0
        Dim tm As Integer = 0
        Dim fail As Boolean = False

        While i < bb.Length
            If bb(i) <= &H80 Then
                stm(k) = bb(i)
                k += 1
                i += 1
            ElseIf bb(i) > &HE0 Then
                seek = bb(i) + (bb(i + 1) * 256) + (bb(i + 2) * 65536)
                kk = 0
                fail = False
                Dim fs As New System.IO.FileStream("table\utf8", System.IO.FileMode.Open, System.IO.FileAccess.Read)
                While True
                    Dim readSize As Integer = fs.Read(bs, 0, bs.Length)
                    For j = 0 To 512 - 1
                        If seek = BitConverter.ToInt32(bs, 4 * j) Then
                            kk += j
                            Exit While
                        End If
                    Next
                    kk += 512
                    If readSize = 0 Then
                        fail = True
                        Exit While
                    End If
                End While
                fs.Close()
                If fail = False Then
                    Dim fss As New System.IO.FileStream("table\sjis", System.IO.FileMode.Open, System.IO.FileAccess.Read)
                    fss.Seek(2 * kk, IO.SeekOrigin.Begin)
                    fss.Read(bs, 0, 2)
                    fss.Close()
                    '半角カナ
                    If bs(1) = 0 Then
                        Array.Copy(bs, 0, stm, k, 1)
                        k += 1
                        '全角
                    Else
                        Array.Copy(bs, 0, stm, k, 2)
                        k += 2
                    End If
                Else
                    '失敗
                    Array.Copy(dummy, 0, stm, k, 1)
                    k += 1
                End If
                i += 3
                End If
        End While

        TextBox2.Text = System.Text.Encoding.GetEncoding(932).GetString(stm)
        s = ""
        For i = 0 To stm.Length - 1
            If stm(i) = 0 Then
                Exit For
            End If
            s &= stm(i).ToString("X2")
        Next

        TextBox3.Text = s



    End Sub
End Class
