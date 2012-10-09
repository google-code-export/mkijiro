Imports System
Imports System.IO
Imports System.Text

Public Class Form4

    Private Sub INI(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        TextBox1.Text = My.Settings.unicodein
        TextBox2.Text = My.Settings.unicodeout
        Select Case My.Settings.cptype
            Case "SJIS"
                RadioButton1.Checked = True

            Case "EUC"
                RadioButton2.Checked = True

            Case "GBK"
                RadioButton3.Checked = True
        End Select

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        My.Settings.unicodein = openfile(My.Settings.unicodein)
        TextBox1.Text = My.Settings.unicodein
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        My.Settings.unicodeout = openfile(My.Settings.unicodeout)
        TextBox2.Text = My.Settings.unicodeout
    End Sub

    Private Function openfile(ByVal tst As String) As String
        Dim ofd As New OpenFileDialog()
        Dim dirst As String = ""
        If tst = "" Then
            tst = Application.StartupPath & "\table\"
        Else
            tst = Path.GetDirectoryName(tst)
        End If

        ofd.InitialDirectory = dirst
        ofd.Filter = "TBLファイル(*.tbl)|*.tbl|すべてのファイル(*.*)|*.*"
        'タイトルを設定する
        ofd.Title = "開くファイルを選択してください"
        ofd.RestoreDirectory = True
        ofd.CheckFileExists = True
        ofd.CheckPathExists = True
        If ofd.ShowDialog() = DialogResult.OK Then
            tst = ofd.FileName
            If tst.Contains(Application.StartupPath & "\table\") Then
                tst = tst.Replace(Application.StartupPath & "\", "")
                Return tst
            Else
                MessageBox.Show("tableフォルダ内にTBLファイルをおいて下さい")
            End If
        End If

        Return tst

    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class