Public Class list
    Friend rplen As Integer = 1
    Friend rmlen As Integer = 1
    Friend matchno As Integer = 1

    '初期化
    Public Sub listview1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim m As MERGE
        m = CType(Me.Owner, MERGE)
        Me.Location = New Point(m.Location.X + 460, m.Location.Y + 190)


        If m.fixedform.Checked = True Then
            Me.FormBorderStyle = FormBorderStyle.FixedToolWindow
        End If

        ListView1.View = View.Details

        'ヘッダーを追加する（ヘッダー名、幅、アライメント）
        ListView1.Columns.Add("値", -1, HorizontalAlignment.Left)
        ListView1.Columns.Add("説明", -1, HorizontalAlignment.Left)
        ListView1.GridLines = My.Settings.gridview
        If ListView1.GridLines = True Then
            CheckBox1.Checked = True
        End If


        Button(1)

        getlisttext(m.cmt_tb.Text)

        RadioButton1.Checked = True
        matchno = 1

        ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
    End Sub

    '適用ボタン
    Private Sub APPLY_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles APPLY.Click
        Dim f As MERGE
        f = CType(Me.Owner, MERGE)
        If ListView1.SelectedItems.Count = 0 Then
            '処理を抜ける
            Exit Sub
        End If

        Dim itemx As New ListViewItem
        itemx = ListView1.SelectedItems(0)
        Dim b4 = itemx.ToString
        b4 = b4.Replace("ListViewItem: {", "")
        b4 = b4.Replace("}", "")
        getpositions(matchno)
        Dim b3 As String = f.cl_tb.Text
        b3 = b3.Remove(rplen, rmlen)
        If b4.Length < rmlen Then
            b4 = b4.PadLeft(rmlen, "0"c)
        End If
        b3 = b3.Insert(rplen, b4.Substring(b4.Length - rmlen, rmlen))
        f.cl_tb.Text = b3
        f.changed.Text = "リストデータが反映されました。"
    End Sub

    Private Sub lsclose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lsclose.Click
         Me.Close()
    End Sub

    '差し替える場所の特定
    Function getpositions(ByVal z As Integer) As Boolean
        Dim m As MERGE
        m = CType(Me.Owner, MERGE)

        Dim type As String = ""
        Dim bit As Integer = 1
        Dim line As Integer = 1
        Dim b2 As String = m.cmt_tb.Text
        Dim b3 As String = m.cl_tb.Text
        Dim i As Integer = 0
        Dim y As Integer = 1
        Dim lslen As Integer = 23

        If m.PSX = True Then
            lslen = 15
        End If
        Dim r As New System.Text.RegularExpressions.Regex( _
    "LIST/.+\.txt\((A|V),([1-9]|[1-9][0-9]),[1-8],[1-8]\)", _
    System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim l As System.Text.RegularExpressions.Match = r.Match(b2)
        While l.Success
            Dim b1 As String = l.Value
            b1 = b1.Substring(b1.Length - 9, 9)
            i = 0
            Dim b4 As String() = b1.Split(CChar(","c))
            For Each s In b4
                Select Case i
                    Case 0
                        type = s.Substring(s.Length - 1, 1)
                    Case 1
                        s = s.Replace(",", "")
                        line = CType(s, Integer)
                    Case 2
                        s = s.Substring(0, 1)
                        bit = CType(s, Integer)
                    Case 3
                        s = s.Substring(0, 1)
                        rmlen = CType(s, Integer)
                End Select
                i += 1
            Next
            If type = "V" Then
                rplen = 11
                If m.PSX = True Then
                    rplen = 7
                End If
            Else
                rplen = 0
                If m.PSX = True Then
                    rplen = -2
                End If
            End If
            l = l.NextMatch()
            rplen += (line - 1) * lslen + bit + 1
            If z = y Then
                matchno = y
                Exit While
            End If
            y += 1
        End While
        Return True
    End Function

#Region "radio"
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        buttonfunc(1)
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged        
        buttonfunc(2)
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton3.CheckedChanged
        buttonfunc(3)
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton4.CheckedChanged
        buttonfunc(4)
    End Sub
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton5.CheckedChanged
        buttonfunc(5)
    End Sub
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton6.CheckedChanged
        buttonfunc(6)
    End Sub
    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton7.CheckedChanged
        buttonfunc(7)
    End Sub
    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton8.CheckedChanged
        buttonfunc(8)
    End Sub
    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton9.CheckedChanged
        buttonfunc(9)
    End Sub
    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton10.CheckedChanged
        buttonfunc(10)
    End Sub
    Function buttonfunc(ByVal t As Integer) As Boolean
        Dim z As Integer = CInt(NumericUpDown1.Value) * 10 + t
        Dim m As MERGE
        m = CType(Me.Owner, MERGE)
        Button(z)
        Return True
    End Function
    'ラジオでリスト読み込み
    Function Button(ByVal z As Integer) As Boolean
        Dim m As MERGE
        m = CType(Me.Owner, MERGE)
        Dim i As Integer = 1
        Dim bcmt As String = m.cmt_tb.Text
        Dim r As New System.Text.RegularExpressions.Regex( _
"LIST/.+\.txt\((A|V|B),([1-9]|[1-9][0-9]),[1-8],[1-8]\)", _
System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim l As System.Text.RegularExpressions.Match = r.Match(bcmt)
        While l.Success
            If i = z Then
                Dim tx As Integer = l.Value.IndexOf(".txt")
                Dim b1 As String = l.Value.Substring(0, tx + 4)
                b1 = b1.Replace("/", "\")
                b1 = My.Application.Info.DirectoryPath.ToString() & "\" & b1
                If System.IO.File.Exists(b1) Then
                    listtextadd(b1, z)
                Else
                    MessageBox.Show("'" + b1 + "'が見つかりませんでした。")
                End If
                Exit While
            End If
            i += 1
            l = l.NextMatch()
        End While
        matchno = z

        ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)

        Return True
    End Function
#End Region
    'リスト配列作成
    Function listtextadd(ByVal lsfile As String, ByVal z As Integer) As Boolean
        'リストボックスに追加する文字列配列を作成
        Dim sr As New System.IO.StreamReader(lsfile, _
    System.Text.Encoding.GetEncoding(932))
        Dim n As Integer = 0
        Dim l As Integer = 0
        Dim i As Integer = 0
        Dim b1 As String = Nothing
        Dim b2 As String = Nothing
        Dim blen As Integer = 0
        Dim p As Integer = 0
        Dim s1(8000) As String
        Dim s2(8000) As String
        getpositions(z)
        While sr.Peek() > -1
            b1 = sr.ReadLine()
            b1 = b1.Replace("0x", "").Trim
            Dim r As New System.Text.RegularExpressions.Regex( _
    "[0-9A-Fa-f]+", _
    System.Text.RegularExpressions.RegexOptions.IgnoreCase Or _
    System.Text.RegularExpressions.RegexOptions.Singleline)
            Dim m As System.Text.RegularExpressions.Match = r.Match(b1)
            Dim mleft As String = Nothing
            If m.Success Then
                mleft = b1.Substring(m.Index + m.Length, b1.Length - (m.Index + m.Length))
            End If
            blen = b1.Length
            'PME end
            If b1.Contains("_END") Then
                Exit While
            End If
            If blen > 1 And m.Success Then
                If n = 7999 Then
                    Exit While
                End If
                b2 = m.Value.ToUpper.Trim
                b2 = b2.PadLeft(rmlen, "0"c)
                s1(n) = b2
                n += 1
                s2(l) = mleft.Trim
                l += 1
            End If
        End While
        sr.Close()
        n = n - 1

        ListView1.Items.Clear()

        '再描画しないようにする
        ListView1.BeginUpdate()
        '配列の内容を一つ一つ追加する
        For i = 0 To n
            ListView1.Items.Add(s1(i))
            ListView1.Items(i).SubItems.Add(s2(i))
        Next
        '再描画するようにする
        ListView1.EndUpdate()

        Return True
    End Function
    'ぐりっどまん消滅
    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            ListView1.GridLines = True
            My.Settings.gridview = True
        Else
            ListView1.GridLines = False
            My.Settings.gridview = False
        End If

    End Sub
    'ページ変更
    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged

        Dim m As MERGE
        m = CType(Me.Owner, MERGE)
        getlisttext(m.cmt_tb.Text)
        RadioButton1.Checked = True
        Dim r As Integer = CInt(NumericUpDown1.Value * 10) + 1
        Button(r)
    End Sub
    'テキスト取得
    Function getlisttext(ByVal buf As String) As Boolean


        Dim i As Integer = CInt(NumericUpDown1.Value) * 10
        Dim v As Integer = 0
        Dim num As String = NumericUpDown1.Value.ToString
        num = num.Replace("0", "")
        RadioButton1.Text = num & "1:"
        RadioButton2.Text = num & "2:"
        RadioButton3.Text = num & "3:"
        RadioButton4.Text = num & "4:"
        RadioButton5.Text = num & "5:"
        RadioButton6.Text = num & "6:"
        RadioButton7.Text = num & "7:"
        RadioButton8.Text = num & "8:"
        RadioButton9.Text = num & "9:"
        v = i + 10
        RadioButton10.Text = v.ToString & ":"

        RadioButton1.Enabled = False
        RadioButton2.Enabled = False
        RadioButton3.Enabled = False
        RadioButton4.Enabled = False
        RadioButton5.Enabled = False
        RadioButton6.Enabled = False
        RadioButton7.Enabled = False
        RadioButton8.Enabled = False
        RadioButton9.Enabled = False
        RadioButton10.Enabled = False

        Dim text As Integer = 0
        Dim w As Integer = 0
        Dim b1 As String = buf
        Dim b2 As String = Nothing
        Dim r As New System.Text.RegularExpressions.Regex( _
"LIST/.+\.txt\((A|V|B),([0-9]|[1-9][0-9]),[1-8],[1-8]\)", _
System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim max As System.Text.RegularExpressions.Match = r.Match(b1)
        While max.Success
            Dim b3 = b1.Substring(text, max.Index - text)
            Select Case w
                Case 0 + i
                    RadioButton1.Enabled = True
                Case 1 + i
                    RadioButton2.Enabled = True
                    RadioButton1.Text &= b3
                Case 2 + i
                    RadioButton3.Enabled = True
                    RadioButton2.Text &= b3
                Case 3 + i
                    RadioButton4.Enabled = True
                    RadioButton3.Text &= b3
                Case 4 + i
                    RadioButton5.Enabled = True
                    RadioButton4.Text &= b3
                Case 5 + i
                    RadioButton6.Enabled = True
                    RadioButton5.Text &= b3
                Case 6 + i
                    RadioButton7.Enabled = True
                    RadioButton6.Text &= b3
                Case 7 + i
                    RadioButton8.Enabled = True
                    RadioButton7.Text &= b3
                Case 8 + i
                    RadioButton9.Enabled = True
                    RadioButton8.Text &= b3
                Case 9 + i
                    RadioButton10.Enabled = True
                    RadioButton9.Text &= b3
                Case 10 + i
                    RadioButton10.Text &= b3
            End Select
            w += 1
            text = max.Index + max.Length
            max = max.NextMatch
        End While
        Dim b4 = b1.Substring(text, b1.Length - text)
        Dim linefeed As Integer = b4.IndexOf(vbLf)
        If linefeed > 0 Then
            b4 = b4.Substring(0, linefeed)
        End If

        Select Case w
            Case 1 + i
                RadioButton1.Text &= b4
            Case 2 + i
                RadioButton2.Text &= b4
            Case 3 + i
                RadioButton3.Text &= b4
            Case 4 + i
                RadioButton4.Text &= b4
            Case 5 + i
                RadioButton5.Text &= b4
            Case 6 + i
                RadioButton6.Text &= b4
            Case 7 + i
                RadioButton7.Text &= b4
            Case 8 + i
                RadioButton8.Text &= b4
            Case 9 + i
                RadioButton9.Text &= b4
            Case 10 + i
                v = i + 10
                RadioButton10.Text &= b4
        End Select
        Dim k As Integer = Convert.ToInt32((w - 1) \ 10)
        NumericUpDown1.Maximum = Convert.ToDecimal(k)
        Return True
    End Function
End Class