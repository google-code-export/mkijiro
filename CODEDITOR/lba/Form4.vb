Imports System
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO

Public Class Form4
    Dim path As String
    Dim node As New TreeNode
    Dim itemx As New ListViewItem
    Dim lba_offset As Integer = 0
    Dim lba_offsetmax As Integer = 0
    Dim g As Boolean

    Private Sub ffload(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            Dim m As New Form1
            m = CType(Me.Owner, Form1)
            path = Me.Text
            If parse_mode.Text = "T" Or parse_mode.Text = "P" Then
                node = m.TreeView1.SelectedNode
            Else
                itemx = m.ListView1.Items(m.ListView1.SelectedIndices(0))
            End If
            Dim en As New Regex("\([0-9]+\)", RegexOptions.ECMAScript)
            Dim cp As Match = en.Match(ComboBox1.Text)
            Dim enc As Integer = 0
            If cp.Success Then
                enc = CInt(cp.Value.Substring(1, cp.Value.Length - 2))
            Else
                MessageBox.Show("CODEPAGE(012345) で指定してださい")
                Exit Sub
            End If
            g = True
            Dim filesize As Integer

            If parse_mode.Text = "T" Then
                filesize = CInt(fsize.Text) << 11
            ElseIf parse_mode.Text = "P" Then
                filesize = CInt(fsize.Text)
            Else
                filesize = CInt(itemx.SubItems(2).Text)
            End If

            lba_offsetmax = filesize >> 11
            If lba_offsetmax >= 1 AndAlso (filesize And &H7FF) = 0 Then
                lba_offsetmax -= 1
            End If

            Dim sb As New StringBuilder

            If File.Exists(path) Then
                Dim fs As New FileStream(path, FileMode.Open, FileAccess.Read)
                Dim isolen As Long = fs.Length
                Dim bs(2047 + 16) As Byte
                Dim cso As Boolean = False
                fs.Read(bs, 0, 20)
                If bs(0) = &H43 AndAlso bs(1) = &H49 AndAlso bs(2) = &H53 AndAlso bs(3) = &H4F Then
                    cso = True
                    isolen = cvt32bit(bs, 8)
                End If

                Dim ct As Integer = 0
                Dim lba As Integer

                If parse_mode.Text = "T" Then
                    If node.Level = 0 Then
                        lba = 16
                    Else
                        lba = CInt(node.Tag.ToString)
                    End If
                ElseIf parse_mode.Text = "P" Then
                    lba = CInt(lbas.Text)
                Else
                    lba = CInt(itemx.SubItems(1).Text)
                End If

                Dim lbabk As Integer = lba
                If lba_offset > 0 AndAlso lba_offset <= lba_offsetmax Then
                    lba += lba_offset
                End If
                Label3.Text = lba.ToString & "/" & (lbabk + lba_offsetmax).ToString
                If lba = lbabk + lba_offsetmax Then
                    Label3.ForeColor = Color.Blue
                Else
                    Label3.ForeColor = Color.Green
                End If

                Dim lba_base As Integer = lba << 11
                Dim binst As String = ""

                If parse_mode.Text = "T" Then
                    sb.AppendLine(node.Text)
                ElseIf parse_mode.Text = "P" Then
                    sb.AppendLine("LITTLE ENDIAN PATHTABLE")
                Else
                    sb.AppendLine(itemx.Text)
                End If
                sb.Append("LBA; ")
                sb.AppendLine(lba.ToString)

                If lba_base >= isolen Then
                    sb.Append("読むことができない破損セクターです")
                    Label3.ForeColor = Color.Red
                Else
                    If cso = False Then
                        fs.Seek(lba_base, SeekOrigin.Begin)
                        fs.Read(bs, 0, 2048)
                    Else
                        bs = unpack_cso(lba)
                    End If

                    Dim str(15) As Byte
                    'Array.Copy(bs, 0, str, 0, 2048)
                    'binst = displayhex(Encoding.GetEncoding(enc).GetString(str))
                    '    While binst.Length < 2048
                    '        binst &= "."
                    'End While

                    Dim ct2 As Integer = 0
                    Dim templen As Integer = 0
                    Array.Resize(bs, 2048 + 16)

                    While ct < 2048
                        sb.Append(lba_base.ToString("X8"))
                        sb.Append("; ")
                        For i = 0 To 15
                            sb.Append(bs(ct + i).ToString("X2"))
                            sb.Append(" ")
                        Next
                        'utf8
                        If enc = 65001 Then
                            If ct <> 0 Then
                                If (bs(ct - 1) And &HE0) = &HE0 Then
                                    Array.Resize(str, 17)
                                    Array.Copy(bs, ct - 1, str, 0, 17)
                                ElseIf (bs(ct - 2) And &HE0) = &HE0 Then
                                    Array.Resize(str, 18)
                                    Array.Copy(bs, ct - 2, str, 0, 18)
                                Else
                                    Array.Resize(str, 16)
                                    Array.Copy(bs, ct, str, 0, 16)
                                End If
                            Else
                                Array.Resize(str, 16)
                                Array.Copy(bs, ct, str, 0, 16)
                            End If
                            'sjis
                        ElseIf enc = 932 Then
                            If ct <> 0 Then
                                If cp932zenkaku(cvtu16bit(bs, ct - 1)) = True AndAlso cp932zenkaku2(cvtu16bit(bs, ct - 2)) = False Then
                                    Array.Resize(str, 17)
                                    Array.Copy(bs, ct - 1, str, 0, 17)
                                Else
                                    Array.Resize(str, 16)
                                    Array.Copy(bs, ct, str, 0, 16)
                                End If
                                If cp932zenkaku(cvtu16bit(bs, ct + 15)) = True AndAlso cp932zenkaku2(cvtu16bit(bs, ct + 14)) = False Then
                                    str(15) = 0
                                End If
                            Else
                                Array.Resize(str, 16)
                                Array.Copy(bs, ct, str, 0, 16)
                            End If
                            'euc
                        ElseIf enc = 51932 Then
                            If ct <> 0 Then
                                If cp51932zenkaku(cvtu16bit(bs, ct - 1)) = True AndAlso cp51932zenkaku(cvtu16bit(bs, ct - 2)) = False Then
                                    Array.Resize(str, 17)
                                    Array.Copy(bs, ct - 1, str, 0, 17)
                                Else
                                    Array.Resize(str, 16)
                                    Array.Copy(bs, ct, str, 0, 16)
                                    If cp51932zenkaku(cvtu16bit(bs, ct + 15)) = True AndAlso cp51932zenkaku(cvtu16bit(bs, ct + 14)) = False Then
                                        str(15) = 0
                                    End If
                                End If
                            Else
                                Array.Resize(str, 16)
                                Array.Copy(bs, ct, str, 0, 16)
                            End If
                        Else
                            Array.Copy(bs, ct, str, 0, 16)
                        End If
                        binst = displayhex(Encoding.GetEncoding(enc).GetString(str))
                        sb.AppendLine(binst)
                        lba_base += 16
                        ct += 16
                    End While
                End If

                TextBox1.Text = sb.ToString
                TextBox1.SelectionStart = 0

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Function cp932zenkaku(ByVal st As UInteger) As Boolean
        If (st >= &H8140 AndAlso st <= &H9FFC) _
            OrElse (st >= &HE040 AndAlso st <= &HEAA4) _
            OrElse (st >= &HFA40 AndAlso st <= &HFC4B) Then
            Return True
        End If
        Return False
    End Function

    Function cp932zenkaku2(ByVal st As UInteger) As Boolean
        If (st >= &H8140 AndAlso st <= &H9FFC) Then
            Return True
        End If
        Return False
    End Function

    Function cp51932zenkaku(ByVal st As UInteger) As Boolean
        Return (st >= &H8E40 AndAlso st = &H8EDF) _
            OrElse (st >= &HA1A0 AndAlso st <= &HFCFF)
    End Function

    Dim str As String() = {vbNullChar, vbCr, vbLf, vbTab}

    Function displayhex(ByVal s As String) As String

        For i = 0 To Str.Length - 1
            s = s.Replace(Str(i), ".")
        Next

        Return s
    End Function


    Function cvt16bit(ByVal b As Byte(), ByVal pos As Integer) As Integer
        Return BitConverter.ToInt16(b, pos)
    End Function


    Function cvtu16bit(ByVal b As Byte(), ByVal pos As Integer) As UInteger
        Return CUInt(((b(pos)) * 256 + b(pos + 1)))
    End Function

    Function cvt32bit(ByVal b As Byte(), ByVal pos As Integer) As Integer
        Return BitConverter.ToInt32(b, pos)
    End Function


    Function unpack_cso(ByVal lba As Integer) As Byte()

        Dim cfs As New FileStream(path, FileMode.Open, FileAccess.Read)
        Dim offset(7) As Byte
        Dim source(2047) As Byte
        Dim bss(23) As Byte
        Dim seek As Integer = 0
        cfs.Read(bss, 0, 24)
        Dim align As Integer = cvt32bit(bss, 20) >> 8

        cfs.Seek(24 + lba * 4, System.IO.SeekOrigin.Begin)
        cfs.Read(offset, 0, 4)
        seek = cvt32bit(offset, 0)
        Dim pos As Integer = (seek And &H7FFFFFFF) << align
        cfs.Read(offset, 0, 4)
        Dim pos2 As Integer = (cvt32bit(offset, 0) And &H7FFFFFFF) << align
        Dim readsize As Integer = pos2 - pos
        If align <> 0 AndAlso (readsize + (1 << align)) < 2048 Then
            readsize += (1 << align)
        End If
        cfs.Seek(pos, System.IO.SeekOrigin.Begin)
        cfs.Read(source, 0, readsize)

        If (seek And &H80000000) <> 0 Then
        Else
            If pos2 = pos Then
                Array.Clear(source, 0, 2048)
            Else
                Dim ms As New MemoryStream()
                ms.Write(source, 0, 2048)
                ms.Position = 0
                Dim zipStream As New System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress)
                zipStream.Read(source, 0, 2048)
                zipStream.Close()
                ms.Close()
            End If
        End If
        cfs.Close()

        Return source

    End Function


    Private Sub ComboBox1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If g = True Then
            ffload(sender, e)
        End If
    End Sub


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        lba_offset = 0
        ffload(sender, e)
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        lba_offset -= 1
        If lba_offset < 0 Then
            lba_offset = 0
        End If
        ffload(sender, e)
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        lba_offset += 1
        If lba_offset > lba_offsetmax Then
            lba_offset = lba_offsetmax
        End If
        ffload(sender, e)
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        lba_offset = lba_offsetmax
        ffload(sender, e)
    End Sub

    Private Sub Form4_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        Dim m As New Form1
        m = CType(Me.Owner, Form1)
        m.enc.Text = ComboBox1.SelectedIndex.ToString
    End Sub
End Class