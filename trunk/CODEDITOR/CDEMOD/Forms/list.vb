Public Class list
    Friend rplen As Integer = 1
    Friend rmlen As Integer = 1
    Friend matchno As Integer = 1

    '初期化
    Public Sub listview1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim m As MERGE
        m = CType(Me.Owner, MERGE)
        Me.Location = New Point(m.Location.X + 460, m.Location.Y + 190)

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

        ListView1.View = View.Details

        'ヘッダーを追加する（ヘッダー名、幅、アライメント）
        ListView1.Columns.Add("値", 60, HorizontalAlignment.Left)
        ListView1.Columns.Add("説明", 200, HorizontalAlignment.Left)
        ListView1.GridLines = My.Settings.gridview
        If ListView1.GridLines = True Then
            CheckBox1.Checked = True
        End If

        Button(1)


        Dim text As Integer = 0
        Dim w As Integer = 1
        Dim b1 As String = m.cmt_tb.Text
        Dim b2 As String = Nothing
        Dim r As New System.Text.RegularExpressions.Regex( _
"LIST/.+\.txt\((A|V|B),([0-9]|[1-9][0-9]),[1-8],[1-8]\)", _
System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim max As System.Text.RegularExpressions.Match = r.Match(b1)
        While max.Success
            Dim b3 = b1.Substring(text, max.Index - text)
            Select Case w
                Case 1
                    RadioButton1.Enabled = True
                Case 2
                    RadioButton2.Enabled = True
                    RadioButton1.Text &= b3
                Case 3
                    RadioButton3.Enabled = True
                    RadioButton2.Text &= b3
                Case 4
                    RadioButton4.Enabled = True
                    RadioButton3.Text &= b3
                Case 5
                    RadioButton5.Enabled = True
                    RadioButton4.Text &= b3
                Case 6
                    RadioButton6.Enabled = True
                    RadioButton5.Text &= b3
                Case 7
                    RadioButton7.Enabled = True
                    RadioButton6.Text &= b3
                Case 8
                    RadioButton8.Enabled = True
                    RadioButton7.Text &= b3
                Case 9
                    RadioButton9.Enabled = True
                    RadioButton8.Text &= b3
                Case 10
                    RadioButton10.Enabled = True
                    RadioButton9.Text &= b3
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
            Case 2
                RadioButton1.Text &= b4
            Case 3
                RadioButton2.Text &= b4
            Case 4
                RadioButton3.Text &= b4
            Case 5
                RadioButton4.Text &= b4
            Case 6
                RadioButton5.Text &= b4
            Case 7
                RadioButton6.Text &= b4
            Case 8
                RadioButton7.Text &= b4
            Case 9
                RadioButton8.Text &= b4
            Case 10
                RadioButton9.Text &= b4
            Case 11
                RadioButton10.Text &= b4
        End Select

        RadioButton1.Checked = True
        matchno = 1
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
        Dim r As New System.Text.RegularExpressions.Regex( _
    "LIST/.+\.txt\((A|V|B),([1-9]|[1-9][0-9]),[1-8],[1-8]\)", _
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
            If type = "V" Or type = "B" Then
                rplen = 11
            Else
                rplen = 0
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
        Button(1)
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        Button(2)
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton3.CheckedChanged
        Button(3)
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton4.CheckedChanged
        Button(4)
    End Sub
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton5.CheckedChanged
        Button(5)
    End Sub
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton6.CheckedChanged
        Button(6)
    End Sub
    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton7.CheckedChanged
        Button(7)
    End Sub
    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton8.CheckedChanged
        Button(8)
    End Sub
    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton9.CheckedChanged
        Button(9)
    End Sub
    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton10.CheckedChanged
        Button(10)
    End Sub

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
                    listtextadd(b1)
                Else
                    MessageBox.Show("'" + b1 + "'が見つかりませんでした。")
                End If
                Exit While
            End If
            i += 1
            l = l.NextMatch()
        End While
        matchno = z
        Return True
    End Function

    'リスト配列作成
    Function listtextadd(ByVal lsfile As String) As Boolean
        'リストボックスに追加する文字列配列を作成
        Dim sr As New System.IO.StreamReader(lsfile, _
    System.Text.Encoding.GetEncoding(932))
        Dim n As Integer = 0
        Dim l As Integer = 0
        Dim b1 As String = Nothing
        Dim b2 As String()
        Dim blen As Integer = 0
        Dim p As Integer = 0
        Dim s1(8000) As String
        Dim s2(8000) As String
        Dim odd As Boolean = False
        While sr.Peek() > -1
            b1 = sr.ReadLine()
            blen = b1.Length
            If b1.Contains("_LST 0x") Then
                b1 = b1.Replace("_LST 0x", "")
                b1 = b1.PadRight(9, "　"c)
                blen = b1.Length
                If b1.Substring(9, blen - 9).Contains(" ") Then
                    b1 = b1.Remove(8, 1)
                    b1 = b1.Replace(" ", "　")
                    b1 = b1.Insert(8, " ")
                ElseIf Not b1.Contains(" ") Then
                    b1 = b1.Remove(8, 1)
                    b1 = b1.Insert(8, " 　")
                End If
            ElseIf b1.Contains("_END") Then
                Exit While
            End If
            If blen > 1 Then
                b2 = b1.Split(CChar(" "))
                For Each s In b2
                    If n = 7999 Then
                        Exit While
                    ElseIf odd = False Then
                        s1(n) = s
                        odd = True
                        n += 1
                    Else
                        s2(l) = s
                        odd = False
                        l += 1
                    End If
                Next
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

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            ListView1.GridLines = True
            My.Settings.gridview = True
        Else
            ListView1.GridLines = False
            My.Settings.gridview = False
        End If

    End Sub
#End Region

End Class