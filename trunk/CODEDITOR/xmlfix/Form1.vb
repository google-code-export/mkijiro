Imports System
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO

Public Class Form1

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Try
            Dim sr As New System.IO.StreamReader(Application.StartupPath & "\nointro.txt", _
                System.Text.Encoding.GetEncoding(932))
            Dim s As String = ""
            Dim ss As String = ""
            Dim index(450) As Integer
            Dim goodcrc(450) As String
            'B9D68496 > FD305564  0003
            Dim r As New Regex("[0-9A-F]{8} > [0-9A-F]{8}  \d{4}", RegexOptions.ECMAScript)
            Dim m As Match
            Dim i As Integer
            While sr.Peek() > -1
                s = sr.ReadLine()
                m = r.Match(s)
                If m.Success Then
                    index(i) = CInt(m.Value.Remove(0, m.Value.Length - 4))
                    goodcrc(i) = m.Value.Substring(11, 8)
                    i += 1
                End If
            End While
            sr.Close()

            Dim ofd As New OpenFileDialog()
            ofd.Filter = "XMLファイル(*.xml;)|*.xml"
            If ofd.ShowDialog() = DialogResult.OK Then
                Dim sr2 As New System.IO.StreamReader(ofd.FileName, _
                    System.Text.Encoding.GetEncoding(65001))
                Dim wr As New System.IO.StreamWriter(Path.GetDirectoryName(ofd.FileName) & "\" & Path.GetFileNameWithoutExtension(ofd.FileName) & "(nointro_fix).xml",
                    False, System.Text.Encoding.GetEncoding(65001))
                Dim q As New Regex("<imageNumber>\d+</imageNumber>", RegexOptions.ECMAScript)
                Dim n As Match
                Dim p As New Regex("<romCRC extension=""\.iso"">[0-9A-F]{8}</romCRC>", RegexOptions.ECMAScript)
                Dim l As Match
                Dim str As New StringBuilder
                Dim rplace As Boolean = False
                i = 0
                While sr2.Peek() > -1
                    s = sr2.ReadLine()
                    n = q.Match(s)
                    l = p.Match(s)
                    If s.Contains("<datVersion>") Then
                        s = vbTab & vbTab & "<datVersion>999</datVersion>"
                    End If

                    If n.Success Then
                        rplace = False
                        ss = n.Value.Remove(0, 13)
                        ss = ss.Remove(ss.Length - 14, 14)
                        While CDbl(ss) > index(i)
                            If index(i) = 0 Then
                                Exit While
                            End If
                            i += 1
                        End While


                        If CDbl(ss) = index(i) Then
                            rplace = True
                        End If
                    End If
                    If l.Success AndAlso rplace = True Then
                        s = s.Replace(l.Value, "<romCRC extension="".iso"">" & goodcrc(i) & "</romCRC>")
                        i += 1
                    End If
                    str.AppendLine(s)
                End While
                sr2.Close()
                wr.Write(str.ToString)
                wr.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "例外")
        End Try
    End Sub
End Class
