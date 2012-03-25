Imports System
Imports System.Text
Imports System.IO
Imports System.Text.RegularExpressions

Public Class Form1

    Dim p As String() = {"sjis.txt", "euc-jp.txt", "gbk.txt", "utf16le.txt", "utf16be.txt"}

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ENCODE.KeyPress
        e.Handled = True
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Try
            Dim output As String = p(ENCODE.SelectedIndex)
            Dim mm As New Regex("CP\d+", RegexOptions.ECMAScript)
            Dim m As Match = mm.Match(ENCODE.Text)
            If m.Success Then
                Dim enc As Integer = CInt(m.Value.Remove(0, 2))
                If File.Exists(output) Then
                    Dim sr As New System.IO.FileStream(output, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                    Dim fs As New System.IO.FileStream("table\" & Path.GetFileNameWithoutExtension(output), System.IO.FileMode.Create, System.IO.FileAccess.Write)
                    Dim fss As New System.IO.FileStream("table\utf8", System.IO.FileMode.Create, System.IO.FileAccess.Write)
                    Dim bs(CInt(sr.Length) - 1) As Byte
                    sr.Read(bs, 0, bs.Length)
                    Dim ss As String = System.Text.Encoding.GetEncoding(enc).GetString(bs)
                    Dim sss As String() = ss.Split(CChar(vbLf))
                    Dim i As Integer = 0
                    Dim bb As Byte() = Nothing
                    Dim bbb As Byte() = Nothing
                    For Each s As String In sss
                        s = s.Replace(vbCr, "")
                        bb = System.Text.Encoding.GetEncoding(enc).GetBytes(s)
                        bbb = System.Text.Encoding.GetEncoding(65001).GetBytes(s)
                        Array.Resize(bb, 2)
                        Array.Resize(bbb, 4)
                        fs.Write(bb, 0, 2)
                        fss.Write(bbb, 0, 4)
                    Next
                    'End While
                    Array.Clear(bbb, 0, 4)
                    fs.Write(bbb, 0, 2)
                    fss.Write(bbb, 0, 4)
                    sr.Close()
                    fs.Close()
                    fss.Close()
                    Beep()
                Else
                    MessageBox.Show(output & ",変換対象テキストがありません")
                End If
            Else
                MessageBox.Show("文字コードが指定できません")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Try
            Dim mm As New Regex("CP\d+", RegexOptions.ECMAScript)
            Dim m As Match = mm.Match(ENCODE.Text)
            If m.Success Then
                Dim enc As Integer = CInt(m.Value.Remove(0, 2))
                Dim output As String = p(ENCODE.SelectedIndex)
                Dim enctable As String = "table\" & Path.GetFileNameWithoutExtension(output)
                If File.Exists("table\utf8") AndAlso File.Exists(enctable) Then
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
                    Dim big As Integer = 0
                    Dim fail As Boolean = False

                    While i < bb.Length
                        If bb(i) < &H80 Then
                            If enc = 1200 Then
                                stm(k) = bb(i)
                                stm(k + 1) = 0
                                k += 2
                            ElseIf enc = 1201 Then
                                stm(k + 1) = bb(i)
                                stm(k) = 0
                                k += 2
                            Else
                                stm(k) = bb(i)
                                k += 1
                            End If
                            i += 1
                        ElseIf bb(i) < &HF0 Then
                            If (bb(i) < &HE0) Then
                                seek = CUInt(bb(i) + (bb(i + 1) * 256))
                            Else
                                seek = CUInt(bb(i) + (bb(i + 1) * 256) + (bb(i + 2) * 65536))
                            End If
                            kk = 0
                            fail = False
                            Dim fs As New System.IO.FileStream("table\utf8", System.IO.FileMode.Open, System.IO.FileAccess.Read)
                            While True
                                Dim readSize As Integer = fs.Read(bs, 0, bs.Length)
                                For j = 0 To 512 - 1
                                    big = BitConverter.ToInt32(bs, 4 * j)
                                    If seek = big Then
                                        kk += j
                                        Exit While
                                    End If
                                    If big = 0 Then
                                        fail = True
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
                                Dim fss As New System.IO.FileStream(enctable, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                                fss.Seek(2 * kk, IO.SeekOrigin.Begin)
                                fss.Read(bs, 0, 2)
                                fss.Close()
                                'CP1201全角
                                If bs(1) = 0 AndAlso enc = 1201 Then
                                    Array.Copy(bs, 0, stm, k, 2)
                                    k += 2
                                    '半角カナ
                                ElseIf bs(1) = 0 Then
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
                            If (bb(i) < &HE0) Then
                                i += 2
                            Else
                                i += 3
                            End If
                        ElseIf bb(i) < &HF0 Then
                            i += 4
                        ElseIf bb(i) < &HF8 Then
                            i += 5
                        ElseIf bb(i) < &HFC Then
                            i += 6
                        ElseIf bb(i) < &HFE Then
                            i += 7
                        Else
                            'BOM
                            k += 1
                            i += 1
                            End If
                    End While

                    TextBox2.Text = System.Text.Encoding.GetEncoding(enc).GetString(stm)
                    s = ""
                    For i = 0 To stm.Length - 1
                        If enc < 1200 AndAlso enc > 1201 Then
                            If stm(i) = 0 Then
                                Exit For
                            End If
                        Else
                            If (i And 1) = 0 AndAlso stm(i) = 0 AndAlso stm(i + 1) = 0 Then
                                Exit For
                            End If
                        End If
                        s &= stm(i).ToString("X2")
                    Next

                    TextBox3.Text = s

                Else
                    MessageBox.Show(output & enctable & ",文字テーブルファイルがありません")
                End If
            Else
                MessageBox.Show("文字コードが指定できません")
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
End Class
