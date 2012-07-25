Imports System
Imports System.Windows.Forms
Imports System.Text
Imports System.Text.RegularExpressions

Public Class datagrid

    Friend edmode As String

    Private Sub datagrid_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim start As DateTime = Now
            DoubleBuffered = True

            Dim m As MERGE
            m = CType(Me.Owner, MERGE)

            If m.fixedform.Checked = True Then
                Me.AutoSize = True
            End If

            If My.Settings.gridsave = True Then
                gridsave.Checked = True
            Else
                gridsave.Checked = False
            End If
            If m.PSX = True Then
                ComboBox1.Items.RemoveAt(0)
                ComboBox1.Items.RemoveAt(1)
                ComboBox1.Items.RemoveAt(3)


            End If

            Dim b1 As String = m.cl_tb.Text
            Dim i As Integer = 0
            Dim mask As String
            If m.PSX = False Then
                mask = "0x[0-9A-F]{8} 0x[0-9A-F]{8}"
                DirectCast(DataGridView1.Columns(1), DataGridViewTextBoxColumn).MaxInputLength = 10
            Else
                DirectCast(DataGridView1.Columns(0), DataGridViewTextBoxColumn).MaxInputLength = 8
                DirectCast(DataGridView1.Columns(1), DataGridViewTextBoxColumn).MaxInputLength = 4
                mask = "[0-9A-F]{8} [0-9A-F]{4}"
            End If

            Dim r As New System.Text.RegularExpressions.Regex( _
    mask, _
    System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            Dim ed As System.Text.RegularExpressions.Match = r.Match(b1)

            Dim list As DataGridViewRow() = Nothing
            Array.Resize(list, 3000)
            Dim dgitem As New DataGridViewRow
            dgitem.CreateCells(DataGridView1)

            While ed.Success
                dgitem = CType(dgitem.Clone, DataGridViewRow)
                If m.PSX = False Then
                    dgitem.Cells(0).Value = ed.Value.Substring(0, 10)
                    dgitem.Cells(1).Value = ed.Value.Substring(11, 10)
                Else
                    dgitem.Cells(0).Value = ed.Value.Substring(0, 8)
                    dgitem.Cells(1).Value = ed.Value.Substring(9, 4)
                End If
                dgitem.Cells(2).Value = "DEC"
                list(i) = dgitem
                list = hex2str_rows("DEC", i, list, 1)
                ed = ed.NextMatch()
                i += 1
            End While
            Array.Resize(list, i)


            mask = "<DGLINE[0-9]{1,3}='.*?'>"

            Dim q As New System.Text.RegularExpressions.Regex( _
    mask, _
    System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            Dim dg_comment As System.Text.RegularExpressions.Match = q.Match(m.dgtext.Text)
            Dim k, l, z, zz As Integer
            zz = i
            i = 0
            While dg_comment.Success Or i < zz
                k = dg_comment.Value.IndexOf("'") + 1
                z = dg_comment.Value.LastIndexOf("'")
                If k > 0 Then
                    b1 = dg_comment.Value.Substring(0, k - 2)
                    b1 = b1.Replace("<DGLINE", "")
                    l = CInt(b1) - 1
                    If l < zz AndAlso l >= 0 AndAlso k < z Then
                        list(l).Cells(4).Value = dg_comment.Value.Substring(k, z - k).ToString
                    End If
                End If
                dg_comment = dg_comment.NextMatch()
                i += 1
            End While


            mask = "<DGMODE[0-9]{1,3}='.*?'>"
            Dim dm As New System.Text.RegularExpressions.Regex(mask, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            Dim dg_mode As System.Text.RegularExpressions.Match = dm.Match(m.dmtext.Text)
            Dim mode As String = ""
            zz = i
            i = 0
            While dg_mode.Success AndAlso i < zz
                k = dg_mode.Value.IndexOf("'") + 1
                z = dg_mode.Value.LastIndexOf("'")
                If k > 0 Then
                    b1 = dg_mode.Value.Substring(0, k - 2)
                    b1 = b1.Replace("<DGMODE", "")
                    l = CInt(b1) - 1
                    If l < zz AndAlso l >= 0 AndAlso k < z Then
                        mode = dg_mode.Value.Substring(k, z - k)
                        list(l).Cells(2).Value = mode
                        list = hex2str_rows(mode, l, list, 1)
                    End If
                End If
                dg_mode = dg_mode.NextMatch()
                i += 1
            End While

            DataGridView1.Rows.AddRange(list)

            DataGridView1.Columns(4).Width = 591 - (DataGridView1.Columns(0).Width + DataGridView1.Columns(1).Width + DataGridView1.Columns(2).Width + DataGridView1.Columns(3).Width)

            timer.Text = (Now - start).TotalSeconds.ToString
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    'CellValidatingイベントハンドラ 
    Private Sub DataGridView1_CellValidating(ByVal sender As Object, _
        ByVal e As DataGridViewCellValidatingEventArgs) _
        Handles DataGridView1.CellValidating

        Dim f As New MERGE
        f = CType(Me.Owner, MERGE)
        Dim dgv As DataGridView = DirectCast(sender, DataGridView)

        Dim mask As String = ""

        '新しい行のセルでなく、セルの内容が変更されている時だけ検証する 
        If e.RowIndex = dgv.NewRowIndex OrElse Not dgv.IsCurrentCellDirty Then
            Exit Sub
        End If

        Dim d As Integer = DataGridView1.CurrentCell.RowIndex
        Dim c As Integer = DataGridView1.CurrentCell.ColumnIndex

        If dgv.Columns(e.ColumnIndex).Name = "編集タイプ" Then

            If Not DataGridView1.Rows(d).Cells(2).Value Is Nothing Then
                Dim check As String = DataGridView1.Rows(d).Cells(2).Value.ToString
                Select Case check
                    Case "DEC", "BINARY32", "BINARY32(16bit)", "BINARY16"
                        DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 11
                    Case "DEC16BIT"
                        DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 6
                        'DataGridView1.Rows(d).Cells(3).Value = "0"
                    Case "OR", "AND", "XOR"
                        DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 10
                    Case "ASM"
                        DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 20
                        'DataGridView1.Rows(d).Cells(3).Value = "0x0"
                End Select
            End If
        End If

        'DOBON.NET http://dobon.net/vb/dotnet/datagridview/cellvalidating.html
        If dgv.Columns(e.ColumnIndex).Name = "入力値" Then

            If Not DataGridView1.Rows(d).Cells(2).Value Is Nothing Then
                Dim check As String = DataGridView1.Rows(d).Cells(2).Value.ToString
                If check.Contains("DEC") Then
                    If f.PSX = False AndAlso check.Contains("16BIT") = False Then
                        DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 11
                    Else
                        DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 6
                    End If
                    Dim str As String = e.FormattedValue.ToString()
                    Dim r As New System.Text.RegularExpressions.Regex( _
            "-?\d+", _
            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                    Dim m As System.Text.RegularExpressions.Match = r.Match(str)
                    If m.Success And m.Value.Length = str.Length Then
                        Label1.Text = ""
                        DataGridView1.Rows(d).Cells(3).Value = m.Value
                        Dim b1 As String = m.Value
                        Dim max As Int64 = Convert.ToInt64(b1)
                        If f.PSX = False AndAlso check.Contains("16BIT") = False Then
                            If max > 4294967294 Then
                                Label1.Text = "4294967294を超えてます"
                                e.Cancel = True
                            ElseIf max < -2147483647 Then
                                Label1.Text = "-2147483647を超えてます"
                                e.Cancel = True
                            End If
                        Else
                            If max > 65535 Then
                                Label1.Text = "65535を超えてます"
                                e.Cancel = True
                            ElseIf max < -32767 Then
                                Label1.Text = "-32767を超えてます"
                                e.Cancel = True
                            End If
                        End If
                    ElseIf str = "" Then

                    Else
                        '行にエラーテキストを設定 
                        Label1.Text = "不正な値です"
                        '入力した値をキャンセルして元に戻すには、次のようにする 
                        dgv.CancelEdit()
                        'キャンセルする 
                        e.Cancel = True
                    End If
                ElseIf check = "OR" Or check = "AND" Or check = "XOR" Then
                    If f.PSX = False Then
                        DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 10
                    Else
                        DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 6
                    End If
                    Dim str As String = e.FormattedValue.ToString()
                    Dim r As New System.Text.RegularExpressions.Regex( _
                     "^0x[0-9A-Fa-f]{1,8}", _
                                System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                    Dim m As System.Text.RegularExpressions.Match = r.Match(str)
                    If m.Success Then
                        Label1.Text = ""
                        DataGridView1.Rows(d).Cells(3).Value = m.Value
                    ElseIf str = "" Then

                    Else
                        '行にエラーテキストを設定 
                        Label1.Text = "0x付き16進数ではありまえん"
                        '入力した値をキャンセルして元に戻すには、次のようにする 
                        dgv.CancelEdit()
                        'キャンセルする 
                        e.Cancel = True
                    End If
                ElseIf check.Contains("BIN") Then
                    DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 11
                    Dim str As String = e.FormattedValue.ToString()
                    Dim r As New System.Text.RegularExpressions.Regex( _
                     "^[-|+]?\d+\.?\d*", _
                                System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                    Dim m As System.Text.RegularExpressions.Match = r.Match(str)
                    If m.Success Then
                        Label1.Text = ""
                        DataGridView1.Rows(d).Cells(3).Value = m.Value
                    ElseIf str = "" Then

                    Else
                        '行にエラーテキストを設定 
                        Label1.Text = "不正な値です"
                        '入力した値をキャンセルして元に戻すには、次のようにする 
                        dgv.CancelEdit()
                        'キャンセルする 
                        e.Cancel = True
                    End If
                ElseIf check.Contains("ASM") Then
                    Dim str As String = e.FormattedValue.ToString()
                    DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 40
                    'Label1.Text = assembler(str)
                Else
                    dgv.CancelEdit()
                    Label1.Text = "編集タイプが選択されてません"
                End If
            End If
        ElseIf dgv.Columns(e.ColumnIndex).Name = "アドレス" Or dgv.Columns(e.ColumnIndex).Name = "値" Then
            Dim str As String = e.FormattedValue.ToString()
            If f.PSX = False Then
                mask = "^0x[0-9a-fA-F]{8}"
            ElseIf dgv.Columns(e.ColumnIndex).Name = "アドレス" Then
                mask = "^[0-9a-fA-F]{8}"
            ElseIf dgv.Columns(e.ColumnIndex).Name = "値" Then
                mask = "^[0-9a-fA-F]{4}"
            End If
            Dim r As New System.Text.RegularExpressions.Regex( _
             mask, _
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            Dim m As System.Text.RegularExpressions.Match = r.Match(str)
            If m.Success Then
                Label1.Text = ""
            Else
                '行にエラーテキストを設定 
                If f.PSX = True AndAlso dgv.Columns(e.ColumnIndex).Name = "値" Then
                    Label1.Text = "必ず(4桁)で入力してください"
                ElseIf f.PSX = True AndAlso dgv.Columns(e.ColumnIndex).Name = "アドレス" Then
                    Label1.Text = "必ず(8桁)で入力してください"
                Else
                    Label1.Text = "必ず0x(8桁)で入力してください"
                End If
                '入力した値をキャンセルして元に戻すには、次のようにする 
                dgv.CancelEdit()
                'キャンセルする 
                e.Cancel = True
            End If
        End If
    End Sub

    'CellPaintingイベントハンドラ
    Private Sub DataGridView1_CellPainting(ByVal sender As Object, _
            ByVal e As DataGridViewCellPaintingEventArgs) _
            Handles DataGridView1.CellPainting
        '列ヘッダーかどうか調べる
        If e.ColumnIndex < 0 And e.RowIndex >= 0 Then
            'セルを描画する
            e.Paint(e.ClipBounds, DataGridViewPaintParts.All)

            '行番号を描画する範囲を決定する
            'e.AdvancedBorderStyleやe.CellStyle.Paddingは無視しています
            Dim indexRect As Rectangle = e.CellBounds
            indexRect.Inflate(-2, -2)
            '行番号を描画する
            TextRenderer.DrawText(e.Graphics, _
                (e.RowIndex + 1).ToString(), _
                e.CellStyle.Font, _
                indexRect, _
                e.CellStyle.ForeColor, _
                TextFormatFlags.Right Or TextFormatFlags.VerticalCenter)
            '描画が完了したことを知らせる
            e.Handled = True
        End If
    End Sub

    Private Sub DataGridView1_EditingControlShowing(ByVal sender As Object, _
        ByVal e As DataGridViewEditingControlShowingEventArgs) _
        Handles DataGridView1.EditingControlShowing
        '表示されているコントロールがDataGridViewTextBoxEditingControlか調べる
        If TypeOf e.Control Is DataGridViewTextBoxEditingControl Then
            Dim dgv As DataGridView = CType(sender, DataGridView)

            '編集のために表示されているコントロールを取得
            Dim tb As DataGridViewTextBoxEditingControl = _
                CType(e.Control, DataGridViewTextBoxEditingControl)

            'イベントハンドラを削除
            RemoveHandler tb.KeyPress, AddressOf dataGridViewTextBox_KeyPress

            '該当する列か調べる
            If dgv.CurrentCell.OwningColumn.Name = "入力値" Or dgv.CurrentCell.ColumnIndex = 0 Or dgv.CurrentCell.ColumnIndex = 1 Then
                'KeyPressイベントハンドラを追加
                AddHandler tb.KeyPress, AddressOf dataGridViewTextBox_KeyPress
            End If


        End If
    End Sub

    'DataGridViewに表示されているテキストボックスのKeyPressイベントハンドラ
    Private Sub dataGridViewTextBox_KeyPress(ByVal sender As Object, _
            ByVal e As KeyPressEventArgs)
        '数字しか入力できないようにする
        Dim d As Integer = DataGridView1.CurrentCell.RowIndex
        Dim c As Integer = DataGridView1.CurrentCell.ColumnIndex

        If e.KeyChar <> "x"c Then
            e.KeyChar = Char.ToUpper(e.KeyChar)
        End If

        If Not DataGridView1.Rows(d).Cells(2).Value Is Nothing Then
            Dim check As String = DataGridView1.Rows(d).Cells(2).Value.ToString
            If c = 3 Then
                If check = "OR" Or check = "AND" Or check = "XOR" Then
                    If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar < "A"c Or e.KeyChar > "F"c) And (e.KeyChar < "a"c Or e.KeyChar > "f"c) And e.KeyChar <> vbBack And e.KeyChar <> "x"c Then
                        e.Handled = True
                    End If
                    DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 10
                ElseIf check = "DEC" Then
                    If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And e.KeyChar <> "-"c And e.KeyChar <> vbBack Then
                        e.Handled = True
                    End If
                    DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 11
                ElseIf check = "DEC16BIT" Then
                    If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And e.KeyChar <> "-"c And e.KeyChar <> vbBack Then
                        e.Handled = True
                    End If
                    DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 6
                ElseIf check = "ASM" Then
                    e.KeyChar = Char.ToLower(e.KeyChar)
                    DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 40
                Else
                    If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And e.KeyChar <> "."c And e.KeyChar <> "-"c And e.KeyChar <> "+"c And e.KeyChar <> vbBack Then
                        e.Handled = True
                    End If
                    DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 11
                End If
            End If
        End If

        If c = 0 Or c = 1 Then
            If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar < "A"c Or e.KeyChar > "F"c) And (e.KeyChar < "a"c Or e.KeyChar > "f"c) And e.KeyChar <> vbBack And e.KeyChar <> "x"c Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub edival(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles APPLY.Click, appy.Click
        Dim m As MERGE
        m = CType(Me.Owner, MERGE)
        Dim mask As String = ""
        Dim add_val As Integer = 1
        If g_address.Checked = True Then
            add_val = 0
        End If

        Dim d As Integer = DataGridView1.CurrentCell.RowIndex
        Dim c As Integer = DataGridView1.CurrentCell.ColumnIndex
        If Not DataGridView1.Rows(d).Cells(2).Value Is Nothing And Not DataGridView1.Rows(d).Cells(3).Value Is Nothing Then
            Dim check As String = DataGridView1.Rows(d).Cells(2).Value.ToString
            Dim str As String = DataGridView1.Rows(d).Cells(3).Value.ToString
            If check = "DEC" Then
                mask = "^-?\d{1,11}"
                Dim r As New System.Text.RegularExpressions.Regex( _
                 mask, _
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim v As System.Text.RegularExpressions.Match = r.Match(str)
                If v.Success AndAlso v.Value.Length = str.Length Then
                    Dim dec As Int64 = (Convert.ToInt64(v.Value) And &HFFFFFFFFF)
                    If Not (m.PSX = True AndAlso g_value.Checked = True) Then
                        DataGridView1.Rows(d).Cells(add_val).Value = "0x" + dec.ToString("X").PadLeft(8, "0"c).ToUpper
                    Else
                        DataGridView1.Rows(d).Cells(add_val).Value = dec.ToString("X").PadLeft(4, "0"c).ToUpper
                    End If

                Else
                    Label1.Text = "不正な値です"
                End If
            ElseIf check = "DEC16BIT" Then
                mask = "^-?\d{1,11}"
                Dim r As New System.Text.RegularExpressions.Regex( _
                 mask, _
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim v As System.Text.RegularExpressions.Match = r.Match(str)
                If v.Success AndAlso v.Value.Length = str.Length Then
                    Dim dec As Int64 = (Convert.ToInt64(v.Value) And &HFFFF)
                    Dim ssss As String = DataGridView1.Rows(d).Cells(add_val).Value.ToString
                    Dim dd As Int32 = CInt(Convert.ToInt64(ssss, 16) >> 16)
                    If Not (m.PSX = True AndAlso g_value.Checked = True) Then
                        DataGridView1.Rows(d).Cells(add_val).Value = "0x" + dd.ToString("X").PadLeft(4, "0"c) + dec.ToString("X").PadLeft(4, "0"c).ToUpper
                    Else
                        DataGridView1.Rows(d).Cells(add_val).Value = dec.ToString("X").PadLeft(4, "0"c).ToUpper
                    End If

                Else
                    Label1.Text = "不正な値です"
                End If
            ElseIf check = "OR" Then
                mask = "^0x[0-9a-fA-F]{1,8}"
                Dim r As New System.Text.RegularExpressions.Regex(mask, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim v As System.Text.RegularExpressions.Match = r.Match(str)
                If v.Success AndAlso v.Value.Length = str.Length Then
                    Dim b1 As Int32 = Convert.ToInt32(DataGridView1.Rows(d).Cells(add_val).Value.ToString, 16)
                    Dim hex As Int32 = Convert.ToInt32(v.Value, 16)
                    If Not (m.PSX = True AndAlso g_value.Checked = True) Then
                        DataGridView1.Rows(d).Cells(add_val).Value = "0x" + Convert.ToString((b1 Or hex), 16).PadLeft(8, "0"c).ToUpper
                    Else
                        DataGridView1.Rows(d).Cells(add_val).Value = Convert.ToString((b1 Or hex), 16).PadLeft(4, "0"c).ToUpper
                    End If

                Else
                    Label1.Text = "不正な値です"
                End If
            ElseIf check = "AND" Then
                mask = "^0x[0-9a-fA-F]{1,8}"
                Dim r As New System.Text.RegularExpressions.Regex(mask, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim v As System.Text.RegularExpressions.Match = r.Match(str)
                If v.Success AndAlso v.Value.Length = str.Length Then
                    Dim b1 As Int32 = Convert.ToInt32(DataGridView1.Rows(d).Cells(add_val).Value.ToString, 16)
                    Dim hex As Int32 = Convert.ToInt32(v.Value, 16)
                    If Not (m.PSX = True AndAlso g_value.Checked = True) Then
                        DataGridView1.Rows(d).Cells(add_val).Value = "0x" + Convert.ToString((b1 And hex), 16).ToString.PadLeft(8, "0"c).ToUpper
                    Else
                        DataGridView1.Rows(d).Cells(add_val).Value = Convert.ToString((b1 And hex), 16).ToString.PadLeft(4, "0"c).ToUpper
                    End If

                Else
                    Label1.Text = "不正な値です"
                End If
            ElseIf check = "XOR" Then
                mask = "^0x[0-9a-fA-F]{1,8}"
                Dim r As New System.Text.RegularExpressions.Regex(mask, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim v As System.Text.RegularExpressions.Match = r.Match(str)
                If v.Success AndAlso v.Value.Length = str.Length Then
                    Dim b1 As Int32 = Convert.ToInt32(DataGridView1.Rows(d).Cells(add_val).Value.ToString, 16)
                    Dim hex As Int32 = Convert.ToInt32(v.Value, 16)
                    If Not (m.PSX = True AndAlso g_value.Checked = True) Then
                        DataGridView1.Rows(d).Cells(add_val).Value = "0x" + Convert.ToString((b1 Xor hex), 16).ToString.PadLeft(8, "0"c).ToUpper
                    Else
                        DataGridView1.Rows(d).Cells(add_val).Value = Convert.ToString((b1 Xor hex), 16).ToString.PadLeft(4, "0"c).ToUpper
                    End If

                Else
                    Label1.Text = "不正な値です"
                End If

            ElseIf check = "ASM" Then
                If m.PSX = False Then
                    Dim asm As String = assembler(str, DataGridView1.Rows(d).Cells(0).Value.ToString)
                    If asm <> "" Then
                        DataGridView1.Rows(d).Cells(add_val).Value = asm
                        'DataGridView1.Rows(d).Cells(4).Value = decoders(DataGridView1.Rows(d).Cells(add_val).Value.ToString)
                    End If
                End If
            Else 'BINARY32/16
                Dim r As New System.Text.RegularExpressions.Regex( _
                 "^[-+]?\d+\.?\d*", _
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim v As System.Text.RegularExpressions.Match = r.Match(str)
                If v.Success AndAlso v.Value.Length = str.Length Then
                    Dim f As Single = Convert.ToSingle(v.Value)
                    Dim bit() As Byte = BitConverter.GetBytes(f)
                    Dim sb As New System.Text.StringBuilder()
                    Dim i As Integer = 3
                    While i >= 0
                        sb.Append(Convert.ToString(bit(i), 16).PadLeft(2, "0"c))
                        i -= 1
                    End While
                    Dim half As String = ""
                    If check = "BINARY32" Then
                        DataGridView1.Rows(d).Cells(add_val).Value = "0x" + sb.ToString.ToUpper
                    ElseIf check = "BIN32>>16" Then
                        If m.PSX = True AndAlso g_value.Checked = True Then
                            DataGridView1.Rows(d).Cells(add_val).Value = sb.ToString.Substring(0, 4).ToUpper
                        Else
                            half = DataGridView1.Rows(d).Cells(add_val).Value.ToString.Substring(0, 6)
                            DataGridView1.Rows(d).Cells(add_val).Value = half & sb.ToString.Substring(0, 4).ToUpper
                        End If
                    ElseIf check = "BINARY16" Then
                        Dim hf As String = sb.ToString
                        hf = converthalffloat(hf)
                        If m.PSX = True AndAlso g_value.Checked = True Then
                            DataGridView1.Rows(d).Cells(add_val).Value = hf
                        Else
                            half = DataGridView1.Rows(d).Cells(add_val).Value.ToString.Substring(0, 6)
                            DataGridView1.Rows(d).Cells(add_val).Value = half & hf
                        End If
                    End If

                Else
                    Label1.Text = "不正な値です"
                End If
            End If
        End If
        Dim gridtx As String = Nothing
        'Dim comment As String = ""
        Dim sl As New StringBuilder
        Dim sm As New StringBuilder
        Dim k As Integer = 0
        While k < DataGridView1.RowCount - 1 AndAlso DataGridView1.Rows(k).Cells(0).Value IsNot Nothing AndAlso DataGridView1.Rows(k).Cells(1).Value IsNot Nothing
            gridtx &= DataGridView1.Rows(k).Cells(0).Value.ToString & " "
            gridtx &= DataGridView1.Rows(k).Cells(1).Value.ToString & vbCrLf
            If Not DataGridView1.Rows(k).Cells(4).Value Is Nothing Then
                If DataGridView1.Rows(k).Cells(4).Value.ToString <> "" Then
                    sl.Append("<DGLINE" & Convert.ToString(k + 1) & "='" & DataGridView1.Rows(k).Cells(4).Value.ToString & "'>")
                End If
            End If
            If DataGridView1.Rows(k).Cells(2).Value.ToString.Contains("DEC") = False Then
                sm.Append("<DGMODE" & Convert.ToString(k + 1) & "='" & DataGridView1.Rows(k).Cells(2).Value.ToString & "'>")
            End If
            k += 1
        End While
        m.cl_tb.Text = gridtx
        m.dgtext.Text = sl.ToString
        m.dmtext.Text = sm.ToString()
        If My.Settings.gridsave = True Then
            m.save_cc_Click(sender, e)
        Else
            m.changed.Text = "データグリッドでコードが変更されてます"
        End If
    End Sub

    Private Sub DataGridView1_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellEnter

        Dim dgv As DataGridView = CType(sender, DataGridView)

        Dim Header As String = dgv.Columns(e.ColumnIndex).HeaderText


        If Header.Contains("備考") Then
            DataGridView1.ImeMode = Windows.Forms.ImeMode.NoControl
        Else
            DataGridView1.ImeMode = Windows.Forms.ImeMode.Disable

        End If

    End Sub

    Private Sub DataGridView1_Cellch(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged
        If DataGridView1.CurrentCell IsNot Nothing Then
            Dim d As Integer = DataGridView1.CurrentRow.Index
            Dim m As MERGE
            m = CType(Me.Owner, MERGE)
            If DataGridView1.Rows(d).Cells(0).Value Is Nothing Then
                If m.PSX = False Then
                    DataGridView1.Rows(d).Cells(0).Value = "0x00000000"
                Else
                    DataGridView1.Rows(d).Cells(0).Value = "00000000"
                End If
            End If
            If DataGridView1.Rows(d).Cells(1).Value Is Nothing Then
                If m.PSX = False Then
                    DataGridView1.Rows(d).Cells(1).Value = "0x00000000"
                Else
                    DataGridView1.Rows(d).Cells(1).Value = "0000"
                End If
            End If
            If DataGridView1.Rows(d).Cells(2).Value Is Nothing Then
                DataGridView1.Rows(d).Cells(2).Value = "DEC"
            End If
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gridsave.CheckedChanged

        If gridsave.Checked = True Then
            My.Settings.gridsave = True
        Else
            My.Settings.gridsave = False
        End If

    End Sub

    Private Sub 備考変換ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles notetag.Click
        Dim f As New gnote
        Dim k = 0
        Dim note As String = ""
        For k = 0 To DataGridView1.RowCount - 1
            If Not DataGridView1.Rows(k).Cells(4).Value Is Nothing Then
                note &= "<DGLINE" & Convert.ToString(k + 1) & "='" & DataGridView1.Rows(k).Cells(4).Value.ToString & "'>" & vbCrLf
            End If
        Next
        f.TextBox2.Text = note
        f.ShowDialog()


        Dim mask As String = "<DGLINE[0-9]{1,3}='.*?'>"
        Dim q As New System.Text.RegularExpressions.Regex( _
mask, _
System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim dg_comment As System.Text.RegularExpressions.Match = q.Match(f.TextBox2.Text)
        Dim b1 As String = ""
        Dim i, l, z, zz As Integer
        zz = DataGridView1.RowCount - 1
        While dg_comment.Success AndAlso i < zz
            k = dg_comment.Value.IndexOf("'") + 1
            z = dg_comment.Value.LastIndexOf("'")
            b1 = dg_comment.Value.Substring(0, k - 2)
            b1 = b1.Replace("<DGLINE", "")
            l = CInt(b1) - 1
            If l < zz AndAlso l <> -1 AndAlso k <= z Then
                DataGridView1.Rows(l).Cells(4).Value = dg_comment.Value.Substring(k, z - k)
            End If
            dg_comment = dg_comment.NextMatch()
            i += 1
        End While

        f.Dispose()
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As Object, _
        ByVal e As KeyEventArgs) Handles DataGridView1.KeyDown

        If (e.KeyData And Keys.Control) = Keys.Control Then
            If Not movedown.Text.Contains("☆") Then
                movedown.Text &= "☆"
                moveup.Text &= "☆"
            End If
        Else
            movedown.Text = movedown.Text.Replace("☆", "")
            moveup.Text = moveup.Text.Replace("☆", "")
        End If

    End Sub

    Private Sub TextBoxm_KeyDown(ByVal sender As Object, _
        ByVal e As KeyEventArgs) Handles DataGridView1.KeyUp

        movedown.Text = movedown.Text.Replace("☆", "")
        moveup.Text = moveup.Text.Replace("☆", "")
    End Sub

    Private Sub 上に移動ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles moveup.Click

        Dim d As Integer = DataGridView1.CurrentCell.RowIndex
        Dim row As DataGridViewRow = DataGridView1.Rows(d)
        Dim CloneWithValues = CType(row.Clone(), DataGridViewRow)
        Dim destination As Integer = d - 1
        If movedown.Text.Contains("☆") = True Then
            destination = 0
            DataGridView1.Rows.Insert(0)
        End If

        If d > 0 AndAlso d < DataGridView1.RowCount - 1 Then
            Dim row2 As DataGridViewRow = DataGridView1.Rows(destination)
            Dim CloneWithValues2 = CType(row2.Clone(), DataGridViewRow)
            For index As Int32 = 0 To row.Cells.Count - 1
                If movedown.Text.Contains("☆") = True Then
                    CloneWithValues.Cells(index).Value = row.Cells(index).Value
                    DataGridView1.Rows(destination).Cells(index).Value = CloneWithValues2.Cells(index).Value
                Else
                    CloneWithValues.Cells(index).Value = row.Cells(index).Value
                    CloneWithValues2.Cells(index).Value = row2.Cells(index).Value
                    DataGridView1.Rows(d).Cells(index).Value = CloneWithValues2.Cells(index).Value
                End If
                DataGridView1.Rows(destination).Cells(index).Value = CloneWithValues.Cells(index).Value
                DataGridView1.Rows(d).Selected = False
                DataGridView1.Rows(destination).Selected = True
                DataGridView1.CurrentCell = DataGridView1.Rows(destination).Cells(0)
                DataGridView1.Focus()
            Next

            If movedown.Text.Contains("☆") = True Then
                DataGridView1.Rows.RemoveAt(d + 1)
            End If

        End If


    End Sub

    Private Sub 下に移動ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles movedown.Click

        Dim d As Integer = DataGridView1.CurrentCell.RowIndex
        Dim row As DataGridViewRow = DataGridView1.Rows(d)
        Dim CloneWithValues = CType(row.Clone(), DataGridViewRow)
        Dim destination As Integer = d + 1

        If movedown.Text.Contains("☆") = True Then
            destination = DataGridView1.RowCount - 1
            DataGridView1.Rows.Insert(DataGridView1.RowCount - 1)
        End If

        If d < DataGridView1.RowCount - 2 Then
            Dim row2 As DataGridViewRow = DataGridView1.Rows(destination)
            Dim CloneWithValues2 = CType(row2.Clone(), DataGridViewRow)
            For index As Int32 = 0 To row.Cells.Count - 1
                If movedown.Text.Contains("☆") = True Then
                    CloneWithValues.Cells(index).Value = row.Cells(index).Value
                    DataGridView1.Rows(destination).Cells(index).Value = CloneWithValues2.Cells(index).Value
                Else
                    CloneWithValues.Cells(index).Value = row.Cells(index).Value
                    CloneWithValues2.Cells(index).Value = row2.Cells(index).Value
                    DataGridView1.Rows(d).Cells(index).Value = CloneWithValues2.Cells(index).Value
                End If
                DataGridView1.Rows(destination).Cells(index).Value = CloneWithValues.Cells(index).Value
                DataGridView1.Rows(d).Selected = False
                DataGridView1.Rows(destination).Selected = True
                DataGridView1.CurrentCell = DataGridView1.Rows(destination).Cells(0)
                DataGridView1.Focus()
            Next

            If movedown.Text.Contains("☆") = True Then
                DataGridView1.Rows.RemoveAt(d)
            End If
        End If
    End Sub

    Private Sub 行コード追加ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addline.Click

        Dim d As Integer = DataGridView1.CurrentCell.RowIndex
        Dim m As MERGE
        m = CType(Me.Owner, MERGE)

        If d < DataGridView1.RowCount - 1 Then
            DataGridView1.Rows.Insert(d + 1)
            For index As Int32 = 0 To 1
                If m.PSX = False Then
                    DataGridView1.Rows(d + 1).Cells(index).Value = "0x00000000"
                Else
                    DataGridView1.Rows(d + 1).Cells(0).Value = "00000000"
                    DataGridView1.Rows(d + 1).Cells(1).Value = "0000"
                End If
                DataGridView1.Rows(d + 1).Cells(2).Value = "DEC"
                DataGridView1.Rows(d).Selected = False
                DataGridView1.Rows(d + 1).Selected = True
                DataGridView1.CurrentCell = DataGridView1.Rows(d + 1).Cells(0)
                DataGridView1.Focus()
            Next
        End If
    End Sub

    Private Sub 行削除ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim d As Integer = DataGridView1.CurrentCell.RowIndex
        If d < DataGridView1.RowCount - 1 Then
            DataGridView1.Rows.RemoveAt(d)
        End If
        If d = DataGridView1.RowCount - 1 Then
            d -= 1
        End If
        DataGridView1.Rows(d).Selected = True
        DataGridView1.CurrentCell = DataGridView1.Rows(d).Cells(0)
        DataGridView1.Focus()
    End Sub

    Private Sub cut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cut.Click
        Try
            Dim d As Integer = DataGridView1.CurrentCell.RowIndex
            Dim row As DataGridViewRow
            Dim st As New StringBuilder
            Dim cursor(3000) As String
            Dim index(3000) As Integer
            Dim indexb As Integer() = Nothing
            Array.Resize(dgcp, 3000)
            Dim i As Integer = 0
            Dim jj As Integer = 0

            If d < DataGridView1.RowCount Then
                For Each r As DataGridViewRow In DataGridView1.SelectedRows
                    If r.Index < DataGridView1.RowCount - 1 Then
                        If jj = 0 Then
                            jj = r.Index
                        End If
                        row = DataGridView1.Rows(r.Index)
                        'st.Append("<DGLINE")
                        'st.Append((r.Index + 1).ToString.PadLeft(3, "0"c))
                        'st.Append("='")
                        'If Not row.Cells(4).Value Is Nothing Then
                        '    st.Append(row.Cells(4).Value.ToString)
                        'End If
                        'st.Append("'>")
                        'st.Append("<DGMODE")
                        'st.Append((r.Index + 1).ToString.PadLeft(3, "0"c))
                        'st.Append("='")
                        'st.Append(row.Cells(2).Value.ToString)
                        'st.Append("'>")
                        'st.Append("<DGVAL")
                        'st.Append((r.Index + 1).ToString.PadLeft(3, "0"c))
                        'st.Append("='")
                        'If Not row.Cells(3).Value Is Nothing Then
                        '    st.Append(row.Cells(3).Value.ToString)
                        'End If
                        'st.Append("'>")
                        'st.Append(vbCrLf)
                        st.Append(edmode)
                        st.Append(row.Cells(0).Value.ToString)
                        st.Append(" ")
                        st.AppendLine(row.Cells(1).Value.ToString)
                        cursor(i) = st.ToString
                        dgcp(i) = CloneWithValues(row)
                        index(i) = r.Index
                        st.Clear()
                        i += 1
                    End If
                Next r
                Array.Resize(cursor, i)
                Array.Resize(index, i)
                Array.Resize(indexb, i)
                Array.Resize(dgcp, i)
                Array.Copy(index, 0, indexb, 0, i)

                Array.Sort(index, cursor)
                Array.Sort(indexb, dgcp)

                For k = 0 To i - 1
                    DataGridView1.Rows.RemoveAt(index(0))
                Next


                For Each s As String In cursor
                    If s <> "" Then
                        st.Append(s)
                    End If
                Next
                If st.ToString <> "" Then
                    Clipboard.SetText(st.ToString)
                End If

                If cursor(0) <> Nothing Then
                    If d - i >= 0 Then
                        d = d - i
                    Else
                        d = jj
                    End If
                    If d >= DataGridView1.RowCount Then
                        d = DataGridView1.RowCount - 1
                    End If
                    DataGridView1.Rows(d).Selected = True
                    DataGridView1.CurrentCell = DataGridView1.Rows(d).Cells(0)
                    DataGridView1.Focus()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Dim dgcp As DataGridViewRow()

    Private Sub コピーToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles copy.Click, CPADV.Click
        Try
            Dim d As Integer = DataGridView1.CurrentCell.RowIndex
            Dim row As DataGridViewRow
            Dim st As New StringBuilder
            Dim cursor(3000) As String
            Dim index(3000) As Integer
            Dim indexb As Integer() = Nothing
            Array.Resize(dgcp, 3000)
            Dim i As Integer = 0
            Dim jj As Integer = 0

            If d < DataGridView1.RowCount Then
                For Each r As DataGridViewRow In DataGridView1.SelectedRows
                    If r.Index < DataGridView1.RowCount - 1 Then
                        If jj = 0 Then
                            jj = r.Index
                        End If
                        row = DataGridView1.Rows(r.Index)

                        'st.Append("<DGLINE")
                        'st.Append((r.Index + 1).ToString.PadLeft(3, "0"c))
                        'st.Append("='")
                        'If Not row.Cells(4).Value Is Nothing Then
                        '    st.Append(row.Cells(4).Value.ToString)
                        'End If
                        'st.Append("'>")
                        'st.Append("<DGMODE")
                        'st.Append((r.Index + 1).ToString.PadLeft(3, "0"c))
                        'st.Append("='")
                        'st.Append(row.Cells(2).Value.ToString)
                        'st.Append("'>")
                        'st.Append("<DGVAL")
                        'st.Append((r.Index + 1).ToString.PadLeft(3, "0"c))
                        'st.Append("='")
                        'If Not row.Cells(3).Value Is Nothing Then
                        '    st.Append(row.Cells(3).Value.ToString)
                        'End If
                        'st.Append("'>")
                        'st.Append(vbCrLf)

                        st.Append(edmode)
                        st.Append(row.Cells(0).Value.ToString)
                        st.Append(" ")
                        st.Append(row.Cells(1).Value.ToString)
                        If sender Is CPADV Then
                            st.Append(" #")
                            If Not row.Cells(3).Value Is Nothing Then
                                st.Append(row.Cells(3).Value.ToString)
                            End If
                            st.Append(",")
                            If Not row.Cells(4).Value Is Nothing Then
                                st.Append(row.Cells(4).Value.ToString)
                            End If
                        End If
                        st.AppendLine()
                        cursor(i) = st.ToString
                        dgcp(i) = CloneWithValues(row)
                        index(i) = r.Index
                        st.Clear()
                        i += 1
                    End If
                Next r
                Array.Resize(cursor, i)
                Array.Resize(index, i)
                Array.Resize(indexb, i)
                Array.Resize(dgcp, i)
                Array.Copy(index, 0, indexb, 0, i)

                Array.Sort(index, cursor)
                Array.Sort(indexb, dgcp)

                For Each s As String In cursor
                    If s <> "" Then
                        st.Append(s)
                    End If
                Next
                If st.ToString <> "" Then
                    Clipboard.SetText(st.ToString)
                End If

                DataGridView1.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub paste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles paste.Click

        Dim m As MERGE
        m = CType(Me.Owner, MERGE)
        Dim d As Integer = DataGridView1.CurrentCell.RowIndex
        Dim i As Integer = 0
        Dim r As New System.Text.RegularExpressions.Regex("[0-9a-fA-F]{8}", RegularExpressions.RegexOptions.ECMAScript)
        Dim hex As System.Text.RegularExpressions.Match = r.Match(Clipboard.GetText)
        Dim psx As New System.Text.RegularExpressions.Regex("[0-9a-fA-F]\x20[0-9a-fA-F]{4}", RegularExpressions.RegexOptions.ECMAScript)
        Dim phex As System.Text.RegularExpressions.Match = psx.Match(Clipboard.GetText)
        'Dim dg As New System.Text.RegularExpressions.Regex("<DGLINE[0-9]{3}='.*?'>", RegularExpressions.RegexOptions.ECMAScript)
        'Dim line As System.Text.RegularExpressions.Match = dg.Match(Clipboard.GetText)
        'Dim dm As New System.Text.RegularExpressions.Regex("<DGMODE[0-9]{3}='.*?'>", RegularExpressions.RegexOptions.ECMAScript)
        'Dim dmm As System.Text.RegularExpressions.Match = dm.Match(Clipboard.GetText)
        'Dim dv As New System.Text.RegularExpressions.Regex("<DGVAL[0-9]{3}='.*?'>", RegularExpressions.RegexOptions.ECMAScript)
        'Dim dvm As System.Text.RegularExpressions.Match = dv.Match(Clipboard.GetText)
        If dgcp IsNot Nothing Then
            Dim z As Integer = DataGridView1.RowCount - 1
            Dim a As Integer = DataGridView1.CurrentCell.RowIndex
            Dim b As Integer = DataGridView1.FirstDisplayedCell.RowIndex
            Dim cp As Integer = dgcp.Length

            Dim list As DataGridViewRow() = Nothing
            Array.Resize(list, 3000)
            For i = 0 To z - 1
                list(i) = CloneWithValues(DataGridView1.Rows(i))
            Next
            Array.Resize(list, z + cp)
            Array.ConstrainedCopy(list, d, list, d + cp, z - d)
            Array.Copy(dgcp, 0, list, d, cp)
            DataGridView1.Rows.Clear()
            DataGridView1.Rows.AddRange(list)
            
            DataGridView1.CurrentCell = DataGridView1(0, a)
            DataGridView1.FirstDisplayedCell = DataGridView1(0, b)

        Else
            'クリップボード
            If d < DataGridView1.RowCount AndAlso (hex.Success Or (m.PSX = True AndAlso phex.Success AndAlso hex.Success = True)) Then
                While hex.Success
                    DataGridView1.Rows.Insert(d + i)
                    If m.PSX = False Then
                        DataGridView1.Rows(d + i).Cells(0).Value = "0x" & hex.Value
                        hex = hex.NextMatch
                        DataGridView1.Rows(d + i).Cells(1).Value = "0x" & hex.Value
                    Else
                        DataGridView1.Rows(d + i).Cells(0).Value = hex.Value
                        DataGridView1.Rows(d + i).Cells(1).Value = phex.Value.Remove(0, 2)
                        phex = phex.NextMatch
                    End If
                    hex = hex.NextMatch

                    'DataGridView1.Rows(d + i).Cells(2).Value = "DEC"

                    'Dim k As Integer = dmm.Value.IndexOf("'") + 1
                    'Dim z As Integer = dmm.Value.LastIndexOf("'")
                    'If k <= z Then
                    '    DataGridView1.Rows(d + i).Cells(2).Value = dmm.Value.Substring(k, z - k)
                    'End If

                    'k = line.Value.IndexOf("'") + 1
                    'z = line.Value.LastIndexOf("'")
                    'If k <= z Then
                    '    DataGridView1.Rows(d + i).Cells(4).Value = line.Value.Substring(k, z - k)
                    'End If

                    'k = dvm.Value.IndexOf("'") + 1
                    'z = dvm.Value.LastIndexOf("'")
                    'If k <= z Then
                    '    DataGridView1.Rows(d + i).Cells(3).Value = dvm.Value.Substring(k, z - k)
                    'End If


                    'line = line.NextMatch
                    'dmm = dmm.NextMatch
                    'dvm = dvm.NextMatch
                    i += 1
                End While
            End If
            DataGridView1.Rows(d).Selected = True
            DataGridView1.CurrentCell = DataGridView1.Rows(d).Cells(0)
            DataGridView1.Focus()
        End If
    End Sub


    Private Sub ComboBox1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Try

            Dim add_val As Integer = 1
            If g_address.Checked = True Then
                add_val = 0
            End If
            Dim start As DateTime = Now
            Dim parse As String = ComboBox1.Text
            Dim z As Integer = DataGridView1.RowCount - 1
            Dim a As Integer = DataGridView1.CurrentCell.RowIndex
            Dim b As Integer = DataGridView1.FirstDisplayedCell.RowIndex

            Dim list As DataGridViewRow() = Nothing
            Array.Resize(list, 3000)
            For i = 0 To z - 1
                list(i) = CloneWithValues(DataGridView1.Rows(i))
                list(i).Cells(2).Value = parse
                list = hex2str_rows(parse, i, list, add_val)
            Next
            Array.Resize(list, z)
            DataGridView1.Rows.Clear()
            DataGridView1.Rows.AddRange(list)

            DataGridView1.CurrentCell = DataGridView1(0, a)
            DataGridView1.FirstDisplayedCell = DataGridView1(0, b)

            timer.Text = (Now - start).TotalSeconds.ToString
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Function CloneWithValues(ByVal row As DataGridViewRow) _
    As DataGridViewRow
        CloneWithValues = CType(row.Clone(), DataGridViewRow)
        For index = 0 To 4
            CloneWithValues.Cells(index).Value = row.Cells(index).Value
        Next

    End Function

    Dim dgaddress As String

    Function hex2str_rows(ByVal float As String, ByVal l As Integer, ByVal list As DataGridViewRow(), ByVal x As Integer) As DataGridViewRow()
        If float = "DEC" Then
            list(l).Cells(3).Value = (Convert.ToInt64(list(l).Cells(x).Value.ToString, 16) And &HFFFFFFFF).ToString
        ElseIf float = "DEC16BIT" Then
            list(l).Cells(3).Value = (Convert.ToInt64(list(l).Cells(x).Value.ToString, 16) And &HFFFF).ToString
        ElseIf float = "BINARY32" Then
            Dim bytes As Byte() = str2bin(list(l).Cells(x).Value.ToString)
            If (bytes(3) And &H7F) > &H31 AndAlso (bytes(3) And &H7F) < &H52 Then
                list(l).Cells(3).Value = BitConverter.ToSingle(bytes, 0)
            End If
        ElseIf float = "BIN32>>16" Then
            Dim ss As String = list(l).Cells(x).Value.ToString
            Dim bytes As Byte() = str2bin(ss.Remove(0, ss.Length - 4).PadRight(8, "0"c))
            If (bytes(3) And &H7F) > &H31 AndAlso (bytes(3) And &H7F) < &H52 Then
                list(l).Cells(3).Value = BitConverter.ToSingle(bytes, 0)
            End If
        ElseIf float = "BINARY16" Then
            Dim ss As String = list(l).Cells(x).Value.ToString
            Dim bytes As Byte() = str2bin(ss.Remove(0, ss.Length - 4).PadRight(8, "0"c))
            Array.ConstrainedCopy(bytes, 2, bytes, 0, 2)
            Array.Resize(bytes, 2)
            If (bytes(1) And &H7F) < &H7C Then
                Dim bytes2 As Byte() = str2bin(converthalffloat2(bytes))
                list(l).Cells(3).Value = BitConverter.ToSingle(bytes2, 0)
            End If
        ElseIf float = "ASM" Then
            list(l).Cells(3).Value = decoders(list(l).Cells(x).Value.ToString, l)
            dgaddress = list(l).Cells(0).Value.ToString
        End If
        Return list
    End Function

    Function str2bin(ByVal temp As String) As Byte()
        temp = temp.Replace("0x", "")
        Dim num(3) As Integer
        Dim bytes(3) As Byte
        For i = 0 To 3
            num(i) = Convert.ToInt32(temp.Substring(6 - 2 * i, 2), 16)
            bytes(i) = Convert.ToByte(num(i))
        Next
        Return bytes
    End Function

    'IEE754単精度浮動小数点binary32を半精度浮動小数点binary16に変換 Cから移植、VB.NET
    Function converthalffloat(ByVal hf As String) As String
        Dim hex As Integer = Convert.ToInt32(hf, 16)
        Dim sign As Integer = (hex >> 31) And 1
        Dim exponent As Integer = (hex >> 23) And &HFF
        Dim fraction As Integer = (hex And &H7FFFFF)

        '        WebSVN()
        'psp - Rev 2457
        '        Subversion(Repositories)
        'Rev:
        '(root)/trunk/prxtool/disasm.C @ 2457
        'Rev 2206 - Blame - Compare with Previous - Last modification - View Log - RSS feed

        '/***************************************************************
        ' * PRXTool : Utility for PSP executables.
        ' * (c) TyRaNiD 2k6
        ' *
        ' * disasm.C - Implementation of a MIPS disassembler
        ' ***************************************************************/
        '/* VFPU 16-bit floating-point format. */ from psplinksource
        '#define VFPU_FLOAT16_EXP_MAX    0x1f
        '#define VFPU_SH_FLOAT16_SIGN    15
        '#define VFPU_MASK_FLOAT16_SIGN  0x1
        '#define VFPU_SH_FLOAT16_EXP     10
        '#define VFPU_MASK_FLOAT16_EXP   0x1f
        '#define VFPU_SH_FLOAT16_FRAC    0
        '#define VFPU_MASK_FLOAT16_FRAC  0x3ff
        '        /* Convert a VFPU 16-bit floating-point number to IEEE754. */
        '        unsigned int float2int=0;
        '        unsigned short float16 = addresscode & 0xFFFF;
        '        unsigned int sign = (float16 >> VFPU_SH_FLOAT16_SIGN) & VFPU_MASK_FLOAT16_SIGN;
        '        int exponent = (float16 >> VFPU_SH_FLOAT16_EXP) & VFPU_MASK_FLOAT16_EXP;
        '        unsigned int fraction = float16 & VFPU_MASK_FLOAT16_FRAC;
        '        float2int = (sign << 31) + ((exponent + 112) << 23) + (fraction << 13);
        exponent -= 112
        exponent <<= 10
        fraction >>= 13
        sign <<= 15
        hex = exponent + fraction
        hex = hex And (&H7FFF)
        If hex > &H7C00 Then '無限
            hex = &H7F80 '数字以外のなにか
        End If
        hex += sign
        hf = hex.ToString("X").PadLeft(4, "0"c)

        Return hf
    End Function

    'IEE754単精度浮動小数点binary32を半精度浮動小数点binary16に変換の逆 Cから移植、VB.NET
    Function converthalffloat2(ByVal b As Byte()) As String
        Dim hex As Integer = BitConverter.ToInt16(b, 0)
        Dim sign As Integer = (hex >> 15) And 1
        Dim exponent As Integer = (hex >> 10) And &H1F
        Dim fraction As Integer = (hex And &H3FF)
        Dim hf As String
        '        WebSVN()
        'psp - Rev 2457
        '        Subversion(Repositories)
        'Rev:
        '(root)/trunk/prxtool/disasm.C @ 2457
        'Rev 2206 - Blame - Compare with Previous - Last modification - View Log - RSS feed

        '/***************************************************************
        ' * PRXTool : Utility for PSP executables.
        ' * (c) TyRaNiD 2k6
        ' *
        ' * disasm.C - Implementation of a MIPS disassembler
        ' ***************************************************************/
        '/* VFPU 16-bit floating-point format. */ from psplinksource
        '#define VFPU_FLOAT16_EXP_MAX    0x1f
        '#define VFPU_SH_FLOAT16_SIGN    15
        '#define VFPU_MASK_FLOAT16_SIGN  0x1
        '#define VFPU_SH_FLOAT16_EXP     10
        '#define VFPU_MASK_FLOAT16_EXP   0x1f
        '#define VFPU_SH_FLOAT16_FRAC    0
        '#define VFPU_MASK_FLOAT16_FRAC  0x3ff
        '        /* Convert a VFPU 16-bit floating-point number to IEEE754. */
        '        unsigned int float2int=0;
        '        unsigned short float16 = addresscode & 0xFFFF;
        '        unsigned int sign = (float16 >> VFPU_SH_FLOAT16_SIGN) & VFPU_MASK_FLOAT16_SIGN;
        '        int exponent = (float16 >> VFPU_SH_FLOAT16_EXP) & VFPU_MASK_FLOAT16_EXP;
        '        unsigned int fraction = float16 & VFPU_MASK_FLOAT16_FRAC;
        '        float2int = (sign << 31) + ((exponent + 112) << 23) + (fraction << 13);
        exponent += 112
        exponent <<= 23
        fraction <<= 13
        sign <<= 31
        hex = exponent + fraction
        hex = hex And &H7FFFFFFF
        hex += sign
        hf = hex.ToString("X").PadLeft(8, "0"c)

        Return hf
    End Function

    'decoder 配列
#Region "decoder"
    Dim decoder As String() = {"nop", "0x00000000", "0xFFFFFFFF", "", _
"li", "0x24000000", "0xFFE00000", "%t,%i", _
"li", "0x34000000", "0xFFE00000", "%t,%I", _
"move", "0x00000021", "0xFC1F07FF", "%d,%s", _
"move", "0x00000025", "0xFC1F07FF", "%d,%s", _
"b", "0x10000000", "0xFFFF0000", "%O", _
"b", "0x04010000", "0xFFFF0000", "%O", _
"bal", "0x04110000", "0xFFFF0000", "%O", _
"bnez", "0x14000000", "0xFC1F0000", "%s,%O", _
"bnezl", "0x54000000", "0xFC1F0000", "%s,%O", _
"beqz", "0x10000000", "0xFC1F0000", "%s,%O", _
"beqzl", "0x50000000", "0xFC1F0000", "%s,%O", _
"neg", "0x00000022", "0xFFE007FF", "%d,%t", _
"negu", "0x00000023", "0xFFE007FF", "%d,%t", _
"not", "0x00000027", "0xFC1F07FF", "%d,%s", _
"jalr", "0x0000F809", "0xFC1FFFFF", "%J", _
"add", "0x00000020", "0xFC0007FF", "%d,%s,%t", _
"addi", "0x20000000", "0xFC000000", "%t,%s,%i", _
"addiu", "0x24000000", "0xFC000000", "%t,%s,%i", _
"addu", "0x00000021", "0xFC0007FF", "%d,%s,%t", _
"and", "0x00000024", "0xFC0007FF", "%d,%s,%t", _
"andi", "0x30000000", "0xFC000000", "%t,%s,%I", _
"beq", "0x10000000", "0xFC000000", "%s,%t,%O", _
"beql", "0x50000000", "0xFC000000", "%s,%t,%O", _
"bgez", "0x04010000", "0xFC1F0000", "%s,%O", _
"bgezal", "0x04110000", "0xFC1F0000", "%s,%O", _
"bgezl", "0x04030000", "0xFC1F0000", "%s,%O", _
"bgtz", "0x1C000000", "0xFC1F0000", "%s,%O", _
"bgtzl", "0x5C000000", "0xFC1F0000", "%s,%O", _
"bitrev", "0x7C000520", "0xFFE007FF", "%d,%t", _
"blez", "0x18000000", "0xFC1F0000", "%s,%O", _
"blezl", "0x58000000", "0xFC1F0000", "%s,%O", _
"bltz", "0x04000000", "0xFC1F0000", "%s,%O", _
"bltzl", "0x04020000", "0xFC1F0000", "%s,%O", _
"bltzal", "0x04100000", "0xFC1F0000", "%s,%O", _
"bltzall", "0x04120000", "0xFC1F0000", "%s,%O", _
"bne", "0x14000000", "0xFC000000", "%s,%t,%O", _
"bnel", "0x54000000", "0xFC000000", "%s,%t,%O", _
"break", "0x0000000D", "0xFC00003F", "%c", _
"cache", "0xbc000000", "0xfc000000", "%k,%o", _
"cfc0", "0x40400000", "0xFFE007FF", "%t,%p", _
"clo", "0x00000017", "0xFC1F07FF", "%d,%s", _
"clz", "0x00000016", "0xFC1F07FF", "%d,%s", _
"ctc0", "0x40C00000", "0xFFE007FF", "%t,%p", _
"max", "0x0000002C", "0xFC0007FF", "%d,%s,%t", _
"min", "0x0000002D", "0xFC0007FF", "%d,%s,%t", _
"dbreak", "0x7000003F", "0xFFFFFFFF", "", _
"div", "0x0000001A", "0xFC00FFFF", "%s,%t", _
"divu", "0x0000001B", "0xFC00FFFF", "%s,%t", _
"dret", "0x7000003E", "0xFFFFFFFF", "", _
"eret", "0x42000018", "0xFFFFFFFF", "", _
"ext", "0x7C000000", "0xFC00003F", "%t,%s,%a,%ne", _
"ins", "0x7C000004", "0xFC00003F", "%t,%s,%a,%ni", _
"j", "0x08000000", "0xFC000000", "%j", _
"jr", "0x00000008", "0xFC1FFFFF", "%J", _
"jalr", "0x00000009", "0xFC1F07FF", "%d,%J", _
"jal", "0x0C000000", "0xFC000000", "%j", _
"lb", "0x80000000", "0xFC000000", "%t,%o", _
"lbu", "0x90000000", "0xFC000000", "%t,%o", _
"lh", "0x84000000", "0xFC000000", "%t,%o", _
"lhu", "0x94000000", "0xFC000000", "%t,%o", _
"ll", "0xC0000000", "0xFC000000", "%t,%o", _
"lui", "0x3C000000", "0xFFE00000", "%t,%I", _
"lw", "0x8C000000", "0xFC000000", "%t,%o", _
"lwl", "0x88000000", "0xFC000000", "%t,%o", _
"lwr", "0x98000000", "0xFC000000", "%t,%o", _
"madd", "0x0000001C", "0xFC00FFFF", "%s,%t", _
"maddu", "0x0000001D", "0xFC00FFFF", "%s,%t", _
"mfc0", "0x40000000", "0xFFE007FF", "%t,%0", _
"mfdr", "0x7000003D", "0xFFE007FF", "%t,%r", _
"mfhi", "0x00000010", "0xFFFF07FF", "%d", _
"mfic", "0x70000024", "0xFFE007FF", "%t,%p", _
"mflo", "0x00000012", "0xFFFF07FF", "%d", _
"movn", "0x0000000B", "0xFC0007FF", "%d,%s,%t", _
"movz", "0x0000000A", "0xFC0007FF", "%d,%s,%t", _
"msub", "0x0000002e", "0xfc00ffff", "%s,%t", _
"msubu", "0x0000002f", "0xfc00ffff", "%s,%t", _
"mtc0", "0x40800000", "0xFFE007FF", "%t,%0", _
"mtdr", "0x7080003D", "0xFFE007FF", "%t,%r", _
"mtic", "0x70000026", "0xFFE007FF", "%t,%p", _
"halt", "0x70000000", "0xFFFFFFFF", "", _
"mthi", "0x00000011", "0xFC1FFFFF", "%s", _
"mtlo", "0x00000013", "0xFC1FFFFF", "%s", _
"mult", "0x00000018", "0xFC00FFFF", "%s,%t", _
"multu", "0x00000019", "0xFC0007FF", "%s,%t", _
"nor", "0x00000027", "0xFC0007FF", "%d,%s,%t", _
"or", "0x00000025", "0xFC0007FF", "%d,%s,%t", _
"ori", "0x34000000", "0xFC000000", "%t,%s,%I", _
"rotr", "0x00200002", "0xFFE0003F", "%d,%t,%a", _
"rotv", "0x00000046", "0xFC0007FF", "%d,%t,%s", _
"seb", "0x7C000420", "0xFFE007FF", "%d,%t", _
"seh", "0x7C000620", "0xFFE007FF", "%d,%t", _
"sb", "0xA0000000", "0xFC000000", "%t,%o", _
"sh", "0xA4000000", "0xFC000000", "%t,%o", _
"sc", "0xE0000000", "0xFC000000", "%t, %o", _
"sllv", "0x00000004", "0xFC0007FF", "%d,%t,%s", _
"sll", "0x00000000", "0xFFE0003F", "%d,%t,%a", _
"slt", "0x0000002A", "0xFC0007FF", "%d,%s,%t", _
"slti", "0x28000000", "0xFC000000", "%t,%s,%i", _
"sltiu", "0x2C000000", "0xFC000000", "%t,%s,%I", _
"sltu", "0x0000002B", "0xFC0007FF", "%d,%s,%t", _
"sra", "0x00000003", "0xFFE0003F", "%d,%t,%a", _
"srav", "0x00000007", "0xFC0007FF", "%d,%t,%s", _
"srlv", "0x00000006", "0xFC0007FF", "%d,%t,%s", _
"srl", "0x00000002", "0xFFE0003F", "%d,%t,%a", _
"sw", "0xAC000000", "0xFC000000", "%t,%o", _
"swl", "0xA8000000", "0xFC000000", "%t,%o", _
"swr", "0xB8000000", "0xFC000000", "%t,%o", _
"sub", "0x00000022", "0xFC0007FF", "%d,%s,%t", _
"subu", "0x00000023", "0xFC0007FF", "%d,%s,%t", _
"sync", "0x0000000F", "0xFFFFFFFF", "", _
"syscall", "0x0000000C", "0xFC00003F", "%C", _
"xor", "0x00000026", "0xFC0007FF", "%d,%s,%t", _
"xori", "0x38000000", "0xFC000000", "%t,%s,%I", _
"wsbh", "0x7C0000A0", "0xFFE007FF", "%d,%t", _
"wsbw", "0x7C0000E0", "0xFFE007FF", "%d,%t", _
"abs.s", "0x46000005", "0xFFFF003F", "%D,%S", _
"add.s", "0x46000000", "0xFFE0003F", "%D,%S,%T", _
"bc1f", "0x45000000", "0xFFFF0000", "%O", _
"bc1fl", "0x45020000", "0xFFFF0000", "%O", _
"bc1t", "0x45010000", "0xFFFF0000", "%O", _
"bc1tl", "0x45030000", "0xFFFF0000", "%O", _
"c.f.s", "0x46000030", "0xFFE007FF", "%S,%T", _
"c.un.s", "0x46000031", "0xFFE007FF", "%S,%T", _
"c.eq.s", "0x46000032", "0xFFE007FF", "%S,%T", _
"c.ueq.s", "0x46000033", "0xFFE007FF", "%S,%T", _
"c.olt.s", "0x46000034", "0xFFE007FF", "%S,%T", _
"c.ult.s", "0x46000035", "0xFFE007FF", "%S,%T", _
"c.ole.s", "0x46000036", "0xFFE007FF", "%S,%T", _
"c.ule.s", "0x46000037", "0xFFE007FF", "%S,%T", _
"c.sf.s", "0x46000038", "0xFFE007FF", "%S,%T", _
"c.ngle.s", "0x46000039", "0xFFE007FF", "%S,%T", _
"c.seq.s", "0x4600003A", "0xFFE007FF", "%S,%T", _
"c.ngl.s", "0x4600003B", "0xFFE007FF", "%S,%T", _
"c.lt.s", "0x4600003C", "0xFFE007FF", "%S,%T", _
"c.nge.s", "0x4600003D", "0xFFE007FF", "%S,%T", _
"c.le.s", "0x4600003E", "0xFFE007FF", "%S,%T", _
"c.ngt.s", "0x4600003F", "0xFFE007FF", "%S,%T", _
"ceil.w.s", "0x4600000E", "0xFFFF003F", "%D,%S", _
"cfc1", "0x44400000", "0xFFE007FF", "%t,%p", _
"ctc1", "0x44c00000", "0xFFE007FF", "%t,%p", _
"cvt.s.w", "0x46800020", "0xFFFF003F", "%D,%S", _
"cvt.w.s", "0x46000024", "0xFFFF003F", "%D,%S", _
"div.s", "0x46000003", "0xFFE0003F", "%D,%S,%T", _
"floor.w.s", "0x4600000F", "0xFFFF003F", "%D,%S", _
"lwc1", "0xc4000000", "0xFC000000", "%T,%o", _
"mfc1", "0x44000000", "0xFFE007FF", "%t,%1", _
"mov.s", "0x46000006", "0xFFFF003F", "%D,%S", _
"mtc1", "0x44800000", "0xFFE007FF", "%t,%1", _
"mul.s", "0x46000002", "0xFFE0003F", "%D,%S,%T", _
"neg.s", "0x46000007", "0xFFFF003F", "%D,%S", _
"round.w.s", "0x4600000C", "0xFFFF003F", "%D,%S", _
"sqrt.s", "0x46000004", "0xFFFF003F", "%D,%S", _
"sub.s", "0x46000001", "0xFFE0003F", "%D,%S,%T", _
"swc1", "0xe4000000", "0xFC000000", "%T,%o", _
"trunc.w.s", "0x4600000D", "0xFFFF003F", "%D,%S", _
"bvf", "0x49000000", "0xFFE30000", "%Zc,%O", _
"bvfl", "0x49020000", "0xFFE30000", "%Zc,%O", _
"bvt", "0x49010000", "0xFFE30000", "%Zc,%O", _
"bvtl", "0x49030000", "0xFFE30000", "%Zc,%O", _
"lv.q", "0xD8000000", "0xFC000002", "%Xq,%Y", _
"lv.s", "0xC8000000", "0xFC000000", "%Xs,%Y", _
"lvl.q", "0xD4000000", "0xFC000002", "%Xq,%Y", _
"lvr.q", "0xD4000002", "0xFC000002", "%Xq,%Y", _
"mfv", "0x48600000", "0xFFE0FF80", "%t,%zs", _
"mfvc", "0x48600000", "0xFFE0FF00", "%t,%2d", _
"mtv", "0x48E00000", "0xFFE0FF80", "%t,%zs", _
"mtvc", "0x48E00000", "0xFFE0FF00", "%t,%2d", _
"sv.q", "0xF8000000", "0xFC000002", "%Xq,%Y", _
"sv.s", "0xE8000000", "0xFC000000", "%Xs,%Y", _
"svl.q", "0xF4000000", "0xFC000002", "%Xq,%Y", _
"svr.q", "0xF4000002", "0xFC000002", "%Xq,%Y", _
"vabs.p", "0xD0010080", "0xFFFF8080", "%zp,%yp", _
"vabs.q", "0xD0018080", "0xFFFF8080", "%zq,%yq", _
"vabs.s", "0xD0010000", "0xFFFF8080", "%zs,%ys", _
"vabs.t", "0xD0018000", "0xFFFF8080", "%zt,%yt", _
"vadd.p", "0x60000080", "0xFF808080", "%zp,%yp,%xp", _
"vadd.q", "0x60008080", "0xFF808080", "%zq,%yq,%xq", _
"vadd.s", "0x60000000", "0xFF808080", "%zs,%ys,%xs", _
"vadd.t", "0x60008000", "0xFF808080", "%zt,%yt,%xt", _
"vasin.p", "0xD0170080", "0xFFFF8080", "%zp,%yp", _
"vasin.q", "0xD0178080", "0xFFFF8080", "%zq,%yq", _
"vasin.s", "0xD0170000", "0xFFFF8080", "%zs,%ys", _
"vasin.t", "0xD0178000", "0xFFFF8080", "%zt,%yt", _
"vavg.p", "0xD0470080", "0xFFFF8080", "%zp,%yp", _
"vavg.q", "0xD0478080", "0xFFFF8080", "%zq,%yq", _
"vavg.t", "0xD0478000", "0xFFFF8080", "%zt,%yt", _
"vbfy1.p", "0xD0420080", "0xFFFF8080", "%zp,%yp", _
"vbfy1.q", "0xD0428080", "0xFFFF8080", "%zq,%yq", _
"vbfy2.q", "0xD0438080", "0xFFFF8080", "%zq,%yq", _
"vcmovf.p", "0xD2A80080", "0xFFF88080", "%zp,%yp,%v3", _
"vcmovf.q", "0xD2A88080", "0xFFF88080", "%zq,%yq,%v3", _
"vcmovf.s", "0xD2A80000", "0xFFF88080", "%zs,%ys,%v3", _
"vcmovf.t", "0xD2A88000", "0xFFF88080", "%zt,%yt,%v3", _
"vcmovt.p", "0xD2A00080", "0xFFF88080", "%zp,%yp,%v3", _
"vcmovt.q", "0xD2A08080", "0xFFF88080", "%zq,%yq,%v3", _
"vcmovt.s", "0xD2A00000", "0xFFF88080", "%zs,%ys,%v3", _
"vcmovt.t", "0xD2A08000", "0xFFF88080", "%zt,%yt,%v3", _
"vcmp.p", "0x6C000080", "0xFF8080F0", "%Zn,%yp,%xp", _
"vcmp.p", "0x6C000080", "0xFFFF80F0", "%Zn,%yp", _
"vcmp.p", "0x6C000080", "0xFFFFFFF0", "%Zn", _
"vcmp.q", "0x6C008080", "0xFF8080F0", "%Zn,%yq,%xq", _
"vcmp.q", "0x6C008080", "0xFFFF80F0", "%Zn,%yq", _
"vcmp.q", "0x6C008080", "0xFFFFFFF0", "%Zn", _
"vcmp.s", "0x6C000000", "0xFF8080F0", "%Zn,%ys,%xs", _
"vcmp.s", "0x6C000000", "0xFFFF80F0", "%Zn,%ys", _
"vcmp.s", "0x6C000000", "0xFFFFFFF0", "%Zn", _
"vcmp.t", "0x6C008000", "0xFF8080F0", "%Zn,%yt,%xt", _
"vcmp.t", "0x6C008000", "0xFFFF80F0", "%Zn,%yt", _
"vcmp.t", "0x6C008000", "0xFFFFFFF0", "%Zn", _
"vcos.p", "0xD0130080", "0xFFFF8080", "%zp,%yp", _
"vcos.q", "0xD0138080", "0xFFFF8080", "%zq,%yq", _
"vcos.s", "0xD0130000", "0xFFFF8080", "%zs,%ys", _
"vcos.t", "0xD0138000", "0xFFFF8080", "%zt,%yt", _
"vcrs.t", "0x66808000", "0xFF808080", "%zt,%yt,%xt", _
"vcrsp.t", "0xF2808000", "0xFF808080", "%zt,%yt,%xt", _
"vcst.p", "0xD0600080", "0xFFE0FF80", "%zp,%vk", _
"vcst.q", "0xD0608080", "0xFFE0FF80", "%zq,%vk", _
"vcst.s", "0xD0600000", "0xFFE0FF80", "%zs,%vk", _
"vcst.t", "0xD0608000", "0xFFE0FF80", "%zt,%vk", _
"vdet.p", "0x67000080", "0xFF808080", "%zs,%yp,%xp", _
"vdiv.p", "0x63800080", "0xFF808080", "%zp,%yp,%xp", _
"vdiv.q", "0x63808080", "0xFF808080", "%zq,%yq,%xq", _
"vdiv.s", "0x63800000", "0xFF808080", "%zs,%ys,%xs", _
"vdiv.t", "0x63808000", "0xFF808080", "%zt,%yt,%xt", _
"vdot.p", "0x64800080", "0xFF808080", "%zs,%yp,%xp", _
"vdot.q", "0x64808080", "0xFF808080", "%zs,%yq,%xq", _
"vdot.t", "0x64808000", "0xFF808080", "%zs,%yt,%xt", _
"vexp2.p", "0xD0140080", "0xFFFF8080", "%zp,%yp", _
"vexp2.q", "0xD0148080", "0xFFFF8080", "%zq,%yq", _
"vexp2.s", "0xD0140000", "0xFFFF8080", "%zs,%ys", _
"vexp2.t", "0xD0148000", "0xFFFF8080", "%zt,%yt", _
"vf2h.p", "0xD0320080", "0xFFFF8080", "%zs,%yp", _
"vf2h.q", "0xD0328080", "0xFFFF8080", "%zp,%yq", _
"vf2id.p", "0xD2600080", "0xFFE08080", "%zp,%yp,%v5", _
"vf2id.q", "0xD2608080", "0xFFE08080", "%zq,%yq,%v5", _
"vf2id.s", "0xD2600000", "0xFFE08080", "%zs,%ys,%v5", _
"vf2id.t", "0xD2608000", "0xFFE08080", "%zt,%yt,%v5", _
"vf2in.p", "0xD2000080", "0xFFE08080", "%zp,%yp,%v5", _
"vf2in.q", "0xD2008080", "0xFFE08080", "%zq,%yq,%v5", _
"vf2in.s", "0xD2000000", "0xFFE08080", "%zs,%ys,%v5", _
"vf2in.t", "0xD2008000", "0xFFE08080", "%zt,%yt,%v5", _
"vf2iu.p", "0xD2400080", "0xFFE08080", "%zp,%yp,%v5", _
"vf2iu.q", "0xD2408080", "0xFFE08080", "%zq,%yq,%v5", _
"vf2iu.s", "0xD2400000", "0xFFE08080", "%zs,%ys,%v5", _
"vf2iu.t", "0xD2408000", "0xFFE08080", "%zt,%yt,%v5", _
"vf2iz.p", "0xD2200080", "0xFFE08080", "%zp,%yp,%v5", _
"vf2iz.q", "0xD2208080", "0xFFE08080", "%zq,%yq,%v5", _
"vf2iz.s", "0xD2200000", "0xFFE08080", "%zs,%ys,%v5", _
"vf2iz.t", "0xD2208000", "0xFFE08080", "%zt,%yt,%v5", _
"vfad.p", "0xD0460080", "0xFFFF8080", "%zp,%yp", _
"vfad.q", "0xD0468080", "0xFFFF8080", "%zq,%yq", _
"vfad.t", "0xD0468000", "0xFFFF8080", "%zt,%yt", _
"vfim.s", "0xDF800000", "0xFF800000", "%xs,%vh", _
"vflush", "0xFFFF040D", "0xFFFFFFFF", "", _
"vh2f.p", "0xD0330080", "0xFFFF8080", "%zq,%yp", _
"vh2f.s", "0xD0330000", "0xFFFF8080", "%zp,%ys", _
"vhdp.p", "0x66000080", "0xFF808080", "%zs,%yp,%xp", _
"vhdp.q", "0x66008080", "0xFF808080", "%zs,%yq,%xq", _
"vhdp.t", "0x66008000", "0xFF808080", "%zs,%yt,%xt", _
"vhtfm2.p", "0xF0800000", "0xFF808080", "%zp,%ym,%xp", _
"vhtfm3.t", "0xF1000080", "0xFF808080", "%zt,%yn,%xt", _
"vhtfm4.q", "0xF1808000", "0xFF808080", "%zq,%yo,%xq", _
"vi2c.q", "0xD03D8080", "0xFFFF8080", "%zs,%yq", _
"vi2f.p", "0xD2800080", "0xFFE08080", "%zp,%yp,%v5", _
"vi2f.q", "0xD2808080", "0xFFE08080", "%zq,%yq,%v5", _
"vi2f.s", "0xD2800000", "0xFFE08080", "%zs,%ys,%v5", _
"vi2f.t", "0xD2808000", "0xFFE08080", "%zt,%yt,%v5", _
"vi2s.p", "0xD03F0080", "0xFFFF8080", "%zs,%yp", _
"vi2s.q", "0xD03F8080", "0xFFFF8080", "%zp,%yq", _
"vi2uc.q", "0xD03C8080", "0xFFFF8080", "%zs,%yq", _
"vi2us.p", "0xD03E0080", "0xFFFF8080", "%zs,%yq", _
"vi2us.q", "0xD03E8080", "0xFFFF8080", "%zp,%yq", _
"vidt.p", "0xD0030080", "0xFFFFFF80", "%zp", _
"vidt.q", "0xD0038080", "0xFFFFFF80", "%zq", _
"viim.s", "0xDF000000", "0xFF800000", "%xs,%vi", _
"vlgb.s", "0xD0370000", "0xFFFF8080", "%zs,%ys", _
"vlog2.p", "0xD0150080", "0xFFFF8080", "%zp,%yp", _
"vlog2.q", "0xD0158080", "0xFFFF8080", "%zq,%yq", _
"vlog2.s", "0xD0150000", "0xFFFF8080", "%zs,%ys", _
"vlog2.t", "0xD0158000", "0xFFFF8080", "%zt,%yt", _
"vmax.p", "0x6D800080", "0xFF808080", "%zp,%yp,%xp", _
"vmax.q", "0x6D808080", "0xFF808080", "%zq,%yq,%xq", _
"vmax.s", "0x6D800000", "0xFF808080", "%zs,%ys,%xs", _
"vmax.t", "0x6D808000", "0xFF808080", "%zt,%yt,%xt", _
"vmfvc", "0xD0500000", "0xFFFF0080", "%zs,%2s", _
"vmidt.p", "0xF3830080", "0xFFFFFF80", "%zm", _
"vmidt.q", "0xF3838080", "0xFFFFFF80", "%zo", _
"vmidt.t", "0xF3838000", "0xFFFFFF80", "%zn", _
"vmin.p", "0x6D000080", "0xFF808080", "%zp,%yp,%xp", _
"vmin.q", "0x6D008080", "0xFF808080", "%zq,%yq,%xq", _
"vmin.s", "0x6D000000", "0xFF808080", "%zs,%ys,%xs", _
"vmin.t", "0x6D008000", "0xFF808080", "%zt,%yt,%xt", _
"vmmov.p", "0xF3800080", "0xFFFF8080", "%zm,%ym", _
"vmmov.q", "0xF3808080", "0xFFFF8080", "%zo,%yo", _
"vmmov.t", "0xF3808000", "0xFFFF8080", "%zn,%yn", _
"vmmul.p", "0xF0000080", "0xFF808080", "%?%zm,%ym,%xm", _
"vmmul.q", "0xF0008080", "0xFF808080", "%?%zo,%yo,%xo", _
"vmmul.t", "0xF0008000", "0xFF808080", "%?%zn,%yn,%xn", _
"vmone.p", "0xF3870080", "0xFFFFFF80", "%zp", _
"vmone.q", "0xF3878080", "0xFFFFFF80", "%zq", _
"vmone.t", "0xF3878000", "0xFFFFFF80", "%zt", _
"vmov.p", "0xD0000080", "0xFFFF8080", "%zp,%yp", _
"vmov.q", "0xD0008080", "0xFFFF8080", "%zq,%yq", _
"vmov.s", "0xD0000000", "0xFFFF8080", "%zs,%ys", _
"vmov.t", "0xD0008000", "0xFFFF8080", "%zt,%yt", _
"vmscl.p", "0xF2000080", "0xFF808080", "%zm,%ym,%xs", _
"vmscl.q", "0xF2008080", "0xFF808080", "%zo,%yo,%xs", _
"vmscl.t", "0xF2008000", "0xFF808080", "%zn,%yn,%xs", _
"vmtvc", "0xD0510000", "0xFFFF8000", "%2d,%ys", _
"vmul.p", "0x64000080", "0xFF808080", "%zp,%yp,%xp", _
"vmul.q", "0x64008080", "0xFF808080", "%zq,%yq,%xq", _
"vmul.s", "0x64000000", "0xFF808080", "%zs,%ys,%xs", _
"vmul.t", "0x64008000", "0xFF808080", "%zt,%yt,%xt", _
"vmzero.p", "0xF3860080", "0xFFFFFF80", "%zm", _
"vmzero.q", "0xF3868080", "0xFFFFFF80", "%zo", _
"vmzero.t", "0xF3868000", "0xFFFFFF80", "%zn", _
"vneg.p", "0xD0020080", "0xFFFF8080", "%zp,%yp", _
"vneg.q", "0xD0028080", "0xFFFF8080", "%zq,%yq", _
"vneg.s", "0xD0020000", "0xFFFF8080", "%zs,%ys", _
"vneg.t", "0xD0028000", "0xFFFF8080", "%zt,%yt", _
"vnop", "0xFFFF0000", "0xFFFFFFFF", "", _
"vnrcp.p", "0xD0180080", "0xFFFF8080", "%zp,%yp", _
"vnrcp.q", "0xD0188080", "0xFFFF8080", "%zq,%yq", _
"vnrcp.s", "0xD0180000", "0xFFFF8080", "%zs,%ys", _
"vnrcp.t", "0xD0188000", "0xFFFF8080", "%zt,%yt", _
"vnsin.p", "0xD01A0080", "0xFFFF8080", "%zp,%yp", _
"vnsin.q", "0xD01A8080", "0xFFFF8080", "%zq,%yq", _
"vnsin.s", "0xD01A0000", "0xFFFF8080", "%zs,%ys", _
"vnsin.t", "0xD01A8000", "0xFFFF8080", "%zt,%yt", _
"vocp.p", "0xD0440080", "0xFFFF8080", "%zp,%yp", _
"vocp.q", "0xD0448080", "0xFFFF8080", "%zq,%yq", _
"vocp.s", "0xD0440000", "0xFFFF8080", "%zs,%ys", _
"vocp.t", "0xD0448000", "0xFFFF8080", "%zt,%yt", _
"vone.p", "0xD0070080", "0xFFFFFF80", "%zp", _
"vone.q", "0xD0078080", "0xFFFFFF80", "%zq", _
"vone.s", "0xD0070000", "0xFFFFFF80", "%zs", _
"vone.t", "0xD0078000", "0xFFFFFF80", "%zt", _
"vpfxd", "0xDE000000", "0xFF000000", "[%vp4,%vp5,%vp6,%vp7]", _
"vpfxs", "0xDC000000", "0xFF000000", "[%vp0,%vp1,%vp2,%vp3]", _
"vpfxt", "0xDD000000", "0xFF000000", "[%vp0,%vp1,%vp2,%vp3]", _
"vqmul.q", "0xF2808080", "0xFF808080", "%zq,%yq,%xq", _
"vrcp.p", "0xD0100080", "0xFFFF8080", "%zp,%yp", _
"vrcp.q", "0xD0108080", "0xFFFF8080", "%zq,%yq", _
"vrcp.s", "0xD0100000", "0xFFFF8080", "%zs,%ys", _
"vrcp.t", "0xD0108000", "0xFFFF8080", "%zt,%yt", _
"vrexp2.p", "0xD01C0080", "0xFFFF8080", "%zp,%yp", _
"vrexp2.q", "0xD01C8080", "0xFFFF8080", "%zq,%yq", _
"vrexp2.s", "0xD01C0000", "0xFFFF8080", "%zs,%ys", _
"vrexp2.t", "0xD01C8000", "0xFFFF8080", "%zt,%yt", _
"vrndf1.p", "0xD0220080", "0xFFFFFF80", "%zp", _
"vrndf1.q", "0xD0228080", "0xFFFFFF80", "%zq", _
"vrndf1.s", "0xD0220000", "0xFFFFFF80", "%zs", _
"vrndf1.t", "0xD0228000", "0xFFFFFF80", "%zt", _
"vrndf2.p", "0xD0230080", "0xFFFFFF80", "%zp", _
"vrndf2.q", "0xD0238080", "0xFFFFFF80", "%zq", _
"vrndf2.s", "0xD0230000", "0xFFFFFF80", "%zs", _
"vrndf2.t", "0xD0238000", "0xFFFFFF80", "%zt", _
"vrndi.p", "0xD0210080", "0xFFFFFF80", "%zp", _
"vrndi.q", "0xD0218080", "0xFFFFFF80", "%zq", _
"vrndi.s", "0xD0210000", "0xFFFFFF80", "%zs", _
"vrndi.t", "0xD0218000", "0xFFFFFF80", "%zt", _
"vrnds.s", "0xD0200000", "0xFFFF80FF", "%ys", _
"vrot.p", "0xF3A00080", "0xFFE08080", "%zp,%ys,%vr", _
"vrot.q", "0xF3A08080", "0xFFE08080", "%zq,%ys,%vr", _
"vrot.t", "0xF3A08000", "0xFFE08080", "%zt,%ys,%vr", _
"vrsq.p", "0xD0110080", "0xFFFF8080", "%zp,%yp", _
"vrsq.q", "0xD0118080", "0xFFFF8080", "%zq,%yq", _
"vrsq.s", "0xD0110000", "0xFFFF8080", "%zs,%ys", _
"vrsq.t", "0xD0118000", "0xFFFF8080", "%zt,%yt", _
"vs2i.p", "0xD03B0080", "0xFFFF8080", "%zq,%yp", _
"vs2i.s", "0xD03B0000", "0xFFFF8080", "%zp,%ys", _
"vsat0.p", "0xD0040080", "0xFFFF8080", "%zp,%yp", _
"vsat0.q", "0xD0048080", "0xFFFF8080", "%zq,%yq", _
"vsat0.s", "0xD0040000", "0xFFFF8080", "%zs,%ys", _
"vsat0.t", "0xD0048000", "0xFFFF8080", "%zt,%yt", _
"vsat1.p", "0xD0050080", "0xFFFF8080", "%zp,%yp", _
"vsat1.q", "0xD0058080", "0xFFFF8080", "%zq,%yq", _
"vsat1.s", "0xD0050000", "0xFFFF8080", "%zs,%ys", _
"vsat1.t", "0xD0058000", "0xFFFF8080", "%zt,%yt", _
"vsbn.s", "0x61000000", "0xFF808080", "%zs,%ys,%xs", _
"vsbz.s", "0xD0360000", "0xFFFF8080", "%zs,%ys", _
"vscl.p", "0x65000080", "0xFF808080", "%zp,%yp,%xs", _
"vscl.q", "0x65008080", "0xFF808080", "%zq,%yq,%xs", _
"vscl.t", "0x65008000", "0xFF808080", "%zt,%yt,%xs", _
"vscmp.p", "0x6E800080", "0xFF808080", "%zp,%yp,%xp", _
"vscmp.q", "0x6E808080", "0xFF808080", "%zq,%yq,%xq", _
"vscmp.s", "0x6E800000", "0xFF808080", "%zs,%ys,%xs", _
"vscmp.t", "0x6E808000", "0xFF808080", "%zt,%yt,%xt", _
"vsge.p", "0x6F000080", "0xFF808080", "%zp,%yp,%xp", _
"vsge.q", "0x6F008080", "0xFF808080", "%zq,%yq,%xq", _
"vsge.s", "0x6F000000", "0xFF808080", "%zs,%ys,%xs", _
"vsge.t", "0x6F008000", "0xFF808080", "%zt,%yt,%xt", _
"vsgn.p", "0xD04A0080", "0xFFFF8080", "%zp,%yp", _
"vsgn.q", "0xD04A8080", "0xFFFF8080", "%zq,%yq", _
"vsgn.s", "0xD04A0000", "0xFFFF8080", "%zs,%ys", _
"vsgn.t", "0xD04A8000", "0xFFFF8080", "%zt,%yt", _
"vsin.p", "0xD0120080", "0xFFFF8080", "%zp,%yp", _
"vsin.q", "0xD0128080", "0xFFFF8080", "%zq,%yq", _
"vsin.s", "0xD0120000", "0xFFFF8080", "%zs,%ys", _
"vsin.t", "0xD0128000", "0xFFFF8080", "%zt,%yt", _
"vslt.p", "0x6F800080", "0xFF808080", "%zp,%yp,%xp", _
"vslt.q", "0x6F808080", "0xFF808080", "%zq,%yq,%xq", _
"vslt.s", "0x6F800000", "0xFF808080", "%zs,%ys,%xs", _
"vslt.t", "0x6F808000", "0xFF808080", "%zt,%yt,%xt", _
"vsocp.p", "0xD0450080", "0xFFFF8080", "%zq,%yp", _
"vsocp.s", "0xD0450000", "0xFFFF8080", "%zp,%ys", _
"vsqrt.p", "0xD0160080", "0xFFFF8080", "%zp,%yp", _
"vsqrt.q", "0xD0168080", "0xFFFF8080", "%zq,%yq", _
"vsqrt.s", "0xD0160000", "0xFFFF8080", "%zs,%ys", _
"vsqrt.t", "0xD0168000", "0xFFFF8080", "%zt,%yt", _
"vsrt1.q", "0xD0408080", "0xFFFF8080", "%zq,%yq", _
"vsrt2.q", "0xD0418080", "0xFFFF8080", "%zq,%yq", _
"vsrt3.q", "0xD0488080", "0xFFFF8080", "%zq,%yq", _
"vsrt4.q", "0xD0498080", "0xFFFF8080", "%zq,%yq", _
"vsub.p", "0x60800080", "0xFF808080", "%zp,%yp,%xp", _
"vsub.q", "0x60808080", "0xFF808080", "%zq,%yq,%xq", _
"vsub.s", "0x60800000", "0xFF808080", "%zs,%ys,%xs", _
"vsub.t", "0x60808000", "0xFF808080", "%zt,%yt,%xt", _
"vsync", "0xFFFF0000", "0xFFFF0000", "%I", _
"vsync", "0xFFFF0320", "0xFFFFFFFF", "", _
"vt4444.q", "0xD0598080", "0xFFFF8080", "%zq,%yq", _
"vt5551.q", "0xD05A8080", "0xFFFF8080", "%zq,%yq", _
"vt5650.q", "0xD05B8080", "0xFFFF8080", "%zq,%yq", _
"vtfm2.p", "0xF0800080", "0xFF808080", "%zp,%ym,%xp", _
"vtfm3.t", "0xF1008000", "0xFF808080", "%zt,%yn,%xt", _
"vtfm4.q", "0xF1808080", "0xFF808080", "%zq,%yo,%xq", _
"vus2i.p", "0xD03A0080", "0xFFFF8080", "%zq,%yp", _
"vus2i.s", "0xD03A0000", "0xFFFF8080", "%zp,%ys", _
"vwb.q", "0xF8000002", "0xFC000002", "%Xq,%Y", _
"vwbn.s", "0xD3000000", "0xFF008080", "%zs,%xs,%I", _
"vzero.p", "0xD0060080", "0xFFFFFF80", "%zp", _
"vzero.q", "0xD0068080", "0xFFFFFF80", "%zq", _
"vzero.s", "0xD0060000", "0xFFFFFF80", "%zs", _
"vzero.t", "0xD0068000", "0xFFFFFF80", "%zt", _
"mfvme", "0x68000000", "0xFC000000", "%t,%i", _
"mtvme", "0xb0000000", "0xFC000000", "%t,%i"}

#End Region

    'decoder PRXTOOLの移植
#Region "decoderaser"

    Function decoders(ByVal str As String, ByVal l As Integer) As String
        Try
            Dim hex As UInteger = Convert.ToUInt32(str, 16)
            Dim mask As UInteger = 0
            Dim mips As UInteger = 0
            Dim asm As String = ""

            Dim z As Integer = 0
            Dim zz As Integer = decoder.Length

            While z < zz
                mips = Convert.ToUInt32(decoder(z + 1), 16)
                mask = Convert.ToUInt32(decoder(z + 2), 16)
                If (hex And mask) = mips Then
                    asm = decoder(z) & " " & decoder(z + 3)
                    asm = decode_arg(asm, hex, l)
                    Exit While
                End If
                z += 4
            End While

            Return asm
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return str
        End Try
    End Function

    Function reg_dec(ByVal z As Integer) As String
        Dim ss As String() = {"zr", "at", "v0", "v1", "a0", "a1", "a2", "a3", "t0", "t1", "t2", "t3", "t4", "t5", "t6", "t7", "s0", "s1", "s2", "s3", "s4", "s5", "s6", "s7", "t8", "t9", "k0", "k1", "gp", "sp", "fp", "ra"}
        Return ss(z)
    End Function

    Function reg_dec_debug(ByVal z As Integer) As String
        Dim dr As String() = {"DRCNTL", "DEPC", "DDATA0", "DDATA1", "IBC", "DBC", "$06", "$07", _
    "IBA", "IBAM", "$10", "$11", "DBA", "DBAM", "DBD", "DBDM", ""}
        If z > 15 Then
            z = 16
            dr(16) = "$" & z.ToString
        End If
        Return dr(z)
    End Function

    Function reg_cop0(ByVal z As Integer) As String
        Dim cop0 As String() = {"INDEX", "RANDOM", "ENTRYLO0", "ENTRYLO1", "CONTEXT", "PAGEMASK", "WIRED", "$7", "BADVADDR", "COUNT", "ENTRYHI", "COMPARE", "STATUS", "CAUSE", "EPC", "PRID", "CONFIG", "LLADDR", "WATCHLO", "WATCHHI", "XCONTEXT", "$21", "$22", "DEBUG", "DEPC", "PERFCNT", "ERRCTL", "CACHEERR", "TAGLO", "TAGHI", "ERROREPC", "DESAVE"}
        Return cop0(z)
    End Function

    Function decode_arg(ByVal str As String, ByVal hex As UInteger, ByVal l As Integer) As String
        Dim ss(str.Length) As String
        Dim z As Integer = 0
        Dim vmmul As Boolean = False
        For Each s As String In str
            ss(z) = s
            z += 1
        Next

        For i = 0 To ss.Length - 1
            If (ss(i) = "%") Then
                i += 1
                Select Case ss(i)
                    Case "0"
                        str = str.Replace("%0", reg_cop0((CInt(hex >> 11) And &H1F)))
                    Case "1"
                        str = str.Replace("%1", "$f" & (CInt(hex >> 11) And &H1F).ToString)

                    Case "a"
                        str = str.Replace("%a", ((CInt(hex >> 6) And &H1F)).ToString)
                    Case "C"
                        str = str.Replace("%C", "$" & (CInt(hex >> 6) And &HFFFFF).ToString("X"))
                    Case "c"
                        str = str.Replace("%c", "$" & (CInt(hex >> 6) And &HFFFFF).ToString("X"))
                    Case "n"
                        Select Case ss(i + 1)
                            Case "e"
                                str = str.Replace("%ne", ((CInt(hex >> 11) And &H1F) + 1).ToString)
                            Case "i"
                                str = str.Replace("%ni", ((CInt(hex >> 11) And &H1F) - (CInt(hex >> 6) And &H1F) + 1).ToString)
                        End Select
                    Case "i"
                        Dim k As Integer = CInt(hex And &HFFFF)
                        Dim minus As String = ""
                        If k > &H7FFF Then
                            k = &H10000 - k
                            minus = "-"
                        End If
                        minus &= "0x" & k.ToString("X")
                        str = str.Replace("%i", minus)
                    Case "I"
                        str = str.Replace("%I", "0x" & (CInt((hex And &HFFFF)).ToString("X")))
                    Case "j"
                        str = str.Replace("%j", "0x" & (CInt((hex And &H3FFFFFF) << 2).ToString("X")))
                    Case "J"
                        str = str.Replace("%J", reg_dec((CInt(hex >> 21) And &H1F)))
                    Case "k"
                        str = str.Replace("%k", "$" & (CInt(hex >> 16) And &H1F).ToString)
                    Case "o"
                        Dim k As Integer = CInt(hex And &HFFFF)
                        Dim minus As String = ""
                        If k > &H7FFF Then
                            k = &H10000 - k
                            minus = "-"
                        End If
                        minus &= "0x" & k.ToString("X")
                        str = str.Replace("%o", minus & "(" & reg_dec((CInt(hex >> 21) And &H1F)) & ")")
                    Case "O"
                        Dim k As Integer = CInt(hex And &HFFFF)
                        Dim minus As String = ""
                        If k > &H7FFF Then
                            k -= &H10000
                        End If
                        k = (k << 2) + 4
                        If DataGridView1.RowCount > 1 Then
                            k += (Convert.ToInt32(DataGridView1.Rows(l).Cells(0).Value.ToString, 16) And &HFFFFFFF)
                        Else
                            k += (Convert.ToInt32(dgaddress, 16) And &HFFFFFFF)
                        End If

                        If k < &H1800000 Then
                            k += &H8800000
                        End If
                        str = str.Replace("%O", "0x" & k.ToString("X"))
                    Case "p"
                        str = str.Replace("%p", (CInt(hex >> 11) And &H1F).ToString)
                    Case "r"
                        str = str.Replace("%r", reg_dec_debug(CInt(hex >> 11) And &H1F))
                    Case "s"
                        str = str.Replace("%s", reg_dec((CInt(hex >> 21) And &H1F)))
                    Case "t"
                        str = str.Replace("%t", reg_dec((CInt(hex >> 16) And &H1F)))
                    Case "d"
                        str = str.Replace("%d", reg_dec((CInt(hex >> 11) And &H1F)))
                    Case "S"
                        str = str.Replace("%S", "$f" & (CInt(hex >> 11) And &H1F).ToString)
                    Case "T"
                        str = str.Replace("%T", "$f" & (CInt(hex >> 16) And &H1F).ToString)
                    Case "D"
                        str = str.Replace("%D", "$f" & (CInt(hex >> 6) And &H1F).ToString)
                    Case "Z"
                        Select Case ss(i + 1)
                            Case "c"
                                str = str.Replace("%Zc", (CInt(hex >> 18) And &H7).ToString)
                            Case "n"
                                str = str.Replace("%Zn", vfpucond(CInt(hex >> 16) And &HF))
                        End Select
                    Case "x"
                        str = str.Replace("%x" & ss(i + 1), vfpureg(CInt(hex >> 16) And &H7F, ss(i + 1)))
                    Case "y"
                        Dim reg As Integer = CInt(hex >> 8) And &H7F
                        If vmmul = True Then
                            If (reg And &H20) <> 0 Then
                                reg = reg And &H5F
                            Else
                                reg = reg Or &H20
                            End If
                        End If
                        str = str.Replace("%y" & ss(i + 1), vfpureg(reg, ss(i + 1)))

                    Case "z"
                        str = str.Replace("%z" & ss(i + 1), vfpureg(CInt(hex And &H7F), ss(i + 1)))

                    Case "v"
                        '// [hlide] completed %v? (? is 3, 5, 8, k, i, h, r, p? (? is (0, 1, 2, 3, 4, 5, 6, 7) ) )
                        Select Case ss(i + 1)
                            Case "3"
                                str = str.Replace("%v" & ss(i + 1), (CInt((hex >> 16) And &H7).ToString))
                                'output = print_int(VI3(opcode), output); i++; 
                            Case "5"
                                str = str.Replace("%v" & ss(i + 1), (CInt((hex >> 16) And &H1F).ToString))
                                'output = print_int(VI5(opcode), output); i++; 
                            Case "8"
                                str = str.Replace("%v" & ss(i + 1), (CInt((hex >> 16) And &HFF).ToString))
                                'output = print_int(VI8(opcode), output); i++; 
                            Case "k"
                                str = str.Replace("%v" & ss(i + 1), print_vfpu_const(CInt((hex >> 16) And &H1F)))
                                'output = print_vfpu_const(VI5(opcode), output); i++; 
                            Case "i"
                                Dim k As Integer = CInt(hex And &HFFFF)
                                Dim minus As String = ""
                                If k > &H7FFF Then
                                    k = &H10000 - k
                                    minus = "-"
                                End If
                                minus &= "0x" & k.ToString("X")
                                str = str.Replace("%v" & ss(i + 1), minus)
                                'output = print_int(IMM(opcode), output); i++; 
                            Case "h"
                                Dim sss As String = ""
                                Dim bytes As Byte() = str2bin(Convert.ToString(hex And &HFFFF, 16).PadRight(8, "0"c))
                                Array.ConstrainedCopy(bytes, 2, bytes, 0, 2)
                                Array.Resize(bytes, 2)
                                If (bytes(1) And &H7F) < &H7C Then
                                    Dim bytes2 As Byte() = str2bin(converthalffloat2(bytes))
                                    If BitConverter.ToSingle(bytes2, 0) > 0.00009 Then
                                        sss = BitConverter.ToSingle(bytes2, 0).ToString
                                    Else
                                        sss = "0"
                                    End If
                                Else
                                    sss = "NaN"
                                End If
                                str = str.Replace("%v" & ss(i + 1), sss)
                                'output = print_vfpu_halffloat(opcode, output); i++; 
                            Case "r"
                                str = str.Replace("%v" & ss(i + 1), print_vfpu_rotator(hex))
                                'output = print_vfpu_rotator(opcode, output); i++; 

                            Case "p"
                                str = str.Replace("%v" & ss(i + 1) & ss(i + 2), print_vfpu_prefix(hex, ss(i + 2)))
                                'if (fmt[i+2]) { output = print_vfpu_prefix(opcode, fmt[i+2], output); i += 2; }
                        End Select
                    Case "2"
                        ': // [hlide] added %2? (? is d, s)
                        Select Case ss(i + 1)
                            Case "d"
                                str = str.Replace("%2" & ss(i + 1), print_cop2(CInt((hex >> 8) And &HFF)))
                                ' : output = print_cop2(VED(opcode), output); i++; break;
                            Case "s"
                                str = str.Replace("%2" & ss(i + 1), print_cop2(CInt(hex And &HFF)))
                                ': output = print_cop2(VES(opcode), output); i++; break;
                        End Select

                    Case "X"
                        str = str.Replace("%X" & ss(i + 1), vfpureg((CInt(hex And 3) << 5) Or (CInt(hex >> 16) And &H1F), ss(i + 1)))

                    Case "Y"
                        str = str.Replace("%Y", "0x" & CInt(hex And &HFFFC).ToString("X") & "(" & reg_dec((CInt(hex >> 21) And &H1F)) & ")")
                        'output = print_ofs(IMM(opcode) & ~3, RS(opcode), output, realregs);

                    Case "?"
                        vmmul = True
                        str = str.Replace("%?", "")

                End Select
            End If
        Next
        Return str
    End Function

    Function print_cop2(ByVal reg As Integer) As String
        Dim vfpu_extra_regs As String() = {"VFPU_PFXS",
         "VFPU_PFXT",
         "VFPU_PFXD",
         "VFPU_CC ",
         "VFPU_INF4",
         "",
         "",
         "VFPU_REV",
         "VFPU_RCX0",
         "VFPU_RCX1",
         "VFPU_RCX2",
         "VFPU_RCX3",
         "VFPU_RCX4",
         "VFPU_RCX5",
         "VFPU_RCX6",
         "VFPU_RCX7"
        }
        Dim ss As String = ""

        If ((reg >= 128) AndAlso (reg < 128 + 16) AndAlso (vfpu_extra_regs(reg - 128)) <> "") Then
            'len = sprintf(output, "%s", vfpu_extra_regs(reg - 128));
            ss = vfpu_extra_regs(reg - 128)
        Else

            ss = "$" & reg.ToString
            'Len = sprintf(output, "$%d", reg)
        End If

        Return ss
    End Function

    Function print_vfpu_prefix(ByVal l As UInteger, ByVal pos As String) As String
        '/* VFPU prefix instruction operands.  The *_SH_* values really specify where
        '   the bitfield begins, as VFPU prefix instructions have four operands
        '   encoded within the immediate field. */
        Dim VFPU_SH_PFX_NEG As UInteger = 16
        Dim VFPU_MASK_PFX_NEG As UInteger = 1   '/* Negation. */
        Dim VFPU_SH_PFX_CST As UInteger = 12
        Dim VFPU_MASK_PFX_CST As UInteger = 1   '/* Constant. */
        Dim VFPU_SH_PFX_ABS_CSTHI As UInteger = 8
        Dim VFPU_MASK_PFX_ABS_CSTHI As UInteger = 1 '/* Abs/Constant (bit 2). */
        Dim VFPU_SH_PFX_SWZ_CSTLO As UInteger = 0
        Dim VFPU_MASK_PFX_SWZ_CSTLO As UInteger = 3 '/* Swizzle/Constant (bits 0-1). */
        Dim VFPU_SH_PFX_MASK As UInteger = 8
        Dim VFPU_MASK_PFX_MASK As UInteger = 1  '/* Mask. */
        Dim VFPU_SH_PFX_SAT As UInteger = 0
        Dim VFPU_MASK_PFX_SAT As UInteger = 3   '/* Saturation. */
        Dim ss As String = ""
        Dim poss As UInteger = CUInt(&H30 + Convert.ToUInt32(pos))
        Dim pfx_cst_names As String() = {"0", "1", "2", "1/2", "3", "1/3", "1/4", "1/6"}
        Dim pfx_swz_names As String() = {"x", "y", "z", "w"}
        Dim pfx_sat_names As String() = {"", "[0:1]", "", "[-1:1]"}

        Select Case pos
            Case "0", "1", "2", "3"

                Dim base As UInteger = CUInt(poss - 48)
                Dim negation As UInteger = (l >> CInt(base + VFPU_SH_PFX_NEG)) And VFPU_MASK_PFX_NEG
                Dim constant As UInteger = (l >> CInt(base + VFPU_SH_PFX_CST)) And VFPU_MASK_PFX_CST
                Dim abs_consthi As UInteger = (l >> CInt(base + VFPU_SH_PFX_ABS_CSTHI)) And VFPU_MASK_PFX_ABS_CSTHI
                Dim swz_constlo As UInteger = (l >> CInt(base << 1)) And VFPU_MASK_PFX_SWZ_CSTLO

                If (negation <> 0) Then
                    ss &= "-"
                    'Len = sprintf(output, "-")
                End If

                If (constant <> 0) Then
                    ss &= pfx_cst_names((CInt(abs_consthi << 2) Or CInt(swz_constlo)))
                    'len += sprintf(output+len, "%s", pfx_cst_names[(abs_consthi << 2) | swz_constlo]);

                Else
                    If (abs_consthi <> 0) Then
                        ss &= pfx_swz_names(CInt(swz_constlo))
                        'len += sprintf(output+len, "|%s|", pfx_swz_names[swz_constlo]);

                    Else
                        ss &= pfx_swz_names(CInt(swz_constlo))
                        'len += sprintf(output+len, "%s", pfx_swz_names[swz_constlo]);

                    End If
                End If

            Case "4", "5", "6", "7"
                Dim base As UInteger = CUInt(poss - &H34)
                Dim mask As UInteger = (l >> CInt(base + VFPU_SH_PFX_MASK)) And VFPU_MASK_PFX_MASK
                Dim saturation As UInteger = (l >> CInt(base << 1)) And VFPU_MASK_PFX_SAT

                If (mask <> 0) Then
                    ss &= "m"
                    'len += sprintf(output, "m");
                Else
                    ss &= pfx_sat_names(CInt(saturation))
                    'len += sprintf(output, "%s", pfx_sat_names[saturation]);
                End If

        End Select

        Return ss

    End Function

    Function print_vfpu_rotator(ByVal l As UInteger) As String

        Dim elements(4) As String
        Dim ss As String
        Dim VFPU_MASK_OP_SIZE As UInteger = &H8080
        Dim VFPU_OP_SIZE_PAIR As UInteger = &H80
        Dim VFPU_OP_SIZE_TRIPLE As UInteger = &H8000
        Dim VFPU_OP_SIZE_QUAD As UInteger = &H8080
        Dim VFPU_SH_ROT_HI As UInteger = 2
        Dim VFPU_MASK_ROT_HI As UInteger = 3
        Dim VFPU_SH_ROT_LO As UInteger = 0
        Dim VFPU_MASK_ROT_LO As UInteger = 3
        Dim VFPU_SH_ROT_NEG As UInteger = 4
        Dim VFPU_MASK_ROT_NEG As UInteger = 1

        Dim opcode As UInteger = l And VFPU_MASK_OP_SIZE
        Dim rotators As UInteger = CUInt((l >> 16) And &H1F)
        Dim opsize, rothi, rotlo, negation, i As UInteger

        '/* Determine the operand size so we'll know how many elements to output. */
        If (opcode = VFPU_OP_SIZE_PAIR) Then
            opsize = 2
        ElseIf (opcode = VFPU_OP_SIZE_TRIPLE) Then
            opsize = 3
        Else
            opsize = 4
            'opsize = (opcode = VFPU_OP_SIZE_QUAD) * 4 
            ';/* Sanity check. */
        End If

        rothi = CUInt((rotators >> 2) And 3)
        rotlo = CUInt((rotators >> 0) And 3)
        negation = CUInt((rotators >> 4) And 1)

        If (rothi = rotlo) Then
            If (negation <> 0) Then
                elements(0) = "-s"
                elements(1) = "-s"
                elements(2) = "-s"
                elements(3) = "-s"
            Else
                elements(0) = "s"
                elements(1) = "s"
                elements(2) = "s"
                elements(3) = "s"
            End If
        Else
            elements(0) = "0"
            elements(1) = "0"
            elements(2) = "0"
            elements(3) = "0"
        End If

        If (negation <> 0) Then
            elements(CInt(rothi)) = "-s"
        Else
            elements(CInt(rothi)) = "s"
        End If

        elements(CInt(rotlo)) = "c"

        ss = "["
        'len = sprintf(output, "[");

        'for (i = 0;;)
        opsize = CUInt(opsize - 1)
        For i = 0 To opsize
            ss &= elements(CInt(i))
            'len += sprintf(output, "%s", elements[i++]);
            If (i >= opsize) Then
                Exit For
            End If
            ss &= " ,"
        Next

        ss &= "]"
        'len += sprintf(output, "]");

        Return ss
    End Function

    Function print_vfpu_const(ByVal k As Integer) As String
        Dim ss As String = ""
        Dim vfpu_const_names As String() = {"", "VFPU_HUGE", "VFPU_SQRT2", "VFPU_SQRT1_2", "VFPU_2_SQRTPI", "VFPU_2_PI", "VFPU_1_PI", "VFPU_PI_4", "VFPU_PI_2", "VFPU_PI", "VFPU_E", "VFPU_LOG2E", "VFPU_LOG10E", "VFPU_LN2", "VFPU_LN10", "VFPU_2PI", "VFPU_PI_6", "VFPU_LOG10TWO", "VFPU_LOG2TEN", "VFPU_SQRT3_2"}
        If ((k > 0) AndAlso (k < 20)) Then
            ss = vfpu_const_names(k)
        Else
            ss = k.ToString
        End If

        Return ss
    End Function

    Function print_vfpu_reg(ByVal reg As Integer, ByVal offset As Integer, ByVal one As String, ByVal two As String) As String
        Dim ss As String
        If (CInt(reg >> 5) And 1) <> 0 Then
            ss = two & CInt((reg >> 2) And 7).ToString & offset.ToString & (reg And 3).ToString
        Else
            ss = one & CInt((reg >> 2) And 7).ToString & (reg And 3).ToString & offset.ToString
        End If

        Return ss
    End Function


    Function vfpureg(ByVal reg As Integer, ByVal s As String) As String
        Dim ss As String = ""
        Select Case s
            Case "s"
                ss = "S" & (CInt(reg >> 2) And 7).ToString & (CInt(reg) And 3).ToString & (CInt(reg >> 5) And 3).ToString
                ' return print_vfpusingle(reg, output);
            Case "q"
                ' return print_vfpuquad(reg, output);
                ss = print_vfpu_reg(reg, 0, "C", "R")
            Case "p"
                ' return print_vfpupair(reg, output);
                If (CInt(reg >> 6) And 1) <> 0 Then
                    ss = print_vfpu_reg(reg, 2, "C", "R")
                Else
                    ss = print_vfpu_reg(reg, 0, "C", "R")
                End If

            Case "t"
                ' return print_vfputriple(reg, output);
                If (CInt(reg >> 6) And 1) <> 0 Then
                    ss = print_vfpu_reg(reg, 1, "C", "R")
                Else
                    ss = print_vfpu_reg(reg, 0, "C", "R")
                End If

            Case "m"
                ' return print_vfpumpair(reg, output);
                If (CInt(reg >> 6) And 1) <> 0 Then
                    ss = print_vfpu_reg(reg, 2, "M", "E")
                Else
                    ss = print_vfpu_reg(reg, 0, "M", "E")
                End If

            Case "n"
                ' return print_vfpumtriple(reg, output);
                If (CInt(reg >> 6) And 1) <> 0 Then
                    ss = print_vfpu_reg(reg, 1, "M", "E")
                Else
                    ss = print_vfpu_reg(reg, 0, "M", "E")
                End If

            Case "o"
                ' return print_vfpumatrix(reg, output);
                ss = print_vfpu_reg(reg, 0, "M", "E")

        End Select
        Return ss
    End Function

    Function vfpucond(ByVal k As Integer) As String
        Dim vfpucmp As String() = {"FL", "EQ", "LT", "LE", "TR", "NE", "GE", "GT", "EZ", "EN", "EI", "ES", "NZ", "NN", "NI", "NS", ""}
        If k > 15 Then
            k = 16
            vfpucmp(16) = k.ToString
        End If
        Return vfpucmp(k)
    End Function

    Function cop_sel(ByVal str As String, ByVal mode As String) As Integer
        Dim cop0 As String() = {"INDEX", "RANDOM", "ENTRYLO0", "ENTRYLO1", "CONTEXT", "PAGEMASK", "WIRED", "7", "BADVADDR", "COUNT", "ENTRYHI", "COMPARE", "STATUS", "CAUSE", "EPC", "PRID", "CONFIG", "LLADDR", "WATCHLO", "WATCHHI", "XCONTEXT", "21", "22", "DEBUG", "DEPC", "PERFCNT", "ERRCTL", "CACHEERR", "TAGLO", "TAGHI", "ERROREPC", "DESAVE"}
        str = str.Replace("$", "")
        Dim i As Integer
        If Integer.TryParse(str, i) Then
            i = CInt(str) And 31
        ElseIf mode = "COP0" Then
            For i = 0 To 32
                If i = 32 Then
                    i = 0
                    Exit For
                ElseIf str.Contains(cop0(i).ToLower) Then
                    Exit For
                End If
            Next
        End If
        Return i
    End Function

    Function debug_reg(ByVal str As String, ByVal hex As Integer) As Integer
        Dim dr As String() = {"DRCNTL", "DEPC", "DDATA0", "DDATA1", "IBC", "DBC", "6", "7", _
 "IBA", "IBAM", "10", "11", "DBA", "DBAM", "DBD", "DBDM"}
        str = str.Replace("$", "").Trim
        Dim i As Integer
        If Integer.TryParse(str, i) Then
            i = CInt(str) And 31
        Else
            For i = 0 To 15
                If i = 32 Then
                    i = 0
                    Exit For
                ElseIf str.Contains(dr(i).ToLower) Then
                    Exit For
                End If
            Next
        End If
        hex = hex Or i << 11
        Return hex
    End Function

#End Region

    'ASM INSERT
#Region "INSERT ASM"
    Function assembler(ByVal str As String, ByVal str2 As String) As String
        Try
            Dim hex As Integer = 0
            Dim hex2 As Integer = Convert.ToInt32(str2, 16) And &H9FFFFFFF
            Dim asm As String = ""
            Dim mips As String = ""

            Dim psdis As New Regex("(\t|\x20|　)*?#.+$")
            Dim psdism As Match = psdis.Match(str)
            If psdism.Success Then
                str = str.Substring(0, psdism.Index)
            End If
            str &= " "

            Dim valhex As New Regex("(\$|0x)[0-9A-Fa-f]{1,8}")
            Dim valhexm As Match = valhex.Match(str)
            If valhexm.Success Then
                str = str.Replace(valhexm.Value, valhexm.Value.ToUpper)
                str = str.Replace("0X", "0x")
            End If
            Dim ss As String() = str.ToLower.Split(CChar(","))
            Dim shead As New Regex("^[a-z0-9\.]+(\x20|\t)+")
            Dim sheadm As Match = shead.Match(str)

            If sheadm.Success Then
                mips = sheadm.Value.Replace(" ", "")
                mips = mips.Replace(vbTab, "")
                str = str.Trim
                ss(0) = ss(0).Replace(sheadm.Value, "")
                If mips = "nop" Then
                ElseIf mips = "syscall" Then
                    hex = 12
                    hex = hex Or valhex_syscall(str, hex)
                ElseIf mips = "break" Then
                    hex = &HD '13
                    hex = hex Or valhex_syscall(str, hex)
                ElseIf mips = "sync" Then
                    hex = 15
                ElseIf mips = "sll" Then
                    hex = reg_boolean3(str, hex, 0)
                    hex = valdec_boolean_para(str, hex, 3)
                ElseIf mips = "rotr" Then
                    hex = &H200002
                    hex = reg_boolean3(str, hex, 0)
                    hex = valdec_boolean_para(str, hex, 3)
                ElseIf mips = "rotv" Then
                    hex = &H46
                    hex = reg_boolean3(str, hex, 0)
                ElseIf mips = "srl" Then
                    hex = &H2
                    hex = reg_boolean3(str, hex, 0)
                    hex = valdec_boolean_para(str, hex, 3)
                ElseIf mips = "sra" Then
                    hex = &H3
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                    hex = valdec_boolean_para(str, hex, 3)
                ElseIf mips = "sllv" Then
                    hex = &H4
                    hex = reg_boolean3(str, hex, 0)
                ElseIf mips = "srlv" Then
                    hex = &H6
                    hex = reg_boolean3(str, hex, 0)
                ElseIf mips = "srav" Then
                    hex = &H7
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                    hex = reg_boolean_para(ss(2), hex, 0)
                ElseIf mips = "jalr" Then
                    hex = &H9
                    If ss.Length = 1 Then
                        Array.Resize(ss, 2)
                        ss(1) = ss(0)
                        ss(0) = "ra"
                    End If
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                ElseIf mips = "movz" Then
                    hex = &HA
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "movn" Then
                    hex = &HB
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "mfhi" Then
                    hex = &H10
                    hex = reg_boolean_para(ss(0), hex, 2)
                ElseIf mips = "mthi" Then
                    hex = &H11
                    hex = reg_boolean_para(ss(0), hex, 0)
                ElseIf mips = "mflo" Then
                    hex = &H12
                    hex = reg_boolean_para(ss(0), hex, 2)
                ElseIf mips = "mtlo" Then
                    hex = &H13
                    hex = reg_boolean_para(ss(0), hex, 0)
                ElseIf mips = "clz" Then
                    hex = &H16
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                ElseIf mips = "clo" Then
                    hex = &H17
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                ElseIf mips = "add" Then
                    hex = &H20
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "addu" Then
                    hex = &H21
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "mov" Or mips = "move" Then
                    hex = &H21
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                ElseIf mips = "sub" Then
                    hex = &H22
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "neg" Then
                    hex = &H22
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "subu" Then
                    hex = &H23
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "negu" Then
                    hex = &H23
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "and" Then
                    hex = &H24
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "or" Then
                    hex = &H25
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "xor" Then
                    hex = &H26
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "nor" Then
                    hex = &H27
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "not" Then
                    hex = &H27
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                ElseIf mips = "slt" Then
                    hex = &H2A
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "sltu" Then
                    hex = &H2B
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "max" Then
                    hex = &H2C
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "min" Then
                    hex = &H2D
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "j" Then
                    hex = &H8000000
                    hex = offset_boolean(str, hex)
                ElseIf mips = "jal" Then
                    hex = &HC000000
                    hex = offset_boolean(str, hex)
                ElseIf mips = "jr" Then
                    hex = &H8
                    hex = reg_boolean_para(ss(0), hex, 0)
                ElseIf mips = "mult" Then
                    hex = &H18
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "multu" Then
                    hex = &H19
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "div" Then
                    hex = &H1A
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "divu" Then
                    hex = &H1B
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "madd" Then
                    hex = &H1C
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "maddu" Then
                    hex = &H1D
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "msub" Then
                    hex = &H2E
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "msubu" Then
                    hex = &H2F
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "bltz" Then
                    hex = &H4000000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgez" Then
                    hex = &H4010000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bltzl" Then
                    hex = &H4020000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgezl" Then
                    hex = &H4030000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bltzal" Then
                    hex = &H4100000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgezal" Then
                    hex = &H4110000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bltzall" Then
                    hex = &H4120000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgezall" Then
                    hex = &H4130000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "mtsab" Then
                    hex = &H4180000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = valhex_boolean(ss(1), hex)
                ElseIf mips = "mtsah" Then
                    hex = &H4190000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = valhex_boolean(ss(1), hex)

                    '0x10 branch
                ElseIf mips = "b" Then
                    hex = &H10000000
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bal" Then
                    hex = &H4110000
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bnez" Then
                    hex = &H14000000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bnezl" Then
                    hex = &H54000000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "beqz" Then
                    hex = &H10000000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "beqzl" Then
                    hex = &H50000000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "beq" Then
                    hex = &H10000000
                    hex = reg_boolean(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bne" Then
                    hex = &H14000000
                    hex = reg_boolean(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "blez" Then
                    hex = &H18000000
                    hex = reg_boolean_para(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgtz" Then
                    hex = &H1C000000
                    hex = reg_boolean_para(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)

                    '0x20 add/boolean
                ElseIf mips = "addi" Then
                    hex = &H20000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "addiu" Then
                    hex = &H24000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "li" Then
                    hex = &H24000000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "slti" Then
                    hex = &H28000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "sltiu" Then
                    hex = &H2C000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "andi" Then
                    hex = &H30000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "ori" Then
                    hex = &H34000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "xori" Then
                    hex = &H38000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "lui" Then
                    hex = &H3C000000
                    hex = reg_boolean_para(str, hex, 1)
                    hex = valhex_boolean(str, hex)

                    '0x40 FPU
                ElseIf mips = "mfc0" Then
                    hex = &H40000000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "COP0") << 11)
                ElseIf mips = "mtc0" Then
                    hex = &H40800000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "COP0") << 11)
                ElseIf mips = "cfc0" Then
                    hex = &H40400000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "") << 11)
                ElseIf mips = "ctc0" Then
                    hex = &H40C00000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "") << 11)
                ElseIf mips = "eret" Then
                    hex = &H42000018
                ElseIf mips = "cfc1" Then
                    hex = &H44400000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "") << 11)
                ElseIf mips = "ctc1" Then
                    hex = &H44C00000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "") << 11)
                ElseIf mips = "mfc1" Then
                    hex = &H44000000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "mtc1" Then
                    hex = &H44800000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "bc1f" Then
                    hex = &H45000000
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bc1t" Then
                    hex = &H45010000
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bc1tl" Then
                    hex = &H45020000
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bc1tl" Then
                    hex = &H45030000
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "add.s" Then
                    hex = &H46000000
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                    hex = float_sel(ss(2), hex, 1)
                ElseIf mips = "sub.s" Then
                    hex = &H46000001
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                    hex = float_sel(ss(2), hex, 1)
                ElseIf mips = "mul.s" Then
                    hex = &H46000002
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                    hex = float_sel(ss(2), hex, 1)
                ElseIf mips = "div.s" Then
                    hex = &H46000003
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                    hex = float_sel(ss(2), hex, 1)
                ElseIf mips = "sqrt.s" Then
                    hex = &H46000004
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "abs.s" Then
                    hex = &H46000005
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "mov.s" Then
                    hex = &H46000006
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "neg.s" Then
                    hex = &H46000007
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "round.w.s" Then
                    hex = &H4600000C
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "trunc.w.s" Then
                    hex = &H4600000D
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "ceil.w.s" Then
                    hex = &H4600000E
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "floor.w.s" Then
                    hex = &H4600000F
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "cvt.s.w" Then
                    hex = &H46800020
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "cvt.w.s" Then
                    hex = &H46000024
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "c.f.s" Then
                    hex = &H46000030
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.un.s" Then
                    hex = &H46000031
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.eq.s" Then
                    hex = &H46000032
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ueq.s" Then
                    hex = &H46000033
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.olt.s" Then
                    hex = &H46000034
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ult.s" Then
                    hex = &H46000035
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ole.s" Then
                    hex = &H46000036
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ule.s" Then
                    hex = &H46000037
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.sf.s" Then
                    hex = &H46000038
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ngle.s" Then
                    hex = &H46000039
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.seq.s" Then
                    hex = &H4600003A
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ngl.s" Then
                    hex = &H4600003B
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.lt.s" Then
                    hex = &H4600003C
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.nge.s" Then
                    hex = &H4600003D
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.le.s" Then
                    hex = &H4600003E
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ngt.s" Then
                    hex = &H4600003F
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)

                    '0x50
                ElseIf mips = "beql" Then
                    hex = &H50000000
                    hex = reg_boolean(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bnel" Then
                    hex = &H54000000
                    hex = reg_boolean(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "blezl" Then
                    hex = &H58000000
                    hex = reg_boolean_para(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgtzl" Then
                    hex = &H5C000000
                    hex = reg_boolean_para(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "mfic" Then
                    hex = &H70000024
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = valdec_boolean_para(str, hex, 2)
                ElseIf mips = "mtic" Then
                    hex = &H70000026
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = valdec_boolean_para(str, hex, 2)
                ElseIf mips = "mfdr" Then
                    hex = &H7000003D
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = debug_reg(ss(1), hex)
                ElseIf mips = "mtdr" Then
                    hex = &H7080003D
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = debug_reg(ss(1), hex)
                ElseIf mips = "dbreak" Then
                    hex = &H7000003F
                ElseIf mips = "dret" Then
                    hex = &H7000003E
                ElseIf mips = "haltl" Then
                    hex = &H70000000
                ElseIf mips = "seb" Then
                    hex = &H7C000420
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "seh" Then
                    hex = &H7C000620
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "wsbh" Then
                    hex = &H7C0000A0
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "wsbw" Then
                    hex = &H7C0000E0
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "bitrev" Then
                    hex = &H7C000520
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "halt" Then
                    hex = &H7C000000
                ElseIf mips = "ext" Then
                    hex = &H7C000000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = valdec_boolean_para(ss(2), hex, 3)
                    hex = valdec_ext_para(ss(3), hex, 2)
                ElseIf mips = "ins" Then
                    hex = &H7C000004
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = valdec_boolean_para(ss(2), hex, 3)
                    hex = valdec_ins_para(ss(3), hex, 2)

                    '0x80
                ElseIf mips = "lb" Then
                    hex = &H80000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lh" Then
                    hex = &H84000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lw" Then
                    hex = &H8C000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lbu" Then
                    hex = &H90000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lhu" Then
                    hex = &H94000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lwu" Then
                    hex = &H9C000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lwl" Then
                    hex = &H88000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lwr" Then
                    hex = &H98000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)

                    '0xA0
                ElseIf mips = "sb" Then
                    hex = &HA0000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "sh" Then
                    hex = &HA4000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "sw" Then
                    hex = &HAC000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "swl" Then
                    hex = &HA8000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "swr" Then
                    hex = &HB8000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                    '0xc0
                ElseIf mips = "ll" Then
                    hex = &HC0000000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lwc1" Then
                    hex = &HC4000000
                    hex = float_sel(ss(0), hex, 1)
                    hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)
                    hex = offset_boolean3(str, hex)

                    '0xe0
                ElseIf mips = "sc" Then
                    hex = &HE0000000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "swc1" Then
                    hex = &HE4000000
                    hex = float_sel(ss(0), hex, 1)
                    hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)
                    hex = offset_boolean3(str, hex)

                ElseIf mips = "cache" Then
                    hex = &HBC000000
                    hex = valdec_boolean_para(ss(0), hex, 1)
                    hex = reg_boolean_para(reg_boolean_lbsb(ss(1)), hex, 0)
                    hex = offset_boolean3(str, hex)
                End If

                asm = "0x" & Convert.ToString(hex, 16).ToUpper.PadLeft(8, "0"c)
            End If

            'Dim vfreg As New Regex("(S|C|M)\d\d\d")

            Return asm
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return ""
        End Try
    End Function


    Function float_sel(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim freg As New Regex("\$(f|fpr)\d{1,2}")
        Dim fregm As Match = freg.Match(str)
        If fregm.Success Then
            Dim dec As New Regex("\d{1,2}")
            Dim decm As Match = dec.Match(fregm.Value)
            hex = hex Or ((CInt(decm.Value) And 31) << 21) >> (5 * k)
        End If
        Return hex
    End Function

    Function offset_boolean(ByVal str As String, ByVal hex As Integer) As Integer
        Dim valhex As New Regex("(\x20|,|\t)(\$|0x)[0-9A-Fa-f]{1,8}$")
        Dim valhexm As Match = valhex.Match(str)
        Dim k As Integer = 0
        If valhexm.Success Then
            k = Convert.ToInt32(valhexm.Value.Replace("$", "").Remove(0, 1), 16)
            If k < &H1800000 Then
                k += &H8800000
            End If
            hex = hex Or ((k >> 2) And &H3FFFFFF)
        End If
        Return hex
    End Function

    Function offset_boolean2(ByVal str As String, ByVal hex As Integer, ByVal hex2 As Integer) As Integer
        Dim valhex As New Regex("(\x20|,|\t)(\$|0x)[0-9A-Fa-f]{1,8}$")
        Dim valhexm As Match = valhex.Match(str)
        Dim valdec As New Regex("(\x20|,|\t)(\+|-)?\d{1,4}$")
        Dim valdecm As Match = valdec.Match(str)
        Dim k As Integer = 0
        If valhexm.Success Then
            k = Convert.ToInt32(valhexm.Value.Replace("$", "").Remove(0, 1), 16)
            If k < &H1800000 Then
                k += &H8800000
            End If
            If hex2 < &H1800000 Then
                hex2 += &H8800000
            End If
            hex = hex Or ((k - hex2 - 4) >> 2 And &HFFFF)
        End If
        If valdecm.Success Then
            hex = hex Or ((Convert.ToInt32(valdecm.Value.Remove(0, 1)) - 1) And &HFFFF)
        End If
        Return hex
    End Function

    Function offset_boolean3(ByVal str As String, ByVal hex As Integer) As Integer
        Dim valhex As New Regex("(\x20|,|\t)-?(\$|0x)[0-9A-Fa-f]{1,4}\(")
        Dim valhexm As Match = valhex.Match(str)
        Dim valdec As New Regex("(\x20|,|\t)(\+|-)?\d{1,5}\(")
        Dim valdecm As Match = valdec.Match(str)
        Dim k As Integer = 0
        If valhexm.Success Then
            Dim s As String = valhexm.Value
            If s.Contains("-") Then
                k = &H10000
                s = s.Replace("-", "")
                k = k - Convert.ToInt32(s.Replace("$", "").Remove(0, 1).Replace("(", ""), 16)
            Else
                k = Convert.ToInt32(s.Replace("$", "").Remove(0, 1).Replace("(", ""), 16)
            End If
            hex = hex Or (k And &HFFFF)
        End If
        If valdecm.Success Then
            hex = hex Or (Convert.ToInt32(valdecm.Value.Remove(0, 1).Replace("(", "")) And &HFFFF)
        End If
        Return hex
    End Function

    Function valhex_syscall(ByVal str As String, ByVal hex As Integer) As Integer
        Dim valhex As New Regex("(\x20|,)(\$|0x)[0-9A-Fa-f]{1,5}$")
        Dim valhexm As Match = valhex.Match(str)
        If valhexm.Success Then
            Dim s As String = valhexm.Value
            Dim minus As Integer = 0
            minus = Convert.ToInt32(s.Replace("$", "").Remove(0, 1), 16)
            hex = hex Or (minus And &HFFFFF) << 6
        End If
        Return hex
    End Function

    Function valhex_boolean(ByVal str As String, ByVal hex As Integer) As Integer
        Dim valhex As New Regex("(\x20|,)-?(\$|0x)[0-9A-Fa-f]{1,4}$")
        Dim valhexm As Match = valhex.Match(str)
        Dim valdec As New Regex("(\x20|,)-?\d{1,5}$")
        Dim valdecm As Match = valdec.Match(str)
        Dim valfloat As New Regex("(\x20|,)-?\d+\.?\d*f$")
        Dim valfloatm As Match = valfloat.Match(str)
        If valhexm.Success Then
            Dim s As String = valhexm.Value
            Dim minus As Integer = 0
            If s.Contains("-") Then
                s = s.Replace("-", "")
                minus = &H10000
                minus -= Convert.ToInt32(s.Replace("$", "").Remove(0, 1), 16) And &HFFFF
            Else
                minus = Convert.ToInt32(s.Replace("$", "").Remove(0, 1), 16)
            End If
            hex = hex Or (minus And &HFFFF)
        End If
        If valdecm.Success Then
            hex = hex Or (Convert.ToInt32(valdecm.Value.Remove(0, 1)) And &HFFFF)
        End If
        If valfloatm.Success Then
            Dim f As Single = Convert.ToSingle(valfloatm.Value.Remove(0, 1).Replace("f", ""))
            Dim bit() As Byte = BitConverter.GetBytes(f)
            Dim sb As New System.Text.StringBuilder()
            Dim i As Integer = 3
            While i >= 0
                sb.Append(Convert.ToString(bit(i), 16).PadLeft(2, "0"c))
                i -= 1
            End While
            hex = hex Or (Convert.ToInt32(sb.ToString.Substring(0, 4), 16))
        End If
        Return hex
    End Function

    Function reg_sel(ByVal s As String) As Integer
        Dim ss As String() = {"zr", "at", "v0", "v1", "a0", "a1", "a2", "a3", "t0", "t1", "t2", "t3", "t4", "t5", "t6", "t7", "s0", "s1", "s2", "s3", "s4", "s5", "s6", "s7", "t8", "t9", "k0", "k1", "gp", "sp", "fp", "ra"}
        Dim i As Integer
        If s = "zero" Then
            i = 0
        ElseIf s = "s8" Then
            i = 30
        Else
            For i = 0 To 31
                If ss(i) = s Then
                    Exit For
                End If
            Next
        End If
        Return i
    End Function

    Function valdec_boolean_para(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim valdec As New Regex("\d{1,2}$")
        Dim valdecm As Match = valdec.Match(str.Trim)
        If valdecm.Success Then
            hex = hex Or (CInt(valdecm.Value) << 21) >> (5 * k)
        End If
        Return hex
    End Function

    Function valdec_ext_para(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim valdec As New Regex("\d{1,2}$")
        Dim valdecm As Match = valdec.Match(str.Trim)
        If valdecm.Success Then
            hex = hex Or ((CInt(valdecm.Value) - 1) << 21) >> (5 * k)
        End If
        Return hex
    End Function

    Function valdec_ins_para(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim valdec As New Regex("-?\d{1,2}$")
        Dim valdecm As Match = valdec.Match(str.Trim)
        If valdecm.Success Then
            hex = hex Or ((CInt(valdecm.Value) + (CInt(hex >> 6) And &H1F) - 1) << 21) >> (5 * k)
        End If
        Return hex
    End Function

    Function reg_boolean_lbsb(ByVal str As String) As String
        Dim ss As String() = str.ToLower.Split(CChar("("))
        Return ss(1)
    End Function

    Function reg_boolean_para(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer

        Dim reg As New Regex("(zero|zr|at|a[0-3]|v[0-1]|t[0-9]|k[0-1]|s[0-8]|sp|gp|fp|ra)")
        Dim regm As Match = reg.Match(str)
        If regm.Success Then
            hex = hex Or ((reg_sel(regm.Value) << 21) >> (5 * k))
        End If
        Return hex
    End Function

    Function reg_boolean(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim reg As New Regex("(zero|zr|at|a[0-3]|v[0-1]|t[0-9]|k[0-1]|s[0-8]|sp|gp|fp|ra)")
        Dim regm As Match = reg.Match(str)
        While regm.Success
            hex = hex Or ((reg_sel(regm.Value) << 21) >> (5 * k))
            regm = regm.NextMatch
            k += 1
        End While
        Return hex
    End Function

    Function reg_boolean2(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim reg As New Regex("(zero|zr|at|a[0-3]|v[0-1]|t[0-9]|k[0-1]|s[0-8]|sp|gp|fp|ra)")
        Dim regm As Match = reg.Match(str)
        While regm.Success
            hex = hex Or ((reg_sel(regm.Value) << 16) << (5 * k))
            regm = regm.NextMatch
            k += 1
        End While
        Return hex
    End Function

    Function reg_boolean3(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim reg As New Regex("(zero|zr|at|a[0-3]|v[0-1]|t[0-9]|k[0-1]|s[0-8]|sp|gp|fp|ra)")
        Dim regm As Match = reg.Match(str)
        While regm.Success
            hex = hex Or ((reg_sel(regm.Value) << 11) << (5 * k))
            regm = regm.NextMatch
            k += 1
        End While
        Return hex
    End Function

    Function reg_boolean4(ByVal str As String, ByVal hex As Integer, ByVal k As Integer) As Integer
        Dim reg As New Regex("(zero|zr|at|a[0-3]|v[0-3]|t[0-9]|k[0-1]|s[0-8]|sp|gp|fp|ra)")
        Dim regm As Match = reg.Match(str)
        While regm.Success
            hex = hex Or ((reg_sel(regm.Value) << 6) << (5 * k))
            regm = regm.NextMatch
            k += 1
        End While
        Return hex
    End Function
#End Region
    'Function float_noma(ByVal str As String) As String
    '    Dim hex As Integer = Convert.ToInt32(str, 16)
    '    Dim sign As Integer = Hex >> 31
    '    Dim exponent As Integer = (Hex >> 23) And &HFF
    '    Dim fraction As Integer = Hex And &H7FFFFF
    '    Dim z As Single = 0.0F
    '    Dim t As Single = 1.0F
    '    Dim float As String = ""
    '    For i = 0 To 22
    '        t /= 2.0F
    '        If (((fraction >> (22 - i)) And 1) = 1) Then
    '            z += t
    '        End If
    '    Next
    '    If sign <> 0 Then
    '        float = "-"
    '    End If
    '    float &= z.ToString
    '    Return float
    'End Function

End Class

