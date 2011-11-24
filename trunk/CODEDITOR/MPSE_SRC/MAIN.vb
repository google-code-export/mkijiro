Option Explicit On
Imports System.Media

#Region "インポート"

Imports System
Imports System.Net
Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.Devices
Imports Microsoft.VisualBasic
Imports System.Text.RegularExpressions

#End Region


#Region "メイン　フォーム"

Public Class MAIN

#Region "共有変数"

    Dim File As New Api.File
    Dim Str As New Api.Str
    Dim Etc As New Api.Etc
    Dim Ctr As New Api.Ctrl
    Dim Net As New Api.Net

    Dim sc_abort As Long = 0
    Dim cnt As Long = 0 'ループカウンタ
    Dim slt As Long = 1 '前に選択したFile No
    Dim slt_cnt As Long = 1 'ファイル数カウンタ
    Dim ofct As String 'オフセット表示用
    Dim flag As Long = 0 'tmpフォルダ作成後に処理開始を許可するコントロール用のフラグ
    Dim flag2 As Long = 0 'File No つまみ誤動作防止用のフラグ
    Dim sc_type As Boolean = True '再開時の解析開始タイプ　記憶用
    Dim save_folder As String = Application.StartupPath() & "\tmp" 'デフォルトの参照先
    Dim index As Integer 'リスト非復元時に各開始アドレスを自動設定するためのIndex保存用
    Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding("Shift-JIS") 'テキストのエンコード指定
    Dim clip_tmp As String
    Dim code_s As Integer = 0
    Dim code_f As Integer = 0

    Const Def_adr As String = "0x0000000"
    Const Max_adr As String = "0x17FFFFF"
    Const Def_adr_real As String = "0x8800000"
    Const Max_adr_real As String = "0x9FFFFFF"
    Const P_Base As Long = &H8800000


    Dim step_ct As Integer
    Dim z_hit As Long = 0

    Dim step_fn As Integer
    Dim b_slt As Long
    Dim list_tmp As String


    Dim Thread As Boolean = False

#End Region


#Region "プログラム起動時の初期設定"

    Public Sub New()

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。


        '参照先テキストボックスの内容をソフトがあるパスに設定
        dir.Text = Application.StartupPath()

        'オフセット符号　+/- を選択済みに
        Offcet_Type.SelectedIndex = 0

        'ツールがあるフォルダに一時ファイル保存フォルダ(tmp)を作成　(既にあれば削除してから)
        File.Del_Dir(Application.StartupPath() & "\tmp")

        My.Computer.FileSystem.CreateDirectory(Application.StartupPath() & "\tmp")

        'tmpフォルダ作成後に処理開始を許可するコントロール用のフラグ
        flag = 1

        '各ツールチップの文字設定

        '対象　関係
        ofct_min_TP.SetToolTip(offcet_min, "input minimam offset.")
        ofct_max_TP.SetToolTip(offcet_max, "input maxumum offset.")
        ofct_type_TP.SetToolTip(Offcet_Type, "set offset type + or -")

        '設定　関係
        FileNo_TP.SetToolTip(NumericUpDown1, "click（▲▼）,change using dump files." _
                             & vbCrLf & "set address/dump before next dump." _
                             & vbCrLf & "And file the slider up or down, the contents of the address will be updated to the File No-enabled.")
        filetx_TP.SetToolTip(file1, "drag & drop dump files.")
        adr_TP.SetToolTip(fl_adr, "Address, search results corresponding to the number of files Dump files.")
        OpenDir_TP.SetToolTip(dmp_open, "Specifies the file number corresponding to dump flies.")

        'オプション　関係
        Active_TP.SetToolTip(CheckBox1, "After the analysis is enabled, " & vbCrLf & " active window to let you know")
        Reset_O_TP.SetToolTip(CheckBox2, "clear button is enabled, " & vbCrLf & " You can leave setting.")
        adrmach_TP.SetToolTip(CheckBox3, "Detailed written statement for advanced features so that you use the ReadMe.txt.")

        'その他　関係
        dir_TP.SetToolTip(tmp_open, "Open the references folder")


        Start_TP.SetToolTip(start, "To begin the analysis in the current configuration")
        Abort_TP.SetToolTip(abort, "Confirmation screen if you select the " & vbCrLf & " You can redo the analysis to interrupt the process")
        ProgressBar_TP.SetToolTip(ProgressBar1, "Displays real-time status after the start of analysis")
        Info_TP.SetToolTip(Info, "File to specify the number three, the analysis results in three files to the " & vbCrLf & " will be back to see the same address offset value " & vbCrLf & " File 3 - If you want to check the address of the match " & vbCrLf & "Please press this button after selecting any item from the list of analysis results")
        folder_TP.SetToolTip(dir, "References folder of the files used in analysis and restore")
        Combobox_TP.SetToolTip(ComboBox1, "This list appears to have hit after data analysis" & vbCrLf & vbCrLf _
                               & "=Before and after the address is always one or two files in the specified file number to the case of three" & vbCrLf _
                               & "Now to see the third match of the address offset value is omitted" & vbCrLf _
                               & "If you want to address all the files, please click details" & vbCrLf & vbCrLf _
                               & "★" & vbTab & "=" & vbTab & "Candidate base address" & vbCrLf _
                               & "＃" & vbTab & "=" & vbTab & "Analysis/selected address in the past")

        ListBox_TP.SetToolTip(ListBox1, "History will be added to the analysis for each" & vbCrLf & vbCrLf & "'N second start address - the address' and they are always one or two files are " & vbCrLf & " 3 to display the address of the file is truncated")

        '機能　関係
        Restore_TP.SetToolTip(Restore, "Restore the selected item from the Run History" & vbCrLf & "'N second start address - the address' please press this button after selecting an item, including the " & vbCrLf & vbCrLf & " ※ execution history are selected below will be deleted " & vbCrLf & " start address of each file analysis Each set will be automatically restored when")
        Reset_TP.SetToolTip(Reset, "Back to the state at startup to initialize the items are turned off " & vbCrLf & vbCrLf & " in the options' configuration file to initialize leave 'check (enabled) and the " & vbCrLf & "The information is not clear the file" & vbCrLf & vbCrLf & "※ analysis of the results list and to initialize and run history will be cleared")

        '解析ファイル　関係
        Save_TP.SetToolTip(saved, "To save the current file, " & vbCrLf & " You can resume the next time you start or stop working")
        Restart_TP.SetToolTip(Restart, "Folder (file) to the resumption of the work load from the previous state")

        'コード化　関係
        Code_BTN_TP.SetToolTip(code, "If you find the base address from the list ★ analysis results and press this button after selecting the objects, including the mark " & vbCrLf & " in the CWC and the configuration information for each folder to see the code generated automatically format Code. txt output as")
        Code_B_TP.SetToolTip(PrevC, "★ If you press this button to have more than one token, " & vbCrLf & " Automatically select the previos candidates.")
        Code_A_TP.SetToolTip(NextC, "★ If you press this button to have more than one token, "& vbCrLf &" Automatically select the next candidate. ")
        '初期ステータス設定
        Status.Text = "wait"
        Ctr.Change_Status(GroupBox11, Status, , False)

        If My.Computer.Clipboard.ContainsText Then
            clip_tmp = My.Computer.Clipboard.GetText
        End If

        Timer1.Enabled = True

        'Me.Text = "Multi Pointer Searcher Ver " & Microsoft.VisualBasic.Left(Application.ProductVersion, 5)

        'My.Settings.SOUND = False
        'My.Settings.CHECK_UPDATE = False
        'My.Settings.DL_URL = ""

        Read_ini()


        If My.Computer.Network.IsAvailable Then

            '    If My.Settings.CHECK_UPDATE And Net.IsInternetConnected(My.Settings.DL_URL) Then
            '        Updata_Check()
            '    End If

        End If

    End Sub

#End Region


#Region "INIファイル読み込み"

    Public Sub Read_ini()

        If My.Computer.FileSystem.FileExists(Application.StartupPath() & "\Setting.ini") Then


            Dim dt(2), tmp As String

            'set.cfgから前回の設定を読み込む
            Using sr As New System.IO.StreamReader(Application.StartupPath() & "\Setting.ini", System.Text.Encoding.GetEncoding("Shift-JIS"))

                Do Until sr.Peek() = -1

                    tmp = sr.ReadLine

                    If InStr(tmp, "'") = 0 Then

                        tmp = Replace(tmp, vbTab, "")

                        dt = Split(tmp, "=")

                        Select Case dt(0).ToUpper

                            Case "SOUND"
                                'My.Settings.SOUND = dt(1)

                            Case "CHECK_UPDATE"
                                'My.Settings.CHECK_UPDATE = dt(1)

                            Case "DL_URL"
                                'My.Settings.DL_URL = dt(1)

                        End Select

                    End If

                Loop

            End Using
        End If

    End Sub

#End Region


#Region "アップデートチェック"

    Public Sub Updata_Check()

        'If Net.File_DL(My.Settings.DL_URL & My.Application.Info.AssemblyName & ".txt", Application.StartupPath() & "\" & My.Application.Info.AssemblyName & ".txt") Then

        '    Dim tmp As String = File.Read_Data(Application.StartupPath() & "\" & My.Application.Info.AssemblyName & ".txt", 0)

        '    File.Del(Application.StartupPath() & "\" & My.Application.Info.AssemblyName & ".txt")

        '    If Microsoft.VisualBasic.Left(Application.ProductVersion, 5) <> tmp Then

        '        MsgBox("Ver " & tmp & " is released")
        '        Process.Start(My.Settings.DL_URL)

        '    End If

        'End If

    End Sub

#End Region


#Region "各ボタンが押された時の処理"

    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
    tmp_open.Click, _
    PrevC.Click, _
    NextC.Click, _
    Info.Click, _
    dmp_open.Click


        Select Case sender.Name

            Case "tmp_open"

                File.Open_Dir(dir.Text)

            Case "PrevC"

                Dim i As Integer = ComboBox1.SelectedIndex - 1
                Dim j, k As Integer

                k = -1

                For j = i To 0 Step -1

                    If InStr(ComboBox1.Items.Item(j), "★") Then

                        k = j
                        Exit For

                    End If

                Next

                If k <> -1 Then

                    ComboBox1.SelectedIndex = k

                End If


            Case "NextC"

                Dim i As Integer = ComboBox1.SelectedIndex + 1
                Dim j, k As Integer

                k = -1

                For j = i To ComboBox1.Items.Count - 1

                    If InStr(ComboBox1.Items.Item(j), "★") Then

                        k = j
                        Exit For

                    End If

                Next

                If k <> -1 Then

                    ComboBox1.SelectedIndex = k

                End If


            Case "Info"    '詳細ボタンを押した時の処理

                'jに解析結果リストで選択された項目値を設定
                Dim i As Integer
                Dim tmp = "", data() As String

                '一時ファイルから各ファイルのオフセット一致アドレスを抽出

                '各情報を読み込み、各データを,で分けて変数に格納
                'data(0)～data(,区切りのデータ最大個数 - 2)　までがファイル数分の各オフセット一致アドレス
                data = Split(File.Read_Data(save_folder & "\" & cnt & ".cfg", ComboBox1.SelectedIndex), ",")

                tmp = "common offset = " & data(data.GetUpperBound(0) - 1) & " 0x" & data(data.GetUpperBound(0)) & vbCrLf & vbCrLf & "ファイルそれぞれの" & vbCrLf & "オフセット値 一致アドレス" & vbCrLf & vbCrLf

                'data(0)～data(,区切りのデータ最大個数 - 2)　までがファイル数分の各オフセット一致アドレスなので
                'それらをメッセージ表示用にファイル数分　追加
                For i = 0 To data.GetUpperBound(0) - 2

                    tmp &= "file" & (i + 1) & " = 0x" & data(i) & vbCrLf

                Next

                MsgBox(tmp, , "details")

            Case "dmp_open"    'ダンプファイル　開くボタンを押した時の処理

                'ファイルを開くダイアログボックスを表示する
                Dim openFileDialog1 As New OpenFileDialog

                With openFileDialog1

                    '起動ディレクトリを設定
                    .InitialDirectory = Application.StartupPath()

                    '[ファイルの種類] ボックスに表示される選択肢を設定する
                    .Filter = "all dump (*.dmp;*.mem;*.ram;*.bin)|*.dmp;*.mem;*.ram;*.bin|CWCheat/PME (*.dmp)|*.dmp|CheatMaster (*.mem)|*.mem|NitePR (*.ram)|*ram|CODEFREAK（*.bin）|*.bin"

                    '最初に表示するフィルタ処理オプションを設定する
                    .FilterIndex = 1

                    'ダイアログ ボックスを閉じる前に、現在のディレクトリを復元する
                    .RestoreDirectory = True

                    'ダイアログ ボックスを表示          [開く]ボタンが押されたら
                    If openFileDialog1.ShowDialog() = DialogResult.OK Then

                        ''選択したファイルのパスを取得
                        file1.Text = openFileDialog1.FileName

                        Dim r As New System.Text.RegularExpressions.Regex("0x[0-9a-fA-F]{0,8}", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                        Dim m As System.Text.RegularExpressions.Match = r.Match(openFileDialog1.SafeFileName)
                        If m.Success Then
                            fl_adr.Text = m.Value
                        End If
                    End If

                End With


        End Select


    End Sub

#End Region


#Region "開始ボタンを押した時の処理"

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles start.Click

        Dim tmp, data() As String

        tmp = ""

        'エラーの状態で処理分岐（解析開始の判断）
        Select Case Check_Err()

            Case 0 '解析開始　（エラーがない）


                sc_abort = 0

                '復元機能使用、リスト非復元=リストボックスのIndex 　リスト復元=コンボボックスのIndex
                Select Case sc_type

                    Case False

                        b_slt = index

                    Case True

                        b_slt = ComboBox1.SelectedIndex
                        list_tmp = ComboBox1.SelectedItem

                End Select


                '処理に不要なコントロールを有効・無効・初期化

                Timer1.Enabled = False

                '対象　関係
                offcet_min.ReadOnly = True
                offcet_max.ReadOnly = True
                Offcet_Type.Enabled = False

                '設定　関係
                NumericUpDown1.ReadOnly = True
                dmp_open.Enabled = False
                file1.ReadOnly = True
                fl_adr.ReadOnly = True

                'オプション　関係
                CheckBox1.Enabled = False
                CheckBox2.Enabled = False
                CheckBox3.Enabled = False


                'その他　関係
                ComboBox1.Items.Clear()
                ComboBox1.Enabled = False
                Info.Enabled = False
                start.Enabled = False
                abort.Enabled = True
                ProgressBar1.Value = 0

                '機能　関係
                Restore.Enabled = False
                Reset.Enabled = False

                '解析ファイル　関係
                Restart.Enabled = False
                saved.Enabled = False

                'コード化　関係
                code.Enabled = False
                PrevC.Enabled = False
                NextC.Enabled = False

                'ループカウンタ
                cnt += 1

                'ファイル数に応じて最終ステップ数を設定
                'ファイル数が3～の場合は絞り込み解析のステップが追加される
                Select Case slt_cnt

                    Case 2

                        step_fn = 2

                    Case Else

                        step_fn = 3

                End Select


                If CheckBox3.Checked Then
                    step_fn += 1
                End If


                If ListBox1.Items.Count = 1 Then

                    '初回用メッセージを追加
                    ListBox1.Items.Add(" ")
                    ListBox1.Items.Add("--- Start ---")

                End If


                '解析するファイルに応じたアドレスの取得処理をカウンター値で処理分岐
                Select Case cnt

                    Case 1 '1回目なら初期ファイルから取得


                        'アドレス・オフセット情報
                        ofct = "The initial value of each file"

                    Case Else '2回目～は解析結果ファイルから取得


                        data = Split(File.Read_Data(save_folder & "\" & (cnt - 1) & ".cfg", b_slt), ","c)

                        'アドレス・オフセット情報
                        ofct = ("0x" & data(0)).PadRight(9, " "c) & " = " & ("0x" & data(1)).PadRight(9, " "c) & " [ " & data(data.GetUpperBound(0) - 1) & ("0x" & data(data.GetUpperBound(0))).PadLeft(9, " "c) & " ] # = " & b_slt

                End Select


                '解析結果リストに
                ListBox1.Items.Add(cnt.ToString.PadLeft(2, " "c) & "times  " & ofct)
                ListBox1.SelectedIndex = ListBox1.Items.Count - 1

                'Step1 各ファイルからポインター抽出処理
                GroupBox11.Text = "status ( Step 1 / " & step_fn & " ) "

                Status.Text = "finding pointer"

                step_ct = 1
                Thread = True

                STEP1.RunWorkerAsync(Offcet_Type.SelectedIndex)


            Case 1, 2, 3 '開始キャンセル、オフセット設定エラー、アドレス共通解析不要

                '解析不要またはキャンセルの為何もしない


            Case 4 'ファイルの有効指定数が最低限必要な2個になっていない

                ' Etc.Beep_Sound(My.Settings.SOUND, 4)
                SystemSounds.Beep.Play()

                tmp = "FileNo is currently selected is not set correctly for the information" _
                        & vbCrLf & "canceled the start of analysis" & vbCrLf & vbCrLf

                '設定のエラー状況に応じてメッセージと各コントロール設定

                If My.Computer.FileSystem.FileExists(file1.Text) = False And _
                (Str.Rep_Tex(fl_adr.Text) = 0 Or Str.Rep_Tex(fl_adr.Text) > Str.Rep_Tex(Max_adr)) Then '全ての情報がおかしい

                    tmp = "invaid address/dump files"

                    file1.Text = "NONE"
                    fl_adr.Text = Def_adr

                ElseIf My.Computer.FileSystem.FileExists(file1.Text) = False Then 'ファイル情報がおかしい

                    file1.Text = "NONE"
                    tmp = "Please set the correct file"

                Else 'アドレス設定がおかしい

                    fl_adr.Text = Def_adr
                    tmp = "Please set the correct address"

                End If

                tmp &= vbCrLf & vbCrLf & "Analysis has been canceled due to errors setting" _
                        & vbCrLf & "After the correct information for each 'start' button please"

                If NumericUpDown1.Value > 2 Then

                    tmp &= vbCrLf & vbCrLf & "※Because the files have already been two or more settings," _
                            & vbCrLf & "　If you push the start button to match the number of files from the検索File No" _
                            & vbCrLf & "　Analysis can start with just a few configuration files"
                End If

                MsgBox(tmp, , "setting error [ file(No) = " & NumericUpDown1.Value & " ]")

            Case 5

                ' Etc.Beep_Sound(My.Settings.SOUND, 4)
                SystemSounds.Beep.Play()

                MsgBox(" either the addresses or dump files" & vbCrLf & _
                "Some overlap with other configuration files" & vbCrLf & vbCrLf & _
                "To begin the analysis of the dump file for each file, the address" & vbCrLf & _
                "Must be set to avoid overlap with other" _
                , , "Configuration file is a duplicate error")

            Case 6

        End Select

    End Sub

#End Region


#Region "各解析　スレッド"

#Region "ステップ1"

    Private Sub Step1_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles STEP1.DoWork

        Dim back As System.ComponentModel.BackgroundWorker = CType(sender, System.ComponentModel.BackgroundWorker)

        Dim target, of_min, of_max As Long
        Dim name, fdata(255) As String

        Dim BUFSIZE As Long = 2097152 ' 1度に処理するサイズ 4の倍数
        Dim buf(BUFSIZE + 1) As Byte ' 読み込み用バッファ

        Dim readSize, readtmp As Long ' 読み込んだバイト数　記憶用

        Dim of_type As Long = CType(e.Argument, String)
        Dim intLE(7) As Byte
        Dim i, k, j, dt, hit As Long

        of_max = Str.Rep_Tex(offcet_max.Text)
        of_min = Str.Rep_Tex(offcet_min.Text)

        hit = 0
        readtmp = 0 '読込済みバッファをクリア

        'ファイル数分　ループ
        For k = 1 To slt_cnt

            '解析するファイルに応じたアドレスの取得処理をカウンター値で処理分岐
            Select Case cnt

                Case 1 '1回目なら初期ファイルから取得

                    fdata = Split(File.Read_Data(save_folder & "\" & k & ".file", 0), ",")

                    'ループ値 Kに対応する初期ファイルからアドレス　取得
                    target = Str.Rep_Tex(fdata(0))

                    'ループ値 Kに対応する初期ファイルからファイル名　取得
                    name = fdata(1)


                Case Else '2回目～は解析結果ファイルから取得

                    fdata = Split(File.Read_Data(save_folder & "\" & k & ".file", 0), ",")

                    'ループ値 Kに対応する初期ファイルからファイル名　取得
                    name = fdata(1)

                    fdata = Split(File.Read_Data(save_folder & "\" & (cnt - 1) & ".cfg", b_slt), ",")

                    '解析結果ファイルからアドレス　取得
                    target = Str.Rep_Tex(fdata(k - 1))

            End Select


            'ファイルの解析開始
            Using src As New FileStream(name, FileMode.Open, FileAccess.Read)

                src.Seek(0, SeekOrigin.Begin)  '先頭までseek


                '解析対象ファイルを開く
                Using Writer As New IO.StreamWriter(save_folder & "\dmp" & k & ".csv")

                    'ファイル解析　メインループ
                    While True

                        readSize = src.Read(buf, 0, BUFSIZE) ' 読み込み

                        readtmp += readSize 'プログレスバー用に読込済みバッファ数を更新

                        '読込状況をプログレスバーに反映
                        back.ReportProgress((readtmp / (25165823 * slt_cnt)) * 100)

                        '読み込んだバッファサイズで処理分岐
                        Select Case readSize

                            Case 0 'ファイル位置が最後
                                Exit While ' 解析完了

                            Case Else 'まだ最後まで到達してない

                                '読み込んだバッファを最後まで処理
                                For i = 4 To readSize Step 4

                                    If back.CancellationPending Then
                                        Exit For
                                    End If


                                    '読み込んだデータからポインターの最小値(0x8800000)より小さい場合はすぐ次へ
                                    If buf(i - 1) = 8 Or buf(i - 1) = 9 Then
                                        'If buf(i - 2) > 127 Then
                                        'If buf(i - 1) > 7 Then


                                        'buf内のByte配列を32bitの数値に
                                        'dt = buf(i - 4) + buf(i - 3) * 2 ^ 8 + buf(i - 2) * 2 ^ 16 + buf(i - 1) * 2 ^ 24
                                        Array.ConstrainedCopy(buf, i - 4, intLE, 0, 4)
                                        dt = BitConverter.ToInt64(intLE, 0)
                                        j = src.Position - BUFSIZE + i - 4


                                        'オフセット符号選択が　+/- , + の場合
                                        If of_type <> 2 Then

                                            'オフセット対象　プラスのみ

                                            '設定したオフセット値　最小・最大の範囲内かどうか
                                            If j <> target Then
                                                If (target - (dt - P_Base)) >= of_min Then
                                                    If (target - (dt - P_Base)) <= of_max Then

                                                        '一致したアドレス、対象までのオフセット値、符号を書き込む
                                                        Writer.WriteLine(Hex(j) & "," & Hex(target - (dt - P_Base)) & ",+", enc)

                                                        If k = 1 Then
                                                            hit += 1
                                                        End If

                                                    End If

                                                End If

                                            End If
                                        End If


                                        'オフセット符号選択が　+/- , - の場合
                                        If of_type <> 1 Then

                                            'オフセット対象　マイナスのみ

                                            '設定したオフセット値　最小・最大の範囲内かどうか
                                            If j <> target Then
                                                If ((dt - P_Base) - target) >= of_min Then
                                                    If ((dt - P_Base) - target) <= of_max Then

                                                        If of_type = 2 Or (of_type = 0 And ((dt - P_Base) - target) <> 0) Then

                                                            '一致したアドレス、対象までのオフセット値、符号を書き込む
                                                            Writer.WriteLine(Hex(j) & "," & Hex((dt - P_Base) - target) & ",-", enc)

                                                            If k = 1 Then
                                                                hit += 1
                                                            End If

                                                        End If

                                                    End If

                                                End If

                                            End If

                                        End If

                                    End If

                                    '  End If


                                Next

                        End Select

                        If back.CancellationPending Then
                            Exit While
                        End If

                    End While

                End Using

            End Using

            If back.CancellationPending Then
                Exit For
            End If

        Next

        If back.CancellationPending Then
            e.Cancel = True
        End If

        z_hit = hit
        e.Result = 1

    End Sub

#End Region 'ファイルからポインター抽出


#Region "ステップ2"

    Private Sub Step2_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles STEP2.DoWork

        Dim back As System.ComponentModel.BackgroundWorker = CType(sender, System.ComponentModel.BackgroundWorker)

        Dim k, i, hit, tmp As Long
        Dim fdata(), sdata() As String

        i = 0
        hit = 0
        tmp = (z_hit * (slt_cnt - 1))


        '比較処理を開始（mchファイルを作る）

        'dmp1.csvを基準にして比較していく
        For k = 2 To slt_cnt

            'ファイル1(dmp1.csv)をオープン
            Using csv1 As New System.IO.StreamReader(save_folder & "\dmp1.csv", System.Text.Encoding.Default)

                'ループ値に応じてファイル名を付けた結果ファイルの出力準備
                Using Writer As New IO.StreamWriter(save_folder & "\mch" & (k - 1) & ".csv")

                    Do Until csv1.Peek() = -1

                        'dmp1.csvから取得したデータ
                        fdata = Split(csv1.ReadLine(), ",")

                        'ループ値に応じた比較対象ファイルを開く
                        Using csv2 As New System.IO.StreamReader(save_folder & "\dmp" & k & ".csv", System.Text.Encoding.Default)

                            Do Until csv2.Peek() = -1 'ファイルの最後までループ

                                If back.CancellationPending Then
                                    Exit Do
                                End If

                                sdata = Split(csv2.ReadLine(), ",") 'CSVデータを,区切りで切り出す

                                'アドレス1,2に共通するオフセット　符号だった場合はコンボボックスに追加

                                'オプション　アドレス末尾一致の選択項目別に処理分岐

                                If fdata(1) = sdata(1) Then
                                    If fdata(2) = sdata(2) Then
                                        If fdata(0)(Len(fdata(0)) - 1) = sdata(0)(Len(sdata(0)) - 1) Then

                                            'ファイルに書き込み
                                            Writer.WriteLine(fdata(0) & "," & sdata(0) & "," & fdata(2) & "," & fdata(1))

                                            If k = 2 Then
                                                hit += 1
                                            End If

                                        End If

                                    End If

                                End If

                            Loop

                        End Using

                        If back.CancellationPending Then
                            Exit Do
                        End If

                        i += 1

                        back.ReportProgress((i / tmp) * 100)


                    Loop

                End Using

            End Using

            If back.CancellationPending Then
                Exit For
            End If

        Next

        If back.CancellationPending = False Then

            'リネーム予定のファイルが存在したら削除
            File.Del(save_folder & "\" & cnt & ".cfg")

            'mch1.csv を ループ値.cfg にリネーム　（解析結果ファイル）
            Rename(save_folder & "\mch1.csv", save_folder & "\" & cnt & ".cfg")

        End If


        If back.CancellationPending Then
            e.Cancel = True
        End If

        z_hit = hit
        e.Result = 2

    End Sub

#End Region '抽出された各ファイルのポインターからファイルの結果と共通する部分のみ残す


#Region "ステップ3"

    Private Sub BackgroundWorker3_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles STEP3.DoWork

        Dim back As System.ComponentModel.BackgroundWorker = CType(sender, System.ComponentModel.BackgroundWorker)

        Dim i, j, k, x As Long
        Dim hit, hit_tmp As Long
        Dim fdata(), sdata(), tmp As String


        Dim loop_num As Long = slt_cnt - 2

        hit = z_hit

        ' cnt値.file を基準に前の比較対象で出来た最後のファイルまでループ
        For k = 2 To slt_cnt - 1

            hit_tmp = hit
            hit = 0
            j = 0


            Using st_cfg As New System.IO.StreamReader(save_folder & "\" & cnt & ".cfg", System.Text.Encoding.Default)

                Using tmp_csv As New IO.StreamWriter(save_folder & "\tmp.csv")

                    Do Until st_cfg.Peek() = -1 'ファイルの最後までループ

                        fdata = Split(st_cfg.ReadLine(), ",") 'CSVデータを,区切りで切り出す

                        'ファイル2(dmp2.csv)をオープン
                        Using main_csv As New System.IO.StreamReader(save_folder & "\mch" & k & ".csv", System.Text.Encoding.Default)

                            Do Until main_csv.Peek() = -1

                                If back.CancellationPending Then
                                    Exit Do
                                End If

                                sdata = Split(main_csv.ReadLine(), ",") 'CSVデータを,区切りで切り出す

                                If fdata(0) = sdata(0) Then
                                    If fdata(fdata.GetUpperBound(0) - 1) = sdata(sdata.GetUpperBound(0) - 1) Then
                                        If fdata(fdata.GetUpperBound(0)) = sdata(sdata.GetUpperBound(0)) Then

                                            tmp = fdata(0)

                                            For i = 1 To fdata.GetUpperBound(0) - 2
                                                tmp &= "," & fdata(i)
                                            Next

                                            tmp_csv.WriteLine(tmp & "," & sdata(1) & "," & fdata(fdata.GetUpperBound(0) - 1) & "," & fdata(fdata.GetUpperBound(0)))

                                            hit += 1

                                            Exit Do

                                        End If
                                    End If
                                End If

                            Loop

                        End Using


                        If back.CancellationPending Then
                            Exit Do
                        End If

                        j += 1

                        x = ((j / hit_tmp) * 100) / loop_num
                        x += (100 / loop_num) * (k - 2)

                        back.ReportProgress(x)

                    Loop

                End Using


            End Using

            If back.CancellationPending Then
                Exit For
            End If

            File.Del(save_folder & "\" & cnt & ".cfg")
            Rename(save_folder & "\tmp.csv", save_folder & "\" & cnt & ".cfg")

        Next

        If back.CancellationPending Then
            e.Cancel = True
        End If

        z_hit = hit
        e.Result = 3

    End Sub

#End Region '残されたポインターから全ファイルに共通しないオフセットを除外


#Region "ステップ4"

    Private Sub BackgroundWorker4_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles STEP4.DoWork

        Dim back As System.ComponentModel.BackgroundWorker = CType(sender, System.ComponentModel.BackgroundWorker)

        Dim data(), tmp, fdata() As String
        Dim dts, dtf, j, i, k, hit As Long

        Using cnt_fl As New System.IO.StreamReader(save_folder & "\" & cnt & ".cfg", System.Text.Encoding.Default)

            Using Writer As New IO.StreamWriter(save_folder & "\tmp.cfg")

                hit = 0
                k = 0

                Do Until cnt_fl.Peek() = -1

                    If back.CancellationPending Then
                        Exit Do
                    End If

                    tmp = cnt_fl.ReadLine()

                    fdata = Split(tmp, ",") '行を順に読み込んでいく

                    data = Split(Replace(tmp, "," & fdata(fdata.GetUpperBound(0) - 1) & "," & fdata(fdata.GetUpperBound(0)), ""), ",")

                    'Array.Sort(data, strlenComp)

                    j = 0

                    For i = 1 To slt_cnt - 1

                        dts = Val("&H" & data(i - 1)) - Str.Rep_Tex(TextBox1.Text)
                        dtf = Val("&H" & data(i - 1)) + Str.Rep_Tex(TextBox1.Text)

                        If Val("&H" & data(i)) >= dts Then
                            If Val("&H" & data(i)) <= dtf Then

                                j += 1

                            End If

                        End If
                    Next

                    If j = slt_cnt - 1 Then

                        '解析結果リストに追加
                        Writer.WriteLine(tmp)
                        hit += 1

                    End If

                    k += 1

                    back.ReportProgress((k / z_hit) * 100)


                Loop 'ループ値　最終解析結果ファイル(*.cfg) 最終行になるまでループ

            End Using

        End Using


        If back.CancellationPending = False Then

            File.Del(save_folder & "\" & cnt & ".cfg")
            Rename(save_folder & "\tmp.cfg", save_folder & "\" & cnt & ".cfg")

        End If


        If back.CancellationPending Then
            e.Cancel = True
        End If

        z_hit = hit
        e.Result = 4

    End Sub

#End Region '特定の領域外にある可能性の低い候補を除外


#Region "各解析スレッドの途中経過を取得"

    Private Sub ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles _
    STEP1.ProgressChanged, _
    STEP2.ProgressChanged, _
    STEP3.ProgressChanged, _
    STEP4.ProgressChanged

        '送られてきた情報をプログレスバーに反映
        ProgressBar1.Value = e.ProgressPercentage

    End Sub

#End Region


#Region "各解析スレッド　終了直後の処理"

    Private Sub RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles _
    STEP1.RunWorkerCompleted, _
    STEP2.RunWorkerCompleted, _
    STEP3.RunWorkerCompleted, _
    STEP4.RunWorkerCompleted

        Thread = False

        If e.Cancelled = False Then

            sc_abort = 0

            Select Case e.Result

                Case 1 'Step2 抽出されたポインターの解析処理

                    If z_hit Then

                        step_ct = 2
                        Status.Text = "finding pointer"
                        Thread = True
                        STEP2.RunWorkerAsync()

                    End If

                Case 2 'Step3 ファイル数が2個以上なら絞り込み解析開始

                    If slt_cnt > 2 And z_hit Then

                        step_ct = 3
                        Status.Text = "Refinements in the analysis"
                        Thread = True
                        STEP3.RunWorkerAsync()

                    End If

                Case 3 'Step4 可能性の低い候補を除外

                    If CheckBox3.Checked And z_hit Then

                        step_ct = 4
                        Status.Text = "Candidates in a less negative potential"
                        Thread = True
                        STEP4.RunWorkerAsync()

                    End If

                Case 4

                    Thread = False

            End Select


            If Thread = False And z_hit = 0 Then

                step_ct = step_fn

            End If

            GroupBox11.Text = "status ( Step " & step_ct & " / " & step_fn & " ) "


        End If


        If Thread = False And sc_abort <> 2 Then

            All_Thread_End() '全ての解析ステップ（スレッド）が停止していたら呼び出す

        End If

    End Sub

#End Region

#End Region


#Region "中断ボタンを押した時の処理"

    Private Sub Abort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles abort.Click

        'Etc.Beep_Sound(My.Settings.SOUND, 1)
        SystemSounds.Beep.Play()

        'ダイアログ1を呼び出して、選択結果によって処理分岐
        Select Case MsgBox("try to suspend the processing and restart analysis？", vbOKCancel Or vbQuestion, "中断")

            Case vbOK

                sc_abort = 1
                Thread_Cancel()

        End Select

    End Sub

#End Region


#Region "スレッドキャンセル"

    Sub Thread_Cancel()

        '直前に実行していたステップ数に応じたスレッドを停止
        Select Case step_ct

            Case 1
                STEP1.CancelAsync()

            Case 2
                STEP2.CancelAsync()

            Case 3
                STEP3.CancelAsync()

            Case 4
                STEP4.CancelAsync()

        End Select

    End Sub

#End Region


#Region "全解析スレッドが完全に完了した後の処理"

    Public Sub All_Thread_End()


        '全ての一時ファイル削除
        File.Del_All(save_folder, "*.csv")

        'プログレスバーを初期化
        ProgressBar1.Value = 0

        Select Case sc_abort

            Case 1

                sc_abort = 0
                File.Del(save_folder & "\" & cnt& & ".cfg")

                cnt -= 1

                GroupBox11.Text = "status"

                Status.Text = "wait（suspend）"

                ListBox1.Items.RemoveAt(ListBox1.Items.Count - 1)

                If sc_type = True Then

                    Make_ComboBox(cnt)
                    ComboBox1.SelectedIndex = b_slt

                End If

                If ComboBox1.Enabled = True Or sc_type = False Or cnt = 0 Then
                    start.Enabled = True
                End If

                'Reset.Enabled = True

            Case 0

                Dim i As Integer

                If z_hit > 0 Then
                    i = Make_ComboBox(cnt)
                End If

                'ステータスを待機に
                Status.Text = "wait（ended analysis）"

                'N回目終了を実行履歴に追加
                ListBox1.Items.Add(cnt.ToString.PadLeft(2, " ") & "times end  Hit = " & ComboBox1.Items.Count.ToString.PadRight(7, " ") & "   ★ = " & i.ToString.PadRight(7, " ")) '履歴に n回目の解析終了　項目追加
                ListBox1.SelectedIndex = ListBox1.Items.Count - 1

                'オプション　通知機能が有効の場合　ツールをアクティブにして通知
                If CheckBox1.Checked = True Then
                    Me.Activate()
                End If

                If cnt > 1 And InStr(list_tmp, "#") = 0 And sc_type Then
                    File.WriteLine(save_folder & "\try.cfg", (cnt - 1) & "," & File.Read_Data(save_folder & "\" & (cnt - 1) & ".cfg", b_slt), True)
                End If

                sc_type = True


                '最終的なヒット数で処理分岐
                Select Case z_hit

                    Case Is > 0 'ヒット数が1以上（続けて解析が可能）

                        start.Enabled = True

                        '解析家かリスト内に基本アドレス候補がある
                        If code.Enabled Then
                            MsgBox("Candidates found the same address offset value to both full file " & vbCrLf & " in the list of results with the analysis ★ Stars is the most likely base address " & vbCrLf & " ★ shall be marked looks for a stable and usable to all coded ", , " candidate base address ")
                        End If


                    Case 0 'ヒット数が0　再解析が必要

                        start.Enabled = False

                        'ヒットなしの通知
                        'MsgBox("did not hit. "& VbCrLf &" to restore any items recovered using "& vbCrLf &" may be re-analyzed and found "& vbCrLf & vbCrLf &" If you want to change the file analysis is initialized by pressing the button Please "," Analysis terminated. ")
                        ListBox1.Items.Add("--- Finish ---") '結果リストに解析終了を追加
                        ListBox1.SelectedIndex = ListBox1.Items.Count - 1

                End Select

        End Select


        '検索が終了した為、必要/不要なコントロールを有効・無効

        abort.Enabled = False

        Reset.Enabled = True

        If sc_type = True Or (sc_type = False And cnt > 0) Then

            'Restore.Enabled = True
            saved.Enabled = True

        End If

        offcet_min.ReadOnly = False
        offcet_max.ReadOnly = False
        Offcet_Type.Enabled = True

        CheckBox1.Enabled = True
        CheckBox2.Enabled = True
        CheckBox3.Enabled = True

        Restart.Enabled = True




    End Sub

#End Region


#Region "復元ボタンを押した時の処理"

    Private Sub Restore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Restore.Click

        Dim msg As String = "restore？"
        Dim ss(), s4 As String
        Dim ss2() As String = New String() {"", "", "", "", "", ""}
        Dim i, j As Integer

        If ListBox1.SelectedIndex + 3 < ListBox1.Items.Count Then

            s4 = ListBox1.Items.Item(ListBox1.SelectedIndex + 2)
            s4 = Replace(s4, " ", "")
            s4 = Replace(s4, "times start", "")
            s4 = Replace(s4, "[", "")
            s4 = Replace(s4, "]#=", ",")
            s4 = Replace(s4, "+", ",+")
            s4 = Replace(s4, "-", ",-")
            s4 = Replace(s4, "=", "")
            s4 = Replace(s4, "0x", ",")

            ss2 = Split(s4, ",")

        End If


        'コンボボックス1から選択項目の文字列取得
        s4 = ListBox1.SelectedItem

        '取得した文字列から不要な文字を排除
        s4 = Replace(s4, " ", "")
        s4 = Replace(s4, "The initial value of each file", "")
        s4 = Replace(s4, "times start", "")
        s4 = Replace(s4, "[", "")
        s4 = Replace(s4, "]#=", ",")
        s4 = Replace(s4, "+", ",+")
        s4 = Replace(s4, "-", ",-")
        s4 = Replace(s4, "=", "")
        s4 = Replace(s4, "0x", ",")

        ss = Split(s4, ",")


        MSG_RST.Label1.ForeColor = Color.Red
        MSG_RST.Label1.Text = Chr(&H22) & "　" & ListBox1.SelectedItem.ToString & "　" & Chr(&H22)

        Ctr.Change_Status(MSG_RST, MSG_RST.Label1, , False)
        Ctr.Change_Status(MSG_RST, MSG_RST.Label3, , False)
        Ctr.Change_Status(MSG_RST, MSG_RST.CheckBox1, , False)
        Ctr.Change_Status(MSG_RST, MSG_RST.TableLayoutPanel1, , False)

        RestoreList_TP.SetToolTip(MSG_RST.CheckBox1, "Back to the same state at the time of analysis results and restoration to take effect " & vbCrLf & " to discard the results of the analysis to select items to disable the " & vbCrLf & " to restore only the start address of each file analysis " & vbCrLf & vbCrLf & "reparse when you want to change the conditions you want to restore an item, please disable")
        If InStr(ListBox1.Items(ListBox1.SelectedIndex + 1), "Hit = 0") <> 0 Then

            MSG_RST.CheckBox1.Enabled = False
            MSG_RST.CheckBox1.Checked = False

            MSG_RST.Label2.Text = "※Zero hits for the selected item" _
            & vbCrLf & "　 	You must change the conditions and re-analysis" _
            & vbCrLf & "　 The analysis results can not be restored"
        Else

            MSG_RST.CheckBox1.Enabled = True
            MSG_RST.CheckBox1.Checked = True

            MSG_RST.Label2.Text = "※Reparse when you want to change the terms from the selected item" _
            & vbCrLf & "　 	Please do not check the list to restore the analysis results" _
            & vbCrLf & "　 （please disable）"

        End If

        Ctr.Change_Status(MSG_RST, MSG_RST.Label2, , False)

        'Etc.Beep_Sound(My.Settings.SOUND, 1)
        SystemSounds.Beep.Play()

        'ダイアログ1を呼び出して、選択結果によって処理分岐
        Select Case MSG_RST.ShowDialog()

            Case vbOK '

                '実行履歴の全項目数から選択された項目までを削除するために不要数取得
                j = ListBox1.Items.Count - ListBox1.SelectedIndex


                'オプション　解析結果リストを復元するかどうかで処理分岐
                Select Case MSG_RST.CheckBox1.Checked

                    Case False '復元しない

                        sc_type = False

                        'リストを復元しないので無効に
                        ComboBox1.Items.Clear()
                        ComboBox1.Enabled = False
                        Info.Enabled = False

                        code.Enabled = False
                        PrevC.Enabled = False
                        NextC.Enabled = False

                        '開始ボタンを有効
                        start.Enabled = True

                        saved.Enabled = True
                        'Restore.Enabled = True

                        'カウンタが1なら実行履歴をさらに2つ分消すため設定、保存ボタンを無効
                        If ss(0) > 1 Then
                            index = ss(5)
                        End If

                        cnt = ss(0) - 1 'カウンターをリセット


                    Case Else '復元する

                        sc_type = True '

                        If cnt <> ss(0) Or ComboBox1.Enabled = False Then

                            cnt = ss(0)

                            Make_ComboBox(cnt)


                            If ss2(5) <> "" Then
                                ComboBox1.SelectedIndex = ss2(5)
                            End If

                        End If

                        If ComboBox1.Enabled Then
                            start.Enabled = True
                        End If

                        '実行履歴で選択したところまで削除対象
                        j -= 2

                End Select

                '不要になった実行履歴を後ろから必要数分　削除
                For i = 1 To j
                    ListBox1.Items.RemoveAt(ListBox1.Items.Count - 1)
                Next

                ListBox1.SelectedIndex = ListBox1.Items.Count - 1

        End Select

    End Sub

#End Region


#Region "結果保存ボタンを押した時の処理"

    Private Sub saved_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saved.Click

        Dim i As Integer
        Dim name, tmp, data() As String
        tmp = ""

        'MsgBox(Path.GetDirectoryName(SaveFileDialog1.FileName) & "\" & Path.GetFileNameWithoutExtension(SaveFileDialog1.FileName))

        'ダイアログタイトル、フィルター設定
        SaveFileDialog1.Title = "And specify the folder name and location to save the results of analysis " & vbCrLf & " Please press the Save button. "
        'SaveFileDialog1.Filter = "テキスト ファイル|*.txt"

        'ダイアログを表示して最後に押したボタンで処理分岐
        Select Case SaveFileDialog1.ShowDialog()

            Case vbOK '

                name = Path.GetDirectoryName(SaveFileDialog1.FileName) & "\" & Path.GetFileNameWithoutExtension(SaveFileDialog1.FileName)

                If My.Computer.FileSystem.DirectoryExists(name) = False Then

                    My.Computer.FileSystem.CreateDirectory(name)


                    'ファイルコピーに備えて全てのファイル削除
                    File.Del_All(name, "*.file")
                    File.Del_All(name, "*.cfg")


                    File.Copy(save_folder & "\try.cfg", name & "\try.cfg")


                    '各ダンプファイルに関する設定を記憶したファイルをコピー
                    For i = 1 To slt_cnt
                        File.Copy(save_folder & "\" & i & ".file", name & "\" & i & ".file")
                    Next


                    '復元用ファイルをコピー
                    For i = 1 To cnt
                        File.Copy(save_folder & "\" & i & ".cfg", name & "\" & i & ".cfg")
                    Next


                    '実行履歴を選択されたフォルダにlistbox.cfgとして保存

                    For i = 0 To ListBox1.Items.Count - 1
                        File.WriteLine(name & "\listbox.cfg", ListBox1.Items.Item(i), True)
                    Next

                    tmp &= offcet_min.Text & vbCrLf 'オフセット最小値
                    tmp &= offcet_max.Text & vbCrLf 'オフセット最大値
                    tmp &= Offcet_Type.SelectedIndex & vbCrLf 'オフセット符号
                    tmp &= cnt & vbCrLf 'ループカウンタ値
                    tmp &= slt_cnt & vbCrLf 'ファイル数カウンタ値
                    tmp &= CheckBox1.Checked & vbCrLf 'オプション1 選択状況
                    tmp &= CheckBox2.Checked & vbCrLf 'オプション2 選択状況
                    tmp &= CheckBox3.Checked & vbCrLf 'オプション3 選択状況
                    tmp &= TextBox1.Text & vbCrLf
                    tmp &= sc_type & vbCrLf '再開時の解析タイプ
                    tmp &= index & vbCrLf '解析タイプが非復元の場合に参照するIndex


                    'set.cfgとして保存
                    File.Write(name & "\set" & Replace(Application.ProductVersion, ".", "") & ".cfg", tmp, False)

                    tmp = ""

                    tmp &= "■　info　■" & vbCrLf
                    tmp &= "save date = " & Now & vbCrLf
                    tmp &= vbCrLf
                    tmp &= "・Search results (analysis of an initial value of each file) & DANPUFAIRU specified " & vbCrLf

                    'ファイルに関する設定情報を読み込み、書き出し
                    For i = 1 To slt_cnt

                        data = Split(File.Read_Data(save_folder & "\" & i & ".file", 0), ",")
                        tmp &= "file" & i & " = 0x" & data(0).PadLeft(7, "0") & " ( " & data(1) & " )" & vbCrLf

                    Next

                    File.Write(name & "\info.txt", tmp)
                    MsgBox(Path.GetFileNameWithoutExtension(name) & "The name of the folder " & vbCrLf & " was saved to the location specified " & vbCrLf & vbCrLf & " ※ information stored in the folder. Txt is " & vbCrLf & " has been noted in the analysis of information. ", , "save successful")

                Else

                    MsgBox("To the specified folder already exists " & vbCrLf & " Could not save ", , " error ")

                End If

        End Select

    End Sub

#End Region


#Region "コード化ボタンを押した時の処理"

    Private Sub code_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles code.Click

        '解析リストで選択されたものが基本アドレス候補かどうか
        Select Case InStr(ComboBox1.SelectedItem, "★")

            Case Is > 0 '★があるので候補

                'ダイアログのタイトル、タイプの選択済み項目を設定
                CODE_SET.ComboBox1.SelectedIndex = 0

                'コード作成
                Select Case CODE_SET.ShowDialog() 'コードの設定画面を表示、最後に押されたボタンで処理分岐

                    Case vbOK 'OKボタンが押された

                        Dim i, j, k As Integer
                        Dim c_name, tmp, Code_txt, data(255), read(255)() As String

                        Code_txt = ""

                        If My.Computer.FileSystem.DirectoryExists(dir.Text & "\" & "Code") = False Then
                            My.Computer.FileSystem.CreateDirectory(dir.Text & "\" & "Code")
                        End If

                        j = 0

                        '実行履歴　2回目から最後までのアドレス、オフセット等を配列へ代入
                        For i = 5 To ListBox1.Items.Count - 1 Step 2

                            tmp = ListBox1.Items.Item(i)
                            tmp = Replace(tmp, " ", "")
                            tmp = Replace(tmp, "times start", "")
                            tmp = Replace(tmp, "[", "")
                            tmp = Replace(tmp, "]#=", ",")
                            tmp = Replace(tmp, "+", ",+")
                            tmp = Replace(tmp, "-", ",-")
                            tmp = Replace(tmp, "=", "")
                            tmp = Replace(tmp, "0x", ",")
                            tmp = Replace(tmp, " ", ",")

                            read(j) = Split(tmp, ",")

                            j += 1

                        Next i


                        '解析結果リストで選択された基本アドレス候補のアドレス、オフセットを配列に代入
                        tmp = ComboBox1.SelectedItem()

                        tmp = Replace(tmp, " ", "")
                        tmp = Replace(tmp, "=", "")
                        tmp = Replace(tmp, "#", "")
                        tmp = Replace(tmp, "[", ",")
                        tmp = Replace(tmp, "]", "")
                        tmp = Replace(tmp, "★", ",")
                        tmp = Replace(tmp, "0x", ",")
                        tmp = "1" & tmp

                        read(j) = Split(tmp, ",")

                        '## ここから ##
                        'コード　共通部分

                        c_name = "Code[" & "0x" & read(j)(1) & " " & read(j)(3) & "0x" & read(j)(4) & "]" & ".txt"


                        Dim bittype As Integer = CODE_SET.ComboBox1.SelectedIndex
                        If bittype = 3 Then
                            bittype = 2
                        End If

                        '参照先にCode.txtとして出力開始

                        Code_txt &= "CWC PointerCode" & vbCrLf
                        Code_txt &= "----------------------------" & vbCrLf

                        Dim coden As String = CODE_SET.TextBox1.Text
                        Dim code(50) As String

                        code(1) = "_L 0x6AAAAAAA 0xWWWWWWWW"
                        code(2) = "_L 0x000T000N 0x0ZZZZZZZ"


                        'コード名の指定がなければ自動設定
                        If coden = "" Then
                            coden = "New Cheat"
                        End If

                        'コード名　書込み
                        Code_txt &= "_C0 " & coden & vbCrLf


                        '基本アドレス（最終アドレス）を設定
                        code(1) = Replace(code(1), "AAAAAAA", read(cnt - 1)(1).PadLeft(7, "0"))

                        code(1) = Replace(code(1), "WWWWWWWW", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0")))


                        '書き込むデータタイプを最終オフセットの符号に合わせてT値設定
                        If read(0)(3) = "+" Then
                            code(2) = Replace(code(2), "T", (bittype))
                        Else
                            code(2) = Replace(code(2), "T", (bittype + 3))
                        End If


                        'CWCコードの追跡回数、書き込むタイプ
                        code(2) = Replace(code(2), "N", cnt)
                        'CWCコード　最終オフセット値
                        code(2) = Replace(code(2), "ZZZZZZZ", read(0)(4).PadLeft(7, "0"))

                        'コード　共通部分
                        '## ここまで ##


                        k = 2 'コード行数最小値=k をポインタ数　1個用にまず2に

                        If cnt > 1 Then 'ポインタ数(ループカウンタ）が2以上

                            k = 3 'コード'コード行数最小値=k を3～に修正

                            For i = 2 To cnt + 1 Step 2

                                If (i Mod 2 = 0) And (i < cnt) Then

                                    code(k) = "_L 0xGIIIIIII 0xHJJJJJJJ"

                                    code(k) = Replace(code(k), "IIIIIII", read(i)(4).PadLeft(7, "0"))
                                    code(k) = Replace(code(k), "JJJJJJJ", read(i - 1)(4).PadLeft(7, "0"))

                                    If read(i)(3) = "+" Then
                                        code(k) = Replace(code(k), "G", "2")
                                    Else
                                        code(k) = Replace(code(k), "G", "3")
                                    End If

                                    If read(i - 1)(3) = "+" Then
                                        code(k) = Replace(code(k), "H", "2")
                                    Else
                                        code(k) = Replace(code(k), "H", "3")
                                    End If


                                ElseIf (i = cnt) Then

                                    code(k) = "_L 0xGIIIIIII 0x00000000"

                                    code(k) = Replace(code(k), "IIIIIII", read(i - 1)(4).PadLeft(7, "0"))

                                    If read(i - 1)(3) = "+" Then
                                        code(k) = Replace(code(k), "G", "2")
                                    Else
                                        code(k) = Replace(code(k), "G", "3")
                                    End If

                                End If

                                '最後まで到達したらループから抜ける
                                If (i = cnt) Or (i + 1 = cnt) Then
                                    Exit For
                                Else
                                    k += 1 'まだ最後ではないのでk値を加算
                                End If

                            Next

                        End If


                        '設定されたコードを全てファイルに書き出し
                        For i = 1 To k
                            Code_txt &= code(i) & vbCrLf
                        Next

                        Code_txt = Code_txt & "" & vbCrLf
                        Code_txt = Code_txt & "ACTIONREPLAY PointerCode" & vbCrLf
                        Code_txt = Code_txt & "-----------------------------" & vbCrLf


                        Select Case cnt
                            Case 1 '単体ポインタの場合

                                'コードのベース
                                Dim codeAR0 As String = "_M 0x6AAAAAAA 0x00000000"
                                Dim codeAR1 As String = "_M 0xBAAAAAAA 0x00000000"
                                Dim codeAR2 As String = "_M 0xTIIIIIII 0xVVVVVVVV"

                                Dim codeAR3 As String = "_M 0xDC000000 0xIIIIIIII"
                                Dim codeAR4 As String = "_M 0xT0000000 0xVVVVVVVV"

                                Dim codeAR5 As String = "_M 0xD2000000 0x00000000"
                                Dim AddNum As String = "8800000"
                                Dim SubNum As String = "FFFFFFFF"
                                Dim OneNum As String = "00000001"
                                Dim result As String
                                Dim result2 As String

                                result = Format(CInt(Val("&h" + read(0)(1)) + Val("&h" + AddNum)), "X7")
                                result2 = Format(CInt(Val("&h" + SubNum) - Val("&h" + read(0)(4)) + Val("&h" + OneNum)), "X8")

                                'ベースの内容から必要なところを　値、各オフセット、基本アドレスに置き換え

                                codeAR0 = Replace(codeAR0, "AAAAAAA", result.PadLeft(7, "0"))
                                codeAR1 = Replace(codeAR1, "AAAAAAA", result.PadLeft(7, "0"))
                                
                                codeAR2 = Replace(codeAR2, "IIIIIII", read(0)(4).PadLeft(7, "0"))
                                codeAR2 = Replace(codeAR2, "VVVVVVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0")))

                                codeAR3 = Replace(codeAR3, "IIIIIIII", result2.PadLeft(8, "0"))
                                codeAR4 = Replace(codeAR4, "VVVVVVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0")))

                                Select Case CODE_SET.ComboBox1.SelectedIndex

                                    Case 0 '8bit

                                        codeAR2 = Replace(codeAR2, "T", "2")
                                        codeAR4 = Replace(codeAR4, "T", "2")

                                    Case 1 '16bit

                                        codeAR2 = Replace(codeAR2, "T", "1")
                                        codeAR4 = Replace(codeAR4, "T", "1")

                                    Case Else '32bit (入力文字数制限は最大32bitまでなのでそのまま）


                                        codeAR2 = Replace(codeAR2, "T", "0")
                                        codeAR4 = Replace(codeAR4, "T", "0")

                                End Select

                                '単体の場合はタイプが特別なのでそれに合わせる
                                If read(0)(3) = "+" Then

                                    'ファイルに書き出し
                                    Code_txt = Code_txt & "_C0 " & coden & vbCrLf
                                    Code_txt = Code_txt & codeAR0 & vbCrLf
                                    Code_txt = Code_txt & codeAR1 & vbCrLf
                                    Code_txt = Code_txt & codeAR2 & vbCrLf
                                    Code_txt = Code_txt & codeAR5 & vbCrLf

                                Else

                                    'ファイルに書き出し
                                    Code_txt = Code_txt & "_C0 " & coden & vbCrLf
                                    Code_txt = Code_txt & codeAR0 & vbCrLf
                                    Code_txt = Code_txt & codeAR1 & vbCrLf
                                    Code_txt = Code_txt & codeAR3 & vbCrLf
                                    Code_txt = Code_txt & codeAR4 & vbCrLf
                                    Code_txt = Code_txt & codeAR5 & vbCrLf

                                End If

                            Case 2 '2重

                                'コードのベース
                                Dim codeAR0 As String = "_M 0x6AAAAAAA 0x00000000"
                                Dim codeAR1 As String = "_M 0xBAAAAAAA 0x00000000"

                                Dim codeAR6 As String = "_M 0xBJJJJJJJ 0x00000000"

                                Dim codeAR7 As String = "_M 0xDC000000 0xJJJJJJJJ"
                                Dim codeAR8 As String = "_M 0xB0000000 0x00000000"

                                Dim codeAR2 As String = "_M 0xTIIIIIII 0xVVVVVVVV"

                                Dim codeAR3 As String = "_M 0xDC000000 0xIIIIIIII"
                                Dim codeAR4 As String = "_M 0xT0000000 0xVVVVVVVV"

                                Dim codeAR5 As String = "_M 0xD2000000 0x00000000"
                                Dim AddNum As String = "8800000"
                                Dim SubNum As String = "FFFFFFFF"
                                Dim OneNum As String = "00000001"
                                Dim p1 As String
                                Dim p2offne As String
                                Dim woffne As String

                                '最初のポインタ+0x8800000
                                p1 = Format(CInt(Val("&h" + read(1)(1)) + Val("&h" + AddNum)), "X7")
                                'ポインタオフセット負
                                p2offne = Format(CInt(Val("&h" + SubNum) - Val("&h" + read(1)(4)) + Val("&h" + OneNum)), "X8")
                                '書き込みオフセット負
                                woffne = Format(CInt(Val("&h" + SubNum) - Val("&h" + read(0)(4)) + Val("&h" + OneNum)), "X8")


                                'ベースの内容から必要なところを　値、各オフセット、基本アドレスに置き換え

                                codeAR0 = Replace(codeAR0, "AAAAAAA", p1.PadLeft(7, "0"))
                                codeAR1 = Replace(codeAR1, "AAAAAAA", p1.PadLeft(7, "0"))
                                codeAR6 = Replace(codeAR6, "JJJJJJJ", read(1)(4).PadLeft(7, "0"))
                                codeAR7 = Replace(codeAR7, "JJJJJJJJ", p2offne.PadLeft(8, "0"))

                                codeAR2 = Replace(codeAR2, "IIIIIII", read(0)(4).PadLeft(7, "0"))
                                codeAR2 = Replace(codeAR2, "VVVVVVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0")))

                                codeAR3 = Replace(codeAR3, "IIIIIIII", woffne.PadLeft(8, "0"))
                                codeAR4 = Replace(codeAR4, "VVVVVVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0")))

                                Select Case CODE_SET.ComboBox1.SelectedIndex

                                    Case 0 '8bit

                                        codeAR2 = Replace(codeAR2, "T", "2")
                                        codeAR4 = Replace(codeAR4, "T", "2")

                                    Case 1 '16bit

                                        codeAR2 = Replace(codeAR2, "T", "1")
                                        codeAR4 = Replace(codeAR4, "T", "1")

                                    Case Else '32bit (入力文字数制限は最大32bitまでなのでそのまま）


                                        codeAR2 = Replace(codeAR2, "T", "0")
                                        codeAR4 = Replace(codeAR4, "T", "0")

                                End Select

                                '正負場合わけ
                                If read(1)(3) = "+" Then

                                    'ファイルに書き出し
                                    Code_txt = Code_txt & "_C0 " & coden & vbCrLf
                                    Code_txt = Code_txt & codeAR0 & vbCrLf
                                    Code_txt = Code_txt & codeAR1 & vbCrLf
                                    Code_txt = Code_txt & codeAR6 & vbCrLf

                                Else

                                    'ファイルに書き出し
                                    Code_txt = Code_txt & "_C0 " & coden & vbCrLf
                                    Code_txt = Code_txt & codeAR0 & vbCrLf
                                    Code_txt = Code_txt & codeAR1 & vbCrLf
                                    Code_txt = Code_txt & codeAR7 & vbCrLf
                                    Code_txt = Code_txt & codeAR8 & vbCrLf

                                End If


                                If read(0)(3) = "+" Then

                                    Code_txt = Code_txt & codeAR2 & vbCrLf
                                    Code_txt = Code_txt & codeAR5 & vbCrLf

                                Else

                                    Code_txt = Code_txt & codeAR3 & vbCrLf
                                    Code_txt = Code_txt & codeAR4 & vbCrLf
                                    Code_txt = Code_txt & codeAR5 & vbCrLf

                                End If
                            Case 3 '3重

                                'コードのベース
                                Dim codeAR0 As String = "_M 0x6AAAAAAA 0x00000000"
                                Dim codeAR1 As String = "_M 0xBAAAAAAA 0x00000000"

                                Dim codeAR9 As String = "_M 0xBKKKKKKK 0x00000000"

                                Dim codeARA As String = "_M 0xDC000000 0xKKKKKKKK"
                                Dim codeARB As String = "_M 0xB0000000 0x00000000"

                                Dim codeAR6 As String = "_M 0xBJJJJJJJ 0x00000000"

                                Dim codeAR7 As String = "_M 0xDC000000 0xJJJJJJJJ"
                                Dim codeAR8 As String = "_M 0xB0000000 0x00000000"


                                Dim codeAR2 As String = "_M 0xTIIIIIII 0xVVVVVVVV"

                                Dim codeAR3 As String = "_M 0xDC000000 0xIIIIIIII"
                                Dim codeAR4 As String = "_M 0xT0000000 0xVVVVVVVV"

                                Dim codeAR5 As String = "_M 0xD2000000 0x00000000"
                                Dim AddNum As String = "8800000"
                                Dim SubNum As String = "FFFFFFFF"
                                Dim OneNum As String = "00000001"
                                Dim p1 As String
                                Dim p2offne As String
                                Dim p3offne As String
                                Dim woffne As String

                                '最初のポインタ+0x8800000
                                p1 = Format(CInt(Val("&h" + read(2)(1)) + Val("&h" + AddNum)), "X7")
                                'ポインタのオフセット負
                                p2offne = Format(CInt(Val("&h" + SubNum) - Val("&h" + read(2)(4)) + Val("&h" + OneNum)), "X8")
                                '２番目のポインタオフセット負
                                p3offne = Format(CInt(Val("&h" + SubNum) - Val("&h" + read(1)(4)) + Val("&h" + OneNum)), "X8")
                                '書き込みオフセット負
                                woffne = Format(CInt(Val("&h" + SubNum) - Val("&h" + read(0)(4)) + Val("&h" + OneNum)), "X8")


                                'ベースの内容から必要なところを　値、各オフセット、基本アドレスに置き換え

                                codeAR0 = Replace(codeAR0, "AAAAAAA", p1.PadLeft(7, "0"))
                                codeAR1 = Replace(codeAR1, "AAAAAAA", p1.PadLeft(7, "0"))
                                codeAR6 = Replace(codeAR6, "JJJJJJJ", read(1)(4).PadLeft(7, "0"))
                                codeAR7 = Replace(codeAR7, "JJJJJJJJ", p3offne.PadLeft(8, "0"))
                                codeAR9 = Replace(codeAR9, "KKKKKKK", read(2)(4).PadLeft(7, "0"))
                                codeARA = Replace(codeARA, "KKKKKKKK", p2offne.PadLeft(8, "0"))

                                codeAR2 = Replace(codeAR2, "IIIIIII", read(0)(4).PadLeft(7, "0"))
                                codeAR2 = Replace(codeAR2, "VVVVVVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0")))

                                codeAR3 = Replace(codeAR3, "IIIIIIII", woffne.PadLeft(8, "0"))
                                codeAR4 = Replace(codeAR4, "VVVVVVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0")))

                                Select Case CODE_SET.ComboBox1.SelectedIndex

                                    Case 0 '8bit

                                        codeAR2 = Replace(codeAR2, "T", "2")
                                        codeAR4 = Replace(codeAR4, "T", "2")

                                    Case 1 '16bit

                                        codeAR2 = Replace(codeAR2, "T", "1")
                                        codeAR4 = Replace(codeAR4, "T", "1")

                                    Case Else '32bit (入力文字数制限は最大32bitまでなのでそのまま）


                                        codeAR2 = Replace(codeAR2, "T", "0")
                                        codeAR4 = Replace(codeAR4, "T", "0")

                                End Select

                                '正負場合わけ
                                If read(2)(3) = "+" Then
                                    Code_txt = Code_txt & "_C0 " & coden & vbCrLf
                                    Code_txt = Code_txt & codeAR0 & vbCrLf
                                    Code_txt = Code_txt & codeAR1 & vbCrLf
                                    Code_txt = Code_txt & codeAR9 & vbCrLf

                                Else
                                    Code_txt = Code_txt & "_C0 " & coden & vbCrLf
                                    Code_txt = Code_txt & codeAR0 & vbCrLf
                                    Code_txt = Code_txt & codeAR1 & vbCrLf
                                    Code_txt = Code_txt & codeARA & vbCrLf
                                    Code_txt = Code_txt & codeARB & vbCrLf

                                End If

                                If read(1)(3) = "+" Then

                                    Code_txt = Code_txt & codeAR6 & vbCrLf

                                Else

                                    Code_txt = Code_txt & codeAR7 & vbCrLf
                                    Code_txt = Code_txt & codeAR8 & vbCrLf

                                End If


                                If read(0)(3) = "+" Then

                                    Code_txt = Code_txt & codeAR2 & vbCrLf
                                    Code_txt = Code_txt & codeAR5 & vbCrLf

                                Else

                                    Code_txt = Code_txt & codeAR3 & vbCrLf
                                    Code_txt = Code_txt & codeAR4 & vbCrLf
                                    Code_txt = Code_txt & codeAR5 & vbCrLf

                                End If

                            Case Else
                                Code_txt = Code_txt & "doesnot output higher than triple DMA code" & vbCrLf
                        End Select


                        Code_txt = Code_txt & "" & vbCrLf
                        Code_txt = Code_txt & "CODEFREAK PointerCode" & vbCrLf
                        Code_txt = Code_txt & "-----------------------------" & vbCrLf

                        Select Case cnt

                            Case 1 '単体ポインタの場合

                                'コードのベース
                                Dim codeCF0 As String = "E0020000 1AAAAAAA"
                                Dim codeCF1 As String = "6AAAAAAA VVVVVVVV"
                                Dim codeCF2 As String = "000T0001 IIIIIIII"

                                'ベースの内容から必要なところを　値、各オフセット、基本アドレスに置き換え
                                codeCF0 = Replace(codeCF0, "AAAAAAA", read(0)(1).PadLeft(7, "0"))
                                codeCF1 = Replace(codeCF1, "AAAAAAA", read(0)(1).PadLeft(7, "0"))
                                codeCF2 = Replace(codeCF2, "IIIIIIII", read(0)(4).PadLeft(8, "0"))
                                codeCF1 = Replace(codeCF1, "VVVVVVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0")))

                                '単体の場合はタイプが特別なのでそれに合わせる
                                If read(0)(3) = "+" Then

                                    codeCF2 = Replace(codeCF2, "T", (bittype).ToString())

                                Else

                                    codeCF2 = Replace(codeCF2, "T", (bittype + 3).ToString())

                                End If

                                'ファイルに書き出し
                                Code_txt = Code_txt & coden & vbCrLf
                                Code_txt = Code_txt & codeCF0 & vbCrLf
                                Code_txt = Code_txt & codeCF1 & vbCrLf
                                Code_txt = Code_txt & codeCF2 & vbCrLf

                            Case Else
                                Code_txt = Code_txt & "CODEFREAK does not support multi pointercode,use ASM cdode" & vbCrLf

                        End Select

                        Code_txt = Code_txt & "" & vbCrLf
                        Code_txt = Code_txt & "FreeCheat PointerCode" & vbCrLf
                        Code_txt = Code_txt & "-----------------------------" & vbCrLf

                        Select Case cnt

                            Case 1 '単体ポインタの場合

                                'コードのベース
                                Dim code1 As String = "_L 0x6AAAAAAA 0xVVVVVVVV"
                                Dim code2 As String = "_L 0x000T0001 0xIIIIIIII"
                                'SCMは}で閉じる必要があるがビルド、ボタンでクラッシュするので除外orz
                                Dim codeFC As String = "$CODE $2 $(6AAAAAAA VVVVVVVV 000T0001 ZZZZZZZZ)"

                                'ベースの内容から必要なところを　値、各オフセット、基本アドレスに置き換え
                                code1 = Replace(code1, "AAAAAAA", read(0)(1).PadLeft(7, "0"))
                                code2 = Replace(code2, "IIIIIIII", read(0)(4).PadLeft(8, "0"))
                                codeFC = Replace(codeFC, "AAAAAAA", read(0)(1).PadLeft(7, "0"))
                                codeFC = Replace(codeFC, "ZZZZZZZZ", read(0)(4).PadLeft(8, "0"))
                                code1 = Replace(code1, "VVVVVVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0")))
                                codeFC = Replace(codeFC, "VVVVVVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0")))

                                '単体の場合はタイプが特別なのでそれに合わせる
                                If read(0)(3) = "+" Then

                                    code2 = Replace(code2, "T", (bittype).ToString())
                                    codeFC = Replace(codeFC, "T", (bittype).ToString())

                                Else

                                    code2 = Replace(code2, "T", (bittype + 3).ToString())
                                    codeFC = Replace(codeFC, "T", (bittype + 3).ToString())

                                End If

                                'ファイルに書き出し
                                Code_txt = Code_txt & "_C0 " & coden & vbCrLf
                                Code_txt = Code_txt & code1 & vbCrLf
                                Code_txt = Code_txt & code2 & vbCrLf
                                Code_txt = Code_txt & vbCrLf
                                Code_txt = Code_txt & "$" & coden & vbCrLf
                                Code_txt = Code_txt & codeFC & vbCrLf

                            Case 2 '2重ポインタの場合

                                'コードのベース
                                Dim code1 As String = "_L 0x6AAAAAAA 0x00000000"
                                Dim code2 As String = "_L 0x000S0000 0xIIIIIIII"
                                Dim code3 As String = "_L 0x60000000 0xVVVVVVVV"
                                Dim code4 As String = "_L 0x000T0001 0xZZZZZZZZ"
                                Dim codeFC As String = "$CODE $2 $(6AAAAAAA 00000000 000S0000 IIIIIIII)"
                                Dim codeFC1 As String = "$CODE $2 $(60000000 VVVVVVVV 000T0001 ZZZZZZZZ)"

                                'ベースの内容から必要なところを　値、各オフセット、基本アドレスに置き換え
                                code1 = Replace(code1, "AAAAAAA", read(1)(1).PadLeft(7, "0"))
                                code4 = Replace(code4, "ZZZZZZZZ", read(0)(4).PadLeft(8, "0"))
                                code2 = Replace(code2, "IIIIIIII", read(1)(4).PadLeft(8, "0"))
                                code3 = Replace(code3, "VVVVVVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0")))

                                codeFC = Replace(codeFC, "AAAAAAA", read(1)(1).PadLeft(7, "0"))
                                codeFC1 = Replace(codeFC1, "ZZZZZZZZ", read(0)(4).PadLeft(8, "0"))
                                codeFC = Replace(codeFC, "IIIIIIII", read(1)(4).PadLeft(8, "0"))
                                codeFC1 = Replace(codeFC1, "VVVVVVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0")))

                                'CWCコードの追跡回数、書き込むタイプ
                                If read(0)(3) = "+" Then

                                    code4 = Replace(code4, "T", (bittype).ToString())
                                    codeFC1 = Replace(codeFC1, "T", (bittype).ToString())

                                Else

                                    code4 = Replace(code4, "T", (bittype + 3).ToString())
                                    codeFC1 = Replace(codeFC1, "T", (bittype + 3).ToString())

                                End If

                                '各オフセットの符号でCWCのコード用にあわせる
                                If read(1)(3) = "+" Then

                                    code2 = Replace(code2, "S", "6")
                                    codeFC = Replace(codeFC, "S", "6")
                                Else

                                    code2 = Replace(code2, "S", "7")
                                    codeFC = Replace(codeFC, "S", "7")

                                End If

                                'ファイルに書き出し
                                Code_txt = Code_txt & "_C0 " & coden & vbCrLf
                                Code_txt = Code_txt & code1 & vbCrLf
                                Code_txt = Code_txt & code2 & vbCrLf
                                Code_txt = Code_txt & code3 & vbCrLf
                                Code_txt = Code_txt & code4 & vbCrLf
                                Code_txt = Code_txt & vbCrLf
                                Code_txt = Code_txt & "$" & coden & vbCrLf
                                Code_txt = Code_txt & codeFC & vbCrLf
                                Code_txt = Code_txt & codeFC1 & vbCrLf

                            Case 3 '3重ポインタの場合

                                'コードのベース
                                Dim code1 As String = "_L 0x6AAAAAAA 0x00000000"
                                Dim code2 As String = "_L 0x000R0000 0xIIIIIIII"
                                Dim code3 As String = "_L 0x60000000 0x00000000"
                                Dim code4 As String = "_L 0x000S0000 0xJJJJJJJJ"
                                Dim code5 As String = "_L 0x60000000 0xVVVVVVVV"
                                Dim code6 As String = "_L 0x000T0001 0xZZZZZZZZ"
                                Dim codeFC As String = "$CODE $2 $(6AAAAAAA 00000000 000R0000 IIIIIIII)"
                                Dim codeFC1 As String = "$CODE $2 $(60000000 00000000 000S0000 JJJJJJJJ)"
                                Dim codeFC2 As String = "$CODE $2 $(60000000 VVVVVVVV 000T0001 ZZZZZZZZ)"

                                'ベースの内容から必要なところを　値、各オフセット、基本アドレスに置き換え
                                code1 = Replace(code1, "AAAAAAA", read(2)(1).PadLeft(7, "0"))
                                code6 = Replace(code6, "ZZZZZZZZ", read(0)(4).PadLeft(8, "0"))
                                code2 = Replace(code2, "IIIIIIII", read(2)(4).PadLeft(8, "0"))
                                code4 = Replace(code4, "JJJJJJJJ", read(1)(4).PadLeft(8, "0"))
                                code5 = Replace(code5, "VVVVVVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0")))

                                'SCM形式
                                codeFC = Replace(codeFC, "AAAAAAA", read(2)(1).PadLeft(7, "0"))
                                codeFC2 = Replace(codeFC2, "ZZZZZZZZ", read(0)(4).PadLeft(8, "0"))
                                codeFC = Replace(codeFC, "IIIIIIII", read(2)(4).PadLeft(8, "0"))
                                codeFC1 = Replace(codeFC1, "JJJJJJJJ", read(1)(4).PadLeft(8, "0"))
                                codeFC2 = Replace(codeFC2, "VVVVVVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0")))

                                'CWCコードの追跡回数、書き込むタイプ
                                If read(0)(3) = "+" Then

                                    code6 = Replace(code6, "T", (bittype).ToString())
                                    codeFC2 = Replace(codeFC2, "T", (bittype).ToString())

                                Else

                                    code6 = Replace(code6, "T", (bittype + 3).ToString())
                                    codeFC2 = Replace(codeFC2, "T", (bittype + 3).ToString())

                                End If


                                '各オフセットの符号でCWCのコード用にあわせる
                                If read(2)(3) = "+" Then

                                    code2 = Replace(code2, "R", "6")
                                    codeFC = Replace(codeFC, "R", "6")

                                Else

                                    code2 = Replace(code2, "R", "7")
                                    codeFC = Replace(codeFC, "R", "7")

                                End If

                                If read(1)(3) = "+" Then

                                    code4 = Replace(code4, "S", "6")
                                    codeFC1 = Replace(codeFC1, "S", "6")

                                Else

                                    code4 = Replace(code4, "S", "7")
                                    codeFC1 = Replace(codeFC1, "S", "7")

                                End If

                                'ファイルに書き出し
                                Code_txt = Code_txt & "_C0 " & coden & vbCrLf
                                Code_txt = Code_txt & code1 & vbCrLf
                                Code_txt = Code_txt & code2 & vbCrLf
                                Code_txt = Code_txt & code3 & vbCrLf
                                Code_txt = Code_txt & code4 & vbCrLf
                                Code_txt = Code_txt & code5 & vbCrLf
                                Code_txt = Code_txt & code6 & vbCrLf
                                Code_txt = Code_txt & vbCrLf
                                Code_txt = Code_txt & "$" & coden & vbCrLf
                                Code_txt = Code_txt & codeFC & vbCrLf
                                Code_txt = Code_txt & codeFC1 & vbCrLf
                                Code_txt = Code_txt & codeFC2 & vbCrLf
                            Case Else

                                Code_txt = Code_txt & "doesnot output higher than triple DMA code" & vbCrLf

                        End Select

                        Code_txt = Code_txt & "" & vbCrLf
                        Code_txt = Code_txt & "NitePR PointerCode" & vbCrLf
                        Code_txt = Code_txt & "----------------------------" & vbCrLf

                        Select Case cnt

                            Case 1 '単体ポインタ+のみ

                                Dim codeNPR As String = "0xFFFFFFFF 0x0AAAAAAA"
                                Dim codeNPR1 As String = "0xIIIIIIII 0xVVVVWWZZ"

                                'ベースの内容から必要なところを　値、各オフセット、基本アドレスに置き換え
                                codeNPR = Replace(codeNPR, "AAAAAAA", read(0)(1).PadLeft(7, "0"))
                                codeNPR1 = Replace(codeNPR1, "IIIIIIII", read(0)(4).PadLeft(8, "0"))

                                Select Case CODE_SET.ComboBox1.SelectedIndex

                                    Case 0 '8bit

                                        codeNPR1 = Replace(codeNPR1, "ZZ", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(2, "0")))
                                        codeNPR1 = Replace(codeNPR1, "VVVVWW", "")

                                    Case 1 '16bit

                                        codeNPR1 = Replace(codeNPR1, "WWZZ", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(4, "0")))
                                        codeNPR1 = Replace(codeNPR1, "VVVV", "")

                                    Case Else '32bit (入力文字数制限は最大32bitまでなのでそのまま）

                                        codeNPR1 = Replace(codeNPR1, "VVVVWWZZ", (Replace(CODE_SET.value.Text, "0x", "").PadRight(8, "0")))

                                End Select

                                If read(0)(3) = "+" Then

                                    'ファイルに書き出し
                                    Code_txt = Code_txt & "#" & coden & vbCrLf
                                    Code_txt = Code_txt & codeNPR & vbCrLf
                                    Code_txt = Code_txt & codeNPR1 & vbCrLf

                                Else

                                    Code_txt = Code_txt & "NitePR does not support negative offset,use ASM code" & vbCrLf

                                End If

                            Case Else

                                Code_txt = Code_txt & "NitePR does not supoort multi pointercode,use ASM code" & vbCrLf
                        End Select

                        Code_txt = Code_txt & "" & vbCrLf
                        Code_txt = Code_txt & "PMETAN Pointercode" & vbCrLf
                        Code_txt = Code_txt & "-----------------------------" & vbCrLf

                        Select Case cnt

                            Case 1 '単体ポインタの場合

                                'コードのベース
                                Dim codeMPE1 As String = "_PIN 0x80000000 0x0AAAAAAA 0x00000000"
                                Dim codeMPE2 As String = "_PWR 0xBT000000 0xIIIIIIII 0xVVVVVVVV"

                                'ベースの内容から必要なところを　値、各オフセット、基本アドレスに置き換え
                                codeMPE1 = Replace(codeMPE1, "AAAAAAA", read(0)(1).PadLeft(7, "0"))
                                codeMPE2 = Replace(codeMPE2, "IIIIIIII", read(0)(4).PadLeft(8, "0"))
                                codeMPE2 = Replace(codeMPE2, "VVVVVVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0")))

                                Select Case CODE_SET.ComboBox1.SelectedIndex

                                    Case 0 '8bit

                                        codeMPE2 = Replace(codeMPE2, "B", "2")

                                    Case 1 '16bit

                                        codeMPE2 = Replace(codeMPE2, "B", "4")

                                    Case Else '32bit (入力文字数制限は最大32bitまでなのでそのまま）

                                        codeMPE2 = Replace(codeMPE2, "B", "8")

                                End Select

                                If read(0)(3) = "+" Then

                                    codeMPE2 = Replace(codeMPE2, "T", "0")

                                Else

                                    codeMPE2 = Replace(codeMPE2, "T", "1")

                                End If

                                'ファイルに書き出し
                                Code_txt = Code_txt & "_CN0 " & coden & vbCrLf
                                Code_txt = Code_txt & codeMPE1 & vbCrLf
                                Code_txt = Code_txt & codeMPE2 & vbCrLf

                            Case 2 '2重ポインタの場合

                                'コードのベース
                                Dim codeMPE1 As String = "_PIN 0x80000000 0x0AAAAAAA 0x00000000"
                                Dim codeMPE2 As String = "_PTR 0x8S000000 0xIIIIIIII 0x00000000"
                                Dim codeMPE3 As String = "_PWR 0xBT000000 0xZZZZZZZZ 0xVVVVVVVV"

                                'ベースの内容から必要なところを　値、各オフセット、基本アドレスに置き換え
                                codeMPE1 = Replace(codeMPE1, "AAAAAAA", read(1)(1).PadLeft(7, "0"))
                                codeMPE3 = Replace(codeMPE3, "ZZZZZZZZ", read(0)(4).PadLeft(8, "0"))
                                codeMPE2 = Replace(codeMPE2, "IIIIIIII", read(1)(4).PadLeft(8, "0"))
                                codeMPE3 = Replace(codeMPE3, "VVVVVVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0")))

                                'CWCコードの追跡回数、書き込むタイプ
                                Select Case CODE_SET.ComboBox1.SelectedIndex

                                    Case 0 '8bit

                                        codeMPE3 = Replace(codeMPE3, "B", "2")

                                    Case 1 '16bit

                                        codeMPE3 = Replace(codeMPE3, "B", "4")

                                    Case Else '32bit (入力文字数制限は最大32bitまでなのでそのまま）

                                        codeMPE3 = Replace(codeMPE3, "B", "8")

                                End Select


                                If read(0)(3) = "+" Then

                                    codeMPE3 = Replace(codeMPE3, "T", "0")

                                Else

                                    codeMPE3 = Replace(codeMPE3, "T", "1")

                                End If

                                '各オフセットの符号でCWCのコード用にあわせる
                                If read(1)(3) = "+" Then

                                    codeMPE2 = Replace(codeMPE2, "S", "0")
                                Else

                                    codeMPE2 = Replace(codeMPE2, "S", "1")

                                End If

                                'ファイルに書き出し
                                Code_txt = Code_txt & "_CN0 " & coden & vbCrLf
                                Code_txt = Code_txt & codeMPE1 & vbCrLf
                                Code_txt = Code_txt & codeMPE2 & vbCrLf
                                Code_txt = Code_txt & codeMPE3 & vbCrLf

                            Case 3 '3重ポインタの場合

                                'コードのベース
                                Dim codeMPE1 As String = "_PIN 0x80000000 0x0AAAAAAA 0x00000000"
                                Dim codeMPE2 As String = "_PTR 0x8Q000000 0xIIIIIIII 0x00000000"
                                Dim codeMPE3 As String = "_PTR 0x8S000000 0xJJJJJJJJ 0x00000000"
                                Dim codeMPE4 As String = "_PWR 0xBT000000 0xZZZZZZZZ 0xVVVVVVVV"

                                'ベースの内容から必要なところを　値、各オフセット、基本アドレスに置き換え
                                codeMPE1 = Replace(codeMPE1, "AAAAAAA", read(2)(1).PadLeft(7, "0"))
                                codeMPE4 = Replace(codeMPE4, "ZZZZZZZZ", read(0)(4).PadLeft(8, "0"))
                                codeMPE2 = Replace(codeMPE2, "IIIIIIII", read(2)(4).PadLeft(8, "0"))
                                codeMPE3 = Replace(codeMPE3, "JJJJJJJJ", read(1)(4).PadLeft(8, "0"))
                                codeMPE4 = Replace(codeMPE4, "VVVVVVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0")))

                                Select Case CODE_SET.ComboBox1.SelectedIndex

                                    Case 0 '8bit

                                        codeMPE4 = Replace(codeMPE4, "B", "2")

                                    Case 1 '16bit

                                        codeMPE4 = Replace(codeMPE4, "B", "4")

                                    Case Else '32bit (入力文字数制限は最大32bitまでなのでそのまま）

                                        codeMPE4 = Replace(codeMPE4, "B", "8")

                                End Select


                                '各オフセットの符号でCWCのコード用にあわせる
                                If read(2)(3) = "+" Then

                                    codeMPE2 = Replace(codeMPE2, "Q", "0")

                                Else

                                    codeMPE2 = Replace(codeMPE2, "Q", "1")

                                End If

                                If read(1)(3) = "+" Then

                                    codeMPE3 = Replace(codeMPE3, "S", "0")

                                Else

                                    codeMPE3 = Replace(codeMPE3, "S", "1")

                                End If

                                If read(0)(3) = "+" Then

                                    codeMPE4 = Replace(codeMPE4, "T", "0")

                                Else

                                    codeMPE4 = Replace(codeMPE4, "T", "1")

                                End If


                                'ファイルに書き出し
                                Code_txt = Code_txt & "_CN0 " & coden & vbCrLf
                                Code_txt = Code_txt & codeMPE1 & vbCrLf
                                Code_txt = Code_txt & codeMPE2 & vbCrLf
                                Code_txt = Code_txt & codeMPE3 & vbCrLf
                                Code_txt = Code_txt & codeMPE4 & vbCrLf

                            Case Else

                                Code_txt = Code_txt & "doesnot output higher than triple DMA code" & vbCrLf

                        End Select
                        Code_txt = Code_txt & "" & vbCrLf
                        Code_txt = Code_txt & "ASM POINTERCODE(MAX OFFSET within 0x7FFF)" & vbCrLf
                        Code_txt = Code_txt & "----------------------------" & vbCrLf

                        Select Case cnt

                            Case 1 '単体ポインタ+のみ

                                Dim ASM1 As String = "0x3C080AAA //lui t0,$0AAA"
                                Dim ASM2 As String = "0x3508AAAA //ori t0,t0,$AAAA"

                                Dim ASM3 As String = "0x3409VVVV //li t1,$VVVV"

                                Dim ASM8 As String = "0x3C09VVVV //lui t1,$VVVV"
                                Dim ASM9 As String = "0x3529VVVV //ori t1,$VVVV"

                                Dim ASM4 As String = "0x8D080000 //lw t0,$0000(t0)"
                                Dim ASM5 As String = "0x11000002 //beqz t0,$+3"
                                Dim ASM6 As String = "0x00000000 //nop"
                                Dim ASM7 As String = "0xAT09ZZZZ //sY t1,$ZZZZ(t0)"

                                Dim temp As String = ""

                                Dim AddNum As String = "08800000"
                                Dim SubNum As String = "FFFFFFFF"
                                Dim OneNum As String = "00000001"
                                Dim p1, woffset, woffne, luiaddress, oriaddress, luival, orival As String


                                '最初のポインタ+0x8800000
                                p1 = Format(CInt(Val("&h" + read(0)(1)) + Val("&h" + AddNum)), "X8")
                                luiaddress = p1.Remove(4, 4)
                                oriaddress = p1.Remove(0, 4)

                                'ポインタのオフセット負
                                woffne = Format(CInt(Val("&h" + SubNum) - Val("&h" + read(0)(4)) + Val("&h" + OneNum)), "X8")
                                woffne = woffne.Remove(0, 4)
                                woffset = Format(CInt(Val("&h" + read(0)(4))), "X8").Remove(0, 4)

                                'ベースの内容から必要なところを　値、各オフセット、基本アドレスに置き換え
                                ASM1 = Replace(ASM1, "0AAA", luiaddress)
                                ASM2 = Replace(ASM2, "AAAA", oriaddress)

                                tmp = ASM1.Substring(0, 10) & ASM2.Substring(0, 10)

                                Code_txt = Code_txt & ASM1 & vbCrLf
                                Code_txt = Code_txt & ASM2 & vbCrLf

                                Select Case CODE_SET.ComboBox1.SelectedIndex

                                    Case 0 '8bit

                                        ASM3 = Replace(ASM3, "VVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(4, "0")))
                                        ASM7 = Replace(ASM7, "T", "1")
                                        ASM7 = Replace(ASM7, "Y", "b")
                                        Code_txt = Code_txt & ASM3 & vbCrLf
                                        tmp &= ASM3.Substring(0, 10)
                                    Case 1 '16bit
                                        ASM3 = Replace(ASM3, "VVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(4, "0")))
                                        ASM7 = Replace(ASM7, "T", "5")
                                        ASM7 = Replace(ASM7, "Y", "h")
                                        Code_txt = Code_txt & ASM3 & vbCrLf

                                        tmp &= ASM3.Substring(0, 10)
                                    Case Else '32bit (入力文字数制限は最大32bitまでなのでそのまま）
                                        luival = Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0").Remove(4, 4)
                                        orival = Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0").Remove(0, 4)
                                        ASM8 = Replace(ASM8, "VVVV", luival)
                                        ASM9 = Replace(ASM9, "VVVV", orival)
                                        ASM7 = Replace(ASM7, "T", "D")
                                        ASM7 = Replace(ASM7, "Y", "w")

                                        Code_txt = Code_txt & ASM8 & vbCrLf
                                        Code_txt = Code_txt & ASM9 & vbCrLf
                                        tmp &= ASM8.Substring(0, 10)
                                        tmp &= ASM9.Substring(0, 10)
                                End Select

                                Code_txt = Code_txt & ASM4 & vbCrLf
                                Code_txt = Code_txt & ASM5 & vbCrLf
                                Code_txt = Code_txt & ASM6 & vbCrLf

                                tmp &= ASM4.Substring(0, 10)
                                tmp &= ASM5.Substring(0, 10)
                                tmp &= ASM6.Substring(0, 10)

                                If read(0)(3) = "+" Then

                                    ASM7 = Replace(ASM7, "ZZZZ", woffset)

                                Else

                                    ASM7 = Replace(ASM7, "ZZZZ", woffne)
                                End If

                                Code_txt = Code_txt & ASM7 & vbCrLf
                                tmp &= ASM7.Substring(0, 10)


                                Code_txt = Code_txt & "" & vbCrLf
                                Code_txt = Code_txt & "CMFUSION ASM POINTERCODE(MAX OFFSET within 0x7FFF)" & vbCrLf
                                Code_txt = Code_txt & "----------------------------" & vbCrLf
                                Code_txt = Code_txt & "_C0 " & coden & vbCrLf
                                Dim cmfsub As String = "_L 0xF00000NN 0x00000000"
                                Dim nn As Integer = tmp.Length \ 20
                                If tmp.Length Mod 20 > 0 Then
                                    nn += 1
                                End If
                                cmfsub = cmfsub.Replace("NN", Convert.ToString(nn, 16).PadLeft(2, "0"))
                                Code_txt = Code_txt & cmfsub
                                For i = 0 To tmp.Length - 1
                                    If i Mod 20 = 0 Then
                                        Code_txt &= vbCrLf & "_L "
                                    ElseIf i Mod 10 = 0 Then
                                        Code_txt &= " "
                                    End If
                                    Code_txt &= tmp(i)
                                Next

                                If tmp.Length Mod 20 > 0 Then
                                    Code_txt &= " 0x00000000"
                                End If
                                Code_txt &= vbCrLf

                                Code_txt = Code_txt & "" & vbCrLf
                                Code_txt = Code_txt & "TEMPAR ASM POINTERCODE(MAX OFFSET within 0x7FFF)" & vbCrLf
                                Code_txt = Code_txt & "----------------------------" & vbCrLf
                                Code_txt = Code_txt & "_C0 " & coden & vbCrLf
                                Dim tmpsub As String = "_N 0xC2000000 0x000000NN"
                                nn = tmp.Length \ 10
                                nn = tmp.Length - nn * 2
                                tmpsub = tmpsub.Replace("NN", Convert.ToString(nn, 16))
                                Code_txt = Code_txt & tmpsub
                                For i = 0 To tmp.Length - 1
                                    If i Mod 20 = 0 Then
                                        Code_txt &= vbCrLf & "_N "
                                    ElseIf i Mod 10 = 0 Then
                                        Code_txt &= " "
                                    End If
                                    Code_txt &= tmp(i)
                                Next

                                If tmp.Length Mod 20 > 0 Then
                                    Code_txt &= " 0x00000000"
                                End If
                                Code_txt &= vbCrLf

                            Case 2

                                Dim ASM1 As String = "0x3C080AAA //lui t0,$0AAA"
                                Dim ASM2 As String = "0x3508AAAA //ori t0,t0,$AAAA"

                                Dim ASM3 As String = "0x3409VVVV //li t1,$VVVV"

                                Dim ASM8 As String = "0x3C09VVVV //lui t1,$VVVV"
                                Dim ASM9 As String = "0x3529VVVV //ori t1,$VVVV"

                                Dim ASM4 As String = "0x8D080000 //lw t0,$0000(t0)"
                                Dim ASM5 As String = "0x11000004 //beqz t0,$+5"
                                Dim ASM6 As String = "0x00000000 //nop"

                                Dim ASMA As String = "0x8D08PPPP //lw t0,$PPPP(t0)"

                                Dim ASM7 As String = "0xAT09ZZZZ //sY t1,$ZZZZ(t0)"

                                Dim temp As String = ""

                                Dim AddNum As String = "08800000"
                                Dim SubNum As String = "FFFFFFFF"
                                Dim OneNum As String = "00000001"
                                Dim p1, woffset, woffne, poffset, poffne, luiaddress, oriaddress, luival, orival As String


                                '最初のポインタ+0x8800000
                                p1 = Format(CInt(Val("&h" + read(1)(1)) + Val("&h" + AddNum)), "X8")
                                luiaddress = p1.Remove(4, 4)
                                oriaddress = p1.Remove(0, 4)

                                'ポインタのオフセット正負
                                poffne = Format(CInt(Val("&h" + SubNum) - Val("&h" + read(1)(4)) + Val("&h" + OneNum)), "X8")
                                poffne = poffne.Remove(0, 4)
                                poffset = Format(CInt(Val("&h" + read(1)(4))), "X8").Remove(0, 4)
                                woffne = Format(CInt(Val("&h" + SubNum) - Val("&h" + read(0)(4)) + Val("&h" + OneNum)), "X8")
                                woffne = woffne.Remove(0, 4)
                                woffset = Format(CInt(Val("&h" + read(0)(4))), "X8").Remove(0, 4)

                                'ベースの内容から必要なところを　値、各オフセット、基本アドレスに置き換え
                                ASM1 = Replace(ASM1, "0AAA", luiaddress)
                                ASM2 = Replace(ASM2, "AAAA", oriaddress)
                                tmp = ASM1.Substring(0, 10) & ASM2.Substring(0, 10)


                                Code_txt = Code_txt & ASM1 & vbCrLf
                                Code_txt = Code_txt & ASM2 & vbCrLf

                                Select Case CODE_SET.ComboBox1.SelectedIndex

                                    Case 0 '8bit

                                        ASM3 = Replace(ASM3, "VVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(4, "0")))
                                        ASM7 = Replace(ASM7, "T", "1")
                                        ASM7 = Replace(ASM7, "Y", "b")
                                        Code_txt = Code_txt & ASM3 & vbCrLf
                                        tmp &= ASM3.Substring(0, 10)
                                    Case 1 '16bit
                                        ASM3 = Replace(ASM3, "VVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(4, "0")))
                                        ASM7 = Replace(ASM7, "T", "5")
                                        ASM7 = Replace(ASM7, "Y", "h")
                                        Code_txt = Code_txt & ASM3 & vbCrLf
                                        tmp &= ASM3.Substring(0, 10)

                                    Case Else '32bit (入力文字数制限は最大32bitまでなのでそのまま）
                                        luival = Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0").Remove(4, 4)
                                        orival = Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0").Remove(0, 4)
                                        ASM8 = Replace(ASM8, "VVVV", luival)
                                        ASM9 = Replace(ASM9, "VVVV", orival)
                                        ASM7 = Replace(ASM7, "T", "D")
                                        ASM7 = Replace(ASM7, "Y", "w")
                                        tmp &= ASM8.Substring(0, 10)
                                        tmp &= ASM9.Substring(0, 10)

                                        Code_txt = Code_txt & ASM8 & vbCrLf
                                        Code_txt = Code_txt & ASM9 & vbCrLf
                                End Select

                                Code_txt = Code_txt & ASM4 & vbCrLf
                                Code_txt = Code_txt & ASM5 & vbCrLf
                                Code_txt = Code_txt & ASM6 & vbCrLf
                                tmp &= ASM4.Substring(0, 10)
                                tmp &= ASM5.Substring(0, 10)
                                tmp &= ASM6.Substring(0, 10)

                                If read(1)(3) = "+" Then

                                    ASMA = Replace(ASMA, "PPPP", poffset)

                                Else

                                    ASMA = Replace(ASMA, "PPPP", poffne)
                                End If

                                If read(0)(3) = "+" Then

                                    ASM7 = Replace(ASM7, "ZZZZ", woffset)

                                Else

                                    ASM7 = Replace(ASM7, "ZZZZ", woffne)

                                End If

                                Code_txt = Code_txt & ASMA & vbCrLf
                                Code_txt = Code_txt & ASM6 & vbCrLf
                                Code_txt = Code_txt & ASM7 & vbCrLf

                                tmp &= ASMA.Substring(0, 10)
                                tmp &= ASM6.Substring(0, 10)
                                tmp &= ASM7.Substring(0, 10)


                                Code_txt = Code_txt & "" & vbCrLf
                                Code_txt = Code_txt & "CMFUSION ASM POINTERCODE(MAX OFFSET within 0x7FFF)" & vbCrLf
                                Code_txt = Code_txt & "----------------------------" & vbCrLf
                                Code_txt = Code_txt & "_C0 " & coden & vbCrLf
                                Dim cmfsub As String = "_L 0xF00000NN 0x00000000"
                                Dim nn As Integer = tmp.Length \ 20
                                If tmp.Length Mod 20 > 0 Then
                                    nn += 1
                                End If
                                cmfsub = cmfsub.Replace("NN", Convert.ToString(nn, 16).PadLeft(2, "0"))
                                Code_txt = Code_txt & cmfsub
                                For i = 0 To tmp.Length - 1
                                    If i Mod 20 = 0 Then
                                        Code_txt &= vbCrLf & "_L "
                                    ElseIf i Mod 10 = 0 Then
                                        Code_txt &= " "
                                    End If
                                    Code_txt &= tmp(i)
                                Next

                                If tmp.Length Mod 20 > 0 Then
                                    Code_txt &= " 0x00000000"
                                End If
                                Code_txt &= vbCrLf

                                Code_txt = Code_txt & "" & vbCrLf
                                Code_txt = Code_txt & "TEMPAR ASM POINTERCODE(MAX OFFSET within 0x7FFF)" & vbCrLf
                                Code_txt = Code_txt & "----------------------------" & vbCrLf
                                Code_txt = Code_txt & "_C0 " & coden & vbCrLf
                                Dim tmpsub As String = "_N 0xC2000000 0x000000NN"
                                nn = tmp.Length \ 10
                                nn = tmp.Length - nn * 2
                                tmpsub = tmpsub.Replace("NN", Convert.ToString(nn, 16))
                                Code_txt = Code_txt & tmpsub
                                For i = 0 To tmp.Length - 1
                                    If i Mod 20 = 0 Then
                                        Code_txt &= vbCrLf & "_N "
                                    ElseIf i Mod 10 = 0 Then
                                        Code_txt &= " "
                                    End If
                                    Code_txt &= tmp(i)
                                Next

                                If tmp.Length Mod 20 > 0 Then
                                    Code_txt &= " 0x00000000"
                                End If
                                Code_txt &= vbCrLf

                            Case 3

                                Dim ASM1 As String = "0x3C080AAA //lui t0,$0AAA"
                                Dim ASM2 As String = "0x3508AAAA //ori t0,t0,$AAAA"

                                Dim ASM3 As String = "0x3409VVVV //li t1,$VVVV"

                                Dim ASM8 As String = "0x3C09VVVV //lui t1,$VVVV"
                                Dim ASM9 As String = "0x3529VVVV //ori t1,$VVVV"

                                Dim ASM4 As String = "0x8D080000 //lw t0,$0000(t0)"
                                Dim ASM5 As String = "0x11000006 //beqz t0,$+7"
                                Dim ASM6 As String = "0x00000000 //nop"

                                Dim ASMB As String = "0x8D08QQQQ //lw t0,$QQQQ(t0)"
                                Dim ASMA As String = "0x8D08PPPP //lw t0,$PPPP(t0)"

                                Dim ASM7 As String = "0xAT09ZZZZ //sY t1,$ZZZZ(t0)"
                                Dim temp As String = ""

                                Dim AddNum As String = "08800000"
                                Dim SubNum As String = "FFFFFFFF"
                                Dim OneNum As String = "00000001"
                                Dim p1, woffset, woffne, poffset, poffne, p1offset, p1offne, luiaddress, oriaddress, luival, orival As String

                                '最初のポインタ+0x8800000
                                p1 = Format(CInt(Val("&h" + read(2)(1)) + Val("&h" + AddNum)), "X8")
                                luiaddress = p1.Remove(4, 4)
                                oriaddress = p1.Remove(0, 4)

                                'ポインタのオフセット正負
                                p1offne = Format(CInt(Val("&h" + SubNum) - Val("&h" + read(2)(4)) + Val("&h" + OneNum)), "X8")
                                p1offne = p1offne.Remove(0, 4)
                                p1offset = Format(CInt(Val("&h" + read(2)(4))), "X8").Remove(0, 4)
                                poffne = Format(CInt(Val("&h" + SubNum) - Val("&h" + read(1)(4)) + Val("&h" + OneNum)), "X8")
                                poffne = poffne.Remove(0, 4)
                                poffset = Format(CInt(Val("&h" + read(1)(4))), "X8").Remove(0, 4)
                                woffne = Format(CInt(Val("&h" + SubNum) - Val("&h" + read(0)(4)) + Val("&h" + OneNum)), "X8")
                                woffne = woffne.Remove(0, 4)
                                woffset = Format(CInt(Val("&h" + read(0)(4))), "X8").Remove(0, 4)

                                'ベースの内容から必要なところを　値、各オフセット、基本アドレスに置き換え
                                ASM1 = Replace(ASM1, "0AAA", luiaddress)
                                ASM2 = Replace(ASM2, "AAAA", oriaddress)
                                tmp = ASM1.Substring(0, 10) & ASM2.Substring(0, 10)

                                Code_txt = Code_txt & ASM1 & vbCrLf
                                Code_txt = Code_txt & ASM2 & vbCrLf

                                Select Case CODE_SET.ComboBox1.SelectedIndex

                                    Case 0 '8bit

                                        ASM3 = Replace(ASM3, "VVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(4, "0")))
                                        ASM7 = Replace(ASM7, "T", "1")
                                        ASM7 = Replace(ASM7, "Y", "b")
                                        Code_txt = Code_txt & ASM3 & vbCrLf
                                        tmp &= ASM3.Substring(0, 10)
                                    Case 1 '16bit
                                        ASM3 = Replace(ASM3, "VVVV", (Replace(CODE_SET.value.Text, "0x", "").PadLeft(4, "0")))
                                        ASM7 = Replace(ASM7, "T", "5")
                                        ASM7 = Replace(ASM7, "Y", "h")
                                        Code_txt = Code_txt & ASM3 & vbCrLf
                                        tmp &= ASM3.Substring(0, 10)

                                    Case Else '32bit (入力文字数制限は最大32bitまでなのでそのまま）
                                        luival = Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0").Remove(4, 4)
                                        orival = Replace(CODE_SET.value.Text, "0x", "").PadLeft(8, "0").Remove(0, 4)
                                        ASM8 = Replace(ASM8, "VVVV", luival)
                                        ASM9 = Replace(ASM9, "VVVV", orival)
                                        ASM7 = Replace(ASM7, "T", "D")
                                        ASM7 = Replace(ASM7, "Y", "w")

                                        Code_txt = Code_txt & ASM8 & vbCrLf
                                        Code_txt = Code_txt & ASM9 & vbCrLf
                                        tmp &= ASM8.Substring(0, 10)
                                        tmp &= ASM9.Substring(0, 10)
                                End Select

                                Code_txt = Code_txt & ASM4 & vbCrLf
                                Code_txt = Code_txt & ASM5 & vbCrLf
                                Code_txt = Code_txt & ASM6 & vbCrLf

                                tmp &= ASM4.Substring(0, 10)
                                tmp &= ASM5.Substring(0, 10)
                                tmp &= ASM6.Substring(0, 10)

                                If read(2)(3) = "+" Then

                                    ASMB = Replace(ASMB, "QQQQ", p1offset)

                                Else

                                    ASMB = Replace(ASMB, "QQQQ", p1offne)
                                End If

                                If read(1)(3) = "+" Then

                                    ASMA = Replace(ASMA, "PPPP", poffset)

                                Else

                                    ASMA = Replace(ASMA, "PPPP", poffne)
                                End If

                                If read(0)(3) = "+" Then

                                    ASM7 = Replace(ASM7, "ZZZZ", woffset)

                                Else

                                    ASM7 = Replace(ASM7, "ZZZZ", woffne)
                                End If

                                Code_txt = Code_txt & ASMB & vbCrLf
                                Code_txt = Code_txt & ASM6 & vbCrLf
                                Code_txt = Code_txt & ASMA & vbCrLf
                                Code_txt = Code_txt & ASM6 & vbCrLf
                                Code_txt = Code_txt & ASM7 & vbCrLf
                                tmp &= ASMB.Substring(0, 10)
                                tmp &= ASM6.Substring(0, 10)
                                tmp &= ASMA.Substring(0, 10)
                                tmp &= ASM6.Substring(0, 10)
                                tmp &= ASM7.Substring(0, 10)

                                Code_txt = Code_txt & "" & vbCrLf
                                Code_txt = Code_txt & "CMFUSION ASM POINTERCODE(MAX OFFSET within 0x7FFF)" & vbCrLf
                                Code_txt = Code_txt & "----------------------------" & vbCrLf
                                Code_txt = Code_txt & "_C0 " & coden & vbCrLf
                                Dim cmfsub As String = "_L 0xF00000NN 0x00000000"
                                Dim nn As Integer = tmp.Length \ 20
                                If tmp.Length Mod 20 > 0 Then
                                    nn += 1
                                End If
                                cmfsub = cmfsub.Replace("NN", Convert.ToString(nn, 16).PadLeft(2, "0"))
                                Code_txt = Code_txt & cmfsub
                                For i = 0 To tmp.Length - 1
                                    If i Mod 20 = 0 Then
                                        Code_txt &= vbCrLf & "_L "
                                    ElseIf i Mod 10 = 0 Then
                                        Code_txt &= " "
                                    End If
                                    Code_txt &= tmp(i)
                                Next

                                If tmp.Length Mod 20 > 0 Then
                                    Code_txt &= " 0x00000000"
                                End If
                                Code_txt &= vbCrLf

                                Code_txt = Code_txt & "" & vbCrLf
                                Code_txt = Code_txt & "TEMPAR ASM POINTERCODE(MAX OFFSET within 0x7FFF)" & vbCrLf
                                Code_txt = Code_txt & "----------------------------" & vbCrLf
                                Code_txt = Code_txt & "_C0 " & coden & vbCrLf
                                Dim tmpsub As String = "_N 0xC2000000 0x000000NN"
                                nn = tmp.Length \ 10
                                nn = tmp.Length - nn * 2
                                tmpsub = tmpsub.Replace("NN", Convert.ToString(nn, 16))
                                Code_txt = Code_txt & tmpsub
                                For i = 0 To tmp.Length - 1
                                    If i Mod 20 = 0 Then
                                        Code_txt &= vbCrLf & "_N "
                                    ElseIf i Mod 10 = 0 Then
                                        Code_txt &= " "
                                    End If
                                    Code_txt &= tmp(i)
                                Next

                                If tmp.Length Mod 20 > 0 Then
                                    Code_txt &= " 0x00000000"
                                End If
                                Code_txt &= vbCrLf

                            Case Else

                                Code_txt = Code_txt & "doesnot output higher than triple DMA code" & vbCrLf
                        End Select


                        Code_txt &= "----------------------------" & vbCrLf
                        Code_txt &= vbCrLf
                        Code_txt &= "※please use the lastest version plugins." & vbCrLf
                        Code_txt &= "if plugins version is too old,it maybe not support pointercode." & vbCrLf
                        Code_txt &= vbCrLf
                        Code_txt &= vbCrLf
                        Code_txt &= vbCrLf


                        '各ファイル　初期値まで共通する値（パス）を作る
                        tmp = Space(j + 1)
                        tmp = Replace(tmp, " ", "[ ")

                        For i = j To 0 Step -1

                            Select Case i

                                Case j
                                    tmp &= "0x" & read(i)(1) & " ] " & read(i)(3) & "0x" & read(i)(4)

                                Case Else
                                    tmp &= " ] " & read(i)(3) & "0x" & read(i)(4)

                            End Select

                        Next

                        Code_txt &= "■　info　■" & vbCrLf
                        Code_txt &= vbCrLf
                        Code_txt &= "・Search results (analysis of an initial value of each file)" & vbCrLf



                        'ファイルに関する設定情報を読み込み、書き出し
                        For i = 1 To slt_cnt

                            data = Split(File.Read_Data(save_folder & "\" & i & ".file", 0), ",")
                            Code_txt &= "file" & i & " = 0x" & data(0).PadLeft(7, "0") & " ( " & data(1) & " )" & vbCrLf

                        Next

                        Code_txt &= vbCrLf

                        Code_txt &= "・baseaddress" & vbCrLf
                        Code_txt &= "0x" & read(j)(1) & vbCrLf

                        Code_txt &= vbCrLf

                        Code_txt &= "・Common address offset to the initial value of each file" & vbCrLf
                        Code_txt &= tmp & vbCrLf
                        Code_txt &= vbCrLf
                        Code_txt &= vbCrLf
                        Code_txt &= "[] = []Adjust the value of the offset in the address (32bit) from the value-0x8800000 minus (following) is the actual address of reference" & vbCrLf
                        Code_txt &= "+/- 0x?? = offset value" & vbCrLf
                        Code_txt &= vbCrLf
                        Code_txt &= "※If we follow the address against the dump file is always base address ([] is the most profound position) to confirm that the " & vbCrLf
                        File.Write(dir.Text & "\" & "Code" & "\" & c_name, Code_txt, False)


                        MsgBox("Code References folder create a folder" & vbCrLf & c_name & " saved as", , "Creating complete file code")


                        If CODE_SET.CheckBox1.Checked Then
                            'Shell(Environ("WINDIR") & "\notepad.exe " & dir.Text & "\" & "Code" & "\" & c_name, vbNormalFocus)
                            File.View(dir.Text & "\" & "Code" & "\" & c_name)
                        End If


                End Select

            Case Else '解析結果リストで選択されたのが基本アドレスの候補じゃない

                MsgBox("You do not choose to address the common final" & vbCrLf & "cannot make pointercode" & vbCrLf & " Please ★ analysis from the results list, press the button and select the code to include the Stars ", , " error ")
        End Select

    End Sub

#End Region


#Region "初期化ボタンを押した時の処理"

    Private Sub Reset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Reset.Click

        '  Etc.Beep_Sound(My.Settings.SOUND, 1)
        SystemSounds.Beep.Play()

        '選択肢で分岐　押されたボタンで処理分岐
        Select Case MsgBox("clear setting？", vbOKCancel Or vbQuestion, "clear")

            Case vbOK 'OKボタンが押された

                flag = 0 '初期化完了まで不要なイベント発生回避の為、フラグをオフに

                '参照先フォルダ変更に備えて現在のパスにある一時フォルダを削除

                Select Case CheckBox2.Checked

                    Case True
                        File.Del_All(save_folder, "*.cfg")
                        File.Copy(dir.Text & "\try.cfg", save_folder & "\try.cfg")

                        'NumericUpDown1.Value = slt_cnt
                        slt = NumericUpDown1.Value

                        If slt_cnt >= 2 Then
                            start.Enabled = True
                        End If

                        'Reset.Enabled = True

                    Case False
                        File.Del_Dir(save_folder)

                        NumericUpDown1.Value = 1
                        file1.Text = "NONE"
                        fl_adr.Text = Def_adr

                        slt_cnt = 1
                        slt = 1

                        'ツールがあるフォルダに一時フォルダを作成
                        File.Del_Dir(Application.StartupPath() & "\tmp")
                        My.Computer.FileSystem.CreateDirectory(Application.StartupPath() & "\tmp")

                        save_folder = Application.StartupPath() & "\tmp"
                        dir.Text = Application.StartupPath()

                        start.Enabled = False
                        'Reset.Enabled = False

                End Select

                'コントロール上の全ての項目を起動時に戻す

                '対象　関係
                offcet_min.Enabled = True
                offcet_max.Enabled = True
                Offcet_Type.Enabled = True
                offcet_min.Text = "0x00000000"
                offcet_max.Text = "0x0000FFFF"
                Offcet_Type.SelectedIndex = 0

                '設定　関係
                NumericUpDown1.ReadOnly = False
                dmp_open.Enabled = True
                file1.ReadOnly = False
                fl_adr.ReadOnly = False

                'オプション　関係
                CheckBox1.Checked = True
                CheckBox2.Checked = False
                CheckBox3.Checked = False
                TextBox1.Enabled = False
                TextBox1.Text = "0x100000"

                GroupBox11.Text = "status"

                Status.Text = "wait"

                '解析結果リストの内容削除、コントロールを無効
                ComboBox1.Items.Clear()
                ComboBox1.Enabled = False
                Info.Enabled = False

                'ボタン　関係
                abort.Enabled = False

                Restore.Enabled = False

                Restart.Enabled = True
                saved.Enabled = False

                code.Enabled = False
                PrevC.Enabled = False
                NextC.Enabled = False


                '実行履歴不要項目削除
                Do While 1 < ListBox1.Items.Count
                    ListBox1.Items.RemoveAt(ListBox1.Items.Count - 1)
                Loop


                If My.Computer.Clipboard.ContainsText Then
                    clip_tmp = My.Computer.Clipboard.GetText
                End If

                Timer1.Enabled = True

                sc_abort = 0
                cnt = 0

                sc_type = True
                flag2 = 0
                code_s = 0
                code_f = 0

                flag = 1 '初期化完了したので、フラグをオンに

        End Select

    End Sub

#End Region


#Region "再開ボタンを押した時の処理"

    Private Sub Restart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Restart.Click

        'フォルダの参照ダイアログボックスを表示
        Dim folderBrowserDialog1 As New FolderBrowserDialog

        folderBrowserDialog1.SelectedPath = Application.StartupPath()

        'ダイアログボックスに[新しいフォルダの作成]ボタンを表示しない場合は False 
        folderBrowserDialog1.ShowNewFolderButton = False

        folderBrowserDialog1.Description = "You can resume the work suspended from the previous" & vbCrLf & "Please specify the folder that contains files"

        If folderBrowserDialog1.ShowDialog() = DialogResult.OK Then


            '再開に必要なファイルがあるかどうか？
            If My.Computer.FileSystem.FileExists(folderBrowserDialog1.SelectedPath & "\set" & Replace(Application.ProductVersion, ".", "") & ".cfg") = True And _
            My.Computer.FileSystem.FileExists(folderBrowserDialog1.SelectedPath & "\listbox.cfg") = True Then


                Dim log_cnt As Integer = 0
                Dim file_cnt As Integer = 0
                Dim i As Integer

                For i = 1 To Split(File.Read_Data(folderBrowserDialog1.SelectedPath & "\set" & Replace(Application.ProductVersion, ".", "") & ".cfg", 3), ",")(0)

                    If My.Computer.FileSystem.FileExists(folderBrowserDialog1.SelectedPath & "\" & i & ".cfg") = True Then
                        log_cnt += 1
                    End If

                Next


                For i = 1 To Split(File.Read_Data(folderBrowserDialog1.SelectedPath & "\set" & Replace(Application.ProductVersion, ".", "") & ".cfg", 4), ",")(0)

                    If My.Computer.FileSystem.FileExists(folderBrowserDialog1.SelectedPath & "\" & i & ".file") Then

                        If My.Computer.FileSystem.FileExists(Split(File.Read_Data(folderBrowserDialog1.SelectedPath & "\" & i & ".file", 0), ",")(1)) = True Then
                            file_cnt += 1
                        End If

                    End If

                Next


                If Split(File.Read_Data(folderBrowserDialog1.SelectedPath & "\set" & Replace(Application.ProductVersion, ".", "") & ".cfg", 3), ",")(0) = log_cnt And _
                   Split(File.Read_Data(folderBrowserDialog1.SelectedPath & "\set" & Replace(Application.ProductVersion, ".", "") & ".cfg", 4), ",")(0) = file_cnt Then



                    '参照先フォルダが変わるのでツールがあるフォルダの一時フォルダ(tmp)を削除
                    If save_folder = Application.StartupPath() & "\tmp" Then
                        File.Del_Dir(Application.StartupPath() & "\tmp")
                    Else
                        File.Del_Dir(save_folder)
                    End If

                    '参照先フォルダ　変更先に一時フォルダ(tmp)を作成　(既にあれば削除してから)
                    File.Del_Dir(folderBrowserDialog1.SelectedPath & "\tmp")
                    My.Computer.FileSystem.CreateDirectory(folderBrowserDialog1.SelectedPath & "\tmp")

                    '再開ファイル指定フォルダ内のファイル全て一時フォルダにコピー
                    File.Copy_All(folderBrowserDialog1.SelectedPath, folderBrowserDialog1.SelectedPath & "\tmp\", "*.file")
                    File.Copy_All(folderBrowserDialog1.SelectedPath, folderBrowserDialog1.SelectedPath & "\tmp\", "*.cfg")

                    '一時ファイル保存先、参照先フォルダをダイアログで指定したフォルダに更新
                    dir.Text = folderBrowserDialog1.SelectedPath
                    save_folder = folderBrowserDialog1.SelectedPath & "\tmp"

                    i = 1

                    'set.cfgから前回の設定を読み込む
                    Using sr As New System.IO.StreamReader(save_folder & "\set" & Replace(Application.ProductVersion, ".", "") & ".cfg", enc)

                        Do Until sr.Peek() = -1

                            Select Case i

                                Case 1 'オフセット最小値
                                    offcet_min.Text = sr.ReadLine()

                                Case 2 'オフセット最大値
                                    offcet_max.Text = sr.ReadLine()

                                Case 3 'オフセット符号
                                    Offcet_Type.SelectedIndex = sr.ReadLine()

                                Case 4 'ループカウンタ
                                    cnt = sr.ReadLine()

                                Case 5 'ファイル数カウンタ
                                    slt_cnt = sr.ReadLine()

                                Case 6 'オプション　アクティブ設定
                                    CheckBox1.Checked = sr.ReadLine()

                                Case 7 'オプション　リスト復元設定
                                    CheckBox2.Checked = sr.ReadLine()

                                Case 8 'オプション　リスト復元設定
                                    CheckBox3.Checked = sr.ReadLine()

                                Case 9
                                    TextBox1.Text = sr.ReadLine()

                                Case 10 '再開時直後の検索タイプ　False = リストから選択なし   True = リストから選択する
                                    sc_type = sr.ReadLine()

                                Case 11 '再開直後の検索タイプが False の場合に自動設定されるアドレス読み込みよう Index値
                                    index = sr.ReadLine()

                            End Select

                            i += 1

                        Loop

                    End Using

                    'listbox.cfg の内容を実行履歴に読み込む
                    ListBox1.Items.Clear()

                    Using sr3 As New System.IO.StreamReader(save_folder & "\listbox.cfg", enc)

                        Do Until sr3.Peek() = -1
                            ListBox1.Items.Add(sr3.ReadLine())
                        Loop

                    End Using

                    ListBox1.SelectedIndex = ListBox1.Items.Count - 1

                    'combobox.cfgの内容を解析結果リストに読み込む
                    Select Case sc_type '解析開始タイプで処理分岐

                        Case True '解析結果リスト復元、リストから解析

                            Make_ComboBox(cnt) '解析結果リストを復元

                            Select Case ComboBox1.Items.Count '解析結果リストの有効アイテム数で処理分岐

                                Case 0 'アイテムが存在しない
                                    start.Enabled = False '開始ボタンを無効

                                Case Else 'アイテムが存在する
                                    start.Enabled = True '開始ボタンを有効

                            End Select

                        Case False '解析結果リスト非復元、そのまま開始

                            'リストをクリア
                            ComboBox1.Items.Clear()
                            ComboBox1.Enabled = False

                            '開始ボタンを有効
                            start.Enabled = True

                    End Select


                    'その他　以後の処理に必要・不要な各コントロールを設定
                    abort.Enabled = False
                    'Restore.Enabled = True
                    'Reset.Enabled = True
                    saved.Enabled = True

                    NumericUpDown1.ReadOnly = Enabled
                    NumericUpDown1.Value = 2 '各データを読み込むためわざと値を2回変更
                    NumericUpDown1.Value = 1

                    dmp_open.Enabled = False
                    file1.ReadOnly = True
                    fl_adr.ReadOnly = True

                    slt = 1

                    Timer1.Enabled = False

                    GroupBox11.Text = "status"
                    Status.Text = "wait（resume）"

                    'Me.Text = Me.Text & " [ " & Replace(Path.GetFileName(dir.Text & ".dmy"), ".dmy", "") & " ]"

                    MsgBox("Load complete " & vbCrLf & " reference is set to the specified folder ", , " Load complete ")
                Else
                    MsgBox("It was not unusual to have a resume to the last saved file " & vbCrLf & vbCrLf & " for resume file is either deleted or moved the dump file " & vbCrLf & " may ", , " error ")
                End If


            Else
                MsgBox("File does not exist you need to resume" & vbCrLf & _
                       "Ver " & Microsoft.VisualBasic.Left(Application.ProductVersion, 5) & " The only file that you saved can be read", , "error")

            End If

        End If

    End Sub

#End Region


#Region "ファイル ドラッグ＆ドロップ"

#Region "拡張子チェック"

    Private Sub file_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles _
    file1.DragDrop, Me.DragDrop

        'ドロップされた内容を表示する
        Dim fi As New System.IO.FileInfo(e.Data.GetData(DataFormats.FileDrop)(0))

        '拡張子が許可されたものならファイル1のテキストボックス更新
        If fi.Extension.ToUpper = ".DMP" Or fi.Extension.ToUpper = ".MEM" Or fi.Extension.ToUpper = ".RAM" Or fi.Extension.ToUpper = ".BIN" Then

            file1.Text = e.Data.GetData(DataFormats.FileDrop)(0)
            file1.SelectionStart = Len(file1.Text)

            Dim r As New System.Text.RegularExpressions.Regex("0x[0-9a-fA-F]{0,8}", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            Dim m As System.Text.RegularExpressions.Match = r.Match(fi.Name)
            If m.Success Then
                fl_adr.Text = m.Value
            End If

        End If

    End Sub

#End Region


#Region "ファイル確定"

    Private Sub file_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles _
    file1.DragEnter, Me.DragEnter

        'ドラッグされている内容が文字列型に変換可能な場合
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy  'コピーを許可するようにドラッグ元に通知する
        End If

    End Sub

#End Region

#End Region


#Region "各テキストボックスが更新されたときの処理"

    Private Sub TextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
    TextBox1.TextChanged, _
    offcet_min.TextChanged, offcet_max.TextChanged, _
    file1.TextChanged, fl_adr.TextChanged, _
    dir.TextChanged

        Select Case sender.Name

            Case "TextBox1", "offcet_min", "offcet_max", "fl_adr"

                '不要文字を自動除去
                Liv.Fix_Str(sender)

                'File No = 2、各情報が正しく設定されたら開始ボタンを有効に
                If sender.Name = "fl_adr" AndAlso start.Enabled = False AndAlso NumericUpDown1.ReadOnly = False AndAlso My.Computer.FileSystem.FileExists(file1.Text) = True AndAlso Str.Rep_Tex(fl_adr.Text) > 0 AndAlso Str.Rep_Tex(fl_adr.Text) <= Str.Rep_Tex(Max_adr) AndAlso NumericUpDown1.Value = 2 Then
                    start.Enabled = True
                End If

            Case "dir"
                dir.SelectionStart = Len(dir.Text)

            Case "file1"    'ファイル設定内容が更新（ファイル指定が）された時の処理

                'File No = 2、各情報が正しく設定されたら開始ボタンを有効に
                If start.Enabled = False And NumericUpDown1.ReadOnly = False And My.Computer.FileSystem.FileExists(file1.Text) = True And Str.Rep_Tex(fl_adr.Text) > 0 And Str.Rep_Tex(fl_adr.Text) <= Str.Rep_Tex(Max_adr) And NumericUpDown1.Value = 2 Then
                    start.Enabled = True
                End If

        End Select

    End Sub

#End Region


#Region "各テキストボックスのフォーカスが失われたときの処理"

    Private Sub TextBox_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
    TextBox1.Leave, _
    offcet_min.Leave, _
    offcet_max.Leave, _
    fl_adr.Leave

        '0x 以外の文字を大文字に
        Dim tmp As String = "0x" & Str.Fix(Replace(sender.Text, "0x", "").ToUpper, "[0-9A-F]", "")

        If tmp = "0x" Then
            tmp = "0x0"
        End If

        sender.Text = tmp

    End Sub

#End Region


#Region "各ボックスのIndexが変更された時の処理"

    Private Sub SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
    ComboBox1.SelectedIndexChanged, _
    ListBox1.SelectedIndexChanged

        Select Case sender.name

            Case "ComboBox1"

                If code.Enabled Then

                    If ComboBox1.SelectedIndex + 1 <= code_s Then
                        PrevC.Enabled = False
                    Else
                        PrevC.Enabled = True
                    End If

                    If ComboBox1.SelectedIndex + 1 >= code_f Then
                        NextC.Enabled = False
                    Else
                        NextC.Enabled = True
                    End If

                End If

            Case "ListBox1"

                If abort.Enabled = False Then

                    If InStr(ListBox1.SelectedItem, "開始") Then
                        Restore.Enabled = True
                    Else
                        Restore.Enabled = False
                    End If

                End If

        End Select

    End Sub

#End Region


#Region "ツール終了前の処理"

    Private Sub MAIN_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If Thread Then

            'Etc.Beep_Sound(My.Settings.SOUND, 1)
            SystemSounds.Beep.Play()
            '選択結果によって処理分岐
            Select Case MsgBox("Do you want to quit the tool to break the analysis？", vbOKCancel Or vbQuestion, "comfirm")

                Case vbOK

                    sc_abort = 2
                    Thread_Cancel() '解析スレッド　キャンセル

                Case Else

                    e.Cancel = True '終了イベント　キャンセル

            End Select

        End If

    End Sub

#End Region


#Region "ツール終了時の処理"

    Private Sub Form_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        'ツールがあるフォルダから一時ファイルを削除
        File.Del_Dir(Application.StartupPath() & "\tmp")

        '参照先フォルダは変更されているか？
        If Application.StartupPath() & "\tmp" <> save_folder Then

            '変更されている

            If (My.Computer.FileSystem.FileExists(dir.Text & "\try.cfg") And My.Computer.FileSystem.FileExists(save_folder & "\try.cfg") And _
                (File.Get_Size(dir.Text & "\try.cfg") < File.Get_Size(save_folder & "\try.cfg"))) _
                Or _
                (My.Computer.FileSystem.FileExists(dir.Text & "\try.cfg") = False And My.Computer.FileSystem.FileExists(save_folder & "\try.cfg")) Then

                Select Case MsgBox("Select (Analysis) has been updated file storage item" _
                                    & vbCrLf & "Do you want to overwrite the current file?" & vbCrLf _
                                    & vbCrLf & "※ selected (analysis) file storage item [try.cfg]" _
                                    & vbCrLf & "If you override the latest in the" _
                                    & vbCrLf & "selected by the end results of the analysis (analysis of) the item" _
                                    & vbCrLf & "over the file to add the latest" _
                                    & vbCrLf & vbCrLf & "resume when the next analysis (selected) item" _
                                    & vbCrLf & "in the list # This is useful because the analysis results will be a symbol" _
                                    , vbOKCancel Or vbQuestion, "check")

                    Case vbOK

                        File.Del(dir.Text & "\try.cfg")
                        File.Copy(save_folder & "\try.cfg", dir.Text & "\try.cfg")
                        'MsgBox("選択（解析）済み項目　記憶ファイルを" & vbCrLf & "最新のものに上書きしました", , "完了")

                End Select

            End If

            '参照先フォルダが変更されているのでそこの一時フォルダも削除
            File.Del_Dir(save_folder)

        End If

    End Sub

#End Region


#Region "File Noの上下つまみで値が変更された時の処理"

    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged

        Dim a As Integer
        Dim tmp = "", data() As String

        'アプリ起動後、準備不完全でも処理を始めるので一時フォルダがクリアされるまで待機
        If flag = 1 Then

            a = slt

            slt = NumericUpDown1.Value

            If a = 0 Then
                a = 1
            End If

            Select Case NumericUpDown1.ReadOnly

                Case False

                    'File Noの各設定状況チェック
                    If My.Computer.FileSystem.FileExists(file1.Text) = True And _
                    Str.Rep_Tex(fl_adr.Text) > 0 And Str.Rep_Tex(fl_adr.Text) <= Str.Rep_Tex(Max_adr) Then

                        '現在選択されているFile Noの情報がまだ未登録ならファイル数カウンタをプラス
                        If My.Computer.FileSystem.FileExists(save_folder & "\" & a & ".file") = False Then
                            slt_cnt += 1
                        End If

                        '各設定に問題なし、ファイル数カウンタ値に応じてファイル内容更新（書き出し）

                        File.WriteLine(save_folder & "\" & a & ".file", Replace(fl_adr.Text, "0x", "") & "," & file1.Text, False)


                        'File No変化、次のファイル指定になるので各設定項目を初期化
                        file1.Text = "NONE"
                        fl_adr.Text = Def_adr

                    Else

                        If My.Computer.FileSystem.FileExists(file1.Text) = False And _
                        (Str.Rep_Tex(fl_adr.Text) = 0 Or Str.Rep_Tex(fl_adr.Text) > Str.Rep_Tex(Max_adr)) _
                        Then '全ての情報がおかしい

                            tmp = "Please correct the address/dump file"
                            file1.Text = "NONE"
                            fl_adr.Text = Def_adr


                        ElseIf My.Computer.FileSystem.FileExists(file1.Text) = False Then 'ファイル情報のみおかしい

                            file1.Text = "NONE"
                            tmp = "Please set the correct file"

                        ElseIf Str.Rep_Tex(fl_adr.Text) = 0 Or Str.Rep_Tex(fl_adr.Text) > Str.Rep_Tex(Max_adr) Then 'アドレス情報のみおかしい

                            fl_adr.Text = Def_adr
                            tmp = "Please set the correct address"

                        End If



                        If My.Computer.FileSystem.FileExists(save_folder & "\" & NumericUpDown1.Value & ".file") = False Then
                            flag2 = 1
                        ElseIf My.Computer.FileSystem.FileExists(save_folder & "\" & a & ".file") = True Then
                            flag2 = 1
                        End If

                    End If


                    If flag2 = 0 And My.Computer.FileSystem.FileExists(save_folder & "\" & NumericUpDown1.Value & ".file") = True Then

                        data = Split(File.Read_Data(save_folder & "\" & NumericUpDown1.Value & ".file", 0), ",")

                        fl_adr.Text = "0x" & data(0)

                        file1.Text = data(1)
                        file1.SelectionStart = Len(file1.Text)

                    End If


                    If flag2 = 1 Then

                        If a <> NumericUpDown1.Value Then

                            tmp &= vbCrLf & vbCrLf & "The state information is set correctly for each of the next File No ▲ ▼ and press the" & vbCrLf & "another (next) you can set the file"
                            If a > 2 Then
                                tmp &= vbCrLf & vbCrLf & "※ files configured in two or more so," & vbCrLf & "File No by pressing the start button to match the number of files from the" & vbCrLf & "You can start it in the analysis of pre-set number of files"
                            Else

                                If slt_cnt < 3 Then
                                    tmp &= vbCrLf & vbCrLf & "※ the minimum number of specified files in the analysis because no two pieces have reached the" & vbCrLf & "button is not effective yet."
                                End If

                            End If


                            MsgBox(tmp, , "setting error [ file(No) = " & a & " ]")

                        End If

                        flag2 = 0

                        slt = a

                        NumericUpDown1.Value = a

                    End If


                Case True


                    If NumericUpDown1.Value > slt_cnt Then
                        NumericUpDown1.Value = slt_cnt
                    End If

                    If flag2 = 0 And My.Computer.FileSystem.FileExists(save_folder & "\" & NumericUpDown1.Value & ".file") = True Then

                        data = Split(File.Read_Data(save_folder & "\" & NumericUpDown1.Value & ".file", 0), ",")

                        fl_adr.Text = "0x" & data(0)

                        file1.Text = data(1)
                        file1.SelectionStart = Len(file1.Text)

                    End If

            End Select

        End If

    End Sub

#End Region


#Region "解析結果リストを作る"

    '関数名：Make_ComboBox（最終解析結果ファイルから解析結果リストを作成）
    '引数　：count  =   リスト作成対象の解析結果ファイルNo
    '戻り値：なし

    Function Make_ComboBox(ByVal count As Integer) As Long


        '事前に解析結果リストをクリア
        ComboBox1.Items.Clear()
        ComboBox1.Enabled = False

        code.Enabled = False
        PrevC.Enabled = False
        NextC.Enabled = False

        If My.Computer.FileSystem.FileExists(save_folder & "\" & count & ".cfg") = False Then

            Make_ComboBox = 0
            Exit Function

        End If

        Dim fdata(), data(slt_cnt), tmp, tmp2, tmp3 As String
        Dim k As Long

        k = 0
        code_s = 0
        code_f = 0


        ComboBox1.BeginUpdate()

        'ループ値　最終解析結果ファイル(*.cfg) を開く
        Using st_cnt As New System.IO.StreamReader(save_folder & "\" & count & ".cfg", System.Text.Encoding.Default)

            Do Until st_cnt.Peek() = -1

                Application.DoEvents() 'ループ中でも他のコントロールを操作可能に

                tmp3 = st_cnt.ReadLine()

                tmp2 = cnt & "," & tmp3

                fdata = Split(tmp3, ",") '行を順に読み込んでいく


                '解析結果追加用　アイテム（ファイル1,2のアドレスのみに省略）
                tmp = "   " & ("0x" & fdata(0)).PadRight(10, " ") & "  =  " & ("0x" & fdata(1)).PadRight(10, " ") & "  [ " & fdata(fdata.GetUpperBound(0) - 1) & " " & ("0x" & fdata(fdata.GetUpperBound(0))).PadLeft(10, " ") & " ]"
                tmp3 = Replace(Replace(tmp3, fdata(fdata.GetUpperBound(0) - 1) & "," & fdata(fdata.GetUpperBound(0)), ""), fdata(0) & ",", "")


                '全ファイルのアドレスが共通する基本アドレス候補（不一致数=0）
                If Len(tmp3) = 0 Then

                    '★印をつけてコード化ボタンを有効に
                    k += 1
                    tmp &= " ★" & k

                    If k = 1 And code_s = 0 Then
                        code_s = ComboBox1.Items.Count + 1
                    End If

                    code_f = ComboBox1.Items.Count + 1

                End If

                '解析結果リスト、過去選択履歴ファイルがあれば読み込み　比較
                If My.Computer.FileSystem.FileExists(save_folder & "\try.cfg") Then

                    Using st_try As New System.IO.StreamReader(save_folder & "\try.cfg", System.Text.Encoding.Default)

                        Do Until st_try.Peek() = -1

                            Application.DoEvents() 'ループ中でも他のコントロールを操作可能に

                            If tmp2 = st_try.ReadLine() Then

                                Mid(tmp, 1, 1) = "#"
                                Exit Do

                            End If

                        Loop

                    End Using

                End If


                '解析結果リストに追加
                ComboBox1.Items.Add(tmp)


            Loop 'ループ値　最終解析結果ファイル(*.cfg) 最終行になるまでループ

        End Using


        ComboBox1.EndUpdate()


        If ComboBox1.Items.Count > 0 Then '解析結果リストにアイテムが1つ以上追加された（ヒットあり）

            '解析結果リストを有効、先頭を選択済み
            ComboBox1.Enabled = True
            ComboBox1.SelectedIndex = 0

            'ファイル指定数が3～の場合は詳細ボタンを有効に
            If slt_cnt > 2 Then
                Info.Enabled = True
            End If

            If k <> 0 Then

                code.Enabled = True
                'Button4.Enabled = True

                If code_f > 1 Then
                    NextC.Enabled = True
                End If

            End If

        End If

        Make_ComboBox = k

    End Function

#End Region


#Region "開始前のエラーチェック"

    '関数名：Check_Err（エラーチェック）
    '引数　：なし
    '戻り値：エラー（原因）
    '       0   =   なし
    '       1   =   開始キャンセル
    '       2   =   オフセット設定
    '       3   =   アドレス共通の為解析不要
    '       4   =   ファイル設定
    '       5   =   設定重複

    Function Check_Err() As Long

        Dim f, h, i, j, k, Err As Integer
        Dim tmp As String

        If InStr(ComboBox1.SelectedItem, "★") <> 0 Then

            MsgBox("Was selected for further analysis of the common address is not required all", , "info")

            Err = 3

            'オフセット値　最小・最大値確認
        ElseIf Str.Rep_Tex(offcet_min.Text) > Str.Rep_Tex(offcet_max.Text) Then

            MsgBox("Offset value (minimum) (maximum) please select a value lower than", , "error")

            offcet_min.Text = "0x00000000"
            offcet_max.Text = "0x0000FFFF"

            Err = 2

            '設定欄が有効時（ループカウンタ=0）に開始ボタンが押されたら　設定項目の情報を最終チェック
        ElseIf NumericUpDown1.ReadOnly = False Then

            If My.Computer.FileSystem.FileExists(file1.Text) = True And _
            Str.Rep_Tex(fl_adr.Text) > 0 And Str.Rep_Tex(fl_adr.Text) <= Str.Rep_Tex(Max_adr) Then

                '現在選択されているFile Noの情報がまだ未登録ならファイル数カウンタをプラス
                If My.Computer.FileSystem.FileExists(save_folder & "\" & NumericUpDown1.Value & ".file") = False Then
                    slt_cnt += 1
                End If

                '各設定に問題なし、ファイル数カウンタ値に応じてファイル内容更新（書き出し）

                File.WriteLine(save_folder & "\" & NumericUpDown1.Value & ".file", Replace(fl_adr.Text, "0x", "") & "," & file1.Text, False)

                j = 0

                Dim data(255)(), dtmp() As String


                tmp = "The settings are as follows:" & vbCrLf & "Do you want to analyze this？" & vbCrLf & vbCrLf & "offset= 0x" & Hex(Str.Rep_Tex(offcet_min.Text)) & " ～ 0x" & Hex(Str.Rep_Tex(offcet_max.Text)) & "（minimum、maximum）" & vbCrLf & vbCrLf

                '有効なファイル設定数を確認
                For i = 1 To slt_cnt

                    If My.Computer.FileSystem.FileExists(save_folder & "\" & i & ".file") Then

                        dtmp = Split(File.Read_Data(save_folder & "\" & i & ".file", 0), ",")
                        tmp &= "file" & i & " = 0x" & dtmp(0).PadLeft(7, "0") & " ( " & Path.GetFileName(dtmp(1)) & " )" & vbCrLf

                        j += 1

                    End If

                Next


                h = 1

                For i = 1 To j - 1

                    data(i) = Split(File.Read_Data(save_folder & "\" & i & ".file", 0), ",")

                    f = 0

                    For k = i + 1 To j

                        If data(i)(0) = Split(File.Read_Data(save_folder & "\" & k & ".file", 0), ",")(0) Or _
                        data(i)(1) = Split(File.Read_Data(save_folder & "\" & k & ".file", 0), ",")(1) Then
                            f += 1
                        End If

                    Next

                    If f = 0 Then
                        h += 1
                    End If

                Next

                If h = j And j >= 2 Then

                    ' Etc.Beep_Sound(My.Settings.SOUND, 1)
                    SystemSounds.Beep.Play()

                    '開始する前に設定情報表示、選択結果で処理分岐
                    Select Case MsgBox(tmp, vbOKCancel Or vbQuestion, "comfirm")

                        Case vbOK '解析を開始するために値を設定

                            slt_cnt = j '有効なファイル数でファイルカウンタを再設定
                            Err = 0

                        Case Else '開始をキャンセル

                            Err = 1

                    End Select

                Else

                    Err = 5

                End If



            Else '現在選択されてるFile Noの各情報に不備がある

                Err = 4

            End If

        End If

        Check_Err = Err

    End Function

#End Region


#Region "クリップボード監視　タイマー"

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        If My.Computer.Clipboard.ContainsText And My.Computer.Clipboard.GetText <> clip_tmp Then

            If (Str.Chk(My.Computer.Clipboard.GetText.ToUpper, "[0-9A-FXL_ \t\n\r]", False) = 0) Then

                fl_adr.Text = "0x" & Microsoft.VisualBasic.Right(Str.Fix(My.Computer.Clipboard.GetText.ToUpper, "[0-9A-F]", "").PadLeft(7, "0"), 7)
                clip_tmp = My.Computer.Clipboard.GetText

            End If

        End If

    End Sub

#End Region


#Region "実行履歴の項目をダブルクリックしたときの処理"

    Private Sub ListBox1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.DoubleClick

        If InStr(ListBox1.SelectedItem, "start") And Thread = False Then
            Restore_Click(sender, e)
        End If

    End Sub

#End Region


#Region "解析候補除外チェックボックス監視"

    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        TextBox1.Enabled = CheckBox3.Checked
    End Sub


    Private Sub CheckBox3_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox3.EnabledChanged

        Select Case CheckBox3.Enabled

            Case True
                TextBox1.Enabled = CheckBox3.Checked

            Case False
                TextBox1.Enabled = False

        End Select

    End Sub

#End Region


#Region "ステータス文字の位置を自動補正"

    Private Sub Status_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Status.Resize
        Ctr.Change_Status(GroupBox11, Status, , False)
    End Sub

#End Region


    Private Sub MAIN_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


    End Sub


    Private Sub SEttingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SEttingToolStripMenuItem.Click

    End Sub

    Private Sub VersionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VersionToolStripMenuItem.Click
        Dim f As New Ver
        f.ShowDialog(Me)
        f.Dispose()
    End Sub
End Class


#End Region


#Region "共通メソッド（クラス）"

Public Class Liv

    Public Shared Sub Fix_Str(ByVal sender As System.Windows.Forms.TextBox)

        Dim Str As New Api.Str
        Dim Etc As New Api.Etc
        Dim real As Boolean = False
        Dim stat As Integer = sender.SelectionStart
        Dim tmp As String = ""


        tmp = Str.Fix(sender.Text, "[0-9a-fA-Fx]", "")

        If Len(tmp) < 2 Then
            tmp = "0x"
        Else
            Mid(tmp, 1, 1) = "0"
            Mid(tmp, 2, 1) = "0"
            tmp = Str.Fix(tmp, "[0-9a-fA-F]", "")
            Mid(tmp, 2, 1) = "x"
        End If

        If stat < 3 Then
            stat = 3
        End If


        If sender.Text <> tmp Then
            sender.Text = tmp
            sender.SelectionStart = stat - 1
            '  Etc.Beep_Sound(My.Settings.SOUND)
            SystemSounds.Beep.Play()

        ElseIf sender.TextLength = sender.MaxLength And (sender.Name = "fl_adr" Or sender.Name = "TextBox1") Then

            If Val("&H" & Replace(tmp, "0x", "")) > &H17FFFFF Then

                If Val("&H" & Replace(tmp, "0x", "")) > &H8800000 Then
                    Dim offsetchanger As Integer = Convert.ToInt32(tmp, 16) - &H8800000
                    tmp = "0x" & offsetchanger.ToString("X").PadLeft(7, "0")
                    sender.Text = tmp
                    real = True
                End If

            If Val("&H" & Microsoft.VisualBasic.Mid(sender.Text, 3, 1)) > 1 Then

                Mid(tmp, 3, 1) = "1"
                sender.Text = tmp
                sender.SelectionStart = 2
                '    Etc.Beep_Sound(My.Settings.SOUND)
                SystemSounds.Beep.Play()

                ElseIf Val("&H" & Microsoft.VisualBasic.Mid(sender.Text, 4, 1)) > 7 AndAlso real = False Then

                    Mid(tmp, 4, 1) = "7"
                    sender.Text = tmp
                    sender.SelectionStart = 3
                    '    Etc.Beep_Sound(My.Settings.SOUND)
                    SystemSounds.Beep.Play()
            End If

            End If

        End If

    End Sub


End Class

#End Region


#Region "Custom TextBox"

'Imports System.Text.RegularExpressions

Public Class Reg_TextBox
    Inherits TextBox

    Public Sub New()
        MyBase.New()
        Me.PermitString = String.Empty
        Me.PermitType = New Boolean
    End Sub


    Private _PermitString As String

    ''' <summary>正規表現</summary>
    Public Property PermitString() As String
        Get
            Return Me._PermitString
        End Get

        Set(ByVal value As String)
            Me._PermitString = value
        End Set
    End Property


    Private _PermitType As Boolean

    ''' <summary>比較方法</summary>
    Public Property PermitType() As Boolean
        Get
            Return Me._PermitType

        End Get

        Set(ByVal value As Boolean)
            Me._PermitType = value
        End Set
    End Property


    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)

        If (Not Me.PermitString Is Nothing) AndAlso (Me.PermitString.Length > 0) Then
            Select Case m.Msg
                Case &H102 'Key Press
                    Dim eKeyPress As New KeyPressEventArgs(Microsoft.VisualBasic.ChrW(m.WParam.ToInt32()))
                    If Not HasPermitChars(eKeyPress.KeyChar, Me.PermitString, Me.PermitType) Then
                        Return
                    End If
                Case &H302 'WM_PASTE
                    Dim stString As String = Clipboard.GetDataObject().GetData(System.Windows.Forms.DataFormats.Text).ToString()
                    If Not stString Is Nothing Then
                        Me.SelectedText = GetPermitedString(stString, Me.PermitString, Me.PermitType, Me.TextLength - Me.SelectionLength, Me.MaxLength)
                    End If
                    Return
            End Select
        End If

        MyBase.WndProc(m)

    End Sub


    ' 許可された文字だけを連結して返す
    Private Shared Function GetPermitedString(ByVal stTarget As String, ByVal chString As String, ByVal chType As Boolean, ByVal chLength As Integer, ByVal chMaxLength As Integer) As String
        Dim stReturn As String = String.Empty

        For Each chTarget As Char In stTarget
            If HasPermitChars(chTarget, chString, chType) And ((chLength + Len(stReturn)) < chMaxLength) Then
                stReturn &= chTarget
            End If
        Next chTarget

        Return stReturn
    End Function

    '許可された文字かどうかの値を返す
    Private Shared Function HasPermitChars(ByVal Str As Char, ByVal filter As String, Optional ByVal type As Boolean = False) As Boolean
        Dim r As New Regex(filter)

        If r.IsMatch(Str, 0) = type Then
            HasPermitChars = True
        End If
        Return False
    End Function

End Class

#End Region
