Option Explicit On

#Region "インポート"

Imports System
Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic
Imports System.Text.RegularExpressions

#End Region


#Region "文字列を操作するクラス"

''' <summary>文字列を操作するクラス</summary>
''' <remarks></remarks>
Public Class Str

#Region "Fix"

    Dim Api As New Api.Etc

    ''' <summary>対象文字列から指定された文字を正規表現を使って置き換えます</summary>
    ''' <param name="Str">対象文字列</param>
    ''' <param name="filter">正規表現</param>
    ''' <param name="fix_s">一致した場合に置き換える文字</param>
    ''' <param name="type">比較方法 True=一致 False=不一致</param>
    ''' <returns>正規表現で置き換えが完了した文字列</returns>
    ''' <remarks></remarks>

    Function Fix(ByVal Str As String, ByVal filter As String, ByVal fix_s As String, Optional ByVal type As Boolean = False) As String

        Dim tmp As String = ""
        Dim r As New Regex(filter) '対象文字内で許可する文字列（正規表現）

        Select Case Len(Str)

            Case Is > 0

                For Each tmp2 As Char In Str

                    If r.IsMatch(tmp2, 0) = (type Xor True) Then

                        tmp &= tmp2

                    Else

                        tmp &= fix_s

                    End If

                Next tmp2

                Return tmp

            Case Else

                Return String.Empty

        End Select

    End Function

#End Region

#Region "Chk"

    ''' <summary>対象文字列に含まれる不要文字数を正規表現で取得する</summary>
    ''' <param name="str">対象文字列</param>
    ''' <param name="filter">正規表現</param>
    ''' <param name="type">比較方法 True=一致 False=不一致</param>
    ''' <returns>文字列の中に含まれた不要文字数</returns>
    ''' <remarks></remarks>
    Function Chk(ByVal str As String, ByVal filter As String, ByVal type As Boolean) As Integer

        Select Case Len(str)

            Case Is > 0
                Chk = Len(str) - Len(Fix(str, filter, "", type))

            Case 0
                Chk = -1

        End Select

    End Function

#End Region

#Region "Chk_Kanji"

    Function Chk_Kanji(ByVal str As String) As Boolean

        If Chk(str, "[一-龠ぁ-んァ-ヴａ-ｚＡ-Ｚ０-９’-￥]", True) Then

            Chk_Kanji = True

        Else

            Chk_Kanji = False

        End If

    End Function

#End Region

#Region "Rep_Tex"

    ''' <summary>16進数（文字列）を数値に変換</summary>
    ''' <param name="str">対象文字列</param>
    '''<returns>変換された数値</returns>
    ''' <remarks></remarks>
    Function Rep_Tex(ByVal str As String) As Long

        'エラー回避用に異常文字の場合は置き換え
        If str = "0x" Or str = "" Then

            str = "0"

        End If

        Rep_Tex = CLng("&H" & Replace(str, "0x", ""))

    End Function

#End Region


End Class

#End Region


#Region "その他のクラス"

''' <summary>その他のクラス</summary>
''' <remarks></remarks>
Public Class Etc


    ''' <summary>引数によってシステムサウンドを鳴らします</summary>
    ''' <param name="switch">True=鳴る False=鳴らない</param>
    ''' <remarks></remarks>
    Public Sub Beep_Sound(ByVal switch As Boolean, Optional ByVal Type As Integer = 2)

        If switch Then

            Select Case Type

                Case 1 'メッセージ（情報）
                    My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Asterisk)

                Case 2 '一般の警告音
                    My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Beep)

                Case 3 'メッセージ（警告）
                    My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)

                Case 4 'システムエラー
                    My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Hand)

                Case 5 'メッセージ（問い合わせ）
                    My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Question)

                Case Else 'その他はBeep
                    My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Beep)

            End Select


        End If

    End Sub

End Class

#End Region


#Region "ファイルを操作するクラス"

''' <summary>ファイルを操作するクラス</summary>
''' <remarks></remarks>
Public Class File

    Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding("Shift-JIS") 'テキストのエンコード指定



    Function Get_Size(ByVal path As String) As Long

        If My.Computer.FileSystem.FileExists(path) Then

            Get_Size = My.Computer.FileSystem.GetFileInfo(path).Length

        Else

            Get_Size = 0

        End If

    End Function



    ''' <summary>ファイルを指定されたアプリケーションで開きます</summary>
    ''' <param name="app">Program File内にあるアプリケーションのパス</param>
    ''' <param name="File">開くファイル</param>
    ''' <param name="Focus">フォーカス</param>
    ''' <remarks></remarks>
    Public Sub App_View(ByVal app As String, ByVal File As String, Optional ByVal Focus As Microsoft.VisualBasic.AppWinStyle = AppWinStyle.NormalFocus)

        Shell(Environ("WINDIR") & "\" & app & " " & File, Focus)

    End Sub


    ''' <summary>ファイルを関連付けられたアプリケーションで開きます</summary>
    ''' <param name="path">開くファイルのパス</param>
    ''' <param name="Focus">フォーカス</param>
    ''' <remarks></remarks>
    Public Sub View(ByVal Path As String, Optional ByVal Focus As Microsoft.VisualBasic.AppWinStyle = AppWinStyle.NormalFocus)

        Shell("rundll32.exe url.dll,FileProtocolHandler " & Path, Focus)

    End Sub


    ''' <summary>フォルダをOS標準のエクスプローラで開きます</summary>
    ''' <param name="path">開くフォルダのパス</param>
    ''' <param name="Focus">フォーカス</param>
    ''' <remarks></remarks>
    Public Sub Open_Dir(ByVal path As String, Optional ByVal Focus As Microsoft.VisualBasic.AppWinStyle = AppWinStyle.NormalFocus)

        Shell("explorer.exe " & path, Focus)

    End Sub



    ''' <summary>文字列を指定したファイルに書き出します</summary>
    ''' <param name="path">対象ファイル</param>
    ''' <param name="data">文字列</param>
    ''' <param name="type_fl">True=追記 False=新規</param>
    ''' <param name="code">文字コード</param>
    ''' <remarks></remarks>
    Public Sub Write(ByVal path As String, ByVal data As String, Optional ByVal type_fl As Boolean = True, Optional ByVal code As String = "Shift-JIS")

        Dim tmp As String = data

        If Len(tmp) = 0 Then

            tmp = ""

        End If

        Using Writer As New IO.StreamWriter(path, type_fl, System.Text.Encoding.GetEncoding(code))

            Writer.Write(tmp, enc)

        End Using

    End Sub


    ''' <summary>文字列を指定したファイルに書き出します（改行付加）</summary>
    ''' <param name="path">対象ファイル</param>
    ''' <param name="data">文字列</param>
    ''' <param name="type_fl">True=追記 False=新規</param>
    ''' <param name="code">文字コード</param>
    ''' <remarks></remarks>
    Public Sub WriteLine(ByVal path As String, ByVal data As String, Optional ByVal type_fl As Boolean = True, Optional ByVal code As String = "Shift-JIS")

        Dim tmp As String = data

        If Len(tmp) = 0 Then

            tmp = ""

        End If


        Using Writer As New IO.StreamWriter(path, type_fl, System.Text.Encoding.GetEncoding(code))

            Writer.WriteLine(tmp, enc)

        End Using


    End Sub


    ''' <summary>フォルダに指定された拡張子のファイルを全てコピーします</summary>
    ''' <param name="path1 ">コピー元</param>
    ''' <param name="path2">コピー先</param>
    ''' <param name="type">拡張子　指定</param>
    ''' <remarks></remarks>
    Public Sub Copy_All(ByVal path1 As String, ByVal path2 As String, ByVal type As String)


        For Each foundFile As String In My.Computer.FileSystem.GetFiles(path1, FileIO.SearchOption.SearchTopLevelOnly, type)

            My.Computer.FileSystem.CopyFile(foundFile, path2 & Path.GetFileName(foundFile), True)

        Next

    End Sub


    ''' <summary>フォルダ内にある指定された拡張子のファイルを全て削除します</summary>
    ''' <param name="path">対象フォルダのパス</param>
    ''' <param name="type">拡張子　指定</param>
    ''' <remarks></remarks>
    Public Sub Del_All(ByVal path As String, ByVal type As String)

        If My.Computer.FileSystem.DirectoryExists(path) = True Then

            For Each foundFile As String In My.Computer.FileSystem.GetFiles(path, FileIO.SearchOption.SearchTopLevelOnly, type)

                My.Computer.FileSystem.DeleteFile(foundFile, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)

            Next

        End If

    End Sub


    ''' <summary>ファイルを削除します</summary>
    ''' <param name="path">対象ファイルのパス</param>
    '''<returns>成功=True 失敗=False</returns>
    ''' <remarks></remarks>
    Function Del(ByVal path As String) As Boolean

        '存在したら削除
        If My.Computer.FileSystem.FileExists(path) = True Then

            My.Computer.FileSystem.DeleteFile(path, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)

            Del = True

        Else

            Del = False

        End If

    End Function


    ''' <summary>フォルダを削除します</summary>
    ''' <param name="path">対象フォルダのパス</param>
    '''<returns>成功=True 失敗=False</returns>
    ''' <remarks></remarks>
    Function Del_Dir(ByVal path As String) As Boolean

        If My.Computer.FileSystem.DirectoryExists(path) = True Then

            My.Computer.FileSystem.DeleteDirectory(path, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)

            Del_Dir = True

        Else

            Del_Dir = False

        End If

    End Function




    ''' <summary>ファイルを指定したフォルダにコピーします</summary>
    ''' <param name="path1">コピー元</param>
    ''' <param name="path2">コピー先</param>
    '''<returns>成功=True 失敗=False</returns>
    ''' <remarks></remarks>
    Function Copy(ByVal path1 As String, ByVal path2 As String) As Boolean

        If My.Computer.FileSystem.FileExists(path1) = True Then

            My.Computer.FileSystem.CopyFile(path1, path2, True)

            Copy = True

        Else

            Copy = False

        End If

    End Function



    ''' <summary>ファイルの指定行を読み込む</summary>
    ''' <param name="path">ファイルのパス</param>
    ''' <param name="seek">行　指定</param>
    '''<returns>読み込んだ文字列</returns>
    ''' <remarks></remarks>
    Function Read_Data(ByVal path As String, ByVal seek As Long) As String

        If My.Computer.FileSystem.FileExists(path) Then

            Using file As New System.IO.StreamReader(path, System.Text.Encoding.Default)

                Dim i As Long

                For i = 1 To seek

                    file.ReadLine() '対象位置まで読込Skip

                Next

                Read_Data = file.ReadLine() '指定行に到達したら文字読込、,区切りで変数格納

            End Using

        Else

            Read_Data = ""

        End If

    End Function




    Function Put_Data(ByVal path As String, ByVal adr As Long, ByVal bit As Integer) As Long

        Dim buf(bit) As Byte ' 読み込み用バッファ

        Dim readSize As Long ' 読み込んだバイト数　記憶用

        Dim i, j As Long
        Dim tmp As Long = 0

        j = 0

        Using src As New FileStream(path, FileMode.Open, FileAccess.Read)

            src.Seek(adr, SeekOrigin.Begin)  '先頭までseek
            readSize = src.Read(buf, 0, bit) ' 読み込み

        End Using

        For i = 1 To bit

            tmp += buf(i - 1) * 2 ^ j

            j += 8

        Next

        Put_Data = tmp

    End Function


End Class

#End Region


#Region "コントロールを操作するクラス"

''' <summary>コントロールを操作するクラス</summary>
''' <remarks></remarks>
Public Class Ctrl


    Dim Str As New Api.Str
    Dim Etc As New Api.Etc

    ''' <summary>テキストボックス系のコントロールで指定した文字のみ入力可能にします</summary>
    ''' <param name="sender">対象コントロール</param>
    ''' <param name="filter">許可する文字　※正規表現</param>
    ''' <param name="type">比較方法 True=一致 False=不一致</param>
    ''' <remarks>
    '''<newpara>
    ''' TextChange イベントで使う
    ''' </newpara>
    ''' </remarks>
    Public Sub Fix_Box(ByVal sender As System.Object, ByVal filter As String, ByVal type As Boolean, Optional ByVal sound As Boolean = False)

        Dim index As Integer = sender.SelectionStart
        Dim tmp As String = Str.Fix(sender.text, filter, "", type)

        If sender.text <> tmp Then

            sender.text = tmp
            sender.SelectionStart = index - 1
            Etc.Beep_Sound(sound)

        End If

    End Sub


    ''' <summary>子コントロールを親コントロールの中央に配置する</summary>
    ''' <param name="Ob1">親コントロール</param>
    ''' <param name="ob2">子コントロール</param>
    ''' <remarks></remarks>
    Public Sub Change_Status(ByVal Ob1 As System.Object, ByVal ob2 As System.Object, Optional ByVal fix_witdh As Boolean = True, Optional ByVal fix_Height As Boolean = True)

        If fix_witdh Then

            ob2.Left = Ob1.Width / 2 - ob2.Width / 2

        End If

        If fix_Height Then

            ob2.top = Ob1.Height / 2 - ob2.Height / 2

        End If


    End Sub


End Class


#End Region


#Region "ネットワーク関係"

Public Class Net

    <System.Runtime.InteropServices.DllImport("wininet.dll")> _
    Shared Function InternetGetConnectedState( _
    ByRef lpdwFlags As Integer, ByVal dwReserved As Integer) As Boolean
    End Function


    Function Get_Size(ByVal URL As String) As Long

        Dim webreq As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(URL),  _
        System.Net.HttpWebRequest)

        'サーバーからの応答を受信するためのWebResponseを取得
        Dim webres As System.Net.HttpWebResponse = CType(webreq.GetResponse(), System.Net.HttpWebResponse)

        Get_Size = webres.ContentLength


    End Function


#Region "ファイルダウンロード"

    Function File_DL(ByVal URL As String, ByVal path As String) As Boolean


        Dim File As New File

        File.Del(path)

        My.Computer.Network.DownloadFile(URL, path)

        If My.Computer.FileSystem.FileExists(path) Then

            File_DL = True

        Else
            File_DL = False

        End If

    End Function

#End Region


#Region "有効なネットワークチェック"

    Public Function IsInternetConnected(ByVal URL As String) As Boolean

        If URL = "" Then
            IsInternetConnected = False
            Exit Function

        End If

        'インターネットに接続されているか確認する
        Dim host As String = URL

        Dim webreq As System.Net.HttpWebRequest = Nothing
        Dim webres As System.Net.HttpWebResponse = Nothing
        Try
            'HttpWebRequestの作成
            webreq = CType(System.Net.WebRequest.Create(host),  _
                System.Net.HttpWebRequest)
            'メソッドをHEADにする
            webreq.Method = "HEAD"
            '受信する
            webres = CType(webreq.GetResponse(), System.Net.HttpWebResponse)
            '応答ステータスコードを表示
            Return True
        Catch
        Finally
            If Not (webres Is Nothing) Then
                webres.Close()
            End If
        End Try
    End Function

#End Region

End Class


#End Region