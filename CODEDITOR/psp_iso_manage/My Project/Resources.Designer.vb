﻿'------------------------------------------------------------------------------
' <auto-generated>
'     このコードはツールによって生成されました。
'     ランタイム バージョン:4.0.30319.239
'
'     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
'     コードが再生成されるときに損失したりします。
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'このクラスは StronglyTypedResourceBuilder クラスが ResGen
    'または Visual Studio のようなツールを使用して自動生成されました。
    'メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    'ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    '''<summary>
    '''  ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("umd_rawimage_manger.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  厳密に型指定されたこのリソース クラスを使用して、すべての検索リソースに対し、
        '''  現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  管理したいイメージを追加します
        '''複数まとめて追加したい場合はエクスプローラーから選択してツリーにドロップして下さい に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property fileadd() As String
            Get
                Return ResourceManager.GetString("fileadd", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  編集リストを保存します に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property filesave() As String
            Get
                Return ResourceManager.GetString("filesave", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  PSPINS形式のゲームリストを出力します
        '''INSでは2BYTE目0X7Cが含まれるものを扱えないので置換します
        '''管理名だけでなく当然ファイル名などにも使えません
        '''//INSダメ文字
        '''－ポл榎掛弓芸鋼旨楯酢掃竹倒培怖翻慾處嘶斈忿掟桍毫烟痞窩縹艚蛞諫轎閖驂黥埈蒴僴礰 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property po_gei() As String
            Get
                Return ResourceManager.GetString("po_gei", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  PSPに転送するディレクトリーを編集します
        '''ツリービューの右クリックから変更できるようになります
        '''//例
        '''X:\PSP\GAME\ に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property pspdir() As String
            Get
                Return ResourceManager.GetString("pspdir", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  clrmamepro用DATファイルを選択します に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property rm() As String
            Get
                Return ResourceManager.GetString("rm", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  clrmamepro用DATファイルを使ってリネームします に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property rmdat() As String
            Get
                Return ResourceManager.GetString("rmdat", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  OFFLINE用XMLファイルを使ってリネームします に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property rmxml() As String
            Get
                Return ResourceManager.GetString("rmxml", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  OFFLINE用XMLを選択します
        '''CRC32と一致する管理名,画像が検索ボタンで適用されます に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property xml() As String
            Get
                Return ResourceManager.GetString("xml", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''   に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property xmlfoot() As String
            Get
                Return ResourceManager.GetString("xmlfoot", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''   に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property xmlhead() As String
            Get
                Return ResourceManager.GetString("xmlhead", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
