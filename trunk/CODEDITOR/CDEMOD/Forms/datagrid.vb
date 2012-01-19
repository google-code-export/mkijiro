Imports System
Imports System.Windows.Forms
Imports System.Text
Imports System.Text.RegularExpressions

Public Class datagrid

    Friend edmode As String

    Private Sub datagrid_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
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

            While ed.Success
                DataGridView1.Rows.Add()
                If m.PSX = False Then
                    DataGridView1.Rows(i).Cells(0).Value = ed.Value.Substring(0, 10)
                    DataGridView1.Rows(i).Cells(1).Value = ed.Value.Substring(11, 10)
                Else
                    DataGridView1.Rows(i).Cells(0).Value = ed.Value.Substring(0, 8)
                    DataGridView1.Rows(i).Cells(1).Value = ed.Value.Substring(9, 4)
                End If
                DataGridView1.Rows(i).Cells(2).Value = "DEC"
                ed = ed.NextMatch()
                i += 1
            End While

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
                        DataGridView1.Rows(l).Cells(4).Value = dg_comment.Value.Substring(k, z - k)
                    End If
                End If
                dg_comment = dg_comment.NextMatch()
                i += 1
            End While


            mask = "<DGMODE[0-9]{1,3}='.*?'>"
            Dim dm As New System.Text.RegularExpressions.Regex(mask, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            Dim dg_mode As System.Text.RegularExpressions.Match = dm.Match(m.dmtext.Text)
            Dim float As String = ""
            zz = i
            i = 0
            While dg_mode.Success Or i < zz
                k = dg_mode.Value.IndexOf("'") + 1
                z = dg_mode.Value.LastIndexOf("'")
                If k > 0 Then
                    b1 = dg_mode.Value.Substring(0, k - 2)
                    b1 = b1.Replace("<DGMODE", "")
                    l = CInt(b1) - 1
                    If l < zz AndAlso l >= 0 AndAlso k < z Then
                        float = dg_mode.Value.Substring(k, z - k)
                        DataGridView1.Rows(l).Cells(2).Value = float
                        If float = "BINARY32" Then
                            Dim bytes As Byte() = str2bin(DataGridView1.Rows(l).Cells(1).Value.ToString)
                            If (bytes(3) And &H7F) > &H30 AndAlso (bytes(3) And &H7F) < &H52 Then
                                DataGridView1.Rows(l).Cells(3).Value = BitConverter.ToSingle(bytes, 0)
                            End If
                        ElseIf float = "BIN32>>16" Then
                            Dim ss As String = DataGridView1.Rows(l).Cells(1).Value.ToString
                            Dim bytes As Byte() = str2bin(ss.Remove(0, ss.Length - 4).PadRight(8, "0"c))
                            If (bytes(3) And &H7F) > &H30 AndAlso (bytes(3) And &H7F) < &H52 Then
                                DataGridView1.Rows(l).Cells(3).Value = BitConverter.ToSingle(bytes, 0)
                            End If
                        ElseIf float = "BINARY16" Then
                            Dim ss As String = DataGridView1.Rows(l).Cells(1).Value.ToString
                            Dim bytes As Byte() = str2bin(ss.Remove(0, ss.Length - 4).PadRight(8, "0"c))
                            Array.ConstrainedCopy(bytes, 2, bytes, 0, 2)
                            Array.Resize(bytes, 2)
                            If (bytes(1) And &H7F) < &H7C Then
                                Dim bytes2 As Byte() = str2bin(converthalffloat2(bytes))
                                DataGridView1.Rows(l).Cells(3).Value = BitConverter.ToSingle(bytes2, 0)
                            End If
                        End If
                    End If
                End If
                dg_mode = dg_mode.NextMatch()
                i += 1
            End While

            DataGridView1.Columns(4).Width = 591 - (DataGridView1.Columns(0).Width + DataGridView1.Columns(1).Width + DataGridView1.Columns(2).Width + DataGridView1.Columns(3).Width)

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
                    End If
                End If
                Else 'BINARY32/16
                    Dim r As New System.Text.RegularExpressions.Regex( _
                     "^[-|+]?\d+\.?\d*", _
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

        Dim d As Integer = DataGridView1.CurrentCell.RowIndex
        Dim row As DataGridViewRow
        Dim st As New StringBuilder
        Dim stc As New StringBuilder
        stc.Append("#")
        Dim cursor(0) As String
        Dim i As Integer = 0
        Dim jj As Integer = 0

        If d < DataGridView1.RowCount Then
            For Each r As DataGridViewRow In DataGridView1.SelectedRows
                If r.Index < DataGridView1.RowCount - 1 Then
                    If jj = 0 Then
                        jj = r.Index
                    End If
                    row = DataGridView1.Rows(r.Index)
                    st.Append("<DGLINE")
                    st.Append((r.Index + 1).ToString.PadLeft(3, "0"c))
                    st.Append("='")
                    If Not row.Cells(4).Value Is Nothing Then
                        st.Append(row.Cells(4).Value.ToString)
                    End If
                    st.Append("'>")
                    st.Append("<DGMODE")
                    st.Append((r.Index + 1).ToString.PadLeft(3, "0"c))
                    st.Append("='")
                    st.Append(row.Cells(2).Value.ToString)
                    st.Append("'>")
                    st.Append("<DGVAL")
                    st.Append((r.Index + 1).ToString.PadLeft(3, "0"c))
                    st.Append("='")
                    If Not row.Cells(3).Value Is Nothing Then
                        st.Append(row.Cells(3).Value.ToString)
                    End If
                    st.Append("'>")
                    st.Append(vbCrLf)
                    st.Append(edmode)
                    st.Append(row.Cells(0).Value.ToString)
                    st.Append(" ")
                    st.Append(row.Cells(1).Value.ToString)
                    st.Append(vbCrLf)
                    Array.Resize(cursor, i + 1)
                    cursor(i) = st.ToString
                    st.Clear()
                    i += 1
                End If
            Next r

        Array.Sort(cursor)

            Dim k = 0
            Dim del = 0
            For Each s As String In cursor
                If s <> "" Then
                    st.Append(s)
                    del = CInt(s.Substring(7, 3)) - k - 1
                    DataGridView1.Rows.RemoveAt(del)
                    k += 1
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

    End Sub

    Private Sub コピーToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles copy.Click

        Dim d As Integer = DataGridView1.CurrentCell.RowIndex
        Dim row As DataGridViewRow
        Dim st As New StringBuilder
        Dim stc As New StringBuilder
        stc.Append("#")
        Dim cursor(0) As String
        Dim cursor2(0) As String
        Dim i As Integer = 0
        Dim jj As Integer = 0

        If d < DataGridView1.RowCount Then
            For Each r As DataGridViewRow In DataGridView1.SelectedRows
                If r.Index < DataGridView1.RowCount - 1 Then
                    If jj = 0 Then
                        jj = r.Index
                    End If
                    row = DataGridView1.Rows(r.Index)
                    st.Append("<DGLINE")
                    st.Append((r.Index + 1).ToString.PadLeft(3, "0"c))
                    st.Append("='")
                    If Not row.Cells(4).Value Is Nothing Then
                        st.Append(row.Cells(4).Value.ToString)
                    End If
                    st.Append("'>")
                    st.Append("<DGMODE")
                    st.Append((r.Index + 1).ToString.PadLeft(3, "0"c))
                    st.Append("='")
                    st.Append(row.Cells(2).Value.ToString)
                    st.Append("'>")
                    st.Append("<DGVAL")
                    st.Append((r.Index + 1).ToString.PadLeft(3, "0"c))
                    st.Append("='")
                    If Not row.Cells(3).Value Is Nothing Then
                        st.Append(row.Cells(3).Value.ToString)
                    End If
                    st.Append("'>")
                    st.Append(vbCrLf)
                    st.Append(edmode)
                    st.Append(row.Cells(0).Value.ToString)
                    st.Append(" ")
                    st.Append(row.Cells(1).Value.ToString)
                    st.Append(vbCrLf)
                    Array.Resize(cursor, i + 1)
                    cursor(i) = st.ToString
                    st.Clear()
                    i += 1
                End If
            Next r

            Array.Sort(cursor)

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
        Dim dg As New System.Text.RegularExpressions.Regex("<DGLINE[0-9]{3}='.*?'>", RegularExpressions.RegexOptions.ECMAScript)
        Dim line As System.Text.RegularExpressions.Match = dg.Match(Clipboard.GetText)
        Dim dm As New System.Text.RegularExpressions.Regex("<DGMODE[0-9]{3}='.*?'>", RegularExpressions.RegexOptions.ECMAScript)
        Dim dmm As System.Text.RegularExpressions.Match = dm.Match(Clipboard.GetText)
        Dim dv As New System.Text.RegularExpressions.Regex("<DGVAL[0-9]{3}='.*?'>", RegularExpressions.RegexOptions.ECMAScript)
        Dim dvm As System.Text.RegularExpressions.Match = dv.Match(Clipboard.GetText)
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

                Dim k As Integer = dmm.Value.IndexOf("'") + 1
                Dim z As Integer = dmm.Value.LastIndexOf("'")
                If k <= z Then
                    DataGridView1.Rows(d + i).Cells(2).Value = dmm.Value.Substring(k, z - k)
                End If

                k = line.Value.IndexOf("'") + 1
                z = line.Value.LastIndexOf("'")
                If k <= z Then
                    DataGridView1.Rows(d + i).Cells(4).Value = line.Value.Substring(k, z - k)
                End If

                k = dvm.Value.IndexOf("'") + 1
                z = dvm.Value.LastIndexOf("'")
                If k <= z Then
                    DataGridView1.Rows(d + i).Cells(3).Value = dvm.Value.Substring(k, z - k)
                End If


                line = line.NextMatch
                dmm = dmm.NextMatch
                dvm = dvm.NextMatch
                i += 1
            End While
            DataGridView1.Rows(d).Selected = True
            DataGridView1.CurrentCell = DataGridView1.Rows(d).Cells(0)
            DataGridView1.Focus()
        End If
    End Sub


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


    Function assembler(ByVal str As String, ByVal str2 As String) As String
        Try
            Dim hex As Integer = 0
            Dim hex2 As Integer = Convert.ToInt32(str2, 16) And &H9FFFFFFF
            Dim asm As String = ""
            Dim ss As String() = str.Split(CChar(","))
            Dim mips As String = ""

            Dim shead As New Regex("^[a-z0-9\.]+\x20+")
            Dim sheadm As Match = shead.Match(str)

            If sheadm.Success Then
                mips = sheadm.Value.Replace(" ", "")
                ss(0) = ss(0).Replace(sheadm.Value, "")
                If mips = "nop" Then
                ElseIf mips = "syscall" Then
                    hex = 12
                ElseIf str = "break" Then
                    hex = 13
                ElseIf mips = "sync" Then
                    hex = 15
                ElseIf mips = "sll" Then
                    hex = reg_boolean3(str, hex, 0)
                    hex = valdec_boolean_para(str, hex, 3)
                ElseIf mips = "rotr" Then
                    hex = hex Or &H200002
                    hex = reg_boolean3(str, hex, 0)
                    hex = valdec_boolean_para(str, hex, 3)
                ElseIf mips = "rotv" Then
                    hex = hex Or &H46
                    hex = reg_boolean3(str, hex, 0)
                ElseIf mips = "srl" Then
                    hex = hex Or &H2
                    hex = reg_boolean3(str, hex, 0)
                    hex = valdec_boolean_para(str, hex, 3)
                ElseIf mips = "sra" Then
                    hex = hex Or &H3
                    hex = reg_boolean3(str, hex, 0)
                    hex = valdec_boolean_para(str, hex, 3)
                ElseIf mips = "sllv" Then
                    hex = hex Or &H4
                    hex = reg_boolean3(str, hex, 0)
                ElseIf mips = "srlv" Then
                    hex = hex Or &H6
                    hex = reg_boolean3(str, hex, 0)
                ElseIf mips = "srav" Then
                    hex = hex Or &H7
                    hex = reg_boolean3(str, hex, 0)
                ElseIf mips = "jalr" Then
                    hex = hex Or &H9
                    If ss.Length = 1 Then
                        Array.Resize(ss, 2)
                        ss(1) = ss(0)
                        ss(0) = "ra"
                    End If
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 2)
                ElseIf mips = "movz" Then
                    hex = hex Or &HA
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "movn" Then
                    hex = hex Or &HB
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "mfhi" Then
                    hex = hex Or &H10
                    hex = reg_boolean_para(str, hex, 2)
                ElseIf mips = "mthi" Then
                    hex = hex Or &H11
                    hex = reg_boolean_para(str, hex, 0)
                ElseIf mips = "mflo" Then
                    hex = hex Or &H12
                    hex = reg_boolean_para(str, hex, 2)
                ElseIf mips = "mtlo" Then
                    hex = hex Or &H13
                    hex = reg_boolean_para(str, hex, 0)
                ElseIf mips = "clz" Then
                    hex = hex Or &H16
                    hex = reg_boolean2(str, hex, 0)
                ElseIf mips = "clo" Then
                    hex = hex Or &H17
                    hex = reg_boolean2(str, hex, 0)
                ElseIf mips = "add" Then
                    hex = hex Or &H20
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "addu" Then
                    hex = hex Or &H21
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "mov" Then
                    hex = hex Or &H21
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "sub" Then
                    hex = hex Or &H22
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "neg" Then
                    hex = hex Or &H22
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "subu" Then
                    hex = hex Or &H23
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "negu" Then
                    hex = hex Or &H23
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "or" Then
                    hex = hex Or &H24
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "and" Then
                    hex = hex Or &H25
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "xor" Then
                    hex = hex Or &H26
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "nor" Then
                    hex = hex Or &H27
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "not" Then
                    hex = hex Or &H27
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                ElseIf mips = "slt" Then
                    hex = hex Or &H2A
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "sltu" Then
                    hex = hex Or &H2B
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "max" Then
                    hex = hex Or &H2C
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "min" Then
                    hex = hex Or &H2D
                    hex = reg_boolean_para(ss(0), hex, 2)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = reg_boolean_para(ss(2), hex, 1)
                ElseIf mips = "j" Then
                    hex = hex Or &H8000000
                    hex = offset_boolean(str, hex)
                ElseIf mips = "jal" Then
                    hex = hex Or &HC000000
                    hex = offset_boolean(str, hex)
                ElseIf mips = "jr" Then
                    hex = hex Or &H8
                    hex = reg_boolean_para(ss(0), hex, 0)
                ElseIf mips = "mult" Then
                    hex = hex Or &H18
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "multu" Then
                    hex = hex Or &H19
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "div" Then
                    hex = hex Or &H1A
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "divu" Then
                    hex = hex Or &H1B
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "madd" Then
                    hex = hex Or &H1C
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "maddu" Then
                    hex = hex Or &H1D
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "msub" Then
                    hex = hex Or &H2E
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "msubu" Then
                    hex = hex Or &H2F
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = reg_boolean_para(ss(1), hex, 1)
                ElseIf mips = "bltz" Then
                    hex = hex Or &H4000000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgez" Then
                    hex = hex Or &H4010000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bltzl" Then
                    hex = hex Or &H4020000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgezl" Then
                    hex = hex Or &H4030000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bltzal" Then
                    hex = hex Or &H4100000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgezal" Then
                    hex = hex Or &H4110000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bltzall" Then
                    hex = hex Or &H4120000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgezall" Then
                    hex = hex Or &H4130000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "mtsab" Then
                    hex = hex Or &H4180000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = valhex_boolean(ss(1), hex)
                ElseIf mips = "mtsah" Then
                    hex = hex Or &H4190000
                    hex = reg_boolean_para(ss(0), hex, 0)
                    hex = valhex_boolean(ss(1), hex)

                    '0x10 branch
                ElseIf mips = "beq" Then
                    hex = hex Or &H10000000
                    hex = reg_boolean(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bne" Then
                    hex = hex Or &H14000000
                    hex = reg_boolean(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "blez" Then
                    hex = hex Or &H18000000
                    hex = reg_boolean_para(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgtz" Then
                    hex = hex Or &H1C000000
                    hex = reg_boolean_para(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)

                    '0x20 add/boolean
                ElseIf mips = "addi" Then
                    hex = hex Or &H20000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "addiu" Then
                    hex = hex Or &H24000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "li" Then
                    hex = hex Or &H24000000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "slti" Then
                    hex = hex Or &H28000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "sltiu" Then
                    hex = hex Or &H2C000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "andi" Then
                    hex = hex Or &H30000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "ori" Then
                    hex = hex Or &H34000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "xori" Then
                    hex = hex Or &H38000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = valhex_boolean(str, hex)
                ElseIf mips = "lui" Then
                    hex = hex Or &H3C000000
                    hex = reg_boolean_para(str, hex, 1)
                    hex = valhex_boolean(str, hex)

                    '0x40 FPU
                ElseIf mips = "mfc0" Then
                    hex = hex Or &H40000000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "COP0") << 11)
                ElseIf mips = "mtc0" Then
                    hex = hex Or &H40800000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "COP0") << 11)
                ElseIf mips = "cfc0" Then
                    hex = hex Or &H40400000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "") << 11)
                ElseIf mips = "ctc0" Then
                    hex = hex Or &H40C00000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "") << 11)
                ElseIf mips = "eret" Then
                    hex = &H42000018
                ElseIf mips = "cfc1" Then
                    hex = hex Or &H40400000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "") << 11)
                ElseIf mips = "ctc1" Then
                    hex = hex Or &H40C00000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = hex Or (cop_sel(ss(1), "") << 11)
                ElseIf mips = "mfc1" Then
                    hex = hex Or &H40000000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "mtc1" Then
                    hex = hex Or &H40800000
                    hex = reg_boolean_para(ss(0), hex, 1)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "bc1f" Then
                    hex = hex Or &H45000000
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bc1t" Then
                    hex = hex Or &H45010000
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bc1tl" Then
                    hex = hex Or &H45020000
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bc1tl" Then
                    hex = hex Or &H45030000
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "add.s" Then
                    hex = hex Or &H46000000
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                    hex = float_sel(ss(2), hex, 1)
                ElseIf mips = "sub.s" Then
                    hex = hex Or &H46000001
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                    hex = float_sel(ss(2), hex, 1)
                ElseIf mips = "mul.s" Then
                    hex = hex Or &H46000002
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                    hex = float_sel(ss(2), hex, 1)
                ElseIf mips = "div.s" Then
                    hex = hex Or &H46000003
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                    hex = float_sel(ss(2), hex, 1)
                ElseIf mips = "sqrt.s" Then
                    hex = hex Or &H46000004
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "abs.s" Then
                    hex = hex Or &H46000005
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "mov.s" Then
                    hex = hex Or &H46000006
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "neg.s" Then
                    hex = hex Or &H46000007
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "round.w.s" Then
                    hex = hex Or &H4600000C
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "trunc.w.s" Then
                    hex = hex Or &H4600000D
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "ceil.w.s" Then
                    hex = hex Or &H4600000E
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "floor.w.s" Then
                    hex = hex Or &H4600000F
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "cvt.s.w" Then
                    hex = hex Or &H46800020
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "cvt.w.s" Then
                    hex = hex Or &H46000024
                    hex = float_sel(ss(0), hex, 3)
                    hex = float_sel(ss(1), hex, 2)
                ElseIf mips = "c.f.s" Then
                    hex = hex Or &H46000030
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.un.s" Then
                    hex = hex Or &H46000031
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.eq.s" Then
                    hex = hex Or &H46000032
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ueq.s" Then
                    hex = hex Or &H46000033
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.olt.s" Then
                    hex = hex Or &H46000034
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ult.s" Then
                    hex = hex Or &H46000035
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ole.s" Then
                    hex = hex Or &H46000036
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ule.s" Then
                    hex = hex Or &H46000037
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.sf.s" Then
                    hex = hex Or &H46000038
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ngle.s" Then
                    hex = hex Or &H46000039
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.seq.s" Then
                    hex = hex Or &H4600003A
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ngl.s" Then
                    hex = hex Or &H4600003B
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.lt.s" Then
                    hex = hex Or &H4600003C
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.nge.s" Then
                    hex = hex Or &H4600003D
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.le.s" Then
                    hex = hex Or &H4600003E
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)
                ElseIf mips = "c.ngl.s" Then
                    hex = hex Or &H4600003F
                    hex = float_sel(ss(0), hex, 2)
                    hex = float_sel(ss(1), hex, 1)

                    '0x50
                ElseIf mips = "beql" Then
                    hex = hex Or &H50000000
                    hex = reg_boolean(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bnel" Then
                    hex = hex Or &H54000000
                    hex = reg_boolean(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "blezl" Then
                    hex = hex Or &H58000000
                    hex = reg_boolean_para(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)
                ElseIf mips = "bgtzl" Then
                    hex = hex Or &H5C000000
                    hex = reg_boolean_para(str, hex, 0)
                    hex = offset_boolean2(str, hex, hex2)

                    '     case 0x70:
                    '        if(a_opcode >> 24 == 0x70){
                    '                switch(a_opcode & 0xE007FF){
                    '                case 0x24:
                    '                pspDebugScreenPuts("mfic     ");
                    '                mipsRegister(a_opcode, T, 1);
                    '                mipsNibble(a_opcode, 2, 0);
                    '                break;

                    '                case 0x26:
                    '                pspDebugScreenPuts("mtic     ");
                    '                mipsRegister(a_opcode, T, 1);
                    '                mipsNibble(a_opcode, 2, 0);
                    '                break;

                    '                case 0x3D:
                    '                pspDebugScreenPuts("mfdr     ");
                    '                mipsRegister(a_opcode, T, 1);
                    '                DrRegister(a_opcode,2,0);
                    '                break;

                    '                case 0x80003D:
                    '                pspDebugScreenPuts("mtdr     ");
                    '                mipsRegister(a_opcode, T, 1);
                    '                DrRegister(a_opcode,2,0);
                    '                break; 
                    '//"mfdr",              0x7000003D, 0xFFE007FF, "%t, %r"},
                    '//"mtdr",              0x7080003D, 0xFFE007FF, "%t, %r"},
                    '//"mfic",              0x70000024, 0xFFE007FF, "%t, %p"},
                    '//"mtic",              0x70000026, 0xFFE007FF, "%t, %p"},
                    '                }
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
                ElseIf mips = "wsbbn" Then
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
                    hex = reg_boolean_para(ss(0), hex, 1)

                    '        switch(a_opcode & 0x03F){
                    '        case 0x0:
                    '        pspDebugScreenPuts("ext      ");
                    '        mipsRegister(a_opcode, T, 1);
                    '        mipsRegister(a_opcode, S, 1);
                    '        mipsNibble(a_opcode, 3, 1);
                    '        a_opcode+=0x800;
                    '        mipsNibble(a_opcode, 2, 0);
                    '        break;

                    '        case 0x4:
                    '        pspDebugScreenPuts("ins      ");
                    '        mipsRegister(a_opcode, T, 1);
                    '        mipsRegister(a_opcode, S, 1);
                    '        mipsNibble(a_opcode, 3, 1);
                    '        mipsins(a_opcode);
                    '        break;
                    '        }
                    '    break;

                    '0x80
                ElseIf mips = "lb" Then
                    hex = hex Or &H80000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lh" Then
                    hex = hex Or &H84000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lw" Then
                    hex = hex Or &H8C000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lbu" Then
                    hex = hex Or &H90000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lhu" Then
                    hex = hex Or &H94000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lwu" Then
                    hex = hex Or &H9C000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lwl" Then
                    hex = hex Or &H88000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "lwr" Then
                    hex = hex Or &H98000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)

                    '0xA0
                ElseIf mips = "sb" Then
                    hex = hex Or &HA0000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "sh" Then
                    hex = hex Or &HA4000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "sw" Then
                    hex = hex Or &HAC000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "swl" Then
                    hex = hex Or &HA8000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)
                ElseIf mips = "swr" Then
                    hex = hex Or &HB8000000
                    hex = reg_boolean2(str, hex, 0)
                    hex = offset_boolean3(str, hex)

                    '0xc0
                ElseIf mips = "lwc1" Then
                    hex = hex Or &HC4000000
                    hex = float_sel(ss(0), hex, 1)
                    hex = reg_boolean_para(ss(1), hex, 0)
                    hex = offset_boolean3(str, hex)

                    '0xe0
                ElseIf mips = "swc1" Then
                    hex = hex Or &HE4000000
                    hex = float_sel(ss(0), hex, 1)
                    hex = reg_boolean_para(ss(1), hex, 0)
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

    Function decoderr(ByVal str As String) As String
        Try
            Dim hex As Integer = Convert.ToInt32(str)
            Dim asm As String = ""

            Return asm
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return str
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

    Function cop_sel(ByVal str As String, ByVal mode As String) As Integer
        Dim cop0 As String() = {"INDEX", "RANDOM", "ENTRYLO0", "ENTRYLO1", "CONTEXT", "PAGEMASK", "WIRED", "7", "BADVADDR", "COUNT", "ENTRYHI", "COMPARE", "STATUS", "CAUSE", "EPC", "PRID", "CONFIG", "LLADDR", "WATCHLO", "WATCHHI", "XCONTEXT", "21", "22", "DEBUG", "DEPC", "PERFCNT", "ERRCTL", "CACHEERR", "TAGLO", "TAGHI", "ERROREPC", "DESAVE"}
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

    Function offset_boolean(ByVal str As String, ByVal hex As Integer) As Integer
        Dim valhex As New Regex("(\x20|,)(\$|0x)[0-9A-Fa-f]{1,8}$")
        Dim valhexm As Match = valhex.Match(str)
        Dim k As Integer = 0
        If valhexm.Success Then
            k = Convert.ToInt32(valhexm.Value.Replace("$", "").Remove(0, 1), 16)
            If k < &H8800000 Then
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
            If k < &H8800000 Then
                k += &H8800000
            End If
            If hex2 < &H8800000 Then
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
        Dim valdecm As Match = valdec.Match(str)
        If valdecm.Success Then
            hex = hex Or (CInt(valdecm.Value) << 21) >> (5 * k)
        End If
        Return hex
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


End Class