Public Class Form1

    'CellValidatingイベントハンドラ 
    Private Sub DataGridView1_CellValidating(ByVal sender As Object, _
        ByVal e As DataGridViewCellValidatingEventArgs) _
        Handles DataGridView1.CellValidating

        Dim dgv As DataGridView = DirectCast(sender, DataGridView)

        '新しい行のセルでなく、セルの内容が変更されている時だけ検証する 
        If e.RowIndex = dgv.NewRowIndex OrElse Not dgv.IsCurrentCellDirty Then
            Exit Sub
        End If

        Dim d As Integer = DataGridView1.CurrentCell.RowIndex
        Dim c As Integer = DataGridView1.CurrentCell.ColumnIndex

        If dgv.Columns(e.ColumnIndex).Name = "編集タイプ" Then

            If Not DataGridView1.Rows(d).Cells(2).Value Is Nothing Then
                Dim check As String = DataGridView1.Rows(d).Cells(2).Value.ToString
                Select check
                    Case "DEC", "BINARY32", "BINARY32(16bit)", "BINARY16"
                        DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 11
                        DataGridView1.Rows(d).Cells(3).Value = ""
                    Case "OR", "AND", "XOR"
                        DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 10
                        DataGridView1.Rows(d).Cells(3).Value = "0x0"
                End Select
            End If
        End If

        'DOBON.NET http://dobon.net/vb/dotnet/datagridview/cellvalidating.html
        If dgv.Columns(e.ColumnIndex).Name = "入力値" Then

            If Not DataGridView1.Rows(d).Cells(2).Value Is Nothing Then
                Dim check As String = DataGridView1.Rows(d).Cells(2).Value.ToString
                If check = "DEC" Then
                    DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 11
                    Dim str As String = e.FormattedValue.ToString()
                    Dim r As New System.Text.RegularExpressions.Regex( _
            "-?\d{1,11}", _
            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                    Dim m As System.Text.RegularExpressions.Match = r.Match(str)
                    If m.Success And m.Length = str.Length Then
                        Label1.Text = ""
                        Dim b1 As String = m.Value
                        Dim max As Int64 = Convert.ToInt64(b1)
                        If max > 2147483647 Then
                            Label1.Text = "2147483647を超えてます"
                            e.Cancel = True
                        ElseIf max < -2147483647 Then
                            Label1.Text = "-2147483647を超えてます"
                            e.Cancel = True
                        End If
                    Else
                        '行にエラーテキストを設定 
                        Label1.Text = "不正な値です"
                        '入力した値をキャンセルして元に戻すには、次のようにする 
                        dgv.CancelEdit()
                        'キャンセルする 
                        e.Cancel = True
                    End If
                ElseIf check = "OR" Or check = "AND" Or check = "XOR" Then
                    DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 10
                    Dim str As String = e.FormattedValue.ToString()
                    Dim r As New System.Text.RegularExpressions.Regex( _
                     "0x[0-9A-Fa-f]{1,8}", _
                                System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                    Dim m As System.Text.RegularExpressions.Match = r.Match(str)
                    If m.Success Then
                        Label1.Text = ""
                    Else
                        '行にエラーテキストを設定 
                        Label1.Text = "不正な値です"
                        '入力した値をキャンセルして元に戻すには、次のようにする 
                        dgv.CancelEdit()
                        'キャンセルする 
                        e.Cancel = True
                    End If
                ElseIf check.Contains("BINARY") Then
                    DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 11
                    Dim str As String = e.FormattedValue.ToString()
                    Dim r As New System.Text.RegularExpressions.Regex( _
                     "-?\d+.?\d*", _
                                System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                    Dim m As System.Text.RegularExpressions.Match = r.Match(str)
                    If m.Success Then
                        Label1.Text = ""
                    Else
                        '行にエラーテキストを設定 
                        Label1.Text = "不正な値です"
                        '入力した値をキャンセルして元に戻すには、次のようにする 
                        dgv.CancelEdit()
                        'キャンセルする 
                        e.Cancel = True
                    End If
                Else
                    dgv.CancelEdit()
                    Label1.Text = "編集タイプが選択されてません"
                End If
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
            If dgv.CurrentCell.OwningColumn.Name = "入力値" Then
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

        If Not DataGridView1.Rows(d).Cells(2).Value Is Nothing Then
            Dim check As String = DataGridView1.Rows(d).Cells(2).Value.ToString
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
            Else
                If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And e.KeyChar <> "."c And e.KeyChar <> "-"c And e.KeyChar <> vbBack Then
                    e.Handled = True
                End If
                DirectCast(DataGridView1.Columns(3), DataGridViewTextBoxColumn).MaxInputLength = 11
            End If
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim m As MERGE
        m = CType(Me.Owner, MERGE)

        Dim b1 As String = m.cl_tb.Text
        Dim i As Integer = 0
        Dim r As New System.Text.RegularExpressions.Regex( _
"0x[0-9A-F]{8} 0x[0-9A-F]{8}", _
System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim ed As System.Text.RegularExpressions.Match = r.Match(b1)
        While ed.Success
            DataGridView1.Rows.Add()
            DataGridView1.Rows(i).Cells(0).Value = ed.Value.Substring(0, 10)
            DataGridView1.Rows(i).Cells(1).Value = ed.Value.Substring(11, 10)
            ed = ed.NextMatch()
            i += 1
        End While

        Dim column As New DataGridViewComboBoxColumn()
        'ComboBoxのリストに表示する項目を指定する
        column.Items.Add("DEC")
        column.Items.Add("OR")
        column.Items.Add("AND")
        column.Items.Add("XOR")
        column.Items.Add("BINARY32")
        column.Items.Add("BINARY32(16bit)")
        column.Items.Add("BINARY16")
        '"Week"列にバインドされているデータを表示する
        column.DataPropertyName = "編集タイプ"
        '"Week"列の代わりにComboBox列を表示する
        DataGridView1.Columns.Insert(DataGridView1.Columns("編集タイプ").Index, column)
        DataGridView1.Columns.Remove("編集タイプ")
        column.Name = "編集タイプ"

    End Sub

    Private Sub edival(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim m As MERGE
        m = CType(Me.Owner, MERGE)
        Dim d As Integer = DataGridView1.CurrentCell.RowIndex
        Dim c As Integer = DataGridView1.CurrentCell.ColumnIndex
        If Not DataGridView1.Rows(d).Cells(2).Value Is Nothing And Not DataGridView1.Rows(d).Cells(3).Value Is Nothing Then
            Dim check As String = DataGridView1.Rows(d).Cells(2).Value.ToString
            If check = "DEC" Then
                Dim dec As Integer = Convert.ToInt32(DataGridView1.Rows(d).Cells(3).Value.ToString)
                DataGridView1.Rows(d).Cells(1).Value = "0x" + dec.ToString("X").PadLeft(8, "0"c).ToUpper
            ElseIf check = "OR" Then
                Dim b1 As Int32 = Convert.ToInt32(DataGridView1.Rows(d).Cells(1).Value.ToString, 16)
                Dim hex As Int32 = Convert.ToInt32(DataGridView1.Rows(d).Cells(3).Value.ToString, 16)
                DataGridView1.Rows(d).Cells(1).Value = "0x" + Convert.ToString((b1 Or hex), 16).PadLeft(8, "0"c).ToUpper
            ElseIf check = "AND" Then
                Dim b1 As Int32 = Convert.ToInt32(DataGridView1.Rows(d).Cells(1).Value.ToString, 16)
                Dim hex As Int32 = Convert.ToInt32(DataGridView1.Rows(d).Cells(3).Value.ToString, 16)
                DataGridView1.Rows(d).Cells(1).Value = "0x" + Convert.ToString((b1 And hex), 16).ToString.PadLeft(8, "0"c).ToUpper
            ElseIf check = "XOR" Then
                Dim b1 As Int32 = Convert.ToInt32(DataGridView1.Rows(d).Cells(1).Value.ToString, 16)
                Dim hex As Int32 = Convert.ToInt32(DataGridView1.Rows(d).Cells(3).Value.ToString, 16)
                DataGridView1.Rows(d).Cells(1).Value = "0x" + Convert.ToString((b1 Xor hex), 16).ToString.PadLeft(8, "0"c).ToUpper
            Else
                Dim f As Single = Convert.ToSingle(DataGridView1.Rows(d).Cells(3).Value.ToString)
                Dim bit() As Byte = BitConverter.GetBytes(f)
                Dim sb As New System.Text.StringBuilder()
                Dim i As Integer = 3
                While i >= 0
                    sb.Append(Convert.ToString(bit(i), 16).PadLeft(2, "0"c))
                    i -= 1
                End While
                Dim half As String = DataGridView1.Rows(d).Cells(1).Value.ToString.Substring(0, 6)
                If check = "BINARY32" Then
                    DataGridView1.Rows(d).Cells(1).Value = "0x" + sb.ToString.ToUpper
                ElseIf check = "BINARY32(16bit)" Then
                    DataGridView1.Rows(d).Cells(1).Value = half + sb.ToString.Substring(0, 4).ToUpper
                ElseIf check = "BINARY16" Then
                    Dim hf As String = sb.ToString
                    hf = converthalffloat(hf)
                    DataGridView1.Rows(d).Cells(1).Value = half & hf
                End If
            End If
        End If
        Dim gridtx As String = Nothing
        Dim k As Integer = 0
        While Not DataGridView1.Rows(k).Cells(0).Value Is Nothing
            gridtx &= DataGridView1.Rows(k).Cells(0).Value.ToString & " "
            gridtx &= DataGridView1.Rows(k).Cells(1).Value.ToString & vbCrLf
            k += 1
        End While
        m.cl_tb.Text = gridtx
    End Sub

    'IEE754単精度浮動小数点binary32を半精度浮動小数点binary16に変換 Cから移植、VB.NET
    Function converthalffloat(ByVal hf As String) As String
        Dim hex As Integer = Convert.ToInt32(hf, 16)
        Dim sign As Integer = (hex And &H80000000)\ &H10000
        Dim exponent As Integer = (hex And &H7F800000) \ &H800000
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
        '(0x3C00>10)&0x1F=15,0x3C00&0x3FF=0
        exponent -= 112
        exponent *= &H400
        fraction \= 8192
        If sign = -32768 Then
            sign = 32768
        End If
        hex = exponent + fraction
        If hex > &H7C00 Then '無限
            hex = &H7F80 '数字以外のなにか
        End If
        hex += sign
        hf = hex.ToString("X")

        Return hf
    End Function
End Class